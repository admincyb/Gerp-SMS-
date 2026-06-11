using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.PurchaseOrderManagement
{
    public class PurchaseOrderCreation
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
            return DataAccess.PurchaseOrderManagement.PurchaseOrderCreation.GetPONumber();
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
            POID = DataAccess.PurchaseOrderManagement.PurchaseOrderCreation.SavePODetails(xmlstr,out poNumber).ToString();
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
            }

            return poNumber;
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
                return GTIService.CommonFunctions.XmlToJson(DataAccess.PurchaseOrderManagement.PurchaseOrderCreation.GetPODetails(pOID));
            }
            catch (Exception ex)
            {
                NLog.Logger logger = NLog.LogManager.GetLogger("Order Creation");
                logger.Log(NLog.LogLevel.Fatal, "Error Occured in " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get PurchaseOrderDetails For Report
        /// </summary>
        /// <param name="purchaseID"></param>
        /// <returns>DataSet</returns>
        public static DataSet PurchaseOrderDetails(int purchaseID, string PurchaseOrderOutRPTSP)
        {
            return DataAccess.PurchaseOrderManagement.PurchaseOrderCreation.GetPurchaseOrderDtls(purchaseID, PurchaseOrderOutRPTSP);
        }

        public static DataSet PurchaseOrderDetailsDOCNOREVISION(int purchaseID, string PurchaseOrderOutRPTSP,int reportpk)
        {
            return DataAccess.PurchaseOrderManagement.PurchaseOrderCreation.PurchaseOrderDetailsDOCNOREVISION(purchaseID, PurchaseOrderOutRPTSP,reportpk);
        }
        public static DataSet PurchaseOrderDetails(int purchaseID,int RevID)
        {
            return DataAccess.PurchaseOrderManagement.PurchaseOrderCreation.GetPurchaseOrderDtls(purchaseID,RevID);
        }

        /// <summary>
        /// Get Current User Address For Report
        /// </summary>
        /// <param name="P_BZU_PK"></param>
        /// <param name="P_ACTIVE"></param> 
        /// <returns>DataSet</returns>
        public static DataSet GetCurrentSBUAddress(int BzuPk,int Active)
        {
            return DataAccess.PurchaseOrderManagement.PurchaseOrderCreation.GetCurrentSBUAddress(BzuPk,Active);
        }

        #region For PO LIsting

        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchBy, string searchValue, User objUser)
        {
            DataTable dtSearch = DataAccess.PurchaseOrderManagement.PurchaseOrderCreation.GetSearchValues(searchBy, searchValue, objUser);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Common.Fields.SEARCHTEXTFIELD, GTIService.Constants.Common.Fields.SEARCHVALUEFIELD);

        }

        /// <summary>
        /// Returns the PO list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetPurchaseOrderDetails(GridPrams grid, int sbuID)
        {
            DataSet dsPOList = DataAccess.PurchaseOrderManagement.PurchaseOrderCreation.GetPurchaseOrderDetails(grid, sbuID);
            string jString = string.Empty;
            if (dsPOList.Tables.Count > 1 && dsPOList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPOList);
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
            return DataAccess.PurchaseOrderManagement.PurchaseOrderCreation.DeletePODetails(poID).ToString();
        }


        #endregion
    }
}
