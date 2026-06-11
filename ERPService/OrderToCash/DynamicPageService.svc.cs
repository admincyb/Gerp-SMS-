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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DynamicPageService" in code, svc and config file together.
    public class DynamicPageService : IDynamicPageService
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public DynamicPageService()
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


        public List<SPADM_FORM_TAB_CFG_GET_Result> GetFormTabList(string formCode)
        {
            AdmFormTabCfgManager admFormTabCfgManagerObj;
            try
            {
                admFormTabCfgManagerObj = new AdmFormTabCfgManager(currentContext);
                return admFormTabCfgManagerObj.GetFormTabList(formCode);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admFormTabCfgManagerObj = null;
            }
        }

        public List<SPADM_FORM_TAB_CONTROL_CFG_GET_Result> GetFormTabControlsList(string formCode, string tabCode,int userPk=0,int sbuPK=1)
        {
            AdmFormTabControlCfgManager admFormTabControlCfgManagerObj;
            try
            {
                admFormTabControlCfgManagerObj = new AdmFormTabControlCfgManager(currentContext);
                return admFormTabControlCfgManagerObj.GetFormTabControlsList(formCode, tabCode, userPk, sbuPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admFormTabControlCfgManagerObj = null;
            }
        }

        public List<TextMaster> GetCrDbAmountList(string Query, int TransactionPK)
        {
            AdmFormTabControlCfgManager admFormTabControlCfgManagerObj;
            try
            {
                admFormTabControlCfgManagerObj = new AdmFormTabControlCfgManager(currentContext);
                return admFormTabControlCfgManagerObj.GetCrDbAmountList(Query, TransactionPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admFormTabControlCfgManagerObj = null;
            }
        }
        #endregion

        public List<SPADM_REPORT_CONTROL_CFG_GET_Result> GetReportControlsList(int reportPK)
        {
            AdmReportControlCfgManager admFormTabControlCfgManagerObj;
            try
            {
                admFormTabControlCfgManagerObj = new AdmReportControlCfgManager(currentContext);
                return admFormTabControlCfgManagerObj.GetReportControlsList(reportPK);
            }
            catch (Exception ex)
            {
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
            finally
            {
                admFormTabControlCfgManagerObj = null;
            }
        }
    }
}
