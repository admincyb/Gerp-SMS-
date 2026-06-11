using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISalesInvoiceService" in both code and config file together.
    [ServiceContract]
    public interface ISalesInvoiceService
    {
        [OperationContract]
        List<FIN_INVOICE_CUS_HDR> GetSalesInvoiceHdr(FIN_INVOICE_CUS_HDR finSalesInvoiceHdrObj, ServiceUtility serviceUtilityObj, int? Status = null, string siNo = null);
        [OperationContract]       
        List<POPayment> GetSalesInvoiceHdr(long invoicePK);
        [OperationContract]
        long SaveSalesInvoiceHdr(List<FIN_INVOICE_CUS_HDR> finSalesInvoiceHdrList, bool isWkfSave = false);
        [OperationContract]
        List<FIN_INVOICE_CUS_HDR> GetInvoiceCusHdrByPK(FIN_INVOICE_CUS_HDR InvoiceHdrObj);
    }
}
