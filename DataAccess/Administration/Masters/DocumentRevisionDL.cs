using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Administration.Masters;
using BusinessObject.Administration.Masters;

namespace DataAccess.Administration.Masters
{
    public class DocumentRevisionDL
    {
        public static DataTable GetReportList(int type)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_TYPE, type)
            };
            DataTable dtresult = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_DOC_REVISION_RPT_MST_AUTO, colParameters);
            return dtresult;
        }

        public static string GetReportData(int pk)
        {
            StringBuilder xml = new StringBuilder(String.Empty);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(Parameters.P_DRM_PK, pk)
            };
            DataTable dtresult = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_DOC_REVISION_RPT_MAP_GET_KV, colParameters);
            foreach (DataRow drProcCtrlTrx in dtresult.Rows)
            {
                xml.Append((drProcCtrlTrx[0]).Equals(DBNull.Value) ? String.Empty : drProcCtrlTrx[0]);
            }
            return xml.ToString();
           
        }
        public static int SaveDocumentRevision(string strXml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_XML, strXml),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.VENDORDETAILVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
             };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_DOC_REVISION_RPT_MAP_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters.VENDORDETAILVALUE]).Value);
            return result;

        }

        public static int DeleteDocumentRevision(int DRM_PK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_DRM_PK, DRM_PK),
                new DBService.Parameters(GTIService.Constants.Material.Parameters.VENDORDETAILVALUE, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
             };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_DOC_REVISION_RPT_MAP_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters.VENDORDETAILVALUE]).Value);
            return result;

        }
        public static DataSet GetRevisionList(int PageNumber, int PageSize, int bizunit, string DocNo,
                         int DocPK, string Doc_type, string Revision, string RevDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
             {
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizunit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM,  PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,PageSize),
              new DBService.Parameters(Parameters.P_DOC_NO,!string.IsNullOrEmpty(DocNo)?DocNo: (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_DOC_PK, DocPK>0?DocPK: (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_DOC_REVISION, !string.IsNullOrEmpty(Revision)?Revision: (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_DOC_REV_DT, !string.IsNullOrEmpty(RevDate)?RevDate: (object)DBNull.Value),
              new DBService.Parameters(Parameters.P_DOC_TYPE, !string.IsNullOrEmpty(Doc_type)?Doc_type: (object)DBNull.Value),
             };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure,Procedures.SPADM_DOC_REVISION_RPT_MAP_LIST, colParameters);
            return dsList;
            //DataSet dtList = new DataSet();
            //dtList = dbService.DataAdapter(CommandType.StoredProcedure,Procedures.SPADM_DOC_REVISION_RPT_MAP_LIST, colParameters);
            //return dtList;
        }
    }
}
