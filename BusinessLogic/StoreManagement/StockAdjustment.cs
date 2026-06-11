using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using System.Data;

namespace BusinessLogic.StoreManagement
{
    class TempWorkFlow
    {
       public int UserPK { get; set; }
        public int PRefID { get; set; }
    }

    public class StockAdjustment
    {
        /// <summary>
        /// function used to get stock adjustment details against a stock audit ID
        /// </summary>
        /// <param name="storeAuditID"></param>
        /// <returns></returns>
        public static string GetStockAdjustmentDetails(int storeAuditID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.StoreManagement.StockAdjustmentDL.GetStoreAuditDetails(storeAuditID));
        }
        /// <summary>
        /// function used to get stock adjustment details against a stock audit ID For Report
        /// </summary>
        /// <param name="storeAuditID"></param>
        /// <returns></returns>
        public static string GetStockAdjustmentDetailsForReport(int storeAuditID)
        {
            return DataAccess.StoreManagement.StockAdjustmentDL.GetStoreAuditDetails(storeAuditID);
        }
        /// <summary>
        /// Function used to save Store adjustment details
        /// </summary>
        /// <param name="storeAdjustmentDtls"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SaveStockAdjustmentDetails(string stockAdjustmentDtls, User objUser)
        {

            string saID = string.Empty;
            string xmlstr = GTIService.CommonFunctions.JsonToXml(stockAdjustmentDtls);
            saID = DataAccess.StoreManagement.StockAdjustmentDL.SaveStockAdjustmentDetails(xmlstr).ToString();
            return saID;
            
        }

        /// <summary>
        /// Method to get stock adjustment List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <param name="procID"></param>
        /// <returns></returns>
        public static string GetStockAdjustmentList(GridPrams grid, User objUser, int procID, string PageUrl)
        {
            DataSet dsReqstList = DataAccess.StoreManagement.StockAdjustmentDL.GetStockAdjustmentList(grid, objUser, procID, PageUrl);
            string jString = string.Empty;
            if (dsReqstList.Tables.Count > 1 && dsReqstList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsReqstList);
            }
            return jString;
        }

        /// <summary>
        /// Method to get Search Values - AutoComplete
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetSearchValues(string searchBy, string searchValue, User objUser)
        {
            DataTable dtSearch = DataAccess.StoreManagement.StoreAuditListDL.GetSearchValues(searchBy, searchValue, objUser);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);

        }

        /// <summary>
        /// Delete Store Audit Details
        /// </summary>
        /// <param name="requestPk"></param>
        /// <returns></returns>
        public static string DeleteStoreAuditDtls(int requestPk)
        {
            return DataAccess.StoreManagement.StoreAuditListDL.DeleteStoreAuditDtls(requestPk).ToString();
        }

        /// <summary>
        /// Get Store Audit Report Details
        /// </summary>
        /// <param name="purchaseRequestID"></param>
        /// <returns></returns>
        public static DataSet GetStoreAuditReportDetails(int purchaseRequestID)
        {
            return DataAccess.StoreManagement.StoreAuditListDL.GetStoreAuditReportDetails(purchaseRequestID);
        }
        //====================== 30-09-2011 ===========================================
        /// <summary>
        /// Get Evaluation Application Id for RefID
        /// </summary>
        /// <param name="refid"></param>
        /// <returns></returns>
        public static int GetEvalAppIDForRefID(int refid)
        {
            return DataAccess.StoreManagement.StockAdjustmentDL.GetEvalAppIDForRefID(refid);
        }
        /// <summary>
        /// Update Ref ID for Evaluation Table
        /// </summary>
        /// <param name="evalPk"></param>
        /// <param name="refid"></param>
        /// <returns></returns>
        public static int UpdateEvaluationRefID(string workFlowDetails, User objUser)
        {
            Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
            WorkflowCore.CoreObjects.DoWorkFlowRequest wrkfReq = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowCore.CoreObjects.DoWorkFlowRequest>(workFlowDetails, settings);
            return DataAccess.StoreManagement.StockAdjustmentDL.UpdateEvaluationDtls(wrkfReq, objUser);
        }
    }
}
