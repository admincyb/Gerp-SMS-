using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GTIService.Constants.Common;

namespace DataAccess.Administration.Masters
{
    public class RelatedLinkDA
    {
        #region Methods

        /// <summary>
        /// method for Get Pages
        /// </summary>
        /// <param name="pageID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetPages(int pageID)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Administration.Masters.RelatedLink.Parameters.PAGEID ,pageID==0?(object)DBNull.Value:pageID),
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.RelatedLink.Procedures.SP_GETPAGES, colParameters).Tables[0];
            return dtList;
        }

        /// <summary>
        /// method for Get Page List
        /// </summary>
        /// <param name="pageID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetRelatedPageList(int pageID)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Administration.Masters.RelatedLink.Parameters.PAGEID ,pageID),
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.RelatedLink.Procedures.SP_GETLISTOFGRELATEDPAGES, colParameters).Tables[0];
            return dtList;
        }

        /// <summary>
        /// method used to Save Related Links and return PK
        /// </summary>
        /// <param name="Xml" Type=object></param>
        /// <returns>int</returns>
        public static int SaveRelatedLinks(string Xml)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Administration.Masters.RelatedLink.Parameters.RELATEDXML ,Xml),
                new DBService.Parameters(CommonConstants.RETURNVAL ,  0, 20,ParameterDirection.Output, DBService.ParameterType.Number),
            };
            dbService.ExecuteNonQuery(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.RelatedLink.Procedures.SP_SAVE, colParameters);
            return Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[CommonConstants.RETURNVAL]).Value);
        }

        /// <summary>
        /// method for Get User Related Pages according User Rights.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="pageID"></param>
        /// <returns>DataTable</returns>
        public static DataTable GetUserRelatedPages(int userID, int pageID, string absoluteUrl)
        {
            DataTable dtList;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            { 
                new DBService.Parameters(GTIService.Constants.Administration.Masters.RelatedLink.Parameters.USERID, userID==0?(object)DBNull.Value:userID),
                new DBService.Parameters(GTIService.Constants.Administration.Masters.RelatedLink.Parameters.PAGEID ,pageID==0?(object)DBNull.Value:pageID),
                new DBService.Parameters("P_PATH" ,absoluteUrl),
            };
            dtList = dbService.DataAdapter(CommandType.StoredProcedure, GTIService.Constants.Administration.Masters.RelatedLink.Procedures.SP_GETLISTOFUSERRELATEDPAGES, colParameters).Tables[0];
            return dtList;
        }

        #endregion
    }
}
