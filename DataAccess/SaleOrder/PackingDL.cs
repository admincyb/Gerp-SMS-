using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Utilities;
using DataAccess;
using BusinessObject.Constants;
using BusinessObject;


namespace DataAccess.ProductionDL
{
    public class PackingTransactionDL
    {
        /// <summary>
        /// method to get Despatch Details
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="XML"></param>
        /// <returns></returns>
        public static DataSet GetPackingDtl(int wid, string[] XML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(PackingTransactionDA.P_WID, wid),
                new DBService.Parameters(PackingTransactionDA.P_XML_PCK_LST, XML[0]==string.Empty? DBNull.Value.ToString():XML[0]),
                new DBService.Parameters(PackingTransactionDA.P_XML_CUS_MST, XML[1]==string.Empty? DBNull.Value.ToString():XML[1]),
                new DBService.Parameters(PackingTransactionDA.P_XML_DOC_NUM, XML[2]==string.Empty? DBNull.Value.ToString():XML[2]),
                new DBService.Parameters(PackingTransactionDA.P_XML_SFT_MST, XML[3]==string.Empty? DBNull.Value.ToString():XML[3]),
                new DBService.Parameters(PackingTransactionDA.P_XML_ORD_HDR, XML[4]==string.Empty? DBNull.Value.ToString():XML[4]),
                new DBService.Parameters(PackingTransactionDA.P_XML_ORD_LST, XML[5]==string.Empty? DBNull.Value.ToString():XML[5]),
                new DBService.Parameters(PackingTransactionDA.P_XML_PCK_DTL, XML[6]==string.Empty? DBNull.Value.ToString():XML[6])

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, PackingTransactionDA.SP_GetPackingDtl, colParameters);
            
        }

        /// <summary>
        /// Save Despatch Details
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static DataSet SavePackingDtl(int wid, string[] XML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(PackingTransactionDA.P_WID, wid),
                new DBService.Parameters(PackingTransactionDA.P_XML_PCK_DTL, XML[0]==string.Empty? DBNull.Value.ToString():XML[0]),
                 new DBService.Parameters(PackingTransactionDA.P_XML_PCK_DEL, XML[1]==string.Empty? DBNull.Value.ToString():XML[1]),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, PackingTransactionDA.SP_SavePackingDtl, colParameters);
            throw new NotImplementedException();
        }
    }
}