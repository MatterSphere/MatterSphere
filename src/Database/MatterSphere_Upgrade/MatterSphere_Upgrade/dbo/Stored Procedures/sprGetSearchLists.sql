CREATE PROCEDURE [dbo].[sprGetSearchLists] (@Group uCodeLookup = null, @Type uCodeLookup = '', @UI uUICultureInfo = '{default}', @Active smallint = 1) AS
	IF not @Group is null
	begin
		IF @Active = 2 AND @Type = ''
			select schcode, schActive, schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, schSourceType, schType, schGroup, Updated, UpdatedBy, Created, CreatedBy 
			from dbsearchlistconfig slc
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schIsReport = 0 and schGroup = @Group
		IF @Type = '' AND @Active = 1
			select schcode, schActive, schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, schSourceType, schType, schGroup, Updated, UpdatedBy, Created, CreatedBy 
			from dbsearchlistconfig slc
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 1 and schIsReport = 0 and schGroup = @Group
		IF @Type = '' AND @Active = 0
			select schcode, schActive, schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, schSourceType, schType, schGroup, Updated, UpdatedBy, Created, CreatedBy 
			from dbsearchlistconfig slc
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 0 and schIsReport = 0 and schGroup = @Group
		IF @Type <> '' AND @Active = 1
			select schcode, schActive, schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, schSourceType, schType, schGroup, Updated, UpdatedBy, Created, CreatedBy 
			from dbsearchlistconfig slc
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 1 and schtype = @Type and schIsReport = 0 and schGroup = @Group
		IF @Type <> '' AND @Active = 0
			select schcode, schActive, schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, schSourceType, schType, schGroup, Updated, UpdatedBy, Created, CreatedBy 
			from dbsearchlistconfig slc
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 0 and schtype = @Type and schIsReport = 0 and schGroup = @Group
	end
	else
	begin
		IF @Active = 2 AND @Type = ''
			select schcode, schActive, schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, schSourceType, schType, schGroup, Updated, UpdatedBy, Created, CreatedBy 
			from dbsearchlistconfig slc
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schIsReport = 0
		IF @Type = '' AND @Active = 1
			select schcode, schActive, schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, schSourceType, schType, schGroup, Updated, UpdatedBy, Created, CreatedBy 
			from dbsearchlistconfig slc
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 1 and schIsReport = 0
		IF @Type = '' AND @Active = 0
			select schcode, schActive, schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, schSourceType, schType, schGroup, Updated, UpdatedBy, Created, CreatedBy 
			from dbsearchlistconfig slc
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 0 and schIsReport = 0
		IF @Type <> '' AND @Active = 1
			select schcode, schActive, schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, schSourceType, schType, schGroup, Updated, UpdatedBy, Created, CreatedBy 
			from dbsearchlistconfig slc
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 1 and schtype = @Type and schIsReport = 0
		IF @Type <> '' AND @Active = 0
			select schcode, schActive, schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, schSourceType, schType, schGroup, Updated, UpdatedBy, Created, CreatedBy 
			from dbsearchlistconfig slc
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 0 and schtype = @Type and schIsReport = 0
	end

