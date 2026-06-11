using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using DataAccess.POInvoicing;
using BusinessObject.POInvoicing;
using GTIService;
using BusinessObject.CommonManagement;
namespace BusinessLogic.POInvoicing
{
   public class DebitCreditTradingBL
    {
        /// <summary>
        /// Get Credit/Debit Notes Trading List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <param name="cusID"></param>
        /// <param name="InvPk"></param>
        /// <param name="PoPk"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
       public static DataSet GetDebitCreditTradingList(GridPrams grid, User objUser, int vendorID, int crdrPk, string invNo, string pageUrl, int type = 0, int status = 0, int cmpPk = 0)
        {
            return DebitCreditTradingDL.GetDebitCreditTradingList(grid, objUser, vendorID, crdrPk, invNo, pageUrl, type, status,cmpPk);
        }
       public static DataTable GetPendingInvList(User objUser, int venPK, int? invPK, int? drcrPk, int pageNumber, int pageSize, DateTime? FromDate, DateTime? ToDate,int invCategory, int invType)
       {
           return DataAccess.POInvoicing.DebitCreditTradingDL.GetPendingInvList(objUser, venPK, invPK, drcrPk, pageNumber, pageSize,FromDate,ToDate,invCategory,invType);
       }
       public static DataTable GetPendingPurchaseInvNumbers(int bizUnit, string searchValue, string vendorId,int? invCategory,int? invType=null)
       {
           return DataAccess.POInvoicing.DebitCreditTradingDL.GetPendingPurchaseInvNumbers(bizUnit, searchValue, vendorId,invCategory,invType);
       }
       public static DebitCreditTradingBO GetDirectPurchaseInvoiceHeaderMUL(string xmlDOSOPK, int cdhPK, byte IsTaxForOtherCharge)
       {
           try
           {
               DebitCreditTradingBO invoiceHeadermulObj = new DebitCreditTradingBO();
               string invoice = DataAccess.POInvoicing.DebitCreditTradingDL.GetDebitCreditHeaderMUL(xmlDOSOPK, cdhPK, IsTaxForOtherCharge);
               if (invoice != string.Empty)
               {
                   invoiceHeadermulObj = (DebitCreditTradingBO)CommonFunctions.DeserializeObject(invoice, invoiceHeadermulObj);
                   return invoiceHeadermulObj;
               }
               else
               {
                   return null;
               }
           }
           catch
           {
               throw;
           }
       }

       /// <summary>
       /// Save CreditDebit Trading
       /// </summary>
       /// <param name="strxml"></param>
       /// <returns></returns>
       public static int? SaveCreditDebitTradingWkf(string strxml, out string CrDrNo)
       {
           return DebitCreditTradingDL.SaveCreditDebitTradingWkf(strxml, out CrDrNo);
       }

       /// <summary>
       /// Delete PO Invoice Trading
       /// </summary>
       /// <param name="invPK"></param>
       /// <param name="lastModDate"></param>
       /// <returns></returns>
       public static int DeleteCreditDebitTradingDetails(int invPK, DateTime lastModDate, string appType, string currentUser)
       {
           return DebitCreditTradingDL.DeleteCreditDebitTradingDetails(invPK, lastModDate, appType, currentUser);
       }

       /// <summary>
       /// Get Direct(Trading) invoice number auto
       /// </summary>
       /// <param name="searchBy"></param>
       /// <param name="searchValue"></param>
       /// <param name="objUser"></param>
       /// <returns></returns>
       public static List<AutoCompleteBO> GetDrCrNoteNoTradingAutoComplete(string searchValue, User objUser, int type)
       {
           List<AutoCompleteBO> result;
           result = new List<AutoCompleteBO>();
           try
           {
               DataTable dtSearch = DataAccess.POInvoicing.DebitCreditTradingDL.GetDrCrNoteNoTradingAutoComplete(searchValue, objUser, type);
               result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
               {
                   LongKey = row.Field<long>(GTIService.Constants.Designation.Fields.PK),
                   Name = row.Field<string>(GTIService.Constants.Designation.Fields.VALUE)
               }).ToList();
           }
           catch
           {
           }
           return result;
       }
       public static bool ValidationForDRCRCancellation(int CurrPK)
       {
           return DebitCreditTradingDL.ValidationForDRCRCancellation(CurrPK);
       }
    }
}
