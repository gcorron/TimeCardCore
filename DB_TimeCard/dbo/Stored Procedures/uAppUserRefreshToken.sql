create procedure uAppUserRefreshToken @userName varchar(20), @refreshToken nvarchar(max), @refreshTokenExpired datetime
AS
update AppUser set refreshToken = @refreshToken, refreshTokenExpired = @refreshTokenExpired
where userName = @userName