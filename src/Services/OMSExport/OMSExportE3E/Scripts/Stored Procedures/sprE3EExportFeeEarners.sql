
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sprE3EExportFeeEarners]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sprE3EExportFeeEarners]
GO


CREATE PROCEDURE [dbo].[sprE3EExportFeeEarners]
AS
RAISERROR ( 'sprE3EExportFeeEarners not used currently', 16 , 1 )