using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    public interface IFinInvoiceVndHdrManager
    {
        long SaveInvoiceHdr(List<FIN_INVOICE_VND_HDR> InvoiceHdrList, bool isWkfSave = false, bool IsAdvInvHasTax = true);

        List<FIN_INVOICE_VND_HDR> GetInvoiceHdr(FIN_INVOICE_VND_HDR InvoiceHdrObj, ServiceUtility utilityObj = null, DateTime? PayByDateFrom = null, DateTime? PayByDateTo = null, int? Status = null, string poNo = null);

        //List<FIN_INVOICE_VND_HDR> GetInvoiceHdrDetails(long invoicePK);

        List<FIN_INVOICE_VND_HDR> GetInvoiceNumberAutoCompleteList(FIN_INVOICE_VND_HDR objPoHeader, ServiceUtility utilityObj);

        //string GetInvoiceNo(ApplicationSubType astPK, int dept, DateTime date, int user, bool update, int appPK);

        long DeleteInvoice(long invoicePK);
        long SaveFinTrxDetails(int? pAppID, string pAppType);
        long UpdateInvoiceHdrJounalizeFlag(int InvPK,bool JounalizeFlag);
        bool IsExistVendorInvoice(string vendorInvoiveNo, int vendorID, long invoicePK);
    }
}
