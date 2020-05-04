CREATE procedure [dbo].[dAppUserRole] @userId int
as
delete AppUserRole
where userId=@userId