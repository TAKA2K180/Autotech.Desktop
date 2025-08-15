# version.ps1 - Auto Versioning Script for Autotech Desktop
$majorMinor = "24.1"
$dateCode = (Get-Date -Format "MMdd")
$buildNumberFile = "$PSScriptRoot\build_number.txt"

# Initialize build number file if not exists
if (!(Test-Path $buildNumberFile)) {
    Set-Content $buildNumberFile 100
}

$buildNumber = [int](Get-Content $buildNumberFile)
$version = "$majorMinor.$dateCode.$buildNumber"

Write-Host "Building version: $version"

# Increment for next build
($buildNumber + 1) | Set-Content $buildNumberFile

# Save version to version.txt for installer
Set-Content -Path "$PSScriptRoot\version.txt" -Value $version

# Path to Main project
$projectFile = "$PSScriptRoot\Autotech.Desktop.Main\Autotech.Desktop.Main.csproj"

# Update .csproj version fields
(Get-Content $projectFile) `
    -replace '<Version>.*</Version>', "<Version>$version</Version>" `
    -replace '<AssemblyVersion>.*</AssemblyVersion>', "<AssemblyVersion>$version</AssemblyVersion>" `
    -replace '<FileVersion>.*</FileVersion>', "<FileVersion>$version</FileVersion>" `
    -replace '<InformationalVersion>.*</InformationalVersion>', "<InformationalVersion>$version</InformationalVersion>" `
| Set-Content $projectFile

# -------------------
# BUILD .NET PROJECT
# -------------------
Write-Host "Restoring packages from official NuGet..."
dotnet restore "$PSScriptRoot\Autotech.Desktop.Main\Autotech.Desktop.Main.csproj" --source "https://api.nuget.org/v3/index.json"
Write-Host "Publishing .NET WinForms project..."
dotnet publish "$PSScriptRoot\Autotech.Desktop.Main\Autotech.Desktop.Main.csproj" -c Release -r win-x64 --self-contained true

# -------------------
# BUILD WIX INSTALLER
# -------------------
Write-Host "Building WiX Installer with version $version ..."
$wixProj = "$PSScriptRoot\Autotech.Desktop.Setup\Autotech.Desktop.Setup.wixproj"

# Detect MSBuild
$msbuildPath2022 = "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
$msbuildPath2019 = "C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"

if (Test-Path $msbuildPath2022) {
    $msbuildPath = $msbuildPath2022
} elseif (Test-Path $msbuildPath2019) {
    $msbuildPath = $msbuildPath2019
} else {
    $msbuildPath = "msbuild"
}

Write-Host "Using MSBuild from: $msbuildPath"
& "$msbuildPath" "$wixProj" /p:Configuration=Release /p:ProductVersion=$version

Write-Host "Installer build complete. MSI will be available in Autotech.Desktop.Setup\bin\Release"
