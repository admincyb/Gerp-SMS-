using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.OutStandingDue;

namespace BusinessLogic.OutStandingDue
{
    public class OutStandingDueBL
    {
        /// <summary>
        /// Get Customer List
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static DataTable GetOutStandingDueList(string xml, int pageNo, int pageSize)
        {
            return OutStandingDueDL.GetOutStandingDueList(xml,pageNo, pageSize);
        }

        /// <summary>
        /// Save Mail Sent Customer List
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static int SaveMail(string pXML)
        {
            return OutStandingDueDL.SaveMail(pXML);
        }
    }
}
