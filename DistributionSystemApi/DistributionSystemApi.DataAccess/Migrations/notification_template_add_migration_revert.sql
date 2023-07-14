IF OBJECT_ID(N'dbo.NotificationTemplate',N'U') IS NOT NULL AND NOT EXISTS (SELECT TOP 1 1 FROM NotificationTemplate)
DROP TABLE dbo.NotificationTemplate
