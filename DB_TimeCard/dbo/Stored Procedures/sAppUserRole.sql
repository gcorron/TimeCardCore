CREATE procedure [dbo].[sAppUserRole] @userId int
as
-- exec sAppUserRole 0
select id, l.descr,case when r.userId is null then 0 else 1 end active
from lookup l
	join lookupgroup g on l.groupId=g.GroupId and g.Descr='Role' and l.active=1
	left outer join AppUserRole r on l.id=r.roleId and r.userId=@userId