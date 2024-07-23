CREATE PROCEDURE [config].[ListAllowedFileIDs]
       @FileIDs nvarchar(max)
AS
BEGIN	   
       set nocount on;
       select items from dbo.SplitStringToTable(@FileIDs, ',')  r 
       WHERE NOT exists (select 1 from [config].[FileAccess] () DA where DA.FIleID = r.items)

end

