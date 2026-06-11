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
using System.Xml;
using BusinessObject.Administration.Masters; 

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class AccountMaping : ERP.Store.UI.MyBasePage  
    {
        #region Variables and Properties
        #region Properties


        /// <summary>
        /// Current PK
        /// </summary>
        private long CurrPK
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }

        /// <summary>
        /// To maintain the LastModifiedTime in viewstate
        /// </summary>
        private DateTime LastModifiedTime
        {
            get
            {
                return this.ViewState[ViewstateStrings.LastModifiedTime] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.LastModifiedTime];
            }
            set
            {
                this.ViewState[ViewstateStrings.LastModifiedTime] = value;
            }
        }

        #endregion
        private DataTable dtMappingDetails;
        private DataTable dtPageData;
        private DataTable dtMappingAccount;
        private DataTable dtMappingData;
        XmlDocument xmlDoc;

        private int PK = 0;
        private int SubType = 0;
        private string Selectfield;
        private string Selectdate;
        DropDownList ddlMappedAccount;
        BusinessObject.User currentUser;
        int result;
        private SOAccountMappingHeader ObjSOAccountMappingHeader;
        // List variables for binding details to controls
        private ActionsEnum commonActions;
        #endregion 

        #region Page Level Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param> 
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        } 
        #endregion 

        #region Get Field Values
        private void GetFieldValues(ControlEnum type)
        {
            switch (type)
            {  
                case ControlEnum.MAPPINGTYPE:
                    dtPageData = BusinessLogic.Administration.Masters.AccountMapingBL.GetMappingType(PK, Convert.ToInt32(DbActiveStatus.ACTIVE));
                    break; 
                case ControlEnum.FILLGRID:
                    dtMappingDetails = new DataTable();
                    dtMappingDetails = BusinessLogic.Administration.Masters.AccountMapingBL.GetMappingDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), Convert.ToInt32(ddlMappingType.SelectedValue));
                    break;
                case ControlEnum.MAPPINGACCOUNT:
                    dtMappingAccount = new DataTable(); 
                    PK = Convert.ToInt32(ddlMappingType.SelectedValue);
                    dtMappingAccount = BusinessLogic.Administration.Masters.AccountMapingBL.GetMappingType(PK, Convert.ToInt32(DbActiveStatus.HASPK));
                    Selectfield = GTIService.Constants.Administration.Masters.AccountMaping.Fields.GRIDFIELD;
                    Selectdate = GTIService.Constants.Administration.Masters.AccountMaping.Fields.Last_mod_date;
                    SubType = Convert.ToInt32(dtMappingAccount.Rows[0][Selectfield]);
                    LastModifiedTime = Convert.ToDateTime(dtMappingAccount.Rows[0][Selectdate]);
                    break; 
                case ControlEnum.MAPPINGADATA:
                    dtMappingData = new DataTable();
                    dtMappingData = BusinessLogic.Administration.Masters.AccountMapingBL.GetMappingTypeData(0, Convert.ToInt32(DbActiveStatus.ACTIVE), SubType, 0);
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
                    case ControlEnum.MAPPINGTYPE:
                        BindDropDown(controlType);
                        break;
                    case ControlEnum.FILLGRID:
                        BindGrid(controlType);
                        break;
                    case ControlEnum.MAPPINGACCOUNT :
                        BindDropDown(controlType);
                        break;
                    case ControlEnum.MAPPINGADATA :
                        BindDropDown(controlType);
                        break; 
                    default:
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
                    GetFieldValues(ControlEnum.MAPPINGTYPE);
                    SetFieldValues(ControlEnum.MAPPINGTYPE); 
                    GetFieldValues(ControlEnum.FILLGRID);
                    SetFieldValues(ControlEnum.FILLGRID); 
                }
            }
            catch
            {
            }
        }
        #endregion  

        #region Get Field Values 
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlEnum controlType)
        {
            try
            {
                Object retObject;
                retObject = null;
                int rowID;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity)); 
                switch (controlType)
                {
                    case ControlEnum.SOMAPPINGHEAD: 
                            ObjSOAccountMappingHeader = new SOAccountMappingHeader(); 
                             ObjSOAccountMappingHeader.CKH_PK = Convert.ToInt32(ddlMappingType.SelectedValue);
                             ObjSOAccountMappingHeader.USER_PK = currentUser.PKUser ;
                             ObjSOAccountMappingHeader.LAST_MOD_DT = LastModifiedTime; 
                             ObjSOAccountMappingHeader.Detail = new List<SOAccountMappingDetails>();
                             List<SOAccountMappingDetails> detailsList = new List<SOAccountMappingDetails>();
                             SOAccountMappingDetails objDetail; 
                               rowID = 0;
                             foreach (GridViewRow grdrow in grdAccoutMappingList.Rows)
                             {
                                        objDetail = new SOAccountMappingDetails();
                                  
                                         HiddenField hdfCKD_PK = (HiddenField)grdAccoutMappingList.Rows[rowID].FindControl("hdfCKD_PK"); 
                                         HiddenField hdfFROM_ACCOUNT = (HiddenField)grdAccoutMappingList.Rows[rowID].FindControl("hdfFROM_ACCOUNT");
                                         DropDownList ddlMappedAccount = (DropDownList)grdAccoutMappingList.Rows[rowID].FindControl("ddlMappedAccount");
                                             
                                             if (Convert.ToInt32(ddlMappedAccount.SelectedValue) > 0 )
                                          { 
                                            
                                             if (!DBNull.Value.Equals(hdfCKD_PK.Value.ToString() ))
                                             {
                                                 objDetail.CID_PK = hdfCKD_PK.Value;
                                             }
                                              
                                             else
                                             {
                                                 objDetail.CID_PK = null;
                                             }  
                                         objDetail.CKD_FROM_ACCOUNT = Convert.ToInt32(hdfFROM_ACCOUNT.Value);
                                         objDetail.CKD_TO_ACCOUNT = Convert.ToInt32(ddlMappedAccount.SelectedValue);
                                         objDetail.CKD_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                                         detailsList.Add(objDetail);  
                                         }
                                 rowID++;
                             }
                             ObjSOAccountMappingHeader.Detail = detailsList;
                         retObject = ObjSOAccountMappingHeader; 
                        break; 
                }
                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
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
                if (dtMappingDetails != null && dtMappingDetails.Rows.Count > 0)
                    grdAccoutMappingList.DataSource = dtMappingDetails;
                   // LastModifiedTime= Date.Now .ToString();
                else
                    grdAccoutMappingList.DataSource = null;
                    grdAccoutMappingList.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        /// <summary>
        /// Bind DropDown  as per type 
        /// </summary>
        /// <param name="drpName"></param>
        private void BindDropDown(ControlEnum controlType)
        {
            switch (controlType)
            {
                // Fill Shift Details to DropDown
                case ControlEnum.MAPPINGTYPE:
                    if (dtPageData != null)
                    {
                        ddlMappingType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CKH_TYPE");
                        ddlMappingType.DataTextField = "CKH_TYPE";
                        ddlMappingType.DataValueField = "CKH_PK";
                        ddlMappingType.DataBind();
                    }
                    ddlMappingType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));

                    break;
                case ControlEnum.MAPPINGADATA :
                    if (dtMappingData != null)
                    {
                        ddlMappedAccount.DataSource = CommonFunctions.HtmlDecodeDataTable(dtMappingData, "COA_NAME");
                        ddlMappedAccount.DataTextField = "COA_NAME";
                        ddlMappedAccount.DataValueField = "COA_PK";
                        ddlMappedAccount.DataBind();  
                    }
                    ddlMappedAccount.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL)); 
                    break;  
                default:
                    break;
            }
        } 
        #endregion 

        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlMappingType")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                switch (commonActions)
                {
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        GetFieldValues(ControlEnum.FILLGRID);
                        SetFieldValues(ControlEnum.FILLGRID);
                        break; 
                    #endregion
                         
                    #region CANCEL
                    case ActionsEnum.CANCEL: 
                        GetFieldValues(ControlEnum.FILLGRID);
                        SetFieldValues(ControlEnum.FILLGRID); 
                        break;
                    #endregion
                         
                    #region SAVE
                    case ActionsEnum.SAVE:

                        if (Convert.ToInt32(ddlMappingType.SelectedValue) < 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("MsgDropdownSelect").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true); 
                            break;
                        } 
                        ObjSOAccountMappingHeader = new SOAccountMappingHeader();
                        ObjSOAccountMappingHeader = (SOAccountMappingHeader)SetUIValuesToObject(ControlEnum.SOMAPPINGHEAD);
                        if (ObjSOAccountMappingHeader != null && ObjSOAccountMappingHeader.Detail != null)
                        {
                            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(ObjSOAccountMappingHeader);
                            result = BusinessLogic.Administration.Masters.AccountMapingBL.AccountMappingHeader(xmlDoc.InnerXml);
                        }
                        if (result > 0)
                        { 
                            GetFieldValues(ControlEnum.FILLGRID);
                            SetFieldValues(ControlEnum.FILLGRID); 
                            litErrorMsg.Text = GetLocalResourceObject("MsgSaveSuccess").ToString(); 
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
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
     
        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (((GridView)sender).ID == "grdAccoutMappingList")
                    {
                        ddlMappedAccount= e.Row.FindControl("ddlMappedAccount") as DropDownList;
                        if (e.Row.RowIndex == 0)
                        {
                        GetFieldValues(ControlEnum.MAPPINGACCOUNT);
                        GetFieldValues(ControlEnum.MAPPINGADATA); 
                        } 
                        SetFieldValues(ControlEnum.MAPPINGADATA); 
                        HiddenField hdfTo_ACCOUNT = e.Row.FindControl("hdfBaseAccountPk") as HiddenField;  
                        if (ddlMappedAccount.Items.FindByValue(hdfTo_ACCOUNT.Value) != null)
                        {
                            ddlMappedAccount.Items.FindByValue(hdfTo_ACCOUNT.Value).Selected = true;
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion 
        #endregion 

        #region ControlEnum 
        public enum ControlEnum
        {
            MAPPINGTYPE,
            MAPPINGACCOUNT,
            FILLGRID,
            MAPPINGADATA,
            SAVE,
            CANCEL,
            SOMAPPINGHEAD 
        } 
        #endregion
    }
}


