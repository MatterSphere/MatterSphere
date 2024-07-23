

CREATE PROCEDURE [dbo].[srepClientNotes] 
(
	@UI uUICultureInfo='{default}',
	@ClID bigint
)

AS 
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT     
	CL.clNo,
	Cl.clName,
	CONVERT ( NVARCHAR ( MAX ) , CL.clNotes ) AS CLNotes, 
	CL.Created, 
	CL.Updated, 
	U.usrInits AS CreatedInits, 
	U1.usrInits AS UpdatedInits
FROM    
	dbo.dbClient CL 
INNER JOIN
    dbo.dbUser U ON CL.CreatedBy = U.usrID 
LEFT OUTER JOIN
    dbo.dbUser U1 ON CL.UpdatedBy = U1.usrID 
WHERE 
	CL.clID = @clID 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClientNotes] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepClientNotes] TO [OMSAdminRole]
    AS [dbo];

