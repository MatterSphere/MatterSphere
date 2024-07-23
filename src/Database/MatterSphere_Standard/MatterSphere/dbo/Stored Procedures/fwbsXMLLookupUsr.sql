

CREATE PROCEDURE dbo.fwbsXMLLookupUsr
	@type nvarchar(15)='ROLES', 
	@lookfor nvarchar(15), 
	@included bit = 1

AS
SET NOCOUNT ON
DECLARE @table table ( usrXML xml , usrID int , usrFullName nvarchar(50) )

INSERT @table ( usrXML , usrID , usrFullName )
SELECT Convert(xml , usrXML) , usrID , usrFullName FROM dbo.dbUser WHERE usrActive = 1 AND AccessType = 'INTERNAL'

IF @included = 1
BEGIN
	SELECT usrID , usrFullName , @lookfor as lookfor , @included as included 
	FROM @table CROSS APPLY usrXML.nodes('/config/settings/property') T(ref) 
	WHERE ref.value('@name' , 'nvarchar(100)') = @type AND Charindex(',' + @lookfor + ',' , ',' + ref.value('@value' , 'nvarchar(max)') + ',') > 0
	ORDER BY usrFullName
END
ELSE
BEGIN
	SELECT usrID , usrFullName , @lookfor as lookfor , @included as included 
	FROM @table  X CROSS APPLY usrXML.nodes('/config/settings/property') T(ref) 
	WHERE ref.value('@name' , 'nvarchar(100)') = @type AND Charindex(',' + @lookfor + ',' , ',' + ref.value('@value' , 'nvarchar(max)') + ',') = 0
	UNION SELECT usrID , usrFullName , @lookfor as lookfor , @included as included 
	FROM @table  
	WHERE CharIndex('property name="Roles"', convert(nvarchar(max), usrxml)) = 0
	
	ORDER BY usrFullName
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsXMLLookupUsr] TO [OMSAdminRole]
    AS [dbo];

