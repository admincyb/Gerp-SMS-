using System.Data;
using BusinessObject;

namespace BusinessLogic.StoreManagement
{
  public  class StoreRequisitionSlipList
  {
      #region Methods
      /// <summary>
      /// Returns the search result list for Autocomplete 
      /// </summary>
      /// <param name="searchBy"></param>
      /// <param name="searchValue"></param>
      /// <returns>string</returns>
      public static string GetSearchValues(string searchBy, string searchValue, int sbuPk, User objUser, int procID)
      {
          DataTable dtSearch = DataAccess.StoreManagement.StoreRequisitionSlipDL.GetSearchValues(searchBy, searchValue, sbuPk, objUser, procID);
          return GTIService.CommonFunctions.GetTextValueList(dtSearch,  GTIService.Constants.Store.Fields_RequisitionSlip.REQUISITIONSEARCHVALUEFIELD,GTIService.Constants.Store.Fields_RequisitionSlip.REQUISITIONSEARCHTEXTFIELD);

      }
      /// <summary>
      /// Returns requisition list in json string format
      /// </summary>
      /// <param name="grid"></param>
      /// <returns>string</returns>
      public static string GetRequisitionList(GridPrams grid, int bizUnit, User objUser, int procID, string PageUrl, int DeptPK=0)
      {
          DataSet dsRequisitionList = DataAccess.StoreManagement.StoreRequisitionSlipDL.GetRequisitionList(grid, bizUnit, objUser, procID, PageUrl, DeptPK);
          string jString = string.Empty;
          if (dsRequisitionList.Tables.Count > 1 && dsRequisitionList.Tables[1].Rows.Count > 0)
          {
              jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsRequisitionList);
          }
          return jString;
      }
      /// <summary>
      /// Delete requisition Details
      /// </summary>
      /// <param name="MRHPK"></param>
      /// <returns>String</returns>
      public static string DeleteRequisition(int MRHPK)
      {
          return DataAccess.StoreManagement.StoreRequisitionSlipDL.DeleteRequisitionDtls(MRHPK).ToString();
      }
      /// <summary>
      /// Get Requisition Details As a XML Format and Convert in to JSON and Return As a JSON String Format
      /// </summary>
      /// <param name="RequisitionID"></param>
      /// <returns></returns>
      public static string GetRequisitionDetails(int Requisition)
      {

          BusinessObject.StoreManagement.StoreRequisitionSlipCreation obj = new BusinessObject.StoreManagement.StoreRequisitionSlipCreation();
          return GTIService.CommonFunctions.XmlToJson(DataAccess.StoreManagement.StoreRequisitionSlipDL.GetRequisitionDetails(Requisition));
      
      }

      public static DataTable GetInvStoreDepartment(bool active, int parent, int type, int category)
      {
          DataTable dtInvStoreDpt = DataAccess.StoreManagement.StoreRequisitionSlipDL.GetInvStoreDepartment(active, parent, type, category);
          return dtInvStoreDpt;//GTIService.CommonFunctions.GetTextValueList(dtInvStoreDpt, GTIService.Constants.Store.Fields_RequisitionSlip.REQUISITIONSEARCHVALUEFIELD, GTIService.Constants.Store.Fields_RequisitionSlip.REQUISITIONSEARCHTEXTFIELD);
      }
      #endregion
  }
}
