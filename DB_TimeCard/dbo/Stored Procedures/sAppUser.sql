CREATE procedure [dbo].[sAppUser]
as
select userId, userName, userFullName,u.active,lastLogin, isnull(l.id,0) contractorId
from AppUser u join lookupgroup g on g.Descr='Contractor'
	left outer join lookup l on l.val=userName and l.groupId=g.GroupId