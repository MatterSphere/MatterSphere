IF EXISTS (SELECT * FROM sys.procedures WHERE object_id = OBJECT_ID(N'[dbo].[schDocuSignRecipients]'))
	DROP PROCEDURE [dbo].[schDocuSignRecipients] 
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[schDocuSignRecipients] 
(
	@fileID bigint
	, @usrID int
	, @UI uUICultureInfo = '{default}'
)
AS
SET NOCOUNT ON;
SET TRAN ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @edition nvarchar(15)
DECLARE @fileResponsibleID int, @filePrincipleID int, @fileManagerID int

SELECT TOP 1 @edition = ISNULL(regEdition, 'EP') FROM dbRegInfo

SELECT @fileResponsibleID = fileResponsibleID, @filePrincipleID = filePrincipleID, @fileManagerID = fileManagerID
FROM dbo.dbFile
WHERE fileID = @fileID

SELECT CAST(ROW_NUMBER() OVER (ORDER BY Name) AS int) AS [Order], R.* FROM (
	SELECT
		contName AS [Name]
		, assocEmail AS [Email]
		, COALESCE(CL.cdDesc, '~' + NULLIF(assocType, '') + '~') AS [Type]
		, NULLIF(contTypeCode, contTypeCode) AS [Role]
	FROM dbo.dbAssociates A 
		INNER JOIN dbo.dbContact C ON C.contID = A.contID 
		LEFT OUTER JOIN dbo.GetCodeLookupDescription ('SUBASSOC', @UI) CL ON CL.cdCode = A.assocType
	WHERE fileID = @fileID AND assocActive = 1
	UNION
	SELECT
		usrFullName AS [Name]
		, usrEmail AS [Email]
		, COALESCE(CL.cdDesc, '~%FEEEARNER%~') AS [Type]
		, NULLIF(usrRole, usrRole) AS [Role]
	FROM dbUser
		LEFT OUTER JOIN dbo.GetCodeLookupDescription (@edition, @UI) CL ON CL.cdCode = '%FEEEARNER%'
	WHERE usrID IN (@fileResponsibleID, @filePrincipleID, @fileManagerID) AND usrActive = 1
	UNION
	SELECT
		usrFullName AS [Name]
		, usrEmail AS [Email]
		, COALESCE(CL.cdDesc, '~USER~') AS [Type]
		, NULLIF(usrRole, usrRole) AS [Role]
	FROM dbUser
		LEFT OUTER JOIN dbo.GetCodeLookupDescription ('SECROLE', @UI) CL ON CL.cdCode = 'USER'
	WHERE usrID = @usrID AND usrID NOT IN (@fileResponsibleID, @filePrincipleID, @fileManagerID) AND usrActive = 1
) R

GO

GRANT EXECUTE
	ON OBJECT::[dbo].[schDocuSignRecipients] TO [OMSRole], [OMSAdminRole]
	AS [dbo];
GO
