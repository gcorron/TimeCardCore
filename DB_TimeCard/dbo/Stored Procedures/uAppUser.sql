CREATE procedure uAppUser @userId int, @userName varchar(20), @userFullName varchar(50), @active bit, @reset bit
as
update AppUser
	set userName=@userName, userFullName=@userFullName, active=@active, passwordExpiry = case when @reset=1 then getdate() else passwordExpiry end
where userId=@userId