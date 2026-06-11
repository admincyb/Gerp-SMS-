using System;
using System.Collections.Generic;
using ERPData;
using System.Diagnostics;
using ERPManager;
using System.Data;
using ERPManager.Inventory;

namespace ERPService.Inventory
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AdmProductionMstService" in code, svc and config file together.
    public class AdmProductPropertiesService : IAdmProductPropertiesService, IAdmProductPropertiesManager
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

        #region Service Methods
        /// <summary>
        /// Constructor for AdmPackingSpecMstService Service
        /// </summary>
        public AdmProductPropertiesService()
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

        public List<ADM_CONST_GRP_TYPE> GetGeneralPropertiesTypes(ADM_CONST_GRP_TYPE AdmConstGrpType)
        {
            AdmProductPropertiesManager productionMstMgr;
            try
            {
                productionMstMgr = new AdmProductPropertiesManager(currentContext);
                return productionMstMgr.GetGeneralPropertiesTypes(AdmConstGrpType);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                productionMstMgr = null;
            }
        }

        public List<ADM_CONST_MST> GetGeneralProperties(ADM_CONST_MST AdmProductionMstObj, ServiceUtility utilityObj)
        {
            AdmProductPropertiesManager productionMstMgr;
            try
            {
                productionMstMgr = new AdmProductPropertiesManager(currentContext);
                return productionMstMgr.GetGeneralProperties(AdmProductionMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                productionMstMgr = null;
            }
        }


        public int SaveGeneralProperties(List<ADM_CONST_MST> admProductionMstList)
        {
            AdmProductPropertiesManager productionMstMgr;
            int? accountMstPK;
            try
            {
                productionMstMgr = new AdmProductPropertiesManager(currentContext);
                accountMstPK = productionMstMgr.SaveGeneralProperties(admProductionMstList);
                currentContext.SaveChanges();
                return accountMstPK.Value;
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
                productionMstMgr = null;
                accountMstPK = null;
            }
        }

        public int DeleteGeneralProperties(List<ADM_CONST_MST> admProductionMstList)
        {
            AdmProductPropertiesManager productionMstMgr;
            try
            {
                productionMstMgr = new AdmProductPropertiesManager(currentContext);
                int result = productionMstMgr.DeleteGeneralProperties(admProductionMstList);
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
                productionMstMgr = null;
            }
        }

        public List<ADM_CONST_GRP> GetGeneralPropertiesGroups(ADM_CONST_GRP AdmConstGrpobj, ServiceUtility utilityObj)
        {
            AdmProductPropertiesManager productionMstMgr;
            try
            {
                productionMstMgr = new AdmProductPropertiesManager(currentContext);
                return productionMstMgr.GetGeneralPropertiesGroups(AdmConstGrpobj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                productionMstMgr = null;
            }
        }

        //Select the parent group
        public List<ADM_CONST_GRP> GetGeneralPropertiesParentGroup(ADM_CONST_GRP AdmConstGrpobj, ServiceUtility utilityObj)
        {
            AdmProductPropertiesManager productionMstMgr;
            try
            {
                productionMstMgr = new AdmProductPropertiesManager(currentContext);
                return productionMstMgr.GetGeneralPropertiesParentGroup(AdmConstGrpobj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                productionMstMgr = null;
            }
        }


        public int SaveGeneralPropertiesGroups(List<ADM_CONST_GRP> AdmConstGroupsList)
        {
            AdmProductPropertiesManager productionMstMgr;
            int? accountMstPK;
            try
            {
                productionMstMgr = new AdmProductPropertiesManager(currentContext);
                accountMstPK = productionMstMgr.SaveGeneralPropertiesGroups(AdmConstGroupsList);
                currentContext.SaveChanges();
                return accountMstPK.Value;
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
                productionMstMgr = null;
                accountMstPK = null;
            }
        }

        public int DeleteGeneralPropertiesGroups(List<ADM_CONST_GRP> AdmConstGroupsList)
        {
            AdmProductPropertiesManager productionMstMgr;
            try
            {
                productionMstMgr = new AdmProductPropertiesManager(currentContext);
                int result = productionMstMgr.DeleteGeneralPropertiesGroups(AdmConstGroupsList);
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
                productionMstMgr = null;
            }
        }

        #endregion
    }
}
