using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.StoreManagement
{
    public class StoreAuditListDL
    {

        #region Methods
        /// <summary>
        /// Get Purchase Request List Details
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetStoreAuditList(GridPrams grid, User objUser, int procID,string PageUrl)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                         
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "SAH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY ,  grid.SortBy == null ||grid.SortBy == "SAH_DATE"|| grid.SortBy == "SAH_NO" ? "SAH_PK": grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID ,procID ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),

            };

            DataSet dsPurchaseRqstList = new DataSet();
            dsPurchaseRqstList = dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_STK_AUD_GET_LIST_WRKF", colParameters);
            return dsPurchaseRqstList;


        }

        /// <summary>
        /// 'Get Purchase Request AutoComplete Details 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY  ,  searchBy),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE   ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK   ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID), 

            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, "SPINV_STK_AUD_AUTO", colParameters).Tables[0];
            return dtSearchValue;
        }

        /// <summary>
        /// Delete purchase Request Details
        /// </summary>
        /// <param name="desigID"></param>
        /// <returns>int</returns>
        public static int DeleteStoreAuditDtls(int requestPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Store.Parameters_StoreAudit.SAHPK, requestPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, "SPINV_STK_AUD_DELETE", colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseRequestID"></param>
        /// <returns></returns>
        public static DataSet GetStoreAuditReportDetails(int purchaseRequestID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.PurchaseRequest.Parameters.PURCHASEREQUESTPK ,  purchaseRequestID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, "", colParameters);
        }
       
        #endregion

    }
}
