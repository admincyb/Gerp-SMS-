using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Utilities;
using DataAccess;
using BusinessObject.Constants;
using BusinessObject;
using ERP.Utilities.Constants.DA;
using BusinessObject.CommonManagement;

namespace DataAccess.ProductionDL
{
    public class CustomerOrderTrackerDL
    {
        /// <summary>
        /// method to get Customers & Orders
        /// </summary>
        /// <param name="pk"></param>
        public static DataSet GetCustomers_Orders(int wid, string[] XML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                
                new DBService.Parameters(CustomerOrderTrackerDA.P_WID, wid),
                new DBService.Parameters(CustomerOrderTrackerDA.P_XML_CUS_MST, XML[0]==string.Empty? DBNull.Value.ToString():XML[0]),
                new DBService.Parameters(CustomerOrderTrackerDA.P_XML_ORD_HDR, XML[1]==string.Empty? DBNull.Value.ToString():XML[1]),              
                new DBService.Parameters(CustomerOrderTrackerDA.P_XML_ORD_LST, XML[2]==string.Empty? DBNull.Value.ToString():XML[2]),
                new DBService.Parameters(CustomerOrderTrackerDA.P_XML_ORD_STT, XML[3]==string.Empty? DBNull.Value.ToString():XML[3])                
               
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, CustomerOrderTrackerDA.SP_GetCustomers_Orders, colParameters);

        }

        /// <summary>
        /// Bin Card Autocomplete Search
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static List<AutoCompleteBO> GetAutoCompleteList(string searchKey, int bizUnit)
        {
            //List For Storing All values
            List<AutoCompleteBO> lstValue = new List<AutoCompleteBO>();
            //Filling Parameter table with data from DA Layer
            //DataTable dtValue = BincardCreateDA.GetAutoCompleteList(searchKey, bizUnit);

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                
                new DBService.Parameters(CustomerOrderTrackerDA.CustomerName, searchKey),
                new DBService.Parameters(CustomerOrderTrackerDA.BizUnit,bizUnit),
                new DBService.Parameters(CustomerOrderTrackerDA.Active, ActiveStatus.ACTIVE )           
               
            };
            DataTable dtValue = dbService.DataAdapter(CommandType.StoredProcedure, CustomerOrderTrackerDA.SP_GetCustomers_Auto, colParameters).Tables[0];



            foreach (DataRow drValue in dtValue.Rows)
            {
                AutoCompleteBO value = new AutoCompleteBO()
                {
                    Key = (drValue[CustomerOrderTrackerDA.CustomerPK] == DBNull.Value) ? -1 : Convert.ToInt32(drValue[CustomerOrderTrackerDA.CustomerPK]),
                    Name = (drValue[CustomerOrderTrackerDA.CustomerCode] == DBNull.Value) ? String.Empty : Convert.ToString(drValue[CustomerOrderTrackerDA.CustomerCode])
                };
                //Add Each Category To category list
                lstValue.Add(value);
            }

            return lstValue;
        }

    }
}