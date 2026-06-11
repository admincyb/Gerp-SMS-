using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERP.Utilities.Constants.DA.Administration;

namespace DataAccess.Administration.Configurations
{
    public class MailSendDA
    {
        /// <summary>
        /// method for Get Mail  Details
        /// </summary>
        /// <param name="mailPK"></param>
        /// <param name="process"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DataTable GetMailDetails(int mailPK, int process, string fromDate, string toDate, int bizUnit)
        {
            DataTable dtMailDetails;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(MailSend.P_MLQ_PK , mailPK==0?(object)DBNull.Value:mailPK),
                new DBService.Parameters(MailSend.P_PROCESS , process<=0?(object)DBNull.Value:process),
                new DBService.Parameters(MailSend.P_FROM_DATE, fromDate == string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(fromDate)),
                new DBService.Parameters(MailSend.P_TO_DATE, toDate == string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(toDate)),
                new DBService.Parameters(MailSend.P_MLQ_STATUS , DBNull.Value),
                new DBService.Parameters(MailSend.P_BIZUNIT , bizUnit)
            };
            dtMailDetails = dbService.DataAdapter(CommandType.StoredProcedure, MailSend.SP_GET_MAILQUE, colParameters).Tables[0];
            return dtMailDetails;
        }
        /// <summary>
        /// Get Filter Field Auto
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="searchText"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataTable GetMailFilterFieldAuto(string fieldName, string searchText, int bizUnit)
        {
            DataTable dtMailDetails;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(ERP.Utilities.CommonConstants.SEARCH_TYPE , fieldName),
                new DBService.Parameters(ERP.Utilities.CommonConstants.SEARCH_VALUE , searchText),
                new DBService.Parameters(MailSend.P_BIZUNIT , bizUnit)
            };
            dtMailDetails = dbService.DataAdapter(CommandType.StoredProcedure, MailSend.SP_GET_MAIL_QUEUE_AUTO, colParameters).Tables[0];
            return dtMailDetails;
        }
    }
}
