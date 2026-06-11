using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.CommonManagement;
using DataAccess.Administration.Configurations;
using BusinessObject.Administration.Configurations;

namespace BusinessLogic.Administration.Configurations
{
    public class NewsManagementBL
    {
        /// <summary>
        /// Method used to get News
        /// </summary>
        /// <param name="newsPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetNews(int newsPK, DbActiveStatus status)
        {
            return NewsManagementDA.GetNews(newsPK, status);
        }

        /// <summary>
        /// Method used to save/update News
        /// </summary>
        /// <param name="objNews"></param>
        /// <param name="user"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static int SaveNews(NewsManagementBO objNews, int user, int sbu)
        {
            return NewsManagementDA.SaveNews(objNews, user, sbu);
        }

        /// <summary>
        /// Method Used To Delete News
        /// </summary>
        /// <param name="newsPK"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int DeleteNews(int newsPK, int user)
        {
            return NewsManagementDA.DeleteNews(newsPK, user);
        }

        /// <summary>
        /// Method Used To Activate/Deactivate the status of the News
        /// </summary>
        /// <param name="newsPK"></param>
        /// <param name="status"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int StatusUpdate(int newsPK, DbActiveStatus status, int user, string lastModDate)
        {
            return NewsManagementDA.StatusUpdate(newsPK, status, user, lastModDate);
        }
    }
}
