create procedure iAppUserRole @userId int, @roleId int
as
insert AppUserRole (userId, roleId) values(@userid, @roleId)