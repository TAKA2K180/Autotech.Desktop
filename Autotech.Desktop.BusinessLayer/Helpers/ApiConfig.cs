using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Threading.Tasks;

namespace Autotech.Desktop.BusinessLayer.Helpers
{
    /// <summary>
    /// Resolves the API base URL at startup so it can be changed without shipping a new build.
    ///
    /// Precedence:
    ///   1. Environment variable AUTOTECH_API_BASE          (developer / support override)
    ///   2. config.json in %ProgramData%\Autotech Desktop    (per-machine override, editable by an admin)
    ///   3. config.json beside the executable                (override shipped with the install)
    ///   4. PrimaryBaseUrl, if it answers                    (the normal case)
    ///   5. SecondaryBaseUrl, if it answers                  (failover when the primary is down)
    ///   6. The remote bootstrap file                        (only reached when both hosts are down)
    ///   7. The last remote value that was successfully read (used when offline)
    ///   8. PrimaryBaseUrl                                   (last resort)
    ///
    /// Nothing here throws. Any failure just falls through to the next source, so a
    /// missing file or a dead bootstrap host can never stop the app from starting.
    /// </summary>
    public static class ApiConfig
    {
        /// <summary>The API the app normally talks to.</summary>
        private const string PrimaryBaseUrl = "https://api.autotechph.online/api/v1";

        /// <summary>Used when the primary does not answer.</summary>
        private const string SecondaryBaseUrl = "https://web-api.autotechph.online/api/v1";

        /// <summary>
        /// Static file you host and control. Edit it to point every installed client at a
        /// new API host on their next launch. If it is unreachable the app still works.
        /// Note this is only consulted when neither host above answers.
        /// </summary>
        private const string BootstrapUrl = "https://config.autotechph.online/desktop-config.json";

        /// <summary>
        /// A value coming from the network is only accepted if its host matches one of these
        /// (exactly, or as a subdomain). Whoever controls the bootstrap file controls where
        /// logins are posted, so this keeps a tampered file from redirecting credentials.
        /// </summary>
        private static readonly string[] AllowedRemoteHosts = { "autotechph.online" };

        private const string EnvironmentVariableName = "AUTOTECH_API_BASE";
        private const string ConfigFileName = "config.json";
        private const string CacheFileName = "api-base.cache";

        /// <summary>Startup budget for the bootstrap fetch. Kept short so a slow host is not felt.</summary>
        private static readonly TimeSpan BootstrapTimeout = TimeSpan.FromSeconds(2);

        /// <summary>
        /// Per-host budget for the reachability probe. Worst case at startup is one of these
        /// per host plus the bootstrap fetch, and only when everything is down.
        /// </summary>
        private static readonly TimeSpan ProbeTimeout = TimeSpan.FromSeconds(2);

        private static bool _initialized;

        /// <summary>The API base URL, without a trailing slash. Safe to read before Initialize().</summary>
        public static string BaseUrl { get; private set; } = PrimaryBaseUrl;

        /// <summary>Which source BaseUrl came from. Written to the log at startup.</summary>
        public static string Source { get; private set; } = "primary";

        /// <summary>
        /// Directory holding the per-machine override and the cached remote value.
        /// ProgramData is used rather than the install folder because the app installs
        /// under Program Files, which a standard user cannot write to.
        /// </summary>
        public static string ConfigDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            "Autotech Desktop");

        /// <summary>
        /// Resolves the base URL. Call once from Program.Main, before any form is shown
        /// or any service is constructed. Blocks for at most <see cref="BootstrapTimeout"/>.
        /// </summary>
        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;

            try
            {
                Resolve();
            }
            catch (Exception ex)
            {
                // Should not happen - Resolve swallows its own failures - but a config
                // problem must never be the reason the app fails to start.
                BaseUrl = PrimaryBaseUrl;
                Source = "primary (resolve failed)";
                SafeLog("ApiConfig: resolution failed, using the primary URL.", ex);
            }

            SafeLog($"ApiConfig: using {BaseUrl} (source: {Source})");
        }

        private static void Resolve()
        {
            // 1. Environment variable.
            var fromEnvironment = Environment.GetEnvironmentVariable(EnvironmentVariableName);
            if (TrySet(fromEnvironment, $"environment variable {EnvironmentVariableName}", requireAllowedHost: false))
                return;

            // 2 & 3. Local override files. A local file beats the remote value on purpose -
            // it is the escape hatch for a machine that needs to point somewhere else.
            if (TrySet(ReadUrlFromFile(Path.Combine(ConfigDirectory, ConfigFileName)),
                       "config file (ProgramData)", requireAllowedHost: false))
                return;

            if (TrySet(ReadUrlFromFile(Path.Combine(AppContext.BaseDirectory, ConfigFileName)),
                       "config file (install folder)", requireAllowedHost: false))
                return;

            // With no network at all there is nothing to probe or fetch, so skip straight
            // to the offline sources rather than burning three timeouts on the splash screen.
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                // 4. The primary API, if it answers.
                if (IsReachable(PrimaryBaseUrl) && TrySet(PrimaryBaseUrl, "primary", requireAllowedHost: false))
                    return;

                // 5. The secondary API, if it answers.
                SafeLog($"ApiConfig: {PrimaryBaseUrl} did not answer, trying {SecondaryBaseUrl}.");

                if (IsReachable(SecondaryBaseUrl) && TrySet(SecondaryBaseUrl, "secondary", requireAllowedHost: false))
                    return;

                // 6. Remote bootstrap - only reached when neither host above answered.
                SafeLog("ApiConfig: neither API host answered, consulting the bootstrap file.");

                if (TrySet(FetchRemoteUrl(), "remote bootstrap", requireAllowedHost: true))
                {
                    WriteCache(BaseUrl);
                    return;
                }
            }
            else
            {
                SafeLog("ApiConfig: no network detected, skipping probes.");
            }

            // 7. Last known good remote value.
            if (TrySet(ReadUrlFromFile(Path.Combine(ConfigDirectory, CacheFileName)),
                       "cached remote value", requireAllowedHost: true))
                return;

            // 8. Nothing resolved - fall back to the primary and let the call sites report
            // the failure the way they always have.
            BaseUrl = PrimaryBaseUrl;
            Source = "primary (nothing reachable)";
        }

        /// <summary>
        /// Returns true if the host answers at all. Any HTTP status other than 5xx counts -
        /// a 401 or 404 on the base path still means DNS, TCP, TLS and the server are fine.
        /// A 5xx means the host is up but the API behind it is not, which is exactly when
        /// failing over is the right move.
        /// </summary>
        private static bool IsReachable(string baseUrl)
        {
            try
            {
                return Task.Run(async () =>
                {
                    using var client = new HttpClient { Timeout = ProbeTimeout };
                    using var response = await client.GetAsync(baseUrl, HttpCompletionOption.ResponseHeadersRead);
                    return (int)response.StatusCode < 500;
                }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // DNS failure, connection refused, TLS error, timeout.
                SafeLog($"ApiConfig: {baseUrl} is not reachable.", ex);
                return false;
            }
        }

        private static bool TrySet(string? candidate, string source, bool requireAllowedHost)
        {
            var normalized = Normalize(candidate);
            if (normalized == null) return false;

            if (!IsAcceptable(normalized, requireAllowedHost))
            {
                SafeLog($"ApiConfig: rejected '{normalized}' from {source}.");
                return false;
            }

            BaseUrl = normalized;
            Source = source;
            return true;
        }

        private static string? Normalize(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            var trimmed = value.Trim().TrimEnd('/');
            return trimmed.Length == 0 ? null : trimmed;
        }

        private static bool IsAcceptable(string url, bool requireAllowedHost)
        {
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) return false;

            if (uri.Scheme != Uri.UriSchemeHttps && uri.Scheme != Uri.UriSchemeHttp) return false;

            if (!requireAllowedHost) return true;

            // Values off the network must be HTTPS and on a host we own.
            if (uri.Scheme != Uri.UriSchemeHttps) return false;

            return AllowedRemoteHosts.Any(allowed =>
                uri.Host.Equals(allowed, StringComparison.OrdinalIgnoreCase) ||
                uri.Host.EndsWith("." + allowed, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Reads a URL from a file. Accepts either a JSON object with an "apiBaseUrl"
        /// property or a plain text file containing just the URL. Blank lines and lines
        /// starting with # are ignored. Returns null if the file is missing or unreadable.
        /// </summary>
        private static string? ReadUrlFromFile(string path)
        {
            try
            {
                if (!File.Exists(path)) return null;

                var content = File.ReadAllText(path).Trim();
                if (content.Length == 0) return null;

                return content.StartsWith("{") ? ParseJson(content) : ParsePlainText(content);
            }
            catch (Exception ex)
            {
                SafeLog($"ApiConfig: could not read '{path}'.", ex);
                return null;
            }
        }

        private static string? ParseJson(string content)
        {
            try
            {
                using var document = JsonDocument.Parse(content);

                foreach (var property in document.RootElement.EnumerateObject())
                {
                    if (property.NameEquals("apiBaseUrl") && property.Value.ValueKind == JsonValueKind.String)
                        return property.Value.GetString();
                }

                return null;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private static string? ParsePlainText(string content)
        {
            return content
                .Split('\n')
                .Select(line => line.Trim())
                .FirstOrDefault(line => line.Length > 0 && !line.StartsWith("#"));
        }

        private static string? FetchRemoteUrl()
        {
            try
            {
                return Task.Run(async () =>
                {
                    using var client = new HttpClient { Timeout = BootstrapTimeout };
                    var content = await client.GetStringAsync(BootstrapUrl);
                    content = content.Trim();
                    return content.StartsWith("{") ? ParseJson(content) : ParsePlainText(content);
                }).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                // Offline, DNS failure, timeout, 404 - all expected in the field.
                SafeLog("ApiConfig: bootstrap fetch failed, falling back.", ex);
                return null;
            }
        }

        private static void WriteCache(string url)
        {
            try
            {
                Directory.CreateDirectory(ConfigDirectory);
                File.WriteAllText(Path.Combine(ConfigDirectory, CacheFileName), url);
            }
            catch (Exception ex)
            {
                // A machine where the cache cannot be written still works, it just has to
                // reach the bootstrap host on every launch.
                SafeLog("ApiConfig: could not write the cache file.", ex);
            }
        }

        private static void SafeLog(string message, Exception? ex = null)
        {
            try
            {
                if (ex == null)
                    LogHelper.Log(message);
                else
                    LogHelper.Log(message, ex);
            }
            catch
            {
                // Logging runs before anything else at startup; it must not be fatal.
            }
        }
    }
}
