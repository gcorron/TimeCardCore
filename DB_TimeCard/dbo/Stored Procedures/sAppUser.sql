CREATE procedure [dbo].[sAppUser]
as
select userId, userName, userFullName,active,lastLogin
from AppUser