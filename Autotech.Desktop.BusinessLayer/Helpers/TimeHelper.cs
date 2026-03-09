using System;

namespace Autotech.Desktop.BusinessLayer.Helpers
{
    /// <summary>
    /// Helper class to manage Philippine Time (PHT - UTC+8) consistently across the application.
    /// This ensures all timestamps are in Philippine timezone regardless of server/client local time.
    /// </summary>
    public static class TimeHelper
    {
        private static readonly TimeZoneInfo PhilippineTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");

        /// <summary>
        /// Gets the current date and time in Philippine Time (UTC+8).
        /// This should be used instead of DateTime.Now to ensure consistency.
        /// </summary>
        public static DateTime GetPhilippineTime()
        {
            try
            {
                // Get current UTC time and convert to Philippine Time
                DateTime utcNow = DateTime.UtcNow;
                return TimeZoneInfo.ConvertTimeFromUtc(utcNow, PhilippineTimeZone);
            }
            catch (Exception ex)
            {
                LogHelper.Log($"Error converting to Philippine time: {ex.Message}");
                // Fallback to local machine time if timezone conversion fails
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Converts a UTC DateTime to Philippine Time.
        /// </summary>
        public static DateTime ConvertUtcToPhilippineTime(DateTime utcDateTime)
        {
            if (utcDateTime.Kind != DateTimeKind.Utc)
            {
                LogHelper.Log("Warning: Provided DateTime is not marked as UTC. Treating it as UTC anyway.");
            }

            try
            {
                return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, PhilippineTimeZone);
            }
            catch (Exception ex)
            {
                LogHelper.Log($"Error converting UTC to Philippine time: {ex.Message}");
                return utcDateTime;
            }
        }

        /// <summary>
        /// Converts a Philippine Time DateTime to UTC.
        /// </summary>
        public static DateTime ConvertPhilippineTimeToUtc(DateTime philDateTime)
        {
            try
            {
                return TimeZoneInfo.ConvertTimeToUtc(philDateTime, PhilippineTimeZone);
            }
            catch (Exception ex)
            {
                LogHelper.Log($"Error converting Philippine time to UTC: {ex.Message}");
                return philDateTime;
            }
        }
    }
}
