using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using BusinessObject.Inventory;

namespace ERPManager.Inventory
{
    public interface IAdmCheckListMstManager
    {
        List<ADM_CHECK_LIST_TYPE_CFG> GetCheckListTypes(ADM_CHECK_LIST_TYPE_CFG CheckListTypesObj);
        List<ADM_CHECK_LIST_GROUP_MST> GetCheckListGroups(ADM_CHECK_LIST_GROUP_MST CheckListGroupsObj,ServiceUtility utilityObj);
        int SaveCheckListGroups(List<ADM_CHECK_LIST_GROUP_MST> CheckListGroupsList);
        int DeleteCheckListGroups(List<ADM_CHECK_LIST_GROUP_MST> CheckListGroupsList);
        List<CheckListItems> GetCheckListItems(ADM_CHECK_LIST_ITEM_MST CheckListItemsObj, ServiceUtility utilityObj);
        int SaveCheckListItems(List<ADM_CHECK_LIST_ITEM_MST> CheckListItemsList);
        int DeleteCheckListItems(List<ADM_CHECK_LIST_ITEM_MST> CheckListItemsList);
    }
}
