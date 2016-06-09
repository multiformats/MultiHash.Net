param (
	[string] $Configuration,
    [string] $GitUserName = "",
    [string] $GitPassword = ""
)

$rootDirectory = Resolve-Path -Path "."

Set-Variable -Name ProductionCodeDirectoryName  -Value 'src'          -Option Constant
Set-Variable -Name DocumentationDirectoryName   -Value 'docs'         -Option Constant
Set-Variable -Name TestCodeDirectoryName        -Value 'tests'        -Option Constant
Set-Variable -Name SamplesDirectoryName         -Value 'samples'      -Option Constant
Set-Variable -Name BuildDirectoryName           -Value 'build' 	      -Option Constant
Set-Variable -Name ArtifactsDirectoryName       -Value 'artifacts'    -Option Constant
Set-Variable -Name NugetPackagesDirectoryName   -Value 'packages'     -Option Constant


New-Item -ItemType Directory -Path $rootDirectory -Name $NugetPackagesDirectoryName -Force
New-Item -ItemType Directory -Path $rootDirectory -Name $DocumentationDirectoryName -Force
New-Item -ItemType Directory -Path $rootDirectory -Name $ArtifactsDirectoryName -Force
New-Item -ItemType Directory -Path (Join-Path -Path $rootDirectory -ChildPath $ArtifactsDirectoryName) -Name $ProductionCodeDirectoryName -Force
New-Item -ItemType Directory -Path (Join-Path -Path $rootDirectory -ChildPath $ArtifactsDirectoryName) -Name $TestCodeDirectoryName -Force

$directoryStructure = @{	
    RootDirectory = $rootDirectory
    DocumentationDirectoryInfo = Join-Path -Path $rootDirectory -ChildPath $DocumentationDirectoryName | Get-Item
    ProductionCodeDirectoryInfo = Join-Path -Path $rootDirectory -ChildPath $ProductionCodeDirectoryName | Get-Item
    TestsDirectoryInfo = Join-Path -Path $rootDirectory -ChildPath $TestCodeDirectoryName | Get-Item
    BuildDirectoryInfo = Join-Path -Path $rootDirectory -ChildPath $BuildDirectoryName | Get-Item
    BuildToolsDirectoryInfo = Join-Path -Path $rootDirectory -ChildPath "$BuildDirectoryName\tools" | Get-Item
    ArtifactsDirectoryInfo = Join-Path -Path $rootDirectory -ChildPath $ArtifactsDirectoryName | Get-Item
    TestArtifactsDirectoryInfo = Join-Path -Path $rootDirectory -ChildPath "$ArtifactsDirectoryName\$TestCodeDirectoryName" | Get-Item
    ProductionCodeArtifactsDirectoryInfo = Join-Path -Path $rootDirectory -ChildPath "$ArtifactsDirectoryName\$ProductionCodeDirectoryName" | Get-Item
    NuGetPackagesDirectoryInfo = Join-Path -Path $rootDirectory -ChildPath $NugetPackagesDirectoryName | Get-Item
}

Get-ChildItem -Path $directoryStructure.BuildToolsDirectoryInfo -Exclude "nuget.*" | Remove-Item -Recurse -Force

$nugetPath = Join-Path -Path $directoryStructure.BuildToolsDirectoryInfo -ChildPath "nuget.exe"

& $nugetPath install "Psake" -OutputDirectory $directoryStructure.BuildToolsDirectoryInfo.FullName -ExcludeVersion -NonInteractive 
& $nugetPath install "GitVersion.CommandLine" -OutputDirectory $directoryStructure.BuildToolsDirectoryInfo.FullName -ExcludeVersion -NonInteractive
& $nugetPath install "xunit.runner.console" -OutputDirectory $directoryStructure.BuildToolsDirectoryInfo.FullName -ExcludeVersion -NonInteractive

$buildTools = @{
	NuGet = $nugetPath
	GitVersion = Join-Path -Path $directoryStructure.BuildToolsDirectoryInfo.FullName -ChildPath "GitVersion.CommandLine\tools\GitVersion.exe"
	XUnit = Join-Path -Path $directoryStructure.BuildToolsDirectoryInfo.FullName -ChildPath "xunit.runner.console\tools\xunit.console.exe" 
}

Import-Module .\build\Build.psm1

Start-Build -Configuration $Configuration -DirectoryStructure $directoryStructure -BuildTools $buildTools -GitUserName $GitUserName -GitPassword $GitPassword

Remove-Module Build