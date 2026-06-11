using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.HRMS.Payroll;
using BusinessObject.HRMS.Payroll;
using GTIService;
using ERP.Utilities.HRMS;
namespace BusinessLogic.HRMS.Payroll
{
    public class OvertimeCalculatorBL
    {
        //Get OT Master List
        public static string GetMonthlyOtList(FilterParameters objFilterParam)
        {
            return OvertimeCalculatorDL.GetMonthlyOtList(objFilterParam);
        }

        /// <summary>
        /// Method to Save Monthly OT Master
        /// </summary>
        /// <param name="stockTransferDtls"></param>
        /// <returns>int</returns>
        public static int SaveMonthlyOtMaster(string xmlstr, out string EmpNames, out string TrxNo)
        {
            return OvertimeCalculatorDL.SaveMonthlyOtMaster(xmlstr, out EmpNames, out TrxNo);
        }

        /// <summary>
        /// Method to Save Other Leave Entry
        /// </summary>
        /// <param name="OtherLeaveEntryDetails"></param>
        /// <returns>int</returns>
        public static int SaveOtherLeaveEntry(string xmlstr, out string EmpNames, out string TrxNo)
        {
            return OvertimeCalculatorDL.SaveOtherLeaveEntry(xmlstr, out EmpNames, out TrxNo);
        }

        /// <summary>
        /// Method to Get Yearly OT List for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetYearlyOtListListingPage(FilterParameters objFilterParam)
        {
            return OvertimeCalculatorDL.GetYearlyOtListListingPage(objFilterParam);
        }
        /// <summary>
        /// Method to Get Yearly Other Leave Entry List for Listing Page
        /// </summary>
        /// <param name="gridParam"></param>
        ///<param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetYearlyOtherLeaveEntryListingPage(FilterParameters objFilterParam)
        {
            return OvertimeCalculatorDL.GetYearlyOtherLeaveEntryListingPage(objFilterParam);
        }

        public static OvertimeCalculatorBO.MonthlyOTMaster GetOTDetailsByPK(int bizUnit, int status, int itemPK)
        {
            try
            {
                OvertimeCalculatorBO.MonthlyOTMaster addOTMaster = new OvertimeCalculatorBO.MonthlyOTMaster();
                string dtl = OvertimeCalculatorDL.GetOTDetailsByPK(bizUnit, status, itemPK);
                if (dtl != string.Empty)
                {
                    addOTMaster = (OvertimeCalculatorBO.MonthlyOTMaster)CommonFunctions.DeserializeObject(dtl, addOTMaster);
                    return addOTMaster;
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
        public static OvertimeCalculatorBO.MonthlyOTMaster OTHERLEAVEENTRYDETAILSBYPK(int bizUnit, int status, int EOLPK)
        {
            try
            {
                OvertimeCalculatorBO.MonthlyOTMaster addOLMaster = new OvertimeCalculatorBO.MonthlyOTMaster();
                string dtl = OvertimeCalculatorDL.OTHERLEAVEENTRYDETAILSBYPK(bizUnit, status, EOLPK);
                if (dtl != string.Empty)
                {
                    addOLMaster = (OvertimeCalculatorBO.MonthlyOTMaster)CommonFunctions.DeserializeObject(dtl, addOLMaster);
                    return addOLMaster;
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
        
        /// <summary>
        /// Method to delete OT Details
        /// </summary>
        /// <param name="HeaderPk">EOT PK </param>
        /// <param name="DetailPk">EOE PK </param>
        /// <param name="lastModDate">Modified Date</param>
        /// <returns></returns>
        public static int? DeleteOTDetails(int? HeaderPk, int? detailsPk, DateTime? lastModDate)
        {
            return OvertimeCalculatorDL.DeleteOTDetails(HeaderPk, detailsPk, lastModDate);
        }
        /// <summary>
        /// Method to delete Other Leave Entry Details
        /// </summary>
        /// <param name="HeaderPk">EOL PK </param>
        /// <param name="DetailPk">EOD PK </param>
        /// <param name="lastModDate">Modified Date</param>
        /// <returns></returns>
        public static int? DeleteOtherLeaveEntryDetails(int? HeaderPk, int? detailsPk, DateTime? lastModDate)
        {
            return OvertimeCalculatorDL.DeleteOtherLeaveEntryDetails(HeaderPk, detailsPk, lastModDate);
        }
        

        public static DataTable GetOvertimeDetailsNumbers(byte Active, int bizUnit, string searchValue)
        {
            try
            {
                return OvertimeCalculatorDL.GetOvertimeDetailsNumbers(Active, bizUnit, searchValue);
            }
            catch
            {
                throw;
            }
        }
         public static DataTable GetOtherLeaveEntryNumbers(byte Active, int bizUnit, string searchValue)
        {
            try
            {
                return OvertimeCalculatorDL.GetOtherLeaveEntryNumbers(Active, bizUnit, searchValue);
            }
            catch
            {
                throw;
            }
        }

    }
}
