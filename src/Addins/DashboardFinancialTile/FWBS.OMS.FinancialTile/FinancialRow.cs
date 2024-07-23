namespace FWBS.OMS.FinancialTile
{
    class FinancialRow
    {
        public FinancialRow(string number, string name, string hours, string time, string costs, string charges, string total)
        {
            ClientNumber = number;
            ClientName = name;
            WIPHours = hours;
            WIPTime = time;
            WIPCosts = costs;
            WIPCharges = charges;
            WIPTotal = total;
        }

        public string ClientNumber { get; }
        public string ClientName { get; }
        public string WIPHours { get; }
        public string WIPTime { get; }
        public string WIPCosts { get; }
        public string WIPCharges { get; }
        public string WIPTotal { get; }
    }
}
