using System.Collections.Generic;
using System.Linq;
using Horizon.Common.Models.Repositories.Blacklist;
using Horizon.Models.IndexReports;
using Horizon.TestiFilter.Common;
using Horizon.ViewModels.IndexReports;

namespace Horizon.Providers
{
    public class BusinessRulesEngineProvider
    {
        private List<BlacklistItem> _blacklist;
        private IFilterProvider _iFilterProvider;

        public BusinessRulesEngineProvider(List<BlacklistItem> blacklist)
        {
            _blacklist = blacklist;
            _iFilterProvider = new IFilterProvider();
        }

        public BREData GetBREData(string documentType, long failed)
        {
            var blacklist = _blacklist.Where(it => it.Extension == documentType).ToList();
            var data = new BREData()
            {
                Blacklist = blacklist
            };

            var iFilter = _iFilterProvider.GetFilter($".{documentType}");
            if (iFilter != null)
            {
                data.IFilterInfo = new IFilterInfo(iFilter.Company, iFilter.FileName, iFilter.Path);
            }

            var withErrorsMessage = "Some files have not been indexed";

            if (failed == 0)
            {
                data.InfoMessage = "All files have been successfully indexed";
                data.Status = "WithoutErrors";
                return data;
            }

            if (blacklist.Any(it => it.FullExtension))
            {
                data.InfoMessage = withErrorsMessage;
                data.RecommendationMessage = "This file extension is on the Blacklist. If you want to index files with this extension, please remove the file extension from the Blacklist.";
                data.Status = "WithErrorsAndInBlacklist";
                return data;
            }

            if (blacklist.Any())
            {
                if (iFilter == null)
                {
                    data.InfoMessage = withErrorsMessage;
                    data.RecommendationMessage = "The system does not have an iFilter to support this extension. Without the appropriate iFilter, the system cannot read the content of this file.";
                    data.Status = "WithErrorsAndWithoutiFilter";
                }
                else
                {
                    data.InfoMessage = withErrorsMessage;
                    data.RecommendationMessage = "Some files you are attempting to index may be on the Blacklist. Please check the details by double clicking on the current row to review errors.";
                    data.Status = "WithErrorsAndPartiallyInBlacklist";
                }

                return data;
            }

            if (iFilter != null)
            {
                data.InfoMessage = withErrorsMessage;
                data.RecommendationMessage = $"The system has an iFilter for this extension. Perhaps, the iFilter {data.IFilterInfo.FileName} by {data.IFilterInfo.Company} reads content with errors. You could try to find some another iFilter for this extension.";
                data.Status = "WithErrorsAndWithiFilter";
            }
            else
            {
                data.InfoMessage = withErrorsMessage;
                data.RecommendationMessage = "The system does not have an iFilter for this file extension. Without an iFilter, the system cannot read content of this file. If you do not wish to index files with this extension, you can add this extension to the Blacklist.";
                data.Status = "WithErrorsAndWithoutiFilter";
            }

            return data;
        }
    }
}
