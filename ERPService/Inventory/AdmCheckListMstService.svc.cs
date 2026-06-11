using System;
using System.Collections.Generic;
using ERPData;
using System.Diagnostics;
using ERPManager;
using System.Data;
using ERPManager.Inventory;
using BusinessObject.Inventory;

namespace ERPService.Inventory
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AdmCheckListMstService" in code, svc and config file together.
    public class AdmCheckListMstService : IAdmCheckListMstService,IAdmCheckListMstManager
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

        #region Service Methods
        /// <summary>
        /// Constructor for AdmPackingSpecMstService Service
        /// </summary>
        public AdmCheckListMstService()
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

        public List<ADM_CHECK_LIST_TYPE_CFG> GetCheckListTypes(ADM_CHECK_LIST_TYPE_CFG CheckListTypesObj)
        {
            AdmCheckListMstManager CheckListMstMgr;
            try
            {
                CheckListMstMgr = new AdmCheckListMstManager(currentContext);
                return CheckListMstMgr.GetCheckListTypes(CheckListTypesObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                CheckListMstMgr = null;
            }
        }

        public List<ADM_CHECK_LIST_GROUP_MST> GetCheckListGroups(ADM_CHECK_LIST_GROUP_MST CheckListGroupsObj, ServiceUtility utilityObj)
        {
            AdmCheckListMstManager CheckListMstMgr;
            try
            {
                CheckListMstMgr = new AdmCheckListMstManager(currentContext);
                return CheckListMstMgr.GetCheckListGroups(CheckListGroupsObj,  utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                CheckListMstMgr = null;
            }
        }

        public int SaveCheckListGroups(List<ADM_CHECK_LIST_GROUP_MST> CheckListGroupsList)
        {
            AdmCheckListMstManager CheckListMstMgr;
            int? groupPK;
            try
            {
                CheckListMstMgr = new AdmCheckListMstManager(currentContext);
                groupPK = CheckListMstMgr.SaveCheckListGroups(CheckListGroupsList);
                currentContext.SaveChanges();
                return groupPK.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                //throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
                return - 1;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                CheckListMstMgr = null;
                groupPK = null;
            }
        }

        public int DeleteCheckListGroups(List<ADM_CHECK_LIST_GROUP_MST> CheckListGroupsList)
        {
            AdmCheckListMstManager CheckListMstMgr;
            try
            {
                CheckListMstMgr = new AdmCheckListMstManager(currentContext);
                int result = CheckListMstMgr.DeleteCheckListGroups(CheckListGroupsList);
                currentContext.SaveChanges();
                return result;
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
                CheckListMstMgr = null;
            }
        }

        public List<CheckListItems> GetCheckListItems(ADM_CHECK_LIST_ITEM_MST CheckListItemsObj, ServiceUtility utilityObj)
        {
            AdmCheckListMstManager CheckListMstMgr;
            try
            {
                CheckListMstMgr = new AdmCheckListMstManager(currentContext);
                return CheckListMstMgr.GetCheckListItems(CheckListItemsObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                CheckListMstMgr = null;
            }
        }

        public int SaveCheckListItems(List<ADM_CHECK_LIST_ITEM_MST> CheckListItemsList)
        {
            AdmCheckListMstManager CheckListMstMgr;
            int? itemPK;
            try
            {
                CheckListMstMgr = new AdmCheckListMstManager(currentContext);
                itemPK = CheckListMstMgr.SaveCheckListItems(CheckListItemsList);
                currentContext.SaveChanges();
                return itemPK.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                //throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
                return -1;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                CheckListMstMgr = null;
                itemPK = null;
            }
        }

        public int DeleteCheckListItems(List<ADM_CHECK_LIST_ITEM_MST> CheckListItemsList)
        {
            AdmCheckListMstManager CheckListMstMgr;
            try
            {
                CheckListMstMgr = new AdmCheckListMstManager(currentContext);
                int result = CheckListMstMgr.DeleteCheckListItems(CheckListItemsList);
                currentContext.SaveChanges();
                return result;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                //throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
                return -1;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                CheckListMstMgr = null;
            }
        }


        #endregion
    }
}
