CREATE TABLE [dbo].[Publisher]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Exchange] VARCHAR(100) NOT NULL, 
    [RouteingKey] VARCHAR(100) NOT NULL, 
    [UserName] VARCHAR(100) NOT NULL, 
    [Password] VARCHAR(100) NOT NULL, 
    [AutomaticRecoveryEnabled] BIT NOT NULL, 
    [HasReProcess] BIT NOT NULL, 
    [HostNames] VARCHAR(255) NOT NULL,
    [ConsumerId] INT NOT NULL,
    CONSTRAINT [FK_dbo_Publisher_dbo_Consumer] FOREIGN KEY (ConsumerId) REFERENCES Consumer(Id)
)
GO

EXECUTE sp_addextendedproperty
        @name = N'Description',
        @value = N'اطلاعات سیستم هایی که پیام به آنها ارسال میشود',
        @level0type = N'SCHEMA',
        @level0name = N'dbo',
        @level1type = N'TABLE',
        @level1name = N'Publisher';
GO