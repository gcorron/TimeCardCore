CREATE PROCEDURE [dbo].[dWork] @workId int
AS
	delete work
	where workId=@workId