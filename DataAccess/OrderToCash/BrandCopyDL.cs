using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.OrderToCash;

namespace DataAccess.OrderToCash
{
   public class BrandCopyDL
    {
       public static int? SaveBrandCopyDetails(string strxml, out string TrxNo)
       {
           DBService dbService = new DBService();
           DBService.Parameters[] colParameters = null;
           colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RET_REF_PK, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PRETNO, string.Empty, 200,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
           int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.SPCRM_CUST_ITEM_COPY_SAVE, colParameters);
           int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
           //int refPK = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RET_REF_PK]).Value);
           TrxNo = string.Empty;// Convert.ToString(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value);
           return result;
       }
    }
}
