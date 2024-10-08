CREATE FUNCTION [dbo].[GetE3EMatterExportXML] (
	@FILEID BIGINT,
	@PROCESS_TYPE NCHAR(3) = N'E3E')
RETURNS NVARCHAR ( MAX ) AS
BEGIN
DECLARE @XML NVARCHAR ( MAX ) 

--TODO - VALIDATE VARIABLE WIDTHS AND SOME FIELDS

--OPTIONS
DECLARE @USEPROXYUSER				BIT
SET @USEPROXYUSER					= 0

--REPLACEMENT VARIABLES
--MATTER
DECLARE @NUMBER						NVARCHAR ( 33 )
DECLARE @CLIENT						NVARCHAR ( 10 )
DECLARE @DISPLAYNAME				NVARCHAR ( 255 )
DECLARE @MATTSTATUS					NVARCHAR ( 20 ) 
DECLARE @MATTTYPE					NVARCHAR ( 30 )
DECLARE @OPENDATE					NVARCHAR ( 19 )
DECLARE @OPENTKPR					NVARCHAR ( 10 )
DECLARE @NARRATIVE					NVARCHAR ( MAX )
DECLARE @LOADSOURCE					NVARCHAR ( 15 ) 
DECLARE @LOADNUMBER					NVARCHAR ( 21 ) --MAX LENGTH OF A NEGATIVE BIGINT 
--MATTDATE
DECLARE @OFFICE						NVARCHAR ( 20 )
DECLARE @DEPARTMENT					NVARCHAR ( 20 )
DECLARE @SECTION					NVARCHAR ( 20 )
DECLARE @PRACTICEGROUP				NVARCHAR ( 20 )
DECLARE @BILLTKPR					NVARCHAR ( 10 )
DECLARE @RSPTKPR					NVARCHAR ( 10 )
DECLARE @SPVTKPR					NVARCHAR ( 10 )
DECLARE @MATTRATE					NVARCHAR ( 20 )
DECLARE @ARRANGEMENT				NVARCHAR ( 20 )
--PROXY
DECLARE @PROXYUSER					NVARCHAR ( 50 )

SELECT
--MATTER
	@NUMBER							= DBCLIENT.CLNO + '-' + DBFILE.FILENO		--TODO - ASK ELITE IS THIS NORMAL?
	, @DISPLAYNAME					= DBO.DBFILE.FILEDESC
	, @MATTSTATUS					= 'Prospective'			--This is a stock code used in 3E
	, @CLIENT						= CONVERT ( NVARCHAR ( 10 ) , CLEXTID )
	, @MATTTYPE						= ISNULL ( DBFILETYPE.FILEACCCODE , 'MISSING' )
	, @OPENDATE						= CONVERT ( NVARCHAR ( 11 ) , DBO.UTCTOLOCALTIME ( DBFILE.CREATED ), 0 )
	, @NARRATIVE					= ISNULL ( FILENOTES , '' )
	, @OPENTKPR						= ISNULL ( CONVERT ( NVARCHAR ( 10 ) , DBFEEEARNER.FEEEXTID ) , 'MISSING' )
	, @OFFICE						= ISNULL ( DBBRANCH.BRCODE , 'MISSING' )
	, @LOADSOURCE					= 'MatterSphere'
	, @LOADNUMBER					= CONVERT ( NVARCHAR ( 21 ) , DBFILE.FILEID ) 
--MATTDATE
	, @DEPARTMENT					= ISNULL ( DBDEPARTMENT.DEPTACCCODE , 'MISSING' )
	, @SECTION						= '000'					--This must be set up in 3E - or changed to firms default
	, @PRACTICEGROUP				= 'Default'				--This must be set up in 3E - or changed to firms default
	, @BILLTKPR						= ISNULL ( CONVERT ( NVARCHAR ( 10 ) , DBFEEEARNER.FEEEXTID ) , 'MISSING' )
	, @RSPTKPR						= ISNULL ( CONVERT ( NVARCHAR ( 10 ) , RESPONSIBLE.FEEEXTID ) , 'MISSING' )
	, @SPVTKPR						= ISNULL ( CONVERT ( NVARCHAR ( 10 ) , COALESCE ( SUPERVISOR.FEEEXTID, RESPONSIBLE.FEEEXTID ) ) , 'MISSING' )
	, @ARRANGEMENT					= 'Hourly'				--This must be set up in 3E - or changed to firms default
--MATTRATE
	, @MATTRATE						= 'Tkpr_Std'			--This must be set up in 3E - or changed to firms default
--PROXY
	, @PROXYUSER					= CASE WHEN @USEPROXYUSER = 1 THEN 'ProxyUser="' + CREATEDBY.USRADID + '"' ELSE '' END 
FROM 
	DBFILE
INNER JOIN 
	DBFILETYPE ON DBFILE.FILETYPE = DBFILETYPE.TYPECODE
INNER JOIN 
	DBCLIENT ON DBCLIENT.CLID = DBFILE.CLID
INNER JOIN 
	DBFEEEARNER ON DBFEEEARNER.FEEUSRID = DBFILE.FILEPRINCIPLEID
INNER JOIN 
	DBFEEEARNER RESPONSIBLE ON RESPONSIBLE.FEEUSRID = DBFILE.FILERESPONSIBLEID
LEFT JOIN 
	DBFEEEARNER SUPERVISOR ON SUPERVISOR.FEEUSRID = DBFILE.FILEMANAGERID
INNER JOIN 
	DBBRANCH ON DBBRANCH.BRID = DBFILE.BRID
INNER JOIN 
	DBDEPARTMENT ON DBDEPARTMENT.DEPTCODE = DBFILE.FILEDEPARTMENT
INNER JOIN 
	DBUSER CREATEDBY ON CREATEDBY.USRID = DBFILE.CREATEDBY
WHERE 
	DBFILE.FILEID = @FILEID AND
	(DBCLIENT.CLEXTID > 0 OR (DBCLIENT.CLEXTTXTID IS NOT NULL AND @PROCESS_TYPE = N'C3E'))

--NEEDS VALIDATION WITH ELITE
SELECT @XML = 
'<Matter_Srv xmlns="http://elite.com/schemas/transaction/process/write/Matter_Srv" ' + @PROXYUSER + '>
  <Initialize xmlns="http://elite.com/schemas/transaction/object/write/Matter">
   <Add>
    <Matter>
     <Attributes>
      <Number>' +								@NUMBER						+ '</Number>
	  <Client>' +						ISNULL( @CLIENT, '%CLIENT%')		+ '</Client>
	  <DisplayName>' +		DBO.GETHTMLENCODE ( @DISPLAYNAME, 0 )			+ '</DisplayName>
	  <MattStatus>' +							@MATTSTATUS					+ '</MattStatus>
	  <MattType>' +			DBO.GETHTMLENCODE ( @MATTTYPE, 0 )				+ '</MattType>
	  <OpenDate>' +								@OPENDATE					+ '</OpenDate>
	  <Narrative>' +		DBO.GETHTMLENCODE ( @NARRATIVE, 0 )				+ '</Narrative>
      <OpenTkpr>' +								@OPENTKPR					+ '</OpenTkpr>
	  <LoadSource>' + 							@LOADSOURCE					+ '</LoadSource>
	  <LoadNumber>' +							@LOADNUMBER					+ '</LoadNumber>
     </Attributes>
     <Children>
	  <MattDate>
       <Edit>
        <MattDate Position="0">
         <Attributes>
          <Office>' +		DBO.GETHTMLENCODE ( @OFFICE, 0 )				+ '</Office>
          <Department>' +	DBO.GETHTMLENCODE ( @DEPARTMENT, 0 )			+ '</Department>
          <Section>' +							@SECTION					+ '</Section>
          <PracticeGroup>' +					@PRACTICEGROUP				+ '</PracticeGroup>
          <BillTkpr>' +							@BILLTKPR					+ '</BillTkpr>
          <RspTkpr>' +							@RSPTKPR					+ '</RspTkpr>
          <SpvTkpr>' +							@SPVTKPR					+ '</SpvTkpr>
          <Arrangement>'+						@ARRANGEMENT				+ '</Arrangement>
         </Attributes>
        </MattDate>
       </Edit>
      </MattDate> 
      <MattRate>
       <Edit>
        <MattRate Position="0">
         <Attributes>
          <Rate>' +								@MATTRATE					+ '</Rate>
         </Attributes>
        </MattRate>
       </Edit>
     </MattRate>
     </Children>
    </Matter>
   </Add>
  </Initialize>
 </Matter_Srv>'

RETURN @XML
END
