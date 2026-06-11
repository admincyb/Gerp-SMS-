using System.Collections.Generic;
using System.ServiceModel;
using ERPData;
using ERPManager;
using System;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPOListService" in both code and config file together.
    [ServiceContract]
    public interface IPOListService 
    {
        #region PO List Functions
        [OperationContract]
        List<PUR_ORDER_HDR> GetPoHeader(PUR_ORDER_HDR objPoHeader, ServiceUtility utilityObj, int Status);
        [OperationContract]
        List<PUR_ORDER_DTL> GetPoDetails(int poPK, ServiceUtility utilityObj);
        [OperationContract]
        List<PUR_ORDER_HDR> GetSelectedPOs(List<long> poPkList, ServiceUtility utilityObj);
        [OperationContract]
        List<FIN_INVOICE_VND_TRX_MPG> GetInvoicedPOs(long invoicePK);
        #endregion
    }
}
