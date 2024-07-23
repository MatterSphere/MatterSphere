

CREATE PROCEDURE [dbo].[fdsprContactLinks_Contacts]
(	
	@CLID AS bigint
	,@UI uUICultureInfo 
)

AS

BEGIN
-- New
	declare @contid bigint
	set @contID = (select clDefaultContact from dbClient where clID = @clid)
	
declare @ContTable as Table
(
    contId bigint
    ,contName nvarchar(128)
	,contType nvarchar(50)
	,contAddress nvarchar(400)
	--New. Extra fields
	,links int
	,linkdesc nvarchar(200)
	,contIsClient bit
	,clientContact bit
)

insert into @ContTable
(
    contId
    ,contName
	,contType
	,contAddress
	,links
	,contIsClient
	,clientContact
)
-- get the client contact
  select
        c.contID as contid
        ,c.contName 
		,COALESCE(CL.cdDesc, '~' +  NULLIF(c.ContTypeCode, '') + '~') as contType
		,dbo.GetAddress(coalesce((select top 1 contaddid from dbcontactaddresses where contid = c.contid and contactive = 1 and contcode = 'PRINCIPLE'),contdefaultaddress), ' ',null) as contAddress
		,null as links
		,c.contIsClient
		,1
  from
        dbclientcontacts cc
  inner join
        dbcontact c on c.contid = cc.contid
  LEFT JOIN
	    [dbo].[GetCodeLookupDescription] ( 'CONTTYPE', @UI ) CL ON CL.[cdCode] = c.ContTypeCode
  where 
        c.contisclient = 1 and c.contid = @contid
        
insert into @ContTable        
(
    contId
    ,contName
	,contType
	,contAddress
	,links
	,contIsClient
	, clientContact
)
   select
        c.contID as contid
        ,c.contName 
		,COALESCE(CL.cdDesc, '~' +  NULLIF(c.ContTypeCode, '') + '~') as contType
		,dbo.GetAddress(coalesce((select top 1 contaddid from dbcontactaddresses where contid = c.contid and contactive = 1 and contcode = 'PRINCIPLE'),contdefaultaddress),' ',null) as contAddress
		,(select count(ContLinkID) as NumLinks from dbcontactlinks where contid = c.contid and contlinkid != CONTID) as [Links]
		,contIsClient
		,1
  from
        dbclientcontacts cc
  inner join
        dbcontact c on c.contid = cc.contid
  LEFT JOIN
	    [dbo].[GetCodeLookupDescription] ( 'CONTTYPE', @UI ) CL ON CL.[cdCode] = c.ContTypeCode
  where 
        cc.clid = @clid 
		and c.contid != @contid
		and cc.clActive = 1

       
SELECT
      *
FROM
      @ContTable
UNION ALL
SELECT 
	CONT.CONTID
    ,CONT.contName 
	,COALESCE(CL.cdDesc, '~' +  NULLIF(CONT.ContTypeCode, '') + '~') as contType
	,dbo.GetAddress(coalesce((select top 1 contaddid from dbcontactaddresses where contid = CONT.contid and contactive = 1 and contcode = 'PRINCIPLE'),contdefaultaddress),' ',null) as contAddress
	-- New. Extra fields to display
	,(select count(ContLinkID) as NumLinks from dbcontactlinks where contid = cont.contid and contlinkid != CONTID) as [Links]
	, COALESCE(CL1.cdDesc, '~' +  LINK.contLinkCode + '~') as [LinkDesc]
	,CONT.contIsClient
	,0
FROM
      dbContact CONT
INNER JOIN 
      dbContactLinks LINK ON CONT.contID = LINK.contLINKID
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'CONTTYPE', @UI ) CL ON CL.[cdCode] = CONT.ContTypeCode
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( 'CONTLINK', @UI ) CL1 ON CL1.[cdCode] = LINK.contLinkCode
WHERE
      LINK.contID = @contid
AND   
      LINK.contID != LINK.contLinkID 
AND 
      NOT EXISTS (select r.contid from @ContTable r where r.contid = cont.contid)
	  
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdsprContactLinks_Contacts] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdsprContactLinks_Contacts] TO [OMSAdminRole]
    AS [dbo];

