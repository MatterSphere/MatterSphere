CREATE FUNCTION [dbo].[GetE3ETimeExportXML] (
	@ID BIGINT,
	@PROCESS_TYPE NCHAR(3) = N'E3E' )
RETURNS NVARCHAR ( MAX ) AS
BEGIN
DECLARE @XML NVARCHAR ( MAX ) 

--SIMPLY BUILD THE STRING TO COLLECT DATA TO SEND THROUGH TO WEBSERVICE
--THIS ALLOWS FOR EASE OF CUSTOMISATION AT SITE IF USING FWBS OMS EXPORT SERVICE

--TODO:
--WHAT TO DO WITH UDF FIELDS AS THIS IS ENTERPRISE LEGACY
--MAPPINGS

DECLARE @WORKDATE				NVARCHAR ( 19 ) 
DECLARE @WORKHRS				NVARCHAR ( 10 ) 
DECLARE @WORKTYPE				NVARCHAR ( 7 )
DECLARE @TIMETYPE				NVARCHAR ( 16 )
DECLARE @TIMEKEEPER				NVARCHAR ( 10 )		--INT IN E3E
DECLARE @MATTER					NVARCHAR ( 10 )		--INT IN E3E
DECLARE @PHASE					NVARCHAR ( 10 )		--PHASE.CODE
DECLARE @TASK					NVARCHAR ( 10 )		--TASK.CODE
DECLARE @ACTIVITY				NVARCHAR ( 10 )		--ACTIVITY.CODE
DECLARE @NARRATIVE				NVARCHAR ( 150 )	

DECLARE @LOADSOURCE				NVARCHAR ( 15 ) 
DECLARE @LOADNUMBER				NVARCHAR ( 21 )		--MAX LENGTH OF A NEGATIVE BIGINT 

--SET THE VALUES
SELECT
	@WORKDATE					= CONVERT ( NVARCHAR ( 11 ) , DBO.UTCTOLOCALTIME ( TIMERECORDED ), 0 )
	, @WORKHRS					= CONVERT ( NVARCHAR ( 10 ) , CONVERT ( DECIMAL ( 9 , 2 ) , TIMEMINS ) / 60 )
	, @WORKTYPE					= 'Default'
	, @TIMETYPE					= ISNULL ( DBACTIVITIES.ACTACCCODE , 'MISSING' )
	, @TIMEKEEPER				= CONVERT ( NVARCHAR ( 10 ) , DBFEEEARNER.FEEEXTID )
	, @MATTER					= ISNULL ( CONVERT ( NVARCHAR ( 10 ) , DBFILE.FILEEXTLINKID ), '%MATTER%' )
	, @PHASE					= ISNULL ( E3E_PHASECODE , '' )
	, @TASK						= ISNULL ( E3E_TASKCODE , '' )
	, @ACTIVITY					= ISNULL ( E3E_ACTIVITYCODE , '' )
	, @NARRATIVE				= DBTIMELEDGER.TIMEDESC
	
	, @LOADSOURCE				= 'MatterSphere'
	, @LOADNUMBER				= CONVERT ( NVARCHAR ( 21 ) , DBTIMELEDGER.ID ) 
FROM 
	DBTIMELEDGER
INNER JOIN 
	DBFILE ON DBFILE.FILEID = DBTIMELEDGER.FILEID
INNER JOIN 
	DBFEEEARNER ON DBFEEEARNER.FEEUSRID = DBTIMELEDGER.FEEUSRID
INNER JOIN 
	DBBRANCH ON DBBRANCH.BRID = DBFILE.BRID
INNER JOIN
	DBACTIVITIES ON DBACTIVITIES.ACTCODE = DBTIMELEDGER.TIMEACTIVITYCODE
WHERE
	DBTIMELEDGER.ID = @ID AND
	(DBFILE.FILEEXTLINKID > 0 OR (DBFILE.FILEEXTLINKTXTID IS NOT NULL AND @PROCESS_TYPE = N'C3E')) AND
	DBTIMELEDGER.TIMETRANSFERRED = 0
--NEEDS VALIDATION WITH ELITE - XML ENCODE TEXT VALUES.

SELECT @XML = 
'<TimeCardPending_Srv xmlns="http://elite.com/schemas/transaction/process/write/TimeCardPending_Srv">
  <Initialize xmlns="http://elite.com/schemas/transaction/object/write/TimeCardPending">
    <Add>
      <TimeCardPending>
        <Attributes>
          <WorkDate>' +											@WORKDATE					+ '</WorkDate>
          <WorkHrs>' +											@WORKHRS					+ '</WorkHrs>
          <WorkType>' +											@WORKTYPE					+ '</WorkType>
          <TimeType>' +						DBO.GETHTMLENCODE ( @TIMETYPE, 0 )				+ '</TimeType>
          <Timekeeper>' +					DBO.GETHTMLENCODE ( @TIMEKEEPER, 0 )			+ '</Timekeeper>
          <Matter>'	+						DBO.GETHTMLENCODE ( @MATTER, 0 )				+ '</Matter>
          <Phase AliasField = "Code">' +	DBO.GETHTMLENCODE ( @PHASE, 0 )					+ '</Phase>
          <Task AliasField = "Code">' +		DBO.GETHTMLENCODE ( @TASK, 0 )					+ '</Task>
          <Activity AliasField = "Code">' +	DBO.GETHTMLENCODE ( @ACTIVITY, 0 )				+ '</Activity>
          <Narrative>'+						DBO.GETHTMLENCODE ( @NARRATIVE, 0 )				+ '</Narrative>

		  <LoadSource>' + 										@LOADSOURCE					+ '</LoadSource>
		  <LoadNumber>' +										@LOADNUMBER					+ '</LoadNumber>
        </Attributes>
      </TimeCardPending>
    </Add>
  </Initialize>
</TimeCardPending_Srv>'

	

RETURN @XML
END