CREATE PROCEDURE [dbo].[sprWorkRecord] (@USRID bigint,  @UI uUICultureInfo = '{default}', @TABLE int = 0)  
AS
	IF(@TABLE = 0 or @TABLE = 1)
	BEGIN
		--Client List	
		SELECT  DISTINCT C.clno, C.clname
		FROM	dbclient C inner join dbFile F on C.clid = F.clid 
		WHERE	F.fileprincipleID = @USRID AND F.FileStatus LIKE 'LIVE%'	
	END	
	
	IF(@TABLE = 0 or @TABLE = 2)
	BEGIN
		-- File List
		SELECT  F.fileNo,F.fileDesc as fileJointDesc,C.clno 
		FROM	dbo.dbFile F inner join dbclient C on C.clid = F.clid 
		WHERE	F.fileprincipleID = @USRID AND F.FileStatus LIKE 'LIVE%'
	END
	
	IF(@TABLE = 0 or @TABLE = 3)
	BEGIN
		-- Time Activities
		SELECT	actCode, COALESCE(CL1.cdDesc, '~' + NULLIF(actCode, '') + '~') AS actCodeDesc
		--, actAccCode
		FROM	dbo.dbActivities
			LEFT JOIN dbo.GetCodeLookupDescription ( 'TIMEACTCODE', @UI ) CL1 ON CL1.[cdCode] =  actCode
		ORDER BY actCodeDesc
	END
