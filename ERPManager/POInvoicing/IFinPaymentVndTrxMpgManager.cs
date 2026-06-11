using System.Collections.Generic;
using ERPData;

namespace ERPManager
{
    public interface IFinPaymentVndTrxMpgManager
    {
        long? SavePaymentDtl(List<FIN_PAYMENT_VND_TRX_MPG> finPaymentVndTrxMpgList, byte Category);
        List<FIN_PAYMENT_VND_TRX_MPG> GetPaymentTrxMpg(long paymentPK);
        List<FIN_INVOICE_VND_HDR> GetPaymentTrxMpg(List<long> InvoicePK);
        List<FIN_PAYMENT_VND_TRX_MPG> GetInvoicePaymentTrxMpg(long invoicePK);
    }
}
