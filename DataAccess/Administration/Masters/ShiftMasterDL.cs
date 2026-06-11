using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Administration.Masters;
using BusinessObject.Administration.Masters;

namespace DataAccess.Administration.Masters
{
    public class ShiftMasterDL
    {
        public static DataTable GetShift(int bizunit,string code,string name, int pageNum, int pageSize,int shiftType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.P_BIZUNIT, bizunit),
                new DBService.Parameters(Parameters.P_SHF_CODE,code == string.Empty ? (object)DBNull.Value : code),  
                new DBService.Parameters(Parameters.P_SHF_NAME,name == string.Empty ? (object)DBNull.Value : name), 
                new DBService.Parameters(Parameters.P_PAGE_NUM , pageNum),
                new DBService.Parameters(Parameters.P_PAGE_SIZE,  pageSize),
                new DBService.Parameters(Parameters.P_SHF_IS_PRODUCTION ,shiftType < 0 ? (object)DBNull.Value : shiftType)
            };
            DataTable dtresult = dbService.DataAdapterTable(CommandType.StoredProcedure, Procedures.SPADM_SHIFT_MST_GET_LIST, colParameters);
            return dtresult;
        }

        public static int SaveShiftDetails(ShiftHeader objShiftHeader)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_PK,objShiftHeader.SHF_PK>0  ? objShiftHeader.SHF_PK :(object)DBNull.Value ), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_CODE,objShiftHeader.SHF_CODE),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_NAME,objShiftHeader.SHF_NAME), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_DESC,objShiftHeader.SHF_DESC), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_TIME_FROM,objShiftHeader.SHF_TIME_FROM), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_TIME_TO,objShiftHeader.SHF_TIME_TO), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_ACTIVE,objShiftHeader.SHF_ACTIVE), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USER,objShiftHeader.SHF_CRTD_BY), 
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT,objShiftHeader.SHF_MOD_BY),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DATE,objShiftHeader.LAST_MOD_DT),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_BIZUNIT,objShiftHeader.SHF_BIZUNIT), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_IS_PRODUCTION,objShiftHeader.SHF_IS_PRODUCTION), 
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_SEQUENCE,objShiftHeader.SHF_SEQUENCE), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_SHIFT_MST_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetShiftType(int? ltmPK, int active, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_SHF_PK, ltmPK.HasValue?ltmPK:(object)DBNull.Value),
                new DBService.Parameters(Parameters.P_ACTIVE, active),
                new DBService.Parameters(Parameters.P_BIZUNIT, bizUnit),
                new DBService.Parameters(Parameters.P_IS_PRODUCTION,(object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPADM_SHIFT_GET_KV, colParameters).Tables[0];
            //return null;
        }




        public static int DeleteShiftDetails(int CurrPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SHF_PK,CurrPK), 
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_LAST_MOD_DT ? SHF_PK :(object)DBNull.Value), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_LAST_MOD_DT , (object)DBNull.Value), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPADM_SHIFT_MST_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

    }
}
