-- =============================================
-- Author:		Renato Nappo
-- Create date: 12 August 2019
-- Description:	Return the milestone plan code(s) for a file management application (taskflow)
-- =============================================
CREATE PROCEDURE [dbo].[sprGetFMMilestonePlans]
	@FMCode nvarchar(15)
AS
BEGIN

	SET NOCOUNT ON;

	declare @appxml xml
	set @appxml = (select AppXML from dbFileManagementApplication where appCode = @FMcode)

	declare @plans table
	(
		planCode nvarchar(15),
		planName nvarchar(100)
	)

	insert into @plans (planCode)
	SELECT distinct mt.mp.value('@plan', 'nvarchar(15)') FROM @appxml.nodes('Config/FileManagementApplication/MilestoneTasks/MilestoneTask') mt(mp)
	update @plans
	set planName = (select MSDescription from dbMSConfig_OMS2K where MSCode = planCode)

	select * from @plans

END
GO


