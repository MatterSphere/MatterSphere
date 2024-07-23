-- =============================================
-- Author:		Renato Nappo
-- Create date: 31/10/17
-- Description:	Return the folder list for a given Client - for use in combo boxes (Data Lists)
-- =============================================
CREATE PROCEDURE [dbo].[GetClientDocumentFolderList]
	@UI nvarchar(15) = null,
	@clientID bigint = null
AS
BEGIN

	set nocount on;

	declare @tblCodes table(FolderCode nvarchar(15), FolderGUID uniqueidentifier, FolderDescription nvarchar(100))
	declare @tblTEMP table(FolderGUID uniqueidentifier, FolderDescription nvarchar(255), FolderCode nvarchar(15))

	insert into @tblCodes
		select * from dbo.GetFolderCodesForClientDocumentTree(@clientID, @UI);

	declare @tbllooper table (id int not null identity, FolderDescription nvarchar(100))
	insert into @tbllooper 
		select distinct FolderDescription from @tblCodes

	declare @i int = 1
	declare @numrows int = (select count(*) from @tbllooper)

	declare @str varchar(MAX)
	declare @tblDataList table (FolderGUIDs nvarchar(MAX), FolderDescription nvarchar(100))

	while (@i <= @numrows)
	begin
		SELECT @str= coalesce(@str + '', '', '''') + '''''''' + Convert(varchar(50),a.FolderGUID) + ''''''''
		FROM (SELECT DISTINCT FolderGUID from @tblCodes where FolderDescription = (select FolderDescription from @tbllooper where id = @i)) a

		insert into @tblDataList
			(FolderGUIDs, FolderDescription)
			values 
			(@str, (select FolderDescription from @tbllooper where id = @i))
		
		set @str = null
		set @i = @i + 1
	end

	select null, dbo.getcodelookupdesc('RESOURCE','RESNOTSET', @UI)
	union
	select * from @tblDataList
END

