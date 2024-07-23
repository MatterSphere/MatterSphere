IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[config].[vwdbFile]'))
	DROP VIEW [config].[vwdbFile]
GO



CREATE VIEW [config].[vwdbFile]
AS
WITH FileDeny( FileID, [Deny], [Secure]) AS
(
	SELECT RUGF.FileID
		, MAX(CASE WHEN SUBSTRING ( PC.DenyMask , 6 , 1 ) & 32 = 32 AND UGM.ID IS NOT NULL THEN 1 END) AS [Deny]
		, CASE WHEN MAX(CASE WHEN UGM.ID IS NOT NULL THEN 1 END) IS NULL THEN 1 END AS Secure
	FROM relationship.UserGroup_File RUGF
		JOIN config.ObjectPolicy PC ON PC.ID = RUGF.PolicyID
		LEFT JOIN config.GetUserAndGroupMembershipNT_NS() UGM ON UGM.ID = RUGF.UserGroupID
		CROSS APPLY config.IsAdministratorTbl_NS() admins
	WHERE admins.IsAdmin = 0
		AND (PC.IsRemote = 0 OR PC.IsRemote IS NULL)
	GROUP BY RUGF.FileID
)
SELECT F.*
FROM config.dbFile AS F 
	LEFT JOIN FileDeny AS FA ON F.fileID = FA.FileID	--JOIN / APPLY to table valued function or ADO.NET dynamic SQL fails in matter centre
WHERE (FA.[Deny] IS NULL) AND (FA.Secure IS NULL) 

GO

CREATE TRIGGER [config].[MatterDelete]
    ON [config].[vwdbFile]
    WITH ENCRYPTION
    INSTEAD OF DELETE
    NOT FOR REPLICATION
    AS 
BEGIN
--The script body was encrypted and cannot be reproduced here.
    RETURN
END
GO

GO
GRANT UPDATE
    ON OBJECT::[config].[vwdbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbFile] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbFile] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[vwdbFile] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[vwdbFile] TO [OMSApplicationRole]
    AS [dbo];


GO

DECLARE @TABLE NVARCHAR(150) = 'dbFile'
	, @VIEW_SCHEMA NVARCHAR(150) = 'config'
	, @VIEW_NAME NVARCHAR(150) = 'vwdbFile'
	, @VIEWSELECT1 nvarchar(max)
	, @VIEWFROM nvarchar(max) = ' (isnull(TBL.ISHIDDEN,0)=0 OR (config.IsPIAdministrator_NS()) = 1)'
	, @VIEW nvarchar(max)
	, @DEF NVARCHAR(MAX)
	, @TID NVARCHAR(10)
	, @WORKDEF NVARCHAR(MAX)
	, @POS1 INT
	, @POS2 INT
IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME= @TABLE AND COLUMN_NAME = 'ISHIDDEN')
BEGIN

	SELECT @DEF=REPLACE( REPLACE( REPLACE( REPLACE(VIEW_DEFINITION, CHAR(10), ' '), CHAR(13), ' '), CHAR(9), ' '),'--JOIN / APPLY to table valued function or ADO.NET dynamic SQL fails in matter centre ','') FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA= @VIEW_SCHEMA AND TABLE_NAME = @VIEW_NAME
	SET @WORKDEF=RIGHT(@DEF,LEN(@DEF)-CHARINDEX(' FROM ',@Def))

		SET @POS1 = CHARINDEX(' AS ', @WORKDEF) +4
		SET @POS2 = CHARINDEX(' ', @WORKDEF, @POS1)
		SET @TID = SUBSTRING(@WORKDEF, @POS1, @POS2 - @POS1)
					
		IF @DEF NOT LIKE '% '+REPLACE(@VIEWFROM, '(TBL.', '('+ @TID + '.') + '%'
		BEGIN
			SET @VIEW=REPLACE(@DEF, 'CREATE ', 'ALTER ')
			SET @VIEW = @VIEW+' AND '+ REPLACE(@VIEWFROM, '(TBL.', '(' + @TID + '.')
		END
	/************************** ALTER VIEW ********************************/
	EXEC (@VIEW)
	PRINT (@VIEW)
END

GO 

