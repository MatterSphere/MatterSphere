using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.HighQ
{
    public interface IHighQ
    {
        DataTable GetMatters(long clientId);
        IDictionary<long, Exception> UploadDocuments(long[] docIds, int? targetFolderId = null);
        int GetRootFolderId(long documentId);
        int GetTargetFolderId(int rootFolderId, IWin32Window owner = null);
    }
}
