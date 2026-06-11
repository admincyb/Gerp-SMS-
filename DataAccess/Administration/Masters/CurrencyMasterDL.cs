using System;
using System.Data;
using BusinessObject;

namespace DataAccess.Administration.Masters
{
   public class CurrencyMasterDL
    {
        /// <summary>
        /// Get Exchange
        /// </summary>
        /// <returns>int- 1(Success)</returns>
        public static DataTable GetExchangeType()
        {
            DBService dbService = new DBService();
           
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.GETEXCHANGETYPE).Tables[0];

        }
        /// <summary>
        /// Used to delete exchange Type
        /// </summary>
        /// <param name="exchangeMasterId"></param>
        /// <returns>string</returns>
        public static string DeleteExchangeType(int exchangeMasterId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Currency.Parameters.EXCHANGEMASTERPK,  exchangeMasterId == 0 ? (object)DBNull.Value :  exchangeMasterId),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.DELETEEXCHANGETYPE, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);
        }
        /// <summary>
        /// Used to Save ExchangeMaster
        /// </summary>
        /// <param name="currencyDet"></param>
        /// <returns>string</returns>
        public static string SaveExchangeMaster(BusinessObject.Administration.Masters.CurrencyMaster currencyDet)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Currency.Parameters.EXCHANGEMASTERPK , currencyDet.ExchangeMasterPk== 0 ? (object)DBNull.Value : currencyDet.ExchangeMasterPk),
                new DBService.Parameters(GTIService.Constants.Currency.Parameters.EXCHANGETOPK , currencyDet.ExchangeToPk),
                new DBService.Parameters(GTIService.Constants.Currency.Parameters.EXCHANGERATE , currencyDet.ExchangeRate),                
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.SAVEEXCHANGEMASTER, colParameters);
            return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value);

        }
        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="GridPrams"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetCurrencyMasterList(GridPrams grid, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0"? (object)DBNull.Value : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL ,  grid.SearchValue == string.Empty ? (object)DBNull.Value : "%"+grid.SearchValue+"%"),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields== null ? (object)DBNull.Value : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy==null ? "CUR_NAME" :grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection==null ? "ASC" :  grid.SortDirection),
            };

            DataSet dtCurrency = new DataSet();
            dtCurrency = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.GETCURRENCYMASTERLISTBYSRH, colParameters);
            return dtCurrency;


        }
        /// <summary>
        /// SAVING CurrencyMaster DETAILS
        /// </summary>
        /// <param name="tankDet"></param>
        /// <returns> INT</returns>
        public static string SaveCurrencyMaster(string strxml)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
              
               
               
                new DBService.Parameters(GTIService.Constants.Currency.Parameters.STRXML,   strxml) ,
                //new DBService.Parameters(GTIService.Constants.Currency.Parameters.BIZUNIT,   tankDet.SBU) ,
                new DBService.Parameters(GTIService.Constants.Currency.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.SAVECURRENCYMASTER, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value).ToString();
        }
       /// <summary>
        /// Save Currency Conversion
       /// </summary>
       /// <param name="strxml"></param>
       /// <returns></returns>
        public static string SaveCurrencyConversion(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Currency.Parameters.STRXML,   strxml) ,
                //new DBService.Parameters(GTIService.Constants.Currency.Parameters.BIZUNIT,   tankDet.SBU) ,
                new DBService.Parameters(GTIService.Constants.Currency.Parameters.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.SPADM_CURRENCY_CONV_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value).ToString();
        }
        /// <summary>
        /// Methord to get the Search Vlaues Corresponding to Search Type 
        /// </summary>
        /// <param name="searchBy"></param>
        /// <param name="searchValue"></param>
       
        /// <returns>DataTable</returns>
        public static DataTable GetCurrencyMasterSearchValues(string searchValue, string searchBy, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Common.Parameters_Common.SEARCHBY ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVALUE,  searchBy) ,
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  bizUnit),
              //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,  sbuPK) 
            };
            DataTable dtSearchValue = new DataTable();
            dtSearchValue = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.GETCURRENCYMASTERSEARCHVALUE, colParameters).Tables[0];
            return dtSearchValue;

        }
        /// <summary>
        /// Delete Tank Details By TANKMASTERPK
        /// </summary>
        /// <param name="tankMasterPk"></param>
        /// <returns>int- 1(Success)</returns>
        public static int DeleteCurrencyMasterDtls(int currencyMasterPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Currency.Parameters.CURRENCYMASTERPK,  currencyMasterPk == 0 ? (object)DBNull.Value :  currencyMasterPk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),

            };

            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.DELETECURRENCYMASTERDETAILS, colParameters);

            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="curConversionPK"></param>
       /// <returns></returns>
        public static int DeleteCurrencyConversionDtls(int curConversionPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Currency.Parameters.V_CUC_PK,  curConversionPK == 0 ? (object)DBNull.Value :  curConversionPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.SPADM_CURRENCY_CONV_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }


        /// <summary>
        /// Get Compounds  for filling combo
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCurrencieForCombo(int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
            
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)
      
              
            };

            DataTable dtDispersions = new DataTable();
            dtDispersions = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.GETCURRENCYFORCOMBO, colParameters).Tables[0];
            return dtDispersions;
        }
        /// <summary>
        /// Get Currency  Details By  Id as A Xml Format
        /// </summary>
        /// <param name="DispersionID"></param>
        /// <returns>Xml Formatted Currency Details</returns>
        public static string GetCurrencyDetail(int currPk)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Currency.Parameters.CURPK ,  currPk),              
              
            };
            DataTable dtCurDetails = new DataTable();
            dtCurDetails = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.GETCURRENCYDETAILS, colParameters).Tables[0];
            string retVal = string.Empty;
            for (int i = 0; i < dtCurDetails.Rows.Count; i++)
                retVal += dtCurDetails.Rows[i][0];
            return retVal;
            //return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.GETCURRENCYDETAILS, colParameters).ToString();
            // return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Dispersion.Parameters.Retval]).Value);
        }
      
        public static DataSet GetCurrencyDetailsList(GridPrams grid, int currPk)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Currency.Parameters.CURPK ,  currPk),              
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS ,  grid.Fields== null ? (object)DBNull.Value : grid.Fields),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize),             

                // new DBService.Parameters(GTIService.Constants.Dispersion.Parameters.Retval, string.Empty,4000, ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };

            DataSet dtCurrencydetails = new DataSet();
            dtCurrencydetails = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.GETCURRENCYDETAILSLIST, colParameters);
            return dtCurrencydetails;

            //DataTable dtCurDetails = new DataTable();
            //dtCurDetails = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.GETCURRENCYDETAILSLIST, colParameters).Tables[0];
            //string retVal = string.Empty;
            //for (int i = 0; i < dtCurDetails.Rows.Count; i++)
            //    retVal += dtCurDetails.Rows[i][0];
            //return retVal;
            //return dbService.ExecuteScalar(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.GETCURRENCYDETAILS, colParameters).ToString();
            // return Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Dispersion.Parameters.Retval]).Value);
        }
        /// <summary>
        /// Returns the search result list for Autocomplete for Vendor exchange Currencies
        /// </summary>
        /// <param name="searchValue"></param>
        /// <param name="vendorPK"></param>
        /// <param name="excDate"></param>
        /// <returns></returns>
        public static DataTable GetVendorExchangeCurrencyAuto(string searchValue, int vendorPK, DateTime? excDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Currency.Parameters.CUR_VALUE ,  searchValue),
              new DBService.Parameters(GTIService.Constants.Currency.Parameters.VEN_PK,  vendorPK),
              new DBService.Parameters(GTIService.Constants.Currency.Parameters.DATE ,  excDate.HasValue ? excDate : (object)DBNull.Value) 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Currency.Procedures.GETVENDOREXCHANGECURRENCY, colParameters).Tables[0];
        }

    }
}
