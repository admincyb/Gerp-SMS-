<%@ Page Title="<%$ Resources:Captions,Title_IODetails %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="InternalOrder.aspx.cs" Inherits="CustomerPortal.Sales.InternalOrder"
    Theme="ClassicExt" validateRequest="false"%>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/AlertControl.ascx" TagName="Alert" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });
        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtSaleOrderDate");
            GrandScriptUtils.DatePickerCommon("txtPODate");
            //            GrandScriptUtils.AddDateRangeCommon("txtBookingDate", "hdfBookingDate", "txtReqByDate", "hdfReqByDate", false, false, true);
            GrandScriptUtils.DatePickerCommon("txtShipmentDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", uiUrl, "hdfCustomer", true, true, "CUSTOMER");
            if ($("[id$=txtCustomer]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }

            GrandScriptUtils.MakeAutoCompleteDDL("txtFromPort", uiUrl + "?SIType=" + $("[id$=ddlSaleOrderType]").val() + "&SaleFromPort=1", "hdfFromPortID", true, true, "FILLPORTDETAILS");

            GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", uiUrl + "?Type=" + $("[id$=hdfCustomer]").val(), "hdfBrand", true, true, "CUTOMERBRANDWITHSPEC");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", uiUrl, "hdfCurrency", true, true, "CURRENCY");
            if ($("[id$=txtCurrency]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
            }
            if ($("[id$=hdfCustomer]").val() == "" || $("[id$=hdfCustomer]").val() == "0") {
                DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
            }
            ShowHideItemDetails($("[id$=hdfIsItemDetailsVisible]").val());
            ShowHideShippingDetails($("[id$=hdfIsShippingDetailsVisible]").val());
            ShowHideTerms($("[id$=hdfIsTermsVisible]").val());
            ShowHideRelatedInformation($("[id$=hdfIsRelatedInformationVisible]").val());
            $('a[disabled=disabled]').click(function () { return false; });
            SetLinkDisabled();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
        }
        function AfterDateSelect(controlID) {
            if (typeof AfterAlertControlDateSelect == "function") {
                AfterAlertControlDateSelect(controlID);
            }
            if (controlID == "txtReqByDate" && $("[id$=txtShipmentDate]").val() == "") {
                $("[id$=txtShipmentDate]").val($("[id$=txtReqByDate]").val());
            }
            else if (controlID == "txtBookingDate") {
                $("[id$=btnBookingDate]").click();
            }
        }
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=btnCustSelected]").click();
            }
            else if (targetControlID == "txtBrand") {
                $("[id$=btnSelectProduct]").click();
            }
            else if (targetControlID == "txtCurrency") {
                $("[id$=btnCurrency]").click();
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=hdfBrand]").val("0");
                $("[id$=txtBrand]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=txtBrandCode]").val("");
                $("[id$=txtPacking]").val("");
                $("[id$=hdfPackingSpec]").val("");
                $("[id$=hdfPackingText]").val("");
                $("[id$=txtQty]").val("");
                $("[id$=hdfUOM]").val("0");
                $("[id$=txtUOM]").val("");
                $("[id$=txtRate]").val("");
                $("[id$=txtDiscount]").val("");
                $("[id$=txtLotNo]").val("");

                $("[id$=hdfProduct]").val("0");
                $("[id$=hdfProductName]").val("");
                $("[id$=txtProduct]").val("");
                $("[id$=txtTotalPiecesCtn]").val("");
                $("[id$=txtAmount]").val("");
                $("[id$=txtTax]").val("");
                $("[id$=txtLotSize]").val("");
                $("[id$=txtDtlRemark]").val("");
                $("[id$=btnCustSelected]").click();
            }
            else if (targetControlID == "txtBrand") {
                $("[id$=txtBrandCode]").val("");
                $("[id$=txtPacking]").val("");
                $("[id$=hdfPackingSpec]").val("");
                $("[id$=hdfPackingText]").val("");
                $("[id$=txtQty]").val("");
                $("[id$=hdfUOM]").val("0");
                $("[id$=txtUOM]").val("");
                $("[id$=txtRate]").val("");
                $("[id$=txtDiscount]").val("");
                $("[id$=txtLotNo]").val("");

                $("[id$=hdfProduct]").val("0");
                $("[id$=hdfProductName]").val("");
                $("[id$=txtProduct]").val("");
                $("[id$=txtTotalPiecesCtn]").val("");
                $("[id$=txtAmount]").val("");
                $("[id$=txtTax]").val("");
                $("[id$=txtLotSize]").val("");
                $("[id$=txtDtlRemark]").val("");
            }
            else if (targetControlID == "txtCurrency") {
                $("[id$=txtCurrency]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfCurrency]").val("0");
                $("[id$=btnCurrency]").click();
            }
        }
        function GoBack() {
            var backURL = document.referrer;
            backURL = backURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
            window.location = backURL;
        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlEntry]").hide();
            }
            else {
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlEntry]").show();
                InitComponents();
            }
            return false;
        }
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode 
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSubmit]").hide();
            }
            else if (mode == 2) {
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
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function ShowContract() {
            ShowContainerDiv('#divTerms', '<%=GetLocalResourceObject("ContractTerms").ToString() %>', '700', '500');
        }
        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
            }
        }
        function CalculateAmount() {
            var qty = 0;
            var rate = 0;
            qty = parseFloat($("[id$=txtQty]").val());
            rate = parseFloat($("[id$=txtRate]").val());
            if (!isNaN(qty) && !isNaN(rate)) {
                var amount = qty * rate;
                $("[id$=txtAmount]").val(amount.toFixed(NumberDigits));
            }
            else
                $("[id$=txtAmount]").val(parseFloat(0).toFixed(NumberDigits));
        }

        function CalculateTotal(sender) {
            var subTotal = $("#[id*=grdItemDetails]").find('[id$=lblSubTotalFooter]').length > 0 ? parseFloat($("#[id*=grdItemDetails]").find('[id$=lblSubTotalFooter]').html().replace(new RegExp(',', 'g'), '')) : parseFloat(0);
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
            $("[id$=txtHdrTotal]").val((netTotal).toFixed(CurrencyDigits));
            $("[id$=txtHdrTotal]").attr("title", (netTotal).toFixed(CurrencyDigits));
            if (totalShipping == 0)
                $("[id$=txtShipping]").val((totalShipping).toFixed(CurrencyDigits));
            if (totalPriceAdj == 0)
                $("[id$=txtPriceAdj]").val((totalPriceAdj).toFixed(CurrencyDigits));
        }
        function ShowHideItemDetails(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divItemDetails]").show();
                $("[id$=imbShowItemDetails]").hide();
                $("[id$=imbHideItemDetails]").show();
            }
            else {
                $("[id$=divItemDetails]").hide();
                $("[id$=imbShowItemDetails]").show();
                $("[id$=imbHideItemDetails]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }
        function ShowHideShippingDetails(flag) {
            ///<summary>
            /// Used to Show/Hide HideShippingDetails Div
            ///</summary>

            //If flag then Show HideShippingDetails
            if (flag == 1) {
                $("[id$=divShippingDetails]").show();
                $("[id$=imbShowShippingDetails]").hide();
                $("[id$=imbHideShippingDetails]").show();
            }
            else {
                $("[id$=divShippingDetails]").hide();
                $("[id$=imbShowShippingDetails]").show();
                $("[id$=imbHideShippingDetails]").hide();
            }
            $("[id$=hdfIsShippingDetailsVisible]").val(flag);
            return false;
        }
        function ShowHideTerms(flag) {
            ///<summary>
            /// Used to Show/Hide Terms Div
            ///</summary>

            //If flag then Show Terms
            if (flag == 1) {
                $("[id$=divTermDetails]").show();
                $("[id$=imbShowTerms]").hide();
                $("[id$=imbHideTerms]").show();
            }
            else {
                $("[id$=divTermDetails]").hide();
                $("[id$=imbShowTerms]").show();
                $("[id$=imbHideTerms]").hide();
            }
            $("[id$=hdfIsTermsVisible]").val(flag);
            return false;
        }
        function ShowHideRelatedInformation(flag) {
            ///<summary>
            /// Used to Show/Hide Related Information Div
            ///</summary>

            //If flag then Show Related Information
            if (flag == 1) {
                $("[id$=divRelatedInformation]").show();
                $("[id$=imbShowRelatedInformation]").hide();
                $("[id$=imbHideRelatedInformation]").show();
            }
            else {
                $("[id$=divRelatedInformation]").hide();
                $("[id$=imbShowRelatedInformation]").show();
                $("[id$=imbHideRelatedInformation]").hide();
            }
            $("[id$=hdfIsRelatedInformationVisible]").val(flag);
            return false;
        }
        function SetNewArtWork(sender) {
            if ($(sender).attr("checked")) {
                $("[id$=lnkArtWorkPC]").hide();
                $("[id$=lnkArtWorkIB]").hide();
                $("[id$=lnkArtWorkIC]").hide();
                $("[id$=lnkArtWorkZB]").hide();
                $("[id$=lnkArtWorkMC]").hide();
                $("[id$=lnkArtWorkSC]").hide();
            }
            else {
                $("[id$=lnkArtWorkPC]").show();
                $("[id$=lnkArtWorkIB]").show();
                $("[id$=lnkArtWorkIC]").show();
                $("[id$=lnkArtWorkZB]").show();
                $("[id$=lnkArtWorkMC]").show();
                $("[id$=lnkArtWorkSC]").show();
            }
        }
        function SetLinkDisabled() {
            if ($("[id$=lnkArtWorkPC]").attr("href") == "" || $("[id$=lnkArtWorkPC]").attr("href") == "#") {
                $("[id$=lnkArtWorkPC]").addClass("link-disabled");
            }
            else {
                $("[id$=lnkArtWorkPC]").removeClass("link-disabled");
            }
            if ($("[id$=lnkArtWorkIB]").attr("href") == "" || $("[id$=lnkArtWorkIB]").attr("href") == "#") {
                $("[id$=lnkArtWorkIB]").addClass("link-disabled");
            }
            else {
                $("[id$=lnkArtWorkIB]").removeClass("link-disabled");
            }
            if ($("[id$=lnkArtWorkIC]").attr("href") == "" || $("[id$=lnkArtWorkIC]").attr("href") == "#") {
                $("[id$=lnkArtWorkIC]").addClass("link-disabled");
            }
            else {
                $("[id$=lnkArtWorkIC]").removeClass("link-disabled");
            }
            if ($("[id$=lnkArtWorkZB]").attr("href") == "" || $("[id$=lnkArtWorkZB]").attr("href") == "#") {
                $("[id$=lnkArtWorkZB]").addClass("link-disabled");
            }
            else {
                $("[id$=lnkArtWorkZB]").removeClass("link-disabled");
            }
            if ($("[id$=lnkArtWorkMC]").attr("href") == "" || $("[id$=lnkArtWorkMC]").attr("href") == "#") {
                $("[id$=lnkArtWorkMC]").addClass("link-disabled");
            }
            else {
                $("[id$=lnkArtWorkMC]").removeClass("link-disabled");
            }
            if ($("[id$=lnkArtWorkSC]").attr("href") == "" || $("[id$=lnkArtWorkSC]").attr("href") == "#") {
                $("[id$=lnkArtWorkSC]").addClass("link-disabled");
            }
            else {
                $("[id$=lnkArtWorkSC]").removeClass("link-disabled");
            }

            if ($("[id*=lnkLstArtWorkPC]").attr("href") == "" || $("[id*=lnkLstArtWorkPC]").attr("href") == "#" || $("[id*=lnkLstArtWorkPC]").attr("href") == "undefined") {
                $("[id*=lnkLstArtWorkPC]").addClass("link-disabled");
            }
            else {
                $("[id*=lnkLstArtWorkPC]").removeClass("link-disabled");
            }
            if ($("[id*=lnkLstArtWorkIB]").attr("href") == "" || $("[id*=lnkLstArtWorkIB]").attr("href") == "#" || $("[id*=lnkLstArtWorkIB]").attr("href") == "undefined") {
                $("[id*=lnkLstArtWorkIB]").addClass("link-disabled");
            }
            else {
                $("[id*=lnkLstArtWorkIB]").removeClass("link-disabled");
            }
            if ($("[id*=lnkLstArtWorkIC]").attr("href") == "" || $("[id*=lnkLstArtWorkIC]").attr("href") == "#" || $("[id*=lnkLstArtWorkIC]").attr("href") == "undefined") {
                $("[id*=lnkLstArtWorkIC]").addClass("link-disabled");
            }
            else {
                $("[id*=lnkLstArtWorkIC]").removeClass("link-disabled");
            }
            if ($("[id*=lnkLstArtWorkZB]").attr("href") == "" || $("[id*=lnkLstArtWorkZB]").attr("href") == "#" || $("[id*=lnkLstArtWorkZB]").attr("href") == "undefined") {
                $("[id*=lnkLstArtWorkZB]").addClass("link-disabled");
            }
            else {
                $("[id*=lnkLstArtWorkZB]").removeClass("link-disabled");
            }
            if ($("[id*=lnkLstArtWorkMC]").attr("href") == "" || $("[id*=lnkLstArtWorkMC]").attr("href") == "#" || $("[id*=lnkLstArtWorkMC]").attr("href") == "undefined") {
                $("[id*=lnkLstArtWorkMC]").addClass("link-disabled");
            }
            else {
                $("[id*=lnkLstArtWorkMC]").removeClass("link-disabled");
            }
            if ($("[id*=lnkLstArtWorkSC]").attr("href") == "" || $("[id*=lnkLstArtWorkSC]").attr("href") == "#" || $("[id*=lnkLstArtWorkSC]").attr("href") == "undefined") {
                $("[id*=lnkLstArtWorkSC]").addClass("link-disabled");
            }
            else {
                $("[id*=lnkLstArtWorkSC]").removeClass("link-disabled");
            }
        }
        function closeDeletePopup() {
            $("[id$=txtReason]").val("");
            $('#divConfirmationWithReason').dialog('close');
            ClosePopup();
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlQuotation">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="151" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li id="pnlPrintIO">
                                        <asp:Button runat="server" TabIndex="152" ID="btnPrintIO" CommandName="PRINTIO" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li id="pnlAlert" runat="server" style="display: none">
                                        <asp:Button runat="server" ID="btnAlert" CommandName="ALERT" TabIndex="153" Text="<%$resources:Controls,Alert %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:Controls,Alert %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-alert" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="154" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                                            <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                            <asp:HiddenField ID="hdfSaleOrderPK" runat="server" />
                                            <asp:HiddenField ID="hdfSaleOrderRate" runat="server" />
                                            <asp:HiddenField ID="hdfDecimalFormatWithComma" runat="server" />
                                            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
                                            <asp:Label ID="lbl" runat="server" Text="<%$ resources:SaleOrderNo%>" AssociatedControlID="lblSaleOrderNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblSaleOrderNo" CssClass="input-small" TabIndex="1"></asp:Label>
                                            <asp:Label runat="server" ID="lblSaleOrderDate" Text="<%$ resources:SaleOrderDate%>"
                                                AssociatedControlID="txtSaleOrderDate" CssClass="middle-lbl-small-c"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSaleOrderDate" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)" TabIndex="1"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfSaleOrderDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtSaleOrderDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SaleOrderDate %>">
                                            </asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfSaleOrderNo" runat="server" />
                                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$ resources:Customer%>" AssociatedControlID="lblCustomerName"></asp:Label>
                                            <asp:Label runat="server" ID="lblCustomerName" CssClass="input-half"></asp:Label>
                                            <%--<asp:Label runat="server" ID="lblCustomerName" ToolTip="<%$ resources:CustomerName%>"  ></asp:Label>--%>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblSaleOrderType" Text="<%$ resources:Type%>" AssociatedControlID="ddlSaleOrderType"></asp:Label>
                                            <asp:DropDownList ID="ddlSaleOrderType" runat="server" CssClass="select-small-a"
                                                TabIndex="2">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfSaleOrderType" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlSaleOrderType"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SaleOrderType %>"
                                                InitialValue="-1">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblCompany" runat="server" Text="<%$resources:Controls,CompanyPlant %>" AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="2" Enabled="false"
                                                CssClass="select-small-a">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trQuotationReference" runat="server" visible="false">
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRefNo" runat="server" Text="<%$ resources:Reference%>" AssociatedControlID="txtRefNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRefNo" Enabled="false" CssClass="input-half input-disabled"
                                                TabIndex="3"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRefDate" runat="server" Text="<%$ resources:ReferenceDate%>" AssociatedControlID="txtRefDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRefDate" Enabled="false" CssClass="date-picker input-disabled"
                                                TabIndex="4"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr runat="server" visible="false">
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBuyer" runat="server" AssociatedControlID="txtCustomer" Text="<%$ resources:Buyer %>">
                                            </asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" MaxLength="100"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomer" runat="server" />
                                            <asp:Button ID="btnCustSelected" runat="server" OnClick="ActionHandler" CommandName="CUSTOMERSELECTED"
                                                EnableTheming="false" Style="display: none" />
                                            <asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtCustomer" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Buyer %>"></asp:RequiredFieldValidator>
                                            <asp:Label ID="Label3" runat="server" AssociatedControlID="txtBuyerAddress"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBuyerAddress" MaxLength="500" TextMode="MultiLine"
                                                CssClass="multiline-m1col input-disabled" onkeydown="return EnableArrowKey(event)"
                                                onpaste="return false;" onkeyup="limitText(this,500);"></asp:TextBox>
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
                                            <asp:Label ID="lblRef" runat="server" Text="<%$ resources:PONo%>" AssociatedControlID="txtPONo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPONo" MaxLength="100"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfPONo" CssClass="star" SetFocusOnError="true" ValidationGroup="so"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtPONo" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Err_PONo %>"></asp:RequiredFieldValidator>
                                            <asp:Label ID="lblReferenceDate" runat="server" Text="<%$ resources:PODate%>" AssociatedControlID="txtPODate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPODate" CssClass="date-picker" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfPODate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtPODate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PODate %>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblBookingDate" Text="<%$ resources:BookingDate%>"
                                                AssociatedControlID="txtBookingDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBookingDate" CssClass="date-picker" onkeydown="return CheckKey(event)"
                                                MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)"></asp:TextBox>
                                            <asp:Button ID="btnBookingDate" runat="server" EnableTheming="false" OnClick="ActionHandler"
                                                CommandName="BOOKINGDATECHANGE" Style="display: none" />
                                            <asp:RequiredFieldValidator ID="vrfBookingDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtBookingDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BookingDate %>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr runat="server" visible="false">
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrency"></asp:Label>
                                            <asp:TextBox ID="txtCurrency" runat="server" CssClass="medium" MaxLength="100"></asp:TextBox>
                                            <asp:HiddenField ID="hdfCurrency" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtCurrency"
                                                Display="Dynamic" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                Text="*" ErrorMessage="<%$ resources:Err_Currency%>"></asp:RequiredFieldValidator>
                                            <asp:Button ID="btnCurrency" runat="server" OnClick="ActionHandler" CommandName="EXCHANGERATE"
                                                EnableTheming="false" Style="display: none" />
                                            <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("ItemDetails").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowItemDetails" OnClientClick="javascript:return ShowHideItemDetails(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowItemDetails %>" />
                                <asp:ImageButton runat="server" ID="imbHideItemDetails" OnClientClick="javascript:return ShowHideItemDetails();"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:HideItemDetails %>" />
                                <asp:HiddenField ID="hdfIsItemDetailsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divItemDetails" style="display: none">
                                <table class="table-devide" id="tblDetails">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:HiddenField ID="hdfDetailPK" runat="server" Value="0" />
                                                <asp:Label ID="lblBrand" runat="server" AssociatedControlID="txtBrand" Text="<%$ resources:Brand_Mand %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtBrand" runat="server" MaxLength="200" TabIndex="5"></asp:TextBox>
                                                <asp:HiddenField ID="hdfBrand" runat="server" />
                                                <asp:Button ID="btnSelectProduct" runat="server" OnClick="ActionHandler" CommandName="PRODUCTSELECTED"
                                                    EnableTheming="false" Style="display: none" />
                                                <asp:RequiredFieldValidator ID="vrfBrand" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="scDetails" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                    runat="server" ControlToValidate="txtBrand" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Brand %>"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblBrandCode" runat="server" AssociatedControlID="txtBrandCode" Text="<%$ resources:BrandCode %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtBrandCode" runat="server" MaxLength="200" Enabled="false" CssClass="input-half input-disabled"
                                                    TabIndex="6"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblPacking" Text="<%$ resources:Packing %>" AssociatedControlID="txtPacking"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPacking" CssClass="input-half input-disabled"
                                                    onkeydown="return EnableArrowKey(event)" onpaste="return false;" TabIndex="8"></asp:TextBox>
                                                <asp:HiddenField ID="hdfPackingSpec" runat="server" />
                                                <asp:HiddenField ID="hdfPackingText" runat="server"  />
                                                <%--<asp:HiddenField ID="hdfArtWork" runat="server" />--%>
                                                <asp:HiddenField runat="server" ID="hdfCBM" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblProduct" runat="server" AssociatedControlID="txtProduct" Text="<%$ resources:Product %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtProduct" runat="server" MaxLength="100" Enabled="false" CssClass="input-half input-disabled"
                                                    TabIndex="7"></asp:TextBox>
                                                <asp:HiddenField ID="hdfProduct" runat="server" />
                                                <asp:HiddenField ID="hdfProductName" runat="server" />
                                                <asp:Label runat="server" ID="lblTotalPiecesCtn" Text="<%$ resources:TotalPiecesCtn %>"
                                                    AssociatedControlID="txtTotalPiecesCtn"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtTotalPiecesCtn" CssClass="input-small input-w80 input-disabled numeric"
                                                    Enabled="false" TabIndex="9"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblReqByDate" Text="<%$ resources:ReqdDate_Mand %>"
                                                    AssociatedControlID="txtReqByDate" CssClass="middle-lbl-small-d"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtReqByDate" MaxLength="12" CssClass="input-small"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;" TabIndex="10"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hdfReqByDate" />
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfReqByDate" CssClass="star" SetFocusOnError="false"
                                                        ValidationGroup="scDetails" EnableClientScript="true" runat="server" ControlToValidate="txtReqByDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqByDate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vreReqByDate" CssClass="star" ValidationGroup="scDetails"
                                                        runat="server" ControlToValidate="txtReqByDate" SetFocusOnError="false" ErrorMessage="<%$ resources:Err_ReqByDate_Valid %>"
                                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                        EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblQty" runat="server" AssociatedControlID="txtQty" Text="<%$ resources:Quantity_Mand %>"><%--<%$ resources:Quantity %>--%>
                                                </asp:Label>
                                                <asp:TextBox ID="txtQty" runat="server" CssClass="input-small numeric" MaxLength="11"
                                                    TabIndex="11" onchange="CalculateAmount();"></asp:TextBox>
                                                <span style="width: 10px; border: 0 none; background: none;" class="nomargin">
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfQuantity" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="scDetails" EnableClientScript="true" runat="server" ControlToValidate="txtQty"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:QuantityValidation ID="vreQuantity" runat="server" ControlToValidate="txtQty"
                                                            NumberDigits="7" ErrorMessage="<%$ resources:Err_Quantity_Valid %>" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="scDetails"></cc1:QuantityValidation>
                                                        <asp:RangeValidator ID="rngQuantity" runat="server" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="scDetails" EnableClientScript="true" Display="Dynamic" Text="*"
                                                            ControlToValidate="txtQty" Type="Double" ErrorMessage="<%$ resources:Err_Quantity_Valid %>"
                                                            MinimumValue="1" MaximumValue="100000000000"></asp:RangeValidator>
                                                    </div>
                                                </span>
                                                <asp:HiddenField ID="hdfUOM" runat="server" />
                                                <asp:TextBox runat="server" ID="txtUOM" CssClass="input-normal Uiinput-uom" Enabled="false"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <div id="Div1" runat="server" visible="false">
                                                    <asp:Label ID="lblRate" runat="server" AssociatedControlID="txtRate" Text="<%$ resources:Rate_Mand %>">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtRate" runat="server" CssClass="input-w80 numeric" MaxLength="16"
                                                        onchange="CalculateAmount();"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="scDetails"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtRate" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:RateValidation ID="vreRate" runat="server" ControlToValidate="txtRate" ErrorMessage="<%$ resources:Err_Rate_Valid %>"
                                                            NumberDigits="10" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                            ValidationGroup="scDetails"></cc1:RateValidation>
                                                        <asp:RangeValidator ID="vrgRate" runat="server" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="scDetails" EnableClientScript="true" Display="Dynamic" Text="*"
                                                            ControlToValidate="txtRate" Type="Double" ErrorMessage="<%$ resources:Err_Rate_Valid %>"
                                                            MinimumValue="1" MaximumValue="10000000000000000"></asp:RangeValidator>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <div class="clear">
                                                </div>
                                                <div runat="server" visible="false">
                                                    <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" Text="<%$ resources:Amount_Mand %>">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtAmount" runat="server" CssClass="input-small input-disabled numeric"
                                                        MaxLength="15" onkeydown="return false;" onpaste="return false;"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="scDetails" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                        </asp:RequiredFieldValidator>
                                                        <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtAmount"
                                                            ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="scDetails"></cc1:AmountValidation>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                                <div id="Div2" runat="server" visible="false">
                                                    <asp:Label ID="lblItemDiscount" runat="server" AssociatedControlID="txtDiscount"
                                                        Text="<%$ resources:Discount %>">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtDiscount" runat="server" CssClass="input-w80 input-disabled numeric"
                                                        MaxLength="14" onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>
                                                    <asp:ImageButton ID="imgDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                        ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCDETAILS" ValidationGroup="scDetails"
                                                        OnClientClick="javascript:ValidatePageNow('scDetails')" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblItemTax" runat="server" AssociatedControlID="txtTax" Text="<%$ resources:Tax %>">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtTax" runat="server" CssClass="input-w80 input-disabled numeric"
                                                        MaxLength="14" onkeydown="return EnableArrowKey(event)" onpaste="return false;"></asp:TextBox>
                                                    <asp:ImageButton ID="imgTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                        ToolTip="<%$ resources:Tax %>" CommandName="TAXDETAILS" ValidationGroup="scDetails"
                                                        OnClientClick="javascript:ValidatePageNow('scDetails')" />
                                                </div>
                                                <%--<asp:CheckBox ID="chkNewArtWork" runat="server" TabIndex="19" Text="<%$resources:NewArtWork%>"
                                                    TextAlign="Left" Checked="false" onclick="SetNewArtWork(this);" />--%>
                                                <asp:Label ID="lblArtWork" runat="server" AssociatedControlID="ddlArtWork" Text="<%$ resources:ArtWork %>">
                                                </asp:Label>
                                                <asp:DropDownList ID="ddlArtWork" runat="server" AutoPostBack="true" CssClass="select-small-b"
                                                    OnSelectedIndexChanged="ActionHandler" TabIndex="12">
                                                </asp:DropDownList>
                                                <div id="DivCaseMark" runat="server">
                                                 <asp:Label ID="lblCaseMark" runat="server" AssociatedControlID="txtCaseMark" Text="<%$ resources:CaseMark %>" CssClass="margnrgt-minus1">
                                                </asp:Label>
                                                <asp:TextBox ID="txtCaseMark" MaxLength="100" runat="server" CssClass="input-half"></asp:TextBox>
                                                </div>
                                                  
                                                <a id="lnkArtWorkPC" runat="server" href="#" visible="false" target="_blank"></a>
                                                <a id="lnkArtWorkIB" runat="server" href="#" visible="false" target="_blank"></a>
                                                <a id="lnkArtWorkIC" runat="server" href="#" visible="false" target="_blank"></a>
                                                <a id="lnkArtWorkZB" runat="server" href="#" visible="false" target="_blank"></a>
                                                <a id="lnkArtWorkMC" runat="server" href="#" visible="false" target="_blank"></a>
                                                <a id="lnkArtWorkSC" runat="server" href="#" visible="false" target="_blank"></a>
                                                <asp:HiddenField ID="hdfArtWorkPC" runat="server" />
                                                <asp:HiddenField ID="hdfArtWorkIB" runat="server" />
                                                <asp:HiddenField ID="hdfArtWorkIC" runat="server" />
                                                <asp:HiddenField ID="hdfArtWorkZB" runat="server" />
                                                <asp:HiddenField ID="hdfArtWorkMC" runat="server" />
                                                <asp:HiddenField ID="hdfArtWorkSC" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblLotNo" runat="server" AssociatedControlID="txtLotNo" Text="<%$ resources:LotNo %>">
                                                </asp:Label>
                                                <asp:TextBox runat="server" ID="txtLotNo" CssClass="input-small" MaxLength="100" TabIndex="13"></asp:TextBox>
                                                <asp:Label ID="lblLotSize" runat="server" AssociatedControlID="txtLotSize" Text="<%$ resources:LotSize %>"
                                                    CssClass="middle-lbl-small-d">
                                                </asp:Label>
                                                <asp:TextBox runat="server" ID="txtLotSize" CssClass="input-small" MaxLength="100" TabIndex="14"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:TextBox ID="txtTotal" runat="server" EnableTheming="false" Text="0" Style="display: none" />
                                                <asp:Label runat="server" ID="lblDtlRemark" Text="<%$ resources:Remarks %>" AssociatedControlID="txtDtlRemark"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDtlRemark" MaxLength="480" TabIndex="15" TextMode="MultiLine"></asp:TextBox>
                                                
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblDtlRemark2" Text="<%$ resources:Remarks2 %>" AssociatedControlID="txtDtlRemark2"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDtlRemark2" MaxLength="480" TabIndex="15" TextMode="MultiLine"></asp:TextBox>
                                                <asp:ImageButton runat="server" ID="btnClearItem" CommandName="CLEARITEM" OnClick="ActionHandler"
                                                    ToolTip="<%$resources:Controls,Clear %>" CommandArgument="SEC_ActionPanel" SkinID="cancel"
                                                    TabIndex="15" style="margin-top:49px!important;" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap scroll-container">
                                <asp:GridView ID="grdItemDetails" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                    AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    Width="1399px" ShowFooter="true" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:BrandName %>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfSODPK" runat="server" Value='<%#Eval("SOD_PK") %>' />
                                                <asp:HiddenField ID="hdfCusItemPK" runat="server" Value='<%#Eval("SOD_CUST_ITEM") %>' />
                                                <asp:Label ID="lblBrandName" runat="server" Text='<%# Eval("SOD_CUST_ITEM_TEXT") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_CUST_ITEM_CODE"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="250px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ProductCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_ITEM_CODE"),16) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("SOD_ITEM_TEXT").ToString()) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%#Eval("SOD_ITEM") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOM" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_UOM_TEXT"),10) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("SOD_UOM_TEXT").ToString()) %>'></asp:Label>
                                                <asp:HiddenField ID="hdfUoM" runat="server" Value='<%#Eval("SOD_UOM") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblfooterTot" Text="<%$ resources:Total %>"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="40px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Qty%>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedNumber(Eval("SOD_QTY")) %>'
                                                    ToolTip='<%# GetFormattedNumber(Eval("SOD_QTY")) %>'></asp:Label>--%>
                                                <asp:Label ID="lblQuantity" runat="server" CssClass="ItemQuantity" Text='<%# GetFormattedNumberWithComma(Eval("SOD_QTY")) %>'
                                                    ToolTip='<%# GetFormattedNumberWithComma(Eval("SOD_QTY")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="60px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblItemTotalQty" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CartonsOrBags %>" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCartonsOrBags" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="100px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblItemTotalCarton" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Rate %>" HeaderStyle-CssClass="amount-numeric"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemRate" runat="server" Text='<%# GetFormattedRate(Eval("SOD_RATE")) %>'
                                                    ToolTip='<%# GetFormattedRate(Eval("SOD_RATE")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <%--<asp:Label ID="lblTotalCBM" runat="server"></asp:Label>--%>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Amount %>" HeaderStyle-CssClass="amount-numeric"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemAmount" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrency(Eval("SOD_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblItemTotalAmount" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Discount %>" HeaderStyle-CssClass="amount-numeric"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemDiscount" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_DISCOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrency(Eval("SOD_DISCOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblDiscountTotal"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Tax %>" HeaderStyle-CssClass="amount-numeric"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemTax" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_TAX")) %>'
                                                    ToolTip='<%# GetFormattedCurrency(Eval("SOD_TAX")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTaxTotal"></asp:Label></FooterTemplate>
                                        </asp:TemplateField>
                                        <%--Pay Now--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Total %>" HeaderStyle-CssClass="amount-numeric"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemTotal" runat="server" Text='<%# GetFormattedCurrency(Eval("SOD_NET_AMOUNT")) %>'
                                                    ToolTip='<%# GetFormattedCurrency(Eval("SOD_NET_AMOUNT")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" CssClass="amount-numeric" />
                                            <FooterStyle CssClass="amount-numeric" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblSubTotalFooter" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LotNo %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_LOT_NO"), 12) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_LOT_NO"))) %>'
                                                    ID="lblItemLotNo"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="85px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LotSize %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_LOT_SIZE"), 10) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_LOT_SIZE"))) %>'
                                                    ID="lblItemLotSize"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ArtWork %>">
                                            <ItemTemplate>
                                                <div class="link-inline">
                                                    <a id="lnkLstArtWorkPC" runat="server" href="#" visible="false" target="_blank">
                                                    </a><a id="lnkLstArtWorkIB" runat="server" href="#" visible="false" target="_blank">
                                                    </a><a id="lnkLstArtWorkIC" runat="server" href="#" visible="false" target="_blank">
                                                    </a><a id="lnkLstArtWorkZB" runat="server" href="#" visible="false" target="_blank">
                                                    </a><a id="lnkLstArtWorkMC" runat="server" href="#" visible="false" target="_blank">
                                                    </a><a id="lnkLstArtWorkSC" runat="server" href="#" visible="false" target="_blank">
                                                    </a>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle Width="180px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ReqdShipDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemReqdShipDate" runat="server" Text='<%#Eval("SOD_REQUIRED_BY") %>'
                                                    ToolTip='<%# Eval("SOD_REQUIRED_BY") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="80px" />
                                        </asp:TemplateField>
                                        <%------------------ CBM-----------------------------%>
                                        <asp:TemplateField HeaderText="<%$ resources:CBM1 %>" HeaderStyle-CssClass="amount-numeric"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <%--  <asp:Label ID="Label6" runat="server" Text='<%# Convert.ToDouble(Eval("SOD_QTY")) %>'></asp:Label>
                                          <asp:Label ID="Label7" runat="server" Text='<%# Convert.ToDouble(Eval("APS_TOTAL_PCS")) %>'></asp:Label>--%>
                                              <%--  <asp:Label ID="lblCBM" runat="server" Text='<%#( Math.Ceiling(Convert.ToDouble(Eval("SOD_QTY")) / Convert.ToDouble(Eval("APS_TOTAL_PCS"))) * Convert.ToDouble(Eval("CBM"))).ToString("N4") %>'
                                                    ToolTip='<%# ((Convert.ToDouble(Eval("SOD_QTY")) / Convert.ToDouble(Eval("APS_TOTAL_PCS"))) * Convert.ToDouble(Eval("CBM"))).ToString("N4") %>'></asp:Label>--%>
                                            <asp:Label ID="lblCBM" runat="server" Text='<%# Eval("SOD_IS_PACK_MAT").ToString() == "1" ||Eval("SOD_IS_PACK_MAT").ToString() == "2" ? "-" :   Math.Round(Convert.ToDouble(Eval("SOD_QTY"))/ Convert.ToDouble(Eval("APS_TOTAL_PCS"))*  Convert.ToDouble(Eval("CBM")), 4).ToString("N4")%>'
                                                    ToolTip='<%# Eval("SOD_IS_PACK_MAT").ToString() == "1" ? "-" : Math.Round(Convert.ToDouble(Eval("SOD_QTY"))/ Convert.ToDouble(Eval("APS_TOTAL_PCS"))*  Convert.ToDouble(Eval("CBM")), 4).ToString("N4")%>'></asp:Label>
                                                </ItemTemplate>
                                            <ItemStyle Width="100px" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:remarks %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SOD_REMARKS"), 14) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_REMARKS"))) %>'
                                                    ID="lblItemRemarks"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="130px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CaseMark %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCaseMark" runat="server" Text='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_CASE_MARK"))) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SOD_CASE_MARK"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="110px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                    CommandArgument="PageAction_Entry" SkinID="imbeditgrid" ToolTip="Edit" TabIndex="16" />
                                            </ItemTemplate>
                                            <ItemStyle Width="50px" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="gridwrap">
                                <asp:Label ID="lblTotalCBMLbl" Font-Bold="true" runat="server" Text="<%$ resources:CBM%>"
                                    AssociatedControlID="lblTotalCBMLbl"></asp:Label>
                                <asp:Label ID="lblTotalCBM" AssociatedControlID="lblTotalCBM" runat="server"></asp:Label>
                            </div>
                            <div id="divCalc" style="display: none">
                                <div class="gridwrap">
                                    <table id="tblCalc" class="gridwraptable gridwrap">
                                        <tr>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrDiscount" SkinID="discount" runat="server" OnClick="ActionHandler"
                                                    ToolTip="<%$ resources:Controls,Discounts %>" CommandName="DISCHEADER" />
                                                <asp:TextBox ID="txtHdrDiscount" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    MaxLength="16" Enabled="false"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfDiscount" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtHdrDiscount"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TotalDiscount%>"></asp:RequiredFieldValidator>--%>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblTax" Text="<%$ resources:Tax%>" AssociatedControlID="txtHdrTax"></asp:Label>
                                            </td>
                                            <td style="text-align: right" class="btn-margin">
                                                <asp:ImageButton ID="imgHdrTax" SkinID="tax" runat="server" OnClick="ActionHandler"
                                                    ToolTip="<%$ resources:Tax %>" CommandName="TAXHEADER" />
                                                <asp:TextBox ID="txtHdrTax" runat="server" CssClass="input-w80 numeric input-disabled"
                                                    Enabled="false" MaxLength="16"></asp:TextBox>
                                                <%--<asp:RequiredFieldValidator ID="vrfTax" CssClass="star" SetFocusOnError="true" ValidationGroup="so"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtHdrTax" Display="Dynamic"
                                                    Text="*" ErrorMessage="<%$ resources:Err_TotalTax%>"></asp:RequiredFieldValidator>--%>
                                                <div class="clear">
                                                    <div class="clear">
                                                    </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblShipping" Text="<%$ resources:Shipping%>" AssociatedControlID="txtShipping"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtShipping" runat="server" CssClass="input-w80 numeric" onchange="CalculateTotal(this);"
                                                    MaxLength="16"></asp:TextBox>
                                                <div class="starwrap">
                                                    <cc1:AmountValidation ID="vamShipping" runat="server" ControlToValidate="txtShipping"
                                                        ErrorMessage="<%$ resources:Err_Valid_Shipping %>" NumberDigits="12" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="so"></cc1:AmountValidation>
                                                    <%--<asp:RequiredFieldValidator ID="vrfShipping" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtShipping"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Shipping%>"></asp:RequiredFieldValidator>--%>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <asp:Label runat="server" ID="lblPriceAdj" Text="<%$ resources:PriceAdj%>" AssociatedControlID="txtPriceAdj"></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:TextBox ID="txtPriceAdj" runat="server" CssClass="input-w80 numeric" onchange="CalculateTotal(this);"
                                                    MaxLength="16"></asp:TextBox>
                                                <div class="starwrap">
                                                    <cc1:AmountValidation ID="vamPriceAdj" runat="server" ControlToValidate="txtPriceAdj"
                                                        ErrorMessage="<%$ resources:Err_Valid_PriceAdj %>" NumberDigits="12" AllowNegative="true"
                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="so"></cc1:AmountValidation>
                                                    <%--<asp:RequiredFieldValidator ID="vrfPriceAdj" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="txtPriceAdj"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PriceAdj%>"></asp:RequiredFieldValidator>--%>
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
                                                <asp:TextBox ID="txtHdrTotal" runat="server" CssClass="input-w80 numeric input-disabled"
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
                                    <%= GetLocalResourceObject("ShippingDetails").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowShippingDetails" OnClientClick="javascript:return ShowHideShippingDetails(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowShippingDetails %>" />
                                <asp:ImageButton runat="server" ID="imbHideShippingDetails" OnClientClick="javascript:return ShowHideShippingDetails();"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:HideShippingDetails %>" />
                                <asp:HiddenField ID="hdfIsShippingDetailsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divShippingDetails" style="display: none">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblShipBy" runat="server" AssociatedControlID="ddlShipBy" Text="<%$ resources:ShipBy %>">
                                                </asp:Label>
                                                <asp:DropDownList ID="ddlShipBy" runat="server" CssClass="select-half" TabIndex="17">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblFromPort" runat="server" AssociatedControlID="txtFromPort" Text="<%$ resources:FromPort %>"></asp:Label>
                                               <%-- <asp:DropDownList ID="ddlFromPortID" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half" TabIndex="20">
                                                </asp:DropDownList>--%>
                                                <asp:TextBox runat="server" ID="txtFromPort" CssClass="input-halfsmall-a" TabIndex="20"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hdfFromPortID" />
                                                <asp:Label ID="lblTranshipment" runat="server" AssociatedControlID="ddlTranshipment"
                                                    Text="<%$ resources:Transhipment %>"></asp:Label>
                                                <asp:DropDownList ID="ddlTranshipment" runat="server" CssClass="select-half" TabIndex="22">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblShipmentDate" Text="<%$ resources:ShipmentDate%>"
                                                    AssociatedControlID="txtShipmentDate"></asp:Label>
                                                <asp:TextBox ID="txtShipmentDateText" runat="server" CssClass="half" Text="<%$ resources:ShipmentText %>"
                                                    MaxLength="100" TabIndex="18"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="txtShipmentDate" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                    MaxLength="11" onpaste="return false;" onchange="AfterDateSelect(null)" TabIndex="19"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblToPort" runat="server" AssociatedControlID="txtToPort" Text="<%$ resources:ToPort %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtToPort" runat="server" MaxLength="100" CssClass="input-halfsmall-a" TabIndex="21"></asp:TextBox>
                                                <asp:Label ID="lblPortofDischarge" runat="server" AssociatedControlID="txtPortofDischarge"
                                                    Text="<%$ resources:FinalDestination %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtPortofDischarge" runat="server" MaxLength="100" CssClass="input-halfsmall-a" TabIndex="23"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblConsigneeDetails" runat="server" AssociatedControlID="ddlConsigneeDetails"
                                                    Text="<%$ resources:ConsigneeDetails %>"></asp:Label>
                                                <asp:DropDownList ID="ddlConsigneeDetails" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half" TabIndex="24">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblConsigneeDtl" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtConsigneeDetails" MaxLength="500" onkeydown="return EnableArrowKey(event)"
                                                    onpaste="return false;" TextMode="MultiLine" CssClass="multiline-m1col input-disabled input-halfsmall-a"
                                                    onkeyup="limitText(this,500);" TabIndex="26"></asp:TextBox>
                                                <asp:HiddenField ID="hdfCNEName" runat="server" />
                                                <asp:HiddenField ID="hdfCNEAddress" runat="server" />
                                                <asp:HiddenField ID="hdfCNECountry" runat="server" />
                                                <asp:HiddenField ID="hdfCNECountryText" runat="server" />
                                                <asp:HiddenField ID="hdfCNEZip" runat="server" />
                                                <asp:HiddenField ID="hdfCNEPhone" runat="server" />
                                                <asp:HiddenField ID="hdfCNEMobile" runat="server" />
                                                <asp:HiddenField ID="hdfCNEFax" runat="server" />
                                                <asp:HiddenField ID="hdfCNEEmail" runat="server" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblShippingAddress" Text="<%$ resources:ShippingAddress %>"
                                                    AssociatedControlID="ddlCustAddress"></asp:Label>
                                                <asp:DropDownList ID="ddlCustAddress" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half" TabIndex="25">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label5" runat="server" AssociatedControlID="txtShippingAddress"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtShippingAddress" MaxLength="500" TextMode="MultiLine"
                                                    EnableTheming="false" CssClass="multiline-m1col input-disabled input-halfsmall-a" onkeydown="limitText(this,500);"
                                                    onkeyup="limitText(this,500);" TabIndex="27"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vreShippingAddress" runat="server" ControlToValidate="txtShippingAddress"
                                                    ErrorMessage="<%$ Resources:Err_ShippingAddress %>" ValidationExpression="^[\s\S]{0,500}$"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                <asp:HiddenField ID="hdfShpName" runat="server" />
                                                <asp:HiddenField ID="hdfShpAddress" runat="server" />
                                                <asp:HiddenField ID="hdfShpCountry" runat="server" />
                                                <asp:HiddenField ID="hdfShpCountryText" runat="server" />
                                                <asp:HiddenField ID="hdfShpZip" runat="server" />
                                                <asp:HiddenField ID="hdfShpPhone" runat="server" />
                                                <asp:HiddenField ID="hdfShpMobile" runat="server" />
                                                <asp:HiddenField ID="hdfShpFax" runat="server" />
                                                <asp:HiddenField ID="hdfShpEmail" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblNotifyParty" runat="server" AssociatedControlID="ddlNotifyParty"
                                                    Text="<%$ resources:NotifyParty %>"></asp:Label>
                                                <asp:DropDownList ID="ddlNotifyParty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half" TabIndex="28">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblNotifyPrty" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtNotifyParty" MaxLength="500" TextMode="MultiLine"
                                                    CssClass="multiline-m1col input-disabled input-halfsmall-a" onkeydown="return EnableArrowKey(event)"
                                                    onpaste="return false;" onkeyup="limitText(this,500);" TabIndex="30"></asp:TextBox>
                                                <asp:HiddenField ID="hdfNPName" runat="server" />
                                                <asp:HiddenField ID="hdfNPAddress" runat="server" />
                                                <asp:HiddenField ID="hdfNPCountry" runat="server" />
                                                <asp:HiddenField ID="hdfNPCountryText" runat="server" />
                                                <asp:HiddenField ID="hdfNPZip" runat="server" />
                                                <asp:HiddenField ID="hdfNPPhone" runat="server" />
                                                <asp:HiddenField ID="hdfNPMobile" runat="server" />
                                                <asp:HiddenField ID="hdfNPFax" runat="server" />
                                                <asp:HiddenField ID="hdfNPEmail" runat="server" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblAgent" runat="server" AssociatedControlID="ddlAgent" Text="<%$ resources:SAgent %>">
                                                </asp:Label>
                                                <asp:DropDownList ID="ddlAgent" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half" TabIndex="29">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdfAgentName" runat="server" />
                                                <asp:HiddenField ID="hdfAgentAddress" runat="server" />
                                                <asp:HiddenField ID="hdfAgentCountry" runat="server" />
                                                <asp:HiddenField ID="hdfAgentCountryText" runat="server" />
                                                <asp:HiddenField ID="hdfAgentZip" runat="server" />
                                                <asp:HiddenField ID="hdfAgentPhone" runat="server" />
                                                <asp:HiddenField ID="hdfAgentMobile" runat="server" />
                                                <asp:HiddenField ID="hdfAgentFax" runat="server" />
                                                <asp:HiddenField ID="hdfAgentEmail" runat="server" />
                                                <asp:Label ID="lblShppingIntimationto" runat="server" AssociatedControlID="txtShppingIntimationto"
                                                    Text="<%$ resources:ShppingIntimationto %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtShppingIntimationto" runat="server" MaxLength="100" CssClass="input-halfsmall-a" TabIndex="31"></asp:TextBox>
                                                <asp:Label ID="lblShppingIntimationtoFax" runat="server" AssociatedControlID="txtShppingIntimationtoFax"
                                                    Text="<%$ resources:ShppingIntimationtoFax %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtShppingIntimationtoFax" runat="server" MaxLength="100" CssClass="input-halfsmall-a" TabIndex="32"></asp:TextBox>

                                                  <asp:Label ID="lblContainerSize" runat="server" AssociatedControlID="txtContainerSize"
                                                    Text="<%$ resources:ContainerSize %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtContainerSize" runat="server" TabIndex="49" MaxLength="100" CssClass="input-halfsmall-a"></asp:TextBox>

                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("Terms").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowTerms" OnClientClick="javascript:return ShowHideTerms(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowTerms %>" />
                                <asp:ImageButton runat="server" ID="imbHideTerms" OnClientClick="javascript:return ShowHideTerms();"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:HideTerms %>" />
                                <asp:HiddenField ID="hdfIsTermsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divTermDetails" style="display: none">
                                <table class="table-devide tablelayout">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblDeliveryTerms" runat="server" AssociatedControlID="ddlDeliveryTerms" CssClass="w25perc"
                                                    Text="<%$ resources:DeliveryTerms %>"></asp:Label>
                                                <asp:DropDownList ID="ddlDeliveryTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half" TabIndex="33">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label1" runat="server" Text="" AssociatedControlID="txtDeliveryTerms" CssClass="w25perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDeliveryTerms" MaxLength="500" TextMode="MultiLine"
                                                    CssClass="multiline-m1col input-halfsmall-a" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" TabIndex="35"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblPaymentTerms" runat="server" AssociatedControlID="ddlPaymentTerms"
                                                    Text="<%$ resources:PaymentTerms %>"></asp:Label>
                                                <asp:DropDownList ID="ddlPaymentTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half" TabIndex="34">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label2" runat="server" Text="" AssociatedControlID="txtPaymentTerms"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPaymentTerms" MaxLength="500" TextMode="MultiLine"
                                                    CssClass="multiline-m1col input-halfsmall-a" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" TabIndex="36"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblSpecialCause" runat="server" AssociatedControlID="ddlSpecialCause"
                                                    Text="<%$ resources:SpecialCause %>"></asp:Label>
                                                <asp:DropDownList ID="ddlSpecialCause" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half" TabIndex="37">
                                                </asp:DropDownList>
                                                <asp:Label ID="Label4" runat="server" Text="" AssociatedControlID="txtSpecialCause"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtSpecialCause" MaxLength="500" TextMode="MultiLine"
                                                    CssClass="multiline-m1col input-halfsmall-a" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" TabIndex="39"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblBankDetails" runat="server" AssociatedControlID="ddlBankDetails"
                                                    Text="<%$ resources:BankDetails %>"></asp:Label>
                                                <asp:DropDownList ID="ddlBankDetails" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half" TabIndex="38">
                                                </asp:DropDownList>
                                                <asp:CheckBox ID="chkNeedAdvPay" runat="server" Text="<%$resources:NeedAdvPay%>"
                                                    TextAlign="Left" Checked="true" TabIndex="40" />
                                                <div class="clear">
                                                </div>
                                                <a id="lnkTerms" runat="server" onclick="ShowContract();" href="#">
                                                    <%=GetLocalResourceObject("ContractTerms").ToString() %>
                                                </a>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("RelatedInformation").ToString() + " :"%></h1>
                                <asp:ImageButton runat="server" ID="imbShowRelatedInformation" OnClientClick="javascript:return ShowHideRelatedInformation(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:ShowRelatedInformation %>" />
                                <asp:ImageButton runat="server" ID="imbHideRelatedInformation" OnClientClick="javascript:return ShowHideRelatedInformation();"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="<%$ resources:HideRelatedInformation %>" />
                                <asp:HiddenField ID="hdfIsRelatedInformationVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divRelatedInformation" style="display: none">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblInspection" runat="server" AssociatedControlID="ddlInspection"
                                                    Text="<%$ resources:Inspection %>"></asp:Label>
                                                <asp:DropDownList ID="ddlInspection" runat="server" CssClass="select-half" TabIndex="41">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfInspection" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlInspection"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Inspection %>" InitialValue="-1">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="lblPackingInstruction" runat="server" AssociatedControlID="txtPackingInstruction"
                                                    Text="<%$ resources:PackingInstruction %>">
                                                </asp:Label>
                                                <asp:TextBox ID="txtPackingInstruction" runat="server" MaxLength="100" CssClass="input-halfsmall-a" TabIndex="43"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblExportDoc" runat="server" AssociatedControlID="ddlExportDoc" Text="<%$ resources:ExportDoc %>"></asp:Label>
                                                <asp:DropDownList ID="ddlExportDoc" runat="server" CssClass="select-half" TabIndex="42">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfExportDoc" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="so" EnableClientScript="true" runat="server" ControlToValidate="ddlExportDoc"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ExportDoc %>" InitialValue="-1">
                                                </asp:RequiredFieldValidator>
                                                <asp:Label ID="lblOriginofGoods" runat="server" AssociatedControlID="ddlOriginofGoods"
                                                    Text="<%$ resources:OriginofGoods %>"></asp:Label>
                                                <asp:DropDownList ID="ddlOriginofGoods" runat="server" OnSelectedIndexChanged="ActionHandler"
                                                    CssClass="select-half" TabIndex="44">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-devide">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TextMode="MultiLine"
                                                    CssClass="multiline-m1col input-halfsmall-a" TabIndex="45"></asp:TextBox>
                                                <%--(BugId:3157) CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>--%>
                                                <asp:RegularExpressionValidator ID="vreRemarks" runat="server" ControlToValidate="txtRemarks"
                                                    ErrorMessage="<%$ Resources:Err_Remarks %>" ValidationExpression="^[\s\S]{0,500}$"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="so" runat="server" />
                    <asp:ValidationSummary ID="vsDetails" ValidationGroup="scDetails" runat="server" />
                </div>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="so" />
            </div>
            <div id="divAlert" style="display: none">
                <uc2:Alert ID="ucrAlert" runat="server" />
            </div>
            <div id="divTerms" class="max-425" style="display: none">
                <asp:Literal ID="ltrTerms" runat="server"></asp:Literal>
            </div>
            <asp:HiddenField ID="hdfTaxCategory" runat="server" />
            <div id="divItemTax" style="display: none">
                <div class="content-wrapper">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-P">
                                    <asp:HiddenField ID="hdfTaxFormula" runat="server" />
                                    <asp:Label ID="lblPopupItemAmount" runat="server" Text="Item Amount" AssociatedControlID="txtPopupItemAmount"></asp:Label>
                                    <asp:TextBox ID="txtPopupItemAmount" CssClass="input-w70 numeric" runat="server"
                                        EnableViewState="false" Enabled="false" MaxLength="11"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblPopupAmount" runat="server" Text="Amount" AssociatedControlID="txtPopupAmount"></asp:Label>
                                    <asp:TextBox ID="txtPopupAmount" runat="server" CssClass="input-w70 numeric" EnableViewState="false"
                                        Enabled="false" MaxLength="15"></asp:TextBox>
                                    <div class="starwrap">
                                        <asp:RequiredFieldValidator ID="vrfTaxAmt" CssClass="star" SetFocusOnError="true"
                                            ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupAmount"
                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                        </asp:RequiredFieldValidator>
                                        <cc1:AmountValidation ID="vamTaxAmt" runat="server" ControlToValidate="txtPopupAmount"
                                            ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="tax"></cc1:AmountValidation>
                                    </div>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-P">
                                    <asp:Label ID="lblPopupTaxType" runat="server" Text="Type" AssociatedControlID="ddlPopupTaxType"></asp:Label>
                                    <asp:DropDownList ID="ddlPopupTaxType" runat="server" CssClass="medium" EnableViewState="true"
                                        OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <asp:Label ID="lblPopupOther" runat="server" Text="Name" AssociatedControlID="txtPopupOther"></asp:Label>
                                    <asp:TextBox ID="txtPopupOther" runat="server" CssClass="medium" EnableViewState="false"
                                        MaxLength="100" Enabled="false"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="vrfPopupOther" CssClass="star" SetFocusOnError="true"
                                        ValidationGroup="tax" EnableClientScript="true" runat="server" ControlToValidate="txtPopupOther"
                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_TaxName %>">
                                    </asp:RequiredFieldValidator>
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
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdfTaxSplitPK" runat="server" Value='<%#Eval("SLT_PK") %>' />
                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("SLT_TAX") %>' />
                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("SLT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("SLT_TAX_TEXT")) %>'
                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("SLT_TAX_TEXT")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("SLT_TAX_TEXT"))) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="35%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxName" runat="server" Text='<%#Eval("SLT_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("SLT_NAME").ToString()) %>'></asp:Label>
                                        <asp:HiddenField ID="hdfTaxName" runat="server" Value='<%#Eval("SLT_NAME") %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="35%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTaxAmount" runat="server" Text='<%#GetFormattedCurrency(Eval("SLT_TAX_AMT")) %>'
                                            ToolTip='<%#GetFormattedCurrency(Eval("SLT_TAX_AMT")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="22%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
