CREATE procedure dAppUser @userId int
as

if not exists(select * from work where contractorid=@userId)
begin
	delete AppUser
	where userId=@userId
end