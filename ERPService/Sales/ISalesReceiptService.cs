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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISalesReceiptService" in both code and config file together.
    [ServiceContract]
    public interface ISalesReceiptService
    {
        [OperationContract]
        List<FIN_RECEIPT_CUS_HDR> GetReceiptHdr(FIN_RECEIPT_CUS_HDR finReceiptHdrObj, ServiceUtility serviceUtilityObj, int? Status = null, string invNo = null, int? PDCStatus = 0);
        [OperationContract]
        List<FIN_RECEIPT_CUS_HDR> GetReceiptHdr(long receiptPK);
        [OperationContract]
        long? SaveReceiptHdr(List<FIN_RECEIPT_CUS_HDR> finReceiptHdrList);
        [OperationContract]
        List<FIN_INVOICE_CUS_HDR> GetReceiptTrxMpg(List<long> InvoicePK);
        [OperationContract]
        List<FIN_RECEIPT_CUS_HDR> GetReceiptNumberAutoCompleteList(FIN_RECEIPT_CUS_HDR salesReceiptObj, ServiceUtility serviceUtilityObj);
        [OperationContract]
        List<FIN_RECEIPT_CUS_TRX_MPG> GetReceiptTrxMpg(long receiptPK);
        [OperationContract]
        string GetReceiptNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK, int? cmpanyPK = null, int? bizunit = null);
        [OperationContract]
        long? DeleteSalesReceiptHdr(long CurrPK);
        [OperationContract]
        double GetConversionFactor(int FromCurrency, int ToCurrency, DateTime TrxDate, int BizUnit);
        [OperationContract]
        bool CheckReceiptHdr(FIN_RECEIPT_CUS_HDR finReceiptCusHdrObj);
    }
}
