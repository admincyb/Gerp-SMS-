using System.Collections.Generic;
using ERPData;
using System;

namespace ERPManager
{
    interface IFinPaymentVndHdrManager
    {
        List<FIN_PAYMENT_VND_HDR> GetPaymentHdr(FIN_PAYMENT_VND_HDR finPaymentVndHdrObj, ServiceUtility serviceUtilityObj, int? Status = null, string invNo = null, int? PDCStatus = 0);
        List<FIN_PAYMENT_VND_HDR> GetPaymentHdr(long paymentPK);
        long? SavePaymentHdr(List<FIN_PAYMENT_VND_HDR> finPaymentVndHdrList);
        List<FIN_PAYMENT_VND_HDR> GetPaymentNumberAutoCompleteList(FIN_PAYMENT_VND_HDR poPaymentObj, ServiceUtility serviceUtilityObj);
        bool CheckPaymentHdr(FIN_PAYMENT_VND_HDR finPaymentVndHdrObj);
    }
}
