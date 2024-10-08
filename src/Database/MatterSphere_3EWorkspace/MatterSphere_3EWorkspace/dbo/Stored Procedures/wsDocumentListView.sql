﻿CREATE PROCEDURE [dbo].[wsDocumentListView]
@FEEUSRID bigint = null,
@CLID bigint = null, 
@FILEID bigint = null,
@SEARCH_TEXT nvarchar(MAX) = null,
@FILTERID nvarchar(MAX) = null,
@DATAKEYS nvarchar(MAX) = null
AS


PRINT '@DATAKEYS'
PRINT @DATAKEYS
PRINT '@SEARCH_TEXT'
PRINT @SEARCH_TEXT
PRINT '@FEEUSRID'
PRINT @FEEUSRID
PRINT '@FILTERID'
PRINT @FILTERID

IF @CLID IS NOT NULL
BEGIN
	PRINT 'SETTING @FEEUSRID = NULL AS @CLID IS POPULATED'
	SET @FEEUSRID = NULL
END 
IF @FILEID IS NOT NULL
BEGIN
	PRINT 'SETTING @FEEUSRID = NULL AS @FILEID IS POPULATED'
	SET @FEEUSRID = NULL
END 


IF @FEEUSRID IS NULL
BEGIN

SELECT    		
	DBO.DBFILE.FILENO 
	, DBO.DBFILE.FILEDESC
	, DBO.DBDOCUMENT.DOCID
	, DBO.DBDOCUMENT.DOCDESC		
	, COALESCE(DBO.DBDOCUMENT.DOCAUTHORED, DBO.DBDOCUMENT.CREATED) AS CREATED
	, AUTH.USRFULLNAME AS AUTHOREDBY
	, REPLACE ( DBO.DBDOCUMENT.DOCEXTENSION , '.' , '' ) AS DOCEXTENSION
	, CREATEDBY.USRFULLNAME AS CREATEDBY
FROM         
	DBO.DBDOCUMENT WITH ( NOLOCK ) 
INNER JOIN
	DBO.DBFILE ON DBO.DBDOCUMENT.FILEID = DBO.DBFILE.FILEID 
INNER JOIN 
	DBO.DBUSER CREATEDBY ON DBDOCUMENT.CREATEDBY = CREATEDBY.USRID
LEFT OUTER JOIN
	DBO.DBUSER AUTH ON DBO.DBDOCUMENT.DOCAUTHOREDBY = AUTH.USRID
WHERE 
	DBO.DBDOCUMENT.CLID = ISNULL ( @CLID , DBO.DBDOCUMENT.CLID ) AND 
	DBO.DBDOCUMENT.FILEID = ISNULL ( @FILEID , DBO.DBDOCUMENT.FILEID ) AND 
	DBO.DBDOCUMENT.DOCDELETED = 0 AND
	DBO.DBDOCUMENT.DOCDESC LIKE '%' + ISNULL ( @SEARCH_TEXT , '' ) + '%'
ORDER BY 
	DBO.DBDOCUMENT.UPDATED DESC  
END
ELSE
BEGIN
	
SELECT TOP 20                                   
	DBO.DBDOCUMENT.DOCID
	, DBO.DBDOCUMENT.DOCDESC                      
	, COALESCE(DBO.DBDOCUMENT.DOCAUTHORED, DBO.DBDOCUMENT.CREATED) AS CREATED
	, DBO.DBCLIENT.CLNO
	, DBO.DBFILE.FILENO 
	, DBO.GETFILEREF(DBO.DBCLIENT.CLNO, DBO.DBFILE.FILENO) AS CLIENTFILENO
	, DBO.DBCLIENT.CLNAME
	, DBO.DBFILE.FILEDESC
	, AUTH.USRFULLNAME AS AUTHOREDBY
	, DBO.DBDOCUMENT.DOCEXTENSION
	, CREATEDBY.USRFULLNAME AS CREATEDBY
FROM         
	DBO.DBDOCUMENT 
INNER JOIN
	DBO.DBFILE ON DBO.DBDOCUMENT.FILEID = DBO.DBFILE.FILEID 
INNER JOIN
	DBO.DBCLIENT ON DBO.DBFILE.CLID = DBO.DBCLIENT.CLID 
INNER JOIN 
	DBO.DBUSER CREATEDBY ON DBDOCUMENT.CREATEDBY = CREATEDBY.USRID
LEFT OUTER JOIN
	DBO.DBUSER AUTH ON DBO.DBDOCUMENT.DOCAUTHOREDBY = AUTH.USRID
WHERE 
	DBO.DBFILE.FILEPRINCIPLEID = @FEEUSRID AND
    DBO.DBDOCUMENT.DOCDELETED = 0 
ORDER BY 
	DBO.DBDOCUMENT.CREATED DESC  


END 