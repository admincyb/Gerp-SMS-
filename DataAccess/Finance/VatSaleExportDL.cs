using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Finance;

namespace DataAccess.Finance
{
    public class VatSaleExportDL
    {
        /// <summary>
        /// Get Vat Sale List 
        /// </summary>
        public static string GetVatSaleList(GridPrams grid, User objUser)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            string strRetVal = "";
            if (grid != null)
            {
                colParameters = new DBService.Parameters[] 
                {            
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.FROMDATE , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.TODATE , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT , objUser.SBUID)
                };
            }
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETVATSALELIST, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
        /// <summary>
        /// Print Vat Sale
        /// </summary>
        public static DataSet GetVatSaleReport(DateTime fromDate, DateTime toDate, User objUser)
        {
            DataSet dsVatSaleReport = new DataSet();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
                {                
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.FROMDATE, fromDate),  
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.TODATE, toDate),  
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT , objUser.SBUID)
                };
            dsVatSaleReport = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETVATSALEREPORT, colParameters);
            return dsVatSaleReport;
        }
        /// <summary>
        /// Save Vat Sale Save
        /// </summary>
        public static DataSet SaveVatSale(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            DataSet dsVatSaleRetList=new DataSet();
            DataSet dsVatSale = new DataSet();
            DataTable dtVatSale = new DataTable();
            DataRow drVatSale = dtVatSale.NewRow();
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SAVEVATSALELIST, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            dtVatSale.Columns.Add("Column1", typeof(int));
            drVatSale["Column1"] = result;
            dtVatSale.Rows.Add(drVatSale);
            dsVatSaleRetList.Tables.Add(dtVatSale);
            if (result == 0)//Check for duplication of invoice no:
            {
                dsVatSale = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SAVEVATSALELIST, colParameters);
                dsVatSaleRetList.Merge(dsVatSale);
            }
            return dsVatSaleRetList;
        }
        /// <summary>
        /// Get Customer name for auto complete in search
        /// </summary>
        public static DataTable GetVatSaleFieldAuto(byte Active, string itemField, int bizUnit, string searchValue)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters( GTIService.Constants.Common.Common.P_ACTIVE, Active),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.FIELD, itemField),
                new DBService.Parameters( GTIService.Constants.Common.Common.P_BIZUNIT, bizUnit),
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.SEARCHVALAUTO, searchValue)
            };
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.VSEFIELDAUTO, colParameters).Tables[0];
            return dtProcess;
        }
        /// <summary>
        /// Get Vat Sale Search List 
        /// </summary>
        public static DataTable GetVatSaleSearchList(GridPrams grid,string customerName,string invoiceNo, User objUser)
        {
            DataTable dtProcess = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            if (grid != null)
            {
                colParameters = new DBService.Parameters[] 
                {            
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.FROMDATE , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.TODATE , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.TSD_INVOICE_NO , invoiceNo== string.Empty ?(Object)DBNull.Value : invoiceNo=="Select/Type"?(Object)DBNull.Value : invoiceNo),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.TSD_CUSTOMER_NAME , customerName== string.Empty ?(Object)DBNull.Value : customerName =="Select/Type"?(Object)DBNull.Value:customerName),
                    new DBService.Parameters(GTIService.Constants.Finance.Parameters.P_BIZUNIT , objUser.SBUID)
                };
            }
            dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.GETVATSALESEARCHLIST, colParameters).Tables[0];
            return dtProcess;
        }
    }
}
