using Newtonsoft.Json;

namespace FWBS.OMS.FinancialTile.Models
{
    class ClientManagementRequest
    {
        [JsonProperty("rowsRequest")]
        public RowsRequestInfo RowsRequest { get; set; }

        [JsonProperty("presentation")]
        public PresentationInfo Presentation { get; set; }

        [JsonProperty("useExistingRun")]
        public bool UseExistingRun { get; set; }

        public class RowsRequestInfo
        {
            [JsonProperty("useCache")]
            public bool UseCache { get; set; }

            [JsonProperty("includeHeader")]
            public bool IncludeHeader { get; set; }

            [JsonProperty("startRow")]
            public int StartRow { get; set; }

            [JsonProperty("rowCount")]
            public int RowCount { get; set; }

            [JsonProperty("startColumn")]
            public int StartColumn { get; set; }

            [JsonProperty("columnCount")]
            public int ColumnCount { get; set; }
        }

        public class PresentationInfo
        {
            [JsonProperty("acrossDimensions")]
            public AcrossDimension[] AcrossDimensions { get; set; }

            [JsonProperty("downDimensions")]
            public DownDimension[] DownDimensions { get; set; }

            [JsonProperty("pageDimensions")]
            public PageDimension[] PageDimensions { get; set; }

            [JsonProperty("boundId")]
            public string BoundId { get; set; }

            [JsonProperty("boundType")]
            public int BoundType { get; set; }

            public class AcrossDimension
            {
                [JsonProperty("type")]
                public int Type { get; set; }

                [JsonProperty("metricAttributes")]
                public MetricAttribute[] MetricAttributes { get; set; }

                [JsonProperty("sortAttributes")]
                public SortAttribute[] SortAttributes { get; set; }

                public class MetricAttribute
                {
                    public MetricAttribute(string id)
                    {
                        Id = id;
                    }

                    [JsonProperty("id")]
                    public string Id { get; set; }
                }

                public class FormatableMetricAttribute : MetricAttribute
                {
                    public FormatableMetricAttribute(string id) : base(id)
                    {
                    }

                    [JsonProperty("format")]
                    public FormatInfo Format { get; set; }

                    public class FormatInfo
                    {
                        [JsonProperty("currencyDisplaySymbol")]
                        public int? CurrencyDisplaySymbol { get; set; }

                        [JsonProperty("currencyPositiveFormat")]
                        public int? CurrencyPositiveFormat { get; set; }

                        [JsonProperty("currencyNegativeFormat")]
                        public int? CurrencyNegativeFormat { get; set; }

                        [JsonProperty("currencyNegativeColor")]
                        public string CurrencyNegativeColor { get; set; }

                        [JsonProperty("currencyDisplayGroupSeperator")]
                        public int? CurrencyDisplayGroupSeperator { get; set; }

                        [JsonProperty("currencyZeroFormat")]
                        public string CurrencyZeroFormat { get; set; }

                        [JsonProperty("numberNegativeFormat")]
                        public int? NumberNegativeFormat { get; set; }

                        [JsonProperty("numberNegativeColor")]
                        public string NumberNegativeColor { get; set; }

                        [JsonProperty("numberDisplayGroupSeperator")]
                        public int? NumberDisplayGroupSeperator { get; set; }

                        [JsonProperty("numberZeroFormat")]
                        public string NumberZeroFormat { get; set; }
                    }
                }

                public class SortAttribute
                {
                    public SortAttribute(string id, bool desc)
                    {
                        Id = id;
                        SortDirection = desc ? -1 : 1;
                    }

                    [JsonProperty("id")]
                    public string Id { get; set; }

                    [JsonProperty("sortDirection")]
                    public int SortDirection { get; set; }
                }
            }

            public class DownDimension
            {
                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("type")]
                public int Type { get; set; }

                [JsonProperty("showSubTotalsWithSingleRows")]
                public bool ShowSubTotalsWithSingleRows { get; set; }

                [JsonProperty("showGroupWithRowsAllZeros")]
                public bool ShowGroupWithRowsAllZeros { get; set; }

                [JsonProperty("showRowsWithAllZeros")]
                public bool ShowRowsWithAllZeros { get; set; }

                [JsonProperty("indent")]
                public bool Indent { get; set; }

                [JsonProperty("displayAttributes")]
                public string[] DisplayAttributes { get; set; }

                [JsonProperty("sortAttributes")]
                public SortAttribute[] SortAttributes { get; set; }

                public class SortAttribute
                {
                    public SortAttribute(string id, int direction)
                    {
                        Id = id;
                        SortDirection = direction;
                    }

                    [JsonProperty("id")]
                    public string Id { get; set; }

                    [JsonProperty("sortDirection")]
                    public int SortDirection { get; set; }
                }
            }

            public class PageDimension
            {
                [JsonProperty("type")]
                public int Type { get; set; }

                [JsonProperty("runs")]
                public string[] Runs { get; set; }

                [JsonProperty("showTotals")]
                public bool ShowTotals { get; set; }
            }
        }
    }
}
