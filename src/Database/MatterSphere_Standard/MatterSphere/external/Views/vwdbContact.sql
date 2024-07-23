
CREATE VIEW [external].[vwdbContact]

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

GO
GRANT UPDATE
    ON OBJECT::[external].[vwdbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbContact] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[external].[vwdbContact] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[external].[vwdbContact] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[external].[vwdbContact] TO [OMSApplicationRole]
    AS [dbo];

