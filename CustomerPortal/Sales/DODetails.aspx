<%@ Page Title="<%$ Resources:Captions,Title_DirectOrder %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="DODetails.aspx.cs" Inherits="CustomerPortal.Sales.DODetails"
    Theme="ClassicExt" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var NumberDigits = 0;
        var CurrencyDigits = 0;
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
            CurrencyDigits = parseInt($("[id$=hdfCurrencyDigits]").val());
        });
        function ShowListing(flag) {
            ///<summary>
            /// Used to handle the Listing And Enrty Section in Page
            ///</summary>
            /// <param name="flag" optional="true" type="String">
            /// flag Determines the Mode if flag then in Listing else in Edit Mode
            /// </param>           
            if (flag) {
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlEntry]").hide();
            }
            else {
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlEntry]").show();
            }
            InitComponents();
            return false;
        }
        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// </param>
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlSaveSubmit]").hide();
                $("[id$=btnAddItem]").hide();
                $("[id$=btnClearItem]").hide();
                $("[id$=pnlDelete]").hide();
                //$("[id$=pnlSubmit]").hide();
                $("[id$=divDetailActions]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                //$("[id$=pnlSubmit]").hide();
            }
        }
        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            //            if (flag) {
            //                $("[id$=tblDetails]").show();
            //                $("[id$=divDetails]").show();
            //                $("[id$=imbShowFilter]").hide();
            //                $("[id$=imbHideFilter]").show();
            //            }
            //            else {
            //                $("[id$=tblDetails]").hide();
            //                $("[id$=divDetails]").hide();
            //                $("[id$=imbShowFilter]").show();
            //                $("[id$=imbHideFilter]").hide();
            //            }
            return false;
        }

        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
            }
        }

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtEnqDate");
            GrandScriptUtils.AddDateRangeCommon("txtBookingDate", "hdfBookingDate", "txtReqByDate", "hdfReqByDate", false, false, true);
            //            GrandScriptUtils.AddDateRangeCommon("txtEnqDate", "hdfEnqDate", "txtReqByDate", "hdfReqByDate", false, false, true);
            //            GrandScriptUtils.DatePickerCommon("txtBookingDate");
            GrandScriptUtils.DatePickerCommon("txtRefDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomer", true, true, "CUSTOMER");
            if ($("[id$=txtCustomer]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", url + "?Type=" + $("[id$=hdfCustomer]").val(), "hdfBrand", true, true, "CUTOMERBRANDWITHSPEC");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            if ($("[id$=hdfCustomer]").val() == "" || $("[id$=hdfCustomer]").val() == "0") {
                DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
            }
            DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
        }
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=hdfBrand]").val("0");
                $("[id$=hdfProduct]").val("0");
                $("[id$=txtBrand]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfBrandCode]").val("");
                $("[id$=txtProduct]").val("");
                $("[id$=hdfProductName]").val("");
                $("[id$=hdfUOM]").val("");
                $("[id$=txtUOM]").val("");
                $("[id$=txtBoxPerCarton]").val("");
                $("[id$=txtPcsPerBox]").val("");
                if ($("[id$=hdfCustomer]").val() != "" && $("[id$=hdfCustomer]").val() != "0") {
                    EnableAuto($("[id$=txtBrand]"));
                    GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", url + "?Type=" + $("[id$=hdfCustomer]").val(), "hdfBrand", true, true, "CUTOMERBRANDWITHSPEC");
                    //txtCustAddress,hdfCustAddress
                }
                else {
                    DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
                }
                $("[id$=btnCustSelected]").click();
            }
            else if (targetControlID == "txtBrand") {
                if ($("[id$=hdfBrand]").val() != "" && $("[id$=hdfBrand]").val() != "0") {
                    $("[id$=hdfProduct]").val("0");
                    $("[id$=txtProduct]").val("");
                    $("[id$=hdfProductName]").val("");
                    $("[id$=btnSelectProduct]").click();
                }
                else {
                    $("[id$=hdfBrandCode]").val("");
                    $("[id$=hdfProduct]").val("0");
                    $("[id$=txtProduct]").val("");
                    $("[id$=hdfProductName]").val("");
                    $("[id$=hdfUOM]").val("");
                    $("[id$=txtUOM]").val("");
                    $("[id$=txtBoxPerCarton]").val("");
                    $("[id$=txtPcsPerBox]").val("");
                }
            }
            else if (targetControlID == "txtCurrency") {
                $("[id$=btnCurrency]").click();
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=hdfBrand]").val("0");
                $("[id$=hdfProduct]").val("0");
                $("[id$=txtBrand]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfBrandCode]").val("");
                $("[id$=txtProduct]").val("");
                $("[id$=hdfProductName]").val("");
                DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
                $("[id$=hdfUOM]").val("");
                $("[id$=txtUOM]").val("");
                $("[id$=txtBoxPerCarton]").val("");
                $("[id$=txtPcsPerBox]").val("");
                $("[id$=btnCustSelected]").click();
            }
            else if (targetControlID == "txtBrand") {
                $("[id$=hdfBrandCode]").val("");
                $("[id$=hdfProduct]").val("0");
                $("[id$=txtProduct]").val("");
                $("[id$=hdfProductName]").val("");
                $("[id$=hdfUOM]").val("");
                $("[id$=txtUOM]").val("");
                $("[id$=txtBoxPerCarton]").val("");
                $("[id$=txtPcsPerBox]").val("");
            }
            else if (targetControlID == "txtCurrency") {
                $("[id$=txtCurrency]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfCurrency]").val("0");
                $("[id$=btnCurrency]").click();
            }
        }
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
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
        function AfterDateSelect(controlID) {
            if (controlID == "txtBookingDate") {
                var bookingDate = $.datepicker.parseDate("dd-M-yy", $('input[id$=txtBookingDate]').val());
                bookingDate.setMonth(bookingDate.getMonth() + 1);
                bookingDate.setDate(0);
                var expectedDateText = $.datepicker.formatDate("dd-M-yy", bookingDate);
                var expectedDate = $.datepicker.formatDate("dd-M-yy", bookingDate);
                $("[id$=txtReqByDate]").val(expectedDateText);
                $("[id$=hdfReqByDate]").val(expectedDate);
                $("[id$=btnBookingDate]").click();

            }
        }
        function CalculateAmount() {
            var qty = 0;
            var rate = 0;
            var brandQty = 0;
            var UOMConv = 0;
            qty = parseFloat($("[id$=txtQty]").val());
            rate = parseFloat($("[id$=txtRate]").val());
            brandQty = parseFloat($("[id$=txtBrandQuantity]").val());
            UOMConv = parseFloat($("[id$=hdfBrandUOMConvFactor]").val());
            if (!isNaN(brandQty)) {
                qty = UOMConv * brandQty;
                $("[id$=txtQty]").val(qty.toFixed(NumberDigits));
            }
            else {
                qty = 0;
                $("[id$=txtQty]").val(qty.toFixed(NumberDigits));
            }
            if (!isNaN(brandQty) && !isNaN(rate)) {
                var amount = brandQty * rate;
                $("[id$=txtAmount]").val(amount.toFixed(CurrencyDigits));
            }
            else
                $("[id$=txtAmount]").val(parseFloat(0).toFixed(CurrencyDigits));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlEnquiry" runat="server">
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
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="27" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiry')"
                                            ValidationGroup="enquiry" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="28"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiry')"
                                            ValidationGroup="enquiry" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="29" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiry')"
                                            ValidationGroup="enquiry" ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="30" Text="<%$resources:ErpRes,Delete %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Delete" />
                                    </li>
                                    <%--innerbutton--%>
                                    <%--<li runat="server" id="pnlInnerSave">
                                        <asp:Button runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="24" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiryDtl')"
                                            ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="SEC_ActionPanel" ValidationGroup="enquiryDtl"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlInnerClear">
                                        <asp:Button runat="server" ID="btnClearItem" CommandName="CLEARITEM" TabIndex="24"
                                            Text="<%$resources:Controls,Clear %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" />
                                    </li>--%>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:ErpRes,Cancel %>" OnClick="ActionHandler"
                                            CommandName="CANCEL" TabIndex="31" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:ErpRes,Cancel %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="Span1" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnList" TabIndex="32" CommandName="ENQUIRYLIST"
                                Text="<%$resources:PageNameRes,DirectOrderListing %>" OnClick="ActionHandler"
                                CommandArgument="SEC_ActionPanel" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="Span2" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnQtnList" TabIndex="33" CommandName="QUOTATIONLIST"
                                Text="<%$resources:PageNameRes,OrderAcceptListing %>" OnClick="ActionHandler"
                                CommandArgument="SEC_ActionPanel" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnEnquiry" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnEnquiry" Text="<%$resources:PageNameRes,DirectOrder %>"
                                TabIndex="34" CommandName="ENQUIRY" OnClick="ActionHandler" CssClass="tab-active"
                                CommandArgument="SEC_ActionPanel" OnClientClick="javascript:return false;"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnQuotation" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnQuotation" Text="<%$resources:PageNameRes,DirectOrderAccept %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="35" CommandName="QUOTE" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <%-- <asp:HiddenField id="hdfSubTab" runat="server" Value="Hdr"/>--%>
                <%--<div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkInfo" Text="Info"
                                TabIndex="33" OnClientClick="javascript:return HideDtl();" 
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkItemDetails" Text="Item Details"
                                TabIndex="33" OnClientClick="javascript:return HideHdr();" 
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>--%>
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                            <div class="contentwrapper">
                                <asp:HiddenField ID="hdfStatus" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
                                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" Value="3" />
                                <asp:HiddenField ID="hdfRateFormat" runat="server" />
                                <div id="divMainTab">
                                    <%--<div class="tab-container-grp">
                                        <ul id="Ul1">
                                            <li><span id="Span2" runat="server" class="tab-active">
                                                <asp:LinkButton runat="server" ID="LinkButton1" TabIndex="26" Text="Info"
                                                    OnClientClick="HideDtl();" CssClass="tab-active"></asp:LinkButton>
                                            </span></li>
                                        </ul>
                                    </div>--%>
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblCustomer" runat="server" AssociatedControlID="txtCustomer" Text="<%$ resources:Customer_Mand %>"><%--<%$ resources:Customer %>--%>
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtCustomer" runat="server" TabIndex="1" MaxLength="100" CssClass="input-half"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfCustomer" runat="server" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="enquiry" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            runat="server" ControlToValidate="txtCustomer" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Customer %>"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <asp:Button ID="btnCustSelected" runat="server" OnClick="ActionHandler" CommandName="CUSTOMERSELECTED"
                                                        EnableTheming="false" Style="display: none" />
                                                    <asp:Label ID="lblRefNo" runat="server" AssociatedControlID="txtRefNo" Text="<%$ resources:RefNo %>" CssClass="input-small">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtRefNo" runat="server" TabIndex="3" MaxLength="100" CssClass="input-small"></asp:TextBox>
                                                    <asp:Label runat="server" ID="lblRefDate" Text="<%$ resources:RefDate %>" AssociatedControlID="txtRefDate"
                                                        CssClass="middle-lbl-c"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtRefDate" TabIndex="4" MaxLength="12" CssClass="input-small"
                                                        onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblToPort" runat="server" AssociatedControlID="txtToPort" Text="<%$ resources:ToPort %>">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtToPort" runat="server" TabIndex="6" MaxLength="100" CssClass="input-half"></asp:TextBox>
                                                
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblEnqTrxNo" Text="<%$ resources:EnqNo %>" AssociatedControlID="lblEnqTrxNoTxt"></asp:Label>
                                                    <asp:Label runat="server" ID="lblEnqTrxNoTxt" CssClass="input-small"></asp:Label>
                                                    <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                                    <asp:Label runat="server" ID="lblEnqDate" Text="<%$ resources:EnqDate %>" AssociatedControlID="txtEnqDate"
                                                        CssClass="middle-lbl-small-d"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtEnqDate" TabIndex="2" MaxLength="12" CssClass="input-small"
                                                        onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfEnqDate" runat="server" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfEnqDate" CssClass="star" SetFocusOnError="false"
                                                            ValidationGroup="enquiryDtl" EnableClientScript="true" runat="server" ControlToValidate="txtEnqDate"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EnqDate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreEnqDate" CssClass="star" ValidationGroup="enquiryDtl"
                                                            runat="server" ControlToValidate="txtEnqDate" SetFocusOnError="false" ErrorMessage="<%$ resources:Err_EnqDate_Valid %>"
                                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                            EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrency"></asp:Label>
                                                    <asp:TextBox ID="txtCurrency" runat="server" CssClass="input-small" TabIndex="9" MaxLength="100"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfCurrency" runat="server" />
                                                    <asp:Button ID="btnCurrency" runat="server" OnClick="ActionHandler" CommandName="EXCHANGERATE"
                                                        EnableTheming="false" Style="display: none" />
                                                    <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                                    <asp:HiddenField ID="hdfVersion" runat="server" Value="0" />
                                                    <asp:Label ID="lblTranshipment" runat="server" AssociatedControlID="ddlTranshipment"
                                                        CssClass="middle-lbl-small-d" Text="<%$ resources:Transhipment %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlTranshipment" runat="server" TabIndex="5" CssClass="select-small-a1">
                                                    </asp:DropDownList>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblShipBy" runat="server" AssociatedControlID="ddlShipBy" Text="<%$ resources:ShipBy %>"   >
                                                    </asp:Label>
                                                    <asp:DropDownList ID="ddlShipBy" runat="server" TabIndex="7" CssClass="select-small-a1">
                                                    </asp:DropDownList>
                                                    <asp:Label runat="server" ID="lblBookingDate" Text="<%$ resources:BookingDate_Mand %>" CssClass="middle-lbl-small-d"
                                                        AssociatedControlID="txtBookingDate"></asp:Label><%--<%$ resources:ReqByDate %>--%>
                                                    <asp:TextBox runat="server" ID="txtBookingDate" TabIndex="7" MaxLength="12" CssClass="input-small"
                                                        onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfBookingDate" runat="server" />
                                                    <asp:Button ID="btnBookingDate" runat="server" EnableTheming="false" OnClick="ActionHandler"
                                                        CommandName="BOOKINGDATECHANGE" Style="display: none" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfBookingDate" CssClass="star" SetFocusOnError="false"
                                                            ValidationGroup="enquiryDtl" EnableClientScript="true" runat="server" ControlToValidate="txtBookingDate"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BookingDate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreBookingDate" CssClass="star" ValidationGroup="enquiryDtl"
                                                            runat="server" ControlToValidate="txtBookingDate" SetFocusOnError="false" ErrorMessage="<%$ resources:Err_BookingDate_Valid %>"
                                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                            EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--<div class="search-colapse">
                                    <table>
                                        <tr>
                                            <td>
                                                <h1>
                                                    <%= GetLocalResourceObject("EnquiryDetails").ToString()%></h1>
                                            </td>
                                            <td>
                                                <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                    ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Enquiry Details"
                                                    TabIndex="65" />
                                                <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                    ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Enquiry Details"
                                                    TabIndex="66" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>--%>
                                </div>
                                <div id="divSubTab">
                                    <div class="fields-grpwrap color-grey grp-before color-white pad-t10">
                                        <div class="header">
                                            <h1>
                                                <%= GetLocalResourceObject("EnquiryDetails").ToString() %></h1>
                                            <%--<div class="button-wrap-right" id="divDetailActions">
                                                <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="10"
                                                    OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiryDtl')"
                                                    ValidationGroup="enquiryDtl" ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="SEC_ActionPanel"
                                                    SkinID="save" />
                                                <asp:ImageButton ID="btnClearItem" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                    TabIndex="11" OnClick="ActionHandler" CommandName="CLEARITEM" SkinID="btnrefresh" />
                                            </div>--%>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <div class="fields-group ">
                                            <table class="table-devide" id="tblDetails">
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="divcol-S">
                                                            <asp:Label ID="lblBrand" runat="server" AssociatedControlID="txtBrand" Text="<%$ resources:Brand_Mand %>"><%--<%$ resources:Brand %>--%>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtBrand" runat="server" TabIndex="10" MaxLength="200" CssClass="input-full"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfBrand" runat="server" />
                                                            <asp:HiddenField ID="hdfBrandCode" runat="server" />
                                                            <asp:Button ID="btnSelectProduct" runat="server" OnClick="ActionHandler" CommandName="PRODUCTSELECTED"
                                                                EnableTheming="false" Style="display: none" />
                                                            <asp:RequiredFieldValidator ID="vrfBrand" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="enquiryDtl" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                                runat="server" ControlToValidate="txtBrand" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Brand %>"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="divcol-S">
                                                            <asp:Label ID="lblProduct" runat="server" AssociatedControlID="txtProduct" Text="<%$ resources:Product %>">
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtProduct" runat="server" MaxLength="100" Enabled="false" CssClass="input-disabled input-full"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfProduct" runat="server" />
                                                            <asp:HiddenField ID="hdfProductName" runat="server" />
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label runat="server" ID="lblPacking" Text="<%$ resources:Packing %>" AssociatedControlID="txtPacking"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtPacking" CssClass="input-disabled input-half" onkeydown="return EnableArrowKey(event)"
                                                                onpaste="return false;"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfPackingSpec" runat="server" />
                                                            <asp:HiddenField ID="hdfPackingText" runat="server" />
                                                            <div class="clear">
                                                            </div>
                                                            <asp:Label ID="lblQty" runat="server" AssociatedControlID="txtBrandQuantity" Text="<%$ resources:Quantity_Mand %>"><%--<%$ resources:Quantity %>--%>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtBrandQuantity" runat="server" CssClass="input-small numeric" MaxLength="11"
                                                                TabIndex="13" onchange="CalculateAmount();"></asp:TextBox>
                                                            <%-- <span style="width: 5px; border: 0 none; background: none;">--%>
                                                            <div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfQuantity" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="enquiryDtl" EnableClientScript="true" runat="server" ControlToValidate="txtBrandQuantity"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:QuantityValidation ID="vreQuantity" runat="server" ControlToValidate="txtBrandQuantity"
                                                                    NonZero="true" NumberDigits="7" ErrorMessage="<%$ resources:Err_Quantity_Valid %>"
                                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiryDtl">
                                                                </cc1:QuantityValidation>
                                                            </div>
                                                            <%-- </span>--%>
                                                            <asp:TextBox runat="server" ID="txtBrandUOM" CssClass="input-normal input-uom-small"
                                                                Enabled="false"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfBrandUOMPK" runat="server" />
                                                            <asp:HiddenField ID="hdfBrandUOMConvFactor" runat="server" Value="1" />
                                                            <asp:TextBox ID="txtQty" runat="server" CssClass="input-small input-disabled numeric"
                                                                MaxLength="11"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txtUOM" CssClass="input-normal Uiinput-uom" Enabled="false"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfUOM" runat="server" />
                                                            <div class="clear">
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label runat="server" ID="lblTotalPiecesCtn" Text="<%$ resources:TotalPiecesCtn %>"
                                                                AssociatedControlID="txtTotalPiecesCtn"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtTotalPiecesCtn" CssClass="input-small input-disabled numeric"
                                                                Enabled="false"></asp:TextBox>
                                                            <asp:Label runat="server" ID="lblReqByDate" Text="<%$ resources:ReqdDate_Mand %>"
                                                                AssociatedControlID="txtReqByDate" CssClass="middle-lbl-small-d"></asp:Label><%--<%$ resources:ReqByDate %>--%>
                                                            <asp:TextBox runat="server" ID="txtReqByDate" TabIndex="12" MaxLength="12" CssClass="input-small"
                                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                            <asp:HiddenField runat="server" ID="hdfReqByDate" />
                                                            <div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfReqByDate" CssClass="star" SetFocusOnError="false"
                                                                    ValidationGroup="enquiryDtl" EnableClientScript="true" runat="server" ControlToValidate="txtReqByDate"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqByDate %>">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="vreReqByDate" CssClass="star" ValidationGroup="enquiryDtl"
                                                                    runat="server" ControlToValidate="txtReqByDate" SetFocusOnError="false" ErrorMessage="<%$ resources:Err_ReqByDate_Valid %>"
                                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                                    EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                            </div>
                                                            <div class="clear">
                                                            </div>
                                                            <asp:Label ID="lblRate" runat="server" AssociatedControlID="txtRate" Text="<%$ resources:Rate_Mand %>"><%--<%$ resources:Quantity %>--%>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtRate" runat="server" CssClass="input-small input-disabled numeric"
                                                                MaxLength="16" Enabled="false"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txtBrandRateUOM" CssClass="input-normal middle-lbl-xsmall-c2" Enabled="false"></asp:TextBox>
                                                            <%--<div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="enquiryDtl"
                                                                    EnableClientScript="true" runat="server" ControlToValidate="txtRate" Display="Dynamic"
                                                                    Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:RateValidation ID="vreRate" runat="server" ControlToValidate="txtRate" ErrorMessage="<%$ resources:Err_Rate_Valid %>"
                                                                    NumberDigits="10" Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star"
                                                                    ValidationGroup="enquiryDtl" ></cc1:RateValidation>
                                                            </div>--%>
                                                            <asp:Label ID="lblAmount" runat="server" AssociatedControlID="txtAmount" Text="<%$ resources:Amount_Mand %>" CssClass="middle-lbl-xsmall-c2"><%--<%$ resources:Quantity %>--%>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="input-small input-disabled numeric"
                                                                MaxLength="15" onkeydown="return EnableArrowKey(event);" onpaste="return false;"></asp:TextBox>
                                                            <div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfAmount" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="enquiryDtl" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:AmountValidation ID="vamAmount" runat="server" ControlToValidate="txtAmount"
                                                                    ErrorMessage="<%$ resources:Err_Amount_Valid %>" NumberDigits="11" Display="Dynamic"
                                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiryDtl"></cc1:AmountValidation>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="divcol-S">
                                                            <asp:Label runat="server" ID="lblDtlRemark" Text="<%$ resources:Remarks %>" AssociatedControlID="txtDtlRemark"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtDtlRemark" TabIndex="13" MaxLength="480" CssClass="input-full"></asp:TextBox>
                                                            <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="14"
                                                                OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiryDtl')"
                                                                ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="SEC_ActionPanel" ValidationGroup="enquiryDtl"
                                                                SkinID="plus" CssClass="margntop2" />
                                                            <asp:ImageButton runat="server" ID="btnClearItem" CommandName="CLEARITEM" TabIndex="15"
                                                                OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>" CommandArgument="SEC_ActionPanel"
                                                                SkinID="cancel" CssClass="margntop2"/>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="button-fieldsgrp">
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <div id="divDetails" runat="server">
                                                <div class="gridwrap">
                                                    <asp:GridView runat="server" ID="grdEnquiryDetails" Width="100%" AllowSorting="True"
                                                        OnRowDataBound="ActionHandler" OnSorting="ActionHandler" AutoGenerateColumns="false"
                                                        ShowFooter="true" EmptyDataRowStyle-CssClass="emptytable">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:Brand %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBrandLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CIM_BRAND_NAME"), 35) %>'
                                                                        ToolTip='<%# Eval("CIM_BRAND_CODE")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="22%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Product %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CIM_ITEM_TEXT"), 48) %>'
                                                                        ToolTip='<%# Eval("CIM_ITEM_TEXT")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="28%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:UoM %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUoMLst" runat="server" Text='<%# Eval("CED_SALE_UOM_TEXT") %>'
                                                                        ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CED_SALE_UOM_TEXT"))) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Quantity %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBrandQuantityLst" runat="server" Text='<%# Eval("CED_SALE_QTY", "{0:N}") %>'
                                                                        ToolTip='<%# Eval("CED_SALE_QTY", "{0:N}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="7%" CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:ItemQty %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuantityLst" runat="server" Text='<%# Eval("CED_ENQ_QTY", "{0:N}") %>'
                                                                        ToolTip='<%# Eval("CED_ENQ_QTY", "{0:N}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterStyle CssClass="amount-numeric" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblItemTotalQty" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="8%" CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:CartonsOrBags %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemCartonsOrBags" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="3%" CssClass="amount-numeric" />
                                                                <FooterStyle CssClass="amount-numeric" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblItemTotalCarton" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:ReqByDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReqByDateLst" runat="server" Text='<%# Eval("CED_REQUIRED_DATE", Resources.ErpRes.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval("CED_REQUIRED_DATE", Resources.ErpRes.DateFormatGrid)%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="7%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Remarks %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemarksLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("CED_REMARKS")), 9) %>'
                                                                        ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CED_REMARKS"))) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRateLst" runat="server" Text='<%# GetFormattedRate(Eval("CED_RATE")) %>'
                                                                        ToolTip='<%# GetFormattedRate(Eval("CED_RATE"))%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="5%" CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:amount %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmountLst" runat="server" Text='<%# Eval("CED_AMOUNT", "{0:c}") %>'
                                                                        ToolTip='<%# Eval("CED_AMOUNT", "{0:c}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterStyle CssClass="amount-numeric" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblItemTotalAmount" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="8%" CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                                        CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                                        SkinID="imbeditgrid" ToolTip="Edit" TabIndex="16" />
                                                                    <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                                        CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                                        SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="17" OnClientClick="return ShowDeleteConfirm(this);" />
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
                                <div id="divDetailsTab">
                                    <div class="fields-grpwrap color-grey grp-before pad-t10 color-white">
                                        <div class="header">
                                            <h1>
                                                <%= GetLocalResourceObject("TermsnCond").ToString() %></h1>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <div class="fields-group">
                                            <table class="table-devide">
                                                <tr>
                                                    <td>
                                                        <div class="div2col-L">
                                                            <asp:Label ID="lblDeliveryTerms" runat="server" AssociatedControlID="ddlDeliveryTerms" CssClass="w25perc"
                                                                Text="<%$ resources:DeliveryTerms %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlDeliveryTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" CssClass="select-half"
                                                                TabIndex="18">
                                                            </asp:DropDownList>
                                                            <div class="clear">
                                                            </div>
                                                            <asp:Label ID="Label3" runat="server" Text="" AssociatedControlID="txtDeliveryTerms" CssClass="w25perc"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtDeliveryTerms" MaxLength="500" TabIndex="19" TextMode="MultiLine"
                                                                CssClass="multiline-1col input-halfsmall-a" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="vreDeliveryTerms" runat="server" ControlToValidate="txtDeliveryTerms"
                                                                ErrorMessage="<%$ Resources:Err_DeliveryTerms %>" ValidationExpression="^[\s\S]{0,500}$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-L">
                                                            <asp:Label ID="lblPaymentTerms" runat="server" AssociatedControlID="ddlPaymentTerms" CssClass="w25perc"
                                                                Text="<%$ resources:PaymentTerms %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlPaymentTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" CssClass="select-half"
                                                                TabIndex="20">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="Label2" runat="server" Text="" AssociatedControlID="txtPaymentTerms" CssClass="w25perc"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtPaymentTerms" MaxLength="500" TabIndex="21" TextMode="MultiLine"
                                                                CssClass="multiline-1col input-halfsmall-a" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="vrePaymentTerms" runat="server" ControlToValidate="txtPaymentTerms"
                                                                ErrorMessage="<%$ Resources:Err_PaymentTerms %>" ValidationExpression="^[\s\S]{0,500}$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="div2col-L">
                                                            <asp:Label ID="lblSpecialCause" runat="server" AssociatedControlID="ddlSpecialCause" CssClass="w25perc"
                                                                Text="<%$ resources:SpecialCause %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlSpecialCause" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" CssClass="select-half"
                                                                TabIndex="22">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="Label4" runat="server" Text="" AssociatedControlID="txtSpecialCause" CssClass="w25perc"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtSpecialCause" MaxLength="500" TabIndex="23" TextMode="MultiLine"
                                                                CssClass="multiline-1col input-halfsmall-a" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="vreSpecialCause" runat="server" ControlToValidate="txtSpecialCause"
                                                                ErrorMessage="<%$ Resources:Err_SpecialCause %>" ValidationExpression="^[\s\S]{0,500}$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-L">
                                                            <asp:Label runat="server" ID="lblShippingAddress" Text="<%$ resources:ShippingAddress %>" CssClass="w25perc"
                                                                AssociatedControlID="ddlCustAddress"></asp:Label>
                                                            <asp:DropDownList ID="ddlCustAddress" runat="server" TabIndex="24" AutoPostBack="true" CssClass="select-half"
                                                                OnSelectedIndexChanged="ActionHandler">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="Label1" runat="server" AssociatedControlID="txtShippingAddress" CssClass="w25perc"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtShippingAddress" MaxLength="500" TabIndex="25"
                                                                TextMode="MultiLine" EnableTheming="false" CssClass="multiline-1col input-halfsmall-a" onkeydown="limitText(this,500);"
                                                                onkeyup="limitText(this,500);"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="vreShippingAddress" runat="server" ControlToValidate="txtShippingAddress"
                                                                ErrorMessage="<%$ Resources:Err_ShippingAddress %>" ValidationExpression="^[\s\S]{0,500}$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="divcol-S">
                                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TabIndex="26" TextMode="MultiLine"
                                                                CssClass="multiline-1col input-halfsmall-a" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="vreRemarks" runat="server" ControlToValidate="txtRemarks"
                                                                ErrorMessage="<%$ Resources:Err_Remarks %>" ValidationExpression="^[\s\S]{0,500}$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="enquiry" runat="server" />
                    <asp:ValidationSummary ID="vsDtl" ValidationGroup="enquiryDtl" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                </div>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="enquiry" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
