Function Start-Build {
	Param (
		[Parameter(Mandatory = $true)]
		$DirectoryStructure,     
        [Parameter(Mandatory = $true)]
        $BuildTools,
        [Parameter(Mandatory = $false)]
        [string] $GitUserName,
        [Parameter(Mandatory = $false)]
        [string] $GitPassword
	)
	DynamicParam {
		# Set the dynamic parameters' name
		$ParameterName = 'Configuration'
		
		# Create the dictionary 
		$RuntimeParameterDictionary = New-Object System.Management.Automation.RuntimeDefinedParameterDictionary

		# Create the collection of attributes
		$AttributeCollection = New-Object System.Collections.ObjectModel.Collection[System.Attribute]
		
		# Create and set the parameters' attributes
		$ParameterAttribute = New-Object System.Management.Automation.ParameterAttribute
		$ParameterAttribute.Mandatory = $true

		# Add the attributes to the attributes collection
		$AttributeCollection.Add($ParameterAttribute)

		# Generate and set the ValidateSet 
		$arrSet = 
            Get-ChildItem -Filter *.ps1 -File -Path (Join-Path -Path $DirectoryStructure.BuildDirectoryInfo.FullName -ChildPath "configuration") | 
            Select-Object -ExpandProperty BaseName            
        
		$ValidateSetAttribute = New-Object System.Management.Automation.ValidateSetAttribute($arrSet)

		# Add the ValidateSet to the attributes collection
		$AttributeCollection.Add($ValidateSetAttribute)

		# Create and return the dynamic parameter
		$RuntimeParameter = New-Object System.Management.Automation.RuntimeDefinedParameter($ParameterName, [string], $AttributeCollection)
		$RuntimeParameterDictionary.Add($ParameterName, $RuntimeParameter)
		return $RuntimeParameterDictionary
    }
    Begin {
        Get-Module Psake | Remove-Module
		Import-Module (Join-Path -Path $DirectoryStructure.BuildToolsDirectoryInfo.FullName -ChildPath "Psake\tools\Psake.psm1")
    }
    Process {
        Invoke-Psake (Join-Path -Path $DirectoryStructure.BuildDirectoryInfo.FullName -ChildPath ("configuration\" + $PSBoundParameters.Configuration + ".ps1")) -Parameters @{"BuildTools"=$BuildTools; "DirectoryStructure"=$DirectoryStructure; "GitUserName"=$GitUserName; "GitPassword"=$GitPassword}
    }
    End {
        Remove-Module Psake   
    }
}