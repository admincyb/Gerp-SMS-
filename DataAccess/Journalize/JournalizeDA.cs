using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.Journalize;
using ERP.Utilities;

namespace DataAccess.Journalize
{
    public class JournalizeDA
    {

        public static FinTrxDetails GetBankReconcileList(FinTransactions finTrxObj, int isReconciled, ref decimal TotalDebit, ref decimal TotalCredit)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            string defaultSelect = "%";
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters (GTIService.Constants.Finance.Parameters.P_FROM_DATE,finTrxObj.FROM_DATE.HasValue ? finTrxObj.FROM_DATE : (object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_TO_DATE,finTrxObj.TO_DATE.HasValue ? finTrxObj.TO_DATE : (object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_FTH_PK,finTrxObj.FTH_PK> 0 ? finTrxObj.FTH_PK : (object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_BIZUNIT,finTrxObj.BIZUNIT> 0 ? finTrxObj.BIZUNIT : (object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_IS_RECONCILED,isReconciled> -1 ? isReconciled : (object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.Active,finTrxObj.FTR_ACTIVE),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_TYPE,string.IsNullOrEmpty(finTrxObj.FTR_TYPE)?  "%" : finTrxObj.FTR_TYPE),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_TYPE_PK,finTrxObj.FTR_TYPE_PK>0 ? finTrxObj.FTR_TYPE_PK : (object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_INSTR_NO,string.IsNullOrEmpty( finTrxObj.FTR_INSTR_NO) ? "%" : finTrxObj.FTR_INSTR_NO ),	

                

               // new DBService.Parameters( GTIService.Constants.Finance.Parameters.TotalDebit, 0, 20,ParameterDirection.Output, DBService.ParameterType.Double),
               // new DBService.Parameters( GTIService.Constants.Finance.Parameters.TotalCredit, 0, 20,ParameterDirection.Output, DBService.ParameterType.Double),

            };
            
            dtList = dbService.DataAdapter(CommandType.StoredProcedure,
                GTIService.Constants.Finance.Procedures.SPFIN_TRX_GET_LIST, colParameters).Tables[0];

            //decimal TotDebit = Convert.ToDecimal(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.TotalDebit]).Value);
            // decimal TotCredit = Convert.ToDecimal(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Finance.Parameters.TotalCredit]).Value);
            TotalDebit = 0;
            TotalCredit = 0;
            string objXml = string.Empty;

            foreach (DataRow row in dtList.Rows)
            {
                objXml += row[0].ToString();
            }

            FinTrxDetails retObj = null;
            if (!objXml.IsNullOrEmptyOrWhitespace())
                retObj = ERP.Utilities.CommonFunctions.XmlDeserialize<FinTrxDetails>(objXml);
            if (retObj != null && retObj.FinTrx != null && retObj.FinTrx.Count >= 2)
            {
                TotalDebit = retObj.FinTrx[1].FTR_DR_AMT_BC;
                TotalCredit = retObj.FinTrx[1].FTR_CR_AMT_BC;
                retObj.FinTrx.RemoveAt(1);
            }

            return retObj;
        }

        public static DataSet GetJournalGainLoss(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            DataSet dsGainLoss = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_GAIN_LOSS_GET, colParameters);
            return dsGainLoss;
        }
        /// <summary>
        /// Save and Submit Voucher(Direct Payment)
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="refID"></param>
        /// <returns></returns>
        public static long? SaveVoucher(string xmlDoc, out int refID, out string retNumber)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_TRX_WKF_SAVE, colParameters);
            long result = Convert.ToInt64(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            retNumber = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            refID = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            return result;
        }
        /// <summary>
        /// Save Journal Voucher
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="refID"></param>
        /// <param name="retNumber"></param>
        /// <returns></returns>
        public static long? SaveJournalVoucher(string xmlDoc, out int refID, out string retNumber, out string retAsrCode)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RET_ASR_CODE,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_TRX_VOUCHER_WKF_SAVE, colParameters);
            long result = Convert.ToInt64(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            retNumber = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            retAsrCode = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.P_RET_ASR_CODE]).Value);
            refID = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static long SaveBankReconciliation(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_BANK_RECONCILIATION_SAVE, colParameters);
            long result = Convert.ToInt64(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        


        /// <summary>
        /// Get Account Details
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static DataSet GetAccountDetails(int accountPk, int haspk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_COA_PK, accountPk),
                new DBService.Parameters(GTIService.Constants.Common.CommonConstants.P_ACTIVE, haspk)
            };
            DataSet dsAccountDtls = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_COA_MST_GET_KV, colParameters);
            return dsAccountDtls;
        }

        public static long SaveVoucherTemplate(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_VOUCHER_TEMPLATE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataSet GetVoucherTemplateList(int templatPK, int active, int templateType, string name)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                //new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_VLH_PK, templatPK==0?(object)DBNull.Value:templatPK),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_VLH_PK, templatPK),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_VLH_ACTIVE, active ==-1 ? (object)DBNull.Value: active ),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_VLH_TYPE, templateType==-1?(object)DBNull.Value:templateType),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_VLH_NAME, name)
            };
            DataSet dsTemplates = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_VOUCHER_TEMPLATE_GET_KV, colParameters);
            return dsTemplates;
        }

        public static DataSet GetVoucherTemplateDetails(int templatPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_VLH_PK, templatPK==0?(object)DBNull.Value:templatPK)
                
            };
            DataSet dsTemplates = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_VOUCHER_TEMPLATE_GET, colParameters);
            return dsTemplates;
        }

        public static int DeleteVoucherTemplate(int templatPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_VLH_PK, templatPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_VOUCHER_TEMPLATE_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int SaveVoucherArchiveDetails(int VoucherPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_FTH_PK, VoucherPK),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_TRX_ARCHIVE_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int UpdatePPCReconciliationDetails(int VoucherPK, int isCancel)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_FTH_PK, VoucherPK),  
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_IS_CANCEL, isCancel),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_PPC_RECONCILIATION_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int UpdatePDCReconciliationDetails(int VoucherPK, int isCancel)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_FTH_PK, VoucherPK),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_IS_CANCEL, isCancel), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_PDC_RECONCILIATION_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static int UpdateInvoiceStock(int RefPK, int PKUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_IVH_PK, RefPK),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_USER_PK, PKUser),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_INVOICE_STOCK_UPDATE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Save and Submit Direct Receipt Voucher  
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="refID"></param>
        /// <returns></returns>
        public static long? SaveDirectReceiptVoucher(string xmlDoc, out int refID, out string retNumber)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_TRX_DIRECT_RECEIPT_VOUCHER_WKF_SAVE, colParameters);
            long result = Convert.ToInt64(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            retNumber = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
            refID = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            return result;
        }

        public static DataTable GetVoucherVersionHistory(long VoucherPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_FTH_PK, VoucherPk)
                
            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPARC_FIN_TRX_VERSION_GET, colParameters).Tables[0];
            return dtResult;
        }


        public static DataTable GetVoucherDetails(string VoucherType, DateTime FromDate, DateTime ToDate, int Status, int Bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_REF_TYPE, VoucherType),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_FROM_DT, FromDate),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_TO_DT, ToDate),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_STATUS,Status >=0 ? Status:(object) DBNull.Value ),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_BIZUNIT, Bizunit),
               // new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_SUB_TYPE,   SubType == string.Empty ? (object) DBNull.Value :  SubType),

            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_TRX_GET_AUTO, colParameters).Tables[0];
            return dtResult;
        }

        public static DataSet GetVoucherDetailsRPT(string FTH_REF_TYPE, string VOUCHER_FROM, string VOUCHER_TO,int V_STATUS, int voucherversion)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_FTH_REF_TYPE, FTH_REF_TYPE),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_VOUCHER_FROM, VOUCHER_FROM),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_VOUCHER_TO, VOUCHER_TO),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_STATUS, V_STATUS>=0 ? V_STATUS:(object) DBNull.Value ),
                new DBService.Parameters(GTIService.Constants.Journalize.Parameter.P_VERSION, voucherversion),
               
            };
            DataSet dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_TRX_MULTIPLE_VOUCHER_RPT, colParameters);
            return dsResult;
        }

        public static int VoucherTransactionExcelImportValidation(string xmlDoc, out string Ret_Xml, out DataTable dt_text)
        {
            Ret_Xml="";
            dt_text = new DataTable();
            DataTable  dtResult= new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
               //  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNAME,string.Empty,200, ParameterDirection.Output, DBService.ParameterType.NVarChar)
                
            };
            int row = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_TRX_DIRECT_PAY_VOUCHER_IMPORT_VAL, colParameters);
            int Result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            //if(Result>0)
            //{
                dtResult= dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Journalize.Procedure.SPFIN_TRX_DIRECT_PAY_VOUCHER_IMPORT_VAL, colParameters).Tables[0];
            //}
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    dt_text = dtResult;
                }
                if (Result > 0)
                {
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtResult.Rows)
                        {
                            Ret_Xml += dr[0].ToString();
                        }
                    }
                }
            return Result;
        }
    }
}
