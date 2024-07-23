

CREATE PROCEDURE [dbo].[srepOldFileAccCode]
(
@UI uUICultureInfo='{default}',
@Info nvarchar(1)
)

AS 

declare @Select nvarchar(1900)
declare @Where nvarchar(2000)
declare @orderby nvarchar(100)

--- Select Statement for the base Query
set @select = N'
SELECT     
	U.usrInits, 
	U.usrFullName, 
	F.created, 
	CL.clNo, 
	F.fileNo, 
	CL.clName, 
	replace(F.fileDesc, char(13) + char(10), '', '') as fileDesc, 
	CL.clAccCode, 
	F.fileAccCode
FROM         
	dbo.dbClient CL
INNER JOIN
	dbo.dbFile F ON CL.clID = F.clID 
INNER JOIN
	dbo.dbUser U ON F.filePrincipleID = U.usrID'

--- Build Where Clause
SET @WHERE = N''

--0 = All; 1 = Only where Matter Account Code exists; 2 = WHere Client Account Code Exists

SET @WHERE = @WHERE + 
		CASE coalesce(@INFO, '0')	
			WHEN '0' THEN N''
			WHEN '1' THEN N' CL.clacccode <>'''' '
			WHEN '2' THEN N' F.fileacccode <>'''' '
		END 

--- Add Where Clause
if @where <> ''
BEGIN
	set @where = N' WHERE ' + @where
END

--- Build OrderBy Clause
set @orderby = N''

declare @sql nvarchar(4000)

--- Add Statements together
set @sql = ' ' + @select + @where + @orderby

--- Debug Print
---print @sql


exec sp_executesql @sql, N'@UI nvarchar(10)', @UI

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepOldFileAccCode] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[srepOldFileAccCode] TO [OMSAdminRole]
    AS [dbo];

