using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.PurchaseOrder;

namespace DataAccess.PurchaseOrderManagement
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
            return dtxml.Rows[0][0].ToString();

        }


        // 25May2011
        /// <summary>
        /// Get Purchase Order Details By PO Number For Report
        /// </summary>
        /// <param name="planID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPurchaseOrderDtls(int purchaseID,string PurchaseOrderOutRPTSP)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POHID,  purchaseID)
                                          
            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, PurchaseOrderOutRPTSP, colParameters);
                return dsActualPlndLinAloc;
        }


        public static DataSet PurchaseOrderDetailsDOCNOREVISION(int purchaseID, string PurchaseOrderOutRPTSP, int reportpk)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POHID,  purchaseID),
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.P_AST_PK,  reportpk)

            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, PurchaseOrderOutRPTSP, colParameters);
            return dsActualPlndLinAloc;
        }

        public static DataSet GetPurchaseOrderDtls(int purchaseID,int RevID)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.POHID,  purchaseID),
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.REVPK,  RevID)
                                          
            };

            DataSet dsActualPlndLinAloc = dbService.DataAdapter(CommandType.StoredProcedure, "SPPUR_ORDER_ARCHIVE_RPT", colParameters);
            return dsActualPlndLinAloc;
        }

        // 06Mar2013
        /// <summary>
        /// Get Current User Address For Report
        /// </summary>
        /// <param name="P_BZU_PK"></param>
        /// <param name="P_ACTIVE"></param> 
        /// <returns>DataSet</returns>
        public static DataSet GetCurrentSBUAddress(int BzuPk, int Active)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.BZUPK,  BzuPk),
                new DBService.Parameters(GTIService.Constants.PurchaseOrder.Parameters.ACTIVE,  Active)
                                          
            };

            DataSet dsCurrentSBUAddress = dbService.DataAdapter(CommandType.StoredProcedure, "SPADM_BIZUNIT_MST_GET_KV", colParameters);
            return dsCurrentSBUAddress;


        }

        
        #region Po Listing
        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue,User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.SEARCHBY ,  searchBy),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.SEARCHVALUE ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,objUser.SBUID),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,objUser.PKUser),
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
        public static DataSet GetPurchaseOrderDetails(GridPrams grid, int sbuID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0"? "POH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , 3),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , grid.UserPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DEPTPK , grid.DeptPK == 0 ? 1 : grid.DeptPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy == null ? "POH_NO" : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "asc" : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,sbuID)
            };

            DataSet dtProduct = new DataSet();
            dtProduct = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.PurchaseOrder.Procedures.GETPODETILSLIST, colParameters);
            return dtProduct;


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
        #endregion

    }
}
