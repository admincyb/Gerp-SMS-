using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using BusinessObject;
using System.Data;

namespace BusinessLogic.StoreManagement
{
    public class StoreRequisitionSlipCreation
    {
        /// <summary>
        /// Save Requisition Details -  Details
        /// </summary>
        /// <param name="requisitionDetails"></param>
        /// <returns>string</returns>
        public static string SaveRequisitionDetails(string requisitionDetails, User objUser)
        {
            string requisitionID = string.Empty;
            List<object> retvals = new List<object>();
            string xmlstr = GTIService.CommonFunctions.JsonToXml(requisitionDetails);
            retvals = DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.SaveRequisitionDetails(xmlstr);
            requisitionID = retvals[0].ToString();
            string jString = Newtonsoft.Json.JsonConvert.SerializeObject(retvals);
            return jString;
        }
        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns>string</returns>
        public static string GetSearchValues(string searchValue)
        {
            DataTable dtSearch = DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.GetSearchValues(searchValue);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Store.Fields_RequisitionSlip.REQUISITIONSEARCHVALUEFIELD, GTIService.Constants.Store.Fields_RequisitionSlip.REQUISITIONSEARCHTEXTFIELD);
        }
        /// <summary>
        /// Get stores
        /// </summary>
        /// <param name="user"></param>
        /// <returns>string</returns>
        public static string GetStores(User objUser, int sbuPk, int flag)
        {
            DataTable dtStores = DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.GetStores(objUser, sbuPk, flag);
            return GTIService.CommonFunctions.GetTextValueList(dtStores, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTTEXTFIELD, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTVALUEFIELD);
        }
        /// <summary>
        /// Get stores  - For SRS
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="flag"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        

        public static string GetStores(User objUser, int sbuPk, int flag,int category)
        {
            DataTable dtStores = DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.GetStores(objUser, sbuPk, flag, category);
            return GTIService.CommonFunctions.GetTextValueList(dtStores, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTTEXTFIELD, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTVALUEFIELD);
        }
        public static DataTable GetAllStores(User objUser, int sbuPk, int flag)
        {
            return DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.GetStores(objUser, sbuPk, flag);
        }
        /// <summary>
        /// Returns SRS No in json string format
        /// </summary>
        /// <param name=""></param>
        /// <returns>String</returns>
        public static string GetSRSNo()
        {
            return DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.GetSRSNo();

        }
        /// <summary>
        /// function used to store requisition report
        /// </summary>
        /// <param name="Requisition"></param>
        /// <returns></returns>
        public static DataSet GetRequisitionReportByReqId(int Requisition)
        {

            DataSet dsetRequisitionReport = DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.GetRequisitionReportByReqId(Requisition);
            return dsetRequisitionReport;

        }
        /// <summary>
        /// Function Used To get previous srs details refered by rijoy in MA
        /// </summary>
        /// <param name="srsID"></param>
        /// <returns></returns>
        public static string GetSRSDetails(int srsID)
        {
            DataSet dsSRSList = DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.GetSRSDetails(srsID);
            string jString = string.Empty;
            if (dsSRSList.Tables.Count > 1 && dsSRSList.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsSRSList);
            }
            return jString;
        }

        /// <summary>
        /// Method to get All Store Name By Type
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <param name="deptType"></param>
        /// <param name="deptPk"></param>
        /// <returns></returns>
        public static string GetStoresByType(User objUser, int sbuPk, int deptType, int deptPk, int deptCompany=0)
        {
            DataTable dtStores = DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.GetStoresByType(objUser, sbuPk, deptType, deptPk,deptCompany);
            return GTIService.CommonFunctions.GetTextValueList(dtStores, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTTEXTFIELD, GTIService.Constants.Store.Fields_RequisitionSlip.STRDPTVALUEFIELD);
        }

        /// <summary>
        /// Method for Get User Privilege Departments For SRS
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="sbuPk"></param>
        /// <returns></returns>
        public static string GetDepartmentDtls(User objUser, int sbuPk)
        {
            DataTable dtSearch = DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.GetDepartmentDtls(objUser, sbuPk);
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.DEPTNAME, GTIService.Constants.Designation.Fields.DEPTPK).ToString();

        }

        public static string FillOtherSBUS(User objUser, int Active)
        {
            DataTable dtSearch = DataAccess.StoreManagement.StoreRequisitionSlipCreationDL.FillOtherSBUS(objUser, Active );
            dtSearch.Rows.Remove(dtSearch.AsEnumerable().Where(r => r.Field<int>("BZU_PK") == objUser.CurrentSBUPK).FirstOrDefault());
            return GTIService.CommonFunctions.GetTextValueList(dtSearch, GTIService.Constants.Designation.Fields.BZU_NAME, GTIService.Constants.Designation.Fields.BZU_PK).ToString();
        }
    }
}
