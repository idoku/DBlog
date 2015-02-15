#Requires -Version 3.0

<#
.SYNOPSIS
为 Visual Studio Web 项目创建并部署 Microsoft Azure 网站。
有关更详细的文档，请访问: http://go.microsoft.com/fwlink/?LinkID=394471 

.EXAMPLE
PS C:\> .\Publish-WebApplicationWebSite.ps1 `
-Configuration .\Configurations\WebApplication1-WAWS-dev.json `
-WebDeployPackage ..\WebApplication1\WebApplication1.zip `
-Verbose

#>
[CmdletBinding(HelpUri = 'http://go.microsoft.com/fwlink/?LinkID=391696')]
param
(
    [Parameter(Mandatory = $true)]
    [ValidateScript({Test-Path $_ -PathType Leaf})]
    [String]
    $Configuration,

    [Parameter(Mandatory = $false)]
    [String]
    $SubscriptionName,

    [Parameter(Mandatory = $false)]
    [ValidateScript({Test-Path $_ -PathType Leaf})]
    [String]
    $WebDeployPackage,

    [Parameter(Mandatory = $false)]
    [ValidateScript({ !($_ | Where-Object { !$_.Contains('Name') -or !$_.Contains('Password')}) })]
    [Hashtable[]]
    $DatabaseServerPassword,

    [Parameter(Mandatory = $false)]
    [Switch]
    $SendHostMessagesToOutput = $false
)


function New-WebDeployPackage
{
    #编写一个函数以生成和打包 Web 应用程序

    #若要生成 Web 应用程序，请使用 MsBuild.exe。若要寻求帮助，请参见 MSBuild 命令行参考，网址如下: http://go.microsoft.com/fwlink/?LinkId=391339
}

function Test-WebApplication
{
    #编辑此函数以对您的 Web 应用程序运行单元测试

    #若要编写一个函数以对 Web 应用程序运行单元测试，请使用 VSTest.Console.exe。若要寻求帮助，请参见 http://go.microsoft.com/fwlink/?LinkId=391340 上的 VSTest.Console 命令行参考
}

function New-AzureWebApplicationWebsiteEnvironment
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [Object]
        $Configuration,

        [Parameter (Mandatory = $false)]
        [AllowNull()]
        [Hashtable[]]
        $DatabaseServerPassword
    )
       
    Add-AzureWebsite -Name $Config.name -Location $Config.location | Out-String | Write-HostWithTime
    # 创建 SQL 数据库。此连接字符串用于部署。
    $connectionString = New-Object -TypeName Hashtable
    
    if ($Config.Contains('databases'))
    {
        @($Config.databases) |
            Where-Object {$_.connectionStringName -ne ''} |
            Add-AzureSQLDatabases -DatabaseServerPassword $DatabaseServerPassword -CreateDatabase |
            ForEach-Object { $connectionString.Add($_.Name, $_.ConnectionString) }           
    }
    
    return @{ConnectionString = $connectionString}   
}

function Publish-AzureWebApplicationToWebsite
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [Object]
        $Configuration,

        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [Hashtable]
        $ConnectionString,

        [Parameter(Mandatory = $true)]
        [ValidateScript({Test-Path $_ -PathType Leaf})]
        [String]
        $WebDeployPackage
    )

    if ($ConnectionString -and $ConnectionString.Count -gt 0)
    {
        Publish-AzureWebsiteProject `
            -Name $Config.name `
            -Package $WebDeployPackage `
            -ConnectionString $ConnectionString
    }
    else
    {
        Publish-AzureWebsiteProject `
            -Name $Config.name `
            -Package $WebDeployPackage
    }
}


# 脚本主例程
Set-StrictMode -Version 3

Remove-Module AzureWebSitePublishModule -ErrorAction SilentlyContinue
$scriptDirectory = Split-Path -Parent $PSCmdlet.MyInvocation.MyCommand.Definition
Import-Module ($scriptDirectory + '\AzureWebSitePublishModule.psm1') -Scope Local -Verbose:$false

New-Variable -Name VMWebDeployWaitTime -Value 30 -Option Constant -Scope Script 
New-Variable -Name AzureWebAppPublishOutput -Value @() -Scope Global -Force
New-Variable -Name SendHostMessagesToOutput -Value $SendHostMessagesToOutput -Scope Global -Force

try
{
    $originalErrorActionPreference = $Global:ErrorActionPreference
    $originalVerbosePreference = $Global:VerbosePreference
    
    if ($PSBoundParameters['Verbose'])
    {
        $Global:VerbosePreference = 'Continue'
    }
    
    $scriptName = $MyInvocation.MyCommand.Name + ':'
    
    Write-VerboseWithTime ($scriptName + ' 开始')
    
    $Global:ErrorActionPreference = 'Stop'
    Write-VerboseWithTime ('{0} $ErrorActionPreference 设置为 {1}' -f $scriptName, $ErrorActionPreference)
    
    Write-Debug ('{0}: $PSCmdlet.ParameterSetName = {1}' -f $scriptName, $PSCmdlet.ParameterSetName)

    # 保存当前订阅。它稍后将在脚本中还原为“当前”状态
    Backup-Subscription -UserSpecifiedSubscription $SubscriptionName
    
    # 验证您是否装有 Azure 模块 0.7.4 或更高版本。
    if (-not (Test-AzureModule))
    {
         throw '您安装的 Microsoft Azure PowerShell 版本已过时。若要安装最新版本，请访问 http://go.microsoft.com/fwlink/?LinkID=320552。'
    }
    
    if ($SubscriptionName)
    {

        # 如果您提供了订阅名称，请验证您的帐户中是否存在相应的订阅。
        if (!(Get-AzureSubscription -SubscriptionName $SubscriptionName))
        {
            throw ("{0}: 找不到订阅名称 $SubscriptionName" -f $scriptName)

        }

        # 将指定的订阅设为当前订阅。
        Select-AzureSubscription -SubscriptionName $SubscriptionName | Out-Null

        Write-VerboseWithTime ('{0}: 订阅设置为 {1}' -f $scriptName, $SubscriptionName)
    }

    $Config = Read-ConfigFile $Configuration 

    #生成并打包 Web 应用程序
    New-WebDeployPackage

    #对您的 Web 应用程序运行单元测试
    Test-WebApplication

    #创建 JSON 配置文件中描述的 Azure 环境
    $newEnvironmentResult = New-AzureWebApplicationWebsiteEnvironment -Configuration $Config -DatabaseServerPassword $DatabaseServerPassword

    #如果用户指定了 $WebDeployPackage，则部署 Web 应用程序包 
    if($WebDeployPackage)
    {
        Publish-AzureWebApplicationToWebsite `
            -Configuration $Config `
            -ConnectionString $newEnvironmentResult.ConnectionString `
            -WebDeployPackage $WebDeployPackage
    }
}
finally
{
    $Global:ErrorActionPreference = $originalErrorActionPreference
    $Global:VerbosePreference = $originalVerbosePreference

    # 将原来处于当前状态的订阅还原为“当前”状态
    Restore-Subscription

    Write-Output $Global:AzureWebAppPublishOutput    
    $Global:AzureWebAppPublishOutput = @()
}
