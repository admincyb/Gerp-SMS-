using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.Journalize;

namespace BusinessLogic.Jouralize
{
    public class JournalizeBL
    {
        /// <summary>
        /// Get BankReconcile List
        /// </summary>
        /// <param name="finTrxObj"></param>
        /// <param name="isReconciled"></param>
        /// <param name="TotalDebit"></param>
        /// <param name="TotalCredit"></param>
        /// <returns></returns>
        public static FinTrxDetails GetBankReconcileList(FinTransactions finTrxObj, int isReconciled, ref decimal TotalDebit, ref decimal TotalCredit)
        {
            return DataAccess.Journalize.JournalizeDA.GetBankReconcileList(finTrxObj, isReconciled, ref TotalDebit, ref TotalCredit);
        }

        public static DataSet GetJournalGainLoss(string strxml)
        {
            return DataAccess.Journalize.JournalizeDA.GetJournalGainLoss(strxml);
        }
        /// <summary>
        /// Get Account Details
        /// </summary>
        /// <param name="accountPk"></param>
        /// <returns></returns>
        public static DataSet GetAccountDetails(int accountPk, int haspk)
        {
            return DataAccess.Journalize.JournalizeDA.GetAccountDetails(accountPk, haspk);
        }
         /// <summary>
        /// Save and Submit Voucher(Direct Payment)
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="refID"></param>
        /// <returns></returns>
        public static long? SaveVoucher(string xmlDoc, out int refID, out string retNumber)
        {
            return DataAccess.Journalize.JournalizeDA.SaveVoucher(xmlDoc, out refID, out retNumber);
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
            return DataAccess.Journalize.JournalizeDA.SaveJournalVoucher(xmlDoc, out refID, out retNumber, out retAsrCode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static long SaveBankReconciliation(string xmlDoc)
        {
            return DataAccess.Journalize.JournalizeDA.SaveBankReconciliation(xmlDoc);
        }
        public static long SaveVoucherTemplate(string xmlDoc)
        {
            return DataAccess.Journalize.JournalizeDA.SaveVoucherTemplate(xmlDoc);
        }

        public static DataSet GetVoucherTemplateList(int templatPK, int active, int templateType, string name = "")
        {
            return DataAccess.Journalize.JournalizeDA.GetVoucherTemplateList(templatPK, active, templateType, name);
        }

        public static DataSet GetVoucherTemplateDetails(int templatPK)
        {
            return DataAccess.Journalize.JournalizeDA.GetVoucherTemplateDetails(templatPK);
        }
        public static int DeleteVoucherTemplate(int templatPK)
        {
            return DataAccess.Journalize.JournalizeDA.DeleteVoucherTemplate(templatPK);
        }

        public static int SaveVoucherArchiveDetails(int VoucherPK)
        {
            return DataAccess.Journalize.JournalizeDA.SaveVoucherArchiveDetails(VoucherPK);
        }

        public static int UpdatePPCReconciliationDetails(int VoucherPK, int isCancel = 0)
        {
            return DataAccess.Journalize.JournalizeDA.UpdatePPCReconciliationDetails(VoucherPK, isCancel);
        }

        public static int UpdatePDCReconciliationDetails(int VoucherPK, int isCancel = 0)
        {
            return DataAccess.Journalize.JournalizeDA.UpdatePDCReconciliationDetails(VoucherPK, isCancel);
        }

        public static int UpdateInvoiceStock(int RefPK, int PKUser)
        {
            return DataAccess.Journalize.JournalizeDA.UpdateInvoiceStock(RefPK, PKUser);
        }

        #region Direct Receipt Voucher  
        /// <summary>
        /// Save and Submit Direct Receipt Voucher  
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="refID"></param>
        /// <returns></returns>
        public static long? SaveDirectReceiptVoucher(string xmlDoc, out int refID, out string retNumber)
        {
            return DataAccess.Journalize.JournalizeDA.SaveDirectReceiptVoucher(xmlDoc, out refID, out retNumber);
        }
        #endregion

        public static DataTable GetVoucherVersionHistory(long VoucherPk)
        {
            return DataAccess.Journalize.JournalizeDA.GetVoucherVersionHistory(VoucherPk);
        }

        public static DataTable GetVoucherDetails(string VoucherType, DateTime FromDate, DateTime ToDate, int Status, int Bizunit)
        {
            return DataAccess.Journalize.JournalizeDA.GetVoucherDetails(VoucherType, FromDate, ToDate, Status, Bizunit);
        }
        public static DataSet GetVoucherDetailsRPT(string FTH_REF_TYPE, String VOUCHER_FROM, string VOUCHER_TO,int V_STATUS, int voucherversion)
        {
            return DataAccess.Journalize.JournalizeDA.GetVoucherDetailsRPT(FTH_REF_TYPE, VOUCHER_FROM, VOUCHER_TO, V_STATUS,voucherversion);
        }

        public static int VoucherTransactionExcelImportValidation(string xmlDoc, out string Ret_Xml, out DataTable dt_text)
        {
            return DataAccess.Journalize.JournalizeDA.VoucherTransactionExcelImportValidation(xmlDoc, out Ret_Xml, out dt_text);
        }
        
    }
}
