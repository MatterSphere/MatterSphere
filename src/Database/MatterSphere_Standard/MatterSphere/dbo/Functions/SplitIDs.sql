

CREATE FUNCTION [dbo].[SplitIDs]
(
   @assocIDs   VARCHAR(MAX),
   @separator CHAR(1)
)
RETURNS TABLE
WITH SCHEMABINDING
AS
   RETURN ( SELECT ID = CONVERT(bigint, ID) FROM ( 
     SELECT ID = x.i.value('(./text())[1]', 'bigint') FROM ( 
       SELECT [XML] = CONVERT(XML, '<i>' + REPLACE(@assocIDs, @separator, '</i><i>') 
       + '</i>').query('.') ) AS a CROSS APPLY [XML].nodes('i') AS x(i)) AS y
     WHERE ID IS NOT NULL
   );


GO
GRANT UPDATE
    ON OBJECT::[dbo].[SplitIDs] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[SplitIDs] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[SplitIDs] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[SplitIDs] TO [OMSApplicationRole]
    AS [dbo];

