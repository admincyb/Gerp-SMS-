using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.PurchaseOrderManagement;
using System.Web;
using System.Web.Script.Serialization;
using GTIService;
using BusinessObject.PurchaseOrderManagement;


namespace BusinessLogic.PurchaseOrderManagement
{
    public class PurchaseOrderGenerateBL
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridPrams"></param>
        /// <param name="bizUnit"></param>
        /// <param name="vendor"></param>
        /// <param name="poID"></param>
        /// <returns></returns>
        public static string GetPendingPurchaseRequest(BusinessObject.GridPrams gridPrams, int bizUnit, int vendor, int poID, int userPK, int prhPK,string sortBy = "",int processId=0,int type=0)
        {
            gridPrams.SortBy = gridPrams.SortBy == null ? sortBy : gridPrams.SortBy;
            gridPrams.SortDirection = gridPrams.SortDirection == null ? "Desc" : gridPrams.SortDirection;
            DataSet dsPRList = PurchaseOrderGenerateDL.GetPendingPurchaseRequest(gridPrams, bizUnit, vendor, poID, userPK, prhPK,processId,type);
            string jString = string.Empty;
            if (dsPRList.Tables.Count > 1 && dsPRList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPRList);
            }
            return jString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poID"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetPurchaseRequestItemVendor(int poID, int bizUnit, string xmlPRItem)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(xmlPRItem);
            DataTable dtSearch = PurchaseOrderGenerateDL.GetPurchaseRequestItemVendor(poID, bizUnit, xmlstr);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dtSearch);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poID"></param>
        /// <returns></returns>
        public static string GetPOVendor(int poID)
        {
            DataTable dtSearch = PurchaseOrderGenerateDL.GetPOVendor(poID);
            return Newtonsoft.Json.JsonConvert.SerializeObject(dtSearch);
        }

        public static string GetVendorRates(int vendorID, string xmlItem)
        {
            string xmlstr = GTIService.CommonFunctions.JsonToXml(xmlItem);
            string rates = string.Empty;
            string taxes = string.Empty;

            DataSet dsSearch = PurchaseOrderGenerateDL.GetVendorRates(vendorID, xmlstr);
            if (dsSearch != null && dsSearch.Tables.Count > 0)
            {
                rates = Newtonsoft.Json.JsonConvert.SerializeObject(dsSearch.Tables[0]);
                if (dsSearch.Tables.Count > 1)
                    foreach (DataRow dr in dsSearch.Tables[1].Rows)
                    {
                        taxes += dr[0].ToString();
                    }
            }
            taxes = taxes == string.Empty ? "null" : GTIService.CommonFunctions.XmlToJson(taxes);
            string vendorRates = "{\"Rates\":" + rates + ",\"Taxes\":" + taxes + "}";
            return vendorRates;
        }

        public static string GetGrnQty(int podPK)
        {
            string Quantity = string.Empty;
            DataSet dsSearch = PurchaseOrderGenerateDL.GetGrnQty(podPK);
            if (dsSearch != null && dsSearch.Tables.Count > 0)
            {
                Quantity = Newtonsoft.Json.JsonConvert.SerializeObject(dsSearch.Tables[0]);
            }
            return Quantity;
        }

        public static string GetRevisionHistory(int pohPK)
        {
            string result = string.Empty;
            DataSet dsSearch = PurchaseOrderGenerateDL.GetRevisionHistory(pohPK);
            if (dsSearch != null && dsSearch.Tables.Count > 0)
            {
                result = Newtonsoft.Json.JsonConvert.SerializeObject(dsSearch.Tables[0]);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetSearchValues(string searchBy, string searchValue, BusinessObject.User objUser, int vendor, int poPK, int DeptPK = 0, int processPk = 0)
        {
            DataTable dtSearch = PurchaseOrderGenerateDL.GetSearchValues(searchBy, searchValue, objUser, vendor, poPK, DeptPK,processPk);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);
        }

        /// <summary>
        /// Get Config Master
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="CfgType"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static string GetConfigMaster(BusinessObject.User objUser, string CfgType,int Active)
        {
            DataTable dtSearch = PurchaseOrderGenerateDL.GetConfigMaster(objUser, CfgType, Active);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.CFGVALUE, GTIService.Constants.Designation.Fields.CFGPK);
        }

        /// <summary>
        /// Get PurchaseOrder Types
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="CfgType"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static string GetPurchaseOrderTypes(BusinessObject.User objUser, int Active)
        {
            DataTable dtSearch = PurchaseOrderGenerateDL.GetPurchaseOrderTypes(objUser, Active);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.CFGVALUE, GTIService.Constants.Designation.Fields.CFGPK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pODetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SavePODetails(string xmlPoDetails, BusinessObject.User objUser)
        {
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(xmlPoDetails);
            retvals = PurchaseOrderGenerateDL.SavePODetails(xmlstr);
            if (Convert.ToInt32(retvals[0]) != 0)
            {
                BusinessObject.CommonManagement.CommonObject.File fileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.File>(xmlPoDetails);
                for (int i = 0; i < fileObject.FILELIST.Count; i++)
                {
                    if (fileObject.FILELIST[i].DOC_PK == 0 && fileObject.FILELIST[i].DOC_TITLE != string.Empty)
                    {
                        CommonManagement.CommonManagement.UploadFromTempFolderForXml(fileObject.FILELIST[i].DOC_NAME, "Purchase");
                    }
                }
            }
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pODetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SavePODetailsWkf(string xmlPoDetails, BusinessObject.User objUser)
        {
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(xmlPoDetails);
            retvals = PurchaseOrderGenerateDL.SavePODetailsWkf(xmlstr);
            if (Convert.ToInt32(retvals[0]) != 0)
            {
                BusinessObject.CommonManagement.CommonObject.File fileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.File>(xmlPoDetails);
                for (int i = 0; i < fileObject.FILELIST.Count; i++)
                {
                    if (fileObject.FILELIST[i].DOC_PK == 0 && fileObject.FILELIST[i].DOC_TITLE != string.Empty)
                    {
                        CommonManagement.CommonManagement.UploadFromTempFolderForXml(fileObject.FILELIST[i].DOC_NAME, "Purchase");
                    }
                }
            }
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

        public static POHeader GetPurchaseOrderNonStock(int pohPK)
        {
            try
            {
                POHeader rfqResponseHeaderObj = new POHeader();
                string quotation = DataAccess.PurchaseOrderManagement.PurchaseOrderGenerateDL.GetPurchaseOrderNonStock(pohPK);
                if (quotation != string.Empty)
                {
                    rfqResponseHeaderObj = (POHeader)CommonFunctions.DeserializeObject(quotation, rfqResponseHeaderObj);
                    return rfqResponseHeaderObj;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static DataTable GetPurchaseType(BusinessObject.User objUser, string CfgType, int Active)
        {
            DataTable dtSearch = PurchaseOrderGenerateDL.GetConfigMaster(objUser, CfgType, Active);
            return dtSearch;
        }

        /// <summary>
        /// 
        /// </summary>       
        /// <param name="bizUnit"></param>  
        /// <param name="POD_PK"></param>
        /// <returns></returns>
        public static string GetPurchaseOrderMapPR(int bizUnit, int POD_PK)
        {
            DataSet dsPRList = PurchaseOrderGenerateDL.GetPurchaseOrderMapPR(bizUnit, POD_PK);
            string jString = string.Empty;
            if (dsPRList.Tables.Count > 0 && dsPRList.Tables[0].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPRList.Tables[0]);
            }
            return jString;
        }

        public static DataTable GetPONonStockHistory(int pohPK)
        {
            try
            {
                DataTable dtPOhistory = DataAccess.PurchaseOrderManagement.PurchaseOrderGenerateDL.GetPONonStockHistory(pohPK);
                return dtPOhistory;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get PO department PK
        /// </summary>
        /// <param name="CrDrPk"></param>
        /// <returns></returns>
        public static int GetPODepartment(long CrDrPk)
        {
            return DataAccess.PurchaseOrderManagement.PurchaseOrderGenerateDL.GetPODepartment(CrDrPk);
        }

        #region Trading
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pODetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SavePODetailsTrading(string xmlPoDetails, BusinessObject.User objUser)
        {
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(xmlPoDetails);
            retvals = PurchaseOrderGenerateDL.SavePODetailsTrading(xmlstr);
            if (Convert.ToInt32(retvals[0]) != 0)
            {
                BusinessObject.CommonManagement.CommonObject.File fileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.File>(xmlPoDetails);
                for (int i = 0; i < fileObject.FILELIST.Count; i++)
                {
                    if (fileObject.FILELIST[i].DOC_PK == 0 && fileObject.FILELIST[i].DOC_TITLE != string.Empty)
                    {
                        CommonManagement.CommonManagement.UploadFromTempFolderForXml(fileObject.FILELIST[i].DOC_NAME, "Purchase");
                    }
                }
            }
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        } 
        #endregion

        public static BusinessObject.DashBoard.PurchaseDashboardHdr GetVendorPOs(int VendorPk, string PONo)
        {
            try
            {
                BusinessObject.DashBoard.PurchaseDashboardHdr objPurchaseDashboard = new BusinessObject.DashBoard.PurchaseDashboardHdr();
                string strResult = DataAccess.PurchaseOrderManagement.PurchaseOrderGenerateDL.GetVendorPOs(VendorPk, PONo);
                if (strResult != string.Empty)
                {
                    objPurchaseDashboard = (BusinessObject.DashBoard.PurchaseDashboardHdr)CommonFunctions.DeserializeObject(strResult, objPurchaseDashboard);
                    return objPurchaseDashboard;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static BusinessObject.DashBoard.POHeader GetPODetails(int POPk)
        {
            try
            {
                BusinessObject.DashBoard.POHeader objPOHeader = new BusinessObject.DashBoard.POHeader();
                string strResult = DataAccess.PurchaseOrderManagement.PurchaseOrderGenerateDL.GetPODetails(POPk);
                if (strResult != string.Empty)
                {
                    objPOHeader = (BusinessObject.DashBoard.POHeader)CommonFunctions.DeserializeObject(strResult, objPOHeader);
                    return objPOHeader;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static string GetPurchaseOrderProjectBudget(string Investor, int CurrencyPK, string PODate)
        {
            try
            {
                DataTable dtBudget = DataAccess.PurchaseOrderManagement.PurchaseOrderGenerateDL.GetPurchaseOrderProjectBudget(Investor, CurrencyPK, PODate);
                return Newtonsoft.Json.JsonConvert.SerializeObject(dtBudget);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
