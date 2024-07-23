CREATE   FUNCTION [dbo].[GetSpecificData]  (@SpType nvarchar(30))
RETURNS nvarchar(255) AS  
BEGIN 
	declare @spData nvarchar (255)
	-- Check based on Branch of Database
	Select @spData = spData from dbSpecificData where spLookup = @SpType and brid = (Select top 1 BRID from dbRegInfo)
	-- Check for Multi Branch
	if (@spData is null)
		Select @spData = spData from dbSpecificData where spLookup = @SpType and brid = 0 
	if (@SpData is null)
		set @spData = ''
	return @spData
END