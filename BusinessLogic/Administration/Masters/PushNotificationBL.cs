using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Administration.Masters;

namespace BusinessLogic.Administration.Masters
{
   public class PushNotificationBL
    {
        public static DataSet GetSendSMSList(ERP.Utilities.HRMS.FilterParameters ObjFilterParameters)
        {
            return PushNotificationDL.GetSendSMSList(ObjFilterParameters);
        }

        public static DataTable GetEmployeeDetails(ERP.Utilities.HRMS.FilterParameters objFilterParam)
        {
            return PushNotificationDL.GetEmployeeDetails(objFilterParam);
        }

        public static int? SaveSendSMSDetails(string strxml)
        {
            try
            {
                return PushNotificationDL.SaveSendSMSDetails(strxml);
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
                return PushNotificationDL.DeleteSMSDetails(strxml);
            }
            catch
            {
                throw;
            }
        }

    }
}
