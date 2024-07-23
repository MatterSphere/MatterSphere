CREATE PROCEDURE [dbo].[sprGetMatterFoldersTree]( @Id BIGINT, @UI uUICultureInfo = N'en-gb')
AS
SET NOCOUNT ON
EXEC [dbo].[sprGetMatterFoldersTreeData] @Id, @UI