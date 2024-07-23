

CREATE VIEW [dbo].[vwDBDocumentIndexView]
AS
Select
	D.DocID,
	DV.VerNumber,
	D.CLID,
	D.FileID,
	D.AssocID,
	D.DocDesc,
	CASE Right(COALESCE(DIR.DirPath,' '),1)
		WHEN ' ' THEN ''
		WHEN '\' THEN DIR.DirPath
		ELSE DIR.DirPath + '\'
	END
	+
	CASE COALESCE(DV.VerToken,'')
		WHEN '' THEN D.DocFileName
		ELSE DV.VerToken
	END
	as DocFullPath,
	COALESCE(DV.Created,D.Created) as VerCreated,
	COALESCE(DV.Updated,D.Updated) as VerUpdated,
	UC.UsrFullName as VerCreatedBy,
	UU.UsrFullName as VerUpdatedBy
From
	DBDocument D
LEFT OUTER JOIN
	DBDocumentVersion DV ON D.DocID = DV.DocID
LEFT OUTER JOIN
	DBDirectory DIR on D.DocDirID = DIR.DirID
INNER JOIN
	DBUser UC ON COALESCE(DV.CreatedBY,D.CreatedBy) = UC.UsrID
INNER JOIN
	DBuser UU on COALESCE(DV.UpdatedBY,D.UpdatedBY) = UU.UsrID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwDBDocumentIndexView] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBDocumentIndexView] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBDocumentIndexView] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwDBDocumentIndexView] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwDBDocumentIndexView] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwDBDocumentIndexView] TO [OMSApplicationRole]
    AS [dbo];

