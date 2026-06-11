using System;
using System.Data;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using GTIService.Constants.Administration.Configurations;
using GTIService.Constants.Common;


namespace DataAccess.Administration.Configurations
{
    public class PageActionDA
    {
        /// <summary>
        /// Methord used to get Page action Details for Listing
        /// </summary>
        /// <param name="pageActionPK"></param>
        /// <param name="P_PAGENO"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataSet GetPageActions(int bizUnit)
        {
            //public static DataSet GetPageActions(int pageActionPK,int P_PAGENO,int bizUnit)
            DBService dbService = new DBService();
            //DBService.Parameters[] colParameters = null;
            //colParameters = new DBService.Parameters[] 
            //{ 
            //     new DBService.Parameters(PageAction.P_PK, pageActionPK==0?(object)DBNull.Value:pageActionPK),  
            //     new DBService.Parameters( CommonConstants.P_PAGENO,P_PAGENO),
            //     new DBService.Parameters( CommonConstants.P_PAGESIZE,int.Parse( CommonConstants.PAGESIZE)),
            //     new DBService.Parameters( CommonConstants.BIZUNIT,bizUnit)

            //};
            //return dbService.DataAdapter(CommandType.StoredProcedure, PageAction.SP_GETPAGEACTIONS, colParameters);    
            return dbService.DataAdapter(CommandType.StoredProcedure, PageActions.SP_GETPAGEACTIONSKV);

        }

        /// <summary>
        /// Methord used to get Page action Details for Listing
        /// </summary>
        /// <param name="pageActionPK"></param>
        /// <param name="P_PAGENO"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataSet GetPageActions(int pageActionPK, DbActiveStatus status, int bizUnit)
        {

            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(PageActions.P_PK, pageActionPK==0?(object)DBNull.Value:pageActionPK),  
                new DBService.Parameters(CommonConstants.P_ACTIVE,status==DbActiveStatus.ALL?(object)DBNull.Value: Convert.ToInt32(status)),                

            };
            return dbService.DataAdapter(CommandType.StoredProcedure, PageActions.SP_GETPAGEACTIONSKV, colParameters);

        }

        /// <summary>
        /// Methord used to get page Details 
        /// </summary>
        /// <param name="pagePK"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataTable GetPages(int pagePK, DbActiveStatus status)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters( CommonConstants.P_ACTIVE,status == DbActiveStatus.ALL ? (object)DBNull.Value:Convert.ToInt32(Enum.Parse(typeof(DbActiveStatus),status.ToString()))),
                new DBService.Parameters(PageActions.P_PAGEPK,pagePK==0?(object)DBNull.Value:pagePK),
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, PageActions.SP_GETPAGES).Tables[0];

        }

        /// <summary>
        /// Methord used to get page Details 
        /// </summary>
        /// <param name="pagePK"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataTable GetPages(int? PagePK, int? Status, string PageUrl)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {
                new DBService.Parameters( CommonConstants.P_ACTIVE,Status.HasValue ? Status : (object)DBNull.Value),
                new DBService.Parameters(PageActions.P_PAGEPK, (PagePK.HasValue && PagePK > 0)?PagePK : (object)DBNull.Value),
                new DBService.Parameters(PageActions.P_PAG_URL, string.IsNullOrEmpty(PageUrl)? (object)DBNull.Value : PageUrl)
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, PageActions.SP_GETPAGES, colParameters).Tables[0];

        }

        /// <summary>
        /// Methord used to save Page Action
        /// </summary>
        /// <param name="pageAct"></param>
        /// <returns></returns>
        public static int SavePageAction(PageActionBO pageAct)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                 new DBService.Parameters(PageActions.P_PK, pageAct.PageActionPK==0?(object)DBNull.Value:pageAct.PageActionPK),  
                 new DBService.Parameters(PageActions.P_PAGE, pageAct.Page),  
                 new DBService.Parameters(PageActions.P_SECTION, pageAct.Section),  
                 new DBService.Parameters(PageActions.P_ACTION, pageAct.Action),  
                 new DBService.Parameters(PageActions.P_DESC,pageAct.ActionDesc),
                 new DBService.Parameters( CommonConstants.BIZUNIT,pageAct.BizUnit),
                 new DBService.Parameters( CommonConstants.USERPK,pageAct.User),
                 new DBService.Parameters( CommonConstants.LASTMODDATE,pageAct.PageActionPK == 0?(object)DBNull.Value:pageAct.LastModDt),

                 new DBService.Parameters( CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),           
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, PageActions.SP_SAVEPAGEACTION, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ CommonConstants.RETURNVAL]).Value);
        }

        /// <summary>
        /// Methord used to delete Page Actions
        /// </summary>
        /// <param name="currPK"></param>
        /// <returns></returns>
        public static int DeletePageAction(int currPK, DateTime lastModBY)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                 new DBService.Parameters(PageActions.P_PK, currPK),  
                 new DBService.Parameters( CommonConstants.LASTMODDATE,lastModBY),
                 new DBService.Parameters( CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),           
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, PageActions.SP_DELETEPAGEACTION, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[ CommonConstants.RETURNVAL]).Value);
        }

        /// <summary>
        /// Methord used to get Page Actions
        /// </summary>
        /// <param name="currPK"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataSet GetPageAction(int currPK, DbActiveStatus status, int bizUnit)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                 new DBService.Parameters(PageActions.P_PK, currPK), 
                 new DBService.Parameters( CommonConstants.BIZUNIT,bizUnit==0?(object)DBNull.Value:bizUnit),
                 new DBService.Parameters( CommonConstants.P_ACTIVE,status)
                 
            };
            return dbService.DataAdapter(CommandType.StoredProcedure, PageActions.SP_GETPAGEACTIONSKV, colParameters);
        }
    }
}
