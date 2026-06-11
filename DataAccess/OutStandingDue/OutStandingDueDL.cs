using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.OutStandingDue
{
   public class OutStandingDueDL
    {
        /// <summary>
        /// Get Customers List
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
       public static DataTable GetOutStandingDueList(string xml, int pageNo, int pageSize)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                    new DBService.Parameters(GTIService.Constants.OutStandingDue.Parameters.P_XML, xml),
                    new DBService.Parameters(GTIService.Constants.OutStandingDue.Parameters.P_PAGE_NUM,  pageNo),
                    new DBService.Parameters(GTIService.Constants.OutStandingDue.Parameters.P_PAGE_SIZE, pageSize)
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.OutStandingDue.Procedures.SPFIN_OUTSTANDING_DUE_RPT, colParameters);
            }
            return ds.Tables[0];

        }

       /// <summary>
       /// Save Mail Sent Customers
       /// </summary>
       /// <param name="pXML"></param>
       /// <returns></returns>
       public static int SaveMail(string pXML)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.OutStandingDue.Parameters.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
           int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.OutStandingDue.Procedures.SPSAL_OUTSTANDING_MAIL_DTL_SAVE, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
           return result;
       }
    }
}
