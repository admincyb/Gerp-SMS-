using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace BusinessLogic.ReportsManagement
{
    public class PurchaseRequestReport
    {
        /// <summary>
        /// Procedure to get Purchase Request Report
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="status"></param>
        /// <param name="requestNumber"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetPurchaseRequests(string fromDate, string toDate, int status, string requestNumber, int sbu)
        {
            return DataAccess.ReportsManagement.PurchaseRequestReportDL.GetPurchaseRequests(fromDate, toDate, status, requestNumber, sbu);
        }
        /// <summary>
        /// Procedure to get Purchase Request Statuses
        /// </summary>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetPRStatus(int sbu)
        {
            return DataAccess.ReportsManagement.PurchaseRequestReportDL.GetPRStatus(sbu);
        }
        /// <summary>
        /// Procedure to get Purchase Request Items
        /// </summary>
        /// <param name="requestNumber"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetPRItems(int requestNumber, int sbu)
        {
            return DataAccess.ReportsManagement.PurchaseRequestReportDL.GetPRItems(requestNumber, sbu);
        }
        /// <summary>
        /// Procedure to get Purchase Request Vendour
        /// </summary>
        /// <param name="requestNumber"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static DataTable GetPRVendour(int requestNumber, int sbu)
        {
            return DataAccess.ReportsManagement.PurchaseRequestReportDL.GetPRVendour(requestNumber, sbu);
        }
    }
}
