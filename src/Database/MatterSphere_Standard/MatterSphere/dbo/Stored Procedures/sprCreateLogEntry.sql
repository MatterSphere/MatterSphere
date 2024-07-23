

CREATE PROCEDURE [dbo].[sprCreateLogEntry] (
			@Type smallint, 
			@User int, 
			@DateTime datetime = null,
			@Description nvarchar(1200), 
			@StringLink nvarchar(100) = null, 
			@NumericLink bigint = null, 
			@ExtendedInfo ntext = null) 
AS

declare @typeSeverity tinyint
declare @logSeverity tinyint

select @typeseverity = typeseverity from dbcaptainslogtype where typeid = @Type
select top 1 @logSeverity = regloggingseverity from dbreginfo

if @typeseverity is null
	return

if (@typeSeverity < @logSeverity)
	return

if (@DateTime is null)
	set @DateTime = getutcdate()

insert into dbcaptainslog (logtypeid, logusrid, logwhen, logdesc, logdataS, logdataN, logextended) 
	values (@Type, @User, @DateTime, @Description, @StringLink, @NumericLink, @ExtendedInfo)

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCreateLogEntry] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCreateLogEntry] TO [OMSAdminRole]
    AS [dbo];

