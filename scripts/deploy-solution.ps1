# $rndm = Get-Random -Minimum -100 -Maximum 100
New-AzResourceGroupDeployment -Name "Deploy-manual" -ResourceGroupName 'dev-PropertyManagement' `
    -TemplateFile ../infra/main.bicep -TemplateParameterFile  ../infra/dev.params.json