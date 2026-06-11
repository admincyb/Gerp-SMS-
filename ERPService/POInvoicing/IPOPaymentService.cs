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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPOPaymentService" in both code and config file together.
    [ServiceContract]
    public interface IPOPaymentService
    {
        [OperationContract]
        List<FIN_PAYMENT_VND_HDR> GetPaymentHdr(FIN_PAYMENT_VND_HDR finPaymentHdrObj, ServiceUtility serviceUtilityObj, int? Status = null, string invNo = null, int? PDCStatus = 0);
        [OperationContract]
        List<FIN_PAYMENT_VND_HDR> GetPaymentHdr(long paymentPK);
        [OperationContract]
        long? SavePaymentHdr(List<FIN_PAYMENT_VND_HDR> finPaymentHdrList);
        [OperationContract]
        List<FIN_INVOICE_VND_HDR> GetPaymentTrxMpg(List<long> InvoicePK);
        [OperationContract]
        List<FIN_PAYMENT_VND_HDR> GetPaymentNumberAutoCompleteList(FIN_PAYMENT_VND_HDR poPaymentObj, ServiceUtility serviceUtilityObj);
        [OperationContract]
        List<FIN_PAYMENT_VND_TRX_MPG> GetPaymentTrxMpg(long paymentPK);
        [OperationContract]
        string GetPaymentNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK, int? cmpanyPK=null);
        [OperationContract]
        double GetConversionFactor(int FromCurrency, int ToCurrency, DateTime TrxDate, int BizUnit);
        [OperationContract]
        long? DeletePaymentHdr(long CurrPK);
        [OperationContract]
        List<FIN_PAYMENT_VND_TRX_MPG> GetInvoicePaymentTrxMpg(long invoicePK);
        [OperationContract]
        bool CheckPaymentHdr(FIN_PAYMENT_VND_HDR finPaymentVndHdrObj);

    }
}
