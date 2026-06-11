using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.Finance.Administration.Masters;
using GTIService.Constants.Common;
using System.Data;

namespace DataAccess.Finance.Administration.Masters
{
    public class InvoiceTypeDL
    {
        public static int SaveInvoiceType(InvoiceTypeBO ObjInvoiceType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(Parameters_Common.P_FTM_PK,ObjInvoiceType.FTM_PK>0  ? ObjInvoiceType.FTM_PK :(object)DBNull.Value ), 
                new DBService.Parameters(Parameters_Common.P_FTM_CODE,ObjInvoiceType.FTM_CODE),  
                new DBService.Parameters(Parameters_Common.P_FTM_NAME,ObjInvoiceType.FTM_NAME), 
                //new DBService.Parameters(Parameters_Common.P_GCM_IS_NONGST,ObjInvoiceType.GCM_IS_NONGST), 
                new DBService.Parameters(Parameters_Common.P_FTM_TRX_TYPE,ObjInvoiceType.FTM_TRX_TYPE), 
                new DBService.Parameters(Parameters_Common.P_FTM_INVOICE_TYPE,ObjInvoiceType.FTM_INVOICE_TYPE), 
                new DBService.Parameters(Parameters_Common.P_FTM_DESC,ObjInvoiceType.FTM_DESC), 
                new DBService.Parameters(Parameters_Common.P_ACTIVE,ObjInvoiceType.ACTIVE), 
                new DBService.Parameters(Parameters_Common.USERPK, ObjInvoiceType.USER_PK), 
                new DBService.Parameters(Parameters_Common.P_LAST_MOD_DT,ObjInvoiceType.LAST_MOD_DT),
                new DBService.Parameters(Parameters_Common.BIZUNIT,ObjInvoiceType.BIZUNIT),
                //new DBService.Parameters(GTIService.Constants.Common.Common.P_USERPK,  ObjInvoiceType.USER_PK),
                new DBService.Parameters(Parameters_Common.P_RET_VAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPFIN_INVOICE_GST_TYPE_MST_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetInvoiceTypeList(string code, string name, int bizUnit, int pageIndex, int pageSize)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters_Common.P_FTM_CODE,code == string.Empty ? (object)DBNull.Value : code),  
                new DBService.Parameters(Parameters_Common.P_FTM_NAME,name == string.Empty ? (object)DBNull.Value : name), 
                new DBService.Parameters(Parameters_Common.BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters_Common.PAGENO , pageIndex),
                new DBService.Parameters(Parameters_Common.P_PAGE_SIZE,  pageSize),
            };
            DataTable dtresult = dbService.DataAdapterTable(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPFIN_INVOICE_GST_TYPE_MST_GET_LIST, colParameters);
            return dtresult;
        }
        public static DataTable GetInvoiceTypeDetailList(int? invPK, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(Parameters_Common.P_FTM_PK, invPK.HasValue?invPK:(object)DBNull.Value),
                new DBService.Parameters(Parameters_Common.P_ACTIVE, active),
                new DBService.Parameters(Parameters_Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters_Common.P_INVOICE_TYPE, (object)DBNull.Value)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPFIN_INVOICE_GST_TYPE_MST_GET_KV, colParameters).Tables[0];
        }
        public static int DeleteInvoiceTypeDetails(int CurrPK, DateTime LastModifiedTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FTM_PK,CurrPK), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_LAST_MOD_DT , LastModifiedTime), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.Procedures.SPFIN_INVOICE_GST_TYPE_MST_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
