using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.Administration.Configurations
{
    public class MarginSetupBL
    {
        /// <summary>
        /// To save Margin Setup
        /// </summary>
        /// <param name="objCompany"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static int SaveMarginSetup(string pXml)
        {
            try
            {
                return DataAccess.Administration.Configurations.MarginSetupDA.SaveMarginSetup(pXml);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Margin Setup
        /// </summary>
        /// <param name="ratePk"></param>
        /// <param name="active"></param>
        /// <returns>DataSet</returns>     
        public static DataSet GetMarginSetup(GridPrams pageParams )
        {
            try
            {
                DataSet dsMarginSetup = DataAccess.Administration.Configurations.MarginSetupDA.GetMarginSetup(pageParams);
                return dsMarginSetup;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get Margin Setup by PK
        /// </summary>
        /// <param name="ratePk"></param>
        /// <param name="active"></param>
        /// <returns>DataTable</returns>     
        public static DataTable GetMarginSetupByPK(int marginSetupPK)
        {
            try
            {
                DataTable dtMarginSetup = DataAccess.Administration.Configurations.MarginSetupDA.GetMarginSetupByPK(marginSetupPK);
                return dtMarginSetup;
            }
            catch
            {
                throw;
            }
        }

        public static int DeleteMarginSetup(int marginSetupPK, DateTime? lastModDate)
        {
            try
            {
                int val = DataAccess.Administration.Configurations.MarginSetupDA.DeleteMarginSetup(marginSetupPK, lastModDate);
                return val;
            }
            catch
            {
                throw;
            }
        }
    }
}
