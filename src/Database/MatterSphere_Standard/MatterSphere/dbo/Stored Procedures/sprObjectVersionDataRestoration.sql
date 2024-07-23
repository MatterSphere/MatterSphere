

-- =============================================
-- Author:		Renato Nappo
-- Create date: 06.06.16
-- Description:	Execute the SQL statements passed to rollback/restore object version data
-- =============================================
CREATE PROCEDURE [dbo].[sprObjectVersionDataRestoration] 
(
	@versiondataquerytable querystringtable readonly,
	@result int OUTPUT
)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @trancount int;
	set @trancount = @@TRANCOUNT;
	DECLARE @strSQL nvarchar(max);


	begin try

		DECLARE sql_cursor CURSOR
			FOR select v.cmd from @versiondataquerytable v

		if @trancount = 0
			begin transaction

			open sql_cursor
			fetch next from sql_cursor into @strSQL
			while @@FETCH_STATUS = 0
			begin
				exec (@strSQL)
				fetch next from sql_cursor into @strSQL
			end
	
			if @trancount = 0
				commit transaction;
				close sql_cursor;
				deallocate sql_cursor;
				select @result = 1;

	end try
	
	begin catch	

		declare @error int, @message varchar(4000), @xstate int;
		select @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();
		if @xstate = -1
			rollback transaction;
		if @xstate = 1 and @trancount = 0
			rollback transaction;
		if @xstate = 1 and @trancount > 0
			rollback transaction sprObjectVersionDataRestoration;		
		print @error
		select @result = 0;

	end catch

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprObjectVersionDataRestoration] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprObjectVersionDataRestoration] TO [OMSAdminRole]
    AS [dbo];

