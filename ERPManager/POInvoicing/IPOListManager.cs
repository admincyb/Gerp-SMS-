using System.Collections.Generic;
using ERPData;


namespace ERPManager
{
   public interface IPOListManager
    {
        List<PUR_ORDER_HDR> GetPoHeader(PUR_ORDER_HDR objPoHeader, ServiceUtility utilityObj,int Status);
        List<PUR_ORDER_HDR> GetSelectedPOs(List<long> poPkList, ServiceUtility utilityObj);
        List<FIN_INVOICE_VND_TRX_MPG> GetInvoicedPOs(long invoicePK);
        List<PUR_ORDER_DTL> GetPoDetails(int poPK, ServiceUtility utilityObj);
        List<INV_GRN_HDR> GetGRNDetails(int poPK, ServiceUtility utilityObj);
        List<INV_GIN_HDR> GetGinDetails(int grnPK, ServiceUtility utilityObj);
        List<INV_STK_TRAN_HDR> GetStockTransferDetails(int ginPK, ServiceUtility utilityObj);
        List<PUR_ORDER_HDR> GetPoNumberAutoCompleteList(PUR_ORDER_HDR objPoHeader, ServiceUtility utilityObj);
        List<INV_GRN_DTL> GetGRNDetailsList(int poDtlPK, ServiceUtility utilityObj);
        List<INV_GIN_DTL> GetGINDetailsList(int grnDtlPK, ServiceUtility utilityObj);

    }
}
