# Setup your machine

Although you can do all your work by automated steps from DevOps, but in reality, you will need sometimes to access Azure manually from your machine to do so check and do some debugging, and check the status or fix some intermittent issues.  

We prefer to run from Azure Powershell, because the capabilities that PowerShell will add to you.  

1. Install or make sure you have [Azure Powershell](https://learn.microsoft.com/en-us/powershell/azure/install-azure-powershell?view=azps-12.1.0) installed

2. Make sure to login to your Azure subscription and get a `context:

If you are not logged in, run the following command: 

```pwsh
Connect-AzAccount
```

After login, and you have multiple subscriptions, setup your context.
 

```pwsh
Get-AzContext # to get the current active context session
Get-AzContext -ListAvailable # to get all available
```

If you have multiple subscriptions/tenats, use the following command to chose the correct one

```pwsh
# to set the active context run one of the following:
Set-AzContext -Context (Get-AzContext -Name <put here your desired context name>) 
Set-AzContext -Name <put here your desired context name>
```
