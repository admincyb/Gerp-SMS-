using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DataAccess.Shipping
{
    public class ShippingUploadsDL
    {
        /// <summary>
        /// Save PO Invoice
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveShippingUploads(string xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ERP.Utilities.Constants.Shipping.ShippingUploads.SPSAL_SHIPPING_PLAN_DOC_DTL_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Save Bill of loading
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static int SaveBillofLoading(string xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, ERP.Utilities.Constants.Shipping.ShippingUploads.SPSAL_BILL_OF_LOADING_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
     
        
        /// <summary>
        /// Get details
        /// </summary>
        /// <param name="shippingPK"></param>
        /// <returns></returns>
        public static string GetBillofLoading(int shippingPK, int Type)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_SHIPPING_PLAN, shippingPK),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_SCD_TYPE, Type),
            };

            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, ERP.Utilities.Constants.Shipping.ShippingUploads.SPSAL_BILL_OF_LOADING_GET, colParameters);
            string strRetVal = "";
            for (int i = 0; i < dsList.Tables[0].Rows.Count; i++)
                strRetVal += dsList.Tables[0].Rows[i][0].ToString();
            return strRetVal;
        }

        /// <summary>
        /// Get Purchase Invoice List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetShippingUploads(int P_SCD_PK, int P_SCD_PLAN_HDR, short P_SCD_ACTIVE, int P_SCD_TYPE)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.ShippingUploads.P_SCD_PK, P_SCD_PK),
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.ShippingUploads.P_SCD_PLAN_HDR, P_SCD_PLAN_HDR),
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.ShippingUploads.P_SCD_ACTIVE, P_SCD_ACTIVE),  
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.ShippingUploads.P_SCD_TYPE, P_SCD_TYPE)
            };

            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, ERP.Utilities.Constants.Shipping.ShippingUploads.SPSAL_SHIPPING_PLAN_DOC_DTL_GET, colParameters);
            return dsList;
        }

        /// <summary>
        /// Get Purchase Invoice List 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetExportList(int P_SNH_PK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(ERP.Utilities.Constants.Shipping.ShippingUploads.P_SNH_PK, P_SNH_PK),
            };

            DataSet dsList = new DataSet();
            dsList = dbService.DataAdapter(CommandType.StoredProcedure, ERP.Utilities.Constants.Shipping.ShippingUploads.SPSAL_EXPORT_FORM_RPT, colParameters);
            return dsList;
        }
    }
}
