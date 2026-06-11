using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObject.CommonManagement;
using GTIService.Constants.Administration.Configurations;
using BusinessObject.Administration.Configurations;
using ERP.Utilities;

namespace DataAccess.Administration.Configurations
{
    public class NewsManagementDA
    {
        /// <summary>
        /// Method used to get News
        /// </summary>
        /// <param name="CurrPK"></param>
        /// <param name="status"></param>
        /// <param name="sbu"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetNews(int newsPK, DbActiveStatus status)
        {
            DataSet dsNews;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(NewsManagements.P_PK, newsPK<=0?0:newsPK),
                new DBService.Parameters(NewsManagements.P_ACTIVE, status == DbActiveStatus.ALL ? (object)DBNull.Value:Convert.ToInt32(status))
            };
            dsNews = dbService.DataAdapter(CommandType.StoredProcedure, NewsManagements.SP_GET, colParameters);
            if (dsNews.Tables[0] != null)
            {
                return dsNews.Tables[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method used to save/update News
        /// </summary>
        /// <param name="objUoM"></param>
        /// <param name="user"></param>
        /// <param name="sbu"></param>
        /// <returns></returns>
        public static int SaveNews(NewsManagementBO objNews, int user, int sbu)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(NewsManagements.P_PK ,objNews.NewsPK),
                new DBService.Parameters(NewsManagements.P_TITLE ,objNews.Title),
                new DBService.Parameters(NewsManagements.P_SHORTDESC ,objNews.ShortDesc),
                new DBService.Parameters(NewsManagements.P_PUBLISHEDDT ,objNews.PublishedDt),
                new DBService.Parameters(NewsManagements.P_DETAILS, objNews.Details),
                new DBService.Parameters(CommonConstants.CREATEDBY ,user),
                new DBService.Parameters(CommonConstants.LASTMODDATE ,objNews.LastModDate=="0"?(object)DBNull.Value:Convert.ToDateTime(objNews.LastModDate)),
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, NewsManagements.SP_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }
        /// <summary>
        /// Method Used To Delete News
        /// </summary>
        /// <param name="uomPK"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int DeleteNews(int newsPK, int user)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(NewsManagements.P_PK ,newsPK),      
                new DBService.Parameters(CommonConstants.CREATEDBY ,user),                             
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, NewsManagements.SP_DELETE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }
        /// <summary>
        /// Method Used To Activate/Deactivate the status of the News
        /// </summary>
        /// <param name="uomPK"></param>
        /// <param name="status"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int StatusUpdate(int newsPK, DbActiveStatus status, int user, string lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;

            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(NewsManagements.P_PK, newsPK),      
                new DBService.Parameters(CommonConstants.CREATEDBY, user),    
                new DBService.Parameters(CommonConstants.ACTIVESTATUS, Convert.ToInt32(status)),
                new DBService.Parameters(CommonConstants.LASTMODDATE, lastModDate == "0" ? (object)DBNull.Value :Convert.ToDateTime( lastModDate)),
                new DBService.Parameters(CommonConstants.RETURNVAL,  0, 20, ParameterDirection.Output, DBService.ParameterType.Number)
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, NewsManagements.SP_STATUSCHANGE, colParameters);

           
            try
            {
                return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
            }
            catch
            {
                return 1;
            }
            
        }
    }
}
