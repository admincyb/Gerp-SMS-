using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.PurchaseOrderManagement;
using System.Web;
using System.Web.Script.Serialization;
using BusinessObject.Sales;
using GTIService;
using BusinessObject;
using DataAccess.ProductionDL;
using BusinessObject.CommonManagement;

namespace BusinessLogic.Sales
{
    public class OrderTrackerBL
    {
        public static DataSet GetCustomers_Orders(int wid, string[] XML)
        {
            return CustomerOrderTrackerDL.GetCustomers_Orders(wid, XML);
        }
        public static List<AutoCompleteBO> GetAutoCompleteList(string searchKey, int bizUnit)
        {
            return CustomerOrderTrackerDL.GetAutoCompleteList(searchKey, bizUnit);
        }
    }
}
