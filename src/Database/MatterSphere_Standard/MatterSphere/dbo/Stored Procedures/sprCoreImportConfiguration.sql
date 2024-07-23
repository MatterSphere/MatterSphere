

CREATE PROCEDURE [dbo].[sprCoreImportConfiguration] (@Source uCodeLookup, @UI uUICultureInfo = '{default}') AS


--CONTTYPE
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, dbo.GetCodeLookupDesc('CONTTYPE', cidata, @UI) as cidesc from dbcoreimportconfig inner join dbcontacttype on cidata = typecode where citype = 'CONTTYPE' and typeactive = 1 group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata

--CLTYPE
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, dbo.GetCodeLookupDesc('CLTYPE', cidata, @UI) as cidesc from dbcoreimportconfig inner join dbclienttype on cidata = typecode where ciid = dbo.GetCoreImportConfig('CLTYPE', @Source, cifilter1, cifilter2, cifilter3) and typeactive = 1 group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata

--FILETYPE
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, dbo.GetCodeLookupDesc('FILETYPE', cidata, @UI) as cidesc from dbcoreimportconfig inner join dbfiletype on cidata = typecode where ciid = dbo.GetCoreImportConfig('FILETYPE', @Source, cifilter1, cifilter2, cifilter3)  and typeactive = 1 group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata

--ASSOCTYPE
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, dbo.GetCodeLookupDesc('SUBASSOC', cidata, @UI) as cidesc  from dbcoreimportconfig inner join dbassociatedtypes on cidata = assoctype where ciid = dbo.GetCoreImportConfig('ASSOCTYPE', @Source, cifilter1, cifilter2, cifilter3) group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata

--INFOTYPE
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3,  cidata, dbo.GetCodeLookupDesc('INFOTYPE', cidata, @UI) as cidesc  from dbcoreimportconfig where ciid = dbo.GetCoreImportConfig('INFOTYPE', @Source, cifilter1, cifilter2, cifilter3)  group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata

--SEARCH
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, dbo.GetCodeLookupDesc('SEARCHLIST', cidata, @UI) as cidesc  from dbcoreimportconfig inner join dbsearchlistconfig on cidata = schcode where ciid = dbo.GetCoreImportConfig('SEARCH', @Source, cifilter1, cifilter2, cifilter3)  and schactive = 1 group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata

--CONTACT
select ciid, citype,  cisource, cifilter1, cifilter2, cifilter3,cidata, contname as cidesc  from dbcoreimportconfig inner join dbcontact on cidata = contid where ciid = dbo.GetCoreImportConfig('CONTACT', @Source, cifilter1, cifilter2, cifilter3)  group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, contname

--CLIENT
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3,cidata, clname as cidesc   from dbcoreimportconfig inner join dbclient on cidata = clid where ciid = dbo.GetCoreImportConfig('CLIENT', @Source, cifilter1, cifilter2, cifilter3)   group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, clname

--FILE
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3,cidata, filedesc as cidesc   from dbcoreimportconfig inner join dbfile on cidata = fileid where ciid = dbo.GetCoreImportConfig('FILE', @Source, cifilter1, cifilter2, cifilter3)  group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, filedesc

--FILE_ALLOC
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, usrfullname as cidesc , dbo.GetCodeLookupDesc('FILETYPE', cifilter1, @UI) as cidesc2
from dbcoreimportconfig 
inner join dbfeeearner on cidata = feeusrid 
inner join dbuser on usrid = feeusrid 
where --ciid = dbo.GetCoreImportConfig('FILE_ALLOC', 'LMS', cifilter1, cifilter2, cifilter3)  
citype = 'FILE_ALLOC' and coalesce(cisource, @Source) = @Source and feeactive = 1 group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, usrfullname

--LOOKUP
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, dbo.GetCodeLookupDesc(cifilter1, cidata, @UI) as cidesc  from dbcoreimportconfig where ciid = dbo.GetCoreImportConfig('LOOKUP', @Source, cifilter1, cifilter2, cifilter3)  group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata

--EXTENDEDDATA
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, dbo.GetCodeLookupDesc('EXTENDEDDATA', cidata, @UI) as cidesc  from dbcoreimportconfig where ciid = dbo.GetCoreImportConfig('EXTENDEDDATA', @Source, cifilter1, cifilter2, cifilter3)  group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata

--MILESTONE
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata, null as cidesc from dbcoreimportconfig where ciid = dbo.GetCoreImportConfig('MILESTONE', @Source, cifilter1, cifilter2, cifilter3)  group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata

--FEEEARNER
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata,  null as cidesc from dbcoreimportconfig where ciid = dbo.GetCoreImportConfig('FEEEARNER', @Source, cifilter1, cifilter2, cifilter3) group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata

--OPTION
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata,  null as cidesc from dbcoreimportconfig where ciid = dbo.GetCoreImportConfig('OPTION', @Source, cifilter1, cifilter2, cifilter3) group by ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata

--SOURCE
select ciid, citype, cisource, cifilter1, cifilter2, cifilter3, cidata,  null as cidesc  from dbcoreimportconfig where citype = 'SOURCE' and cidata = @source

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCoreImportConfiguration] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCoreImportConfiguration] TO [OMSAdminRole]
    AS [dbo];

