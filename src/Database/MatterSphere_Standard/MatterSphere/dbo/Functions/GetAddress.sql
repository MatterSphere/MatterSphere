

CREATE FUNCTION [dbo].[GetAddress] (@ADDID BIGINT, @SEPARATOR nvarchar(10), @UI nvarchar(10))
returns nvarchar(400)
as
begin

declare @TMP nvarchar(400)
declare @ADD1 uAddressLine
declare @ADD2 uAddressLine
declare @ADD3 uAddressLine
declare @ADD4 uAddressLine
declare @ADD5 uAddressLine
declare @COUNTRY uCountry
declare @ADDPC uPostcode
if @UI is null set @UI = ''
if @SEPARATOR is null set @SEPARATOR = ', '



select 
	@add1 = addline1,
	@add2 = addline2,
	@add3 = addline3,
	@add4 = addline4,
	@add5 = addline5,
	@addpc = addpostcode,
	@country = addcountry
from 
	dbaddress
where
	addid = @addid


if coalesce(@add1,'') <>'' set @tmp = @add1
if coalesce(@add2,'') <>'' 
BEGIN
	if @tmp <>'' set @tmp = @tmp + @SEPARATOR
	set @tmp = @tmp + @add2
END 
if coalesce(@add3,'') <>'' 
BEGIN
	if @tmp <>'' set @tmp = @tmp + @SEPARATOR
	set @tmp = @tmp + @add3
END 
if coalesce(@add4,'') <>'' 
BEGIN
	if @tmp <>'' set @tmp = @tmp + @SEPARATOR
	set @tmp = @tmp + @add4
END 
if coalesce(@add5,'') <>'' 
BEGIN
	if @tmp <>'' set @tmp = @tmp + @SEPARATOR
	set @tmp = @tmp + @add5
END 
if coalesce(@addpc,'') <>'' 
BEGIN
	if @tmp <>'' set @tmp = @tmp + @SEPARATOR
	set @tmp = @tmp + @addpc
END 



if @country <> (select regdefcountry from dbreginfo)
BEGIN
	set @tmp = @tmp + @SEPARATOR
	set @tmp = @tmp + dbo.getcodelookupdesc('COUNTRIES',(select ctrycode from dbcountry where ctryid = @country),@UI)
END

return @tmp

end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAddress] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAddress] TO [OMSAdminRole]
    AS [dbo];

