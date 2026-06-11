using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using WorkflowCore;

namespace BusinessLogic.AccountManagement
{
    public class InboxManagement
    {
        /// <summary>
        /// Methord Used to get the Task deatils for the login User
        /// </summary>
        /// <param name="gridPrams"></param>
        ///  <param name="userpk"></param>
        public static string GetInboxTasks(BusinessObject.GridPrams grdparms, int userpk, int procID)
        {
            WorkflowCore.CoreService coreservice = new CoreService();
            DataSet dsTaskDtls = null;// coreservice.GetInboxTaskAndCompleted(userpk, grdparms.FromDate, grdparms.ToDate, grdparms.Flag, grdparms.PageNumber, grdparms.PageSize, grdparms.SortBy, grdparms.SortDirection, procID);
            string jString = string.Empty;
            if (dsTaskDtls.Tables.Count > 1 && dsTaskDtls.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsTaskDtls);
            }
            return jString;
        }

        /// <summary>
        /// Methord Used to get the Intimation deatils for the login User
        /// </summary>
        /// <param name="gridPrams"></param>
        ///  <param name="userpk"></param>
        public static string GetInboxInitmation(BusinessObject.GridPrams grdParm, int userPk, int procID)
        {
            WorkflowCore.CoreService objCore = new CoreService();
            DataSet dsIntimations = null;//  objCore.GetIntimations(userPk, grdParm.FromDate, grdParm.ToDate, grdParm.PageNumber, grdParm.PageSize, grdParm.SortBy, grdParm.SortDirection);
            string jString = string.Empty;
            if (dsIntimations.Tables.Count > 1 && dsIntimations.Tables[1].Rows.Count > 0)
            {
                jString = Newtonsoft.Json.JsonConvert.SerializeObject(dsIntimations);
            }
            return jString;
           
        }

        /// <summary>
        /// Methord Used to Revoke a completed task
        /// </summary>
        /// <param name="refID"></param>
        ///  <param name="userpk"></param>
        ///  ///  <param name="actionID"></param>
        public static string RevokeCompletedTask(int userPk,int refID,int actionID)
        {
            WorkflowCore.CoreService objService = new CoreService();
            string jString = string.Empty;
            jString = objService.RevokeCmpltdTask(userPk, refID, actionID).ToString();
            return jString;
        }
    }
}
