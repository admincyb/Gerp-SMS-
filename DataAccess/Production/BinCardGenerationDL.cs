using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Production;

namespace DataAccess.Production
{
    public class BinCardGenerationDL
    {
        /// <summary>
        /// Function Used To get the running BinCard number
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>Bin Card Generation</for>
        /// Used in fill the Bin Card number
        /// <returns></returns>
        public static string GetBinCardNumber()
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            int result;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Production.Parameters.BCHID, 0, 20,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETBINCARDNO, colParameters);
            result = int.Parse(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Production.Parameters.BCHID]).Value.ToString());
            return Convert.ToString(result);
        }

        /// <summary>
        /// Function Used Save/update Bin Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>Bin Card Creation</for>
        /// Used in Saving and updating Bin Card Details
        /// <returns></returns>
        public static string SaveBinDetails(string xmlBinDetails, out string binNumber)
        {
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(xmlVendorDetails);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Production.Parameters.BCHXML ,(object)xmlBinDetails,DBService.ParameterType.XML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.Production.Parameters.RETVAL , "0", 100,ParameterDirection.Output, DBService.ParameterType.NVarChar),
                 
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.SAVEBINCARD, colParameters);
            binNumber = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Production.Parameters.RETVAL]).Value.ToString();
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value).ToString();
            //+ "," + ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseOrder.Parameters.RETVAL]).Value;

        }

        /// <summary>
        /// Function Used to Get BIn Details
        /// </summary>
        /// <param name=null></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <for>Bin Card Creation</for>
        /// Used Get Bin Details
        /// <returns></returns>
        public static string GetBinDetails(int binID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Production.Parameters.BINPK , binID),                
                 
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETBINCARDDETAILS, colParameters).Tables[0];
            return dtxml.Rows[0][0].ToString();
//            return @"<root> 
//  <UserPk>2</UserPk> 
//  <BizUnitPk>1</BizUnitPk> 
//  <LAST_MOD_DT /> 
//  <BCH_NO>BIN-09052011-3</BCH_NO> 
//  <BCH_PK>1</BCH_PK> 
//  <BCHTXT_PLAN>PLAN A</BCHTXT_PLAN> 
//  <BCH_PLAN>1</BCH_PLAN> 
//  <BCHTXT_COMP_BATCH>COM-24052011</BCHTXT_COMP_BATCH> 
//  <BCH_COMP_BATCH>1</BCH_COMP_BATCH> 
//  <BCHTXT_PRODUCT>PRODUCT A</BCHTXT_PRODUCT> 
//  <BCH_PRODUCT>1</BCH_PRODUCT> 
//  <BCHTXT_SHIFT>SHIFT A</BCHTXT_SHIFT> 
//  <BCH_SHIFT>1</BCH_SHIFT> 
//  <BCH_PROD_BATCH>PRD-001</BCH_PROD_BATCH> 
//  <BCHTXT_LINE>LINE-1</BCHTXT_LINE> 
//  <BCH_LINE>1</BCH_LINE> 
//  <BCH_TUMB_ST_TM>05:11 PM</BCH_TUMB_ST_TM> 
//  <BCH_TOT_WT>1</BCH_TOT_WT> 
//  <BCH_TOT_AVG_WT>1</BCH_TOT_AVG_WT> 
//  <BCH_TOT_PCS>1000</BCH_TOT_PCS> 
//  <BCH_INSP_DT>09-May-2011</BCH_INSP_DT> 
//  <BCHTXT_INSP_BY>Admin</BCHTXT_INSP_BY> 
//  <BCH_INSP_BY>-1</BCH_INSP_BY> 
//  <BCH_WT_A>1</BCH_WT_A> 
//  <BCH_WT_B>1</BCH_WT_B> 
//  <BCH_WT_C>1</BCH_WT_C> 
//  <BCH_AVG_WT_A>1</BCH_AVG_WT_A> 
//  <BCH_AVG_WT_B>1</BCH_AVG_WT_B> 
//  <BCH_AVG_WT_C>1</BCH_AVG_WT_C> 
//  <BCH_PCS_A>1000</BCH_PCS_A> 
//  <BCH_PCS_B>1000</BCH_PCS_B> 
//  <BCH_PCS_C>1000</BCH_PCS_C> 
//  </root>";

        }


        #region Bin Listing
        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.SEARCHBY ,  searchBy),
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.SEARCHVALUE ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,objUser.SBUID),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK,objUser.PKUser),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETSEARCHVALUE, colParameters).Tables[0];
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
        public static DataSet GetBinCardDetails(GridPrams grid, int sbuID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            if (grid.SortBy == "BCH_NO")
            {
                grid.SortBy = "BCH_PK";
            }
            colParameters = new DBService.Parameters[] 
            {            

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0" || grid.SearchBy == "Date" ? "BCH_NO" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL , grid.SearchValue == "" ? "%" : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy == null ||grid.SortBy == "BCH_NO"|| grid.SortBy=="BCH_INSP_DT" ? "BCH_PK" : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "desc" : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,sbuID),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE , grid.FromDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.FromDate) ),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE, grid.ToDate== string.Empty ?(object)DBNull.Value: Convert.ToDateTime(grid.ToDate) )
            };

            DataSet dtProduct = new DataSet();
            dtProduct = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Production.Procedures.GETBINDETAILSLIST, colParameters);
            return dtProduct;


        }

        /// <summary>
        /// Methord to get the Delete Corresponding Po with Provided PO ID
        /// </summary>
        /// <param name="vendorID"></param>
        /// <returns>DataTable</returns>
        public static int DeleteBinCardDetails(int binID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.BCHID , binID),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.DELETEBINDETAILS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }

        #endregion


      
    }
}
