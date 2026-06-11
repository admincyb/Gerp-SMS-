using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;

namespace ERPService.Inventory
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAdmProductPropertiesService" in both code and config file together.
    [ServiceContract]
    public interface IAdmProductPropertiesService
    {
        #region Product Properties Functions
        [OperationContract]
        List<ADM_CONST_GRP_TYPE> GetGeneralPropertiesTypes(ADM_CONST_GRP_TYPE AdmConstGrpType);
        [OperationContract]
        List<ADM_CONST_MST> GetGeneralProperties(ADM_CONST_MST AdmProductionMstObj, ServiceUtility utilityObj);
        [OperationContract]
        int SaveGeneralProperties(List<ADM_CONST_MST> admProductionMstList);
        [OperationContract]
        int DeleteGeneralProperties(List<ADM_CONST_MST> admProductionMstList);
        [OperationContract]
        List<ADM_CONST_GRP> GetGeneralPropertiesGroups(ADM_CONST_GRP AdmConstGrpobj, ServiceUtility utilityObj);
        [OperationContract]
        int SaveGeneralPropertiesGroups(List<ADM_CONST_GRP> AdmConstGroupsList);
        [OperationContract]
        int DeleteGeneralPropertiesGroups(List<ADM_CONST_GRP> AdmConstGroupsList);
        #endregion
    }
}
