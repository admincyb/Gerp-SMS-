using System.Data;
using BusinessObject;
using System;

namespace BusinessLogic.SubDepartmentManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class SubDepartmentMaster
    {
        #region Methods
        /// <summary>
        /// Get Department Details 
        /// </summary>
        /// <param name="mainStoreID"></param>
        /// <param name="userId"></param>
        /// <returns>string</returns>
        public static string GetDepartmentDtls(User objUser, int sbuPk)
        {
            DataTable dtSearch = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetDepartmentDtls(objUser, sbuPk);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.DEPTNAME, GTIService.Constants.Designation.Fields.DEPTPK).ToString();

        }
        /// <summary>
        /// Get Department Details 
        /// </summary>
        /// <param name="mainStoreID"></param>
        /// <param name="userId"></param>
        /// <returns>string</returns>
        public static string GetDepartmentDtls(int sbuPk)
        {
            DataTable dtSearch = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetDepartmentDtls(sbuPk);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.DEPTNAME, GTIService.Constants.Designation.Fields.DEPTPK).ToString();

        }
        /// <summary>
        /// Get Non Store 
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <returns></returns>
        public static string GetNonStoreDeptDeparment(User objUser, int sbuPk)
        {
            DataTable dtSearch = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetNonStoreDeptDeparment(objUser, sbuPk);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.DEPTNAME, GTIService.Constants.Designation.Fields.DEPTPK).ToString();

        }
        /// <summary>
        /// Get Non Store GetNonStoreDeptUserSwitch
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <returns></returns>
        public static string GetNonStoreDeptUserSwitch(int sbuPk)
        {
            DataTable dtSearch = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetNonStoreDeptUserSwitch(sbuPk);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.DEPTNAME, GTIService.Constants.Designation.Fields.DEPTPK).ToString();

        }
        /// <summary>
        /// Get Department Details 
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="baseDpt"></param>
        /// <returns></returns>
        public static string GetDepartmentDtls(User objUser, int sbuPk, int baseDpt)
        {
            DataTable dtSearch = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetDepartmentDtls(objUser, sbuPk, baseDpt);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.DEPTNAME, GTIService.Constants.Designation.Fields.DEPTPK).ToString();

        }

        /// <summary>
        /// Get Sub Department List Details
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetSubDeptList(GridPrams grid, int bizUnit, int Status)
        {
            DataSet dsDesigList = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetSubDepartmentList(grid, bizUnit, Status);
            string jString = string.Empty;
            if (dsDesigList.Tables.Count > 1 && dsDesigList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsDesigList);
            }
            return jString;
        }

        /// <summary>
        /// Get AutoCompleted value For Search
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchBy, string searchValue, int bizUnit)
        {
            DataTable dtSearch = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetSearchValues(searchBy, searchValue, bizUnit);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);

        }

        /// <summary>
        /// Save Sub Department Details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>string</returns>
        public static string SaveSuDepartment(string requestData)
        {
            BusinessObject.SubDepartmentManagement.SubDepartmentMaster subDept = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.SubDepartmentManagement.SubDepartmentMaster>(requestData);
            subDept.Status = 1;
            subDept.DPT_PROJECT = string.IsNullOrEmpty(subDept.DPT_PROJECT) ? "-1" : subDept.DPT_PROJECT;
            return DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.SaveSubDepartment(subDept);
        }

        /// <summary>
        /// Delete Sub Dept Details by SubDeptID
        /// </summary>
        /// <param name="subDeptID"></param>
        /// <returns>string</returns>
        public static string DeleteSubDepartment(int subDeptID)
        {
            return DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.DeleteSubDepartmentDtls(subDeptID).ToString();
        }

        /// <summary>
        /// Get Sub Department Details By SubDept ID 
        /// </summary>
        /// <param name="subDeptID"></param>
        /// <returns>string</returns>
        public static string GetSubDeptDetailsBySubDeptID(int subDeptID)
        {
            DataSet dsDesigList = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetSubDepartmentDtlsBySubDeptID(subDeptID);
            string jString = string.Empty;
            jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsDesigList);
            return jString;
        }

        /// <summary>
        /// Get Sub Department Details By SubDept ID 
        /// </summary>
        /// <param name="subDeptID"></param>
        /// <returns>string</returns>
        public static Department GetDeptDetailsByID(int subDeptID)
        {
            Department dept = null;
            DataSet dsDesigList = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetSubDepartmentDtlsBySubDeptID(subDeptID);
            if (dsDesigList != null && dsDesigList.Tables.Count > 0 && dsDesigList.Tables[0] != null && dsDesigList.Tables[0].Rows.Count > 0)
            {
                dept = new Department()
                {
                    BaseCurrency = Convert.ToInt32(dsDesigList.Tables[0].Rows[0]["DPT_CURR"]),
                    CurrentDeptPK = Convert.ToInt32(dsDesigList.Tables[0].Rows[0]["DPT_PK"]),
                    CurrentDept = dsDesigList.Tables[0].Rows[0]["DPT_NAME"].ToString(),
                    CurrentSBUPK = Convert.ToInt32(dsDesigList.Tables[0].Rows[0]["DPT_BIZUNIT"])
                };
            }
            return dept;
        }

        /// <summary>
        /// Get All Inventory department details 
        /// </summary>
        /// <Created By>Vineeth</Created>
        /// <For>PO Dept Filling</For>
        /// <Used In>PoDepFilling,FillSelectedDepartDetails </Used>
        /// <param name="userID"></param>
        /// <returns>DataTable</returns>
        public static string GetInvDepartment(int bIZUNIT, int deptID)
        {
            string jString = string.Empty;
            if (deptID != 0)
            {
                DataTable dtDept = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetInvDepartment(bIZUNIT, deptID);
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtDept);
            }
            else
                jString = GTIService.CommonFunctions.GetTextValueList(DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetInvDepartment(bIZUNIT, deptID), GTIService.Constants.SubDepartment.Fields.VALUE, GTIService.Constants.SubDepartment.Fields.PK);
            return jString;
        }

        /// <summary>
        /// Method to get Inventory Stores list
        /// </summary>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <returns></returns>
        public static string GetInventoryStores(int sbuPk, int deptType, int userPK,int deptCompany=0)
        {
            DataTable dtDept = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetInventoryStores(sbuPk, deptType, userPK,deptCompany);
            return GTIService.CommonFunctions.GetTextValueList(dtDept, GTIService.Constants.SubDepartment.Fields.VALUE, GTIService.Constants.SubDepartment.Fields.PK);
        }
        public static string GetIssuingStores(int sbuPk, int deptType, int userPK,int deptCompany, int MaterialPK)
        {
            DataTable dtDept = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetIssuingStores(sbuPk, deptType, userPK, deptCompany, MaterialPK);
            return GTIService.CommonFunctions.GetTextValueList(dtDept, GTIService.Constants.SubDepartment.Fields.VALUE, GTIService.Constants.SubDepartment.Fields.PK);
        }


        /// <summary>
        /// Get Department Details ============= For SRS, Material Issue, Material Accept
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <param name="deptPk"></param>
        /// <returns></returns>
        public static string GetStoresByType(int deptPk, int deptType, User objUser, int sbuPk, int userFlag,int MenuType = 0)
        {
            DataTable dtStores = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetStoresByType(deptPk, deptType, objUser, sbuPk, userFlag, MenuType);
            return GTIService.CommonFunctions.GetTextValueList(dtStores, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTTEXTFIELD, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTVALUEFIELD);
        }

        /// <summary>
        /// Get Department Details ============= For SRS, Material Issue, Material Accept
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <param name="deptPk"></param>
        /// <returns></returns>
        public static string GetGeneralStores(int deptPk, int deptType, User objUser, int sbuPk, int userFlag, int deptChild = 0, int deptCompany = 0)
        {
            DataTable dtStores = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetGeneralStores(deptPk, deptType, objUser, sbuPk, userFlag, deptChild,deptCompany);
            return GTIService.CommonFunctions.GetTextValueList(dtStores, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTTEXTFIELD, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTVALUEFIELD);
        }

        /// <summary>
        /// Get All stores for Store Audit
        /// </summary>
        /// <param name="deptPk"></param>
        /// <param name="deptType"></param>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="userFlag"></param>
        /// <param name="deptCatg"></param>
        /// <returns></returns>
        public static DataTable GetAllStores(int deptPk, int deptType, User objUser, int sbuPk, int userFlag, int deptCatg)
        {
            return DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetAllStores(deptPk, deptType, objUser, sbuPk, userFlag, deptCatg);

        }
        /// <summary>
        /// Method to get All store Names
        /// </summary>
        /// <param name="deptPk"></param>
        /// <param name="deptType"></param>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="userFlag"></param>
        /// <param name="deptCatg"></param>
        /// <returns></returns>
        public static string GetAllStoresDetails(int deptPk, int deptType, User objUser, int sbuPk, int userFlag, int deptCatg)
        {
            DataTable dtStores = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetAllStores(deptPk, deptType, objUser, sbuPk, userFlag, deptCatg);
            return GTIService.CommonFunctions.GetTextValueList(dtStores, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTTEXTFIELD, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTVALUEFIELD);

        }

        /// <summary>
        /// Method to get Inventory Stores list
        /// </summary>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <returns></returns>
        public static string GetInventoryStoresBasedOnConfig(string fieldName, int sbuPk, int userPK, int processId)
        {
            DataTable dtSearch = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetInventoryStoresBasedOnConfig(fieldName, sbuPk, userPK, processId);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);
        }

        ///// <summary>
        ///// Method to get Inventory Stores as DataTable 
        ///// </summary>
        ///// <param name="sbuPk"></param>
        ///// <param name="deptType"></param>
        ///// <returns></returns>
        //public static DataTable GetInventoryStoresByConfig(string fieldName, int sbuPk, int userPK, int processId)
        //{
        //    DataTable dtSearch = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetInventoryStoresBasedOnConfig(fieldName, sbuPk, userPK, processId);
        //    return dtSearch;
        //}

        /// <summary>
        /// Get Department Details ============= For SRS, Material Issue, Material Accept
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <param name="deptPk"></param>
        /// <returns></returns>
        public static DataTable GetGeneralStoresByConfig(int deptPk, int deptType, User objUser, int sbuPk, int userFlag, int deptChild = 0, int deptCompany = 0, int DeptCategory = 0)
        {
            DataTable dtStores = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetGeneralStores(deptPk, deptType, objUser, sbuPk, userFlag, deptChild, deptCompany, DeptCategory);
            return dtStores;
        }

        public static DataTable GetInventoryStoresByConfig_New(int sbuPk, int deptType, int userPK, int deptCompany = 0)
        {
            DataTable dtDept = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetInventoryStores(sbuPk, deptType, userPK, deptCompany);
            return dtDept;
        }

        #endregion


    }
}
