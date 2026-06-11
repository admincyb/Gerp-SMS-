using System.Collections.Generic;
using ERPData;

namespace ERPManager
{
    public interface IFinTrxManager
    {
        #region Private Variables
        long SaveFinTrx(List<FIN_TRX> finTrxList);
        int UpdateBankReconciliation(List<FIN_TRX> finTrxList);
        List<FIN_TRX> GetAccoutPayables(long VendorID, ServiceUtility utilityObj);
        int GetAccoutPayablesCount(long VendorID, ServiceUtility utilityObj);
        List<FIN_TRX> GetAccoutReceivables(long VendorID, ServiceUtility utilityObj);
        List<FIN_TRX> GetAccoutPayablesList(long ID, string TYpe, ServiceUtility utilityObj);
        List<FIN_TRX> GetBankReconcileList(FIN_TRX finTrxObj, int isReconciled, ServiceUtility utilityObj,ref decimal TotalDebit,ref decimal TotalCredit);
        #endregion
    }
}
