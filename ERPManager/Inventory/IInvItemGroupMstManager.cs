using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    public interface IInvItemGroupMstManager
    {
        List<INV_ITEM_GROUP_MST> GetInvItemGroupMst(INV_ITEM_GROUP_MST InvItemMstGroupObj, ServiceUtility utilityObj);
        int SaveInvItemGroupMst(List<INV_ITEM_GROUP_MST> InvItemMstGroupList,List<INV_ITEM_MST> InvItemMstList);
        int DeleteInvItemGroupMst(List<INV_ITEM_GROUP_MST> InvItemMstGroupList);
        List<INV_ITEM_MST> GetInvItemMst(INV_ITEM_MST InvItemMstObj, ItemCategory itemCategory = ItemCategory.FinishedGoods);
    }
}
