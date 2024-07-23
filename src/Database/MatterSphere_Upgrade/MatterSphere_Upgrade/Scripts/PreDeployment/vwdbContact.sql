IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[config].[vwdbContact]'))
	DROP VIEW [config].[vwdbContact]
GO

CREATE VIEW [config].[vwdbContact]
AS
	WITH admins AS
	(SELECT * FROM  config.IsAdministratorTbl_NS() AS C )

	, ContactDeny( ContactID, [Deny], [Secure]) AS
	(
		SELECT RUGC.ContactID
			, MAX(CASE WHEN SUBSTRING( PC.DenyMask , 10 , 1 ) & 128 = 128 AND UGM.ID IS NOT NULL THEN 1 END) AS [Deny]
			, CASE WHEN MAX(CASE WHEN UGM.ID IS NOT NULL THEN 1 END) IS NULL THEN 1 END AS Secure
		FROM relationship.UserGroup_Contact RUGC
			JOIN config.ObjectPolicy PC ON PC.ID = RUGC.PolicyID
			LEFT JOIN config.GetUserAndGroupMembershipNT_NS() UGM ON UGM.ID = RUGC.UserGroupID
		WHERE(SELECT IsAdmin FROM admins) = 0
			AND (PC.IsRemote = 0 OR PC.IsRemote IS NULL)
		GROUP BY RUGC.ContactID
	)
	SELECT C.*
	FROM         
		config.dbContact AS C LEFT OUTER JOIN
		ContactDeny AS CA ON C.contID = CA.ContactID	--JOIN / APPLY to table valued function or ADO.NET dynamic SQL fails in matter centre
	WHERE     
		(CA.[Deny] IS NULL) AND (CA.Secure IS NULL)

GO
CREATE TRIGGER [config].[ContactDelete]
    ON [config].[vwdbContact]
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
    ON OBJECT::[config].[vwdbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbContact] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[config].[vwdbContact] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[config].[vwdbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[config].[vwdbContact] TO [OMSApplicationRole]
    AS [dbo];

GO

DECLARE @TABLE NVARCHAR(150) = 'dbContact'
	, @VIEW_SCHEMA NVARCHAR(150) = 'config'
	, @VIEW_NAME NVARCHAR(150) = 'vwdbContact'
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

IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[external].[vwdbContact]'))
BEGIN
DECLARE @VIEW nvarchar(max)
SET @VIEW = N'
ALTER VIEW [external].[vwdbContact]
AS
	SELECT DISTINCT Entity.*
	FROM
		config.dbContact AS Entity 
	LEFT JOIN
		[external].[ContactAccess] ( ) Access on Access.ContactID = Entity.contID 
	LEFT JOIN -- Include Client Contacts
		(SELECT DISTINCT CC.contID FROM dbo.dbClientContacts CC JOIN
		[external].[dbClient] C on C.clID = CC.clID ) ClientContacts on Entity.contID = ClientContacts.contID
	LEFT JOIN -- Include Associates
		(SELECT DISTINCT A.contID FROM config.dbAssociates A JOIN
		[external].[dbfile] F on F.fileID = A.fileID ) Associates on Entity.contID = Associates.contID
	LEFT JOIN -- Exclude Denies
		[external].ContactAccessDenies Denies on Denies.ID = Entity.contID
	CROSS APPLY -- Include Created by (if no security applied)
		(SELECT usrID FROM dbUser WHERE dbUser.usrADID = config.GetUserLogin()) U --Doing this here is more efficient than in the where clause
	LEFT JOIN
		(SELECT DISTINCT ContactID FROM [relationship].[UserGroup_Contact] RUGC JOIN [config].[ObjectPolicy] PC ON PC.ID = RUGC.[PolicyID] WHERE PC.IsRemote = 1 ) SecuredContacts on SecuredContacts.ContactID = Entity.ContID
	WHERE
	( 
		(Access.ContactID  is not null) or 
		(ClientContacts.contID is not null) or -- Include Client Contacts 
		(Associates.contID is not null) or -- Include Associates 
		(Entity.CreatedBy = U.usrID AND SecuredContacts.ContactID is null) -- Include Created by (if no security applied)
	) 
	AND 
	(
		(Denies.ID is null)-- Exclude Denies
	)
	 --AND Entity.SecurityOptions & 2 = 2
'
EXEC (@VIEW)
END
GO