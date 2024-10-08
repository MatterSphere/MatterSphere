CREATE PROCEDURE [dbo].[sprC3EUpdateClients]
AS
--CHECK IF A CUSTOM VERSION EXISTS - MUST RETURN SAME COLUMNS AS STANDARD VERSION
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sprC3EUpdateClients_CUSTOM]') AND type in (N'P', N'PC'))
	EXEC [dbo].[sprC3EUpdateClients_CUSTOM]
ELSE
SELECT 
	DBCLIENT.CLID
	--TODO - Get Entity?  Address?
	, DBCLIENT.CLDEFAULTCONTACT AS DEFAULTCONTACTID
	, DBO.GETE3ECLIENTUPDATEXML ( DBCLIENT.CLID, N'C3E' ) AS CLIENT
	, CONTEXTID AS ENTITYINDEX
	, CONTEXTTXTID AS ENTITYID
	, CLEXTID AS CLIENTINDEX
	, CLEXTTXTID AS CLIENTID
FROM
	DBO.DBCLIENT
INNER JOIN 
	DBCONTACT ON DBCONTACT.CONTID = DBCLIENT.CLDEFAULTCONTACT
WHERE
	CLNEEDEXPORT = 1 AND ISNULL(CLEXTID, 0) <> -1 AND -- NEGATIVE NUMBERS USED TO INDICATE AN ERROR OR PART CREATION
	(CLEXTTXTID IS NOT NULL OR CLEXTID > 0) AND 
	(DBCONTACT.contExtTxtID IS NOT NULL OR DBCONTACT.contExtID > 0)