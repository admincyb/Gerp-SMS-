using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using System.Data;
using BusinessObject.Common;

namespace ERPSMS_v01.UserControls
{
    public partial class MenuControl1 : System.Web.UI.UserControl
    {
        #region Variables

        BusinessObject.User currentUser;
        ActionsEnum commonActions;

        #endregion

        #region Events

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillLocation(false);
            }
        }

        protected void ActionHandler(object sender, EventArgs e)
        {
            if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
            {
                commonActions = ActionsEnum.LINKBUTTONCLICK;
            }
            switch (commonActions)
            {
                case ActionsEnum.LINKBUTTONCLICK:
                    Response.Redirect((sender as LinkButton).CommandArgument,true);
                    break;
            }
        }

        #region DetailsView Events

        /// <summary>
        /// Fill repeater with Group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            List<BusinessObject.RightGroupNames> src = (List<BusinessObject.RightGroupNames>)((Repeater)sender).DataSource;
            ((Repeater)e.Item.FindControl("rptLinks")).DataSource = src[e.Item.ItemIndex].RightLinks;
            ((Repeater)e.Item.FindControl("rptLinks")).DataBind();
        }
        /// <summary>
        /// Fill Repeater with Menu items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rptMenu_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            List<BusinessObject.MenuBO> src = (List<BusinessObject.MenuBO>)((DataList)sender).DataSource;
            ((Repeater)e.Item.FindControl("rptGroup")).DataSource = src[e.Item.ItemIndex].RightGroupNames;
            ((Repeater)e.Item.FindControl("rptGroup")).DataBind();

            ((DataList)e.Item.FindControl("dtlstIcons")).DataSource = src[e.Item.ItemIndex].IconList;
            ((DataList)e.Item.FindControl("dtlstIcons")).DataBind();
        }

        #endregion

        #region DropdownSelected Index Event

        /// <summary>
        /// Event on location change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDepartment(currentUser.PKUser, Convert.ToInt32(this.ddlLocation.SelectedValue), true);
        }

        /// <summary>
        /// Event on Department change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCostCenter_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillMenu(true);
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// Fill Location Dropdown based on UserPK and SBU
        /// </summary>
        private void FillLocation(bool isEditUser)
        {
            DataTable sbuDetails;
            sbuDetails = BusinessLogic.CommonManagement.CommonBL.GetBizUnit(currentUser.PKUser, 0);
            if (sbuDetails.Rows.Count > 0)
            {
                this.ddlLocation.DataSource = sbuDetails;
                this.ddlLocation.DataTextField = "BZU_NAME";
                this.ddlLocation.DataValueField = "DPT_BIZUNIT";
                this.ddlLocation.DataBind();
                if (currentUser.SBUID != 0)
                    this.ddlLocation.SelectedValue = currentUser.SBUID.ToString();
                if (isEditUser)
                {
                    BusinessObject.SBU sbu = new BusinessObject.SBU() { CurrentSBU = this.ddlLocation.SelectedItem.Text, CurrentSBUPK = int.Parse(this.ddlLocation.SelectedValue) };
                    BusinessLogic.AccountManagement.UserAuthBL userAuth = new BusinessLogic.AccountManagement.UserAuthBL();
                    userAuth.SetUserProperty(UserPropertyEnum.CurrentSBU, sbu);
                }
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FillDepartment(currentUser.PKUser, Convert.ToInt32(this.ddlLocation.SelectedValue), isEditUser);
            }
            else
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.HttpContext.Current.User = null;
               Response.Redirect("~/Login.aspx", true);
            }


        }

        /// <summary>
        /// Fill Department Dropdown
        /// </summary>
        /// <param name="userPK"></param>
        /// <param name="sbuID"></param>
        private void FillDepartment(int userPK, int sbuID, bool isEditUser)
        {
            DataTable dtDept;
            this.ddlCostCenter.Items.Clear();
            dtDept = BusinessLogic.CommonManagement.CommonBL.GetDepartment(userPK, sbuID);
            this.ddlCostCenter.DataSource = dtDept;
            this.ddlCostCenter.DataTextField = "DPT_NAME";
            this.ddlCostCenter.DataValueField = "DPT_PK";
            this.ddlCostCenter.DataBind();
            if (dtDept.Rows.Count > 0)
            {
                try
                {
                    if (currentUser.CurrentDeptPK != 0)
                        this.ddlCostCenter.SelectedValue = currentUser.CurrentDeptPK.ToString();
                }
                catch
                {
                }
                if (isEditUser)
                {
                    BusinessObject.Department dept = new BusinessObject.Department() { CurrentSBU = this.ddlLocation.SelectedItem.Text, CurrentSBUPK = int.Parse(this.ddlLocation.SelectedValue), CurrentDept = this.ddlCostCenter.SelectedItem.Text, CurrentDeptPK = int.Parse(this.ddlCostCenter.SelectedValue) };
                    BusinessLogic.AccountManagement.UserAuthBL userAuth = new BusinessLogic.AccountManagement.UserAuthBL();
                    userAuth.SetUserProperty(UserPropertyEnum.CurrentDept, dept);
                }
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                Literal litSBU = (Literal)this.Page.Master.FindControl("litSBU");
                litSBU.Text = string.Format(Resources.Messages.SBUDisplay.ToString(), ddlLocation.SelectedItem.Text);
                Literal litDept = (Literal)this.Page.Master.FindControl("litDept");
                litDept.Text = string.Format(Resources.Messages.DepartmentDisplay.ToString(), ddlCostCenter.SelectedItem.Text);
                FillMenu(isEditUser);
            }
            else
            {
                Literal litSBU = (Literal)this.Page.Master.FindControl("litSBU");
                litSBU.Text = string.Format(Resources.Messages.SBUDisplay.ToString(), ddlLocation.SelectedItem.Text);
                Literal litDept = (Literal)this.Page.Master.FindControl("litDept");
                litDept.Text = string.Format(Resources.Messages.DepartmentDisplay.ToString(), string.Empty);
            }

        }

        /// <summary>
        /// Fill the menu based on selected costcenter and userPK
        /// </summary>
        private void FillMenu(bool isEditUser)
        {
            if (isEditUser)
            {
                BusinessObject.Department dept = new BusinessObject.Department() { CurrentSBU = ddlLocation.SelectedItem.Text, CurrentSBUPK = int.Parse(ddlLocation.SelectedValue), CurrentDept = ddlCostCenter.SelectedItem.Text, CurrentDeptPK = int.Parse(ddlCostCenter.SelectedValue) };
                BusinessLogic.AccountManagement.UserAuthBL userAuth = new BusinessLogic.AccountManagement.UserAuthBL();
                userAuth.SetUserProperty(UserPropertyEnum.CurrentDept, dept);
                Literal litSBU = (Literal)this.Page.Master.FindControl("litSBU");
                litSBU.Text = string.Format(Resources.Messages.SBUDisplay.ToString(), ddlLocation.SelectedItem.Text);
                Literal litDept = (Literal)this.Page.Master.FindControl("litDept");
                litDept.Text = string.Format(Resources.Messages.DepartmentDisplay.ToString(), ddlCostCenter.SelectedItem.Text);
                Session[SessionStrings.CurDept] = ddlCostCenter.SelectedValue;
            }
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            #region Culture Checking
            HttpCookie userCulture = Request.Cookies["Culture"];
            string cultureCode = userCulture != null ? userCulture.Value : currentUser.UserCulture;
            string menuMode = cultureCode == GetGlobalResourceObject("ConfigurationsRes", "DefaultCulture").ToString() ? "1" : GetGlobalResourceObject("ConfigurationsRes", "MenuLanguageMode").ToString();
            #endregion
            rptMenu.DataSource = BusinessLogic.Administration.Configurations.MenuManagement.GetMenuDetails(currentUser,Convert.ToInt32(menuMode));
            rptMenu.DataBind();
        }

        #endregion
    }
}