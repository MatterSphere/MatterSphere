
/****** Object:  StoredProcedure [dbo].[fwbsAddColumn]    Script Date: 12/06/2011 11:41:19 ******/

CREATE PROCEDURE [dbo].[fwbsAddColumn]
	(  
   	@Tablename nvarchar(100) ,					-- Name of Table to add column to 
	@ColumnName nvarchar(100) ,  				-- Column Name
	@columnDesc nvarchar(300)    					-- Column Description.    i.e. char (10) not null
	)
AS

DECLARE  
	@Script nvarchar(400) , 						-- SQL Script string variable
	@Publication nvarchar(50) , 					-- Name of Publication
	@Version nvarchar(500)						-- SQL Server version string

-- Existence check
IF EXISTS (  SELECT * FROM sys.columns c WHERE c.object_id = object_id(@tablename,'U') AND c.NAME = @ColumnName)
BEGIN
	PRINT + char(13) + char(9) + 'Column ' + @ColumnName + ' already exists in table ' + @Tablename + '. Column names must be unique, terminating procedure.'
	RETURN
END


BEGIN
	BEGIN TRANSACTION
	SET @script = 'ALTER TABLE ' + @tablename + ' ADD ' + @columnname + ' ' + @columndesc
		EXEC sp_executesql @script
	IF @@Error <> 0 	
	BEGIN
		ROLLBACK TRANSACTION
		PRINT + char(13) + char(9) + 'Error creating Column ' + @ColumnName + '. Rolling back transaction and terminating procedure.'
		RETURN
	END
	COMMIT TRANSACTION
	PRINT + char(13) + char(9) + 'Column ' + @ColumnName + ' successfully added to table ' + @Tablename + '.'
	RETURN
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fwbsAddColumn] TO [OMSAdminRole]
    AS [dbo];

