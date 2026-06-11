using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;
using BusinessObject.CommonManagement;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPOInvoiceService" in both code and config file together.
    [ServiceContract]
    public interface IPOInvoiceService
    {
        [OperationContract]
        List<FIN_INVOICE_VND_HDR> GetInvoiceHdr(FIN_INVOICE_VND_HDR finInvoiceHdrObj, ServiceUtility serviceUtilityObj,
             DateTime? PayByDateFrom = null, DateTime? PayByDateTo = null, int? Status = null, string poNo = null);
        [OperationContract]
        ServiceUtility GetInvoiceHdrCount(FIN_INVOICE_VND_HDR finInvoiceHdrObj, ServiceUtility serviceUtilityObj);        
        [OperationContract]
        long? SaveInvoiceHdr(List<FIN_INVOICE_VND_HDR> finInvoiceHdrList, bool isWkfSave = false, bool IsAdvInvHasTax = true);
        [OperationContract]
        double GetConversionFactor(int FromCurrency, int ToCurrency, DateTime TrxDate, int BizUnit);
        [OperationContract]
        long SaveFinTrxDetails(int? pAppID, string pAppType);
        [OperationContract]
        long UpdateInvoiceHdrJounalizeFlag(int InvPK, bool JounalizeFlag);
        [OperationContract]
        string GetInvoiceNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK,int? cmpanyPK=null);
        [OperationContract]
        bool IsExistVendorInvoice(string vendorInvoiveNo, int vendorID, long invoicePK);
        [OperationContract]
        int AttachDocumentDelete(int? docPk, int? docTask, int? docTaskId);
    }
}
