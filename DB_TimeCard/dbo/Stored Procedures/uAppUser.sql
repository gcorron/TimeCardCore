CREATE procedure [dbo].[uAppUser] @userId int, @userName varchar(20), @userFullName varchar(50), @active bit, @reset bit
as

declare @passwordHash binary(64)
if @reset=1 or @userId=0
begin
	set @passwordHash=HASHBYTES('SHA2_512',(
		select val from lookup l join lookupgroup g on l.groupId=g.GroupId where g.Descr='System' and l.descr='Initial Password'
		))
end

if @userId=0
begin
	insert AppUser (userName,userFullName, passwordHash, active, loginAttempts, reset)
	values (@userName, @userFullName, @passwordHash, @active,0,1)
	set @userId=SCOPE_IDENTITY()
end
else
begin
	update AppUser
		set userName=@userName,
		 userFullName=@userFullName,
		 active=@active,
		 passwordHash = case @reset when 1 then @passwordHash else passwordHash end,
		 reset=@reset
	where userId=@userId
end
select @userid