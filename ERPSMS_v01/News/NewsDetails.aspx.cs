using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.AccountManagement;
using BusinessObject.Administration.Configurations;
using BusinessObject.CommonManagement;
using BusinessLogic.Administration.Configurations;
using ERP.Utilities;
using GTIService.Constants.Administration.Configurations;

namespace ERPSMS_v01.News
{
    public partial class NewsDetails :System.Web.UI.Page // ERP.Store.UI.MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString[QueryStrings.newsRequested] != null)
            {
                GetNewsDetails(Convert.ToInt32(Request.QueryString[QueryStrings.newsRequested]));
            }
        }

        /// <summary>
        /// Method to get News Details corresponding to the PK
        /// </summary>
        /// <param name="currPK"></param>
        private void GetNewsDetails(int currPK)
        {
            DataTable dtNews;
            dtNews = new DataTable();
            dtNews = NewsManagementBL.GetNews(currPK, DbActiveStatus.HASPK);
            if (dtNews != null && dtNews.Rows.Count > 0)
            {
                lblTitle.Text = HttpUtility.HtmlDecode(dtNews.Rows[0][NewsManagements.F_TITLE].ToString());
                lblShrtDesc.Text = HttpUtility.HtmlDecode(dtNews.Rows[0][NewsManagements.F_SHORTDESC].ToString());
                divNewsDetails.InnerHtml = HttpUtility.HtmlDecode(dtNews.Rows[0][NewsManagements.F_DETAILS].ToString()).Replace("\n", "<br />"); ;
            }

        }
    }
}