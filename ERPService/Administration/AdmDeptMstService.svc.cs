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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AdmDeptMstService" in code, svc and config file together.
    public class AdmDeptMstService : IAdmDeptMstService
    {
       #region Private Variables
        ERPEntities currentContext;
        #endregion
          #region Service Methods
        /// <summary>
        /// Constructor for ContainerInspectionService Service
        /// </summary>
        public AdmDeptMstService()
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


        public List<ADM_DEPT_MST> GetDepartmentList(ADM_DEPT_MST admDeptMstObj, ServiceUtility utilityObj)
        {
            AdmDeptMstManager admDeptMstManager;
            try
            {
                admDeptMstManager = new AdmDeptMstManager(currentContext);
                return admDeptMstManager.GetDepartmentList(admDeptMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admDeptMstManager = null;
            }
        }
        #endregion
    }
}
