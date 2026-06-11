using System;
using System.Data;
using BusinessObject;
using System.Collections.Generic;

namespace DataAccess.StoreManagement
{
  /// <summary>
  /// This class is used to communicate with data access layer.
  /// </summary>
    public class NewItemRequestDL
  {
      #region Methods

      /// <summary>
      /// SAVING New Item DETAILS
      /// </summary>
      /// <param name="newItem"></param>
      /// <returns></returns>
        public static List<object> SaveNewItemRequest(BusinessObject.StoreManagement.NewItemRequestBO newItem)
      {

          DBService dbService = new DBService();
          DBService.Parameters[] colParameters = null;

          colParameters = new DBService.Parameters[] 
            { 
       
              
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.NEWITEMREQUESTID,   newItem.ITR_PK == 0 ? (object)DBNull.Value: newItem.ITR_PK) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.NIRNO ,   newItem.NIR_NO) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.PLACEREQUESTTO,   newItem.MRH_DEPT_STR) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.REQUIREDFOR,newItem.DeptPk) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.DATE ,   newItem.NIR_SUBMITTED_DATE) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.REQUIREDBYDATE,   newItem.NIR_REQUIRED_DATE) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.CATEGORY,newItem.MaterialType == -1 ? (object)DBNull.Value:newItem.MaterialType) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.NAME ,   newItem.NameTitle) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.REQUESTFREQUENCY,   newItem.RequestFrequency) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.FRD,newItem.FRD) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.QTYREQUIRED ,   newItem.QtyRequired) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.UOM,   newItem.UOM) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.KNOWNVENDORS,newItem.KnownVendors) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.DESCRIPTION ,   newItem.Description) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.PURPOSE,   newItem.Purpose) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.COMMERCIALDETAILS,newItem.CommercialDetails) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.USERPK ,   newItem.UserPk) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.ACTIVE ,   1) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.REMARKS ,   newItem.Remarks) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.SBU,   newItem.BizUnitPk) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.ACTIONID ,   newItem.ActionID) ,
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.PRETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.NIRRETURNNO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };


          List<object> retvals = new List<object>();
          int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_NewItemRequest.SAVENEWITEMREQUEST, colParameters);
          int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_NewItemRequest.PRETVAL]).Value);
          string nirNo = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_NewItemRequest.NIRRETURNNO]).Value.ToString();
          retvals.Add(result);
          retvals.Add(nirNo);
          return retvals;

      }
        /// <summary>
        /// Function Used To Get NIR No 
        /// </summary>
        /// <returns>string</returns>
        public static string GetNIRNo()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
               
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.ITEMPK, 0, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_NewItemRequest.GETNIRNO, colParameters);

            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_NewItemRequest.ITEMPK]).Value);

        }
      /// <summary>
        /// Method to get the Search Values Corresponding to Search Type
      /// </summary>
      /// <param name="searchBy"></param>
      /// <param name="searchValue"></param>
      /// <param name="sbuPk"></param>
      /// <param name="objUser"></param>
      /// <param name="procId"></param>
      /// <returns></returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int sbuPk, User objUser, int procId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Store.Parameters_NewItemRequest.SEARCHBY , searchBy),
              new DBService.Parameters( GTIService.Constants.Store.Parameters_NewItemRequest.SEARCHVALUE  ,searchValue),
              new DBService.Parameters( GTIService.Constants.Store.Parameters_NewItemRequest.PROCESSID  ,procId),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT ,  sbuPk),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.USERPK,  objUser.PKUser)
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_NewItemRequest.GETSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }
       
        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetNewItemRequestList(GridPrams grid, int bizUnit, User objUser, int procID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
           
              new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.ID , grid.SearchBy == "0" || grid.SearchBy == "Date" ? GTIService.Constants.Store.Fields_NewItemRequest.NIRNO : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.SRCH , grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , procID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,   grid.SortBy==GTIService.Constants.Store.Fields_NewItemRequest.NIRNO ||  grid.SortBy==GTIService.Constants.Store.Fields_NewItemRequest.NIRDATE ?GTIService.Constants.Store.Fields_NewItemRequest.ITEMPK:grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC,  grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.FROMDATE , grid.FromDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.TODATE, grid.ToDate == string.Empty ? (object)DBNull.Value : Convert.ToDateTime(grid.ToDate) )

            };

            DataSet dtStore = new DataSet();
            dtStore = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_NewItemRequest.GETNEWITEMREQUESTLISTBYSRH, colParameters);
            return dtStore;


        }
        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static DataSet GetNewItemCheckList(int bizUnit, string itemName)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
           
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.NAME ,"%"+itemName+"%"),
              

            };

            DataSet dsCheckList = new DataSet();
            dsCheckList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_NewItemRequest.GETNEWITEMCHECKLIST, colParameters);
            return dsCheckList;


        }
        /// <summary>
        /// Delete New Item Request Details By itemID
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static int DeleteNewItemRequestDtls(int itemID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.ITEMPK,  itemID == 0 ? (object)DBNull.Value :  itemID),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.PRETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_NewItemRequest.DELETENEWITEMREQUEST, colParameters);

            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Store.Parameters_NewItemRequest.PRETVAL]).Value);



        }

        /// <summary>
        /// get New Item Request details
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetNewItemRequestDetails(int itemID, int active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.ITEMPK,  itemID ),
                new DBService.Parameters(GTIService.Constants.Store.Parameters_NewItemRequest.ACTIVE,  active),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Store.Procedure_NewItemRequest.GETNEWITEMREQUESTKV, colParameters).Tables[0];
        }
  }
      #endregion
}
