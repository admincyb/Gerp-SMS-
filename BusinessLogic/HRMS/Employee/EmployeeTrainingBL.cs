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
   public class EmployeeTrainingBL
    {
        /// <summary>
        /// For get   lsit
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="p"></param>
        /// <returns></returns>
       public static DataTable GetEmployeeTrainingList(string fromDate, string ToDate, string topic,  int bizUnit, int status, int pageNo, int pageSize)
        {
            return EmployeeTrainingDL.GetEmployeeTrainingList(fromDate, ToDate, topic,  bizUnit, status, pageNo, pageSize);
        }

       public static EmployeeTrainingHeader_PopUp GetEmployeeTrainingDetailsPopUp(FilterParameters objFilterParam)
        {
            try
            {
                EmployeeTrainingHeader_PopUp addolidayMaster = new EmployeeTrainingHeader_PopUp();
                string dtl = EmployeeTrainingDL.GetEmployeeTrainingDetailsPopUp(objFilterParam);
                if (dtl != string.Empty)
                {
                    addolidayMaster = (EmployeeTrainingHeader_PopUp)CommonFunctions.DeserializeObject(dtl, addolidayMaster);
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

        public static int? SaveEmployeeTrainingDetails(string strxml, out string TrxNo)
        {
            try
            {
                return EmployeeTrainingDL.SaveEmployeeTrainingDetails(strxml, out TrxNo);
            }
            catch
            {
                throw;
            }
        }

        public static EmployeeTrainingHeader GetEmployeeTrainingByPK(int itemPK)
        {
            try
            {
                EmployeeTrainingHeader addolidayMaster = new EmployeeTrainingHeader();
                string dtl = EmployeeTrainingDL.GetEmployeeTrainingByPK(itemPK);
                if (dtl != string.Empty)
                {
                    addolidayMaster = (EmployeeTrainingHeader)CommonFunctions.DeserializeObject(dtl, addolidayMaster);
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


        public static int DeleteEmployeeTraining(int pk, string lastModifiedDate)
        {
            return EmployeeTrainingDL.DeleteEmployeeTraining(pk, lastModifiedDate);
        }

        public static DataSet GetEmployeeTrainingReport(int currPK)
        {
            return EmployeeTrainingDL.GetEmployeeTrainingReport(currPK);
        }

    }
}
