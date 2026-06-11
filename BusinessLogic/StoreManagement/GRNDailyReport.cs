using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.StoreManagement
{
    public class GRNDailyReport
    {
        #region Methods

        /// <summary>
        /// Get GRN daily Stock report
        /// </summary>
        /// <param name="store"></param>
        /// <param name="date"></param>
        /// <param name="catg"></param>
        /// <returns></returns>
        public static DataTable GetGRNDailyDtls(int store, string date, int catg, int sbu)
        {
            return DataAccess.StoreManagement.GRNDailyReport.GetGRNDailyDtls( store,  date,  catg, sbu);
        }
        /// <summary>
        /// Get Store Name
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetStoreDtls(User objUser)
        {
            return DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.GetStores(objUser, objUser.SBUID, 1);
        }
        /// <summary>
        /// Get Category Details
        /// </summary>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetCategoryDtls(int sbu)
        {
            return DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryList(sbu);
        }

        #endregion
    }
}
