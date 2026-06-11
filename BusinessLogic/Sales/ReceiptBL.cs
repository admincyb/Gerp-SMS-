using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;

namespace BusinessLogic.Sales
{
    public class ReceiptBL
    {
        /// <summary>
        /// Method to save sales receipt
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="ReceiptNo"></param>
        /// <returns></returns>
        public static long? SaveReceipt(string xmlDoc, out string ReceiptNo)
        {
            return DataAccess.Sales.ReceiptDL.SaveReceipt(xmlDoc, out ReceiptNo);
        }

        /// <summary>
        /// Method to delete sales receipt
        /// </summary>
        /// <param name="ReceiptPk"></param>
        /// <param name="UserPk"></param>
        /// <param name="LastModifiedTime"></param>
        /// <param name="appType"></param>
        /// <returns></returns>
        public static long? DeleteReceipt(long ReceiptPk, int UserPk, DateTime LastModifiedTime, string appType)
        {
            return DataAccess.Sales.ReceiptDL.DeleteReceipt(ReceiptPk, UserPk, LastModifiedTime, appType);
        }

        /// <summary>
        /// Method to get receipt list
        /// </summary>
        /// <param name="gridPrams"></param>
        /// <param name="currentUser"></param>
        /// <param name="CustomerPk"></param>
        /// <param name="ReceiptPk"></param>
        /// <param name="invNo"></param>
        /// <param name="pageUrl"></param>
        /// <param name="status"></param>
        /// <param name="PDCStatus"></param>
        /// <param name="cmpPk"></param>
        /// <returns></returns>
        public static DataSet GetReceiptList(GridPrams gridPrams, User currentUser, int CustomerPk, long ReceiptPk, string invNo, string pageUrl, int status = 0, int PDCStatus = 0, int cmpPk = 0)
        {
            return DataAccess.Sales.ReceiptDL.GetReceiptList(gridPrams, currentUser, CustomerPk, ReceiptPk, invNo, pageUrl, status, PDCStatus, cmpPk);
        }
    }
}
