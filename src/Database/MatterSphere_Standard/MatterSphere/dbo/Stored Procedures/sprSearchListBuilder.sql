CREATE PROCEDURE dbo.sprSearchListBuilder (@Code uCodeLookup, @Version BIGINT = 0, @Force BIT = 0, @UI uUICultureInfo = '{default}')  
AS
SET NOCOUNT ON

DECLARE @listview dbo.uXML
	, @curversion BIGINT
	, @X XML
	, @USER NVARCHAR(200)

--If no code is passed get out as we do not want to process the XML		
IF @Code = '' OR @Code IS NULL
	RETURN 0	

SET @USER = config.GetUserLogin() 

-- Retrieve the search list version information.
--select @listview = convert(varchar(8000), schlistview)  from dbsearchlistconfig where schcode = @code
SELECT @curversion = schVersion
	, @listview = schListView 
FROM dbo.dbSearchListConfig 
WHERE schCode = @Code

-- Check the version numbers.
IF (@version < @curversion) OR @force = 1
BEGIN
	--DataTable = SEARCHLIST
	SELECT  SL.*
		, dbo.GetCodeLookupDesc('OMSSEARCH', SL.schcode, @UI) AS schdesc 
	FROM dbo.dbsearchlistconfig SL   
	WHERE schcode = @Code

	SET @X = @listview

	--Data Table = LISTVIEW
	SELECT  
		xmlTable.xmlCol.value('@mappingName', 'nvarchar(50)') AS lvmapping
		, dbo.GetCodeLookupDesc('SLCAPTION',  xmlTable.xmlCol.value('@lookup', 'uCodeLookup'), @UI) AS lvdesc
		, xmlTable.xmlCol.value('@width', 'int') AS lvwidth
		, CAST(CASE xmlTable.xmlCol.value('@sortable', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0 ELSE 1 END AS BIT) AS lvsortable
		, xmlTable.xmlCol.value('@format', 'nvarchar(50)') AS lvformat
		, CAST(CASE xmlTable.xmlCol.value('@incQuickSearch', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0 ELSE 1 END AS BIT) AS lvincquicksearch
		,  dbo.GetCodeLookupDesc('SLCAPTION', xmlTable.xmlCol.value('@nullText', 'nvarchar(15)'), @UI) AS lvnulltext
		, xmlTable.xmlCol.value('@dataListName', 'nvarchar(15)') AS lvdatalistname
		, xmlTable.xmlCol.value('@dataCodeType', 'nvarchar(15)') AS lvdatacodetype
		, xmlTable.xmlCol.value('@conditions', 'nvarchar(500)') AS conditions
		, xmlTable.xmlCol.value('@roles', 'nvarchar(500)') AS roles
		, xmlTable.xmlCol.value('@sourceIs', 'nvarchar(15)') AS sourceIs
		, xmlTable.xmlCol.value('@displayAs', 'nvarchar(15)') AS displayAs
	FROM @X.nodes('/searchList/listView/column') xmlTable(xmlCol)

	--Data Table = BUTTONS
	SELECT  
		xmlTable.xmlCol.value('@name', 'nvarchar(20)') AS btnname
		, dbo.GetCodeLookupDesc('SLBUTTON',  xmlTable.xmlCol.value('@lookup', 'uCodeLookup'), @UI) AS btndesc
		, dbo.GetCodeLookupHelp('SLBUTTON',  xmlTable.xmlCol.value('@lookup', 'uCodeLookup'), @UI) AS btnhelp
		, CAST(CASE xmlTable.xmlCol.value('@visible', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0 ELSE 1 END AS BIT) AS btnvisible
		, xmlTable.xmlCol.value('@mode', 'nvarchar(15)') AS mode
		, xmlTable.xmlCol.value('@parameter', 'nvarchar(1000)') AS parameter
		, xmlTable.xmlCol.value('@buttonStyle', 'nvarchar(15)') AS buttonStyle
		, xmlTable.xmlCol.value('@buttonGlyph', 'nvarchar(10)') AS buttonGlyph
		, CAST(CASE xmlTable.xmlCol.value('@pnlBtnVisible', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0  WHEN '1' THEN 1 ELSE 0 END AS BIT) AS pnlBtnVisible
		, dbo.GetCodeLookupDesc('SLPBUTTON', xmlTable.xmlCol.value('@pnlBtnText', 'nvarchar(15)'), @UI) AS pnlBtnCaptionDesc
		, xmlTable.xmlCol.value('@pnlBtnIndex', 'int') AS pnlBtnIndex
		, CAST(CASE xmlTable.xmlCol.value('@contextMenuVisible', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0  WHEN '1' THEN 1 ELSE 0 END AS BIT) AS contextMenuVisible
		, CAST(CASE xmlTable.xmlCol.value('@enabledWithNoRows', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0  WHEN '1' THEN 1 ELSE 0 END AS BIT) AS enabledWithNoRows
		, xmlTable.xmlCol.value('@conditions', 'nvarchar(500)') AS buttonConditions
		, xmlTable.xmlCol.value('@roles', 'nvarchar(500)') AS roles
		, CAST(CASE xmlTable.xmlCol.value('@enabledMultiSelect', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0  WHEN '1' THEN 1 ELSE 0 END AS BIT) AS enabledMultiSelect
		, xmlTable.xmlCol.value('@parent', 'nvarchar(50)') AS parent
	FROM @X.nodes('searchList/buttons/button') xmlTable(xmlCol)

	--Data Table = LISTVIEW_HEADER
	SELECT  
		CAST(CASE xmlTable.xmlCol.value('@captionVisible', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0 WHEN '1' THEN 1 ELSE 0 END AS BIT) AS lvcaptionvisible
		, CAST(CASE xmlTable.xmlCol.value('@quickSearchVisible', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0 WHEN '1' THEN 1 ELSE 0 END AS BIT) AS lvquicksearchvisible
		, dbo.GetCodeLookupDesc('SLVIEW',  xmlTable.xmlCol.value('@captionLookup', 'uCodeLookup'), @UI) AS lvcaptiondesc
		, xmlTable.xmlCol.value('@doubleClickAction', 'nvarchar(50)') AS lvdblclickaction
		, xmlTable.xmlCol.value('@searchTypeVisible', 'nvarchar(5)') AS lvtypevisible
		, xmlTable.xmlCol.value('@quickSearchPrefix', 'nvarchar(50)') AS lvquicksearchprefix
		, xmlTable.xmlCol.value('@imageIndex', 'int') AS lvimageindex
		, xmlTable.xmlCol.value('@imageColumn', 'nvarchar(50)') AS lvimagecolumn
		, xmlTable.xmlCol.value('@imageResouce', 'nvarchar(50)') AS lvimageresouce
		, xmlTable.xmlCol.value('@rowHeight', 'nvarchar(50)') AS lvrowheight
		, CAST(CASE xmlTable.xmlCol.value('@multiSelect', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0 WHEN '1' THEN 1 ELSE 0 END AS BIT) AS lvmultiselect
		, xmlTable.xmlCol.value('@helpFileName', 'nvarchar(500)') AS lvhelpfilename
		, xmlTable.xmlCol.value('@helpKeyword', 'nvarchar(500)') AS lvhelpkeyword
		, xmlTable.xmlCol.value('@saveSearch', 'nvarchar(15)') AS lvsaveSearch
	FROM @X.nodes('searchList/listView') xmlTable(xmlCol)

--User customization
	SET @X = (SELECT CAST(schListView AS XML) FROM dbo.dbUserSearchListColumns WHERE NTLogin = @USER AND schcode = @Code)

	SELECT  
		xmlTable.xmlCol.value('@mappingName', 'nvarchar(50)') AS lvmapping
		, dbo.GetCodeLookupDesc('SLCAPTION',  xmlTable.xmlCol.value('@lookup', 'uCodeLookup'), @UI) AS lvdesc
		, xmlTable.xmlCol.value('@width', 'int') AS lvwidth
		, CAST(CASE xmlTable.xmlCol.value('@sortable', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0 ELSE 1 END AS BIT) AS lvsortable
		, xmlTable.xmlCol.value('@format', 'nvarchar(50)') AS lvformat
		, CAST(CASE xmlTable.xmlCol.value('@incQuickSearch', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0 ELSE 1 END AS BIT) AS lvincquicksearch
		,  dbo.GetCodeLookupDesc('SLCAPTION', xmlTable.xmlCol.value('@nullText', 'nvarchar(15)'), @UI) AS lvnulltext
		, xmlTable.xmlCol.value('@dataListName', 'nvarchar(15)') AS lvdatalistname
		, xmlTable.xmlCol.value('@dataCodeType', 'nvarchar(15)') AS lvdatacodetype
		, xmlTable.xmlCol.value('@conditions', 'nvarchar(500)') AS conditions
		, xmlTable.xmlCol.value('@roles', 'nvarchar(500)') AS roles
		, xmlTable.xmlCol.value('@sourceIs', 'nvarchar(15)') AS sourceIs
		, xmlTable.xmlCol.value('@displayAs', 'nvarchar(15)') AS displayAs
		, CAST(CASE xmlTable.xmlCol.value('@visible', 'nvarchar(5)') WHEN 'true' THEN 1 WHEN 'false' THEN 0 ELSE 1 END AS BIT) AS visible
		, xmlTable.xmlCol.value('@orderIndex', 'int') AS orderIndex

	FROM @X.nodes('/searchList/listView/column') xmlTable(xmlCol)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprSearchListBuilder] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprSearchListBuilder] TO [OMSAdminRole]
    AS [dbo];

