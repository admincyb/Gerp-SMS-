<%@ Page Title="<%$ Resources:Captions,Title_EnquiryDetails %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="EnquiryDetails.aspx.cs" Inherits="ERPSMS_v01.Sales.EnquiryDetails"
    Theme="ClassicExt" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        var NumberDigits = 0;

        $(document).ready(function () {
            NumberDigits = parseInt($("[id$=hdfNumberDigits]").val());
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
                $("[id$=divDetailActions]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
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
        function InitComponents() {
            if ($('[id$="txtEnqDate"]').length > 0) {
                GrandScriptUtils.AddDateRangeCommon("txtEnqDate", "hdfEnqDate", "txtReqByDate", "hdfReqByDate", false, false, true);
            }
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomer", true, true, "CUSTOMER");
            if ($("[id$=txtCustomer]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", url + "?Type=" + $("[id$=hdfCustomer]").val(), "hdfBrand", true, true, "CUTOMERBRANDWITHSPEC");
            GrandScriptUtils.MakeAutoCompleteDDL("txtProduct", url + "?Type=" + $("[id$=hdfCustomer]").val(), "hdfProduct", true, true, "CUTOMERPRODUCT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCurrency", url, "hdfCurrency", true, true, "CURRENCY");
            if ($("[id$=hdfCustomer]").val() == "" || $("[id$=hdfCustomer]").val() == "0") {
                DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
            }
            DisableAuto($("[id$=txtProduct]"), $("[id$=hdfProduct]"));
            $("[id$=txtExpMinRate]").ForceNumericOnly();
            $("[id$=txtExpMaxRate]").ForceNumericOnly();
            DisableAuto($("[id$=txtCurrency]"), $("[id$=hdfCurrency]"));
        }
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=hdfBrand]").val("0");
                $("[id$=hdfProduct]").val("0");
                $("[id$=txtBrand]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=txtProduct]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfUOM]").val("");
                $("[id$=txtUOM]").val("");
                $("[id$=txtPiecesPerCarton]").val("");
                $("[id$=txtPacking]").val("");
                if ($("[id$=hdfCustomer]").val() != "" && $("[id$=hdfCustomer]").val() != "0") {
                    EnableAuto($("[id$=txtBrand]"));
                    GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", url + "?Type=" + $("[id$=hdfCustomer]").val(), "hdfBrand", true, true, "CUTOMERBRANDWITHSPEC");
                    //EnableAuto($("[id$=txtProduct]"));
                    GrandScriptUtils.MakeAutoCompleteDDL("txtProduct", url + "?Type=" + $("[id$=hdfCustomer]").val(), "hdfProduct", true, true, "CUTOMERPRODUCT");
                    //txtCustAddress,hdfCustAddress
                }
                else {
                    DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
                }
                DisableAuto($("[id$=txtProduct]"), $("[id$=hdfProduct]"));
                $("[id$=btnCustSelected]").click();
            }
            else if (targetControlID == "txtBrand") {
                if ($("[id$=hdfBrand]").val() != "" && $("[id$=hdfBrand]").val() != "0") {
                    $("[id$=hdfProduct]").val("0");
                    $("[id$=txtProduct]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                    $("[id$=btnSelectProduct]").click();
                }
                else {
                    $("[id$=hdfProduct]").val("0");
                    $("[id$=txtProduct]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                    $("[id$=hdfUOM]").val("");
                    $("[id$=txtUOM]").val("");
                    $("[id$=txtPiecesPerCarton]").val("");
                    $("[id$=txtPacking]").val("");
                }
            }
            else if (targetControlID == "txtProduct") {
                if ($("[id$=hdfProduct]").val() != "" && $("[id$=hdfProduct]").val() != "0") {
                    $("[id$=hdfBrand]").val("0");
                    $("[id$=txtBrand]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                    $("[id$=btnSelectProduct]").click();
                }
                else {
                    $("[id$=hdfBrand]").val("0");
                    $("[id$=txtBrand]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                    $("[id$=hdfUOM]").val("");
                    $("[id$=txtUOM]").val("");
                    $("[id$=txtPiecesPerCarton]").val("");
                    $("[id$=txtPacking]").val("");
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
                $("[id$=txtProduct]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
                DisableAuto($("[id$=txtProduct]"), $("[id$=hdfProduct]"));
                $("[id$=hdfUOM]").val("");
                $("[id$=txtUOM]").val("");
                $("[id$=txtPiecesPerCarton]").val("");
                $("[id$=txtPacking]").val("");
                $("[id$=btnCustSelected]").click();
            }
            else if (targetControlID == "txtBrand") {
                $("[id$=hdfProduct]").val("0");
                $("[id$=txtProduct]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfUOM]").val("");
                $("[id$=txtUOM]").val("");
                $("[id$=txtPiecesPerCarton]").val("");
                $("[id$=txtPacking]").val("");
            }
            else if (targetControlID == "txtProduct") {
                $("[id$=hdfBrand]").val("0");
                $("[id$=txtBrand]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfUOM]").val("");
                $("[id$=txtUOM]").val("");
                $("[id$=txtPiecesPerCarton]").val("");
                $("[id$=txtPacking]").val("");
            }
            else if (targetControlID == "txtCurrency") {
                $("[id$=txtCurrency]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfCurrency]").val("0");
                $("[id$=btnCurrency]").click();
            }
        }
        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
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
        //        function HideDtl() {

        //            $("[id$=btnSave]").show();
        //            $("[id$=btnDelete]").show();
        //            $("[id$=btnAddItem]").hide();
        //            $("[id$=btnClearItem]").hide();


        //            $("[id$=divSubTab]").hide();
        //            $("[id$=divMainTab]").show();

        //            $("[id$=lnkInfo]").removeClass("tab-active");
        //            $("[id$=lnkInfo]").removeClass("tab-inactive");
        //            $("[id$=lnkInfo]").addClass("tab-active");

        //            $("[id$=lnkItemDetails]").removeClass("tab-active");
        //            $("[id$=lnkItemDetails]").removeClass("tab-inactive");
        //            $("[id$=lnkItemDetails]").addClass("tab-inactive");
        //            $("[id$=hdfSubTab]").val("Hdr");

        //            return false;

        //        }
        //        function HideHdr() {

        //            if ($("[id$=hdfCustomer]").val() != "" && $("[id$=hdfCustomer]").val() != "0") {

        //                $("[id$=btnSave]").hide();
        //                $("[id$=btnDelete]").hide();
        //                $("[id$=btnAddItem]").show();
        //                $("[id$=btnClearItem]").show();

        //                $("[id$=divMainTab]").hide();
        //                $("[id$=divSubTab]").show();

        //                $("[id$=lnkItemDetails]").removeClass("tab-active");
        //                $("[id$=lnkItemDetails]").removeClass("tab-inactive");
        //                $("[id$=lnkItemDetails]").addClass("tab-active");

        //                $("[id$=lnkInfo]").removeClass("tab-active");
        //                $("[id$=lnkInfo]").removeClass("tab-inactive");
        //                $("[id$=lnkInfo]").addClass("tab-inactive");
        //                $("[id$=hdfSubTab]").val("Dtl");
        //            }
        //            else {
        //                ShowErrorMessage('<ul><li><%=GetLocalResourceObject("Err_Customer").ToString() %></li></ul>', '<%=Resources.ErpRes.Information %>');
        //            }
        //            return false;

        //        }
        function ValidateRange(src, args) {
            var fromRate = parseFloat($("[id$=txtExpMinRate]").val());
            var toRate = parseFloat($("[id$=txtExpMaxRate]").val());
            if (isNaN(fromRate) || isNaN(toRate) || (fromRate <= toRate)) {
                args.IsValid = true;
            } else {
                args.IsValid = false;
            }
        }

        function QtyConvertion(sender) {
            var qty = 0;
            var brandQty = 0;
            var UOMConv = 0;
            qty = parseFloat($("[id$=txtQty]").val());
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
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="26" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiry')"
                                            ValidationGroup="enquiry" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="27"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiry')"
                                            ValidationGroup="enquiry" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="28" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiry')"
                                            ValidationGroup="enquiry" ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="29" Text="<%$resources:ErpRes,Delete %>"
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
                                            CommandName="CANCEL" TabIndex="30" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:ErpRes,Cancel %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="Span1" runat="server" class="list-inactive">
                            <asp:LinkButton runat="server" ID="lbnList" TabIndex="31" CommandName="ENQUIRYLIST"
                                CommandArgument="SEC_ActionPanel" ToolTip="<%$resources:PageNameRes,EnquiryListing %>"
                                OnClick="ActionHandler" CssClass="list-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnEnquiry" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnEnquiry" Text="<%$resources:PageNameRes,Enquiry %>"
                                ToolTip="<%$resources:PageNameRes,Enquiry %>" TabIndex="32" CommandName="ENQUIRY"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CssClass="tab-active"
                                OnClientClick="javascript:return false;"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnQuotation" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnQuotation" Text="<%$resources:PageNameRes,Quotation %>"
                                CommandArgument="SEC_ActionPanel" ToolTip="<%$resources:PageNameRes,Quotation %>"
                                TabIndex="33" CommandName="QUOTATION" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfNumberDigits" runat="server" Value="3" />
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
                                                    <asp:Label runat="server" ID="lblEnqTrxNo" Text="<%$ resources:EnqNo %>" AssociatedControlID="lblEnqTrxNoTxt"></asp:Label>
                                                    <asp:Label runat="server" ID="lblEnqTrxNoTxt" CssClass="input-small"></asp:Label>
                                                    <asp:Label runat="server" ID="lblEnqDate" Text="<%$ resources:EnqDate %>" AssociatedControlID="txtEnqDate"
                                                        CssClass="middle-lbl-small-d"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtEnqDate" TabIndex="1" MaxLength="12" CssClass="input-small hasDatepicker"
                                                        onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfEnqDate" runat="server" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfEnqDate" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="enquiry" EnableClientScript="true" runat="server" ControlToValidate="txtEnqDate"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EnqDate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreEnqDate" CssClass="star" ValidationGroup="enquiry"
                                                            runat="server" ControlToValidate="txtEnqDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_EnqDate_Valid %>"
                                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                            EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                        <asp:RequiredFieldValidator ID="vrfEnqDtlDate" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="enquiryDtl" EnableClientScript="true" runat="server" ControlToValidate="txtEnqDate"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EnqDate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreEnqDtlDate" CssClass="star" ValidationGroup="enquiryDtl"
                                                            runat="server" ControlToValidate="txtEnqDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_EnqDate_Valid %>"
                                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                            EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblCustomer" runat="server" AssociatedControlID="txtCustomer" Text="<%$ resources:Customer_Mand %>"><%--<%$ resources:Customer %>--%>
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtCustomer" runat="server" TabIndex="4" MaxLength="100" CssClass="select-half valid"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfCustomer" runat="server" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="enquiry" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            runat="server" ControlToValidate="txtCustomer" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Customer %>"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <asp:Button ID="btnCustSelected" runat="server" OnClick="ActionHandler" CommandName="CUSTOMERSELECTED"
                                                        EnableTheming="false" Style="display: none" />

                                                    
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblCurrency" Text="<%$ resources:Currency%>" AssociatedControlID="txtCurrency"></asp:Label>
                                                    <asp:TextBox ID="txtCurrency" runat="server" CssClass="select-half" TabIndex="6" MaxLength="100"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfCurrency" runat="server" />
                                                    <asp:RequiredFieldValidator ID="vrfCurrency" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="enquiry" EnableClientScript="true" runat="server" ControlToValidate="txtCurrency"
                                                        Display="Dynamic" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                        Text="*" ErrorMessage="<%$ resources:Err_Currency%>"></asp:RequiredFieldValidator>
                                                    <asp:RequiredFieldValidator ID="vrfCurrencyDtl" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="enquiryDtl" EnableClientScript="true" runat="server" ControlToValidate="txtCurrency"
                                                        Display="Dynamic" InitialValue="<%$ resources:Messages, AutoDefaultValue %>"
                                                        Text="*" ErrorMessage="<%$ resources:Err_Currency%>"></asp:RequiredFieldValidator>
                                                    <asp:Button ID="btnCurrency" runat="server" OnClick="ActionHandler" CommandName="EXCHANGERATE"
                                                        EnableTheming="false" Style="display: none" />
                                                    <asp:HiddenField ID="hdfExchangeRate" runat="server" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblShipBy" runat="server" AssociatedControlID="ddlShipBy" Text="<%$ resources:ShipBy %>">
                                                    </asp:Label>
                                                    <asp:DropDownList ID="ddlShipBy" runat="server" TabIndex="2"  CssClass="input-small">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblTranshipment" runat="server" AssociatedControlID="ddlTranshipment"
                                                        Text="<%$ resources:Transhipment %>" CssClass="middle-lbl-small-e"></asp:Label>
                                                    <asp:DropDownList ID="ddlTranshipment" runat="server" TabIndex="3" CssClass="select-small-b">
                                                    </asp:DropDownList>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblToPort" runat="server" AssociatedControlID="txtToPort" Text="<%$ resources:ToPort %>">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtToPort" runat="server" TabIndex="4" MaxLength="100" CssClass="input-half"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdfVersion" runat="server" Value="0" />
                                                    <asp:Label ID="lblPageDept" runat="server" Text="<%$ resources:Controls,Department %>"
                                                        AssociatedControlID="lblPageDeptText"></asp:Label>
                                                    <asp:Label ID="lblPageDeptText" runat="server" CssClass="select-half"></asp:Label>
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
                                                            <asp:TextBox ID="txtBrand" runat="server" TabIndex="7" MaxLength="200"></asp:TextBox>
                                                            <asp:HiddenField runat="server" ID="hdfBrandCode" />
                                                            <asp:HiddenField ID="hdfBrand" runat="server" />
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
                                                            <asp:Label ID="lblProduct" runat="server" AssociatedControlID="txtProduct" Text="<%$ resources:Product_Mand %>"><%--<%$ resources:Product %>--%>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtProduct" runat="server" MaxLength="200" CssClass="input-disabled"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfProduct" runat="server" />
                                                            <asp:RequiredFieldValidator ID="vrfProduct" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="enquiryDtl" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                                runat="server" ControlToValidate="txtProduct" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Product %>"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label runat="server" ID="lblPacking" Text="<%$ resources:Packing %>" AssociatedControlID="txtPacking"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtPacking" CssClass="select-half input-disabled" onkeydown="return EnableArrowKey(event)"
                                                                Enabled="false"></asp:TextBox>
                                                            <asp:HiddenField runat="server" ID="hdfPackingPK" />
                                                            <asp:HiddenField ID="hdfPackingSpec" runat="server" />
                                                            <asp:Label ID="lblQty" runat="server" AssociatedControlID="txtBrandQuantity" Text="<%$ resources:Quantity_Mand %>"><%--<%$ resources:Quantity %>--%>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtBrandQuantity" runat="server" CssClass="Uiinput-qty numeric input-small"
                                                                MaxLength="13" TabIndex="9" onblur="QtyConvertion(this);"></asp:TextBox>
                                                            <%-- <span style="width: 5px; border: 0 none; background: none;">--%>
                                                            <div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfQuantity" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="enquiryDtl" EnableClientScript="true" runat="server" ControlToValidate="txtBrandQuantity"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                                </asp:RequiredFieldValidator>
                                                                <cc1:QuantityValidation ID="vreQuantity" runat="server" ControlToValidate="txtBrandQuantity"
                                                                    NumberDigits="7" ErrorMessage="<%$ resources:Err_Quantity_Valid %>" Display="Dynamic"
                                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiryDtl"
                                                                    NonZero="true"></cc1:QuantityValidation>
                                                            </div>
                                                            <%-- </span>--%>
                                                            <asp:TextBox runat="server" ID="txtBrandUOM" CssClass="input-normal input-uom-small"
                                                                Enabled="false"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfBrandUOMPK" runat="server" />
                                                            <asp:HiddenField ID="hdfBrandUOMConvFactor" runat="server" Value="1" />
                                                             <label  class="middle-lbl-xsmall-c"></label>
                                                            <asp:TextBox ID="txtQty" runat="server" CssClass="Uiinput-qty input-disabled numeric input-small"
                                                                MaxLength="13" Enabled="false"></asp:TextBox>
                                                            <asp:TextBox runat="server" ID="txtUOM" CssClass="input-normal input-uom-small" Enabled="false"></asp:TextBox>
                                                            
                                                           
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label runat="server" ID="lblPiecesPerCarton" Text="<%$ resources:PiecesPerCarton %>"
                                                                AssociatedControlID="txtPiecesPerCarton"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtPiecesPerCarton" CssClass="Uiinput-qty input-disabled numeric input-small"
                                                                Enabled="false"></asp:TextBox>
                                                           <%-- <div class="clear">
                                                            </div>--%>
                                                            <asp:Label runat="server" ID="lblReqByDate" Text="<%$ resources:ReqdDate_Mand %>"
                                                                AssociatedControlID="txtReqByDate" CssClass="middle-lbl-small-d"></asp:Label><%--<%$ resources:ReqByDate %>--%>
                                                            <asp:TextBox runat="server" ID="txtReqByDate" TabIndex="8" MaxLength="12" CssClass="Uidate-picker input-small"
                                                                onkeydown="return CheckKey(event)" onpaste="return false;" AutoCompleteType="None"></asp:TextBox>
                                                            <asp:HiddenField runat="server" ID="hdfReqByDate" />
                                                            <div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfReqByDate" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="enquiryDtl" EnableClientScript="true" runat="server" ControlToValidate="txtReqByDate"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqByDate %>">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="vreReqByDate" CssClass="star" ValidationGroup="enquiryDtl"
                                                                    runat="server" ControlToValidate="txtReqByDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_ReqByDate_Valid %>"
                                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                                    EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                            </div>
                                                            <div class="clear">
                                                            </div>
                                                             <asp:HiddenField ID="hdfUOM" runat="server" />
                                                            <asp:Label runat="server" ID="lblExpMinRate" Text="<%$ resources:ExpMinRate %>" AssociatedControlID="txtExpMinRate"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtExpMinRate" CssClass="Uiinput-amount numeric input-small"
                                                                TabIndex="10" MaxLength="14"></asp:TextBox>
                                                            <cc1:RateValidation ID="vreExpMinRate" runat="server" ControlToValidate="txtExpMinRate"
                                                                ErrorMessage="<%$ resources:Err_ExpMinRate_Valid %>" NumberDigits="10" Display="Dynamic"
                                                                Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiryDtl"></cc1:RateValidation>
                                                            <asp:TextBox runat="server" ID="txtExpMinRateUOM" CssClass="input-normal input-small hide"
                                                                Enabled="false" ></asp:TextBox>

                                                            <asp:Label runat="server" ID="lblExpMaxRate" Text="<%$ resources:ExpMaxRate %>" AssociatedControlID="txtExpMaxRate" CssClass="middle-lbl-small-d line12"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtExpMaxRate" CssClass="Uiinput-amount numeric input-small"
                                                                TabIndex="11" MaxLength="14"></asp:TextBox>
                                                            <cc1:RateValidation ID="vreExpMaxRate" runat="server" ControlToValidate="txtExpMaxRate"
                                                                ErrorMessage="<%$ resources:Err_ExpMaxRate_Valid %>" NumberDigits="10" Display="Dynamic"
                                                                Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiryDtl"></cc1:RateValidation>
                                                            <asp:CustomValidator ID="vcsExpMaxRate" runat="server" ControlToValidate="txtExpMaxRate"
                                                                ErrorMessage="<%$ resources:Err_Rate_Range %>" ClientValidationFunction="ValidateRange"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiryDtl"></asp:CustomValidator>
                                                            <asp:TextBox runat="server" ID="txtExpMaxRateUOM" CssClass="input-normal small"
                                                                Enabled="false"></asp:TextBox>
                                                           
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="divcol-S">
                                                            <asp:Label runat="server" ID="lblDtlRemark" Text="<%$ resources:Remarks %>" AssociatedControlID="txtDtlRemark"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtDtlRemark" TabIndex="12" MaxLength="480"></asp:TextBox>
                                                            <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="13" CssClass="margntop2"
                                                                OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiryDtl')"
                                                                ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="PageAction_Entry" ValidationGroup="enquiryDtl"
                                                                SkinID="plus" />
                                                            <asp:ImageButton runat="server" ID="btnClearItem" CommandName="CLEARITEM" TabIndex="14" CssClass="margntop2"
                                                                OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>" CommandArgument="PageAction_Entry"
                                                                SkinID="cancel" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="button-fieldsgrp">
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <div id="divDetails" runat="server" >
                                                <div class="gridwrap grid-maxw1400">
                                                    <asp:GridView runat="server" ID="grdEnquiryDetails" AllowSorting="True"
                                                        ShowFooter="true" OnRowDataBound="ActionHandler" OnSorting="ActionHandler" AutoGenerateColumns="false"
                                                        EmptyDataRowStyle-CssClass="emptytable">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:Brand %>">
                                                                <ItemTemplate>
                                                                    <%--<asp:HiddenField ID="hdfEnqDtlPK" runat="server" Value='<%# Eval("") %>' />
                                                    <asp:HiddenField ID="hdfEnqItemPK" runat="server" Value='<%# Eval("") %>' />--%>
                                                                    <asp:Label ID="lblBrandLst" runat="server" Text='<%# Eval("CIM_BRAND_NAME")%>' ToolTip='<%# Eval("CIM_BRAND_NAME")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label runat="server" ID="lblfooterTot" Text="<%$ resources:Total %>"></asp:Label>
                                                                </FooterTemplate>
                                                                <ItemStyle Width="380px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Product %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductLst" runat="server" Text='<%# Eval("CIM_ITEM_TEXT")%>' ToolTip='<%# Eval("CIM_ITEM_TEXT")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="300px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:UoM %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUoMLst" runat="server" Text='<%# Eval("CED_SALE_UOM_TEXT") %>'
                                                                        ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CED_SALE_UOM_TEXT"))) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="30px" />
                                                                <%--CIM_UOM_TEXT--%>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Quantity %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <%--<asp:Label ID="lblBrandQuantityLst" runat="server" Text='<%# Eval("CED_SALE_QTY", "{0:N}") %>'
                                                                        ToolTip='<%# Eval("CED_SALE_QTY", "{0:N}")%>'></asp:Label>--%>
                                                                    <asp:Label ID="lblBrandQuantityLst" runat="server" Text='<%# Eval("CED_SALE_QTY", "{0:c}") %>'
                                                                        ToolTip='<%# Eval("CED_SALE_QTY", "{0:c}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="50px" CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:ItemQty %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <%--<asp:Label ID="lblQuantityLst" runat="server" Text='<%# Eval("CED_ENQ_QTY", "{0:N}") %>'
                                                                        ToolTip='<%# Eval("CED_ENQ_QTY", "{0:N}")%>'></asp:Label>--%>
                                                                    <asp:Label ID="lblQuantityLst" runat="server" Text='<%# Eval("CED_ENQ_QTY", "{0:c}") %>'
                                                                        ToolTip='<%# Eval("CED_ENQ_QTY", "{0:c}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="70px" CssClass="amount-numeric" />
                                                                <FooterStyle CssClass="amount-numeric" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblItemTotalQty" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:PriceRange %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPriceRange" runat="server" Text='<%# (Eval("CED_EXP_MIN_RATE") == null ? "NA" : GetFormattedRate(Eval("CED_EXP_MIN_RATE")))+" - "+(Eval("CED_EXP_MAX_RATE") == null ? "NA" : GetFormattedRate(Eval("CED_EXP_MAX_RATE")))%>'
                                                                        ToolTip='<%# (Eval("CED_EXP_MIN_RATE") == null ? "NA" : GetFormattedRate(Eval("CED_EXP_MIN_RATE")))+" - "+(Eval("CED_EXP_MAX_RATE") == null ? "NA" : GetFormattedRate(Eval("CED_EXP_MAX_RATE")))%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="80px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:CartonPerBags %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblItemCartonsOrBags" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="50px" CssClass="amount-numeric" />
                                                                <FooterStyle CssClass="amount-numeric" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblItemTotalCarton" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="<%$ resources:Packing %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPcsPerBoxLst" runat="server" Text='<%# Eval("APS_NAME") %>'
                                                                        ToolTip='<%# Eval("APS_NAME")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="7%" CssClass="amount-numeric" />
                                                            </asp:TemplateField>--%>
                                                            <%-- <asp:TemplateField HeaderText="<%$ resources:PiecesPerCarton1 %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBoxPerCartonLst" runat="server" Text='<%# Eval("APS_TOTAL_PCS") %>'
                                                                        ToolTip='<%# Eval("APS_TOTAL_PCS")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="7%" CssClass="amount-numeric" />
                                                            </asp:TemplateField>--%>
                                                            <%--  CBM & Weight--%>
                                                            <asp:TemplateField HeaderText="<%$ resources:CBM %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCBM" runat="server" Text='<%# Eval("CBM", "{0:N4}") %>' ToolTip='<%# Eval("CBM", "{0:N4}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="50px" CssClass="amount-numeric" />
                                                                <FooterStyle CssClass="amount-numeric" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalCBM" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Weight %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblWeight" runat="server" Text='<%# Eval("NET_WT", "{0:N3}") %>' ToolTip='<%# Eval("NET_WT", "{0:N3}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="50px" CssClass="amount-numeric" />
                                                                <FooterStyle CssClass="amount-numeric" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalWeight" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <%-- End CBM & Weight--%>
                                                            <asp:TemplateField HeaderText="<%$ resources:ReqByDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReqByDateLst" runat="server" Text='<%# Eval("CED_REQUIRED_DATE", Resources.ErpRes.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval("CED_REQUIRED_DATE", Resources.ErpRes.DateFormatGrid)%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="80px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Remarks %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemarksLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("CED_REMARKS")), 10) %>'
                                                                        ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CED_REMARKS"))) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="100px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                                        CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                                        SkinID="imbeditgrid" ToolTip="Edit" TabIndex="15" />
                                                                    <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                                        CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                                        OnClientClick="return ShowDeleteConfirm(this);" SkinID="imbdeletegrid" ToolTip="Delete"
                                                                        TabIndex="16" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="50px" Wrap="false" />
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
                                                        <div class="div2col-S">
                                                            <asp:Label ID="lblDeliveryTerms" runat="server" AssociatedControlID="ddlDeliveryTerms"
                                                                Text="<%$ resources:DeliveryTerms %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlDeliveryTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                                TabIndex="17" CssClass="select-half-a">
                                                            </asp:DropDownList>
                                                            <div class="clear">
                                                            </div>
                                                            <asp:Label ID="Label3" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtDeliveryTerms" MaxLength="500" TabIndex="19" TextMode="MultiLine"
                                                                CssClass="multiline-1col select-half" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="vreDeliveryTerms" runat="server" ControlToValidate="txtDeliveryTerms"
                                                                ErrorMessage="<%$ Resources:Err_DeliveryTerms %>" ValidationExpression="^[\s\S]{0,500}$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label ID="lblPaymentTerms" runat="server" AssociatedControlID="ddlPaymentTerms"
                                                                Text="<%$ resources:PaymentTerms %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlPaymentTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                                TabIndex="18" CssClass="select-half-a">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="Label2" runat="server" Text="" AssociatedControlID="txtPaymentTerms"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtPaymentTerms" MaxLength="500" TabIndex="20" TextMode="MultiLine"
                                                                CssClass="multiline-1col select-half" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="vrePaymentTerms" runat="server" ControlToValidate="txtPaymentTerms"
                                                                ErrorMessage="<%$ Resources:Err_PaymentTerms %>" ValidationExpression="^[\s\S]{0,500}$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label ID="lblSpecialCause" runat="server" AssociatedControlID="ddlSpecialCause"
                                                                Text="<%$ resources:SpecialCause %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlSpecialCause" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                                TabIndex="21" CssClass="select-half-a">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="Label4" runat="server" Text="" AssociatedControlID="txtSpecialCause"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtSpecialCause" MaxLength="500" TabIndex="23" TextMode="MultiLine"
                                                                CssClass="multiline-1col select-half" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="vreSpecialCause" runat="server" ControlToValidate="txtSpecialCause"
                                                                ErrorMessage="<%$ Resources:Err_SpecialCause %>" ValidationExpression="^[\s\S]{0,500}$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label runat="server" ID="lblShippingAddress" Text="<%$ resources:ShippingAddress %>"
                                                                AssociatedControlID="ddlCustAddress"></asp:Label>
                                                            <asp:DropDownList ID="ddlCustAddress" runat="server" TabIndex="22" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ActionHandler" CssClass="select-half-a">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="Label1" runat="server" AssociatedControlID="txtShippingAddress"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtShippingAddress" MaxLength="500" TabIndex="24"
                                                                TextMode="MultiLine" EnableTheming="false" CssClass="multiline-1col select-half" onkeydown="limitText(this,500);"
                                                                onkeyup="limitText(this,500);"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="vreShippingAddress" runat="server" ControlToValidate="txtShippingAddress"
                                                                ErrorMessage="<%$ Resources:Err_ShippingAddress %>" ValidationExpression="^[\s\S]{0,500}$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <%--<asp:Label ID="lblOriginofGoods" runat="server" AssociatedControlID="ddlOriginofGoods"
                                                    Text="<%$ resources:OriginofGoods %>"></asp:Label>
                                                <asp:DropDownList ID="ddlOriginofGoods" runat="server" OnSelectedIndexChanged="ActionHandler">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblBankDetails" runat="server" AssociatedControlID="ddlBankDetails"
                                                    Text="<%$ resources:BankDetails %>"></asp:Label>
                                                <asp:DropDownList ID="ddlBankDetails" runat="server" OnSelectedIndexChanged="ActionHandler">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblFromPort" runat="server" AssociatedControlID="ddlFromPort" Text="<%$ resources:FromPort %>"></asp:Label>
                                                <asp:DropDownList ID="ddlFromPort" runat="server" OnSelectedIndexChanged="ActionHandler">
                                                </asp:DropDownList>--%>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="divcol-S">
                                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TabIndex="25" TextMode="MultiLine"
                                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
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
