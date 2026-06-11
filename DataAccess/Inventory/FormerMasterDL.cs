using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.Inventory
{
    public class FormerMasterDL
    {
        ///// <summary>
        ///// Get Product Properties
        ///// </summary>
        ///// <param name="bizUnit"></param>
        ///// <param name="category"></param>
        ///// <param name="active"></param>
        ///// <returns></returns>
        //public static DataTable GetProductProperties(int bizUnit, int groupType, int groupValue, int active)
        //{
        //    DataSet ds = null;
        //    {
        //        DBService dbService = new DBService();
        //        DBService.Parameters[] colParameters = null;
        //        colParameters = new DBService.Parameters[] 
        //        { 
        //            new DBService.Parameters("P_CON_PK",0),
        //        new DBService.Parameters("P_CON_BIZUNIT", bizUnit),
        //        new DBService.Parameters("P_CGT_VALUE", groupType),
        //        new DBService.Parameters("P_CNG_VALUE", groupValue),
        //        new DBService.Parameters("P_CON_ACTIVE", active)               
        //        };
        //        ds = dbService.DataAdapter(CommandType.StoredProcedure, "SPADM_CONST_MST_GET_KV", colParameters);
        //    }
        //    return ds.Tables[0];

        //}


        /// <summary>
        /// To Get Products by Category
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="bizUnit"></param>
        /// <returns>Products DataTable</returns>
        public static DataTable GetProduct(int categoryPK, int bizUnit, int type, int processCategory, int surface, int grade, int size, int shade)
        {
            DataTable dtProduct;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters("P_PRO_PK", 0),
                new DBService.Parameters("P_PRO_TYPE", categoryPK == 0 ? (object)DBNull.Value:categoryPK),
                new DBService.Parameters("P_PRO_POLYMER", (object)DBNull.Value),
                new DBService.Parameters("P_PRO_ACTIVE", 1),
                new DBService.Parameters("P_BIZUNIT", bizUnit),
                new DBService.Parameters("P_ISD_NATURE", type == 0 ? (object)DBNull.Value:type),
                new DBService.Parameters("P_ISD_PROCESS", processCategory == 0 ? (object)DBNull.Value:processCategory),
                new DBService.Parameters("P_ISD_SURFACE", surface == 0 ? (object)DBNull.Value:surface),
                new DBService.Parameters("P_ISD_GRADE", grade == 0 ? (object)DBNull.Value:grade),
                new DBService.Parameters("P_ISD_SIZE", size == 0 ? (object)DBNull.Value:size),
                new DBService.Parameters("P_ISD_COLOUR", shade == 0 ? (object)DBNull.Value:shade)
            };
            dtProduct = dbService.DataAdapter(CommandType.StoredProcedure, "SPPRD_PRODUCT_MASTER_GET_KV", colParameters).Tables[0];
            return dtProduct;
        }

        /// <summary>
        /// Get Product Properties
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="category"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetProductProperties(int bizUnit, int groupType, int groupValue, int active)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                    new DBService.Parameters("P_CON_PK",0),
                new DBService.Parameters("P_CON_BIZUNIT", bizUnit),
                new DBService.Parameters("P_CGT_VALUE", groupType),
                new DBService.Parameters("P_CNG_VALUE", groupValue),
                new DBService.Parameters("P_CON_ACTIVE", active)               
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, "SPADM_CONST_MST_GET_KV", colParameters);
            }
            return ds.Tables[0];
        }


        /// <summary>
        /// Get Former Mapped Products
        /// </summary>
        /// <param name="formerPk"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetFormerMappedProducts(int formerPk, int bizUnit)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                    new DBService.Parameters("P_LFM_FORMER", formerPk),
                new DBService.Parameters("P_BIZUNIT", bizUnit == 0 ? (object)DBNull.Value:bizUnit) 
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, "SPPRD_FORMER_PROD_MAP_GET_LIST", colParameters);
            }
            return ds.Tables[0];
        }

        /// <summary>
        /// Delete PO Invoice
        /// </summary>
        /// <param name="invPK"></param>       
        /// <returns></returns>
        public static int DeleteFormerMasterDetails(int ItmPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.ITM_PK, ItmPk),              
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SP_DeleteFormerDetails, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

    }
}
