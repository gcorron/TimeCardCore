create procedure dAppUserRole @userId int, @roleId int
as
delete AppUserRole
where userId=@userId and roleId = @roleId