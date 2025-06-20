# Require PowerShell 7 or newer cause net standard 2.1 is required
if ($PSVersionTable.PSVersion.Major -lt 7) {
    Write-Warning "This script needs PowerShell 7+. Current version: $($PSVersionTable.PSVersion). Get it here https://github.com/PowerShell/PowerShell/releases/tag/v7.5.1"
    exit 1
}
Set-StrictMode -Version 3.0
# import the library
Add-Type -Path "ArxLibertatisProcGenTools.dll"

[ArxLibertatisProcGenTools.ScriptFunc]::PrintPsDocs()