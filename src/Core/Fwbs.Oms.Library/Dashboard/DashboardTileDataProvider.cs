using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FWBS.Common;

namespace FWBS.OMS.Dashboard
{
    public static class DashboardTileDataProvider
    {
        public static List<MatterRow> GetMatterList(string query, string filter, string orderby, int page, out int total, int size = 50)
        {
            Session.CurrentSession.CheckLoggedIn();
            FeeEarner feeEarner = Session.CurrentSession.CurrentFeeEarner;
            var connection = Session.CurrentSession.Connection;
            var parameters = new IDataParameter[6]
            {
                connection.CreateParameter("FeeEarnerID", typeof(int), feeEarner != null ? (object)feeEarner.ID : DBNull.Value),
                connection.CreateParameter("MAX_RECORDS", size),
                connection.CreateParameter("Query", query),
                connection.CreateParameter("Filter", filter),
                connection.CreateParameter("PageNo", page),
                connection.CreateParameter("OrderBY", orderby)
            };
            DataSet dataSet = connection.ExecuteProcedureDataSet("dbo.dshMatterList", new[] { "dashboardCells" }, parameters);
            var files = new List<MatterRow>();
            total = 0;

            if (dataSet.Tables[0].Rows.Any())
            {
                total = ConvertDef.ToInt32(dataSet.Tables[0].Rows[0]["Total"], 0);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    DateTime reviewDate;
                    bool reviewDateConverted = DateTime.TryParse(row["fileReviewDate"].ToString(), out reviewDate);

                    var item = new MatterRow(
                        ConvertDef.ToInt64(row["clID"], 0),
                        ConvertDef.ToInt64(row["fileID"], 0),
                        row["clNo"].ToString(),
                        row["fileNo"].ToString(),
                        row["fileDesc"].ToString())
                    {
                        ReviewDate = reviewDateConverted ? reviewDate : (DateTime?)null
                    };
                    files.Add(item);
                }
            }

            return files;
        }

        public static List<TaskRow> GetTasks(string query, string filter, string orderby, int page, out int total, int size = 50)
        {
            Session.CurrentSession.CheckLoggedIn();
            var connection = Session.CurrentSession.Connection;
            var parameters = new IDataParameter[7]
            {
                connection.CreateParameter("UI", Session.CurrentSession.DefaultCulture),
                connection.CreateParameter("MAX_RECORDS", size),
                connection.CreateParameter("PageNo", page),
                connection.CreateParameter("Query", query),
                connection.CreateParameter("USER", Session.CurrentSession.CurrentUser.ID),
                connection.CreateParameter("Filter", filter),
                connection.CreateParameter("OrderBY", orderby)
            };
            DataSet dataSet = connection.ExecuteProcedureDataSet("dbo.dshTasks", new[] { "dashboardCells" }, parameters);
            var tasks = new List<TaskRow>();
            total = 0;

            if (dataSet.Tables[0].Rows.Any())
            {
                total = ConvertDef.ToInt32(dataSet.Tables[0].Rows[0]["Total"], 0);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    DateTime completedDate;
                    bool completedDateConverted = DateTime.TryParse(row["tskCompleted"].ToString(), out completedDate);

                    var item = new TaskRow(
                        ConvertDef.ToInt64(row["tskID"], 0),
                        ConvertDef.ToInt64(row["clID"], 0),
                        ConvertDef.ToInt64(row["fileID"], 0),
                        row["clNo"].ToString(),
                        row["fileNo"].ToString(),
                        row["tskDesc"].ToString(),
                        ConvertDef.ToDateTime(row["tskDue"], DateTime.MinValue),
                        row["tskTypeDesc"].ToString())
                    {
                        StatusCode = row["tskStatusCode"].ToString(),
                        StatusDescription = row["tskStatusDesc"].ToString(),
                        Team = row["tskTeamName"].ToString(),
                        CompletedDate = completedDateConverted ? completedDate : (DateTime?)null
                    };

                    int userId;
                    if (int.TryParse(row["usrID"].ToString(), out userId))
                    {
                        item.SetAssignedTo(userId, row["AssignedTo"].ToString());
                    }

                    item.SetCreatedBy(ConvertDef.ToInt32(row["tskCreatedBy"], 0), row["CreatedBy"].ToString());
                    tasks.Add(item);
                }
            }

            return tasks;
        }

        public static List<FavoriteRow> GetFavorites(string query, string filter, string orderby, int page, out int total, int size = 50)
        {
            Session.CurrentSession.CheckLoggedIn();
            var connection = Session.CurrentSession.Connection;
            var parameters = new IDataParameter[5]
            {
                connection.CreateParameter("MAX_RECORDS", size),
                connection.CreateParameter("PageNo", page),
                connection.CreateParameter("Filter", filter),
                connection.CreateParameter("Query", string.IsNullOrWhiteSpace(query) ? null : $"%{query}%"),
                connection.CreateParameter("OrderBY", orderby)
            };
            DataSet dataSet = connection.ExecuteProcedureDataSet("dbo.dshRecentFavourites", new[] { "dashboardCells" }, parameters);
            var favorites = new List<FavoriteRow>();
            total = 0;

            if (dataSet.Tables[0].Rows.Any())
            {
                total = ConvertDef.ToInt32(dataSet.Tables[0].Rows[0]["Total"], 0);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var itemType = GetFavoriteItemType(row["usrFavType"].ToString());
                    string description = null;
                    switch (itemType)
                    {
                        case FavoriteRow.FavoriteItemType.Client:
                            description = row["clName"].ToString();
                            break;
                        case FavoriteRow.FavoriteItemType.File:
                            description = row["fileDesc"].ToString();
                            break;
                        case FavoriteRow.FavoriteItemType.Precedent:
                            description = row["precDesc"].ToString();
                            break;
                    }

                    if ((itemType == FavoriteRow.FavoriteItemType.Client ||
                         itemType == FavoriteRow.FavoriteItemType.File) && string.IsNullOrEmpty(row["clNo"].ToString()))
                    {
                        continue;
                    }

                    var item = new FavoriteRow(
                        id: ConvertDef.ToInt32(row["FavID"], 0),
                        description: description,
                        type: itemType,
                        combo: row["combo"].ToString(),
                        modified: ConvertDef.ToDateTime(row["Updated"], DateTime.MinValue),
                        extension: row["precExtension"].ToString())
                    {
                        ClientNo = row["clNo"].ToString(),
                        FileNo = row["fileNo"].ToString(),
                    };

                    long clientId;
                    if (long.TryParse(row["clId"].ToString(), out clientId))
                    {
                        item.ClientId = clientId;
                    }

                    long fileId;
                    if (long.TryParse(row["fileID"].ToString(), out fileId))
                    {
                        item.FileId = fileId;
                    }

                    long precId;
                    if (long.TryParse(row["PrecID"].ToString(), out precId))
                    {
                        item.PrecedentId = precId;
                    }

                    favorites.Add(item);
                }
            }

            return favorites;
        }

        public static List<KeyDateRow> GetKeyDates(string query, int page, out int total, int size = 50)
        {
            Session.CurrentSession.CheckLoggedIn();
            var connection = Session.CurrentSession.Connection;
            var parameters = new IDataParameter[3]
            {
                connection.CreateParameter("MAX_RECORDS", size),
                connection.CreateParameter("PageNo", page),
                connection.CreateParameter("Query", string.IsNullOrWhiteSpace(query) ? null : $"%{query}%")
            };
            DataSet dataSet = connection.ExecuteProcedureDataSet("dbo.dshKeyDates", new[] { "dashboardCells" }, parameters);
            var keyDates = new List<KeyDateRow>();
            total = 0;

            if (dataSet.Tables[0].Rows.Any())
            {
                total = ConvertDef.ToInt32(dataSet.Tables[0].Rows[0]["Total"], 0);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var item = new KeyDateRow(
                        id: ConvertDef.ToInt32(row["kdID"], 0),
                        date: ConvertDef.ToDateTime(row["kdDate"], DateTime.MinValue).ToLocalTime(),
                        description: row["kdDesc"].ToString(),
                        fileId: ConvertDef.ToInt64(row["fileID"], 0))
                    {
                        KeyDateType = row["kdtypedesc"].ToString(),
                        ClientNo = row["clNo"].ToString(),
                        FileNo = row["fileNo"].ToString(),
                    };

                    keyDates.Add(item);
                }
            }

            return keyDates;
        }

        public static List<AppointmentRow> GetAppointments(DateTime start, DateTime end)
        {
            Session.CurrentSession.CheckLoggedIn();
            var connection = Session.CurrentSession.Connection;
            var parameters = new IDataParameter[3]
            {
                connection.CreateParameter("UI", Session.CurrentSession.DefaultCulture),
                connection.CreateParameter("StarDT", start),
                connection.CreateParameter("EndDT", end)
            };
            DataSet dataSet = connection.ExecuteProcedureDataSet("dbo.dshCalendar", new[] { "dashboardCells" }, parameters);
            var terminology = Session.CurrentSession.Terminology;
            var appointments = new List<AppointmentRow>();

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var item = new AppointmentRow(
                    id: ConvertDef.ToInt64(row["appID"], 0),
                    type: terminology.Parse(row["AppTypeDesc"].ToString(), false),
                    start: ConvertToLocalDate(ConvertDef.ToDateTime(row["appDate"], DateTime.MinValue), row["appTimeZone"].ToString()),
                    end: ConvertToLocalDate(ConvertDef.ToDateTime(row["appEndDate"], DateTime.MinValue), row["appTimeZone"].ToString()),
                    description: row["appDesc"].ToString(),
                    location: row["appLocation"].ToString(),
                    allDay: ConvertDef.ToBoolean(row["appAllDay"], false));

                appointments.Add(item);
            }

            return appointments;
        }

        private static DateTime ConvertToLocalDate(DateTime date, string timeZoneInfo)
        {
            TimeZoneInfo tzInfo;
            try
            {
                tzInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneInfo);
            }
            catch
            {
                tzInfo = TimeZoneInfo.Local;
            }

            return TimeZoneInfo.ConvertTime(DateTime.SpecifyKind(date, DateTimeKind.Unspecified), TimeZoneInfo.Utc, tzInfo);
        }

        private static FavoriteRow.FavoriteItemType GetFavoriteItemType(string value)
        {
            switch (value)
            {
                case "CLINETFT":
                    return FavoriteRow.FavoriteItemType.Client;
                case "CLINETFILEFT":
                    return FavoriteRow.FavoriteItemType.File;
                case "PRECFAV":
                    return FavoriteRow.FavoriteItemType.Precedent;
            }

            throw new ArgumentException();
        }
    }
}
