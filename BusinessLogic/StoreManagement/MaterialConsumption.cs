using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BusinessObject;
using System.Data;

namespace BusinessLogic.StoreManagement
{
   public class MaterialConsumption
    {
        /// <summary>
        /// Save Requisition Details -  Details
        /// </summary>
        /// <param name="requisitionDetails"></param>
        /// <returns>string</returns>
        public static string SaveConsumptionDetails(string requisitionDetails, User objUser)
        {
            string requisitionID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(requisitionDetails);
            retvals = DataAccess.StoreManagement.MaterialConsumptionDL.SaveConsumptionDetails(xmlstr);
            requisitionID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }
        /// <summary>
        /// Returns ICH No in json string format
        /// </summary>
        /// <param name=""></param>
        /// <returns>String</returns>
        public static string GetICHNo()
        {
            return DataAccess.StoreManagement.MaterialConsumptionDL.GetICHNo();

        }
        /// <summary>
        /// Returns requisition list in json string format
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>string</returns>
        public static string GetConsumptionList(GridPrams grid, int bizUnit, User objUser, int procID)
        {
            DataSet dsRequisitionList = DataAccess.StoreManagement.MaterialConsumptionDL.GetConsumptionList(grid, bizUnit, objUser, procID);
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
        public static string GetConsumptionDetails(int Consumption)
        {

            BusinessObject.StoreManagement.MaterialConsumptionCreation obj = new BusinessObject.StoreManagement.MaterialConsumptionCreation();
            return GTIService.CommonFunctions.XmlToJson(DataAccess.StoreManagement.MaterialConsumptionDL.GetConsumptionDetails(Consumption));

        }
        /// <summary>
        /// Delete requisition Details
        /// </summary>
        /// <param name="MRHPK"></param>
        /// <returns>String</returns>
        public static string DeleteConsumption(int ICHPK)
        {
            return DataAccess.StoreManagement.MaterialConsumptionDL.DeleteConsumptionDtls(ICHPK).ToString();
        }
        /// <summary>
        /// function used to store requisition report
        /// </summary>
        /// <param name="Requisition"></param>
        /// <returns></returns>
        public static DataSet GetConsumptionReportByReqId(int RequistID)
        {

            DataSet dsetRequisitionReport = DataAccess.StoreManagement.MaterialConsumptionDL.GetConsumptionReportByReqId(RequistID);
            return dsetRequisitionReport;

        }

        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchBy, string searchValue, int sbuPk, User objUser, int procID)
        {
            DataTable dtSearch = DataAccess.StoreManagement.MaterialConsumptionDL.GetSearchValues(searchBy, searchValue, sbuPk, objUser, procID);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Store.Fields_MaterialConsumption.REQUISITIONSEARCHVALUEFIELD, GTIService.Constants.Store.Fields_MaterialConsumption.REQUISITIONSEARCHTEXTFIELD);

        }

    }
}
