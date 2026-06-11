using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using ERP.Utilities;
using System.Diagnostics;
using System.Data;
using ERPManager;
using ERPManager.Employee;

namespace ERPService.Employee
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeesService" in code, svc and config file together.
    public class EmployeesService : IEmployeesService
    {
       

        #region Private Variables
        ERPEntities currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public EmployeesService()
        {
            try
            {
                currentContext = new ERPEntities();
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vendorObj"></param>
        /// <returns></returns>
        public int SaveEmpEmployeeMst(List<EmpEmployeeMst> EmpEmployeeMstList)
        {
            EmpEmployeeMstManager EmpEmployeeMstMgr;
            int? empEmployeeMstPK;
            try
            {
                EmpEmployeeMstMgr = new EmpEmployeeMstManager(currentContext);
                empEmployeeMstPK = EmpEmployeeMstMgr.SaveEmpEmployeeMst(EmpEmployeeMstList);
                currentContext.SaveChanges();
                return empEmployeeMstPK.Value;
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                EmpEmployeeMstMgr = null;
                empEmployeeMstPK = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmpEmployeeMstList"></param>
        /// <returns></returns>
        public int DeleteEmpEmployeeMst(List<EmpEmployeeMst> EmpEmployeeMstList)
        {
            EmpEmployeeMstManager EmpEmployeeMstMgr;
            try
            {
                EmpEmployeeMstMgr = new EmpEmployeeMstManager(currentContext);
                EmpEmployeeMstMgr.DeleteEmpEmployeeMst(EmpEmployeeMstList);
                return currentContext.SaveChanges();
            }
            catch (OptimisticConcurrencyException ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (UpdateException ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                EmpEmployeeMstMgr = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmpEmployeeMstObj"></param>
        /// <returns></returns>
        public List<EmpEmployeeMst> GetEmpEmployeeMst(EmpEmployeeMst EmpEmployeeMstObj, ServiceUtility utilityObj = null)
        {
            EmpEmployeeMstManager EmpEmployeeMstMgr;
            try
            {
                EmpEmployeeMstMgr = new EmpEmployeeMstManager(currentContext);
                return EmpEmployeeMstMgr.GetEmpEmployeeMst(EmpEmployeeMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                EmpEmployeeMstMgr = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmpEmployeeMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public ServiceUtility GetEmpEmployesCount(EmpEmployeeMst EmpEmployeeMstObj, ServiceUtility utilityObj = null)
        {
            EmpEmployeeMstManager EmpEmployeeMstMgr;
            try
            {
                EmpEmployeeMstMgr = new EmpEmployeeMstManager(currentContext);
                return EmpEmployeeMstMgr.GetEmpEmployesCount(EmpEmployeeMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                EmpEmployeeMstMgr = null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EmpEmployeeMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public ServiceUtility GetEmpEmployeeMstCount(EmpEmployeeMst EmpEmployeeMstObj, ServiceUtility utilityObj = null)
        {
            EmpEmployeeMstManager EmpEmployeeMstMgr;
            try
            {
                EmpEmployeeMstMgr = new EmpEmployeeMstManager(currentContext);
                return EmpEmployeeMstMgr.GetEmpEmployeeMstCount(EmpEmployeeMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                EmpEmployeeMstMgr = null;
            }
        }
        /// <summary>
        /// Gets new instance of employee masters with default values
        /// </summary>
        /// <returns>employee master object</returns>
        public EmpEmployeeMst GetInitilizedEmpEmployeeMst()
        {
            EmpEmployeeMstManager empEmployeeMstMgr;
            try
            {
                empEmployeeMstMgr = new EmpEmployeeMstManager(currentContext);
                return empEmployeeMstMgr.GetInitilizedEmpEmployeeMst();
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        /// <summary>
        /// Gets List of Employees
        /// </summary>
        /// <returns>employee master object</returns>
        public List<EmpEmployeeMst> GetEmpEmployeeMstAutoCompleteList(EmpEmployeeMst empEmployeeMstObj, ServiceUtility utilityObj)
        {

            EmpEmployeeMstManager empEmployeeMstMgr;
            try
            {
                empEmployeeMstMgr = new EmpEmployeeMstManager(currentContext);
                return empEmployeeMstMgr.GetEmpEmployeeMstAutoCompleteList(empEmployeeMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }

        }
        /// <summary>
        /// Gets List of Employees who dosenot have a user ID
        /// </summary>
        /// <returns>employee master object</returns>
        public List<EmpEmployeeMst> GetNonUserEmpEmployeeMstAutoCompleteList(EmpEmployeeMst empEmployeeMstObj, ServiceUtility utilityObj)
        {
            EmpEmployeeMstManager empEmployeeMstMgr;
            try
            {
                empEmployeeMstMgr = new EmpEmployeeMstManager(currentContext);
                return empEmployeeMstMgr.GetNonUserEmpEmployeeMstAutoCompleteList(empEmployeeMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Get Employee Type Constant Data
        /// </summary>
        /// <param name="xacEmpTypeObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns></returns>
        public List<XacEmpType> GetXacEmpType(XacEmpType xacEmpTypeObj)
        {
            EmpEmployeeMstManager empEmployeeMstMgr;
            try
            {
                empEmployeeMstMgr = new EmpEmployeeMstManager(currentContext);
                return empEmployeeMstMgr.GetXacEmpType(xacEmpTypeObj);
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }
        #endregion
    }
}
