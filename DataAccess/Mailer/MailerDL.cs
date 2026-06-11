using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERP.Utilities;
using DataAccess;
using BusinessObject.Constants;
using BusinessObject;
using ERP.Utilities.Constants;
using BusinessObject.Mailer;

namespace DataAccess.Mailer
{
    public class MailerDL
    {
        /// <summary>
        /// Save Brand Details
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static int SaveMail(string pXML)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(MailerDA.P_XML, pXML),
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, MailerDA.SP_SaveMail, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
        /// <summary>
        /// Delete Customer Mail
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static int DeleteMail(int pk,DateTime lastModDate)
        {
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(MailerDA.P_MLQ_PK, pk),
                new DBService.Parameters(MailerDA.P_LAST_MOD_DT, lastModDate),

                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, MailerDA.SP_DeleteMail, colParameters);
            int result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
   /// <summary>
   /// Get Mail details
   /// </summary>
   /// <param name="mailPK"></param>
   /// <returns></returns>
        public static DataSet GetMail(int mailPK)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(MailerDA.P_CMH_PK, mailPK),
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, MailerDA.SP_GetMail, colParameters);
            }
            return ds;

        }
        /// <summary>
        /// Get Customer Mails List
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataTable GetCustomerMails(string pXML)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                new DBService.Parameters(MailerDA.P_XML, pXML),
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, MailerDA.SP_GetCustomerMail, colParameters);
            }
            return ds.Tables[0];
        }
        public static DataSet GetEnquiryList(GridPrams grid, User objUser, int procID, int userPK, short status, int enqPK, int cusPK, string pageURL)
        {
            return DataAccess.SaleOrder.EnquiryDL.GetEnquiryList(grid, objUser, procID, userPK, status, enqPK, cusPK, pageURL);
        }
        /// <summary>
        /// Get Mail Listing
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        
        public static DataSet GetMailList(GridPrams grid, User objUser,int customerPK,string subject,int status,int type)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.BIZUNIT, objUser.SBUID),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO, grid.PageNumber == 0 ?(Object)DBNull.Value : grid.PageNumber),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.CMH_TYPE, type == 0 ?(Object)DBNull.Value : type),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize  == 0 ?(Object)DBNull.Value : grid.PageSize),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTBY  , grid.SortBy== string.Empty ? "%" : grid.SortBy),
                //  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENBY  , grid.ThenBy== string.Empty ? "%" : grid.ThenBy),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.SORTDIRC  , grid.SortDirection== string.Empty ? "%" : grid.SortDirection),
                  //new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.THENDIRC  , grid.ThenDirection== string.Empty ? "%" : grid.ThenDirection),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.FROMDATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.FromDate)),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.TODATE  , grid.ToDate== string.Empty ?(Object)DBNull.Value : Convert.ToDateTime(grid.ToDate)),
                  new DBService.Parameters(MailerDA.P_CUS_PK, customerPK > 0 ? customerPK : (object)DBNull.Value),
                  new DBService.Parameters(MailerDA.P_SUBJECT, subject=="" ? (object)DBNull.Value : "'%" +subject+ "%'"),
                  new DBService.Parameters(MailerDA.P_STATUS, status),
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, MailerDA.SP_GetMailList, colParameters);
            }
            return ds;
        
        }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="fromDate"></param>
      /// <param name="toDate"></param>
      /// <param name="partyType"></param>
      /// <param name="partyPK"></param>
      /// <param name="applicationType"></param>
      /// <returns></returns>

        public static DataSet GetMailQList(GridPrams grid, int partyType, int partyPK, int appTypePK,int status,string subj)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGENO, grid.PageNumber == 0 ?(Object)DBNull.Value : grid.PageNumber),
                  new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.PAGESIZE, grid.PageSize  == 0 ?(Object)DBNull.Value : grid.PageSize),
                  new DBService.Parameters(MailerDA.P_FROM_DATE  , grid.FromDate== string.Empty ?(Object)DBNull.Value : grid.FromDate),
                  new DBService.Parameters(MailerDA.P_TO_DATE  ,grid.ToDate== string.Empty ?(Object)DBNull.Value : grid.ToDate),
                  new DBService.Parameters(MailerDA.P_MLQ_PARTY_TYPE, partyType > 0 ? partyType : (object)DBNull.Value),
                  new DBService.Parameters(MailerDA.P_MLQ_PARTY_PK, partyPK > 0 ? partyPK : (object)DBNull.Value),
                  new DBService.Parameters(MailerDA.P_MLQ_STATUS, status> -1 ? status :(Object)DBNull.Value ),
                  new DBService.Parameters(MailerDA.P_APT_PK, appTypePK > 0 ? appTypePK : (object)DBNull.Value),
                  new DBService.Parameters(MailerDA.P_MLQ_SUBJECT, subj == string.Empty ? (Object)DBNull.Value : subj)
                 // new DBService.Parameters(MailerDA.P_MLQ_APP_TYPE  ,applicationType== string.Empty ?(Object)DBNull.Value : applicationType),

                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, MailerDA.SP_GetMaiQlList, colParameters);
            }
            return ds;

        }
        /// <summary>
        /// Get Mail details
        /// </summary>
        /// <param name="mailQPK"></param>
        /// <param name="bizUnit"></param>
        /// <returns></returns>
        public static DataSet GetMailQueue(int mailQPK,int bizUnit)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                  new DBService.Parameters(MailerDA.P_MLQ_PK,mailQPK),
                  new DBService.Parameters(MailerDA.P_BIZUNIT, bizUnit)
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, MailerDA.SP_GetMaiQlList, colParameters);
            }
            return ds;

        }

        /// <summary>
        /// Save mail attachments for Mail Que
        /// </summary>
        /// <param name="objAttachmentBO"></param>
        /// <returns></returns>
        public static long? SaveMailAttachment(MailAttachmentBO objAttachmentBO)
        {
            int? result = null;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(MailerDA.P_ADD_APP_TYPE, objAttachmentBO.ADD_APP_TYPE),
                new DBService.Parameters(MailerDA.P_ADD_APP_SUB_TYPE, objAttachmentBO.ADD_APP_SUB_TYPE > 0 ? objAttachmentBO.ADD_APP_SUB_TYPE : (Object)DBNull.Value),
                new DBService.Parameters(MailerDA.P_ADD_APP_PK, objAttachmentBO.ADD_APP_PK),
                new DBService.Parameters(MailerDA.P_ADD_SL_NO, objAttachmentBO.ADD_SL_NO),
                new DBService.Parameters(MailerDA.P_ADD_TITLE, objAttachmentBO.ADD_TITLE),
                new DBService.Parameters(MailerDA.P_ADD_NAME, objAttachmentBO.ADD_NAME),
                new DBService.Parameters(MailerDA.P_ADD_PATH, objAttachmentBO.ADD_PATH),
                new DBService.Parameters(MailerDA.P_ADD_TYPE, objAttachmentBO.ADD_TYPE),
                new DBService.Parameters(MailerDA.P_ADD_DESC, objAttachmentBO.ADD_DESC),
                new DBService.Parameters(MailerDA.P_ACTIVE, objAttachmentBO.ACTIVE),
                new DBService.Parameters(MailerDA.P_USER_PK, objAttachmentBO.USER_PK),
                new DBService.Parameters(MailerDA.P_BIZUNIT, objAttachmentBO.BIZUNIT), 
                 new DBService.Parameters(MailerDA.P_ADD_VERSION, objAttachmentBO.ADD_VERSION),      
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, MailerDA.SPADM_MAIL_APP_DOC_DTL_SAVE, colParameters);
            result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }

        public static DataSet GetMailTemplate(int CurrPK,string TemplateName, short Status, int BizUnit, int Editable, int PageNo, int PageSize)
        {
            DataSet ds = null;
            {
                DBService dbService = new DBService();
                DBService.Parameters[] colParameters = null;
                colParameters = new DBService.Parameters[] 
                { 
                  new DBService.Parameters(MailerDA.P_TML_PK,CurrPK),
                  new DBService.Parameters(MailerDA.P_ACTIVE, Status),
                  new DBService.Parameters(MailerDA.P_BIZUNIT, BizUnit),
                  new DBService.Parameters(MailerDA.P_IS_EDIT, Editable),
                  new DBService.Parameters(MailerDA.P_TML_NAME2, string.IsNullOrEmpty(TemplateName)? (Object)DBNull.Value : TemplateName),
                };
                ds = dbService.DataAdapter(CommandType.StoredProcedure, MailerDA.SPADM_MAIL_TEMPLATE_GET_KV, colParameters);
            }
            return ds;
        }

        public static int SaveMailTemplate(MailTemplateBO mailTemplateObj, DateTime LastModifiedTime)
        {
            int result = 0;
            DBService dbService = new DBService();
            DBService.Parameters[] colParameters = null;
            colParameters = new DBService.Parameters[] 
            {                
                new DBService.Parameters(MailerDA.P_TML_PK, mailTemplateObj.TML_PK),
                new DBService.Parameters(MailerDA.P_TML_NAME, mailTemplateObj.TML_NAME),
                new DBService.Parameters(MailerDA.P_TML_TEMPLATE, mailTemplateObj.TML_TEMPLATE),
                new DBService.Parameters(MailerDA.P_ACTIVE, mailTemplateObj.ACTIVE),
                new DBService.Parameters(MailerDA.P_CRTD_BY, mailTemplateObj.CRTD_BY),
                new DBService.Parameters(MailerDA.P_LAST_MOD_DT, LastModifiedTime),
                new DBService.Parameters(MailerDA.P_TML_TO_CC, mailTemplateObj.TML_TO_CC),
                new DBService.Parameters(MailerDA.P_TML_TO_BCC, mailTemplateObj.TML_TO_BCC),
                new DBService.Parameters(MailerDA.P_TML_FROM, mailTemplateObj.TML_FROM),
                new DBService.Parameters(MailerDA.P_TML_IS_EDIT, mailTemplateObj.TML_IS_EDIT),
                new DBService.Parameters(MailerDA.P_TML_NAME2, mailTemplateObj.TML_NAME2),
                new DBService.Parameters(MailerDA.P_TML_MOD_BY, mailTemplateObj.TML_MOD_BY),                    
                new DBService.Parameters(GTIService.Constants.Common.Parameters_Common.RETVAL, 0, 20,ParameterDirection.Output, DBService.ParameterType.Number)
            };
            int rows = dbService.ExecuteNonQuery(CommandType.StoredProcedure, MailerDA.SPADM_MAIL_TEMPLATE_SAVE, colParameters);
            result = Convert.ToInt32(((IDataParameter)dbService.oCommand.Parameters[GTIService.Constants.Common.Parameters_Common.RETVAL]).Value);
            return result;
        }
    }
}

