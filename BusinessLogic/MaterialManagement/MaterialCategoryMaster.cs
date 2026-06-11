using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.CommonManagement;
using System.Web.Script.Serialization;
using System.Web;
using System.Xml;
using Newtonsoft.Json;
using BusinessObject.MaterialManagement;

namespace BusinessLogic.MaterialManagement
{
    /// <summary>
    /// Class Used for Access all meterail category releted function
    /// </summary>
    public class MaterialCategoryMaster
    {
        /// <summary>
        ///  Function Used To Save Material Category 
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static string SaveMaterialCategory(string requestData)
        {
            BusinessObject.MaterialManagement.MaterialCategory materialCategory = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.MaterialManagement.MaterialCategory>(requestData);
            MaterialCategorySaveAccounts objAccount = new MaterialCategorySaveAccounts();
            objAccount.MaterialAccountlst = new List<CategoryDetails>();
            foreach (MaterialCategoryAccounts lst in materialCategory.MaterialAccountlst)
            {
                objAccount.MaterialAccountlst.Add(new CategoryDetails { ICC_ACCOUNT = lst.ICC_ACCOUNT, ICC_PK = lst.ICC_PK, ICC_ACCOUNT_TEXT = lst.ICC_ACCOUNT_TEXT, ICC_ACCOUNT_TYPE = lst.ICC_ACCOUNT_TYPE, ICC_ACCOUNT_TYPE_TEXT = lst.ICC_ACCOUNT_TYPE_TEXT, ICC_COMPANY = lst.ICC_COMPANY, ICC_COMPANY_TEXT = lst.ICC_COMPANY_TEXT });
            }
            string xmlstr = ERP.Utilities.CommonFunctions.XmlSerialize<MaterialCategorySaveAccounts>(objAccount);
            return DataAccess.MaterialManagement.MaterialCategoryMasterDL.SaveMaterialCategory(materialCategory, xmlstr);
        }

        /// <summary>
        /// Function Used To Get all Material Category 
        /// </summary>
        /// <returns></returns>
        public static string GetMaterialCategoryDtls(int materialCategoryParentPK,int sbuPK,int itemCategoryValue=0)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryDtls(materialCategoryParentPK, sbuPK, itemCategoryValue);
            string jString = string.Empty;
            if (dtCategory.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTreeList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYPK, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPARENTPK, GTIService.Constants.Material.Fields.HASCHILD, string.Empty, string.Empty, string.Empty); 
            }
            return jString;
        }
        /// <summary>
        /// Function Used To Get all Material Category except fg for tree
        /// </summary>
        /// <returns></returns>
        public static string GetMaterialCategoryDtlsWithoutFG(int materialCategoryParentPK, int sbuPK)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryDtlsWithoutFG(materialCategoryParentPK, sbuPK);
            string jString = string.Empty;
            if (dtCategory.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTreeList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYPK, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPARENTPK, GTIService.Constants.Material.Fields.HASCHILD, string.Empty, string.Empty, string.Empty);
            }
            return jString;
        }
        /// <summary>
        /// Function Used To Get all Material Category except fg for tree
        /// </summary>
        /// <returns></returns>
        public static string GetMaterialCategoryDtlsWithoutFG(int materialCategoryParentPK, int sbuPK ,int itemType,int store)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryDtlsWithoutFG(materialCategoryParentPK, sbuPK, itemType, store);
            string jString = string.Empty;
            if (dtCategory.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTreeList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYPK, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPARENTPK, GTIService.Constants.Material.Fields.HASCHILD, string.Empty, string.Empty, string.Empty);
            }
            return jString;
        }
        /// <summary>
        /// Function Used To Get all Material Category  Without Semi And Finished gooods
        /// </summary>
        /// <returns></returns>
        public static string GetMaterialCategoryWithoutSemiAndFinished(int materialCategoryParentPK, int sbuPK)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryWithoutSemiAndFinished(materialCategoryParentPK, sbuPK);
            string jString = string.Empty;
            if (dtCategory.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTreeList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYPK, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPARENTPK, GTIService.Constants.Material.Fields.HASCHILD, string.Empty, string.Empty, string.Empty); 
            }
            return jString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="materialCategoryParentPK"></param>
        /// <param name="type"></param>
        /// <param name="sbuPK"></param>
        /// <returns></returns>
        public static string GetMaterialCategoryTypeWithoutSemiAndFinished(int materialCategoryParentPK,int type, int sbuPK)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryTypeWithoutSemiAndFinished(materialCategoryParentPK, type, sbuPK);
            string jString = string.Empty;
            if (dtCategory.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTreeList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYPK, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPARENTPK, GTIService.Constants.Material.Fields.HASCHILD, string.Empty, string.Empty, string.Empty); 
            }
            return jString;
        }
        

        /// <summary>
        /// Function Used To Get Details Material Category By Catogory ID
        /// </summary>
        /// <param name="materialCategoryPK"></param>
        /// <returns></returns>
        public static string GetMaterialCategory(int materialCategoryPK)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategory(materialCategoryPK));
        }

        /// <summary>
        /// Function Used To Get all Material Category To Bind Combo
        /// </summary>
        /// <returns></returns>
        public static string GetMaterialCategoryList(int sbuPk)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryList(sbuPk);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPK);
        }
        /// <summary>
        /// Function Used To Get all Material Category except fg To Bind Combo
        /// </summary>
        /// <returns></returns>
        public static string GetMaterialCategoryListExceptFG(int sbuPk, int itemType, int store, int showSFG, string srchValue="%")
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryListExceptFG(sbuPk, itemType, store, showSFG, srchValue);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPK);
        }

        public static string GetMaterialCategoryAutoList(int sbuPk, int itemType, int store,string searchValue)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryAutoList(sbuPk, itemType, store, searchValue);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPK);
        }

        /// <summary>
        /// Function Used To Get all Material Category except fg To Bind Combo
        /// Show materials based on Qty available (not based on Active)
        /// </summary>
        /// <returns></returns>
        public static string GetMaterialCategoryStkAutoList(int sbuPk, int itemType, int store, string searchValue, int stock=1)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryStkAutoList(sbuPk, itemType, store, searchValue,stock);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPK);
        }
        /// <summary>
        /// Function Used To Get all Material Category To Bind Combo Without Semi And Finished gooods
        /// </summary>
        /// <returns></returns>
        public static string GetMaterialCategoryListWithoutSemiAndFinished(int sbuPk, int ITCVAL = 0,int includeFG=0)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryListWithoutSemiAndFinished(sbuPk, ITCVAL, includeFG);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPK);
        }
          /// <returns></returns>
        public static string GetMaterialCategoryTypeListWithoutSemiAndFinished(int sbuPk,int type)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryTypeListWithoutSemiAndFinished(sbuPk,type);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPK);
        }
         /// <summary>
        /// Function Used To Get all Material Category To Bind Combo
        /// </summary>
        /// <returns></returns>
        public static string GetMaterialCategoryRawMaterialList(int sbuPk)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryRawMaterialList(sbuPk);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPK);
        }
        
        /// <summary>
        /// Function Used To Get all Material Category To Bind Combo
        /// </summary>
        /// <returns></returns>
        public static string GetMaterialCategoryListAuto(int sbuPk, string srhcType,int type=0,int PM_WorkOrder=0)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryListAuto(sbuPk, srhcType,type, PM_WorkOrder);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPK);
        }

        /// <summary>
        /// Function Used To Get all Material Category To Bind Combo
        /// </summary>
        /// <returns></returns>
        public static string GetMaterialCategoryListExcepetFGAuto(int sbuPk, string srhcType)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryListExcepetFGAuto(sbuPk, srhcType);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPK);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptPK"></param>
        /// <param name="srchValue"></param>
        /// <returns></returns>
        public static string GetMaterialCategoryListDeptAuto(int sbuPk,int deptPK, string srchValue,int IsStock=-1)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryListDeptAuto(sbuPk,deptPK, srchValue, IsStock);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPK);
        }
        public static string GetPlantToPlantMaterialCategoryAuto(int sbuPk,int deptPK, string srchValue,int IsStock=-1)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetPlantToPlantMaterialCategoryAuto(sbuPk,deptPK, srchValue, IsStock);
            return GTIService.CommonFunctions.GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPK);
        }

        /// <summary>
        /// Function Used To Get all Material Category To Bind AutoComplete
        /// </summary>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetMaterialCategoryAuto(int sbuPk, string srhcType)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryListExcepetFGAuto(sbuPk, srhcType);
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>(GTIService.Constants.Material.Fields.CATEGORYPK),
                    Name = row.Field<string>(GTIService.Constants.Material.Fields.CATEGORYNAME)
                }).ToList();
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Returns Key Value List based on supplied data table
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="TextField"></param>
        /// <param name="ValueField"></param>
        /// <returns></returns>
        public static string GetTextValueList(DataTable dt, string TextField, string ValueField)
        {
            JavaScriptSerializer mySerializer = new JavaScriptSerializer();
            List<BusinessObject.MaterialManagement.TextValueList> TextValueList = new List<BusinessObject.MaterialManagement.TextValueList>();
            foreach (DataRow dr in dt.Rows)
            {
                TextValueList.Add(new BusinessObject.MaterialManagement.TextValueList() { Text = HttpUtility.HtmlDecode(Convert.ToString(dr[TextField])), Value = Convert.ToString(dr[ValueField])});
            }
            return mySerializer.Serialize(TextValueList);
        }

        /// <summary>
        /// Function Used To Get UOM Name By Category
        /// Function refered sajeer,
        /// </summary>
        /// <returns></returns>
        public static string GetUOMByCategory(int materialCategoryPK, int sbuPk)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetUOMByCategory(materialCategoryPK, sbuPk);
            string jString = string.Empty;
            if (dtCategory.Rows.Count > 0)
            {
                //jString = GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.UOMNAME, GTIService.Constants.Material.Fields.UOMPK,GTIService.Constants.Material.Fields.QCINSP);
                jString = GetTextValueList(dtCategory, GTIService.Constants.Material.Fields.UOMNAME, GTIService.Constants.Material.Fields.UOMPK);
            }
            return jString;
        }

        /// <summary>
        /// Method Used to Delete Material Category Detials
        /// </summary>
        /// <param name="ctgID"></param>
        /// <returns></returns>
        public static string DeleteMaterialCategory(int ctgID)
        {
            return DataAccess.MaterialManagement.MaterialCategoryMasterDL.DeleteMaterialCategory(ctgID).ToString();
        }

        /// <summary>
        /// Method Used to Get Material GRN Types
        /// </summary>
        /// <param name="context"></param>
        public static string GetGrnTypes(int bizUnit)
        {
            DataTable dtSearch =  DataAccess.CommonManagement.CommonDA.GetAppConfig(bizUnit, "ITC GRN TYPE", string.Empty);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.CFGVALUE, GTIService.Constants.Designation.Fields.CFGPK);
        }

        public static string GetMaterialCategoryByType(int materialCategoryParentPK, int sbuPK, int itemType, int store,int showSFG)
        {
            DataTable dtCategory = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryByType(materialCategoryParentPK, sbuPK, itemType, store,showSFG);
            string jString = string.Empty;
            if (dtCategory.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTreeList(dtCategory, GTIService.Constants.Material.Fields.CATEGORYPK, GTIService.Constants.Material.Fields.CATEGORYNAME, GTIService.Constants.Material.Fields.CATEGORYPARENTPK, GTIService.Constants.Material.Fields.HASCHILD, string.Empty, string.Empty, string.Empty);
            }
            return jString;
        }
        /// <summary>
        /// Method Used to Get ADM_Const Mst Values
        /// </summary>
        /// <param name="context"></param>
        public static string GetConstMstValues(int constPK, int constGroup, ConstGroupType? groupType, int groupValue, int active, int bizUnit)
        {
            DataTable dtSearch = DataAccess.CommonManagement.CommonDA.GetConstMstValues(constPK, constGroup, groupType, groupValue, active, bizUnit);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.CON_NAME, GTIService.Constants.Common.Fields.CON_VALUE);
        }

      /// <summary>
        /// To Fill Product Category
      /// </summary>
      /// <param name="PrdCategoryPK"></param>
      /// <param name="Active"></param>
        /// <param name="itcValue">2 for products, 9 for Brand products</param>
      /// <returns></returns>
        public static DataTable GetProductCategory(int PrdCategoryPK, int Active, int bizunit,int itcValue = 2)
        {
            return DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetProductCategory(PrdCategoryPK, Active, bizunit, itcValue);
        }

        public static string GetMaterialCategoryAccounts(int materialCategoryParentPK)
        {
            DataSet dsResult = DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryAccounts(materialCategoryParentPK);
            string jString = string.Empty;
            if (dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsResult.Tables[0]);
            }
            return jString;
          
        }

        #region External Material Issue (Multiple)

        public static DataTable GetItemCategory(int sbuPk, int itemType, int store, string searchValue)
        {
            return DataAccess.MaterialManagement.MaterialCategoryMasterDL.GetMaterialCategoryStkAutoList(sbuPk, itemType, store, searchValue);
        }
        public static DataTable GetItemCode(string searchValue, int categoryPk, int store)
        {
            return DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialSearchValueByCategoryAndStoreStk(searchValue, categoryPk, store);
        }
        public static DataTable GetItemName(string searchValue, int bizUnit, int Type, int IssueAgainst)
        {
            return DataAccess.StoreManagement.ExternalMaterialIssueDL.GetItemNameDDL(searchValue,bizUnit, Type, IssueAgainst);
        }
        public static DataTable GetSearchValues(string searchBy, string searchValue, int sbuPk, User objUser, int procID, int transactionType)
        {
            return DataAccess.StoreManagement.ExternalMaterialIssueDL.GetSearchValues(searchBy, searchValue, sbuPk, objUser, procID, transactionType);
        }

        #endregion
    }
}
