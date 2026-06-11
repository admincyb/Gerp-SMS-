using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    public interface IAdmProductPropertiesManager
    {
        List<ADM_CONST_GRP_TYPE> GetGeneralPropertiesTypes(ADM_CONST_GRP_TYPE AdmConstGrpType);
        List<ADM_CONST_MST> GetGeneralProperties(ADM_CONST_MST AdmProductionMstObj, ServiceUtility utilityObj);
        int SaveGeneralProperties(List<ADM_CONST_MST> admProductionMstList);
        int DeleteGeneralProperties(List<ADM_CONST_MST> admProductionMstList);
        List<ADM_CONST_GRP> GetGeneralPropertiesGroups(ADM_CONST_GRP AdmConstGrpobj, ServiceUtility utilityObj);
        int SaveGeneralPropertiesGroups(List<ADM_CONST_GRP> AdmConstGroupsList);
        int DeleteGeneralPropertiesGroups(List<ADM_CONST_GRP> AdmConstGroupsList);
       
    }
}
