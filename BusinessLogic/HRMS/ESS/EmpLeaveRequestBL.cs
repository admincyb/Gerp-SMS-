using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.HRMS.ESS;
using ERP.Utilities.HRMS;
using ERP.Utilities;
using System.Data;
using DataAccess.HRMS;

namespace BusinessLogic.HRMS.ESS
{
    public class EmpLeaveRequestBL
    {

        #region AutoComplete
        public static DataTable GetAutoCompleteEmployeeESS(string searchKey = "", int active = 1, int? empPk = null, string toDate = null)
        {
            return EmpLeaveRequestDL.GetAutoCompleteEmployeeESS(searchKey, active, empPk, toDate);
        }
        #endregion


        public static DataSet GetESSLeaveEntryList(FilterParameters objFilterParam)
        {
            return EmpLeaveRequestDL.GetESSLeaveEntryList(objFilterParam);
        }




        public static int SaveEmployeeLeaveESS(string xmlstr, out string RetNo, out string empResignDate)  //
        {
            return EmpLeaveRequestDL.SaveEmployeeLeaveESS(xmlstr, out RetNo, out  empResignDate);  //, out  empResignDate
        }

        public static EmpLeaveRequestBO.ESSLeaveRequestMaster GetLeaveDetailsByPk(FilterParameters objFilterParam)
        {
            try
            {
                EmpLeaveRequestBO.ESSLeaveRequestMaster objLeave = new EmpLeaveRequestBO.ESSLeaveRequestMaster();
                string result = EmpLeaveRequestDL.GetLeaveDetails(objFilterParam);
                if (!string.IsNullOrEmpty(result))
                {
                    objLeave = (EmpLeaveRequestBO.ESSLeaveRequestMaster)CommonFunctions.DeserializeObject(result, objLeave);
                    return objLeave;
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


        public static int DeleteESSLeave(int CurrPK, DateTime LastModifiedTime)
        {
            return EmpLeaveRequestDL.DeleteESSLeave(CurrPK, LastModifiedTime);
        }

        public static double GetNoOfLeaves(FilterParameters objFilterParameters)
        {
            return EmpLeaveRequestDL.GetNoOfLeaves(objFilterParameters);
        }

    }
}
