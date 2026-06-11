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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AdmCompanyMstService" in code, svc and config file together.
    public class AdmCompanyMstService : IAdmCompanyMstService
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion
          #region Service Methods
        /// <summary>
        /// Constructor for ContainerInspectionService Service
        /// </summary>
        public AdmCompanyMstService()
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


        public List<ADM_COMPANY_MST> GetCompanyList(ADM_COMPANY_MST admCompanyMstObj, ServiceUtility utilityObj)
        {
            AdmCompanyMstManager admCompanyMstManager;
            try
            {
                admCompanyMstManager = new AdmCompanyMstManager(currentContext);
                return admCompanyMstManager.GetCompanyList(admCompanyMstObj, utilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admCompanyMstManager = null;
            }
        }
        #endregion

    }
}
