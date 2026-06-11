using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Finance;
using BusinessObject.CommonManagement;

namespace DataAccess.Finance
{
    public class FCReverseDL
    {
        /// <summary>
        /// Get Purchase Invoice List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetFCRList(GridPrams grid, User objUser, string vNo, int BankPk, int PoPk, int Active, int? Status = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            if (grid != null)
            {
                colParameters = new DBService.Parameters[] 
                {            
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
                    //Filtration by voucher no
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.SEARCHVAL  , grid.SearchValue== string.Empty || grid.SearchValue=="0" ? "%" : grid.SearchValue+"%"),
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),   
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.FITERSTATUS,  Status == null ? (object) DBNull.Value : Status ),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.Active, Active),            
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.HRH_PK,  PoPk == 0 ? (object) DBNull.Value :  PoPk.ToString()),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.DTL_VOUCHER_NO,  vNo == null? (object) DBNull.Value :  vNo),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.CBM_PK,  BankPk == 0 ? (object) DBNull.Value :  BankPk.ToString())
                };
            }
            else
            {
                colParameters = new DBService.Parameters[] 
                { 
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),       
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.Active, Active),            
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.HRH_PK,  PoPk == 0 ? (object) DBNull.Value :  PoPk.ToString()),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.CBM_PK,  BankPk == 0 ? (object) DBNull.Value :  BankPk.ToString())
                };
            }
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETFCRLIST, colParameters);
            return dsList;
        }
        /// <summary>
        /// Get FCHold Revert Details 
        /// </summary>
        /// <param name="PK"></param>
        /// <param name="accountPK"></param>
        /// <param name="curPK"></param>
        /// <returns></returns>
        public static DataSet GetFCHoldRevertDetails(int PK, int accountPK, int curPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(Parameters.HRH_PK, PK==0?(object) DBNull.Value:PK),       
               new DBService.Parameters(Parameters.CBM_PK, accountPK),
               new DBService.Parameters(Parameters.CUR_PK, curPK),
              };
            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETFCHOLDDTL, colParameters);
            return dsList;
        }
        /// <summary>
        /// Save FCHold Revert Details 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static long SaveFCHoldRevertDetails(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SAVEFCHOLD, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="hrhPK"></param>
        /// <param name="lastModDate"></param>
        /// <returns></returns>
        public static int DeleteFCHoldRevertDetails(int hrhPK, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(Parameters.HRH_PK, hrhPK==0?(object) DBNull.Value:hrhPK), 
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.DELETEFCHOLD, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }


        public static DataTable GetFCVoucherNumberAuto(byte Active, int bizUnit, string searchValue)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters( GTIService.Constants.Common.Common.P_ACTIVE, Active),
                new DBService.Parameters( GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.SEARCHVALAUTO, searchValue)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.FCVOUCHERNUMBERAUTO, colParameters).Tables[0];
            return dtProcess;
        }
    }
}
