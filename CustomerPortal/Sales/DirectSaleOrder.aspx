<%@ Page Title="<%$ Resources:Captions, Title_DirectSaleOrder %>" Language="C#" AutoEventWireup="true"
    MasterPageFile="~/ERPSMS_2.Master" CodeBehind="DirectSaleOrder.aspx.cs" Inherits="CustomerPortal.Sales.DirectSaleOrder"
    ValidateRequest="false" Theme="ClassicExt" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="vc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="pc1" %>
<%--<%@ Register Src="~/UserControls/AlertControl.ascx" TagName="Alert" TagPrefix="uc2" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* Sales Contract - Discount Percentage */
        .div2col-P .divpercentage
        {
            width: 132px;
            float: left;
        }
        .div2col-P .divpercentage .dvperc-lbl
        {
            width: 96px;
            margin-right: 0px !important;
        }
        .div2col-P .divpercentage .dvperc-inpt
        {
            width: 25px !important;
            margin-right: 0px !important;
        }
    </style>
    <script type="text/javascript">

        var NumberDigits = 0;
        var CurrencyDigits = 0;
        var rateDigits = 0;
        var ExchangeRateDigits = 0;
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
            rateDigits = parseInt($("[id$=hdfRateDigits]").val());
            ExchangeRateDigits = parseInt($("[id$=hdfExchangeRateDigits]").val());
            $("[id$=hdfPreviousUrl]").val(document.referrer);
            $("[id$=txtSaleOrderDate]").focus();
        });

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtSaleOrderDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtDeliveryDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtReqDate", false, false);

            GrandScriptUtils.DatePickerCommon("txtFromDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtAdvCustomerSrch", uiUrl, "hdfAdvCustomerPKSrch", true, true, "CUSTOMER");

            var pageURLSO = '<%= Resources.PageURL.DirectSaleOrderURL %>';
            GrandScriptUtils.MakeAutoCompleteDDL("txtDSONumber", uiUrl + "?Type=SOH_NO" + "&PAGE_URL=" + pageURLSO, "hdfDSONoPK", true, true, "GetDirectSONoAuto");

            $("[id*=txtExchangeRate]").ForceNumericOnly();
            $("[id*=txtItemQuantity]").ForceNumericOnly();
            $("[id*=txtPopupAmount]").ForceNumericOnly();
            $("[id*=txtPopupCharge]").ForceNumericOnly();
            $("[id*=txtUnitPrice]").ForceNumericOnly();

            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", uiUrl, "hdfCustomer", true, true, "CUSTOMER");
            if ($("[id$=txtCustomer]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }

            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", uiUrl, "hdfCurrency", true, true, "CURRENCY");
            if ($("[id$=txtCurrency]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }

            GrandScriptUtils.MakeAutoCompleteDDL("txtItemCategory", uiUrl, "hdfItemCategory", true, true, "GETITEMCATEGORYSALE");
            GrandScriptUtils.MakeAutoCompleteDDL("txtPackingSpec", uiUrl, "hdfPackingSpec", true, true, "PACKINGSPEC");
            SetItemAutoComplete();
            ShowHideorderDetails($("[id$=hdfIsOrderDetailsVisible]").val());
            ShowHideAttachDocs($("[id$=hdfIsAttachDocsVisible]").val());

            $('a[disabled=disabled]').click(function () { return false; });

            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            if ($("[id$=hdfCustomer]").val() == "" || $("[id$=hdfCustomer]").val() == "0") {
                DisableAuto($("[id$=txtItemCategory]"), $("[id$=hdfItemCategory]"));
            }

            if ($("[id$=hdfItemCatVal]").val() != "9" || ($("[id$=hdfItemCategory]").val() == '' || $("[id$=hdfItemCategory]").val() == '0')) {
                DisableAuto($("[id$=txtPackingSpec]"), $("[id$=hdfPackingSpec]"));
            }

            if ($("[id$=hdfNeedPackSpecVal]").val() == "1" && ($("[id$=hdfPackingSpec]").val() == '' || $("[id$=hdfPackingSpec]").val() == '0')) {
                DisableAuto($("[id$=txtItem]"), $("[id$=hdfItem]"));
            }
            else if ($("[id$=hdfNeedPackSpecVal]").val() == "0" && ($("[id$=hdfItemCategory]").val() == '' || $("[id$=hdfItemCategory]").val() == '0')) {
                DisableAuto($("[id$=txtItem]"), $("[id$=hdfItem]"));
            }
        }

        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=btnCustomerSelected]").click();
            }
            else if (targetControlID == "txtItemCategory") {
                $("[id$=btnSelectItemCategory]").click();
                EnableAuto($("[id$=txtItem]"));
                SetItemAutoComplete();
            }
            else if (targetControlID == "txtPackingSpec") {
                $("[id$=txtItem]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfItem]").val("0");
                EnableAuto($("[id$=txtItem]"));
                SetItemAutoComplete();
            }
            else if (targetControlID == "txtItem") {
                $("[id$=btnItemSelected]").click();
                //EnableAuto($("[id$=txtItem]"));
            }
        }
        function EnableItemAutoComplete() {
            EnableAuto($("[id$=txtItem]"));
            SetItemAutoComplete();
        }
        function SetItemAutoComplete() {
            var itmCategory = 0;
            var packSpec = 0;
            var SubType = 0;
            if ($("[id$=hdfItemCategory]").val() != "" && $("[id$=hdfItemCategory]").val() != "0") {
                itmCategory = $("[id$=hdfItemCategory]").val()
            }
            if ($("[id$=hdfPackingSpec]").val() != "" && $("[id$=hdfPackingSpec]").val() != "0") {
                packSpec = $("[id$=hdfPackingSpec]").val();
            }
            if ($("select[id$=ddlProdSubCategory]").val() != "" && $("select[id$=ddlProdSubCategory]").val() != "0") {
                SubType = $("select[id$=ddlProdSubCategory]").val();
            }
            GrandScriptUtils.MakeAutoCompleteDDL("txtItem", uiUrl + "?ItmCategory=" + itmCategory + "&PackSpec=" + packSpec + "&SubType=" + SubType, "hdfItem", true, true, "GETITEMSALE");
        }
        function DisableAuto(extender, hfield) {
            ///<summary>
            /// Used to disable Autocomplete
            ///</summary>
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }

        function EnableAuto(extender) {
            ///<summary>
            /// Used to enable Autocomplete
            ///</summary>
            $(extender).removeAttr("disabled");
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
        }

        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=hdfUOM]").val("0");
                $("[id$=txtUOM]").val("");
                $("[id$=txtDiscount]").val("");
                $("[id$=txtAmount]").val("");
                $("[id$=txtTax]").val("");
                $("[id$=txtLotSize]").val("");
                $("[id$=txtDtlRemark]").val("");
                $("[id$=btnCustSelected]").click();
                $("[id$=txtCurrency]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfCurrency]").val("0");
                $("[id$=txtItem]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfItem]").val("0");
                $("[id$=txtItemCategory]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfItemCategory]").val("0");
                $("[id$=txtPackingSpec]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfPackingSpec]").val("0");
                DisableAuto($("[id$=txtItem]"), $("[id$=hdfItem]"));
                DisableAuto($("[id$=txtItemCategory]"), $("[id$=hdfItemCategory]"));
                DisableAuto($("[id$=txtPackingSpec]"), $("[id$=hdfPackingSpec]"));
            }
            else if (targetControlID == "txtCurrency") {
                $("[id$=txtCurrency]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfCurrency]").val("0");
                $("[id$=btnCurrency]").click();
            }
            else if (targetControlID == "txtItemCategory") {
                $("[id$=txtItemCategory]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfItemCategory]").val("0");
                $("[id$=txtItem]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfItem]").val("0");
                SetItemAutoComplete();
            }
            else if (targetControlID == "txtPackingSpec") {
                $("[id$=txtPackingSpec]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfPackingSpec]").val("0");
                $("[id$=txtItem]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfItem]").val("0");
                SetItemAutoComplete();
            }
        }

        function GoBack() {
            var backURL = document.referrer;
            backURL = backURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
            window.location = backURL;
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
                //InitComponents();
            }
            return false;
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlSubmit]").hide();

            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=divAmendDate]").hide();
                $("[id$=txtAmendDate]").val("");
                $("[id$=txtAmendDate]").hide();
                $("[id$=pnlPrintSO]").hide();
            }
        }

        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
                $("[id$=txtFromDate]").focus();
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
                $("[id$=txtAdvCustomerSrch]").focus();
            }
            return false;
        }

        function ShowHideAttachDocs(flag) {
            ///<summary>
            /// Used to Show/Hide Attach Document Div
            ///</summary>

            //If flag then Show Attach Document
            if (flag == 1) {
                $("[id$=divAttachDocs]").show();
                $("[id$=imbShowAttachDocs]").hide();
                $("[id$=imbHideAttachDocs]").show();
            }
            else {
                $("[id$=divAttachDocs]").hide();
                $("[id$=imbShowAttachDocs]").show();
                $("[id$=imbHideAttachDocs]").hide();
            }
            $("[id$=hdfIsAttachDocsVisible]").val(flag);
            return false;
        }

        function ShowHideorderDetails(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>
            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divOrderDetails]").show();
                $("[id$=imbShowOrderDetails]").hide();
                $("[id$=imbHideOrderDetails]").show();
            }
            else {
                $("[id$=divOrderDetails]").hide();
                $("[id$=imbShowOrderDetails]").show();
                $("[id$=imbHideOrderDetails]").hide();
            }
            $("[id$=hdfIsOrderDetailsVisible]").val(flag);
            return false;
        }

        function CalculateAmount(sender) {
            //            alert($(sender).attr('id'));
            $("[id$=hdfErrorMsgType]").val("0"); //For Resetting (Bug ID:  16120)

            var qty = 0;
            var rate = 0;
            var ctn = 0;
            var discount = 0;
            var tax = 0;
            var Dtlamount = 0;
            var brandQty = 0;
            var UOMConv = 0;
            rate = parseFloat($("[id$=txtUnitPrice]").val());
            discount = $("[id$=txtDiscount]").val() == '' ? 0 : parseFloat($("[id$=txtDiscount]").val());
            Dtlamount = parseFloat($("[id$=txtAmount]").val());
            tax = $("[id$=txtTax]").val() == '' ? 0 : parseFloat($("[id$=txtTax]").val());
            brandQty = parseFloat($("[id$=txtItemQuantity]").val());
            UOMConv = parseFloat($("[id$=hdfBrandUOMConversion]").val());

            if (!isNaN(brandQty) && !isNaN(rate)) {
                var amount = brandQty * rate;
                $("[id$=txtAmount]").val(amount.toFixed(CurrencyDigits));

                var totAmount = (amount - discount) + tax;
                $("[id$=txtTotalAmt]").val(totAmount.toFixed(CurrencyDigits));
            }
            else {
                $("[id$=txtAmount]").val(parseFloat(0).toFixed(CurrencyDigits));
                $("[id$=txtTotalAmt]").val(parseFloat(0).toFixed(CurrencyDigits));
            }

            $("[id$=hdfQtyTemp]").val($("[id$=txtItemQuantity]").val());

            if ($(sender).attr('id') == $('[id$=txtItemQuantity]').attr('id')) {
                $("[id$=hdfFocusPdctAdd]").val("1");
            } else if ($(sender).attr('id') == $('[id$=txtUnitPrice]').attr('id')) {
                $("[id$=hdfFocusPdctAdd]").val("2");
            }

            $("[id$=btnTooltip]").click();

        }

        function CalculatePouchQty() {
            if ($("[id$=hdfPouchPcs]").val() != '' && parseFloat($("[id$=hdfPouchPcs]").val()) > 0) {
                var EnteredQty = $("[id$=txtItemPchQty]").val() == '' ? 0 : $("[id$=txtItemPchQty]").val();
                var TotalPouchQty = (parseFloat($("[id$=hdfTotalBagPcs]").val()) * parseFloat(EnteredQty)) / parseFloat($("[id$=hdfPouchPcs]").val())
                $("[id$=txtItemQuantity]").val(parseFloat(TotalPouchQty).toFixed(0));
            }
            else
                $("[id$=txtItemQuantity]").val(0);

//            $("[id$=btnPchQtyChange]").click();
        }

        function CalculateBoxQty() {
            if ($("[id$=hdfTotalBagPcs]").val() != '' && parseFloat($("[id$=hdfTotalBagPcs]").val()) > 0) {
                var EnteredQty = $("[id$=txtItemQuantity]").val() == '' ? 0 : $("[id$=txtItemQuantity]").val();
                var TotalBoxQty = (parseFloat($("[id$=hdfPouchPcs]").val()) * parseFloat(EnteredQty)) / parseFloat($("[id$=hdfTotalBagPcs]").val());
                $("[id$=txtItemPchQty]").val(parseFloat(TotalBoxQty).toFixed(NumberDigits));
            }
            else
                $("[id$=txtItemPchQty]").val('');
        }

        function CalculateTotal(sender) {
            var subTotal = $("#[id*=grdOrderDetails]").find('[id$=lblItemTotalAmnt]').length > 0 ? parseFloat($("#[id*=grdOrderDetails]").find('[id$=lblItemTotalAmnt]').html().replace(new RegExp(',', 'g'), '')) : parseFloat(0);
            subTotal = isNaN(subTotal) ? 0 : subTotal;
            var totalDiscount = parseFloat($("[id$=txtHdrDiscount]").val());
            totalDiscount = isNaN(totalDiscount) ? 0 : totalDiscount;
            var totalTax = parseFloat($("[id$=txtHdrTax]").val());
            totalTax = isNaN(totalTax) ? 0 : totalTax;
            var totalShipping = parseFloat($("[id$=txtHdrOtrCharge]").val());
            totalShipping = isNaN(totalShipping) ? 0 : totalShipping;
            var totalPriceAdj = parseFloat($("[id$=txtHdrPriceAdj]").val());
            totalPriceAdj = isNaN(totalPriceAdj) ? 0 : totalPriceAdj;

            var netTotal = (subTotal + totalTax + totalShipping + totalPriceAdj) - totalDiscount;
            $("[id$=txtHdrTotal]").val(netTotal.toFixed(parseInt($("[id$=hdfCurrencyDigits]").val())));
            $("[id$=txtHdrTotal]").attr("title", (netTotal).toFixed(parseInt($("[id$=hdfCurrencyDigits]").val())));
            if (totalShipping == 0)
                $("[id$=txtHdrOtrCharge]").val((totalShipping).toFixed(CurrencyDigits));
            if (totalPriceAdj == 0)
                $("[id$=txtHdrPriceAdj]").val((totalPriceAdj).toFixed(CurrencyDigits));
        }

        //For finding and removing duplicate and other group validation controls
        //Array of present validations
        var validationArrayGroup;
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

        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
            }
        }

        //For checking if validation control in Array of present validations
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
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

        function AfterGridExpand(row) {
            if ($("[id$=grdSaleOrderSearchList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedOrders]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
                }
            }
        }
        function ShowShortClose() {
            ShowContainerDiv('[id$=divShortClose]', '<%= GetGlobalResourceObject("Controls","ShortClose").ToString() %>', '450', '200');
        }
        //For Show Confirm Short Close
        function ShowConfirmShortClose() {

            var msgTitle;
            var msg;
            msgTitle = '<%= GetGlobalResourceObject("Controls","ShortClose").ToString() %>';
            msg = '<%= GetGlobalResourceObject("Messages","ConfirmShortCloseDirectSO").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $(this).dialog("close");
                        $("[id$=btnShCloseSave]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }

        function CalcTax() {
            var Amount = $("[id$=txtPopupAmount]").val();
            var TaxPer = $("[id$=txtTaxPerc]").val();
            var DecimalCount = $("[id$=hdfDecimalVal]").val();
            var Total = 0;
            if (TaxPer != '' && TaxPer > 0)
                Total = (parseFloat(Amount) * parseFloat(TaxPer)) / 100;
            $("[id$=txtPopupCharge]").val(Total.toFixed(DecimalCount));
        }

        function AfterDateSelect(controlID) {
            if (controlID == "txtDeliveryDate") {
                GrandScriptUtils.DatePickerCommon("txtReqDate");
                $("[id$=txtReqDate]").val($("[id$=txtDeliveryDate]").val());
            }
        }
    </script>
    <style type="text/css">
        [class="ui-widget-overlay"]
        {
            position: fixed !important;
        }
        [aria-labelledby^="ui-dialog"]
        {
            position: fixed !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlQuotation">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li id="pnlNew">
                                        <asp:Button runat="server" TabIndex="51" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>" CommandArgument="SEC_ActionPanel"
                                            ToolTip="<%$Resources:Controls,Add%>" />
                                    </li>
                                    <li id="pnlEdit">
                                        <asp:Button runat="server" TabIndex="52" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li id="pnlEditforCancel">
                                        <asp:Button runat="server" TabIndex="55" ID="btnEditforCancel" CommandName="EDITFORCANCEL"
                                            OnClick="ActionHandler" Text="Cancel SO" CommandArgument="SEC_ActionPanel" SkinID="btnInner-cancel1"
                                            ToolTip="Cancel SO" />
                                    </li>
                                    <li id="pnlAmend">
                                        <asp:Button runat="server" TabIndex="54" ID="btnAmend" CommandName="AMEND" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Amend %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-amend"
                                            ToolTip="<%$resources:Controls,Amend %>" />
                                    </li>
                                    <li id="pnlCopy">
                                        <asp:Button runat="server" TabIndex="55" ID="btnCopy" CommandName="COPY" OnClick="ActionHandler"
                                            SkinID="btnInner-copy" Text="<%$Resources:Controls,Copy%>" CommandArgument="SEC_ActionPanel"
                                            ToolTip="<%$Resources:Controls,Copy%>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="53" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li id="Li1" runat="server">
                                        <asp:Button runat="server" TabIndex="54" ID="btnPrint" CommandName="PRINTSO" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="60"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('so')"
                                            ValidationGroup="so" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="61" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('sod')" ValidationGroup="so"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="62" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('so')" ValidationGroup="so"
                                            ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-save" />
                                    </li>
                                    <li id="pnlPrintSO" runat="server">
                                        <asp:Button runat="server" TabIndex="69" ID="btnPrintSO" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="63" Text="<%$resources:ErpRes,Delete %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Delete" />
                                    </li>
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" CommandName="DELETESUBMIT" TabIndex="64"
                                            Text="<%$resources:ErpRes,CancelSubmit %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,CancelSubmit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="65" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfPreviousUrl" runat="server" />
                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfRateDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfExchangeRateDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfisItemHaveTax" runat="server" Value="0" />
                <%--To maintain Header & Detail Tax - Discount (Line Item Tax-Discount)--%>
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="45" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="46" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive" OnClientClick="javascript:return SelectedCheckBoxCount(1);"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server" Style="display: none;">
                        <asp:TableCell>
                            <%--Listing Page Table Row--%>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:Controls,ShowFilter %>" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:Controls,HideFilter %>" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="input-small margnrgt1-5per"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="lbl-20-1perc"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="1" CssClass="input-small" MaxLength="11"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblPortType" Text="<%$ resources:SOType %>" AssociatedControlID="ddlPortType"
                                                CssClass="lbl-20-8perc"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlPortType" CssClass="lbl-19-6perc" TabIndex="1">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblAdvCustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtAdvCustomerSrch"></asp:Label>
                                            <asp:TextBox ID="txtAdvCustomerSrch" runat="server" CssClass="select-half margnbotm0"
                                                MaxLength="100" TabIndex="2"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfAdvCustomerPKSrch" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <%--"divcol-S padgtop7"--%>
                                            <asp:Label ID="lblDSONumber" runat="server" Text="<%$ resources:DsoNo %>" AssociatedControlID="txtDSONumber"
                                                CssClass="lbl-20-8perc"></asp:Label>
                                            <asp:TextBox ID="txtDSONumber" runat="server" MaxLength="100" TabIndex="2" CssClass="lbl-23perc margnbotm0"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfDSONoPK" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblStatus" Text="Status" AssociatedControlID="ddlStatus"
                                                CssClass="lbl-18perc"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-a1 margnbotm0"
                                                TabIndex="2">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Approved %>" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Submitted %>" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Draft %>" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Cancelled %>" Value="4"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                ValidationGroup="Search" OnClick="ActionHandler" TabIndex="2" CommandName="SEARCH"
                                                SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                TabIndex="2" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap">
                                <%--hierarchical-wrap--%>
                                <asp:HiddenField ID="hdfSO_ExpandPosition" runat="server" />
                                <cc1:ExtGridView runat="server" ID="grdSaleOrderSearchList" AutoGenerateColumns="False"
                                    ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    AllowSorting="True" OnSorting="ActionHandler" Width="100%" OnRowDataBound="ActionHandler"
                                    PageSize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="8" runat="server" Checked="false"
                                                    GroupName="SelectOne" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping2(this);"
                                                    OnCheckedChanged="ActionHandler" AutoPostBack="true" />
                                                <asp:HiddenField runat="server" ID="hdfDSOID" Value='<%# Eval("SOH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDSOStatus" Value='<%# Eval("SOH_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDSODELStatus" Value='<%# Eval("SOH_DEL_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfDepartmentID" Value='<%# Eval("SOH_DEPT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DsoDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDSODate" runat="server" Text='<%# Eval("SOH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("SOH_DATE", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%--Date--%>
                                        <asp:TemplateField HeaderText="<%$ resources:DsoNo %>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblDSONo" runat="server" Text='<%# Eval("SOH_NO") == "" ? GetGlobalResourceObject("Messages","DocGenerationNew").ToString(): Eval("SOH_NO")%>'
                                                    ToolTip='<%# Eval("SOH_NO") == "" ? GetGlobalResourceObject("Messages","DocGenerationNew").ToString(): Eval("SOH_NO")%>'></asp:Label>--%>
                                                <asp:LinkButton ID="lnkDSONo" CssClass="text-underline" runat="server" Text='<%# Eval("SOH_NO") == "" ? GetGlobalResourceObject("Messages","DocGenerationNew").ToString(): Eval("SOH_NO")%>'
                                                    OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval(Resources.DataFieldRes.SalesOrderPk) %>'
                                                    ToolTip='<%# Eval("SOH_NO") == "" ? GetGlobalResourceObject("Messages","DocGenerationNew").ToString(): Eval("SOH_NO")%>'></asp:LinkButton>
                                                <%--SOH_PK--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%--SONum--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerGrd" runat="server" Text='<%# Eval("SOH_CUSTOMER_TEXT") %>'
                                                    ToolTip='<%#Eval("SOH_CUSTOMER_TEXT") %>'></asp:Label>
                                                <asp:Button runat="server" ID="btnOrderDetails" OnClick="ActionHandler" CommandName="SODETAILS"
                                                    CommandArgument='<%# Eval("SOH_PK") %>' EnableTheming="false" Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%--Customer--%>
                                        <asp:TemplateField HeaderText="<%$ resources:SOType %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDsoType" Text='<%#Eval("SOH_TYPE_TEXT") %>' ToolTip='<%#Eval("SOH_TYPE_TEXT") %>'
                                                    runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%--Type--%>
                                        <asp:TemplateField HeaderText="<%$ resources:DeliveryDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDSODelDate" runat="server" Text='<%# Eval("SOH_DELIVERY_DATE", Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval("SOH_DELIVERY_DATE", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%--DeliveryDate--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDsoCurrency" Text='<%#Eval("SOH_CURRENCY_TEXT") %>' ToolTip='<%#Eval("SOH_CURRENCY_TEXT") %>'
                                                    runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="4%" HorizontalAlign="Left" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <%--Currency--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDSOAmnt" runat="server" Text='<%# GetFormattedNumberWithComma(Eval("SOH_NET_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedNumberWithComma(Eval("SOH_NET_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" CssClass="numeric" />
                                            <HeaderStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--Amount--%>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    ToolTip='<%# Eval("SOH_STATUS_TEXT") %>' CssClass='<%# Eval("ASC_CSS_CLASS") %>'
                                                    TabIndex="9" />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval("SOH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgShortClose" ToolTip="<%$ resources:ShortCloseSC %>" runat="server"
                                                    OnClick="ActionHandler" CommandName="SHORTCLOSE" CommandArgument='<%# Eval("SOH_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" CssClass="numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <asp:GridView runat="server" ID="grdDSOList" AutoGenerateColumns="False" GridLines="None"
                                                        EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false" OnRowDataBound="ActionHandler"
                                                        Width="100%">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblInnerEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDSOItem" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_ITEM_TEXT"),85) %>'
                                                                        ToolTip='<%# Eval("SOD_ITEM_TEXT") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="23%" HorizontalAlign="Left" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <%--Item--%>
                                                            <asp:TemplateField HeaderText="<%$ resources:ReqdDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDSOReqDt" runat="server" Text='<%# Eval("SOD_REQUIRED_BY", Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval("SOD_REQUIRED_BY", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" />
                                                            </asp:TemplateField>
                                                            <%--ReqDate--%>
                                                            <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGrdUom" runat="server" Text='<%# Eval("SOD_UOM_TEXT") %>' ToolTip='<%# Eval("SOD_UOM_TEXT") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="amount-numeric" Width="3%" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <%--UOM--%>
                                                            <asp:TemplateField HeaderText="<%$ resources:OrderQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" ID="lblDsoOrdrQty" Text='<%# GetFormattedNumberWithComma(Eval("SOD_QTY")) %>'
                                                                        ToolTip='<%# GetFormattedNumberWithComma(Eval("SOD_QTY")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="amount-numeric" Width="5%" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <%--OrderQty--%>
                                                            <asp:TemplateField HeaderText="<%$ resources:InvQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDsoInvQty" Text='<%# GetFormattedNumberWithComma(Eval("SOD_QTY_INVOICED")) %>'
                                                                        ToolTip='<%# GetFormattedNumberWithComma(Eval("SOD_QTY_INVOICED")) %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="amount-numeric" Width="5%" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <%--InvQty--%>
                                                            <asp:TemplateField HeaderText="<%$ resources:UnitPrice %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDSOUnitPrc" runat="server" Text='<%# GetFormattedNumberWithComma(Eval("SOD_RATE")) %>'
                                                                        ToolTip='<%# GetFormattedNumberWithComma(Eval("SOD_RATE")) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="amount-numeric" Width="5%" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <%--UnitPrice--%>
                                                        </Columns>
                                                        <%--Second--%>
                                                        <RowStyle CssClass="table-secondlevel" />
                                                        <HeaderStyle CssClass="table-secondlevela" />
                                                    </asp:GridView>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--Inner Grid--%>
                                    </Columns>
                                    <%--First--%>
                                    <RowStyle CssClass="table-firstlevel" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                    <FooterStyle CssClass="table-firstlevela-total" />
                                </cc1:ExtGridView>
                                <pc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                            <div class="clear">
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide" id="tblDetailHdr">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfIscartYes" runat="server" />
                                            <asp:HiddenField ID="hdfTotalCBM" runat="server" />
                                            <asp:HiddenField ID="hdfIsItemYes" runat="server" />
                                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                            <asp:HiddenField ID="hdfDecimalFormatWithComma" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
                                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfExchangeRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfWeightFormat" runat="server" />
                                            <asp:HiddenField ID="hdfSaleOrderPK" runat="server" />
                                            <asp:HiddenField ID="hdfIsCopy" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfSaleOrderRate" runat="server" />
                                            <asp:HiddenField ID="hdfIsContinueCBM" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdfAddItem" runat="server" />
                                            <asp:Label ID="lbl" runat="server" Text="<%$ resources:SaleOrderNo%>" AssociatedControlID="lblSaleOrderNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblSaleOrderNo" CssClass="input-small-a margnrgt0-8per"
                                                TabIndex="1"></asp:Label>
                                            <div style="width: 18px; display: inline-block;">
                                                <asp:ImageButton ID="btnRevision" runat="server" OnClick="ActionHandler" CommandName="REVISIONHISTORY"
                                                    SkinID="history" ToolTip="<%$resources:RevisionHistory %>" />
                                            </div>
                                            <asp:Label runat="server" ID="lblSaleOrderDate" Text="<%$ resources:SaleOrderDate%>"
                                                AssociatedControlID="txtSaleOrderDate" CssClass="lbl-17-3perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSaleOrderDate" CssClass="input-small" TabIndex="1"
                                                onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfSaleOrderDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtSaleOrderDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SaleOrderDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfSaleOrderNo" runat="server" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomer" runat="server" AssociatedControlID="txtCustomer" Text="<%$ resources:Customer %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" MaxLength="100" TabIndex="4" CssClass="select-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomer" runat="server" />
                                            <asp:Button ID="btnCustomerSelected" runat="server" OnClick="ActionHandler" CommandName="CUSTOMERSELECTED"
                                                EnableTheming="false" Style="display: none" />
                                            <asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtCustomer" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Customer %>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="Label3" runat="server" AssociatedControlID="txtCusAddress"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCusAddress" MaxLength="500" TextMode="MultiLine"
                                                TabIndex="8" CssClass="multiline-m1col select-half" onpaste="return false;" onkeyup="limitText(this,500);"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCusAddress" runat="server" />
                                            <asp:HiddenField ID="hdfCusCountry" runat="server" />
                                            <asp:HiddenField ID="hdfCusCountryText" runat="server" />
                                            <asp:HiddenField ID="hdfCusZip" runat="server" />
                                            <asp:HiddenField ID="hdfCusPhone" runat="server" />
                                            <asp:HiddenField ID="hdfCusMobile" runat="server" />
                                            <asp:HiddenField ID="hdfCusFax" runat="server" />
                                            <asp:HiddenField ID="hdfCusEmail" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomerRef" runat="server" AssociatedControlID="txtCustomerRef"
                                                Text="Customer Ref#"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCustomerRef" TabIndex="5" CssClass="medium" onpaste="return false;"
                                                onkeyup="limitText(this,300);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfCustomerRef" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtCustomerRef"
                                                Display="Dynamic" Text="*" ErrorMessage="Enter Customer Ref#"></asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblSOType" Text="<%$ resources:SOType %>" AssociatedControlID="ddlSOType"
                                                CssClass="middle-lbl-a"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlSOType" CssClass="lbl-19-6perc" TabIndex="6">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfSOType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlSOType"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SaleOrderType %>"
                                                InitialValue="-1">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small-a" TabIndex="7"
                                                MaxLength="100"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtCurrency"
                                                    Display="Dynamic" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                    Text="*" ErrorMessage="<%$ resources:Err_Currency%>"></asp:RequiredFieldValidator>
                                            </div>
                                            <asp:Button ID="btnCurrency" runat="server" OnClick="ActionHandler" CommandName="EXCHANGERATE"
                                                EnableTheming="false" Style="display: none" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                            <asp:Label ID="lblExchangeRate" runat="server" Text="<%$ resources:ExchangeRate %>"
                                                AssociatedControlID="txtExchangeRate" CssClass="middle-lbl-a" /><%-- TabIndex="13"--%>
                                            <asp:TextBox ID="txtExchangeRate" runat="server" MaxLength="8" TabIndex="8" CssClass="numeric input-small"
                                                onkeypress="return isFloatNumberKey(event);" />
                                            <div id="divAmendDate" runat="server">
                                                <asp:Label runat="server" ID="lblAmendDate" Text="<%$ resources:AmendmentDate%>"
                                                    AssociatedControlID="txtAmendDate" CssClass="margn-rgt0"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtAmendDate" CssClass="input-small input-disabled"
                                                    Enabled="false" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"
                                                    onchange="AfterDateSelect(null)"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblDeliveryDate" Text="<%$ resources:DeliveryDate%>"
                                                AssociatedControlID="txtDeliveryDate" CssClass="middle-lbl-d"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDeliveryDate" CssClass="medium" TabIndex="9" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:Button ID="btnDeliveryDate" runat="server" EnableTheming="false" OnClick="ActionHandler"
                                                CommandName="BOOKINGDATECHANGE" Style="display: none" />
                                            <asp:RequiredFieldValidator ID="vrfDeliveryDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtDeliveryDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_DeliveryDate %>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <label>
                                                <asp:Literal runat="server" ID="Label1"></asp:Literal></label>
                                            <label style="color:#7289ad; font-weight: bolder;font-size:small; text-align:left;">
                                                <asp:Literal runat="server" ID="lblSpecialCat" Visible="false"></asp:Literal></label>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="tr1" runat="server" visible="true">
                                    <td>
                                        <div class="div2col-S" id="divSBUCompany">
                                            <asp:Label ID="lblCompany" runat="server" Text="Company" AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" CssClass="select-half">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("OrderDetails").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowOrderDetails" OnClientClick="javascript:return ShowHideorderDetails(1);"
                                    ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowOrderDetails %>"
                                    TabIndex="9" />
                                <asp:ImageButton runat="server" ID="imbHideOrderDetails" OnClientClick="javascript:return ShowHideorderDetails(0);"
                                    ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideOrderDetails %>"
                                    TabIndex="9" />
                                <asp:HiddenField ID="hdfIsOrderDetailsVisible" runat="server" Value="1" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divOrderDetails">
                                <%--style="display: none"--%>
                                <table class="table-devide" id="tblDetails">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:HiddenField ID="hdfDetailPK" runat="server" Value="0" />
                                                <asp:Label ID="lblItemCategory" runat="server" AssociatedControlID="txtItemCategory"
                                                    Text="<%$ resources:ItemCategory %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtItemCategory" runat="server" TabIndex="9" CssClass="input-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfItemCategory" runat="server" />
                                                <asp:HiddenField ID="hdfItemCatVal" runat="server" Value="0" />
                                                <asp:Button ID="btnSelectItemCategory" runat="server" OnClick="ActionHandler" CommandName="ITEMCATEGORYSELECTED"
                                                    EnableTheming="false" Style="display: none" />
                                                <asp:RequiredFieldValidator ID="vrfItemCategory" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="scDetails" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtItemCategory" Display="Dynamic" Text="*"
                                                    ErrorMessage="<%$ resources:Err_ItemCategory %>"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblPackSpec" runat="server" AssociatedControlID="txtPackingSpec" Text="<%$ resources:PackingSpec %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtPackingSpec" runat="server" TabIndex="9" CssClass="input-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfPackingSpec" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <div id="divProdSubCategory" runat="server" class="display-inline">
                                                    <asp:Label ID="lblProdSubCategory" runat="server" Text="<%$ resources:Controls,SubCategory %>"
                                                        AssociatedControlID="ddlProdSubCategory" CssClass="lbl-20-5perc margn-rgt0" />
                                                    <asp:DropDownList ID="ddlProdSubCategory" runat="server" CssClass="select-9-4per"
                                                        TabIndex="10" onchange="SetItemAutoComplete();" />
                                                    <asp:Label ID="lblItem" runat="server" AssociatedControlID="txtItem" Text="<%$ resources:Item %>"
                                                        CssClass="lbl-10-5perc">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtItem" runat="server" TabIndex="10" CssClass="select-w59-2per margnlft-minus4"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfItem" runat="server" />
                                                    <asp:Button ID="btnItemSelected" runat="server" OnClick="ActionHandler" CommandName="ITEMSELECTED"
                                                        EnableTheming="false" Style="display: none" />
                                                    <asp:RequiredFieldValidator ID="vrfItem" CssClass="star" SetFocusOnError="true" ValidationGroup="scDetails"
                                                        EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                        runat="server" ControlToValidate="txtItem" Display="Dynamic" Text="*" ErrorMessage="<%$ Resources:Err_Item %>"></asp:RequiredFieldValidator>
                                                </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblQty" runat="server" AssociatedControlID="txtItemQuantity" Text="<%$ resources:Quantity_Req %>">
                                                </asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfQtyDespatched" Value='0' />
                                                <asp:HiddenField runat="server" ID="hdfQtyInvoiced" Value='0' />
                                                <asp:HiddenField runat="server" ID="hdfQtyTemp" Value='0' />
                                                <asp:HiddenField runat="server" ID="hdfBrandUOM" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfBrandUOMConversion" Value="1" />
                                                <asp:HiddenField runat="server" ID="hdfTotalBagPcs" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfPouchPcs" Value="0" />
                                                <asp:Button ID="btnTooltip" runat="server" OnClick="ActionHandler" CommandName="TOOLTIP"
                                                    Style="display: none" EnableTheming="false" />
                                                <asp:TextBox ID="txtItemPchQty" runat="server" CssClass="input-xsmall-b numeric"
                                                    TabIndex="11" onblur="CalculatePouchQty();" placeholder="<%$ resources:Bags %>"
                                                    ToolTip="<%$ resources:Bags %>"></asp:TextBox>
                                                <asp:TextBox ID="txtItemQuantity" runat="server" CssClass="input-small numeric" TabIndex="11"
                                                    onblur="CalculateAmount(this);" placeholder="<%$ resources:Pouch %>" ToolTip="<%$ resources:Pouch %>"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfItemQuantity" CssClass="star" SetFocusOnError="true"
                                                        Style="padding: 0px !important;" ValidationGroup="scDetails" EnableClientScript="true"
                                                        runat="server" ControlToValidate="txtItemQuantity" Display="Dynamic" Text="*"
                                                        ErrorMessage="<%$ resources:Err_Quantity %>">
                                                    </asp:RequiredFieldValidator>
                                                    <vc1:QuantityValidation ID="vreQuantity" runat="server" ControlToValidate="txtItemQuantity"
                                                        Style="padding: 0px !important;" NumberDigits="7" ErrorMessage="<%$ resources:Err_Quantity_Valid %>"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="scDetails"
                                                        NonZero="true">
                                                    </vc1:QuantityValidation>
                                                </div>
                                                <asp:Label ID="lblUom" runat="server" AssociatedControlID="ddlUOM" Text="UOM" CssClass="middle-lbl-small-c">
                                                </asp:Label>
                                                <asp:DropDownList ID="ddlUOM" TabIndex="11" runat="server" CssClass="medium" EnableViewState="true">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdfUOM" runat="server" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblDiscount" runat="server" CssClass="margntop2 input-disabled numeric"
                                                    AssociatedControlID="txtDiscount" onkeydown="return EnableArrowKey(event)" onpaste="return false;"
                                                    Text="<%$ resources:Discount %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDiscount" CssClass="input-small input-disabled numeric"
                                                    MaxLength="100" onblur="CalculateAmount(this);" onkeydown="return EnableArrowKey(event);"
                                                    onpaste="return false;"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfCustomDiscPerc" runat="server" Value="0" />
                                                <div style="width: 18px; display: inline-block;">
                                                    <asp:ImageButton ID="imgDiscountDtl" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                        TabIndex="13" CssClass=" margn-lft3 margntop2" ToolTip="<%$ resources:Controls,Discounts %>"
                                                        CommandName="DISCDETAILS" />
                                                </div>
                                                <asp:Label ID="lblTax" runat="server" AssociatedControlID="txtTax" Text="<%$ resources:Tax %>"
                                                    CssClass="lbl-18-3perc margntop2"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtTax" CssClass="input-small input-disabled numeric"
                                                    MaxLength="100" onblur="CalculateAmount(this);" onkeydown="return EnableArrowKey(event)"
                                                    onpaste="return false;"></asp:TextBox>
                                                <div style="width: 18px; display: inline-block;">
                                                    <asp:ImageButton ID="imgTaxDtl" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                        CssClass=" margn-lft3 margntop2" TabIndex="14" ToolTip="<%$ resources:Tax %>"
                                                        CommandName="TAXDETAILS" />
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblUnitPrice" runat="server" AssociatedControlID="txtUnitPrice" Text="<%$ resources:UnitPrice_Req %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtUnitPrice" runat="server" CssClass="input-small  numeric" MaxLength="18"
                                                    TabIndex="12" onblur="CalculateAmount(this);"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfUnitPrice" CssClass="star" SetFocusOnError="true"
                                                        Style="padding: 0px !important;" ValidationGroup="scDetails" EnableClientScript="true"
                                                        runat="server" ControlToValidate="txtUnitPrice" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_UnitPrice %>">
                                                    </asp:RequiredFieldValidator>
                                                    <vc1:RateValidation ID="vreUnitPrice" runat="server" ControlToValidate="txtUnitPrice"
                                                        Style="padding: 0px !important;" NumberDigits="7" ErrorMessage="<%$ resources:Err_UnitPrice_Valid %>"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="scDetails"
                                                        NonZero="true">
                                                    </vc1:RateValidation>
                                                </div>
                                                <asp:TextBox runat="server" ID="txtItemUnitUOM" CssClass="input-normal small" Enabled="false"></asp:TextBox>
                                                <span style="width: 4px; visibility: hidden;"></span>
                                                <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" Text="<%$ resources:Amount_Mand %>"
                                                    CssClass="middle-lbl-xsmall-e">
                                                </asp:Label>
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="input-small input-disabled numeric"
                                                    onkeydown="return EnableArrowKey(event);" onpaste="return false;"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="scDetails" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                    </asp:RequiredFieldValidator>
                                                    <vc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtAmount"
                                                        ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="scDetails">
                                                    </vc1:AmountValidation>
                                                </div>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblTotalAmt" runat="server" AssociatedControlID="txtTotalAmt" Text="<%$ resources:TotalAmt_Req %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtTotalAmt" runat="server" CssClass="input-small input-disabled numeric"
                                                    MaxLength="18" onblur="CalculateAmount(this);" onkeydown="return EnableArrowKey(event);"
                                                    onpaste="return false;"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfTotalAmt" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="scDetails" EnableClientScript="true" runat="server" ControlToValidate="txtTotalAmt"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalAmt %>">
                                                    </asp:RequiredFieldValidator>
                                                    <vc1:RateValidation ID="vreTotalAmt" runat="server" ControlToValidate="txtTotalAmt"
                                                        ErrorMessage="<%$ resources:Err_TotalAmt_Valid %>" NumberDigits="10" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="scDetails"
                                                        NonZero="false">
                                                    </vc1:RateValidation>
                                                </div>
                                                <span style="width: 4px; visibility: hidden;"></span>
                                                <asp:Label ID="lblReqDate" runat="server" AssociatedControlID="txtReqDate" Text="<%$ resources:ReqDate_Req %>"
                                                    CssClass="lbl-18-5perc">
                                                </asp:Label>
                                                <asp:TextBox ID="txtReqDate" runat="server" CssClass="input-small" onkeydown="return EnableArrowKey(event);"
                                                    onpaste="return false;" TabIndex="15"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfReqDate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="scDetails" EnableClientScript="true" runat="server" ControlToValidate="txtReqDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqDate %>">
                                                    </asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblDtlRemark" Text="<%$ resources:Remarks %>" AssociatedControlID="txtDtlRemark"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDtlRemark" TabIndex="27" MaxLength="480" TextMode="MultiLine"
                                                    CssClass="multiline"></asp:TextBox>
                                                <asp:CheckBox ID="chkRemarkToAll" runat="server" TextAlign="Left" ToolTip="<%$ resources:ApplyforAll%>"
                                                    TabIndex="27" CssClass="span-normal null-graph minw-17" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblDtlRemark2" Text="<%$ resources:AddlRemarks %>"
                                                    AssociatedControlID="txtDtlRemark2"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDtlRemark2" TabIndex="27" MaxLength="480" TextMode="MultiLine"
                                                    CssClass="multiline"></asp:TextBox>
                                                <asp:CheckBox ID="chkAddtlRemarkToAll" runat="server" TextAlign="Left" ToolTip="<%$ resources:ApplyforAll%>"
                                                    TabIndex="27" CssClass="span-normal null-graph minw-17" />
                                                <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="17"
                                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('scDetails');"
                                                    ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="scDetails"
                                                    SkinID="plus" />
                                                <asp:ImageButton runat="server" ID="btnClearItem" CommandName="CLEARITEM" TabIndex="18"
                                                    OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>" CommandArgument="PageAction_Entry"
                                                    SkinID="cancel" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="gridwrap scroll-container">
                                    <asp:GridView ID="grdOrderDetails" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                        AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                        Width="130%" ShowFooter="true" OnRowDataBound="ActionHandler" FooterStyle-Height="5px">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:ItemCategory %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfSODPK" runat="server" Value='<%#Eval("SOD_PK") %>' />
                                                    <asp:HiddenField ID="hdfCusItemCategPK" runat="server" Value='<%#Eval("SOD_ITEM_CATEGORY") %>' />
                                                    <asp:HiddenField ID="hdfPackSpec" runat="server" Value='<%#Eval("SOD_PACK_SPEC") %>' />
                                                    <asp:HiddenField ID="hdfPackSpecName" runat="server" Value='<%#Eval("SOD_PACK_SPEC_NAME") %>' />
                                                    <asp:Label ID="lblgrdItemCateg" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_ITEM_CATEGORY_TEXT"),40) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_ITEM_CATEGORY_TEXT"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblfooterTot" Text="<%$ resources:Total %>"></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle Width="12%" Wrap="true" />
                                            </asp:TemplateField>
                                            <%--Item Category--%>
                                            <asp:TemplateField HeaderText="<%$ resources:ItemName%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdItemName" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_ITEM_TEXT"),50) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_ITEM_TEXT"))) %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfItemName" runat="server" Value='<%#Eval("SOD_ITEM") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="24%" />
                                            </asp:TemplateField>
                                            <%--Item Name--%>
                                            <asp:TemplateField HeaderText="<%$ resources:UOM%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdUOM" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_UOM_TEXT"),10) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_UOM_TEXT"))) %>'></asp:Label>
                                                    <asp:HiddenField ID="hdfgrdUoM" runat="server" Value='<%#Eval("SOD_UOM") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="4%" />
                                            </asp:TemplateField>
                                            <%--UOM--%>
                                            <asp:TemplateField HeaderText="<%$ resources:Qty%>" HeaderStyle-CssClass="amount-numeric">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdQuantity" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedNumberWithComma(Eval("SOD_QTY")) %>'
                                                        ToolTip='<%# GetFormattedNumberWithComma(Eval("SOD_QTY")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5.5%" CssClass="amount-numeric" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblItemTotalQty" runat="server"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle CssClass="amount-numeric" Width="5.5%" />
                                            </asp:TemplateField>
                                            <%--Quantity--%>
                                            <asp:TemplateField HeaderText="<%$ resources:UnitPrice %>" HeaderStyle-CssClass="amount-numeric">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdUnitPrice" runat="server" Text='<%# GetFormattedRate(Eval("SOD_RATE")) %>'
                                                        ToolTip='<%# GetFormattedRate(Eval("SOD_RATE")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <%--Unit Price--%>
                                            <asp:TemplateField HeaderText="<%$ resources:Amount %>" HeaderStyle-CssClass="amount-numeric">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdAmount" runat="server" Text='<%# GetFormattedNumberWithComma(Eval("SOD_AMOUNT")) %>'
                                                        ToolTip='<%# GetFormattedNumberWithComma(Eval("SOD_AMOUNT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="6.5%" CssClass="amount-numeric" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblItemTotalmount" runat="server" CssClass="amount-numeric"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle CssClass="amount-numeric" Width="6.5%" />
                                            </asp:TemplateField>
                                            <%--Amount--%>
                                            <asp:TemplateField HeaderText="<%$ resources:Discount %>" HeaderStyle-CssClass="amount-numeric">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdDiscount" runat="server" Text='<%# GetFormattedNumberWithComma(Eval("SOD_DISCOUNT")) %>'
                                                        ToolTip='<%# GetFormattedNumberWithComma(Eval("SOD_DISCOUNT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="6%" CssClass="amount-numeric" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblItemTotalDiscount" runat="server" CssClass="amount-numeric"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle CssClass="amount-numeric" Width="6%" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderStyle-CssClass="amount-numeric">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdDiscountPerc" runat="server" ForeColor="Red" Text='<%# Convert.ToString(Eval("SOD_DISC_PERC")) == "0" ? "" : Eval("SOD_DISC_PERC") + "%" %>'
                                                        ToolTip='<%# Convert.ToString(Eval("SOD_DISC_PERC")) == "0" ? "" : Eval("SOD_DISC_PERC")+ "%" %>' ></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" Font-Size="9px" />
                                            </asp:TemplateField>
                                            <%--Discount--%>
                                            <asp:TemplateField HeaderText="<%$ resources:Tax %>" HeaderStyle-CssClass="amount-numeric">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdTax" runat="server" Text='<%# GetFormattedNumberWithComma(Eval("SOD_TAX")) %>'
                                                        ToolTip='<%# GetFormattedNumberWithComma(Eval("SOD_TAX")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="6%" CssClass="amount-numeric" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblItemTotalTax" runat="server" CssClass="amount-numeric"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle CssClass="amount-numeric" Width="6%" />
                                            </asp:TemplateField>
                                            <%--Tax--%>
                                            <asp:TemplateField HeaderText="<%$ resources:Total %>" HeaderStyle-CssClass="amount-numeric">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdTotal" runat="server" Text='<%# GetFormattedNumberWithComma(Eval("SOD_NET_AMOUNT")) %>'
                                                        ToolTip='<%# GetFormattedNumberWithComma(Eval("SOD_NET_AMOUNT")) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="7%" CssClass="amount-numeric" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblItemTotalAmnt" runat="server" CssClass="amount-numeric"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle CssClass="amount-numeric" Width="7%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Width="1%" />
                                            </asp:TemplateField>
                                            <%--Total--%>
                                            <asp:TemplateField HeaderText="<%$ resources:ReqdDate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblgrdReqDate" runat="server" Text='<%# Eval("SOD_REQUIRED_BY", Resources.Constants.DateFormatGrid) %>'
                                                        ToolTip='<%# Eval("SOD_REQUIRED_BY", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="6%" />
                                            </asp:TemplateField>
                                            <%--Required Date--%>
                                            <asp:TemplateField HeaderText="<%$ resources:Remarks %>" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_REMARKS"), 70) %>'
                                                        ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_REMARKS"))) %>'
                                                        ID="lblgrdRemarks"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="40%" />
                                            </asp:TemplateField>
                                            <%--Remarks--%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                        CommandArgument="PageAction_Entry" SkinID="imbeditgrid" ToolTip="<%$resources:Controls,Edit %>"
                                                        TabIndex="19" />
                                                    <%--OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"--%>
                                                    <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                        CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirmationMsg(this);"
                                                        SkinID="imbdeletegrid" ToolTip="<%$resources:Controls,Delete %>" TabIndex="19" />
                                                    <%--OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" Wrap="false" />
                                            </asp:TemplateField>
                                            <%--Edit & Delete--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div id="divCalc">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                        <tr>
                                            <td style="width: 87%; text-align: right">
                                                <asp:Label runat="server" ID="lblHdrDiscount" Text="<%$ resources:Discount%>" AssociatedControlID="txtHdrDiscount"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    TabIndex="20" ToolTip="<%$ resources:Tax %>" CommandName="DISCOUNTHEADER" />
                                                <asp:TextBox ID="txtHdrDiscount" runat="server" CssClass="input-w97 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblHdrOtrCharge" Text="<%$ resources:OtherCharge%>"
                                                    AssociatedControlID="txtHdrOtrCharge"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrOtrCharge" SkinID="shipping" runat="server" OnClick="ActionHandler"
                                                    TabIndex="21" ToolTip="<%$ resources:OtherCharge %>" CommandName="OTHERCHARGEHEADER" />
                                                <asp:TextBox ID="txtHdrOtrCharge" runat="server" CssClass="input-w97 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblHdrTax" Text="<%$ resources:Tax%>" AssociatedControlID="txtHdrTax"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    TabIndex="22" ToolTip="<%$ resources:Tax %>" CommandName="TAXHEADER" />
                                                <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w97 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblHdrPriceAdj" Text="<%$ resources:PriceAdj%>" AssociatedControlID="txtHdrPriceAdj"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrPriceAdj" runat="server" CssClass="input-w97 numeric" onchange="CalculateTotal(this);"
                                                    MaxLength="16" TabIndex="23"></asp:TextBox>
                                                <div class="starwrap">
                                                    <vc1:AmountValidation ID="vamPriceAdj" runat="server" ControlToValidate="txtHdrPriceAdj"
                                                        ErrorMessage="<%$ resources:Err_Valid_PriceAdj %>" NumberDigits="12" AllowNegative="true"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="so">
                                                    </vc1:AmountValidation>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTotal" Text="<%$ resources:Total%>" AssociatedControlID="txtHdrTotal"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w97 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("AttachDocuments").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowAttachDocs" OnClientClick="javascript:return ShowHideAttachDocs(1);"
                                    ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowAttachDocs %>"
                                    TabIndex="24" />
                                <asp:ImageButton runat="server" ID="imbHideAttachDocs" OnClientClick="javascript:return ShowHideAttachDocs();"
                                    ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideAttachDocs %>"
                                    TabIndex="24" />
                                <asp:HiddenField ID="hdfIsAttachDocsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divAttachDocs">
                                <div class="divcol-S">
                                    <asp:Label ID="lblFileUpload" runat="server" Text="<%$ resources:AttachFile %>" AssociatedControlID="fupUpload"></asp:Label>
                                    <div class="fileupload-main">
                                        <asp:FileUpload ID="fupUpload" runat="server" TabIndex="25" CssClass="margn-rgt0 upload-area" />
                                        <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$resources:Err_File_Upload%>">                                                        
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <a id="anchorFile" runat="server" target="_blank" tabindex="55"></a>
                                    <asp:Button runat="server" ID="btnUpload" CommandName="ADDITEMUPLOAD" TabIndex="26"
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
                                                    <%#Container.DataItemIndex + 1 %>
                                                    <%-- <asp:Label ID="lblSlNo" runat="server" Text='<%# Eval("DOC_SEQ_NO") %>' ToolTip='<%# Eval("DOC_SEQ_NO") %>'></asp:Label>--%>
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
                                                        tabindex="27" title="<%$resources:Controls,View %>" target="_blank" href='<%#Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'>
                                                    </a>
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-CssClass="file-details">
                                                <ItemTemplate>
                                                    <asp:Button ID="lnkEdit" runat="server" OnClick="ActionHandler" CommandName="EDITITEMUPLOAD"
                                                        TabIndex="27" SkinID="edit-icon" ToolTip="<%$ resources:Controls,Edit %>" Style="margin-right: 3px!important;" /><%--CommandArgument="PageAction_Entry" OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender" --%>
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-CssClass="file-details">
                                                <ItemTemplate>
                                                    <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEMUPLOAD"
                                                        TabIndex="27" SkinID="delete-icon" ToolTip="<%$ resources:Controls,Delete %>"
                                                        OnClientClick="return ShowDeleteConfirmationMsg(this);" /><%--CommandArgument="PageAction_Entry" OnLoad="btnAction_Load" OnPreRender="btnAction_PreRender"--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:HiddenField ID="hdfDelStatusCurrent" runat="server" />
                    <asp:HiddenField ID="hdfWrkfStatusCurrent" runat="server" />
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="so" runat="server" />
                    <asp:ValidationSummary ID="vsDetails" ValidationGroup="scDetails" runat="server" />
                    <asp:ValidationSummary ID="vsTax" ValidationGroup="tax" runat="server" />
                    <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                </div>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="so" />
            </div>
            <%--<div id="divAlert" style="display: none">
                <uc2:Alert ID="ucrAlert" runat="server" />
            </div>--%>
            <div id="divTerms" class="max-425" style="display: none">
                <asp:Literal ID="ltrTerms" runat="server"></asp:Literal></div>
            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
            <div id="divItemTax" style="display: none">
                <div class="Button-container-popup">
                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text='<%$ Resources:Controls,Apply %>'
                        OnClick="ActionHandler" CommandArgument="PageAction_Entry" CommandName="TAXAPPLY"
                        TabIndex="59" />
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
                                    <asp:Label ID="lblPopupAmount" runat="server" Text="<%$ resources:Amount %>" AssociatedControlID="txtPopupAmount"></asp:Label><asp:TextBox
                                        ID="txtPopupAmount" CssClass="input-w70 numeric" runat="server" EnableViewState="false"
                                        Enabled="false" MaxLength="11"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <div runat="server" id="dvPerc">
                                        <asp:Label ID="lblPerc" runat="server" Text="<%$ resources:DiscPerc %>" AssociatedControlID="txtTaxPerc"></asp:Label>
                                        <asp:TextBox ID="txtTaxPerc" TabIndex="55" runat="server" CssClass="input-w70 numeric margnlft-minus4"
                                            EnableViewState="false" MaxLength="15" onChange="CalcTax();"></asp:TextBox>
                                    </div>
                                    <asp:Label ID="lblPopupCharge" runat="server" Text="<%$ resources:Charge %>" AssociatedControlID="txtPopupCharge"></asp:Label><asp:TextBox
                                        ID="txtPopupCharge" TabIndex="55" runat="server" CssClass="input-w70 numeric"
                                        EnableViewState="false" Enabled="false" MaxLength="15"></asp:TextBox><div class="starwrap">
                                            <asp:RequiredFieldValidator ID="vrfCharge" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupCharge"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Charge %>">
                                            </asp:RequiredFieldValidator><vc1:AmountValidation ID="vamTaxAmt" runat="server"
                                                ControlToValidate="txtPopupAmount" ErrorMessage="<%$ resources:Err_Charge_Valid %>"
                                                NumberDigits="11" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                ValidationGroup="tax">
                                            </vc1:AmountValidation></div>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-P">
                                    <asp:Label ID="lblPopupTaxType" runat="server" Text="Type" AssociatedControlID="ddlPopupTaxType"></asp:Label>
                                    <asp:DropDownList ID="ddlPopupTaxType" TabIndex="54" runat="server" CssClass="medium margn-lft3"
                                        EnableViewState="true" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblPopupOther" runat="server" Text="<%$ resources:Name %>" AssociatedControlID="txtPopupOther"></asp:Label>
                                    <asp:TextBox ID="txtPopupOther" runat="server" TabIndex="56" CssClass="lbl-37-2perc"
                                        EnableViewState="false" MaxLength="100" Enabled="false"></asp:TextBox><asp:RequiredFieldValidator
                                            ID="vrfPopupOther" CssClass="star" SetFocusOnError="true" ValidationGroup="tax"
                                            EnableClientScript="true" runat="server" ControlToValidate="txtPopupOther" Display="Dynamic"
                                            Text="*" ErrorMessage="<%$ resources:Err_Name %>">
                                        </asp:RequiredFieldValidator>
                                    <asp:ImageButton ID="imgPopupAdd" SkinID="imbaddnew" CssClass="margntop2" runat="server"
                                        OnClick="ActionHandler" CommandArgument="PageAction_Entry" ValidationGroup="tax"
                                        CommandName="TAXADD" TabIndex="57" OnClientClick="javascript:ValidatePageNow('tax')" />
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdTaxDetails" Width="100%" AllowSorting="false"
                            OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Name %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgrdTaxName" runat="server" Text='<%#Eval("SLT_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SLT_NAME"))) %>'></asp:Label><asp:HiddenField
                                            ID="hdfTaxName" runat="server" Value='<%#Eval("SLT_NAME") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="35%" />
                                </asp:TemplateField>
                                <%--Name--%>
                                <asp:TemplateField HeaderText="<%$ Resources:Type %>">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("SLT_PK") %>' />
                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("SLT_TAX") %>' />
                                        <asp:Label ID="lblgrdTaxText" runat="server" Text='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SLT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("SLT_TAX_TEXT"))) + (Convert.ToString(Eval("SLT_DISC_PERC")) == "0" ? "" : " (" + Convert.ToString(Eval("SLT_DISC_PERC"))+"%)") %>'
                                            ToolTip='<%# HttpUtility.HtmlDecode( HttpUtility.HtmlDecode(Convert.ToString(Eval("SLT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom  : Convert.ToString(Eval("SLT_TAX_TEXT"))))+ (Convert.ToString(Eval("SLT_DISC_PERC")) == "0" ? "" :   " (" +Convert.ToString(Eval("SLT_DISC_PERC"))+"%)") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="35%" />
                                </asp:TemplateField>
                                <%--Type--%>
                                <asp:TemplateField HeaderText="<%$ resources:Amount %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblgrdTaxAmount" runat="server" Text='<%#GetFormattedCurrencyWithComma(Eval("SLT_TAX_AMT")) %>'
                                            ToolTip='<%#GetFormattedCurrencyWithComma(Eval("SLT_TAX_AMT")) %>'></asp:Label></ItemTemplate>
                                    <ItemStyle Width="22%" />
                                </asp:TemplateField>
                                <%--Amount--%>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="TAXDELETE"
                                            TabIndex="58" CommandArgument="PageAction_Entry" SkinID="btnclose" ToolTip="<%$ Resources:Controls,Remove %>" />
                                        <%--OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"--%>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                                <%--Remove--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <%--Popup--%>
            <div id="divShortClose" style="display: none">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S padgtop7">
                                <asp:Label runat="server" ID="lblRemark" Text="<%$ resources:Remark%>" AssociatedControlID="txtRemarks"></asp:Label>
                                <asp:TextBox runat="server" ID="txtRemarks" TabIndex="18" TextMode="MultiLine" onkeyup="limitText(this,250);"
                                    onpaste="return false;" CssClass="input-w65per" MaxLength="250">
                                </asp:TextBox>
                                <div class="clear">
                                </div>
                                <asp:Label runat="server" ID="lblRefNo" Text="<%$ resources:RefNo%>" AssociatedControlID="txtRefNo"></asp:Label>
                                <asp:TextBox runat="server" ID="txtRefNo" CssClass="input-w65per" TabIndex="19" MaxLength="50">
                                </asp:TextBox>
                            </div>
                        </td>
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label class="margnrgt3">
                                    </label>
                                    <asp:Button runat="server" ToolTip="<%$ resources:ShortCloseSC %>" ID="btnShortCloseConfm"
                                        SkinID="btnInner-shortclose" Text="<%$Resources:Controls,ShortClose%>" class="inputbtn"
                                        OnClientClick="return ShowConfirmShortClose();" />
                                </div>
                            </td>
                        </tr>
                    </tr>
                </table>
            </div>
            <div id="divRevisionHistory" style="display: none" class="content-wrapper">
                <div class="gridwrap">
                    <asp:GridView runat="server" ID="grdRevisionHistory" Width="100%" AutoGenerateColumns="false"
                        EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ resources:RevDate %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRevisionDate" runat="server" Text='<%# Eval("SOH_DATE", Resources.ErpRes.DateFormatGrid) %>'
                                        ToolTip='<%# Eval("SOH_DATE", Resources.ErpRes.DateFormatGrid) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="30%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:SaleOrderNo %>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkRevisionPrint" runat="server" ToolTip="View" CssClass="text-underline"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="40%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:CUR %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRCUR" runat="server" Text='<%# Eval("SOH_CURRENCY_TEXT") %>' ToolTip='<%# Eval("SOH_CURRENCY_TEXT") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRTotalAmount" runat="server" Text='<%# GetFormattedCurrency(Eval("SOH_NET_AMOUNT")) %>'
                                        ToolTip='<%# GetFormattedCurrency(Eval("SOH_NET_AMOUNT")) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <asp:HiddenField runat="server" ID="hdfEnableSubType" Value="0" />
            <asp:HiddenField runat="server" ID="hdfAgtSaves" Value="-1" />
            <asp:HiddenField runat="server" ID="hdfErrorMsgType" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsPostback" Value="0" />
            <asp:HiddenField runat="server" ID="hdfFocusPdctAdd" Value="0" />
            <asp:HiddenField runat="server" ID="hdfOldCBMLimitPK" Value="0" />
            <asp:HiddenField runat="server" ID="hdfNewCBMLimitPK" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsWrkflwClose" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsLotNoIOReview" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsMsgIOReview" Value="0" />
            <asp:HiddenField runat="server" ID="hdfIsCancelled" Value="0" />
            <asp:HiddenField runat="server" ID="hdfVersion" Value="0" />
            <asp:HiddenField runat="server" ID="hdfMailAttachmentName" Value="" />
            <asp:HiddenField runat="server" ID="hdfIsForAment" Value="0" />
            <asp:HiddenField ID="hdfDecimalVal" runat="server" Value="0" />
            <asp:HiddenField ID="hdfNeedPackSpecVal" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCusDiscPerc" runat="server" Value="0" />
            <div style="display: none">
                <asp:Button runat="server" ID="btnDummySaveSubmit" CommandName="WRKFSUBMIT" TabIndex="64"
                    Text="" OnClick="ActionHandler" SkinID="btnInner-submit" />
                <asp:Button ID="btnShCloseSave" runat="server" OnClick="ActionHandler" CommandName="SHORTCLOSESAVE" />
                <asp:Button ID="btnPchQtyChange" runat="server" OnClick="ActionHandler" CommandName="TOOLTIP" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
