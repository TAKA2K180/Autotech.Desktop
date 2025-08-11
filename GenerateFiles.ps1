$publishPath = "F:\Autotech\Autotech.Desktop\Autotech.Desktop.Main\bin\Release\net6.0-windows\win-x64"
$outputWxs = "F:\Autotech\Autotech.Desktop\Autotech.Desktop.Setup\ProductFiles.wxs"

$componentList = @()
$componentRefs = @()

Get-ChildItem -Path $publishPath -File | ForEach-Object {
    $guid = [guid]::NewGuid().ToString()
    # Use full name (with extension) to ensure unique Id
    $idSafeName = $_.Name.Replace('.', '_').Replace('-', '_')
    $relativePath = $_.Name

    $component = @"
    <Component Id='Comp_$idSafeName' Guid='{$guid}'>
        <File Source='`$(var.Autotech.Desktop.Main.TargetDir)$relativePath' KeyPath='yes' />
    </Component>
"@
    $componentList += $component
    $componentRefs += "      <ComponentRef Id='Comp_$idSafeName' />"
}

$wxsContent = @"
<?xml version="1.0" encoding="UTF-8"?>
<Wix xmlns="http://schemas.microsoft.com/wix/2006/wi">
  <Fragment>
    <DirectoryRef Id="INSTALLFOLDER">
      $(($componentList -join "`n      "))
    </DirectoryRef>

    <ComponentGroup Id="ProductComponents">
      $(($componentRefs -join "`n      "))
    </ComponentGroup>
  </Fragment>
</Wix>
"@

Set-Content -Path $outputWxs -Value $wxsContent
Write-Host "ProductFiles.wxs generated at $outputWxs"
