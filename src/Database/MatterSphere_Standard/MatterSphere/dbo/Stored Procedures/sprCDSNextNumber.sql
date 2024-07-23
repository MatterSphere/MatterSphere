

CREATE PROCEDURE [dbo].[sprCDSNextNumber] (@feeID bigint, @indate datetime = null, @clid bigint = null, @ufnheadcode varchar(12) = '''') 
AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
declare @startnum as int
declare @ufndate as varchar(6)
declare @newcdsnumber as varchar(12)

---' Test Data
---select @feeid = 1007463955

if @indate is null -- No Date sent through to stored proc so create date from today
	select @indate = getutcdate()

select @startnum = right('000' + feeCDSStartnum,3) from dbfeeearner where feeusrid = @feeid
select @ufndate = RIGHT('00' + Convert(nvarchar(2),datepart(dd,@indate)),2) + RIGHT('00' + Convert(nvarchar(2),datepart(mm,@indate)),2) + RIGHT(Convert(nvarchar(4),datepart(yyyy,@indate)),2)

if @startnum is null
	select @startnum = '001' --- set this to one, not good practice but useful overide if not configured.

print 'Current Statistics Information'
Print @startnum
print @indate
print @ufndate

if (Select Count(*) from dbufn where ufncode like @ufndate + '%') = 0
BEGIN
	--- Return the UFN Number set
	set @newcdsnumber =  @ufndate + '/' + right('000' + convert(varchar(5), @startnum),3) 
	select @ufndate + '/' + right('000' + convert(varchar(5), @startnum),3)
END
ELSE
BEGIN
	Print 'Loop Check Required'
--- Check 	
	WHILE (SELECT count(*) FROM dbufn where ufncode like @ufndate + '/' + right('000' + convert(varchar(5), @startnum),3)) > 0 
	BEGIN
		Print 'Startnumber Incr'
		set @Startnum = @Startnum + 1
	END
	set @newcdsnumber =  @ufndate + '/' + right('000' + convert(varchar(5), @startnum),3)
	select @ufndate + '/' + right('000' + convert(varchar(5), @startnum),3)

END

print @CLID
print @ufnheadcode

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCDSNextNumber] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCDSNextNumber] TO [OMSAdminRole]
    AS [dbo];

