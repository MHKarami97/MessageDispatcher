CREATE TABLE [dbo].[Consumer]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Exchange] VARCHAR(100) NOT NULL, 
    [ExchangeType] VARCHAR(100) NOT NULL, 
    [ExchangeAutoDelete] BIT NOT NULL, 
    [Durable] BIT NOT NULL, 
    [RouteingKey] VARCHAR(100) NOT NULL, 
    [UserName] VARCHAR(100) NOT NULL, 
    [Password] VARCHAR(100) NOT NULL, 
    [Queue] VARCHAR(100) NOT NULL, 
    [RequeueMessageRetryCount] INT NOT NULL, 
    [AutomaticRecoveryEnabled] BIT NOT NULL,
    [HostNames] VARCHAR(255) NOT NULL
)
GO

EXECUTE sp_addextendedproperty
        @name = N'Description',
        @value = N'اطلاعات ارسال کنندگان به این سیستم',
        @level0type = N'SCHEMA',
        @level0name = N'dbo',
        @level1type = N'TABLE',
        @level1name = N'Consumer';
GO