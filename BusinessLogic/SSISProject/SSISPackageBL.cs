using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic.SSISProject
{
   public class SSISPackageBL
    {
        /// <summary>
        /// To Retrive Package Import Error Details
        /// </summary>      
        /// <returns>DataTable</returns>
        public static DataTable GetErrors()
        {
            return DataAccess.SSISProject.SSISPackageDL.GetErrors();
        }
    }
}
