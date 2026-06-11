using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.HRMS.Payroll;
using System.Data;
using BusinessObject.HRMS.Payroll;
using ERP.Utilities;

namespace BusinessLogic.HRMS.Payroll
{
  public  class Income_Tax_01BL
    {

        /// <summary>
        /// Method to Save Income tax details
        /// </summary>
        /// <param name="stockTransferDtls"></param>
        /// <returns>int</returns>
      public static int SaveIncomeTax_01(string xmlstr)
        {
            return Income_Tax_01DL.SaveIncomeTax_01(xmlstr);
        }
      public static DataSet GetTaxAmountByPk(int? IT1_EPS_PK, int IT1_EMP_PK, int IT1_TYPE, int IT1_SECTION, int IT1_ITEM = 0)
      {
          return Income_Tax_01DL.GetTaxAmountByPk(IT1_EPS_PK, IT1_EMP_PK,IT1_TYPE, IT1_SECTION, IT1_ITEM);
      }

      public static DataSet GetTaxAmountByPk(int IT1_TYPE, int IT1_SECTION, int IT1_ITEM = 0)
      {
          return Income_Tax_01DL.GetTaxAmountByPk(0, 0, IT1_TYPE, IT1_SECTION, IT1_ITEM);
      }
      public static Income_Tax_01BO.Tax01_PayrollDetails GetEmployeePayrollDetailList(int epsPK)
      {
          try
          {
              Income_Tax_01BO.Tax01_PayrollDetails empPayObj = new Income_Tax_01BO.Tax01_PayrollDetails();
              string empPaydtl = Income_Tax_01DL.GetEmployeePayrollDetailList(epsPK);
              if (empPaydtl != string.Empty)
              {
                  empPayObj = (Income_Tax_01BO.Tax01_PayrollDetails)CommonFunctions.DeserializeObject(empPaydtl, empPayObj);
                  return empPayObj;
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
    }
}
