using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERP.Utilities;
using System.Data;
using BusinessObject.AlertManagement;

namespace DataAccess.AlertManagement
{
    public class AlertsDA
    {
        public static System.Data.DataSet GetAlertDetails(int currPK, byte active, DateTime? date, string TypeCode, int? TypePK, int bizUnit, int userPK, int? isSystem = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_PK, currPK == 0 ? (object) DBNull.Value : currPK ),
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, active),
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.AS_ON_DATE, date == null ? (object) DBNull.Value : date),
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_TRX_TYPE, string.IsNullOrEmpty(TypeCode) ? (object) DBNull.Value : TypeCode ),
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_TRX_PK, TypePK == 0 ? (object) DBNull.Value : TypePK ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, userPK),
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.IS_SYSTEM, isSystem == 0 ? (object) DBNull.Value : isSystem ),
                
          
            };
            DataSet dsRFQTaxDetails = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Alert.Procedures.SPADM_ALERT_TRX_HDR_GET_KV, colParameters);
            return dsRFQTaxDetails;
        }

        public static int SaveAlertDetails(AlertBO alertBoObj)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_PK, alertBoObj.ATH_PK),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_NO, string.IsNullOrEmpty(alertBoObj.ATH_NO)?(Object)DBNull.Value:alertBoObj.ATH_NO),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_DATE, alertBoObj.ATH_DATE),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_TRX_TYPE, string.IsNullOrEmpty(alertBoObj.ATH_TRX_TYPE)?(Object)DBNull.Value:alertBoObj.ATH_TRX_TYPE),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_TRX_PK, alertBoObj.ATH_TRX_PK),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_DUE_DAYS, alertBoObj.ATH_DUE_DAYS),
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_TRX_DATE, alertBoObj.ATH_TRX_DATE),
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_DUE_DATE, alertBoObj.ATH_DUE_DATE),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_NAME, alertBoObj.ATH_NAME),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_BASIS, alertBoObj.ATH_BASIS==null?(Object)DBNull.Value:alertBoObj.ATH_BASIS),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_ALERT_TYPE, alertBoObj.ATH_ALERT_TYPE==null?(Object)DBNull.Value:alertBoObj.ATH_ALERT_TYPE),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_NOTIFY_BFR, alertBoObj.ATH_NOTIFY_BFR),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_NOTIFY_BFR_UOM, alertBoObj.ATH_NOTIFY_BFR_UOM==null?(Object)DBNull.Value:alertBoObj.ATH_NOTIFY_BFR_UOM),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_REMARKS, string.IsNullOrEmpty(alertBoObj.ATH_REMARKS)?(Object)DBNull.Value:alertBoObj.ATH_REMARKS),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_NARRATION, string.IsNullOrEmpty(alertBoObj.ATH_NARRATION)?(Object)DBNull.Value:alertBoObj.ATH_NARRATION),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_NOTIFY_MESSAGE, alertBoObj.ATH_NOTIFY_MESSAGE),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_NOTIFY_EMAIL, alertBoObj.ATH_NOTIFY_EMAIL),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_NOTIFY_SMS, alertBoObj.ATH_NOTIFY_SMS),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_NOTIFY_USER, alertBoObj.ATH_NOTIFY_USER==0?(object)DBNull.Value:alertBoObj.ATH_NOTIFY_USER),  
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_STATUS, alertBoObj.ATH_STATUS),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE, alertBoObj.ACTIVE),          
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, alertBoObj.USER_PK),          
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, alertBoObj.BIZUNIT),          
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, alertBoObj.LAST_MOD_DT.HasValue ?(object)DBNull.Value
                    :alertBoObj.LAST_MOD_DT.Value.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),        
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Alert.Procedures.SPADM_ALERT_TRX_HDR_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int? DeleteAlertDetails(int CurrPK, DateTime LastModifiedTime)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_PK, CurrPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, LastModifiedTime.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Alert.Procedures.SPADM_ALERT_TRX_HDR_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataSet GetAlertInbox(int userPk, int currentPage, int pageSize, string fromdate, string toDate, string type, int? typePK, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   

                new DBService.Parameters(CommonConstants.P_PAGENUM, currentPage == 0 ? 1 : currentPage ),
                new DBService.Parameters(CommonConstants.P_PAGESIZE, pageSize),
                new DBService.Parameters(CommonConstants.P_FROM_DATE, string.IsNullOrEmpty(fromdate)?(object)DBNull.Value:Convert.ToDateTime(fromdate)),
                new DBService.Parameters(CommonConstants.P_TO_DATE, string.IsNullOrEmpty(toDate)?(object)DBNull.Value:Convert.ToDateTime(toDate)),
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_TRX_TYPE,string.IsNullOrEmpty(type)?(object)DBNull.Value:type),
                new DBService.Parameters(GTIService.Constants.Alert.Parameters.ATH_TRX_PK, typePK??(object) DBNull.Value), //== 0 ? (object) DBNull.Value : typePK ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USR_PK, userPk == 0 ? (object) DBNull.Value : userPk ),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)          
            };
            DataSet dsAlertList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Alert.Procedures.SPADM_ALERT_TRX_INBOX_GET, colParameters);
            return dsAlertList;
        }
    }
}
