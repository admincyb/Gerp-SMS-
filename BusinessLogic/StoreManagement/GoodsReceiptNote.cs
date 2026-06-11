using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject;
using System.Data;

namespace BusinessLogic.StoreManagement
{
    public class GoodsReceiptNote
    {
        /// <summary>
        /// Function Used To get Goods Receipt Note xml details based on the id
        /// </summary>
        /// <param name="Requisition"></param>
        /// <returns></returns>
        public static string GetGoodsReceiptNoteDetails(int grnID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.StoreManagement.GoodsReceiptNoteDL.GetGoodsReceiptNoteDetails(grnID));
        }
        /// <summary>
        /// Function Used To get Goods Receipt Note xml details based on the id for Print
        /// </summary>
        /// <param name="grnID"></param>
        /// <returns></returns>
        public static string GetGoodsReceiptNoteDetailsReport(int grnID)
        {
            return DataAccess.StoreManagement.GoodsReceiptNoteDL.GetGoodsReceiptNoteDetails(grnID);
        }

        /// <summary>
        /// Function Used To save Goods Receipt Note
        /// </summary>
        /// <param name="goodsReceiptNoteDetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SaveGoodsReceiptNote(string goodsReceiptNoteDetails, User objUser)
        {
            //string goodsReceiptNotePK = string.Empty;
            //WorkflowCore.CoreObjects.DoWorkFlowRequest objRequest = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
            //objRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowCore.CoreObjects.DoWorkFlowRequest>(goodsReceiptNoteDetails);
            //objRequest.UserPK = objUser.PKUser;
            //string xmlstr = GTIService.CommonFunctions.JsonToXml(goodsReceiptNoteDetails);
            //List<object> retvals = DataAccess.StoreManagement.GoodsReceiptNoteDL.SaveGoodsReceiptNote(xmlstr);
            //goodsReceiptNotePK = retvals[0].ToString();
            //if (objRequest.ActionID > 0 && Convert.ToInt32(goodsReceiptNotePK) > 0)
            //{
            //    objRequest.ApplicationID = Convert.ToInt32(goodsReceiptNotePK);
            //    WorkflowCore.CoreService obj = new WorkflowCore.CoreService();
            //    int ReferenceID = obj.DoWorkFlow(objRequest);
            //    if (ReferenceID > 0)
            //    {
            //        BusinessObject.CommonManagement.CommonObject.WorkFlowComment workFlowComment = new BusinessObject.CommonManagement.CommonObject.WorkFlowComment();
            //        workFlowComment = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.WorkFlowComment>(goodsReceiptNoteDetails);
            //        workFlowComment.ReferenceID = ReferenceID;
            //        workFlowComment.ApplicationID = objRequest.ApplicationID;
            //        if (workFlowComment.WrkfComment != string.Empty)
            //        {
            //            BusinessLogic.CommonManagement.CommonManagement.SaveWrkfCommentList(workFlowComment);
            //        }
            //    }
            //}
            //string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            //return jString;
            string requisitionID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(goodsReceiptNoteDetails);
            retvals = DataAccess.StoreManagement.GoodsReceiptNoteDL.SaveGoodsReceiptNote(xmlstr);
            //Fileuploader
            if (Convert.ToInt32(retvals[0]) != 0)
            {
                BusinessObject.CommonManagement.CommonObject.File fileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.File>(goodsReceiptNoteDetails);
                for (int i = 0; i < fileObject.FILELIST.Count; i++)
                {
                    if (fileObject.FILELIST[i].DOC_PK == 0 && fileObject.FILELIST[i].DOC_TITLE != string.Empty)
                    {
                        CommonManagement.CommonManagement.UploadFromTempFolderForXml(fileObject.FILELIST[i].DOC_NAME, "GRN");
                    }
                }
            }

            //End
            
            requisitionID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetPendingSearchAuto(string searchBy, string searchValue, int vendorPK, int grnPK, User objUser)
        {
            DataTable dtSearch = DataAccess.StoreManagement.GoodsReceiptNoteDL.GetPendingSearchAuto(searchBy, searchValue, vendorPK, grnPK, objUser);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static string GetPurchaseOrderPending(GridPrams grid, int sbuID, int vendor, int grnID, int storeID, int pohPK=0)
        {
            DataSet dsPOList = DataAccess.StoreManagement.GoodsReceiptNoteDL.GetPurchaseOrderPending(grid, sbuID, vendor, grnID, storeID, pohPK);
            string jString =  string.Empty;
            if (dsPOList.Tables.Count > 1 && dsPOList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPOList);
            }
            return jString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poID"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static string GetPreviousGRNDetailsView(int poID, int sbuID)
        {
            DataSet dsPOList = DataAccess.StoreManagement.GoodsReceiptNoteDL.GetPreviousGRNDetailsView(poID, sbuID);
            string jString = string.Empty;
            if (dsPOList.Tables.Count > 1 && dsPOList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPOList);
            }
            return jString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static string GetGoodsReceiptNoteList(GridPrams grid, User objUser, int procID, string pageUrl, string grnNo, string poNo, int venPK, string refNo, int filterStatus, int cmpPk = 0)
        {
            DataSet dsGRNList = DataAccess.StoreManagement.GoodsReceiptNoteDL.GetGoodsReceiptNoteList(grid, objUser, procID, pageUrl,grnNo,poNo,venPK,refNo,filterStatus,cmpPk);
            string jString = string.Empty;
            if (dsGRNList.Tables.Count > 1 && dsGRNList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsGRNList);
            }
            return jString;
        }

        /// <summary>
        /// Function Used To get GRN no sequece
        /// </summary>
        /// <returns></returns>
        public static string GetGRN()
        {
            return DataAccess.StoreManagement.GoodsReceiptNoteDL.GetGRN();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetGRNSearchValue(string searchBy, string searchValue, User objUser,string pageUrl)
        {
            DataTable dtSearch = DataAccess.StoreManagement.GoodsReceiptNoteDL.GetGRNSearchValue(searchBy, searchValue, objUser,pageUrl);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestPk"></param>
        /// <returns></returns>
        public static string DeleteGoodsReceiptNote(int grnPk)
        {
            return DataAccess.StoreManagement.GoodsReceiptNoteDL.DeleteGoodsReceiptNote(grnPk).ToString();
        }
        /// <summary>
        /// Get 
        /// </summary>
        /// <param name="ginID"></param>
        /// <param name="sbuID"></param>
        /// <returns></returns>
        public static string GetGRNDetailsView(int ginID, int sbuID)
        {
            DataSet dsPOList = DataAccess.StoreManagement.GoodsReceiptNoteDL.GetGRNDetailsView(ginID, sbuID);
            string jString = string.Empty;
            if (dsPOList.Tables.Count > 1 && dsPOList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPOList);
            }
            return jString;
        }

        public static DataSet GetGRNDetailsForNewReport(int grnID)
        {
            return DataAccess.StoreManagement.GoodsReceiptNoteDL.GetGRNDetailsForNewReport(grnID);
        }
    }
}
