using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel;
using System.Threading;
using System.Configuration;
using System.Net;
using BusinessObject;
using System.IO;

namespace HRMS.BackgroundTasks
{
    public class BackgroundTaskService
    {
        #region properties and Variables
        #region properties
        #region public properties
        /// <summary>
        /// Flag indicating whether the service is running or not
        /// </summary>
        public bool IsStarted { get; set; }
        public DataTable dtPaySlipMailList { get; set; }
        public bool FromExternalSource { get; set; }
        #endregion
        #region private properties
        /// <summary>
        /// The mail service background worker thread
        /// </summary>
        private BackgroundWorker MailBackgroundService { get; set; }
      
        /// <summary>
        /// Current user details
        /// </summary>
        public User currentUser { get; set; }

        #endregion
        #endregion

       
        DataSet dsMailQue;

        #endregion

        #region public methods
        /// <summary>
        /// Start Mail service
        /// </summary>
        public void StartBackgroundService()
        {
            MailBackgroundService = new BackgroundWorker();
            MailBackgroundService.DoWork += new DoWorkEventHandler(BackgroundService);
            MailBackgroundService.RunWorkerAsync();
            IsStarted = true;
        }
        /// <summary>
        /// End Mail service
        /// </summary>
        public void StopBackgroundService()
        {
            IsStarted = false;
            MailBackgroundService.Dispose();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Mail service background worker thread body
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void BackgroundService(Object Sender, DoWorkEventArgs e)
        {
            Monitor.Enter(this);           
            //service starting
            try
            {
                if (FromExternalSource != true)
                {
                    dtPaySlipMailList = null;
                    GetPaySlipMailList();
                }
                string uploadPath = string.Empty;
                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                {
                    uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\Attachments";
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);
                    uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\Attachments\\";
                }
                else
                {
                    uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "Attachments";
                    if (!Directory.Exists(uploadPath))
                        Directory.CreateDirectory(uploadPath);
                    uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "Attachments\\";
                }
                foreach (DataRow drMailDtl in dtPaySlipMailList.Rows)
                {
                    string targetUrl = ConfigurationManager.AppSettings["WEBREQURL"].ToString() + ERP.Utilities.Constants.DA.PageURL.PDFPAYSLIPURL;
                    HttpWebRequest URLReq;
                    HttpWebResponse URLRes;
                    /*Get Report server url with required parameters */
                    //RecPK={0}&RptType={1}&RptSubType={2}&EmpName={3}&SbuID={4}&RptName={5}&Token={6}&UserID={7}&Dept={8}&FilePath={9}
                    string strFilePath = uploadPath + drMailDtl["EPM_FILE_NAME"].ToString() + ".pdf";
                    string LinkAddress = string.Format(targetUrl,
                        //RecPK 
                                             drMailDtl["EPM_EPS_PK"].ToString()
                        //RptType
                                            , "PAYRL"
                        //RptSubType
                                            , "9"
                        //EmpName
                                            , currentUser.EmpName
                        //SbuID
                                            , currentUser.SBUID.ToString()
                        //RptName
                                            , drMailDtl["EPM_FILE_NAME"].ToString() + ".pdf"
                        //Token
                                            , "1"
                        //UserID
                                            , currentUser.PKUser.ToString()
                        //Dept
                                            , currentUser.CurrentDeptPK.ToString()
                        //FilePath
                                            , HttpUtility.HtmlEncode(uploadPath)
                                          );

                    if (!string.IsNullOrEmpty(LinkAddress))
                    {
                        try
                        {

                            URLReq = (HttpWebRequest)WebRequest.Create(LinkAddress);
                            URLRes = (HttpWebResponse)URLReq.GetResponse();
                            if (URLRes.StatusCode == HttpStatusCode.OK)
                            {
                                int result = DataAccess.Administration.Configurations.ViewMailsDA.PaySlipMailQueSave(Convert.ToInt32(drMailDtl["EPM_PK"]), strFilePath, "PDF", currentUser.SBUID);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //  Thread.CurrentThread.Abort();
            }
            finally
            {
                StopBackgroundService();
            }
        }

        /// <summary>
        /// Method to get mail list for sending
        /// </summary>
        private void GetPaySlipMailList()
        {
            dtPaySlipMailList = null;
            dsMailQue = DataAccess.Administration.Configurations.ViewMailsDA.GetPaySlipMailList((int?)null, 0);
            if (dsMailQue != null && dsMailQue.Tables.Count > 0)
                dtPaySlipMailList = dsMailQue.Tables[0];
        }
        #endregion
    }
}