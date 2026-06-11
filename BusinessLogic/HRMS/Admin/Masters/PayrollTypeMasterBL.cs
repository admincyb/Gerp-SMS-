using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.HRMS.Admin.Masters;
using BusinessObject.HRMS.Admin.Masters;
using GTIService;

namespace BusinessLogic.HRMS.Admin.Masters
{
    public class PayrollTypeMasterBL
    {
        /// <summary>
        /// Method to Get Salary Template List for Listing Page
        /// </summary>
        /// <param name="currPk"></param>
        /// <param name="active"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="bizUnit"></param>
        /// <param name="ProcessMode"></param>
        /// <param name="PayrollTypeNameCode"></param>
        /// <param name="PayrollTypeName"></param>
        /// <returns></returns>
        public static DataSet GetPayrollTypeList(int? currPk, int active, int pageNo, int pageSize, int bizUnit, int ProcessMode, string PayrollTypeNameCode = null, string PayrollTypeName = null)
        {
            return PayrollTypeMasterDL.GetPayrollTypeList(currPk, active, pageNo, pageSize, bizUnit, ProcessMode, PayrollTypeNameCode, PayrollTypeName);
        }

        public static int UpdatePayrollTypeMasterStatus(int currPK, int status, int userPK, string lastModDate)
        {
            return PayrollTypeMasterDL.UpdatePayrollTypeMasterStatus(currPK, status, userPK, lastModDate);
        }

        public static DataTable GetSPayrollType(int? ltmPK, int active, int bizUnit)
        {
            return PayrollTypeMasterDL.GetSPayrollType(ltmPK, active, bizUnit);
        }

        /// <summary>
        /// Method to Save Salary Template
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns>int</returns>
        public static int SavePayrollTypeMaster(string xmlstr)
        {
            return PayrollTypeMasterDL.SavePayrollTypeMaster(xmlstr);
        }

        /// <summary>
        /// Method to Delete Salary Template
        /// </summary>
        /// <param name="pk"></param>
        /// /// <param name="lastModifiedDate"></param>
        /// <returns>int</returns>
        public static int DeletePayrollTypeMaster(int pk, string lastModifiedDate)
        {
            return PayrollTypeMasterDL.DeletePayrollTypeMaster(pk, lastModifiedDate);
        }

        public static PayrollTypeMasterBO.PayrollType GetUser(int pk)
        {
            PayrollTypeMasterBO.PayrollType objPayrol = new PayrollTypeMasterBO.PayrollType();
            string user =  PayrollTypeMasterDL.GetUser(pk);
            if (user != string.Empty)
            {
                objPayrol = (PayrollTypeMasterBO.PayrollType)CommonFunctions.DeserializeObject(user, objPayrol);
                return objPayrol;
            }
            else
            {
                return null;
            }
        }
    }
}
