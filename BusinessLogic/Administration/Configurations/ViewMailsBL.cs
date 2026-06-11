using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Administration.Configurations;

namespace BusinessLogic.Administration.Configurations
{
    public class ViewMailsBL
    {
        /// <summary>
        /// method for  Get Mail Details Details
        /// </summary>
        /// <param name="costCenterPK"></param>
        /// <param name="mailPK"></param>
        /// <returns></returns>
        public static DataTable GetMailDetails(int costCenterPK, int mailPK)
        {
            return ViewMailsDA.GetMailDetails(costCenterPK, mailPK);
        }
        /// <summary>
        /// Medhod to get mails for mail send service
        /// </summary>
        /// <param name="costCenterPK"></param>
        /// <param name="mailPK"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static DataTable GetMailDetails(int costCenterPK, int mailPK, int count)
        {
            return ViewMailsDA.GetMailDetails(costCenterPK, mailPK, count);
        }
        /// <summary>
        /// method for saving Mail Details details.
        /// </summary>
        /// <param name="mailStatus"></param>
        /// <param name="mailPK"></param>
        /// <returns></returns>
        public static int SaveMailDetails(int mailStatus, int mailPK)
        {

            return ViewMailsDA.SaveMailDetails(mailStatus, mailPK);
        }


    }
}
