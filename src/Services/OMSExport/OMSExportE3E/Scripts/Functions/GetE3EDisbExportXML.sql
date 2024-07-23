
/****** Object:  UserDefinedFunction [dbo].[GetE3EDisbExportXML]    Script Date: 4/10/2017 9:48:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO






CREATE FUNCTION [dbo].[GetE3EDisbExportXML] ( @ID BIGINT )
RETURNS NVARCHAR ( MAX ) AS
BEGIN
DECLARE @XML NVARCHAR ( MAX ) 

--SIMPLY BUILD THE STRING TO COLLECT DATA TO SEND THROUGH TO WEBSERVICE
--THIS ALLOWS FOR EASE OF CUSTOMISATION AT SITE IF USING FWBS OMS EXPORT SERVICE

--TODO:
--WHAT TO DO WITH UDF FIELDS AS THIS IS ENTERPRISE LEGACY
--MAPPINGS

DECLARE @COSTDATE				NVARCHAR ( 19 ) 
DECLARE @COSTTYPE				NVARCHAR ( 15 )
DECLARE @MATTER					NVARCHAR ( 10 )		--INT IN E3E
DECLARE @NARRATIVE				NVARCHAR ( 150 )	

DECLARE @AMT					NVARCHAR ( 10 )
DECLARE @LOADSOURCE				NVARCHAR ( 15 ) 
DECLARE @LOADNUMBER				NVARCHAR ( 21 )		--MAX LENGTH OF A NEGATIVE BIGINT 

--SET THE VALUES
SELECT
	@COSTDATE					= CONVERT ( NVARCHAR ( 11 ) , DBO.UTCTOLOCALTIME ( FL.FINITEMDATE ), 0 )
	, @AMT						= CONVERT ( NVARCHAR ( 10 ) , CONVERT ( DECIMAL ( 9 , 2 ) , 1))
	, @COSTTYPE					= ISNULL ( dbPostingEntryType.postCode , 'MISSING' )
	, @MATTER					= CONVERT ( NVARCHAR ( 10 ) , DBFILE.FILEEXTLINKID )
	, @NARRATIVE				= FL.finDesc + CASE WHEN FL.FINPAYNAME IS NULL THEN '' ELSE '; ' + FL.FINPAYNAME END
														+ CASE WHEN FL.FINTHEIRREF IS NULL THEN '' ELSE '; REF:' + FL.FINTHEIRREF END
	, @LOADNUMBER				= CONVERT ( NVARCHAR ( 21 ) , FL.FINLOGID ) 

FROM 
	dbFinancialLedger FL
INNER JOIN 
	DBFILE ON DBFILE.FILEID = FL.FILEID
INNER JOIN
	dbPostingEntryType ON dbPostingEntryType.POSTID = FL.finEntryID
WHERE
	fl.FinLogID = @ID AND
	DBFILE.FILEEXTLINKID IS NOT NULL AND
	ISNULL(FL.finTransferred,0) = 0
--NEEDS VALIDATION WITH ELITE - XML ENCODE TEXT VALUES.

SELECT @XML = 
'<CostCardPending_Srv xmlns="http://elite.com/schemas/transaction/process/write/CostCardPending_Srv">
	<Initialize xmlns="http://elite.com/schemas/transaction/object/write/CostCardPending">
		<Add>
			<CostCardPending>
				<Attributes>
					<WorkDate>'+						@COSTDATE		+ '</WorkDate>
					<Matter>' +		DBO.GETHTMLENCODE ( @MATTER, 0 )	+ '</Matter>
					<IsNB>0</IsNB>
					<IsNoCharge>0</IsNoCharge>
					<WorkQty>1</WorkQty>
					<Narrative>' +	DBO.GETHTMLENCODE ( @NARRATIVE, 0 )	+ '</Narrative>
					<CostType>' +	DBO.GETHTMLENCODE ( @COSTTYPE, 0 )	+ '</CostType>
					<LoadNumber>' +						@LOADNUMBER		+ '</LoadNumber>
					<LoadSource>Mattersphere</LoadSource>
				</Attributes>
			</CostCardPending>
		</Add>
	</Initialize>
</CostCardPending_Srv>'

	

RETURN @XML
END

GO

