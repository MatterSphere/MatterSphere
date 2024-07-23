

CREATE PROCEDURE [dbo].[schSearchPrecedentByTitle] (@TITLE nvarchar(50), @LANGUAGE uUICultureInfo = '', @UI uUICultureInfo = '{default}') AS

select 
PrecTitle,
PrecID,
PrecDesc,
PrecCategory,
PrecSubCategory,
PrecMinorCategory,
PrecAddressee,
PrecType, 
precLanguage,
precmultiprec,
dbo.GetCodeLookupDesc('DOCTYPE', prectype, @UI) as PrecTypeDesc,
dbo.GetCodeLookupDesc('PRECCAT', preccategory, @UI) as PrecCategoryDesc,
dbo.GetCodeLookupDesc('PRECSUBCAT', precsubcategory, @UI) as PrecSubCategoryDesc, 
dbo.GetCodeLookupDesc('PRECMINORCAT', precminorcategory, @UI) as PrecMinorCategoryDesc, 
dbo.GetCodeLookupDesc('PRECLIBRARY', preclibrary, @UI) as PrecLibraryDesc,
dbo.GetCodeLookupDesc('CONTTYPE', precaddressee, @UI) as PrecAddresseeDesc
from DBPrecedents
where prectitle = @TITLE -- and precid = dbo.GetPrecedent(prectitle, prectype, preclibrary, preccategory, precsubcategory, @Language)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchPrecedentByTitle] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[schSearchPrecedentByTitle] TO [OMSAdminRole]
    AS [dbo];

