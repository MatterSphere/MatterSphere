

CREATE FUNCTION [dbo].[SetSpecialSeed]  ( @Prefix varchar(1), @NoOfLetters tinyint, @NoOfNumbers tinyint, @ID bigint)

RETURNS nvarchar(14)
AS
BEGIN
	DECLARE @ClientNo nvarchar(12)
	-- Error checks
	if @Prefix NOT LIKE '[A-Z]' AND @Prefix NOT LIKE '[1-9]'
		SET @Prefix = ''
	if @NoOfLetters > 6 
		SET @NoOfLetters = 3
	if @NoOfLetters = 0
		SET @NoOfLetters = 3
	if @NoOfNumbers > 6
		SET @NoOfNumbers = 3
	if @NoOfNumbers < 3
		SET @NoOfNumbers = 3

DECLARE @CHARSET Nvarchar(100)
DECLARE @NEXTNUM bigint

      SELECT @CHARSET= coalesce(@Prefix,'') +upper ( ( SELECT left ( coalesce ( clsearch1 , 'XXXXXX' ) , @NoOfLetters ) FROM dbclient WHERE clid = @id ) ) ;

      select @NEXTNUM=coalesce(max(cast(replace(clno,@CHARSET,'') as integer))+1 ,'1') from dbClient where clNo like @CHARSET+'%';

	  select @ClientNo=@CHARSET+replicate('0', @NoOfNumbers - len(@NEXTNUM)) + cast (@NEXTNUM as varchar)
	  
	RETURN @ClientNo
END
