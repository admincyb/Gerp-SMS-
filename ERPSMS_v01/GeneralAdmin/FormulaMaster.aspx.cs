using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Administration.Masters;
using ERPSMS_v01.Administration.Masters;
using System.Xml;
using BusinessObject.Common;
using ERP.Utilities;
using System.Data;
using BusinessLogic.Administration.Masters;
using BusinessObject;
using BusinessObject.ILibrary;
using BusinessLogic.Administration.Configurations;
using BusinessObject.CommonManagement;
using ERPSMS_v01.UserControls;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class FormulaMaster : System.Web.UI.Page
    {
        #region Variables and Properties
        #region Variables
        User currentUser;
        private ActionsEnum commonActions;
        public DataTable dtFormulaList;
        string arg;
        #endregion

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

        private int CurrPk
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = null;
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

        #endregion

        #region Page Page_PreRender
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            if (EntryStatus == EntryStatus.EDITMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(3);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
               // lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(0);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
               // lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();

            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
               // lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
              //  lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
            }
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
               // lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
        }
        #endregion

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }

        private void PageActionHandler()
        {
            try
            {
                GetFieldValues(ControlEnum.GRID);
                SetFieldValues(ControlEnum.GRID);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                int result=0;                
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else
                    if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                    {
                        commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                    }
                    else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                    {
                        commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                    }
                
                switch (commonActions)
                {
                    #region NEW
                    //To add new companydetails
                    case ActionsEnum.NEW:
                        CurrPK = 0;
                        EntryStatus = EntryStatus.NEWMODE;
                        break;
                    #endregion

                    #region EDIT
                    case ActionsEnum.EDIT:
                           arg = ((ImageButton)sender).CommandArgument;
                           int FrlPk = Convert.ToInt32(arg);
                           break;
                    #endregion

                    #region Save
                    case ActionsEnum.SAVE:
                           result = BusinessLogic.CommonManagement.CommonBL.SaveFormulaDetails();
                           if (result > 0)
                           {
                               litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                               ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                               ResetForm(ControlEnum.CLEAR);
                               EntryStatus = EntryStatus.LISTMODE;
                               GetFieldValues(ControlEnum.GRID);
                               SetFieldValues(ControlEnum.GRID);
                           }
                           break;
                    #endregion

                    #region Delete
                    case ActionsEnum.DELETE:
                           result = BusinessLogic.CommonManagement.CommonBL.DeleteFormulaDetails(CurrPk);
                           if (result > 0)
                           {
                               litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                               ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                               this.EntryStatus = EntryStatus.LISTMODE;
                               this.CurrPK = 0;
                               //ResetForm(ControlEnum.CLEAR);
                               //EntryStatus = EntryStatus.LISTMODE;
                               GetFieldValues(ControlEnum.GRID);
                               SetFieldValues(ControlEnum.GRID);
                           }
                           break;
                    #endregion

                    #region LIST,CANCEL
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        //uclPaging.CurrentPage = 0;
                        //this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        //ResetForm(ControlEnum.CLEAR);
                        //EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlEnum.GRID);
                        SetFieldValues(ControlEnum.GRID);
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

        #region Get Field Values
        private void GetFieldValues(ControlEnum type)
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                switch (type)
                {
                    case ControlEnum.GRID:
                        dtFormulaList = new DataTable();
                        dtFormulaList = BusinessLogic.CommonManagement.CommonBL.GetFormulaList(currentUser.CurrentSBUPK);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnum.GRID:
                        BindGrid();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid()
        {
            try
            {
                if (dtFormulaList != null && dtFormulaList.Rows.Count > 0)
                {
                    grdFormulaList.DataSource = dtFormulaList;
                }
                else
                {
                    grdFormulaList.DataSource = null;
                }
                grdFormulaList.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
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
                #region CLEAR
                case ControlEnum.CLEAR:
                    CurrPK = 0;
                    break;
                #endregion
            }
        }
        #endregion
    }
}