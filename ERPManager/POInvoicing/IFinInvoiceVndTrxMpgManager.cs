using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{   
    public interface IFinInvoiceVndTrxMpgManager
    {
        long SaveInvoiceTrxMpg(List<FIN_INVOICE_VND_TRX_MPG> InvoiceTrxMpgList, bool isWkfSave = false);
       
        List<FIN_INVOICE_VND_TRX_MPG> GetInvoiceTrxMpg(FIN_INVOICE_VND_TRX_MPG InvoiceHdrObj, ServiceUtility utilityObj = null);
    }
}
