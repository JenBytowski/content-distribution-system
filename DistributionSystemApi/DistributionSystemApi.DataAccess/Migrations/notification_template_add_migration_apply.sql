IF OBJECT_ID(N'dbo.NotificationTemplate',N'U') IS NULL

CREATE TABLE dbo.NotificationTemplate(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	Description VARCHAR(200) NOT NULL
	)