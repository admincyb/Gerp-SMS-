using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.Finance;
using ERP.Utilities;
namespace DataAccess.Finance
{
    public class COAFinGrpMapDL
    {
        public static DataTable GetCOAFinGrpList(int type, int pk, int template, int mapped,int isGroup,int sbuID)
        {
            DataTable dtData;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_COA_TYPE,type==0?(object)DBNull.Value:type),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RTC_PK,pk==0?(object)DBNull.Value:pk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_RTC_TEMPLATE,template==0?(object)DBNull.Value:template),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_MAPPED_FLAG,mapped),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_COA_IS_GROUP,isGroup),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.BIZUNIT,sbuID>0 ? sbuID : (object)DBNull.Value) 
            };
            dtData = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_COA_MST_GET_LIST, colParameters).Tables[0];
            return dtData;
        }
        public static int SaveCOAFinGrp(COAFinGrpMapBO COAFinGrpMapObj)
        {
            string xmlDoc = CommonFunctions.XmlSerialize(COAFinGrpMapObj);
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {        
                new DBService.Parameters(GTIService.Constants.Common.Parameters.P_XML,xmlDoc),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.FIN_REPORT_COA_MAP_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
