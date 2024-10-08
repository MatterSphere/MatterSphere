-- CM. 2012.07.27
-- LOOPS THROUGH ALL FILES IN THE OMS IMPORT TABLE AND TRANSFERS THEM INTO THE
-- MATTERSPHERE INTEGRATION MAPPING TABLE.

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udSprENTIntegrationMapperFiles]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[udSprENTIntegrationMapperFiles]
GO


CREATE PROCEDURE [dbo].[udSprENTIntegrationMapperFiles]
(@INTEGRATIONSYSTEMNAME nvarchar(50) = 'ENTERPRISE')
AS

DECLARE @internalID nvarchar(50)
DECLARE @externalID nvarchar(50)

DECLARE importFile_cursor cursor for
Select 
	F.fileID as internalID
	,MAPPEDCLIENT.EXTERNALID + '.' + F.FILENO  -- This needs to be checked and customised!
	 as externalID						
from 
	[OMSImport].[dbo].FileDetails FD
inner join
	[dbo].dbFile F ON F.fileNo = FD.fileNo
inner join
	DBO.fdGetExternalID ( 'ENTERPRISE' , 'CLIENT' ) MAPPEDCLIENT ON MAPPEDCLIENT.INTERNALID = CONVERT ( NVARCHAR ( 50 ) , F.CLID )
where
	F.clID = (Select internalID from fddbintegrationmapping where externalid = FD.clNo and EntityID = '9EC7680D-7E93-4627-9DE4-4CF056B34299')
	and FD.DCFlag = 1; -- Does a File import at all when there is no corresponding fee earner? (6)

open importFile_cursor;

fetch next from importFile_cursor into @internalid, @externalid;

while @@fetch_status = 0
begin

	exec dbo.fdSprInsertIntegrationMapping @INTEGRATIONSYSTEMNAME, 'FILE', @internalID, @externalID

	fetch next from importFile_cursor into @internalid, @externalid;
end
close importFile_cursor;
deallocate importFile_cursor;

GO


