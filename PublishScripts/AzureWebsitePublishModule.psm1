#  AzureWebSitePublishModule.psm1 是一个 Windows PowerShell 脚本模块。此模块导出可自动执行 Web 应用程序的生命周期管理的 Windows PowerShell 函数。您可按原样使用这些函数，也可以针对您的应用程序和发布环境自定义这些函数。

Set-StrictMode -Version 3

# 用来保存原始订阅的变量。
$Script:originalCurrentSubscription = $null

# 用来保存存储帐户的变量。
$Script:originalCurrentStorageAccount = $null

# 用来保存用户指定订阅的存储帐户的变量。
$Script:originalStorageAccountOfUserSpecifiedSubscription = $null

# 用来保存订阅名称的变量。
$Script:userSpecifiedSubscription = $null


<#
.SYNOPSIS
在消息前添加日期和时间作为前缀。

.DESCRIPTION
在消息前添加日期和时间作为前缀。此函数是针对写入到 Error 和 Verbose 流的消息而设计的。

.PARAMETER  Message
指定不带日期的消息。

.INPUTS
System.String

.OUTPUTS
System.String

.EXAMPLE
PS C:\> Format-DevTestMessageWithTime -Message "将 $filename 文件添加到目录"
2/5/2014 1:03:08 PM - 将 $filename 文件添加到目录

.LINK
Write-VerboseWithTime

.LINK
Write-ErrorWithTime
#>
function Format-DevTestMessageWithTime
{
    [CmdletBinding()]
    param
    (
        [Parameter(Position=0, Mandatory = $true, ValueFromPipeline = $true)]
        [String]
        $Message
    )

    return ((Get-Date -Format G)  + ' - ' + $Message)
}


<#

.SYNOPSIS
写入一条添加了当前时间作为前缀的错误消息。

.DESCRIPTION
写入一条添加了当前时间作为前缀的错误消息。此函数会先调用 Format-DevTestMessageWithTime 函数来添加时间作为前缀，然后再将消息写入到 Error 流中。

.PARAMETER  Message
指定在错误消息调用过程中使用的消息。您可以通过管道将消息字符串传送给此函数。

.INPUTS
System.String

.OUTPUTS
无。此函数向 Error 流中写入数据。

.EXAMPLE
PS C:> Write-ErrorWithTime -Message "Failed. Cannot find the file."

Write-Error: 2/6/2014 8:37:29 AM - Failed. Cannot find the file.
 + CategoryInfo     : NotSpecified: (:) [Write-Error], WriteErrorException
 + FullyQualifiedErrorId : Microsoft.PowerShell.Commands.WriteErrorException

.LINK
Write-Error

#>
function Write-ErrorWithTime
{
    [CmdletBinding()]
    param
    (
        [Parameter(Position=0, Mandatory = $true, ValueFromPipeline = $true)]
        [String]
        $Message
    )

    $Message | Format-DevTestMessageWithTime | Write-Error
}


<#
.SYNOPSIS
写入一条添加了当前时间作为前缀的详细消息。

.DESCRIPTION
写入一条添加了当前时间作为前缀的详细消息。由于它会调用 Write-Verbose，因此该消息仅在脚本使用 Verbose 参数运行时或者在 VerbosePreference 首选项设置为 Continue 时才会显示。

.PARAMETER  Message
指定在详细消息调用过程中使用的消息。您可以通过管道将消息字符串传送给此函数。

.INPUTS
System.String

.OUTPUTS
无。此函数向 Verbose 流中写入数据。

.EXAMPLE
PS C:> Write-VerboseWithTime -Message "The operation succeeded."
PS C:>
PS C:\> Write-VerboseWithTime -Message "The operation succeeded." -Verbose
VERBOSE: 1/27/2014 11:02:37 AM - The operation succeeded.

.EXAMPLE
PS C:\ps-test> "The operation succeeded." | Write-VerboseWithTime -Verbose
VERBOSE: 1/27/2014 11:01:38 AM - The operation succeeded.

.LINK
Write-Verbose
#>
function Write-VerboseWithTime
{
    [CmdletBinding()]
    param
    (
        [Parameter(Position=0, Mandatory = $true, ValueFromPipeline = $true)]
        [String]
        $Message
    )

    $Message | Format-DevTestMessageWithTime | Write-Verbose
}


<#
.SYNOPSIS
写入一条添加了当前时间作为前缀的宿主消息。

.DESCRIPTION
此函数向宿主程序(Write-Host)写入一条添加了当前时间作为前缀的消息。写入到宿主程序所产生的影响不尽相同。大多数用作 Windows PowerShell 宿主的程序都会将这些消息写入到标准输出。

.PARAMETER  Message
指定不带日期的基础消息。您可以通过管道将消息字符串传送给此函数。

.INPUTS
System.String

.OUTPUTS
无。此函数将消息写入到宿主程序。

.EXAMPLE
PS C:> Write-HostWithTime -Message "操作已成功。"
1/27/2014 11:02:37 AM - 操作已成功。

.LINK
Write-Host
#>
function Write-HostWithTime
{
    [CmdletBinding()]
    param
    (
        [Parameter(Position=0, Mandatory = $true, ValueFromPipeline = $true)]
        [String]
        $Message
    )
    
    if ((Get-Variable SendHostMessagesToOutput -Scope Global -ErrorAction SilentlyContinue) -and $Global:SendHostMessagesToOutput)
    {
        if (!(Get-Variable -Scope Global AzureWebAppPublishOutput -ErrorAction SilentlyContinue) -or !$Global:AzureWebAppPublishOutput)
        {
            New-Variable -Name AzureWebAppPublishOutput -Value @() -Scope Global -Force
        }

        $Global:AzureWebAppPublishOutput += $Message | Format-DevTestMessageWithTime
    }
    else 
    {
        $Message | Format-DevTestMessageWithTime | Write-Host
    }
}


<#
.SYNOPSIS
如果属性或方法为对象成员，则返回 $true。否则返回 $false。

.DESCRIPTION
如果属性或方法为对象成员，则返回 $true。对于类的静态方法以及视图(如 PSBase 和 PSObject)，此函数返回 $false。

.PARAMETER  Object
指定在测试过程中使用的对象。请输入一个包含对象或者包含可返回对象的表达式的变量。您不能指定类型(例如 [DateTime])，也不能通过管道向此函数传送对象。

.PARAMETER  Member
指定在测试过程中使用的属性或方法的名称。指定方法时，请省略方法名后面的圆括号。

.INPUTS
无。此函数不从管道中获取任何输入。

.OUTPUTS
System.Boolean

.EXAMPLE
PS C:\> Test-Member -Object (Get-Date) -Member DayOfWeek
True

.EXAMPLE
PS C:\> $date = Get-Date
PS C:\> Test-Member -Object $date -Member AddDays
True

.EXAMPLE
PS C:\> [DateTime]::IsLeapYear((Get-Date).Year)
True
PS C:\> Test-Member -Object (Get-Date) -Member IsLeapYear
False

.LINK
Get-Member
#>
function Test-Member
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [Object]
        $Object,

        [Parameter(Mandatory = $true)]
        [String]
        $Member
    )

    return $null -ne ($Object | Get-Member -Name $Member)
}


<#
.SYNOPSIS
如果 Azure 模块的版本为 0.7.4 或更高版本，则返回 $true。否则返回 $false。

.DESCRIPTION
如果 Azure 模块的版本为 0.7.4 或更高版本，则 Test-AzureModuleVersion 会返回 $true。如果该模块未安装或为更低的版本，则此函数返回 $false。此函数无参数。

.INPUTS
无

.OUTPUTS
System.Boolean

.EXAMPLE
PS C:\> Get-Module Azure -ListAvailable
PS C:\> #No module
PS C:\> Test-AzureModuleVersion
False

.EXAMPLE
PS C:\> (Get-Module Azure -ListAvailable).Version

Major  Minor  Build  Revision
-----  -----  -----  --------
0      7      4      -1

PS C:\> Test-AzureModuleVersion
True

.LINK
Get-Module

.LINK
PSModuleInfo object (http://msdn.microsoft.com/en-us/library/system.management.automation.psmoduleinfo(v=vs.85).aspx)
#>
function Test-AzureModuleVersion
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [ValidateNotNull()]
        [System.Version]
        $Version
    )

    return ($Version.Major -gt 0) -or ($Version.Minor -gt 7) -or ($Version.Minor -eq 7 -and $Version.Build -ge 4)
}


<#
.SYNOPSIS
如果所安装的 Azure 模块版本为 0.7.4 或更高版本，则返回 $true。

.DESCRIPTION
如果所安装的 Azure 模块版本为 0.7.4 或更高版本，则 Test-AzureModule 返回 $true。如果该模块未安装或为更低的版本，则返回 $false。此函数无参数。

.INPUTS
无

.OUTPUTS
System.Boolean

.EXAMPLE
PS C:\> Get-Module Azure -ListAvailable
PS C:\> #No module
PS C:\> Test-AzureModule
False

.EXAMPLE
PS C:\> (Get-Module Azure -ListAvailable).Version

Major  Minor  Build  Revision
-----  -----  -----  --------
    0      7      4      -1

PS C:\> Test-AzureModule
True

.LINK
Get-Module

.LINK
PSModuleInfo object (http://msdn.microsoft.com/en-us/library/system.management.automation.psmoduleinfo(v=vs.85).aspx)
#>
function Test-AzureModule
{
    [CmdletBinding()]

    $module = Get-Module -Name Azure

    if (!$module)
    {
        $module = Get-Module -Name Azure -ListAvailable

        if (!$module -or !(Test-AzureModuleVersion $module.Version))
        {
            return $false;
        }
        else
        {
            $ErrorActionPreference = 'Continue'
            Import-Module -Name Azure -Global -Verbose:$false
            $ErrorActionPreference = 'Stop'

            return $true
        }
    }
    else
    {
        return (Test-AzureModuleVersion $module.Version)
    }
}


<#
.SYNOPSIS
将当前 Microsoft Azure 订阅保存在脚本作用域内的 $Script:originalSubscription 变量中。

.DESCRIPTION
Backup-Subscription 函数将当前 Microsoft Azure 订阅(Get-AzureSubscription -Current)及其存储帐户以及由脚本($UserSpecifiedSubscription)更改的订阅及其存储帐户保存在脚本作用域内。通过保存这些值，在当前状态发生更改的情况下，可以使用函数(如 Restore-Subscription)来将原来的当前状态的订阅和存储帐户还原为当前状态。

.PARAMETER UserSpecifiedSubscription
指定将在其中创建和发布新资源的订阅的名称。该函数会将该订阅及其存储帐户的名称保存在脚本作用域中。此参数是必需的。

.INPUTS
无

.OUTPUTS
无

.EXAMPLE
PS C:\> Backup-Subscription -UserSpecifiedSubscription Contoso
PS C:\>

.EXAMPLE
PS C:\> Backup-Subscription -UserSpecifiedSubscription Contoso -Verbose
VERBOSE: Backup-Subscription: Start
VERBOSE: Backup-Subscription: Original subscription is Microsoft Azure MSDN - Visual Studio Ultimate
VERBOSE: Backup-Subscription: End
#>
function Backup-Subscription
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [AllowEmptyString()]
        [string]
        $UserSpecifiedSubscription
    )

    Write-VerboseWithTime 'Backup-Subscription: 开始'

    $Script:originalCurrentSubscription = Get-AzureSubscription -Current -ErrorAction SilentlyContinue
    if ($Script:originalCurrentSubscription)
    {
        Write-VerboseWithTime ('Backup-Subscription: 原始订阅为 ' + $Script:originalCurrentSubscription.SubscriptionName)
        $Script:originalCurrentStorageAccount = $Script:originalCurrentSubscription.CurrentStorageAccountName
    }
    
    $Script:userSpecifiedSubscription = $UserSpecifiedSubscription
    if ($Script:userSpecifiedSubscription)
    {        
        $userSubscription = Get-AzureSubscription -SubscriptionName $Script:userSpecifiedSubscription -ErrorAction SilentlyContinue
        if ($userSubscription)
        {
            $Script:originalStorageAccountOfUserSpecifiedSubscription = $userSubscription.CurrentStorageAccountName
        }        
    }

    Write-VerboseWithTime 'Backup-Subscription: 结束'
}


<#
.SYNOPSIS
将保存在脚本作用域内的 $Script:originalSubscription 变量中的 Microsoft Azure 订阅还原为“当前”状态。

.DESCRIPTION
Restore-Subscription 函数将 $Script:originalSubscription 变量中保存的订阅设为当前订阅(再次)。如果原始订阅有对应的存储帐户，此函数会将该存储帐户设为当前订阅的当前帐户。此函数仅在环境中存在非 Null $SubscriptionName 变量时才还原订阅。否则，此函数将退出。如果 $SubscriptionName 内已填入值，但 $Script:originalSubscription 为 $null，Restore-Subscription 将使用 Select-AzureSubscription cmdlet 清除 Microsoft Azure PowerShell 内订阅的当前设置和默认设置。此函数无参数，不接受任何输入，也不返回任何值(void)。您可以使用 -Verbose 来向 Verbose 流写入消息。

.INPUTS
无

.OUTPUTS
无

.EXAMPLE
PS C:\> Restore-Subscription
PS C:\>

.EXAMPLE
PS C:\> Restore-Subscription -Verbose
VERBOSE: Restore-Subscription: Start
VERBOSE: Restore-Subscription: End
#>
function Restore-Subscription
{
    [CmdletBinding()]
    param()

    Write-VerboseWithTime 'Restore-Subscription: 开始'

    if ($Script:originalCurrentSubscription)
    {
        if ($Script:originalCurrentStorageAccount)
        {
            Set-AzureSubscription `
                -SubscriptionName $Script:originalCurrentSubscription.SubscriptionName `
                -CurrentStorageAccountName $Script:originalCurrentStorageAccount
        }

        Select-AzureSubscription -SubscriptionName $Script:originalCurrentSubscription.SubscriptionName
    }
    else 
    {
        Select-AzureSubscription -NoCurrent
        Select-AzureSubscription -NoDefault
    }
    
    if ($Script:userSpecifiedSubscription -and $Script:originalStorageAccountOfUserSpecifiedSubscription)
    {
        Set-AzureSubscription `
            -SubscriptionName $Script:userSpecifiedSubscription `
            -CurrentStorageAccountName $Script:originalStorageAccountOfUserSpecifiedSubscription
    }

    Write-VerboseWithTime 'Restore-Subscription: 结束'
}


<#
.SYNOPSIS
验证配置文件并返回一个由配置文件值组成的哈希表。

.DESCRIPTION
Read-ConfigFile 函数验证 JSON 配置文件并返回由所选值组成的哈希表。
-- 它首先将 JSON 文件转换为 PSCustomObject。网站哈希表具有下列键:
-- Location: 网站位置
-- Databases: 网站 SQL 数据库

.PARAMETER  ConfigurationFile
指定您 Web 项目的 JSON 配置文件的路径及名称。Visual Studio 会在您创建 Web 项目时自动生成 JSON 文件，并将该文件存储在您解决方案中的 PublishScripts 文件夹中。

.PARAMETER HasWebDeployPackage
指示 Web 应用程序有一个 Web 部署包 ZIP 文件。若要指定 $true 值，请使用 -HasWebDeployPackage 或 HasWebDeployPackage:$true。若要指定 false 值，请使用 HasWebDeployPackage:$false。此参数为必填项。

.INPUTS
无。您不能通过管道向此函数传送输入。

.OUTPUTS
System.Collections.Hashtable

.EXAMPLE
PS C:\> Read-ConfigFile -ConfigurationFile <path> -HasWebDeployPackage


Name                           Value                                                                                                                                                                     
----                           -----                                                                                                                                                                     
databases                      {@{connectionStringName=; databaseName=; serverName=; user=; password=}}                                                                                                  
website                        @{name="mysite"; location="West US";}                                                      
#>
function Read-ConfigFile
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [ValidateScript({Test-Path $_ -PathType Leaf})]
        [String]
        $ConfigurationFile
    )

    Write-VerboseWithTime 'Read-ConfigFile: 开始'

    # 获取 JSON 文件的内容(使用 -raw 时会忽略换行符)并将其转换成 PSCustomObject
    $config = Get-Content $ConfigurationFile -Raw | ConvertFrom-Json

    if (!$config)
    {
        throw ('Read-ConfigFile: ConvertFrom-Json 失败: ' + $error[0])
    }

    # 确定 environmentSettings 对象是否具有“webSite”属性(不考虑属性值)
    $hasWebsiteProperty =  Test-Member -Object $config.environmentSettings -Member 'webSite'

    if (!$hasWebsiteProperty)
    {
        throw 'Read-ConfigFile: 配置文件没有 webSite 属性。'
    }

    # 使用 PSCustomObject 中的值生成一个哈希表
    $returnObject = New-Object -TypeName Hashtable

    $returnObject.Add('name', $config.environmentSettings.webSite.name)
    $returnObject.Add('location', $config.environmentSettings.webSite.location)

    if (Test-Member -Object $config.environmentSettings -Member 'databases')
    {
        $returnObject.Add('databases', $config.environmentSettings.databases)
    }

    Write-VerboseWithTime 'Read-ConfigFile: 结束'

    return $returnObject
}


<#
.SYNOPSIS
创建一个 Microsoft Azure 网站。

.DESCRIPTION
使用特定名称和位置创建一个 Microsoft Azure 网站。此函数调用 Azure 模块中的 New-AzureWebsite cmdlet。如果订阅没有具有指定名称的网站，则此函数将创建该网站并返回一个网站对象。否则，它将返回现有网站。

.PARAMETER  Name
为新网站指定一个名称。此名称在 Microsoft Azure 中必须是唯一的。此参数是必需的。

.PARAMETER  Location
指定网站的位置。有效的值为 Microsoft Azure 位置，例如“West US”。此参数是必需的。

.INPUTS
无。

.OUTPUTS
Microsoft.WindowsAzure.Commands.Utilities.Websites.Services.WebEntities.Site

.EXAMPLE
Add-AzureWebsite -Name TestSite -Location "West US"

Name       : contoso
State      : Running
Host Names : contoso.azurewebsites.net

.LINK
New-AzureWebsite
#>
function Add-AzureWebsite
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [String]
        $Name,

        [Parameter(Mandatory = $true)]
        [String]
        $Location
    )

    Write-VerboseWithTime 'Add-AzureWebsite: 开始'
    $website = Get-AzureWebsite -Name $Name -ErrorAction SilentlyContinue

    if ($website)
    {
        Write-HostWithTime ('Add-AzureWebsite: 现有网站 ' +
        $website.Name + ' 已找到')
    }
    else
    {
        if (Test-AzureName -Website -Name $Name)
        {
            Write-ErrorWithTime ('网站 {0} 已存在' -f $Name)
        }
        else
        {
            $website = New-AzureWebsite -Name $Name -Location $Location
        }
    }

    $website | Out-String | Write-VerboseWithTime
    Write-VerboseWithTime 'Add-AzureWebsite: 结束'

    return $website
}

<#
.SYNOPSIS
当该 URL 为绝对 URL 且其方案为 https 时，返回 $True。

.DESCRIPTION
Test-HttpsUrl 函数将输入 URL 转换成 System.Uri 对象。当该 URL 为绝对(而非相对) URL 且其方案为 https 时，此函数返回 $True。如果上述两项条件中有任意一项条件不满足，或者输入字符串无法转换为 URL，则此函数将返回 $false。

.PARAMETER Url
指定要测试的 URL。请输入一个 URL 字符串。

.INPUTS
无。

.OUTPUTS
System.Boolean

.EXAMPLE
PS C:\>$profile.publishUrl
waws-prod-bay-001.publish.azurewebsites.windows.net:443

PS C:\>Test-HttpsUrl -Url 'waws-prod-bay-001.publish.azurewebsites.windows.net:443'
False
#>
function Test-HttpsUrl
{

    param
    (
        [Parameter(Mandatory = $true)]
        [String]
        $Url
    )

    # 如果无法将 $uri 转换成 System.Uri 对象，则 Test-HttpsUrl 将返回 $false
    $uri = $Url -as [System.Uri]

    return $uri.IsAbsoluteUri -and $uri.Scheme -eq 'https'
}


<#
.SYNOPSIS
创建一个供您连接 Microsoft Azure SQL 数据库的字符串。

.DESCRIPTION
Get-AzureSQLDatabaseConnectionString 函数会对连接字符串进行汇编，以便连接到 Microsoft Azure SQL 数据库。

.PARAMETER  DatabaseServerName
指定 Microsoft Azure 订阅中现有数据库服务器的名称。所有 Microsoft Azure SQL 数据库都必须与 SQL 数据库服务器关联。要获取服务器名称，请使用 Get-AzureSqlDatabaseServer cmdlet (Azure 模块)。此参数是必需的。

.PARAMETER  DatabaseName
指定 SQL 数据库的名称。此名称可以是现有 SQL 数据库的名称，也可以是用于新 SQL 数据库的名称。此参数是必需的。

.PARAMETER  Username
指定 SQL 数据库管理员的名称。此用户名将为 $Username@DatabaseServerName。此参数是必需的。

.PARAMETER  Password
指定 SQL 数据库管理员的密码。请以纯文本格式输入密码。不允许使用安全字符串。此参数是必需的。

.INPUTS
无。

.OUTPUTS
System.String

.EXAMPLE
PS C:\> $ServerName = (Get-AzureSqlDatabaseServer).ServerName[0]
PS C:\> Get-AzureSQLDatabaseConnectionString -DatabaseServerName $ServerName `
        -DatabaseName 'testdb' -UserName 'admin'  -Password 'password'

Server=tcp:testserver.database.windows.net,1433;Database=testdb;User ID=admin@testserver;Password=password;Trusted_Connection=False;Encrypt=True;Connection Timeout=20;
#>
function Get-AzureSQLDatabaseConnectionString
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [String]
        $DatabaseServerName,

        [Parameter(Mandatory = $true)]
        [String]
        $DatabaseName,

        [Parameter(Mandatory = $true)]
        [String]
        $UserName,

        [Parameter(Mandatory = $true)]
        [String]
        $Password
    )

    return ('Server=tcp:{0}.database.windows.net,1433;Database={1};' +
           'User ID={2}@{0};' +
           'Password={3};' +
           'Trusted_Connection=False;' +
           'Encrypt=True;' +
           'Connection Timeout=20;') `
           -f $DatabaseServerName, $DatabaseName, $UserName, $Password
}


<#
.SYNOPSIS
使用 Visual Studio 生成的 JSON 配置文件中的值创建 Microsoft Azure SQL 数据库。

.DESCRIPTION
Add-AzureSQLDatabases 函数从 JSON 文件的数据库部分中获取信息。Add-AzureSQLDatabases (复数)这个函数会对 JSON 文件中的每个 SQL 数据库调用 Add-AzureSQLDatabase (单数)函数。Add-AzureSQLDatabase (单数)会调用 New-AzureSqlDatabase cmdlet (Azure 模块)以创建 SQL 数据库。此函数不返回数据库对象，而是返回由用来创建数据库的值组成的哈希表。

.PARAMETER DatabaseConfig
接受一个由 PSCustomObjects 组成的数组作为参数，这些对象来自于当 JSON 文件有网站属性时 Read-ConfigFile 函数返回的 JSON 文件。该文件包含 environmentSettings.databases 属性。您可以通过管道将该列表传送给此函数。
PS C:\> $config = Read-ConfigFile <name>.json
PS C:\> $DatabaseConfig = $config.databases| where {$_.connectionStringName}
PS C:\> $DatabaseConfig
connectionStringName: Default Connection
databasename : TestDB1
edition   :
size     : 1
collation  : SQL_Latin1_General_CP1_CI_AS
servertype  : New SQL Database Server
servername  : r040tvt2gx
user     : dbuser
password   : Test.123
location   : West US

.PARAMETER  DatabaseServerPassword
为 SQL 数据库服务器管理员指定密码。输入一个包含 Name 和 Password 键的哈希表。Name 值为 SQL 数据库服务器的名称。Password 值为管理员密码。例如: @Name = "TestDB1"; Password = "password" 此参数为可选项。如果您省略此参数或者 SQL 数据库服务器名称与 $DatabaseConfig 对象的 serverName 属性的值不匹配，则此函数将在连接字符串中对 SQL 数据库使用 $DatabaseConfig 对象的 Password 属性。

.PARAMETER CreateDatabase
确认您是否要创建数据库。此参数是可选的。

.INPUTS
System.Collections.Hashtable[]

.OUTPUTS
System.Collections.Hashtable

.EXAMPLE
PS C:\> $config = Read-ConfigFile <name>.json
PS C:\> $DatabaseConfig = $config.databases| where {$_.connectionStringName}
PS C:\> $DatabaseConfig | Add-AzureSQLDatabases

Name                           Value
----                           -----
ConnectionString               Server=tcp:testdb1.database.windows.net,1433;Database=testdb;User ID=admin@testdb1;Password=password;Trusted_Connection=False;Encrypt=True;Connection Timeout=20;
Name                           Default Connection
Type                           SQLAzure

.LINK
Get-AzureSQLDatabaseConnectionString

.LINK
Create-AzureSQLDatabase
#>
function Add-AzureSQLDatabases
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true, ValueFromPipeline = $true)]
        [PSCustomObject]
        $DatabaseConfig,

        [Parameter(Mandatory = $false)]
        [AllowNull()]
        [Hashtable[]]
        $DatabaseServerPassword,

        [Parameter(Mandatory = $false)]
        [Switch]
        $CreateDatabase = $false
    )

    begin
    {
        Write-VerboseWithTime 'Add-AzureSQLDatabases: 开始'
    }
    process
    {
        Write-VerboseWithTime ('Add-AzureSQLDatabases: 正在创建 ' + $DatabaseConfig.databaseName)

        if ($CreateDatabase)
        {
            # 使用 DatabaseConfig 值创建一个新 SQL 数据库(除非已存在这样的数据库)
            # 已禁止显示命令输出。
            Add-AzureSQLDatabase -DatabaseConfig $DatabaseConfig | Out-Null
        }

        $serverPassword = $null
        if ($DatabaseServerPassword)
        {
            foreach ($credential in $DatabaseServerPassword)
            {
               if ($credential.Name -eq $DatabaseConfig.serverName)
               {
                   $serverPassword = $credential.password             
                   break
               }
            }               
        }

        if (!$serverPassword)
        {
            $serverPassword = $DatabaseConfig.password
        }

        return @{
            Name = $DatabaseConfig.connectionStringName;
            Type = 'SQLAzure';
            ConnectionString = Get-AzureSQLDatabaseConnectionString `
                -DatabaseServerName $DatabaseConfig.serverName `
                -DatabaseName $DatabaseConfig.databaseName `
                -UserName $DatabaseConfig.user `
                -Password $serverPassword }
    }
    end
    {
        Write-VerboseWithTime 'Add-AzureSQLDatabases: 结束'
    }
}


<#
.SYNOPSIS
创建一个新的 Microsoft Azure SQL 数据库。

.DESCRIPTION
Add-AzureSQLDatabase 函数使用 Visual Studio 生成的 JSON 配置文件中的数据创建一个 Microsoft Azure SQL 数据库并返回新数据库。如果订阅在指定 SQL 数据库服务器中已经有一个使用指定的数据库名称的 SQL 数据库，则此函数将返回现有数据库。此函数会调用实际创建 SQL 数据库的 New-AzureSqlDatabase cmdlet (Azure 模块)。

.PARAMETER DatabaseConfig
接受一个 PSCustomObject 作为参数，它来自于当 JSON 文件有网站属性时 Read-ConfigFile 函数返回的 JSON 配置文件。该文件包含 environmentSettings.databases 属性。您不能通过管道将该对象传送给此函数。Visual Studio 会为所有 Web 项目都生成一个 JSON 配置文件，并将该文件存储在您的解决方案的 PublishScripts 文件夹中。

.INPUTS
无。此函数不从管道中获取任何输入

.OUTPUTS
Microsoft.WindowsAzure.Commands.SqlDatabase.Services.Server.Database

.EXAMPLE
PS C:\> $config = Read-ConfigFile <name>.json
PS C:\> $DatabaseConfig = $config.databases | where connectionStringName
PS C:\> $DatabaseConfig

connectionStringName    : Default Connection
databasename : TestDB1
edition      :
size         : 1
collation    : SQL_Latin1_General_CP1_CI_AS
servertype   : New SQL Database Server
servername   : r040tvt2gx
user         : dbuser
password     : Test.123
location     : West US

PS C:\> Add-AzureSQLDatabase -DatabaseConfig $DatabaseConfig

.LINK
Add-AzureSQLDatabases

.LINK
New-AzureSQLDatabase
#>
function Add-AzureSQLDatabase
{
    [CmdletBinding()]
    param
    (
        [Parameter(Mandatory = $true)]
        [ValidateNotNull()]
        [Object]
        $DatabaseConfig
    )

    Write-VerboseWithTime 'Add-AzureSQLDatabase: 开始'

    # 如果参数值没有 serverName 属性，或者未填入 serverName 属性值，则失败。
    if (-not (Test-Member $DatabaseConfig 'serverName') -or -not $DatabaseConfig.serverName)
    {
        throw 'Add-AzureSQLDatabase: DatabaseConfig 值中缺少数据库 serverName (必需)。'
    }

    # 如果参数值没有 databasename 属性，或者 databasename 属性内未填入值，则失败。
    if (-not (Test-Member $DatabaseConfig 'databaseName') -or -not $DatabaseConfig.databaseName)
    {
        throw 'Add-AzureSQLDatabase: DatabaseConfig 值中缺少 databasename (必需)。'
    }

    $DbServer = $null

    if (Test-HttpsUrl $DatabaseConfig.serverName)
    {
        $absoluteDbServer = $DatabaseConfig.serverName -as [System.Uri]
        $subscription = Get-AzureSubscription -Current -ErrorAction SilentlyContinue

        if ($subscription -and $subscription.ServiceEndpoint -and $subscription.SubscriptionId)
        {
            $absoluteDbServerRegex = 'https:\/\/{0}\/{1}\/services\/sqlservers\/servers\/(.+)\.database\.windows\.net\/databases' -f `
                                     $subscription.serviceEndpoint.Host, $subscription.SubscriptionId

            if ($absoluteDbServer -match $absoluteDbServerRegex -and $Matches.Count -eq 2)
            {
                 $DbServer = $Matches[1]
            }
        }
    }

    if (!$DbServer)
    {
        $DbServer = $DatabaseConfig.serverName
    }

    $db = Get-AzureSqlDatabase -ServerName $DbServer -DatabaseName $DatabaseConfig.databaseName -ErrorAction SilentlyContinue

    if ($db)
    {
        Write-HostWithTime ('Create-AzureSQLDatabase: 正在使用现有数据库 ' + $db.Name)
        $db | Out-String | Write-VerboseWithTime
    }
    else
    {
        $param = New-Object -TypeName Hashtable
        $param.Add('serverName', $DbServer)
        $param.Add('databaseName', $DatabaseConfig.databaseName)

        if ((Test-Member $DatabaseConfig 'size') -and $DatabaseConfig.size)
        {
            $param.Add('MaxSizeGB', $DatabaseConfig.size)
        }
        else
        {
            $param.Add('MaxSizeGB', 1)
        }

        # 如果 $DatabaseConfig 对象有一个排序规则属性，并且该属性既不为 Null，也不为空
        if ((Test-Member $DatabaseConfig 'collation') -and $DatabaseConfig.collation)
        {
            $param.Add('Collation', $DatabaseConfig.collation)
        }

        # 如果 $DatabaseConfig 对象有一个版本属性，并且该属性既不为 Null，也不为空
        if ((Test-Member $DatabaseConfig 'edition') -and $DatabaseConfig.edition)
        {
            $param.Add('Edition', $DatabaseConfig.edition)
        }

        # 将哈希表写入到详细流中
        $param | Out-String | Write-VerboseWithTime
        # 通过展开调用 New-AzureSqlDatabase (禁止显示输出)
        $db = New-AzureSqlDatabase @param
    }

    Write-VerboseWithTime 'Add-AzureSQLDatabase: 结束'
    return $db
}
