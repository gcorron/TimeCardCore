CREATE procedure [dbo].[uAppUserSeedAdmin] @userName varchar(20), @password varchar(20)
as

--uAppUserSeedAdmin 'admin','admin'
declare @passwordHash binary(64)=HASHBYTES('SHA2_512',@password)


insert AppUser (userName,userFullName,passwordHash, passwordExpiry,lastLogin,loginAttempts,active)
	values(@userName,@userName,@passwordHash,dateadd(year,1,getdate()),null,0,1)

declare @userId int = SCOPE_IDENTITY()
declare @roleId int
select @roleId=id from lookup l join lookupGroup g on l.groupId=g.GroupId
where g.Descr='Role' and l.descr='Admin'
insert appUserRole (userId,roleId) values(@userId,@roleId)