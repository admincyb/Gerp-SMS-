using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;

namespace ERPManager
{
    public interface IInvItemMstManager
    {
        List<INV_ITEM_MST> GetInvItemMst(INV_ITEM_MST InvItemMstObj, ServiceUtility utilityObj);
        int SaveInvItemMst(List<INV_ITEM_MST> InvItemMstList, int BizUnit);
        int DeleteInvItemMst(List<INV_ITEM_MST> InvItemMstList);
        List<INV_UOM_MST> GetUomMst(INV_UOM_MST InvUomMstObj, ServiceUtility utilityObj);
        List<INV_UOM_MST> GetUomMstAutoCompleteList(INV_UOM_MST invUOMObj, ServiceUtility utilityObj);
        List<SPADM_CONST_GRP_GET_KV_Result> GetControlsList(int? CngPk, int? CngGrpType, int? CgtVal, int? BizUnit, byte? Active, int? CngParent = null, string splCond = null);
        List<INV_ITEM_GROUP_MST> GetProductGroups(INV_ITEM_GROUP_MST InvItemGroupMstObj);
    }
}
