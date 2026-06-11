using System;
using System.Collections.Generic;
using ERPData;
using System.Diagnostics;
using ERPManager;
using System.Data;
using ERPManager.Sales;


namespace ERPService.Sales
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ContainerEvaluationService" in code, svc and config file together.
    public class ContainerEvaluationService : IContainerEvaluationService,IContainerEvaluationManager
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion

          #region Service Methods
        /// <summary>
        /// Constructor for ContainerEvaluationService Service
        /// </summary>
        public ContainerEvaluationService()
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
        /// Get ContainerEvaluation No
        /// </summary>
        /// <param name="astVal"></param>
        /// <param name="dept"></param>
        /// <param name="date"></param>
        /// <param name="user"></param>
        /// <param name="update"></param>
        /// <param name="appPK"></param>
        /// <returns></returns>
        public string GetContainerEvaluationNo(string aptCode, int astVal, int dept, DateTime date, int user, bool update, int appPK,int? cmpanyPk=null)
        {
            CommonFunctionsManager commonFunctionsMgr;
            try
            {
                commonFunctionsMgr = new CommonFunctionsManager(currentContext);
                return commonFunctionsMgr.GetTrxDocNo(aptCode, astVal, dept, date, user, update, appPK,cmpanyPk);
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

        public List<SAL_CONTAINER_EVAL_HDR> GetContainerEvaluationList(SAL_CONTAINER_EVAL_HDR SALCONTAINEREVALHDRobj, ServiceUtility utilityObj)
        {
            ContainerEvaluationManager ContainerInspectionMgr;
            try
            {
                ContainerInspectionMgr = new ContainerEvaluationManager(currentContext);
                return ContainerInspectionMgr.GetContainerEvaluationList(SALCONTAINEREVALHDRobj, utilityObj);
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

        public int SaveContainerEvaluation(List<SAL_CONTAINER_EVAL_HDR> SALCONTAINEREVALHDRobj)
        {
            ContainerEvaluationManager ContainerInspectionMgr;
            int? ContainerInspectionPK;
            try
            {
                ContainerInspectionMgr = new ContainerEvaluationManager(currentContext);
                ContainerInspectionPK = ContainerInspectionMgr.SaveContainerEvaluation(SALCONTAINEREVALHDRobj);
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

        public int DeleteContainerEvaluation(List<SAL_CONTAINER_EVAL_HDR> SalContainerEvelHdrList)
        {
            ContainerEvaluationManager ContainerInspectionMgr;
            try
            {
                ContainerInspectionMgr = new ContainerEvaluationManager(currentContext);
                int result = ContainerInspectionMgr.DeleteContainerEvaluation(SalContainerEvelHdrList);
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

        public List<PUR_VENDOR_MST> GetCompany(PUR_VENDOR_MST PUR_VENDOR_MSTobj)
        {
            ContainerEvaluationManager ContainerInspectionMgr;
            try
            {
                ContainerInspectionMgr = new ContainerEvaluationManager(currentContext);
                return ContainerInspectionMgr.GetCompany(PUR_VENDOR_MSTobj);
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
