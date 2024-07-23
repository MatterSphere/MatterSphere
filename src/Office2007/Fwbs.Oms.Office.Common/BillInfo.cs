using System;
using System.Runtime.InteropServices;

namespace Fwbs.Oms.Office.Common
{
    [Guid("F0C6BDB7-9C00-44af-8222-FF8A732F45BC"), ProgId("Fwbs.Oms.Office.Common.BillInfo"), ComVisible(true)]
    public sealed class BillInfo
    {

        FWBS.OMS.BillingInfo _bill = null;

        private BillInfo()
        { }

        internal BillInfo(long assocID)
        {
            _bill = new FWBS.OMS.BillingInfo(FWBS.OMS.Associate.GetAssociate(assocID));
        }


        public string BillNo
        {
            get
            {
                return _bill.BillNo;
            }
        }

        public DateTime BillDate
        {
            get
            {
                return _bill.BillDate;
            }
            set
            {
                _bill.BillDate = value;
            }
        }

        public string Category
        {
            get
            {
                return _bill.Category;
            }
            set
            {
                _bill.Category = value;
            }
        }

        public Single VATRate
        {
            get
            {
                return _bill.VATRate;
            }
        }

        public Decimal OnAccountAmount
        {
            get
            {
                return _bill.OnAccountAmount;
            }
            set
            {
                _bill.OnAccountAmount = value;
            }
        }

        public Decimal PaidDisbursments
        {
            get
            {
                return _bill.PaidDisbursments;
            }
            set
            {
                _bill.PaidDisbursments = value;
            }
        }

        public Decimal UnpaidDisbursments
        {
            get
            {
                return _bill.UnpaidDisbursments;
            }
            set
            {
                _bill.UnpaidDisbursments = value;
            }
        }

        public Decimal VATAmount
        {
            get
            {
                return _bill.VATAmount;
            }
            set
            {
                _bill.VATAmount = value;
            }
        }

        public Decimal ProfessionalFees
        {
            get
            {
                return _bill.ProfessionalFees;
            }
            set
            {
                _bill.ProfessionalFees = value;
            }
        }

        public Decimal TotalDisbursments
        {
            get
            {
                return _bill.TotalDisbursments;
            }
        }

        public Decimal TotalCost
        {
            get
            {
                return _bill.TotalCost;
            }
        }

        public Decimal NetCost
        {
            get
            {
                return _bill.NetCost;
            }
        }

        public Decimal TotalOutstanding
        {
            get
            {
                return _bill.TotalOutstanding;
            }
        }

        public void Update()
        {
            _bill.Update();
        }
    }
}
