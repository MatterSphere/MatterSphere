

CREATE PROCEDURE [dbo].[sprExcGetAppointments] 
(@Branch int,@UI uUICultureInfo = '{default}')

as

if (@Branch = 0)
	select A.*,appLocation as Location,APPType.cdDesc as Category
		from dbappointments A join dbuser U on A.feeusrID = U.usrID 
		left join dbo.GetCodeLookupDescription('APPTYPE',@UI) APPType on APPType.cdCode = A.APPType
		where appDate >= getutcdate()-1 and U.usrExchangeSync = 1 and (appActive = 1 or (appActive = 0 and COALESCE (appMAPIEntryID,'') <> '' ))
		and ( appExchangeSync = 1 or appExchangeSync is null)
		order by feeusrid

else

	select A.*, appLocation AS Location , APPType.cdDesc as Category
		from dbappointments A join dbuser U on A.feeusrID = U.usrID 
		left join dbo.GetCodeLookupDescription('APPTYPE',@UI) APPType on APPType.cdCode = A.APPType
		where appDate >= getutcdate()-1 and U.brID = @Branch and U.usrExchangeSync = 1 and (appActive = 1 or (appActive = 0 and COALESCE (appMAPIEntryID,'') <> '' ))
		and ( appExchangeSync = 1 or appExchangeSync is null)
		order by feeusrid

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprExcGetAppointments] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprExcGetAppointments] TO [OMSAdminRole]
    AS [dbo];

