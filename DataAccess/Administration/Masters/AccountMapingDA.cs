using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.Administration.Masters;
using BusinessObject.CommonManagement;
using GTIService.Constants.Common;
using GTIService.Constants.Administration.Masters;
namespace DataAccess.Administration.Masters
{
    public class AccountMapingDA
    {
        /// <summary>
        /// Get Mapping Deatils
        /// </summary>
        /// <returns></returns>
        public static DataTable GetMappingType(int PK,int active)
        { 
            DataTable MappingType = new DataTable();
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Administration .Masters.AccountMaping.Parameters.P_CKH_PK , PK == 0 ? (object)DBNull.Value : PK),     
                new DBService.Parameters(GTIService.Constants.Administration .Masters.AccountMaping.Parameters.P_ACTIVE  , active == 0 ? (object)DBNull.Value : active),               
            };
            MappingType = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration .Masters.AccountMaping.Procedures .SP_GETMAPPINGTYPE, colParameters).Tables[0];
            return MappingType;

        }


        /// <summary>
        /// To retrieve Mapping details
        /// </summary>
        /// <param name="CompanyPK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetMappingDetails(int Active,int MappingPK)
        {
            DataTable dtMapping;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Administration .Masters.AccountMaping.Parameters.P_ACTIVE ,Active),
                new DBService.Parameters(GTIService.Constants.Administration .Masters.AccountMaping.Parameters.P_CKH_PK ,MappingPK),
            };
            dtMapping = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.AccountMaping.Procedures.SP_GETMAPPINGTYPE_LIST , colParameters).Tables[0];
            return dtMapping;
        } 

        /// <summary>
        /// To retrieve Mapping details
        /// </summary>
        /// <param name="CompanyPK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetMappingTypeData(int PK, int Active, int SubType, int Group)
        {
            DataTable dtMapping;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                
                new DBService.Parameters(GTIService.Constants.Administration .Masters.AccountMaping.Parameters.P_COA_PK , PK == 0 ? (object)DBNull.Value : PK),    
                //new DBService.Parameters(GTIService.Constants.Administration .Masters.AccountMaping.Parameters.P_COA_PK ,PK),
                new DBService.Parameters(GTIService.Constants.Administration .Masters.AccountMaping.Parameters.P_ACTIVE ,Active),
                new DBService.Parameters(GTIService.Constants.Administration .Masters.AccountMaping.Parameters.P_COA_SUB_TYPE ,SubType),
                new DBService.Parameters(GTIService.Constants.Administration .Masters.AccountMaping.Parameters.P_COA_IS_GROUP ,Group),
            };
            dtMapping = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.AccountMaping.Procedures.SPFIN_COA_MST_GET_KV, colParameters).Tables[0];
            return dtMapping;
        }

        /// <summary>
        /// Save Account Mapping Header
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
  

        public static int AccountMappingHeader(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.AccountMaping.Procedures.SPFIN_COA_LINK_HDR_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        /// <summary>
        /// To retrieve Mapping details
        /// </summary>
        /// <param name="CompanyPK"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetCostCenter(int PK)
        {
            DataTable dtResult;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Administration .Masters.AccountMaping.Parameters.P_COA_PK , PK == 0 ? (object)DBNull.Value : PK)
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.AccountMaping.Procedures.SPFIN_COA_COST_CENTER_GET, colParameters).Tables[0];
            return dtResult;
        }

        /// <summary>
        /// Save Cost center Allocation
        /// </summary>
        /// <param name="strxml"></param>
        /// <returns></returns>
        public static int SaveCostCenter(string xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.XML, xml),  
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.AccountMaping.Procedures.SPFIN_COA_COST_CENTER_MPG_SAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }


         
    }
}
