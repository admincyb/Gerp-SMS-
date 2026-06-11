using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;
using BusinessObject.Inventory;

namespace ERPService.Sales
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IContainerEvaluationService" in both code and config file together.
    [ServiceContract]
    public interface IContainerEvaluationService
    {
        #region Container Evaluation Functions

        [OperationContract]
        string GetContainerEvaluationNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK,int? cmpanyPK=null);
        [OperationContract]
        List<SAL_CONTAINER_EVAL_HDR> GetContainerEvaluationList(SAL_CONTAINER_EVAL_HDR SALCONTAINEREVALHDRobj, ServiceUtility utilityObj);
        [OperationContract]
        int SaveContainerEvaluation(List<SAL_CONTAINER_EVAL_HDR> SALCONTAINEREVALHDRobj);
        [OperationContract]
        int DeleteContainerEvaluation(List<SAL_CONTAINER_EVAL_HDR> SalContainerEvelHdrList);
        [OperationContract]
        List<PUR_VENDOR_MST> GetCompany(PUR_VENDOR_MST PUR_VENDOR_MSTobj);
        #endregion
    }
}
