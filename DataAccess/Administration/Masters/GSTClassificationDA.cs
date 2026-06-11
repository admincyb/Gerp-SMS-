using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.Administration.Masters;
using System.Data;
using GTIService.Constants.Common;

namespace DataAccess.Administration.Masters
{
    public class GSTClassificationDA
    {
        public static int SaveGSTClassificationDetails(GSTClassificationBO objGSTClassification)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(Parameters_Common.P_GCM_PK,objGSTClassification.GCM_PK>0  ? objGSTClassification.GCM_PK :(object)DBNull.Value ), 
                new DBService.Parameters(Parameters_Common.P_GCM_CODE,objGSTClassification.GCM_CODE),  
                new DBService.Parameters(Parameters_Common.P_GCM_NAME,objGSTClassification.GCM_NAME), 
                new DBService.Parameters(Parameters_Common.P_GCM_IS_NONGST,objGSTClassification.GCM_IS_NONGST), 
                new DBService.Parameters(Parameters_Common.P_GCM_TAXABILITY,objGSTClassification.GCM_TAXABILITY), 
                new DBService.Parameters(Parameters_Common.P_GCM_DESC,objGSTClassification.GCM_DESC), 
                new DBService.Parameters(Parameters_Common.P_ACTIVE,objGSTClassification.ACTIVE), 
                new DBService.Parameters(Parameters_Common.USERPK,objGSTClassification.GCM_CRTD_BY), 
                new DBService.Parameters(Parameters_Common.P_LAST_MOD_DT,objGSTClassification.LAST_MOD_DT),
                new DBService.Parameters(Parameters_Common.BIZUNIT,objGSTClassification.BIZUNIT_PK),
                new DBService.Parameters(Parameters_Common.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPFIN_GST_CLASS_MST_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetGstList(string code, string name, int bizUnit, int pageIndex, int pageSize)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters_Common.P_GCM_CODE,code == string.Empty ? (object)DBNull.Value : code),  
                new DBService.Parameters(Parameters_Common.P_GCM_NAME,name == string.Empty ? (object)DBNull.Value : name), 
                new DBService.Parameters(Parameters_Common.BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters_Common.PAGENO , pageIndex),
                new DBService.Parameters(Parameters_Common.P_PAGE_SIZE,  pageSize),
            };
            DataTable dtresult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPFIN_GST_CLASS_MST_GET_LIST, colParameters);
            return dtresult;
        }

        public static DataTable GetGstDetailList(int? gstPK, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(Parameters_Common.P_GCM_PK, gstPK.HasValue?gstPK:(object)DBNull.Value),
                new DBService.Parameters(Parameters_Common.P_ACTIVE, active),
                new DBService.Parameters(Parameters_Common.P_BIZUNIT, bizUnit),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPFIN_GST_CLASS_MST_GET_KV, colParameters).Tables[0];
        }

        public static int DeleteGstDetails(int CurrPK, DateTime LastModifiedTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_GCM_PK,CurrPK), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_LAST_MOD_DT , LastModifiedTime), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPFIN_GST_CLASS_MST_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
