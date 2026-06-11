using System;
using System.Collections.Generic;
using ERPData;
using System.Diagnostics;
using ERPManager;
using System.Data;
using ERPManager.Sales;


namespace ERPService.Sales
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ContainerInspectionService" in code, svc and config file together.
    public class ContainerInspectionService : IContainerInspectionService,IContainerInspectionManager
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

          #region Service Methods
        /// <summary>
        /// Constructor for ContainerInspectionService Service
        /// </summary>
        public ContainerInspectionService()
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


        /// <summary>
        /// Get ContainerInspection No
        /// </summary>
        /// <param name="astVal"></param>
        /// <param name="dept"></param>
        /// <param name="date"></param>
        /// <param name="user"></param>
        /// <param name="update"></param>
        /// <param name="appPK"></param>
        /// <returns></returns>
        public string GetContainerInspectionNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK, int? cmpanyPK=null)
        {
            CommonFunctionsManager commonFunctionsMgr;
            try
            {
                commonFunctionsMgr = new CommonFunctionsManager(currentContext);
                return commonFunctionsMgr.GetTrxDocNo(aptCode, astVal, dept, date, user, update, appPK, cmpanyPK);
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
                commonFunctionsMgr = null;
            }
        }

        public List<SAL_CONTAINER_INSP_HDR> GetContainerInspectionList(SAL_CONTAINER_INSP_HDR SALCONTAINEREVALHDRobj, ServiceUtility utilityObj)
        {
            ContainerInspectionManager ContainerInspectionMgr;
            try
            {
                ContainerInspectionMgr = new ContainerInspectionManager(currentContext);
                return ContainerInspectionMgr.GetContainerInspectionList(SALCONTAINEREVALHDRobj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                ContainerInspectionMgr = null;
            }
        }

        public int SaveContainerInspection(List<SAL_CONTAINER_INSP_HDR> SALCONTAINEREVALHDRobj)
        {
            ContainerInspectionManager ContainerInspectionMgr;
            int? ContainerInspectionPK;
            try
            {
                ContainerInspectionMgr = new ContainerInspectionManager(currentContext);
                ContainerInspectionPK = ContainerInspectionMgr.SaveContainerInspection(SALCONTAINEREVALHDRobj);
                currentContext.SaveChanges();
                return ContainerInspectionPK.Value;
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
                ContainerInspectionMgr = null;
                ContainerInspectionPK = null;
            }
        }

        public int DeleteContainerInspection(List<SAL_CONTAINER_INSP_HDR> SalContainerEvelHdrList)
        {
            ContainerInspectionManager ContainerInspectionMgr;
            try
            {
                ContainerInspectionMgr = new ContainerInspectionManager(currentContext);
                int result = ContainerInspectionMgr.DeleteContainerInspection(SalContainerEvelHdrList);
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
                ContainerInspectionMgr = null;
            }
        }

        public List<SAL_DESPATCH_HDR> GetDeliveryOrders(SAL_DESPATCH_HDR SAL_DESPATCH_HDRobj)
        {
            ContainerInspectionManager ContainerInspectionMgr;
            try
            {
                ContainerInspectionMgr = new ContainerInspectionManager(currentContext);
                return ContainerInspectionMgr.GetDeliveryOrders(SAL_DESPATCH_HDRobj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                ContainerInspectionMgr = null;
            }
        }
        #endregion
    }
}
