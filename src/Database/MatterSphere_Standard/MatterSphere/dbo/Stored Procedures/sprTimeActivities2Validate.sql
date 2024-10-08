﻿
CREATE PROC sprTimeActivities2Validate ( @FILEID BIGINT , @ACTCODE UCODELOOKUP , @VALIDATED BIT = 0 OUTPUT )
AS

--VALIDATE THE FILE - CUSTOM VERSIONS MUST ACCEPT @FILEID AND @ACTCODE AND RETURN @VALIDATED

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sprTimeActivities2Validate_CUSTOM]') AND type in (N'P', N'PC'))
BEGIN 
	PRINT 'ENTERING CUSTOM VERSION : sprTimeActivities2Validate_CUSTOM'
	EXEC sprTimeActivities2Validate_CUSTOM @FILEID , @ACTCODE , @VALIDATED OUTPUT
	RETURN
END

IF EXISTS ( SELECT FILEID FROM DBFILE WHERE FILEID = @FILEID )
BEGIN
	PRINT 'FILE EXISTS'
	--IF THE FILE TYPE HAS A CODE ASSIGNED THEN CHECK THE ACTIVITIT IS LINKED CORRECTLY
	DECLARE @CHECKCODE UCODELOOKUP
	SELECT @CHECKCODE = DBFILETYPE.FILETIMEACTIVITYGROUP FROM DBFILETYPE INNER JOIN DBFILE ON DBFILE.FILETYPE = DBFILETYPE.TYPECODE WHERE FILEID = @FILEID

	IF ISNULL ( @CHECKCODE , '' ) = ''
	BEGIN
		--FILES FILE TYPE IS NOT SET SO NOT VALIDATION
		PRINT 'FILES FILE TYPE DOES NOT HAVE A GROUP CODE SO NOT VALIDATION REQUIRED - CONTINUE'
		SET @VALIDATED = 1
	END
	ELSE
	BEGIN
		PRINT 'CHECKING FOR CODE ' + @CHECKCODE
		IF EXISTS ( SELECT * FROM DBACTIVITYTOFILETIMEACTIVITYGROUP WHERE ACTCODE = @ACTCODE AND FILETIMEACTIVITYGROUP = @CHECKCODE AND ACTIVE = 1 )
		BEGIN
			PRINT 'FILES FILETYPE GROUP SETTING HAS ACTIVE SETTING FOR ACTIVITY CODE'
			SET @VALIDATED = 1
		END
		ELSE
		BEGIN
			PRINT 'INVALID FILETYPE GROUP SETTING FOR THE FILE'
			SET @VALIDATED = 0
		END
	END
END
ELSE
BEGIN
		--FILEID DOES NOT EXIST
	PRINT 'FILE DOES NOT EXIST'
	SET @VALIDATED = 0
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTimeActivities2Validate] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprTimeActivities2Validate] TO [OMSAdminRole]
    AS [dbo];

