using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.PurchaseOrder;

namespace DataAccess.PurchaseOrderManagement
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
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            int result;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POHID, 0, 20,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETPURCHSENO, colParameters);
            result = int.Parse(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseOrder.Parameters.POHID]).Value.ToString());
            return Convert.ToString(result);
        }

        /// <summary>
        /// Function Used Save/update PO Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>purchase order Creation</for>
        /// Used in Saving and updating PO Details
        /// <returns></returns>
        public static string SavePODetails(string xmlPoDetails,out string poNumber)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(xmlVendorDetails);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.XMLPO ,(object)xmlPoDetails,DBService.ParameterType.XML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.RETVAL , "0", 100,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                 
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.SAVEPURCHASEORDER, colParameters);
            poNumber = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseOrder.Parameters.RETVAL]).Value.ToString();
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value).ToString();            
            //+ "," + ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseOrder.Parameters.RETVAL]).Value;

        }

        /// <summary>
        /// Function Used Save/update PO Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Riyas</Createdby>
        /// <for>purchase order Creation (Service)</for>
        /// Used in Saving and updating PO Details Returns Both PO Number and PK. For Showing PO Number in pop up message after submitting purchase order service 
        /// <returns></returns>
        public static List<object> SavePOServiceDetails(string xmlPoDetails, out string poNumber)
        {
           
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.XMLPO ,(object)xmlPoDetails,DBService.ParameterType.XML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.RETVAL , "0", 100,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                 
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.SAVEPURCHASEORDER, colParameters);         
            poNumber = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseOrder.Parameters.RETVAL]).Value.ToString();
            string result = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value.ToString();
            List<object> retvals = new List<object>();
            retvals.Add(result);
            retvals.Add(poNumber);
            return retvals;

        }

        /// <summary>
        /// Function Used Save/update PO Service Details
        /// </summary>      
        public static int? SavePOServiceDetailsWkf(string xmlstr, out string poNumber)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.XML, xmlstr),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 20,ParameterDirection.Output, DBService.ParameterType.VarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)

            };          
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrderGenerates.Procedures.SPPUR_ORDER_WKF_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
            poNumber = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value.ToString();
            return result;
        }


        /// <summary>
        /// Function Used to Get PO Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>purchase order Creation</for>
        /// Used Get PO Details
        /// <returns></returns>
        public static string GetPODetails(int poID){
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POHID , poID),                
                 
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETPURCHASEORDER, colParameters).Tables[0];
            string strRetVal = "";
            for (int i = 0; i < dtxml.Rows.Count; i++)
                strRetVal += dtxml.Rows[i][0].ToString();
            return strRetVal;
            
           // return dtxml.Rows[0][0].ToString();

        }


        /// <summary>
        /// Function Used to Get PR Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>purchase order Creation</for>
        /// Used Get PO Details
        /// <returns></returns>
        public static DataTable GetPRDetails(int poID, int itemID,int uom, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POHID , poID==0?(object)DBNull.Value:poID),      
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.ITEMID , itemID),       
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POHUOM , uom), 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT , bizUnit), 
                 
            };
            DataTable dtPRDetails = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETPRDETAILS, colParameters).Tables[0];
            return dtPRDetails;

        }

        #region Po Listing
        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue,int processPK, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY, searchBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID, processPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, objUser.PKUser),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        ///  Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="pageURL"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetPurchaseAutoSearchValue(string searchBy, string searchValue, string pageURL, User objUser, int type = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHBY, searchBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE, searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL, pageURL),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, objUser.PKUser),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_TYPE, type > 0 ? type : (object)DBNull.Value),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }
        
        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="fields"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="status"></param>
        /// <param name="Searchtxt"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPurchaseOrderDetails(GridPrams grid, int sbuID, int procID, string pageUrl, byte orderGroup)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            if (grid.SortBy == "POH_NO")
            {
                grid.SortBy = "POH_PK";
            }
            colParameters = new DBService.Parameters[] 
            {            

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "POH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS, grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , procID ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , grid.UserPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DEPTPK , grid.DeptPK == 0 ? 1 : grid.DeptPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy == null ||grid.SortBy == "POH_NO"|| grid.SortBy=="POH_DATE" ? "POH_PK" : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "desc" : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,sbuID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS ,  string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : ( Convert.ToInt16(grid.FilterStatus)==-1 ? (object)DBNull.Value: grid.FilterStatus)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  pageUrl==string.Empty ?(object)DBNull.Value:pageUrl),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.ORDERGROUP ,  orderGroup)

            };

            DataSet dtProduct = new DataSet();
            dtProduct = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETPODETILSLIST, colParameters);
            return dtProduct;
        }

        /// <summary>
        /// Method for polist Searching
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
        public static DataSet GetPurchaseOrderList(GridPrams grid, int sbuID, string pageUrl, int status, int transactionStatus, int vendor, string POhNo, string PRNo, string IONo, int reqStore, byte orderGroup, string ItmName, int cmpPk = 0, int poType = 0, int pohPOCategory = 0, int reqDept = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            if (grid.SortBy == "POH_NO")
            {
                grid.SortBy = "POH_PK";
            }
            colParameters = new DBService.Parameters[] 
            {            

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS , "*"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "desc" : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS , status ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS ,  string.IsNullOrEmpty(grid.FilterStatus) ? (object)DBNull.Value : ( Convert.ToInt16(grid.FilterStatus)==-1 ? (object)DBNull.Value: grid.FilterStatus)),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.STATUS ,transactionStatus >= 0 ? transactionStatus : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.VENDOR ,vendor > 0 ? vendor : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.POH_NO ,POhNo==string.Empty ? (object)DBNull.Value : POhNo),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRH_NO ,PRNo==string.Empty ? (object)DBNull.Value : PRNo),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SCNO ,IONo==string.Empty ? (object)DBNull.Value : IONo),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_NAME ,ItmName==string.Empty ? (object)DBNull.Value : ItmName),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DEPTPK , reqStore > 0 ? reqStore : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , grid.UserPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  pageUrl==string.Empty ?(object)DBNull.Value:pageUrl),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,sbuID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.ORDERGROUP ,  orderGroup > 0 ? orderGroup : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COMPANY_PK , cmpPk > 0 ? cmpPk : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_POH_TYPE , poType > 0 ? poType : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_POH_PO_CATEGORY , pohPOCategory > 0 ? pohPOCategory : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POH_MENU_TYPE , grid.POH_MENU_TYPE > 0 ? grid.POH_MENU_TYPE : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_POH_CONV_INV_NO ,grid.InvNo==string.Empty ? (object)DBNull.Value : grid.InvNo),
              new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_POH_ISSUE_DEPT , reqDept > 0 ? reqDept : (object)DBNull.Value),
            };

            DataSet dtProduct = new DataSet();
            dtProduct = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETPODETILSLIST, colParameters);
            return dtProduct;
        }



        /// <summary>
        /// method for getting pending Po list Corresponding to materialid
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="fields"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>   
        /// <param name="materialid"></param>
        /// <returns>DataSet</returns>
        public static DataTable GetPendingPurchaseOrderDetails(GridPrams grid, int itemID, int toUOM=0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            if (grid.SortBy == "POH_NO")
            {
                grid.SortBy = "POH_PK";
            }
            colParameters = new DBService.Parameters[] 
            {  
                        
              new DBService.Parameters( GTIService.Constants.PurchaseOrder.Parameters.P_ITEM ,itemID),
              new DBService.Parameters( GTIService.Constants.Material.Parameters.TO_UOM ,toUOM ==0?(object)DBNull.Value:toUOM)

            };
                 
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETPENDINGPOLIST, colParameters).Tables[0];
        }

        /// <summary>
        /// Methord to get the Delete Corresponding Po with Provided PO ID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>DataTable</returns>
        public static int DeletePODetails(int poID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.POHID , poID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.DELETEPODETAILS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poShortClose"></param>
        /// <returns></returns>
        public static int SavePOShortClose(BusinessObject.PurchaseOrderGeneration.POShortClose poShortClose)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POHPK , poShortClose.POID),      
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.REMARKS , poShortClose.Remarks),  
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.REFNO , poShortClose.RefNo),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , poShortClose.UserPk),
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_CHECK_FLAG , poShortClose.CheckFlag),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.POSHORTCLOSE , colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }

        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="fields"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="status"></param>
        /// <param name="Searchtxt"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPurchaseOrderDetailsView(int poID, int sbuID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {            
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POHID , poID),      
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT , sbuID), 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETPODETILSVIEW, colParameters);
        }

        /// <summary>
        /// Method for polist trading Searching
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
        public static DataSet GetPurchaseOrderListTrading(GridPrams grid, int sbuID, string pageUrl, int status, int transactionStatus, int vendor, string POhNo, string PRNo, string IONo, int reqStore, byte orderGroup, string ItmName, int cmpPk = 0, int poType = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            if (grid.SortBy == "POH_NO")
            {
                grid.SortBy = "POH_PK";
            }
            colParameters = new DBService.Parameters[] 
            {            

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS , "*"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "desc" : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FILTERSTATUS , status ),            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.STATUS ,transactionStatus >= 0 ? transactionStatus : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.FromDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.ToDate) ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.VENDOR ,vendor > 0 ? vendor : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.POH_NO ,POhNo==string.Empty ? (object)DBNull.Value : POhNo),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRH_NO ,PRNo==string.Empty ? (object)DBNull.Value : PRNo),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SCNO ,IONo==string.Empty ? (object)DBNull.Value : IONo),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ITM_NAME ,ItmName==string.Empty ? (object)DBNull.Value : ItmName),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DEPTPK , reqStore > 0 ? reqStore : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , grid.UserPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  pageUrl==string.Empty ?(object)DBNull.Value:pageUrl),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,sbuID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.ORDERGROUP ,  orderGroup),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COMPANY_PK , cmpPk > 0 ? cmpPk : (object)DBNull.Value),
              new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_POH_TYPE , poType > 0 ? poType : (object)DBNull.Value),
            };

            DataSet dtResult = new DataSet();
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETPODETILSLISTTRADING, colParameters);
            return dtResult;
        }

        #endregion

        /// <summary>
        /// Function Used to Get PO Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>purchase order Creation</for>
        /// Used Get PO Details
        /// <returns></returns>
        public static string GetPODetailsTrading(int poID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POHID , poID),                
                 
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETPURCHASEORDERTRADING, colParameters).Tables[0];
            string strRetVal = "";
            for (int i = 0; i < dtxml.Rows.Count; i++)
                strRetVal += dtxml.Rows[i][0].ToString();
            return strRetVal;

            // return dtxml.Rows[0][0].ToString();

        }
    }
}
