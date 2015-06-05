企业支付回执
==================


基本配置
-----------
**接收邮箱：** guangzhoukjzf@customs.gov.cn

**部署服务器：** 10.53.1.186

**部署路径：** ```E:\EntReg\Receipt\Receipt.GetMail\EnterpriseRegistratoin.Console.cmd```

**数据库：** ```Server=10.53.1.186;Database=MessageStore;User ID=******;Password=*******```

**执行方式：** 使用系统的计划任务，周期为每 30min 执行一次

**执行环境：** DNX451


功能说明
-----------
计划任务会定时接收邮件，接收后会过滤，只接受附件含有 ```xls[x]``` 的邮件，并存储到数据库。

存储的信息包括 邮件的 *发件人名称*、*发件人地址*、*发件时间*、*主题*、*内容*，附件的*名称*、*类型*、*大小*、*内容*。附件内容保存到数据库的 FileTable。

可以以文件共享方式通过以下路径访问：
```\\10.53.1.186\mssqlserver\MessageStore\AttachmentFile\ ```


或通过数据库访问相应的 FileTable.

安装说明
------------
本项目采用 ASP.NET 5 框架开发，使用的是 beta 4 的版本。运行环境为 DNX451。
运行环境的安装参考 [ASP.NET 开源项目库](https://github.com/aspnet/Home/)。

**Dependencies:**
EntityFramework: 7.0.0
OpenPop.NET: 2.3.0

**devDependencies:**
Xunit
FluentAssertion


详细配置信息
----------------





### Console
配置内容，见 EnterpriseRegistration.Console\config.json


### Web



### Database
使用安装于10.53.1.186上的SQL Server 2014 with SP1，利用了 [FileTable](https://msdn.microsoft.com/en-us/ff929144.aspx) 功能。 FileTable 是 SQL Server 2012 提供的新功能，基于 2008 引入的 [FILESTREAM](https://msdn.microsoft.com/en-us/gg471497) 技术开发。

FileTable 提供 Windows 文件访问 API 的支持，可以用文件浏览器、文件共享等方式直接访问数据库里面的文件。

数据库生成利用了 Microsoft EntityFramework 的 CodeFirst 模式生成，FileTable则自己创建。

##### Message Table

| ColumnName | Type | Note |
|------------|------|------|
|MessageId   | Guid | 邮件ID|
| FromName   | String | 发件人名称 |
| FromAddress|String | 发件人地址 |
| Subject | String | 主题 |
|Body | String | 邮件内容，收取邮件的HTML形式的内容，没有的话收取Text形式 |

##### Attachment Table

| ColumnName | Type | Note |
|------------|-----|-----|
| AttachmentId | Guid | 附件ID |
| HashName | String | 附件的实际存储名称 |
| OriginalName | String | 附件的原本文件名 |
| MIMEType | String | MIME类型字段 |
| Size | int | 附件的大小 |
| MessageId | Guid | 对应的邮件ID |
| stream_id | Guid |对应文件内容的ID|

##### AttachmentFile Table

| ColumnName | Type | Note |
|----------|----|-----|
| stream_id | Guid | 文件内容ID |
| file_stream | byte[] | 文件内容 |
| name | String | 文件名 |
| ... | ... | ... |


