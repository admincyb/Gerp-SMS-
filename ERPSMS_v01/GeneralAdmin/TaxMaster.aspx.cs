using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class TaxMaster : ERP.Store.UI.MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Type"] != null)
                {
                    Type.Value = Request.QueryString["Type"].ToString();
                }
                else
                {
                    Type.Value = "0";
                }
                hdfGstEnabled.Value = GetGlobalResourceObject("ConfigurationsRes", "EnableGST").ToString();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadComplete(EventArgs e)
        {
            if (!IsPostBack)
            {
                //assign Page BreadCrumb
                base.OnLoadComplete(e);
                AssignLocalBreadCrumb();
            }
        }
        /// <summary>
        /// Assign breadCrumb
        /// </summary>
        public void AssignLocalBreadCrumb()
        {
            try
            {
                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "1")
                {
                    if (this.GetLocalResourceObject("BreadcrumbTax") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbTax").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("TaxTitle").ToString();
                    }
                }
                else if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "4")
                {
                    if (this.GetLocalResourceObject("BreadcrumbDeduction") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbDeduction").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("DeductionTitle").ToString();
                    }
                }
                else
                {
                    if (this.GetLocalResourceObject("BreadcrumbChanrges") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbChanrges").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("PoTitle").ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}