using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Mailer;
using BusinessObject;
namespace BusinessLogic.Mailer
{
    public class SaleForecastBL
    {
        /// <summary>
        /// Get Customer List
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static int SaveMail(string pXML)
        {
            return SaleForecastDL.SaveMail(pXML);
        }

        /// <summary>
        /// Get Mail Template
        /// </summary>
        /// <param name="aptcode"></param>
        /// <returns></returns>
        public static DataSet GetMailTemplate(string aptcode)
        {
            return SaleForecastDL.GetMailTemplate(aptcode);
        }
    }
}

