using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.Inventory
{
    public class ProductsDA
    {
        /// <summary>
        /// Methode used for get the Property Grade XML
        /// </summary>
        /// <param name="groupType"></param>
        /// <param name="productPK"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetPropertyGradeXML(int groupType, int productPK)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.PRODUCTPROPPK,productPK),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.GROUPTYPEPK ,groupType),
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SP_GETPROPERTYANDGRADE, colParameters).Tables[0];
            return dtList;

        }
        /// <summary>
        /// Get Item Category
        /// </summary>
        /// <param name="itemCode"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetItemCategory(string itemCode, int active, int value = 0)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.ITC_CODE,string.IsNullOrEmpty(itemCode)?(object)DBNull.Value:itemCode),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.ITC_ACTIVE ,active),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.ITC_PK,0),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ITC_VALUE, value == 0 ? (object)DBNull.Value : value)
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SP_CATEGORY_GET, colParameters).Tables[0];
            return dtList;
        }

        
        /// <summary>
        /// Save Store Mapping
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveStoreMap(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPINV_PROD_DEPT_MAP_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Save Product Group
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveProductGroup(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SP_ProductGroupSave, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// To Get Product Categories
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="categoryPK" value="0 for all"></param>
        /// <returns>Category DataTable</returns>
        public static DataTable GetCategory(int bizUnit, int categoryPK)
        {
            DataTable dtCategory;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_CATEGORY, categoryPK==0?(object)DBNull.Value:categoryPK)
            };
            dtCategory = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SP_GetCategory, colParameters).Tables[0];
            return dtCategory;
        }
        /// <summary>
        /// To Fill Product Plan Groups
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="Active"></param>
        /// <param name="Bizunit"></param>
        /// <returns></returns>
        public static DataTable GetProductPlanGroups(int PlanGroupPK, int Active, int Bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PIG_PK, PlanGroupPK),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ACTIVE, Active),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_BIZUNIT, Bizunit)
            };
            DataTable dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SP_PRD_PLAN_ITEM_GROUP_MST_GET_KV, colParameters).Tables[0];
            return dtGroups;
        }

        #region Planning Groups master
        /// <summary>
        /// Save Product Group
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SavePlanningGroup(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SP_PlanningGroupSave, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// To Fill Product Plan Groups
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="Active"></param>
        /// <param name="Bizunit"></param>
        /// <returns></returns>
        public static DataTable GetProductItems(int CurrPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                 new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PIG_PK, CurrPK),
                //new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ACTIVE, Active),
                //new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_BIZUNIT, Bizunit)
            };
            DataTable dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPPRD_PLAN_ITEM_GROUP_MST_PRODUCT_GET, colParameters).Tables[0];
            return dtGroups;
        }

        /// <summary>
        /// To Fill Product Plan Groups
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="Active"></param>
        /// <param name="Bizunit"></param>
        /// <returns></returns>
        public static DataTable GetPlanningItemGroupList(string pageNum, int pageSize, int CurrPK, string PlanningGroupCode, string PlanningGroupName)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PAGE_NUM , pageNum),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PAGE_SIZE,  pageSize),
                //new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_BIZUNIT, bizUnit),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PIG_PK,CurrPK == 0 ? (object)DBNull.Value : CurrPK),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PIG_CODE,PlanningGroupCode == string.Empty ? (object)DBNull.Value : PlanningGroupCode), 
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PIG_NAME,PlanningGroupName == string.Empty ? (object)DBNull.Value : PlanningGroupName),
                //new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ACTIVE,name == string.Empty ? (object)DBNull.Value : name)
               
            };
            DataTable dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPPRD_PLAN_ITEM_GROUP_MST_GET_LIST, colParameters).Tables[0];
            return dtGroups;
        }

        /// <summary>
        /// To Fill Product Plan Groups
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="Active"></param>
        /// <param name="Bizunit"></param>
        /// <returns></returns>
        public static DataSet GetPlanningItemEdit(int CurrPK)
        {
            //DBService dbService = new DBService();
            //DBService.Parameters[] colParameters = null;
            //colParameters = new DBService.Parameters[] 
            //{
            //    new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PIG_PK,CurrPK),
            //    new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ACTIVE,2)

            //};
            //DataTable dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPPRD_PLAN_ITEM_GROUP_MST_GET_KV, colParameters).Tables[0];
            //return dtGroups;

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {           
               new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PIG_PK,CurrPK)
              
            };
            DataSet dtGroups = new DataSet();
            dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPPRD_PLAN_ITEM_GROUP_MST_GET, colParameters);
            return dtGroups;
        }

        /// <summary>
        /// Delete Product Group
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int DeleteInvItemPlanningMst(int CurrPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PIG_PK, CurrPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SP_PlanningGroupDelete, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }


        /// <summary>
        /// To Fill Product Plan Groups Autocomplete
        /// </summary>
        /// <param name="PlanPK"></param>
        /// <param name="Active"></param>
        /// <param name="Bizunit"></param>
        /// <returns></returns>
        public static DataTable GetPlanningGroupAutocomplete(string searchKey, string SearchBy)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_SEARCH_VAL, searchKey),
                 new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_TYPE, SearchBy)
                //new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ACTIVE, Active),
                //new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_BIZUNIT, Bizunit)
            };
            DataTable dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPPRD_PLAN_ITEM_GROUP_MST_AUTO, colParameters).Tables[0];
            return dtGroups;
        }

        #endregion

        /// <summary>
        /// Methode used for get the mapped lines against product
        /// </summary>
        /// <param name="groupType"></param>
        /// <param name="productPK"></param>
        /// <returns>DataTable</returns>
        public static string GetProductLines(int productPK)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            string strXml = string.Empty;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PRO_PK,productPK) 
            };

            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPADM_PRODUCT_LINE_GET_XML, colParameters).Tables[0];
            if (dtList != null && dtList.Rows.Count > 0)
            {
                foreach (DataRow drPlan in dtList.Rows)//addding each row to string
                {
                    strXml = strXml + Convert.ToString(drPlan[0]);
                }
            }
            return strXml;
        }

        public static int SaveLineItemMap(string pXml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_XML, pXml),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPPRD_PRODUCT_LINE_MAP_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// To get uploaded file for the group of product
        /// </summary>
        /// <param name="Product"></param>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static DataTable GetProductGroupDoc(int Product, int Task)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ITM_PK, Product),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_DOC_TASK, Task)
            };
            DataTable dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPADM_DOC_ATTACH_PRODUCT_GET, colParameters).Tables[0];
            return dtGroups;
        }
        /// <summary>
        /// To get GST class list
        /// </summary>
        /// <param name="Product"></param>
        /// <param name="Task"></param>
        /// <returns></returns>
        public static DataTable GetGSTclassificationList(int PK, int active, int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_GCM_PK, PK>0 ? PK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_BIZUNIT, bizunit)
            };
            DataTable dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPFIN_GST_CLASS_MST_GET_KV, colParameters).Tables[0];
            return dtGroups;
        }


        public static int BrandProductSave(int ItemPK,int UserPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_PRO_PK, ItemPK),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_USER_PK, UserPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SPPRD_BRAND_PRODUCT_COPY_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
