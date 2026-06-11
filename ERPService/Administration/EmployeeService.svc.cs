using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using ERPData;
using System.Diagnostics;
using ERPManager;

namespace ERPService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "EmployeeService" in code, svc and config file together.
    public class EmployeeService : IEmployeeService
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion
        public EmployeeService()
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

        public  List<EmpEmployeeMst> GetEmployeeMst(EmpEmployeeMst EmployeeMstObj, ServiceUtility utilityObj)
        {
            EmployeeMstManager EmployeeMstMgr;
            try
            {
                EmployeeMstMgr = new EmployeeMstManager(currentContext);
                return EmployeeMstMgr.GetEmployeeMst(EmployeeMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                EmployeeMstMgr = null;
            }
        }

        public int DeleteEmployeeMst(List<EmpEmployeeMst> EmployeeMstList)
        {
            EmployeeMstManager EmployeeMstMgr;
            int? PK;
            try
            {
                EmployeeMstMgr = new EmployeeMstManager(currentContext);
                PK = EmployeeMstMgr.DeleteEmployeeMst(EmployeeMstList);
                currentContext.SaveChanges();
                return PK.Value;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                EmployeeMstMgr = null;
            }
        }

        public ServiceUtility GetEmployeeMstCount(EmpEmployeeMst EmployeeMstObj, ServiceUtility utilityObj)
        {
            EmployeeMstManager EmployeeMstMgr;
            try
            {
                EmployeeMstMgr = new EmployeeMstManager(currentContext);
                return EmployeeMstMgr.GetEmployeeMstCount(EmployeeMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                EmployeeMstMgr = null;
            }
        }

        public int SaveEmployeeMst(List<EmpEmployeeMst> EmployeeMstList)
        {
            EmployeeMstManager EmployeeMstMgr;
            int? PK;
            try
            {
                EmployeeMstMgr = new EmployeeMstManager(currentContext);
                PK = EmployeeMstMgr.SaveEmployeeMst(EmployeeMstList);
                currentContext.SaveChanges();
                return PK.Value;
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                EmployeeMstMgr = null;
            }
        }
        public List<EmpEmployeeMst> GetEmployeeMstAutoCompleteList(EmpEmployeeMst EmployeeMstObj, ServiceUtility utilityObj)
        {
            EmployeeMstManager EmpEmployeeMstMgr;
            try
            {
                EmpEmployeeMstMgr = new EmployeeMstManager(currentContext);
                return EmpEmployeeMstMgr.GetEmployeeMstAutoCompleteList(EmployeeMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                EmpEmployeeMstMgr = null;
            }
        }
    }
}
