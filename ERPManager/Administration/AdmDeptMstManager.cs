using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using ERP.Utilities;
using System.Diagnostics;

namespace ERPManager
{
    public class AdmDeptMstManager : IAdmDeptMstManager
    {
         #region Private Variables
        /// <summary>
        /// Gets or sets currency db context
        /// </summary>
        ERPEntities currentEntity;
        #endregion
        #region Manager Methods

        /// <summary>
        /// Initializes a new instance of country master manager
        /// </summary>
        /// <param name="currentEntity" value="Current db context"></param>
        public AdmDeptMstManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            //Handler for unknown exceptions
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        public List<ADM_DEPT_MST> GetDepartmentList(ADM_DEPT_MST admDeptMstObj, ServiceUtility utilityObj)
        {
            IQueryable<ADM_DEPT_MST> loadEntityQry;
            List<ADM_DEPT_MST> admDeptListObj;
            try
            {
                loadEntityQry = (from dep in this.currentEntity.ADM_DEPT_MST
                                 where dep.DPT_PK == (admDeptMstObj.DPT_PK == 0 ? dep.DPT_PK : admDeptMstObj.DPT_PK)
                                && dep.DPT_ACTIVE == admDeptMstObj.DPT_ACTIVE
                                 select dep);
                admDeptListObj = loadEntityQry.ToList();
                return admDeptListObj;
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                loadEntityQry = null;
                admDeptListObj = null;
            }
        }
        #endregion
    }
}
