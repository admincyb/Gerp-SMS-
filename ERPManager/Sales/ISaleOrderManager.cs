using System.Collections.Generic;
using ERPData;


namespace ERPManager
{
   public interface ISaleOrderManager
    {
       List<SAL_ORDER_HDR> GetSaleOrderHeader(SAL_ORDER_HDR objSAL_ORDER_HDR, ServiceUtility utilityObj, ApplicationSubType applicationSubType, int? status,PageType pageType,int WrkfStatus=-1);
        List<SAL_ORDER_HDR> GetSelectedSaleOrders(List<long> soPkList, ServiceUtility utilityObj);
        List<FIN_INVOICE_CUS_TRX_MPG> GetInvoicedSaleOrders(long invoicePK);
        List<SAL_ORDER_DTL> GetSaleOrderDetails(int soPK, ServiceUtility utilityObj);
        List<SAL_ORDER_HDR> GetSoNumberAutoCompleteList(SAL_ORDER_HDR objSaleOrderHeader, ServiceUtility utilityObj);
    }
}
