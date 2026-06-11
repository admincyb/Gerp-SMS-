using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Inventory;
using BusinessObject.Inventory;
using ERP.Utilities;

namespace BusinessLogic.Inventory
{
    public class VendorItemDocBL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialCategoryParentPK"></param>
        /// <param name="type"></param>
        /// <param name="sbuPK"></param>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryTypeWithoutSemiAndFinished(int materialCategoryParentPK, int type, int sbuPK)
        {
            return VendorItemDocDL.GetMaterialCategoryTypeWithoutSemiAndFinished(materialCategoryParentPK, type, sbuPK);
        }
        public static DataTable GetItemList(ERP.Utilities.Dashboard.FilterParameters objFilterParam,string VendorPk, string Category, string Item)
        {
            return VendorItemDocDL.GetItemList(objFilterParam,VendorPk, Category, Item);
        }

        public static int SaveVendorDocs(string saveXml)
        {
            return VendorItemDocDL.SaveVendorDocs(saveXml);
        }

        public static VendorItemDocBO GetDocs(string EditPk)
        {
            VendorItemDocBO objVendorItemDocBO = new VendorItemDocBO();
            string xml = VendorItemDocDL.GetDocs(EditPk);
            if (xml != string.Empty)
            {
                objVendorItemDocBO = (VendorItemDocBO)CommonFunctions.DeserializeObject(xml, objVendorItemDocBO);
                return objVendorItemDocBO;
            }
            else
            {
                return null;
            }
        }
    }
}
