using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.HRMS.Employee;
using System.Data;
using ERP.Utilities;
using DataAccess.HRMS.Employee;
using ERP.Utilities.HRMS;

namespace BusinessLogic.HRMS.Employee
{
    public class EmployeeTransferBL
    {

        public static int? SaveEmployeeTransfer(string xmlDoc, out string trxNo, out DataTable dtErrorList)
        {
            return EmployeeTransferDL.SaveEmployeeTransfer(xmlDoc, out trxNo, out dtErrorList);
        }

        public static DataTable GetTransferReason(int ConPK, int Active)
        {
            return EmployeeTransferDL.GetTransferReason(ConPK, Active);
        }

        public static DataTable GetEmpTransferList(FilterParameters objFilterParam)
        {
            return EmployeeTransferDL.GetEmpTransferList(objFilterParam);
        }

        public static EmployeeTransferHeader GetEmpTransferDetails(int CurrPK)
        {
            try
            {
                EmployeeTransferHeader objEmpTransfer = new EmployeeTransferHeader();
                string xmlResult = EmployeeTransferDL.GetEmpTransferDetails(CurrPK);
                if (xmlResult != string.Empty)
                {
                    objEmpTransfer = (EmployeeTransferHeader)CommonFunctions.DeserializeObject(xmlResult, objEmpTransfer);
                    return objEmpTransfer;
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

        public static int? DeleteEmpTransfer(int CurrPK, DateTime LastModifiedTime)
        {
            return EmployeeTransferDL.DeleteEmpTransfer(CurrPK, LastModifiedTime);
        }

        public static DataTable GetEmpTransferNumbers(byte Active, int bizUnit, string searchValue)
        {
            try
            {
                return EmployeeTransferDL.GetEmpTransferNumbers(Active, bizUnit, searchValue);
            }
            catch
            {
                throw;
            }
        }

        public static DataSet GetEmployeeTransferReport(int currPK)
        {
            return EmployeeTransferDL.GetEmployeeTransferReport(currPK);
        }
    }
}
