using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Administration.Masters;

namespace BusinessLogic.Administration.Masters
{
    public class SendSMSBL
    {

        public static DataSet GetSendSMSList(ERP.Utilities.HRMS.FilterParameters ObjFilterParameters)
        {
            return SendSMSDL.GetSendSMSList(ObjFilterParameters);
        }

        public static DataTable GetEmployeeDetails(ERP.Utilities.HRMS.FilterParameters objFilterParam)
        {
            return SendSMSDL.GetEmployeeDetails(objFilterParam);
        }

        public static int? SaveSendSMSDetails(string strxml)
        {
            try
            {
                return SendSMSDL.SaveSendSMSDetails(strxml);
            }
            catch
            {
                throw;
            }
        }

        public static int? DeleteSMSDetails(string strxml)
        {
            try
            {
                return SendSMSDL.DeleteSMSDetails(strxml);
            }
            catch
            {
                throw;
            }
        }

 

    }
}
