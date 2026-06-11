using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.PurchaseRequestManagement
{
    public class PurchaseRequestTradingListDL
    {
        #region Methods
        /// <summary>
        /// Get Purchase Request Trading List Details
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPurchaseRequestTradingList(GridPrams grid, User objUser, string PageUrl, int procID, int transactionStatus = 0, int reqStore = 0, string ioNo = null, string ItemName = null, int reqDept = 0, string reqBy = null, int FilterStatus = 0, string prNo = null, int cmpPk = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                         
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy==null? "PRH_DATE": grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID ,procID ),            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.STATUS ,  transactionStatus>=0 ? transactionStatus :(object)DBNull.Value),              
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value:grid.FromDate),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE , grid.ToDate== string.Empty ?(object)DBNull.Value: grid.ToDate) ,
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COMPANY_PK , cmpPk > 0 ? cmpPk : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DEPTPK , reqStore > 0 ? reqStore : (object)DBNull.Value),         
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_NAME ,ItemName==string.Empty ? (object)DBNull.Value : ItemName),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRH_ISSUE_DEPT , reqDept > 0 ? reqDept : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , grid.UserPK),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS , FilterStatus),             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRH_NO ,prNo==string.Empty ? (object)DBNull.Value : prNo),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_SOH_NO ,ioNo==string.Empty ? (object)DBNull.Value : ioNo),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRH_USER ,reqBy==string.Empty ? (object)DBNull.Value : reqBy),
            };

            DataSet dsPurchaseRqstList = new DataSet();
            dsPurchaseRqstList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASERQSTTRDLISTDTLS, colParameters);
            return dsPurchaseRqstList;


        }
        /// <summary>
        /// Delete purchase Request Trading List Details
        /// </summary>
        /// <param name="desigID"></param>
        /// <returns>int</returns>
        public static int DeletePurchaseRequestTradingList(int requestPk, string remarks)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Fields.PURCHASERQSTPK,  requestPk == 0 ? (object)DBNull.Value :  requestPk),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Fields.REMARKS,  remarks == string.Empty ? (object)DBNull.Value :  remarks),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.DELETEPURCHASERQSTTRDLIST, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

        }


        /// <summary>
        /// 'Get Purchase Request Trading List AutoComplete Details 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int processPK, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
             
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY ,searchBy),  
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE ,searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK ,objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID ,processPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT ,objUser.SBUID), 
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.PURCHASERQSTTRDLISTAUTO, colParameters).Tables[0];
            return dtSearchValue;
        }

        #endregion
    }
}
