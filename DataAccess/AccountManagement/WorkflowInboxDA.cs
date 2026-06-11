using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using BusinessObject.AccountManagement;

namespace DataAccess.AccountManagement
{
    public class WorkflowInboxDA
    {
        public static DataTable GetWorkflowDetails(WorkflowInboxBO objWorkflowInbox, User objUser)
        {
            DataTable dtWorkflow;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters(GTIService.Constants.Accounts.WorkflowInbox.Parameters.PRefPK,objWorkflowInbox.RefID),
                new DBService.Parameters(GTIService.Constants.Accounts.WorkflowInbox.Parameters.PUserPK,objUser.PKUser)
            };           
            dtWorkflow = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Accounts.WorkflowInbox.Procedures.SP_WORKFLOW_GET, colParameters).Tables[0]; 
            return dtWorkflow;
        }
        /// <summary>
        /// Get Inbox Summary
        /// </summary>
        /// <param name="RefPk"></param>
        /// <returns></returns>
        public static DataTable GetInboxSummary(int RefPk)
        {
            DataTable dtResult;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                 new DBService.Parameters(GTIService.Constants.Accounts.WorkflowInbox.Parameters.pReference , RefPk)
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Accounts.WorkflowInbox.Procedures.SpWkfTransactionSummaryGet, colParameters).Tables[0];
            return dtResult;
        }
        /// <summary>
        /// Get Inbox Summary
        /// </summary>
        /// <param name="RefPk"></param>
        /// <returns></returns>
        public static DataTable GetProcessFill(string searchkey)
        {
            DataTable dtResult;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {  
                 new DBService.Parameters(GTIService.Constants.Accounts.WorkflowInbox.Parameters.pReference , searchkey)
            };
            dtResult = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Accounts.WorkflowInbox.Procedures.SpWkfTransactionSummaryGet, colParameters).Tables[0];
            return dtResult;
        }

    }
}
