namespace Horizon.Models.IndexReports
{
    public class IFilterInfo
    {
        public IFilterInfo(string company, string fileName, string path)
        {
            Company = company;
            FileName = fileName;
            Path = path;
        }

        public string Company { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
    }
}
