IF OBJECT_ID('config.AssociateDelete', 'TR') IS NOT NULL
DROP TRIGGER config.AssociateDelete
GO
CREATE TRIGGER config.AssociateDelete ON config.vwdbAssociates
INSTEAD OF DELETE NOT FOR REPLICATION
AS
BEGIN
DELETE DP
	FROM config.dbAssociates DP
	JOIN DELETED D ON D.assocID = DP.assocID
END
GO

IF OBJECT_ID('config.ClientDelete', 'TR') IS NOT NULL
DROP TRIGGER config.ClientDelete
GO
CREATE TRIGGER config.ClientDelete ON config.vwdbClient
INSTEAD OF DELETE NOT FOR REPLICATION
AS
BEGIN
DELETE DP
	FROM config.dbClient DP
	JOIN DELETED D ON D.clID = DP.clID
END
GO

IF OBJECT_ID('config.ContactDelete', 'TR') IS NOT NULL
DROP TRIGGER config.ContactDelete
GO
CREATE TRIGGER config.ContactDelete ON config.vwdbContact
INSTEAD OF DELETE NOT FOR REPLICATION
AS
BEGIN
DELETE DP
	FROM config.dbContact DP
	JOIN DELETED D ON D.contID = DP.contID
END
GO

IF OBJECT_ID('config.DocumentDelete', 'TR') IS NOT NULL
DROP TRIGGER config.DocumentDelete
GO
CREATE TRIGGER config.DocumentDelete ON config.vwdbDocument
INSTEAD OF DELETE NOT FOR REPLICATION
AS
BEGIN
DELETE DP
	FROM config.dbDocument DP
	JOIN DELETED D ON D.docID = DP.docID
END
GO

IF OBJECT_ID('config.DocumentPreviewDelete', 'TR') IS NOT NULL
DROP TRIGGER config.DocumentPreviewDelete
GO
CREATE TRIGGER config.DocumentPreviewDelete ON config.vwdbDocumentPreview
INSTEAD OF DELETE NOT FOR REPLICATION
AS
BEGIN
DELETE DP
	FROM config.dbDocumentPreview DP
	JOIN DELETED D ON D.docID = DP.docID
END
GO

IF OBJECT_ID('config.MatterDelete', 'TR') IS NOT NULL
DROP TRIGGER config.MatterDelete
GO
CREATE TRIGGER config.MatterDelete ON config.vwdbFile
INSTEAD OF DELETE NOT FOR REPLICATION
AS
BEGIN
DELETE DP
	FROM config.dbFile DP
	JOIN DELETED D ON D.fileID = DP.fileID
END
GO

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'config.GetUsersSecurityContext') AND type in (N'P', N'PC'))
	DROP PROCEDURE [config].[GetUsersSecurityContext]
GO

CREATE PROCEDURE [config].[GetUsersSecurityContext] 
	@ntLogin nvarchar(200)
AS 
SET NOCOUNT ON
SELECT 
	MTP.[SecurableType] ,
	MTP.[PermissionCode] as [Permission] ,
	NULL as ObjectID
FROM 
	[config].[SystemPolicy] SP 
JOIN
	[config].[GetUserAndGroupMembershipSupport] ( @ntLogin ) GM ON GM.[PolicyID] = SP.[ID] CROSS APPLY [config].[SystemMaskToPermissions] ( SP.[AllowMask] , SP.[DenyMask] ) MTP
GROUP BY 
	MTP.[PermissionCode] ,  MTP.[SecurableType] 
HAVING
	Sum([Allow]) = 0
GO