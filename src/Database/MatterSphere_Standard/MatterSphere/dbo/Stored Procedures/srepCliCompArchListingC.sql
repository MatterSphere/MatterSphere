

CREATE PROCEDURE [dbo].[srepCliCompArchListingC]
(
	@UI uUICultureInfo='{default}',
	@CLNO nvarchar(50)
)

AS 
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

SELECT   
	X.archID,  
	X.clNo, 
	X.clName, 
	X.archRef, 
	X.archDesc,
	X.archDestroyDate,
	X.archInStorage,
	X.Created, 
	X.usrInits AS AuthInits, 
	X.ArchTypeDesc, 
	X.ArchLocDesc,
	X.contSurname,
	X.archActive,
	X.archAuthBy,
	B.checkedOutBy,
	B.IssuedTo
FROM
	(
	SELECT   
		A.archID,  
		CL.clNo, 
		CL.clName, 
		A.archRef, 
		A.archDesc,
		A.archDestroyDate,
		A.archInStorage,
		A.Created, 
		U.usrInits, 
		GCLD1.cdDesc AS ArchTypeDesc,
		GCLD2.cdDesc AS ArchLocDesc,
		CI.contSurname,
		A.archActive,
		A.archAuthBy
	FROM         
		dbo.dbClient CL
	INNER JOIN
		dbo.dbArchive A ON CL.clID = A.clID 
	LEFT OUTER JOIN
		dbo.dbUser U ON A.archAuthBy = U.usrID
	LEFT OUTER JOIN
		dbo.dbContactIndividual CI ON CL.clDefaultContact = CI.contID
	LEFT JOIN
		dbo.GetCodeLookupDescription (N'ARCHTYPE' , @UI ) GCLD1 ON GCLD1.cdCode = A.archType
	LEFT JOIN
		dbo.GetCodeLookupDescription (N'LOCTYPE' ,  @UI ) GCLD2 ON GCLD2.cdCode = A.archStorageLoc
	WHERE 
		CL.clNo = @clNo
	) X 
LEFT OUTER JOIN
	(
	SELECT
		T1.logID,
		U1.usrFullName AS CheckedOutBy,
		U2.contName AS IssuedTo
	FROM
		dbTracking T1 
	INNER JOIN
		dbArchive A1 ON A1.archID = T1.logID
	INNER JOIN
		dbUser U1 on U1.usrID = T1.trackCheckedOutBy
	LEFT OUTER JOIN
		dbContact U2 ON U2.contID = T1.trackIssuedTo
	WHERE
		T1.trackCheckedIn IS NULL AND
		T1.trackCheckedOut IS NOT NULL AND
		A1.archActive = 1
	GROUP BY
		T1.logID,
		U1.usrFullName,
		U2.contName
	) B ON X.archID = B.logID

ORDER BY
	CASE
		WHEN X.contSurname IS NOT NULL THEN X.contSurname
		ELSE X.clName
	END ASC 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliCompArchListingC] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliCompArchListingC] TO [OMSAdminRole]
    AS [dbo];

