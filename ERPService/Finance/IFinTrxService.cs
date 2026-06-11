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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFinTrxService" in both code and config file together.
    [ServiceContract]
    public interface IFinTrxService
    {
        [OperationContract]
        long SaveFinTrx(List<FIN_TRX> finTrxList);
        [OperationContract]
        int UpdateBankReconciliation(List<FIN_TRX> finTrxList);
        [OperationContract]
        long SaveFinTrx(List<FIN_TRX_HDR> finTrxHdrList);
        [OperationContract]
        List<FIN_TRX> GetAccoutPayables(long VendorID, ServiceUtility utilityObj);
        [OperationContract]
        List<FIN_TRX> GetAccoutReceivables(long VendorID, ServiceUtility utilityObj);
        [OperationContract]
        int GetAccoutPayablesCount(long VendorID, ServiceUtility utilityObj);
        [OperationContract]
        long DeleteFinTrx(string REFTYPE, int REFPK , int FTHPK);
        [OperationContract]
        List<FIN_TRX> GetBankReconcileList(FIN_TRX finTrxObj, int isReconciled, ServiceUtility utilityObj,ref decimal TotalDebit,ref decimal TotalCredit);
    }
}
