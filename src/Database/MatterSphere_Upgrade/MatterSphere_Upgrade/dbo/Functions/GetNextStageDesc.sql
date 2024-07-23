

CREATE  FUNCTION [dbo].[GetNextStageDesc] (@fileid bigint)  
RETURNS nvarchar(2000) AS  
BEGIN 
declare @SQL nvarchar(1000)
declare @NEXTSTAGE smallint
declare @TMPSTRING nvarchar(150)
 
select @nextstage = coalesce(MSNextDueStage,0) from dbmsdata_oms2k where fileid = @fileid
 
select
 @tmpstring = case 
  when @nextstage = 0 then 'All stages save been completed'
  when @nextstage = 1 then MSStage1Desc
  when @nextstage = 2 then MSStage2Desc
  when @nextstage = 3 then MSStage3Desc
  when @nextstage = 4 then MSStage4Desc
  when @nextstage = 5 then MSStage5Desc
  when @nextstage = 6 then MSStage6Desc
  when @nextstage = 7 then MSStage7Desc
  when @nextstage = 8 then MSStage8Desc
  when @nextstage = 9 then MSStage9Desc
  when @nextstage = 10 then MSStage10Desc
  when @nextstage = 11 then MSStage11Desc
  when @nextstage = 12 then MSStage12Desc
  when @nextstage = 13 then MSStage13Desc
  when @nextstage = 14 then MSStage14Desc
  when @nextstage = 15 then MSStage15Desc
  when @nextstage = 16 then MSStage16Desc
  when @nextstage = 17 then MSStage17Desc
  when @nextstage = 18 then MSStage18Desc
  when @nextstage = 19 then MSStage19Desc
  when @nextstage = 20 then MSStage20Desc
  when @nextstage = 21 then MSStage21Desc
  when @nextstage = 22 then MSStage22Desc
  when @nextstage = 23 then MSStage23Desc
  when @nextstage = 24 then MSStage24Desc
  when @nextstage = 25 then MSStage25Desc
  when @nextstage = 26 then MSStage26Desc
  when @nextstage = 27 then MSStage27Desc
  when @nextstage = 28 then MSStage28Desc
  when @nextstage = 29 then MSStage29Desc
  when @nextstage = 30 then MSStage30Desc
 end
 from 
  dbmsconfig_oms2k C 
 inner join 
  dbMSData_oms2k D on C.mscode = D.mscode
 where 
  D.fileid =  @fileid
--if @nextstage >0 set @tmpstring = @tmpstring +  ' that is due for completion on '
return @tmpstring
end

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNextStageDesc] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNextStageDesc] TO [OMSAdminRole]
    AS [dbo];

