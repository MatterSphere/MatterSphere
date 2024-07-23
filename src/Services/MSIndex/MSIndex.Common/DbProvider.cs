using System;
using System.Data;
using System.Data.SqlClient;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;

namespace MSIndex.Common
{
    public class DbProvider : IDbProvider
    {
        private const string ADDRESSES_INDEX_SP = "MergeAddress";
        private const string APPOINTMENTS_INDEX_SP = "MergeAppointment";
        private const string ASSOCIATES_INDEX_SP = "MergeAssociate";
        private const string CONTACTS_INDEX_SP = "MergeContact";
        private const string CLIENTS_INDEX_SP = "MergeClient";
        private const string DOCUMENTS_INDEX_SP = "MergeDocument";
        private const string FILES_INDEX_SP = "MergeFile";
        private const string PRECEDENTS_INDEX_SP = "MergePrecedent";
        private const string TASKS_INDEX_SP = "MergeTask";
        private const string USERS_INDEX_SP = "MergeUsers";

        private readonly string _connection;
        
        public DbProvider(string connection)
        {
            _connection = connection;
        }
        
        public void Index(MSAddress[] addresses)
        {
            using (var dataTable = new DataTable("Table"))
            {
                dataTable.Columns.Add(new DataColumn("mattersphereid", typeof(long)));
                dataTable.Columns.Add(new DataColumn("sc", typeof(string)));
                dataTable.Columns.Add(new DataColumn("modifieddate", typeof(DateTime)));
                dataTable.Columns.Add(new DataColumn("ugdp", typeof(string)));
                dataTable.Columns.Add(new DataColumn("op", typeof(char)));

                foreach (var address in addresses)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["mattersphereid"] = address.MatterSphereId;
                    dataRow["sc"] = address.Address;
                    dataRow["modifieddate"] = address.ModifiedDate;
                    dataRow["ugdp"] = address.UGDP;
                    dataRow["op"] = address.Operation;

                    dataTable.Rows.Add(dataRow);
                }

                SaveTable(dataTable, ADDRESSES_INDEX_SP);
            }
        }

        public void Index(MSAppointment[] appointments)
        {
            using (var dataTable = new DataTable("Table"))
            {
                dataTable.Columns.Add(new DataColumn("mattersphereid", typeof(long)));
                dataTable.Columns.Add(new DataColumn("[file-id]", typeof(long)));
                dataTable.Columns.Add(new DataColumn("[client-id]", typeof(long)));
                var clientIdColumn = dataTable.Columns.Add("[associate-id]", typeof(long));
                clientIdColumn.AllowDBNull = true;
                var documentIdColumn = dataTable.Columns.Add("[document-id]", typeof(long));
                documentIdColumn.AllowDBNull = true;
                dataTable.Columns.Add(new DataColumn("appointmentType", typeof(string)));
                dataTable.Columns.Add(new DataColumn("appDesc", typeof(string)));
                dataTable.Columns.Add(new DataColumn("appLocation", typeof(string)));
                dataTable.Columns.Add(new DataColumn("modifieddate", typeof(DateTime)));
                dataTable.Columns.Add(new DataColumn("ugdp", typeof(string)));
                dataTable.Columns.Add(new DataColumn("op", typeof(char)));
                
                foreach (var appointment in appointments)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["mattersphereid"] = appointment.MatterSphereId;
                    dataRow["[file-id]"] = appointment.FileId;
                    dataRow["[client-id]"] = appointment.ClientId;
                    dataRow["[associate-id]"] = (object)appointment.AssociateId.HasValue?? DBNull.Value;
                    dataRow["[document-id]"] = (object)appointment.DocumentId.HasValue ?? DBNull.Value;
                    dataRow["appointmentType"] = appointment.AppointmentType;
                    dataRow["appDesc"] = appointment.Description;
                    dataRow["appLocation"] = appointment.Location;
                    dataRow["modifieddate"] = appointment.ModifiedDate;
                    dataRow["ugdp"] = appointment.UGDP;
                    dataRow["op"] = appointment.Operation;

                    dataTable.Rows.Add(dataRow);
                }

                SaveTable(dataTable, APPOINTMENTS_INDEX_SP);
            }
        }

        public void Index(MSAssociate[] associates)
        {
            using (var dataTable = new DataTable("Table"))
            {
                dataTable.Columns.Add(new DataColumn("mattersphereid", typeof(long)));
                dataTable.Columns.Add(new DataColumn("[file-id]", typeof(long)));
                dataTable.Columns.Add(new DataColumn("[contact-id]", typeof(long)));
                dataTable.Columns.Add(new DataColumn("associateType", typeof(string)));
                dataTable.Columns.Add(new DataColumn("assocHeading", typeof(string)));
                dataTable.Columns.Add(new DataColumn("assocSalut", typeof(string)));
                dataTable.Columns.Add(new DataColumn("assocAddressee", typeof(string)));
                dataTable.Columns.Add(new DataColumn("assocNotes", typeof(string)));
                dataTable.Columns.Add(new DataColumn("modifieddate", typeof(DateTime)));
                dataTable.Columns.Add(new DataColumn("ugdp", typeof(string)));
                dataTable.Columns.Add(new DataColumn("op", typeof(char)));

                foreach (var associate in associates)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["mattersphereid"] = associate.MatterSphereId;
                    dataRow["[file-id]"] = associate.FileId;
                    dataRow["[contact-id]"] = associate.ContactId;
                    dataRow["associateType"] = associate.AssociateType;
                    dataRow["assocHeading"] = associate.Heading;
                    dataRow["assocSalut"] = associate.Salutation;
                    dataRow["assocAddressee"] = associate.Addressee;
                    dataRow["assocNotes"] = associate.Notes;
                    dataRow["modifieddate"] = associate.ModifiedDate;
                    dataRow["ugdp"] = associate.UGDP;
                    dataRow["op"] = associate.Operation;
                   
                    dataTable.Rows.Add(dataRow);
                }

                SaveTable(dataTable, ASSOCIATES_INDEX_SP);
            }
        }

        public void Index(MSClient[] clients)
        {
            using (var dataTable = new DataTable("Table"))
            {
                dataTable.Columns.Add(new DataColumn("mattersphereid", typeof(long)));
                dataTable.Columns.Add(new DataColumn("[address-id]", typeof(long)));
                dataTable.Columns.Add(new DataColumn("clientType", typeof(string)));
                dataTable.Columns.Add(new DataColumn("clName", typeof(string)));
                dataTable.Columns.Add(new DataColumn("clNo", typeof(string)));
                dataTable.Columns.Add(new DataColumn("clNotes", typeof(string)));
                dataTable.Columns.Add(new DataColumn("modifieddate", typeof(DateTime)));
                dataTable.Columns.Add(new DataColumn("ugdp", typeof(string)));
                dataTable.Columns.Add(new DataColumn("op", typeof(char)));

                foreach (var client in clients)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["mattersphereid"] = client.MatterSphereId;
                    dataRow["[address-id]"] = client.AddressId;
                    dataRow["clientType"] = client.ClientType;
                    dataRow["clName"] = client.Name;
                    dataRow["clNo"] = client.Number;
                    dataRow["clNotes"] = client.Notes;
                    dataRow["modifieddate"] = client.ModifiedDate;
                    dataRow["ugdp"] = client.UGDP;
                    dataRow["op"] = client.Operation;

                    dataTable.Rows.Add(dataRow);
                }

                SaveTable(dataTable, CLIENTS_INDEX_SP);
            }
        }

        public void Index(MSContact[] contacts)
        {
            using (var dataTable = new DataTable("Table"))
            {
                dataTable.Columns.Add(new DataColumn("mattersphereid", typeof(long)));
                dataTable.Columns.Add(new DataColumn("[address-id]", typeof(long)));
                dataTable.Columns.Add(new DataColumn("contactType", typeof(string)));
                dataTable.Columns.Add(new DataColumn("contName", typeof(string)));
                dataTable.Columns.Add(new DataColumn("contNotes", typeof(string)));
                dataTable.Columns.Add(new DataColumn("modifieddate", typeof(DateTime)));
                dataTable.Columns.Add(new DataColumn("ugdp", typeof(string)));
                dataTable.Columns.Add(new DataColumn("op", typeof(char)));

                foreach (var contact in contacts)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["mattersphereid"] = contact.MatterSphereId;
                    dataRow["[address-id]"] = contact.AddressId;
                    dataRow["contactType"] = contact.ContactType;
                    dataRow["contName"] = contact.Name;
                    dataRow["contNotes"] = contact.Notes;
                    dataRow["modifieddate"] = contact.ModifiedDate;
                    dataRow["ugdp"] = contact.UGDP;
                    dataRow["op"] = contact.Operation;

                    dataTable.Rows.Add(dataRow);
                }

                SaveTable(dataTable, CONTACTS_INDEX_SP);
            }
        }

        public void Index(MSDocument[] documents)
        {
            using (var dataTable = new DataTable("Table"))
            {
                dataTable.Columns.Add(new DataColumn("mattersphereid", typeof(long)));
                dataTable.Columns.Add(new DataColumn("[associate-id]", typeof(long)));
                dataTable.Columns.Add(new DataColumn("[client-id]", typeof(long)));
                dataTable.Columns.Add(new DataColumn("[file-id]", typeof(long)));
                dataTable.Columns.Add(new DataColumn("docDeleted", typeof(bool)));
                dataTable.Columns.Add(new DataColumn("documentExtension", typeof(string)));
                dataTable.Columns.Add(new DataColumn("documentType", typeof(string)));
                dataTable.Columns.Add(new DataColumn("docDesc", typeof(string)));
                dataTable.Columns.Add(new DataColumn("usrFullName", typeof(string)));
                dataTable.Columns.Add(new DataColumn("modifieddate", typeof(DateTime)));
                dataTable.Columns.Add(new DataColumn("ugdp", typeof(string)));
                dataTable.Columns.Add(new DataColumn("op", typeof(char)));

                foreach (var document in documents)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["mattersphereid"] = document.MatterSphereId;
                    dataRow["[associate-id]"] = document.AssociateId;
                    dataRow["[client-id]"] = document.ClientId;
                    dataRow["[file-id]"] = document.FileId;
                    dataRow["docDeleted"] = document.DocDeleted;
                    dataRow["documentExtension"] = document.DocumentExtension;
                    dataRow["documentType"] = document.DocumentType;
                    dataRow["docDesc"] = document.Description;
                    dataRow["usrFullName"] = document.Author;
                    dataRow["modifieddate"] = document.ModifiedDate;
                    dataRow["ugdp"] = document.UGDP;
                    dataRow["op"] = document.Operation;

                    dataTable.Rows.Add(dataRow);
                }

                SaveTable(dataTable, DOCUMENTS_INDEX_SP);
            }
        }

        public void Index(MSFile[] files)
        {
            using (var dataTable = new DataTable("Table"))
            {
                dataTable.Columns.Add(new DataColumn("mattersphereid", typeof(long)));
                dataTable.Columns.Add(new DataColumn("[client-id]", typeof(long)));
                dataTable.Columns.Add(new DataColumn("fileType", typeof(string)));
                dataTable.Columns.Add(new DataColumn("fileStatus", typeof(string)));
                dataTable.Columns.Add(new DataColumn("fileDesc", typeof(string)));
                dataTable.Columns.Add(new DataColumn("fileNotes", typeof(string)));
                dataTable.Columns.Add(new DataColumn("modifieddate", typeof(DateTime)));
                dataTable.Columns.Add(new DataColumn("ugdp", typeof(string)));
                dataTable.Columns.Add(new DataColumn("op", typeof(char)));

                foreach (var file in files)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["mattersphereid"] = file.MatterSphereId;
                    dataRow["[client-id]"] = file.ClientId;
                    dataRow["fileType"] = file.FileType;
                    dataRow["fileStatus"] = file.FileStatus;
                    dataRow["fileDesc"] = file.Description;
                    dataRow["fileNotes"] = file.Notes;
                    dataRow["modifieddate"] = file.ModifiedDate;
                    dataRow["ugdp"] = file.UGDP;
                    dataRow["op"] = file.Operation;

                    dataTable.Rows.Add(dataRow);
                }

                SaveTable(dataTable, FILES_INDEX_SP);
            }
        }

        public void Index(MSPrecedent[] precedents)
        {
            using (var dataTable = new DataTable("Table"))
            {
                dataTable.Columns.Add(new DataColumn("mattersphereid", typeof(long)));
                dataTable.Columns.Add(new DataColumn("precedentType", typeof(string)));
                dataTable.Columns.Add(new DataColumn("precCategory", typeof(string)));
                dataTable.Columns.Add(new DataColumn("precDeleted", typeof(bool)));
                dataTable.Columns.Add(new DataColumn("precedentExtension", typeof(string)));
                dataTable.Columns.Add(new DataColumn("precDesc", typeof(string)));
                dataTable.Columns.Add(new DataColumn("precTitle", typeof(string)));
                dataTable.Columns.Add(new DataColumn("precLibrary", typeof(string)));
                dataTable.Columns.Add(new DataColumn("precSubCategory", typeof(string)));
                dataTable.Columns.Add(new DataColumn("modifieddate", typeof(DateTime)));
                dataTable.Columns.Add(new DataColumn("ugdp", typeof(string)));
                dataTable.Columns.Add(new DataColumn("op", typeof(char)));

                foreach (var precedent in precedents)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["mattersphereid"] = precedent.MatterSphereId;
                    dataRow["precedentType"] = precedent.PrecedentType;
                    dataRow["precCategory"] = precedent.Category;
                    dataRow["precDeleted"] = precedent.Deleted;
                    dataRow["precedentExtension"] = precedent.Extension;
                    dataRow["precDesc"] = precedent.Description;
                    dataRow["precTitle"] = precedent.Title;
                    dataRow["precLibrary"] = precedent.Library;
                    dataRow["precSubCategory"] = precedent.SubCategory;
                    dataRow["modifieddate"] = precedent.ModifiedDate;
                    dataRow["ugdp"] = precedent.UGDP;
                    dataRow["op"] = precedent.Operation;

                    dataTable.Rows.Add(dataRow);
                }

                SaveTable(dataTable, PRECEDENTS_INDEX_SP);
            }
        }

        public void Index(MSTask[] tasks)
        {
            using (var dataTable = new DataTable("Table"))
            {
                dataTable.Columns.Add(new DataColumn("mattersphereid", typeof(long)));
                dataTable.Columns.Add(new DataColumn("file-id", typeof(long)));
                dataTable.Columns.Add(new DataColumn("document-id", typeof(long)));
                dataTable.Columns.Add(new DataColumn("taskType", typeof(string)));
                dataTable.Columns.Add(new DataColumn("tskDesc", typeof(string)));
                dataTable.Columns.Add(new DataColumn("tskNotes", typeof(string)));
                dataTable.Columns.Add(new DataColumn("modifieddate", typeof(DateTime)));
                dataTable.Columns.Add(new DataColumn("ugdp", typeof(string)));
                dataTable.Columns.Add(new DataColumn("op", typeof(char)));

                foreach (var task in tasks)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["mattersphereid"] = task.MatterSphereId;
                    dataRow["file-id"] = task.FileId;
                    dataRow["document-id"] = task.DocumentId;
                    dataRow["taskType"] = task.TaskType;
                    dataRow["tskDesc"] = task.Description;
                    dataRow["tskNotes"] = task.Notes;
                    dataRow["modifieddate"] = task.ModifiedDate;
                    dataRow["ugdp"] = task.UGDP;
                    dataRow["op"] = task.Operation;

                    dataTable.Rows.Add(dataRow);
                }

                SaveTable(dataTable, TASKS_INDEX_SP);
            }
        }

        public void Index(MSUser[] users)
        {
            using (var dataTable = new DataTable("Table"))
            {
                dataTable.Columns.Add(new DataColumn("mattersphereid", typeof(int)));
                dataTable.Columns.Add(new DataColumn("usrinits", typeof(string)));
                dataTable.Columns.Add(new DataColumn("usralias", typeof(string)));
                dataTable.Columns.Add(new DataColumn("usrad", typeof(string)));
                dataTable.Columns.Add(new DataColumn("usrsql", typeof(string)));
                dataTable.Columns.Add(new DataColumn("usrfullname", typeof(string)));
                dataTable.Columns.Add(new DataColumn("usractive", typeof(string)));
                dataTable.Columns.Add(new DataColumn("modifieddate", typeof(DateTime)));
                dataTable.Columns.Add(new DataColumn("usrAccessList", typeof(string)));
                dataTable.Columns.Add(new DataColumn("op", typeof(char)));

                foreach (var user in users)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["mattersphereid"] = user.MatterSphereId;
                    dataRow["usrinits"] = user.UserInits;
                    dataRow["usralias"] = user.UserAlias;
                    dataRow["usrad"] = user.UserAD;
                    dataRow["usrsql"] = user.UserSql;
                    dataRow["usrfullname"] = user.UserFullname;
                    dataRow["usractive"] = user.UserActive;
                    dataRow["modifieddate"] = user.ModifiedDate;
                    dataRow["usrAccessList"] = user.UserAccessList;
                    dataRow["op"] = user.Operation;
                    
                    dataTable.Rows.Add(dataRow);
                }

                SaveTable(dataTable, USERS_INDEX_SP);
            }
        }

        private void SaveTable(DataTable table, string spName)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand(spName, conn))
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Source", table);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
