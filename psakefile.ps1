Task PublishCSharp -Depends PackCSharp {
}

Task PackCSharp -Depends BuildCSharp, EstimateVersions {
    $project = "./src/CSharp/CSharp.csproj"
    $project = Resolve-Path $project
    $project = $project.Path

    Exec { dotnet pack --configuration Release --output $script:trashFolder -p:version=$script:NextVersion $project }
}

Task EstimateVersions {
    $Version = [Version]$Version

    Assert ($Version.Revision -eq -1) "Version should be formatted as Major.Minor.Patch like 1.2.3"
    Assert ($Version.Build -ne -1) "Version should be formatted as Major.Minor.Patch like 1.2.3"

    $Version = $Version.ToString()
    $script:NextVersion = $Version
}

Task BuildCSharp -Depends Clean {
    $solution = Resolve-Path "./src/Messaging.sln"
    $solution = $solution.Path
    Exec { dotnet restore $solution }
    Exec { dotnet build $solution }
}

Task Clean -Depends Init {
}

Task Init {
    $date = Get-Date
    $ticks = $date.Ticks
    $script:trashFolder = Join-Path -Path . -ChildPath ".trash"
    $script:trashFolder = Join-Path -Path $script:trashFolder -ChildPath $ticks.ToString("D19")
    New-Item -Path $script:trashFolder -ItemType Directory | Out-Null
    $script:trashFolder = Resolve-Path -Path $script:trashFolder
}