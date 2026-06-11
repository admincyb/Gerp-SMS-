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
    public class SalaryPaymentBL
    {

        /// <summary>
        /// For get   lsit
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        //public static DataTable GetSalaryPaymentList(string fromDate, string ToDate, int paymentType_Pk, int bankPk, int bizUnit, int status, int pageNo, int pageSize, string chequeNo)
        //{
        //    return SalaryPaymentDL.GetSalaryPaymentList(fromDate, ToDate, paymentType_Pk, bankPk, bizUnit, status, pageNo, pageSize, chequeNo);
        //}
        public static DataTable GetSalaryPaymentList(FilterParameters gridParam, int bankPk)
        {
            return SalaryPaymentDL.GetSalaryPaymentList( gridParam,bankPk);
        }

        public static SalaryPaymentHeader_PopUp GetSalaryPaymentDetails(FilterParameters objFilterParams)
        {
            try
            {
                SalaryPaymentHeader_PopUp addolidayMaster = new SalaryPaymentHeader_PopUp();
                string dtl = SalaryPaymentDL.GetSalaryPaymentDetails( objFilterParams);
                if (dtl != string.Empty)
                {
                    addolidayMaster = (SalaryPaymentHeader_PopUp)CommonFunctions.DeserializeObject(dtl, addolidayMaster);
                    return addolidayMaster;
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

        public static int? SaveSalaryPaymentDetails(string strxml, out string TrxNo)
        {
            try
            {
                return SalaryPaymentDL.SaveSalaryPaymentDetails(strxml, out TrxNo);
            }
            catch
            {
                throw;
            }
        }

        public static SalaryPaymentHeader GetSalaryPaymentByPK(int bizUnit, int status, int itemPK)
        {
            try
            {
                SalaryPaymentHeader addolidayMaster = new SalaryPaymentHeader();
                string dtl = SalaryPaymentDL.GetSalaryPaymentByPK(bizUnit, status, itemPK);
                if (dtl != string.Empty)
                {
                    addolidayMaster = (SalaryPaymentHeader)CommonFunctions.DeserializeObject(dtl, addolidayMaster);
                    return addolidayMaster;
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


        public static int DeleteSalaryPayment(int pk, string lastModifiedDate)
        {
            return SalaryPaymentDL.DeleteSalaryPayment(pk, lastModifiedDate);
        }


        public static DataSet GetSalaryPaymentRPT(int currPK)
        {
            return SalaryPaymentDL.GetSalaryPaymentRPT(currPK);
        }
        public static DataSet GetEmpSalaryPaymentRPT(int currPK)
        {
            return SalaryPaymentDL.GetEmpSalaryPaymentRPT(currPK);
        }

        public static DataTable GetDATDetails(int PymntModDetPk, out string srtResult)
        {
            return SalaryPaymentDL.GetDATDetails(PymntModDetPk,out srtResult);
        }

        public static DataTable GetSalaryPaymentNumbers(byte Active, int bizUnit, string searchValue)
        {
            try
            {
                return SalaryPaymentDL.GetSalaryPaymentNumbers(Active, bizUnit, searchValue);
            }
            catch
            {
                throw;
            }
        }
    }
}
