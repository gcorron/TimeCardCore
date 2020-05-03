CREATE procedure uLogin @userName varchar(20), @password varchar(50)
as
declare @passwordHash nvarchar(max)=HASHBYTES('SHA2_512',@password)

update AppUser
	set passwordHash=@passwordHash,loginAttempts=0, passwordExpiry=dateadd(year,1,getdate())
where userName=@userName and passwordExpiry < getdate()