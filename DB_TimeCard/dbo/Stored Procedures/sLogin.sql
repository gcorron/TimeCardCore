﻿

CREATE procedure [dbo].[sLogin] @userName varchar(50), @password varchar(20)
as
-- exec  slogin 'a','b'
-- select * from appUser
declare @userId int
declare @contractorId int
declare @passwordHash binary(64)=HASHBYTES('SHA2_512',@password)

declare @result varchar(20)
declare @lastlogin datetime=getdate()
declare @userfullname varchar(50)

select @result = case when loginAttempts > 10 then 'LOCKOUT' else
	case when userId = null then 'NO'
	when passwordHash=@passwordHash or @password = null then
		'OK'
		--case when passwordExpiry > getdate() then 'OK' else 'EXPIRED' end
	else 'NO' end
	end,
	@userId=userId,
	@userfullname = userFullName
from AppUser
where userName=@userName and active=1

if @userId is null
begin
	set @userId=0
	set @result='NO'
end

if @password is not null
begin
	if @result = 'NO'
	begin
		update appUser set loginAttempts=loginAttempts + 1
		where userId=@userId
	end
	if @result='OK'
	begin
		update appUser set lastLogin=@lastlogin, loginAttempts = 0
		where userId=@userId 
		set @contractorId = isnull((select id from lookup
			where groupid=(select groupid from lookupGroup where descr='Contractor')
				and val=@userName),0)
	end
end

select @result result, @userId userId, @lastlogin lastlogin, @userFullName userFullName, @contractorId contractorId