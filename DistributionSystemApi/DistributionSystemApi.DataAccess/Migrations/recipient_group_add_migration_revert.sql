IF OBJECT_ID(N'dbo.RecipientGroup',N'U') IS NOT NULL AND NOT EXISTS (SELECT TOP 1 1 FROM RecipientGroup)
DROP TABLE dbo.RecipientGroup
GO

IF OBJECT_ID(N'dbo.Recipient',N'U') IS NOT NULL AND NOT EXISTS (SELECT TOP 1 1 FROM Recipient)
DROP TABLE dbo.Recipient

GO
IF OBJECT_ID(N'dbo.RecipientRecipientGroup',N'U') IS NOT NULL AND NOT EXISTS (SELECT TOP 1 1 FROM RecipientRecipientGroup)
DROP TABLE dbo.RecipientRecipientGroup