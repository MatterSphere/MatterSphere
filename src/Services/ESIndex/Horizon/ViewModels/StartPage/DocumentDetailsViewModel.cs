using System.Collections.Generic;
using System.Linq;
using Horizon.Common.Models.Common;
using Horizon.Models.StartPage;
using Horizon.ViewModels.IndexProcess;
using Horizon.ViewModels.IndexReports;

namespace Horizon.ViewModels.StartPage
{
    public class DocumentDetailsViewModel : StartPageItemViewModel
    {
        private List<StartPageInfoItem> _startPageInfo = new List<StartPageInfoItem>
        {
            new StartPageInfoItem(StartPageInfoTypeEnum.DocumentsRead, "Documents Read", "NoteText"),
            new StartPageInfoItem(StartPageInfoTypeEnum.PrecedentsRead, "Precedents Read", "ClipboardText"),
            new StartPageInfoItem(StartPageInfoTypeEnum.Blacklist, "Blacklist", "LinkOff"),
            new StartPageInfoItem(StartPageInfoTypeEnum.IndexingStatus, "Buckets Read", "Contain")
        };

        private readonly StartPageInfoItem _currentStartPageInfo;


        public DocumentDetailsViewModel(MainViewModel main) : base(main)
        {
        }

        public DocumentDetailsViewModel(StartPageInfoTypeEnum startPageInfoType, long total, long? subNumber, string subNumberLabel, MainViewModel main) : base(total, subNumber, subNumberLabel, main)
        {
            _currentStartPageInfo = _startPageInfo.FirstOrDefault(info => info.Type == startPageInfoType);
            if (_currentStartPageInfo != null)
            {
                Title = _currentStartPageInfo.Title;
                Icon = _currentStartPageInfo.Icon;
            }
        }

        protected override void OpenDetails()
        {
            if (_currentStartPageInfo != null)
            {
                switch (_currentStartPageInfo.Type)
                {
                    case StartPageInfoTypeEnum.DocumentsRead:
                        Main.CurrentPage = new DocumentsDataViewModel(Main, ContentableEntityTypeEnum.Document);
                        Main.OpenCrawlProcessMenu();
                        Main.DocumentsReadChecked = true;
                        break;
                    case StartPageInfoTypeEnum.PrecedentsRead:
                        Main.CurrentPage = new DocumentsDataViewModel(Main, ContentableEntityTypeEnum.Precedent);
                        Main.OpenCrawlProcessMenu();
                        Main.PrecedentsReadChecked = true;
                        break;
                    case StartPageInfoTypeEnum.Blacklist:
                        Main.CurrentPage = new BlacklistViewModel(Main);
                        Main.OpenCrawlProcessMenu();
                        Main.BlacklistChecked = true;
                        break;
                    case StartPageInfoTypeEnum.IndexingStatus:
                        Main.CurrentPage = new IndexStatusViewModel(Main);
                        Main.OpenCrawlProcessMenu();
                        Main.ProcessStatusChecked = true;
                        break;
                }
            }
        }
    }
}
