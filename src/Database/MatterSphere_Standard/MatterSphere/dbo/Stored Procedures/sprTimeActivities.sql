CREATE PROCEDURE [dbo].[sprTimeActivities] (@FILEID bigint,@FEEusrID bigint, @UI uUICultureInfo = '{default}', @ACTIVE smallint = -1, @LEGAIDCAT smallint = -1, @INCLNull bit = 0) AS
 
declare @filebanding int
declare @filefundcode uCodeLookup
declare @filerateperunit money
declare @feecharge money
declare @feecost money
declare @filecurisocode nvarchar(5)
declare @isLegalAid bit
declare @feeLAGrade tinyint
 
--- Assign default Variables as Required if LEGALAIDCAT is -1 then get from file else overide
-- Performance May 2017 - Change to identify default schema
if exists (select top 1 1 from sys.synonyms where [schema_id] =schema_id('dbo') and base_object_name like '_config_.%')
begin
	if @LEGAIDCAT = -1
		 select @legaidcat = fileLACategory, @isLegalAid = ftLegalAidCharged, @filecurisocode = filecurisocode, @filebanding = filebanding, @filefundCode = fileFundCode, @filerateperunit = filerateperunit from config.dbfile inner join dbfundtype on dbfile.filefundcode = dbfundtype.ftcode where fileid = @fileid
		else
		 select @isLegalAid = ftLegalAidCharged, @filecurisocode = filecurisocode, @filebanding = filebanding, @filefundCode = fileFundCode, @filerateperunit = filerateperunit from config.dbfile inner join dbfundtype on dbfile.filefundcode = dbfundtype.ftcode where fileid = @fileid
end
else
begin
	if @LEGAIDCAT = -1
		 select @legaidcat = fileLACategory, @isLegalAid = ftLegalAidCharged, @filecurisocode = filecurisocode, @filebanding = filebanding, @filefundCode = fileFundCode, @filerateperunit = filerateperunit from dbfile inner join dbfundtype on dbfile.filefundcode = dbfundtype.ftcode where fileid = @fileid
		else
		 select @isLegalAid = ftLegalAidCharged, @filecurisocode = filecurisocode, @filebanding = filebanding, @filefundCode = fileFundCode, @filerateperunit = filerateperunit from dbfile inner join dbfundtype on dbfile.filefundcode = dbfundtype.ftcode where fileid = @fileid
end

--- Print Variables for Debug Purposes
print @filebanding
print @filefundcode
print Convert(nvarchar(20),@filerateperunit)
 
--- Get Fee Earner rates and Grades
select @feeLAGrade = feeLAgrade, @feecost = feecost, @feecharge = CASE @filebanding
   when 1 then feeRateBand1
   when 2 then feeRateBand2
   when 3 then feeRateBand3
   when 4 then feeRateBand4
   when 5 then feeRateBand5
   ELSE feeRateBand3
   END from dbfeeearner where feeUsrID = @FEEusrID
 
--- Print final Debug Information
print Convert(nvarchar(20),@feelagrade)
print Convert(nvarchar(10),@legaidcat)
 
--- Check if File is Legal Aid based and decide query on this basis.
if @islegalaid = 0
 BEGIN
  IF @INCLNULL = 1
   BEGIN
    select null as actCode, dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', @UI) as actCodeDesc, null as actAccCode, null as actChargeable, null as actTemplateMatch, null as actFixedValue, 0 as actCharge, 0 as actCost, 1 as actActive union 
    SELECT     actCode, COALESCE(CL1.cdDesc, '~' + NULLIF(dbActivities.actCode, '') + '~') AS actCodeDesc, actAccCode, actChargeable, actTemplateMatch, actFixedValue, 
     actCharge = case @filerateperunit
      when null then @feecharge
      when 0 then @feecharge
      ELSE @filerateperunit
      END * actChargeable, actcost = @feecost, actActive
    FROM       dbo.dbActivities
	LEFT JOIN dbo.GetCodeLookupDescription ( 'TIMEACTCODE', @UI ) CL1 ON CL1.[cdCode] =  dbActivities.actCode
    where actactive = 1
    ORDER BY actCodeDesc
   END
  ELSE
   BEGIN
    SELECT     actCode, COALESCE(CL1.cdDesc, '~' + NULLIF(dbActivities.actCode, '') + '~') AS actCodeDesc, actAccCode, actChargeable, actTemplateMatch, actFixedValue, 
     actCharge = case @filerateperunit
      when null then @feecharge
      when 0 then @feecharge
      ELSE @filerateperunit
      END * actChargeable, actcost = @feecost, actActive
    FROM       dbo.dbActivities
	LEFT JOIN dbo.GetCodeLookupDescription ( 'TIMEACTCODE', @UI ) CL1 ON CL1.[cdCode] =  dbActivities.actCode
    where actactive = 1
    ORDER BY actCodeDesc
   END
 END
ELSE
 BEGIN
  print N'Legal Aid'
  if @INCLNULL = 1
   BEGIN
    select null as actCode, dbo.GetCodeLookupDesc('RESOURCE', 'RESNOTSET', @UI) as actCodeDesc, null as actAccCode, null as actChargeable, null as actTemplateMatch, null as actFixedValue, 0 as actCharge, 0 as actCost, 1 as actActive, null as actFixedRateLegal union 
    SELECT     dbo.dbActivities.actCode, COALESCE(CL1.cdDesc, '~' + NULLIF(dbActivities.actCode, '') + '~') AS actCodeDesc, dbo.dbActivities.actAccCode, 
                         dbo.dbActivities.actChargeable, dbo.dbActivities.actTemplateMatch, dbo.dbActivities.actFixedValue, 
                         dbo.dbLegalAidActivities.ActivityCharge as actCharge, actcost = @feecost, dbo.dbActivities.actActive, dbo.dbActivities.actFixedRateLegal
    FROM         dbo.dbActivities INNER JOIN
                       dbo.dbLegalAidActivities ON dbo.dbActivities.actCode = dbo.dbLegalAidActivities.ActivityCode
	LEFT JOIN dbo.GetCodeLookupDescription ( 'TIMEACTCODE', @UI ) CL1 ON CL1.[cdCode] =  dbActivities.actCode
    WHERE dblegalaidActivities.ActivityLegalAidGrade = @feeLAGrade and dblegalaidactivities.ActivityLegalAidCat = @legaidcat
    and actactive = 1
    ORDER BY actCodeDesc
   END
  ELSE
   BEGIN  
    SELECT     dbo.dbActivities.actCode, COALESCE(CL1.cdDesc, '~' + NULLIF(dbActivities.actCode, '') + '~') AS actCodeDesc, dbo.dbActivities.actAccCode, 
                         dbo.dbActivities.actChargeable, dbo.dbActivities.actTemplateMatch, dbo.dbActivities.actFixedRateLegal, dbo.dbActivities.actFixedValue, 
                         dbo.dbLegalAidActivities.ActivityCharge as actCharge, actcost = @feecost, dbo.dbActivities.actActive
    FROM         dbo.dbActivities INNER JOIN
                       dbo.dbLegalAidActivities ON dbo.dbActivities.actCode = dbo.dbLegalAidActivities.ActivityCode
	LEFT JOIN dbo.GetCodeLookupDescription ( 'TIMEACTCODE', @UI ) CL1 ON CL1.[cdCode] =  dbActivities.actCode
    WHERE dblegalaidActivities.ActivityLegalAidGrade = @feeLAGrade and dblegalaidactivities.ActivityLegalAidCat = @legaidcat
    and actactive = 1
    ORDER BY actCodeDesc
   END
 END
