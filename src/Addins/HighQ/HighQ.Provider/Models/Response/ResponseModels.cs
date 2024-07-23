using Newtonsoft.Json;

namespace FWBS.OMS.HighQ.Models.Response
{
    internal class ResponseModels
    {
        public class HeadModel
        {
            [JsonProperty("headcolumn")]
            public HeadColumn[] HeadColumns { get; set; }
        }

        public class HeadColumn
        {
            [JsonProperty("columnid")]
            public string ColumnId { get; set; }

            [JsonProperty("columnvalue")]
            public string Name { get; set; }
        }

        public class ItemModel
        {
            [JsonProperty("itemid")]
            public int ItemId { get; set; }

            [JsonProperty("column")]
            public ColumnModel[] Columns { get; set; }
        }

        public class ColumnModel
        {
            [JsonProperty("attributecolumnid")]
            public int ColumnId { get; set; }

            [JsonProperty("displaydata")]
            public DisplayDataModel DisplayData { get; set; }
        }

        public class DisplayDataModel
        {
            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("choices")]
            public ChoicesModel Choices { get; set; }

            [JsonProperty("folders")]
            public FoldersModel Folders { get; set; }

            public string DisplayValue
            {
                get
                {
                    if (Choices != null)
                    {
                        return Choices.Choice.Label;
                    }

                    if (Folders != null)
                    {
                        return Folders.Folder.FolderId.ToString();
                    }

                    return Value;
                }
            }
        }

        public class ChoicesModel
        {
            [JsonProperty("choice")]
            public ChoiceModel Choice { get; set; }

            public override string ToString()
            {
                return Choice.ToString();
            }
        }

        public class ChoiceModel
        {
            [JsonProperty("label")]
            public string Label { get; set; }

            public override string ToString()
            {
                return Label;
            }
        }

        public class FoldersModel
        {
            [JsonProperty("folder")]
            public FolderModel Folder { get; set; }

            public override string ToString()
            {
                return Folder.ToString();
            }
        }

        public class FolderModel
        {
            [JsonProperty("folderid")]
            public int FolderId { get; set; }

            [JsonProperty("foldername")]
            public string FolderName { get; set; }

            public override string ToString()
            {
                return FolderName;
            }
        }

        public class LookupUsersModel
        {
            [JsonProperty("lookupuser")]
            public LookupUserModel LookupUser { get; set; }

            public override string ToString()
            {
                return LookupUser.ToString();
            }
        }

        public class LookupUserModel
        {
            [JsonProperty("userdisplayname")]
            public string UserDisplayName { get; set; }

            public override string ToString()
            {
                return UserDisplayName;
            }
        }
    }
}
