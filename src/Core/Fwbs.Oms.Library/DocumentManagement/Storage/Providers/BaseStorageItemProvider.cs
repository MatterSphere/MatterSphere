using System;
using System.Data;
using System.IO;

namespace FWBS.OMS.DocumentManagement.Storage.Providers
{
    public abstract class BaseStorageItemProvider
    {
        protected System.IO.FileInfo FindPath(IStorageItem item, SystemDirectories dirType, object branchID)
        {
            DataTable dir = Session.CurrentSession.GetDirectories();

            try
            {
                DataView vw = new DataView(dir);
                vw.Sort = "brid desc";

                // Check the current logged in branch directory.
                string path = String.Empty;
                vw.RowFilter = String.Format("dircode='{1}' and brid = '{0}'", Session.CurrentSession.CurrentBranch.ID, dirType.ToString().ToUpper());

                if (vw.Count > 0)
                {
                    // There is a Local Directory for this type so check this first
                    if (item.Token.StartsWith(@"\"))
                        path = Path.Combine(Convert.ToString(vw[0]["dirpath"]), item.Token.Substring(1).Trim());
                    else
                        path = Path.Combine(Convert.ToString(vw[0]["dirpath"]), item.Token.Trim());

                    if (System.IO.File.Exists(path))
                        return new FileInfo(path);
                }


                //Check the branch id on the actual precedent next.
                if (branchID != DBNull.Value)
                {
                    vw.RowFilter = String.Format("dircode='{1}' and brid = '{0}'", branchID, dirType.ToString().ToUpper());

                    if (vw.Count > 0)
                    {
                        if (item.Token.StartsWith(@"\"))
                            path = Path.Combine(Convert.ToString(vw[0]["dirpath"]), item.Token.Substring(1));
                        else
                            path = Path.Combine(Convert.ToString(vw[0]["dirpath"]), item.Token);

                        if (System.IO.File.Exists(path))
                            return new FileInfo(path);
                    }
                }

                //Check the catch all branch.
                vw.RowFilter = String.Format("dircode='{1}' and brid is null", branchID, dirType.ToString().ToUpper());
                if (vw.Count > 0)
                {
                    if (item.Token.StartsWith(@"\"))
                        path = Path.Combine(Convert.ToString(vw[0]["dirpath"]), item.Token.Substring(1));
                    else
                        path = Path.Combine(Convert.ToString(vw[0]["dirpath"]), item.Token);

                    if (System.IO.File.Exists(path))
                        return new FileInfo(path);
                }


                return null;

            }
            catch (Exception ex)
            {
                //DMB 18/10/2005 
                throw new StorageException("exFindPathErr", "Error Creating Path variable for location of document", ex);
            }
        }
    }
}