

CREATE PROCEDURE [dbo].[fwbsListAssociateContactsMover]
	(
	  @contName nvarchar(128)
	, @contAddress nvarchar(150)
	, @Postcode nvarchar(20)
	, @Contid bigint
	)

AS

DECLARE
	  @SQL nvarchar(2500)
	, @Where nvarchar(1000)
SET @contName = Replace(@contName , '''' , '''''' )
SET @SQL = '

SELECT     C.*, REPLACE(REPLACE(COALESCE (ADR.addLine1, '''') + '', '' + COALESCE (ADR.addLine2, '''') + '', '' + COALESCE (ADR.addLine3, '''') 
                      + '', '' + COALESCE (ADR.addLine4, '''') + '', '' + COALESCE (ADR.addLine5, '''') + '', '' + COALESCE (ADR.addPostcode, ''''), '', , '', '', ''), '', , '', '', '') 
                      AS ConcatAddress,
                          (SELECT     (COUNT(contID))
                            FROM          dbassociates
                            WHERE      contID = C.contID) AS assocCount
FROM         dbContact C INNER JOIN
                      dbContactAddresses ON C.contID = dbContactAddresses.contID INNER JOIN
                      dbAddress ADR ON dbContactAddresses.contaddID = ADR.addID '

SET @Where = ' WHERE C.ContID <>  + @ContID '


IF @contName <> ''
BEGIN
	SET @Where  = @Where + ' AND C.contName Like ''%' + @contName + '%'''
END 


IF @contAddress <> ''
BEGIN 
	SET @Where = @Where + ' AND  REPLACE(REPLACE(COALESCE (ADR.addLine1, '''') + '', '' + COALESCE (ADR.addLine2, '''') + '', '' + COALESCE (ADR.addLine3, '''')  + '', '' + COALESCE (ADR.addLine4, '''') + '', '' + COALESCE (ADR.addLine5, '''') + '', '' + COALESCE (ADR.addPostcode, ''''), '', , '', '', ''), '', , '', '', '') like ''%'' + @contAddress + ''%'''
END


IF @Postcode <> ''
BEGIN
		SET @Where = ' AND  COALESCE(ADR.addPostcode,'''') Like ''%'' + @Postcode + ''%'''
END

SET @SQL = @SQL + @Where + ' ORDER BY C.contName '
--debug
--print @sql


EXEC sp_executesql @SQL , N'@contName nvarchar(128)	, @contAddress nvarchar(150) , @Postcode nvarchar(20) , @contID bigint' , @contName , @contAddress , @Postcode , @contID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsListAssociateContactsMover] TO [OMSAdminRole]
    AS [dbo];

