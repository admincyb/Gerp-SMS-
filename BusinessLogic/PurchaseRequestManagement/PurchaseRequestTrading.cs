using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.PurchaseRequestManagement
{
    public class PurchaseRequestTrading
    {
        /// <summary>
        /// Function Used To get purchase request Trading xml details based on the id
        /// </summary>
        /// <param name="Requisition"></param>
        /// <returns></returns>
        public static string GetPurchaseRequestTradingDetails(int purchaseRequestID)
        {
            return GTIService.CommonFunctions.XmlToJson(DataAccess.PurchaseRequestManagement.PurchaseRequestTradingDL.GetPurchaseRequestTradingDetails(purchaseRequestID));
        }
        /// <summary>
        /// Function Used To get the list of the rol / msl material qty list
        /// </summary>
        /// <returns></returns>
        public static string GetPurchaseRequestTrading(int bizUnit, int dept, int type, int prPK, int rowCount, int itmCategory)
        {
            DataSet dsPucReqList = DataAccess.PurchaseRequestManagement.PurchaseRequestTradingDL.GetPurchaseRequestTrading(bizUnit, dept, type, prPK, rowCount, itmCategory);
            string jString = string.Empty;
            if (dsPucReqList.Tables.Count > 1 && dsPucReqList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsPucReqList);
            }
            return jString;
        }

        /// <summary>
        /// Function Used To save purchase request Trading
        /// </summary>
        /// <param name="purReqDetails"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static string SavePurchaseRequestTradingList(string purReqDetails, User objUser)
        {

            string requisitionID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(purReqDetails);
            retvals = DataAccess.PurchaseRequestManagement.PurchaseRequestTradingDL.SavePurchaseRequestTradingList(xmlstr);
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

    }
}
