using System.Data;
using BusinessObject;
using DataAccess.MaterialManagement;
using System;
using BusinessObject.CommonManagement;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessLogic.MaterialManagement
{
    /// <summary>
    /// This class is used to communicate with Dataaccess layer.
    /// </summary>
    public class MaterialMaster
    {
        #region Methods

        /// <summary>
        /// Save vendorDetails Details -  Details
        /// </summary>
        /// <param name="vendorDetails"></param>
        /// <returns>string</returns>
        public static string SaveMaterialVendorDetails(string vendorDetails)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(vendorDetails);
            //Call removeInvalidElement for firefox Bug Fix (removing binary file data from the converted string)
            string nodeToRemove = "prevObject,context,0";
            xmlstr = GTIService.CommonFunctions.removeInvalidElement(xmlstr, nodeToRemove);
            return DataAccess.MaterialManagement.MaterialMasterDL.SaveMaterialVendorDetails(xmlstr).ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strXml"></param>
        /// <returns></returns>
        public static int SaveOrderItemDetails(string strXml)
        {
            return DataAccess.MaterialManagement.MaterialMasterDL.SaveOrderItemDetails(strXml);
        }
        /// <summary>
        /// Get vendorDetails Details As a XML Format and Convert in to JSON and Return As a JSON String Format
        /// </summary>
        /// <param name="itemPk"></param>
        /// <returns></returns>
        public static string GetVendorMappingDetails(int itemPk)
        {

            BusinessObject.MaterialManagement.Vendor obj = new BusinessObject.MaterialManagement.Vendor();
            return GTIService.CommonFunctions.XmlToJson(DataAccess.MaterialManagement.MaterialMasterDL.GetVendorMappingDetails(itemPk));

        }
       /// <summary>
       /// Save Vendor Material
       /// </summary>
       /// <param name="requestData"></param>
       /// <param name="objUser"></param>
       /// <returns></returns>
        public static string SaveVenMaterial(string requestData, User objUser)
        {
            string materiaID = string.Empty;
            BusinessObject.MaterialManagement.VenMaterial material = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.MaterialManagement.VenMaterial>(requestData);
            materiaID = DataAccess.MaterialManagement.MaterialMasterDL.SaveVenMaterial(material, objUser);
            return materiaID;
        }
        /// <summary>
        /// Saving material details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>string</returns>
        public static string SaveMaterial(string requestData, User objUser, int departementPK)
        {
            //string materiaID = string.Empty;

            //BusinessObject.MaterialManagement.Material material = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.MaterialManagement.Material>(requestData);
            //materiaID = DataAccess.MaterialManagement.MaterialMasterDL.SaveMaterial(material, departementPK);
            //int procId = FillProcessId(objUser.CurrentDeptPK);
            //DoWorkFlow(procId, Convert.ToInt32(materiaID), Convert.ToInt32(objUser.PKUser));
            //return materiaID;
            string storeXxml = GTIService.CommonFunctions.JsonToXml(requestData);

            //Call removeInvalidElement for firefox Bug Fix (removing binary file data from the converted string)
            string nodeToRemove = "prevObject,context,0";
            storeXxml = GTIService.CommonFunctions.removeInvalidElement(storeXxml, nodeToRemove);

            string materiaID = string.Empty;
            BusinessObject.CommonManagement.CommonObject.File fileObject=new CommonObject.File();
            WorkflowCore.CoreObjects.DoWorkFlowRequest objRequest = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
            BusinessObject.MaterialManagement.WorkFlowMaterial objWorkFlowMaterial = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.MaterialManagement.WorkFlowMaterial>(requestData);
            objRequest.ProcessID = objWorkFlowMaterial.MaterialProcessID;
            objRequest.TaskID = objWorkFlowMaterial.MaterialTaskID;
            objRequest.ActionID = objWorkFlowMaterial.MaterialActionID;
            objRequest.ReferenceID = objWorkFlowMaterial.MaterialReferenceID;
            objRequest.ApplicationID = objWorkFlowMaterial.MaterialApplicationID;
            objRequest.UserPK = objUser.PKUser;
            BusinessObject.MaterialManagement.Material material = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.MaterialManagement.Material>(requestData);
            if (material.ITM_SET == 3)
            {
                fileObject= Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.File>(requestData);

                if (fileObject.FILELIST!=null && fileObject.FILELIST.Count > 0)
                {
                   // material.DOC_PK = fileObject.FILELIST[0].DOC_PK;
                    material.DOC_NAME = fileObject.FILELIST[0].DOC_NAME;
                    material.DOC_SEQ_NO = fileObject.FILELIST[0].DOC_SEQ_NO;
                    material.DOC_TITLE = fileObject.FILELIST[0].DOC_TITLE;
                    material.DOC_TYPE = fileObject.FILELIST[0].DOC_TYPE;
                    /////Save File Path
                    string folder = "Material/";
                    string uploadPath = GTIService.Constants.Common.FileUpload.UPLOADURL.Replace("\\","/");
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["DomainPath"].ToLower()))
                    {
                        material.DOC_PATH = "~/"+uploadPath + folder + material.DOC_NAME;
                    }
                    else
                    {
                        material.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["DomainPath"].ToLower() + uploadPath + folder + material.DOC_NAME;
                    }
                    /////
                }
            }
            materiaID = DataAccess.MaterialManagement.MaterialMasterDL.SaveMaterial(material, departementPK, storeXxml);
            //Image Upload
            if (Convert.ToInt32(materiaID) > 0 && material.ITM_SET == 3)
            {
               if(fileObject.FILELIST!=null)
                for (int i = 0; i < fileObject.FILELIST.Count; i++)
                {
                    if (fileObject.FILELIST[i].DOC_PK == 0 && fileObject.FILELIST[i].DOC_TITLE != string.Empty)
                    {
                        CommonManagement.CommonManagement.UploadFromTempFolderForXml(fileObject.FILELIST[i].DOC_NAME, "Material");
                    }
                }
            }
            if (objRequest.ActionID > 0 && Convert.ToInt32(materiaID) > 0)
            {
                objRequest.ApplicationID = Convert.ToInt32(materiaID);
                WorkflowCore.CoreService obj = new WorkflowCore.CoreService();
                int ReferenceID = obj.DoWorkFlow(objRequest);
            }
            return materiaID;
            
        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private static int FillProcessId(int deptPk)
        {
            int procId = 0;
            string path = "/Administration/Masters/MaterialMaster.aspx";
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, deptPk);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                
            }
            return procId;

        }

        public static int DoWorkFlow(int procId, int appId, int user)
        {
          
            try
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtTask = wrkfService.GetInitialTaskAction(procId);
                if (dtTask != null && dtTask.Rows.Count > 0)
                {
                    WorkflowCore.CoreObjects.DoWorkFlowRequest wrkfReq = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
                    wrkfReq.ActionID = int.Parse(dtTask.Rows[0]["Ndtp"].ToString());
                    wrkfReq.ProcessID = procId;
                    wrkfReq.ReferenceID = 0;
                    wrkfReq.ApplicationID = appId;
                    wrkfReq.UserPK = user;
                    wrkfReq.TaskID = int.Parse(dtTask.Rows[0]["Kmsp"].ToString());
                    return wrkfService.DoWorkFlow(wrkfReq);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchBy, string searchValue, int sbuPk)
        {
            DataTable dtSearch = DataAccess.MaterialManagement.MaterialMasterDL.GetSearchValues(searchBy, searchValue, sbuPk);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Material.Fields.MATERIALSEARCHTEXTFIELD, GTIService.Constants.Material.Fields.MATERIALSEARCHVALUEFIELD);         
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="sbuPk"></param>
        /// <returns></returns>
        public static string GetSearchTypeValues(string searchBy, string searchValue,int type, int sbuPk)
        {
            DataTable dtSearch = DataAccess.MaterialManagement.MaterialMasterDL.GetSearchTypeValues(searchBy, searchValue,type, sbuPk);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Material.Fields.MATERIALSEARCHTEXTFIELD, GTIService.Constants.Material.Fields.MATERIALSEARCHVALUEFIELD);           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="type"></param>
        /// <param name="sbuPk"></param>
        /// <returns></returns>
        public static string GetMaterialCodeNameByCategoryAuto(string searchBy, string searchValue, int type, int sbuPk,int catId, int IsWorkOrderItem=0, int BrandPK = 0)
        {
            DataTable dtSearch = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialCodeNameByCategoryAuto(searchBy, searchValue, type, sbuPk,catId, IsWorkOrderItem, BrandPK);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Material.Fields.MATERIALSEARCHTEXTFIELD, GTIService.Constants.Material.Fields.MATERIALSEARCHVALUEFIELD);

        }
        /// <summary>
        /// Returns the search result list for Autocomplete for purchase request
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryPK"></param>
        /// <returns>string</returns>
        public static string GetMaterialNameSearchValues(string searchValue, int categoryPK)
        {
            DataTable dtSearch = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialNameSearchValues(searchValue, categoryPK);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Material.Fields.MATERIALSEARCHTEXTFIELD, GTIService.Constants.Material.Fields.MATERIALSEARCHVALUEFIELD);

        }

        /// <summary>
        /// Returns the search result list for Autocomplete for purchase request
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryPK"></param>
        /// <returns>string</returns>
        public static string GetMaterialSearchValueByCategoryAndStore(string searchValue, int categoryPK, int store, int active=0, int Alternate = 0, int ItemPK = 0, int StockExist = 1)
        {
            DataTable dtSearch = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialSearchValueByCategoryAndStore(searchValue, categoryPK, store, active, Alternate, ItemPK, StockExist);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Material.Fields.MATERIALSEARCHTEXTFIELD, GTIService.Constants.Material.Fields.MATERIALSEARCHVALUEFIELD);

        }
        public static string GetMaterialsPlantToPlant(string searchValue, int categoryPK, int store, int active = 0, int Alternate = 0, int ItemPK = 0, int StockExist = 1)
        {
            DataTable dtSearch = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialsPlantToPlant(searchValue, categoryPK, store, active, Alternate, ItemPK, StockExist);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Material.Fields.MATERIALSEARCHTEXTFIELD, GTIService.Constants.Material.Fields.MATERIALSEARCHVALUEFIELD);
        }
        /// <summary>
        /// Returns the search result list for Autocomplete for purchase request
        /// Show materials based on Qty available (not based on Active)
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryPK"></param>
        /// <returns>string</returns>
        public static string GetMaterialSearchValueByCategoryAndStoreStk(string searchValue, int categoryPK, int store)
        {
            DataTable dtSearch = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialSearchValueByCategoryAndStoreStk(searchValue, categoryPK, store);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Material.Fields.MATERIALSEARCHTEXTFIELD, GTIService.Constants.Material.Fields.MATERIALSEARCHVALUEFIELD);

        }
        /// <summary>
        /// Returns the search result list for Autocomplete for purchase request
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="categoryPK"></param>
        /// <returns>string</returns>
        public static List<AutoCompleteBO> GetMaterialByCategoryAuto(string searchValue, int categoryPK, int store)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
            DataTable dtSearch = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialSearchValueByCategoryAndStore(searchValue, categoryPK, store);
            result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
            {
                Key = row.Field<int>(GTIService.Constants.Material.Fields.MATERIALSEARCHVALUEFIELD),
                Name = row.Field<string>(GTIService.Constants.Material.Fields.MATERIALSEARCHTEXTFIELD)
            }).ToList();
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// Returns material list in json string format
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetMaterialList(GridPrams grid, int bizUnit)
        {
            DataSet dsMaterialList = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialList(grid, bizUnit);
            string jString = string.Empty;
            if (dsMaterialList.Tables.Count > 1 && dsMaterialList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsMaterialList);
            }
            return jString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemPK"></param>
        /// <returns></returns>
        public static string GetOrderItem(int itemPK,int itemType)
        {
            return DataAccess.MaterialManagement.MaterialMasterDL.GetOrderItem(itemPK, itemType);
        }

        public static string GetOrderItemByTypeItemQty(int itemTypePK, int itemPK, decimal qty, int operationPK, int customerPK = 0, int brandPK = 0, int subContractorPK = 0, int WOPK = 0)
        {
            return DataAccess.MaterialManagement.MaterialMasterDL.GetOrderItemByTypeItemQty(itemTypePK, itemPK, qty, operationPK, customerPK, brandPK, subContractorPK, WOPK);
        }        

        /// <summary>
        /// Material Category,  Level base
        /// </summary>
        /// <param name="sbuPk"></param>
        /// <returns></returns>
        public static string GetMaterialCategoryByLevel(int sbuPk, int level)
        {
            DataTable dtStores = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialCategoryByLevel(sbuPk, level);
            return GTIService.CommonFunctions.GetTextValueList(dtStores, GTIService.Constants.Material.Fields.CATEGORYNAME,  GTIService.Constants.Material.Fields.CATEGORYPK);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetMaterialTypeList(GridPrams grid, int type, int bizUnit, int active, int ITMCAT)
        {
            DataSet dsMaterialList = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialTypeList(grid, type, bizUnit, active, ITMCAT);
            string jString = string.Empty;
            if (dsMaterialList.Tables.Count > 1 && dsMaterialList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsMaterialList);
            }
            return jString;
        }
        /// <summary>
        ///  Get WO Mateial List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <param name="ItemCat"></param>
        /// <param name="isWorkOrderItem"></param>
        /// <param name="WOType"></param>
        /// <param name="isMapped"></param>
        /// <returns></returns>
        public static DataSet GetWOMaterialList(GridPrams grid,int bizUnit, int active, int ItemCat = 0, int isWorkOrderItem = 0, int WOType = 1, int isMapped = 0, int subType = 0, int customer = 0, int brand = 0, int PackingCustomer = 0)
        {
            DataSet dsMaterialList = DataAccess.MaterialManagement.MaterialMasterDL.GetWOMaterialList(grid, bizUnit, active, ItemCat, isWorkOrderItem, WOType, isMapped, subType, customer, brand, PackingCustomer);
            return dsMaterialList;
        }
        /// <summary>
        /// Get Mateial List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="type"></param>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <param name="ItemCat"></param>
        /// <param name="isWorkOrderItem"></param>
        /// <returns></returns>
        public static DataSet GetMaterialList(GridPrams grid, int type, int bizUnit, int active, int ItemCat=0,int isWorkOrderItem=0)
        {
            DataSet dsMaterialList = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialTypeList(grid, type, bizUnit, active, ItemCat, isWorkOrderItem);
            return dsMaterialList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemPK"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static string GetMaterialStores(int itemPK, int bizUnit)
        {
            DataTable dtMaterial = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialStores(itemPK, bizUnit);
            string jString = string.Empty;
            if (dtMaterial.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTreeList(dtMaterial, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREEPK, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREENAME, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREEPARENT, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREEHASCHILD, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREEISCHECKED, string.Empty, string.Empty);
            }
            return jString;
        }

        /// <summary>
        /// Returns material list in json string format
        /// </summary>
        /// <summary>
        /// This Function Used To Get all Material  For Dispersion Master
        /// </summary>
        /// <param name=""></param>
        /// <returns>String</returns>
        public static string GetMaterialName()
        {
            DataTable dtMaterialList = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialName();
            string jString = string.Empty;
            if (dtMaterialList.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialList, GTIService.Constants.Material.Fields.MATERIALNAME , GTIService.Constants.Material.Fields.MATERIALPK);
            }
            return jString;
        }
       /// <summary>
       /// Function Used To Get all Material UMO by material pk
       /// </summary>
       /// <summary>
       /// This Function Used To Get all Material UOM For Dispersion Master
       /// </summary>
       /// <param name="materialPK"></param>
       /// <param name="status"></param>
       /// <returns>string</returns>
        public static string GetMaterialUMODtls(int materialPK, int status)
        {
            DataTable dtMaterialUMODtls = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialUMODtls(materialPK, status);
            string jString = string.Empty;
            if (dtMaterialUMODtls.Rows.Count > 0)
            {
                if (status == 0)
                {
                   jString= GTIService.CommonFunctions.GetTextValueList(dtMaterialUMODtls, GTIService.Constants.Material.Fields.MATERIALUMOTEXTFIELD, GTIService.Constants.Material.Fields.MATERIALUMOVALUEFIELD);
                }
                else if (status == 1)
                {
                    jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialUMODtls);
                }
                
            }
            return jString;
        }

        /// <summary>
        /// Function Used To Get all Material UMO by material pk
        /// </summary>
        /// <summary>
        /// This Function Used To Get all Material UOM For Dispersion Master
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="status"></param>
        /// <returns>string</returns>
        public static string GetUOMConvExistsByMaterial(int materialPK)
        {
            DataTable dtMaterialUMODtls = DataAccess.MaterialManagement.MaterialMasterDL.GetUOMConvExistsByMaterial(materialPK);
            string jString = string.Empty;
            if (dtMaterialUMODtls.Rows.Count > 0)
            {

                jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialUMODtls, GTIService.Constants.Material.Fields.UOMNAME, GTIService.Constants.Material.Fields.UOMPK);
            }
            return jString;
        }

       /// <summary>
       ///  Function Used To Get all Material from category ID
       /// </summary>
       /// <param name="materialPK"></param>
       /// <param name="status"></param>
       /// <returns>string</returns>
        public static string GetMaterialByCategory(int CategoryID, int itemID, int sbuPk,string searchValue="")
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialByCategory(CategoryID, itemID, sbuPk, searchValue);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialDtls, GTIService.Constants.Material.Fields.ITMEMCODE, GTIService.Constants.Material.Fields.ITEMPK);
                
            }
            return jString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <param name="itemID"></param>
        /// <param name="sbuPk"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static string GetMaterial(int CategoryID, int itemID, int sbuPk, string searchValue = "")
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialByCategory(CategoryID, itemID, sbuPk, searchValue);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
            }
            return jString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="sbuPk"></param>
        /// <returns></returns>
        public static string GetRelatedMaterial(int itemID, int sbuPk)
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetRelatedMaterial(itemID, sbuPk);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
            }
            return jString;
        }
        public static string GetBOMaterial(int itemID, int sbuPk)
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetBOMaterial(itemID, sbuPk);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
            }
            return jString;
        }
        

        /// <summary>
        ///  Function Used To Get all MaterialName((Code)Name) from category ID
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="status"></param>
        /// <returns>string</returns>
        public static string GetMaterialCodeNameByCategory(int CategoryID, int itemID, int sbuPk, string searchValue = "")
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialCodeNameByCategory(CategoryID, itemID, sbuPk, searchValue);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialDtls, GTIService.Constants.Material.Fields.ITMEMCODENAME, GTIService.Constants.Material.Fields.ITEMPK);

            }
            return jString;
        }

        public static DataTable GetAllMaterial(int sbuPk)
        {
            return DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialByCategory(0, 0, sbuPk);
        }

        /// <summary>
        ///  Function Used To Get all Material from category ID,store
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="status"></param>
        /// <returns>string</returns>
        public static string GetMaterialByCategoryAndStore(int CategoryID, int itemID, int sbuPk, int type, int userPK, int store,int stock,int active,string searchValue="", int IsStoreRequest = 0)
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialByCategoryAndStore(CategoryID, itemID, sbuPk, type, userPK, store, stock,active, searchValue, IsStoreRequest);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialDtls, GTIService.Constants.Material.Fields.ITMEMCODENAME, GTIService.Constants.Material.Fields.ITEMPK);

            }
            return jString;
        }

        public static string GetMaterialByCategoryAndStoreAuto(int CategoryID, int itemID, int sbuPk, int type, int userPK, int store,string searchValue)
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialByCategoryAndStoreAuto(CategoryID, itemID, sbuPk, type, userPK, store, searchValue);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialDtls, GTIService.Constants.Material.Fields.ITMEMCODENAME, GTIService.Constants.Material.Fields.ITEMPK);

            }
            return jString;
        }


        /// <summary>
        ///  Function Used To Get all Material details corresponding to a material pk
        /// </summary>
        /// <param name="materialPK"></param>
        /// <returns>string</returns>
        public static string GetMaterialDetails(int itemID, int sbuPk, int dept = 0, int active = 0, int vendorPK = 0)
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialDetails(itemID, sbuPk, dept,active, vendorPK);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
            }
            return jString;
        }
        public static string GetBrandDetails(int brandPK)
        {
            DataTable dtBrandDetails = DataAccess.SaleOrder.CustomerProductDL.GetBrandDetails(brandPK);
            string jString = string.Empty;
            if (dtBrandDetails.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtBrandDetails);
            }
            return jString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static string GetPakingMaterialDetails(int itemID)
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetPakingMaterialDetails(itemID);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
            }
            return jString;
        }

        /// <summary>
        /// Function Used To Get Material Description
        /// </summary>
        /// <param name="materialID"></param>
        /// <param name="sbuPk"></param>
        /// <returns></returns>
        public static string GetMaterialDescription(int materialID, int departmentID,int toUOM=0)
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialDescription(materialID, departmentID, toUOM);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
            }
            return jString;
        }
             /// <summary>
        ///  Function Used To Get all Material details corresponding to a material pk 
        /// </summary>
        /// <param name="materialPK"></param>
        /// <returns>string</returns>
        public static string GetMaterialDetailsForStore(int itemID, int sbuPk, DateTime? date=null)
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetCurrentStockForStore(itemID, sbuPk, date);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
            }
            return jString;
        }


        /// <summary>
        ///  Function Used To Get all Stores CurrentStock corresponding to a material pk 
        /// </summary>
        /// <param name="materialPK"></param>
        /// <returns>string</returns>
        public static string GetBreakupForStoreStock(int itemID, int deptType, int deptCat, DateTime? date, int toUOM = 0)
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetBreakupForStoreCurrStock(itemID, deptType, deptCat, date, toUOM);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
            }
            return jString;
        }



        /// <summary>
        /// Get current stock for store by matrial id
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="store"></param>
        /// <returns></returns>
        public static string GetCurrentStockForStore(int itemID, int store)
        {
            DataTable dtCurrentStock = DataAccess.MaterialManagement.MaterialMasterDL.GetCurrentStockForStore(itemID, store);
            string jString = string.Empty;
            if (dtCurrentStock.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtCurrentStock);
            }
            return jString;
        }
        /// <summary>
        /// Delete Material Details
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns>String</returns>
        public static string DeleteMaterial(int materialID)
        {
            return MaterialMasterDL.DeleteMaterialDtls(materialID).ToString();
        }
        /// <summary>
        /// Get Item Name Details
        /// </summary>
        /// <param name="materialID"></param>
        /// <returns>string</returns>
        public static string GetItemName(int materialID)
        {
            return MaterialMasterDL.GetItemName(materialID);
        }
        /// <summary>
        /// Get UOM Details by material PK
        /// </summary>
        /// <param name="materialPK"></param>
        /// <returns></returns>
        public static string GetUomDtlsByMaterialPk(int materialPK)
        {
            DataTable dtSearch = DataAccess.MaterialManagement.MaterialMasterDL.GetUomDtlsByMaterialPk(materialPK);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, "UOM_NAME", "UOM_PK");

        }


        /// <summary>
        /// Methord used to get the material Uom Conversion factor by passing the material and New UOM
        /// </summary>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>Po Creation</for>
        /// <Used In>Finding uom Conversion when adding material</Used>
        /// <param name="materialPK"></param>
        /// <param name="uom"></param>
        public static string GetMaterialUOMConversion(int materialPK, int uom)
        {
            DataTable dtUomConversion = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialUOMConversion(materialPK, uom);
            string jString = string.Empty;
            if (dtUomConversion.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtUomConversion);
            }
            return jString;
        }

        /// <summary>
        /// Function used to get Material Corresponding to a department PK
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public static string GetDepartmentMaterials(int storeID, string searchVal)
        {
            DataTable dtMaterials = DataAccess.MaterialManagement.MaterialMasterDL.GetDepartmentMaterials(storeID,searchVal);
            string jString = string.Empty;
            if (dtMaterials.Rows.Count > 0)
            {
                //jString = GTIService.CommonFunctions.GetTextValueList(dtMaterials, "ITM_CODE", "IDM_ITEM");
                jString = GTIService.CommonFunctions.GetTextValueList(dtMaterials, "ITM_TEXT", "IDM_ITEM");
            }
            return jString;
        }

        /// <summary>
        /// Function used to get Material Corresponding to a department PK
        /// </summary>
        /// <param name="DepartmentID"></param>
        /// <returns></returns>
        public static string GetDepartmentCategoryMaterial(int storePK, int categoryPK, string searchVal,int bizUnit)
        {
            DataTable dtMaterials = DataAccess.MaterialManagement.MaterialMasterDL.GetDepartmentCategoryMaterial(storePK, categoryPK, searchVal, bizUnit);
            string jString = string.Empty;
            if (dtMaterials.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtMaterials, "VALUE", "PK");
            }
            return jString;
        }

        /// <summary>
        ///  Function Used To Get all Material details corresponding to a material pk
        /// </summary>
        /// <param name="materialPK"></param>
        /// <returns>string</returns>
        public static string GetInActiveMaterialDetails(int itemID, int sbuPk, int status)
        {
            DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetInActiveMaterialDetails(itemID, sbuPk, status);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
            }
            return jString;
        }

        //NewMaterial start
        /// <summary>
        /// Get Order Details As a XML Format and Convert in to JSON and Return As a JSON String Format
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static string GetRateHistory(int itemPK,int vendorPK)
        {
            try
            {
                DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetRateHistory(itemPK, vendorPK);
                string jString = string.Empty;
                if (dtMaterialDtls.Rows.Count > 0)
                {
                    jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
                }
                return jString;
                //return GTIService.CommonFunctions.XmlToJson(DataAccess.MaterialManagement.MaterialMasterDL.GetRateHistory(itemPK,vendorPK));
               
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Rate History");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }
        //New End

        /// <summary>
        /// Get Item Rates
        /// </summary>
        /// <param name="itemPK"></param>
        /// <param name="vendorPK"></param>
        /// <returns></returns>
        public static string GetItemRates(int itemPK, int vendorPK, int toUOMPK = 0)
        {
            try
            {
                DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetItemRates(itemPK, vendorPK, toUOMPK);
                string jString = string.Empty;
                if (dtMaterialDtls.Rows.Count > 0)
                {
                    jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
                }
                return jString;
                //return GTIService.CommonFunctions.XmlToJson(DataAccess.MaterialManagement.MaterialMasterDL.GetRateHistory(itemPK,vendorPK));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Rate History");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }
        #endregion

        public static string GetBatchNo(int itemPK, int deptPK, int batchPK, DateTime? date = null, int? IsShowZeroQtyBatches = 0, DateTime? transDate = null, int TestResult=0)
        {
            try
            {
                DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetBatchNo(itemPK, deptPK, batchPK, date, IsShowZeroQtyBatches, transDate, TestResult);
                string jString = string.Empty;
                if (dtMaterialDtls.Rows.Count > 0)
                {
                    jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialDtls, GTIService.Constants.Material.Fields.BATCHNO, GTIService.Constants.Material.Fields.STOCKBATCH);
                }
                return jString;  
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Rate History");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }

        public static string GetBatchNo_Consumption(int itemPK, int deptPK, int batchPK, DateTime? date = null, int? IsShowZeroQtyBatches = 0, DateTime? transDate = null, int? cdhPk = 0)
        {
            try
            {
                DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetBatchNo_Consumption(itemPK, deptPK, batchPK, date, IsShowZeroQtyBatches, transDate, cdhPk);
                string jString = string.Empty;
                if (dtMaterialDtls.Rows.Count > 0)
                {
                    jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialDtls, GTIService.Constants.Material.Fields.BATCHNO, GTIService.Constants.Material.Fields.STOCKBATCH);
                }
                else
                    jString = "{}";
                return jString;
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Rate History");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }

        public static string GetBatchDetails(int batchPK, int GrnBatchConfg)
        {
            try
            {
                DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetBatchDetails(batchPK, GrnBatchConfg);
                string jString = string.Empty;
                if (dtMaterialDtls.Rows.Count > 0)
                {
                    jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
                }
                return jString;
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Rate History");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get Batch Details For Dispersion
        /// </summary>
        /// <returns></returns>
        public static string GetBatchDetailsDispersion(int batchPK,int active,int bizunit)
        {
            try
            {
                DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetBatchDetailsDispersion(batchPK,active,bizunit);
                string jString = string.Empty;
                if (dtMaterialDtls.Rows.Count > 0)
                {
                    jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
                }
                return jString;
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Rate History");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }

        public static string CheckItemCodeExist(int ItemCode)
        {
            try
            {
                DataTable dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.CheckItemCodeExist(ItemCode);
                string jString = string.Empty;
                if (dtMaterialDtls.Rows.Count > 0)
                {
                    jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterialDtls);
                }
                return jString;
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Rate History");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetMaterialTypes(GridPrams grid, int type, int bizUnit)
        {
            DataSet dsMaterialList = DataAccess.MaterialManagement.MaterialMasterDL.GetMaterialTypeList(grid, type, bizUnit, (int)DbActiveStatus.ACTIVE);
            return dsMaterialList;
        }

        /// <summary>
        ///  Function Used To Get GST Classification list for DDL
        /// </summary>
        /// <param name="materialPK"></param>
        /// <returns>string</returns>
        public static string GetGSTClassificationList(int Pk, int sbuPk, int status)
        {
            DataTable dtList = DataAccess.MaterialManagement.MaterialMasterDL.GetGSTClassificationList(Pk, sbuPk, status);
            string jString = string.Empty;
            if (dtList.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtList, GTIService.Constants.Material.Fields.GCMCODE, GTIService.Constants.Material.Fields.GCMPK);
            }
            return jString;
        }
        /// <summary>
        /// Saving material details From PR
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns>string</returns>
        public static string SaveMaterialFromPR(string requestData, User objUser, int departementPK)
        {

            string storeXxml = GTIService.CommonFunctions.JsonToXml(requestData);

            //Call removeInvalidElement for firefox Bug Fix (removing binary file data from the converted string)
            string nodeToRemove = "prevObject,context,0";
            storeXxml = GTIService.CommonFunctions.removeInvalidElement(storeXxml, nodeToRemove);

            string materiaID = string.Empty;
            BusinessObject.CommonManagement.CommonObject.File fileObject = new CommonObject.File();
            WorkflowCore.CoreObjects.DoWorkFlowRequest objRequest = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
            BusinessObject.MaterialManagement.WorkFlowMaterial objWorkFlowMaterial = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.MaterialManagement.WorkFlowMaterial>(requestData);
            objRequest.ProcessID = objWorkFlowMaterial.MaterialProcessID;
            objRequest.TaskID = objWorkFlowMaterial.MaterialTaskID;
            objRequest.ActionID = objWorkFlowMaterial.MaterialActionID;
            objRequest.ReferenceID = objWorkFlowMaterial.MaterialReferenceID;
            objRequest.ApplicationID = objWorkFlowMaterial.MaterialApplicationID;
            objRequest.UserPK = objUser.PKUser;
            BusinessObject.MaterialManagement.MaterialBO material = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.MaterialManagement.MaterialBO>(requestData);

            materiaID = DataAccess.MaterialManagement.MaterialMasterDL.SaveMaterialFromPR(material, departementPK, storeXxml);
            if (objRequest.ActionID > 0 && Convert.ToInt32(materiaID) > 0)
            {
                objRequest.ApplicationID = Convert.ToInt32(materiaID);
                WorkflowCore.CoreService obj = new WorkflowCore.CoreService();
                int ReferenceID = obj.DoWorkFlow(objRequest);
            }
            return materiaID;

        }
        #region PR Trading
        /// <summary>
        /// Function Used To Get all Material UMO by material pk
        /// </summary>
        /// <summary>
        /// This Function Used To Get all Material UOM For Dispersion Master
        /// </summary>
        /// <param name="materialPK"></param>
        /// <param name="status"></param>
        /// <returns>string</returns>
        public static string GetItemUOMTrading(int materialPK)
        {
            DataTable dtMaterialUMODtls = DataAccess.MaterialManagement.MaterialMasterDL.GetItemUOMTrading(materialPK);
            string jString = string.Empty;
            if (dtMaterialUMODtls.Rows.Count > 0)
            {

                jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialUMODtls, GTIService.Constants.Material.Fields.UOM_CODE, GTIService.Constants.Material.Fields.UOMPK);
            }
            return jString;
        }
        #endregion


        #region External Material issue Multiple
        public static DataTable GetBatchNoAuto(int itemPK, int deptPK, int batchPK, DateTime? date = null, int? IsShowZeroQtyBatches = 0, DateTime? transDate = null, int? cdhPk = 0)
        {
            return DataAccess.MaterialManagement.MaterialMasterDL.GetBatchNo_Consumption(itemPK, deptPK, batchPK, date, IsShowZeroQtyBatches, transDate, cdhPk);
        }
        public static System.Data.DataSet GetEMIMultipleList(GridPrams grid,string pageUrl,string fromDate, string toDate, int trnStatus, int issueStore, int itmCatPK, int itmPk, string issueNo, int issueType, int issueTo, int itemName, int bizUnit, int userPK, int pageIndex, int pageSize)
        {
            return DataAccess.MaterialManagement.MaterialMasterDL.GetEMIMultipleList(grid, pageUrl,fromDate, toDate, trnStatus, issueStore, itmCatPK, itmPk, issueNo, issueType, issueTo, itemName, bizUnit, userPK, pageIndex, pageSize);
        }
        #endregion
    }
}
