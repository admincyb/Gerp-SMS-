using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.PurchaseOrderManagement
{
    public class PurchaseOrderGeneration
    {
         /// <summary>
        /// Function Used To get the running po number
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>purchase order</for>
        /// Used in fill the po number
        /// <returns></returns>
        public static string GetPONumber()
        {
            return DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.GetPONumber();
        }

        
        /// <summary>
        /// Function Used Save/update PO Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>purchase order Creation</for>
        /// Used in Saving and updating PO Details
        /// <returns></returns>
        public static string SavePODetails(string xmlPoDetails, BusinessObject.User objUser)
        {
            string POID = string.Empty;
            WorkflowCore.CoreObjects.DoWorkFlowRequest objRequest = new WorkflowCore.CoreObjects.DoWorkFlowRequest();
            objRequest = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkflowCore.CoreObjects.DoWorkFlowRequest>(xmlPoDetails);
            objRequest.UserPK = objUser.PKUser;

            string xmlstr = GTIService.CommonFunctions.JsonToXml(xmlPoDetails);
            string poNumber = string.Empty;
            POID = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.SavePODetails(xmlstr,out poNumber).ToString();
            //used to update the files to Permanenet location
            if (Convert.ToInt32(POID) != 0)
            {
                //used to update the files to Permanenet location
                BusinessObject.CommonManagement.CommonObject.File fileObject = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.File>(xmlPoDetails);
                for (int i = 0; i < fileObject.FILELIST.Count; i++)
                {
                 if (fileObject.FILELIST[i].DOC_PK == 0)
                    {
                        if (fileObject.FILELIST[i].DOC_TITLE != string.Empty)
                            {
                                if (CommonManagement.CommonManagement.UploadFromTempFolderForXml(fileObject.FILELIST[i].DOC_NAME, "Purchase"))
                                {

                                }
                            }
                    }
                }
                
            }

            //Need to Do workflow only on submit if Draft we skip the workflow step
            if (objRequest.ActionID != 0)
            {
                objRequest.ApplicationID = Convert.ToInt32(POID);
                WorkflowCore.CoreService obj = new WorkflowCore.CoreService();
                int ReferenceID = obj.DoWorkFlow(objRequest);
                //Used to Save the comments for workflow
                if (ReferenceID > 0)
                {
                    BusinessObject.CommonManagement.CommonObject.WorkFlowComment workFlowComment = new BusinessObject.CommonManagement.CommonObject.WorkFlowComment();
                    workFlowComment = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.CommonManagement.CommonObject.WorkFlowComment>(xmlPoDetails);
                    workFlowComment.ApplicationID = Convert.ToInt32(POID);
                    workFlowComment.UserPk = objUser.PKUser;
                    workFlowComment.ReferenceID = ReferenceID;
                    if (workFlowComment.WrkfComment != string.Empty)
                    {
                        BusinessLogic.CommonManagement.CommonManagement.SaveWrkfCommentList(workFlowComment);
                    }
                }
            }

            return poNumber;
        }


        public static string SavePONonStockDetails(string xmlPoDetails, BusinessObject.User objUser)
        {
            string POID = string.Empty;
          

            string poNumber = string.Empty;
            POID = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.SavePODetails(xmlPoDetails, out poNumber).ToString();

            return POID;
        }

        public static List<object> SavePOServiceDetails(string xmlPoDetails, BusinessObject.User objUser)
        {
            List<object> retvals = new List<object>();
            string poNumber = string.Empty;
            retvals = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.SavePOServiceDetails(xmlPoDetails, out poNumber);

            return retvals;
        }
        /// <summary>
        /// Function Used Save PO ServiceDetails using New Workflow SP
        /// </summary>      
        /// <Createdby>Riyas</Createdby>   
        /// Used in Saving and updating PO Details
        /// <returns></returns>
        public static int? SavePOServiceDetailsWkf(string xmlPoDetails, out string poNumber)
        {           
            return DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.SavePOServiceDetailsWkf(xmlPoDetails, out poNumber);
        }

        /// <summary>
        /// Function Used Get PO Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>purchase order Creation</for>
        /// Used in Saving and updating PO Details
        /// <returns></returns>
        public static string GetPODetails(int pOID)
        {
            try
            {
                //BusinessObject.OrderManagement.OrderMaster obj = new BusinessObject.OrderManagement.OrderMaster();
                return GTIService.CommonFunctions.XmlToJson(DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.GetPODetails(pOID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }


        public static string CreatePRData()
        {
            //PRH_PK,PRH_NO,PRH_DATE,PRD_QTY_APPROVED,QTY_BALANCE,PRD_UOM,UOM_NAME,QTY_ORDER

            object[] lstFields = new object[] { "PRH_PK", "PRH_NO", "PRH_DATE", "PRD_QTY_APPROVED", "QTY_BALANCE", "QTY_ORDERED","PRD_UOM", "UOM_NAME", "QTY_ORDER" };
            DataTable dt = GTIService.CommonFunctions.CreateDataTable(lstFields);

            DataRow dr;
            dr = dt.NewRow();
            dr["PRH_PK"] = "1";
            dr["PRH_NO"] = "PR-04MAR2011-01";
            dr["PRH_DATE"] = "04-Mar-2011";
            dr["PRD_QTY_APPROVED"] = "50.000";
            dr["QTY_BALANCE"] = "30.000";
            dr["QTY_ORDERED"] = "30.000";
            dr["PRD_UOM"] = "1";
            dr["UOM_NAME"] = "Kg";
            dr["QTY_ORDER"] = "0.000";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["PRH_PK"] = "2";
            dr["PRH_NO"] = "PR-04MAR2011-02";
            dr["PRH_DATE"] = "05-Mar-2011";
            dr["PRD_QTY_APPROVED"] = "50.000";
            dr["QTY_BALANCE"] = "50.000";
            dr["QTY_ORDERED"] = "50.000";
            dr["PRD_UOM"] = "1";
            dr["UOM_NAME"] = "Kg";
            dr["QTY_ORDER"] = "0.000";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["PRH_PK"] = "3";
            dr["PRH_NO"] = "PR-04MAR2011-03";
            dr["PRH_DATE"] = "06-Mar-2011";
            dr["PRD_QTY_APPROVED"] = "30.000";
            dr["QTY_BALANCE"] = "30.000";
            dr["QTY_ORDERED"] = "30.000";
            dr["PRD_UOM"] = "1";
            dr["UOM_NAME"] = "Kg";
            dr["QTY_ORDER"] = "0.000";
            dt.Rows.Add(dr);

            return  Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            
        }


        /// <summary>
        /// Function Used Get PR Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>purchase order Creation</for>
        /// Used in Saving and updating PO Details
        /// <returns></returns>
        public static string GetPRDetails(int poID, int itemID,int uom,int bizUnit)
        {

            return Newtonsoft.Json.JsonConvert.SerializeObject(DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.GetPRDetails(poID,itemID,uom,bizUnit)); 
        }

       

        #region For PO LIsting

        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchBy, string searchValue,int processPK, User objUser)
        {
            DataTable dtSearch = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.GetSearchValues(searchBy, searchValue, processPK, objUser);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);

        }
        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="pageURL"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string GetPurchaseAutoSearchValue(string searchBy, string searchValue, string pageURL, User objUser, int type = 0)
        {
            DataTable dtSearch = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.GetPurchaseAutoSearchValue(searchBy, searchValue, pageURL, objUser, type);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);

        }

        /// <summary>
        /// Returns the PO list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetPurchaseOrderDetails(GridPrams grid, int sbuID, int procID, string pageUrl, byte orderGroup)
        {
            grid.SortBy = grid.SortBy == null ? "POH_DATE" : grid.SortBy;
            grid.SortDirection = grid.SortDirection == null ? "Desc" : grid.SortDirection;
            grid.SearchBy = grid.SearchBy == "0" ? "POH_NO" : grid.SearchBy;
            grid.SearchValue = grid.SearchValue == "" ? "%" : grid.SearchValue;
            DataSet dsPOList = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.GetPurchaseOrderDetails(grid, sbuID, procID, pageUrl, orderGroup);

            string jString = string.Empty;
            if (dsPOList.Tables.Count > 1 && dsPOList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPOList);
            }
            return jString;
        }
        /// <summary>
        /// Get PO List Search value
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <param name="pageUrl"></param>
        /// <param name="transactionStatus"></param>
        /// <param name="vendor"></param>
        /// <param name="POhNo"></param>
        /// <param name="PRNo"></param>
        /// <param name="IONo"></param>
        /// <param name="reqStore"></param>
        /// <returns></returns>
        public static string GetPurchaseOrderList(GridPrams grid, int sbuID, string pageUrl, int status, int transactionStatus, int vendor, string POhNo, string PRNo, string IONo, int reqStore, byte orderGroup,string ItemName,int cmpPk=0)
        {
            grid.SortBy = grid.SortBy == null ? "POH_DATE" : grid.SortBy;
            grid.SortDirection = grid.SortDirection == null ? "Desc" : grid.SortDirection;
            grid.SearchBy = grid.SearchBy == "0" ? "POH_NO" : grid.SearchBy;
            grid.SearchValue = grid.SearchValue == "" ? "%" : grid.SearchValue;
            DataSet dsPOList = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.GetPurchaseOrderList(grid, sbuID, pageUrl, status, transactionStatus, vendor, POhNo, PRNo, IONo, reqStore, orderGroup, ItemName,cmpPk);

            string jString = string.Empty;
            if (dsPOList.Tables.Count > 1 && dsPOList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPOList);
            }
            return jString;
        }

         /// <summary>
        /// Returns the Pending PO list with respect to material id 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetPendingPODetails(GridPrams grid, int materialid, int toUOM=0)
        {
            grid.SortBy = grid.SortBy == null ? "POH_DATE" : grid.SortBy;
            grid.SortDirection = grid.SortDirection == null ? "Desc" : grid.SortDirection;
            grid.SearchBy = grid.SearchBy == "0" ? "POH_NO" : grid.SearchBy;
            grid.SearchValue = grid.SearchValue == "" ? "%" : grid.SearchValue;

            DataTable dtPOList = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.GetPendingPurchaseOrderDetails(grid, materialid, toUOM);
            string jString = string.Empty;
            if (dtPOList.Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dtPOList);
            }
            return jString;
        }


        /// <summary>
        /// Logic Methord used to delete a PO details by passing PO ID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>string</returns>
        public static string DeletePODetails(int poID)
        {
            return DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.DeletePODetails(poID).ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poDetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SavePOShortClose(string poDetails, User objUser)
        {
            BusinessObject.PurchaseOrderGeneration.POShortClose poShortClose = Newtonsoft.Json.JsonConvert.DeserializeObject<BusinessObject.PurchaseOrderGeneration.POShortClose>(poDetails);
            poShortClose.UserPk = objUser.PKUser;
            return DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.SavePOShortClose(poShortClose).ToString();
        }

        /// <summary>
        /// Returns the PO list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetPurchaseOrderDetailsView(int poID, int sbuID)
        {
            DataSet dsPOList = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.GetPurchaseOrderDetailsView(poID, sbuID);
            string jString = string.Empty;
            if (dsPOList.Tables.Count > 1 && dsPOList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPOList);
            }
            return jString;
        }

        /// <summary>
        /// Get PO List Trading Search value
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="sbuID"></param>
        /// <param name="pageUrl"></param>
        /// <param name="transactionStatus"></param>
        /// <param name="vendor"></param>
        /// <param name="POhNo"></param>
        /// <param name="PRNo"></param>
        /// <param name="IONo"></param>
        /// <param name="reqStore"></param>
        /// <returns></returns>
        public static string GetPurchaseOrderListTrading(GridPrams grid, int sbuID, string pageUrl, int status, int transactionStatus, int vendor, string POhNo, string PRNo, string IONo, int reqStore, byte orderGroup, string ItemName)
        {
            grid.SortBy = grid.SortBy == null ? "POH_DATE" : grid.SortBy;
            grid.SortDirection = grid.SortDirection == null ? "Desc" : grid.SortDirection;
            grid.SearchBy = grid.SearchBy == "0" ? "POH_NO" : grid.SearchBy;
            grid.SearchValue = grid.SearchValue == "" ? "%" : grid.SearchValue;
            DataSet dsPOList = DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.GetPurchaseOrderListTrading(grid, sbuID, pageUrl, status, transactionStatus, vendor, POhNo, PRNo, IONo, reqStore, orderGroup, ItemName);

            string jString = string.Empty;
            if (dsPOList.Tables.Count > 1 && dsPOList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPOList);
            }
            return jString;
        }
        /// <summary>
        /// Function Used Get PO Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>purchase order Creation</for>
        /// Used in Saving and updating PO Details
        /// <returns></returns>
        public static string GetPODetailsTrading(int pOID)
        {
            try
            {
                //BusinessObject.OrderManagement.OrderMaster obj = new BusinessObject.OrderManagement.OrderMaster();
                return GTIService.CommonFunctions.XmlToJson(DataAccess.PurchaseOrderManagement.PurchaseOrderGeneration.GetPODetailsTrading(pOID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Function Used To get Item,Category,Requested Qty,Ordered Qty,Rcvd Qty,Accepted Qty,Invoiced Qty Details against PR id
        /// </summary>
        /// <param name="Requisition"></param>
        /// <returns></returns>
        public static DataTable GetPOListItemDetails(int transTYPE, int transPK)
        {
            return DataAccess.PurchaseOrderManagement.PurchaseOrderGenerateDL.GetPOListItemDetails(transTYPE, transPK);
        }
        #endregion


    }
}
