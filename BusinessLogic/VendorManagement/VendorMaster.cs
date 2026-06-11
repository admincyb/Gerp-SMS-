using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.VendorManagement
{
    public class VendorMaster
    {
        #region Methods for Machinery Vendor Management
        /// <summary>
        /// Get Vendor Details - For Listing
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetVendorDtls(GridPrams grid)
        {
            DataSet dsVendorList = DataAccess.VendorManagement.VendorMasterDL.GetVendorDtls(grid);
            string jString = string.Empty;
            if (dsVendorList.Tables.Count > 1 && dsVendorList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsVendorList);
            }
            return jString;
        }
        /// <summary>
        /// Delete Vendor Details By Vendor ID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>string</returns>
        public static string DeleteVendorDtls(int vendorID)
        {
            return DataAccess.VendorManagement.VendorMasterDL.DeleteVendorDtls(vendorID).ToString();
        }
        /// <summary>
        ///  Save Vendor Details 
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="userPk"></param>
        /// <returns>string</returns>
        public static string SaveVendor(string requestData, int userPk)
        {
            BusinessObject.MachineryManagement.Machinery.Vendor vendor = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.MachineryManagement.Machinery.Vendor>(requestData);
            vendor.UserPK = userPk;
            return DataAccess.VendorManagement.VendorMasterDL.SaveVendor(vendor).ToString(); ;
        }
        /// <summary>
        /// Get Vendor Details By VendorID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>string</returns>
        public static string GetVendor()
        {
            DataTable dtSearch = DataAccess.VendorManagement.VendorMasterDL.GetVendor();
            string jString = GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Machinery.Fields.VENDORNAM, GTIService.Constants.Machinery.Fields.VENDORPK);
            return jString;
        }
        #endregion

        #region Methods For Vendor Management

        /// <summary>
        /// To get vendors for listing
        /// </summary>
        /// <returns></returns>
        public static string GetVendors(int BizUnitPk)
        {
            DataTable dtVendors = DataAccess.VendorManagement.VendorMasterDL.GetVendors(BizUnitPk);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtVendors, GTIService.Constants.Vendor.Fields.VENDORNAME, GTIService.Constants.Vendor.Fields.VENDORPK);
            return jString;
        }
        /// <summary>
        /// To get Active vendors for listing
        /// </summary>
        /// <returns></returns>
        public static string GetActiveVendors(int BizUnitPk)
        {
            DataTable dtVendors = DataAccess.VendorManagement.VendorMasterDL.GetActiveVendors(BizUnitPk);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtVendors, GTIService.Constants.Vendor.Fields.VENDORNAME, GTIService.Constants.Vendor.Fields.VENDORPK);
            return jString;
        }

        /// <summary>
        /// To get Active vendors(Dealer & PM Manufacturer only) for listing
        /// </summary>
        /// <returns></returns>
        public static string GetActiveVendorsOnly(int BizUnitPk)
        {
            DataTable dtVendors = DataAccess.VendorManagement.VendorMasterDL.GetActiveVendorsOnly(BizUnitPk);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtVendors, GTIService.Constants.Vendor.Fields.VENDORNAME, GTIService.Constants.Vendor.Fields.VENDORPK);
            return jString;
        }


        /// <summary>
        /// Get vendors based on type
        /// </summary>
        /// <param name="BizUnitPk"></param>
        /// <returns></returns>
        public static string GetTypeVendors(int BizUnitPk, int type)
        {
            DataTable dtVendors = DataAccess.VendorManagement.VendorMasterDL.GetTypeVendors(BizUnitPk, type);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtVendors, GTIService.Constants.Vendor.Fields.VENDORNAME, GTIService.Constants.Vendor.Fields.VENDORPK);
            return jString;
        }
        /// <summary>
        /// Get All Active vendors based on type
        /// </summary>
        /// <param name="BizUnitPk"></param>
        /// <returns></returns>
        public static string GetTypeVendorsActive(int BizUnitPk, int type,int VenPK)
        {
            DataTable dtVendors = DataAccess.VendorManagement.VendorMasterDL.GetTypeVendorsActive(BizUnitPk, type,VenPK);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtVendors, GTIService.Constants.Vendor.Fields.VENDORNAME, GTIService.Constants.Vendor.Fields.VENDORPK);
            return jString;
        }

        public static DataTable GetVendorforReport(int BizUnitPk, int? vendertype = null, int? active = null)
        {
            return DataAccess.VendorManagement.VendorMasterDL.GetVendors(BizUnitPk, vendertype,active);
        }
        /// <summary>
        /// To get vendor address
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns></returns>
        public static string GetVendorAddress(int vendorID)
        {
            DataTable dtVendorAddress = DataAccess.VendorManagement.VendorMasterDL.GetVendorAddress(vendorID);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtVendorAddress, GTIService.Constants.Vendor.Fields.VNDCODE, GTIService.Constants.Vendor.Fields.VNDADDRESS);
            return jString;
        }





        /// <summary>
        /// Get Country For Auto Complete
        /// </summary>
        /// <returns></returns>
        public static string GetCountry(string searchValue)
        {
            DataTable dtCountry = DataAccess.VendorManagement.VendorMasterDL.GetCountry(searchValue);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtCountry, GTIService.Constants.Vendor.Fields.VND_SearchFields.SEARCHVAL, GTIService.Constants.Vendor.Fields.VND_SearchFields.SEARCHID);
            return jString;
        }
        /// <summary>
        /// Get States For Auto Complete
        /// </summary>
        /// <returns></returns>
        public static string GetStates(string searchValue)
        {
            DataTable dtStates = DataAccess.VendorManagement.VendorMasterDL.GetStates(searchValue);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtStates, GTIService.Constants.Vendor.Fields.VND_SearchFields.SEARCHVAL, GTIService.Constants.Vendor.Fields.VND_SearchFields.SEARCHID);
            return jString;
        }
        /// <summary>
        /// Get Values For Auto Complete
        /// </summary>
        /// <returns></returns>
        public static string GetSearchVals(string searchBy, string searchValue)
        {
            DataTable dtSearch = DataAccess.VendorManagement.VendorMasterDL.GetSearchVals(searchBy, searchValue);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Vendor.Fields.VND_SearchFields.SEARCHVAL, GTIService.Constants.Vendor.Fields.VND_SearchFields.SEARCHID);
            return jString;
        }
        /// <summary>
        /// Get States For Auto Complete
        /// </summary>
        /// <returns></returns>
        public static string GetCurrency()
        {
            DataTable dtStates = DataAccess.VendorManagement.VendorMasterDL.GetCurrency();
            string jString = GTIService.CommonFunctions.GetTextValueList(dtStates, GTIService.Constants.Vendor.Fields.CURRENCY, GTIService.Constants.Vendor.Fields.CURRENCYPK);
            return jString;
        }
        /// <summary>
        /// Save vendor details
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static string SaveVendorDetails(string requestData)
        {
            BusinessObject.VendorManagement.Vendor vendor = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.VendorManagement.Vendor>(requestData);
            return DataAccess.VendorManagement.VendorMasterDL.SaveVendorDetails(vendor).ToString(); ;
        }
        /// <summary>
        /// Get Vendor Details - For Listing
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetVendorsList(GridPrams grid)
        {
            DataSet dsVendorsList = DataAccess.VendorManagement.VendorMasterDL.GetVendorsList(grid);
            string jString = string.Empty;
            if (dsVendorsList.Tables.Count > 1 && dsVendorsList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsVendorsList);
            }
            return jString;
        }
        /// <summary>
        /// Delete Vendor Details By Vendor ID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>string</returns>
        public static string DeleteVendorDetails(int vendorID)
        {
            return DataAccess.VendorManagement.VendorMasterDL.DeleteVendorDetails(vendorID).ToString();
        }

        /// <summary>
        /// Methode used to get the purhase order vendor 
        /// </summary>
        /// <param name="bizUnitPk"></param>
        /// <returns></returns>
        public static string GetPurchaseOrderVendors(int bizUnitPk, int grhPK, int shpDeptPK = 0)
        {
            DataTable dtVendors = DataAccess.VendorManagement.VendorMasterDL.GetPurchaseOrderVendors(bizUnitPk, grhPK, shpDeptPK);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtVendors, GTIService.Constants.Vendor.Fields.VENDORNAME, GTIService.Constants.Vendor.Fields.VENDORPK);
            return jString;
        }


        //////////////////////////////////////////////////////////////////////////////New
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetAddressTypeList(int bizUnitPK)
        {
            DataTable dtAddressTypeList = DataAccess.VendorManagement.VendorMasterDL.GetAddressTypeList(bizUnitPK);
            //convert to json string
            return GTIService.CommonFunctions.GetTextValueList(dtAddressTypeList, GTIService.Constants.Vendor.Fields.CONFIGTEXT, GTIService.Constants.Vendor.Fields.CONFIGVALUE);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable GetVendorAddressTypeList(int bizUnitPK)
        {
            DataTable dtAddressTypeList = DataAccess.VendorManagement.VendorMasterDL.GetAddressTypeList(bizUnitPK);
            return dtAddressTypeList;
        }

        /// <summary>
        /// Get Vendor Item Tax details
        /// </summary>
        /// <param name="itemPK"></param>
        /// <returns></returns>
        public static string GetVendorItemTaxDiscount(int itemPK, int category)
        {
            try
            {

                return GTIService.CommonFunctions.XmlToJson(DataAccess.VendorManagement.VendorRegistrationDL.GetVendorItemTaxDiscount(itemPK, category));

            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Save Vendor Bank
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="objUser"></param>
        /// <returns>string</returns>
        public static string SaveVendorBank(string requestData, User objUser)
        {
            BusinessObject.VendorManagement.BankDetails bank = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.VendorManagement.BankDetails>(requestData);
            bank.VBD_ACTIVE = 1;
            bank.VBD_MOD_BY = objUser.PKUser;
            requestData = ObjectToJsonString(bank);
            string bankID = string.Empty;
            string xmlstr = GTIService.CommonFunctions.JsonToXml(requestData);
            //xmlstr = xmlstr.Replace("<root>", "<Root>");
            //xmlstr = xmlstr.Replace("</root>", "</Root>");
            bankID = DataAccess.VendorManagement.VendorRegistrationDL.SaveVendorBank(xmlstr).ToString();
            return bankID;
        }

        /// <summary>
        /// save local address
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SaveVenLocalAddress(string requestData, User objUser)
        {
            BusinessObject.VendorManagement.LocalAddressDetails Localaddress = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.VendorManagement.LocalAddressDetails>(requestData);
            Localaddress.VNC_LC_ACTIVE = 1;
            Localaddress.VNC_LC_MOD_BY = objUser.PKUser;
            Localaddress.VNC_LC_BIZUNIT = objUser.CurrentSBUPK;
            requestData = ObjectToJsonString(Localaddress);
            string LocalAddressID = string.Empty;
            string xmlstr = GTIService.CommonFunctions.JsonToXml(requestData);
            //xmlstr = xmlstr.Replace("<root>", "<Root>");
            //xmlstr = xmlstr.Replace("</root>", "</Root>");
            LocalAddressID = DataAccess.VendorManagement.VendorRegistrationDL.SaveVenLocalAddress(xmlstr).ToString();
            return LocalAddressID;
        }



        public static string ObjectToJsonString(object obj)
        {
            var javaScriptSerializer = new
            System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonString = javaScriptSerializer.Serialize(obj);
            return jsonString;
        }
        #endregion

        public static DataTable GetVendor(User objUser, int VendorPk, short Active, string VendorName)
        {
            DataTable dtVendor = DataAccess.VendorManagement.VendorMasterDL.GetVendor(objUser, VendorPk, Active, VendorName);
            return dtVendor;
        }

        /// <summary>
        /// Get Vendor banks
        /// </summary>
        /// <param name="currentUser"></param>
        /// <param name="VendorbankPk"></param>
        /// <param name="vendorPk"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static DataTable GetVendorBanks(User objUser, int VendorbankPk, int VendorPk, short Active)
        {
            DataTable dtVendorBanks = DataAccess.VendorManagement.VendorMasterDL.GetVendorBanks(objUser, VendorbankPk, VendorPk, Active);
            return dtVendorBanks;
        }

        public static DataTable GetVendorLocalAddress(User objUser, int VendorLcAddrsPk, int VendorPk, short Active)
        {
            DataTable dtVendorBanks = DataAccess.VendorManagement.VendorMasterDL.GetVendorLocalAddress(objUser, VendorLcAddrsPk, VendorPk, Active);
            return dtVendorBanks;
        }

        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="pageURL"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetVendorAutoSearch(string searchBy, string searchValue, int bizUnitPK)
        {
            DataTable dtSearch = DataAccess.VendorManagement.VendorMasterDL.GetVendorAutoSearch(searchBy, searchValue, bizUnitPK);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);

        }
        public static string GetPOVendorAutoSearch(string searchValue, int bizUnitPK)
        {
            DataTable dtSearch = DataAccess.VendorManagement.VendorMasterDL.GetPOVendorAutoSearch( searchValue, bizUnitPK);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.VEN_NAME, GTIService.Constants.Common.Fields.VEN_PK);

        }

        /// <summary>
        /// Get Values For Auto Complete
        /// </summary>
        /// <returns></returns>
        public static string GetVendorsByRoleAuto(int venPK, int vrmRole, string venName, int bizUnitPK, int active)
        {
            DataTable dtSearch = DataAccess.VendorManagement.VendorMasterDL.GetVendorsByRoleAuto(venPK, vrmRole, venName, bizUnitPK, active);
            string jString = GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Vendor.Fields.VENDORNAME, GTIService.Constants.Vendor.Fields.VENDORPK);
            return jString;
        }
        public static DataTable GetVendorsByRole(int venPK, int vrmRole, string venName, int bizUnitPK, int active)
        {
            return DataAccess.VendorManagement.VendorMasterDL.GetVendorsByRoleAuto(venPK, vrmRole, venName, bizUnitPK, active);
        }
    }
}
