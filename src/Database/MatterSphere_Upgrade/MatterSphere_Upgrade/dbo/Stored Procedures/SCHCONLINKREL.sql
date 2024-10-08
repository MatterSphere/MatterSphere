﻿CREATE PROCEDURE dbo.SCHCONLINKREL
(
	@CONTLINKID BIGINT
	, @ORDERBY NVARCHAR(MAX) = NULL
)  
AS
SET TRAN ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT ON

DECLARE @Select NVARCHAR(MAX)

SET @Select = N'
WITH Res AS (
SELECT
	C.contName
	, CL.*
FROM dbo.dbContactLinks CL
	INNER JOIN dbContact C ON C.contID = CL.contLinkID
WHERE CL.contID = @CONTLINKID
)
SELECT *
FROM Res
'
IF @ORDERBY IS NULL
	SET  @Select =  @Select + N'ORDER BY contLinkID'
ELSE 
	IF @ORDERBY NOT LIKE '%contLinkID%'
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY  + N', contLinkID'
	ELSE 
		SET  @Select =  @Select + N'ORDER BY ' + @ORDERBY
--PRINT @Select
EXEC sp_executesql @Select,  N'@CONTLINKID BIGINT', @CONTLINKID 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCONLINKREL] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SCHCONLINKREL] TO [OMSAdminRole]
    AS [dbo];