using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.HRMS.TaskTracker
{
    public class Procedures
    {
        public const string TASKHDRGETLIST = "SPTSK_TASK_HDR_GET_LIST";
        public const string TASKSAVE = "SPTSK_TASK_HDR_SAVE";
        public const string GETTASK = "SPTSK_TASK_HDR_GET_KV";
        public const string GETUSERS = "SPADM_USER_MST_GET_KV";
        public const string SAVETASKSTATUS = "SPTSK_TASK_STATUS_DTL_SAVE";
        public const string SPTSK_TASK_STATUS_DTL_GET_LIST = "SPTSK_TASK_STATUS_DTL_GET_LIST";
        public const string DELETETASK = "SPTSK_TASK_HDR_DELETE";

        //For TaskCategory
        public const string TASKCATEGORYSAVE = "SPTSK_TASK_CATEGORY_MST_SAVE";
        public const string GETCATEGORY = "SPTSK_TASK_CATEGORY_MST_GET_KV";
        public const string GETCATEGORYITEMS = "SPTSK_TASK_CATEGORY_ITEM_DTL_GET";
    }
}
