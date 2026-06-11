using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.HRMS.Payroll;
using System.Data;
using ERP.Utilities.HRMS;
using BusinessObject.HRMS.Payroll;
using ERP.Utilities;

namespace BusinessLogic.HRMS.Payroll
{
   public class EmployeeAppraisalDetailsBL
    {
       public static DataSet GetEmployeeAppraisalDetails(FilterParameters objFilterParam)
       {
           return EmployeeAppraisalDetailsDL.GetEmployeeAppraisalDetails(objFilterParam);
       }

       //Fill employee in (XML)
       public static EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader GetEmployeeAppraisalByPk(int empPk, int CurrEmployeePK)
       {
           try
           {
               EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader ObjEmployeeBasicInfomtn = new EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader();
               string employee = EmployeeAppraisalDetailsDL.GetEmployeeAppraisalByPk(empPk, CurrEmployeePK);
               if (employee != string.Empty)
               {
                   ObjEmployeeBasicInfomtn = (EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader)CommonFunctions.DeserializeObject(employee, ObjEmployeeBasicInfomtn);
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


       public static int? SaveEmployeeAppraisalDetails(string strxml, out string TrxNo)
       {
           try
           {
               return EmployeeAppraisalDetailsDL.SaveEmployeeAppraisalDetails(strxml, out TrxNo);
           }
           catch
           {
               throw;
           }
       }

       public static int DeleteEmployeeAppraisal(int CurrPK, DateTime LastModifiedTime)
       {
           return EmployeeAppraisalDetailsDL.DeleteEmployeeAppraisal(CurrPK, LastModifiedTime);
       }

    }
}
