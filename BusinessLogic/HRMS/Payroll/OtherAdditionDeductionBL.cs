using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.HRMS.Payroll;
using System.Data;
using BusinessObject.HRMS.Payroll;
using GTIService;
using ERP.Utilities.HRMS;

namespace BusinessLogic.HRMS.Payroll
{
    public class OtherAdditionDeductionBL
    {
        public static int? SaveAdditionDeductionDetails(string strxml, out string TrxNo)
        {
            return OtherAdditionDeductionDL.SaveAdditionDeductionDetails(strxml, out TrxNo);
        }

        public static DataTable GetAdditionDeductionList(FilterParameters gridParam, int bizUnit, int type, int payelmt)
        {
            return OtherAdditionDeductionDL.GetAdditionDeductionList(gridParam, bizUnit, type, payelmt);
        }

        public static AdditionDeductionHeader GetAdditionDeductionByPK(int bizUnit, int status, int itemPK)
        {
            try
            {
                AdditionDeductionHeader addDedObj = new AdditionDeductionHeader();
                string dtl = OtherAdditionDeductionDL.GetAdditionDeductionByPK(bizUnit, status, itemPK);
                if (dtl != string.Empty)
                {
                    addDedObj = (AdditionDeductionHeader)CommonFunctions.DeserializeObject(dtl, addDedObj);
                    return addDedObj;
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
            // return OtherAdditionDeductionDL.GetAdditionDeductionByPK(bizUnit, status, itemPK);
        }

        public static int DeleteAdditionDeduction(int pk, string lastModifiedDate)
        {
            return OtherAdditionDeductionDL.DeleteAdditionDeduction(pk, lastModifiedDate);
        }

        public static AdditionDeductionHeader GetAdditionDeduction(FilterParameters objFilterParams, decimal Amount, string Remarks)
        {
            try
            {
                AdditionDeductionHeader addDedObj = new AdditionDeductionHeader();
                string dtl = OtherAdditionDeductionDL.GetAdditionDeduction(objFilterParams, Amount, Remarks);
                if (dtl != string.Empty)
                {
                    addDedObj = (AdditionDeductionHeader)CommonFunctions.DeserializeObject(dtl, addDedObj);
                    return addDedObj;
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

        public static DataTable GetAdditionDeductionNumbers(byte Active, int bizUnit, string searchValue)
        {
            try
            {
                return OtherAdditionDeductionDL.GetAdditionDeductionNumbers(Active, bizUnit, searchValue);
            }
            catch
            {
                throw;
            }
        }

        public static int AdditionDeductionImport(string xmlLanding, ref DataTable dtOut)
        {
            return OtherAdditionDeductionDL.AdditionDeductionImport(xmlLanding, ref dtOut);
        }
    }
}
