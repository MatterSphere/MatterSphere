-- CM. 2012.07.27
-- LOOPS THROUGH ALL CLIENTS IN THE OMS IMPORT TABLE AND TRANSFERS THEM INTO THE
-- MATTERSPHERE INTEGRATION MAPPING TABLE.


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udSprENTIntegrationMapperClients]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[udSprENTIntegrationMapperClients]
GO



CREATE PROCEDURE [dbo].[udSprENTIntegrationMapperClients]
(@INTEGRATIONSYSTEMNAME nvarchar(50) = 'ENTERPRISE')
AS

DECLARE @internalID nvarchar(50)
DECLARE @externalID nvarchar(50)

DECLARE importClient_cursor cursor for
Select 
	CL.clid as internalID
	,CD.clno as externalID 
from 
	[OMSImport].[dbo].ClientDetails CD
inner join
	[dbo].dbClient CL ON CL.clNo = CD.clNo
where 
	CD.DCFlag = 1;

open importClient_cursor;

fetch next from importClient_cursor into @internalid, @externalid;

while @@fetch_status = 0
begin

	exec dbo.fdSprInsertIntegrationMapping @INTEGRATIONSYSTEMNAME, 'CLIENT', @internalID, @externalID

	fetch next from importClient_cursor into @internalid, @externalid;
end
close importClient_cursor;
deallocate importClient_cursor;

GO


