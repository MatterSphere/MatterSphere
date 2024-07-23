CREATE PROCEDURE [dbo].[sprOMSAssociateTypeTabList] 

AS
BEGIN
DECLARE @t1 as table (typeCode nvarchar(50), xmldata xml)

insert into @t1
select typeCode, cast(typeXML as xml) from dbAssociateType 


SELECT typeCode
	, COALESCE(CL1.cdDesc, '~' + NULLIF(res.typeCode, '') + '~') as [Associate Type]
	, COALESCE(CL2.cdDesc, '~' + NULLIF(res.flookup, '') + '~') as [Tab Description]
	, res.[Tab Source]
	, COALESCE(CL3.cdDesc, '~' + NULLIF(res.fGroup, '') + '~') as [Group]
FROM
(
	SELECT     
		typeCode,
		t.value('@lookup[1]','nvarchar(200)' ) as flookup,
		t.value('@source[1]','nvarchar(200)' ) as [Tab Source],
		t.value('@group[1]','nvarchar(200)' ) as fGroup
	FROM    
		@t1
		   cross Apply xmldata.nodes('/Config/Dialog/Tabs/Tab') as tabs(t)
	) res
	LEFT JOIN dbo.GetCodeLookupDescription ( 'SUBASSOC', '' ) CL1 ON CL1.[cdCode] =  res.typeCode
	LEFT JOIN dbo.GetCodeLookupDescription ( 'dlgtabcaption', '' ) CL2 ON CL2.[cdCode] =  res.flookup
	LEFT JOIN dbo.GetCodeLookupDescription ( 'DLGGROUPCAPTION', '' ) CL3 ON CL3.[cdCode] =  res.fGroup

END
