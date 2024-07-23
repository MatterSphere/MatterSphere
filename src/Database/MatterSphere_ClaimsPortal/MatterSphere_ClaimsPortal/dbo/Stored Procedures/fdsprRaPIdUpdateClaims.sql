CREATE PROCEDURE [dbo].[fdsprRaPIdUpdateClaims] 
(
	@claimsListFromPortal dbo.tblClaimsPortalClaimsList readonly
)  
AS
	-- Update any existing claims in fdRaPIdClaim
	UPDATE fdRaPIdClaim
	set rpdClmActivityGuid = c.activityEngineGuid, 
		rpdClmAppRef = c.applicationReferences, 
		rpdClmAttachmentsCount = c.attachmentsCount,
		rpdClmCreationTime = c.creationTime, 
		rpdClmCurrentUserID = c.currentUserID, 
		rpdClmLockStatus = c.lockStatus,
		rpdClmLockUserID = c.lockUserId, 
		rpdClmPhaseCacheID = c.phaseCacheId, 
		rpdClmPhaseCacheName = c.phaseCacheName,
		rpdClmPrintDocCount = c.printableDocumentsCount, 
		rpdClmVerMajor = c.versionMajor, 
		rpdClmVerMinor = c.versionMinor
	FROM @claimsListFromPortal c
	INNER JOIN fdRaPIdClaim rc ON rc.rpdClmAppID = c.applicationId


	-- Insert any new claims
	INSERT INTO fdRaPIdClaim
	SELECT 
		c.applicationId as rpdClmAppID, 
		c.activityEngineGuid as rpdClmActivityGuid, 
		c.applicationReferences as rpdClmAppRef, 
		c.attachmentsCount as rpdClmAttachmentsCount,
		c.creationTime as rpdClmCreationTime,
		c.currentUserID as rpdClmCurrentUserID, 
		c.lockStatus as	rpdClmLockStatus,
		c.lockUserId as rpdClmLockUserID, 
		c.phaseCacheId as rpdClmPhaseCacheID, 
		c.phaseCacheName as rpdClmPhaseCacheName,
		c.printableDocumentsCount as rpdClmPrintDocCount, 
		c.versionMajor as rpdClmVerMajor, 
		c.versionMinor as rpdClmVerMinor,
		newId() as rowguid
	FROM @claimsListFromPortal c
	LEFT JOIN fdRaPIdClaim rc ON rc.rpdClmAppID = c.applicationId
	WHERE rc.rpdClmAppID IS NULL


	-- Synchronise with existing matters in fdRaPIdClaimantDetails
	UPDATE fdRaPIdClaimantDetails
	set rpdClmtEngineGUID = c.activityEngineGuid, 
		rpdClmtPhaseCacheID = c.phaseCacheId, 
		rpdClmtPhaseCacheName = c.phaseCacheName
	FROM @claimsListFromPortal c
	INNER JOIN fdRaPIdClaimantDetails cd ON cd.rpdClmtAppID = c.applicationId
	
	select 0, 0