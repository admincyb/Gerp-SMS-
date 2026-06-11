using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BusinessObject;
using System.Data;
using BusinessObject.MaterialManagement;
using ERP.Utilities;

namespace BusinessLogic.StoreManagement
{
   public class ExternalMaterialIssueBL
    {



        /// <summary>
        ///  Returns Issue to list
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="parentDepartment"></param>
        /// <returns></returns>
       public static string GetIssuingTypeList(int bizUnit, int issuingType, int despatched, string searchValue, int? DeptType)
        {
            DataTable dtType = DataAccess.StoreManagement.ExternalMaterialIssueDL.GetIssuingTypeList(bizUnit, issuingType, despatched, searchValue,DeptType);
            return GTIService.CommonFunctions.GetTextValueList(dtType, GTIService.Constants.Common.Fields.ISSUETYPETEXT, GTIService.Constants.Common.Fields.ISSUETYPEPK);

        }

        public static string GetDamageStore(int DeptPK)
        {
            DataTable dtType = DataAccess.StoreManagement.ExternalMaterialIssueDL.GetDamageStore(DeptPK);
            return GTIService.CommonFunctions.GetTextValueList(dtType, GTIService.Constants.Common.Fields.ISSUETYPETEXT, GTIService.Constants.Common.Fields.ISSUETYPEPK);
        }

        public static string GetAssetFormer(int bizUnit)
        {
            DataTable dtType = DataAccess.StoreManagement.ExternalMaterialIssueDL.GetAssetFormer(bizUnit);
            return GTIService.CommonFunctions.GetTextValueList(dtType, GTIService.Constants.Common.Fields.ITC_NAME, GTIService.Constants.Common.Fields.ITC_PK);
        }

        /// <summary>
        /// Save External Material Issue - Details  
        /// </summary>
        /// <param name="MaterialIssueDetails"></param>
        /// <returns>string</returns>
        public static string SaveExternalMaterialIssue(string requisitionDetails, User objUser)
        {
            string requisitionID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(requisitionDetails);
            retvals = DataAccess.StoreManagement.ExternalMaterialIssueDL.SaveExternalMaterialIssue(xmlstr);
            requisitionID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }
       /// <summary>
       /// Save External Material Issue/ Receipt - Details  with Workflow
       /// </summary>
       /// <param name="requisitionDetails"></param>
       /// <param name="objUser"></param>
       /// <returns></returns>
       public static string SaveExternalMaterialIssueWkf(string requisitionDetails, User objUser)
        {
            string requisitionID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(requisitionDetails);
            retvals = DataAccess.StoreManagement.ExternalMaterialIssueDL.SaveExternalMaterialIssueWkf(xmlstr);
            requisitionID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

        public static string SaveEMIDamageWkf(string emiDamage, User objUser)
        {
            string requisitionID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(emiDamage);
            retvals = DataAccess.StoreManagement.ExternalMaterialIssueDL.SaveEMIDamageWkf(xmlstr);
            requisitionID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

        /// <summary>
        /// Returns ICH No in json string format
        /// </summary>
        /// <param name=""></param>
        /// <returns>String</returns>
        public static string GetICHNo(int bizUnit, int transactionType)
        {
            return DataAccess.StoreManagement.ExternalMaterialIssueDL.GetICHNo(bizUnit, transactionType);

        }
        /// <summary>
        /// Returns requisition list in json string format
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
       public static string GetIssuingList(GridPrams grid, int bizUnit, User objUser, int procID, int transactionType, string pageURL, DateTime? fromDate, DateTime? toDate, string issueNo, int trnStatus, int issueType, int issueTo, int issueStore, string lotNo, string refNo, int itmCatPK, int itmPk)// , int cmpPk
        {
            DataSet dsRequisitionList = DataAccess.StoreManagement.ExternalMaterialIssueDL.GetIssuingList(grid, bizUnit, objUser, procID, transactionType, pageURL, fromDate, toDate, issueNo, trnStatus, issueType, issueTo, issueStore, lotNo, refNo, itmCatPK, itmPk);//, cmpPk
            string jString = string.Empty;
            if (dsRequisitionList.Tables.Count > 1 && dsRequisitionList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsRequisitionList);
            }
            return jString;
        }
       /// <summary>
       ///  Returns requisition list in json string format
       /// </summary>
       /// <param name="grid"></param>
       /// <param name="bizUnit"></param>
       /// <param name="objUser"></param>
       /// <param name="procID"></param>
       /// <param name="transactionType"></param>
       /// <param name="pageURL"></param>
       /// <param name="fromDate"></param>
       /// <param name="toDate"></param>
       /// <param name="issueNo"></param>
       /// <param name="trnStatus"></param>
       /// <param name="issueType"></param>
       /// <param name="issueTo"></param>
       /// <param name="issueStore"></param>
       /// <param name="lotNo"></param>
       /// <param name="refNo"></param>
       /// <param name="itmCatPK"></param>
       /// <param name="itmPk"></param>
       /// <returns></returns>
       public static string GetIssuingListWkf(GridPrams grid, int bizUnit, User objUser, int procID, int transactionType, string pageURL, DateTime? fromDate, DateTime? toDate, string issueNo, int trnStatus, int issueType, int issueTo, int issueStore, string lotNo, string refNo, int itmCatPK, int itmPk)// , int cmpPk
        {
            DataSet dsRequisitionList = DataAccess.StoreManagement.ExternalMaterialIssueDL.GetIssuingListWkf(grid, bizUnit, objUser, procID, transactionType, pageURL, fromDate, toDate, issueNo, trnStatus, issueType, issueTo, issueStore, lotNo, refNo, itmCatPK, itmPk);//, cmpPk
            string jString = string.Empty;
            if (dsRequisitionList.Tables.Count > 1 && dsRequisitionList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsRequisitionList);
            }
            return jString;
        }
        /// <summary>
        /// Get Requisition Details As a XML Format and Convert in to JSON and Return As a JSON String Format
        /// </summary>
        /// <param name="RequisitionID"></param>
        /// <returns></returns>
        public static string GetMaterialIssueDetails(int materialIssueID)
        {

            BusinessObject.StoreManagement.ExternalMaterialIssue obj = new BusinessObject.StoreManagement.ExternalMaterialIssue();
            return GTIService.CommonFunctions.XmlToJson(DataAccess.StoreManagement.ExternalMaterialIssueDL.GetMaterialIssueDetails(materialIssueID));

        }
        /// <summary>
        /// Get Requisition Details As a XML Format and Convert in to JSON and Return As a JSON String Format
        /// </summary>
        /// <param name="RequisitionID"></param>
        /// <returns></returns>
        public static string GetDetailsFromCRDR(int crdrPK)
        {

            BusinessObject.StoreManagement.ExternalMaterialIssue obj = new BusinessObject.StoreManagement.ExternalMaterialIssue();
            return GTIService.CommonFunctions.XmlToJson(DataAccess.StoreManagement.ExternalMaterialIssueDL.GetDetailsFromCRDR(crdrPK));

        }
        /// <summary>
        /// Delete requisition Details
        /// </summary>
        /// <param name="MRHPK"></param>
        /// <returns>String</returns>
        public static string DeleteExternalMaterialIssue(int ICHPK, int? USERPK, int HasWorkflow = 0)
        {
            return DataAccess.StoreManagement.ExternalMaterialIssueDL.DeleteExternalMaterialIssue(ICHPK, USERPK, HasWorkflow).ToString();
        }
        /// <summary>
        /// function used to store requisition report
        /// </summary>
        /// <param name="Requisition"></param>
        /// <returns></returns>
        public static DataSet GetIssuingReportByReqId(int RequistID)
        {

            DataSet dsetRequisitionReport = DataAccess.StoreManagement.ExternalMaterialIssueDL.GetIssuingReportByReqId(RequistID);
            return dsetRequisitionReport;

        }

        /// <summary>
        /// function used to Store Requisition (EMI) report
        /// </summary>
        /// <param name="Requisition"></param>
        /// <returns></returns>
        public static DataSet GetStoreRequisitionByReqId(int requistID)
        {

            DataSet dsetRequisitionReport = DataAccess.StoreManagement.ExternalMaterialIssueDL.GetStoreRequisitionByReqId(requistID);

            return dsetRequisitionReport;
        }

        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchBy, string searchValue, int sbuPk, User objUser, int procID,int transactionType)
        {
            DataTable dtSearch = DataAccess.StoreManagement.ExternalMaterialIssueDL.GetSearchValues(searchBy, searchValue, sbuPk, objUser, procID, transactionType);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Store.Fields_MaterialConsumption.REQUISITIONSEARCHVALUEFIELD, GTIService.Constants.Store.Fields_MaterialConsumption.REQUISITIONSEARCHTEXTFIELD);

        }
       
       /// <summary>
        /// 
        /// </summary>
        /// <param name="configText"></param>
        /// <param name="spclCond"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetWeightedAverage(int active, string acfSetting, string acfData, int bizUnit)
        {
            DataTable dtWeightedAvg = DataAccess.StoreManagement.ExternalMaterialIssueDL.GetWeightedAverage(active, acfSetting, acfData, bizUnit);
            return dtWeightedAvg;
        }

        public static string GetGRNAutoComplteList(int bizUnit, string srchValue)
        {
            DataTable dtType = DataAccess.StoreManagement.ExternalMaterialIssueDL.GetGRNAutoComplteList(bizUnit, srchValue);
            return GTIService.CommonFunctions.GetTextValueList(dtType, GTIService.Constants.Store.Fields.GRH_GRN_REF_NO, GTIService.Constants.Store.Fields.GRH_PK);

        }

        public static string GetGRNDetailsList(int GRNPK, int IssuingStore)
        {
            BusinessObject.StoreManagement.ExternalMaterialIssue obj = new BusinessObject.StoreManagement.ExternalMaterialIssue();
            return GTIService.CommonFunctions.XmlToJson(DataAccess.StoreManagement.ExternalMaterialIssueDL.GetGRNDetailsList(GRNPK, IssuingStore));
            //return DataAccess.StoreManagement.ExternalMaterialIssueDL.GetGRNDetailsList(GRNPK);
        }

        #region External Material Issue (Multiple)
        public static EMIMultiple GetEMIDetails(int CurrPK)
        {
            try
            {
                EMIMultiple objEMI = new EMIMultiple();
                string result = DataAccess.StoreManagement.ExternalMaterialIssueDL.GetEMIDetails(CurrPK);
                //string result = "<root><ICH_REF_NO>12</ICH_REF_NO></root>";
                if (!string.IsNullOrEmpty(result))
                {
                    objEMI = (EMIMultiple)CommonFunctions.DeserializeObject(result, objEMI);
                    return objEMI;
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

       //External Material Issue Multiple Report
        /// <summary>
        /// function used to store requisition report
        /// </summary>
        /// <param name="Requisition"></param>
        /// <returns></returns>
        public static DataSet GetEMIMultipleReport(int RequistID)
        {

            DataSet dsetRequisitionReport = DataAccess.StoreManagement.ExternalMaterialIssueDL.GetEMIMultipleReport(RequistID);
            return dsetRequisitionReport;

        }

        #endregion
    }
}
