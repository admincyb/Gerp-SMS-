using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ERP.Utilities;

namespace ERPSMS_v01.Administration.Masters
{
    public partial class MaterialMaster : Common.UserPages
    {
        int vendorID = 0;
        BusinessObject.User objUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hdfCurDate.Value = DateTime.Now.ToString(CommonConstants.DATEFORMAT);
                BusinessObject.CommonManagement.CommonObject.File file = new BusinessObject.CommonManagement.CommonObject.File();
                file.FILELIST = new List<BusinessObject.CommonManagement.CommonObject.File.FileList>();
                FILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
                TEMPFILELIST.Value = Newtonsoft.Json.JsonConvert.SerializeObject(file);
                if (Request.QueryString["Type"] != null)
                {
                    ITM_SET.Value = Request.QueryString["Type"].ToString();
                }
                else
                    ITM_SET.Value = "1";
                if (Request.QueryString["Code"] != null)
                {
                    SearchValue.Text = Request.QueryString["Code"].ToString();
                }
                else
                    SearchValue.Text = string.Empty;
                   
                objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillMappingDetails();
                if (Request.QueryString["RefID"] != null)
                {
                    FillWorkFlowDetails(Convert.ToInt32(Request.QueryString["RefID"]));
                }
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
                //Breadcrumb Packing
                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "3")
                {
                    if (this.GetLocalResourceObject("BreadcrumbPacking") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbPacking").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("PackingTitle").ToString();
                    }
                }
                else
                    //Breadcrumb Services
                    if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "4")
                    {
                        if (this.GetLocalResourceObject("BreadcrumbServices") != null)
                        {
                            string breadCrumb;
                            breadCrumb = this.GetLocalResourceObject("BreadcrumbServices").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                            lblBreadCrum.Text = breadCrumb;
                            Page.Title = GetLocalResourceObject("ServicesTitle").ToString();
                        }
                    }
                    else
                    {
                        if (this.GetLocalResourceObject("Breadcrumb") != null)
                        {
                            string breadCrumb;
                            breadCrumb = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                            lblBreadCrum.Text = breadCrumb;
                            Page.Title = GetLocalResourceObject("MaterialTitle").ToString();
                        }
                    }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Function Used to fill Vendor details to hiddenfiled
        /// </summary>
        /// <param name="vendorID"></param>        
        private void FillMappingDetails()
        {
            if (vendorID != 0)
            {
                //MappingDetailsList.Value = BusinessLogic.VendorManagement.VendorRegistration.GetVendorDetails(vendorID);
            }
            else
            {
                BusinessObject.MaterialManagement.Vendor vendorObject = new BusinessObject.MaterialManagement.Vendor();
                vendorObject.MappingDetailsList = new List<BusinessObject.MaterialManagement.MappingDetailsList> { };
                //Assigning initialized VendorObject to hidden field (VendorDetails) MaterialDetails
                MappingDetailsList.Value = Newtonsoft.Json.JsonConvert.SerializeObject(vendorObject);
            }
        }
        /// <summary>
        /// Method to do Action For Work Flow
        /// </summary>
        private void FillWorkFlowDetails(int refID)
        {
            int status = 0;
            btnSave.Visible = true;
            btnReset.Visible = true;
            MaterialReferenceID.Value = refID.ToString();
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtAppStatus = wrkfService.GetAppStatus(Convert.ToInt32(refID), ((BusinessObject.User)HttpContext.Current.User.Identity).PKUser);
            if (dtAppStatus.Rows.Count > 0)
            {
                status = 2;
                DataRow drow = dtAppStatus.Rows[0];
                MaterialProcessID.Value = drow["Wtlpscro"].ToString();
                MaterialTaskID.Value = drow["KdsSkt"].ToString();
                MaterialApplicationID.Value = drow["refApplication"].ToString();
                MaterialReferenceID.Value = Request.QueryString["RefID"].ToString();
                MaterialActionID.Value = drow["Ndtp"].ToString();
            }
            else
            {
                DataTable dtAppID = wrkfService.GetApplicationID(Convert.ToInt32(refID));
                if (dtAppID.Rows.Count > 0)
                {
                    status = 1;
                    DataRow drow = dtAppID.Rows[0];
                    MaterialApplicationID.Value = drow["refApplication"].ToString();
                    btnSave.Visible = false;
                    btnReset.Visible = false;
                }
            }
            MaterialDetailsObj.Value = BusinessLogic.MaterialManagement.MaterialMaster.GetInActiveMaterialDetails(int.Parse(MaterialApplicationID.Value), objUser.SBUID, status);
        }
    }
}