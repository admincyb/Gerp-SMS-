using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace DataAccess.StoreManagement
{
    public class StoreAdjustmentDL
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static string SaveStoreAuditAdjustmentDetails(string xmlstr)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {     
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.SPURCHASEREQUESTXML , xmlstr),
                new DBService.Parameters(GTIService.Constants.PurchaseRequest.Parameters.PRETVAL , 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, "", colParameters);
            return ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.PurchaseRequest.Parameters.PRETVAL]).Value.ToString();
        }
      
    }
}
