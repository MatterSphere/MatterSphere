using System.Data.SqlClient;
using System.Reflection;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.Settings
{
    public class AboutViewModel : PageViewModel
    {
        public AboutViewModel(MainViewModel mainViewModel)
            : base(mainViewModel)
        {
            Version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

            var connBuilder = new SqlConnectionStringBuilder(mainViewModel.Settings.DbConnection);
            SqlServer = connBuilder.DataSource;
            SqlDatabase = connBuilder.InitialCatalog;

            var sb = new System.Text.StringBuilder();
            foreach (var indexInfo in new DAL.AdoNetRepository.IndexStructureRepository(connBuilder.ConnectionString).GetIndices())
            {
                sb.AppendFormat("{0} ({1}), ", indexInfo.Name, indexInfo.IndexType);
            }
            sb.Length -= 2;
            ElasticsearchIndexes = sb.ToString();
            ElasticsearchServer = mainViewModel.Settings.ElasticsearchServer;
        }

        private string _version;
        public string Version
        {
            get { return _version; }
            set
            {
                if (_version != value)
                {
                    _version = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _sqlServer;
        public string SqlServer
        {
            get { return _sqlServer; }
            set
            {
                if (_sqlServer != value)
                {
                    _sqlServer = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _sqlDatabase;
        public string SqlDatabase
        {
            get { return _sqlDatabase; }
            set
            {
                if (_sqlDatabase != value)
                {
                    _sqlDatabase = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _esServer;
        public string ElasticsearchServer
        {
            get { return _esServer; }
            set
            {
                if (_esServer != value)
                {
                    _esServer = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _esIndexes;
        public string ElasticsearchIndexes
        {
            get { return _esIndexes; }
            set
            {
                if (_esIndexes != value)
                {
                    _esIndexes = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
