using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;
namespace ERPService
{
     [ServiceContract]
    public interface IFinInvoiceCusDetailService
    {
         [OperationContract]
         List<FIN_INVOICE_CUS_DTL> GetInvoiceCusDtlByPK(FIN_INVOICE_CUS_DTL InvoiceDtlObj);
    }
}