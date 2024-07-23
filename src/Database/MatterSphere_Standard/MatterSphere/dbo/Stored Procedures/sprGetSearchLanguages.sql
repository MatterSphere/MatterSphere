

CREATE PROCEDURE [dbo].[sprGetSearchLanguages](@BRID bigint, @UI nvarchar(15))
AS

DECLARE @SPDATA NVARCHAR ( 255 ) 

if exists(SELECT SPDATA FROM DBSPECIFICDATA DS WHERE SPLOOKUP = 'SEARCHSERVLANG' 
				   AND DS.BRID = @BRID)
 begin
      SELECT  @SPDATA = ';' + SPDATA + ';' 
	     FROM 
			   DBSPECIFICDATA DS
		 WHERE 
		   SPLOOKUP = 'SEARCHSERVLANG' 
		   AND DS.BRID = @BRID
 end

 else

  begin

      SELECT  @SPDATA = ';' + SPDATA + ';' 
	     FROM 
			   DBSPECIFICDATA DS
		 WHERE 
		   SPLOOKUP = 'SEARCHSERVLANG' 
		   AND DS.BRID = 0
  end  
	   
	  

SELECT 
       LANGCODE 
       , LU.CDDESC AS LANGUAGEDESCRIPTION
FROM 
       DBLANGUAGE 
INNER JOIN 
       DBO.GETCODELOOKUPDESCRIPTION ( 'LANGUAGE' , @UI ) LU ON LU.CDCODE = DBLANGUAGE.LANGCODE
WHERE 
       CHARINDEX ( ';' + LANGCODE + ';' , @SPDATA ) > 0


SET ANSI_NULLS ON

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprGetSearchLanguages] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprGetSearchLanguages] TO [OMSAdminRole]
    AS [dbo];

