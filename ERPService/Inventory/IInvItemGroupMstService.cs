using System.ServiceModel;
using System.Collections.Generic;
using ERPData;
using ERPManager;

namespace ERPService.Inventory
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IInvItemGroupMstService" in both code and config file together.
    [ServiceContract]
    public interface IInvItemGroupMstService
    {
        #region Product Group Functions
        [OperationContract]
        List<INV_ITEM_GROUP_MST> GetInvItemGroupMst(INV_ITEM_GROUP_MST InvItemMstGroupObj, ServiceUtility utilityObj);
        [OperationContract]
        int SaveInvItemGroupMst(List<INV_ITEM_GROUP_MST> InvItemMstGroupList, List<INV_ITEM_MST> InvItemMstList);
        [OperationContract]
        int DeleteInvItemGroupMst(List<INV_ITEM_GROUP_MST> InvItemMstGroupList);
        [OperationContract]
        List<INV_ITEM_MST> GetInvItemMst(INV_ITEM_MST InvItemMstObj, ItemCategory itemCategory = ItemCategory.FinishedGoods);
        #endregion
    }
}
