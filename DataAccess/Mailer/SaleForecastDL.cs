using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Utilities;
using DataAccess;
using BusinessObject.Constants;
using BusinessObject;
using ERP.Utilities.Constants;

namespace DataAccess.Mailer
{
    public class SaleForecastDL
    {
        /// <summary>
        /// Save Brand Details
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveMail(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(MailerDA.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, "SPSAL_FORECAST_MAIL_DTL_SAVE", colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// Get Mail Template
        /// </summary>
        /// <param name="aptcode"></param>
        /// <returns></returns>
        public static DataSet GetMailTemplate(string aptcode)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                  new DBService.Parameters("P_TML_PK",(Object)DBNull.Value),
                  new DBService.Parameters("P_APT_CODE",aptcode),
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, "SPADM_MAIL_TEMPLATE_GET_KV", colParameters);
            }
            return ds;

        }
    }
}

