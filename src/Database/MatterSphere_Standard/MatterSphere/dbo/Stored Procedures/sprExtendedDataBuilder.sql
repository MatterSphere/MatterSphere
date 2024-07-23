

CREATE PROCEDURE [dbo].[sprExtendedDataBuilder] (@UI uUICultureInfo = '{default}') 
AS
select 
	Q.qufieldname,
	Q.quname,
	Q.qucode,
	COALESCE(CL1.cdDesc, '~' + NULLIF(Q.qucode, '') + '~') as qudesc,
	C.ctrlwintype as [qucontrol],
	Q.qudatalist, 
	Q.qutaborder,
	Q.quminlength,
	Q.qumaxlength,	
	Q.quwidth,
	Q.quheight,
	Q.quhidden,
	dbo.GetCodeLookupHelp('ENQQUESTION', Q.qucode, @UI) as quhelp,
	Q.qumask,
	Q.quextendeddata,
	Q.qudefault,
	Q.qureadonly,
	Q.qurequired,
	Q.qucaptionwidth,
	Q.quX,
	Q.quY,
	Q.qucustom,
	(case when qucommand is null then null else dbo.GetCodeLookupHelp('ENQCOMMAND', CMD.cmdcode, @UI) end) as [qucmdhelp],
	(case when qucommand is null then null else CMD.cmdmethod end) as [qucmdmethod],
	(case when qucommand is null then null else CMD.cmdparameters end) as [qucmdparameters],
	(case when qucommand is null then null else CMD.cmdtype end) as [qucmdtype],
	Q.qucommandretval,
	Q.qucasing,
	Q.quanchor,
	Q.quformat
from dbenquiryquestion Q
inner join dbenquirycontrol C on C.ctrlid = Q.quctrlid
inner join dbenquirycommand CMD on CMD.cmdcode = Q.qucommand
LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQQUESTION', @UI ) CL1 ON CL1.[cdCode] =  Q.qucode
where quid is null

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprExtendedDataBuilder] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprExtendedDataBuilder] TO [OMSAdminRole]
    AS [dbo];

