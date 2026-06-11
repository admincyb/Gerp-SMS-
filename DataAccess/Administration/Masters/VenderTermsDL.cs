using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService;

using BusinessObject;
namespace DataAccess.Administration.Masters
{
    public  class VenderTermsDL
    {
        /// <summary>
        /// Method to save vendor terms
        /// </summary>
        /// <param name="Vendorterm"></param>
        /// <returns></returns>
        public static string SaveVendorterm(BusinessObject.Administration.Masters.VendorTerm vendorterm)
        {
             DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;

           colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.TERMSPK , vendorterm.TermPK == 0? (object)DBNull.Value : vendorterm.TermPK ),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.TERMSDESCR , vendorterm.TermDescr),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.TERMSMODBY , vendorterm.Userpk),
               // new DBService.Parameters(GTIService.Constants.Vendor.Parameters.TERMSMODDATE , DateTime.Today),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.TERMSRETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                 new DBService.Parameters(GTIService.Constants.Vendor.Parameters.TERMSSTATUS , vendorterm.ACTIVE),
                 new DBService.Parameters(GTIService.Constants.Vendor.Parameters.TERMSTITLE , vendorterm.TermTitle),
                 new DBService.Parameters(GTIService.Constants.Vendor.Parameters.TERMSTYPE , vendorterm.Termtype),
                 new DBService.Parameters(GTIService.Constants.Vendor.Parameters.TERMSBUZUNIT , vendorterm.SBU),//------------------------to be changed--------------------------------------
            };
              dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.VENDORTERMSAVEMASTERSAVE, colParameters);
              return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Vendor.Parameters.TERMSRETVAL]).Value).ToString();
            
            
          //  return "0";
        }
        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetVendorTermsList(GridPrams grid, int bizUnitPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
             // new DBService.Parameters(GTIService.Constants.Vendor.Parameters.MATERIALLSTSTATUS , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Vendor.Parameters.SEARCHNAME , grid.SearchBy==string.Empty?(object)DBNull.Value:grid.SearchBy),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.SEARCHVAL , grid.SearchValue==string.Empty?(object)DBNull.Value:"%"+grid.SearchValue+"%"),
                  new DBService.Parameters(GTIService.Constants.Vendor.Parameters.SORTBY , grid.SortBy==string.Empty?(object)DBNull.Value:grid.SortBy),
                  new DBService.Parameters(GTIService.Constants.Vendor.Parameters.SORTDIR, grid.SortDirection==string.Empty?(object)DBNull.Value:grid.SortDirection),
                  new DBService.Parameters(GTIService.Constants.Vendor.Parameters.PAGENO,grid.PageNumber),
                      new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,bizUnitPk),
                  new DBService.Parameters(GTIService.Constants.Vendor.Parameters.FIELDLIST , grid.Fields==string.Empty?(object)DBNull.Value:grid.Fields),

            };

            DataSet dtMaterial = new DataSet();
            dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.VENDORTERMSAVEMASTERGET, colParameters);
            return dtMaterial;


        }
        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetVendorTermsList(int termsPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
             
            };

            DataSet dtMaterial = new DataSet();
            dtMaterial = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.VENDORTERMSAVEMASTERGET, colParameters);
            return dtMaterial;


        }
        /// <summary>
        /// Delete material Details By TermsID
        /// </summary>
        /// <param name="TermsID"></param>
        /// <returns>int- 1(Success)</returns>
        public static int DeleteVendorTermsDtls(int termsPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.TERMSPK,  termsPK == 0 ? (object)DBNull.Value :  termsPK),

            };

           return dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.VENDORTERMSDELE, colParameters);

           // return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Material.Parameters.MATERIALRETURNVALUE]).Value);



        }
        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
        /// <param name="BizUnitPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetSearchValues(string searchBy, string searchValue, int bizUnitPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Vendor.Parameters.FIELDNAME ,  searchBy),
              new DBService.Parameters( GTIService.Constants.Vendor.Parameters.FIELDVAL  ,  searchValue),
               new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnitPk),
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.VENDORTERMAUTO, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// Returns the search result list for Autocomplete 
        /// </summary>
        /// <param name="BizUnitPk"></param>
        /// <Createdby>Vineeth Babu</Createdby>
        /// <For>Po Generation</For>
        /// <usedin>Po Creation Listing Vendor Terms</usedin>
        /// <returns></returns>
        public static DataTable GetVendorTerms(int vendorID,int termspk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.TERMSPK,termspk == 0?(object)DBNull.Value:termspk),
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.VENDORID , vendorID),
            };
            DataTable dtTerms = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORTERMS, colParameters).Tables[0];
            return dtTerms;

        }
    }

    
}
