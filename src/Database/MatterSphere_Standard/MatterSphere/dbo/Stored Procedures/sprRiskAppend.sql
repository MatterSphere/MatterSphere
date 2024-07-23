

CREATE PROCEDURE [dbo].[sprRiskAppend]
(
 @RISKHEADERID bigint, 
 @RISKCODE ucodelookup, 
 @FILEID bigint, 
 @RISKCREATEDBY bigint, 
 @RISKDOCID bigint,
 @RISKTOTALSCORE int
 
)
AS
 
if (@fileid is not null)
begin
  
 insert into dbRiskHeader
  (
   riskHeaderID,
   riskCode,
   fileID,
   riskCreatedBy,
   riskDocID,
   riskTotalScore)
 values
  (
   @riskHeaderID,
   @riskCode,
   @fileID,
   @riskCreatedBy,
   @riskDocID,
   @riskTotalScore)
end  
 
select @@rowcount, @@rowcount

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRiskAppend] TO [OMSRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[sprRiskAppend] TO [OMSAdminRole]
    AS [dbo];

