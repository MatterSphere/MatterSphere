--DELETE FROM FDDBINTEGRATIONMAPPING

DECLARE @SYSTEMID UNIQUEIDENTIFIER
DECLARE @ENTITYID UNIQUEIDENTIFIER
DECLARE @ERRORMESSAGE NVARCHAR ( 100 ) 

DECLARE @INTEGRATIONSYSTEMNAME NVARCHAR ( 50 )
DECLARE @INTEGRATIONENTITYNAME NVARCHAR ( 50 )

SET @INTEGRATIONSYSTEMNAME = 'Enterprise' 
SET @INTEGRATIONENTITYNAME = 'File'

IF ( SELECT COUNT ( * ) FROM FDDBINTEGRATIONSYSTEM WHERE NAME = @INTEGRATIONSYSTEMNAME ) = 0
BEGIN 
	SET @ERRORMESSAGE = 'Integration System not set up for ' + @INTEGRATIONSYSTEMNAME 
	RAISERROR ( @ERRORMESSAGE , 15 , 1 )
	RETURN
END 

IF ( SELECT COUNT ( * ) FROM FDDBINTEGRATIONENTITY WHERE NAME = @INTEGRATIONENTITYNAME ) = 0
BEGIN 
	SET @ERRORMESSAGE =  'Integration Entity not set up for ' + @INTEGRATIONENTITYNAME
	RAISERROR ( @ERRORMESSAGE , 15 , 1 )
	RETURN
END 

SELECT @SYSTEMID = ID FROM FDDBINTEGRATIONSYSTEM WHERE NAME = @INTEGRATIONSYSTEMNAME 
SELECT @ENTITYID = ID FROM FDDBINTEGRATIONENTITY WHERE NAME = @INTEGRATIONENTITYNAME

--CLIENT IMPORT
INSERT INTO	FDDBINTEGRATIONMAPPING
(
	SYSTEMID
	, ENTITYID
	, INTERNALID
	, EXTERNALID
)

SELECT 
	@SYSTEMID
	, @ENTITYID
	, FILEID
	, ENTERPRISE_MMATTER
FROM 
	DBFILE

LEFT JOIN 
(
	SELECT 
		FDDBINTEGRATIONMAPPING.*
	FROM 
		FDDBINTEGRATIONMAPPING
	INNER JOIN 
		FDDBINTEGRATIONSYSTEM ON FDDBINTEGRATIONSYSTEM.ID = FDDBINTEGRATIONMAPPING.SYSTEMID
	INNER JOIN 
		FDDBINTEGRATIONENTITY ON FDDBINTEGRATIONENTITY.ID = FDDBINTEGRATIONMAPPING.ENTITYID
	WHERE
		SYSTEMID = @SYSTEMID AND
		ENTITYID = @ENTITYID
) EXISTING ON EXISTING.INTERNALID = DBFILE.FILEID
WHERE
	EXISTING.ID IS NULL AND
	ENTERPRISE_MMATTER IS NOT NULL


--IF OK - DROP THE COLUMN
--ALTER TABLE DBFILE DROP COLUMN ENTERPRISE_MMATTER