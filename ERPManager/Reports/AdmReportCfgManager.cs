using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;

namespace ERPManager
{
    public class AdmReportCfgManager : IAdmReportCfgManager
    {
         #region Private Variables
        /// <summary>
        /// Holds current database context
        /// </summary>
        ERPEntities currentEntity;
        #endregion
        #region Manager Methods
        /// <summary>
        /// Payment Header Manager Construcor
        /// </summary>
        /// <param name="currentEntity"></param>
        /// <example></example>
        public AdmReportCfgManager(ERPEntities currentEntity)
        {
            try
            {
                this.currentEntity = currentEntity;
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);
            }
        }

        /// <summary>
        /// Get Payment Header List
        /// </summary>
        /// <param name="finPaymentHdrObj"></param>
        /// <param name="serviceUtilityObj"></param>
        /// <returns></returns>
        public List<ADM_REPORT_CFG> GetReportCfg(int reportPK)
        {
            List<ADM_REPORT_CFG> ADM_REPORT_CFG_Obj = new List<ADM_REPORT_CFG>();

            try
            {
                ADM_REPORT_CFG_Obj = (from rpt in this.currentEntity.ADM_REPORT_CFG
                                      where (rpt.RPT_PK == reportPK)
                                      select rpt).ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);

            }

            return ADM_REPORT_CFG_Obj;

        }

        public List<ADM_REPORT_CFG> GetReportCfgAutoCompleteList(ADM_REPORT_CFG admReportCfgObj, ServiceUtility utilityObj)
        {
            List<ADM_REPORT_CFG> ADM_REPORT_CFG_obj = new List<ADM_REPORT_CFG>();

            IQueryable<ADM_REPORT_CFG> ADM_REPORT_CFG_Qry;
            IQueryable<ADM_REPORT_CFG> ADM_REPORT_CFG_Qry1;

            List<ADM_APP_CONFIG_MST> ADM_APP_CONFIG_MSTListObj = null;
            string splCondition = string.Empty;
            try
            {
                //select * from ADM_APP_CONFIG_MST where ACF_SETTING='CLIENT CODE'
                ADM_APP_CONFIG_MSTListObj = (from cfg in this.currentEntity.ADM_APP_CONFIG_MST
                                             where cfg.ACF_SETTING == "CLIENT CODE"
                                             select cfg).ToList();
                if (ADM_APP_CONFIG_MSTListObj != null && ADM_APP_CONFIG_MSTListObj.Count > 0)
                {
                    splCondition = ADM_APP_CONFIG_MSTListObj[0].ACF_DATA;
                }

                if (utilityObj.User > 0)
                {
                    ADM_REPORT_CFG_Qry = (from rpt in this.currentEntity.ADM_REPORT_CFG
                                          join rptuser in this.currentEntity.ADM_REPORT_USER_MAP on rpt.RPT_PK equals rptuser.RUM_REPORT
                                          where rpt.RPT_ACTIVE == admReportCfgObj.RPT_ACTIVE
                                          && rpt.RPT_GROUP == (admReportCfgObj.RPT_GROUP > 0 ? admReportCfgObj.RPT_GROUP : rpt.RPT_GROUP)
                                          && rpt.RPT_NAME.Contains(utilityObj.FilterValue)
                                          && ((!string.IsNullOrEmpty(splCondition)) ? rpt.RPT_SPL_COND.Contains(splCondition) : true)
                                          && (utilityObj.User > 0 ? rptuser.RUM_USER == utilityObj.User : true)
                                          orderby rpt.RPT_NAME
                                          select rpt);
                }
                else
                {
                    ADM_REPORT_CFG_Qry = (from rpt in this.currentEntity.ADM_REPORT_CFG
                                          where rpt.RPT_ACTIVE == admReportCfgObj.RPT_ACTIVE
                                          && rpt.RPT_GROUP == (admReportCfgObj.RPT_GROUP > 0 ? admReportCfgObj.RPT_GROUP : rpt.RPT_GROUP)
                                          && rpt.RPT_NAME.Contains(utilityObj.FilterValue)
                                          && ((!string.IsNullOrEmpty(splCondition)) ? rpt.RPT_SPL_COND.Contains(splCondition) : true)
                                          orderby rpt.RPT_NAME
                                          select rpt);
                }

                ADM_REPORT_CFG_obj = ADM_REPORT_CFG_Qry.ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);

            }

            return ADM_REPORT_CFG_obj;
        }
        #endregion
    }
}
