using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using ERP.Utilities;
using System.Diagnostics;

namespace ERPManager
{
    public class AdmCompanyMstManager : IAdmCompanyMstManager
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
        public AdmCompanyMstManager(ERPEntities currentEntity)
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

        public List<ADM_COMPANY_MST> GetCompanyList(ADM_COMPANY_MST admCompanyMstObj, ServiceUtility utilityObj = null)
        {
            IQueryable<ADM_COMPANY_MST> loadEntityQry;
            List<ADM_COMPANY_MST> admCompanyListObj;
            try
            {
                loadEntityQry = (from cmp in this.currentEntity.ADM_COMPANY_MST
                                 where cmp.CMP_PK == (admCompanyMstObj.CMP_PK <= 0 ? cmp.CMP_PK : admCompanyMstObj.CMP_PK)
                                && cmp.CMP_ACTIVE == admCompanyMstObj.CMP_ACTIVE 
                                orderby cmp.CMP_DISPLAY_CODE,cmp.CMP_NAME
                                 select cmp);
                admCompanyListObj = loadEntityQry.ToList();
                //Set page size one if not given
                ////utilityObj = new ServiceUtility();
                ////utilityObj.PageSize = utilityObj.PageSize == 0 ? 1 : utilityObj.PageSize;
                ////utilityObj.TotalRecords = loadEntityQry.Count();
                //////Filter Query
                ////utilityObj.SortBy = "CMP_NAME";
                ////utilityObj.SortDirection = "ASC";
                ////admCompanyListObj = loadEntityQry.SortRecords<ADM_COMPANY_MST>(utilityObj).ToList();
                return admCompanyListObj;
            }
            catch (Exception ex)
            {
                throw CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                loadEntityQry = null;
                admCompanyListObj = null;
            }
        }        
        #endregion
    }
}
