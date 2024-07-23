

CREATE PROCEDURE [dbo].[sprGetReportsLists] (@Group uCodeLookup = null, @Type uCodeLookup = '', @UI uUICultureInfo = '{default}', @Active smallint) AS
	IF not @Group is null
	begin
		IF @Active = 2 AND @Type = ''
			select slc.schcode, slc.schActive, slc.schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, slc.schSourceType, slc.schGroup, slc.Updated, slc.UpdatedBy, slc.Created, slc.CreatedBy, r.rptversion 
			from dbsearchlistconfig slc inner join dbreport r on r.rptcode = slc.schcode 
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schIsReport = 1 and schGroup = @Group
		IF @Type = '' AND @Active = 1
			select slc.schcode, slc.schActive, slc.schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, slc.schSourceType, slc.schGroup, slc.Updated, slc.UpdatedBy, slc.Created, slc.CreatedBy, r.rptversion 
			from dbsearchlistconfig slc inner join dbreport r on r.rptcode = slc.schcode 
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 1 and schIsReport = 1 and schGroup = @Group
		IF @Type = '' AND @Active = 0
			select slc.schcode, slc.schActive, slc.schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, slc.schSourceType, slc.schGroup, slc.Updated, slc.UpdatedBy, slc.Created, slc.CreatedBy, r.rptversion 
			from dbsearchlistconfig slc inner join dbreport r on r.rptcode = slc.schcode 
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 0 and schIsReport = 1 and schGroup = @Group
		IF @Type <> '' AND @Active = 1
			select slc.schcode, slc.schActive, slc.schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, slc.schSourceType, slc.schGroup, slc.Updated, slc.UpdatedBy, slc.Created, slc.CreatedBy, r.rptversion 
			from dbsearchlistconfig slc inner join dbreport r on r.rptcode = slc.schcode 
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 1 and schtype = @Type and schIsReport = 1 and schGroup = @Group
		IF @Type <> '' AND @Active = 0
			select slc.schcode, slc.schActive, slc.schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, slc.schSourceType, slc.schGroup, slc.Updated, slc.UpdatedBy, slc.Created, slc.CreatedBy, r.rptversion 
			from dbsearchlistconfig slc inner join dbreport r on r.rptcode = slc.schcode 
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 0 and schtype = @Type and schIsReport = 1 and schGroup = @Group
	end
	else
	begin
		IF @Active = 2 AND @Type = ''
			select slc.schcode, slc.schActive, slc.schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, slc.schSourceType, slc.schGroup, slc.Updated, slc.UpdatedBy, slc.Created, slc.CreatedBy, r.rptversion 
			from dbsearchlistconfig slc inner join dbreport r on r.rptcode = slc.schcode 
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schIsReport = 1
		IF @Type = '' AND @Active = 1
			select slc.schcode, slc.schActive, slc.schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, slc.schSourceType, slc.schGroup, slc.Updated, slc.UpdatedBy, slc.Created, slc.CreatedBy, r.rptversion 
			from dbsearchlistconfig slc inner join dbreport r on r.rptcode = slc.schcode 
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 1 and schIsReport = 1
		IF @Type = '' AND @Active = 0
			select slc.schcode, slc.schActive, slc.schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, slc.schSourceType, slc.schGroup, slc.Updated, slc.UpdatedBy, slc.Created, slc.CreatedBy, r.rptversion 
			from dbsearchlistconfig slc inner join dbreport r on r.rptcode = slc.schcode 
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 0 and schIsReport = 1
		IF @Type <> '' AND @Active = 1
			select slc.schcode, slc.schActive, slc.schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, slc.schSourceType, slc.schGroup, slc.Updated, slc.UpdatedBy, slc.Created, slc.CreatedBy, r.rptversion 
			from dbsearchlistconfig slc inner join dbreport r on r.rptcode = slc.schcode 
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 1 and schtype = @Type and schIsReport = 1
		IF @Type <> '' AND @Active = 0
			select slc.schcode, slc.schActive, slc.schStyle, COALESCE(CL1.cdDesc, '~' + NULLIF(slc.schcode, '') + '~') as schdesc, slc.schSourceType, slc.schGroup, slc.Updated, slc.UpdatedBy, slc.Created, slc.CreatedBy, r.rptversion 
			from dbsearchlistconfig slc inner join dbreport r on r.rptcode = slc.schcode 
			LEFT JOIN dbo.GetCodeLookupDescription ( 'OMSSEARCH', @UI ) CL1 ON CL1.[cdCode] =  slc.schcode
			where schactive = 0 and schtype = @Type and schIsReport = 1
	end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprGetReportsLists] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprGetReportsLists] TO [OMSAdminRole]
    AS [dbo];

