use ContentDistributionSystem

begin tran

IF OBJECT_ID(N'dbo.NotificationTemplate',N'U') is not null and not exists (select top 1 1 from NotificationTemplate)
drop table dbo.NotificationTemplate

commit;