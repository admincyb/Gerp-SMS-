using System.ServiceModel;
using System.Collections.Generic;
using ERPData;
using ERPManager;

namespace ERPService.Inventory
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IInvItemMstService" in both code and config file together.
    [ServiceContract]
    public interface IInvItemMstService
    {
        #region Product Master Functions
        [OperationContract]
        List<INV_ITEM_MST> GetInvItemMst(INV_ITEM_MST InvItemMstObj, ServiceUtility utilityObj);
        [OperationContract]
        int SaveInvItemMst(List<INV_ITEM_MST> InvItemMstList,int BizUnit);
        [OperationContract]
        int DeleteInvItemMst(List<INV_ITEM_MST> InvItemMstList);
        [OperationContract]
        List<INV_UOM_MST> GetUomMst(INV_UOM_MST InvUomMstObj, ServiceUtility utilityObj);
        [OperationContract]
        List<INV_UOM_MST> GetUomMstAutoCompleteList(INV_UOM_MST invUOMObj, ServiceUtility utilityObj);
        [OperationContract]
        List<SPADM_CONST_GRP_GET_KV_Result> GetControlsList(int? CngPk, int? CngGrpType, int? CgtVal, int? BizUnit, byte? Active, int? CngParent = null, string splCond = null);
        [OperationContract]
        List<INV_ITEM_GROUP_MST> GetProductGroups(INV_ITEM_GROUP_MST InvItemGroupMstObj);
        #endregion
    }
}
