using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Reports;
namespace DataAccess.ReportsManagement
{
    public class GenerateReportDL
    {
        public static DataSet GetVoucherDtls(string appType, long? RecPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_AppType,  appType),
                new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_RccPK,  RecPK)
                                          
            };

            DataSet dsReportData = dbService.DataAdapter(CommandType.StoredProcedure, "SPFIN_TRX_VOUCHER_RPT", colParameters);
            return dsReportData;


        }

        public static DataTable GetReportParameters(string aptCode, int astCode, DateTime AppvdDate)
        {
            DataTable dtPackingStaion;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                 new DBService.Parameters(GenerateReport.P_APT_CODE , aptCode==string.Empty?(object)DBNull.Value:aptCode),
                 new DBService.Parameters(GenerateReport.P_AST_VALUE, astCode==0?(object)DBNull.Value:astCode),
                 new DBService.Parameters(GenerateReport.P_TRX_DATE, AppvdDate)
            };
            dtPackingStaion = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPADM_APP_SUB_TYPE_DATA_GET, colParameters).Tables[0];
            return dtPackingStaion;
        }

        /// <summary>
        /// To get General Ledger Data
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        /// 
             


        public static DataTable GetGeneralLedgerData(string xmlDoc)
        {
            DataTable dtSCcost = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                 {                
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc)
                   
                 };
            dtSCcost = dbService.DataAdapter(CommandType.StoredProcedure, "SPFIN_ACCOUNT_STATEMENT_RPT", colParameters).Tables[0];
            return dtSCcost;
        }

        public static DataTable GetGeneralLedgerFinYearData(string xmlDoc)
        {
            DataTable dtSCcost = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
                 {
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc)

                 };
            dtSCcost = dbService.DataAdapter(CommandType.StoredProcedure, "SPFIN_ACCOUNT_STATEMENT_RPT_FIN_YEAR", colParameters).Tables[0];
            return dtSCcost;
        }

        public static DataTable GetCostCenterLedgerData(int astCode, DateTime FromDate, DateTime ToDate, string xmlDoc)
        {
            DataTable dtPackingStaion;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                 new DBService.Parameters(GenerateReport.P_APT_CODE , astCode==0?(object)DBNull.Value:astCode),
                 new DBService.Parameters(GenerateReport.P_TRX_DATE, FromDate),
                 new DBService.Parameters(GenerateReport.P_TO_DATE, ToDate),
                 new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc)
            };
            dtPackingStaion = dbService.DataAdapter(CommandType.StoredProcedure, "SPFIN_COSTCENTERWISE_LEDGER_DETAILS", colParameters).Tables[0];
            return dtPackingStaion;
        }
        public static DataTable GetSFGData(int astCode, DateTime FromDate, DateTime ToDate)
        {
            DataTable dtSFGproduction;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                 new DBService.Parameters(GenerateReport.P_APT_CODE , astCode==0?(object)DBNull.Value:astCode),
                 new DBService.Parameters(GenerateReport.P_TRX_DATE, FromDate),
                 new DBService.Parameters(GenerateReport.P_TO_DATE, ToDate),
                
            };
            dtSFGproduction = dbService.DataAdapter(CommandType.StoredProcedure, "SPPRD_COMP_WISE_STK_DTL_MIS_RPT_NEW", colParameters).Tables[0];
            return dtSFGproduction;
        }
        public static DataTable GetSFGDISPData(int astCode, DateTime FromDate, DateTime ToDate)
        {
            DataTable dtSFGDisp;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                 new DBService.Parameters(GenerateReport.P_APT_CODE , astCode==0?(object)DBNull.Value:astCode),
                 new DBService.Parameters(GenerateReport.P_TRX_DATE, FromDate),
                 new DBService.Parameters(GenerateReport.P_TO_DATE, ToDate),
                
            };
            dtSFGDisp = dbService.DataAdapter(CommandType.StoredProcedure, "SPPRD_DISP_WISE_STK_DTL_MIS_RPT_NEW", colParameters).Tables[0];
            return dtSFGDisp;
        }
        public static DataTable GetAccountDtls(int BizUnitPK)
        {
            DataTable dtSCOA = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
                 {
                   new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, BizUnitPK)
                 };
            dtSCOA = dbService.DataAdapter(CommandType.StoredProcedure, "SPFIN_COA_LIST_RPT", colParameters).Tables[0];
            return dtSCOA;
        }

        /// <summary>
        /// To get Party Ledger Data
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static DataTable GetPartyLedgerData(string xmlDoc)
        {
            DataTable dtSCcost = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                 {                
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc)
                   
                 };
            dtSCcost = dbService.DataAdapter(CommandType.StoredProcedure, "SPFIN_PARTY_STATEMENT_RPT", colParameters).Tables[0];
            return dtSCcost;
        }

        /// <summary>
        /// To get Sub Ledger Data
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static DataTable GetSubLedgerData(string xmlDoc)
        {
            DataTable dtSCcost = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                 {                
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc)
                   
                 };
            dtSCcost = dbService.DataAdapter(CommandType.StoredProcedure, "SPFIN_SUB_ACCOUNT_STATEMENT_RPT", colParameters).Tables[0];
            return dtSCcost;
        }

        /// <summary>
        /// To get General Ledger Consolidated Data
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static DataTable GetGeneralLedgerConsolidatedData(string xmlDoc)
        {
            DataTable dtSCcost = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                 {                
                    new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc)
                   
                 };
            dtSCcost = dbService.DataAdapter(CommandType.StoredProcedure, "SPFIN_ACC_STMT_CONS_RPT", colParameters).Tables[0];
            return dtSCcost;
        }

        /// <summary>
        /// To get Profit Report data
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataTable GetProfitReport(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, pXML),
            };
            var dsRpt = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPDWH_SALE_CONTRACT_WISE_PROFIT_PROCESS_GET, colParameters);//SPSAL_CONTRACT_WISE_PROFIT_MIS_RPT
            return dsRpt.Tables[0];
        }

        /// <summary>
        /// Process Profit Report data
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int ProcessProfitReport(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GenerateReport.SPDWH_SALE_CONTRACT_WISE_PROFIT_PROCESS_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataTable GetItemCategory(int BizUnitPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, BizUnitPK)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPINV_ITEM_CATEGORY_FILTER, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetItemCategoryFormer(int BizUnitPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, BizUnitPK)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPINV_ITEM_CATEGORY_FILTER_CUSTOME_MMT, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetDeptStores(int BizUnitPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, BizUnitPK)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPADM_DEPT_MST_FILTER, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetItemsByCategory(int CategotyPK, string CategoryPKs = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_CATEGORY, CategotyPK > 0 ? CategotyPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_CATEGORY_LST, CategoryPKs == null ? (object)DBNull.Value : CategoryPKs)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPINV_ITEM_MST_CATEGORY_WISE_FILTER, colParameters).Tables[0];
            return dtResult;
        }
        public static DataTable GetItemsByCategoryFormer(int CategotyPK, string CategoryPKs = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_CATEGORY, CategotyPK > 0 ? CategotyPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_CATEGORY_LST, CategoryPKs == null ? (object)DBNull.Value : CategoryPKs)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPINV_ITEM_MST_CATEGORY_WISE_FILTER_CUSTOME_MMT, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetTransactionNoByMode(int modepk,string Fromdate,string Todate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITD_TRX_MODE , modepk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , Fromdate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE , Todate)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPADM_TRANSACTION_NO_DATA_GET, colParameters).Tables[0];
            return dtResult;
        }
        
        public static DataTable GetTrasactionMode()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_CATEGORY, CategotyPK)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPADM_TRANSACTION_MODE_DATA_GET, colParameters).Tables[0];
            return dtResult;
        }
        public static DataTable GetPlant()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_CATEGORY, CategotyPK)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPADM_PLANT_DATA_GET, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetClassificationByCategory(int CategotyPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_CATEGORY, CategotyPK)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPADM_IPD_CLASSIFICATION_FILTER, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetFinYear(int BizUnitPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, BizUnitPK)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPFIN_YEAR_MST_FILTER, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetDateByFinYear(int FinyearPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_FYR_PK, FinyearPK)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPFIN_YEAR_DATE_FILTER, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetCurrentFinYear(int BizUnitPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, BizUnitPK)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPFIN_CURRENT_YEAR_MST_GET, colParameters).Tables[0];
            return dtResult;
        }
        public static DataTable GetCategoryTreeNodes(int BizUnitPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, BizUnitPK)
            };
            var dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPINV_ITEM_CATEGORY_TREE_FILTER, colParameters).Tables[0];
            return dtResult;
        }

        public static DataSet GetBinCardIssueOutputReport(string BatchNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIH_PK, BatchNo)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPPRD_BIN_CARD_ISSUE_GET_RPT, colParameters);
        }
        public static DataSet GetIssueOutputReport(string xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML, xml)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPINV_WORK_ORDER_ITEM_ISSUE_GET_RPT, colParameters);
        }

        public static DataSet GetCartonIssueOutputReport(string BatchNo)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_HBI_PK, BatchNo)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SPPRD_BIN_CARD_PACK_ISSUE_HDR_GET_RPT, colParameters);
        }

        public static DataSet GetBinCardTraceReportData(string BatchNo, int SBU)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_NO, BatchNo),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BIZUNIT, SBU),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GenerateReport.SP_GET_BIN_CARD_REPORT, colParameters);
        }
    }
}
