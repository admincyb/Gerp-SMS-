using ERPManager;
using System.Collections.Generic;
using ERPData;
using System;
using System.Diagnostics;
using System.Data;

namespace ERPService.Inventory
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InvItemMstService" in code, svc and config file together.
    public class InvItemMstService : IInvItemMstService, IInvItemMstManager
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

        #region Service Methods
        /// <summary>
        /// Constructor for InvItemMstService
        /// </summary>
        public InvItemMstService()
        {
            try
            {
                currentContext = new ERPEntities();
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public List<INV_ITEM_MST> GetInvItemMst(INV_ITEM_MST InvItemMstObj, ServiceUtility utilityObj)
        {
            InvItemMstManager invItemMstMgr;
            try
            {
                invItemMstMgr = new InvItemMstManager(currentContext);
                return invItemMstMgr.GetInvItemMst(InvItemMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstMgr = null;
            }
        }

        public List<INV_ITEM_MST> GetInvItemMstAdvSearch(INV_ITEM_MST InvItemMstObj, ServiceUtility utilityObj, int ItemCategoryPrdn,int IsSBUSpecific,int BizUnit)
        {
            InvItemMstManager invItemMstMgr;
            try
            {
                invItemMstMgr = new InvItemMstManager(currentContext);
                return invItemMstMgr.GetInvItemMstAdvSearch(InvItemMstObj, utilityObj, ItemCategoryPrdn, IsSBUSpecific, BizUnit);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstMgr = null;
            }
        }

        public int SaveInvItemMst(List<INV_ITEM_MST> InvItemMstList,int BizUnit)
        {
            InvItemMstManager invItemMstMgr;
            int? invItemMstPK;
            try
            {
                invItemMstMgr = new InvItemMstManager(currentContext);
                invItemMstPK = invItemMstMgr.SaveInvItemMst(InvItemMstList, BizUnit);
                currentContext.SaveChanges();
                return invItemMstPK.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstMgr = null;
                invItemMstPK = null;
            }
        }

        public int DeleteInvItemMst(List<INV_ITEM_MST> InvItemMstList)
        {
            InvItemMstManager invItemMstMgr;
            try
            {
                invItemMstMgr = new InvItemMstManager(currentContext);
                int result = invItemMstMgr.DeleteInvItemMst(InvItemMstList);
                currentContext.SaveChanges();
                return result;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
               // throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
                return -1;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstMgr = null;
            }
        }
        
        public List<INV_UOM_MST> GetUomMst(INV_UOM_MST InvUomMstObj, ServiceUtility utilityObj)
        {
            InvItemMstManager invItemMstMgr;
            try
            {
                invItemMstMgr = new InvItemMstManager(currentContext);
                return invItemMstMgr.GetUomMst(InvUomMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstMgr = null;
            }
        }
        
        public List<INV_UOM_MST> GetUomMstAutoCompleteList(INV_UOM_MST invUOMObj, ServiceUtility utilityObj)
        {
            InvItemMstManager invItemMstMgr;
            try
            {
                invItemMstMgr = new InvItemMstManager(currentContext);
                return invItemMstMgr.GetUomMstAutoCompleteList(invUOMObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstMgr = null;
            }
        }

        public List<SPADM_CONST_GRP_GET_KV_Result> GetControlsList(int? CngPk, int? CngGrpType, int? CgtVal, int? BizUnit, byte? Active, int? CngParent = null, string splCond = null)
        {
            InvItemMstManager invItemMstManagerObj;
            try
            {
                invItemMstManagerObj = new InvItemMstManager(currentContext);
                return invItemMstManagerObj.GetControlsList(CngPk, CngGrpType, CgtVal, BizUnit, Active, CngParent, splCond);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstManagerObj = null;
            }
        }

        public List<INV_ITEM_GROUP_MST> GetProductGroups(INV_ITEM_GROUP_MST InvItemGroupMstObj)
        {
            InvItemMstManager invItemMstManagerObj;
            try
            {
                invItemMstManagerObj = new InvItemMstManager(currentContext);
                return invItemMstManagerObj.GetProductGroups(InvItemGroupMstObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstManagerObj = null;
            }
        }
        #endregion

        /// <summary>
        /// Function Used For Item Code Duplication Checking
        /// </summary>
        /// <param name="invItemMstObj"></param>
        /// <returns></returns>
        public bool IsItemCodeExist(INV_ITEM_MST invItemMstObj,int IsSBUSpecific)
        {
            InvItemMstManager invItemMstManagerObj;
            try
            {
                invItemMstManagerObj = new InvItemMstManager(currentContext);
                return invItemMstManagerObj.IsItemCodeExist(invItemMstObj, IsSBUSpecific);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstManagerObj = null;
            }
        }

        public List<INV_ITEM_REL_MAP> GetRelatedItems(int ItemPk)
        {
            InvItemMstManager invItemMstManagerObj;
            try
            {
                invItemMstManagerObj = new InvItemMstManager(currentContext);
                return invItemMstManagerObj.GetRelatedItems(ItemPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstManagerObj = null;
            }
        }

        public List<INV_ITEM_SUB_TYPE_MAP> GetSubTypeItems(int ItemPk)
        {
            InvItemMstManager invItemMstManagerObj;
            try
            {
                invItemMstManagerObj = new InvItemMstManager(currentContext);
                return invItemMstManagerObj.GetSubTypeItems(ItemPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstManagerObj = null;
            }
        }
        public List<INV_ITEM_PACK_ITEM_MAP> GetPackMatItems(int ItemPk)
        {
            InvItemMstManager invItemMstManagerObj;
            try
            {
                invItemMstManagerObj = new InvItemMstManager(currentContext);
                return invItemMstManagerObj.GetPackMatItems(ItemPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstManagerObj = null;
            }
        }

        public INV_ITEM_MST GetInvItemMst(int productPk)
        {
            InvItemMstManager invItemMstManagerObj;
            try
            {
                invItemMstManagerObj = new InvItemMstManager(currentContext);
                return invItemMstManagerObj.GetInvItemMst(productPk);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstManagerObj = null;
            }
        }

        public int SaveInvRelatedItemMap(List<INV_ITEM_REL_MAP> invRelItemMapList, int ParentItemPk)
        {            
            InvItemMstManager invItemMstMgr;
            int? invItemMstPK;
            try
            {
                invItemMstMgr = new InvItemMstManager(currentContext);
                invItemMstPK = invItemMstMgr.SaveInvRelatedItemMap(invRelItemMapList, ParentItemPk);
                currentContext.SaveChanges();
                return invItemMstPK.Value;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstMgr = null;
            }
        }

        public int SaveInvSubTypeItemMap(List<INV_ITEM_SUB_TYPE_MAP> invRelItemMapList, int ParentItemPk)
        {
            InvItemMstManager invItemMstMgr;
            int? invItemMstPK;
            try
            {
                invItemMstMgr = new InvItemMstManager(currentContext);
                invItemMstPK = invItemMstMgr.SaveInvSubTypeItemMap(invRelItemMapList, ParentItemPk);
                currentContext.SaveChanges();
                return invItemMstPK.Value;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstMgr = null;
            }
        }

        public int SaveInvPackMatItemMap(List<INV_ITEM_PACK_ITEM_MAP> invRelItemMapList, int ParentItemPk)
        {
            InvItemMstManager invItemMstMgr;
            int? invItemMstPK;
            try
            {
                invItemMstMgr = new InvItemMstManager(currentContext);
                invItemMstPK = invItemMstMgr.SaveInvPackMatItemMap(invRelItemMapList, ParentItemPk);
                currentContext.SaveChanges();
                return invItemMstPK.Value;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstMgr = null;
            }
        }

        public int DeleteInvRelatedItemMap(INV_ITEM_REL_MAP objInvItemRelMap)
        {
            InvItemMstManager invItemMstMgr;
            try
            {
                invItemMstMgr = new InvItemMstManager(currentContext);
                int result = invItemMstMgr.DeleteInvRelatedItemMap(objInvItemRelMap);
                currentContext.SaveChanges();
                return result;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                // throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
                return -1;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstMgr = null;
            }
        }

        public int DeleteInvSubTypeItemMap(INV_ITEM_SUB_TYPE_MAP objInvItemRelMap)
        {
            InvItemMstManager invItemMstMgr;
            try
            {
                invItemMstMgr = new InvItemMstManager(currentContext);
                int result = invItemMstMgr.DeleteInvSubTypeItemMap(objInvItemRelMap);
                currentContext.SaveChanges();
                return result;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                // throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
                return -1;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstMgr = null;
            }
        }
        public int DeleteInvPackMatItemMap(INV_ITEM_PACK_ITEM_MAP objInvItemRelMap)
        {
            InvItemMstManager invItemMstMgr;
            try
            {
                invItemMstMgr = new InvItemMstManager(currentContext);
                int result = invItemMstMgr.DeleteInvPackMatItemMap(objInvItemRelMap);
                currentContext.SaveChanges();
                return result;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                // throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
                return -1;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstMgr = null;
            }
        }

        public List<SPPRD_INV_ITEM_PACK_COMB_GET_Result> GetPrdPackingCombinations(int BizUnit, int GroupItem, int prdItem)
        {
            InvItemMstManager invItemMstManagerObj;
            try
            {
                invItemMstManagerObj = new InvItemMstManager(currentContext);
                return invItemMstManagerObj.GetPrdPackingCombinations(BizUnit, GroupItem, prdItem);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemMstManagerObj = null;
            }
        }
    }
}