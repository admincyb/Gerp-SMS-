using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTIService.Constants.HRMS.Payroll;
using ERP.Utilities.HRMS;
using System.Data;

namespace DataAccess.HRMS.Payroll
{
    public class Income_Tax_01DL
    {

        /// Method to Save Income Tax Details
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SaveIncomeTax_01(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(Parameters.P_XML , xmlstr),
                new DBService.Parameters(Parameters.P_RET_VAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)                
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPHRM_HRM_INCOME_TAX_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[Parameters.P_RET_VAL]).Value);
            return result;
        }

        public static DataSet GetTaxAmountByPk(int? IT1_EPS_PK, int IT1_EMP_PK, int IT1_TYPE, int IT1_SECTION, int IT1_ITEM = 0)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.IT1_TYPE, IT1_TYPE >0?IT1_TYPE:(object)DBNull.Value),
                new DBService.Parameters(Parameters.IT1_SECTION, IT1_SECTION >0?IT1_SECTION:(object)DBNull.Value),        
                new DBService.Parameters(Parameters.IT1_ITEM, IT1_ITEM >0?IT1_ITEM:(object)DBNull.Value),
                new DBService.Parameters(Parameters.IT1_EPS_PK, IT1_EPS_PK >0?IT1_EPS_PK:(object)DBNull.Value),        
                new DBService.Parameters(Parameters.IT1_EMP_PK, IT1_EMP_PK >0?IT1_EMP_PK:(object)DBNull.Value)
            };

            DataSet dsResult = new DataSet();
            dsResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_INCOME_TAX_01_GET_KV, colParameters);
            return dsResult;
        }

        public static string GetEmployeePayrollDetailList(int epsPK)
        {
            string strRetVal = "";
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_EPS_PK, epsPK == 0 ? (object)DBNull.Value : epsPK)
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.SPHRM_EMP_INCOME_TAX_01_GET_XML, colParameters).Tables[0];
            foreach (DataRow dr in dtxml.Rows)
            {
                strRetVal += dr[0].ToString();
            }
            return strRetVal;
        }
    }
}
