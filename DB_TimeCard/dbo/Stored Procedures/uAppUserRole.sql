create procedure uAppUserRole @userId int, @roleId int
as
if not exists(select * from AppUserRole where userId=@userId and roleId=@roleId)
	insert AppUserRole (userId, roleId) values(@userId, @roleId)