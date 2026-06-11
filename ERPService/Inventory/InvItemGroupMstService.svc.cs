using ERPManager;
using System.Collections.Generic;
using ERPData;
using System;
using System.Diagnostics;
using System.Data;

namespace ERPService.Inventory
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "InvItemGroupMstService" in code, svc and config file together.
    public class InvItemGroupMstService : IInvItemGroupMstService, IInvItemGroupMstManager
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

        #region Service Methods
        /// <summary>
        /// Constructor for InvItemGroupMstService
        /// </summary>
        public InvItemGroupMstService()
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


        public List<INV_ITEM_GROUP_MST> GetInvItemGroupMst(INV_ITEM_GROUP_MST InvItemMstGroupObj, ServiceUtility utilityObj)
        {
            InvItemGroupMstManager invItemGroupMstMgr;
            try
            {
                invItemGroupMstMgr = new InvItemGroupMstManager(currentContext);
                return invItemGroupMstMgr.GetInvItemGroupMst(InvItemMstGroupObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemGroupMstMgr = null;
            }
        }

        public int SaveInvItemGroupMst(List<INV_ITEM_GROUP_MST> InvItemMstGroupList, List<INV_ITEM_MST> InvItemMstList)
        {
            InvItemGroupMstManager invItemGroupMstMgr;
            int? invItemGroupMstPK;
            try
            {
                invItemGroupMstMgr = new InvItemGroupMstManager(currentContext);
                invItemGroupMstPK = invItemGroupMstMgr.SaveInvItemGroupMst(InvItemMstGroupList, InvItemMstList);
                currentContext.SaveChanges();
                return invItemGroupMstPK.Value;
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
                invItemGroupMstMgr = null;
                invItemGroupMstPK = null;
            }
        }

        public int DeleteInvItemGroupMst(List<INV_ITEM_GROUP_MST> InvItemMstGroupList)
        {
            InvItemGroupMstManager invItemGroupMstMgr;
            try
            {
                invItemGroupMstMgr = new InvItemGroupMstManager(currentContext);
                int result = invItemGroupMstMgr.DeleteInvItemGroupMst(InvItemMstGroupList);
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
                invItemGroupMstMgr = null;
            }
        }

        public List<INV_ITEM_MST> GetInvItemMst(INV_ITEM_MST InvItemMstObj, ItemCategory itemCategory = ItemCategory.FinishedGoods)
        {
            InvItemGroupMstManager invItemGroupMstMgr;
            try
            {
                invItemGroupMstMgr = new InvItemGroupMstManager(currentContext);
                return invItemGroupMstMgr.GetInvItemMst(InvItemMstObj,itemCategory);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemGroupMstMgr = null;
            }
        }

        public long? SaveDocAttachemts(List<ADM_DOC_ATTACH> DocAttachList, int DocTaskId, int Task)
        {
            InvItemGroupMstManager invItemGroupMstMgrObj;
            long? DocPk;
            try
            {
                invItemGroupMstMgrObj = new InvItemGroupMstManager(currentContext);
                DocPk = invItemGroupMstMgrObj.SaveDocAttachemts(DocAttachList, DocTaskId, Task);
                currentContext.SaveChanges();
                return DocPk.Value;
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
                invItemGroupMstMgrObj = null;
                DocPk = null;
            }
        }

        public List<ADM_DOC_ATTACH> GetDocAttachments(ADM_DOC_ATTACH admDocAttachObj, ServiceUtility serviceUtilityObj)
        {
            InvItemGroupMstManager invItemGroupMstMgrObj;
            try
            {
                invItemGroupMstMgrObj = new InvItemGroupMstManager(currentContext);
                return invItemGroupMstMgrObj.GetDocAttachments(admDocAttachObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemGroupMstMgrObj = null;
            }
        }
        /// <summary>
        /// Function Used For ProductGroup Duplication Checking
        /// </summary>
        /// <param name="invItemGroupMstMgrObj"></param>
        /// <returns></returns>
        public bool IsProductGroupExist(INV_ITEM_GROUP_MST invItemGroupMstMgrObject)
        {
            InvItemGroupMstManager invItemGroupMstMgrObj;
            try
            {
                invItemGroupMstMgrObj = new InvItemGroupMstManager(currentContext);
                return invItemGroupMstMgrObj.IsProductGroupExist(invItemGroupMstMgrObject);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                invItemGroupMstMgrObj = null;
            }
        }
        #endregion
    }
}