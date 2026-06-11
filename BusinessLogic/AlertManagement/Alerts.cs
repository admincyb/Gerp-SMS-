using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.AlertManagement;

namespace BusinessLogic.AlertManagement
{
    public class Alerts
    {
        public static System.Data.DataSet GetAlertDetails(int currPK, byte active, DateTime? date, string TypeCode, int? TypePK, int bizUnit, int userPK,int? isSystem=0)
        {
            return DataAccess.AlertManagement.AlertsDA.GetAlertDetails(currPK, active, date, TypeCode, TypePK, bizUnit, userPK, isSystem);
        }

        public static int SaveAlertDetails(AlertBO alertBoObj)
        {
            return DataAccess.AlertManagement.AlertsDA.SaveAlertDetails(alertBoObj);
        }

        public static int? DeleteAlertDetails(int CurrPK, DateTime LastModifiedTime)
        {
            return DataAccess.AlertManagement.AlertsDA.DeleteAlertDetails(CurrPK, LastModifiedTime);
        }

        public static DataSet GetAlertInbox(int userPK,int currentPage,int pageSize,string fromdate,string toDate,string type,int? typePK, int bizUnit)
        {
            return DataAccess.AlertManagement.AlertsDA.GetAlertInbox(userPK,currentPage, pageSize, fromdate, toDate, type, typePK, bizUnit);
        }
    }
}
