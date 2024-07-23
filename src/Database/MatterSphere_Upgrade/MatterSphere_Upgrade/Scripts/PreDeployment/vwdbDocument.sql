IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[config].[vwdbDocument]'))
	DROP VIEW [config].[vwdbDocument]
GO

CREATE VIEW [config].[vwdbDocument]
AS

WITH [DocumentAllowDeny] ( [DocumentID], [Allow], [Deny], [Secure]) AS
(
	SELECT 
			RUGD.[DocumentID],
			MAX(CASE WHEN Substring ( PC.AllowMask , 9 , 1 ) & 128 = 128 AND UGM.ID IS NOT NULL THEN 1 ELSE NULL END) as [Allow] ,
			MAX(CASE WHEN Substring ( PC.DenyMask , 9 , 1 ) & 128 = 128 AND UGM.ID IS NOT NULL THEN 1 ELSE NULL END) as [Deny] ,
			CASE WHEN MAX(CASE WHEN UGM.ID IS NOT NULL THEN 1 END) IS NULL THEN 1 END AS Secure
	FROM
			[relationship].[UserGroup_Document] RUGD
	 CROSS APPLY config.IsAdministratorTbl_NS() admins
	JOIN
			[config].[ObjectPolicy] PC ON PC.ID = RUGD.[PolicyID]
	LEFT JOIN
			[config].[GetUserAndGroupMembershipNT_NS] () UGM ON RUGD.[UserGroupID] = UGM.[ID]
    WHERE admins.IsAdmin = 0 AND (PC.IsRemote = 0 or PC.IsRemote is Null)
	GROUP BY [DocumentID]
)

SELECT D.*
FROM config.dbDocument AS D 
	LEFT OUTER JOIN (
		SELECT [DocumentID]
			, CASE WHEN [Allow] IS NULL AND [Deny] IS NULL THEN 1 ELSE [Deny] END AS [Deny] 
			, [Secure]
		FROM [DocumentAllowDeny]
		) AS DA ON D.DocID = DA.DocumentID	--JOIN / APPLY to table valued function or ADO.NET dynamic SQL fails in matter centre
WHERE (DA.[Deny] IS NULL) AND (DA.Secure IS NULL) 

GO

CREATE TRIGGER [config].[DocumentDelete]
    ON [config].[vwdbDocument]
    WITH ENCRYPTION
    INSTEAD OF DELETE
    NOT FOR REPLICATION
    AS 
BEGIN
--The script body was encrypted and cannot be reproduced here.
    RETURN
END
GO

GRANT UPDATE
    ON OBJECT::[config].[vwdbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbDocument] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbDocument] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[vwdbDocument] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[vwdbDocument] TO [OMSApplicationRole]
    AS [dbo];

DECLARE @TABLE NVARCHAR(150) = 'dbDocument'
	, @VIEW_SCHEMA NVARCHAR(150) = 'config'
	, @VIEW_NAME NVARCHAR(150) = 'vwdbDocument'
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
