using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    interface IFinReceiptCusTrxMpgManager
    {
        long? SaveReceiptDtl(List<FIN_RECEIPT_CUS_TRX_MPG> finReceiptCusTrxMpgList, Byte Category);
        List<FIN_RECEIPT_CUS_TRX_MPG> GetReceiptTrxMpg(long receiptPK);
        List<FIN_INVOICE_CUS_HDR> GetReceiptTrxMpg(List<long> InvoicePK);
    }
}
