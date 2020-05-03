CREATE procedure sLookupRolesForUser @userName varchar(50)
as
-- exec sLookupRolesForUser 'Greg Corron'
select lu2.descr from lookup lu1
	join AppUserRole r on lu1.id=r.userId and val=@userName
	join lookup lu2 on r.roleId=lu2.id