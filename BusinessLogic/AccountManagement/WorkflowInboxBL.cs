using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.AccountManagement;
using BusinessObject;
using DataAccess.AccountManagement;
using System.Data;

namespace BusinessLogic.AccountManagement
{
    public class WorkflowInboxBL
    {
        /// <summary>
        /// To Save Workflow Directly from Inbox
        /// </summary>
        /// <param name="objWorkflowInbox"></param>
        /// <param name="objUser"></param>
        /// <returns></returns>
        public static DataTable GetWorkflowDetails(WorkflowInboxBO objWorkflowInbox, User objUser)
        {
            return WorkflowInboxDA.GetWorkflowDetails(objWorkflowInbox, objUser);
        }
           /// <summary>
        /// Get Inbox Summary
        /// </summary>
        /// <param name="RefPk"></param>
        /// <returns></returns>
        public static DataTable GetInboxSummary(int RefPk)
        {
            return WorkflowInboxDA.GetInboxSummary(RefPk);
        }
        ///
        /// Get WorkFlow Process
        /// 
        public static DataTable GetProcessFill(string searchkey)
        {
            return WorkflowInboxDA.GetProcessFill(searchkey);
        }


    }
}
