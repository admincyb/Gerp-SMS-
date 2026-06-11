using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    interface IFinInvoiceCusDetailsManager
    {
        List<FIN_INVOICE_CUS_DTL> GetInvoiceCusDtlByPK(FIN_INVOICE_CUS_DTL InvoiceDtlObj);
    }
}
