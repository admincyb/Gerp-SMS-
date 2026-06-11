using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess.Mailer;
using BusinessObject;
using BusinessObject.Mailer;
namespace BusinessLogic.Mailer
{
    public class MailerBL
    {
        /// <summary>
        /// Get Customer List
        /// </summary>
        /// <param name="bizUnit"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        public static int SaveMail(string pXML)
        {
            return MailerDL.SaveMail(pXML);
        }
        /// <summary>
        /// Delete Customer Mail
        /// </summary>
        /// <param name="pk"></param>
        /// <returns></returns>
        public static int DeleteMail(int pk,DateTime lastModDate)
        {
            return MailerDL.DeleteMail(pk, lastModDate);
        }

        /// <summary>
        /// Get Mail Details
        /// </summary>
        /// <param name="mailPK"></param>
        /// <returns></returns>
        public static DataSet GetMail(int mailPK)
        {
            return MailerDL.GetMail(mailPK);
        }
        /// <summary>
        /// Get Customer Mails
        /// </summary>
        /// <param name="pXML"></param>
        /// <returns></returns>
        public static DataTable GetCustomerMails(string pXML)
        {
            return MailerDL.GetCustomerMails(pXML);
        }
        /// <summary>
        /// Get Mail List
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="objUser"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static DataSet GetMailList(GridPrams grid, User objUser,int customerPK,string subject, int status,int type)
        {
            return MailerDL.GetMailList(grid, objUser,customerPK,subject, status,type);
        }
     /// <summary>
     /// 
     /// </summary>
     /// <param name="grid"></param>
     /// <param name="partyType"></param>
     /// <param name="partyPK"></param>
     /// <param name="applicationType"></param>
     /// <returns></returns>
        public static DataSet GetMailQList(GridPrams grid, int partyType, int partyPK, int appTypePK,int status,string Subj)
        {
            return MailerDL.GetMailQList(grid, partyType, partyPK, appTypePK, status, Subj);
        }
        /// <summary>
        /// Get Mail Queue Details
        /// </summary>
        /// <param name="mailQPk"></param>
        /// <param name="bizUnitPK"></param>
        /// <returns></returns>
        public static DataSet GetMailQueue(int mailQPk, int bizUnit)
        {
            return MailerDL.GetMailQueue(mailQPk, bizUnit);
        }

        public static long? SaveMailAttachment(MailAttachmentBO objAttachmentBO)
        {
            return MailerDL.SaveMailAttachment(objAttachmentBO);
        }


        public static DataSet GetMailTemplate(int CurrPK, string TemplateName, short Status, int BizUnit, int Editable, int PageNo, int PageSize)
        {
            return MailerDL.GetMailTemplate(CurrPK, TemplateName, Status, BizUnit, Editable, PageNo, PageSize);
        }

        public static int SaveMailTemplate(MailTemplateBO mailTemplateObj, DateTime LastModifiedTime)
        {
            return MailerDL.SaveMailTemplate(mailTemplateObj, LastModifiedTime);
        }
    }
}
