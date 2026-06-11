using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    interface IFinReceiptCusHdrManager
    {
        List<FIN_RECEIPT_CUS_HDR> GetReceiptHdr(FIN_RECEIPT_CUS_HDR finRecipetHdrObj, ServiceUtility serviceUtilityObj, int? Status = null, string invNo = null, int? PDCStatus = 0);
        List<FIN_RECEIPT_CUS_HDR> GetReceiptHdr(long receiptPK);
        long? SaveReceiptHdr(List<FIN_RECEIPT_CUS_HDR> finReceiptVndHdrList);
        List<FIN_RECEIPT_CUS_HDR> GetReceiptNumberAutoCompleteList(FIN_RECEIPT_CUS_HDR salesReceiptObj, ServiceUtility serviceUtilityObj);
        bool CheckReceiptHdr(FIN_RECEIPT_CUS_HDR finReceiptCusHdrObj);
    }
}
