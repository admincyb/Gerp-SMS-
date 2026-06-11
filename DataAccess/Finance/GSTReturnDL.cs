using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using GTIService.Constants.Finance;

namespace DataAccess.Finance
{
   public class GSTReturnDL
    {
       //Get Details [Go btn]
       public static string GetDetails(DateTime startDate, DateTime endDate, int bizUnit, byte Active, int companyPk)
       {
           DataTable dtProcess = new DataTable();
           DBService dbService = new DBService();
           string strRetVal = "";
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
                {
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.FROM_DATE, startDate),
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.TO_DATE, endDate),
                    //new DBService.Parameters( GTIService.Constants.Common.Common.P_ACTIVE, Active),
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizUnit),
                    //new DBService.Parameters( GTIService.Constants.Finance.Parameters.COMPANY_PK, companyPk),
                };
           dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETDETAILS, colParameters).Tables[0];
           foreach (DataRow dr in dtProcess.Rows)
           {
               strRetVal += dr[0].ToString();
           }
           return strRetVal;
       }
       //Get Record for edit
       public static string GetDetailsWithPk(int GSTReturnPk, int bizUnit, byte Active, int companyPk)
       {
           DataTable dtProcess = new DataTable();
           DBService dbService = new DBService();
           string strRetVal = "";
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
                {
                    new DBService.Parameters( GTIService.Constants.Finance.Parameters.TGH_PK, GSTReturnPk),
                    //new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizUnit),
                    //new DBService.Parameters( GTIService.Constants.Common.Common.P_ACTIVE, Active),
                    //new DBService.Parameters( GTIService.Constants.Finance.Parameters.COMPANY_PK, companyPk),
                };
           dtProcess = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETGSTRETURNDETAILS, colParameters).Tables[0];
           foreach (DataRow dr in dtProcess.Rows)
           {
               strRetVal += dr[0].ToString();
           }
           return strRetVal;
       }
       public static DataSet GetGSTReturnReport(int RecPK)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(Parameters.TGH_PK, RecPK==0?(object) DBNull.Value:RecPK)
              };
           DataSet dsList = new DataSet();
           dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETGSTRETURNREPORT, colParameters);
           return dsList;
       }
       //Save GST Return
       public static int? SaveGSTReturn(string strxml)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SAVEGSTRETURNLIST, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
           return result;
       }
       //Get GST Return grid
       public static DataSet GetReportList(byte Active, int bizUnit, int companyPk)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters( GTIService.Constants.Finance.Parameters.P_BIZUNIT, bizUnit),
              };
           DataSet dsList = new DataSet();
           dsList = dbService.DataAdapter(CommandType.StoredProcedure, Procedures.GETGSTRETURNLIST, colParameters);
           return dsList;
       }
       //For Delete Record
       public static int DeleteGSTReturn( int GSTReturnPk, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters( GTIService.Constants.Finance.Parameters.TGH_PK, GSTReturnPk),
                //(Object)DBNull.Value 
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_USER ,currentUser),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.DELETEGSTRETURN, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
