use ContentDistributionSystem

begin tran

IF OBJECT_ID(N'dbo.NotificationTemplate',N'U') is null
create table dbo.NotificationTemplate(
	ID varchar(200) primary key,
	Description int not null
	)

commit;