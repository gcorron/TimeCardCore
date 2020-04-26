CREATE procedure [dbo].[sLookup] @group varchar(50)=null, @groupid int=0, @descr varchar(50)= null, @val varchar(50) = null
as
select id,descr,val,active from lookup
where
	(descr=@descr or @descr is null)
	and (val=@val or @val is null)
	and groupid=case when @group is null then @groupid else (select groupid from LookupGroup where descr=@group) end
	order by descr