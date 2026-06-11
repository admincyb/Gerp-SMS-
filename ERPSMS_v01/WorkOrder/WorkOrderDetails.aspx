<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="WorkOrderDetails.aspx.cs"
    Inherits="ERPSMS_v01.WorkOrder.WorkOrderDetails" Theme="ClassicExt" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var CurrencyDigits = 0;
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

        $(document).ready(function () {
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });
        function InitComponents() {
            DateInit();
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            ShowHideStoreDtl();
            ShowHideDescription();
            ShowHideAttachments();
            SwitchTab(1);
            GrandScriptUtils.MakeAutoCompleteDDL("txtSubContractor", "VendorManagement.do?Action=GetVendorsByRoleAuto&SBUPk=" + $("[id$=BizUnitPk]").val() + "&VRM_ROLE=7", "hdfSubContractor", true, true, "");
            GrandScriptUtils.MakeAutoCompleteDDL("txtSubContr", "VendorManagement.do?Action=GetVendorsByRoleAuto&SBUPk=" + $("[id$=BizUnitPk]").val() + "&VRM_ROLE=7", "hdfSubContr", true, true, "");
            //GrandScriptUtils.MakeAutoComplete("txtWOItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=1&IsWorkOrderItem=1" + "&FLDNAME=ITM_TEXT", "hdfWOItem", true, false, false, true);
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtWOItem", url, "hdfWOItem", true, true, 0, "WORKORDERITEM");
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtWONo", url, "hdfWONo", true, true, 4, "WONUMBER");

            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCustomer", url + "?IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerID", true, true, 0, "CUSTOMERLIST");
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", uiUrl + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrand", true, true, "CUTOMERBRAND");

            FillMaterialAutoComplete(0);
            //GrandScriptUtils.MakeAutoCompleteDDLNEW("txtSubContr", url, "hdfSubContr", true, true, 4, "");
            FillBOMMaterialCategory();
            FillBOMMaterial();

            $("[id*=txtWORate]").ForceNumericOnly();
            $("[id*=txtWOQty]").ForceNumericOnly();
            $("[id*=txtBOMQty]").ForceNumericOnly();
            $("[id*=txtPriceAdj]").ForceNumericOnly();
            $("[id*=txtAdjustNow]").ForceNumericOnly();

            ShowHideExpand();
        }

        function InitCustomerBrands() {
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtCustomer", url + "?IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerID", true, true, 0, "CUSTOMERLIST");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", uiUrl + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrand", true, true, "CUSTOMERBRANDCODENAMEWITHSPEC");
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", uiUrl + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrand", true, true, "CUTOMERBRAND");
        }
        function InitProducts() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtBOMItem", url + "?Type=1&BinSubType=" + $("[id$=ddlBOMCategory]").val(), "hdfBOMItem", true, true, "GETPRODUCTS");
        }
        function InitBrands() {
            //GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", uiUrl + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrand", true, true, "CUSTOMERBRANDCODENAMEWITHSPEC");
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", uiUrl + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrand", true, true, "CUTOMERBRAND");
        }
        function InitPackingMaterials() {
            GrandScriptUtils.MakeAutoComplete("txtBOMPMCategory", "MaterialCategory.do?Action=GetMaterialCategoryListAuto" + "&Type=3", "hdfBOMPMCatPK", true, false, "BizUnitPk", true);
            GrandScriptUtils.MakeAutoComplete("txtBOMPMItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=3" + "&FLDNAME=ITM_TEXT&BrandPK=" + $("[id$=hdfBrand]").val(), "hdfBOMPMItem", true, false, "hdfBOMPMItem", true);
        }
        function InitPakingMaterialAuto() {

            GrandScriptUtils.MakeAutoComplete("txtBOMPMItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=3" + "&FLDNAME=ITM_TEXT&BrandPK=" + $("[id$=hdfBrand]").val(), "hdfBOMPMItem", true, false, "hdfBOMPMCatPK", true);

        }
        function EnableDisableWOItem(disable) {
            if (disable == 1) {
                DisableAuto($("[id$=txtWorkOrderItem]"), $("[id$=hdfWorkOrderItem]"));
            }
            else {
                EnableAuto($("[id$=txtWorkOrderItem]"), $("[id$=hdfWorkOrderItem]"));
            }
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
            }
            return false;
        }

        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
            return false;
        }

        function ShowHideStoreDtl(flag) {
            //<summary>Function Used to Show Store details Panel </summary>
            //If flag then Show
            if (flag == 1) {
                $("#imgStoreDtlHide").show();
                $("#imgStoreDtlShow").hide();
                $("#divStoreDtl").show();
            }
            else {
                $("#imgStoreDtlHide").hide();
                $("#imgStoreDtlShow").show();
                $("#divStoreDtl").hide();
            }
        }

        //function ShowHideWODtl(flag) {
        //    //<summary>Function Used to Show WO details Panel </summary>
        //    //If flag then Show
        //    if (flag == 1) {
        //        $("#imgWODtlHide").show();
        //        $("#imgWODtlShow").hide();
        //        $("#divWODtl").show();
        //    }
        //    else {
        //        $("#imgWODtlHide").hide();
        //        $("#imgWODtlShow").show();
        //        $("#divWODtl").hide();
        //    }
        //}

        //function ShowHideBOM(flag) {
        //    //<summary>Function Used to Show BOM Panel </summary>
        //    //If flag then Show
        //    if (flag == 1) {
        //        $("#imgBOMHide").show();
        //        $("#imgBOMShow").hide();
        //        $("#divBOM").show();
        //    }
        //    else {
        //        $("#imgBOMHide").hide();
        //        $("#imgBOMShow").show();
        //        $("#divBOM").hide();
        //    }
        //}

        function ShowHideAttachments(flag) {
            //<summary>Function Used to Show BOM Panel </summary>
            //If flag then Show
            if (flag == 1) {
                $("#imgAttachHide").show();
                $("#imgAttachShow").hide();
                $("#divAttachments").show();
            }
            else {
                $("#imgAttachHide").hide();
                $("#imgAttachShow").show();
                $("#divAttachments").hide();
            }
        }

        function ShowHideDescription(flag) {
            //<summary>Function Used to Show Description Panel </summary>
            //If flag then Show
            if (flag == 1) {
                $("#imgDescriptionHide").show();
                $("#imgDescriptionShow").hide();
                $("#divDescription").show();
            }
            else {
                $("#imgDescriptionHide").hide();
                $("#imgDescriptionShow").show();
                $("#divDescription").hide();
            }
        }

        function ShowHideTerms(flag) {
            //<summary>Function Used to Show Description Panel </summary>
            //If flag then Show
            if (flag == 1) {
                $("#imgTermsHide").show();
                $("#imgTermsShow").hide();
                $("#divTerms").show();
            }
            else {
                $("#imgTermsHide").hide();
                $("#imgTermsShow").show();
                $("#divTerms").hide();
            }
        }

        function SwitchTab(tab) {
            if (tab == 1) {
                $("[id$=hdfTabNo").val("1");
                $("#tab4Content").hide();//divBOM
                $("#tab3Content").show();//divWODtl
                $("[id$=spnWODList]").attr('class', 'tab-active');
                $("[id$=spnBOMList]").attr('class', 'tab-inactive');
            }
            else if (tab == 2) {
                $("[id$=hdfTabNo").val("2");
                $("#tab4Content").show();//divBOM
                $("#tab3Content").hide();//divWODtl
                $("[id$=spnWODList]").attr('class', 'tab-inactive');
                $("[id$=spnBOMList]").attr('class', 'tab-active');
            }
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode 
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                DisableAuto($("[id$=txtWODate]"), false);
                DisableAuto($("[id$=txtSubContractor]"), $("[id$=hdfSubContractor]"));
                DisableAuto($("[id$=txtWorkOrderItem]"), $("[id$=hdfWorkOrderItem]"));
                DisableAuto($("[id$=txtBOMCategory]"), $("[id$=hdfBOMCategory]"));
                DisableAuto($("[id$=txtBOMItem]"), $("[id$=hdfBOMItem]"));
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomerID]"));
                DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
                DisableAuto($("[id$=txtBOMPMCategory]"), $("[id$=hdfBOMPMCatPK]"));
                DisableAuto($("[id$=txtBOMPMItem]"), $("[id$=txtBOMPMItem]"));
            }
            else {
                EnableAuto($("[id$=txtWODate]"), false);
                EnableAuto($("[id$=txtSubContractor]"), $("[id$=hdfSubContractor]"));
                EnableAuto($("[id$=txtWorkOrderItem]"), $("[id$=hdfWorkOrderItem]"));
                EnableAuto($("[id$=txtBOMCategory]"), $("[id$=hdfBOMCategory]"));
                EnableAuto($("[id$=txtBOMItem]"), $("[id$=hdfBOMItem]"));
                EnableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomerID]"));
                EnableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
                EnableAuto($("[id$=txtBOMPMCategory]"), $("[id$=hdfBOMPMCatPK]"));
                EnableAuto($("[id$=txtBOMPMItem]"), $("[id$=txtBOMPMItem]"));
            }
        }

        function EnableDisableAmendItems(enable) {
            if (enable == 1) {
                EnableAuto($("[id$=txtWorkOrderItem]"), $("[id$=hdfWorkOrderItem]"));
            }
            else {
                DisableAuto($("[id$=txtWorkOrderItem]"), $("[id$=hdfWorkOrderItem]"));
            }
        }

        function DisableBOMCategory() {
            DisableAuto($("[id$=ddlBOMCategory]"), false);
            DisableAuto($("[id$=txtBOMCategory]"), $("[id$=hdfBOMCategory]"));
        }
        function EnableBOMCategory() {
            EnableAuto($("[id$=ddlBOMCategory]"), false);
            EnableAuto($("[id$=txtBOMCategory]"), $("[id$=hdfBOMCategory]"));
        }

        function ShowDeleteConfirmationMsg(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '<%= Resources.ErpRes.MsgDeleteConfirm %>';
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }
                        return false;
                    }
                }
            });
            return false;
        }

        function ShowCustomerConfirmationMsg(btn, message) {

            if ($("[id$=hdfCustomerID]").val() == 0 && $("[id$=ddlItemType]").val() > 1) {

            } else {
                return true;
            }

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '<%= GetLocalResourceObject("CustomerConfirm").ToString() %>';
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }
                        return false;
                    }
                }
            });
            return false;
        }

        function DateInit() {
            //<summary>function used to make datepicker</summary>
            GrandScriptUtils.DatePickerCommon("txtWODate");
            GrandScriptUtils.DatePicker("txtWODtlDate", false, false);
        }

        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            //Traversing from bottom through all the validation controls in the page
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        //checks if the control is already in the validation array
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            //insert new conrol to the Array of present validations
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        //remove if control is already in Array of present validations
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                }
            }
        }

        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function InitWOItem() {
            //var selectItem = $("[id$=ddlWOItemType]").val();
            var selectItem = $("[id$=ddlItemType]").val();
            $("[id$=ddlWOItemType]").val(selectItem);
            switch (selectItem) {
                case "1":
                    FillMaterialAutoComplete(0);
                    break;
                case "2":
                    FillMaterialAutoComplete(0);
                    break;
                case "3":
                    FillMaterialAutoComplete(0);
                    break;
            }
        }
        //function InitItemType() {
        //    var selectItem = $("[id$=ddlItemType]").val();
        //    $("[id$=ddlWOItemType]").val(selectItem);
        //    switch (selectItem) {
        //        case "1":
        //            FillMaterialAutoComplete(0);
        //            break;
        //        case "2":
        //            FillMaterialAutoComplete(0);
        //            break;
        //        case "3":
        //            FillMaterialAutoComplete(0);
        //            break;
        //    }
        //}
        function FillMaterialAutoComplete(SelectVal) {
            //<summary> Function Used to make Item field as auto complete </summary>
            //GrandScriptUtils.MakeAutoComplete("txtWorkOrderItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=ddlWOItemType]").val() + "&IsWorkOrderItem=1" + "&FLDNAME=ITM_TEXT&CustomerPK=" + $("[id$=hdfCustomerID]").val(), "hdfWorkOrderItem", true, false, false, true);
            var CustomerID;
            if ($("[id$=ddlItemType]").val() == 3)//brand
                CustomerID = $("[id$=hdfCustomerID]").val();
            else
                CustomerID = 0;

            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtWorkOrderItem", url + "?OperationPK=" + $("[id$=ddlOperOrWork]").val() + "&Type=" + $("[id$=ddlWOItemType]").val() + "&CustomerID=" + CustomerID + "&BrandID=" + $("[id$=hdfBrand]").val(), "hdfWorkOrderItem", true, true, true, "WOITEM");
            EnableDisableWOItem($("[id$=hdfDisableWOItem]").val());
        }
        function FillMaterialDetails(materialID) {
            ///<summary>Function Used Fill the material Details corresponding to the id </summary>
            $.get("MaterialManagement.do?Action=GetMaterialDetails&SBUPk=" + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID + "&VendorPK=" + $("[id$=hdfSubContractor]").val(), function (data) {
                if (data) {
                    if (materialID != 0) {
                        $("[id$=txtUOM]").val(data[0].UOM_CODE);
                        $("[id$=hdfItemUOM]").val(data[0].ITM_UOM);
                        $("[id$=txtWORate]").val(data[0].ITV_PRICE);
                        CalculateAmount();
                        if ($("[id$=ddlItemType]").val() == "3") {
                            $("[id$=hdfItemCategory]").val(data[0].CIM_PROCESS_TEXT);
                            $("[id$=hdfItemCategoryPK]").val(data[0].CIM_PROCESS);
                        }
                    }
                }
                else {
                    $("[id$=txtUOM]").val("");
                    $("[id$=hdfItemUOM]").val(0);
                    $("[id$=txtWORate]").val("");
                }
            });
        }

        function FillBOMItemDetails(materialID) {
            ///<summary>Function Used Fill the material Details corresponding to the id </summary>
            $.get("MaterialManagement.do?Action=GetMaterialDetails&SBUPk=" + $("[id$=BizUnitPk]").val() + "&MaterialID=" + materialID + "&VendorPK=" + $("[id$=hdfSubContractor]").val(), function (data) {
                if (data) {
                    if (materialID != 0) {
                        $("[id$=txtBOMUOM]").val(data[0].UOM_CODE);
                        $("[id$=hdfBOMUOM]").val(data[0].ITM_UOM);
                    }
                }
                else {
                    $("[id$=txtBOMUOM]").val("");
                    $("[id$=hdfBOMUOM]").val(0);
                }
            });
        }

        function AfterAutoCompleteSelect(targetControlID) {
            //<summary> Function Used to an event fire after select category then fill material and uom </summary>
            if (targetControlID == "txtWorkOrderItem") {
                FillMaterialDetails($("[id$=hdfWorkOrderItem]").val());
                $("[id$=hdfSubContractorItem]").val($("[id$=hdfWorkOrderItem]").val());
            }
            if (targetControlID == "txtSubContractor") {

                if ($("[id$=hdfSubContractorOld]").val() != $("[id$=hdfSubContractor]").val() && $("[id$=hdfIsDataAdded]").val() == 1) {
                    ShowVendorChangeConfirmationMsg(this);
                }
                else {
                    if (parseInt($("[id$=hdfSubContractorItem]").val()) > 0) {
                        FillMaterialDetails($("[id$=hdfSubContractorItem]").val());
                        $("[id$=btnContractorSelected]").click();
                    }
                    $("[id$=btnVendor]").click();
                }
            }
            if (targetControlID == "txtBOMItem") {
                FillBOMItemDetails($("[id$=hdfBOMItem]").val());
            }
            if (targetControlID == "txtCustomer") {
                InitBrands();
                FillMaterialAutoComplete();
            }
            if (targetControlID == "txtBOMCategory") {
                FillBOMMaterial();
            }
            if (targetControlID == "txtBrand") {
                InitBrands();
                $("[id$=hdfDisableWOItem]").val("0");
                FillMaterialAutoComplete();
            }
            if (targetControlID == "txtBOMPMItem") {//txtBOMPMItem
                FillBOMItemDetails($("[id$=hdfBOMPMItem]").val());
            }
            if (targetControlID == "txtBOMPMCategory") {
                InitPakingMaterialAuto();
            }
        }
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                InitBrands();
            }
            if (targetControlID == "txtWorkOrderItem") {
                FillMaterialAutoComplete();
            }
        }

        function CalculateAmount() {
            var rate = parseFloat($("[id$=txtWORate]").val());
            var qty = parseFloat($("[id$=txtWOQty]").val());
            var amt = rate * qty;
            if (!isNaN(amt)) {
                $("[id$=txtWOAmt]").val(amt.toFixed(CurrencyDigits));
            }
            else {
                amt = 0;
                $("[id$=txtWOAmt]").val(amt.toFixed(CurrencyDigits));
            }
        }

        function FillBOMMaterialCategory() {
            //<summary> Function Used to make material category field as auto complete </summary>
            GrandScriptUtils.MakeAutoComplete("txtBOMCategory", "MaterialCategory.do?Action=GetMaterialCategoryListAuto" + "&Type=1", "hdfBOMCategory", true, false, "BizUnitPk", true);
        }
        function FillBOMMaterial() {
            //<summary> Function Used to make Item field as auto complete </summary>
            //GrandScriptUtils.MakeAutoComplete("txtBOMItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=" + $("[id$=hdfBOMCategory]").val() + "&FLDNAME=ITM_TEXT", "hdfBOMItem", true, false, "MaterialCategoryPK", true);
            GrandScriptUtils.MakeAutoComplete("txtBOMItem", "MaterialManagement.do?Action=GetMaterialCodeNameByCategoryAuto&AUTOSEARCH=1&SBUPk=" + $("[id$=BizUnitPk]").val() + "&Type=1&IsWorkOrderItem=1" + "&FLDNAME=ITM_TEXT", "hdfBOMItem", true, false, "hdfBOMCategory", true);
        }

        function ClearTerms(type) {
            ///<summary>function used Clear Terms Details </summary>
            if (type == "Vendor") {
                VendorTerms = new Array();
                $("[id$=ddlVendorTerms]").val("-1");
                $("[id$=hdfVendorTermID]").val("");
                $("[id$=txtVendorTermText]").val("");
            }
            else if (type == "General") {
                Terms = new Array();
                $("[id$=ddlGeneralTerms]").val("0");
                $("[id$=hdfGeneralTermText]").val("");
                $("[id$=txtGeneralTermText]").val("");
                $("[id$=hdfGeneralTermID]").val("");
            }
            return false;
        }

        function RevisionHistory() {
            $("#divRevisionHistory").dialog("open");
            $("#divRevisionHistory").dialog(
                {
                    width: 420,
                    title: $("id$=hdfRevistoryText").val() //"Translate(RevisionHistory)"
                });
            //return false;
        }

        function CalculateTotal(sender) {
            var subTotal = parseFloat($('[id$=txtSubTotal]').val());
            subTotal = isNaN(subTotal) ? 0 : subTotal;
            var totalDiscount = parseFloat($("[id$=txtHdrDiscount]").val());
            totalDiscount = isNaN(totalDiscount) ? 0 : totalDiscount;
            var totalTax = parseFloat($("[id$=txtHdrTax]").val());
            totalTax = isNaN(totalTax) ? 0 : totalTax;
            var totalShipping = parseFloat($("[id$=txtShipping]").val());
            totalShipping = isNaN(totalShipping) ? 0 : totalShipping;
            var totalPriceAdj = parseFloat($("[id$=txtPriceAdj]").val());
            totalPriceAdj = isNaN(totalPriceAdj) ? 0 : totalPriceAdj;

            var netTotal = (subTotal + totalTax + totalShipping + totalPriceAdj) - totalDiscount;
            $("[id$=txtHdrTotal]").val(netTotal.toFixed(parseInt($("[id$=hdfCurrencyDigits]").val())));
            $("[id$=txtHdrTotal]").attr("title", (netTotal).toFixed(parseInt($("[id$=hdfCurrencyDigits]").val())));
            if (totalShipping == 0)
                $("[id$=txtShipping]").val((totalShipping).toFixed(CurrencyDigits));
            if (totalPriceAdj == 0)
                $("[id$=txtPriceAdj]").val((totalPriceAdj).toFixed(CurrencyDigits));
        }

        //function SetPadding() {
        //    $('.foo').css({"padding-right":"51px"});
        //}

        function ShowAllocationConfirmationMsg(btn, message) {

            if ($("[id$=hdfHasAllocation]").val() == 1 && $("[id$=hdfFullyAllocated]").val() == 0) {

            } else {
                return true;
            }

            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '<%= GetLocalResourceObject("AllocationConfirm").ToString() %>';
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }
                        return false;
                    }
                }
            });
            return false;
        }

        function ShowVendorChangeConfirmationMsg(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = message ? message : '<%= GetLocalResourceObject("VendorChangeConfirm").ToString() %>';
            $("#popupHolder").html("");
            //To set fit to screen
            $('html, body').animate({ scrollTop: '0px' }, 0);
            $('html, body').css('overflow', 'hidden');
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");

                        if (parseInt($("[id$=hdfSubContractorItem]").val()) > 0) {
                            FillMaterialDetails($("[id$=hdfSubContractorItem]").val());
                            $("[id$=btnContractorSelected]").click();
                        }
                        $("[id$=btnVendor]").click();

                    },
                    Cancel: function (e) {
                        $('html').css('overflow', 'auto');
                        $('body').css('overflow', 'visible');
                        $('#divmodel').hide();
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }

                        $("[id$=hdfSubContractor]").val($("[id$=hdfSubContractorOld]").val());
                        $("[id$=txtSubContractor]").val($("[id$=hdfSubContractorTextOld]").val());

                        return false;
                    }
                }
            });
            return false;
        }

        function CheckAllIssues(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=grdIssueDetails.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }

        function ItemListSelection() {
            var selectedRowColor;
            selectedRowColor = '<%= Resources.ErpRes.selectedRowColor %>';

            $("#[id*=grdWorkOrder] input[type=hidden][id*=hdfWOID]").each(function (index) {
                if ($(this).val() == $("[id$=hdfSelectedItemWOPK]").val()) {
                    $(this).closest('tr').css('background-color', selectedRowColor);
                }
            });
        }

        function AfterGridExpand(row) {

            if ($("[id$=grdWOItems]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedWOItem]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetIssue]").click();
                }
            }

            if ($("[id$=grdIssueList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedIssueItem]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetGrn]").click();
                }
            }

            var names = $(row).parent().parent().attr('id').split('_')[4];
            if ($("[id$=grdGRNList]").attr('id').indexOf(names)) {// == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedGRNItem]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetPouchWallet]").click();
                }
            }
        }

        function ShowHideExpand() {
            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }

        function UpdateAdjustNow(targetRow) {
            $("[id$=hdfAdjustRowId]").val(targetRow.dataset.rowid);
            $("[id$=hdfAdjustNow]").val(targetRow.value);
            $("[id$=btnCalculate]").click();
        }

    </script>
    <style>
        h1.search-colapse-normal {
            font-size: 11px;
            border: 0;
            margin-top: 3px;
            background: #E4F2E4;
            padding: 0px 7px !important;
        }

        .scroll-container {
            width: 100%;
            max-width: 100% !important;
            overflow: auto;
        }

        .emptyfooter {
            height: 19px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlWorkOrder">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPnl" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlAmend">
                                        <asp:Button runat="server" TabIndex="74" ID="btnAmend" CommandName="AMEND" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Amend %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-amend"
                                            ToolTip="<%$resources:Controls,Amend %>" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="75" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript: return ValidatePageNow('wo') && ShowAllocationConfirmationMsg(this);"
                                            ValidationGroup="wo" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="76" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript: return ValidatePageNow('wo') && ShowAllocationConfirmationMsg(this);"
                                            ValidationGroup="wo" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ToolTip="<%$resources:Controls,Save %>" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="77" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            CommandName="CANCEL" TabIndex="78" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="79" ID="btnPrintWO" CommandName="PRINTWO" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="10" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" TabIndex="11" Text="<%$resources:Controls,Print %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPnl" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" Visible="false" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <%--------------colpase btn start----------%>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1><%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn end----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblTrnStatus" Text="<%$ resources:TrnStatus %>" AssociatedControlID="ddlTrnStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlTrnStatus" runat="server" CssClass="select-small-e" TabIndex="1">
                                            </asp:DropDownList>

                                            <br />

                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="3" CssClass="lbl-26perc"
                                                MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl-xsmall-b1"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="4" CssClass="lbl-26perc"
                                                MaxLength="17" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblWOItem" runat="server" Text="<%$resources:WOItem %>" AssociatedControlID="txtWOItem"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtWOItem" runat="server" TabIndex="2" CssClass="input-half-60-8"
                                                onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfWOItem" runat="server" Value="0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblSubContr" runat="server" Text="<%$resources:SubContractor %>" AssociatedControlID="txtSubContr"></asp:Label>
                                            <asp:TextBox ID="txtSubContr" runat="server" TabIndex="5" CssClass="select-half-b margnbotm0"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfSubContr" runat="server" Value="0" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblWONo" runat="server" Text="<%$resources:WONo %>" AssociatedControlID="txtWONo"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox ID="txtWONo" runat="server" TabIndex="6" CssClass="input-small margnbotm0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfWONo" runat="server" Value="0" />

                                            <asp:Label ID="lblStatus" runat="server" Text="<%$resources:Status %>" AssociatedControlID="ddlStatus"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b margnbotm0 margn-rgt2" TabIndex="7">
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterNotClosed %>" Text="<%$ Resources:BindValues, StatusFilterNotClosed%>"
                                                    Selected="True">
                                                </asp:ListItem>
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterAll %>" Text="<%$ Resources:BindValues, StatusFilterAll%>">
                                                </asp:ListItem>
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterClosed %>" Text="<%$ Resources:BindValues, StatusFilterClosed%>">
                                                </asp:ListItem>
                                            </asp:DropDownList>

                                            <asp:Label ID="Label1" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>

                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                TabIndex="8" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                                OnClick="ActionHandler" CommandName="SEARCH" />
                                            <asp:ImageButton ID="btnClear" runat="server" TabIndex="9" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                                ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                                                OnClick="ActionHandler" CommandName="CLEAR" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap">
                                <asp:GridView runat="server" ID="grdWorkOrder" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable" OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="Label1" runat="server" TabIndex="23" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbWOList" CssClass="itemlist"
                                                    ToolTip="<%$Resources:ItemDetails %>" OnClick="ActionHandler" CommandName="WOITEMDETAILS" alt="" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:WODate %>" SortExpression="<%$ resources:DataFieldRes,EmployeeCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWODate" runat="server" ToolTip='<%# Eval("WIH_DATE", Resources.Constants.DateFormatGridExpanded)%>'
                                                    Text='<%# Eval("WIH_DATE", Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:WONo %>" SortExpression="WIH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWONumber" runat="server" ToolTip='<%# Eval("WIH_NO") %>'
                                                    Text='<%# Eval("WIH_NO") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfWIH_NO" runat="server" Value='<%# Eval("WIH_NO") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:RefNo %>" SortExpression="WIH_REF_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWORefNumber" runat="server" ToolTip='<%# Eval("WIH_REF_NO") %>'
                                                    Text='<%# Eval("WIH_REF_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:SubContractor %>" SortExpression="WIH_VENDOR_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWOSubContractor" runat="server" ToolTip='<%# Eval("WIH_VENDOR_TEXT") %>'
                                                    Text='<%# Eval("WIH_VENDOR_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="22%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:WOItems %>" SortExpression="WIH_ITEM_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWOItems" runat="server" ToolTip='<%# Eval("WIH_ITEM_TEXT") %>'
                                                    Text='<%# Eval("WIH_ITEM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="26%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Qty %>" SortExpression="WIH_TOTAL_QTY" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" ToolTip='<%# Eval("WIH_TOTAL_QTY") %>'
                                                    Text='<%# Eval("WIH_TOTAL_QTY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" SortExpression="WIH_NET_AMOUNT" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" ToolTip='<%# Eval("WIH_NET_AMOUNT") %>'
                                                    Text='<%# Eval("WIH_NET_AMOUNT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ErpRes,Status  %>" SortExpression="<%$ resources:ErpRes,Status %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" ToolTip='<%# Eval("WIH_STATUS_TEXT") %>'
                                                    Text='<%# Eval("WIH_STATUS_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Quantity %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWOQty" runat="server" ToolTip='<%# Eval("WIH_TOTAL_QTY") %>'
                                                    Text='<%# Eval("WIH_TOTAL_QTY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Amount %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWOAmt" runat="server" ToolTip='<%# GetFormattedCurrency(Eval("WIH_TOTAL_AMOUNT")) %>'
                                                    Text='<%# GetFormattedCurrency(Eval("WIH_TOTAL_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Issued %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIssueStatus" runat="server" ToolTip='<%# Eval("ISSUE_STATUS") %>'
                                                    Text='<%# Eval("ISSUE_STATUS") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>--%>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Issued_Received %>" Visible="false">
                                            <ItemTemplate>
                                                <image id="imgIssued" title=""
                                                    class='<%# (Eval("WIH_STATUS")).ToString() == "1" ? "active" : "inactive" %>'
                                                    alt=""></image>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>--%>
                                        <%--<asp:TemplateField Visible="false">
                                            <ItemTemplate>
                                                <image id="imgReceived" title="" class="" alt=""></image>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" VerticalAlign="Middle" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Action %>">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>" OnClick="ActionHandler"
                                                    TabIndex="14" SkinID="imbeditgrid" CommandName="EDITITEM" CommandArgument='<%# Eval("WIH_PK") %>' />
                                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$resources:ErpRes,View %>" OnClick="ActionHandler"
                                                    TabIndex="14" SkinID="btnview" CommandName="VIEW" CommandArgument='<%# Eval("WIH_PK") %>' />
                                                <asp:ImageButton runat="server" ID="imbPrint" ToolTip="<%$Resources:Controls,Print %>" OnClick="ActionHandler"
                                                    TabIndex="14" SkinID="btnPrint" CommandName="PRINT" CommandArgument='<%# Eval("WIH_PK") %>' />
                                                <asp:ImageButton runat="server" TabIndex="14" ID="imbShortClose" CommandName="SHORTCLOSE" OnClientClick="return ShowDeleteConfirm(this,'Are you sure want to close this WO.?');"
                                                    SkinID="btnclose" alt="<%$Resources:Controls,ShortClose%>" title="<%$Resources:Controls,ShortClose%>" />
                                                <asp:ImageButton runat="server" TabIndex="14" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>" OnClick="ActionHandler"
                                                    SkinID="imbdeletegrid" CommandName="DELETEWO" CommandArgument='<%# Eval("WIH_PK") %>' OnClientClick="return ShowDeleteConfirm(this);" />
                                                <asp:Button ID="imgApproved" ToolTip='<%# Eval("ISSUE_STATUS") %>' runat="server"
                                                    CommandName="SHOWISSUEDETAILS" OnClick="ActionHandler" />
                                                <asp:ImageButton ID="btnStockAdjust" ToolTip="<%$ resources:StockAdjustment %>" runat="server"
                                                    CommandName="SHOWISSUEPOPUP" SkinID="imgProcPayroll" OnClick="ActionHandler"
                                                    Visible='<%# Eval("WIH_STK_EXCESS_FLAG").ToString() == "1" ? true : false %>' />
                                                <asp:HiddenField runat="server" ID="hdfWOStatus" Value='<%# Eval("WIH_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfUserStatus" Value='<%# Eval("USER_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfWOID" Value='<%# Eval("WIH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfRefID" Value='<%# Eval("refPK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfWOProcessID" Value='<%# Eval("refProcess") %>' />
                                                <asp:HiddenField runat="server" ID="hdfIssueStatus" Value='<%# Eval("ISSUE_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField runat="server" ID="hdfWOStatus" Value='<%# Eval("WIH_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfUserStatus" Value='<%# Eval("USER_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfWOID" Value='<%# Eval("WIH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfRefID" Value='<%# Eval("refPK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfWOProcessID" Value='<%# Eval("refProcess") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" HorizontalAlign="Left" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                            <h4>Item Details</h4>
                            <div class="gridwrap hierarchical-wrap">
                                <cc2:ExtGridView runat="server" ID="grdWOItems" AutoGenerateColumns="False" OnRowDataBound="ActionHandler"
                                    ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    ShowFooter="true" PageSize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItem" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ITM_NAME").ToString()),35) %>'
                                                    ToolTip='<%# Eval("ITM_NAME") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedWOItem" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfWODetailItem" Value='<%# Eval("ITM_PK") %>' />
                                                <asp:Button runat="server" ID="btnGetIssue" OnClick="ActionHandler" CommandName="ISSUEDETAILS"
                                                    CommandArgument='<%# Eval("WID_PK") %>' EnableTheming="false" Style="display: none" />
                                            </ItemTemplate>
                                            <ItemStyle Width="53%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ItemCat %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCat" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ITM_CATEGORY_TEXT").ToString()),35) %>'
                                                    ToolTip='<%# Eval("ITM_CATEGORY_TEXT") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:WOQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPOQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("WID_QTY")) +" "+Eval("WID_UOM_TEXT") %>' ToolTip='<%#GetFormattedNumberWithSeperation(Eval("WID_QTY")) +" "+Eval("WID_UOM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="rate-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:WORcvdQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPORcvdQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("WID_RCVD_QTY")) +" "+Eval("WID_UOM_TEXT") %>' ToolTip='<%#GetFormattedNumberWithSeperation(Eval("WID_RCVD_QTY")) +" "+Eval("WID_UOM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="rate-numeric" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:WOInvQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPOInvQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("WID_QTY_INVOICED")) +" "+Eval("WID_UOM_TEXT") %>' ToolTip='<%#GetFormattedNumberWithSeperation(Eval("WID_QTY_INVOICED")) +" "+Eval("WID_UOM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="rate-numeric" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <cc2:ExtGridView runat="server" ID="grdIssueList" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                        CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                        CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" CellPadding="3"
                                                        ForeColor="#333333" AllowPaging="false">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblGRNList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:IssueNo %>">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkIssueNo" CssClass="text-underline" runat="server" Text='<%# Eval("ISSUE_NO") %>' OnClick="ActionHandler"
                                                                        CommandName="PRINTISSUE" CommandArgument='<%# Eval("ISSUE_PK") %>' ToolTip='<%# Eval("ISSUE_NO") %>'></asp:LinkButton>
                                                                    <asp:HiddenField runat="server" ID="hdfIsExpandedIssueItem" Value="0" />
                                                                    <asp:HiddenField runat="server" ID="hdfWODetailItem" Value='<%# Eval("ISSUE_ITEM") %>' />
                                                                    <asp:HiddenField runat="server" ID="hdfIssueItemType" Value='<%# Eval("WIB_ITEM_TYPE") %>' />
                                                                    <asp:Button runat="server" ID="btnGetGrn" OnClick="ActionHandler" CommandName="GRNDETAILS"
                                                                        CommandArgument='<%# Eval("WIH_PK") %>' EnableTheming="false" Style="display: none" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="23%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:IssueDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIssueDetailDate" runat="server" Text='<%# Eval("ISSUE_DATE", Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval("ISSUE_DATE", Resources.Constants.DateFormatGrid) %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIssuedItem" runat="server" Text='<%# Eval("ISSUE_ITM_NAME") %>'
                                                                        ToolTip='<%# Eval("ISSUE_ITM_NAME") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="43%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIssuedQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("ISSUE_QTY")) +" "+Eval("ISSUE_QTY_UOM_TEXT") %>'
                                                                        ToolTip='<%#GetFormattedNumberWithSeperation(Eval("ISSUE_QTY")) +" "+Eval("ISSUE_QTY_UOM_TEXT") %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="rate-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <div class="hierarchical-gridwrap">
                                                                        <cc2:ExtGridView runat="server" ID="grdGRNList" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                                            CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                                            CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" CellPadding="3"
                                                                            ForeColor="#333333" AllowPaging="false">
                                                                            <EmptyDataTemplate>
                                                                                <asp:Label ID="lblGRNList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                            </EmptyDataTemplate>
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="<%$ resources:GRNNo %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lnkGrnNo" CssClass="text-underline" runat="server" Text='<%# Eval("GRH_NO") %>' OnClick="ActionHandler"
                                                                                            CommandName="PRINTGRN" CommandArgument='<%# Eval("GRH_PK") %>' ToolTip='<%# Eval("GRH_NO") %>'></asp:LinkButton>
                                                                                        <asp:HiddenField runat="server" ID="hdfIsExpandedGRNItem" Value="0" />
                                                                                        <asp:HiddenField runat="server" ID="hdfGRNPk" Value='<%# Eval("GRH_PK") %>' />
                                                                                        <asp:Button runat="server" ID="btnGetPouchWallet" OnClick="ActionHandler" CommandName="WALLETDETAILS"
                                                                                            EnableTheming="false" Style="display: none" CommandArgument='<%# Eval("GRD_WIH_PK") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="22%" />
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGRNDate" runat="server" Text='<%# Eval("GRH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                                                            ToolTip='<%# Eval("GRH_DATE", Resources.Constants.DateFormatGrid) %>'>
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="8%" />
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:GrnStore %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGRNStore" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("GRH_DEPT_TEXT"),23) %>'
                                                                                            ToolTip='<%# Eval("GRH_DEPT_TEXT") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="30%" />
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:GRNRcvdQty %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGRNRecievedQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("GRD_QTY_RECEIVED")) +" "+Eval("GRD_UOM_TEXT") %>'
                                                                                            ToolTip='<%# GetFormattedNumberWithSeperation(Eval("GRD_QTY_RECEIVED")) +" "+Eval("GRD_UOM_TEXT") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                                                    <HeaderStyle CssClass="rate-numeric" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:GRNAccQty %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGRNQtyAccepted" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("GRD_QTY_ACCEPTED")) +" "+Eval("GRD_UOM_TEXT") %>'
                                                                                            ToolTip='<%# GetFormattedNumberWithSeperation(Eval("GRD_QTY_ACCEPTED")) +" "+Eval("GRD_UOM_TEXT") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                                                    <HeaderStyle CssClass="rate-numeric" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <div class="hierarchical-gridwrap">
                                                                                            <cc2:ExtGridView runat="server" ID="grdPouchWallet" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                                                                CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                                                                CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" CellPadding="3"
                                                                                                ForeColor="#333333" AllowPaging="false">
                                                                                                <EmptyDataTemplate>
                                                                                                    <asp:Label ID="lblGRNList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                                                </EmptyDataTemplate>
                                                                                                <Columns>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:WalletNo %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton ID="lnkWalletNo" CssClass="text-underline" runat="server" Text='<%# Eval("BVH_BIN_CARD_TEXT") %>' OnClick="ActionHandler"
                                                                                                                CommandName="PRINTWALLET" CommandArgument='<%# Eval("BVH_BIN_CARD") %>' ToolTip='<%# Eval("BVH_BIN_CARD_TEXT") %>'></asp:LinkButton>
                                                                                                            <asp:HiddenField runat="server" ID="hdfWalletPK" Value='<%# Eval("BVH_BIN_CARD") %>' />
                                                                                                            <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                                                            <asp:HiddenField runat="server" ID="hdfWalletPouchType" Value='<%# Eval("BVH_MENU_TYPE") %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="22%" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGRNDate" runat="server" Text='<%# Eval("BVH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                                                                                ToolTip='<%# Eval("BVH_DATE", Resources.Constants.DateFormatGrid) %>'>
                                                                                                            </asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="8%" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:TotalPcs %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGRNRecievedQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("BVH_TOTAL_PCS")) %>'
                                                                                                                ToolTip='<%# GetFormattedNumberWithSeperation(Eval("BVH_TOTAL_PCS")) %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                                                                        <HeaderStyle CssClass="rate-numeric" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:WalletPcs %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGRNQtyAccepted" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("BVH_TOTAL_PACK_PCS")) %>'
                                                                                                                ToolTip='<%# GetFormattedNumberWithSeperation(Eval("BVH_TOTAL_PACK_PCS")) %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                                                                        <HeaderStyle CssClass="rate-numeric" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:BalanePcs %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblGRNStore" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("BVH_BALANCE_PCS")) %>'
                                                                                                                ToolTip='<%#GetFormattedNumberWithSeperation(Eval("BVH_BALANCE_PCS")) %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="30%" HorizontalAlign="Right" />
                                                                                                        <HeaderStyle CssClass="rate-numeric" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField>
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                </Columns>
                                                                                                <RowStyle CssClass="table-thirdlevel" />
                                                                                                <HeaderStyle CssClass="table-thirdlevela" />
                                                                                            </cc2:ExtGridView>
                                                                                        </div>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <RowStyle CssClass="table-thirdlevel" />
                                                                            <HeaderStyle CssClass="table-thirdlevela" />
                                                                        </cc2:ExtGridView>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="rate-numeric" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <RowStyle CssClass="table-thirdlevel" />
                                                        <HeaderStyle CssClass="table-thirdlevela" />
                                                    </cc2:ExtGridView>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="nopadding" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="table-firstlevel" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                </cc2:ExtGridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <div class="contentwrapper">
                                <div>
                                    <div id="tab1Content">
                                        <div id="divWorkOrder">
                                            <div>
                                                <table class="table-devide">
                                                    <tr>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label ID="lbl_WorkOrderNo" runat="server" AssociatedControlID="lblWorkOrderNo" Text='<%$ Resources:WONo%>'></asp:Label>
                                                                <asp:Label ID="lblWorkOrderNo" runat="server" Text="" CssClass="input-small-a margnrgt0-8per"></asp:Label>
                                                                <div style="width: 18px; display: inline-block;">
                                                                    <asp:ImageButton ID="btnRevision" runat="server" OnClick="ActionHandler" CommandName="REVISIONHISTORY"
                                                                        TabIndex="11" SkinID="history" ToolTip="<%$resources:RevisionHistory %>" />
                                                                </div>
                                                                <asp:Label ID="lblWODate" runat="server" Text='<%$ Resources:WODate%>'
                                                                    CssClass="lbl-16-2perc" AssociatedControlID="txtWODate"></asp:Label>
                                                                <asp:TextBox ID="txtWODate" runat="server" TabIndex="12" onkeydown="return CheckKey(event)"
                                                                    onpaste="return false;" CssClass="input-small"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="wo" EnableClientScript="true" runat="server" ControlToValidate="txtWODate"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                                </asp:RequiredFieldValidator>
                                                                <div class="clear">
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label ID="lblRefNo" runat="server" Text='<%$ Resources:Controls,RefNo%>'
                                                                    AssociatedControlID="txtRefNo"></asp:Label>
                                                                <asp:TextBox ID="txtRefNo" runat="server" MaxLength="50" TabIndex="13" CssClass="input-half-22-1"></asp:TextBox>
                                                                <div class="clear">
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label ID="lblSubContractor" runat="server" Text='<%$resources:SubContractor %>'
                                                                    AssociatedControlID="txtSubContractor"></asp:Label>
                                                                <asp:TextBox ID="txtSubContractor" runat="server" TabIndex="14" CssClass="select-half-22-1"></asp:TextBox>
                                                                <asp:HiddenField ID="hdfSubContractor" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdfSubContractorItem" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdfSubContractorOld" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hdfSubContractorTextOld" runat="server" Value="" />

                                                                <asp:RequiredFieldValidator ID="vrfSubContractor" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="woDtl" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                                    runat="server" ControlToValidate="txtSubContractor" Display="Dynamic" Text="*"
                                                                    ErrorMessage="<%$ resources:Err_SubContractor %>"></asp:RequiredFieldValidator>
                                                                <div class="clear">
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label ID="lblCurrency" runat="server" Text="<%$resources:Currency %>"
                                                                    AssociatedControlID="txtCurrency"></asp:Label>
                                                                <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small" Enabled="false"></asp:TextBox>
                                                                <asp:HiddenField ID="hdfVendorCurrency" runat="server" Value="0" />

                                                                <asp:Label ID="lblItemType" runat="server" Text="<%$ resources:ItemType %>"
                                                                    AssociatedControlID="ddlItemType" CssClass="middle-lbl-b"></asp:Label>
                                                                <asp:DropDownList ID="ddlItemType" runat="server" onchange="javascript:InitWOItem();" AutoPostBack="true"
                                                                    OnSelectedIndexChanged="ActionHandler" TabIndex="15" CssClass="input-small">
                                                                    <%--<asp:ListItem Text="<%$ resources:Material %>" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ resources:Product %>" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="<%$ resources:Brand %>" Value="3"></asp:ListItem>--%>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trCustomerBrand" visible="false">
                                                        <td>
                                                            <div class="div2col-S" runat="server" id="divCustomer">
                                                                <asp:Label ID="lblCustomer" runat="server" Text="<%$ resources:Customer %>"
                                                                    AssociatedControlID="txtCustomer"></asp:Label>
                                                                <asp:TextBox ID="txtCustomer" runat="server" CssClass="select-half-22-1" TabIndex="16"></asp:TextBox>
                                                                <asp:HiddenField ID="hdfCustomerID" runat="server" Value="0" />
                                                                <asp:RequiredFieldValidator ID="reqCustomer" CssClass="star" SetFocusOnError="true" Enabled="false"
                                                                    ValidationGroup="woDtl" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                                    runat="server" ControlToValidate="txtCustomer" Display="Dynamic" Text="*"
                                                                    ErrorMessage="<%$ resources:Err_Customer %>"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="div2col-S" runat="server" id="divBrand">
                                                                <asp:Label ID="Label2" runat="server" Text="<%$ resources:Brand %>"
                                                                    AssociatedControlID="txtBrand"></asp:Label>
                                                                <asp:TextBox ID="txtBrand" runat="server" CssClass="select-half-04-02" TabIndex="17"></asp:TextBox>

                                                                <asp:RequiredFieldValidator ID="reqBrand" CssClass="star" SetFocusOnError="true" Enabled="false"
                                                                    ValidationGroup="woDtl" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                                    runat="server" ControlToValidate="txtBrand" Display="Dynamic" Text="*"
                                                                    ErrorMessage="<%$ resources:Err_Brand %>"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <asp:HiddenField ID="hdfBrand" runat="server" Value="0" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tab2Content">
                                        <h1 class="search-colapse-normal">
                                            <%=Resources.Captions.StoreDetails%>
                                            <img id="imgStoreDtlShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                                alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowHideStoreDtl(1);" />
                                            <img id="imgStoreDtlHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                                alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:ShowHideStoreDtl();" />
                                        </h1>
                                        <div id="divStoreDtl">
                                            <table class="table-devide">
                                                <tr>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label ID="lblLocation" runat="server" Text='<%$Resources:Controls,Location %>'
                                                                AssociatedControlID="ddlLocation"></asp:Label>
                                                            <asp:DropDownList ID="ddlLocation" runat="server" TabIndex="18" CssClass="select-half"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="vrfLocation" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="wo" EnableClientScript="true" InitialValue="-1"
                                                                runat="server" ControlToValidate="ddlLocation" Display="Dynamic" Text="*"
                                                                ErrorMessage="<%$ resources:Err_Location %>"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label ID="lblDeliveryTo" runat="server" Text='<%$Resources:Controls,DeliveryTo %>'
                                                                AssociatedControlID="ddlDeliveryTo"></asp:Label>
                                                            <asp:DropDownList ID="ddlDeliveryTo" runat="server" CssClass="select-half-22-1-1" TabIndex="19"></asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="vrfDeliveryTo" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="wo" EnableClientScript="true" InitialValue="-1"
                                                                runat="server" ControlToValidate="ddlDeliveryTo" Display="Dynamic" Text="*"
                                                                ErrorMessage="<%$ resources:Err_DeliveryTo %>"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    <div id="tab2_1Content">
                                        <h1 class="search-colapse-normal">
                                            <%=Resources.Captions.WODescription%>
                                            <img id="imgDescriptionShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                                alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowHideDescription(1);" />
                                            <img id="imgDescriptionHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                                alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:ShowHideDescription();" />
                                        </h1>
                                        <div id="divDescription">
                                            <table class="table-devide">
                                                <tr>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label runat="server" CssClass="lbl-description-label" ID="lblDescription" Text='<%$ resources:Description %>'
                                                                AssociatedControlID="txtDescription"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" TabIndex="20"
                                                                CssClass="input-full-04-02" Height="30px"></asp:TextBox>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>

                                    <div id="divTabContainer">
                                        <ul id="tab-menu">
                                            <li>
                                                <span id="spnWODList" runat="server" class="tab-active">
                                                    <a href="#" onclick="SwitchTab(1)"><%=Resources.Captions.WODetails %></a>
                                                </span>
                                            </li>
                                            <li>
                                                <span id="spnBOMList" runat="server" class="tab-inactive">
                                                    <a href="#" onclick="SwitchTab(2)" runat="server" id="aTab2"><%=Resources.Captions.MaterialsToBeIssued %></a>
                                                </span>
                                            </li>
                                        </ul>
                                    </div>

                                    <div id="tab3Content">
                                        <%--<h1 class="search-colapse-normal">
                                            <%=Resources.Captions.WODetails%>
                                            <img id="imgWODtlShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                                alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowHideWODtl(1);" />
                                            <img id="imgWODtlHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                                alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:ShowHideWODtl();" />
                                        </h1>--%>
                                        <div id="divWODtl">
                                            <table class="gridwraptable">
                                                <tr>
                                                    <th align="left" style="width: 8%"><%=GetLocalResourceObject("Operation").ToString()%></th>
                                                    <th align="left" style="width: 8%"><%=GetLocalResourceObject("ItemType").ToString()%></th>
                                                    <th align="left" style="width: 28%"><%=GetLocalResourceObject("WOItem").ToString()%></th>
                                                    <th align="left" style="width: 6%"><%=Resources.Controls.UOM %></th>
                                                    <th class="txtAlign-right padgrgt8" style="width: 9%; padding-right: 18px !important;"><%=Resources.Controls.Qty %></th>
                                                    <th class="txtAlign-right padgrgt8" style="width: 9%; padding-right: 18px !important;"><%=Resources.Controls.Rate %></th>
                                                    <th class="txtAlign-right padgrgt8" style="width: 9%"><%=GetLocalResourceObject("Amount").ToString()%></th>
                                                    <th align="left" style="width: 2%"><%=Resources.Controls.ReqDate %></th>
                                                    <th align="left" style="width: 17%"><%=Resources.Controls.Comments %></th>
                                                    <th style="width: 2%"></th>
                                                </tr>
                                                <tr class="grd-rowhead">
                                                    <td>
                                                        <asp:DropDownList ID="ddlOperOrWork" runat="server" TabIndex="21" Width="85%" onchange="javascript:FillMaterialAutoComplete();">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="vrfOperOrWork" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="woDtl" EnableClientScript="true" InitialValue="-1"
                                                            runat="server" ControlToValidate="ddlOperOrWork" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_OperWork %>"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="hdfSlNo" runat="server" Value="0" />
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlWOItemType" runat="server" TabIndex="22" Width="100%" Enabled="false">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWorkOrderItem" runat="server" TabIndex="23" Width="94%">
                                                        </asp:TextBox>
                                                        <asp:HiddenField ID="hdfWorkOrderItem" runat="server" Value="0" />
                                                        <asp:RequiredFieldValidator ID="vrfWorkOrderItem" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="woDtl" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            runat="server" ControlToValidate="txtWorkOrderItem" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_WOItem %>"></asp:RequiredFieldValidator>

                                                        <asp:HiddenField ID="hdfItemCategory" runat="server" />
                                                        <asp:HiddenField ID="hdfItemCategoryPK" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtUOM" runat="server" CssClass="input-disabled" Enabled="false" Width="90%" TabIndex="24"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfItemUOM" runat="server" Value="0" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWOQty" runat="server" CssClass="numeric" Width="80%" TabIndex="25"
                                                            onkeyup="javascript:CalculateAmount();"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfWOQty" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="woDtl" EnableClientScript="true" runat="server" ControlToValidate="txtWOQty"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Qty %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="crfWOQty" CssClass="star" SetFocusOnError="true"
                                                            Type="Double" Operator="GreaterThan" ValueToCompare="0" ValidationGroup="woDtl"
                                                            EnableClientScript="true" InitialValue="0" runat="server" ControlToValidate="txtWOQty"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Qty %>">
                                                        </asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWORate" runat="server" CssClass="numeric" Width="80%" TabIndex="26"
                                                            onkeyup="javascript:CalculateAmount();"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfWORate" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="woDtl" EnableClientScript="true" runat="server" ControlToValidate="txtWORate"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:CompareValidator ID="crfWORate" CssClass="star" SetFocusOnError="true"
                                                            Type="Double" Operator="GreaterThan" ValueToCompare="0" ValidationGroup="woDtl"
                                                            EnableClientScript="true" InitialValue="0" runat="server" ControlToValidate="txtWORate"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                        </asp:CompareValidator>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWOAmt" runat="server" CssClass="numeric input-disabled" TabIndex="27"
                                                            Enabled="false" Width="90%"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWODtlDate" runat="server" onkeydown="return CheckKey(event)" TabIndex="28"
                                                            onpaste="return false;" CssClass="date-picker"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtWODtlRemarks" CssClass="txt-left" runat="server"
                                                            MaxLength="30" TabIndex="29" Width="100%"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDWORKORDERDETAIL" TabIndex="30"
                                                            OnClick="ActionHandler" OnClientClick="javascript: return ValidatePageNow('woDtl') && ShowCustomerConfirmationMsg(this);"
                                                            ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="woDtl"
                                                            SkinID="plus" />
                                                        <asp:HiddenField ID="hdfEdit" runat="server" Value="0" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="gridwrap scroll-container">
                                                <asp:GridView ID="grdWODetails" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" OnRowDataBound="ActionHandler"
                                                    Width="100%" ShowFooter="true" FooterStyle-CssClass="emptyfooter">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:Operation %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWOdtlOperOrWork" runat="server" Text='<%#Eval("Operation") %>'
                                                                    ToolTip='<%#Eval("Operation") %>'></asp:Label>
                                                                <asp:HiddenField runat="server" ID="hdfWOdtlOperOrWorkPK" Value='<%#Eval("OperationPK") %>' />
                                                                <%--<asp:HiddenField ID="hdfWOdtlSlNo" runat="server" Value='<%#Eval("SlNo") %>' />--%>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="8%" Wrap="true" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:WONo %>" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWOdtlNo" runat="server" Text='<%#Eval("WorkOrderNo") %>'
                                                                    ToolTip='<%#Eval("WorkOrderNo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" Wrap="true" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:ItemType %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWOdtlItemType" runat="server" Text='<%#Eval("ItemType") %>'
                                                                    ToolTip='<%#Eval("ItemType") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="8.5%" Wrap="true" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:WOItem %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlItem" runat="server" Text='<%#Eval("Item") %>'
                                                                    ToolTip='<%#Eval("Item") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfWODtlItemPK" runat="server" Value='<%#Eval("ItemPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="29%" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Controls,UOM%>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlUOM" runat="server" Text='<%#Eval("UOM") %>'
                                                                    ToolTip='<%#Eval("UOM") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfWODtlUOMPK" runat="server" Value='<%#Eval("UOMPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,Qty %>" HeaderStyle-CssClass="amount-numeric">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlQty" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedRate(Eval("Quantity")) %>'
                                                                    ToolTip='<%# GetFormattedRate(Eval("Quantity")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,Rate %>" HeaderStyle-CssClass="amount-numeric">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlRate" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedRate(Eval("Rate")) %>'
                                                                    ToolTip='<%# GetFormattedRate(Eval("Rate")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="9%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Amount %>" HeaderStyle-CssClass="amount-numeric">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlAmt" runat="server" Text='<%# GetFormattedRate(Eval("Amount")) %>'
                                                                    ToolTip='<%# GetFormattedRate(Eval("Amount")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,ReqDate %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWODtlReqDate" runat="server" Text='<%# Eval("RequiredDate", Resources.Constants.DateFormatGridExpanded)%>'
                                                                    ToolTip='<%# Eval("RequiredDate", Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="8.5%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,Comments %>" ItemStyle-CssClass="foo">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtWODtlRemark" runat="server" Text='<%#Eval("Remarks") %>' TabIndex="31"
                                                                    ToolTip='<%#Eval("Remarks") %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="6.5%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnWODtlEdit" runat="server" OnClick="ActionHandler" CommandName="EDITGRID"
                                                                    CommandArgument='<%# Eval("SlNo") %>' SkinID="imbeditgrid" />
                                                                <asp:ImageButton ID="btnWODtlRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEWORKORDERDETAIL"
                                                                    CommandArgument='<%# Eval("SlNo") %>' OnClientClick="return ShowDeleteConfirmationMsg(this);"
                                                                    SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="32" />
                                                                <%--OnPreRender="btnAction_PreRender" --%>
                                                                <asp:HiddenField ID="hdfWODtlSlNo" runat="server" Value='<%# Eval("SlNo") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <div id="divCalc">
                                            <div class="gridwrap">
                                                <table id="tblCalc" class="gridwraptable gridwrap">
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label runat="server" ID="lblSubTotal" Text="<%$ resources:SubTotal%>" AssociatedControlID="txtSubTotal"></asp:Label>
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:TextBox ID="txtSubTotal" runat="server" CssClass="input-w97 numeric input-disabled" onchange="CalculateTotal(this);"
                                                                MaxLength="16" TabIndex="47" Enabled="false"></asp:TextBox>
                                                            <div class="starwrap">
                                                                <cc1:AmountValidation ID="vamSubTotal" runat="server" ControlToValidate="txtSubTotal"
                                                                    ErrorMessage="<%$ resources:Err_Valid_SubTotal %>" NumberDigits="12" AllowNegative="true"
                                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="wo"></cc1:AmountValidation>
                                                            </div>
                                                            <div class="clear">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 88%; text-align: right">
                                                            <div class="floatLeft">
                                                            </div>
                                                            <asp:Label runat="server" ID="lblDiscount" Text="<%$ resources:Discounts%>" AssociatedControlID="txtHdrDiscount"></asp:Label>
                                                        </td>
                                                        <td style="text-align: right" class="btn-margin">
                                                            <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                                TabIndex="32" ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCHEADER" />
                                                            <asp:TextBox ID="txtHdrDiscount" runat="server" CssClass="input-w97 numeric input-disabled"
                                                                MaxLength="48" Enabled="false"></asp:TextBox>
                                                            <div class="clear">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <div class="floatLeft" style="display: none;">
                                                                <asp:Label ID="lblHGrossWt" Font-Bold="true" runat="server" Text="<%$ resources:GrossWeight%>"
                                                                    AssociatedControlID="lblHGrossWt"></asp:Label>
                                                                <asp:Label ID="lblGrossWeight" AssociatedControlID="lblGrossWeight" runat="server"></asp:Label>
                                                            </div>
                                                            <asp:Label runat="server" ID="lblShipping" Text="<%$ resources:Shipping%>" AssociatedControlID="txtShipping"></asp:Label>
                                                        </td>
                                                        <td style="text-align: right" class="btn-margin">
                                                            <asp:ImageButton ID="imgShippingCharge" SkinID="shipping" runat="server" OnClick="ActionHandler"
                                                                TabIndex="49" ToolTip="<%$ resources:ShippingCharge %>" CommandName="SHIPPINGHEADER" />
                                                            <asp:TextBox ID="txtShipping" runat="server" CssClass="input-w97 numeric  input-disabled"
                                                                MaxLength="16" Enabled="false" TabIndex="25"></asp:TextBox>
                                                            <div class="starwrap">
                                                                <cc1:AmountValidation ID="vamShipping" runat="server" ControlToValidate="txtShipping"
                                                                    ErrorMessage="<%$ resources:Err_Valid_Shipping %>" NumberDigits="12" Display="Dynamic"
                                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="wo"></cc1:AmountValidation>
                                                            </div>
                                                            <div class="clear">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTax" Text="<%$ resources:Taxes%>" AssociatedControlID="txtHdrTax"></asp:Label>
                                                        </td>
                                                        <td style="text-align: right" class="btn-margin">
                                                            <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                                TabIndex="50" ToolTip="<%$ resources:Tax %>" CommandName="TAXHEADER" />
                                                            <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w97 numeric input-disabled"
                                                                Enabled="false" MaxLength="16"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:PriceAdj%>" AssociatedControlID="txtPriceAdj"></asp:Label>
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:TextBox ID="txtPriceAdj" runat="server" CssClass="input-w97 numeric" onchange="CalculateTotal(this);"
                                                                MaxLength="16" TabIndex="51"></asp:TextBox>
                                                            <div class="starwrap">
                                                                <cc1:AmountValidation ID="vamPriceAdj" runat="server" ControlToValidate="txtPriceAdj"
                                                                    ErrorMessage="<%$ resources:Err_Valid_PriceAdj %>" NumberDigits="12" AllowNegative="true"
                                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="wo"></cc1:AmountValidation>
                                                            </div>
                                                            <div class="clear">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="text-align: right">
                                                            <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:GrandTotal%>" AssociatedControlID="txtHdrTotal"></asp:Label>
                                                        </td>
                                                        <td style="text-align: right">
                                                            <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w97 numeric input-disabled"
                                                                Enabled="false" MaxLength="52"></asp:TextBox>
                                                            <div class="clear">
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tab4Content">
                                        <%--<h1 class="search-colapse-normal">
                                            <%=Resources.Captions.MaterialsToBeIssued%>
                                            <img id="imgBOMShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                                alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowHideBOM(1);" />
                                            <img id="imgBOMHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                                alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:ShowHideBOM();" />
                                        </h1>--%>
                                        <div id="divBOM">
                                            <table class="gridwraptable">
                                                <tr>
                                                    <th align="left" style="width: 8%"><%=GetLocalResourceObject("Operation").ToString() %></th>
                                                    <th align="left" style="width: 8%"><%=GetLocalResourceObject("ItemType").ToString() %></th>
                                                    <th align="left" style="width: 20%"><%=GetLocalResourceObject("WOItem").ToString() %></th>
                                                    <th align="left" style="width: 20%"><%=Resources.Controls.Category %></th>
                                                    <th align="left" style="width: 20%"><%=GetLocalResourceObject("IssueItem").ToString() %></th>
                                                    <th align="left" style="width: 5%"><%=Resources.Controls.UOM %></th>
                                                    <th class="txtAlign-right padgrgt8" style="width: 9%; padding-right: 20px !important;"><%=Resources.Controls.Qty %></th>
                                                    <%----%>
                                                    <th align="left" style="width: 8%"><%=GetLocalResourceObject("In").ToString() %></th>
                                                    <th style="width: 2%"></th>
                                                </tr>
                                                <tr class="grd-rowhead">
                                                    <td>
                                                        <asp:DropDownList ID="ddlBOMOperWork" runat="server" TabIndex="33" Width="85%" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="vrfBOMOperWork" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="-1"
                                                            runat="server" ControlToValidate="ddlBOMOperWork" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_OperWork %>"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBOMItemType" runat="server" TabIndex="34" Width="90%" AutoPostBack="true"
                                                            OnSelectedIndexChanged="ActionHandler">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="vrfBOMItemType" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="-1"
                                                            runat="server" ControlToValidate="ddlBOMItemType" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_ItemType %>"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBOMWOItem" runat="server" TabIndex="35" Width="90%">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="vrfBOMWOItem" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="-1"
                                                            runat="server" ControlToValidate="ddlBOMWOItem" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_WOItem %>"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td id="tdBOMCategoryTxt" runat="server">
                                                        <asp:TextBox ID="txtBOMCategory" runat="server" Width="90%" TabIndex="36"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfBOMCategory" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            runat="server" ControlToValidate="txtBOMCategory" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_Category %>"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="hdfBOMCategory" runat="server" Value="0" />
                                                    </td>
                                                    <td id="tdBOMCategoryDdl" runat="server" visible="false">
                                                        <asp:DropDownList ID="ddlBOMCategory" runat="server" Width="90%" TabIndex="37"
                                                            onchange="javascript:InitProducts();">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="vrfDdlBOMCategory" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            runat="server" ControlToValidate="ddlBOMCategory" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_Category %>"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                                                    </td>
                                                    <td id="tdPMCategory" runat="server" visible="false">
                                                        <asp:TextBox ID="txtBOMPMCategory" runat="server" Width="90%" TabIndex="38"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfBOMPMCategory" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            runat="server" ControlToValidate="txtBOMPMCategory" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_Category %>"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField runat="server" ID="hdfBOMPMCatPK" Value="0" />
                                                    </td>
                                                    <td id="tdBOMItem" runat="server">
                                                        <asp:TextBox ID="txtBOMItem" runat="server" Width="90%" TabIndex="39"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfBOMItem" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            runat="server" ControlToValidate="txtBOMItem" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_Item %>"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField ID="hdfBOMItem" runat="server" Value="0" />
                                                    </td>
                                                    <td id="tdBOMPMItem" runat="server" visible="false">
                                                        <asp:TextBox ID="txtBOMPMItem" runat="server" Width="90%" TabIndex="40"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfBOMPMItem" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            runat="server" ControlToValidate="txtBOMPMCategory" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_Item %>"></asp:RequiredFieldValidator>
                                                        <asp:HiddenField runat="server" ID="hdfBOMPMItem" Value="0" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBOMUOM" runat="server" CssClass="input-disabled" Enabled="false" Width="90%" TabIndex="41"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfBOMUOM" runat="server" Value="0" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtBOMQty" runat="server" CssClass="numeric" Width="80%" TabIndex="42"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfBOMQty" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" runat="server" ControlToValidate="txtBOMQty"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Qty %>">
                                                        </asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlBOMType" runat="server" TabIndex="43" Width="85%">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="vrfBOMType" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="bom" EnableClientScript="true" InitialValue="-1"
                                                            runat="server" ControlToValidate="ddlBOMType" Display="Dynamic" Text="*"
                                                            ErrorMessage="<%$ resources:Err_Type %>"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton runat="server" ID="imbBOMAdd" CommandName="ADDBOM" TabIndex="44"
                                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('bom');"
                                                            ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="scDetails"
                                                            SkinID="plus" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="gridwrap scroll-container">
                                                <asp:GridView ID="grdBOM" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false" OnRowDataBound="ActionHandler"
                                                    Width="100%" ShowFooter="true" FooterStyle-CssClass="emptyfooter">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:Operation %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMOperOrWork" runat="server" Text='<%# Eval("Operation") %>'
                                                                    ToolTip='<%# Eval("Operation") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMOperOrWork" runat="server" Value='<%# Eval("OperationPK") %>' />
                                                                <asp:HiddenField ID="hdfIsBOM" runat="server" Value='<%# Eval("IsBOM") %>' />
                                                                <asp:HiddenField ID="SlNo" runat="server" Value='<%# Eval("SlNo") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="9.3%" Wrap="true" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:ItemType %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMItemType" runat="server" Text='<%# Eval("ItemType") %>'
                                                                    ToolTip='<%# Eval("ItemType") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMItemType" runat="server" Value='<%# Eval("ItemTypePK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" Wrap="true" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:WOItem %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMDtlWOItem" runat="server" Text='<%# Eval("Item") %>'
                                                                    ToolTip='<%# Eval("Item") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMDtlWOItem" runat="server" Value='<%# Eval("ItemPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="31%" Wrap="true" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Category %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMDtlCategory" runat="server" Text='<%# Eval("Category") %>'
                                                                    ToolTip='<%# Eval("Category") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMDtlCategory" runat="server" Value='<%# Eval("CategoryPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:IssueItem %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMDtlItem" runat="server" Text='<%# Eval("Material") %>'
                                                                    ToolTip='<%# Eval("Material") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMDtlItem" runat="server" Value='<%# Eval("MaterialPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Controls,UOM%>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMDtlUOM" runat="server" Text='<%# Eval("UOM") %>'
                                                                    ToolTip='<%# Eval("UOM") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMDtlUOM" runat="server" Value='<%# Eval("UOMPK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,Qty %>" HeaderStyle-CssClass="amount-numeric">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMDtlQty" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedRate(Eval("Quantity")) %>'
                                                                    ToolTip='<%#GetFormattedRate(Eval("Quantity")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,ActualQty %>" HeaderStyle-CssClass="amount-numeric">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtBOMDtlActualQty" runat="server" CssClass="numeric bomDtl-txt-22-1"
                                                                    Text='<%# Convert.ToDecimal(Eval("ActualQuantity")) == 0 ? GetFormattedRate(Eval("Quantity")) : GetFormattedRate(Eval("ActualQuantity")) %>'></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" />
                                                            <%--CssClass="amount-numeric"--%>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,IssueQty %>" HeaderStyle-CssClass="amount-numeric">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIssueQty" runat="server" CssClass="ItemQuantity" Text='<%#GetFormattedRate(Eval("IssueQuantity")) %>'
                                                                    ToolTip='<%#GetFormattedRate(Eval("IssueQuantity")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ Resources:Controls,Remarks %>">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtBOMDtlRemarks" runat="server" Text='<%# Eval("Remarks") %>' Height="15px" TabIndex="45"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="8%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:In %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBOMDtlType" runat="server" Text='<%# Eval("BOMType") %>'
                                                                    ToolTip='<%# Eval("BOMType") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdfBOMDtlType" runat="server" Value='<%# Eval("BOMTypePK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnBOMDtlRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEBOMITEM"
                                                                    CommandArgument='<%# Eval("MaterialPK") %>' OnClientClick="return ShowDeleteConfirmationMsg(this);"
                                                                    SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="46" /><%--OnPreRender="btnAction_PreRender"--%>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="btnBOMAllocate" runat="server" OnClick="ActionHandler" CommandName="ALLOCATE"
                                                                    CommandArgument='<%# Eval("MaterialPK") %>' SkinID="show-carton" ToolTip="<%$ resources:Allocate %>" TabIndex="46"
                                                                    Visible='<%# Convert.ToBoolean(Eval("IsStockExist")) == true ? true : false %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="tab4_1Content">
                                        <h1 class="search-colapse-normal">
                                            <%=Resources.Controls.TermsAndConditions%>
                                            <img id="imgTermsShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                                alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowHideTerms(1);" />
                                            <img id="imgTermsHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                                alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:ShowHideTerms();" />
                                        </h1>
                                        <div id="divTerms">
                                            <div class="div3col-val">
                                                <label for="ddlVendorTerms">
                                                    <%=Resources.Controls.VendorTerms%></label>
                                                <asp:DropDownList ID="ddlVendorTerms" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                    Width="120px" TabIndex="53" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:ImageButton runat="server" ID="imbVendorTerms" SkinID="btnrefresh" OnClientClick="javascript:return ClearTerms('Vendor')"
                                                    Width="16px" TabIndex="54" />
                                                <div class="clear">
                                                </div>
                                                <div class="scroll-h150" style="margin: 0">
                                                    <asp:TextBox runat="server" TextMode="MultiLine" Height="80px" Width="380px" ID="txtVendorTermText"
                                                        EnableTheming="false" TabIndex="55"></asp:TextBox>
                                                </div>
                                                <asp:HiddenField runat="server" ID="hdfVendorTermID" Value="" />
                                                <asp:HiddenField runat="server" ID="hdfVendorTermText" />
                                            </div>
                                            <div class="div3col-val">
                                                <label for="ddlGeneralTerms">
                                                    <%=GetGlobalResourceObject("Controls", "GeneralTerms")%></label>
                                                <asp:DropDownList ID="ddlGeneralTerms" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                    Width="120px" TabIndex="56" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <asp:ImageButton runat="server" ID="imgbtnclear" SkinID="btnrefresh" OnClientClick="javascript:return ClearTerms('General')"
                                                    Width="16px" TabIndex="57" />
                                                <div class="clear">
                                                </div>
                                                <div class="scroll-h150" style="margin: 0">
                                                    <asp:TextBox runat="server" ID="txtGeneralTermText" TextMode="MultiLine" Height="80px" Width="380px"
                                                        EnableViewState="false" EnableTheming="false" TabIndex="58"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfGeneralTermID" runat="server" />
                                                    <asp:HiddenField ID="POH_GROUP" runat="server" />
                                                </div>
                                                <asp:HiddenField runat="server" ID="hdfGeneralTermText" />
                                            </div>
                                            <div class="div3col-val">
                                                <label for="txtComments" class="txt-lft" style="margin-bottom: 14px!important;">
                                                    <%=Resources.Controls.Comments%></label>
                                                <div class="clear">
                                                </div>
                                                <asp:TextBox runat="server" ID="txtComments" TextMode="MultiLine" EnableTheming="false"
                                                    Width="395px" Height="80px" TabIndex="59"></asp:TextBox>
                                            </div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </div>
                                    <div id="tab5Content">
                                        <h1 class="search-colapse-normal">
                                            <%=Resources.Captions.Attachments%>
                                            <img id="imgAttachShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                                alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowHideAttachments(1);" />
                                            <img id="imgAttachHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                                alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:ShowHideAttachments();" />
                                        </h1>
                                        <div id="divAttachments">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblFileUpload" runat="server" Text="Attach File" AssociatedControlID="fupUpload"></asp:Label>
                                                <div class="fileupload-main">
                                                    <asp:FileUpload ID="fupUpload" runat="server" TabIndex="60" CssClass="margn-rgt0 upload-area" />
                                                    <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$resources:Err_File_Upload%>">                                                        
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                                <a id="anchorFile" runat="server" target="_blank" tabindex="61"></a>
                                                <asp:Button runat="server" ID="btnUpload" CommandName="ADDITEMUPLOAD" TabIndex="62"
                                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('upload')"
                                                    CommandArgument="PageAction_Entry" ToolTip="<%$resources:ErpRes,Add%>" ValidationGroup="upload"
                                                    Text="<%$resources:ErpRes,Add %>" SkinID="btnInner-add" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                            <div class="gridwrap">
                                                <asp:GridView runat="server" ID="grdUploads" Width="100%" PageSize="<%$resources:UploadPageSize%>"
                                                    AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                                    OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$resources:Messages,Msg_EmptyGrid%>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$resources:SlNo%>">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                                <asp:HiddenField runat="server" ID="hdfPK" Value='<%#Eval("DOC_PK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$resources:File %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFile" runat="server" Text='<%#Eval("DOC_NAME") %>' ToolTip='<%#Eval("DOC_NAME") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="92%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="file-details">
                                                            <ItemTemplate>
                                                                <a runat="server" id="fileView" class="download-icon nomargin" style="margin-right: -1px!important;"
                                                                    title="<%$resources:View %>" target="_blank" href='<%#Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'></a>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="file-details">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEMUPLOAD" TabIndex="63"
                                                                    SkinID="edit-icon" ToolTip="Edit" Style="margin-right: 3px!important;" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ItemStyle-CssClass="file-details">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEMUPLOAD" TabIndex="64"
                                                                    SkinID="delete-icon" ToolTip="Delete" OnClientClick="return ShowDeleteConfirmationMsg(this);" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="divData">
                            </div>

                            <div id="divWkfSubmit" style="display: none;">
                                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                                <uc1:workflowusercomments id="ucrWrkf" runat="server" validationgroup="wo" />
                            </div>

                            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
                            <div id="divItemTax" style="display: none">
                                <div class="Button-container-popup">
                                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                                        CommandArgument="PageAction_Entry" CommandName="TAXAPPLY" TabIndex="65" />
                                </div>
                                <div class="content-wrapper">
                                    <%-- *********************Checkbox Region Start*******************************************************************--%>
                                    <div id="divTaxApplicableAmount" style="display: none" runat="server">
                                        <label for="chkSubTotal" style="width: 148px">
                                            <%=Resources.Controls.SubTotal%>
                                        </label>
                                        <asp:CheckBox ID="chkSubTotal" runat="server" Checked="false" OnCheckedChanged="ActionHandler"
                                            AutoPostBack="true" />
                                        <label for="chkDiscount" style="width: 120px">
                                            <%=Resources.Controls.Discounts%>
                                        </label>
                                        <asp:CheckBox ID="chkDiscount" runat="server" Checked="false" OnCheckedChanged="ActionHandler"
                                            AutoPostBack="true" />
                                        <label for="chkOtherCharges" style="width: 125px">
                                            <%=Resources.Controls.OtherCharges%>
                                        </label>
                                        <asp:CheckBox ID="chkOtherCharges" runat="server" Checked="true" OnCheckedChanged="ActionHandler"
                                            AutoPostBack="true" />
                                    </div>
                                    <%--   *******************End Check Box Region ****************************************************************--%>
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:HiddenField ID="hdfTaxFormula" runat="server" />
                                                    <asp:Label ID="lblPopupItemAmount" runat="server" Text="<%$ resources:Amount %>"
                                                        AssociatedControlID="txtPopupItemAmount"></asp:Label><asp:TextBox ID="txtPopupItemAmount"
                                                            CssClass="input-w70 numeric" runat="server" EnableViewState="false" Enabled="false"
                                                            MaxLength="11"></asp:TextBox><div class="clear">
                                                            </div>
                                                    <div runat="server" id="dvPerc">
                                                        <asp:Label ID="lblPerc" runat="server" Text="<%$ resources:DiscPerc %>" AssociatedControlID="txtTaxPerc"></asp:Label><asp:TextBox
                                                            ID="txtTaxPerc" TabIndex="66" runat="server" CssClass="input-w70 numeric" EnableViewState="false"
                                                            MaxLength="15" onChange="CalcTax();"></asp:TextBox>
                                                    </div>
                                                    <asp:Label ID="lblPopupAmount" runat="server" Text="<%$ resources:Charge %>" AssociatedControlID="txtPopupAmount"></asp:Label><asp:TextBox
                                                        ID="txtPopupAmount" TabIndex="67" runat="server" CssClass="input-w70 numeric"
                                                        EnableViewState="false" Enabled="false" MaxLength="15"></asp:TextBox><div class="starwrap">
                                                            <asp:RequiredFieldValidator ID="vrfTaxAmt" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupAmount"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                            </asp:RequiredFieldValidator><cc1:AmountValidation ID="vamTaxAmt" runat="server"
                                                                ControlToValidate="txtPopupAmount" ErrorMessage="<%$ resources:Err_Amount_Valid %>"
                                                                NumberDigits="11" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                                ValidationGroup="tax"></cc1:AmountValidation>
                                                        </div>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-P">
                                                    <asp:Label ID="lblPopupTaxType" runat="server" Text="Type" AssociatedControlID="ddlPopupTaxType"></asp:Label>
                                                    <asp:DropDownList ID="ddlPopupTaxType" TabIndex="68" runat="server" CssClass="medium" EnableViewState="true"
                                                        OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                                    </asp:DropDownList>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblPopupOther" runat="server" Text="Name" AssociatedControlID="txtPopupOther"></asp:Label><asp:TextBox
                                                        ID="txtPopupOther" runat="server" TabIndex="69" CssClass="medium" EnableViewState="false"
                                                        MaxLength="100" Enabled="false"></asp:TextBox><asp:RequiredFieldValidator ID="vrfPopupOther"
                                                            CssClass="star" SetFocusOnError="true" ValidationGroup="tax" EnableClientScript="true"
                                                            runat="server" ControlToValidate="txtPopupOther" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Name %>">
                                                        </asp:RequiredFieldValidator><asp:ImageButton ID="imgPopupAdd" SkinID="imbaddnew"
                                                            runat="server" OnClick="ActionHandler" CommandArgument="PageAction_Entry" ValidationGroup="tax"
                                                            CommandName="TAXADD" TabIndex="57" OnClientClick="javascript:ValidatePageNow('tax')" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdTaxDetails" Width="100%" AllowSorting="false"
                                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("TaxName") %>' ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("TaxName"))) %>'></asp:Label><asp:HiddenField
                                                            ID="hdfTaxName" runat="server" Value='<%#Eval("TaxName") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Type">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("TaxHeaderPK") %>' />
                                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("TaxPK") %>' />
                                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("TaxName")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("TaxName"))) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode( HttpUtility.HtmlDecode(Convert.ToString(Eval("TaxName")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("TaxName")))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Discount%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDiscPer" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("DiscountPercentage")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("DiscountPercentage")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("TaxAmount")) %>'
                                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("TaxAmount")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="TAXDELETE"
                                                            TabIndex="70" CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender"
                                                            SkinID="btnclose" ToolTip="Remove" /><%--OnLoad="btnAction_Load"--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                            <%--  Shortclose region Start--------------------%>
                            <div id="divShortClose" title="<%=Resources.Controls.ShortClose%>" style="display: none">
                                <div class="divcolmiddle-S">
                                    <label for="lblWONum"><%=GetLocalResourceObject("WONo").ToString()%></label>
                                    <asp:Label ID="lblWIH_NO" class="lbl-22perc" runat="server"></asp:Label>
                                    <label for="Remarks">
                                        <%=Resources.Controls.Remark%>*</label>
                                    <asp:TextBox runat="server" ID="Remarks" TabIndex="71" MaxLength="200" TextMode="MultiLine" Height="40px">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqRemarks" CssClass="star" SetFocusOnError="true" EnableClientScript="true"
                                        ValidationGroup="CancelWO" runat="server" ControlToValidate="Remarks" Display="Dynamic"
                                        Text="*" ErrorMessage="<%$ resources:EnterRemarks %>">
                                    </asp:RequiredFieldValidator>
                                    <label for="RefNo">
                                        <%=Resources.Controls.RefNo%></label>
                                    <asp:TextBox runat="server" ID="RefNo" TabIndex="72" MaxLength="14">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lbnSpace" runat="server" AssociatedControlID="btnWOCancel"></asp:Label>
                                    <asp:HiddenField ID="WOID" runat="server" Value="0"></asp:HiddenField>
                                    <asp:Button runat="server" ID="btnWOCancel" Text="<%$ resources:ShortClose %>" CommandName="CANCELWO" OnClick="ActionHandler" ValidationGroup="CancelWO"
                                        class="inputbtn" Width="100px" Height="20px" OnClientClick="ValidatePageNow('CancelWO');" TabIndex="73" />
                                    <div class="clear">
                                    </div>
                                </div>
                            </div>
                            <%-- End Shortclose region -----------------%>

                            <%--  RevisionHistory Start--------------------%>
                            <div id="divRevisionHistory" title="<%=Resources.Captions.RevisionHistory%>" style="display: none">
                                <div class="content-wrapper">
                                    <%--<table rules="all" id="grdRevisionHistory" grandtype="GrandGrid" paging="false" editable="false"
                                        class="gridwraptable gridwrap">
                                        <thead>
                                            <tr>
                                                <th fieldmap="POH_PK" isvisible="false"></th>
                                                <th fieldmap="POH_VERSION" isvisible="false"></th>
                                                <th fieldmap="POH_DATE" align="left" width="30%">
                                                    <%=Resources.Controls.RevDate%>
                                                </th>
                                                <th fieldmap="POH_NO" align="left" width="32%">
                                                    <%=Resources.Controls.PoNumber%>
                                                </th>
                                                <th fieldmap="POH_CURRENCY_TEXT" align="left" width="10%">
                                                    <%=Resources.Controls.Currency%>
                                                </th>
                                                <th fieldmap="POH_TOTAL_VALUE" align="right" width="28%">
                                                    <%=Resources.Controls.TotalAmount%>
                                                </th>
                                            </tr>
                                        </thead>
                                    </table>--%>
                                    <asp:GridView ID="grdRevisionHistory" runat="server" Width="100%" AutoGenerateColumns="false">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$resources:Messages,Msg_EmptyGrid%>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$resources:Controls,RevDate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRHDate" runat="server" Text='<%#Eval("WIH_DATE") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="30%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:WONo %>">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkRHWONO" runat="server" Text='<%#Eval("WIH_NO") %>'
                                                        OnClick="ActionHandler" CommandName="HISTORYPRINT" CommandArgument='<%#Eval("WIH_PK") %>'></asp:LinkButton>
                                                    <%--<asp:Label ID="lblRHWONo" runat="server" Text='<%#Eval("WIH_NO") %>'></asp:Label>--%>
                                                    <asp:HiddenField ID="hdfRHWOPK" runat="server" Value='<%#Eval("WIH_PK") %>' />
                                                    <asp:HiddenField ID="hdfRHWODate" runat="server" Value='<%#Eval("WIH_DATE") %>' />
                                                    <asp:Label ID="lblVersion" runat="server" Text='<%#Eval("WIH_VERSION") %>' Visible="false" />
                                                </ItemTemplate>
                                                <ItemStyle Width="32%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Controls,Currency %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRHCurrency" runat="server" Text='<%#Eval("WIH_CURRENCY_TEXT") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Controls,TotalAmount %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRHAmount" runat="server" Text='<%#Eval("WIH_NET_TOTAL") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="28%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <%--  RevisionHistory End--------------------%>

                            <%--Allocation div Start--%>
                            <div id="divAllocation" style="display: none">
                                <div class="content-wrapper">
                                    <asp:Panel runat="server" ID="pnlPopup" Style="max-height: 350px; overflow-y: auto;">
                                        <div class="Button-container-popup">
                                            <asp:Button ID="btnApplyBatches" SkinID="btnInner-add-dsd" runat="server" Text="<%$ Resources:Apply %>"
                                                OnClick="ActionHandler" CommandName="APPLY" />
                                        </div>
                                        <table style="font-weight: bold; background: #f1f1f1; margin-bottom: 10px;">
                                            <tr>
                                                <td>
                                                    <asp:Label runat="server" ID="lblItemPopup" Text="" CssClass="hgt-auto"></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfItemPopup" Value="0"></asp:HiddenField>
                                                    <asp:HiddenField runat="server" ID="HiddenField2" Value="0" />
                                                </td>
                                                <td style="text-align: right; min-width: 26%;">

                                                    <asp:Label runat="server" ID="lblForStock" Text="<%$ Resources:Stock %>"></asp:Label>
                                                    <asp:Label runat="server" ID="lblTotalStock" Text="<%$ Resources: Controls,Stock %>"></asp:Label>
                                                    |
                                                    <asp:Label runat="server" ID="lblUomName" Text="<%$ Resources:TextUom %>"></asp:Label>
                                                    <asp:Label runat="server" ID="lblUomPopup" Text="" CssClass="hgt-auto"></asp:Label>

                                                    <asp:HiddenField runat="server" ID="hdfUOMPopup" Value="0"></asp:HiddenField>
                                                </td>
                                            </tr>
                                        </table>
                                        <table class="gridwraptable gridwrap filter-arrow" style="margin-bottom: -4px!important;">
                                            <tr>
                                                <th width="34%">
                                                    <%=GetGlobalResourceObject("Controls","BatchNo")%>
                                                    <asp:RequiredFieldValidator ID="vrfBatchesPopUp" SetFocusOnError="true" ValidationGroup="newentryPopup"
                                                        CssClass="star" EnableClientScript="true" runat="server" ControlToValidate="ddlBatchesPopUp"
                                                        Text="*" ErrorMessage="<%$ resources:Msg_SelectBatch %>" Display="Dynamic" InitialValue="-1"></asp:RequiredFieldValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:DropDownList runat="server" ID="ddlBatchesPopUp" Width="210px" TabIndex="1"
                                                        OnSelectedIndexChanged="ActionHandler" AutoPostBack="true" onchange="Page_BlockSubmit = false;"
                                                        ValidationGroup="none">
                                                    </asp:DropDownList>
                                                </th>
                                                <th class="grd-head-rgt" style="vertical-align: top;" width="38%">
                                                    <%=GetGlobalResourceObject("Controls","Stock")%>
                                                    <div class="clear">
                                                    </div>
                                                    <div>
                                                        <asp:Label runat="server" ID="lblStockPopUp" Text=""></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdfBatchPopupUOM" />
                                                    </div>
                                                </th>
                                                <th class="grd-head-rgt" width="22%">
                                                    <%=GetGlobalResourceObject("Controls","Quantity")%>
                                                    <asp:RequiredFieldValidator ID="vrcLatexQtyPopUp" SetFocusOnError="true" ValidationGroup="newentryPopup"
                                                        CssClass="star" EnableClientScript="true" runat="server" ControlToValidate="txtQtyPopUp"
                                                        Text="*" ErrorMessage="<%$ resources:Msg_ActualQty %>" Display="Dynamic"></asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="vcfLatexQtyPopUp" runat="server" ErrorMessage="<%$ resources:QtymustbeGreaterZero %>"
                                                        Operator="GreaterThan" CssClass="star" ValueToCompare="0" ValidationGroup="newentryPopup"
                                                        Text="*" SetFocusOnError="true" ControlToValidate="txtQtyPopUp" Display="Dynamic"
                                                        Type="Double"></asp:CompareValidator>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:TextBox runat="server" ID="txtQtyPopUp" Style="float: left;" CssClass="numeric input-w110"
                                                        TabIndex="2" autocomplete="off" MaxLength="14" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                                                </th>
                                                <th width="6%" style="vertical-align: top;">
                                                    <%=GetGlobalResourceObject("Controls","Action")%>
                                                    <div class="clear">
                                                    </div>
                                                    <div style="text-align: center; margin-top: 2px;">
                                                        <asp:ImageButton runat="server" ID="imgAddBatchPopup" SkinID="imbaddnew" OnClick="ActionHandler"
                                                            TabIndex="3" CommandName="ADDBATCHES" ValidationGroup="newentryPopup" OnClientClick="javascript:return ValidatePageNow('newentryPopup')" />
                                                    </div>
                                                    <asp:HiddenField runat="server" ID="hdnPopupstatus" Value="0" />
                                                </th>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="grdBatchDetailsPopup" runat="server" CssClass="grdTable" AutoGenerateColumns="False"
                                            AllowPaging="false" HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-HorizontalAlign="Center"
                                            ShowHeader="false" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true"
                                            OnRowDataBound="ActionHandler" OnRowCommand="ActionHandler">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBatchNoGridPopup" runat="server" Text='<%# Eval("Batch") %>'></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdfBatchPopup" Value='<%# Eval("BatchPK") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="37%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockGridPopup" runat="server" Text='<%# GetFormattedRate(Eval("StockQty")) %>'
                                                            CssClass="numeric"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="37%" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblQtyGridPopupTotal" runat="server" Text="<%$ resources:totalbatchqty %>"
                                                            CssClass="numeric" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQtyGridPopup" runat="server" Text='<%# GetFormattedRate(Eval("ActualQty")) %>'
                                                            CssClass="numeric"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblPopupFooterTotal" runat="server" CssClass="numeric" />
                                                    </FooterTemplate>
                                                    <ItemStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" Style="margin-left: 21%;"
                                                            runat="server" ID="btnDeletepopupBatches" TabIndex="4" SkinID="imbdeletegrid"
                                                            OnClick="ActionHandler" CommandName="DELETE_ACTION" ToolTip="<%$ Resources: Controls,Delete %>"
                                                            OnClientClick="return ShowDeleteConfirm(this);" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="6%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <table style="font-weight: bold; background: #f1f1f1;">
                                            <tr>
                                                <td style="width: 45%;"></td>
                                                <td style="width: 27%; text-align: right;">
                                                    <asp:Label runat="server" ID="Label13" Text="<%$ resources:RequiredQty %>" CssClass="numeric"></asp:Label>
                                                </td>
                                                <td style="width: 21%; text-align: right;">
                                                    <asp:Label runat="server" ID="lblTotalQtyPopup" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 7%;"></td>
                                            </tr>
                                        </table>
                                        <table style="font-weight: bold;">
                                            <tr>
                                                <td style="width: 45%;"></td>
                                                <td style="width: 27%; text-align: right;">
                                                    <asp:Label runat="server" ID="Label14" Text="<%$ resources:balRequired %>" CssClass="numeric"></asp:Label>
                                                </td>
                                                <td style="width: 21%; text-align: right;">
                                                    <asp:Label runat="server" ID="lblBalanceQty" Text=""></asp:Label>
                                                </td>
                                                <td style="width: 7%;"></td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </div>
                            </div>

                            <div id="divIssueDetails" style="display: none">
                                <div class="content-wrapper">
                                    <asp:Panel runat="server" ID="pnlIssueDetails" Style="max-height: 350px; overflow-y: auto;">
                                        <div class="Button-container-popup">
                                            <asp:Button runat="server" ID="imbPrintIssue" CommandName="PRINTPOPUP" OnClick="ActionHandler"
                                                Text="<%$resources:Controls,Print %>" SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" />
                                        </div>
                                        <asp:GridView ID="grdIssueDetails" runat="server" Width="100%" AutoGenerateColumns="false"
                                            EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$resources:Messages,Msg_EmptyGrid%>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkSelectAllIssue" runat="server" onclick="CheckAllIssues(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelectIssue" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$resources:IssueNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssueNo" runat="server" Text='<%# Eval("ISSUE_NO") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfIssuePK" runat="server" Value='<%# Eval("ISSUE_PK") %>'></asp:HiddenField>
                                                        <asp:HiddenField ID="hdfItemType" runat="server" Value='<%# Eval("WIB_ITEM_TYPE") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$resources:IssueDate %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssueDate" runat="server" Text='<%# Eval("ISSUE_DATE") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$resources:IssueItem %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssueItem" runat="server" Text='<%# Eval("ISSUE_ITM_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="50%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$resources:Qty %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWOIssueQty" runat="server" Text='<%#Eval("ISSUE_QTY") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton runat="server" ID="imbIssuePrint" ToolTip="<%$Resources:Controls,Print %>" OnClick="ActionHandler"
                                                            TabIndex="14" SkinID="btnPrint" CommandName="PRINTRECORD" CommandArgument='<%# Eval("ISSUE_PK") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </div>

                            <div id="divStockAdjust" style="display: none">
                                <div class="content-wrapper">
                                    <asp:Panel runat="server" ID="Panel1" Style="max-height: 350px; overflow-y: auto;">
                                        <div class="Button-container-popup">
                                            <asp:Button runat="server" ID="btnSaveStockAdjust" CommandName="ITEMSAVE" OnClick="ActionHandler"
                                                Text="<%$resources:Controls,Save %>" SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Save %>" />
                                        </div>
                                        <h1 class="search-colapse-normal">
                                            <asp:Label ID="lblStockAdjustWONo" runat="server" Text=""></asp:Label>
                                            <asp:HiddenField ID="hdfStockAdjustWOPK" runat="server" Value="0" />
                                        </h1>
                                        <asp:GridView ID="grdWOPendingStock" runat="server" Width="100%" AutoGenerateColumns="false"
                                            EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$resources:Messages,Msg_EmptyGrid%>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$resources:Batch %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBatch" runat="server" Text='<%# Eval("BatchNo") %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfBatch" runat="server" Value='<%# Eval("BatchPK") %>' />
                                                        <asp:HiddenField ID="hdfRowID" runat="server" Value='<%# Eval("SlNo") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$resources:Type %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("ItemTypeText").ToString() %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfType" runat="server" Value='<%# Eval("ItemType") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="20%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$resources:IssuedQty %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIssuedQty" runat="server" Text='<%# GetFormattedRate(Eval("IssuedQty")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$resources:ConsumedQty %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblConsumedQty" runat="server" Text='<%# GetFormattedRate(Eval("ConsumedQty")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$resources:ReturnedQty %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReturnedQty" runat="server" Text='<%# GetFormattedRate(Eval("ReturnedQty")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$resources:ActualStock %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActualStock" runat="server" Text='<%# GetFormattedRate(Eval("BalanceQty")) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfAdjusted" runat="server" Value='<%# GetFormattedRate(Eval("AdjustedQty")) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$resources:AdjustNow %>" HeaderStyle-CssClass="amount-numeric">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtAdjustNow" runat="server" Text='<%# GetFormattedRate(Eval("AdjustNow")) %>'
                                                            CssClass="input-w97 numeric" onblur="UpdateAdjustNow(this);" data-RowID='<%# Eval("SlNo") %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <label for="Remarks"><%=Resources.Controls.Remark%>*</label>
                                        <asp:TextBox runat="server" ID="txtRemarks" TabIndex="71" MaxLength="200" TextMode="MultiLine" 
                                            Height="40px" CssClass="input-full">
                                        </asp:TextBox>
                                    </asp:Panel>
                                </div>
                            </div>

                            <div id="diverror" style="display: none">
                                <%--Use this label to bind the server errors--%>
                                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>

                                <asp:ValidationSummary ID="vsPage" ValidationGroup="wo" runat="server" />
                                <asp:ValidationSummary ID="vsDetails" ValidationGroup="woDtl" runat="server" />
                                <asp:ValidationSummary ID="vsBOM" ValidationGroup="bom" runat="server" />
                                <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                                <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                                <asp:ValidationSummary ID="vsSalesCost" ValidationGroup="scCost" runat="server" />
                                <asp:ValidationSummary ID="vsOrderQty" ValidationGroup="OrderQty" runat="server" />
                                <asp:ValidationSummary ID="vsCancelWO" ValidationGroup="CancelWO" runat="server" />
                                <asp:ValidationSummary ID="vsAllocation" ValidationGroup="newentryPopup" runat="server" />
                            </div>

                            <div id="hiddenButtons" style="display: none">
                                <asp:Button ID="btnVendor" runat="server" OnClick="ActionHandler" CommandName="VENDORMAPPING" />
                                <asp:Button ID="btnEdit" runat="server" OnClick="ActionHandler" CommandName="EDIT" />
                                <asp:Button ID="btnContractorSelected" runat="server" OnClick="ActionHandler" CommandName="VENDORSELECTED" />
                                <asp:Button ID="Button1" runat="server" OnClick="ActionHandler" CommandName="VENDORSELECTED" />
                                <asp:Button ID="btnCalculate" runat="server" CommandName="CALCULATE" OnClick="ActionHandler" />
                            </div>

                            <asp:HiddenField ID="hdfCurrencyDigits" runat="server" />
                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                            <asp:HiddenField ID="hdfDecimalFormatWithComma" runat="server" />
                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                            <asp:HiddenField ID="hdfRateDecimalDigits" Value="3" runat="server" />
                            <asp:HiddenField ID="hdfIsSBUVendor" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfExchangeRate" runat="server" Value="-1" />
                            <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfCloseWO" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfIsAmend" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfWORefID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfWorkOrderProcessID" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfRevistoryText" runat="server" Value="" />
                            <asp:HiddenField ID="hdfBaseCurrency" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfTabNo" runat="server" Value="1" />
                            <asp:HiddenField ID="hdfHasAllocation" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfFullyAllocated" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfIsDataAdded" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfSelectedItemWOPK" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfDisableWOItem" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfIsViewer" runat="server" Value="0" />

                            <asp:HiddenField ID="hdfAdjustRowId" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfAdjustNow" runat="server" Value="0" />

                        </asp:TableCell>
                    </asp:TableRow>

                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>

                </asp:Table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
