IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetE3EEntityOrgExportXML]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetE3EEntityOrgExportXML]
GO

CREATE FUNCTION [dbo].[GetE3EEntityOrgExportXML] ( @CONTID BIGINT )
RETURNS NVARCHAR ( MAX ) AS
BEGIN
DECLARE @XML NVARCHAR ( MAX ) 

--OPTIONS
DECLARE @USEPROXYUSER			BIT
SET @USEPROXYUSER				= 0

--REPLACEMENT VARIABLES
--ENTITY BASED
DECLARE @COMMENT				NVARCHAR ( 4000 )

--ENTITYORG BASED
DECLARE @ORGNAME				NVARCHAR ( 128 )

--SITE/ADDRESS BASED
DECLARE @SITE_DESCRIPTION		NVARCHAR ( 50 ) 
DECLARE @SITE_SITETYPE			NVARCHAR ( 50 ) 
DECLARE @STREET					NVARCHAR ( 64 )
DECLARE	@ADDITIONAL1			NVARCHAR ( 64 ) 
DECLARE @ADDITIONAL2			NVARCHAR ( 64 )
DECLARE @CITY					NVARCHAR ( 64 )
DECLARE @COUNTY					NVARCHAR ( 64 )
DECLARE @STATE					NVARCHAR ( 16 )
DECLARE @ZIPCODE				NVARCHAR ( 20 )
DECLARE @COUNTRY				NVARCHAR ( 8 )
DECLARE @SITE_PHONE_DESCRIPTION	NVARCHAR ( 50 )
DECLARE @SITE_PHONE_NUMBER		NVARCHAR ( 30 )
DECLARE @SITE_EMAIL_EMAILADDR	NVARCHAR ( 200 )
DECLARE @SITE_URL				NVARCHAR ( 255 ) 

--OTHER
DECLARE @PROXYUSER				NVARCHAR ( 50 )
DECLARE @LOADSOURCE				NVARCHAR ( 15 ) 
DECLARE @LOADNUMBER				NVARCHAR ( 21 ) --MAX LENGTH OF A NEGATIVE BIGINT 
DECLARE @CONTEXTID				BIGINT

--CHILDREN XML IF NEEDED
DECLARE @PHONEXML				NVARCHAR ( 1000 )
DECLARE @EMAILXML				NVARCHAR ( 1000 )
DECLARE @URLXML					NVARCHAR ( 1000 ) 

--IF CONTACT TYPE IS COMPANY THEN RETURN NULL AS THIS IS FOR ENTITYPERSON
DECLARE @CONTTYPEXML NVARCHAR ( MAX )
SELECT @CONTTYPEXML = TYPEXML FROM DBCONTACT INNER JOIN DBCONTACTTYPE ON DBCONTACT.CONTTYPECODE = DBCONTACTTYPE.TYPECODE WHERE CONTID = @CONTID

IF ( SELECT CHARINDEX ( 'GeneralType="Company"' , @CONTTYPEXML ) ) = 0
	RETURN NULL




--COLLECT THE DATA
SELECT
	@COMMENT					= ISNULL ( CONTNOTES , '' )
	, @ORGNAME					= ISNULL ( CONTNAME , 'MISSING' )
	, @SITE_DESCRIPTION			= 'Default Address - MatterSphere'
	, @SITE_SITETYPE			= 'Billing'
	, @STREET					= ISNULL ( ADDLINE1 , 'MISSING' )
	, @ADDITIONAL1				= ISNULL ( ADDLINE2 , '' )
	, @ADDITIONAL2				= ISNULL ( ADDLINE3 , '' )
	, @CITY						= ISNULL ( ADDLINE4 , '' )
	, @COUNTY					= CASE CTRYCODE
									WHEN 'US' THEN ''
									WHEN 'CA' THEN ''
									WHEN 'AU' THEN ''
									ELSE ISNULL ( ADDLINE5 , '' )
								END 
	, @STATE					= CASE CTRYCODE
									WHEN 'US' THEN ISNULL ( ADDLINE5 , '' )
									WHEN 'CA' THEN ISNULL ( ADDLINE5 , '' )
									WHEN 'AU' THEN ISNULL ( ADDLINE5 , '' )
									ELSE ''
								END
	, @ZIPCODE					= ISNULL ( ADDPOSTCODE , '' )
	, @COUNTRY					= CTRYCODE
	, @SITE_PHONE_DESCRIPTION	= 'Default Number'
	, @SITE_PHONE_NUMBER		= ISNULL ( ( SELECT TOP 1 CONTNUMBER FROM DBCONTACTNUMBERS WHERE CONTID = @CONTID ORDER BY CONTORDER ) , '' )--FIRST NUMBER FOR DEFAULT CONTACT
	, @SITE_EMAIL_EMAILADDR		= ISNULL ( ( SELECT TOP 1 CONTEMAIL FROM DBCONTACTEMAILS WHERE CONTID = @CONTID ORDER BY CONTORDER ) , '' ) --FIRST NUMBER FOR DEFAULT CONTACT
	, @SITE_URL					= ISNULL ( CONTWEBSITE , '' ) 

	, @LOADSOURCE				= 'MatterSphere'
	, @LOADNUMBER				= CONVERT ( NVARCHAR ( 21 ) , DBCONTACT.CONTID ) 
	, @CONTEXTID				= DBCONTACT.CONTEXTID
	, @PROXYUSER				= CASE WHEN @USEPROXYUSER = 1 THEN 'ProxyUser="' + CREATEDBY.USRADID + '"' ELSE '' END 
FROM 
	DBCONTACT 
INNER JOIN 
	DBADDRESS ON DBADDRESS.ADDID = DBCONTACT.CONTDEFAULTADDRESS
INNER JOIN 
	DBCOUNTRY ON DBCOUNTRY.CTRYID = DBADDRESS.ADDCOUNTRY
INNER JOIN 
	DBUSER CREATEDBY ON CREATEDBY.USRID = DBCONTACT.CREATEDBY
WHERE 
	DBCONTACT.CONTID = @CONTID
	
--IF ALREADY EXPORTED RETURN NULL
IF ISNUMERIC ( @CONTEXTID ) = 1 RETURN NULL

--CHECK FOR ADDRESS OF TYPE BILLING
IF EXISTS ( SELECT TOP 1 DBADDRESS.ADDID FROM DBADDRESS INNER JOIN DBCOUNTRY ON DBCOUNTRY.CTRYID = DBADDRESS.ADDCOUNTRY INNER JOIN DBCONTACTADDRESSES ON DBCONTACTADDRESSES.CONTADDID = DBADDRESS.ADDID WHERE DBCONTACTADDRESSES.CONTID = @CONTID AND DBCONTACTADDRESSES.CONTCODE = 'BILLING' AND DBCONTACTADDRESSES.CONTACTIVE = 1 )
BEGIN
	SELECT TOP 1
	@STREET						= ISNULL ( ADDLINE1 , 'MISSING' )
	, @ADDITIONAL1				= ISNULL ( ADDLINE2 , '' )
	, @ADDITIONAL2				= ISNULL ( ADDLINE3 , '' )
	, @CITY						= ISNULL ( ADDLINE4 , '' )
	, @COUNTY					= CASE CTRYCODE
									WHEN 'US' THEN ''
									WHEN 'CA' THEN ''
									WHEN 'AU' THEN ''
									ELSE ISNULL ( ADDLINE5 , '' )
								END 
	, @STATE					= CASE CTRYCODE
									WHEN 'US' THEN ISNULL ( ADDLINE5 , '' )
									WHEN 'CA' THEN ISNULL ( ADDLINE5 , '' )
									WHEN 'AU' THEN ISNULL ( ADDLINE5 , '' )
									ELSE ''
								END

	, @ZIPCODE					= ISNULL ( ADDPOSTCODE , '' )
	, @COUNTRY					= CTRYCODE
FROM
	DBADDRESS
INNER JOIN 
	DBCOUNTRY ON DBCOUNTRY.CTRYID = DBADDRESS.ADDCOUNTRY
INNER JOIN 
	DBCONTACTADDRESSES ON DBCONTACTADDRESSES.CONTADDID = DBADDRESS.ADDID
WHERE
	DBCONTACTADDRESSES.CONTID = @CONTID AND
	DBCONTACTADDRESSES.CONTCODE = 'BILLING' AND 
	DBCONTACTADDRESSES.CONTACTIVE = 1
ORDER BY 
	CONTORDER ASC
END 
	
--CANNOT INCLUDE PHONE AND EMAIL SECTIONS IF THERE IS NO PHONE OR EMAIL TO PASS THROUGH SO TREAT THESE SEPARATELY
IF @SITE_PHONE_NUMBER != ''
SET @PHONEXML = '
              <Site_Phone>
               <Add>
                <Site_Phone>
                 <Attributes>
                  <Description>' +			@SITE_PHONE_DESCRIPTION		+ '</Description>
                  <Number>' +				@SITE_PHONE_NUMBER			+ '</Number>
                 </Attributes>
                </Site_Phone>
               </Add>
              </Site_Phone>'
IF @SITE_EMAIL_EMAILADDR != ''
SET @EMAILXML = '
              <Site_EMail>
               <Add>
                <Site_EMail>
                 <Attributes>
                  <EmailAddr>' +	DBO.GETHTMLENCODE ( @SITE_EMAIL_EMAILADDR, 0 )	+ '</EmailAddr>
                 </Attributes>
                </Site_EMail>
               </Add>
              </Site_EMail>'
IF @SITE_URL != ''
SET @URLXML = '
              <Site_URL>
               <Add>
                <Site_URL>
                 <Attributes>
                  <URL>' +					@SITE_URL					+ '</URL>
                 </Attributes>
                </Site_URL>
               </Add>
              </Site_URL>'

SET @XML = 
'<NxBizTalkEntityOrgLoad xmlns="http://elite.com/schemas/transaction/process/write/NxBizTalkEntityOrgLoad" ' + @PROXYUSER + '>
  <Initialize xmlns="http://elite.com/schemas/transaction/object/write/EntityOrg">
   <Add>
    <EntityOrg>
     <Attributes>
      <Comment>' +				DBO.GETHTMLENCODE ( @COMMENT, 0 )				+ '</Comment>
      <OrgName>' +				DBO.GETHTMLENCODE ( @ORGNAME, 0 )				+ '</OrgName>
	  <LoadSource>' + 								@LOADSOURCE					+ '</LoadSource>
	  <LoadNumber>' +								@LOADNUMBER					+ '</LoadNumber>
     </Attributes>
     <Children>
      <Relate>
       <Edit>
        <Relate Position="0">
         <Children>
          <Site>
           <Add>
            <Site>
             <Attributes>
              <Description>' +						@SITE_DESCRIPTION			+ '</Description>
              <SiteType>' +							@SITE_SITETYPE				+ '</SiteType>
              <Street>' +		DBO.GETHTMLENCODE ( @STREET, 0 )				+ '</Street>
              <Additional1>' +	DBO.GETHTMLENCODE ( @ADDITIONAL1, 0 )			+ '</Additional1>
              <Additional2>' +	DBO.GETHTMLENCODE ( @ADDITIONAL2, 0 )			+ '</Additional2>
              <City>' +			DBO.GETHTMLENCODE ( @CITY, 0 )					+ '</City>
              <County>' +		DBO.GETHTMLENCODE ( @COUNTY, 0 )				+ '</County>
              <State>' +		DBO.GETHTMLENCODE ( @STATE, 0 )					+ '</State>
              <ZipCode>' +		DBO.GETHTMLENCODE ( @ZIPCODE, 0 )				+ '</ZipCode>
              <Country>' +							@COUNTRY					+ '</Country>
			  <IsDefault>1</IsDefault>
             </Attributes>'
             
IF ISNULL ( @PHONEXML , '' ) != '' OR ISNULL ( @URLXML , '' ) !='' OR ISNULL ( @EMAILXML , '' ) !=''
SET @XML = @XML + 
'
            <Children>' + ISNULL ( @PHONEXML , '' ) + ISNULL ( @URLXML , '' ) + ISNULL ( @EMAILXML , '' ) + '
             </Children>'
             
--COMPLETE XML
SET @XML = @XML + '
            </Site>
           </Add>
          </Site>
         </Children>
        </Relate>
       </Edit>
      </Relate>
     </Children>
    </EntityOrg>
   </Add>
  </Initialize>
 </NxBizTalkEntityOrgLoad>'


RETURN @XML
END
GO


