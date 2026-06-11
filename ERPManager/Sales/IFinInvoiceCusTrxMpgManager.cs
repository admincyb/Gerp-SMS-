using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    
    public interface IFinInvoiceCusTrxMpgManager
    {
        long SaveSalesInvoiceTrxMpg(List<FIN_INVOICE_CUS_TRX_MPG> InvoiceTrxMpgList,byte type, bool isWkfSave = false);

        List<FIN_INVOICE_CUS_TRX_MPG> GetSalesInvoiceTrxMpg(FIN_INVOICE_CUS_TRX_MPG InvoiceHdrObj, ServiceUtility utilityObj = null);
    }
}
