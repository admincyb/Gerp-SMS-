using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ERP.Utilities.HRMS;
using DataAccess.HRMS.Payroll;
using BusinessObject.HRMS.Payroll;
using ERP.Utilities;

namespace BusinessLogic.HRMS.Payroll
{
  public  class BulkAppraisalBL
    {
      public static DataSet GetEmployeeAppraisalList(FilterParameters objFilterParam)
      {
          return BulkAppraisalDL.GetEmployeeAppraisalList(objFilterParam);
      }


      public static BulkAppraisalBO.EmpAppHeader GetEmployeeAppraisalDetails(FilterParameters objFilterParam)
        {
            try
            {
                BulkAppraisalBO.EmpAppHeader ObjEmployeeBasicInfomtn = new BulkAppraisalBO.EmpAppHeader();
                string employee = BulkAppraisalDL.GetEmployeeAppraisalDetails(objFilterParam);
                if (employee != string.Empty)
                {
                    ObjEmployeeBasicInfomtn = (BulkAppraisalBO.EmpAppHeader)CommonFunctions.DeserializeObject(employee, ObjEmployeeBasicInfomtn);
                    return ObjEmployeeBasicInfomtn;
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


      public static int? SaveBulkAppraisalDetails(string strxml, out string TrxNo, out DataTable dtErrorList)
      {
          try
          {
              return BulkAppraisalDL.SaveBulkAppraisalDetails(strxml, out TrxNo, out  dtErrorList);
          }
          catch
          {
              throw;
          }
      }

      //Fill employee in (XML)
      public static BulkAppraisalBO.SalaryBulkAppraisalHeader GetSalaryAppraisalByPk(int empPk)
      {
          try
          {
              BulkAppraisalBO.SalaryBulkAppraisalHeader ObjAppInfomtn = new BulkAppraisalBO.SalaryBulkAppraisalHeader();
              string employee = BulkAppraisalDL.GetSalaryAppraisalByPk(empPk);
              if (employee != string.Empty)
              {
                  ObjAppInfomtn = (BulkAppraisalBO.SalaryBulkAppraisalHeader)CommonFunctions.DeserializeObject(employee, ObjAppInfomtn);
                  return ObjAppInfomtn;
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
      public static int DeleteSalaryAppraisal(int CurrPK, DateTime LastModifiedTime)
      {
          return BulkAppraisalDL.DeleteSalaryAppraisal(CurrPK, LastModifiedTime);
      }
    }
}
