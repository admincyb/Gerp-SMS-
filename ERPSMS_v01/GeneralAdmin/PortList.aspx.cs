using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using BusinessObject;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.Common;
using DataAccess.CommonManagement;
using BusinessLogic.Administration.Masters;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class PortList : ERP.Store.UI.MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
            #region Breadcrumb
            if (this.GetLocalResourceObject("Breadcrumb") != null)
            {
                string breadCrumb;
                breadCrumb = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                lblBreadCrum.Text = breadCrumb;
                Page.Title = GetLocalResourceObject("Title_Port").ToString();
            }
            #endregion
        }

        #region Variables and Properties
        #region Properties

        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }
        /// <summary>
        /// Current PK
        /// </summary>
        private int CurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }

        #endregion
        BusinessObject.User currentUser;
        private DataTable dtPortList;
        private DataTable dtPortEdit;
        // List variables for binding details to controls
        private ActionsEnum commonActions;
        #endregion

        #region Get Field Values
        private void GetFieldValues(ControlEnum type)
        {
            switch (type)
            {
                case ControlEnum.PORTLIST:
                    dtPortList = new DataTable();
                    dtPortList = BusinessLogic.Administration.Masters.PortMasterBL.GetPortList(txtCodeFilterList.Text, txtNameFilterList.Text, currentUser.SBUID);
                    break;
                
                case ControlEnum.PORTEDIT:
                    dtPortEdit = BusinessLogic.Administration.Masters.PortMasterBL.GetPortEdit(this.CurrPK, Convert.ToInt32(DbActiveStatus.HASPK));
                    break;
  
                default:
                    break;
            }
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnum.FILLGRID:
                        BindGrid(controlType);
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region PageActionHandler

        private void PageActionHandler()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                if (!IsPostBack)
                {
                    GetFieldValues(ControlEnum.PORTLIST);
                    SetFieldValues(ControlEnum.FILLGRID);
                }
            }
            catch
            {
            }
        }

        #endregion

        
        #region Page Page_PreRender
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlEnum controlType)
        {
            try
            {
                if (dtPortList != null && dtPortList.Rows.Count > 0)
                    grdPortList.DataSource =AddFromToStatus(dtPortList);
                else
                    grdPortList.DataSource = null;
                grdPortList.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private DataTable AddFromToStatus(DataTable dtPort)
        {
            if (dtPort != null && dtPort.Rows.Count > 0)
            {
                DataColumn colTo = new DataColumn("TO_PORT");
                DataColumn colFrom = new DataColumn("FROM_PORT");
                dtPort.Columns.Add(colTo);
                dtPort.Columns.Add(colFrom);
                foreach (DataRow roow in dtPort.Rows)
                {
                    //From Port
                    if (roow["PRM_IS_SALES_FROM"].ToString() == "1" && roow["PRM_IS_PUR_FROM"].ToString() == "1")
                        roow["FROM_PORT"] = "Sales,Purchase";
                    else
                    {
                        if (roow["PRM_IS_SALES_FROM"].ToString() == "1")
                            roow["FROM_PORT"] = "Sales";
                        if (roow["PRM_IS_PUR_FROM"].ToString() == "1")
                            roow["FROM_PORT"] = "Purchase";
                    }

                    // To Port
                    if (roow["PRM_IS_SALES_TO"].ToString() == "1" && roow["PRM_IS_PUR_TO"].ToString() == "1")
                        roow["TO_PORT"] = "Sales,Purchase";
                    else
                    {
                        if (roow["PRM_IS_SALES_TO"].ToString() == "1")
                            roow["TO_PORT"] = "Sales";
                        if (roow["PRM_IS_PUR_TO"].ToString() == "1")
                            roow["TO_PORT"] = "Purchase";
                    }
                }
            }

            return dtPort;
        }

        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                int result;
                result = 0;

                Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                bool bIsChecked = false;
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                switch (commonActions)
                {
                    #region EDIT
                    //To edit companydetails
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdPortList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCmpPk")).Value);
                                //EntryStatus = EntryStatus.EDITMODE;
                                //GetFieldValues(ControlEnum.PORTEDIT);
                                //SetFieldValues(ControlEnum.PORTEDIT);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            Response.Redirect(Resources.PageURL.PortDetails + "?prmPK=" + CurrPK, true);
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region NEW
                    //To add new companydetails
                    case ActionsEnum.NEW:
                        Response.Redirect(Resources.PageURL.PortDetails, true);
                        break;
                    #endregion

                    #region DELETE
                    //To delete companydetails
                    case ActionsEnum.DELETE:
                        foreach (GridViewRow grdrow in grdPortList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCmpPk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            string routeURL = Resources.PageURL.PortList.ToString();
                            result = PortMasterBL.DeletePort(CurrPK);

                            DbDeleteStatus deleteStatus = (DbDeleteStatus)result;
                            switch (deleteStatus)
                            {
                                case DbDeleteStatus.DELETED://If deletion is success
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PortDetails);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                                case DbDeleteStatus.REFERRED://If referred to another page
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PortDetails);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                                case DbDeleteStatus.SQLERROR://Sql error
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.PortDetails);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region LIST

                    case ActionsEnum.LIST:
                     
                        //ResetForm(ControlEnum.CLEAR);
                        //EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlEnum.PORTLIST);
                        SetFieldValues(ControlEnum.FILLGRID);
                        break;
                    #endregion

                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlEnum.CLEARFILTER);
                        GetFieldValues(ControlEnum.PORTLIST);
                        SetFieldValues(ControlEnum.FILLGRID);
                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {

            }
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlEnum controlType)
        {
            switch (controlType)
            {
               

                case ControlEnum.CLEARFILTER:
                    txtNameFilterList.Text = string.Empty;
                    txtCodeFilterList.Text = string.Empty;                 
                    CurrPK = 0;                 
                    this.EntryStatus = EntryStatus.LISTMODE;
                    break;
            }
        }
        #endregion

        #region ControlEnum

        public enum ControlEnum
        {
            PORTLIST,
            EDIT,
            NEW,
            FILLGRID,
            PORTEDIT,
            CLEARFILTER
        }

        #endregion

    }
}