using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using System.Data;

namespace BusinessLogic.StoreManagement
{
    public class MaterialIssue
    {
        /// <summary>
        /// Function Used To get Goods Receipt Note xml details based on the id
        /// </summary>
        /// <param name="Requisition"></param>
        /// <returns></returns>
        public static string GetMaterialIssueDetails(int miID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.StoreManagement.MaterilaIssueDL.GetMaterialIssueDetails(miID));
        }
        public static string GetMRIssueDetails(int miID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.StoreManagement.MaterilaIssueDL.GetMRIssueDetails(miID));
        }
        /// <summary>
        /// Get Requesting Store
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        public static DataTable GetRequestingStore(int appID)
        {
            return DataAccess.StoreManagement.MaterilaIssueDL.GetRequestingStore(appID);
        }
        public static DataTable GetMRStore(int appID)
        {
            return DataAccess.StoreManagement.MaterilaIssueDL.GetMRStore(appID);
        }
        /// <summary>
        /// Function Used To get Goods Receipt Note xml details based on the id
        /// </summary>
        /// <param name="Requisition"></param>
        /// <returns></returns>
        public static string GetMaterialIssueDetailsForReport(int miID)
        {
            return DataAccess.StoreManagement.MaterilaIssueDL.GetMaterialIssueDetails(miID);
        }

        public static string GetMaterialIssueDetailsForReportDOCNOREVISION(int miID,int reportpk)
        {
            return DataAccess.StoreManagement.MaterilaIssueDL.GetMaterialIssueDetailsForReportDOCNOREVISION(miID,reportpk);
        }

        public static string GetMaterialAcceptDetailsForReport(int miID)
        {
            return DataAccess.StoreManagement.MaterilaIssueDL.GetMaterialAcceptDetails(miID);
        }
        /// <summary>
        /// Fuction Used To get Material Issue Trx print details
        /// </summary>
        /// <param name="miID"></param>
        /// <returns></returns>
        public static string GetMRIssueDetailsForReport(int miID)
        {
            return DataAccess.StoreManagement.MaterilaIssueDL.GetMIssueDetails(miID);
        }
        /// <summary>
        /// Function Used To save Material Issue Details
        /// </summary>
        /// <param name="materialIssueDetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SaveMaterialIssue(string materialIssueDetails, User objUser)
        {
            string materialIssueID = string.Empty;
            List<object> retvals = new List<object>();
            WorkflowCore.CoreObjects.DoWorkFlowRequest objRequest = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
            objRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowCore.CoreObjects.DoWorkFlowRequest>(materialIssueDetails);
            objRequest.UserPK = objUser.PKUser;
            string xmlstr = GTIService.CommonFunctions.JsonToXml(materialIssueDetails);
            retvals = DataAccess.StoreManagement.MaterilaIssueDL.SaveMaterialIssue(xmlstr);
            
            materialIssueID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }
        public static string SaveMaterialIssueWkf(string materialIssueDetails, User objUser)
        {
            string materialIssueID = string.Empty;
            List<object> retvals = new List<object>();
            WorkflowCore.CoreObjects.DoWorkFlowRequest objRequest = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
            objRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowCore.CoreObjects.DoWorkFlowRequest>(materialIssueDetails);
            objRequest.UserPK = objUser.PKUser;
            string xmlstr = GTIService.CommonFunctions.JsonToXml(materialIssueDetails);
            retvals = DataAccess.StoreManagement.MaterilaIssueDL.SaveMaterialIssueWkf(xmlstr);
            
            materialIssueID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

        public static string SaveMRIssue(string materialIssueDetails, User objUser)
        {
            string materialIssueID = string.Empty;
            List<object> retvals = new List<object>();
            WorkflowCore.CoreObjects.DoWorkFlowRequest objRequest = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
            objRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowCore.CoreObjects.DoWorkFlowRequest>(materialIssueDetails);
            objRequest.UserPK = objUser.PKUser;
            string xmlstr = GTIService.CommonFunctions.JsonToXml(materialIssueDetails);
            retvals = DataAccess.StoreManagement.MaterilaIssueDL.SaveMRIssue(xmlstr);

            materialIssueID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

        public static string SaveMRIssuePlantToPlant(string materialIssueDetails, User objUser)
        {
            string materialIssueID = string.Empty;
            List<object> retvals = new List<object>();
            WorkflowCore.CoreObjects.DoWorkFlowRequest objRequest = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
            objRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowCore.CoreObjects.DoWorkFlowRequest>(materialIssueDetails);
            objRequest.UserPK = objUser.PKUser;
            string xmlstr = GTIService.CommonFunctions.JsonToXml(materialIssueDetails);
            retvals = DataAccess.StoreManagement.MaterilaIssueDL.SaveMRIssuePlantToPlant(xmlstr);

            materialIssueID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

        /// <summary>
        /// Function Used To get autocomplete
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetPendingSearchAuto(string searchBy, string searchValue, int searchCorr, User objUser, int searchCorr1)
        {
            DataTable dtSearch = DataAccess.StoreManagement.MaterilaIssueDL.GetPendingSearchAuto(searchBy, searchValue, searchCorr, objUser, searchCorr1);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);
        }
        public static string GetPendingWIHSearchAuto(string searchBy, string searchValue, int searchCorr, User objUser, int searchCorr1,int PendingWO)
        {
            DataTable dtSearch = DataAccess.StoreManagement.MaterilaIssueDL.GetPendingWIHSearchAuto(searchBy, searchValue, searchCorr, objUser, searchCorr1, PendingWO);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);
        }
        public static string GetMIPendingSearchAuto(string searchBy, string searchValue, User objUser, int dept, int MenuType = 0)
        {
            DataTable dtSearch = DataAccess.StoreManagement.MaterilaIssueDL.GetMIPendingSearchAuto(searchBy, searchValue, objUser, dept, MenuType);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);
        }

        public static string GetWOPending(GridPrams grid, int sbuID, int store, int miPK, int dept, int mrhPK,int PendingWO)
        {
            DataSet dsSRSList = DataAccess.StoreManagement.MaterilaIssueDL.GetWOPending(grid, sbuID, store, miPK, dept, mrhPK, PendingWO);
            string jString = string.Empty;
            if (dsSRSList.Tables.Count > 1 && dsSRSList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsSRSList);
            }
            return jString;
        }
        /// <summary>
        /// Function Used To get pending srs details
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static string GetSRSPending(GridPrams grid, int sbuID, int store, int miPK, int dept, int mrhPK)
        {
            DataSet dsSRSList = DataAccess.StoreManagement.MaterilaIssueDL.GetSRSPending(grid, sbuID, store, miPK, dept, mrhPK);
            string jString = string.Empty;
            if (dsSRSList.Tables.Count > 1 && dsSRSList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsSRSList);
            }
            return jString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <param name="miPK"></param>
        /// <param name="dept"></param>
        /// <param name="mrhPK"></param>
        /// <returns></returns>
        public static string GetMRPending(GridPrams grid, int sbuID, int miPK, int dept, int mrhPK, int MenuType = 0, int CurrDept = 0)
        {
            DataSet dsSRSList = DataAccess.StoreManagement.MaterilaIssueDL.GetMRPending(grid, sbuID, miPK, dept, mrhPK, MenuType, CurrDept);
            string jString = string.Empty;
            if (dsSRSList.Tables.Count > 1 && dsSRSList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsSRSList);
            }
            return jString;
        }

        /// <summary>
        /// Function Used To get previous srs details
        /// </summary>
        /// <param name="srsID"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static string GetPreviousSRSDetailsView(int srsID)
        {
            DataSet dsSRSList = DataAccess.StoreManagement.MaterilaIssueDL.GetPreviousSRSDetailsView(srsID);
            string jString = string.Empty;
            if (dsSRSList.Tables.Count > 1 && dsSRSList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsSRSList);
            }
            return jString;
        }

        /// <summary>
        /// Function Used To get MI List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static string GetMIList(GridPrams grid, User objUser,string pageURL)
        {
            DataSet dsMIList = DataAccess.StoreManagement.MaterilaIssueDL.GetMIList(grid, objUser, pageURL);
            string jString = string.Empty;
            if (dsMIList.Tables.Count > 1 && dsMIList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsMIList);
            }
            return jString;
        }

        public static string GetMRIssueList(GridPrams grid, User objUser, string pageURL)
        {
            DataSet dsMIList = DataAccess.StoreManagement.MaterilaIssueDL.GetMRIssueList(grid, objUser, pageURL);
            string jString = string.Empty;
            if (dsMIList.Tables.Count > 1 && dsMIList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsMIList);
            }
            return jString;
        }

        /// <summary>
        /// Function Used To get MI no sequece
        /// </summary>
        /// <returns></returns>
        public static string GetMINO()
        {
            return DataAccess.StoreManagement.MaterilaIssueDL.GetMINO();
        }

        /// <summary>
        /// Function Used To get MI autocomplete.
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetMISearchValue(string searchBy, string searchValue, User objUser)
        {
            DataTable dtSearch = DataAccess.StoreManagement.MaterilaIssueDL.GetMISearchValue(searchBy, searchValue, objUser);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);
        }

        /// <summary>
        /// Function Used To delete MI details.
        /// </summary>
        /// <param name="requestPk"></param>
        /// <returns></returns>
        public static string DeleteMaterialIssue(int miPk)
        {
            //return DataAccess.StoreManagement.MaterilaIssueDL.DeleteMaterialIssue(miPk).ToString();
            List<object> retvals = new List<object>();
            retvals = DataAccess.StoreManagement.MaterilaIssueDL.DeleteMaterialIssue(miPk);         
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="miPk"></param>
        /// <returns></returns>
        public static string DeleteMRIssue(int miPk)
        {
            //return DataAccess.StoreManagement.MaterilaIssueDL.DeleteMaterialIssue(miPk).ToString();
            List<object> retvals = new List<object>();
            retvals = DataAccess.StoreManagement.MaterilaIssueDL.DeleteMRIssue(miPk);
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }
        public static string DeleteMRIssuePlantToPlant(int miPk)
        {
            //return DataAccess.StoreManagement.MaterilaIssueDL.DeleteMaterialIssue(miPk).ToString();
            List<object> retvals = new List<object>();
            retvals = DataAccess.StoreManagement.MaterilaIssueDL.DeleteMRIssuePlantToPlant(miPk);
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

        // Sumesh 07112011
        /// <summary>
        /// Method to get All Stores
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="category"></param>
        /// <param name="deptType"></param>
        /// <returns></returns>
        public static string GetAllStores(User objUser, int sbuPk, int category)
        {
            DataTable dtStores = DataAccess.StoreManagement.MaterilaIssueDL.GetAllStores( objUser,  sbuPk,  category);
            return GTIService.CommonFunctions.GetTextValueList(dtStores, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTTEXTFIELD, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTVALUEFIELD);
        }

        public static string GetMaterialIssueForRateAdjustment(string FromDate, string ToDate, int bizunit, int DeptPK)
        {
            return DataAccess.StoreManagement.MaterilaIssueDL.GetMaterialIssueForRateAdjustment(FromDate, ToDate, bizunit, DeptPK);
        }

        public static int? SaveMaterialIssueRateAdjustment(string xmlDoc, out int refID, out string transNo)
        {
            return DataAccess.StoreManagement.MaterilaIssueDL.SaveMaterialIssueRateAdjustment(xmlDoc, out refID, out transNo);
        }

        public static DataSet GetRateAdjustmentList(int bizunit, int transPK, string transDate, BusinessObject.GridPrams gridParamObj, int DeptPK)
        {
            return DataAccess.StoreManagement.MaterilaIssueDL.GetRateAdjustmentList(bizunit, transPK, transDate, gridParamObj, DeptPK);
        }

        public static string GetRateAdjustmentByPK(int TranPK)
        {
            return DataAccess.StoreManagement.MaterilaIssueDL.GetRateAdjustmentByPK(TranPK);
        }

        public static DataTable GetMIRateAdjustNumbers(int bizunit, string searchKey)
        {
            return DataAccess.StoreManagement.MaterilaIssueDL.GetMIRateAdjustNumbers(bizunit, searchKey);
        }
    }
}
