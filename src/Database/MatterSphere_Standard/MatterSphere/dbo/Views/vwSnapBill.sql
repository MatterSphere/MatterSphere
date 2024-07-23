

CREATE VIEW[dbo].[vwSnapBill]
AS
SELECT DATEPART(mm, dbo.dbBillInfo.billDate) AS Month, DATEPART(yyyy, dbo.dbBillInfo.billDate) AS Year, SUM(dbo.dbBillInfo.billProCosts) AS SumFees, 
                      SUM(dbo.dbBillInfo.billPaidDisb) AS SumBillPaidDisb, SUM(dbo.dbBillInfo.billNYPDisb) AS SumBillNYPDisb, dbo.dbFile.filePrincipleID
FROM         dbo.dbFile INNER JOIN
                      dbo.dbBillInfo ON dbo.dbFile.fileID = dbo.dbBillInfo.fileID
GROUP BY DATEPART(mm, dbo.dbBillInfo.billDate), DATEPART(yyyy, dbo.dbBillInfo.billDate), dbo.dbFile.filePrincipleID

GO
GRANT UPDATE
    ON OBJECT::[dbo].[vwSnapBill] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapBill] TO [OMSRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapBill] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[vwSnapBill] TO [OMSAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[vwSnapBill] TO [OMSApplicationRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[vwSnapBill] TO [OMSApplicationRole]
    AS [dbo];

