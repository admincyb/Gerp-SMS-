
using System;
using System.Collections.Generic;
using System.Data;
using BusinessObject;

namespace BusinessLogic.StoreManagement
{
    public class GoodsInspectionNote
    {
        # region Methods

        /// <summary>
        /// Get Auto Complete Searchj For GRN Number
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="searchCorr"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetPendingSearchAuto(string searchBy, string searchValue, int searchCorr, User objUser, int IsStockItem)
        {
            DataTable dtSearch = DataAccess.StoreManagement.GoodsInspectionNote.GetPendingGRNSearchAuto(searchBy, searchValue, searchCorr, objUser, IsStockItem);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);
        }
        /// <summary>
        /// Get Grn Store
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        public static DataTable GetGrnStore(int appID)
        {
            return DataAccess.StoreManagement.GoodsInspectionNote.GetGrnStore(appID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appID"></param>
        /// <returns></returns>
        public static DataTable GetGinStore(int appID)
        {
            return DataAccess.StoreManagement.GoodsInspectionNote.GetGinStore(appID);
        }

        /// <summary>
        /// Get GRN Number 
        /// </summary>
        /// <returns></returns>
        public static string GetGINO()
        {
            return DataAccess.StoreManagement.GoodsInspectionNote.GetGINNumber();
        }

        /// <summary>
      /// Get GRN Item Pending List, To Inpsection
      /// </summary>
      /// <param name="grid"></param>
      /// <param name="sbuID"></param>
      /// <param name="store"></param>
      /// <returns></returns>
        public static string GetGRNItemPending(GridPrams grid, int sbuID, int store, int ginPk, int grhPk,int IsStockItem)
        {
            DataSet dsGRNItemList = DataAccess.StoreManagement.GoodsInspectionNote.GetGRNItemPending(grid, sbuID, store, ginPk, grhPk, IsStockItem);
            string jString = string.Empty;
            if (dsGRNItemList.Tables.Count > 1 && dsGRNItemList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsGRNItemList);
            }
            return jString;
        }
      
        /// <summary>
        /// Save GIn Details
        /// </summary>
        /// <param name="goodsReceiptNoteDetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SaveGoodsInspectionNote(string goodsReceiptNoteDetails, User objUser)
        {
            string ginID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(goodsReceiptNoteDetails);
            retvals = DataAccess.StoreManagement.GoodsInspectionNote.SaveGoodsInspectionNote(xmlstr);
            ginID = retvals[0].ToString();
            if (Convert.ToInt32(ginID) > 0)
            {
                BusinessObject.CommonManagement.CommonObject.File fileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.File>(goodsReceiptNoteDetails);
                for (int i = 0; i < fileObject.FILELIST.Count; i++)
                {
                    if (fileObject.FILELIST[i].DOC_PK == 0 && fileObject.FILELIST[i].DOC_TITLE != string.Empty)
                    {
                        CommonManagement.CommonManagement.UploadFromTempFolderForXml(fileObject.FILELIST[i].DOC_NAME, "GIN");
                    }
                }
            }

            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;

            
        }

        /// <summary>
        /// Get Good Inspection Note Details By PK
        /// </summary>
        /// <param name="ginID"></param>
        /// <returns></returns>
        public static string GetGoodsInspectionNoteDetails(int ginID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.StoreManagement.GoodsInspectionNote.GetGoodsInspectionNoteDetails(ginID));
        }

        /// <summary>
        /// Get Good Inpection Note Details List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetGoodsInspectionNoteList(GridPrams grid, User objUser, int procID, string PageUrl, string ginNo, string grnNo, string poNo, int venPK, string depName, int filterStatus, int cmpPk = 0)
        {
            DataSet dsGINList = DataAccess.StoreManagement.GoodsInspectionNote.GetGoodsInspectionNoteList(grid, objUser, procID, PageUrl,ginNo, grnNo, poNo, venPK, depName, filterStatus,cmpPk);
            string jString = string.Empty;
            if (dsGINList.Tables.Count > 1 && dsGINList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsGINList);
            }
            return jString;
        }

        /// <summary>
       /// GIn AutoComplete Search
       /// </summary>
       /// <param name="searchBy"></param>
       /// <param name="searchValue"></param>
       /// <param name="objUser"></param>
       /// <returns></returns>
        public static string GetGINSearchValue(string searchBy, string searchValue, User objUser,string pageUrl)
        {
            DataTable dtSearch = DataAccess.StoreManagement.GoodsInspectionNote.GetGINSearchValue(searchBy, searchValue, objUser,pageUrl);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.VALUE, GTIService.Constants.Designation.Fields.PK);
        }

        /// <summary>
        /// Delete GIN Details By GIN Pk
        /// </summary>
        /// <param name="grnPk"></param>
        /// <returns></returns>
        public static string DeleteGoodsInspectionNote(int grnPk)
        {
            return DataAccess.StoreManagement.GoodsInspectionNote.DeleteGoodsInspectionNote(grnPk).ToString();
        }


        /// <summary>
        /// Get GIN Details By GIN Pk for Print
        /// </summary>
        /// <param name="grnPk"></param>
        /// <returns></returns>
        public static DataSet GetGoodsInspectionNoteTables(int grnPk)
        {
            return DataAccess.StoreManagement.GoodsInspectionNote.GetGoodsInspectionNoteTables(grnPk);
        }

        /// <summary>
        /// Get GIN Already Inspected Details
        /// </summary>
        /// <param name="ginID"></param>
        /// <returns></returns>
        public static string GetGINInspectedDetails(int grnDtlPk, int status, int sbu, int store, int ginPk)
        {
            DataSet dsDtls = DataAccess.StoreManagement.GoodsInspectionNote.GetGINAlreadyInspectedDetails(grnDtlPk, status, sbu, store, ginPk);
            string jString = string.Empty;
            if (dsDtls.Tables.Count > 1 && dsDtls.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsDtls);
            }
            return jString;
           
        }
        # endregion
    }
}

