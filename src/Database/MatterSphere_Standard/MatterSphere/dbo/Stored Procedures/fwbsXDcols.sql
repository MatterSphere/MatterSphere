

CREATE PROCEDURE [dbo].[fwbsXDcols] @table nvarchar(100)
AS

SELECT 
	CASE WHEN SC.[colorder] = 1 THEN 40 END as imagecol , SO.[name] , SC.[name] , ST.[name] , CASE WHEN  SC.[Xtype] IN ( 231 , 239 ) THEN SC.[length]/2 ELSE SC.[length] END as length  , SC.[isnullable]
FROM
	dbo.[sysobjects] SO 
JOIN 
	dbo.[syscolumns] SC ON SO.[id] = SC.[id]
JOIN
	dbo.[systypes] ST ON SC.[xusertype] = ST.[xusertype]
WHERE 
	SO.[name] = @table

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsXDcols] TO [OMSAdminRole]
    AS [dbo];

