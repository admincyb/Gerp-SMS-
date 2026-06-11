using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.PurchaseOrderManagement;
using System.Web;
using System.Web.Script.Serialization;

namespace BusinessLogic.Sales
{
    public class ContainerInspectionBL
    {
        /// <summary>
        /// Get Sales Invoice OrderDetails For Report
        /// </summary>
        /// <param name="saleOrderID"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetContainerInspectionDtls(int ContainerInspectionID, string SalesInvRptSp)
        {
            return DataAccess.ProductionDL.SaleOrderDL.GetSalesInvoiceDtls(ContainerInspectionID, SalesInvRptSp);
        }
    }
}
