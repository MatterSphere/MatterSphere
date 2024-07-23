Print 'Starting AdvSecurity\UpdateUserGroup_Inheritance.sql'

/************* AdvSecurity\UpdateUserGroup_Inheritance ************************/
/************** Populate Inheritance Column ***********************************/
update c set block_inheritance = 1
 from relationship.usergroup_client c
join
(select count(*) a ,clid,usergroupid from relationship.UserGroup_File group by clid,UserGroupID) a
on c.ClientID = a.clid
and c.UserGroupID = a.UserGroupID
join
(select count(f.fileid) a,f.clid from config.dbFile f
 join (select distinct clid from relationship.UserGroup_File) ugf on ugf.clID =f.clid group by f.clid) b
 on a.clid = b.clID
 where a.a <> b.a;
GO

/************** Populate Inheritance Column ***********************************/
update a set inherited = 'C' from relationship.UserGroup_Document a
join relationship.UserGroup_Client b
on a.clid = b.ClientID
and a.PolicyID = b.PolicyID
and a.UserGroupID = b.UserGroupID
where a.inherited is null and b.block_inheritance is null;
GO

update a set inherited = 'F' from relationship.UserGroup_Document a
join relationship.UserGroup_File b
on a.clid = b.FileID
and a.PolicyID = b.PolicyID
and a.UserGroupID = b.UserGroupID
where a.inherited is null;
GO

/************** Populate Inheritance Column ***********************************/
update a set inherited = 'C' from relationship.UserGroup_File a
join relationship.UserGroup_Client b
on a.clid = b.ClientID
and a.PolicyID = b.PolicyID
and a.UserGroupID = b.UserGroupID
where a.inherited is null and b.block_inheritance is null;
GO

/************** Populate Inheritance Column ***********************************/
update a set inherited = 'C' from relationship.UserGroup_Contact a
join relationship.UserGroup_Client b
on a.clid = b.ClientID
and a.PolicyID = b.PolicyID
and a.UserGroupID = b.UserGroupID
where a.inherited is null;
GO

