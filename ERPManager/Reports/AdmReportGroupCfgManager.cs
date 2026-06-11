using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;

namespace ERPManager
{
    public class AdmReportGroupCfgManager : IAdmReportGroupCfgManager
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
        public AdmReportGroupCfgManager(ERPEntities currentEntity)
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


        public List<ADM_REPORT_GROUP_CFG> GetReportGroupAutoCompleteList(ADM_REPORT_GROUP_CFG admReportGroupCfgObj, ServiceUtility utilityObj)
        {
            List<ADM_REPORT_GROUP_CFG> ADM_REPORT_GROUP_CFG_obj = new List<ADM_REPORT_GROUP_CFG>();

            IQueryable<ADM_REPORT_GROUP_CFG> ADM_REPORT_GROUP_CFG_Qry;
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

                ADM_REPORT_GROUP_CFG_Qry = (from rgc in this.currentEntity.ADM_REPORT_GROUP_CFG
                                            where rgc.RGC_ACTIVE == admReportGroupCfgObj.RGC_ACTIVE
                                            && rgc.RGC_PK == (admReportGroupCfgObj.RGC_PK > 0 ? admReportGroupCfgObj.RGC_PK : rgc.RGC_PK)
                                            && rgc.RGC_NAME.StartsWith(utilityObj.FilterValue)
                                            && ((!string.IsNullOrEmpty(splCondition)) ? rgc.RGC_SPL_COND.Contains(splCondition) : true)
                                            orderby rgc.RGC_NAME
                                            select rgc);
                ADM_REPORT_GROUP_CFG_obj = ADM_REPORT_GROUP_CFG_Qry.ToList();
            }
            catch (Exception ex)
            {
                //Throws a new exception to service class with class name - method name - server side exception process result as exception message
                throw ERP.Utilities.CommonFunctions.ProcessServerException(this.GetType().Name, new StackFrame(1, true).GetMethod().Name, ex);

            }
            return ADM_REPORT_GROUP_CFG_obj;
        }
        #endregion
    }
}
