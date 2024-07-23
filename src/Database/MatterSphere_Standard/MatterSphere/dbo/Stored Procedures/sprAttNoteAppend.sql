

CREATE PROCEDURE [dbo].[sprAttNoteAppend]
(
	@ACTCODE ucodelookup,
	@ASSOCID bigint,
	@CLID bigint,
	@FEEUSRID bigint,
	@FILEID bigint,
	@LEGAIDCAT SMALLINT,
	@LEGAIDGRADE TINYINT,
	@TIMECHARGE MONEY,
	@TIMECOST MONEY,
	@TIMEDESC NVARCHAR(150),
	@TIMEFORMAT SMALLINT,
	@TIMEMINS INT,
	@TIMERECORDED DATETIME,
	@TIMEUNITS INT,
	@USRID BIGINT
)
AS

if @fileid is not null 
BEGIN
	INSERT INTO dbTimeLedger
		(
			timeactivitycode,
			associd,
			clid,
			feeusrid,
			fileid,
			timelegalaidcat,
			timelegalaidgrade,
			timecharge,
			timecost,
			timedesc,
			timeformat,
			timemins,
			timerecorded,
			timeunits,
			createdby)
	values
		(
			@ACTCODE,
			@ASSOCID,
			@CLID,
			@FEEUSRID,
			@FILEID,
			@LEGAIDCAT,
			@LEGAIDGRADE,
			@TIMECHARGE,
			@TIMECOST,
			@TIMEDESC,
			@TIMEFORMAT,
			@TIMEMINS,
			@TIMERECORDED,
			@TIMEUNITS,
			@USRID)
END 

select scope_identity(),scope_identity(),db_name()

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAttNoteAppend] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprAttNoteAppend] TO [OMSAdminRole]
    AS [dbo];

