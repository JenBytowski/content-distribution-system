IF OBJECT_ID(N'dbo.RecipientGroup', N'U') IS NULL

CREATE TABLE dbo.RecipientGroup(
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Title VARCHAR(200) NOT NULL,
    ) 
GO

IF OBJECT_ID(N'dbo.Recipient',N'U') IS NULL

CREATE TABLE dbo.Recipient(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	Title VARCHAR(200) NOT NULL,
	Email VARCHAR(200) NOT NULL,
	TelephoneNumber VARCHAR(200),
	GroupId UNIQUEIDENTIFIER
	)
GO

ALTER TABLE dbo.Recipient
ADD CONSTRAINT FK_Recipient_RecipientGroup
FOREIGN KEY (GroupId) REFERENCES dbo.RecipientGroup(Id);