using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERPManager;
using ERPData;
using BusinessObject.Inventory;
using ERPService;
using ERPService.Inventory;
using ERPService.Sales;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;

namespace ERPSMS_v01.UserControls
{
    public partial class CheckListControl : System.Web.UI.UserControl
    {

        #region Variables 

        
        // Indicates the state as well as action
        private ActionsEnum commonActions;

        private BusinessObject.User currentUser;

        private ServiceUtility serviceUtilityObj;

        private SAL_CONTAINER_EVAL_HDR SalContainerEvalHdrObj;
        private ADM_CHECK_LIST_GROUP_MST admCheckListGroupMstObj;
        private ADM_CHECK_LIST_ITEM_MST admCheckListItemMstObj;
        private ADM_CONST_MST admConstMstObj;
        private ADM_CHECK_LIST_TRX_HDR admCheckListTrxHdrObj;
        private ADM_CHECK_LIST_TRX_DTL admCheckListTrxDtlObj;

        private List<ADM_CHECK_LIST_GROUP_MST> admCheckListGroupMstList;
        private List<CheckListItems> admCheckListItemMstList;
        private List<ADM_CONST_MST> admConstMstList;
        private List<ADM_CHECK_LIST_TRX_HDR> admCheckListTrxHdrList;
        private List<ADM_CHECK_LIST_TRX_DTL> admCheckListTrxDtlList;

        private string PageScript;

        #endregion

        #region PageMethods

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ltlTitle.Text = CLTitle;
                //CheckList Type
                if (CLType != 0)
                {
                    GetFieldValues(ControlsEnum.GROUP);
                    SetFieldValues(ControlsEnum.GROUP);

                    CLGroup = CLGroup == 0 ? Convert.ToInt32(ddlGroup.SelectedValue) : CLGroup;

                    GetFieldValues(ControlsEnum.CONTROL);
                    SetFieldValues(ControlsEnum.CONTROL);

                    if (HeaderCurrPK > 0)
                    {
                        GetFieldValues(ControlsEnum.CHECKLISTDATA);
                        SetFieldValues(ControlsEnum.CHECKLISTDATA);
                    }
                }
            }
        }
         /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                if (PageScript != string.Empty)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageScript", "$(document).ready(function(){" + PageScript + "});", true);
                }
            }
             
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }

        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            if (CLGroup != 0)
            {
                GetFieldValues(ControlsEnum.CONTROL);
                SetFieldValues(ControlsEnum.CONTROL);
            }
            
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            AdmCheckListMstService AdmCheckListServiceClient = null;
            CommonService CommonServiceClient = null;
            ContainerInspectionService ContainerInspectionServiceClient = null;
            try
            {
                ContainerInspectionServiceClient = new ContainerInspectionService();
                ContainerInspectionServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(ContainerInspectionServiceClient);
                AdmCheckListServiceClient = new AdmCheckListMstService();
                AdmCheckListServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmCheckListServiceClient);
                admCheckListGroupMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_GROUP_MST>();
                CommonServiceClient = new CommonService();
                admCheckListItemMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_ITEM_MST>();
                SalContainerEvalHdrObj = ERP.Utilities.CommonFunctions.Initilize<SAL_CONTAINER_EVAL_HDR>();
                admCheckListTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_HDR>();
                admCheckListTrxDtlObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_DTL>();
                switch (type)
                {
                    case ControlsEnum.CHECKLISTDATA:
                        admCheckListTrxHdrObj.CLH_TRX_PK = HeaderCurrPK;
                        admCheckListTrxHdrObj.CLH_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                        admCheckListTrxHdrObj.CLH_TRX_TYPE = CLTypeCode;
                        admCheckListTrxHdrObj.CLH_PK = 0;
                        admCheckListTrxHdrList = CommonServiceClient.GetCheckListTrxHdrList(admCheckListTrxHdrObj);
                        admCheckListTrxDtlList = new List<ADM_CHECK_LIST_TRX_DTL>();
                        if (admCheckListTrxHdrList != null && admCheckListTrxHdrList.Count != 0)
                        {
                            CheckListHdrCurrPK = admCheckListTrxHdrList[0].CLH_PK;
                            admCheckListTrxDtlObj.CLD_TRX_HDR = admCheckListTrxHdrList[0].CLH_PK;
                            admCheckListTrxDtlObj.CLD_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                            admCheckListTrxDtlObj.CLD_PK = 0;
                            admCheckListTrxDtlList = CommonServiceClient.GetCheckListTrxDtlList(admCheckListTrxDtlObj);
                        }
                        break;
                    case ControlsEnum.GROUP:
                        serviceUtilityObj = new ServiceUtility();
                        //serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.CGMPK : SortBy;
                        //serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortAscending : SortDirection;
                        admCheckListGroupMstObj.CGM_PK = 0;
                        admCheckListGroupMstObj.CGM_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                        admCheckListGroupMstObj.CGM_TYPE = CLType;
                        admCheckListGroupMstList = AdmCheckListServiceClient.GetCheckListGroups(admCheckListGroupMstObj, serviceUtilityObj);

                        break;
                    case ControlsEnum.CONTROL:
                        serviceUtilityObj = new ServiceUtility();
                        admCheckListItemMstObj.CHI_PK = 0;
                        admCheckListItemMstObj.CHI_GROUP = CLGroup;
                        admCheckListItemMstObj.CHI_ACTIVE = (byte)DbActiveStatus.ACTIVE;
                        admCheckListItemMstList = AdmCheckListServiceClient.GetCheckListItems(admCheckListItemMstObj, serviceUtilityObj).OrderBy(c => c.CHI_SEQUENCE).ToList();
                        break;
                   

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admCheckListGroupMstObj = null;
                AdmCheckListServiceClient = null;
                CommonServiceClient = null;
                admCheckListItemMstObj = null;
                SalContainerEvalHdrObj = null;
                ContainerInspectionServiceClient = null;
                admCheckListTrxHdrObj = null;
                admCheckListTrxDtlObj = null;
            }
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    case ControlsEnum.CHECKLISTDATA:
                        GetCheckListUIValuesFromObject();
                        break;
                    case ControlsEnum.CONTROL:
                        LoadControl();
                        break;
                    case ControlsEnum.GROUP:
                        BindGroupDropDown();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Action Handlers

        #region
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                switch (commonActions)
                {
                    #region DropDownChange
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        CLGroup  = Convert.ToInt32(ddlGroup.SelectedValue);
                        GetFieldValues(ControlsEnum.CONTROL);
                        SetFieldValues(ControlsEnum.CONTROL);
                        break;
                    #endregion

                  
                }
            }
            catch (Exception ex)
            {
                //Process Exception and show error message
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
            finally
            {
                //reset all objects
            }
        }


        #endregion





        #endregion

        #region HelperMethods
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        public ADM_CHECK_LIST_TRX_HDR SetCheckListHeaderValuesToObject(ADM_CHECK_LIST_TRX_HDR admCheckListTrxHdrObj)
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                admCheckListTrxHdrObj.CLH_PK = CheckListHdrCurrPK;
                admCheckListTrxHdrObj.CLH_CHECK_LIST_GROUP = Convert.ToInt32(ddlGroup.SelectedValue);
                admCheckListTrxHdrObj.CLH_DESC = null;
                admCheckListTrxHdrObj.CLH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                admCheckListTrxHdrObj.CLH_IS_DELETED = false;
                admCheckListTrxHdrObj.CLH_BIZUNIT = currentUser.SBUID;
                admCheckListTrxHdrObj.CLH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                admCheckListTrxHdrObj.CLH_CRTD_DT = DateTime.Now;
                admCheckListTrxHdrObj.CLH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                return admCheckListTrxHdrObj;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                admCheckListTrxHdrObj = null;
            }
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        public List<ADM_CHECK_LIST_TRX_DTL> SetCheckListDetailsValuesToObject(int HdrPk)
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                admCheckListTrxDtlList = new List<ADM_CHECK_LIST_TRX_DTL>();
                string value;
                value = string.Empty;
                GetFieldValues(ControlsEnum.CONTROL);
                HiddenField hdfPK;
                if (admCheckListItemMstList != null && admCheckListItemMstList.Count != 0)
                {
                    foreach (CheckListItems items in admCheckListItemMstList)
                    {
                        string typeid = "hdf" + items.CHI_PK + "Type";
                        HiddenField hdfType = (HiddenField)pnlControls.FindControl(typeid);
                        if (hdfType != null && !string.IsNullOrEmpty(hdfType.Value))
                        {
                            admCheckListTrxDtlObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CHECK_LIST_TRX_DTL>();

                            #region case
                            switch ((ControlTypes)(Enum.Parse(typeof(ControlTypes), hdfType.Value)))
                            {
                                #region Text
                                case ControlTypes.Text:
                                    TextBox txtCtrlId = (TextBox)pnlControls.FindControl("txt" + items.CHI_PK);
                                    if (txtCtrlId != null)
                                    {
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_ITEM = items.CHI_PK;
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_VALUE = txtCtrlId.Text.Trim();
                                        admCheckListTrxDtlObj.CLD_LOV_ITEM = null;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CHI_PK + "PK");
                                    if (hdfPK != null && !string.IsNullOrEmpty(hdfPK.Value))
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = Convert.ToInt32(hdfPK.Value);
                                    }
                                    else
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = 0;
                                    }
                                    break;
                                #endregion

                                #region Numeric
                                case ControlTypes.Numeric:
                                    TextBox txtNumCtrlId = (TextBox)pnlControls.FindControl("txt" + items.CHI_PK);
                                    if (txtNumCtrlId != null)
                                    {
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_ITEM = items.CHI_PK;
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_VALUE = txtNumCtrlId.Text.Trim();
                                        admCheckListTrxDtlObj.CLD_LOV_ITEM = null;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CHI_PK + "PK");
                                    if (hdfPK != null && !string.IsNullOrEmpty(hdfPK.Value))
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = Convert.ToInt32(hdfPK.Value);
                                    }
                                    else
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = 0;
                                    }
                                    break;
                                #endregion

                                #region HourText
                                case ControlTypes.HourText:
                                    TextBox hrtxt = (TextBox)pnlControls.FindControl("txt" + items.CHI_PK);
                                    if (hrtxt != null)
                                    {
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_ITEM = items.CHI_PK;
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_VALUE = hrtxt.Text.Trim();
                                        admCheckListTrxDtlObj.CLD_LOV_ITEM = null;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CHI_PK + "PK");
                                    if (hdfPK != null && !string.IsNullOrEmpty(hdfPK.Value))
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = Convert.ToInt32(hdfPK.Value);
                                    }
                                    else
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = 0;
                                    }
                                    break;
                                #endregion

                                #region Date
                                case ControlTypes.Date:
                                    TextBox date = (TextBox)pnlControls.FindControl("txt" + items.CHI_PK);
                                    if (date != null)
                                    {
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_ITEM = items.CHI_PK;
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_VALUE = date.Text.Trim();
                                        admCheckListTrxDtlObj.CLD_LOV_ITEM = null;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CHI_PK + "PK");
                                    if (hdfPK != null && !string.IsNullOrEmpty(hdfPK.Value))
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = Convert.ToInt32(hdfPK.Value);
                                    }
                                    else
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = 0;
                                    }
                                    break;
                                #endregion

                                #region DateTime
                                case ControlTypes.DateTime:
                                    TextBox dttxt = (TextBox)pnlControls.FindControl("txt" + items.CHI_PK);
                                    if (dttxt != null)
                                    {
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_ITEM = items.CHI_PK;
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_VALUE = dttxt.Text.Trim();
                                        admCheckListTrxDtlObj.CLD_LOV_ITEM = null;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CHI_PK + "PK");
                                    if (hdfPK != null && !string.IsNullOrEmpty(hdfPK.Value))
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = Convert.ToInt32(hdfPK.Value);
                                    }
                                    else
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = 0;
                                    }
                                    break;
                                #endregion

                                #region TimePicker
                                case ControlTypes.TimePicker:
                                    TextBox tmtxt = (TextBox)pnlControls.FindControl("txt" + items.CHI_PK);
                                    if (tmtxt != null)
                                    {
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_ITEM = items.CHI_PK;
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_VALUE = tmtxt.Text.Trim();
                                        admCheckListTrxDtlObj.CLD_LOV_ITEM = null;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CHI_PK + "PK");
                                    if (hdfPK != null && !string.IsNullOrEmpty(hdfPK.Value))
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = Convert.ToInt32(hdfPK.Value);
                                    }
                                    else
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = 0;
                                    }
                                    break;
                                #endregion

                                #region DropDown
                                case ControlTypes.DropDown:
                                    DropDownList ddlDrop = (DropDownList)pnlControls.FindControl("ddl" + items.CHI_PK);
                                    if (ddlDrop != null)
                                    {
                                        if (ddlDrop.Items.Count > 0)
                                        {
                                            admCheckListTrxDtlObj.CLD_CHECK_LIST_ITEM = items.CHI_PK;
                                            admCheckListTrxDtlObj.CLD_CHECK_LIST_VALUE = ddlDrop.SelectedItem.Text;
                                            admCheckListTrxDtlObj.CLD_LOV_ITEM = Convert.ToInt32(ddlDrop.SelectedValue);
                                        }
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CHI_PK + "PK");
                                    if (hdfPK != null && !string.IsNullOrEmpty(hdfPK.Value))
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = Convert.ToInt32(hdfPK.Value);
                                    }
                                    else
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = 0;
                                    }
                                    break;
                                #endregion

                                #region CheckBox
                                case ControlTypes.CheckBox:

                                    CheckBox chk = (CheckBox)pnlControls.FindControl("chk" + items.CHI_PK);
                                    if (chk != null)
                                    {
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_ITEM = items.CHI_PK;
                                        admCheckListTrxDtlObj.CLD_CHECK_LIST_VALUE = chk.Checked.ToString();
                                        admCheckListTrxDtlObj.CLD_LOV_ITEM = null;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CHI_PK + "PK");
                                    if (hdfPK != null && !string.IsNullOrEmpty(hdfPK.Value))
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = Convert.ToInt32(hdfPK.Value);
                                    }
                                    else
                                    {
                                        admCheckListTrxDtlObj.CLD_PK = 0;
                                    }
                                    break;
                                #endregion


                            }
                            #endregion
                            admCheckListTrxDtlObj.CLD_TRX_HDR = HdrPk;
                            admCheckListTrxDtlObj.CLD_DESC = string.Empty;
                            admCheckListTrxDtlObj.CLD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            admCheckListTrxDtlObj.CLD_MOD_BY = Convert.ToInt32(currentUser.PKUser);
                            admCheckListTrxDtlObj.CLD_MOD_DT = DateTime.Now;
                            admCheckListTrxDtlList.Add(admCheckListTrxDtlObj);
                        }

                    }

                }



                return admCheckListTrxDtlList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                admCheckListTrxDtlObj = null;
                admCheckListTrxDtlList = null;
                admCheckListItemMstList = null;
            }
        }

        /// <summary>
        /// Sets the Check List UI input controls from the object values
        /// </summary>
        /// <param name="controlType">Controls to Bind</param>
        private void GetCheckListUIValuesFromObject()
        {
            try
            {
                if (admCheckListTrxDtlList != null && admCheckListTrxDtlList.Count != 0)
                {
                    ddlGroup.Enabled = false;
                    HiddenField hdfPK;
                    foreach (ADM_CHECK_LIST_TRX_DTL items in admCheckListTrxDtlList)
                    {
                        string typeid = "hdf" + items.CLD_CHECK_LIST_ITEM + "Type";
                        HiddenField hdfType = (HiddenField)pnlControls.FindControl(typeid);
                        if (hdfType != null && !string.IsNullOrEmpty(hdfType.Value))
                        {
                            #region case
                            switch ((ControlTypes)(Enum.Parse(typeof(ControlTypes), hdfType.Value)))
                            {
                                #region Text
                                case ControlTypes.Text:
                                    TextBox txtCtrlId = (TextBox)pnlControls.FindControl("txt" + items.CLD_CHECK_LIST_ITEM);
                                    if (txtCtrlId != null)
                                    {
                                        txtCtrlId.Text = items.CLD_CHECK_LIST_VALUE;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CLD_CHECK_LIST_ITEM + "PK");
                                    if (hdfPK != null)
                                    {
                                        hdfPK.Value = items.CLD_PK.ToString();
                                    }
                                    break;
                                #endregion

                                #region Numeric
                                case ControlTypes.Numeric:
                                    TextBox txtNumCtrlId = (TextBox)pnlControls.FindControl("txt" + items.CLD_CHECK_LIST_ITEM);
                                    if (txtNumCtrlId != null)
                                    {
                                        txtNumCtrlId.Text = items.CLD_CHECK_LIST_VALUE;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CLD_CHECK_LIST_ITEM + "PK");
                                    if (hdfPK != null)
                                    {
                                        hdfPK.Value = items.CLD_PK.ToString();
                                    }
                                    break;
                                #endregion

                                #region HourText
                                case ControlTypes.HourText:
                                    TextBox hrtxt = (TextBox)pnlControls.FindControl("txt" + items.CLD_CHECK_LIST_ITEM);
                                    if (hrtxt != null)
                                    {
                                        hrtxt.Text = items.CLD_CHECK_LIST_VALUE;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CLD_CHECK_LIST_ITEM + "PK");
                                    if (hdfPK != null)
                                    {
                                        hdfPK.Value = items.CLD_PK.ToString();
                                    }
                                    break;
                                #endregion

                                #region Date
                                case ControlTypes.Date:
                                    TextBox date = (TextBox)pnlControls.FindControl("txt" + items.CLD_CHECK_LIST_ITEM);
                                    if (date != null)
                                    {
                                        date.Text = items.CLD_CHECK_LIST_VALUE;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CLD_CHECK_LIST_ITEM + "PK");
                                    if (hdfPK != null)
                                    {
                                        hdfPK.Value = items.CLD_PK.ToString();
                                    }
                                    break;
                                #endregion

                                #region DateTime
                                case ControlTypes.DateTime:
                                    TextBox dttxt = (TextBox)pnlControls.FindControl("txt" + items.CLD_CHECK_LIST_ITEM);
                                    if (dttxt != null)
                                    {
                                        dttxt.Text = items.CLD_CHECK_LIST_VALUE;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CLD_CHECK_LIST_ITEM + "PK");
                                    if (hdfPK != null)
                                    {
                                        hdfPK.Value = items.CLD_PK.ToString();
                                    }
                                    break;
                                #endregion

                                #region TimePicker
                                case ControlTypes.TimePicker:
                                    TextBox tmtxt = (TextBox)pnlControls.FindControl("txt" + items.CLD_CHECK_LIST_ITEM);
                                    if (tmtxt != null)
                                    {
                                        tmtxt.Text = items.CLD_CHECK_LIST_VALUE;
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CLD_CHECK_LIST_ITEM + "PK");
                                    if (hdfPK != null)
                                    {
                                        hdfPK.Value = items.CLD_PK.ToString();
                                    }
                                    break;
                                #endregion

                                #region DropDown
                                case ControlTypes.DropDown:
                                    DropDownList ddlDrop = (DropDownList)pnlControls.FindControl("ddl" + items.CLD_CHECK_LIST_ITEM);
                                    if (ddlDrop != null)
                                    {
                                        if (ddlDrop.Items.Count > 0)
                                        {
                                            ddlDrop.SelectedValue = items.CLD_LOV_ITEM.ToString();
                                        }
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CLD_CHECK_LIST_ITEM + "PK");
                                    if (hdfPK != null)
                                    {
                                        hdfPK.Value = items.CLD_PK.ToString();
                                    }
                                    break;
                                #endregion

                                #region CheckBox
                                case ControlTypes.CheckBox:

                                    CheckBox chk = (CheckBox)pnlControls.FindControl("chk" + items.CLD_CHECK_LIST_ITEM);
                                    if (chk != null)
                                    {
                                        chk.Checked = Convert.ToBoolean(items.CLD_CHECK_LIST_VALUE);
                                    }
                                    hdfPK = (HiddenField)pnlControls.FindControl("hdf" + items.CLD_CHECK_LIST_ITEM + "PK");
                                    if (hdfPK != null)
                                    {
                                        hdfPK.Value = items.CLD_PK.ToString();
                                    }
                                    break;
                                #endregion


                            }
                            #endregion
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Method for Dropdownlist binding
        /// </summary>
        private void BindGroupDropDown()
        {
            try
            {
                ddlGroup.Items.Clear();
                ddlGroup.SelectedIndex = -1;
                ddlGroup.SelectedValue = null;
                ddlGroup.ClearSelection(); 
                if (admCheckListGroupMstList != null && admCheckListGroupMstList.Count > 0)
                {
                    ddlGroup.DataSource = admCheckListGroupMstList;
                    ddlGroup.DataTextField = Resources.DataFieldRes.CGMName;
                    ddlGroup.DataValueField = Resources.DataFieldRes.CGMPK;
                    ddlGroup.AppendDataBoundItems = true;
                    ddlGroup.DataBind();
                    ddlGroup.TabIndex = (short)(TabIndex + 1);
                    TabIndex = TabIndex + 1;
                }
                else
                {
                    ddlGroup.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                }
                CLGroup = Convert.ToInt32(ddlGroup.SelectedValue);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method for Load the dynamic controls
        /// </summary>
        private void LoadControl()
        {

            try
            {
                pnlControls.Controls.Clear();
                if(CLGroup >0)
                {
                    ddlGroup.SelectedValue = CLGroup.ToString();
                }
                 
                BindControls();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }

        }

        /// <summary>
        /// Bind the dynamic controls 
        /// </summary>
        /// <returns></returns>  
        private void BindControls()
        {
            if (admCheckListItemMstList != null && admCheckListItemMstList.Count > 0)
            {
                AdmProductPropertiesService AdmProductPropertiesServiceClient = null;
                AdmProductPropertiesServiceClient = new AdmProductPropertiesService();
                AdmProductPropertiesServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(AdmProductPropertiesServiceClient);
                admConstMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONST_MST>();
                Table Tbl = new Table();
                Tbl.CssClass = "table-devide";
                TableRow Tr;
                TableCell Tc;
                TableCell TcCopy = new TableCell();
                HiddenField hdnType;
                HiddenField hdnPK;
                HtmlGenericControl div;
                DataTable dtFieldControls = LINQToDataTable(admCheckListItemMstList);
                PageScript = "";// " function InitComponents() {";//starting of initcomponent script
                try
                {
                    if (dtFieldControls.Rows.Count > 0)
                    {
                        hdnControlCount.Value = dtFieldControls.Rows.Count.ToString();
                        //set div style
                        //string div2colStyle = "";
                        string div2colStyle = "divcolmiddle-S2";
                        //bool flag = false;
                        //for (int i = 0, j = 1; i < dtFieldControls.Rows.Count; i++, j++)
                        //{
                        //    DataRow drFieldControls = dtFieldControls.Rows[i];
                        //    if (drFieldControls["CHI_NAME"].ToString().Length > 0 && drFieldControls["CHI_NAME"].ToString().Length <= 15)
                        //    {
                        //        div2colStyle = "div2col-S";
                        //    }
                        //    else if (drFieldControls["CHI_NAME"].ToString().Length > 15 && drFieldControls["CHI_NAME"].ToString().Length <= 25)
                        //    {
                        //        div2colStyle = "div2col-M";
                        //        flag = true;
                        //    }
                        //    else if (drFieldControls["CHI_NAME"].ToString().Length > 25)
                        //    {
                        //        div2colStyle = "div2col-M1";
                        //        break;
                        //    }
                        //}
                        //if (div2colStyle != "div2col-M1")
                        //{
                        //    div2colStyle = flag == true ? "div2col-M" : "div2col-S";
                        //}


                        //Data Table Iterate
                        Tr = new TableRow();
                        Tc = new TableCell();
                        for (int i = 0, j = 1; i < dtFieldControls.Rows.Count; i++, j++)
                        {
                            DataRow drFieldControls = dtFieldControls.Rows[i];
                           // Tr = new TableRow();

                           // Tc = new TableCell();

                            div = new HtmlGenericControl("div");
                            div.Attributes.Add("class", div2colStyle);

                            switch ((ControlTypes)(Enum.Parse(typeof(ControlTypes), drFieldControls["CTL_NAME"].ToString())))
                            {
                                #region Controls
                                #region Text
                                case ControlTypes.Text:

                                    div.Controls.Add(new Label()
                                    {
                                        ID = "lbl" + drFieldControls["CHI_PK"].ToString(),
                                        Text = drFieldControls["CHI_NAME"].ToString(),
                                        AssociatedControlID = "txt" + drFieldControls["CHI_PK"].ToString()
                                    });

                                    TextBox txt = new TextBox()
                                    {
                                        ID = "txt" + drFieldControls["CHI_PK"].ToString(),
                                        Text = "",
                                        MaxLength = 200,
                                        TabIndex = (short)(TabIndex + j)
                                    };
                                    div.Controls.Add(txt);
                                    hdnType = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "Type",
                                        Value = drFieldControls["CTL_NAME"].ToString()
                                    };
                                    div.Controls.Add(hdnType);
                                    hdnPK = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "PK",
                                        Value = "0"
                                    };
                                    div.Controls.Add(hdnPK);
                                    break;
                                #endregion
                                #region HourText
                                case ControlTypes.HourText:

                                    div.Controls.Add(new Label()
                                    {
                                        ID = "lbl" + drFieldControls["CHI_PK"].ToString(),
                                        Text = drFieldControls["CHI_NAME"].ToString(),
                                        AssociatedControlID = "txt" + drFieldControls["CHI_PK"].ToString()
                                    });

                                    TextBox hrtxt = new TextBox()
                                    {
                                        ID = "txt" + drFieldControls["CHI_PK"].ToString(),
                                        Text = "",
                                        TabIndex = (short)(TabIndex + j),
                                        CssClass ="medium",
                                        MaxLength =50
                                    };

                                    div.Controls.Add(hrtxt);
                                    hdnType = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "Type",
                                        Value = drFieldControls["CTL_NAME"].ToString()
                                    };
                                    div.Controls.Add(hdnType);
                                    hdnPK = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "PK",
                                        Value = "0"
                                    };
                                    div.Controls.Add(hdnPK);
                                    break;
                                #endregion
                                #region Date
                                case ControlTypes.Date:

                                    div.Controls.Add(new Label()
                                    {
                                        ID = "lbl" + drFieldControls["CHI_PK"].ToString(),
                                        Text = drFieldControls["CHI_NAME"].ToString(),
                                        AssociatedControlID = "txt" + drFieldControls["CHI_PK"].ToString()
                                    });

                                    TextBox date = new TextBox()
                                    {
                                        ID = "txt" + drFieldControls["CHI_PK"].ToString(),
                                        Text = "",
                                        TabIndex = (short)(TabIndex + j),
                                        CssClass = "medium",
                                        MaxLength = 50
                                    };
                                    div.Controls.Add(date);
                                    hdnType = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "Type",
                                        Value = drFieldControls["CTL_NAME"].ToString()
                                    };
                                    div.Controls.Add(hdnType);
                                    hdnPK = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "PK",
                                        Value = "0"
                                    };
                                    div.Controls.Add(hdnPK);
                                    PageScript = PageScript + CommonFunctions.GenerateDynamicScript(drFieldControls["CTL_NAME"].ToString(), date.ID, null, null, null);
                                    break;
                                #endregion
                                #region DateTime
                                case ControlTypes.DateTime:

                                    div.Controls.Add(new Label()
                                    {
                                        ID = "lbl" + drFieldControls["CHI_PK"].ToString(),
                                        Text = drFieldControls["CHI_NAME"].ToString(),
                                        AssociatedControlID = "txt" + drFieldControls["CHI_PK"].ToString()
                                    });

                                    TextBox dttxt = new TextBox()
                                    {
                                        ID = "txt" + drFieldControls["CHI_PK"].ToString(),
                                        Text = "",
                                        TabIndex = (short)(TabIndex + j),
                                        MaxLength = 50,
                                        CssClass = "medium"
                                    };
                                    dttxt.Attributes.Add("onkeydown", "return false");
                                    dttxt.Attributes.Add("onpaste", "return false");
                                    div.Controls.Add(dttxt);
                                    hdnType = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "Type",
                                        Value = drFieldControls["CTL_NAME"].ToString()
                                    };
                                    div.Controls.Add(hdnType);
                                    hdnPK = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "PK",
                                        Value = "0"
                                    };
                                    div.Controls.Add(hdnPK);
                                    PageScript = PageScript + CommonFunctions.GenerateDynamicScript(drFieldControls["CTL_NAME"].ToString(), dttxt.ID, null, null, null);
                                    break;
                                #endregion
                                #region TimePicker
                                case ControlTypes.TimePicker:

                                    div.Controls.Add(new Label()
                                    {
                                        ID = "lbl" + drFieldControls["CHI_PK"].ToString(),
                                        Text = drFieldControls["CHI_NAME"].ToString(),
                                        AssociatedControlID = "txt" + drFieldControls["CHI_PK"].ToString()
                                    });

                                    TextBox tmtxt = new TextBox()
                                    {
                                        ID = "txt" + drFieldControls["CHI_PK"].ToString(),
                                        Text = "",
                                        TabIndex = (short)(TabIndex + j),
                                        CssClass = "medium",
                                        MaxLength = 50
                                    };
                                    tmtxt.Attributes.Add("onpaste", "return false");

                                    div.Controls.Add(tmtxt);
                                    hdnType = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "Type",
                                        Value = drFieldControls["CTL_NAME"].ToString()
                                    };
                                    div.Controls.Add(hdnType);
                                    hdnPK = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "PK",
                                        Value = "0"
                                    };
                                    div.Controls.Add(hdnPK);
                                    PageScript = PageScript + CommonFunctions.GenerateDynamicScript(drFieldControls["CTL_NAME"].ToString(), tmtxt.ID, null, null, null);
                                    break;
                                #endregion
                                #region Numeric
                                case ControlTypes.Numeric:

                                    div.Controls.Add(new Label()
                                    {
                                        ID = "lbl" + drFieldControls["CHI_PK"].ToString(),
                                        Text = drFieldControls["CHI_NAME"].ToString(),
                                        AssociatedControlID = "txt" + drFieldControls["CHI_PK"].ToString()
                                    });

                                    TextBox nutxt = new TextBox()
                                    {
                                        ID = "txt" + drFieldControls["CHI_PK"].ToString(),
                                        Text = "",
                                        MaxLength = 5,
                                        TabIndex = (short)(TabIndex + j),
                                        CssClass = "medium"
                                    };
                                    nutxt.Attributes.Add("onkeypress", "return isNumberKey(event)");
                                    div.Controls.Add(nutxt);
                                    hdnType = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "Type",
                                        Value = drFieldControls["CTL_NAME"].ToString()
                                    };
                                    div.Controls.Add(hdnType);
                                    hdnPK = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "PK",
                                        Value = "0"
                                    };
                                    div.Controls.Add(hdnPK);
                                    break;
                                #endregion
                                #region DropDown
                                case ControlTypes.DropDown:

                                    div.Controls.Add(new Label()
                                    {
                                        ID = "lbl" + drFieldControls["CHI_PK"].ToString(),
                                        Text = drFieldControls["CHI_NAME"].ToString(),
                                        AssociatedControlID = "ddl" + drFieldControls["CHI_PK"].ToString()
                                    });

                                    DropDownList ddlDrop = new DropDownList()
                                    {
                                        ID = "ddl" + drFieldControls["CHI_PK"].ToString(),
                                        TabIndex = (short)(TabIndex + j)
                                    };

                                    serviceUtilityObj = new ServiceUtility();
                                    admConstMstObj.CON_PK = 0;
                                    admConstMstObj.CON_GROUP = Convert.ToInt32(drFieldControls["CHI_CONST_GROUP"].ToString());
                                    admConstMstList = AdmProductPropertiesServiceClient.GetGeneralProperties(admConstMstObj, serviceUtilityObj);
                                    if (admConstMstList != null && admConstMstList.Count > 0)
                                    {
                                        admConstMstList = admConstMstList.Where(f => f.CON_ACTIVE == 1).ToList();  
                                        ddlDrop.DataTextField = "CON_NAME";
                                        ddlDrop.DataValueField = "CON_PK";
                                        ddlDrop.DataSource = admConstMstList;
                                        ddlDrop.DataBind();
                                    }

                                    div.Controls.Add(ddlDrop);
                                    hdnType = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "Type",
                                        Value = drFieldControls["CTL_NAME"].ToString()
                                    };
                                    div.Controls.Add(hdnType);
                                    hdnPK = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "PK",
                                        Value = "0"
                                    };
                                    div.Controls.Add(hdnPK);

                                    break;
                                #endregion
                                #region CheckBox
                                case ControlTypes.CheckBox:
                                    CheckBox chk = new CheckBox()
                                    {
                                        ID = "chk" + drFieldControls["CHI_PK"].ToString(),
                                        Text = drFieldControls["CHI_NAME"].ToString(),
                                        TextAlign = TextAlign.Left,
                                        TabIndex = (short)(TabIndex + j)
                                    };
                                    div.Controls.Add(chk);
                                    hdnType = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "Type",
                                        Value = drFieldControls["CTL_NAME"].ToString()
                                    };
                                    div.Controls.Add(hdnType);
                                    hdnPK = new HiddenField()
                                    {
                                        ID = "hdf" + drFieldControls["CHI_PK"].ToString() + "PK",
                                        Value = "0"
                                    };
                                    div.Controls.Add(hdnPK);
                                    break;
                                #endregion
                                #endregion
                            }

                            Tc.Controls.Add(div);

                            //if (j % 2 == 0)
                            //{
                            //    Tc.Controls.Add(div);
                            //    Tr.Controls.Add(TcCopy);
                            //    Tr.Controls.Add(Tc);
                            //    Tbl.Controls.Add(Tr);
                            //    TcCopy = null;
                            //}
                            //else if (j % 2 != 0 && j == dtFieldControls.Rows.Count)
                            //{
                            //    Tc.Controls.Add(div);
                            //    Tr.Controls.Add(Tc);
                            //    Tbl.Controls.Add(Tr);
                            //}
                            //else
                            //{
                            //    TcCopy = new TableCell();
                            //    TcCopy.Controls.Add(div);
                            //}

                        }
                        //
                        Tr.Controls.Add(Tc);
                        Tbl.Controls.Add(Tr);
                        //
                        pnlControls.Controls.Add(Tbl);
                    }
                    //PageScript = PageScript +"}";//End of initcomponents
                    //Register page script
                    //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "pagescript", PageScript, true);
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), "pagescript", PageScript, true);

                }
                catch
                {
                    throw;
                }
                finally
                {
                    admConstMstObj = null;
                    AdmProductPropertiesServiceClient = null;
                    admConstMstList = null;
                }
            }
        }

        /// <summary>
        /// Method for creating datatable from list
        /// </summary>
        /// <returns></returns>      
        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others          will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
        #endregion

        #region Get/Set Properties
        public string CLTitle
        {
            get { return ViewState["CLTtitle"] == null ? string.Empty : ViewState["CLTtitle"].ToString();  }
            set { ViewState["CLTtitle"] = value; }
        }
        public int CLType
        {
            get { return ViewState["CLType"] == null ? 0 : Convert.ToInt32(ViewState["CLType"]); }
            set { ViewState["CLType"] = value; }
        }
        public string CLTypeCode
        {
            get { return ViewState["CLTypeCode"] == null ? string.Empty  : ViewState["CLTypeCode"].ToString(); }
            set { ViewState["CLTypeCode"] = value; }
        }
        public int CLGroup
        {
            get { return Session["CLGroup"] == null ? 0 : Convert.ToInt32(Session["CLGroup"]); }
            set { Session["CLGroup"] = value; }
        }
        public int TabIndex
        {
            get { return Session["CLTab"] == null ? 0 : Convert.ToInt32(Session["CLTab"]); }
            set { Session["CLTab"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int HeaderCurrPK
        {
            get
            {
                return ViewState["HeaderCurrPK"] == null ? 0 : (int)ViewState["HeaderCurrPK"];
            }
            set
            {
                ViewState["HeaderCurrPK"] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CheckListHdrCurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CheckListGroupPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CheckListGroupPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CheckListGroupPK] = value;
            }
        }
      
        #endregion

        #region Enum
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            TYPE,
            CONTROL,
            GROUP,
            CHECKLISTDATA
        }

        /// <summary>
        /// Define Controltype Enum
        /// </summary>
        enum ControlTypes
        {
            Page,
            Label,
            Text,
            DropDown,
            DateTime,
            Numeric,
            Button,
            Spacer,
            GridView,
            CheckBox,
            TextArea,
            TimePicker,
            Header,
            Table,
            Iframe,
            HiddenField,
            HourText,
            FileUpload,
            Date,
            LinkButton,
            ImageButton,
            ValidationSummary,
            DateRange

        }

        #endregion
    }
}