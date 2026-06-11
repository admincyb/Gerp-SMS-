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
using BusinessLogic.Administration.Configurations;

using BusinessObject.Administration.Configurations;


namespace ERPSMS_v01.Administration.Configurations
{
    public partial class CompanyList : ERP.Store.UI.MyBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #region Variables and Properties
        #region Properties
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

        private DataTable dtCompanyList;
        // List variables for binding details to controls
        private ActionsEnum commonActions;
        private BusinessObject.User currentUser;
        #endregion

        #region Get Field Values
        private void GetFieldValues(ControlEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            switch (type)
            {
                case ControlEnum.COMPANYLIST:
                    dtCompanyList = new DataTable();
                    dtCompanyList = BusinessLogic.Administration.Configurations.CompanyBL.GetCompanyDetails(0, Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID);
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
            try
            {
                if (!IsPostBack)
                {
                    GetFieldValues(ControlEnum.COMPANYLIST);
                    SetFieldValues(ControlEnum.FILLGRID);
                }
            }
            catch
            {
            }
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
                if (dtCompanyList != null && dtCompanyList.Rows.Count > 0)
                    grdCompanyList.DataSource = dtCompanyList;
                else
                    grdCompanyList.DataSource = null;
                grdCompanyList.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                        foreach (GridViewRow grdrow in grdCompanyList.Rows)
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
                            Response.Redirect(Resources.PageURL.CompanyDetails+"?cmpPK="+CurrPK, true);                           
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
                        Response.Redirect(Resources.PageURL.CompanyDetails, true); 
                        break;
                    #endregion

                    #region DELETE
                    //To delete companydetails
                    case ActionsEnum.DELETE:
                        foreach (GridViewRow grdrow in grdCompanyList.Rows)
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
                            string routeURL = Resources.PageURL.CompanyList.ToString();
                            result=CompanyBL.DeleteCompany(CurrPK);

                            DbDeleteStatus deleteStatus = (DbDeleteStatus)result;
                            switch (deleteStatus)
                            {
                                case DbDeleteStatus.DELETED://If deletion is success
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CompanyDetails);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                                case DbDeleteStatus.REFERRED://If referred to another page
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Ref;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CompanyDetails);

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + routeURL + "');", true);
                                    break;
                                case DbDeleteStatus.SQLERROR://Sql error
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.CompanyDetails);

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

        #region ControlEnum

        public enum ControlEnum
        {
            COMPANYLIST,
            EDIT,
            NEW,
            FILLGRID
        }

        #endregion
    }
}