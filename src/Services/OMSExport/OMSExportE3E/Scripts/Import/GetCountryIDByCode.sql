
GO

/****** Object:  UserDefinedFunction [dbo].[GetCountryIDByCode]    Script Date: 05/31/2013 14:07:53 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetCountryIDByCode]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[GetCountryIDByCode]
GO


GO

/****** Object:  UserDefinedFunction [dbo].[GetCountryIDByCode]    Script Date: 05/31/2013 14:07:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetCountryIDByCode]  (@Type uCodeLookup = 'COUNTRIES', @Code nvarchar(15),  @UI uUICultureInfo = '{default}')  
RETURNS int AS  
BEGIN 
	declare @ctryID int
		select @ctryID = C.ctryID from dbCountry C JOIN  dbcodelookup CU  on C.ctryCode = CU.cdCode
		where cdtype = @Type and cdCode = @Code and @UI Like cdUICultureInfo + '%'
	
	if (@ctryID = '')
		BEGIN
			SET @ctryID = NULL
		END
	return @ctryID
END

GO