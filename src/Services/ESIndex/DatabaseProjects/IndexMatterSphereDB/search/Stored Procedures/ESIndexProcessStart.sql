CREATE PROCEDURE search.ESIndexProcessStart
AS
SET NOCOUNT ON;
INSERT INTO search.ESIndexProcess(FinishDate)
VALUES(NULL);
