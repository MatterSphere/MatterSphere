

CREATE PROCEDURE [dbo].[sprContactPassiveSearch]
	(
		@UI uCodeLookup,
		@contName nvarchar(128),
		@addLine1 nvarchar(100),
		@postCode nvarchar(20)
	)
AS
DECLARE @selectSQL nvarchar(4000) , @whereSQL nvarchar(1000) , @unionSQL nvarchar(1500)
SET @selectSQL = '
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT TOP 50 * FROM ( SELECT
	CONT.contid,
	CONT.contname,
	X.[cdDesc] as contTypedesc,
	Y.[cdDesc] as conGroupDesc,
	CONT.contIsClient,
	CONT.contApproved,
	CI.contDOB,
	ADR.addID,
	ADR.addline1,
	ADR.addline2,
	ADR.addline3,
	ADR.addline4,
	ADR.addline5,
	ADR.addpostcode
FROM 
	dbContact CONT
INNER JOIN
	dbContactType CT on CT.typeCode = CONT.contTypeCode
LEFT JOIN
	dbAddress ADR ON ADR.addID = CONT.contDefaultAddress
LEFT JOIN
	dbContactIndividual CI ON CI.contID = CONT.contID
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''CONTYPE'' , @UI ) X ON X.[cdCode] = CONT.[contTypeCode]
LEFT JOIN
	[dbo].[GetCodeLookupDescription] ( ''CONTGROUP'' , @UI ) Y ON Y.[cdCode] = CONT.[contGroup] '

SET @whereSQL = ''
SET @unionSQL = ' ) A '

IF @addLine1 IS NOT NULL
BEGIN
	SET @whereSQL = @whereSQL + ' AND ADR.[addLine1] LIKE ''%'' + @addLine1 + ''%'''
	SET @unionSQL = '
		UNION ALL SELECT TOP 50
			NULL as contid,
			NULL as contname,
			NULL as conttypedesc,
			NULL as congroupdesc,
			NULL as contisclient,
			NULL as contapproved,
			NULL as contdob,
			ADR.addID,
			ADR.addline1,
			ADR.addline2,
			ADR.addline3,
			ADR.addline4,
			ADR.addline5,
			ADR.addpostcode
		FROM
			dbAddress ADR '
END

IF @postCode IS NOT NULL
BEGIN
	SET @whereSQL = @whereSQL + ' AND ADR.[addPostCode] LIKE ''%'' + @postCode + ''%'''
	SET @unionSQL = '
	UNION SELECT TOP 50
		NULL as contid,
		NULL as contname,
		NULL as conttypedesc,
		NULL as congroupdesc,
		NULL as contisclient,
		NULL as contapproved,
		NULL as contdob,
		ADR.addID,
		ADR.addline1,
		ADR.addline2,
		ADR.addline3,
		ADR.addline4,
		ADR.addline5,
		ADR.addpostcode
	FROM
		dbAddress ADR ' 
END


IF @unionSQL <>  ' ) A '
	SET @unionSQL = @unionSQL +  ' WHERE ' +  Substring ( @whereSQL , 6 , 994 ) + ' ) A '

--IF @contName IS NOT NULL
IF @whereSQL IS NOT NULL
	SET @whereSQL = @whereSQL + ' AND CONT.[contName] LIKE ''%'' + @contName + ''%'''
ELSE
	SET @whereSQL = @whereSQL + ' CONT.[contName] LIKE ''%'' + @contName + ''%'''	

IF LEFT ( @whereSQL , 4 ) = ' AND'
	SET @whereSQL =  ' WHERE ' +  Substring ( @whereSQL , 6 , 994 )


SET @selectSQL = @selectSQL + @whereSQL + @unionSQL + ' ORDER BY A.contName DESC'

-- Debug
 PRINT @selectSQL

EXECUTE sp_executeSQL @selectSQL ,
		N'@UI uCodeLookup ,
		@contName nvarchar(128) ,
		@addLine1 nvarchar(100) ,
		@postCode nvarchar(20)',
		@UI ,
		@contName ,
		@addLine1 ,
		@postCode


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprContactPassiveSearch] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprContactPassiveSearch] TO [OMSAdminRole]
    AS [dbo];

