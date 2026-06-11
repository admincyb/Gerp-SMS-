using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using gErpProductionPlanning.ClassLibrary;
using ERP.Utilities;
using DataAccess;
using BusinessObject.Constants;


namespace DataAccess.ProductionDL
{
    public class CustomerOrderCreationDL
    {
        /// <summary>
        /// method to get Despatch Details
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="XML"></param>
        /// <returns></returns>
        public static DataSet GetCustomerOrder(int wid, string[] XML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(CustomerOrderCreationDA.P_WID, wid),
                new DBService.Parameters(CustomerOrderCreationDA.P_XML_SOH_LST, XML[0]==string.Empty? DBNull.Value.ToString():XML[0]),
                new DBService.Parameters(CustomerOrderCreationDA.P_XML_DOC_NUM, XML[1]==string.Empty? DBNull.Value.ToString():XML[1]),
                new DBService.Parameters(CustomerOrderCreationDA.P_XML_CUS_MST, XML[2]==string.Empty? DBNull.Value.ToString():XML[2]),
                new DBService.Parameters(CustomerOrderCreationDA.P_XML_PDG_MST, XML[3]==string.Empty? DBNull.Value.ToString():XML[3]),
                new DBService.Parameters(CustomerOrderCreationDA.P_XML_SIZ_MST, XML[4]==string.Empty? DBNull.Value.ToString():XML[4]),
                new DBService.Parameters(CustomerOrderCreationDA.P_XML_UOM_MST, XML[5]==string.Empty? DBNull.Value.ToString():XML[5]),
                new DBService.Parameters(CustomerOrderCreationDA.P_XML_SOD_LST, XML[6]==string.Empty? DBNull.Value.ToString():XML[6])
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, CustomerOrderCreationDA.SP_GetOrder, colParameters);
        }

        /// <summary>
        /// Save Despatch Details
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static DataSet SaveCustomerOrder(int wid, string[] XML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(CustomerOrderCreationDA.P_WID, wid),
                new DBService.Parameters(CustomerOrderCreationDA.P_XML_SO_DTL, XML[0]==string.Empty? DBNull.Value.ToString():XML[0]),
                new DBService.Parameters(CustomerOrderCreationDA.P_XML_SO_DEL, XML[1]==string.Empty? DBNull.Value.ToString():XML[1])
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, CustomerOrderCreationDA.SP_SaveOrder, colParameters);

        }
    }
}