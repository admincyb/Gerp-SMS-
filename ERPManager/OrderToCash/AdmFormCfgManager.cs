using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Diagnostics;
using System.Data;

namespace ERPManager
{
    public class AdmFormCfgManager : IAdmFormCfgManager
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
        public AdmFormCfgManager(ERPEntities currentEntity)
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


       


        #endregion
    }
}
