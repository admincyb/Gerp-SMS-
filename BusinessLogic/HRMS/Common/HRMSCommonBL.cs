using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.HRMS.Common;
using BusinessObject.HRMS.Employee;

namespace BusinessLogic.HRMS.Common
{
    public class HRMSCommonBL
    {
        public static DataTable GetHrmsCommonConstMst(int groupTypeValue, int groupValue, int? bizUnit, int configPk = 0, int active = 1)
        {
            return HRMSCommonDL.GetHrmsCommonConstMst(groupTypeValue, groupValue, bizUnit, configPk, active);
        }

        public static DataTable GetHrmsCommonConfigMst(string configType = null, string configSplCond = null, int? bizUnit = null, int configPk = 0, int active = 1)
        {
            return HRMSCommonDL.GetHrmsCommonConfigMst(configType, configSplCond, bizUnit, configPk, active);
        }
        /// <summary>
        /// Validate Formula
        /// </summary>
        /// <param name="formulaCode"></param>
        /// <param name="formulaName"></param>
        /// <returns></returns>
        public static int ValidateFormula(string formulaCode, string formulaName, out string formulaCodeOut, out string formulaNameOut)
        {
            return HRMSCommonDL.ValidateFormula(formulaCode, formulaName, out formulaCodeOut, out formulaNameOut);
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
            return HRMSCommonDL.GetCashOrBank(bizUnit, type, accType, pk, active);
        }


        public static string GetNextTabUrl(int curTab, string[] strTabNextURL, string[] strInactivetabs, string defaultUrl)
        {
            string nextUrl = defaultUrl;
            List<EmpTabDetails> tabDetails = new List<EmpTabDetails>();
            foreach (string s in strTabNextURL)
            {
                string[] pageDetails = s.ToString().Split('-');
                tabDetails.Add(new EmpTabDetails() { Id = int.Parse(pageDetails[0]), NextPageUrl = pageDetails[1].ToString() });
            }
            if (tabDetails.Any(cus => cus.Id == curTab))
            {
                while (true)
                {
                    if (strInactivetabs.Contains((curTab + 1).ToString()))
                        curTab++;
                    else
                    {
                        var record = tabDetails.Where(x => x.Id == curTab).SingleOrDefault();
                        if (record != null)
                            nextUrl = tabDetails.Where(x => x.Id == curTab).SingleOrDefault().NextPageUrl;
                        break;
                    }
                }
            }
            return nextUrl;
        }
    }
}
