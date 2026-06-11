using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace DataAccess.Finance.Administration.Masters
{
    public class BankGuaranteeDL
    {
        public static int? SaveBankGuarantee(string xmlDoc)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
               new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xmlDoc),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rowsAffected = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_BANK_GUARANTEE_MST_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        public static DataTable GetKVBankGuarantee(int pk, int active, int bizUnit,int partyPk, int bankPk, int pageNo, int pageSize,string sortBy,string sortDirection)
        {
            DataTable dtData;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BGM_PK,pk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_ACTIVE,active == 3 ? (object)DBNull.Value : active),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,bizUnit),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BGM_PARTY,partyPk>0 ?partyPk:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BGM_BANK,bankPk>0 ?bankPk:(object)DBNull.Value),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_NUM,pageNo == 0? 1:pageNo),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_PAGE_SIZE,pageSize),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,sortBy),
                //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC,sortDirection),
            };
            dtData = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_BANK_GUARANTEE_MST_GET_KV, colParameters).Tables[0];
            return dtData;
        }
        public static int DeleteBankGuarantee(int pk, DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.P_BGM_PK,pk),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.LAST_MOD_DT, lastModDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Finance.Procedures.SPFIN_BANK_GUARANTEE_MST_DELETE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
