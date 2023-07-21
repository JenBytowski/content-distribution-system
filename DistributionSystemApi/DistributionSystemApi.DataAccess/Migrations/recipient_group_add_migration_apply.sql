IF OBJECT_ID(N'dbo.RecipientGroup', N'U') IS NULL

CREATE TABLE dbo.RecipientGroup(
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Title VARCHAR(100) NOT NULL,
    ) 
GO

IF OBJECT_ID(N'dbo.Recipient',N'U') IS NULL

CREATE TABLE dbo.Recipient(
	Id UNIQUEIDENTIFIER PRIMARY KEY,
	Title VARCHAR(100) NOT NULL,
	Email VARCHAR(256) NOT NULL,
	TelephoneNumber VARCHAR(15)
	)
GO

IF OBJECT_ID(N'dbo.RecipientRecipientGroup',N'U') IS NULL

CREATE TABLE dbo.RecipientRecipientGroup (
	Id UNIQUEIDENTIFIER,
    RecipientId UNIQUEIDENTIFIER,
    GroupId UNIQUEIDENTIFIER,
    PRIMARY KEY (RecipientId, GroupId),
    FOREIGN KEY (RecipientId) REFERENCES dbo.Recipient(Id),
    FOREIGN KEY (GroupId) REFERENCES dbo.RecipientGroup(Id)
)
GO


CREATE UNIQUE INDEX idx_unique_telephone
ON dbo.Recipient (TelephoneNumber)
WHERE TelephoneNumber IS NOT NULL;

ALTER TABLE dbo.Recipient
ADD CONSTRAINT UQ_Email UNIQUE (Email)

ALTER TABLE dbo.RecipientGroup
ADD CONSTRAINT UQ_Title UNIQUE (Title)