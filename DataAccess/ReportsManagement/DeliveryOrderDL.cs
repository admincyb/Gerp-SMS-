using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace DataAccess.ReportsManagement
{
    public class DeliveryOrderDL
    {
        /// <summary>
        /// Get Delivery Order Details By DPH PK For Report
        /// </summary>
        /// <param name="dphPK"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetDeliveryOrderDtls(int dphPK,int SubType = 0)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_DPH_PK,  dphPK),
                new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_AST_VALUE,  SubType)
                                          
            };

            DataSet dsReportData = dbService.DataAdapter(CommandType.StoredProcedure, "SPSAL_DESPATCH_GET_RPT", colParameters);
            return dsReportData;
        }

        /// <summary>
        /// Get Delivery Order Details By DPH PK For Report
        /// </summary>
        /// <param name="dphPK"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetDeliveryOrderDetailsDOCNOREVISION(int dphPK, int SubType = 0,int reportpk=0)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[]
            {
                new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_DPH_PK,  dphPK),
                new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_AST_VALUE,  SubType),
                new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_AST_PK,  reportpk)

            };

            DataSet dsReportData = dbService.DataAdapter(CommandType.StoredProcedure, "SPSAL_DESPATCH_GET_RPT", colParameters);
            return dsReportData;
        }
        /// <summary>
        /// Get Packing List - 2 Details By DPH PK For Report
        /// </summary>
        /// <param name="dphPK"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetPackingListDtls(int dphPK)
        {
            DBService dbService = new DBService();

            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Reports.Parameters.P_DPH_PK,  dphPK)
                                          
            };

            DataSet dsReportData = dbService.DataAdapter(CommandType.StoredProcedure, "SPSAL_DESPATCH_PL2_GET_RPT", colParameters);
            return dsReportData;
        }
    }
}
