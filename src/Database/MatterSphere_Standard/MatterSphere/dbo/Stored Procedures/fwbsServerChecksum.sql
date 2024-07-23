

CREATE PROCEDURE [dbo].[fwbsServerChecksum] @ObjectType char (2) 

AS

DECLARE  @ObjectString nvarchar(50)


SET @ObjectString =
	CASE @ObjectType
		WHEN 'P' THEN 'CREATEPROCEDURE'
		WHEN 'FN' THEN 'CREATEFUNCTION'
		WHEN 'V' THEN 'CREATEVIEW'
		WHEN 'TR' THEN 'CREATETRIGGER'
		WHEN 'IF' THEN 'CREATEFUNCTION'
	END

SELECT sc.[id] , so.[name] as Object ,
	CASE 
		WHEN   (SELECT  isnumeriC (substring ([text] , charindex ( 'checksum' , [text] ) + 11 ,  5 )) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') = 0
		THEN 0
		ELSE isnull (  (SELECT  convert ( int , substring ([text] , charindex ( 'checksum' , [text] ) + 11 ,  5)) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') , 0 )
		END  as ObjectHeaderChecksum ,
	1 + sum(	 len ( replace (replace ( replace (replace (coalesce ( Substring ( [text] , 1  , 4000) , ' ' ) , char(9) , '') , char(10) , '') , char(13) , '') , ' ' , '' )  ) -
	charindex (@ObjectString ,  replace (replace ( replace (replace (coalesce ( Substring ( [text] , 1  , 4000) , ' ' ) , char(9) , '') , char(10) , '') , char(13) , '') , ' ' , '' )) ) as ObjectChecksum,
	CASE 
		WHEN   (SELECT  isnumeriC (substring ([text] , charindex ( 'checksum' , [text] ) + 11 ,  5 )) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') = 0
		THEN 12
		WHEN isnull (  (SELECT  convert ( int , substring ([text] , charindex ( 'checksum' , [text] ) + 11 ,  5)) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') , 0 )	= 0
		THEN 12
		WHEN 	(1 + sum (  len ( replace (replace ( replace (replace (coalesce ( Substring ( [text] , 1  , 4000) , ' ' ) , char(9) , '') , char(10) , '') , char(13) , '') , ' ' , '' )  ) -
				 charindex (@ObjectString ,  replace (replace ( replace (replace (coalesce ( Substring ( [text] , 1  , 4000) , ' ' ) , char(9) , '') , char(10) , '') , char(13) , '') , ' ' , '' )) ) -	
				isnull (  (SELECT  convert ( int , substring ([text] , charindex ( 'checksum' , [text] ) + 11 ,  5)) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') , 0 ) )= 0
		THEN 0
		ELSE  14
		END as ImageColumn ,
	CASE 
		WHEN   (SELECT  isnumeric (substring ([text] , charindex ( 'Database Version' , [text] ) + 17 ,  5 )) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') = 0
		THEN '?'
		ELSE isnull (  (SELECT  substring ([text] , charindex ( 'Database Version:'   , [text] ) + 19 , 5 ) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') , '?' )
		END as DatabaseVersion ,
	CASE
		WHEN   (SELECT  isnumeric (substring ([text] , charindex ('Database Version' , [text] ) + 17 ,  5 )) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') = 0
		THEN '?'
		WHEN @ObjectType = 'FN'
		THEN isnull (  (SELECT  substring ([text] , charindex ( 'Function Version:'   , [text] ) + 18 , 2 ) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') , '?' )
		WHEN @ObjectType = 'TR'
		THEN isnull (  (SELECT  substring ([text] , charindex ( 'Trigger Version:'   , [text] ) + 17 , 2 ) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') , '?' )
		WHEN @ObjectType = 'P'
		THEN isnull (  (SELECT  substring ([text] , charindex ( 'Procedure Version:'   , [text] ) + 19 , 2 ) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') , '?' )
		WHEN @ObjectType = 'V'
		THEN isnull (  (SELECT  substring ([text] , charindex ( 'View Version:'   , [text] ) + 14 , 2 ) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') , '?' )
		WHEN @ObjectType = 'IF'
		THEN isnull (  (SELECT  substring ([text] , charindex ( 'Function Version:'   , [text] ) + 18 , 2 ) FROM dbo.[syscomments] WHERE  [id] = SC.[id] and [colid] = 1 and [text ] like '%checksum%') , '?' )
		END as ObjectVersion

FROM
	dbo.[syscomments] SC 
JOIN
	dbo.[sysobjects] SO ON SC.[id] = SO.[id] 
WHERE 
	SO.[xtype] = @ObjectType AND SO.[category] = 0
GROUP BY 
	SO.[name] ,  SC.[id]

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsServerChecksum] TO [OMSAdminRole]
    AS [dbo];

