using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.StoreManagement
{
    public class StockDetailsReport
    {
        /// <summary>
        /// get Stock Details
        /// </summary>
        /// <param name="date"></param>
        /// <param name="store"></param>
        /// <param name="catg"></param>
        /// <param name="item"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetStockDetails(string date, int store, int catg,int item , int sbu)
        {
            return DataAccess.StoreManagement.StockDetailsReportDL.GetStockDetails(date, store, catg, item, sbu);
        }
        /// <summary>
        /// Get Material Name by material Category
        /// </summary>
        /// <param name="catg"></param>
        /// <param name="itemPK"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetMaterialDtls(int catg, int itemPK, int sbu)
        {
            return DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialByCategory(catg, itemPK, sbu);
        }

        /// <summary>
        /// Get all Store Name
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetStoreDtls(User objUser)
        {
            return DataAccess.StoreManagement.StockDetailsReportDL.GetStoreNames(objUser.SBUID);
        }
/// <summary>
        /// Get all Store Name
        /// </summary>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetAllStoreDtls(User objUser, int category)
        {
            return DataAccess.StoreManagement.StockDetailsReportDL.GetAllStoreNames(objUser, category);
        }

        public static DataTable GetStockDetailsForRpt(string date, int store, string dateTo, int Category, int sbu)
        {
            return DataAccess.StoreManagement.StockDetailsReportDL.GetStockDetailsForRpt(date, store, dateTo,Category,sbu);
        }
    }
}
