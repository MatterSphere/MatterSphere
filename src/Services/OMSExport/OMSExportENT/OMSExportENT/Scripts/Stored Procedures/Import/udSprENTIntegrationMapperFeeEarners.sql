-- CM. 2012.07.27
-- LOOPS THROUGH NEW FEE EARNERS (WHERE EXTID = -900) AND TRANSFERS THEM INTO THE
-- MATTERSPHERE INTEGRATION MAPPING TABLE.

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udSprENTIntegrationMapperFeeEarners]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[udSprENTIntegrationMapperFeeEarners]
GO


CREATE PROCEDURE [dbo].[udSprENTIntegrationMapperFeeEarners]
(@INTEGRATIONSYSTEMNAME nvarchar(50) = 'ENTERPRISE')
AS

DECLARE @internalID nvarchar(50)
DECLARE @externalID nvarchar(50)

DECLARE importFeeEarner_cursor cursor for
Select 
	U.usrID as internalID
	,U.usrInits as externalID 
from 
	[MCEnterprise].[dbo].dbUser U
where 
	U.usrextid = -900	-- flag: waiting to be mapped
	and U.CreatedBy = -101 -- Import User


open importFeeEarner_cursor;

fetch next from importFeeEarner_cursor into @internalid, @externalid;

while @@fetch_status = 0
begin

	-- 1. INSERT INTO MAPPING TABLE
	exec dbo.fdSprInsertIntegrationMapping @INTEGRATIONSYSTEMNAME, 'FEE EARNER', @internalID, @externalID

	-- 2. UPDATE EXTERNAL USER ID (WITH FLAG)
	update 
		dbUser 
	set 
		usrExtID = -1000			-- flag: mapped in integration table
	where 
		usrID = @internalID;		-- mark as mapped into mapping table
	
	fetch next from importFeeEarner_cursor into @internalid, @externalid;
end
close importFeeEarner_cursor;

deallocate importFeeEarner_cursor;

GO

