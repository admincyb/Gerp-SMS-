using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.HRMS.Common
{
    public class HRMSCommonDL
    {
        public static DataTable GetHrmsCommonConstMst(int? groupTypeValue = null, int? groupValue = null, int? bizUnit = null, int configPk = 0, int active = 1)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.CON_PK, configPk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ACTIVE,  active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_GROUP_TYPE_VALUE, 
                                            groupTypeValue.HasValue?groupTypeValue.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_GROUP_VALUE,  
                                            groupValue.HasValue? groupValue.Value:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT, bizUnit.HasValue?bizUnit.Value:(object)DBNull.Value),
            };

            DataTable dtCommonConfig = dbService.DataAdapter(CommandType.StoredProcedure
                                            , GTIService.Constants.HRMS.Employee.Procedures.GET_COMMONDROPDOWNLIST
                                            , colParameters).Tables[0];
            return dtCommonConfig;
        }

        public static DataTable GetHrmsCommonConfigMst(string configType = null, string configSplCond = null, int? bizUnit = null, int configPk = 0, int active = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.CFG_PK, configPk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ACTIVE,  active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CFG_TYPE, !string.IsNullOrWhiteSpace(configType)?configType:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CFG_SPL_COND, !string.IsNullOrWhiteSpace(configSplCond)? configSplCond:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT, bizUnit.HasValue?bizUnit.Value:(object)DBNull.Value),
            };

            DataTable dtCommonConfig = dbService.DataAdapter(CommandType.StoredProcedure
                                            , GTIService.Constants.HRMS.Admin.Masters.Procedures.SPADM_CONFIG_MST_GET_KV
                                            , colParameters).Tables[0];
            return dtCommonConfig;
        }

        /// <summary>
        /// To validate formula
        /// </summary>
        /// <param name="formulaCode"></param>
        /// <param name="formulaName"></param>
        /// <returns></returns>
        public static int ValidateFormula(string formulaCode,string formulaName, out string formulaCodeOut,out string formulaNameOut)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_FORMULA_CODE, !string.IsNullOrWhiteSpace(formulaCode)?formulaCode:(object)DBNull.Value, 500,ParameterDirection.InputOutput, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.HRMS.Payroll.Parameters.P_FORMULA_NAME, !string.IsNullOrWhiteSpace(formulaName)?formulaName:(object)DBNull.Value, 500,ParameterDirection.InputOutput, DBService.ParameterType.NVarChar),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.Payroll.Procedures.SPHRM_FORMULA_VALIDATE, colParameters);
            formulaCodeOut = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Payroll.Parameters.P_FORMULA_CODE]).Value);
            formulaNameOut = Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.HRMS.Payroll.Parameters.P_FORMULA_NAME]).Value);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="type">1 => CASH 2 => BANK</param>
        /// <param name="accType"></param>
        /// <param name="pk"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetCashOrBank(int? bizUnit, int? type, int? accType, int pk = 0, int active = 1)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CBM_PK, pk),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_ACTIVE,  active),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CBM_TYPE, type??(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_CBM_ACC_TYPE, accType?? (object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.HRMS.Employee.Parameters.P_BIZUNIT, bizUnit??(object)DBNull.Value),
            };

            DataTable dtTable = dbService.DataAdapter(CommandType.StoredProcedure
                                            , GTIService.Constants.HRMS.Employee.Procedures.SPFIN_CASH_BANK_MST_GET_KV
                                            , colParameters).Tables[0];
            return dtTable;

        }
    }
}
