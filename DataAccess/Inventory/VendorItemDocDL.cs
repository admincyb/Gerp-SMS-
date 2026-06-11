using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.Inventory
{
    public class VendorItemDocDL
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
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.MATERIALCATEGORYPARENTPK  , materialCategoryParentPK > 0 ? materialCategoryParentPK : (object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.ITC_VALUE  , type > 0 ? type : (object)DBNull.Value), 
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_BIZUNIT  , sbuPK) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.GETMATERIALCATEGORYWITHOUTSEMIANDFINISHED, colParameters).Tables[0];
        }
        public static DataTable GetItemList(ERP.Utilities.Dashboard.FilterParameters objFilterParam, string VendorPk, string Category, string Item)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_VEN_PK  , string.IsNullOrEmpty(VendorPk) ? (object)DBNull.Value : VendorPk),
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_ITM_CATEGORY  , string.IsNullOrEmpty(Category) ? (object)DBNull.Value : Category), 
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_ITM_PK  , string.IsNullOrEmpty(Item) ? (object)DBNull.Value : Item) ,
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_PAGE_NO  , objFilterParam.PageNumber), 
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_PAGE_SIZE  ,  objFilterParam.PageSize) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPPUR_VENDOR_ITEM_MAP_GET_LIST, colParameters).Tables[0];
        }

        public static int SaveVendorDocs(string saveXml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, saveXml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPINV_ITEM_VENDOR_MAP_DOC_DTL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static string GetDocs(string EditPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Inventory.Parameters.P_ITV_PK  , EditPk), 
            }; 
            string strRetVal = string.Empty;
            DataTable dtxml =dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPINV_ITEM_VENDOR_MAP_DOC_DTL_GET_XML, colParameters).Tables[0];
            
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
    }
}
