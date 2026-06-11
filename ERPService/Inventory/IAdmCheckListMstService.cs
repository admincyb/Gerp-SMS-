using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERPManager;
using BusinessObject.Inventory;

namespace ERPService.Inventory
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAdmCheckListMstService" in both code and config file together.
    [ServiceContract]
    public interface IAdmCheckListMstService
    {
        #region CheckList Master Functions
        [OperationContract]
        List<ADM_CHECK_LIST_TYPE_CFG> GetCheckListTypes(ADM_CHECK_LIST_TYPE_CFG CheckListTypesObj);
        [OperationContract]
        List<ADM_CHECK_LIST_GROUP_MST> GetCheckListGroups(ADM_CHECK_LIST_GROUP_MST CheckListGroupsObj, ServiceUtility utilityObj);
        [OperationContract]
        int SaveCheckListGroups(List<ADM_CHECK_LIST_GROUP_MST> CheckListGroupsList);
        [OperationContract]
        int DeleteCheckListGroups(List<ADM_CHECK_LIST_GROUP_MST> CheckListGroupsList);
        [OperationContract]
        List<CheckListItems> GetCheckListItems(ADM_CHECK_LIST_ITEM_MST CheckListItemsObj, ServiceUtility utilityObj);
        [OperationContract]
        int SaveCheckListItems(List<ADM_CHECK_LIST_ITEM_MST> CheckListItemsList);
        [OperationContract]
        int DeleteCheckListItems(List<ADM_CHECK_LIST_ITEM_MST> CheckListItemsList);
        #endregion
    }
}