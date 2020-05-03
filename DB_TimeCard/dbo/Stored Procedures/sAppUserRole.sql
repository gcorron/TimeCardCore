create procedure sAppUserRole @userId int
as
select descr
from AppUserRole r join lookup l on r.roleId=l.id
where r.userId=@userId