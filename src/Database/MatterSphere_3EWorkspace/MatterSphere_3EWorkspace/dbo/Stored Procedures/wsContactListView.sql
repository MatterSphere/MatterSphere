﻿CREATE PROCEDURE [dbo].[wsContactListView]

@FEEUSRID BIGINT,
@UI NVARCHAR(15)='EN-GB',
@SEARCH_TEXT NVARCHAR(MAX) = NULL,
@FILTERID NVARCHAR(MAX) = NULL,
@DATAKEYS NVARCHAR(MAX) = NULL
AS

/*
	THIS STORED PROCEDURE RETURNS THE TOP 100 CONTACTS BASED ON CONTACT ORDER
	AS MATTERSPHERE HAS NOT CONCEPT OF CONTACTS BEING OWNED BY A FEE EARNER AS SUCH
	@SEARCH_TEXT PARAMETER PERFORMS A LIKE SEARCH ON THE CONTACT NAME ONLY
*/

--RESET FEEUSRID BASED ON FILTERS
IF @FILTERID = 'ALL'
BEGIN
	PRINT 'RESETTING @FEEUSRID'
	SET @FEEUSRID = NULL
END 

IF @DATAKEYS IS NULL 
BEGIN
	SELECT 
	TOP 100 
	DBCONTACT.CONTID
	, DBCONTACT.CONTNAME 
	, DBO.GETADDRESS ( DBCONTACT.CONTDEFAULTADDRESS  , ', ' , @UI ) AS ADDRESS
	, DBCONTACT.CREATED 
	, ( SELECT TOP 1 CONTNUMBER  FROM DBCONTACTNUMBERS WHERE CONTCODE='TELEPHONE' AND DBCONTACTNUMBERS.CONTID=DBCONTACT.CONTID AND DBCONTACTNUMBERS.CONTACTIVE =1 ORDER BY CONTORDER ) AS TELEPHONE
	, ( SELECT TOP 1 CONTNUMBER  FROM DBCONTACTNUMBERS WHERE CONTCODE='MOBILE' AND DBCONTACTNUMBERS.CONTID=DBCONTACT.CONTID AND DBCONTACTNUMBERS.CONTACTIVE =1 ORDER BY CONTORDER ) AS MOBILE
	, ( SELECT TOP 1 CONTEMAIL  FROM DBCONTACTEMAILS WHERE  DBCONTACTEMAILS.CONTID=DBCONTACT.CONTID AND DBCONTACTEMAILS.CONTACTIVE =1 ORDER BY CONTORDER ) AS EMAIL
	, ( SELECT COUNT (*) FROM DBASSOCIATES WHERE DBASSOCIATES.CONTID = DBCONTACT.CONTID AND DBASSOCIATES.ASSOCACTIVE = 1 ) AS ASSOCCOUNT
FROM
	DBCONTACT
WHERE
	CONTNAME LIKE '%' + ISNULL ( @SEARCH_TEXT, '' ) + '%'

ORDER BY 
	CONTNAME 
END


IF @DATAKEYS IS NOT NULL 
BEGIN
    SELECT 
	TOP 100
	DBCONTACT.CONTID
	, DBCONTACT.CONTNAME 
	, DBO.GETADDRESS ( DBCONTACT.CONTDEFAULTADDRESS , ', ' , @UI ) AS ADDRESS
	, DBCONTACT.CREATED 
	, ( SELECT TOP 1 CONTNUMBER  FROM DBCONTACTNUMBERS WHERE CONTCODE='TELEPHONE' AND DBCONTACTNUMBERS.CONTID=DBCONTACT.CONTID AND DBCONTACTNUMBERS.CONTACTIVE =1 ORDER BY CONTORDER ) AS TELEPHONE
	, ( SELECT TOP 1 CONTNUMBER  FROM DBCONTACTNUMBERS WHERE CONTCODE='MOBILE' AND DBCONTACTNUMBERS.CONTID=DBCONTACT.CONTID AND DBCONTACTNUMBERS.CONTACTIVE =1 ORDER BY CONTORDER ) AS MOBILE
	, ( SELECT TOP 1 CONTEMAIL  FROM DBCONTACTEMAILS WHERE  DBCONTACTEMAILS.CONTID=DBCONTACT.CONTID AND DBCONTACTEMAILS.CONTACTIVE =1 ORDER BY CONTORDER ) AS EMAIL
	, ( SELECT COUNT (*) FROM DBASSOCIATES WHERE DBASSOCIATES.CONTID = DBCONTACT.CONTID AND DBASSOCIATES.ASSOCACTIVE = 1 ) AS ASSOCCOUNT
FROM
	DBCONTACT
WHERE
	DBCONTACT.CONTID IN ( SELECT VALUE FROM DBO.SPLITSTRING ( @DATAKEYS , ',' ) ) AND
	DBCONTACT.CONTNAME LIKE '%' + ISNULL ( @SEARCH_TEXT, '' ) + '%'
ORDER BY 
	DBCONTACT.CONTNAME 
END 
