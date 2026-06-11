using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTIService;
using System.Data;
using System.IO;
using BusinessObject;

namespace DataAccess.MaterialManagement
{
    /// <summary>
    /// Class Used for Access all meterail category releted function
    /// </summary>
    public class MaterialCategoryMasterDL
    {
        /// <summary>
        /// Function Used To Save Material Category 
        /// </summary>
        /// <param name="materialCategory"></param>
        /// <returns></returns>
        public static string SaveMaterialCategory(BusinessObject.MaterialManagement.MaterialCategory materialCategory, string AccountList = null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALCATEGORYPK  , materialCategory.MaterialCategoryPK == 0 ? (object)DBNull.Value: materialCategory.MaterialCategoryPK),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.PURACCOUNT  , materialCategory.PurAccount == "0" ? (object)DBNull.Value: materialCategory.PurAccount),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.POITEMTYPE  , materialCategory.PoCategory == "0" ? (object)DBNull.Value: materialCategory.PoCategory),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.INVACCOUNT  , materialCategory.InvAccount == "0" ? (object)DBNull.Value: materialCategory.InvAccount),
                                new DBService.Parameters( GTIService.Constants.Material.Parameters.SALEACCOUNT  , materialCategory.SaleAccount == "0" ? (object)DBNull.Value: materialCategory.SaleAccount),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.CONACCOUNT  , materialCategory.ConsumptionAccount == "0" ? (object)DBNull.Value: materialCategory.ConsumptionAccount),

                new DBService.Parameters( GTIService.Constants.Material.Parameters.CATEGORYCODE , materialCategory.CategoryCode),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.CATEGORYNAME , materialCategory.CategoryName),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.INACTIVEPERIOD , materialCategory.InactivePeriod),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.CATBIZUNIT , materialCategory.SBU),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.CATEGORYDESC , materialCategory.CategoryDesc),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALCATEGORYPARENTPK , materialCategory.MaterialCategoryParentPK),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.UOMTYPE , 7),//BugID:- 6564 Discussed with Manoj Sir always Pass NA as UOM TYPE 
                new DBService.Parameters( GTIService.Constants.Material.Parameters.CATEGORYTYPE , materialCategory.CategoryType),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ACTIVE , 1),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.CREATEDBY , materialCategory.UserPk),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MODBY , materialCategory.UserPk),
                //new DBService.Parameters( GTIService.Constants.Material.Parameters.NEEDQCINSP , string.IsNullOrEmpty(materialCategory.RequireInspection)==true?0:1),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITC_IS_STOCK , string.IsNullOrEmpty(materialCategory.Stock)==true?0:1),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITC_IS_DIR_GRN ,materialCategory.GrnType),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITC_RPT_CATEGORY ,materialCategory.ReportCategory == "0" ? (object)DBNull.Value: materialCategory.ReportCategory),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITC_IS_SALE , string.IsNullOrEmpty(materialCategory.ITC_IS_SALE)==true?0:1),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITC_IS_VCH_POST , string.IsNullOrEmpty(materialCategory.ITC_IS_VCH_POST)==true?0:1),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_XML , !string.IsNullOrEmpty(AccountList)? AccountList : (object)DBNull.Value),
                
                new DBService.Parameters( GTIService.Constants.Common.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SAVEMATERIALCATEGORY, colParameters);
            return ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value.ToString();
        }
        /// <summary>
        /// Function Used To Get all Material Category 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryDtls(int materialCategoryParentPK, int sbuPK,int itemCategoryValue=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALCATEGORYPARENTPK  , materialCategoryParentPK), 
                new DBService.Parameters( GTIService.Constants.Material.Parameters.CATEGORYBIZ  , sbuPK) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITC_VALUE  , itemCategoryValue == 0 ? (object)DBNull.Value: itemCategoryValue) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORY, colParameters).Tables[0];
        }
        /// <summary>
        /// Function Used To Get all Material Category except fg fro tree
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryDtlsWithoutFG(int materialCategoryParentPK, int sbuPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALCATEGORYPARENTPK  , materialCategoryParentPK), 
                new DBService.Parameters( GTIService.Constants.Material.Parameters.CATEGORYBIZ  , sbuPK) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYEXCEPTFG, colParameters).Tables[0];
        }
        /// <summary>
        /// Function Used To Get all Material Category except fg fro tree
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryDtlsWithoutFG(int materialCategoryParentPK, int sbuPK, int itemType, int store)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALCATEGORYPARENTPK  ,materialCategoryParentPK == 0 ? (object)DBNull.Value: materialCategoryParentPK ), 
                new DBService.Parameters( GTIService.Constants.Material.Parameters.CATEGORYBIZ  , sbuPK) ,
                 new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALRETURNTYPE  , itemType) ,
                 new DBService.Parameters( GTIService.Constants.Material.Parameters.STORE  , store) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYEXCEPTFG, colParameters).Tables[0];
        }
        /// <summary>
        /// Function Used To Get all Material Category 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryWithoutSemiAndFinished(int materialCategoryParentPK, int sbuPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALCATEGORYPARENTPK  , materialCategoryParentPK), 
                new DBService.Parameters( GTIService.Constants.Material.Parameters.CATEGORYBIZ  , sbuPK) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYWITHOUTSEMIANDFINISHED, colParameters).Tables[0];
        }
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
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALCATEGORYPARENTPK  , materialCategoryParentPK),
                 new DBService.Parameters( GTIService.Constants.Material.Parameters.ITC_VALUE  , type), 
                new DBService.Parameters( GTIService.Constants.Material.Parameters.CATEGORYBIZ  , sbuPK) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYWITHOUTSEMIANDFINISHED, colParameters).Tables[0];
        }

        /// <summary>
        /// Function Used To Get all Material Category 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryList(int sbuPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk), 
               
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORY, colParameters).Tables[0];
        }
        /// <summary>
        /// Function Used To Get all Material Category except fg for dropdown
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryListExceptFG(int sbuPk, int itemType, int store, int showSFG, string srchValue = "%")
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk), 
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALRETURNTYPE  , itemType) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.STORE  , store) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_SHOW_SFG  , showSFG),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITC_NAME  ,string.IsNullOrEmpty(srchValue) ? (object)DBNull.Value : srchValue) 
               
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYEXCEPTFG, colParameters).Tables[0];
        }

        public static DataTable GetMaterialCategoryAutoList(int sbuPk, int itemType, int store, string searchValue)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk), 
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALRETURNTYPE  , itemType) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.STORE  , store),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITC_NAME  , searchValue) 
               
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYEXCEPTFGAUTO, colParameters).Tables[0];
        }
        /// <summary>
        /// Function Used To Get all Material Category 
        /// Show materials based on Qty available (not based on Active)
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryStkAutoList(int sbuPk, int itemType, int store, string searchValue,int stock=1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk), 
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALRETURNTYPE  , itemType) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.STORE  , store),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITC_NAME  , searchValue) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_HAS_STOCK ,stock) ,
               
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYSTKEXCEPTFGAUTO, colParameters).Tables[0];
        }
        /// <summary>
        /// Function Used To Get all Material Category 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryListWithoutSemiAndFinished(int sbuPk, int ITCVAL=0,int includeFG=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk),
                new DBService.Parameters("P_INCLUDE_FG" , includeFG>0 ? includeFG: (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.ITC_VALUE,ITCVAL > 0 ? ITCVAL : (object)DBNull.Value)
               
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYWITHOUTSEMIANDFINISHED, colParameters).Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbuPk"></param>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryTypeListWithoutSemiAndFinished(int sbuPk, int type)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk), 
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.ITC_VALUE  , type), 
               
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYWITHOUTSEMIANDFINISHED, colParameters).Tables[0];
        }
        /// <summary>
        /// Function Used To Get all Material Category 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryRawMaterialList(int sbuPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk), 
               
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYRAWMATERIAL, colParameters).Tables[0];
        }


        /// <summary>
        /// Function Used To Get all Material Category 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryListAuto(int sbuPk, string srhcType,int type=0,int PM_WorkOrder=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.SERACHVALUE, srhcType),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_ITC_VALUE, type>0?type:(object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_WORK_ORDER, PM_WorkOrder>0?PM_WorkOrder:(object)DBNull.Value),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYAUTO, colParameters).Tables[0];
        }

        /// <summary>
        /// Function Used To Get all Material Category 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryListExcepetFGAuto(int sbuPk, string srhcType)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.SERACHVALUE, srhcType),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.FLAG, 0),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYAUTO, colParameters).Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptPK"></param>
        /// <param name="srhcType"></param>
        /// <returns></returns>
        public static DataTable GetMaterialCategoryListDeptAuto(int sbuPk,int deptPK, string srchValue, int IsStock=-1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                 new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_DEPT  , deptPK),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_ITC_IS_STOCK , IsStock>0 ? IsStock : (object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITC_NAME, srchValue),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_ITEM_CATEGORY_DEPT_AUTO, colParameters).Tables[0];
        }
        public static DataTable GetPlantToPlantMaterialCategoryAuto(int sbuPk,int deptPK, string srchValue, int IsStock=-1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                 new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_DEPT  , deptPK),
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.P_ITC_IS_STOCK , IsStock>0 ? IsStock : (object)DBNull.Value),
                new DBService.Parameters( GTIService.Constants.Material.Parameters.ITC_NAME, srchValue),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_ITEM_CATEGORY_DEPT_MR_AUTO, colParameters).Tables[0];
        }


        /// <summary>
        /// Function Used To Get UOM Name By Category 
        /// Function refered sajeer,
        /// </summary>
        /// <returns></returns>
        public static DataTable GetUOMByCategory(int materialCategoryPK, int sbuPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALCATEGORYPK  , (materialCategoryPK == 0 || materialCategoryPK == -1) ? (object)DBNull.Value: materialCategoryPK),  
                 new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT  , sbuPk)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETUOMTYPECATEGORY, colParameters).Tables[0];
        }

        /// <summary>
        /// Function Used To Get Details Material Category By Catogory ID
        /// </summary>
        /// <param name="materialCategoryPK"></param>
        /// <returns></returns>
        public static BusinessObject.MaterialManagement.MaterialCategory GetMaterialCategory(int materialCategoryPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALCATEGORYPK  , materialCategoryPK == 0 ? (object)DBNull.Value: materialCategoryPK),  
            };
            System.Data.Common.DbDataReader reader = dbService.ExecuteReader(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORY, colParameters);

            BusinessObject.MaterialManagement.MaterialCategory materialCategory = null;
            if (reader != null && reader.Read())
            {
                materialCategory = new BusinessObject.MaterialManagement.MaterialCategory();
                materialCategory.MaterialCategoryPK = reader[GTIService.Constants.Material.Fields.CATEGORYPK] == null ? 0 : Convert.ToInt32(reader[GTIService.Constants.Material.Fields.CATEGORYPK]);
                materialCategory.PoCategory = reader[GTIService.Constants.Material.Fields.PO_ITEM_TYPE] == null ? "0" : reader[GTIService.Constants.Material.Fields.PO_ITEM_TYPE].ToString();
                materialCategory.CategoryCode = reader[GTIService.Constants.Material.Fields.CATEGORYCODE] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.CATEGORYCODE]);
                materialCategory.CategoryName = reader[GTIService.Constants.Material.Fields.CATEGORYNAME] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.CATEGORYNAME]);
                materialCategory.SBU = reader[GTIService.Constants.Material.Fields.CATEGORYBIZ] == null ? 0 : Convert.ToInt32(reader[GTIService.Constants.Material.Fields.CATEGORYBIZ]);
                materialCategory.UOMType = reader[GTIService.Constants.Material.Fields.UOMTYPE] == null ? 0 : Convert.ToInt32(reader[GTIService.Constants.Material.Fields.UOMTYPE]);
                materialCategory.CategoryType = reader[GTIService.Constants.Material.Fields.CATEGORYTYPE] == null ? 0 : Convert.ToInt32(reader[GTIService.Constants.Material.Fields.CATEGORYTYPE]);
                materialCategory.MaterialCategoryParentPK = reader[GTIService.Constants.Material.Fields.CATEGORYPARENTPK] == null ? 0 : Convert.ToInt32(reader[GTIService.Constants.Material.Fields.CATEGORYPARENTPK]);
                materialCategory.CategoryDesc = reader[GTIService.Constants.Material.Fields.CATEGORYDESC] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.CATEGORYDESC]);
                materialCategory.InactivePeriod = reader[GTIService.Constants.Material.Fields.INACTIVEPERIOD] == (object)DBNull.Value ? 0 : Convert.ToInt32(reader[GTIService.Constants.Material.Fields.INACTIVEPERIOD]);
                materialCategory.MaterialCategoryParentName = reader[GTIService.Constants.Material.Fields.CATEGORYPARENTNAME] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.CATEGORYPARENTNAME]);
                materialCategory.HasChild = reader[GTIService.Constants.Material.Fields.HASCHILD] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.HASCHILD]);
                materialCategory.IsUsing = reader[GTIService.Constants.Material.Fields.ISUSING] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.ISUSING]);
                materialCategory.IsDefault = reader[GTIService.Constants.Material.Fields.ISDEFAULT] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.ISDEFAULT]);
                materialCategory.PurAccount = reader[GTIService.Constants.Material.Fields.PURACCOUNT] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.PURACCOUNT]);

                materialCategory.SaleAccount = reader[GTIService.Constants.Material.Fields.SALEACCOUNT] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.SALEACCOUNT]);

                materialCategory.InvAccount = reader[GTIService.Constants.Material.Fields.INVACCOUNT] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.INVACCOUNT]);
                materialCategory.ConsumptionAccount = reader[GTIService.Constants.Material.Fields.CONACCOUNT] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.CONACCOUNT]);
                // materialCategory.RequireInspection = reader[GTIService.Constants.Material.Fields.NEEDQCINSP] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.NEEDQCINSP]);
                materialCategory.Stock = reader[GTIService.Constants.Material.Fields.ITC_IS_STOCK] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.ITC_IS_STOCK]);
                materialCategory.GrnType = Convert.ToInt16(reader[GTIService.Constants.Material.Fields.ITC_IS_DIR_GRN] == null ? "0" : Convert.ToString(reader[GTIService.Constants.Material.Fields.ITC_IS_DIR_GRN]));
                materialCategory.ReportCategory = reader[GTIService.Constants.Material.Fields.ITC_RPT_CATEGORY] == null ?  string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.ITC_RPT_CATEGORY]);
                materialCategory.ITC_IS_SALE = reader[GTIService.Constants.Material.Fields.ITC_IS_SALE] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.ITC_IS_SALE]);
                materialCategory.ITC_IS_VCH_POST = reader[GTIService.Constants.Material.Fields.ITC_IS_VCH_POST] == null ? string.Empty : Convert.ToString(reader[GTIService.Constants.Material.Fields.ITC_IS_VCH_POST]);
            }
            return materialCategory;
        }
        /// <summary>
        /// Method Used to Delete Material Category Detials
        /// </summary>
        /// <param name="ctgID"></param>
        /// <returns></returns>
        public static int DeleteMaterialCategory(int ctgID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Material.Parameters.CATEGORYPK,  ctgID == 0 ? (object)DBNull.Value :  ctgID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.DELETEMATERIALCATEGORY, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);
        }

        public static DataTable GetMaterialCategoryByType(int materialCategoryParentPK, int sbuPK, int itemType, int store,int showSFG)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALCATEGORYPARENTPK  , materialCategoryParentPK>0?materialCategoryParentPK: (object)DBNull.Value), 
                new DBService.Parameters( GTIService.Constants.Material.Parameters.CATEGORYBIZ  , sbuPK) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.MATERIALRETURNTYPE  , itemType) ,
                new DBService.Parameters( GTIService.Constants.Material.Parameters.STORE  , store), 
                new DBService.Parameters( GTIService.Constants.Material.Parameters.P_SHOW_SFG  , showSFG) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.GETMATERIALCATEGORYEXCEPTFG, colParameters).Tables[0];
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="PrdCategoryPK"></param>
       /// <param name="Active"></param>
       /// <param name="itcValue">2 for products, 9 for Brand products</param>
       /// <returns></returns>
        public static DataTable GetProductCategory(int PrdCategoryPK, int Active, int bizunit, int itcValue=2)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.ITC_PK, PrdCategoryPK),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.ITC_ACTIVE, Active),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ITC_VALUE, itcValue),
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_BIZUNIT, bizunit)
            };
            DataTable dtGroups = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Inventory.Procedures.SP_CATEGORY_GET, colParameters).Tables[0];
            return dtGroups;
        }

        /// <summary>
        /// To get accounts mapped to item category
        /// </summary>
        /// <param name="ITCPK"></param>
        /// <returns></returns>
        public static DataSet GetMaterialCategoryAccounts(int ITCPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Inventory.Parameters.P_ITC_PK, ITCPK)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Material.Procedures.SPINV_ITEM_CATEGORY_COA_MAP_GET, colParameters);
        }
    }
}
