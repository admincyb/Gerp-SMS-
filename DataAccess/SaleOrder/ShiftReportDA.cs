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
    public class ShiftReportDL
    {
        /// <summary>
        /// method to get saleorder details
        /// </summary>
        /// <param name="pk"></param>
        public static DataSet GetShiftReport(int wid, string[] XML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                
                new DBService.Parameters(ShiftReport.P_WID, wid),
                //
                new DBService.Parameters(ShiftReport.P_XML_DOC_NUM, XML[0]==string.Empty? DBNull.Value.ToString():XML[0]),
                new DBService.Parameters(ShiftReport.P_XML_SFT_MST, XML[1]==string.Empty? DBNull.Value.ToString():XML[1]),
                new DBService.Parameters(ShiftReport.P_XML_PRO_MST, XML[2]==string.Empty? DBNull.Value.ToString():XML[2]),
                new DBService.Parameters(ShiftReport.P_XML_LNE_MST, XML[3]==string.Empty? DBNull.Value.ToString():XML[3]),
                new DBService.Parameters(ShiftReport.P_XML_SFT_LST, XML[4]==string.Empty? DBNull.Value.ToString():XML[4]),
                new DBService.Parameters(ShiftReport.P_XML_SFT_DTL, XML[5]==string.Empty? DBNull.Value.ToString():XML[5]),
               
               
            };
            return dbService.DataAdapter(CommandType.StoredProcedure,ShiftReport.SP_GetShiftReport, colParameters);

        }

        /// <summary>
        /// Save Sale Order
        /// </summary>
        /// <param name="strxml"></param>
        public static DataSet SaveShiftReport(int wid, string[] strXml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(ShiftReport.P_WID, wid),
                new DBService.Parameters(ShiftReport.P_XML_SFT_RPT, strXml[0]),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, ShiftReport.SP_SaveShiftReport, colParameters);

        }
    }
}