# Needed to make c# 6.0 work
Framework "4.6"

properties {
    $BuildTools
    $DirectoryStructure
    $GitUserName
    $GitPassword
}

Function Clear-Directory{
    Param (
        [Parameter(Mandatory = $true)]
        [String] $Path
    )
    Process	
    {
        Get-ChildItem -Path $Path -Recurse -Force | 
            Remove-Item -Force -Recurse 
    }
} 

Function Restore-NuGetPackages{
    Param (
        [Parameter(Mandatory = $true)]
        [String] $Path,
        [Parameter(Mandatory = $true)]
        [String] $NuGetPath,
        [Parameter(Mandatory = $true)]
        [String] $OutputDirectory
    )
    Process	{
        Get-ChildItem -Path $Path -Recurse -Filter "packages.config" -ErrorAction SilentlyContinue |
            ForEach-Object -Process {
                $packageConfigFile = $_.FullName
                Invoke-Expression -Command "$NuGetPath install $packageConfigFile -OutputDirectory $OutputDirectory -NonInteractive -Verbosity detailed"
            }
    }
}

Function New-NuGetPackage {
    Param (
        [Parameter(Mandatory = $true)]
        [String] $NuGetPath,
        [Parameter(Mandatory = $true)]
        [String] $NuSpecPath,
        [Parameter(Mandatory = $true)]
        [String] $FilesBasePath,
        [Parameter(Mandatory = $true)]
        [String] $Version,
        [Parameter(Mandatory = $true)]
        [String] $OutputDirectory
    )
    Process	{
        Invoke-Expression -Command "$NuGetPath pack $NuSpecPath -BasePath $FilesBasePath -OutputDirectory $OutputDirectory -Version $Version -NonInteractive -Verbosity detailed"
    }
}

Function Invoke-MSBuild {
    Param (
        [Parameter(Mandatory = $true)]
        [String] $ProjectPath,
        [Parameter(Mandatory = $true)]
        [String] $Configuration,
        [Parameter(Mandatory = $true)]
        [String] $OutputDirectory
    )
    Process	{
        $msbuildCommand = "MSBuild /nologo /target:Rebuild /verbosity:n $ProjectPath /p:Configuration='$Configuration' /p:OutDir='$OutputDirectory'"
        Invoke-Expression -Command $msbuildCommand
    }
}

Function Invoke-XUnit {
    Param (
        [Parameter(Mandatory = $true)]
        [String] $XUnitPath,
        [Parameter(Mandatory = $true)]
        [String] $TestAssemblyPath,
        [Parameter(Mandatory = $true)]
        [String] $TestReportOutputPath
    )
    
    Process	{
        $xunitCommand = "$XUnitPath $TestAssemblyPath -serialize -diagnostics -html $TestReportOutputPath"
        Invoke-Expression -Command $xunitCommand | Out-Host
    }
}

Function Get-VersionVariables {
    Param(
        [Parameter(Mandatory=$false)]
        [string] $GitUserName = "",
    
        [Parameter(Mandatory=$false)]
        [string] $GitPassword = ""
    )
    
    $gitVersion = $BuildTools.GitVersion
    $gitVersionCommand = "$gitVersion /u $GitUserName /p $GitPassword"
    $result = Invoke-Expression -Command $gitVersionCommand

    if ($LASTEXITCODE -ne 0){
        Write-Error 'message' @{ 
            text="GERROR $($result)" 
            }
    } 
    else {
        return ConvertFrom-Json ($result -join "`n");
    }
}

Function New-GlobalAssemblyInfo {
    Param (
        [Parameter(Mandatory = $true)]
        $VersionVariables
    )
    
    Process {
@"
    using System.Reflection;
    
    #if DEBUG
    [assembly: AssemblyConfiguration("Debug")]
    #else
    [assembly: AssemblyConfiguration("Release")]
    #endif
    [assembly: AssemblyCompany("Maurice CGP Peters")]
    [assembly: AssemblyCopyright("Copyright © 2016")]
    [assembly: AssemblyTrademark("")]
    [assembly: AssemblyVersion("$($VersionVariables.AssemblySemVer)")]
    [assembly: AssemblyFileVersion("$($VersionVariables.MajorMinorPatch)")]
    [assembly: AssemblyInformationalVersion("$($VersionVariables.InformationalVersion)")]
    [assembly: AssemblyMetadata("gitCommitHash", "$($VersionVariables.Sha)")]
    
"@      
    }
}

Task Clean -Depends CleanBuildArtifacts, CleanNuGetPackages

Task CleanBuildArtifacts{
    Clear-Directory -Path $DirectoryStructure.TestArtifactsDirectoryInfo.FullName
    Clear-Directory -Path $DirectoryStructure.ProductionCodeArtifactsDirectoryInfo.FullName
}

Task CleanNuGetPackages{
    Clear-Directory -Path $DirectoryStructure.NuGetPackagesDirectoryInfo.FullName
}

Task RestoreNuGetPackages{
    Restore-NuGetPackages -NuGetPath $BuildTools.NuGet -Path $DirectoryStructure.ProductionCodeDirectoryInfo -OutputDirectory $DirectoryStructure.NuGetPackagesDirectoryInfo
    Restore-NuGetPackages -NuGetPath $BuildTools.NuGet -Path $DirectoryStructure.TestsDirectoryInfo -OutputDirectory $DirectoryStructure.NuGetPackagesDirectoryInfo
}

Task Compile {   
    $globalAssemblyInfo = New-GlobalAssemblyInfo -Version (Get-VersionVariables -GitUserName $GitUserName -GitPassword $GitPassword)
    New-Item -ItemType file -Path (Join-Path -Path $DirectoryStructure.ProductionCodeDirectoryInfo -ChildPath "GlobalAssemblyInfo.cs") -Value $globalAssemblyInfo -Force
    
    $productionCodeProjects = Get-ChildItem -Path $DirectoryStructure.ProductionCodeDirectoryInfo -Recurse -Include "*.csproj" -ErrorAction SilentlyContinue
    $productionCodeProjects | ForEach-Object -Process {
        Invoke-MSBuild -Configuration "Debug" -ProjectPath $_.FullName -OutputDirectory (Join-Path -Path $DirectoryStructure.ProductionCodeArtifactsDirectoryInfo -ChildPath $_.Directory.Name)
    }

    $testCodeProjects = Get-ChildItem -Path $DirectoryStructure.TestsDirectoryInfo -Recurse -Include "*.csproj" -ErrorAction SilentlyContinue
    $testCodeProjects | ForEach-Object -Process {
        Invoke-MSBuild -Configuration "Debug" -ProjectPath $_.FullName -OutputDirectory (Join-Path -Path $DirectoryStructure.TestArtifactsDirectoryInfo -ChildPath $_.Directory.Name)
    } 
}

Task Test {  
    $testAssemblies = Get-ChildItem -Path $DirectoryStructure.TestArtifactsDirectoryInfo -Recurse -Include "*.Tests.dll" -ErrorAction SilentlyContinue
    $testAssemblies | ForEach-Object -Process {
        $TestReportOutputPath = Join-Path -Path $_.Directory.FullName -ChildPath "XUnitTestResults.html"
        Invoke-XUnit -XUnitPath $BuildTools.XUnit -TestAssemblyPath $_.FullName -TestReportOutputPath $TestReportOutputPath
    }
}

Task CreateNuGetPackages {  
    $packageVersion = (Get-VersionVariables).NuGetVersionV2
    $nuSpecFiles = Get-ChildItem -Path $DirectoryStructure.ProductionCodeDirectoryInfo -Recurse -Include "*.nuspec" -ErrorAction SilentlyContinue
    $nuSpecFiles | ForEach-Object -Process {
        $artifactsPath = Join-Path -Path $DirectoryStructure.ProductionCodeArtifactsDirectoryInfo -ChildPath $_.Directory.Name    
        New-NuGetPackage -NuGetPath $BuildTools.NuGet -Version $packageVersion -NuSpecPath $_.FullName -FilesBasePath $artifactsPath -OutputDirectory $artifactsPath
    } 
}

Task Default -Depends Clean, RestoreNuGetPackages, Compile, Test, CreateNuGetPackages