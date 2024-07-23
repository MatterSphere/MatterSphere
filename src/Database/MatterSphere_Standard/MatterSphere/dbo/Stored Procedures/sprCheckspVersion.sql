

CREATE PROCEDURE [dbo].[sprCheckspVersion] 
	@procedureName nvarchar(200) 
	
AS
DECLARE @retVersion int , @headerText nvarchar(20)

SET @headerText =(SELECT  CONVERT (int , (Substring ( [text] , CharIndex('Procedure Version:' , [text]) +19  , 1))) FROM syscomments C JOIN sysobjects O ON O.[id] = C.[id] 
										WHERE  O.[type] = 'p' AND [colid] = 1 AND O.[name] = @procedureName )

SET @retVersion = convert ( int , (@headerText))

--@retVersion will return 0 if proc exists but there is no version, -1 if it does not exist and the version value is it does exist and has a value
IF @retVersion IS NULL
	BEGIN
		IF EXISTS ( SELECT  O.[id] FROM syscomments C JOIN sysobjects O ON O.[id] = C.[id] WHERE  O.[type] = 'p' AND O.[name] = @procedureName)
			SET @retVersion = 0
		ELSE 
			SET @retVersion = -1
	END
		
SELECT @retVersion

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCheckspVersion] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCheckspVersion] TO [OMSAdminRole]
    AS [dbo];

