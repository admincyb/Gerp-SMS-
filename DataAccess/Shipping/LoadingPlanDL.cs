using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.Shipping
{
   public class LoadingPlanDL
    {
        /// <summary>
        /// Save PO Invoice
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SavelLoadingPlan(string xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ERP.Utilities.Constants.Shipping.LoadingPlan.SP_SaveLoadingPlan, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get Purchase Invoice List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetLoadingPlan(int LPHID, int SBUID, int LPHACTV)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                //new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.LPH_ACTIVE, LPHACTV),
                //new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.LPD_PK, LPDID),
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.SNH_PK, LPHID),  

                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, SBUID)
            };

            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, ERP.Utilities.Constants.Shipping.LoadingPlan.SP_GetLoadingPlan, colParameters);
            return dsList;
        }

        /// <summary>
        /// Get Loading Plan Report
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetLoadingPlanReport(int? LPHPK, int SNHPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                  
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.P_LPH_PK, LPHPK),  
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.P_SNH_PK, SNHPK), 
            };

            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, ERP.Utilities.Constants.Shipping.LoadingPlan.SP_GetLoadingPlanReport, colParameters);
            return dsList;
        }
        /// Get SC Details  
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSCDetails(int spPk, int active, int bizuit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.P_SNH_PK, spPk == 0 ? (object) DBNull.Value :  spPk), 
              //new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.P_LPH_PK, spPk == 0 ? (object) DBNull.Value :  spPk),              
              new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.P_ACTIVE,  active == 0 ? (object) DBNull.Value :  active),
              new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.P_BIZUNIT,  active == 0 ? (object) DBNull.Value :  bizuit)
            };

            DataTable dtSCList = new DataTable();
            dtSCList = dbService.DataAdapter(CommandType.StoredProcedure, ERP.Utilities.Constants.Shipping.LoadingPlan.SP_GetSCDetails, colParameters).Tables[0];
            return dtSCList;
        }
        /// Get Brand Details  
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetBrandDetails(int soPk,int sodPk,int shpPlanPk, int active, int bizuit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.P_SOH_PK, soPk == 0 ? (object) DBNull.Value :  soPk), 
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.P_SOD_PK, sodPk == 0 ? (object) DBNull.Value :  sodPk), 
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.P_SNH_PK, shpPlanPk == 0 ? (object) DBNull.Value :  shpPlanPk), 
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.P_ACTIVE,  active == 0 ? (object) DBNull.Value :  active),
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.LoadingPlan.P_BIZUNIT,  active == 0 ? (object) DBNull.Value :  bizuit)
            };
            DataTable dt = new DataTable();
            dt = dbService.DataAdapter(CommandType.StoredProcedure, ERP.Utilities.Constants.Shipping.LoadingPlan.SP_GetBrandDetails, colParameters).Tables[0];
            return dt;
        }
    }
}
