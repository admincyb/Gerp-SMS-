using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace DataAccess.PurchaseRequestManagement
{
    /// <summary>
    /// 
    /// </summary>
    public class PurchaseRequestListingDL
    {
        #region Methods
        /// <summary>
        /// Get Purchase Request List Details
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPurchaseRequestList(GridPrams grid, User objUser, string PageUrl, int procID, int transactionStatus = 0, int reqStore = 0, string ioNo = null, string ItemName = null, int reqDept = 0, string reqBy = null, int FilterStatus = 0, string prNo = null, int cmpPk = 0, int POCategory = 0, int PRGroup = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),              
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? (object)DBNull.Value : grid.SearchBy ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL  , grid.SearchValue== string.Empty ? "%" : grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO ,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE ,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields),
             // //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY ,  grid.SortBy == null ||grid.SortBy == "PRH_DATE"|| grid.SortBy == "PRH_NO" ? GTIService.Constants.PurchaseRequest.Fields.PURCHASERQSTPK: grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy==null? "PRH_DATE": grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC , grid.SortDirection == null ? GTIService.Constants.PurchaseRequest.Fields.DSCORDERBY : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID ,procID ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK ,  objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.STATUS ,  transactionStatus>=0 ? transactionStatus :(object)DBNull.Value),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.FromDate) ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.ToDate) ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS ,  string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : grid.FilterStatus),

             


              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value:Convert.ToDateTime(grid.FromDate)),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE , grid.ToDate== string.Empty ?(object)DBNull.Value:  Convert.ToDateTime(grid.ToDate) ),

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value:grid.FromDate),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE , grid.ToDate== string.Empty ?(object)DBNull.Value: grid.ToDate) ,

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COMPANY_PK , cmpPk > 0 ? cmpPk : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DEPTPK , reqStore > 0 ? reqStore : (object)DBNull.Value),

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_NAME ,ItemName==string.Empty ? (object)DBNull.Value : ItemName),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRH_ISSUE_DEPT , reqDept > 0 ? reqDept : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , grid.UserPK),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRH_USER , reqBy > 0 ? reqBy : (object)DBNull.Value),
             
                   new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS , FilterStatus),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS , FilterStatus > 0 ? FilterStatus : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRH_NO ,prNo==string.Empty ? (object)DBNull.Value : prNo),

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_SOH_NO ,ioNo==string.Empty ? (object)DBNull.Value : ioNo),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRH_USER ,reqBy==string.Empty ? (object)DBNull.Value : reqBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRH_PO_CATEGORY ,POCategory > 0 ? POCategory : (object)DBNull.Value),
              new DBService.Parameters("P_PRH_GROUP",PRGroup>0 ? PRGroup :(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRH_TYPE, grid.PRH_TYPE > 0 ? grid.PRH_TYPE :(object)DBNull.Value),

            };

            DataSet dsPurchaseRqstList = new DataSet();
            dsPurchaseRqstList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASERQSTDTLS, colParameters);
            return dsPurchaseRqstList;


        }

        public static DataSet GetMaterialRequestList(GridPrams grid, User objUser, string PageUrl, int procID, int transactionStatus = 0, int reqStore = 0, string ioNo = null, int ItemCode = 0, int reqDept = 0, string reqBy = null, int FilterStatus = 0, string prNo = null, int cmpPk = 0, int POCategory = 0, int PRGroup = 0)
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
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_PK ,ItemCode > 0 ? ItemCode : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRH_ISSUE_DEPT , reqDept > 0 ? reqDept : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , grid.UserPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS , FilterStatus),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRH_NO ,prNo==string.Empty ? (object)DBNull.Value : prNo),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_SOH_NO ,ioNo==string.Empty ? (object)DBNull.Value : ioNo),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRH_USER ,reqBy==string.Empty ? (object)DBNull.Value : reqBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRH_PO_CATEGORY ,POCategory > 0 ? POCategory : (object)DBNull.Value),
              new DBService.Parameters("P_PRH_GROUP",PRGroup>0 ? PRGroup :(object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRH_TRX_TYPE, grid.POH_MENU_TYPE > 0 ? grid.POH_MENU_TYPE : (object)DBNull.Value),
            };
            DataSet dsPurchaseRqstList = new DataSet();
            dsPurchaseRqstList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETMATERIALRQSTDTLS, colParameters);
            return dsPurchaseRqstList;


        }


        /// <summary>
        /// 'Get Purchase Request AutoComplete Details 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int processPK, User objUser, string pageUrl = null, int type = 0)
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
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,string.IsNullOrEmpty(pageUrl)?(object)DBNull.Value :pageUrl ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_TYPE , type > 0 ? type : (object)DBNull.Value),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.PURCHASERQSTAUTO, colParameters).Tables[0];
            return dtSearchValue;
        }
        /// <summary>
        /// 'Get Material Request AutoComplete Details 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetMaterialSearchValues(string searchBy, string searchValue, int processPK, User objUser, string pageUrl = null, int MenuType = 0)
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
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,string.IsNullOrEmpty(pageUrl)?(object)DBNull.Value :pageUrl ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRH_TRX_TYPE,MenuType > 0 ? MenuType : (object)DBNull.Value)
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.MATERIALRQSTAUTO, colParameters).Tables[0];
            return dtSearchValue;
        }

        public static DataTable ValidateItemStock(int Dept, int MaterialPK, decimal Qty)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_PK, MaterialPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_QTY, Qty),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.DEPTPK, Dept > 0 ? Dept : (object)DBNull.Value)
            };
            DataTable dtResult = new DataTable();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.SPINV_STOCK_VALIDATE, colParameters).Tables[0];
            return dtResult;
        }

        /// <summary>
        /// Delete purchase Request Details
        /// </summary>
        /// <param name="desigID"></param>
        /// <returns>int</returns>
        public static int DeletePurchaseRequestDtls(int requestPk, string remarks)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Fields.PURCHASERQSTPK,  requestPk == 0 ? (object)DBNull.Value :  requestPk),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Fields.REMARKS,  remarks == string.Empty ? (object)DBNull.Value :  remarks),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.DELETEPURCHASERQST, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

        }
        public static int DeleteMRDtls(int requestPk, string remarks)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Fields.PURCHASERQSTPK,  requestPk == 0 ? (object)DBNull.Value :  requestPk),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Fields.REMARKS,  remarks == string.Empty ? (object)DBNull.Value :  remarks),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.DELETEMRRQST, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

        }
        public static int DeleteMRDtlsPlantToPlant(int requestPk, string remarks)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Fields.PURCHASERQSTPK,  requestPk == 0 ? (object)DBNull.Value :  requestPk),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Fields.REMARKS,  remarks == string.Empty ? (object)DBNull.Value :  remarks),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.DELETEMRRQSTP2P, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

        }

        /// <summary>
        /// Delete Material Request Details
        /// </summary>
        /// <param name="desigID"></param>
        /// <returns>int</returns>
        public static int DeleteMaterialRequestDtls(int requestPk, string remarks)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Fields.PURCHASERQSTPK,  requestPk == 0 ? (object)DBNull.Value :  requestPk),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Fields.REMARKS,  remarks == string.Empty ? (object)DBNull.Value :  remarks),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.DELETEMATERIALRQST, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseRequestID"></param>
        /// <returns></returns>
        public static DataSet GetPurchaseRequestReportDetails(int purchaseRequestID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.PurchaseRequest.Parameters.PURCHASEREQUESTPK ,  purchaseRequestID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASERQSTDTLSREPORT, colParameters);
        }


        /// </summary>
        /// <param name="purchaseRequestID"></param>
        /// <returns></returns>
        public static DataSet GetPurchaseRequestReportDetailsDOCNOREVISION(int purchaseRequestID, int reportpk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.PurchaseRequest.Parameters.PURCHASEREQUESTPK ,  purchaseRequestID),
                new DBService.Parameters( GTIService.Constants.PurchaseRequest.Parameters.P_AST_PK ,  reportpk),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETPURCHASERQSTDTLSREPORT, colParameters);
        }
        /// <summary>
        /// To Get Material Request Trx Print Details
        /// </summary>
        /// <param name="materialRequestID"></param>
        /// <returns></returns>
        public static DataSet GetMaterialRequestReportDetails(int materialRequestID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters( GTIService.Constants.PurchaseRequest.Parameters.PURCHASEREQUESTPK ,  materialRequestID)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.GETMATERIALRQSTDETAILSREPORT, colParameters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseRequestID"></param>
        /// <returns></returns>
        public static DataSet GetCompoundUsageSummary(int RecPK, int SbuID, DateTime startOfMonth, DateTime endOfMonth, int Size = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_COMP_BATCH ,  RecPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT ,SbuID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE ,startOfMonth),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE ,endOfMonth),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PRO_SIZE,Size)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.SPPRD_BIN_CARD_COMP_BATCH_DTL_RPT, colParameters);
        }

        public static DataSet GetCompoundYieldCost(int RecPK, int SbuID, DateTime startOfMonth, DateTime endOfMonth)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BCH_COMP_BATCH ,  RecPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT ,SbuID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE ,startOfMonth),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE ,endOfMonth),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseRequest.Procedures.SPPRD_BIN_CARD_COMP_BATCH_DTL_RPT_NEW, colParameters);
        }


        #endregion


    }
}
