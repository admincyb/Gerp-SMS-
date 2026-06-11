using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject;
using ERP.Utilities;

namespace DataAccess.HRMS.TaskTracker
{
    public class TaskHomeDL
    {
        /// <summary>
        /// Get Task Header List
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="userPk"></param>
        /// <param name="userPk"></param>
        /// <param name="mode"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetTaskHdrList(GridDataPropertyBinder grdDataProperties, User objUser, short mode, int? assignTo, int? isCompleted, int? assignedBy)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_PAGENO, grdDataProperties.CurrentPage),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_PAGE_SIZE,grdDataProperties.PageSize),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_USR_PK,objUser.PKUser),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_BIZUNIT,objUser.SBUID),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_MODE,mode), // 0:My Task, 1:Pending Task, 2:Completed Task       
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_TSK_ASSIGN_TO,assignTo),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_IS_COMPLETED,isCompleted??0),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_TSK_ASSIGN_BY,assignedBy??0)                
            };

            DataTable dtTaskHdr = new DataTable();
            dtTaskHdr = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.TaskTracker.Procedures.TASKHDRGETLIST, colParameters).Tables[0];
            return dtTaskHdr;
        }

        /// <summary>
        /// Get Single Task
        /// </summary>
        /// <param name="taskPk"></param>
        /// <param name="active"></param>
        /// <param name="taskParent"></param>
        /// <param name="bizUnit"></param>
        /// <param name="lstNode"></param>
        /// /// <param name="lstStatus"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetSingleTask(int taskPk, int? active, int? taskParent, int? bizUnit, int? lstNode, int? lstStatus,int? lstParent)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_TSK_PK, taskPk),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_BIZUNIT,bizUnit),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_TSK_PARENT,taskParent),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_LST_NODE,lstNode??0),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_LST_STATUS,lstStatus??0),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_LST_PARENT,lstParent??0)
            };

            DataSet dsTask = new DataSet();
            dsTask = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.TaskTracker.Procedures.GETTASK, colParameters);
            return dsTask;
        }

        /// <summary>
        /// Get Users
        /// </summary>
        /// <param name="taskPk"></param>
        /// <param name="active"></param>
        /// <param name="taskParent"></param>
        /// <param name="bizUnit"></param>
        /// <param name="lstNode"></param>
        /// /// <param name="lstStatus"></param>
        /// <returns>DataSet</returns>
        public static DataTable GetUsers(int userpk, string value, int active, int bizunit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.USERPK, userpk >0?userpk:(object)DBNull.Value),                
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.VALUE, value),   
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_ACTIVE, active),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_BIZUNIT,bizunit)                             
            };

            DataTable dtUsers = new DataTable();
            dtUsers = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.TaskTracker.Procedures.GETUSERS, colParameters).Tables[0];
            return dtUsers;
        }

        /// <summary>
        /// Save TaskInformation
        /// </summary>       
        /// <returns>RetValues as a Class</returns>
        public static RetValues SaveTaskInfo(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.PRETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.PRETNO,"",20,ParameterDirection.Output, DBService.ParameterType.NVarChar)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.TaskTracker.Procedures.TASKSAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            string number = ((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.PRETNO]).Value.ToString();
            return new RetValues() { RetVal=result, RetNumber=number };
        }

        /// <summary>
        /// Status Update
        /// </summary>       
        /// <returns>result</returns>
        public static int UpdateStatus(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.PRETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.TaskTracker.Procedures.SAVETASKSTATUS, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;            
        }

        /// <summary>
        /// Category Save
        /// </summary>       
        /// <returns>result</returns>
        public static int SaveTaskCategory(string strxml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.XML, strxml),  
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.PRETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.TaskTracker.Procedures.TASKCATEGORYSAVE, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;            
        }

        /// <summary>
        /// Get Categories
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns>Datatable</returns>
        public static DataTable GetCategoryList(int categoryPK, int bizunit, int? active, int? group=null)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_TCT_PK, categoryPK), 
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_BIZUNIT,bizunit),
                 new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_ACTIVE, active),
                  new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_TCT_GROUP, group)
            };

            DataTable dtCategory = new DataTable();
            dtCategory = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.TaskTracker.Procedures.GETCATEGORY, colParameters).Tables[0];
            return dtCategory;
        }

        /// <summary>
        /// Get Category Details
        /// </summary>
        /// <param name="categoryPK"></param>
        /// <param name="active"></param>
        /// <param name="bizUnit"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetCategoryDetails(int categoryPK)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {   
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_TCI_CATEGORY, categoryPK)                       
            };

            DataSet dsCategoryDetails = new DataSet();
            dsCategoryDetails = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.TaskTracker.Procedures.GETCATEGORYITEMS, colParameters);
            return dsCategoryDetails;
        }

        /// <summary>
        /// Get Task History List
        /// </summary>
        /// <param name="grdDataProperties"></param>
        /// <param name="taskPk"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetTaskHistoryList(GridDataPropertyBinder grdDataProperties, int? taskPk)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            {   
                //new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_PAGENO, grdDataProperties.CurrentPage),
                //new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_PAGE_SIZE,grdDataProperties.PageSize),
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_TSK_PK,taskPk)           
            };

            DataTable dtTaskHistory = new DataTable();
            dtTaskHistory = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.HRMS.TaskTracker.Procedures.SPTSK_TASK_STATUS_DTL_GET_LIST, colParameters).Tables[0];
            return dtTaskHistory;
        }

        /// <summary>
        /// Delete Task
        /// </summary>       
        /// <returns>result</returns>
        public static int DeleteTask(int taskPK, DateTime lastmodDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_TSK_PK, taskPK),  
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.P_LAST_MOD_DT, lastmodDate.ToString(GTIService.Constants.Common.CommonConstants.LastModDateFormat)),  
                new DBService.Parameters(GTIService.Constants.HRMS.TaskTracker.Parameters.PRETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.HRMS.TaskTracker.Procedures.DELETETASK, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}
