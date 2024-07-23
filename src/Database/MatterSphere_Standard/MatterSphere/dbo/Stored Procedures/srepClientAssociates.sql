

CREATE PROCEDURE [dbo].[srepClientAssociates]
(
	@UI uUICultureInfo='{default}',
	@FILEID bigint
)

AS 

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT     
	CL.clNo, 
	CL.clName, 
	F.fileNo, 
	F.fileDesc, 
	C.contName, 
	X.cdDesc AS AssocType,
	Y.cdDesc AS ContType,
	CASE
		WHEN AD1.addLine1 IS NOT NULL THEN AD1.addLine1
		ELSE AD.addLine1
	END AS addLine1,
	(SELECT TOP 1
		CNB.ContNumber
	 FROM
		dbContactNumbers CNB
	 WHERE
		CNB.contID = C.contID AND CNB.contCode = 'Telephone' AND CNB.contOrder = 0 AND CNB.contActive = 1
	) as Contnumber,
	(SELECT
		MAX(D.Created)
	 FROM
		dbo.dbDocument D
	 WHERE
		D.assocID = A.assocID AND D.docDirection = 0
	) AS LastDocDate 
FROM
	dbo.dbAssociates A
LEFT OUTER JOIN
    dbo.dbAddress AD ON dbo.GetAssocAddressID(A.assocID) = AD.addID 
INNER JOIN
	dbo.dbContact C ON A.contID = C.contID
LEFT OUTER JOIN
	dbo.dbAddress AD1 ON c.contdefaultAddress = AD1.addID
INNER JOIN
    dbo.dbFile F ON A.fileID = F.fileID
INNER JOIN
    dbo.dbClient CL ON F.clID = CL.clID
LEFT JOIN
	dbo.GetCodeLookupDescription ( 'SUBASSOC' , @UI ) X  ON X.cdCode = A.AssocType
LEFT JOIN
	dbo.GetCodeLookupDescription ( 'CONTTYPE' , @UI ) Y  ON Y.cdCode = C.contTypeCode 
WHERE
	A.assocActive = 1 AND F.fileID = @fileID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClientAssociates] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClientAssociates] TO [OMSAdminRole]
    AS [dbo];

