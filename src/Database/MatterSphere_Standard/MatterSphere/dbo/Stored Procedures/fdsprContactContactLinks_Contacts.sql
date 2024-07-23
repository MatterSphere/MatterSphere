
-- Author:		<Renato>
-- Create date: <11 Aug 2015>
-- Description:	<Retrieves the linked contacts for a Contact>
-- ==================================================
CREATE PROCEDURE [dbo].[fdsprContactContactLinks_Contacts]
(	
	@ASSOCID AS bigint
	,@UI uUICultureInfo 
)

AS

BEGIN

declare @contID bigint 
SET @ContID = (select contID from dbAssociates where assocID = @ASSOCID)
	
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
)

insert into @ContTable
(
    contId
    ,contName
	,contType
	,contAddress
	,contIsClient
)
  select
        c.contID as contid
        ,c.contName 
		,COALESCE(CL.cdDesc, '~' +  NULLIF(c.ContTypeCode, '') + '~') as contType
		,dbo.GetAddress(coalesce((select top 1 contaddid from dbcontactaddresses where contid = c.contid and contactive = 1 and contcode = 'PRINCIPLE'),contdefaultaddress),' ',null) as contAddress
		,c.contIsClient
  from
        dbclientcontacts cc
  inner join
        dbcontact c on c.contid = cc.contid
  LEFT JOIN
	    [dbo].[GetCodeLookupDescription] ( 'CONTTYPE', @UI ) CL ON CL.[cdCode] = c.ContTypeCode
  where 
        c.contisclient = 1 and c.contid = @contid
  and
		cc.clActive = 1
        
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
	, COALESCE(CL1.cdDesc, '~' +  NULLIF(LINK.contLinkCode, '') + '~') as [LinkDesc]
	,CONT.contIsClient
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


SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdsprContactContactLinks_Contacts] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fdsprContactContactLinks_Contacts] TO [OMSAdminRole]
    AS [dbo];

