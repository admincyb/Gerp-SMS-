using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.Jouralize
{
    public class PettyCashRefillBL
    {
        public static DataSet GetPettyCashRefillDate(int account)
        {
            return DataAccess.Journalize.PettyCashRefillDA.GetPettyCashRefillDate(account);
        }
        public static int? SavePettyCashRefill(int account, DateTime refillDate, User objUser, int? pk, DateTime? lastModDate = null)
        {
            return DataAccess.Journalize.PettyCashRefillDA.SavePettyCashRefill(account, refillDate, objUser, pk,lastModDate);
        }

        public static DataTable GetPettyCashList(DateTime? FromDate, DateTime? ToDate, int? CvdAccount, int bizUnit, int PageNo = 0, int pageSize = 20)
        {
            return DataAccess.Journalize.PettyCashRefillDA.GetPettyCashList(FromDate, ToDate, CvdAccount, bizUnit, PageNo, pageSize);
        }
    }
}
