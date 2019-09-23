param(
    [Parameter(Mandatory = $true)]
    [string]$Version,
    [switch]$CSharp,
    [switch]$CPP
)

if ($CSharp) {
    Invoke-psake -buildFile .\psakefile.ps1 -taskList PublishCSharp -parameters @{"Version" = $Version }
}

if ($CPP) {
    Invoke-psake -buildFile .\psakefile.ps1 -taskList PublishCPP -parameters @{"Version" = $Version }
}


