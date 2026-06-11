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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ChartOfAccountService" in code, svc and config file together.
    public class ChartOfAccountService : IChartOfAccountService
    {
        #region Private Variables
        ERPEntities currentContext;
        #endregion
        #region Service Methods
        /// <summary>
        /// 
        /// </summary>
        public ChartOfAccountService()
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
        /// <summary>
        /// Gets list of COA Master 
        /// </summary>
        /// <param name="FinCoaMstObj"></param>
        /// <param name="utilityObj"></param>
        /// <returns>List of Bank Master</returns>
        public List<FIN_COA_MST> GetFinCoaMst(FIN_COA_MST FinCoaMstObj, ServiceUtility utilityObj)
        {
            throw new NotImplementedException();
        }
         #endregion
    }
}
