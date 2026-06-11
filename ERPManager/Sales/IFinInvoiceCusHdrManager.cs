using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
   
    public interface IFinInvoiceCusHdrManager
    {
        long SaveSalesInvoiceHdr(List<FIN_INVOICE_CUS_HDR> InvoiceHdrList, bool isWkfSave = false);

        List<FIN_INVOICE_CUS_HDR> GetSalesInvoiceHdr(FIN_INVOICE_CUS_HDR InvoiceHdrObj, ServiceUtility utilityObj = null, int? Status = null, string siNo = null);

        //List<FIN_INVOICE_CUS_HDR> GetSalesInvoiceHdrDetails(long invoicePK);

        List<FIN_INVOICE_CUS_HDR> GetSalesInvoiceNumberAutoCompleteList(FIN_INVOICE_CUS_HDR objSalesHeader, ServiceUtility utilityObj);

        //string GetSalesInvoiceNo(ApplicationSubType astPK, int dept, DateTime date, int user, bool update, int appPK);

        long DeleteSalesInvoice(long invoicePK);
    }
}
