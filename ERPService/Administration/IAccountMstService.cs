using System.Collections.Generic;
using System.ServiceModel;
using ERPData;
using ERPManager;

namespace ERPService.Administration
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAccountMstService" in both code and config file together.
    [ServiceContract]
    public interface IAccountMstService
    {
        #region Account Master Functions
        [OperationContract]
        List<FIN_COA_MST> GetFinCoaMst(FIN_COA_MST FinCoaMstObj, ServiceUtility utilityObj);
        [OperationContract]
        int SaveAccountsMaster(List<FIN_COA_MST> accountMstList,int ParentChildValidation);
        [OperationContract]
        int DeleteAccountsMaster(List<FIN_COA_MST> accountMstList);
        [OperationContract]
        List<FIN_COA_MST> GetCoaMstAutoCompleteList(FIN_COA_MST finCoaMstObj, string voucherType, int accType, ServiceUtility utilityObj,int CompanyPK);
        [OperationContract]
        List<FIN_COA_MST> GetAccountCoaMstAutoCompleteList(FIN_COA_MST finCoaMstObj, int accType, ServiceUtility utilityObj);
        #endregion
    }
}
