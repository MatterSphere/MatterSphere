

CREATE PROCEDURE [dbo].[sprDateWizardBuilder] (@Type uCodeLookup, @UI uUICultureInfo = '{default}', @UIType tinyint = 0) AS


--DATEWIZARD
select *, dbo.GetCodeLookupDesc('DATEWIZTYPES', typecode, @UI) as [typedesc] from dbdatewiztypes where typecode = @Type

--DATES
select * from dbdatewizdates where typecode = @Type

--KEYDATES
select top 0 * from dbkeydates


--QUESTIONS
select null as [qufieldname], 
	datecode as quname,
	datecode as qucode,
	COALESCE(CL1.cdDesc, '~' + NULLIF(datecode, '') + '~') as qudesc,
	(case when @UIType = 0 then C.ctrlwintype else C.ctrlwebtype end) as [qucontrol],
	null as qudatalist, 
	dateorder as qutaborder,
	0 as quminlength,
	0 as qumaxlength,	
	300 as quwidth,
	20 as quheight,
	convert(bit, 0) as quhidden,
	dbo.GetCodeLookupHelp('ENQQUESTION', datecode, @UI) as quhelp,
	null as qumask,
	null as quextendeddata,
	null as qudefault,
	convert(bit, (case when dateeditable = 1 then 0 else 1 end)) as qureadonly,
	dateeditable as qurequired,
	150 as qucaptionwidth,
	0 as quX,
	0 as quY,
	null as quanchor,
	null as qucustom,
	null as qucmdhelp,
	null as qucmdmethod,
	null as qucmdparameters,
	null as qucmdtype,
	convert(bit, 0) as qucommandretval,
	null as qucasing,
	[dateformat] as quformat 
from dbdatewizdates
inner join dbenquirycontrol C on datectrlid = C.ctrlid
LEFT JOIN dbo.GetCodeLookupDescription ( 'DATEWIZDATES', @UI ) CL1 ON CL1.[cdCode] =  datecode
order by dateorder

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprDateWizardBuilder] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprDateWizardBuilder] TO [OMSAdminRole]
    AS [dbo];

