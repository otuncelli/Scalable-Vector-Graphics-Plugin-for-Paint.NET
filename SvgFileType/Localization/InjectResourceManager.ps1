(Get-Content 'Localization\StringResources.Designer.cs').replace('new global::System.Resources.ResourceManager', 'new SingleAssemblyResourceManager') | Set-Content 'Localization\StringResources.Designer.cs' -Encoding utf8