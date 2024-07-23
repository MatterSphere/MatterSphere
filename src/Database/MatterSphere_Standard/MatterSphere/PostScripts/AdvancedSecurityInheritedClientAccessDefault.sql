IF NOT EXISTS(SELECT * FROM config.PolicyType WHERE PolicyTypeCode = N'LIMITEDFILEDEF')
BEGIN
	DECLARE @ID UNIQUEIDENTIFIER = NEWID()
	EXEC config.CreatePolicyTemplate @policyTypeCode=N'LIMITEDFILEDEF', @isSystemPolicy=0, @name=N'Limited File Default', @usrID=-1, @allowMask=N'0x00000000807F011D80800000000000000000000000000000000000000000000000', @denyMask=N'0x000000160000000000060000000000000000000000000000000000000000000000', @ID=@ID, @IsRemote=0
	EXEC dbo.sprCreateCodeLookup @Type=N'POLICY', @Code=N'LIMITEDFILEDEF', @Description=N'Limited File Default', @Help=N'', @Notes=NULL, @UI=N'{default}', @System=0, @Deletable=1, @AddLink=NULL, @Group=0
END