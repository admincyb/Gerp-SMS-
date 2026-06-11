using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject.Common;
using ERPSMS_v01.UserControls;
using CustomControls;
using System.Data;
using BusinessObject.PurchaseOrderManagement;
using System.Linq;
using BusinessLogic;
using System.Xml;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.PurchaseOrderManagement
{
    public partial class RFQSearch : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        /// <summary>
        /// 
        /// </summary>
        private List<RFQPurchaseRequest> PRSearchResult
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.PRSearchResult] == null ? null : (List<RFQPurchaseRequest>)Session[ERP.Utilities.SessionStrings.PRSearchResult];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.PRSearchResult] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private List<RFQPurchaseRequest> PRSelected
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.PRSelected] == null ? null : (List<RFQPurchaseRequest>)Session[ERP.Utilities.SessionStrings.PRSelected];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.PRSelected] = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private RFQParameters RFQParameters
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.RFQParameters] == null ? null : (RFQParameters)Session[ERP.Utilities.SessionStrings.RFQParameters];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.RFQParameters] = value;
            }
        }
        #region Properties
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        //page related Entity Object
        private DataSet dsPageData;
        private DataTable dtPageData;
        private string xmlParameter;
        private BusinessObject.User objUser;
        private int selectedItem;

        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    PRSearchResult = null;
                    PRSelected = null;
                    RFQParameters = null;
                    Session[ERP.Utilities.SessionStrings.RFQItemDetailParameters] = null;
                    Session[ERP.Utilities.SessionStrings.RFQPK] = null;
                    Session[ERP.Utilities.SessionStrings.PRSearchResult] = null;
                    Session[ERP.Utilities.SessionStrings.PRSelected] = null;
                    Session[ERP.Utilities.SessionStrings.RFQVendor] = null;

                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = "SerialNo";
                    grdPRSearch.DataKeyNames = datakeyarray;
                    txtItemReqDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);

                    grdPRSelected.DataKeyNames = datakeyarray;

                    //FromDate.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString(Resources.ErpRes.DateFormatShort);
                    //hdfFrmDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString();
                    //ToDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);
                    //hdfToDate.Value = DateTime.Now.ToString();
                    // Qty Decimal setting for Purchase Start
                    hdfDecimalFormat.Value = "#0.";
                    int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                    for (int i = 0; i < NoDecimalDigitsP2P; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                    }
                    // End  Qty Decimal setting for Purchase 

                    GetFieldValues(ControlsEnum.DEFAULT);
                    PRSearchResult = new List<RFQPurchaseRequest>();
                    if (dsPageData != null && dsPageData.Tables.Count > 1)
                    {
                        int i = 1;
                        foreach (DataRow row in dsPageData.Tables[1].Rows)
                        {
                            PRSearchResult.Add(new RFQPurchaseRequest()
                            {
                                SerialNo = i,
                                PRDetailPK = row.Field<int>(Resources.DataFieldRes.PRDetailPK),
                                ItemDesc = row.Field<string>(Resources.DataFieldRes.ItemDesc),
                                ItemCode = row.Field<string>(Resources.DataFieldRes.ItemCode),
                                ItemName = row.Field<string>(Resources.DataFieldRes.ItemName),
                                ItemText = row.Field<string>(Resources.DataFieldRes.ItemText),
                                PRDetailBalanceQty = row.Field<decimal>(Resources.DataFieldRes.PRDetailBalanceQty),
                                PRDetailDate = row.Field<string>(Resources.DataFieldRes.PRDetailDate),
                                PRDetailSpec = row.Field<string>(Resources.DataFieldRes.PRDetailSpec),
                                PRDetailItem = row.Field<int>(Resources.DataFieldRes.PRDetailItem),
                                PRDetailReqDate = row.Field<string>(Resources.DataFieldRes.PRDetailReqDate),
                                PRDetailUOM = row.Field<int>(Resources.DataFieldRes.PRDetailUOM),
                                PRDetailUOMText = row.Field<string>(Resources.DataFieldRes.PRDetailUOMText),
                                PRHeaderNo = row.Field<string>(Resources.DataFieldRes.PRHeaderNo),
                                PRDeptText = row.Field<string>(Resources.DataFieldRes.PRHDEPTTEXT),
                            });
                            i++;
                        }
                    }
                    SetFieldValues(ControlsEnum.DEFAULT);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region PageLevel Events
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
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {

                    case ControlsEnum.DEFAULT:
                        dsPageData = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetPurchaseRequestList(
                            new BusinessObject.GridPrams()
                            {
                                SearchValue = txtSearchValue.Text,
                                SearchBy = ddlSearchType.SelectedValue,
                                FromDate = FromDate.Text,
                                ToDate = ToDate.Text
                            }, objUser, 0);
                        break;
                    case ControlsEnum.VENDORS:
                        dsPageData = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetVendorListbyPR(xmlParameter);
                        break;
                    case ControlsEnum.ITEMRATES:
                        dsPageData = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetItemRates(selectedItem, 0);
                        break;
                    case ControlsEnum.ITEM:
                        dtPageData = BusinessLogic.StoreManagement.StockTransferBL.GetItemDetails(selectedItem, 0);
                        break;
                }
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

                    case ControlsEnum.DEFAULT:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.SELECTEDPR:
                        BindGrid(controlType);
                        divSelectedItems.Visible = PRSelected != null && PRSelected.Count > 0;
                        break;
                    case ControlsEnum.VENDORS:
                        BindCheckBoxList(controlType);
                        divSelectedVendors.Visible = dsPageData != null && dsPageData.Tables.Count > 0;
                        break;
                    case ControlsEnum.ITEMRATES:
                        BindGrid(controlType);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
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

        #region -- For Buttons ---
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            int selectedPRPK;
            CheckBox chkPR;
            List<int> selectedPRs;
            IEnumerable<RFQPurchaseRequest> selectedPRDetails;
            RFQParameters vendorParameters;
            RFQPurchaseRequest prDetail;
            string selectedPRDPK;
            string selectedItemPK;
            int itemPk;
            if (sender.GetType().IsEquivalentTo(typeof(Button)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
            {
                commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
            }
            else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
            {
                commonActions = ActionsEnum.SHOWDETAILS;
            }
            switch (commonActions)
            {
                case ActionsEnum.SEARCH:
                    GetFieldValues(ControlsEnum.DEFAULT);
                    PRSearchResult = new List<RFQPurchaseRequest>();
                    if (dsPageData != null && dsPageData.Tables.Count > 1)
                    {
                        int i = 1;
                        foreach (DataRow row in dsPageData.Tables[1].Rows)
                        {
                            PRSearchResult.Add(new RFQPurchaseRequest()
                            {
                                SerialNo = i,
                                PRDetailPK = row.Field<int>(Resources.DataFieldRes.PRDetailPK),
                                ItemDesc = row.Field<string>(Resources.DataFieldRes.ItemDesc),
                                ItemCode = row.Field<string>(Resources.DataFieldRes.ItemCode),
                                ItemName = row.Field<string>(Resources.DataFieldRes.ItemName),
                                ItemText = row.Field<string>(Resources.DataFieldRes.ItemText),
                                PRDetailBalanceQty = row.Field<decimal>(Resources.DataFieldRes.PRDetailBalanceQty),
                                PRDetailDate = row.Field<string>(Resources.DataFieldRes.PRDetailDate),
                                PRDetailSpec = row.Field<string>(Resources.DataFieldRes.PRDetailSpec),
                                PRDetailItem = row.Field<int>(Resources.DataFieldRes.PRDetailItem),
                                PRDetailReqDate = row.Field<string>(Resources.DataFieldRes.PRDetailReqDate),
                                PRDetailUOM = row.Field<int>(Resources.DataFieldRes.PRDetailUOM),
                                PRDetailUOMText = row.Field<string>(Resources.DataFieldRes.PRDetailUOMText),
                                PRHeaderNo = row.Field<string>(Resources.DataFieldRes.PRHeaderNo),
                                PRDeptText = row.Field<string>(Resources.DataFieldRes.PRHDEPTTEXT),
                            });
                            i++;
                        }
                    }
                    SetFieldValues(ControlsEnum.DEFAULT);
                    PRSelected = new List<RFQPurchaseRequest>();
                    SetFieldValues(ControlsEnum.SELECTEDPR);
                    chlVendors.Items.Clear();
                    break;
                case ActionsEnum.ADDRFQPR:
                    selectedPRs = new List<int>();
                    foreach (GridViewRow gvr in grdPRSearch.Rows)
                    {
                        if (gvr.RowType == DataControlRowType.DataRow)
                        {
                            chkPR = gvr.FindControl("chkSelection") as CheckBox;
                            if (chkPR != null && chkPR.Checked)
                            {
                                selectedPRs.Add(Convert.ToInt32(grdPRSearch.DataKeys[gvr.RowIndex][0].ToString()));
                            }
                        }
                    }
                    if (selectedPRs.Count > 0)
                    {
                        selectedPRDetails = PRSearchResult.AsEnumerable().Where(row => selectedPRs.Contains(row.SerialNo));

                        if (hdfIsValidItem.Value == CommonConstants.SELECT_VALUE_ONE || PRSelected == null ||
                            PRSelected.Where(row => selectedPRDetails.Where(itm => itm.PRDetailItem == row.PRDetailItem).Count() > 0).Count() == 0)
                        {
                            hdfIsValidItem.Value = CommonConstants.SELECT_VALUE_ZERO;
                            if (PRSelected != null && PRSelected.Count > 0)
                            {
                                PRSelected = PRSelected.Union(selectedPRDetails).ToList();
                            }
                            else
                                PRSelected = selectedPRDetails.ToList();
                            PRSearchResult = PRSearchResult.AsEnumerable().Where(row => !selectedPRs.Contains(row.SerialNo)).ToList();
                            SetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.SELECTEDPR);
                        }
                        else
                        {
                            hdfIsValidItem.Value = CommonConstants.SELECT_VALUE_ONE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Item_Repeat").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowItemConfirm1", "ShowItemConfirm('" + btnAddPR.ClientID + "', '" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                    }
                    BindVendors();
                    break;
                case ActionsEnum.ADDITEM:
                    selectedPRPK = Convert.ToInt32(grdPRSearch.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                    if (selectedPRPK > 0)
                    {
                        selectedPRDetails = PRSearchResult.Where(row => selectedPRPK == row.SerialNo);
                        if (hdfIsValidItem.Value == CommonConstants.SELECT_VALUE_ONE || PRSelected == null ||
                            PRSelected.Where(row => row.PRDetailItem == selectedPRDetails.First().PRDetailItem).Count() == 0)
                        {
                            hdfIsValidItem.Value = CommonConstants.SELECT_VALUE_ZERO;
                            if (PRSelected != null && PRSelected.Count > 0)
                            {
                                PRSelected = PRSelected.Union(selectedPRDetails).ToList();
                            }
                            else
                                PRSelected = selectedPRDetails.ToList();
                            PRSearchResult = PRSearchResult.Where(row => selectedPRPK != row.SerialNo).ToList();
                            SetFieldValues(ControlsEnum.DEFAULT);
                            SetFieldValues(ControlsEnum.SELECTEDPR);
                            BindVendors();
                        }
                        else
                        {
                            hdfIsValidItem.Value = CommonConstants.SELECT_VALUE_ONE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Item_Repeat").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowItemConfirm1", "ShowItemConfirm('" + ((ImageButton)sender).ClientID + "', '" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);

                        }
                    }
                    break;
                case ActionsEnum.REMOVEITEM:
                    selectedPRPK = Convert.ToInt32(grdPRSelected.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                    selectedPRDPK = ((((ImageButton)sender).Parent.Parent as GridViewRow).FindControl("hdfPRDtlPK") as HiddenField).Value;
                    if (!string.IsNullOrEmpty(selectedPRDPK))
                        selectedItem = Convert.ToInt32(selectedPRDPK);
                    if (selectedPRPK > 0)
                    {
                        selectedPRDetails = PRSelected.Where(row => selectedPRPK == row.SerialNo);
                        if (selectedItem > 0)
                        {
                            if (PRSearchResult != null && PRSearchResult.Count > 0)
                            {
                                PRSearchResult = PRSearchResult.Union(selectedPRDetails).ToList();
                            }
                            else
                                PRSearchResult = selectedPRDetails.ToList();
                        }
                        PRSelected = PRSelected.Where(row => selectedPRPK != row.SerialNo).ToList();
                        SetFieldValues(ControlsEnum.DEFAULT);
                        SetFieldValues(ControlsEnum.SELECTEDPR);
                        BindVendors();
                    }
                    break;
                case ActionsEnum.ITEMSELECTED:
                    if (hdfItem.Value != string.Empty && hdfItem.Value != CommonConstants.SELECT_VALUE_ZERO)
                    {
                        selectedItem = Convert.ToInt32(hdfItem.Value);
                        GetFieldValues(ControlsEnum.ITEM);
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            hdfTtemCode.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0][Resources.DataFieldRes.ItemCode].ToString());
                            hdfTtemName.Value = HttpUtility.HtmlDecode(dtPageData.Rows[0][Resources.DataFieldRes.ItemName].ToString());
                            //txtItemDesc.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0][Resources.DataFieldRes.ItemDesc].ToString());
                            txtItemUOM.Text = HttpUtility.HtmlDecode(dtPageData.Rows[0][Resources.DataFieldRes.UomCode].ToString());
                            hdfItemUOM.Value = dtPageData.Rows[0][Resources.DataFieldRes.ItemUOM].ToString();
                        }
                        else
                        {
                            //txtItemDesc.Text = string.Empty;
                            txtItemUOM.Text = string.Empty;
                            hdfItemUOM.Value = string.Empty;
                            hdfTtemCode.Value = string.Empty;
                            hdfTtemName.Value = string.Empty;
                        }
                    }
                    else
                    {
                        hdfTtemCode.Value = string.Empty;
                        hdfTtemName.Value = string.Empty;
                        txtItemUOM.Text = string.Empty;
                        hdfItemUOM.Value = string.Empty;
                    }
                    break;
                case ActionsEnum.ADDLITEMNEW:
                    //validate Page
                    if (!IsValid)
                    {
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                    }
                    else//valid
                    {
                        itemPk = Convert.ToInt32(hdfItem.Value);
                        if (PRSelected == null)
                            PRSelected = new List<RFQPurchaseRequest>();
                        //if (hdfIsValidItem.Value == CommonConstants.SELECT_VALUE_ZERO  || PRSelected == null ||
                        //    PRSelected.Where(row => row.PRDetailItem == itemPk).Count() == 0)
                        {
                            hdfIsValidItem.Value = CommonConstants.SELECT_VALUE_ZERO;
                            prDetail = new RFQPurchaseRequest()
                            {
                                SerialNo = PRSelected.Count == 0 ? 1 : (PRSelected.Max(itm => itm.SerialNo) + 1),
                                ItemText = HttpUtility.HtmlEncode(txtItem.Text),
                                PRDetailItem = itemPk,
                                ItemCode = HttpUtility.HtmlEncode(hdfTtemCode.Value),
                                ItemName = HttpUtility.HtmlEncode(hdfTtemName.Value),
                                //ItemDesc = HttpUtility.HtmlEncode(txtItemDesc.Text),
                                PRDetailSpec = HttpUtility.HtmlEncode(txtItemSpec.Text),
                                PRDetailBalanceQty = Convert.ToDecimal(txtItemQty.Text),
                                PRDetailUOMText = HttpUtility.HtmlEncode(txtItemUOM.Text),
                                PRDetailUOM = Convert.ToInt32(hdfItemUOM.Value),
                                PRDetailReqDate = txtItemReqDate.Text
                            };
                            PRSelected.Add(prDetail);
                            SetFieldValues(ControlsEnum.SELECTEDPR);
                            ResetForm();
                            BindVendors();
                        }
                        //else
                        //{
                        //    hdfIsValidItem.Value = CommonConstants.SELECT_VALUE_ONE;
                        //    litErrorMsg.Text = GetLocalResourceObject("Msg_Item_Repeat").ToString();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowItemConfirm1", "ShowItemConfirm('" + imbAddItem.ClientID + "', '" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        //}
                    }
                    break;
                case ActionsEnum.ADDLITEM:
                    //validate Page
                    if (!IsValid)
                    {
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                    }
                    else//valid
                    {
                        itemPk = Convert.ToInt32(hdfItem.Value);
                        if (PRSelected == null)
                            PRSelected = new List<RFQPurchaseRequest>();
                        if (hdfIsValidItem.Value == CommonConstants.SELECT_VALUE_ONE || PRSelected == null ||
                            PRSelected.Where(row => row.PRDetailItem == itemPk).Count() == 0)
                        {
                            hdfIsValidItem.Value = CommonConstants.SELECT_VALUE_ZERO;
                            prDetail = new RFQPurchaseRequest()
                            {
                                SerialNo = PRSelected.Count == 0 ? 1 : (PRSelected.Max(itm => itm.SerialNo) + 1),
                                ItemText = HttpUtility.HtmlEncode(txtItem.Text),
                                PRDetailItem = itemPk,
                                ItemCode = HttpUtility.HtmlEncode(hdfTtemCode.Value),
                                ItemName = HttpUtility.HtmlEncode(hdfTtemName.Value),
                                //ItemDesc = HttpUtility.HtmlEncode(txtItemDesc.Text),
                                PRDetailSpec = HttpUtility.HtmlEncode(txtItemSpec.Text),
                                PRDetailBalanceQty = Convert.ToDecimal(txtItemQty.Text),
                                PRDetailUOMText = HttpUtility.HtmlEncode(txtItemUOM.Text),
                                PRDetailUOM = Convert.ToInt32(hdfItemUOM.Value),
                                PRDetailReqDate = txtItemReqDate.Text
                            };
                            PRSelected.Add(prDetail);
                            SetFieldValues(ControlsEnum.SELECTEDPR);
                            ResetForm();
                            BindVendors();
                        }
                        else
                        {
                            hdfIsValidItem.Value = CommonConstants.SELECT_VALUE_ONE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Item_Repeat").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowItemConfirm1", "ShowItemConfirm('" + btnAddNew.ClientID + "', '" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                    }
                    break;
                case ActionsEnum.ITEMRATES:
                    selectedItemPK = ((((ImageButton)sender).Parent.Parent as GridViewRow).FindControl("hdfItemPK") as HiddenField).Value;
                    if (!string.IsNullOrEmpty(selectedItemPK))
                        selectedItem = Convert.ToInt32(selectedItemPK);
                    if (selectedItem > 0)
                    {
                        selectedPRDetails = PRSelected.Where(row => selectedItem == row.PRDetailItem);
                        if (selectedPRDetails != null && selectedPRDetails.Count() > 0)
                        {
                            lblItemCodeTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(selectedPRDetails.First().ItemCode), 30);
                            lblItemNameTxt.Text = CommonFunctions.GetShortString(HttpUtility.HtmlDecode(selectedPRDetails.First().ItemName), 30);
                            lblItemCodeTxt.ToolTip = HttpUtility.HtmlDecode(selectedPRDetails.First().ItemCode);
                            lblItemNameTxt.ToolTip = HttpUtility.HtmlDecode(selectedPRDetails.First().ItemName);
                        }
                        GetFieldValues(ControlsEnum.ITEMRATES);
                        SetFieldValues(ControlsEnum.ITEMRATES);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=ItemRateDialog]','" + GetLocalResourceObject("Rates").ToString() + "','930','500');", true);
                    }
                    break;
                case ActionsEnum.LISTVENDORS:
                    BindVendors();
                    break;
                case ActionsEnum.REQUESTQUOTE:
                case ActionsEnum.RFQREQUEST:
                    vendorParameters = RFQParameters;
                    if (vendorParameters != null)
                    {
                        vendorParameters.VendorParameters = new List<RFQVendorParameter>();
                        foreach (ListItem vendor in chlVendors.Items)
                        {
                            if (vendor.Selected)
                            {
                                vendorParameters.VendorParameters.Add(new RFQVendorParameter()
                                {
                                    VEN_PK = Convert.ToInt32(vendor.Value)
                                });
                            }
                        }
                    }
                    if (vendorParameters != null && vendorParameters.VendorParameters != null && vendorParameters.VendorParameters.Count > 0)
                    {
                        RFQParameters = vendorParameters;
                        Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.RequestForQuote), false);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("VendorNotselected").ToString()
                                           + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    break;
                case ActionsEnum.CANCEL:
                    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.RFQListing), false);
                    break;
            }
        }

        #endregion

        #region --- For Grid Actions----

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ActionHandler(object sender, GridViewRowEventArgs e)
        //{
        //    int sthPK = 0;
        //    try
        //    {
        //        #region Grid Fixed Columns
        //        if ((sender as GridView).ID == "grdPoList")
        //        {
        //            if (e.Row.RowType == DataControlRowType.DataRow)
        //            {
        //                Label lblVendor = e.Row.FindControl("lblVendor") as Label;
        //                Label lblShipping = e.Row.FindControl("lblShipping") as Label;
        //                if (PurOrderHdrList != null && PurOrderHdrList.Count > 0)
        //                {

        //                    //   lblVendor.Text = PurOrderHdrList[e.Row.RowIndex].PUR_VENDOR_MST.VEN_CODE;
        //                    ADM_DEPT_MST AdmDeptMstObj = new ADM_DEPT_MST();
        //                    //  lblShipping.Text = PurOrderHdrList[e.Row.RowIndex].ADM_DEPT_MST.DPT_NAME;
        //                }
        //            }
        //        }

        //        #endregion

        //        if ((sender as GridView).ID == "grdStockTransfer")
        //        {
        //            if (e.Row.RowType == DataControlRowType.DataRow)
        //            {
        //                sthPK = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfStockTransferPK")).Value);
        //                Label lblTranTo = e.Row.FindControl("lblStTransferTo") as Label;

        //                List<INV_STK_TRAN_DTL> objList = InvStkTranHdrList[0].INV_STK_TRAN_DTL.Where(aa => aa.SFD_PK == sthPK).ToList();
        //                foreach (INV_STK_TRAN_DTL objItem in objList)
        //                    lblTranTo.Text = objItem.ADM_DEPT_MST1.DPT_NAME;
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
        //    }
        //}

        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ActionHandler(object sender, GridViewSortEventArgs e)
        //{
        //    try
        //    {
        //        if (SortBy == e.SortExpression)
        //        {
        //            ////Toggle the sort expression
        //            //if (SortDirection == Resources.gComsRes.SortAscending)
        //            //    SortDirection = Resources.gComsRes.SortDescending;
        //            //else
        //            //    SortDirection = Resources.gComsRes.SortAscending;
        //        }
        //        else
        //        {
        //            //SortBy = e.SortExpression;
        //            //SortDirection = Resources.gComsRes.SortAscending;
        //        }

        //        this.PageIndex = CommonConstants.SELECT_VALUE_ONE;
        //        GetFieldValues(ControlsEnum.DEFAULT);
        //        SetFieldValues(ControlsEnum.DEFAULT);
        //        EntryStatus = EntryStatus.LISTMODE;
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
        //    }
        //}

        #endregion

        #endregion
        #region Helper Methods

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <param name="mode">Save Action</param>
        /// <returns>Object to Save</returns>
        //private object SetUIValuesToObject(ActionsEnum mode)
        //{
        //    object returnObj = null;
        //    try
        //    {
        //        bool bIsChecked = false;

        //        foreach (GridViewRow grdrow in grdPoList.Rows)
        //        {
        //            RadioButton rbtn;
        //            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
        //            if (rbtn.Checked)
        //            {
        //                bIsChecked = true;
        //                break;
        //            }
        //        }

        //        if (bIsChecked)
        //            switch (mode)
        //            {
        //                case ActionsEnum.PICKFORINVOICING:
        //                    if (!IsSameVendor(SelectedVendors, VendorID))
        //                    {
        //                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Vendor").ToString();
        //                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
        //                    }
        //                    else if (!IsExixtPk(SelectedPos, PoId))
        //                    {
        //                        //Add Vendors
        //                        if (SelectedVendors != null)
        //                        {
        //                            SelectedVendorsList = SelectedVendors;
        //                        }
        //                        else
        //                        {
        //                            SelectedVendorsList = new List<long>();
        //                        }
        //                        SelectedVendorsList.Add(VendorID);
        //                        SelectedVendors = SelectedVendorsList;
        //                        //Add Pos
        //                        if (SelectedPos != null)
        //                        {
        //                            SelectedPOList = SelectedPos;
        //                        }
        //                        else
        //                        {
        //                            SelectedPOList = new List<long>();
        //                        }
        //                        SelectedPOList.Add(PoId);
        //                        SelectedPos = SelectedPOList;
        //                        SelectedPosCount = SelectedPos.Count;

        //                        btnPickForInvoice.Text = GetLocalResourceObject("PickPoForInvoicing").ToString() + "(" + SelectedPosCount.ToString() + ")";

        //                    }
        //                    else
        //                    {
        //                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;

        //                        // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
        //                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

        //                    }
        //                    break;
        //            }
        //        else
        //        {
        //            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_PO").ToString();
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
        //        }
        //        return returnObj;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;

        //    }

        //}
        /// <summary>
        /// Set values to Session for handling edit and view or navigation
        /// </summary>
        //private void SetUIEditView(ActionsEnum mode)
        //{
        //    try
        //    {
        //        //Session[SessionStrings.ActivityPK] = null;
        //        bool bIsChecked = false;

        //        //foreach (GridViewRow grdrow in grdActivities.Rows)
        //        //{
        //        //    RadioButton rbtn;
        //        //    rbtn = (RadioButton)grdrow.FindControl("rbtSelect");

        //        //    if (rbtn.Checked)
        //        //    {
        //        //        // get pk from the grid and assign to CurrPk
        //        //        CurrPK = Convert.ToInt32(grdActivities.DataKeys[grdrow.RowIndex].Values[0]);
        //        //        Session[SessionStrings.ActivityPK] = CurrPK;
        //        //        bIsChecked = true;
        //        //    }
        //        //}
        //        if (bIsChecked)
        //            switch (mode)
        //            {
        //                //case ActionsEnum.BASICINFO:
        //                //    Response.Redirect(Resources.PageURL.ManageActivity, true);
        //                //    break;
        //                //case ActionsEnum.DOCUMENTS:
        //                //    Response.Redirect(Resources.PageURL.ActivityAttachment, true);
        //                //    break;
        //                //case ActionsEnum.CHARGES:
        //                //    Response.Redirect(Resources.PageURL.ActivityCharges, true);
        //                //    break;
        //                //case ActionsEnum.FLIGHTINFO:
        //                //    Response.Redirect(Resources.PageURL.ActivityFlightInfo, true);
        //                //    break;
        //                //case ActionsEnum.ACTIVITYINPUTS:
        //                //    Response.Redirect(Resources.PageURL.ActivityInputs, true);
        //                //    break;
        //                //case ActionsEnum.FREEUSAGE:
        //                //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ActivityFreeUsage), false);
        //                //    break;
        //                //case ActionsEnum.REPORT:
        //                //    Response.Redirect(Resources.PageURL.ActivityCostReport, true);
        //                //    break;

        //            }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + "Please selct a row from the list" + "');", true);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.DEFAULT:
                        if (PRSearchResult != null && PRSearchResult.Count > 0)
                            //PRSearchResult = PRSearchResult.OrderBy(prq => prq.PRHeaderNo).ThenBy(prq => prq.PRDetailItem).ToList();
                        foreach (var item in PRSearchResult)
                        {
                            if (item.PRDetailSpec == "undefined" || item.PRDetailSpec == null)
                            {
                                item.PRDetailSpec = string.Empty;
                            }
                        }
                        grdPRSearch.DataSource = PRSearchResult;
                        grdPRSearch.DataBind();
                        break;
                    case ControlsEnum.SELECTEDPR:
                        if (PRSelected != null && PRSelected.Count > 0)
                            PRSelected = PRSelected.OrderBy(prq => prq.PRHeaderNo).ThenBy(prq => prq.PRDetailItem).ToList();

                        foreach (var item in PRSelected)
                        {
                            if (item.PRDetailSpec == "undefined" || item.PRDetailSpec == null)
                            {
                                item.PRDetailSpec = string.Empty;
                            }
                        }
                        grdPRSelected.DataSource = PRSelected;
                        grdPRSelected.DataBind();
                        break;
                    case ControlsEnum.ITEMRATES:
                        grdItemRates.DataSource = dsPageData.Tables[0];
                        grdItemRates.DataBind();
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method for Dropdownlist binding
        /// </summary>
        private void BindDropDownList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    //case ControlsEnum.AIRCRAFTTYPE:
                    //    ddlAcftType.Items.Clear();
                    //    if (adAircraftTypeMstList != null && adAircraftTypeMstList.Count > 0)
                    //    {
                    //        ddlAcftType.DataSource = adAircraftTypeMstList;
                    //        ddlAcftType.DataTextField = Resources.DataFieldRes.AircraftTypeCode;
                    //        ddlAcftType.DataValueField = Resources.DataFieldRes.AircraftTypePK;
                    //        ddlAcftType.DataBind();
                    //    }
                    //    ddlAcftType.Items.Insert(0, new ListItem(Resources.gComsRes.Select, CommonConstants.SELECTVAL));
                    //    break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method for CheckBox List binding
        /// </summary>
        /// <param name="controlType"></param>
        private void BindCheckBoxList(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {

                    case ControlsEnum.VENDORS:
                        chlVendors.Items.Clear();
                        if (dsPageData != null && dsPageData.Tables.Count > 0)
                        {
                            chlVendors.DataSource = dsPageData.Tables[0];
                            chlVendors.DataTextField = Resources.DataFieldRes.VendorName;
                            chlVendors.DataValueField = Resources.DataFieldRes.VendorPK;
                            chlVendors.DataBind();
                        }
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ResetForm()
        {
            txtItemCategory.Text = string.Empty;
            hdfItemCategory.Value = string.Empty;
            txtItem.Text = string.Empty;
            hdfItem.Value = string.Empty;
            //txtItemDesc.Text = string.Empty;
            hdfTtemCode.Value = string.Empty;
            hdfTtemName.Value = string.Empty;
            txtItemSpec.Text = string.Empty;
            txtItemQty.Text = string.Empty;
            txtItemUOM.Text = string.Empty;
            hdfItemUOM.Value = string.Empty;
            txtItemReqDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);
        }

        private void BindVendors()
        {
            RFQParameters vendorParameters;

            vendorParameters = new RFQParameters();
            vendorParameters.ItemParameters = new List<RFQItemParameter>();
            if (PRSelected != null && PRSelected.Count > 0)
            {
                vendorParameters.ItemParameters = PRSelected.Select(prs => new RFQItemParameter()
                {
                    PRD_PK = prs.PRDetailPK,
                    ITM_PK = prs.PRDetailItem,
                    PRD_Qty = prs.PRDetailBalanceQty,
                    PRD_UOM = prs.PRDetailUOM,
                    PRD_Spec = prs.PRDetailSpec,
                    PRD_ReqDate = Convert.ToDateTime(prs.PRDetailReqDate)
                }).ToList();
            }
            RFQParameters = vendorParameters;
            xmlParameter = CommonFunctions.XmlSerialize<RFQParameters>(vendorParameters);
            GetFieldValues(ControlsEnum.VENDORS);
            SetFieldValues(ControlsEnum.VENDORS);
        }

        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }


        #endregion
        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            // uclPaging.CurrentPage = 1;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            //this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            //this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            //this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        //{
        //    try
        //    {
        //        //switch (e.Action)
        //        //{
        //        //    case NavigationEnum.PAGECHANGE:
        //        //        uclPaging.CurrentPage = e.CurrentPage;
        //        //        break;
        //        //    case NavigationEnum.FIRST:
        //        //        // Assignment the first page index.
        //        //        if (e.CurrentPage > 1)
        //        //            uclPaging.CurrentPage = 1;
        //        //        break;
        //        //    case NavigationEnum.LAST:
        //        //        // Assignment the last page index.
        //        //        if (e.CurrentPage <= e.TotalPages)
        //        //            uclPaging.CurrentPage = e.TotalPages;
        //        //        break;
        //        //    case NavigationEnum.NEXT:
        //        //        // Increment the next page index.
        //        //        if (e.CurrentPage <= e.TotalPages)
        //        //            uclPaging.CurrentPage++;
        //        //        break;
        //        //    case NavigationEnum.PREVIOUS:
        //        //        // Decrement the previous page index.
        //        //        if (e.CurrentPage > 1)
        //        //            uclPaging.CurrentPage--;
        //        //        break;
        //        //}
        //        //PageIndex = uclPaging.CurrentPage.ToString();
        //        GetFieldValues(ControlsEnum.DEFAULT);
        //        SetFieldValues(ControlsEnum.DEFAULT);
        //        EnableDisableButtons(e.TotalPages);
        //        EntryStatus = EntryStatus.LISTMODE;
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
        //    }
        //}
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link
            //uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //// Should we disable the previous link
            //uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //// Should we enable the next link
            //uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            //// Should we enable the last link
            //uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }
        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            //try
            //{
            //    if (EntryStatus == EntryStatus.VIEWMODE)
            //    {
            //        hdfDefaultSubmit.Value = string.Empty;
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
            //    }
            //    else if (EntryStatus == EntryStatus.NEWMODE)
            //    {
            //        hdfDefaultSubmit.Value = string.Empty;
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
            //    }
            //    else if (EntryStatus == EntryStatus.ENTRYMODE)
            //    {
            //        hdfDefaultSubmit.Value = string.Empty;
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
            //    }
            //    else if (EntryStatus == EntryStatus.LISTMODE)
            //    {
            //        hdfDefaultSubmit.Value = btnSearch.ClientID;
            //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
            //    }

            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            //}
            //catch (Exception ex)
            //{
            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            //}
        }

        #endregion

        #region Enum
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            SELECTEDPR,
            VENDORS,
            ITEMRATES,
            ITEM
        }

        #endregion

    }
}