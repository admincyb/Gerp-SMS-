using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.SSISProject
{
    public class SSISPackageDL
    {
        /// <summary>
        /// To Retrive Package Import Error Details
        /// </summary>      
        /// <returns>DataTable</returns>
        public static DataTable GetErrors()
        {
            DBService dbService = new DBService();
            DataSet ds = dbService.DataAdapter(CommandType.StoredProcedure, "SPBRAND_ERROR_OUTPUT");
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return new DataTable();
        }
    }
}
