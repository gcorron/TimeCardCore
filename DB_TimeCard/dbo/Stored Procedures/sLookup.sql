CREATE procedure [dbo].[sLookup] @group varchar(50), @descr varchar(50)= null, @val varchar(50) = null
as
select id,descr,val from lookup
where
	(descr=@descr or @descr is null)
	and (val=@val or @val is null)
	and groupid=(select groupid from LookupGroup where descr=@group)