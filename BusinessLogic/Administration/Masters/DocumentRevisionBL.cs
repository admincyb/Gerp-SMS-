using DataAccess.Administration.Masters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Administration.Masters
{
    public class DocumentRevisionBL
    {
        public static DataTable GetReportList(int type)
        {
            return DocumentRevisionDL.GetReportList(type);
        }
        public static string GetReportData(int pk)
        {
            return DocumentRevisionDL.GetReportData(pk);
        }
        public static int SaveDocumentRevision(string strXml)
        {
            return DocumentRevisionDL.SaveDocumentRevision(strXml);
        }

        public static int DeleteDocumentRevision(int DRM_PK)
        {
            return DocumentRevisionDL.DeleteDocumentRevision(DRM_PK);
        }
        public static DataSet GetRevisionList(int PageNumber, int PageSize, int bizunit, string DocNo,
                         int DocPK, string Doc_type, string Revision, string RevDate)
        {
            DataSet dsRevisionList = DocumentRevisionDL.GetRevisionList(PageNumber,PageSize,bizunit,DocNo,
                         DocPK,Doc_type,Revision,RevDate);
            return dsRevisionList;
        }

    }
}
