using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Administration.Configurations;

namespace BusinessLogic.Administration.Configurations
{
    public class MailSendBL
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
            return MailSendDA.GetMailDetails(mailPK, process, fromDate, toDate, bizUnit);
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
            return MailSendDA.GetMailFilterFieldAuto(fieldName, searchText, bizUnit);
        }
    }
}
