/****** Object:  StoredProcedure [dbo].[sprENTUpdateClients]    Script Date: 07/27/2012 14:31:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sprENTUpdateClients]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sprENTUpdateClients]
GO


CREATE PROCEDURE [dbo].[sprENTUpdateClients]
AS

--CHECK IF A CUSTOM VERSION EXISTS - MUST RETURN SAME COLUMNS AS STANDARD VERSION
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sprENTUpdateClients_CUSTOM]') AND type in (N'P', N'PC'))
	EXEC [dbo].[sprENTUpdateClients_CUSTOM]
ELSE
BEGIN
	SELECT 
		CLID
		--ENSURE THESE COLUMN NAMES MAP TO ENTERPRISE CLIENTLOADSERVICE PROPERTIES
		, MAPPEDCLIENT.EXTERNALID			AS CLNUM
		, LEFT ( CLNAME , 60 ) 				AS CLNAME1
		, CASE
			WHEN LEN ( CLNAME ) > 60 THEN SUBSTRING ( CLNAME , 61 , 20 )
			ELSE ' '
		END									AS CLNAME2
		, LEFT ( CLNAME , 60 )				AS CLADDR1
		, ISNULL ( CLIADD.ADDLINE1 , CONTADD.ADDLINE1 )	AS CLADDR2
		, ISNULL ( CLIADD.ADDLINE2 , CONTADD.ADDLINE2 )	AS CLADDR3
		, ISNULL ( CLIADD.ADDLINE3 , CONTADD.ADDLINE3 )	AS CLADDR4
		, ISNULL ( CLIADD.ADDLINE4 , CONTADD.ADDLINE4 )	AS CLADDR5
		, LTRIM ( COALESCE ( CLIADD.ADDLINE5 , CONTADD.ADDLINE5 , '' ) + ' ' + COALESCE ( CLIADD.ADDPOSTCODE , CONTADD.ADDPOSTCODE , '' ) ) AS CLADDR6
		, ISNULL ( CLIADD.ADDPOSTCODE , CONTADD.ADDPOSTCODE )	AS CLZIP
		, DBO.GETCODELOOKUPDESC('COUNTRIES', ISNULL(CLICTRY.CTRYCODE, CONTCTRY.CTRYCODE), '') AS CLCOUNTRY
		,(SELECT TOP 1 CONTNUMBER FROM DBCONTACTNUMBERS WHERE CONTID = CL.CLDEFAULTCONTACT AND CONTCODE = 'TELEPHONE' AND CONTACTIVE = 1 ORDER BY CONTORDER) AS CLPHONE
		, CLNAME						AS CLSORT
		,(SELECT TOP 1 CONTNUMBER FROM DBCONTACTNUMBERS WHERE CONTID = CL.CLDEFAULTCONTACT AND CONTCODE = 'FAX' AND CONTACTIVE = 1 ORDER BY CONTORDER) AS CLFAX
		, MAPPEDUSER.EXTERNALID AS CORGATY
		, CASE CLTYPECODE --NO MAPPING FOR THIS CURRENTLY - ALWAYS CUSTOMISED?
			WHEN '1' THEN 'P'
			ELSE 'CO'
		END								AS CBUSTYPE
	FROM         
		DBO.DBCLIENT CL 
	LEFT JOIN 
		DBO.DBADDRESS CLIADD ON CL.CLDEFAULTADDRESS = CLIADD.ADDID
	LEFT JOIN 
		DBCOUNTRY CLICTRY ON CLIADD.ADDCOUNTRY = CLICTRY.CTRYID  
	JOIN 
		DBCONTACT CONT ON CONT.CONTID = CL.CLDEFAULTCONTACT
	JOIN 
		DBADDRESS CONTADD ON CONTADD.ADDID = CONT.CONTDEFAULTADDRESS
	LEFT JOIN 
		DBCOUNTRY CONTCTRY ON CONTADD.ADDCOUNTRY = CONTCTRY.CTRYID
	JOIN 
		DBBRANCH BR ON BR.BRID = CL.BRID
	JOIN 
		DBUSER FEEUSR ON FEEUSR.USRID = CL.FEEUSRID
	JOIN 
		DBUSER USR ON USR.USRID = CL.CREATEDBY
	LEFT JOIN 
		DBCONTACTINDIVIDUAL CONTIND ON CONTIND.CONTID = CL.CLDEFAULTCONTACT
	INNER JOIN 
		DBFEEEARNER F ON F.FEEUSRID = CL.FEEUSRID
	LEFT JOIN 
		DBDEPARTMENT DEPTFEE ON DEPTFEE.DEPTCODE = F.FEEDEPARTMENT
	INNER JOIN 
		DBO.fdGetExternalID ( 'ENTERPRISE' , 'FEE EARNER' ) MAPPEDUSER ON MAPPEDUSER.INTERNALID = CONVERT ( NVARCHAR ( 50 ) , CL.FEEUSRID)
	INNER JOIN 
		DBO.fdGetExternalID ( 'ENTERPRISE' , 'CLIENT' ) MAPPEDCLIENT ON MAPPEDCLIENT.INTERNALID = CONVERT ( NVARCHAR ( 50 ) , CL.CLID )
	WHERE 
		-- only include clients that need exporting
		CL.CLNEEDEXPORT = 1
END
GO


