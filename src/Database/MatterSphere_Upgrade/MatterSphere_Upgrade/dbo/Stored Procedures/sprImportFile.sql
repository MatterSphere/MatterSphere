

CREATE PROCEDURE [dbo].[sprImportFile] (
@addLine1 nvarchar(64), -- details of default address record
@addLine2 nvarchar(64),
@addLine3 nvarchar(64),
@addLine4 nvarchar(64),
@addLine5 nvarchar(64),
@addPostcode nvarchar(20),
@addCountry int, --will need to get country from dbcountry table
@contSalut nvarchar(50), -- salution used when writing to contact/associate
@contTitle nvarchar(10), --title
@contFirstName nvarchar(20), -- first middle and surname will be concatenated for contact/client name
@contMiddleNames nvarchar(30),
@contSurname nvarchar(20),
@contTypeCode nvarChar(15), --contact type from admin kit codelookup [where cdtype = CONTTYPE]
@location nvarchar(15), --Code lookup where cdType = INFOADDTYPE
@telephone nvarchar(30),
@fax nvarchar(30),
@mobile nvarchar(30),
@email nvarchar(200),
@clientType nvarchar(15),
@feeusrID int, --shared value at the moment [ID of feeearner who introduced client & file principle and responsible]  
@notes nvarchar(500),
@fileDesc nvarchar(255), --Description of file
@fileDepartment nvarchar(15), --codelookup of department [cdtype = dept]
@fileType nvarchar(15), --codelookup [Code lookup where cdType = FILETYPE]
@fileFundCode nvarchar(15),  --codelookup [Code lookup where cdType = FUNDTYPE]
@filecurISOCode char(3), -- currency code e.g 'GBP'
@filelacategory smallint --legal aid category eg 1 
) 
AS
set nocount on
--declare any required variables
declare @addid bigint -- capture the newly iserted identity value
declare @contid bigint -- capture the newly iserted identity value
declare @clientid bigint -- capture the newly iserted identity value
declare @fileid bigint -- capture the newly iserted identity value
declare @error int --used to capture error numbers
declare @count int --used to do record counts
declare @contName nvarchar(128)
declare @fileWarningPerc int --file financials
declare @fileCreditLimit money --file financials
declare @fileRatePerUnit money --file financials
declare @fileBanding money --file financials
declare @branchID int --ID of branch 
declare @RESPONSIBLE bigint --ID of the responsibleid

BEGIN TRANSACTION
-- set inital value for error tracker
set @error = 0
set @contname = coalesce(@contTitle,'') + ' ' + coalesce(@contfirstname,'') + ' ' +  coalesce(@contmiddlenames,'') + ' ' + coalesce(@contsurname,'')
--perofrm a clean up of double spaces
set @contname = replace(@contname,'  ',' ')
set @contname = replace(@contname,'  ',' ')
--get the branch ID 
set @branchID = (select top 1 brid from dbreginfo)

-- see if address already exists
set @count = (select count(*) from dbaddress 
	where addline1 = @addline1 and addline2 = @addline2 and addline3 = @addline3 and addline4 = @addline4 and addline5 = @addline5 and addPostcode = @addPostcode)

-- check if we need to insert
if(@count >0)
	begin
	set @addid = (select addid from dbaddress where addline1 =@addline1 and addline2 = @addline2 and addline3 = @addline3 and addline4 = @addline4 and addline5 = @addline5 and addPostcode = @addPostcode)		
	end
else
	begin
	--create address get address ID into @addid
	insert into dbaddress(addline1,addline2,addline3,addline4,addline5,addPostcode,addCountry)
	values(@addline1,@addline2,@addline3,@addline4,@addline5,@addPostcode,@addCountry)
	select @addid = scope_identity(), @error = @@error
	end

--check for an error
if (@error <> 0) GOTO ErrorHandler

--create contact record
insert into dbcontact(contIsClient,contTypeCode,contName,contDefaultAddress,contsalut,created)
	values(1,@contTypeCode,@contName,@addid,@contSalut,getutcdate())
--check error and get id
select @contid = scope_identity(), @error = @@error

if (@error <> 0) GOTO ErrorHandler
--*********************************************** contact id
print @contid

--create a contact address record
insert into dbcontactaddresses(contid,contaddid,contcode) values(@contid,@addid,@location)

--create contact phone numbers 
if(@telephone <> '' and @telephone is not null)
insert into dbcontactnumbers(contid,contcode,contnumber,contextracode) values(@contid,'TELEPHONE',@telephone,@location)

if(@fax <> '' and @fax is not null)
insert into dbcontactnumbers(contid,contcode,contnumber,contextracode) values(@contid,'FAX',@fax,@location)

if(@mobile <> '' and @mobile is not null)
insert into dbcontactnumbers(contid,contcode,contnumber,contextracode) values(@contid,'MOBILE',@mobile,@location)

--insert contact email
if(@email <> '' and @email is not null)
insert into dbcontactemails(contid,contemail,contcode) values(@contid,@email,@location)

-- insert supporting records into contactcompany
insert into dbcontactcompany(contid) values(@contid)

-- insert into contact individual
insert into dbcontactindividual(contid) values(@contid)

--insert client record
insert into dbclient(clTypeCode,clName,clSearch1,clSearch2,clSearch3,clSearch4,clSearch5,clSource,brID,feeusrID,clNotes,clDefaultContact,clAutoCreated
,clAutoSource,clAutoType)
values(@clienttype,@contName,@contfirstname,@contsurname,@contmiddlenames,'','','UNKNOWN',@branchid,@feeusrID,@notes,@contid,getutcdate()
,'TEST','PROC')

--check error and get id
select @clientid = scope_identity(), @error = @@error
if (@error <> 0) GOTO ErrorHandler
--print 'Client ' + @clientid

--tie client to contact
insert into dbclientcontacts(clid,contid) values(@clientid,@contid)

-- get financial values
select @fileWarningPerc = coalesce(ftWarningPerc,0),
	@fileCreditLimit = coalesce(ftCreditLimit,0),
	@fileRatePerUnit = coalesce(ftRatePerUnit,0),
	@fileBanding = coalesce(ftBand,0) from dbfundtype where ftcode=@fileFundCode and ftcurISOCode=@filecurISOCode

--Retrieve the fee earner manager
select @RESPONSIBLE = feeresponsibleto from dbfeeearner where feeusrid = @feeusrID

-- now insert the new file record
insert into dbfile (clID,fileno,fileDesc,fileResponsibleID,filePrincipleID,fileDepartment,brID,fileType,fileStatus,fileFundCode,filecurISOCode,
fileWarningPerc,fileCreditLimit,fileOriginalLimit,fileRatePerUnit,fileBanding,filelacategory)
values(@clientid,'',@fileDesc,@RESPONSIBLE,@feeusrID,@fileDepartment,@branchid,@fileType,'LIVE',@fileFundCode,@filecurISOCode,
@fileWarningPerc,@fileCreditLimit,@fileCreditLimit,@fileRatePerUnit,@fileBanding,@filelacategory)

--check error and get id
select @fileid = scope_identity(), @error = @@error
if (@error <> 0) GOTO ErrorHandler
--print 'File ' + @fileid

--finally add the contact as an associate
insert into dbassociates(fileid,contid,assocorder,assocType,assocSalut,assocAddressee,assocHeading,assocdefaultaddID)
values(@fileid,@contid,0,'CLIENT',@contSalut,@contname,@filedesc,null)

--One final error check
if (@@error <> 0) GOTO ErrorHandler

COMMIT TRANSACTION
return 0
	
Errorhandler:
ROLLBACK TRANSACTION
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprImportFile] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprImportFile] TO [OMSAdminRole]
    AS [dbo];

