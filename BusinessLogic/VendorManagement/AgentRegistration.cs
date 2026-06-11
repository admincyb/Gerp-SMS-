using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BusinessObject;
using System.Data;
using System.IO;
using DataAccess.VendorManagement;

namespace BusinessLogic.VendorManagement
{
   public class AgentRegistration
    {
        /// <summary>
        /// Save Agent Details - Both Header And Details
        /// </summary>
        /// <param name="orderMaster"></param>
        /// <returns>OrderPk / Exception Value</returns>
        public static string RegisterAgent(string agentDetails, User objUser)
        {
            string AgentID = string.Empty;
            string requisitionID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(agentDetails);
            AgentID = DataAccess.VendorManagement.AgentRegistrationDL.SaveAgentDetails(xmlstr).ToString();
            //used to update the files to Permanenet location
            BusinessObject.CommonManagement.CommonObject.File fileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.File>(agentDetails);
            for (int i = 0; i < fileObject.FILELIST.Count; i++)
            {
                if (fileObject.FILELIST[i].DOC_PK == 0)
                {
                    if (fileObject.FILELIST[i].DOC_TITLE != string.Empty)
                    {
                        if (CommonManagement.CommonManagement.UploadFromTempFolderForXml(fileObject.FILELIST[i].DOC_NAME, "Vendor"))
                        {

                        }
                    }
                }
            }

            return AgentID;
        }

        /// <summary>
        /// Save Agent Bank
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="objUser"></param>
        /// <returns>string</returns>
        public static string SaveAgentBank(string requestData, User objUser)
        {
            BusinessObject.VendorManagement.BankDetails bank = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.VendorManagement.BankDetails>(requestData);
            bank.VBD_ACTIVE = 1;
            bank.VBD_MOD_BY = objUser.PKUser;
            requestData = ObjectToJsonString(bank);
            string bankID = string.Empty;
            string xmlstr = GTIService.CommonFunctions.JsonToXml(requestData);           
            bankID = DataAccess.VendorManagement.AgentRegistrationDL.SaveAgentBank(xmlstr).ToString();
            return bankID;
        }

        public static string ObjectToJsonString(object obj)
        {
            var javaScriptSerializer = new
            System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(obj);
            return jsonString;
        }

        /// <summary>
        /// Get Agent Details As a XML Format and Convert in to JSON and Return As a JSON String Format
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public static string GetAgentDetails(int AgentID)
        {
            try
            {

                return GTIService.CommonFunctions.XmlToJson(DataAccess.VendorManagement.AgentRegistrationDL.GetAgentDetails(AgentID));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// get vendor role Details
        /// </summary>
        /// <param name="deptPK"></param>
        /// <param name="deptParentPK"></param>
        /// <param name="bizUnit"></param>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public static string GetUserRole(int venPK, int bizUnit)
        {

            DataTable dtMaterial = DataAccess.VendorManagement.VendorRegistrationDL.GetVendorRole(venPK, bizUnit);
            string jString = string.Empty;
            if (dtMaterial.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTreeList(dtMaterial, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREEPK, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREENAME, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREEPARENT, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREEHASCHILD, GTIService.Constants.Administration.Masters.StoreMaterialMapping.Fields.TREEISCHECKED, string.Empty, "IS_ITEM");
            }
            return jString;
        }

        /// <summary>
        /// Save Tax Discout for vendor Material
        /// </summary>
        /// <param name="xmlTaxDiscount"></param>
        /// <returns></returns>
        public static string TaxDiscountApply(string xmlTaxDiscount)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(xmlTaxDiscount);
            string taxID = DataAccess.VendorManagement.VendorRegistrationDL.TaxDiscountApply(xmlstr).ToString();
            return taxID;
        }


        /// <summary>
        /// Returns the Agent list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetAgentDetails(GridPrams grid, int sbuID, int procId, string PageUrl, int venRole)
        {
            DataSet dsvendorList = DataAccess.VendorManagement.AgentRegistrationDL.GetAgentDetails(grid, sbuID, procId, PageUrl,venRole);
            string jString = string.Empty;
            if (dsvendorList.Tables.Count > 1 && dsvendorList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsvendorList);
            }
            return jString;
        }

        ///// <summary>
        ///// Get Vendor Materials
        ///// </summary>
        ///// <param name="grid"></param>
        ///// <param name="sbuID"></param>
        ///// <param name="procId"></param>
        ///// <param name="PageUrl"></param>
        ///// <returns></returns>
        //public static string GetVndMaterials(GridPrams grid, int AgentID)
        //{
        //    DataSet dsvendorList = DataAccess.VendorManagement.VendorRegistrationDL.GetVndMaterials(grid, AgentID);
        //    string jString = string.Empty;
        //    if (dsvendorList.Tables.Count > 1 && dsvendorList.Tables[1].Rows.Count > 0)
        //    {
        //        jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsvendorList);
        //    }
        //    return jString;
        //}

        ///// <summary>
        ///// Returns the search result list for Autocomplete 
        ///// </summary>
        ///// <param name="searchBy"></param>
        ///// <param name="searchValue"></param>
        ///// <returns>string</returns>
        //public static string GetSearchValues(string searchBy, string searchValue, User objuser, int procId)
        //{
        //    DataTable dtSearch = DataAccess.VendorManagement.VendorRegistrationDL.GetSearchValues(searchBy, searchValue, objuser, procId);
        //    return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);

        //}

        /// <summary>
        /// Logic Methord used to delete a vendor details by passing vendor ID
        /// </summary>
        /// <param name="AgentID"></param>
        /// <returns>string</returns>
        public static string DeleteAgentDetails(int AgentID,DateTime? LastModDate= null)
        {
            return DataAccess.VendorManagement.AgentRegistrationDL.DeleteAgentDetails(AgentID,LastModDate).ToString();
        }
        ///// <summary>
        ///// Delete Vendor material
        ///// </summary>
        ///// <param name="materialID"></param>
        ///// <returns></returns>
        //public static string DeleteVndMaterial(int materialID)
        //{
        //    return DataAccess.VendorManagement.VendorRegistrationDL.DeleteVndMaterial(materialID).ToString();
        //}

        ///// <summary>
        ///// To get vendor Deatils corresponding to vendor
        ///// </summary>
        ///// <param name="AgentID"></param>
        ///// <returns></returns>
        //public static string GetVendorDtls(int AgentID)
        //{
        //    DataTable dtVendorsDtls = DataAccess.VendorManagement.VendorRegistrationDL.GetVendorDtls(AgentID);
        //    string jString = string.Empty;
        //    if (dtVendorsDtls.Rows.Count > 0)
        //    {
        //        jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtVendorsDtls);
        //    }
        //    return jString;
        //}

        ///// <summary>
        ///// To get vendor Deatils corresponding to vendor
        ///// </summary>
        ///// <param name="AgentID"></param>
        ///// <returns></returns>
        //public static DataTable GetVendorData(int AgentID)
        //{
        //    return DataAccess.VendorManagement.VendorRegistrationDL.GetVendorDtls(AgentID);
        //}

        ///// <summary>
        ///// Get Vendor mapped Material list
        ///// </summary>
        ///// <Created By>Vineeth</Created>
        ///// <For>PO Filling Material Corresponding to Vendor selected</For>
        ///// <Used In>Pomaterial Filling </Used>
        ///// <param name="AgentID"></param>
        ///// <returns>string</returns>
        //public static string GetVendorMaterials(int AgentID)
        //{
        //    DataTable dtSuppliedMaterial = DataAccess.VendorManagement.VendorRegistrationDL.GetVendorMaterials(AgentID);
        //    string jString = GTIService.CommonFunctions.GetTextValueList(dtSuppliedMaterial, GTIService.Constants.Vendor.Fields.SUPPLIEDMATERIALNAME, GTIService.Constants.Vendor.Fields.SUPPLIEDMATERIALPK);
        //    return jString;

        //}
        ///// <summary>
        ///// Get Vendor store mapped Material list
        ///// </summary>
        ///// <param name="AgentID"></param>
        ///// <param name="storeID"></param>
        ///// <returns></returns>
        //public static string GetVendorStoreMaterials(int AgentID, int storeID, int userPK, string searchValue = "")
        //{
        //    DataTable dtSuppliedMaterial = DataAccess.VendorManagement.VendorRegistrationDL.GetVendorStoreMaterials(AgentID, storeID, userPK, searchValue);
        //    string jString = GTIService.CommonFunctions.GetTextValueList(dtSuppliedMaterial, GTIService.Constants.Vendor.Fields.SUPPLIEDMATERIALNAME, GTIService.Constants.Vendor.Fields.SUPPLIEDMATERIALPK);
        //    return jString;

        //}

        ///// <summary>
        ///// Get Vendor mapped Material list
        ///// </summary>
        ///// <Created By>Vineeth</Created>
        ///// <For>PO Filling Material Corresponding to Vendor selected</For>
        ///// <Used In>Pomaterial Filling </Used>
        ///// <param name="AgentID"></param>
        ///// <returns>string</returns>
        //public static string GetVendorMaterialsCode(int AgentID)
        //{
        //    DataTable dtSuppliedMaterial = DataAccess.VendorManagement.VendorRegistrationDL.GetVendorMaterials(AgentID);
        //    string jString = GTIService.CommonFunctions.GetTextValueList(dtSuppliedMaterial, GTIService.Constants.Vendor.Fields.SUPPLIEDMATERIALCODE, GTIService.Constants.Vendor.Fields.SUPPLIEDMATERIALPK);
        //    return jString;

        //}


        ///// <summary>
        ///// Get Material details for a corresponding vendor
        ///// </summary>
        ///// <Created By>Vineeth</Created>
        ///// <For>PO Filling Material Corresponding to Vendor selected</For>
        ///// <Used In>Po material details Filling </Used>
        ///// <param name="AgentID"></param>
        ///// <returns>string</returns>
        //public static string GetVendorMaterialDetails(int AgentID, int materialID)
        //{
        //    DataTable dtMaterial = DataAccess.VendorManagement.VendorRegistrationDL.GetVendorMaterialDetails(AgentID, materialID);
        //    return Newtonsoft.Json.JsonConvert.SerializeObject(dtMaterial);

        //}

        ///// <summary>
        ///// Get Material details for a corresponding vendor
        ///// </summary>
        ///// <Created By>Vineeth</Created>
        ///// <For>PO Filling Material Corresponding to Vendor selected</For>
        ///// <Used In>Po material details Filling </Used>
        ///// <param name="AgentID"></param>
        ///// <returns>string</returns>
        //public static string GetMaterialUOM(int AgentID, int materialID)
        //{
        //    DataTable dtMaterialUOM = DataAccess.VendorManagement.VendorRegistrationDL.GetMaterialUOM(AgentID, materialID);
        //    string jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialUOM, GTIService.Constants.UOM.Fields.UOMCODE, GTIService.Constants.UOM.Fields.UOMPK);
        //    return jString;

        //}

        ///// <summary>
        ///// Get Order Details As a XML Format and Convert it into dataset
        ///// </summary>
        ///// <param name="orderID"></param>
        ///// <returns></returns>
        //public static DataSet GetVendorDetailsReport(int AgentID)
        //{
        //    DataSet dsVendor = new DataSet();
        //    string xml = DataAccess.VendorManagement.VendorRegistrationDL.GetVendorDetailsReport(AgentID);
        //    dsVendor.ReadXml(new XmlTextReader(new StringReader(xml)));
        //    return dsVendor;
        //}

        //public static DataSet GetVendorPerformanceReport(int vendorPK)
        //{
        //    DataSet dsVendor = new DataSet();
        //    return DataAccess.VendorManagement.VendorRegistrationDL.GetVendorPerformanceReport(vendorPK);
        //    //dsVendor.ReadXml(new XmlTextReader(new StringReader(xml)));
        //    //return dsVendor;
        //}
        //public static DataSet GetAppSubType(string appCode)
        //{
        //    DataSet dsVendor = new DataSet();
        //    return DataAccess.VendorManagement.VendorRegistrationDL.GetAppSubType(appCode);
        //    //dsVendor.ReadXml(new XmlTextReader(new StringReader(xml)));
        //    //return dsVendor;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="searchBy"></param>
        ///// <param name="searchValue"></param>
        ///// <param name="objUser"></param>
        ///// <returns></returns>
        //public static string GetVendorPOType(BusinessObject.User objUser, string CfgType, int Active)
        //{
        //    DataTable dtSearch = VendorRegistrationDL.GetVendorPOType(objUser, CfgType, Active);
        //    return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.CFGVALUE, GTIService.Constants.Designation.Fields.CFGPK);
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name=""></param>
        ///// <param name=""></param>
        ///// <param name=""></param>
        ///// <returns></returns>
        //public static string GetAccountTypesBank()
        //{
        //    //todo: Shihab Account Type Binding
        //    DataTable dtAccountTypes = VendorRegistrationDL.GetAccountTypesBank();
        //    string jString = string.Empty;
        //    if (dtAccountTypes.Rows.Count > 0)
        //    {
        //        jString = GTIService.CommonFunctions.GetTextValueList(dtAccountTypes, "CON_NAME", "CON_PK");
        //    }
        //    return jString;
        //}

        /// <summary>
        /// Delete Vendor Bank
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public static string DeleteAgentBank(int bankId)
        {
            return DataAccess.VendorManagement.AgentRegistrationDL.DeleteAgentBank(bankId).ToString();
        }

        /// <summary>
        /// Get Vendor banks
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="VendorbankPk"></param>
        /// <param name="vendorPk"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataTable GetAgentBanks(User objUser, int VendorbankPk, int VendorPk, short Active)
        {
            DataTable dtVendorBanks = DataAccess.VendorManagement.AgentRegistrationDL.GetAgentBanks(objUser, VendorbankPk, VendorPk, Active);
            return dtVendorBanks;
        }
    }
}
