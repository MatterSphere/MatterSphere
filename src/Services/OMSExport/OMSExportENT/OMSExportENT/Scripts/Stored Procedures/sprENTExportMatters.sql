/****** Object:  StoredProcedure [dbo].[sprENTExportMatters]    Script Date: 07/27/2012 14:29:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sprENTExportMatters]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sprENTExportMatters]
GO


CREATE PROCEDURE [dbo].[sprENTExportMatters] 
AS
--CHECK IF A CUSTOM VERSION EXISTS - MUST RETURN SAME COLUMNS AS STANDARD VERSION
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sprENTExportMatters_CUSTOM]') AND type in (N'P', N'PC'))
	EXEC [dbo].[sprENTExportMatters_CUSTOM]
ELSE
BEGIN	
	SELECT  
		F.FILEID
		, MAPPEDCLIENT.EXTERNALID			AS MCLIENT
		, (SELECT TOP 1 SPDATA FROM DBSPECIFICDATA WHERE spLookup = '__FDENTFILMODEL')	AS MODEL -- CHECK
		, MAPPEDCLIENT.EXTERNALID + '.' 
			+ REPLICATE ( '0' , 6 -LEN ( F.FILENO ) )
			+ F.FILENO							AS MMATTER
		, MAPPEDCLIENT.EXTERNALID + '.' 
			+ REPLICATE ( '0' , 6 -LEN ( F.FILENO ) )
			+ F.FILENO							AS MNAME
		, LEFT ( REPLACE ( F.FILEDESC , CHAR ( 13 ) + CHAR ( 10 ) , ', ' ) , 60 )				AS MDESC1
		, CASE 
			WHEN LEN ( F.FILEDESC ) > 60 THEN SUBSTRING ( REPLACE ( F.FILEDESC , CHAR ( 13 ) + CHAR ( 10 ) , ', ' ) , 61 , 60 )	
			ELSE ' '
		END																						AS MDESC2		
		, CASE 
			WHEN LEN ( F.FILEDESC ) > 120 THEN SUBSTRING ( REPLACE ( F.FILEDESC , CHAR ( 13 ) + CHAR ( 10 ) , ', ' ) , 121 , 60 )	
			ELSE ' '
		END																						AS MDESC3
		, 'PE'													AS MSTATUS			-- 'PENDING' (CANNOT RECORD TIME, UNTIL STATUS CHANGE IN ENTERPRISE)
		--, 'IP'													AS MSTATUS			-- 'IN PROGRESS' (THIS WILL ALLOW TIME)
		, B.BRCODE						                        AS MLOC		
		, MAPPEDRESPONSIBLE.EXTERNALID							AS MORGATY
		, MAPPEDPRINCIPLE.EXTERNALID							AS MBILLATY
		, ISNULL ( MAPPEDMANAGER.EXTERNALID , MAPPEDRESPONSIBLE.EXTERNALID )	AS MSUPATY
		, DEPT.DEPTACCCODE										AS MDEPT
		, FTYPE.FILEACCCODE										AS MPRAC
		--UDF'S GO BELOW - ALWAYS PREFIX WITH UDF_ AND ENSURE VALIDATION RULES ARE KNOWN
		
	FROM         
		DBO.DBCLIENT CL 
	INNER JOIN 
		DBO.DBFILE F ON CL.CLID = F.CLID
	INNER JOIN 
		DBFUNDTYPE FUNDTYPE ON FUNDTYPE.FTCODE = F.FILEFUNDCODE AND FUNDTYPE.FTCURISOCODE = F.FILECURISOCODE
	INNER JOIN 
		DBDEPARTMENT DEPT ON F.FILEDEPARTMENT = DEPT.DEPTCODE
	LEFT JOIN 
		DBUSER UO ON F.CREATEDBY = UO.USRID
	LEFT JOIN 
		DBUSER UM ON F.FILEMANAGERID = UM.USRID
	LEFT JOIN 
		DBUSER UP ON F.FILEPRINCIPLEID = UP.USRID
	LEFT JOIN 
		DBUSER UR ON F.FILERESPONSIBLEID = UR.USRID JOIN DBBRANCH B ON B.BRID = F.BRID
	LEFT JOIN 
		DBFILETYPE FTYPE ON FTYPE.TYPECODE = F.FILETYPE
	INNER JOIN --INNER JOIN SO WE KNOW ITS BEEN POPULATED
		DBO.fdGetExternalID ( 'ENTERPRISE' , 'CLIENT' ) MAPPEDCLIENT ON MAPPEDCLIENT.INTERNALID = CONVERT ( NVARCHAR ( 50 ) , F.CLID )
	INNER JOIN --INNER JOIN SO WE KNOW ITS BEEN POPULATED
		DBO.fdGetExternalID ( 'ENTERPRISE' , 'FEE EARNER' ) MAPPEDRESPONSIBLE ON MAPPEDRESPONSIBLE.INTERNALID = CONVERT ( NVARCHAR ( 50 ) , F.FILERESPONSIBLEID)
	INNER JOIN --INNER JOIN SO WE KNOW ITS BEEN POPULATED
		DBO.fdGetExternalID ( 'ENTERPRISE' , 'FEE EARNER' ) MAPPEDPRINCIPLE ON MAPPEDPRINCIPLE.INTERNALID = CONVERT ( NVARCHAR ( 50 ) , F.FILEPRINCIPLEID)
	LEFT JOIN
		DBO.fdGetExternalID ( 'ENTERPRISE' , 'FEE EARNER' ) MAPPEDMANAGER ON MAPPEDMANAGER.INTERNALID = CONVERT ( NVARCHAR ( 50 ) , F.FILEMANAGERID)
	LEFT OUTER JOIN --BUT NO MAPPING FOR THE FILE
		DBO.fdGetExternalID ( 'ENTERPRISE' , 'FILE' ) MAPPEDFILE ON MAPPEDFILE.INTERNALID = CONVERT ( NVARCHAR ( 50 ) , F.FILEID )
	WHERE 
		F.FILENEEDEXPORT = 1 AND 
		MAPPEDFILE.EXTERNALID IS NULL AND 
		MAPPEDCLIENT.EXTERNALID IS NOT NULL
END 
GO


