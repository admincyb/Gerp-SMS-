using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Administration.Masters
{
    public partial class MaterialCategoryMaster : Common.UserPages
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ConfigurationSettings();
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
                //Breadcrumb Material Return
                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "8")
                {
                    if (this.GetLocalResourceObject("BreadcrumbAssetCategory") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbAssetCategory").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("AssetTitle").ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {

            #region Show/Hide SaleItem Checkbox  in Material Category master
            if (GetGlobalResourceObject("ConfigurationsRes", "ShowSaleItemMaterialCategory").ToString() == "1")
            {
                hdfShowSaleItem.Value = "1";
            }
            else
            {
                hdfShowSaleItem.Value = "0";
            }
            hdfCategoryACBySBU.Value = GetGlobalResourceObject("ConfigurationsRes", "CategoryACBySBU").ToString();
            #endregion
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            if (hdfCategoryACBySBU.Value == "1")
            {
                thCompany.Visible = false;
                thSBU.Visible = true;
            }
            else
            {
                thCompany.Visible = true;
                thSBU.Visible = false;
            }

        }
    }
}