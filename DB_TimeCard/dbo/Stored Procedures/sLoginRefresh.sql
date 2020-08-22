

CREATE procedure [dbo].[sLoginRefresh] @userName varchar(50), @refreshToken nvarchar(max), @refreshTokenExpired datetime
as
-- exec  slogin 'a','b'
-- select * from appUser
declare @userId int
declare @contractorId int

declare @result varchar(20)
declare @lastlogin datetime=getdate()
declare @userfullname varchar(50)

select @result = 
	case when userId = null then 'NO'
	when refreshToken=@refreshToken and refreshTokenExpired>@refreshTokenExpired  and reset=0 then 'OK' else 'NO' end,
	@userId=userId,
	@userfullname = userFullName
from AppUser
where userName=@userName and active=1

if @userId is null
begin
	set @userId=0
	set @result='NO'
end
else
begin
	set @contractorId = isnull((select id from lookup
		where groupid=(select groupid from lookupGroup where descr='Contractor')
			and val=@userName),0)
end

select @result result, @userId userId, @userName userName, @lastlogin lastlogin, @userFullName userFullName, @contractorId contractorId