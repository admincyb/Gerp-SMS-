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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ReportService" in code, svc and config file together.
    public class ReportService : IReportService
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public ReportService()
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

        #endregion

        public List<ADM_REPORT_GROUP_CFG> GetReportGroupAutoCompleteList(ADM_REPORT_GROUP_CFG admReportGroupCfgObj, ERPManager.ServiceUtility serviceUtilityObj)
        {

            AdmReportGroupCfgManager admReportGroupCfgMgr;
            try
            {
                admReportGroupCfgMgr = new AdmReportGroupCfgManager(currentContext);
                return admReportGroupCfgMgr.GetReportGroupAutoCompleteList(admReportGroupCfgObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admReportGroupCfgMgr = null;
            }
        }

        public List<ADM_REPORT_CFG> GetReportCfgAutoCompleteList(ADM_REPORT_CFG admReportCfgObj, ERPManager.ServiceUtility serviceUtilityObj)
        {
            AdmReportCfgManager admReportCfgMgr;
            try
            {
                admReportCfgMgr = new AdmReportCfgManager(currentContext);
                return admReportCfgMgr.GetReportCfgAutoCompleteList(admReportCfgObj, serviceUtilityObj);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admReportCfgMgr = null;
            }
        }

        public List<ADM_REPORT_CFG> GetReportCfg(int reportPK)
        {
            AdmReportCfgManager admReportCfgMgr;
            try
            {
                admReportCfgMgr = new AdmReportCfgManager(currentContext);
                return admReportCfgMgr.GetReportCfg(reportPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admReportCfgMgr = null;
            }
        }
    }
}
