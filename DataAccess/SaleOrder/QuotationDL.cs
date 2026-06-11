using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Common;

namespace DataAccess.SaleOrder
{
    public class QuotationDL
    {
        public static string GetQuotation(int enqPK)
        {
            //DBService dbService = new DBService();

            //DBService.Parameters[] colParameters = null;
            //colParameters = new DBService.Parameters[] 
            //{   
            //    new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_RFH_PK,  enqPK),
                                          
            //};

            //DataSet dsQuotation = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.GETQUOTATION, colParameters);
            //return dsQuotation;
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                //new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_RFH_PK, enqPK),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.CEH_PK, enqPK)
               
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.RequestForQuote.Procedures.GETQUOTATION, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }

        public static int? SaveQuotationDetails(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SAVEQUOTATION, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;

        }

        public static DataSet GetQuotationTaxDetails(int taxPK, int category, int bizUnit, byte active)
        {
            DBService dbService = new DBService(); 
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, active),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXPK, taxPK == 0 ? (object) DBNull.Value : taxPK ),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.TaxSettings.Parameters.TAXCATEGORY, category == 0 ? (object) DBNull.Value : category),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, bizUnit)          
            };
            DataSet dsRFQTaxDetails = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.TaxSettings.Procedures.CATEGORYVALUEGET, colParameters);
            return dsRFQTaxDetails;
        }

        public static int? GenerateSaleOrder(int CurrPK, int userPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.CEH_PK, CurrPK),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK, userPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GENERATESALEORDER, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataSet GetItemDetails(int itemPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
              new DBService.Parameters(GTIService.Constants.Sales.Parameters.ITEM_PK, itemPK)
            };

            DataSet dsDesig = new DataSet();
            dsDesig = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GETITEMDETAILS, colParameters);
            return dsDesig;
        }

        public static DataSet GetItemRates(int itemPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.RequestForQuote.Parameters.P_ITM_PK, itemPK > 0 ? itemPK : (object)DBNull.Value)
            };
            DataSet dsItemRates = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GET_ITEM_RATES, colParameters);
            return dsItemRates;
        }
        public static DataSet GetQuoationRevisionHistory(int itemPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.CEH_PK, itemPK > 0 ? itemPK : (object)DBNull.Value)
            };
            DataSet dsItemRates = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.GETQUOTATIONARCHIVE, colParameters);
            return dsItemRates;
        }

        public static DataTable GetCrmQuoations(string Value, int CustomerPK, int SbuID)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.VALUE, Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CUS_PK, CustomerPK > 0 ? CustomerPK : (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_BIZUNIT, SbuID)
            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPCRM_LEAD_QUOTE_HDR_AUTO, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable GetCrmQuoationDetails(int QuoteId)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CQH_PK, QuoteId)
            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPCRM_LEAD_QUOTE_HDR_SO_MPG_GET, colParameters).Tables[0];
            return dtResult;
        }

        public static DataTable ValidateCRMCustomer(int QuoteId)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Sales.Parameters.P_CQH_PK, QuoteId)
            };
            DataTable dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Sales.Procedures.SPSAL_ORDER_CRM_CUSTOMER_VALIDATE, colParameters).Tables[0];
            return dtResult;
        }
    }
}
