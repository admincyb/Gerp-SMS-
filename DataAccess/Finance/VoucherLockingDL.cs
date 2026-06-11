using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Finance;
using BusinessObject.CommonManagement;
using BusinessObject.Finance;

namespace DataAccess.Finance
{
    public class VoucherLockingDL
    {
        /// <summary>
        /// Get Transactions
        /// </summary>
        /// <param name="ApplicationTypePk"></param>
        /// <param name="Active"></param>
        /// <param name="SplCondition"></param>
        /// <returns></returns>
        public static DataTable GetTransactions(int ApplicationTypePk, int Active, string SplCondition)
        {
            DataTable dtTransactions = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Common.Common.P_ACTIVE, Active),
                new DBService.Parameters( GTIService.Constants.Common.Common.P_APT_SPL_COND, SplCondition),
                new DBService.Parameters( GTIService.Constants.Common.Common.P_APT_PK, ApplicationTypePk)
            };
            dtTransactions = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPADM_APP_TYPE_MST_GET_KV, colParameters).Tables[0];
            return dtTransactions;
        }

        public static DataTable GetPartyOrVoucherDDL(int TransactionType, int VoucherType, DateTime AsOnDate, int Status, int BizUnit, int VoucherPk, int PartyPk, string Field)
        {
            DataTable dtTransactions = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Common.Common.P_APT_PK, TransactionType),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_CON_PK, VoucherType),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_AS_ON_DATE, AsOnDate),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_LST_STATUS, Status),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_BIZUNIT, BizUnit),
                new DBService.Parameters( GTIService.Constants.Common.Common.P_FTH_PK,  VoucherPk == 0||VoucherPk == -1 ? (object) DBNull.Value :  VoucherPk),
                new DBService.Parameters( GTIService.Constants.Common.Common.P_PARTY_PK,  PartyPk == 0||PartyPk == -1 ? (object) DBNull.Value :  PartyPk),
                new DBService.Parameters( GTIService.Constants.Common.Common.P_FLD_NAME, Field)
            };
            dtTransactions = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPFIN_YEAR_END_TRX_GET_AUTO, colParameters).Tables[0];
            return dtTransactions;
        }


        public static DataTable GetVoucherTypes(int ConPk, int ConActive, int ConGroupTypeVal, int ConGroupVal)
        {
            DataTable dtVoucherTypes = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_CON_PK, ConPk),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_CON_ACTIVE, ConActive),
                new DBService.Parameters(  GTIService.Constants.Finance.Parameters.P_CGT_VALUE, ConGroupTypeVal),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_CNG_VALUE, ConGroupVal)
            };
            dtVoucherTypes = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPADM_CONST_MST_GET_KV, colParameters).Tables[0];
            return dtVoucherTypes;
        }


        /// <summary>
        /// Get voucher list
        /// </summary>
        /// <param name="TransactionType"></param>
        /// <param name="VoucherType"></param>
        /// <param name="AsOnDate"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static DataTable GetVoucherList(int TransactionType, int VoucherType, DateTime AsOnDate, int Status, int BizUnit, int VoucherPk, int PartyPk, int PNO, int PSize)
        {
            DataTable dtVoucherList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Common.Common.P_APT_PK, TransactionType),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_CON_PK, VoucherType),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_AS_ON_DATE, AsOnDate),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_LST_STATUS, Status),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_BIZUNIT, BizUnit),
                   new DBService.Parameters( GTIService.Constants.Common.Common.P_FTH_PK,  VoucherPk == 0||VoucherPk == -1 ? (object) DBNull.Value :  VoucherPk),
                 new DBService.Parameters( GTIService.Constants.Common.Common.P_PARTY_PK,  PartyPk == 0||PartyPk == -1 ? (object) DBNull.Value :  PartyPk),
                 new DBService.Parameters("P_PAGE_NO",  PNO == 0 ? (object) DBNull.Value :  PNO),
              new DBService.Parameters("P_PAGE_SIZE",  PSize == 0 ? (object) DBNull.Value :  PSize)
            };
            dtVoucherList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPFIN_YEAR_END_TRX_GET_LIST, colParameters).Tables[0];
            return dtVoucherList;
        }

        public static int? YearEndVoucher(string strxml)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPFIN_YEAR_END_TRX_DTL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int? SaveVoucherLockingDetails(VoucherLockingBO objVoucherLoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_FLL_PK, objVoucherLoc.FLL_PK),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_FLL_DATE, objVoucherLoc.FLL_DATE),
                new DBService.Parameters(  GTIService.Constants.Finance.Parameters.P_FLL_REMARKS, objVoucherLoc.FLL_REMARKS),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_ACTIVE, objVoucherLoc.FLL_ACTIVE),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.USERPK, objVoucherLoc.FLL_CRTD_BY),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT, objVoucherLoc.FLL_BIZUNIT),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_FLL_MODULE, objVoucherLoc.FLL_MODULE),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPFIN_LOCK_LOG_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetVoucherLockList(int? PK, int? Active, int BizUnit, int LockMOD)
        {
            DataTable dtVoucherLockList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_FLL_PK, PK.HasValue? PK : (object) DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_ACTIVE, Active.HasValue? Active : (object) DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_BIZUNIT, BizUnit),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_FLL_MODULE, LockMOD)
            };
            dtVoucherLockList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPFIN_LOCK_LOG_GET_KV, colParameters).Tables[0];
            return dtVoucherLockList;
        }

        public static bool IsVoucherLocked(DateTime VoucherDate, int VoucherPk, int BizUnit, ref string LockUptoDate)
        {
            DataTable dtVoucherLockList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_DATE, VoucherDate),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT, BizUnit),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_FTH_PK, VoucherPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dtVoucherLockList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPFIN_LOCK_CHECK, colParameters).Tables[0];
            if (dtVoucherLockList != null && dtVoucherLockList.Rows.Count > 0)
                LockUptoDate = dtVoucherLockList.Rows[0]["FIN_LOCK_UPTO"].ToString();
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            if (result > 0)
                return true;
            else
                return false;

        }

        public static bool IsFinYearLocked(int finYearPK, int BizUnit)
        {
            DataTable dtVoucherLockList = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT, BizUnit),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_FYR_PK, finYearPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPFIN_YEAR_LOCK_CHECK, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            if (result > 0)
                return true;
            else
                return false;

        }


    }
}
