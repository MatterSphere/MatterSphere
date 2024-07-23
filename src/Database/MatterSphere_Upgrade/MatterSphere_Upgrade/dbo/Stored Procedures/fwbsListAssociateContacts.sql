

CREATE PROCEDURE [dbo].[fwbsListAssociateContacts]
	(
	  @contName nvarchar(128)
	, @contAddress nvarchar(150)
	, @Postcode nvarchar(20)
	)

AS

DECLARE
	  @SQL nvarchar(2500)
	, @Where nvarchar(1000)

SET @contName = Replace(@contName , '''' , '''''' )

SET @SQL = '

SELECT     
		C.contID,C.contName
		, C.contTypeCode
		, REPLACE(REPLACE(COALESCE (ADR.addLine1, '''') + '', '' + COALESCE (ADR.addLine2, '''') + '', '' + COALESCE (ADR.addLine3, '''') 
                      + '', '' + COALESCE (ADR.addLine4, '''') + '', '' + COALESCE (ADR.addLine5, '''') + '', '' + COALESCE (ADR.addPostcode, ''''), '', , '', '', ''), '', , '', '', '') 
                      AS ConcatAddress
        , (SELECT (COUNT(contID)) FROM dbassociates WHERE contID = C.contID) AS assocCount
		, (SELECT (COUNT(contID)) FROM dbAssociates	WHERE contID = C.contID AND assocActive = 0) AS assocInactiveCount
FROM         
		dbContact C 
INNER JOIN
        dbContactAddresses ON C.contID = dbContactAddresses.contID 
        AND contActive = 1 
INNER JOIN
        dbAddress ADR ON dbContactAddresses.contaddID = ADR.addID'

SET @Where = ''


IF @contName <> ''
BEGIN
	SET @Where  = ' C.contName Like ''%' + @contName + '%'''
END 


IF @contAddress <> ''
BEGIN 
IF @Where = ''
	SET @Where = '  REPLACE(REPLACE(COALESCE (ADR.addLine1, '''') + '', '' + COALESCE (ADR.addLine2, '''') + '', '' + COALESCE (ADR.addLine3, '''')  + '', '' + COALESCE (ADR.addLine4, '''') + '', '' + COALESCE (ADR.addLine5, '''') + '', '' + COALESCE (ADR.addPostcode, ''''), '', , '', '', ''), '', , '', '', '') like ''%'' + @contAddress + ''%'''
ELSE
	SET @Where = @Where + ' AND  REPLACE(REPLACE(COALESCE (ADR.addLine1, '''') + '', '' + COALESCE (ADR.addLine2, '''') + '', '' + COALESCE (ADR.addLine3, '''')  + '', '' + COALESCE (ADR.addLine4, '''') + '', '' + COALESCE (ADR.addLine5, '''') + '', '' + COALESCE (ADR.addPostcode, ''''), '', , '', '', ''), '', , '', '', '') like ''%'' + @contAddress + ''%'''
END


IF @Postcode <> ''
BEGIN
	IF @Where = ''
		SET @Where = '  COALESCE(ADR.addPostcode,'''') Like ''%'' + @Postcode + ''%'''
	ELSE
		SET @Where = @Where +' AND  COALESCE(ADR.addPostcode,'''') Like ''%'' + @Postcode + ''%'''
END


IF @Where <> ''
	SET @Where = ' WHERE ' + @Where 

SET @SQL = @SQL + @Where + ' ORDER BY C.contName '


print @SQL


EXEC sp_executesql @SQL , N'@contName nvarchar(128)	, @contAddress nvarchar(150) , @Postcode nvarchar(20)' , @contName , @contAddress , @Postcode

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsListAssociateContacts] TO [OMSAdminRole]
    AS [dbo];

