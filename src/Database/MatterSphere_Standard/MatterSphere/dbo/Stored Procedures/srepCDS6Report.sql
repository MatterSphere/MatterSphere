

CREATE PROCEDURE [dbo].[srepCDS6Report] 
(
	@BRID int,
	@UFNCLAIMEDDATE datetime = NULL ,
	@FINALISE bit = 0,
	@PRINTCOPY bit = 0,
	@SUMMARY bit = 0,
	@REPORTTITLE nvarchar(50) = '',
	@UPTOCONCLUDEDDATE datetime = NULL ,
	@STARTDATE datetime = NULL ,
	@ENDDATE datetime = NULL ,
	@HIDDENDATE datetime = NULL ,
	@UI uUICultureInfo = '{default}'
)
AS

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED 

DECLARE @SELECT nvarchar(3000)
DECLARE @WHERE nvarchar(3000)
DECLARE @ORDERBY nvarchar(100)

-- select
SET @SELECT = N'
SELECT 
	X.cdDesc AS CrimLARef,
	CDS.UFNCDSRef, 
	CDS.UFNProCosts, 
	CDS.UFNDisb, 
	CDS.UFNTravel, 
	CDS.UFNWaiting, 
	CASE 
		WHEN SUBSTRING(CDS.UFNCode,1,6)=''000000'' THEN ''''
		ELSE CDS.UFNCode 
	END AS UFNCode, 
	CDS.UFNCLCode, 
	CDS.UFNOffCode, 
	CDS.UFNOutCode, 
	CDS.UFNConcluded, 
	CDS.UFNSuspects, 
	CDS.UFNAttendances, 
	CDS.UFNIdentifier as UFNIdentifier, 
	CDS.UFNDuty, 
	CDS.UFNYouth, 
	CDS.UFNCDS11Created, 
	CDS.UFNClaimed, 
	CDS.UFNClaimedDate, 
	CASE 
		WHEN [UFNCDSSupplementary]=1 THEN ''(Supplementary to an earlier claim)''
		ELSE ''''
	END AS SuppRef,
	CASE 
		WHEN @PRINTCOPY = 1 then ''COPY''
		else null 
	END AS CopyText,
	@SUMMARY as Summary,
	CASE
		WHEN UFNEthnicOrigin IS NOT NULL THEN
			Replicate(''0'',2 -len(UFNEthnicOrigin)) + convert(nvarchar(2),UFNEthnicOrigin)
		ELSE ''99'' 
	END AS EthnicOrigin,
	CDS.UFNSex AS Sex,
	CASE
		WHEN CDS.UFNDisabilityFlag IS NOT NULL THEN CDS.UFNDisabilityFlag
		ELSE ''U''
	END AS DisabilityFlag,
	CASE
		WHEN UFNEthnicOrigin IS NOT NULL THEN
			Left(Replicate(''0'',2 -len(UFNEthnicOrigin)) + convert(nvarchar(2),UFNEthnicOrigin),1)
		ELSE ''9'' 
	END AS Ethnic1,
	CASE
		WHEN UFNEthnicOrigin IS NOT NULL THEN
			Right(Replicate(''0'',2 -len(UFNEthnicOrigin)) + convert(nvarchar(2),UFNEthnicOrigin),1)
		ELSE ''9'' 
	END AS Ethnic2
FROM 
	DBCDSClaim CDS
INNER JOIN 
	dbBranch BR on BR.brid = CDS.UFNBRID
LEFT OUTER JOIN
	dbo.GetCodeLookUpDescription(''CRIMLAREF'', @UI) AS X ON X.cdCode = BR.brCode '

-- where clause
SET @WHERE = N' WHERE CDS.UFNBRID = @BRID'

-- UfnClaimedDate filter
IF (@UFNCLAIMEDDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND (CDS.UFNClaimedDate >= @STARTDATE AND CDS.UFNClaimedDate < @ENDDATE) '
END
ELSE
BEGIN
	SET @WHERE = @WHERE + ' AND (CDS.UFNClaimedDate is Null) '
END

-- UptoConcluded date filter
IF (@UPTOCONCLUDEDDATE IS NOT NULL)
BEGIN
	SET @WHERE = @WHERE + ' AND ufnconcluded <= @UPTOCONCLUDEDDATE '
END

-- order by clause
SET @ORDERBY = N' ORDER BY CDS.UFNCDSRef '

-- Finalise the report and set the date completed flag...
if @FINALISE = 1 
BEGIN
	IF @UPTOCONCLUDEDDATE is Null 
		BEGIN
			SET @UPTOCONCLUDEDDATE = @HIDDENDATE
	END
	DECLARE @DATETOSET datetime 
	set @DATETOSET = @UPTOCONCLUDEDDATE
	update 
		dbCDSClaim 
	set 
		UFNClaimedDate = @DATETOSET 
	where 
		UFNClaimedDate is null and 
		UFNConcluded < DateAdd(dd,1,@UPTOCONCLUDEDDATE) and
		UFNBRID = @BRID
END

--- Add Statements together
DECLARE @SQL nvarchar(4000)
set @SQL = Rtrim(@SELECT) + Rtrim(@WHERE) + Rtrim(@ORDERBY)

--- Debug Print
-- PRINT @SQL


exec sp_executesql @sql,
 N'
	@BRID int
	, @UFNCLAIMEDDATE datetime
	, @FINALISE bit
	, @PRINTCOPY bit
	, @SUMMARY bit 
	, @REPORTTITLE nvarchar(50)
	, @UPTOCONCLUDEDDATE datetime
	, @STARTDATE datetime
	, @ENDDATE datetime
	, @HIDDENDATE datetime
	,@UI uUICultureInfo'
	, @BRID
	, @UFNCLAIMEDDATE
	, @FINALISE
	, @PRINTCOPY
	, @SUMMARY
	, @REPORTTITLE
	, @UPTOCONCLUDEDDATE
	, @STARTDATE
	, @ENDDATE
	, @HIDDENDATE
	, @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCDS6Report] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepCDS6Report] TO [OMSAdminRole]
    AS [dbo];

