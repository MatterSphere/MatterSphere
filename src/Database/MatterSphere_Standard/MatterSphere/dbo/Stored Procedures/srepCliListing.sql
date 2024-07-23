

CREATE PROCEDURE [dbo].[srepCliListing]
(
	@UI uUICultureInfo='{default}',
	@FIRMCONT nvarchar(50) = null,
	@SOURCE nvarchar(50) = null,
	@CLTYPE nvarchar(50) = null,
	@CLASSIFICATION nvarchar(10) = null,
	@STARTDATE datetime = null,
	@ENDDATE datetime = null
)

AS 

DECLARE @SELECT nvarchar(3000)
DECLARE @WHERE nvarchar(1000)

--- Set the Select Statement
SET @SELECT = N'
SELECT     
	CL.clNo, 
	CL.clName,
    COALESCE(CL1.cdDesc, ''~'' + NULLIF(CL.cltypeCode, '''') + ''~'') AS ClTypeDesc, 
	CL.clSearch1,
	CL.clSearch2,
	CL.Created,
	U.usrInits AS ContInits, 
	U.usrFullName AS ContName,
	COALESCE(A.addPostCode, A1.AddPostCode) AS PostCode,
	CASE 
	WHEN CL.clSource = ''CONTACT'' THEN ''Contact: '' + C1.contName
	WHEN CL.clSource = ''USER'' THEN ''User: '' + U1.usrFullName
	ELSE COALESCE(CL2.cdDesc, ''~'' + NULLIF(CL.clSource, '''') + ''~'')
	END AS Source,
	U2.usrInits
FROM         
	dbo.dbClient CL 
LEFT OUTER JOIN
	dbo.dbUser U ON CL.feeusrID = U.usrID
LEFT OUTER JOIN
	dbo.dbAddress A ON A.addID = CL.clDefaultAddress
LEFT OUTER JOIN
	dbo.dbContact C ON C.contID = CL.clDefaultContact
LEFT OUTER JOIN
	dbo.dbAddress A1 ON A1.addID = C.contDefaultAddress
LEFT OUTER JOIN
	dbo.dbContact C1 ON C1.contID = CL.clSourceContact
LEFT OUTER JOIn
	dbo.dbUser U1 ON U1.usrID = CL.clSourceUser
INNER JOIN
	dbo.dbUser U2 ON U2.usrID = CL.CreatedBy
LEFT JOIN dbo.GetCodeLookupDescription ( ''CLTYPE'', @UI ) CL1 ON CL1.[cdCode] = CL.cltypeCode
LEFT JOIN dbo.GetCodeLookupDescription ( ''SOURCE'', @UI ) CL2 ON CL2.[cdCode] = CL.clSource'

--- Set the Where Statement
SET @WHERE = '
WHERE
	COALESCE(U.usrID, '''') = COALESCE(@FIRMCONT, U.usrID, '''') AND
	COALESCE(CL.clSource, '''') = COALESCE(@SOURCE, CL.clSource, '''') AND
	COALESCE(CL.clTypeCode, '''') = COALESCE(@CLTYPE, CL.clTypeCode, '''') AND
	(CL.Created BETWEEN COALESCE(dbo.GetStartDate(@STARTDATE), CL.Created) AND COALESCE(dbo.GetEndDate(@ENDDATE), CL.Created))'

if @CLASSIFICATION is not null
begin
	IF @CLASSIFICATION = 'TRUE'
		BEGIN
			SET @WHERE = @WHERE + ' AND CL.clPreClient =  1'
		END
	ELSE
		BEGIN
			SET @WHERE = @WHERE + ' AND CL.clPreClient =  0'
		END
END

DECLARE @SQL nvarchar(4000)
--- Join the Statements together
SET @SQL = @SELECT + @WHERE

--- Debug Print
PRINT @SQL

exec sp_executesql @sql, N'@UI nvarchar(10), @FIRMCONT nvarchar(50), @SOURCE nvarchar(50), @CLTYPE nvarchar(50), @STARTDATE datetime, @ENDDATE datetime', @UI, @FIRMCONT, @SOURCE, @CLTYPE, @STARTDATE, @ENDDATE

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliListing] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCliListing] TO [OMSAdminRole]
    AS [dbo];

