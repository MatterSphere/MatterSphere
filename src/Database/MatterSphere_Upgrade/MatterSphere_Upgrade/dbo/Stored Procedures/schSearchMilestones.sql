

CREATE PROCEDURE [dbo].[schSearchMilestones] 
@UI uUICultureInfo = '{default}' , 
	@MAX_RECORDS int = 0 ,  
	@FEEUSRID bigint , 
	@FILESTATUS uCodeLookup = null ,  
	@DEPT uCodeLookup = null , 
	@FILETYPE uCodeLookup = null , 
	@DATERANGE uCodeLookup ,
	@currentUTCtime datetime ,
	@currentLocalTime datetime 
	, @ORDERBY NVARCHAR(MAX) = NULL

AS

DECLARE @Select nvarchar(3000) , @Top nvarchar(10) , @Where nvarchar(1000) , @sql nvarchar(4000) , @timeOffset smallint

if @MAX_RECORDS > 0
	set @Top = N'TOP ' + Convert(nvarchar, @MAX_RECORDS)
else
	set @Top = N''

SET @timeOffSet = DateDiff ( hour , @currentUTCtime , @currentLocalTime )

SET @select = N'SET NOCOUNT ON SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
WITH Res AS(
SELECT  
		F.fileID , 
		C.clNo , 
		C.clName , 
		F.fileNo , 
		F.fileDesc, 
	    MD.MSNextDueDate , 
		C.clNo + ''/'' + F.fileNo AS clfileno , 
		F.filePrincipleID, 
		U.usrInits , 
		U.usrFullName ,
        X.cdDesc AS filestatusdesc , 
		Y.cdDesc AS filetypedesc , 
		Z.cdDesc AS filedeptdesc ,
		MC.msdescription,
		MSSTGDESC = CASE MD.msnextduestage
		   WHEN 1 then MC.MSStage1desc
		   WHEN 2 then MC.MSStage2desc
		   WHEN 3 then MC.MSStage3desc
		   WHEN 4 then MC.MSStage4desc
		   WHEN 5 then MC.MSStage5desc
		   WHEN 6 then MC.MSStage6desc
		   WHEN 7 then MC.MSStage7desc
		   WHEN 8 then MC.MSStage8desc
		   WHEN 9 then MC.MSStage9desc
		   WHEN 10 then MC.MSStage10desc
		   WHEN 11 then MC.MSStage11desc
		   WHEN 12 then MC.MSStage12desc
		   WHEN 13 then MC.MSStage13desc
		   WHEN 14 then MC.MSStage14desc
		   WHEN 15 then MC.MSStage15desc
		   WHEN 16 then MC.MSStage16desc
		   WHEN 17 then MC.MSStage17desc
		   WHEN 18 then MC.MSStage18desc
		   WHEN 19 then MC.MSStage19desc
		   WHEN 20 then MC.MSStage20desc
		   WHEN 21 then MC.MSStage21desc
		   WHEN 22 then MC.MSStage22desc
		   WHEN 23 then MC.MSStage23desc
		   WHEN 24 then MC.MSStage24desc
		   WHEN 25 then MC.MSStage25desc
		   WHEN 26 then MC.MSStage26desc
		   WHEN 27 then MC.MSStage27desc
		   WHEN 28 then MC.MSStage28desc
		   WHEN 29 then MC.MSStage29desc
		   WHEN 30 then MC.MSStage30desc
		END 
FROM         
	dbo.dbClient C
INNER JOIN
	dbo.dbFile F ON C.clID = F.clID 
INNER JOIN
    dbo.dbMSData_OMS2K MD ON F.fileID = MD.fileID 
INNER JOIN
    dbo.dbmsConfig_oms2k MC ON MD.mscode = MC.mscode 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID
LEFT JOIN
	dbo.GetCodeLookupDescription ( ''FILESTATUS'' , @UI ) X ON X.cdCode = F.fileStatus
LEFT JOIN
	dbo.GetCodeLookupDescription ( ''FILETYPE'' , @UI ) Y ON Y.cdCode = F.fileType
LEFT JOIN
	dbo.GetCodeLookupDescription ( ''DEPT'' , @UI ) Z ON Z.cdCode = F.fileDepartment'


SET @where = ''

IF @fileStatus IS NOT NULL
	SET @where = @where + ' AND F.fileStatus = @fileStatus'

IF @feeUsrID IS NOT NULL
	SET @where = @where + ' AND U.usrID = @feeUsrID'

IF @dept IS NOT NULL
	SET @where = @where + ' AND F.fileDepartment = @dept'

IF @fileType IS NOT NULL
	SET @where = @where + ' AND F.fileType = @fileType'

SELECT @where = CASE 
	WHEN @dateRange = 'WITHIN7' THEN @where + ' AND ( DateAdd ( hour , @timeOffSet , MD.MSNextDueDate ) >=  Dateadd ( day , Datediff ( day , 0 , @currentLocaltime ) , 0 ) AND DateAdd ( hour , @timeOffSet , MD.MSNextDueDate ) <  Dateadd ( day , Datediff ( day , 0 , @currentLocaltime ) , 8 )) '
	WHEN @dateRange = 'OVER7' THEN @where + ' AND DateAdd ( day , 7 , ( DateAdd ( hour , @timeOffSet , MD.MSNextDueDate ) ) ) <  @currentLocaltime '
	WHEN @dateRange = 'OD' THEN @where + ' AND DateAdd ( hour , @timeOffSet , MD.MSNextDueDate ) <= Dateadd ( second , -1 , Dateadd ( day , Datediff ( day , 0 , @currentLocaltime ) , 1 ))'
	ELSE @where
END
	
IF @where <> ''
	SET @where = ' WHERE ' + Substring ( @where , 6 , 2995 )

SET @select = @select + @where

SET @select = @select + N'
)
SELECT *
FROM Res
'

IF @ORDERBY IS NULL
	SET  @select =  @select + N'ORDER BY MSNextDueDate'
ELSE 
	IF @ORDERBY NOT LIKE '%MSNextDueDate%'
		SET  @select =  @select + N'ORDER BY ' + @ORDERBY  + N', MSNextDueDate'
	ELSE 
		SET  @select =  @select + N'ORDER BY ' + @ORDERBY


PRINT @select

EXEC sp_executesql @select ,  
	N'@UI uUICultureInfo , 
	@MAX_RECORDS int , 
	@FEEUSRID bigint , 
	@FILESTATUS uCodeLookup , 
	@DEPT uCodeLookup , 
	@FILETYPE uCodeLookup , 
	@DATERANGE uCodeLookup ,
	@currentUTCtime datetime ,
	@currentLocalTime datetime ,
	@timeOffSet smallint' , 
	@UI , 
	@MAX_RECORDS , 
	@FEEUSRID , 
	@FILESTATUS , 
	@DEPT , 
	@FILETYPE , 
	@DATERANGE ,
	@currentUTCtime ,
	@currentLocalTime ,
	@timeOffSet

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchMilestones] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchMilestones] TO [OMSAdminRole]
    AS [dbo];

