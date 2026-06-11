using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BusinessObject.HRMS.TaskTracker;
using ERP.Utilities;
using System.Data;
using System.Reflection;
using BusinessObject;

namespace BusinessLogic.HRMS.TaskTracker
{
    public class TaskHomeBL
    {
        /// <summary>
        /// Get Task Headers
        /// </summary>
        /// <param name="grdDataProperties"></param>
        /// <param name="userPk"></param>
        /// <param name="bizUnit"></param>
        /// <param name="mode"></param>
        /// <returns>List<TaskBOList></returns>
        public static DataTable GetTaskHdrList(GridDataPropertyBinder grdDataProperties, User objUser, short mode, int? assignTo, int? isCompleted, int? assignedBy)
        {
            DataTable dtHdrTaskList = DataAccess.HRMS.TaskTracker.TaskHomeDL.GetTaskHdrList(grdDataProperties, objUser, mode, assignTo, isCompleted, assignedBy);
           // List<TaskBO> lstTaskList = dtHdrTaskList.ToList<TaskBO>();// ToList<TaskBO>(dtHdrTaskList);
           //// List<TaskBO> lstTaskList = ToListTaskBo(dtHdrTaskList);// dtHdrTaskList.ToList<TaskBO>();// ToList<TaskBO>(dtHdrTaskList);
            return dtHdrTaskList;
            
        }

        /// <summary>
        /// Get Task Main
        /// </summary>
        /// <param name="grdDataProperties"></param>
        /// <param name="userPk"></param>
        /// <param name="bizUnit"></param>
        /// <param name="mode"></param>
        /// <returns>List<TaskBOList></returns>
        public static DataSet GetSingleTask(int taskPk, int? active, int? taskParent, int? bizUnit,  int? listNode, int? lstStatus)
        {
            DataSet dsTaskList = DataAccess.HRMS.TaskTracker.TaskHomeDL.GetSingleTask(taskPk, active,taskParent, bizUnit,  listNode, lstStatus, null);           
            return dsTaskList;
        }

        /// <summary>
        /// Get Task Main
        /// </summary>
        /// <param name="grdDataProperties"></param>
        /// <param name="userPk"></param>
        /// <param name="bizUnit"></param>
        /// <param name="mode"></param>
        /// <returns>List<TaskBOList></returns>
        public static DataSet GetTaskInfo(int taskPk, int active, int? taskParent, int bizUnit, int? listNode, int? lstStatus, int? lstParent)
        {
            DataSet dsTaskList = DataAccess.HRMS.TaskTracker.TaskHomeDL.GetSingleTask(taskPk, active,taskParent, bizUnit,  listNode, lstStatus, lstParent);            
            return dsTaskList;
        }

      
       
        /// <summary>
        /// Get Users
        /// </summary>       
        /// <returns>DataTable<TaskBOList></returns>
        public static DataTable GetUsers(int userpk, string value, int active, int bizunit)
        {
            return DataAccess.HRMS.TaskTracker.TaskHomeDL.GetUsers(userpk, value, active, bizunit);
        }

        /// <summary>
        /// Saving Task Info
        /// </summary>       
        /// <returns>DataTable<TaskBOList></returns>
        public static RetValues SaveTaskInfo(string strxml)
        {
            return DataAccess.HRMS.TaskTracker.TaskHomeDL.SaveTaskInfo(strxml);
        }

        /// <summary>
        /// Update Status
        /// </summary>       
        /// <returns>DataTable<TaskBOList></returns>
        public static int UpdateStatus(string strxml)
        {
            return DataAccess.HRMS.TaskTracker.TaskHomeDL.UpdateStatus(strxml);
        }

         /// <summary>
        /// Save Task category
        /// </summary>       
        /// <returns>DataTable<TaskBOList></returns>
        public static int SaveTaskCategory(string strxml)
        {
            return DataAccess.HRMS.TaskTracker.TaskHomeDL.SaveTaskCategory(strxml);
        }

        /// <summary>
        /// Get CategoryDetails
        /// </summary>       
        /// <returns>DataSet<TaskBOList></returns>
        public static DataSet GetCategoryDetails(int categoryPK)
        {
            return DataAccess.HRMS.TaskTracker.TaskHomeDL.GetCategoryDetails(categoryPK);
        }

        /// <summary>
        /// Get CategoryList
        /// </summary>       
        /// <returns>Dataset<TaskBOList></returns>
        public static DataTable GetCategoryList(int categoryPK, int bizunit, int? active, int? group=null)
        {
            return DataAccess.HRMS.TaskTracker.TaskHomeDL.GetCategoryList(categoryPK, bizunit, active, group);
        }

        private static List<TaskBO> ToListTaskBo(DataTable dt)
        {
            List<TaskBO> lstTaskBo = new List<TaskBO>();

            lstTaskBo = dt.AsEnumerable()
                  .Select(s => new TaskBO
                  {
                      REC_COUNT = s.Field<int?>("REC_COUNT"),
                      ROW_NO = s.Field<int?>("ROW_NO"),
                      TSK_ACTIVE = s.Field<int?>("TSK_ACTIVE"),
                      TSK_ASSIGN_TO = s.Field<int?>("TSK_ASSIGN_TO"),
                      TSK_ASSIGN_TO_TEXT = s.Field<string>("TSK_ASSIGN_TO_TEXT"),
                      TSK_BIZUNIT = s.Field<int>("TSK_BIZUNIT"),
                      TSK_CATEGORY = s.Field<int>("TSK_CATEGORY"),
                      TSK_CATEGORY_TEXT = s.Field<string>("TSK_CATEGORY_TEXT"),
                      //TSK_CMP_DATE = s.Field<DateTime?>("TSK_CMP_DATE"),
                      TSK_COMPANY = s.Field<int>("TSK_COMPANY"),
                      TSK_CRTD_BY = s.Field<int>("TSK_CRTD_BY"),
                      //TSK_CRTD_DT = s.Field<DateTime?>("TSK_CRTD_DT"),
                      //TSK_DATE = s.Field<DateTime?>("TSK_DATE"),
                      TSK_DEPT = s.Field<int>("TSK_DEPT"),
                      TSK_DESC = s.Field<string>("TSK_DESC"),
                      //TSK_EST_DATE = s.Field<DateTime?>("TSK_EST_DATE"),
                      //TSK_EXP_DATE = s.Field<DateTime?>("TSK_EXP_DATE"),
                      TSK_MOD_BY = s.Field<int>("TSK_MOD_BY"),
                      //TSK_MOD_DT = s.Field<DateTime?>("TSK_MOD_DT"),
                      TSK_NAME = s.Field<string>("TSK_NAME"),
                      TSK_NO = s.Field<string>("TSK_NO"),
                      TSK_PARENT = s.Field<int?>("TSK_PARENT"),
                      TSK_PK = s.Field<int?>("TSK_PK"),
                      TSK_STATUS = s.Field<int?>("TSK_STATUS"),
                      TSK_STATUS_TEXT = s.Field<string>("TSK_STATUS_TEXT"),
                      TSK_TRX_STATUS = s.Field<int?>("TSK_TRX_STATUS")
                  })
                  .ToList();
            return lstTaskBo;
        }

        /// <summary>
        /// Get Task History List
        /// </summary>
        /// <param name="grdDataProperties"></param>
        /// <param name="taskPk"></param>      
        /// <returns>DataTable</returns>
        public static DataTable GetTaskHistoryList(GridDataPropertyBinder grdDataProperties, int? taskPk)
        {
            DataTable dtHdrTaskList = DataAccess.HRMS.TaskTracker.TaskHomeDL.GetTaskHistoryList(grdDataProperties, taskPk);
            return dtHdrTaskList;
        }

         /// <summary>
        /// Delete Task
        /// </summary>       
        /// <returns>result</returns>
        public static int DeleteTask(int taskPK, DateTime lastmodDate)
        {
            return DataAccess.HRMS.TaskTracker.TaskHomeDL.DeleteTask(taskPK, lastmodDate);
        }
    }
}





//private static List<TSource> ToList<TSource>(DataTable dataTable) where TSource : new()
//{
//   //var lst1= dataTable.AsEnumerable().ToList();
//   //List<BusinessObject.HRMS.TaskTracker.TaskBO> lst2 = dataTable.AsEnumerable().Select(x=> x.
//   var dataList = new List<TSource>();

//    const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
//    var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
//                         select new 
//                         {
//                             Name = aProp.Name,
//                             Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType
//                         }).ToList();


//    var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
//                             select new { Name = aHeader.ColumnName, Type = aHeader.DataType }).ToList();
//    var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();

//    foreach (DataRow dataRow in dataTable.AsEnumerable().ToList())
//    {
//        var aTSource = new TSource();
//        foreach (var aField in commonFields)
//        {
//            PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name);
//            propertyInfos.SetValue(aTSource, dataRow[aField.Name], null);
//        }
//        dataList.Add(aTSource);
//    }
//    return dataList;
//}