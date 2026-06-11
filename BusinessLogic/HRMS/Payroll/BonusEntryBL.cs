using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.HRMS.Employee;
using DataAccess.HRMS.Payroll;
using BusinessLogic.HRMS.Admin.Masters;
using DataAccess.HRMS.Admin.Masters;
using BusinessObject.HRMS.Payroll;
using ERP.Utilities.HRMS;
using ERP.Utilities;

namespace BusinessLogic.HRMS.Payroll
{
    public class BonusEntryBL
    {

        public static DataTable GetEmployeeBonusList(string fromDate, string ToDate, int bonusType, int bizUnit, int status, int pageNo, int pageSize)
        {
            return BonusEntryDL.GetEmployeeBonusList(fromDate, ToDate, bonusType, bizUnit, status, pageNo, pageSize);
        }

        public static DataTable GetBonusType(int bizUnit)
        {
            return BonusEntryDL.GetBonusType(bizUnit);
        }

        public static DataTable GetBonusType(int currpk,int bizUnit,int active)
        {
            return BonusEntryDL.GetBonusType(currpk,bizUnit,active);
        }

        public static int? SaveBonusEntryDetails(string strxml, out string TrxNo)
        {
            return BonusEntryDL.SaveBonusEntryDetails(strxml, out TrxNo);
        }

        public static DataTable GetEmployeeBonusDetailsPopUp(FilterParameters objFilterParam)
        {
            try
            {
                return BonusEntryDL.GetEmployeeBonusDetailsPopUp(objFilterParam);
            }
            catch
            {
                throw;
            }
        }

        public static BonusEntryHeader GetEmployeeDetailsByPK(int itemPK)
        {
            try
            {
                BonusEntryHeader objBonusMaster = new BonusEntryHeader();
                string dtl = BonusEntryDL.GetEmployeeDetailsByPK(itemPK);
                if (dtl != string.Empty)
                {
                    objBonusMaster = (BonusEntryHeader)CommonFunctions.DeserializeObject(dtl, objBonusMaster);
                    return objBonusMaster;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static int DeleteEmployeeBonus(int pk, string lastModifiedDate)
        {
            return BonusEntryDL.DeleteEmployeeBonus(pk, lastModifiedDate);
        }
    }
}
