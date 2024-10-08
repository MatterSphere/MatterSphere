-- =============================================
-- Author:		Renato Nappo
-- Create date: 23 August 2019
-- Description:	Check for duplicate Precedents when saving a new Precedent
-- =============================================
CREATE PROCEDURE [dbo].[sprCheckForDuplicatePrecedent]
(
	@PrecTitle nvarchar(50) = null,
	@PrecType nvarchar(15) = null,
	@PrecLibrary nvarchar(15) = null,
	@PrecCategory nvarchar(15) = null,
	@PrecSubCategory nvarchar(15) = null,
	@PrecMinorCategory nvarchar(15) = null,
	@PrecLanguage nvarchar(15) = null,
	@PrecId bigint = null
)
AS

declare @sql nvarchar(max)

set @sql = N'select * from dbPrecedents where PrecTitle = @PrecTitle and PrecType = @PrecType'

if @PrecLibrary is not null 
	set @sql = @sql + N' and PrecLibrary = @PrecLibrary'
else
	set @sql = @sql + N' and PrecLibrary is null'	

if @PrecCategory is not null 
	set @sql = @sql + N' and PrecCategory = @PrecCategory'	
else
	set @sql = @sql + N' and PrecCategory is null'

if @PrecSubCategory is not null 
	set @sql = @sql + N' and PrecSubCategory = @PrecSubCategory'
else
	set @sql = @sql + N' and PrecSubCategory is null'

if @PrecMinorCategory is not null 
	set @sql = @sql + N' and PrecMinorCategory = @PrecMinorCategory'
else
	set @sql = @sql + N' and PrecMinorCategory is null'

if @PrecLanguage is not null
	set @sql = @sql + N' and PrecLanguage = @PrecLanguage'	
else
	set @sql = @sql + N' and PrecLanguage is null'

if @PrecId is not null
	set @sql = @sql + N' and precID <> @PrecId'


print @sql

exec sp_executesql @sql,  N'
	@PrecTitle nvarchar(50),
	@PrecType nvarchar(15),
	@PrecLibrary nvarchar(15),
	@PrecCategory nvarchar(15),
	@PrecSubCategory nvarchar(15),
	@PrecMinorCategory nvarchar(15),
	@PrecLanguage nvarchar(15),
	@PrecId bigint',

	@PrecTitle,
	@PrecType,
	@PrecLibrary,
	@PrecCategory,
	@PrecSubCategory,
	@PrecMinorCategory,
	@PrecLanguage,
	@PrecId

