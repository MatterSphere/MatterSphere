using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.MatterList
{
    public class BudgetBuilder
    {
        private BudgetPopup _container;
        private OMSFile _omsFile;

        public BudgetBuilder(BudgetPopup container, OMSFile omsFile)
        {
            _container = container;
            _omsFile = omsFile;
        }

        public void BuildStandardView()
        {
            _container.OMSFileNo = $"{_omsFile.Client.ClientNo}-{_omsFile.FileNo}";
            _container.OMSFileId = _omsFile.ID;

            var total = new ucBudgetItem
            {
                Title = ResourceLookup.GetLookupText("TOTALWIPBTD", "Total : ", "Work in Progress + Balance to Date"),
                Value = string.Format(_omsFile.CurrencyFormat, "{0:C}", _omsFile.TimeWIP + _omsFile.TimeBilled)
            };
            _container.AddBudgetItem(total, true);

            var matterBTD = new ucBudgetItem
            {
                Title = ResourceLookup.GetLookupText("FILEBTD", "%FILE% BTD:", "%FILE% Balance to Date"),
                Value = string.Format(_omsFile.CurrencyFormat, "{0:C}", _omsFile.TimeBilled)
            };
            _container.AddBudgetItem(matterBTD, true);

            var matterWIP = new ucBudgetItem
            {
                Title = ResourceLookup.GetLookupText("CLIENTWIP", "%CLIENT% WIP:", "%CLIENT% Work in Progress"),
                Value = string.Format(_omsFile.CurrencyFormat, "{0:C}", _omsFile.Client.TimeWIP)
            };
            _container.AddBudgetItem(matterWIP, true);

            var noCharge = new ucBudgetItem
            {
                Title = CodeLookup.GetLookup("FTCLDESC", _omsFile.FundingType.CreditLimitCode),
                Value = string.Format(_omsFile.CurrencyFormat, "{0:C}", _omsFile.CreditLimit)
            };
            _container.AddBudgetItem(noCharge, false);

            var available = new ucBudgetItem
            {
                Title = ResourceLookup.GetLookupText("AVAILABLE", "Available : ", "Credit Limit - Work in Progress + Balance to Date"),
                Value = string.Format(_omsFile.CurrencyFormat, "{0:C}", _omsFile.CreditLimit - (_omsFile.TimeWIP + _omsFile.TimeBilled))
            };
            available.SetHighlight();

            _container.AddBudgetItem(available, false);
        }
    }
}
