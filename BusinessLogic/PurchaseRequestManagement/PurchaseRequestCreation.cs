using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using DataAccess.Administration.Masters;

namespace BusinessLogic.PurchaseRequestManagement
{
    public class PurchaseRequestCreation
    {
        /// <summary>
        /// Function Used To get purchase request xml details based on the id
        /// </summary>
        /// <param name="Requisition"></param>
        /// <returns></returns>
        public static string GetPurchaseRequestDetails(int purchaseRequestID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.GetPurchaseRequestDetails(purchaseRequestID));
        }

        public static string GetForPlanningRequestDetails(int RequestID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.GetForPlanningRequestDetails(RequestID));
        }

        public static string FillForMaterialPlanningPurchase(int RequestID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.FillForMaterialPlanningPurchase(RequestID));
        }
        

        public static string GetMaterialRequestDetails(int matrialRequestID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.GetMaterialRequestDetails(matrialRequestID));
        }

        /// <summary>
        /// Function Used To save purchase request
        /// </summary>
        /// <param name="purReqDetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SavePurchaseRequestDetails(string purReqDetails, User objUser)
        {
           
            string requisitionID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(purReqDetails);
            retvals = DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.SavePurchaseRequestDetails(xmlstr);
            requisitionID = retvals[0].ToString();
            if (Convert.ToInt32(retvals[0]) != 0)
            {
                BusinessObject.CommonManagement.CommonObject.File fileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.File>(purReqDetails);
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

        public static string SaveMaterialRequestDetails(string materialReqDetails, User objUser)
        {

            string requisitionID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(materialReqDetails);
            retvals = DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.SaveMaterialRequestDetails(xmlstr);
            requisitionID = retvals[0].ToString();
            if (Convert.ToInt32(retvals[0]) != 0)
            {
                BusinessObject.CommonManagement.CommonObject.File fileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.File>(materialReqDetails);
                for (int i = 0; i < fileObject.FILELIST.Count; i++)
                {
                    if (fileObject.FILELIST[i].DOC_PK == 0 && fileObject.FILELIST[i].DOC_TITLE != string.Empty)
                    {
                        CommonManagement.CommonManagement.UploadFromTempFolderForXml(fileObject.FILELIST[i].DOC_NAME, "MatrialRequest");
                    }
                }
            }
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

        public static string SaveMaterialRequestPlantToPlant(string materialReqDetails, User objUser)
        {

            string requisitionID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(materialReqDetails);
            retvals = DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.SaveMaterialRequestPlantToPlant(xmlstr);
            requisitionID = retvals[0].ToString();
            if (Convert.ToInt32(retvals[0]) != 0)
            {
                BusinessObject.CommonManagement.CommonObject.File fileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.File>(materialReqDetails);
                for (int i = 0; i < fileObject.FILELIST.Count; i++)
                {
                    if (fileObject.FILELIST[i].DOC_PK == 0 && fileObject.FILELIST[i].DOC_TITLE != string.Empty)
                    {
                        CommonManagement.CommonManagement.UploadFromTempFolderForXml(fileObject.FILELIST[i].DOC_NAME, "MatrialRequest");
                    }
                }
            }
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

        /// <summary>
        /// Function Used To get the list of the rol / msl material qty list
        /// </summary>
        /// <returns></returns>
        public static string GetPurchaseRequest(int bizUnit, int dept, int type, int prPK, int rowCount, int itmCategory)
        {
            DataSet dsPucReqList = DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.GetPurchaseRequest(bizUnit, dept, type, prPK,rowCount,itmCategory);
            string jString = string.Empty;
            if (dsPucReqList.Tables.Count > 1 && dsPucReqList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPucReqList);
            }
            return jString;
        }
        /// <summary>
        /// Get Packing Materials
        /// </summary>
        /// <param name="sohPK"></param>
        /// <param name="prhPK"></param>
        /// <returns></returns>
        public static string PackingMaterials(int sohPK, int prhPK, int dept, int type,int itmCatSC)
        {
            DataSet dsPucReqList = DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.PackingMaterials(sohPK, prhPK, dept, type, itmCatSC);
            string jString = string.Empty;
            if (dsPucReqList.Tables.Count > 1 && dsPucReqList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPucReqList);
            }
            return jString;
        }

        /// <summary>
        /// Customer Item Request Get
        /// </summary>
        /// <param name="sohPK"></param>
        /// <param name="prhPK"></param>
        /// <param name="dept"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string CustomerItemRequestGet(int sohPK, int prhPK, int dept, int type)
        {
            DataSet dsPucReqList = DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.CustomerItemRequestGet(sohPK, prhPK, dept, type);
            string jString = string.Empty;
            if (dsPucReqList.Tables.Count > 1 && dsPucReqList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPucReqList);
            }
            return jString;
        }
        /// <summary>
        /// Function Used To get the IO Numbers
        /// </summary>
        /// <returns></returns>
        public static string GetIONumber(int bizUnit,int prhPK)
        {
            DataTable dtDept = DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.GetIONumber(bizUnit,prhPK);
            return GTIService.CommonFunctions.GetTextValueList(dtDept, GTIService.Constants.PurchaseRequest.Fields.IOFIELD, GTIService.Constants.PurchaseRequest.Fields.IOPK);
        }



        /// <summary>
        /// Function Used To get puchase request no sequece
        /// </summary>
        /// <returns></returns>
        public static string GetPRNO()
        {
            return DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.GetPRNO();
        }

        public static string GetIONumberAuto(int bizUnit, int prhPK, int sohPk, string srchValue, int deptPk = 0, int isGlove = 0, int showAll = 0,int IsDispatched=1)
        {
            DataTable dtResult = DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.GetIONumberAuto(bizUnit, prhPK, sohPk, srchValue, deptPk, isGlove, showAll,IsDispatched);
            return GTIService.CommonFunctions.GetTextValueList(dtResult, GTIService.Constants.PurchaseRequest.Fields.IOFIELD, GTIService.Constants.PurchaseRequest.Fields.IOPK);

        }

        /// <summary>
        /// Function Used To get Item,Category,Requested Qty,Ordered Qty,Rcvd Qty,Accepted Qty,Invoiced Qty Details against PR id
        /// </summary>
        /// <param name="Requisition"></param>
        /// <returns></returns>
        public static DataTable GetPRListItemDetails(int transTYPE, int transPK)
        {
            return DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.GetPRListItemDetails(transTYPE,transPK);
        }

        public static string GetCostCenterByPK(int cnmPK, int active, int bizUnit, int IssueSubDept = 0)
        {
            DataTable dtCostCenters= CostCenterMasterDA.GetCostCenterByPK(cnmPK, active, bizUnit, IssueSubDept);
            return GTIService.CommonFunctions.GetTextValueList(dtCostCenters, GTIService.Constants.PurchaseRequest.Fields.CCFIELD, GTIService.Constants.PurchaseRequest.Fields.CCPK);
        }
        public static DataTable GetBudgetDetails(int Refid)
        {
            return DataAccess.PurchaseRequestManagement.PurchaseRequestCreationDL.GetBudgetDetails(Refid);
        }
    }
}
