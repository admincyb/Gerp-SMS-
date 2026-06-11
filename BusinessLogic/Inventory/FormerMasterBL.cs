using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.MaterialManagement;
using GTIService;

namespace BusinessLogic.Inventory
{
   public class FormerMasterBL
    {
        ///// <summary>
        ///// Get Product Properties
        ///// </summary>
        ///// <param name="bizUnit"></param>
        ///// <param name="category"></param>
        ///// <param name="active"></param>
        ///// <returns></returns>
        //public static DataTable GetProductProperties(int bizUnit, int groupType, int groupValue)
        //{
        //    int active = 1;
        //    return DataAccess.Inventory.FormerMasterDL.GetProductProperties(bizUnit, groupType, groupValue, active);
        //}

        /// <summary>
        /// To Get Products by Category
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <returns>Products DataTable</returns>
        public static DataTable GetProduct(int categoryPK, int bizUnit, int type, int processCategory, int surface, int grade, int size,int shade)
        {
            return DataAccess.Inventory.FormerMasterDL.GetProduct(categoryPK, bizUnit, type, processCategory, surface, grade, size, shade);
        }

        /// <summary>
        /// Get Product Properties
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="category"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetProductProperties(int bizUnit, int groupType, int groupValue)
        {
            int active = 1;
            return DataAccess.Inventory.FormerMasterDL.GetProductProperties(bizUnit, groupType, groupValue, active);
        }

        /// <summary>
        /// Get Former Mapped Products
        /// </summary>
        /// <param name="formerPk"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetFormerMappedProducts(int formerPk, int bizUnit)
        {
            return DataAccess.Inventory.FormerMasterDL.GetFormerMappedProducts(formerPk, bizUnit);
        }

        /// <summary>
        /// Delete Former master
        /// </summary>
        /// <param name="PK"></param>        
        /// <returns></returns>
        public static int DeleteFormerMasterDetails(int ItmPK)
        {
            return DataAccess.Inventory.FormerMasterDL.DeleteFormerMasterDetails(ItmPK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemPK"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetMaterialStores(int itemPK, int bizUnit)
        {
            DataTable dtMaterial = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialStores(itemPK, bizUnit);
            return dtMaterial;
        }

        public static int SaveMaterialVendorDetails(string strxml)
        {
            return DataAccess.MaterialManagement.MaterialMasterDL.SaveMaterialVendorDetails(strxml);
        }

        public static VenMappingHeader GetVendorMappingDetails(int itmPK)
        {
            try
            {
                VenMappingHeader VenObj = new VenMappingHeader();
                string VenList = DataAccess.MaterialManagement.MaterialMasterDL.GetVendorMappingDetails(itmPK);
                if (VenList != string.Empty)
                {
                    VenObj = (VenMappingHeader)CommonFunctions.DeserializeObject(VenList, VenObj);
                    return VenObj;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

    }
}
