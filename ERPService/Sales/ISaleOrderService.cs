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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISaleOrderService" in both code and config file together.
    [ServiceContract]
    public interface ISaleOrderService
    {
        [OperationContract]
        List<SAL_ORDER_HDR> GetSaleOrderHeader(SAL_ORDER_HDR objSaleOrderHeader, ServiceUtility utilityObj, ApplicationSubType applicationSubType, int? status,PageType pageType,int wrkfStatus=-1);
        [OperationContract]
        List<SAL_ORDER_HDR> GetSelectedSaleOrders(List<long> soPkList, ServiceUtility utilityObj);
        [OperationContract]
        List<FIN_INVOICE_CUS_TRX_MPG> GetInvoicedSaleOrders(long invoicePK);
        [OperationContract]
        List<SAL_ORDER_DTL> GetSaleOrderDetails(int soPK, ServiceUtility utilityObj);
        [OperationContract]
        List<SAL_ORDER_HDR> GetSoNumberAutoCompleteList(SAL_ORDER_HDR objSaleOrderHeader, ServiceUtility utilityObj);
    }
}
