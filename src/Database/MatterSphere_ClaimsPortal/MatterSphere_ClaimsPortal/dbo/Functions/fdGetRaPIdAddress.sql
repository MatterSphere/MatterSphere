-- =============================================
-- Author:		Peter Barnett
-- Create date: 26-04-2010
-- Description:	Converts RaPId address fields in single address field
-- Package: Used to support RaPId RTA PI portal only
-- =============================================
CREATE FUNCTION [dbo].[fdGetRaPIdAddress] 
(
	-- Add the parameters for the function here
	@houseName nvarchar(50),
	@houseNo nvarchar(10),
	@sStreet1 nvarchar(50),
	@street2 nvarchar(50),
	@district nvarchar(50),
	@city nvarchar(50),
	@county nvarchar(50),
	@postcode nvarchar(50),
	@country nvarchar(50)
	
)
RETURNS nvarchar(500)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @RaPIdAddress nvarchar(500)
	-- Add the T-SQL statements to compute the return value here
	select	@RaPIdAddress = 
		CASE when @houseName is null or @houseName = '' THEN '' Else (@houseName + CHAR(13)) END +	
		CASE When @houseNo is null or @houseNo = '' then '' else (@houseNo + ' ') END + 
		@sStreet1 + CHAR(13) +
		CASE when @street2 is null or @street2 = '' THEN '' else (@street2 + CHAR(13)) END +
		CASE when @district is null or @district = '' then '' else (@district + CHAR(13)) END +
		CASE when @city is null or @city = '' then '' else (@city + CHAR(13))  END +
		CASE when @county is null or @county = '' then '' else (@county + CHAR(13)) END +
		@postcode +
		CASE when @country is null or @country = '' or @country = 'England' then '' else (CHAR(13) + @country) END

	-- Return the result of the function
	RETURN @RaPIdAddress

END