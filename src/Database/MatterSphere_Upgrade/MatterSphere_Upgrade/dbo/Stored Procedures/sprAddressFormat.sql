

CREATE  PROCEDURE [dbo].[sprAddressFormat] (@Country int, @UI uUICultureInfo = '{default}', @UIType tinyint = 0) AS
declare @xml nvarchar(1000)
declare @handle int
declare @addformat varchar(800)
select @xml = ctryaddressformat from dbcountry where ctryID = @Country
if @xml is null
begin
set @xml = 
	'<ADDRESSLINES>
	<Line field="addLine1" question="txtAdd1"/>
	<Line field="addLine2" question="txtAdd2"/>
	<Line field="addLine3" question="txtAdd3"/>
	<Line field="addLine4" question="txtTown"/>
	<Line field="addLine5" question="txtCity"/>
	<Line field="addPostcode" question="txtPostCode"/>
	<Line field="addCountry" question="cboCountry"/>
</ADDRESSLINES>'
end
exec sp_xml_preparedocument @handle output, @xml
--- Adjusted MNW to get extended layout properties for Country information layout.
select @addformat = AddressFormat FROM OPENXML(@HANDLE,'ADDRESSLINES',2)
 with (AddressFormat varchar(800))

select field as [qufieldname], 
	Q.quname,
	Q.qucode,
	 COALESCE(CL1.cdDesc, '~' + NULLIF(Q.qucode, '') + '~') as qudesc,
			(case when @UIType = 0 then C.ctrlwintype else C.ctrlwebtype end)	as [qucontrol],
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
	Q.quanchor,
	Q.quFormat,
	Q.qucustom,
	Q.quFilter,
	(case when qucommand is null then null else dbo.GetCodeLookupHelp('ENQCOMMAND', CMD.cmdcode, @UI) end) as [qucmdhelp],
	(case when qucommand is null then null else CMD.cmdmethod end) as [qucmdmethod],
	(case when qucommand is null then null else CMD.cmdparameters end) as [qucmdparameters],
	(case when qucommand is null then null else CMD.cmdtype end) as [qucmdtype],
	Q.qucommandretval,
	Q.qucasing,
	AddressFormat = @addformat
from openxml(@handle, 'ADDRESSLINES/Line', 1) 
	with (field varchar(25),
		question  nvarchar(30))
inner join dbenquiryquestion Q on Q.quname = question
inner join dbenquiry dbe on dbe.enqid = q.enqid and dbe.enqcode = 'SCRADDTEMPLATE' 
	inner join dbenquirycontrol C
		on Q.quctrlid = C.ctrlid
	left join dbenquirycommand CMD
		on qucommand = CMD.cmdcode 
	LEFT JOIN dbo.GetCodeLookupDescription ( 'ENQQUESTION', @UI ) CL1 ON CL1.[cdCode] =  Q.qucode
order by Q.quorder
exec sp_xml_removedocument @handle

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddressFormat] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAddressFormat] TO [OMSAdminRole]
    AS [dbo];

