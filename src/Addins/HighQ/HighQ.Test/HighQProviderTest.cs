using System.Net;
using FWBS.OMS.HighQ.Models;
using FWBS.OMS.HighQ.Models.Response;
using FWBS.OMS.HighQ.Providers;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace HighQ.Test
{
    [TestFixture]
    public class HighQProviderTest
    {
        private readonly int _iSheetId = 1;
        private readonly string _clientNo = "F9";
        private readonly string _fileNo = "8";
        private readonly string _clientColumn = "MSClientNo";
        private readonly string _fileColumn = "MSFileNo";
        private readonly string _folderColumn = "ExportFolder";
        private readonly int _folderColumnId = 4;

        [Test]
        public void TestGetFolderId()
        {
            var restClientMock = new Mock<IRestClient>();
            restClientMock.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns<RestRequest>(GetRestResponse);
            var hqProvider = new HighQProvider(null, restClientMock.Object);
            var folderDetails = new FolderDetails(_iSheetId, _clientNo, _fileNo, _clientColumn, _fileColumn, _folderColumn);

            var folderId = hqProvider.GetFolderId(folderDetails);

            Assert.AreEqual(_folderColumnId, folderId);
        }

        private IRestResponse GetRestResponse(RestRequest request)
        {
            if (request.Resource == $"/{1}/isheets/{_iSheetId}/columns")
            {
                var model = new ColumnsResponse
                {
                    Columns = new []
                    {
                        new ColumnsResponse.ColumnModel
                        {
                            Id = 1,
                            Title = _clientColumn,
                            Type = "Single line text"
                        },
                        new ColumnsResponse.ColumnModel
                        {
                            Id = 2,
                            Title = _fileColumn,
                            Type = "Single line text"
                        },
                        new ColumnsResponse.ColumnModel
                        {
                            Id = 3,
                            Title = _folderColumn,
                            Type = "Folder link"
                        }
                    }
                };
                return new RestResponse
                {
                    Content = JsonConvert.SerializeObject(model),
                    StatusCode = HttpStatusCode.OK
                };
            }

            if (request.Resource == $"/{3}/isheet/{_iSheetId}/search")
            {
                var model = new SearchISheetItemResponse
                {
                    Content = new SearchISheetItemResponse.ContentModel
                    {
                        Head = new ResponseModels.HeadModel
                        {
                            HeadColumns = new ResponseModels.HeadColumn[0]
                        },
                        Data = new SearchISheetItemResponse.DataModel
                        {
                            Items = new ResponseModels.ItemModel[]
                            {
                                new ResponseModels.ItemModel()
                                {
                                    Columns = new[]
                                    {
                                        new ResponseModels.ColumnModel
                                        {
                                            ColumnId = 3,
                                            DisplayData = new ResponseModels.DisplayDataModel
                                            {
                                                Folders = new ResponseModels.FoldersModel
                                                {
                                                    Folder = new ResponseModels.FolderModel
                                                    {
                                                        FolderId = _folderColumnId
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        TotalRecordCount = 1
                    }
                };

                return new RestResponse
                {
                    Content = JsonConvert.SerializeObject(model),
                    StatusCode = HttpStatusCode.OK
                };
            }

            return new RestResponse();
        }
    }
}
