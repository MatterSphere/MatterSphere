
/*
	* SON_DB ASSUMED SO CHANGE IF NECESSARY
	* DOES NOT DROP AND RE-CREATE USER SETTING - CUSTOMER MAY CHANGE PASSWORD AND CUSTOMISE CODE CALLING IT
		o GRANT ACCESS ON CUSTOMISED STORED PROCS
	* TESTED ON SQL 2008, AND v3.9 ENTERPRISE

	1. DROP STORED PROC MatterSphereCheckMatterForTime IF IT EXISTS
	2. ADD STORED MatterSphereCheckMatterForTime PROC TO ENTERPRISE DATABASE 
	3. DROP STORED PROC MatterSphereGetTimeKeepers IF IT EXISTS
	4. ADD STORED MatterSphereGetTimeKeepers PROC TO ENTERPRISE DATABASE 
	2. ADD MatterSphereAccess LOG IN TO SERVER
	3. ADD MatterSphereAccess USER TO ENTERPRISE DATABASE
	4. GIVE EXECUTE PERMISSIONS 
*/



-- * CREATE THE MATTERSPHERE TIME CAPTURE PROC
USE son_db
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MatterSphereCheckMatterForTime]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[MatterSphereCheckMatterForTime]
GO

CREATE PROC MatterSphereCheckMatterForTime ( @MMATTER VARCHAR(15) , @TIMERECDATE DATETIME )
AS
	/*
		DESIGNED TO BE CALLED BY MATTERSPHERE TIME CAPTURE TO QUERY DATA FOR VALIDATION FOR TIME CARD
		
		2012-JUN-01 - GARETH MEECH, CONRAD MCLAUGHLIN, DAVID PRENTICE, CLARE DENBY
		
		V1
	*/
	
	--CHECK IF A CUSTOM VERSION EXISTS
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MatterSphereCheckMatterForTime_CUSTOM]') AND type in (N'P', N'PC'))
	EXEC [dbo].[MatterSphereCheckMatterForTime_CUSTOM] @MMATTER, @TIMERECDATE
ELSE	
BEGIN

		-- [0] Matter Header
		SELECT 
			  MMATTER
			  ,MACTGROUP
			  ,MOPENDT 
			  ,MSTATUS
			  ,MTIMEUNIT
		FROM 
			  MATTER WITH ( NOLOCK )
		WHERE 
			  MMATTER = @MMATTER

		-- [1] Activities
		SELECT DISTINCT
			  D.ACTCODE
			  , A.ACTNAME
		FROM 
			  MATTER M WITH ( NOLOCK )
			  LEFT JOIN ACTGROUPHEAD H ON H.ACTGROUP = ISNULL(M.MACTGROUP, H.ACTGROUP)
			  INNER JOIN ACTGROUPDET D ON D.ACTGROUP = ISNULL(M.MACTGROUP, D.ACTGROUP)
			  INNER JOIN ACTCODE A ON A.ACODE = ISNULL(D.ACTCODE, A.ACODE)
		WHERE 
			  MMATTER = @MMATTER
		ORDER BY
			  ACTNAME

		-- [2] UDF Header
		SELECT 
			  MTUDEF2 
			  , MTUDEF3
			  , MTUDEF4
			  , MTUDEF5
			  , CONVERT ( BIT , CASE ISNULL ( MREQUIRE2 , '' ) WHEN 'Y' THEN 1 ELSE 0 END ) MREQUIRE2
			  , CONVERT ( BIT , CASE ISNULL ( MREQUIRE3 , '' ) WHEN 'Y' THEN 1 ELSE 0 END ) MREQUIRE3
			  , CONVERT ( BIT , CASE ISNULL ( MREQUIRE4 , '' ) WHEN 'Y' THEN 1 ELSE 0 END ) MREQUIRE4
			  , CONVERT ( BIT , CASE ISNULL ( MREQUIRE5 , '' ) WHEN 'Y' THEN 1 ELSE 0 END ) MREQUIRE5
		FROM
			  MATTUDF1 WITH ( NOLOCK )
		WHERE
			  MMATTER = @MMATTER AND
			  @TIMERECDATE BETWEEN mtdate1 AND mtdate2 AND --TODO : CHECK FOR UTC
			  MTTYPE = 'T'


		-- [3] UDF Definition 2
		SELECT
			  NULL AS UDVALUE
			  , '(Please Select)' AS UDDESC
			  , NULL AS UDFVLST
		UNION SELECT
			  U.UDVALUE
			  , U.UDDESC
			  , U.UDFVLST
		FROM
			  UDFVLST U WITH ( NOLOCK )
		INNER JOIN 
			  MATTUDF1 M WITH ( NOLOCK ) ON M.MTUDEF2 = U.UDFVLST
		WHERE
			  MMATTER = @MMATTER AND
			  @TIMERECDATE BETWEEN MTDATE1 AND MTDATE2 AND --TODO : CHECK FOR UTC
			  MTTYPE = 'T'
			  AND ISNULL(udsumonly, 'N') = 'N'
		ORDER BY
			  UDFVLST


		-- [4] UDF Definition 3
		SELECT
			  NULL AS UDVALUE
			  , '(Please Select)' AS UDDESC
			  , NULL AS UDFVLST
		UNION SELECT
			  U.UDVALUE
			  , U.UDDESC
			  , U.UDFVLST
		FROM
			  UDFVLST U WITH ( NOLOCK )
		INNER JOIN 
			MATTUDF1 M WITH ( NOLOCK ) ON M.MTUDEF3 = U.UDFVLST
		WHERE
			  MMATTER = @MMATTER AND
			  @TIMERECDATE BETWEEN MTDATE1 AND MTDATE2 AND --TODO : CHECK FOR UTC
			  MTTYPE = 'T'
			  AND ISNULL(udsumonly, 'N') = 'N'
		ORDER BY
			  UDFVLST

		-- [5] UDF Definition 4
		SELECT
			  NULL AS UDVALUE
			  , '(Please Select)' AS UDDESC
			  , NULL AS UDFVLST
		UNION SELECT
			  U.UDVALUE
			  , U.UDDESC
			  , U.UDFVLST
		FROM
			UDFVLST U WITH ( NOLOCK )
		INNER JOIN 
			MATTUDF1 M WITH ( NOLOCK ) ON M.MTUDEF4 = U.UDFVLST
		WHERE
			  MMATTER = @MMATTER AND
			  @TIMERECDATE BETWEEN MTDATE1 AND MTDATE2 AND --TODO : CHECK FOR UTC
			  MTTYPE = 'T'
			  AND ISNULL(udsumonly, 'N') = 'N'
		ORDER BY
			  UDFVLST
		      
		-- [6] UDF Definition 5 
		SELECT
			  NULL AS UDVALUE
			  , '(Please Select)' AS UDDESC
			  , NULL AS UDFVLST
		UNION SELECT
			  U.UDVALUE
			  , U.UDDESC
			  , U.UDFVLST
		FROM
			  UDFVLST U WITH ( NOLOCK )
		INNER JOIN 
			MATTUDF1 M WITH ( NOLOCK ) ON M.MTUDEF5 = U.UDFVLST
		WHERE
			  MMATTER = @MMATTER AND
			  @TIMERECDATE BETWEEN MTDATE1 AND MTDATE2 AND --TODO : CHECK FOR UTC
			  MTTYPE = 'T'
			  AND ISNULL(udsumonly, 'N') = 'N'
		ORDER BY
			  UDFVLST
		      

		-- [7] Time Keepers (needs checking, needs to return all allowed timekeepers if null on matter)
		SELECT 
			  DATA.TKINIT 
			  ,DATA.TKEMDATE
		FROM 
		( 
			  SELECT DISTINCT 
					MWTKINIT AS TKINIT 
					,TKEMDATE
			  FROM 
					MATTWORK M WITH ( NOLOCK )
			  LEFT JOIN 
					TIMEKEEP T WITH ( NOLOCK ) ON T.TKINIT = M.MWTKINIT
			  WHERE 
					MMATTER = @MMATTER AND 
					MWTONLY = 'Y' AND 
					MWTKINIT != '!'
			  UNION SELECT
					TKINIT
					,TKEMDATE
			  FROM
					TIMEKEEP WITH ( NOLOCK )
			  WHERE
					TKTITLE IN ( SELECT MWTITLE FROM MATTWORK WHERE MMATTER = @MMATTER AND MWTONLY = 'Y' AND MWTITLE != '!' )
		) DATA
		INNER JOIN 
			  TIMEKEEP MASTER_TIMEKEEP ON MASTER_TIMEKEEP.TKINIT = DATA.TKINIT
		WHERE
			  TKEFLAG = 'Y'


		-- [8] UDF descriptions
		SELECT 
			  CTTITL1     
			  ,CTTITL2    
			  ,CTTITL3    
			  ,CTTITL4
			  ,CTTITL5
		FROM
			  DBO.CNTLREC WITH ( NOLOCK )


		-- [9] All Time Keepers (check)
		SELECT 
			  TKINIT
			  ,TKEMDATE
		FROM 
			  TIMEKEEP WITH ( NOLOCK )
		WHERE
			  TKEFLAG = 'Y'


		-- [10] Accounting Periods
		SELECT 
			  COUNT(*)
		FROM 
			  PERIODT WITH ( NOLOCK )
		WHERE 
			  ( @TIMERECDATE BETWEEN PEBEDT AND PEENDT AND PETIME = 'OP' )
			  OR    ( PEBEDT >= @TIMERECDATE AND PETIME = 'OP' )
END
GO


-- * CREATE THE GET TIME KEEPERS PROC
USE [son_db]
GO

/****** Object:  StoredProcedure [dbo].[MatterSphereGetTimeKeepers]    Script Date: 08/23/2012 11:33:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MatterSphereGetTimeKeepers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[MatterSphereGetTimeKeepers]
GO

USE [son_db]
GO

CREATE PROC [dbo].[MatterSphereGetTimeKeepers]
AS
	/*
		DESIGNED TO BE CALLED BY MATTERSPHERE FEE EARNER MAPPER UTILITY
		
		2012-AUG-21 - CONRAD MCLAUGHLIN
		
		V1
	*/
	
	--CHECK IF A CUSTOM VERSION EXISTS
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MatterSphereCheckTimeKeepers]') AND type in (N'P', N'PC'))
	EXEC [dbo].[MatterSphereGetTimeKeepers_CUSTOM]
ELSE	
BEGIN
	SELECT
		null as Initials
		,'(Please Select)' as Name
		,null as ModifiedDate
UNION SELECT 
		tkinit 	-- DO NOT CHANGE!
		,ISNULL(tkfirst, '') + ' ' + ISNULL(tklast,'') + ' [' + tkinit + ']'  -- Default display of timekeeper
		,tkmoddate
	FROM   
		[son_db].[dbo].[timekeep]
	ORDER BY 
		Name
END
GO



--* CHECK IF LOGIN EXISTS, AND THEN ADD IF IT DOESNT
USE [master]
GO
IF NOT EXISTS(SELECT * FROM MASTER..SYSLOGINS WHERE NAME = 'MatterSphereAccess')
BEGIN
	EXEC master.dbo.sp_addlogin @loginame = N'MatterSphereAccess', @passwd = N'$MSAccess$', @defdb = N'master'
END
GO

--* CHECK IF USER EXISTS
USE [son_db]
GO
IF NOT EXISTS ( SELECT * FROM SYSUSERS WHERE name = 'MatterSphereAccess' )
CREATE USER [MatterSphereAccess] FOR LOGIN [MatterSphereAccess]
EXEC sp_addrolemember 'db_datareader', 'MatterSphereAccess'
GO

-- * GRANT EXECUTE PERMISSIONS TO USER FOR TIME CAPTURE PROC
USE [son_db]
GO
GRANT EXECUTE ON [MatterSphereCheckMatterForTime] TO MatterSphereAccess

-- * GRANT EXECUTE PERMISSIONS TO USER FOR GET TIME KEEPERS PROC
USE [son_db]
GO
GRANT EXECUTE ON [MatterSphereGetTimeKeepers] TO MatterSphereAccess