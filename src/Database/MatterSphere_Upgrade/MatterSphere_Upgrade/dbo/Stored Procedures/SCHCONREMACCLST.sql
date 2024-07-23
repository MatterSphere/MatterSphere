CREATE PROCEDURE dbo.SCHCONREMACCLST
(
	@CONTID BIGINT
	, @ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @Select NVARCHAR(MAX)

SET @Select = N'
WITH Res AS (
SELECT
	IFP.contID
	, IFP.clID
	, IFP.fileID
	, dbo.GetFileRef(CL.clNo, F.fileNo) AS Ref
	, CL.clName
	, F.fileDesc
	, IP.proUserName
	, IP.proEmail
FROM dbo.dbInteractiveFileProfile IFP
	INNER JOIN dbo.dbInteractiveProfile IP ON IP.contID = IFP.contID
	INNER JOIN dbFile F ON IFP.fileID = F.fileID
	INNER JOIN dbClient CL ON CL.clID = IFP.clID
WHERE IFP.contID = @CONTID and
	F.filestatus like ''%LIVE%'' and 
	F.fileallowexternal = 1
)
SELECT 
	contID
	, clID
	, fileID
	, Ref
	, clName
	, fileDesc
	, proUserName
	, proEmail
FROM Res
'
IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY clID'
ELSE 
	IF @ORDERBY NOT LIKE '%clID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', clID'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY
--PRINT @Select
EXEC sp_executesql @Select,  N'@CONTID BIGINT', @CONTID 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCONREMACCLST] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCONREMACCLST] TO [OMSAdminRole]
    AS [dbo];
