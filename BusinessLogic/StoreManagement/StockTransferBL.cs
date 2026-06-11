
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using BusinessObject;
using System;
using BusinessObject.CommonManagement;
using System.Linq;

namespace BusinessLogic.StoreManagement
{
    public class StockTransferBL
    {

        #region Methods

        #region ============================================= Entry Section =========================================
        /// <summary>
        /// Get Stock transfer Number 
        /// </summary>
        /// <returns></returns>
        public static string GetStockTransfer()
        {
            return DataAccess.StoreManagement.StockTransferDA.GetStockTransferNumber();
        }
        /// <summary>
        ///  Get Stock Trasnfer details , for Click Add Selected Item To list, get Details - Transfer Details and Allocated Details
        /// </summary>
        /// <param name="storeTransfer"></param>
        /// <param name="ginDtls"></param>
        /// <returns></returns>
        public static string GetPRPODetails(int sbu, int type, int itemList, string xmlPKDtls, int pk)
        {
            string xmlDtls = GTIService.CommonFunctions.JsonToXml(xmlPKDtls);
            var xmlString = string.Empty;
            if (itemList == 0)
            {
                DataTable dtPOPRDtls = DataAccess.StoreManagement.StockTransferDA.GetPRPODetails(sbu, type, xmlDtls, pk);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < dtPOPRDtls.Rows.Count; i++)
                {
                    sb.Append(dtPOPRDtls.Rows[i][0].ToString());

                }
                if (sb.Length > 0)
                {
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(sb.ToString());
                    xmlString = Newtonsoft.Json.JsonConvert.SerializeXmlNode(xDoc.FirstChild);
                }
                return xmlString;
            }
            else
            {

                DataTable dtStores = DataAccess.StoreManagement.StockTransferDA.GetAllocatedItems(sbu, type, itemList, xmlDtls);
                return GTIService.CommonFunctions.GetTextValueList(dtStores, GTIService.Constants.StockTransfer.Fields.ItemName, GTIService.Constants.StockTransfer.Fields.ItemPk);
            }
        }
        /// <summary>
        /// Method to Get Stock Tansfer Details In Edit Mode
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static string GetStockTransferDetails(int pk)
        {
            var xmlString = string.Empty;
            DataTable dtPOPRDtls = DataAccess.StoreManagement.StockTransferDA.GetStockTransferDetails(pk);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dtPOPRDtls.Rows.Count; i++)
            {
                sb.Append(dtPOPRDtls.Rows[i][0].ToString());

            }
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(sb.ToString());
            xmlString = Newtonsoft.Json.JsonConvert.SerializeXmlNode(xDoc.FirstChild);
            return xmlString;

        }
        /// <summary>
        /// Get GIn Details
        /// </summary>
        /// <param name="grnDtlPk"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <param name="store"></param>
        /// <param name="ginPk"></param>
        /// <returns></returns>
        public static string GetGINDetails(int grnDtlPk, int status, int sbu, int store, int ginPk)
        {
            DataSet dsDtls = DataAccess.StoreManagement.StockTransferDA.GetGINDetails(grnDtlPk, status, sbu, store, ginPk);
            string jString = string.Empty;
            if (dsDtls.Tables.Count > 1 && dsDtls.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsDtls.Tables[1]);
            }
            return jString;

        }
        /// <summary>
        /// Method to get 
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <param name="itemID"></param>
        /// <param name="sbuPk"></param>
        /// <param name="type"></param>
        /// <param name="userPK"></param>
        /// <param name="store"></param>
        /// <returns></returns>
        public static string GetItemByStore(int CategoryID, int itemID, int sbuPk, int type, int userPK, int store)
        {
            DataTable dtMaterialDtls = DataAccess.StoreManagement.StockTransferDA.GetMaterialByCategoryAndStore(CategoryID, itemID, sbuPk, type, userPK, store);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialDtls, GTIService.Constants.StockTransfer.Fields.DeptName, GTIService.Constants.StockTransfer.Fields.DeptPk);

            }
            return jString;
        }
        /// <summary>
        /// Method to Get material UOm Details
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static string GetItemUOMDetails(int itemID, int status)
        {
            DataTable dtMaterialDtls = DataAccess.StoreManagement.StockTransferDA.GetMaterialUOMDetails(itemID, status);
            string jString = string.Empty;
            if (dtMaterialDtls.Rows.Count > 0)
            {
                jString = GTIService.CommonFunctions.GetTextValueList(dtMaterialDtls, GTIService.Constants.StockTransfer.Fields.UOMCode, GTIService.Constants.StockTransfer.Fields.UOMPk);

            }
            return jString;
        }
        /// <summary>
        /// Method to Get material Details by PK
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataTable GetItemDetails(int itemID, int status)
        {
            return DataAccess.StoreManagement.StockTransferDA.GetMaterialUOMDetails(itemID, status);
        }
        /// <summary>
        /// Method to Save Stock Transfer Details
        /// </summary>
        /// <param name="stockTransferDtls"></param>
        /// <returns></returns>
        public static string SaveStockTransferDtls(string stockTransferDtls, string lastModDate)
        {
            string ginID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(stockTransferDtls);
            retvals = DataAccess.StoreManagement.StockTransferDA.SaveStockTransferDetails(xmlstr, lastModDate);
            ginID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }
        #endregion

        #region =============================================  Lsiting Section =========================================
        /// <summary>
        /// Get Stock Transfer Details List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetStockTransferList(GridPrams grid, User objUser, int procID, string PageUrl, string saNo, string ginNo, string grnNo, string poNo, int venPK, string depName, int filterStatus, int cmpPk = 0)
        {
            DataSet dsGINList = DataAccess.StoreManagement.StockTransferDA.GetStockTransferList(grid, objUser, procID, PageUrl,saNo,ginNo,grnNo,poNo,venPK,depName,filterStatus,cmpPk);
            string jString = string.Empty;
            if (dsGINList.Tables.Count > 1 && dsGINList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsGINList);
            }
            return jString;
        }
        /// <summary>
        /// Delete Stock Transfer Details
        /// </summary>
        /// <param name="grnPk"></param>
        /// <returns></returns>
        public static string DeleteStockTransferDtls(int stockTransferPk, string lastModDt)
        {           
            string refTransNo = string.Empty;
            List<object> retvals = new List<object>();
            retvals = DataAccess.StoreManagement.StockTransferDA.DeleteStockTransferDetails(stockTransferPk, lastModDt);
            refTransNo = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }
        /// <summary>
        /// Get Auto Complete Searchj For GRN Number
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetAutoCompleteSearch(string searchBy, string searchValue, User objUser, int procId)
        {
            DataTable dtSearch = DataAccess.StoreManagement.StockTransferDA.GetDetailsForAutoSearch(searchBy, searchValue, objUser, procId);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);
        }
        #endregion

        /// <summary>
        /// Get AuoComplete Search Details
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetStockAdmissionAutocomplete(string searchBy, string searchValue, int processPK, User objUser)
        {
            List<AutoCompleteBO> result;
            result = new List<AutoCompleteBO>();
            try
            {
                DataTable dtSearch = DataAccess.StoreManagement.StockTransferDA.GetDetailsForAutoSearch(searchBy, searchValue, objUser, processPK);               
                result = dtSearch.AsEnumerable().Select(row => new AutoCompleteBO()
                {
                    Key = row.Field<int>(GTIService.Constants.Designation.Fields.PK),
                    Name = row.Field<string>(GTIService.Constants.Designation.Fields.VALUE)
                }).ToList();
            }
            catch
            {
            }
            return result;
        }

        #region =============================================  Report Section =========================================
        /// <summary>
        /// Method to Get Stock Tansfer Details In Edit Mode
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static string GetStockTransferRptDetails(int pk)
        {
            //return DataAccess.StoreManagement.StockTransferDA.GetStockTransferRptDetails(pk); ;
            string xml = string.Empty;
            DataTable dtPrcCtrlList = DataAccess.StoreManagement.StockTransferDA.GetStockTransferRptDetails(pk);
            if (dtPrcCtrlList != null)
            {
                if (dtPrcCtrlList.Rows.Count > 0)
                {
                    foreach (DataRow drData in dtPrcCtrlList.Rows)
                    {
                        xml += Convert.ToString(drData[0]);
                    }
                }
            }
            return xml;
        }
        #endregion
        #endregion
    }
}
