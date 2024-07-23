

CREATE PROCEDURE [dbo].[sprCreateCodeLookup] (@Type uCodeLookup, @Code uCodeLookup, @Description nvarchar(1000), @Help nvarchar(500), @Notes nvarchar(500), @UI uUICultureInfo = '{default}', @System bit = 0, @Deletable bit = 1, @AddLink uCodeLookup, @Group bit = 0)
as
-- Written By - DJRM

declare @Culture uUICultureInfo

--Check to see if the {default} item exists, if not, insert one into the lookup table using the parameters passed.
if (select count(*) from dbcodelookup where cdtype = @Type and cdcode = @Code and cdUICultureInfo = '{default}') = 0
begin
	insert into dbcodelookup 
		(cdtype, 
		cdcode, 
		cduicultureinfo, 
		cddesc, 
		cdsystem, 
		cddeletable, 
		cdhelp, 
		cdnotes,
		cdaddlink,
		cdgroup)
	values
		(@Type, @Code, '{default}', @Description, @System, @Deletable, @Help, @Notes, @AddLink, @Group)
	
end

--If it is not a {default} item already being added then insert the new localixed lookup item into the table using the passed paramaters.
if (@UI is not null) and (@UI <> '{default}')
begin
	set @Culture = @UI
	
	insert into dbcodelookup 
		(cdtype, 
		cdcode, 
		cduicultureinfo, 
		cddesc, 
		cdsystem, 
		cddeletable, 
		cdhelp, 
		cdnotes,
		cdaddlink,
		cdgroup)
	values
		(@Type, @Code, @UI, @Description, @System, @Deletable, @Help, @Notes, @AddLink, @Group)
		
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCreateCodeLookup] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprCreateCodeLookup] TO [OMSAdminRole]
    AS [dbo];

