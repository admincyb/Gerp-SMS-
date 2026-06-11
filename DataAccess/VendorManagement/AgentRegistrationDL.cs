using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using BusinessObject;
using GTIService.Constants.Vendor;

namespace DataAccess.VendorManagement
{
   public class AgentRegistrationDL
    {
        public static string SaveAgentDetails(string xmlAgentDetails)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Agent.Parameters.P_VEN_XML ,(object)xmlAgentDetails,DBService.ParameterType.XML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                 
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Agent.Procedures.SAVEAgent, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value).ToString();

        }       

        public static string GetAgentDetails(int AgentID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Agent.Parameters.PAGENTID , AgentID),                
                 
            };
            DataTable dtxml = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Agent.Procedures.GETAgentDTL, colParameters).Tables[0];
            string strXML = string.Empty;
            for (int i = 0; i < dtxml.Rows.Count; i++)
            {
                strXML += dtxml.Rows[i][0].ToString();
            }
            return strXML;

        }
      

        /// <summary>
        /// method for search based on the Criteria
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="fields"></param>
        /// <param name="sortBy"></param>
        /// <param name="sortDirection"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="status"></param>
        /// <param name="Searchtxt"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetAgentDetails(GridPrams grid, int sbuID, int procId, string PageUrl, int venRole)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            if (grid.SearchBy == "VEN_TYPE")
                grid.SearchValue = grid.SearchValue + "%";
            else
                grid.SearchValue = "%" + grid.SearchValue + "%";

            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHNAME , grid.SearchBy == "0"? "VEN_NAME" : grid.SearchBy ),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SEARCHVAL , grid.SearchValue == "" ? "%" : grid.SearchValue,DataAccess.DBService.ParameterType.NVarChar),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FIELDS , grid.Fields == "" ? "*" : grid.Fields),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PROCESSID , procId),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.USERPK , grid.UserPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.DEPTPK , grid.DeptPK == 0 ? 1 : grid.DeptPK),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO,  grid.PageNumber),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE,  grid.PageSize),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY,  grid.SortBy == null ? (object)DBNull.Value : grid.SortBy),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC, grid.SortDirection == null ? "Asc" : grid.SortDirection),
              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGEURL ,  PageUrl==string.Empty ?(object)DBNull.Value:PageUrl),

               new DBService.Parameters(GTIService.Constants.Agent.Parameters.P_VEN_ROLE,  venRole),

              new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT,sbuID)
            };

            DataSet dtProduct = new DataSet();
            dtProduct = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Agent.Procedures.GETAGENTDETILSLIST, colParameters);
            return dtProduct;


        }

      
        /// <summary>
        /// Methord to get the Delete Corresponding Agent with Provided agent ID
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns>DataTable</returns>
        public static int DeleteAgentDetails(int agentID,DateTime? LastModDate=null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.VENDRSPK , agentID),
                 new DBService.Parameters(Parameters.P_LAST_MOD_DT , LastModDate),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.DELETEVENDORS, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }

        /// <summary>
        /// Methord to get the Agent details corresponding to a Agent
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetAgentDtls(int agentID)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
              new DBService.Parameters( GTIService.Constants.Agent.Parameters.AgentPK ,  agentID),
              
            };
            DataTable dtvendor = new DataTable();
            dtvendor = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Agent.Procedures.GETAgentDTL, colParameters).Tables[0];
            return dtvendor;
        }             

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlVendorBank"></param>
        /// <returns>string VBD_PK (Bank Id)</returns>
        public static string SaveAgentBank(string xmlVendorBank)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Vendor.Parameters.P_XML ,(object)xmlVendorBank,DBService.ParameterType.XML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters.RETVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                 
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.SAVEVENDORBANK, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters.RETVAL]).Value).ToString();

        }     

        /// <summary>
        /// Delete Vendor Bank
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns>int</returns>
        public static int DeleteAgentBank(int bankId)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(Parameters.P_VBD_PK , bankId),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, Procedures.DELETEVENDORBANK, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
        }

        /// <summary>
        /// Get Vendor banks
        /// </summary>
        /// <param name="objUser"></param>
        /// <param name="AgentBankPk"></param>
        /// <param name="AgentPk"></param>
        /// <param name="Active"></param>
        /// <returns></returns>
        public static DataTable GetAgentBanks(User objUser, int AgentBankPk, int AgentPk, short Active)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                 new DBService.Parameters(GTIService.Constants.Agent.Parameters.P_VBD_PK,AgentBankPk),
                new DBService.Parameters(GTIService.Constants.Agent.Parameters.P_VENDOR,(AgentPk > 0)? AgentPk : (object)DBNull.Value),                
                new DBService.Parameters(GTIService.Constants.Agent.Parameters.P_ACTIVE, Active )              
            };

            DataTable dtVendorBank = new DataTable();
            dtVendorBank = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Vendor.Procedures.GETVENDORBANKDTL, colParameters).Tables[0];
            return dtVendorBank;
        }
    }
}
