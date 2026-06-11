using System.Data;
using BusinessObject;
using System.Web;
using System.IO;
using System;
using System.Xml;
using BusinessObject.CommonManagement;
namespace BusinessLogic.CommonManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class CommonManagement
    {
        /// <summary>
        /// Check Folder Exists or Not
        /// </summary>
        /// <param name="uploadUrl"></param>
        /// <returns>Bool</returns>
        public static bool CheckFolderExists(string uploadUrl)
        {
            if (System.IO.Directory.Exists(uploadUrl))

                return true;
            else
                return false;

        }
        /// <summary>
        /// Create Folder, If Folder Not Exists
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="uploadFolder"></param>
        /// <returns>Bool</returns>
        public static bool CreateFolder(string rootPath, string uploadFolder)
        {
            try
            {
                System.IO.Directory.CreateDirectory(rootPath + uploadFolder);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Delete File From Folder
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool DeleteFileFromFolder(string filePath)
        {
            string uploadPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath);
            try
            {
                File.Delete(uploadPath + filePath);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }



        /// <summary>
        /// Save File Details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static string SaveFileDtls(string requestData)
        {
            BusinessObject.Common.fileUpload fupload = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Common.fileUpload>(requestData);
            fupload.Folder = "Store";
            UploadFromTempFolder(fupload.FileID, "Store");
            return string.Empty;
        }
        /// <summary>
        /// Upload Frm Temp File To Upload Folder
        /// </summary>
        /// <param name="fupload"></param>
        /// <returns></returns>
        public static bool UploadFromTempFolder(string fileID, string folder)
        {
            bool status = true;
            string tempFolder = GTIService.Constants.Common.FileUpload.TEMPFOLDER;
            string[] fileIDs = fileID.Split(',');

            string uploadPath = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + GTIService.Constants.Common.FileUpload.UPLOADURL;


            if (CheckFolderExists(uploadPath + folder))
            {
                for (int indx = 0; indx < fileIDs.Length; indx++)
                {
                    status = UploadFile(uploadPath + tempFolder + "\\" + fileIDs[indx].ToString(), uploadPath + folder + "\\" + fileIDs[indx].ToString());
                }
            }
            else
            {
                if (CreateFolder(uploadPath, folder))
                {
                    for (int indx = 0; indx < fileIDs.Length; indx++)
                    {
                        status = UploadFile(uploadPath + tempFolder + "\\" + fileIDs[indx].ToString(), uploadPath + folder + "\\" + fileIDs[indx].ToString());
                    }
                }
            }
            return status;
        }


        public static bool UploadFromTempFolderForXml(string fileID, string folder)
        {
            bool status = true;
            string tempFolder = GTIService.Constants.Common.FileUpload.TEMPFOLDER;
            string uploadPath = HttpContext.Current.Request.MapPath("~/") + GTIService.Constants.Common.FileUpload.UPLOADURL;
            if (CheckFolderExists(uploadPath + folder))
            {
                status = UploadFile(uploadPath + tempFolder + "\\" + fileID, uploadPath + folder + "\\" + fileID);

            }
            else
            {
                if (CreateFolder(uploadPath, folder))
                {

                    status = UploadFile(uploadPath + tempFolder + "\\" + fileID, uploadPath + folder + "\\" + fileID);

                }
            }
            return status;
        }

        /// <summary>
        /// Move File From Source To Destination Folder
        /// </summary>
        /// <param name="tempPath"></param>
        /// <param name="uploadPath"></param>
        public static bool UploadFile(string tempPath, string uploadPath)
        {
            try
            {
                File.Move(tempPath, uploadPath);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// Method for get country as data Table
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCountries()
        {
            return DataAccess.CommonManagement.CommonDL.GetCountry();
        }
        /// <summary>
        /// Methord used to get the Country Details
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public static string GetCountry()
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.CommonManagement.CommonDL.GetCountry(), GTIService.Constants.Common.Fields.COUNTRYNAME, GTIService.Constants.Common.Fields.COUNTRYID);
        }


        /// <summary>
        /// Method for Project Autocomplete
        /// </summary>
        /// <returns></returns>
        public static string GetProjectListAuto(string srchBy, string srhcType)
        {
            DataTable dtProject = DataAccess.CommonManagement.CommonDL.GetProjectListAuto(srchBy, srhcType);
            return GTIService.CommonFunctions.GetTextValueList(dtProject, GTIService.Constants.Common.Fields.PROJECTNAME, GTIService.Constants.Common.Fields.PROJECTID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountpk"></param>
        /// <param name="active"></param>
        /// <param name="subtype"></param>
        /// <param name="IsGroup"></param>
        /// <returns></returns>
        public static string GetAccount(int accountpk, int active, int subtype, int IsGroup, int IsIncludeAccCode, int BizUnit = 0)
        {
             return GTIService.CommonFunctions.GetTextValueList(DataAccess.CommonManagement.CommonDL.GetAccount(accountpk, active, subtype, IsGroup, BizUnit), IsIncludeAccCode == 1 ? GTIService.Constants.Common.Fields.ACCOUNTNAMETEXT : GTIService.Constants.Common.Fields.ACCOUNTNAME, GTIService.Constants.Common.Fields.ACCOUNTPK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountpk"></param>
        /// <param name="active"></param>
        /// <param name="subtype"></param>
        /// <param name="IsGroup"></param>
        /// <returns></returns>
        public static string GetAccountWithCode(int accountpk, int active, int subtype, int IsGroup, int IsIncludeAccCode,int sbuID)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.CommonManagement.CommonDL.GetAccount(accountpk, active, subtype, IsGroup, sbuID), IsIncludeAccCode == 1 ? GTIService.Constants.Common.Fields.ACCOUNTNAMETEXT : GTIService.Constants.Common.Fields.ACCOUNTNAMEWITHCODE, GTIService.Constants.Common.Fields.ACCOUNTPK);
        }

        public static string GetSBU(int userPK)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.CommonManagement.CommonDA.GetBizUnit(0), GTIService.Constants.Common.Fields.BZU_NAME, GTIService.Constants.Common.Fields.BZU_PK);
        }

        public static DataTable GetAllBizUnit(int sbuID, int Active)
        {
            return DataAccess.CommonManagement.CommonDA.GetAllBizUnit(sbuID, Active);
        }

        /// <summary>
        /// Get Company List
        /// </summary>
        /// <returns></returns>
        public static string GetCompany(int apsPK, short active, int bizUnit)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.CompanyMasterDA.GetCompanyMaster(apsPK, active, bizUnit).Tables[0], GTIService.Constants.Common.Fields.CMP_NAME, GTIService.Constants.Common.Fields.CMP_PK);
        }

        /// <summary>
        /// Get Company Auto
        /// </summary>
        /// <returns></returns>
        public static string GetCompanyAutoComplete(int apsPK, short active, int bizUnit, string SplCond = null)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.CompanyMasterDA.GetCompanyMaster(apsPK, active, bizUnit, SplCond).Tables[0], GTIService.Constants.Common.Fields.CMP_DISPLAY_NAME, GTIService.Constants.Common.Fields.CMP_PK);
        }



        /// <summary>
        /// Get Company List as DataTable
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCompanyList(int apsPK, short active, int bizUnit)
        {
            return DataAccess.Administration.Masters.CompanyMasterDA.GetCompanyMaster(apsPK, active, bizUnit).Tables[0];
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupValue"></param>
        /// <returns></returns>
        public static string GetCommonDDL(int groupValue)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.CompanyMasterDA.GetCommonDDL(groupValue).Tables[0], GTIService.Constants.Common.Fields.CON_NAME, GTIService.Constants.Common.Fields.CON_VALUE);
        }

        /// <summary>
        /// Methord used to get the State Details Corresponding to country ID
        /// </summary>
        /// <param name="country"></param>
        /// <param name=""></param>
        public static string GetState(int countryID)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.CommonManagement.CommonDL.GetState(countryID), GTIService.Constants.Common.Fields.STATENAME, GTIService.Constants.Common.Fields.STATEID);

        }

        /// <summary>
        /// Methord used to get the Country Details
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public static string GetCurrency(User objUser, int bizUnit)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.CommonManagement.CommonDL.GetCurrency(objUser, bizUnit), GTIService.Constants.Common.Fields.CURRENCYCODE, GTIService.Constants.Common.Fields.CURRENCYID);
        }
        /// <summary>
        /// GetDepartment Details
        /// </summary>
        /// <returns></returns>

        public static string GetDepartment()
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.CommonManagement.CommonDL.GetDepartment(), GTIService.Constants.Common.Fields.DEPTNAME, GTIService.Constants.Common.Fields.DEPTID);
        }

        /// <summary>
        /// Get DeptDetails To Fill Department Details DropDown GetDepartmentTypeandText
        /// </summary>
        /// <param name="deptPK"></param>
        /// <param name="deptParentPK"></param>
        /// <returns>string</returns>
        public static string GetDepartmentName(int bizUnit)
        {
            DataTable dtDept = DataAccess.CommonManagement.CommonDL.GetDepartmentName(bizUnit);
            return GTIService.CommonFunctions.GetTextValueList(dtDept, GTIService.Constants.Common.Fields.DEPARTMENTNAME, GTIService.Constants.Common.Fields.DEPARTMENTPK);

        }
        /// <summary>
        /// Get Accounts
        /// </summary>
        /// <param name="subType"></param>
        /// <returns></returns>
        public static string GetAccountType(int subType)
        {
            DataTable dtAccounts = CommonBL.GetAccountType(0, subType, 1);
            return GTIService.CommonFunctions.GetTextValueList(dtAccounts, GTIService.Constants.Common.Fields.ACCOUNTNAME, GTIService.Constants.Common.Fields.ACCOUNTPK);

        }
        /// <summary>
        /// Get Application Status
        /// </summary>
        /// <param name="type"></param>
        /// <param name="subType"></param>
        /// <returns></returns>
        public static string GetAppStatus(string type, string subType)
        {
            DataTable dtAccounts = CommonBL.GetAppStatus(type, subType);
            return GTIService.CommonFunctions.GetTextValueList(dtAccounts, GTIService.Constants.Common.Fields.ASC_NAME, GTIService.Constants.Common.Fields.ASC_VALUE);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string GetAccountType(string xml)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(xml);
            DataTable dtAccounts = CommonBL.GetAccountType(xmlstr);
            return GTIService.CommonFunctions.GetTextValueList(dtAccounts, GTIService.Constants.Common.Fields.ACCOUNTNAMETEXT, GTIService.Constants.Common.Fields.ACCOUNTPK);

        }

        public static string GetAccountTypeAuto(string SubType, int AccountPK, int Active, int IsGroup, int BizUnitPK, string srchValue)          
        {
            //string xmlstr = GTIService.CommonFunctions.JsonToXml(xml);
            //DataTable dtAccounts = CommonBL.GetAccountType(xmlstr);
            //return GTIService.CommonFunctions.GetTextValueList(dtAccounts, GTIService.Constants.Common.Fields.ACCOUNTNAMETEXT, GTIService.Constants.Common.Fields.ACCOUNTPK);
            DataTable dtSearch = DataAccess.MaterialManagement.MaterialMasterDL.GetAccountTypeAuto(SubType, AccountPK, Active, IsGroup, BizUnitPK, srchValue);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Material.Fields.MATERIALSEARCHTEXTFIELD, GTIService.Constants.Material.Fields.MATERIALSEARCHVALUEFIELD);

        }
        /// <summary>
        /// Get Plant
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static string GetPlant(string xml)
        {
            DataTable dtPlants = CommonBL.GetPlant(xml);
            return GTIService.CommonFunctions.GetTextValueList(dtPlants, GTIService.Constants.Common.Fields.PLT_NAME, GTIService.Constants.Common.Fields.PLT_PK);

        }

        /// <summary>
        /// Get Workflow Status
        /// </summary>
        /// <param name="refID">Referance ID</param>
        /// <param name="processID">Process ID</param>
        /// <returns></returns>
        public static string GetWorkflowStatus(int refID, int processID)
        {
            DataTable dtStatus = DataAccess.CommonManagement.CommonDL.GetWorkflowStatus(refID, processID);
            string jString = string.Empty;
            if (dtStatus.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtStatus);
            }
            return jString;

            //  return dtDept.Rows[0]["PROCESS_PK"].ToString();
        }

        /// <summary>
        /// Get Workflow Status
        /// </summary>
        /// <param name="refID">Referance ID</param>
        /// <param name="processID">Process ID</param>
        /// <returns></returns>
        public static DataTable GetWorkProcesstatus(int refID, int processID)
        {
            return DataAccess.CommonManagement.CommonDL.GetWorkflowStatus(refID, processID);
        }

        /// <summary>
        /// Get Process ID
        /// </summary>
        /// <param name="path"></param>
        /// <param name="DeptID"></param>
        /// <returns></returns>
        public static string GetProcesID(string path, int DeptID)
        {
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtDept = wrkfService.GetProcessID(path, DeptID);
            string jString = string.Empty;
            if (dtDept.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtDept);
            }
            return jString;

            //  return dtDept.Rows[0]["PROCESS_PK"].ToString();
        }
        /// <summary>
        /// Get Parent Departments categories Name .
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="parentDepartment"></param>
        /// <returns></returns>
        public static string GetParentDepartmentCategories(int bizUnit, string parentDepartment, int CFG_PK = 0)
        {
            DataTable dtDept = DataAccess.CommonManagement.CommonDL.GetParentDepartmentCategories(bizUnit, parentDepartment, CFG_PK);
            return GTIService.CommonFunctions.GetTextValueList(dtDept, GTIService.Constants.Common.Fields.CATEGORYNAME, GTIService.Constants.Common.Fields.CATEGORYPK);
        }

        /// <summary>
        /// Get Parent Departments categories Name .
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="parentDepartment"></param>
        /// <returns></returns>
        public static string GetParentDepartmentCategoriesByID(int bizUnit, string parentDepartment)
        {
            DataTable dtDept = DataAccess.CommonManagement.CommonDL.GetParentDepartmentCategoriesByID(bizUnit, parentDepartment);
            return GTIService.CommonFunctions.GetTextValueList(dtDept, GTIService.Constants.Common.Fields.CATEGORYNAME, GTIService.Constants.Common.Fields.CATEGORYPK);
        }


        /// <summary>
        /// Get DeptDetails To Fill Department Details DropDown  type and text
        /// </summary>
        /// <param name="deptPK"></param>
        /// <param name="deptParentPK"></param>
        /// <returns>string</returns>
        public static string GetParentDepartments(int bizUnit)
        {
            DataTable dtDept = DataAccess.CommonManagement.CommonDL.GetParentDepartments(bizUnit);
            return GTIService.CommonFunctions.GetTextValueList(dtDept, GTIService.Constants.Common.Fields.DEPARTMENTNAME, GTIService.Constants.Common.Fields.DEPARTMENTPK);

        }
        /// <summary>
        /// Get Department Details For TreeView
        /// </summary>
        /// <param name="deptPK"></param>
        /// <param name="deptParentPK"></param>
        /// <returns></returns>
        public static string GetDepartmentDtls(int deptPK, int deptParentPK, int bizUnit, int userGroup)
        {
            DataTable dtDept = DataAccess.CommonManagement.CommonDL.GetDepartmentDtls(deptPK, deptParentPK, bizUnit, userGroup);
            string jString = string.Empty;
            if (dtDept.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTreeList(dtDept, GTIService.Constants.Common.Fields.DEPARTMENTPK, GTIService.Constants.Common.Fields.DEPARTMENTNAME, GTIService.Constants.Common.Fields.DEPARTMENTPARENT, GTIService.Constants.Common.Fields.HASCHILD, GTIService.Constants.Common.Fields.HASUSERGROUP, string.Empty, string.Empty);
            }
            return jString;
        }
        //05042011

        /// <summary>
        /// Get Country name  For AutoComplete
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public static string GetCountryAutoComplete(string countryName)
        {
            DataTable dtSearch = DataAccess.CommonManagement.CommonDL.GetCountrySearchValues(countryName);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.VALUE, GTIService.Constants.Common.Fields.PK);

        }
        /// <summary>
        /// Get State Name For AutoComplete Search
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="countryPK"></param>
        /// <returns></returns>
        public static string GetStateAutoComplete(string stateName, int countryPK)
        {
            DataTable dtSearch = DataAccess.CommonManagement.CommonDL.GetStateSearchValues(stateName, countryPK);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.VALUE, GTIService.Constants.Common.Fields.PK);

        }
        /// <summary>
        /// Get Currency name For AutoComplete
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        public static string GetCurrencyAutoComplete(string countryName)
        {
            DataTable dtSearch = DataAccess.CommonManagement.CommonDL.GetCurrencySearchValues(countryName);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.VALUE, GTIService.Constants.Common.Fields.PK);

        }


        ///// For FileUpload 06042011

        /// <summary>
        /// Save File Details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static string SaveFileTest(string requestData)
        {
            BusinessObject.Common.fileUpload fupload = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.Common.fileUpload>(requestData);
            fupload.Folder = "Store";
            UploadFromTempFolder(fupload.FileID, "Store");
            return string.Empty;
        }

        /// <summary>
        /// Get User Group List
        /// </summary>
        /// <returns></returns>
        public static string GetUserGroup()
        {
            DataTable dtUsrGroup = DataAccess.CommonManagement.CommonDL.GetUserGroup();
            return GTIService.CommonFunctions.GetTextValueList(dtUsrGroup, GTIService.Constants.Common.Fields.USERGROUPNAME, GTIService.Constants.Common.Fields.USERGROUPPK);
        }

        /// <summary>
        /// Get User Permission To Initial Task
        /// </summary>
        /// <returns></returns>
        public static bool GetInitialTaskPermission(int userID, int processID)
        {
            DataTable dtUsrPermission = DataAccess.CommonManagement.CommonDL.GetInitialTaskPermission(userID, processID);
            if (Convert.ToInt32(dtUsrPermission.Rows[0][0].ToString()) > 0)
                return true;
            else
                return false;

        }

        /// <summary>
        /// Get Process Details By UserPK
        /// </summary>
        /// <param name="userPK"></param>
        /// <returns></returns>
        public static string GetProcess(int userPK)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.CommonManagement.CommonDL.GetProcessList(userPK), GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refPk"></param>
        /// <returns></returns>
        public static string GetWrkfCommentList(int refPk, int appID, int procID)
        {
            DataSet dsWrkfComment = DataAccess.CommonManagement.CommonDL.GetWrkfCommentList(refPk, appID, procID);
            string jString = string.Empty;
            if (dsWrkfComment.Tables.Count > 1 && dsWrkfComment.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsWrkfComment);
            }
            return jString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refPk"></param>
        /// <returns></returns>
        public static string SaveWrkfCommentList(BusinessObject.CommonManagement.CommonObject.WorkFlowComment workFlowComment)
        {
            return DataAccess.CommonManagement.CommonDL.SaveWrkfCommentList(workFlowComment);
        }



        /// <summary>
        /// Function used to get ShiftDetails to fill dropdown
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetShift(int bizUnit, string searchval)
        {
            DataTable dtShift = DataAccess.CommonManagement.CommonDL.GetShift(bizUnit, searchval);
            string jString = string.Empty;
            if (dtShift.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtShift, "SHF_NAME", "SHF_PK");
                //jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtShift);
            }
            return jString;
        }

        /// <summary>
        /// Function used to get ShiftDetails to fill dropdown
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetEmployees(int bizUnit, string searchval)
        {
            DataTable dtShift = DataAccess.CommonManagement.CommonDL.GetEmployees(bizUnit, searchval);
            string jString = string.Empty;
            if (dtShift.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtShift, "EMP_NAME", "USER_PK");
                //jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtShift);
            }
            return jString;
        }

        /// <summary>
        /// Function used to get Products to fill dropdown
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetProducts(int bizUnit)
        {
            DataTable dtProducts = DataAccess.CommonManagement.CommonDL.GetProducts(bizUnit);
            string jString = string.Empty;
            if (dtProducts.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtProducts, "PRO_NAME", "PRO_PK");
            }
            return jString;
        }

        /// <summary>
        /// Get Constant Table Values
        /// </summary>
        /// <param name="constPK"></param>
        /// <param name="constGroup"></param>
        /// <param name="groupType"></param>
        /// <param name="groupValue"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetConstMstValues(int constPK, int constGroup, ConstGroupType? groupType, int groupValue, int active, int bizUnit)
        {
            DataTable dtConstMstValues = DataAccess.CommonManagement.CommonDA.GetConstMstValues(constPK, constGroup, groupType, groupValue, active, bizUnit);
            string jString = string.Empty;
            if (dtConstMstValues.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtConstMstValues, "CON_NAME", "CON_PK");
            }
            return jString;
        }

        /// <summary>
        /// Get Vaetgory Value
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static string GetCategoryValue(int categoryPK, int active)
        {
            DataTable dtCatValue = DataAccess.CommonManagement.CommonDA.GetCategoryValue(categoryPK, active);
            string jString = string.Empty;
            if (dtCatValue.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtCatValue);
            }
            return jString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="constPK"></param>
        /// <param name="constGroup"></param>
        /// <param name="groupType"></param>
        /// <param name="groupValue"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetConstMstValuesDtl(int constPK, int constGroup, ConstGroupType? groupType, int groupValue, int active, int bizUnit)
        {
            DataTable dtConstMstValues = DataAccess.CommonManagement.CommonDA.GetConstMstValues(constPK, constGroup, groupType, groupValue, active, bizUnit);
            string jString = string.Empty;
            if (dtConstMstValues.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtConstMstValues);
            }
            return jString;
        }

        /*
          
         */
        /// <summary>
        /// 
        /// </summary>
        /// <param name="custPK"></param>
        /// <param name="name"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static string GetCustomer(int custPK, string name, int bizUnit, int active)
        {
            DataTable dsCustomers = BusinessLogic.Sales.CustomerProduct.GetCustomer(custPK, name, bizUnit, active).Tables[0];
            string jString = string.Empty;
            if (dsCustomers.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dsCustomers, "CUS_NAME", "CUS_PK");
            }
            return jString;
        }

        /// <summary>
        /// Function used to get Products to fill dropdown
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetConstMstValues(int bizUnit)
        {
            DataTable dtProducts = DataAccess.CommonManagement.CommonDL.GetProducts(bizUnit);
            string jString = string.Empty;
            if (dtProducts.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtProducts, "PRO_NAME", "PRO_PK");
            }
            return jString;
        }

        /// <summary>
        /// Function used to get Plans to fill dropdown
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetPlans(int bizUnit)
        {
            DataTable dtPlans = DataAccess.CommonManagement.CommonDL.GetPlans(bizUnit);
            string jString = string.Empty;
            if (dtPlans.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtPlans, "PLN_Name", "PLN_PK");
            }
            return jString;
        }


        /// <summary>
        /// Function used to get Plans to fill dropdown
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetCompoundBatchno(int bizUnit, int prdType)
        {
            DataTable dtCompound = DataAccess.CommonManagement.CommonDL.GetCompoundBatchno(bizUnit, prdType);
            string jString = string.Empty;
            if (dtCompound.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtCompound, "CTH_BATCH_NO", "CTH_PK");
            }
            return jString;
        }

        /// <summary>
        /// Function used to get Lines to fill dropdown
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetLines(int bizUnit)
        {
            DataTable dtLines = DataAccess.CommonManagement.CommonDL.GetLines(bizUnit);
            string jString = string.Empty;
            if (dtLines.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtLines, "LNE_NAME", "LNE_PK");
            }
            return jString;
        }

        /// <summary>
        /// Function used to get Product Details
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetProductDetails(int productID)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(DataAccess.CommonManagement.CommonDL.GetProductDetails(productID));

        }


        /// <summary>
        /// Function used to get CheckList Details
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetCheckListDetails(int checkListID)
        {
            return GTIService.CommonFunctions.XmlToJson1(DataAccess.CommonManagement.CommonDL.GetCheckListDetails(checkListID));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuName"></param>
        /// <param name="userPK"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetMenuAutoComplete(string menuName, int userPK, int bizUnit, string menuType)
        {
            DataTable dtSearch = DataAccess.CommonManagement.CommonDL.GetMenuAutoComplete(menuName, userPK, bizUnit, menuType);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, "MNU_NAME", "PAG_URL");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workFlowDetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static int SaveWorkFlow(string workFlowDetails, User objUser)
        {
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            WorkflowCore.CoreObjects.DoWorkFlowRequest wrkfReq = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowCore.CoreObjects.DoWorkFlowRequest>(workFlowDetails, settings);
            WorkflowCore.CoreObjects.WorkFlowComment wrkfcmts = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowCore.CoreObjects.WorkFlowComment>(workFlowDetails, settings);
            return DataAccess.CommonManagement.CommonDL.SaveWorkFlow(wrkfReq, wrkfcmts, objUser);
        }

        public static DataTable GetUserCustomer(int userPK, int bizUnit)
        {
            return DataAccess.CommonManagement.CommonDL.GetUserCustomer(userPK, bizUnit);
        }

        public static string GetTemplateCheckList(int checkListPK, int bizUnit, int active, int processID)
        {
            DataTable dtCheckList = DataAccess.CommonManagement.CommonDL.GetTemplateCheckList(checkListPK, bizUnit, active, processID);
            string jString = string.Empty;
            if (dtCheckList.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtCheckList, "CHT_NAME", "CHT_PK");
            }
            return jString;
        }
        public static DataSet GetProductIterationReport(int testPk)
        {
            return DataAccess.CommonManagement.CommonDL.GetProductIterationReport(testPk);
        }
        public static DataSet GetBincardProductReport(int productPk, DateTime fromDate, DateTime toDate, int linePk, int shiftPk, int productType, int bizUnit)
        {
            return DataAccess.CommonManagement.CommonDL.GetBincardProductReport(productPk, fromDate, toDate, linePk, shiftPk, productType, bizUnit);
        }

        public static string GetDepartmentsDDL(int Pk, int Active, int UserPk, int bizUnit, int ProcessID, int RefID)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.CompanyMasterDA.GetDepartments(Pk, Active, UserPk, bizUnit, ProcessID, RefID), GTIService.Constants.Common.Fields.CON_NAME, GTIService.Constants.Common.Fields.CON_VALUE);
        }
        public static string POCategoryDDLGet(int Pk, int Active, int UserPk, int bizUnit, int ProcessID, int RefID)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.CompanyMasterDA.POCategoryDDLGet(Pk, Active, UserPk, bizUnit, ProcessID, RefID), GTIService.Constants.Common.Fields.CON_NAME, GTIService.Constants.Common.Fields.CON_VALUE);
        }

        public static string GetSubDepartmentsDDL(int Pk, int Active, int ParentPk, int bizUnit)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.CompanyMasterDA.GetSubDepartments(Pk, Active, ParentPk, bizUnit), GTIService.Constants.Common.Fields.CON_NAME, GTIService.Constants.Common.Fields.CON_VALUE);
        }
        /// <summary>
        /// SPADM_CONST_MST_GET_KV
        /// </summary>
        /// <param name="Pk"></param>
        /// <param name="Active"></param>
        /// <param name="Group"></param>
        /// <param name="GroupTypeConst"></param>
        /// <param name="GroupConstant"></param>
        /// <param name="BizUnit"></param>
        /// <returns></returns>
        public static string GetConstantValue(int Pk, int Active, int Group, int GroupTypeConst, int GroupConstant, int BizUnit)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.CompanyMasterDA.GetConstantValue(Pk, Active, Group, GroupTypeConst, GroupConstant, BizUnit), GTIService.Constants.Common.Fields.CON_NAME, GTIService.Constants.Common.Fields.CON_VALUE);
        }

        public static string GetInvestmentValue(int Pk, int Active, int Group, int GroupTypeConst, int GroupConstant, int BizUnit)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.CompanyMasterDA.GetConstantValue(Pk, Active, Group, GroupTypeConst, GroupConstant, BizUnit), GTIService.Constants.Common.Fields.CON_CODE, GTIService.Constants.Common.Fields.CON_VALUE);
        }

        public static string GetInvestmentList(int PK,string SearchValue, int Active, int BizUnit)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.CompanyMasterDA.GetInvestmentList(PK,SearchValue, Active, BizUnit), GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);

        }

        /// <summary>
        /// SPADM_CONST_MST_GET_KV
        /// </summary>
        /// <param name="Pk"></param>
        /// <param name="Active"></param>
        /// <param name="Group"></param>
        /// <param name="GroupTypeConst"></param>
        /// <param name="GroupConstant"></param>
        /// <param name="BizUnit"></param>
        /// <returns></returns>
        public static string GetConstantValueAuto(int Pk, int Active, int Group, int GroupTypeConst, int GroupConstant, int BizUnit, string SearchValue = "")
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.CompanyMasterDA.GetConstantValueAuto(Pk, Active, Group, GroupTypeConst, GroupConstant, BizUnit, SearchValue), GTIService.Constants.Common.Fields.CON_NAME, GTIService.Constants.Common.Fields.CON_VALUE);
        }

        /// <summary>
        /// Get Company List
        /// </summary>
        /// <returns></returns>
        public static string GetCompanyMappingDetails(int cmpPk, short active, int bizUnit, int deptPK)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.CompanyMasterDA.GetCompanyMappingDetails(cmpPk, active, bizUnit, deptPK).Tables[0], GTIService.Constants.Common.Fields.CMP_DISPLAY_NAME, GTIService.Constants.Common.Fields.CMP_PK);
        }

        /// <summary>
        /// Get Company List
        /// </summary>
        /// <returns></returns>
        public static string GetCompanyDisplayNames(int cmpPk, short active, int bizUnit, int deptPK)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.Administration.Masters.CompanyMasterDA.GetCompanyMappingDetails(cmpPk, active, bizUnit, deptPK).Tables[0], GTIService.Constants.Common.Fields.CMP_DISPLAY_CODE, GTIService.Constants.Common.Fields.CMP_PK);
        }

        /// <summary>
        /// Get Account Types
        /// </summary>
        /// <param name="subType"></param>
        /// <returns></returns>
        public static string GetAccountTypesforMapping(int bizunit, string subType, string splCond)
        {
            DataTable dtAccounts = CommonBL.GetAccountTypesforMapping(bizunit, subType, splCond);
            return GTIService.CommonFunctions.GetTextValueList(dtAccounts, GTIService.Constants.Common.Fields.ADM_CFG_TEXT, GTIService.Constants.Common.Fields.ADM_CFG_VALUE);

        }
        public static string GetPoCreators(int bizUnit)
        {
            return GTIService.CommonFunctions.GetTextValueList(DataAccess.CommonManagement.CommonDL.GetPoCreators(bizUnit), GTIService.Constants.Common.Fields.EMPLOYEENAME, GTIService.Constants.Common.Fields.EMPLOYEEPK);
        }
    }
}
