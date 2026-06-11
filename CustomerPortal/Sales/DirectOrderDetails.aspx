<%@ Page Title="<%$ Resources:Captions,Title_DeirectOrderDetails %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="DirectOrderDetails.aspx.cs" Inherits="CustomerPortal.Sales.DirectOrderDetails" Theme="Classic" %>

<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

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
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtEnqDate", "hdfEnqDate", "txtReqByDate", "hdfReqByDate", false, false, true);
            GrandScriptUtils.DatePickerCommon("txtRefDate");
            GrandScriptUtils.DatePickerCommon("txtBookingDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomer", true, true, "CUSTOMER");
            if ($("[id$=txtCustomer]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrand", url + "?Type=" + $("[id$=hdfCustomer]").val(), "hdfBrand", true, true, "CUTOMERBRANDWITHSPEC");
            GrandScriptUtils.MakeAutoCompleteDDL("txtProduct", url + "?Type=" + $("[id$=hdfCustomer]").val(), "hdfProduct", true, true, "CUTOMERPRODUCT");
            if ($("[id$=hdfCustomer]").val() == "" || $("[id$=hdfCustomer]").val() == "0") {
                DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
            }
            DisableAuto($("[id$=txtProduct]"), $("[id$=hdfProduct]"));
        }
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=hdfBrand]").val("0");
                $("[id$=hdfProduct]").val("0");
                $("[id$=txtBrand]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=txtProduct]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfUOM]").val("");
                $("[id$=txtUOM]").val("");
                $("[id$=txtBoxPerCarton]").val("");
                $("[id$=txtPcsPerBox]").val("");
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
                    $("[id$=txtBoxPerCarton]").val("");
                    $("[id$=txtPcsPerBox]").val("");
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
                    $("[id$=txtBoxPerCarton]").val("");
                    $("[id$=txtPcsPerBox]").val("");
                }
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
                $("[id$=txtBoxPerCarton]").val("");
                $("[id$=txtPcsPerBox]").val("");
                $("[id$=btnCustSelected]").click();
            }
            else if (targetControlID == "txtBrand") {
                $("[id$=hdfProduct]").val("0");
                $("[id$=txtProduct]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfUOM]").val("");
                $("[id$=txtUOM]").val("");
                $("[id$=txtBoxPerCarton]").val("");
                $("[id$=txtPcsPerBox]").val("");
            }
            else if (targetControlID == "txtProduct") {
                $("[id$=hdfBrand]").val("0");
                $("[id$=txtBrand]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=hdfUOM]").val("");
                $("[id$=txtUOM]").val("");
                $("[id$=txtBoxPerCarton]").val("");
                $("[id$=txtPcsPerBox]").val("");
            }
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
        function AfterDateSelect(controlID) {
            if (controlID == "txtBookingDate") {
                $("[id$=btnBookingDate]").click();
                
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
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="23" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiry')"
                                            ValidationGroup="enquiry" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="24" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiry')"
                                            ValidationGroup="enquiry" ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="25" Text="<%$resources:ErpRes,Delete %>"
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
                                            CommandName="CANCEL" TabIndex="26" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
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
                            <asp:LinkButton runat="server" ID="lbnList" TabIndex="26" CommandName="ENQUIRYLIST"
                                OnClick="ActionHandler" CssClass="list-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnEnquiry" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnEnquiry" Text="<%$resources:PageNameRes,Order %>"
                                TabIndex="27" CommandName="ENQUIRY" OnClick="ActionHandler" CssClass="tab-active"
                                OnClientClick="javascript:return false;"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnQuotation" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnQuotation" Text="<%$resources:PageNameRes,OrderAccept %>"
                                TabIndex="28" CommandName="QUOTE" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
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
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                            <div class="contentwrapper">
                                <asp:HiddenField ID="hdfStatus" runat="server" Value="0" />
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
                                                    <asp:TextBox ID="txtCustomer" runat="server" TabIndex="1" MaxLength="100"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfCustomer" runat="server" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="enquiry" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                            runat="server" ControlToValidate="txtCustomer" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Customer %>"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <asp:Button ID="btnCustSelected" runat="server" OnClick="ActionHandler" CommandName="CUSTOMERSELECTED"
                                                        EnableTheming="false" Style="display: none" />
                                                    <asp:Label runat="server" ID="lblEnqDate" Text="<%$ resources:EnqDate %>" AssociatedControlID="txtEnqDate"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtEnqDate" TabIndex="2" MaxLength="12" CssClass="Uidate-picker"
                                                        onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfEnqDate" runat="server" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfEnqDate" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="enquiryDtl" EnableClientScript="true" runat="server" ControlToValidate="txtEnqDate"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EnqDate %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreEnqDate" CssClass="star" ValidationGroup="enquiryDtl"
                                                            runat="server" ControlToValidate="txtEnqDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_EnqDate_Valid %>"
                                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                            EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblRefDate" Text="<%$ resources:RefDate %>" AssociatedControlID="txtRefDate"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtRefDate" TabIndex="2" MaxLength="12" CssClass="Uidate-picker"
                                                        onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label ID="lblToPort" runat="server" AssociatedControlID="txtBrand" Text="<%$ resources:ToPort %>">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtToPort" runat="server" TabIndex="4" MaxLength="100"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblEnqTrxNo" Text="<%$ resources:EnqNo %>" AssociatedControlID="lblEnqTrxNoTxt"></asp:Label>
                                                    <asp:Label runat="server" ID="lblEnqTrxNoTxt"></asp:Label>
                                                    <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                                    <asp:Label ID="lblRefNo" runat="server" AssociatedControlID="txtRefNo" Text="<%$ resources:RefNo %>">
                                                    </asp:Label>
                                                    <asp:TextBox ID="txtRefNo" runat="server" TabIndex="4" MaxLength="100"></asp:TextBox>
                                                    <asp:Label ID="lblShipBy" runat="server" AssociatedControlID="ddlShipBy" Text="<%$ resources:ShipBy %>">
                                                    </asp:Label>
                                                    <asp:DropDownList ID="ddlShipBy" runat="server" TabIndex="3">
                                                    </asp:DropDownList>
                                                    <asp:Label ID="lblTranshipment" runat="server" AssociatedControlID="ddlTranshipment"
                                                        Text="<%$ resources:Transhipment %>"></asp:Label>
                                                    <asp:DropDownList ID="ddlTranshipment" runat="server" TabIndex="5">
                                                    </asp:DropDownList>
                                                    <asp:HiddenField ID="hdfVersion" runat="server" Value="0" />
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
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label ID="lblBrand" runat="server" AssociatedControlID="txtBrand" Text="<%$ resources:Brand_Mand %>"><%--<%$ resources:Brand %>--%>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtBrand" runat="server" TabIndex="6" MaxLength="200"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfBrand" runat="server" />
                                                            <asp:Button ID="btnSelectProduct" runat="server" OnClick="ActionHandler" CommandName="PRODUCTSELECTED"
                                                                EnableTheming="false" Style="display: none" />
                                                            <asp:RequiredFieldValidator ID="vrfBrand" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="enquiryDtl" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                                runat="server" ControlToValidate="txtBrand" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Brand %>"></asp:RequiredFieldValidator>
                                                            <asp:Label ID="lblQty" runat="server" AssociatedControlID="txtQty" Text="<%$ resources:Quantity_Mand %>"><%--<%$ resources:Quantity %>--%>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtQty" runat="server" CssClass="Uiinput-qty" MaxLength="17" TabIndex="7"></asp:TextBox>
                                                            <span style="width: 10px; border: 0 none; background: none;">
                                                                <div class="starwrap">
                                                                    <asp:RequiredFieldValidator ID="vrfQuantity" CssClass="star" SetFocusOnError="true"
                                                                        ValidationGroup="enquiryDtl" EnableClientScript="true" runat="server" ControlToValidate="txtQty"
                                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                                    </asp:RequiredFieldValidator>
                                                                    <asp:RegularExpressionValidator ID="vreQuantity" runat="server" ControlToValidate="txtQty"
                                                                        ErrorMessage="<%$ resources:Err_Quantity_Valid %>" ValidationExpression="^\$?([0-9]{0,14})?(\.[0-9]{0,2})?$"
                                                                        Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiryDtl">
                                                                    </asp:RegularExpressionValidator>
                                                                </div>
                                                            </span>
                                                            <asp:TextBox runat="server" ID="txtUOM" CssClass="input-normal Uiinput-uom" Enabled="false"></asp:TextBox>
                                                            <div class="clear">
                                                            </div>
                                                            <asp:Label runat="server" ID="lblBookingDate" Text="<%$ resources:BookingDate_Mand %>"
                                                                AssociatedControlID="txtBookingDate"></asp:Label><%--<%$ resources:ReqByDate %>--%>
                                                            <asp:TextBox runat="server" ID="txtBookingDate" TabIndex="8" MaxLength="12" CssClass="Uidate-picker"
                                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                                <asp:Button ID="btnBookingDate" runat="server" EnableTheming="false" OnClick="ActionHandler" CommandName="BOOKINGDATECHANGE" style="display:none" />
                                                            <div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfBookingDate" CssClass="star" SetFocusOnError="true"
                                                                    ValidationGroup="enquiryDtl" EnableClientScript="true" runat="server" ControlToValidate="txtBookingDate"
                                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BookingDate %>">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="vreBookingDate" CssClass="star" ValidationGroup="enquiryDtl"
                                                                    runat="server" ControlToValidate="txtBookingDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_BookingDate_Valid %>"
                                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                                    EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                            </div>
                                                            <div class="clear">
                                                            </div>
                                                            <asp:Label runat="server" ID="lblPcsPerBox" Text="<%$ resources:PcsPerBox %>" AssociatedControlID="txtPcsPerBox"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtPcsPerBox" CssClass="Uiinput-qty" Enabled="false"></asp:TextBox>
                                                            <div class="clear">
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-S">
                                                            <asp:Label ID="lblProduct" runat="server" AssociatedControlID="txtProduct" Text="<%$ resources:Product_Mand %>"><%--<%$ resources:Product %>--%>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtProduct" runat="server" MaxLength="100"></asp:TextBox>
                                                            <asp:HiddenField ID="hdfProduct" runat="server" />
                                                            <asp:RequiredFieldValidator ID="vrfProduct" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="enquiryDtl" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                                runat="server" ControlToValidate="txtProduct" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Product %>"></asp:RequiredFieldValidator>
                                                            <%--<asp:Label runat="server" ID="lblUOM" Text="<%$ resources:UOM %>" AssociatedControlID="txtUOM"></asp:Label>--%>
                                                            <asp:Label runat="server" ID="lblReqByDate" Text="<%$ resources:ReqdDate_Mand %>"
                                                                AssociatedControlID="txtReqByDate"></asp:Label><%--<%$ resources:ReqByDate %>--%>
                                                            <asp:TextBox runat="server" ID="txtReqByDate" TabIndex="8" MaxLength="12" CssClass="Uidate-picker"
                                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
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
                                                            <asp:Label ID="lblRate" runat="server" AssociatedControlID="txtRate" Text="<%$ resources:Rate_Mand %>"><%--<%$ resources:Quantity %>--%>
                                                            </asp:Label>
                                                            <asp:TextBox ID="txtRate" runat="server" CssClass="Uiinput-rate" MaxLength="17" TabIndex="7"></asp:TextBox>
                                                            <div class="starwrap">
                                                                <asp:RequiredFieldValidator ID="vrfRate" CssClass="star" SetFocusOnError="true" ValidationGroup="enquiryDtl"
                                                                    EnableClientScript="true" runat="server" ControlToValidate="txtRate" Display="Dynamic"
                                                                    Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                                </asp:RequiredFieldValidator>
                                                                <asp:RegularExpressionValidator ID="vreRate" runat="server" ControlToValidate="txtRate"
                                                                    ErrorMessage="<%$ resources:Err_Rate_Valid %>" ValidationExpression="^\$?([0-9]{0,13})?(\.[0-9]{0,3})?$"
                                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiryDtl">
                                                                </asp:RegularExpressionValidator>
                                                            </div>
                                                            <asp:HiddenField ID="hdfUOM" runat="server" />
                                                            <asp:HiddenField ID="hdfPackingSpec" runat="server" />
                                                            <div class="clear">
                                                            </div>
                                                            <asp:Label runat="server" ID="lblBoxPerCarton" Text="<%$ resources:BoxPerCarton %>"
                                                                AssociatedControlID="txtBoxPerCarton"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtBoxPerCarton" CssClass="Uiinput-qty" Enabled="false"></asp:TextBox>
                                                            <div class="clear">
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <div class="divcol-S">
                                                            <asp:Label runat="server" ID="lblDtlRemark" Text="<%$ resources:Remarks %>" AssociatedControlID="txtDtlRemark"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtDtlRemark" TabIndex="9" MaxLength="480"></asp:TextBox>
                                                            <%--<asp:TextBox runat="server" ID="txtDtlRemark" TabIndex="9" MaxLength="500" TextMode="MultiLine"
                                                                CssClass="multiline-1line" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>--%>
                                                            <asp:ImageButton runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="10"
                                                                OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiryDtl')"
                                                                ToolTip="<%$resources:ErpRes,Add %>" CommandArgument="SEC_ActionPanel" ValidationGroup="enquiryDtl"
                                                                SkinID="plus" />
                                                            <asp:ImageButton runat="server" ID="btnClearItem" CommandName="CLEARITEM" TabIndex="11"
                                                                OnClick="ActionHandler" ToolTip="<%$resources:Controls,Clear %>" CommandArgument="SEC_ActionPanel"
                                                                SkinID="cancel" />
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
                                                        EmptyDataRowStyle-CssClass="emptytable">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:Brand %>">
                                                                <ItemTemplate>
                                                                    <%--<asp:HiddenField ID="hdfEnqDtlPK" runat="server" Value='<%# Eval("") %>' />
                                                    <asp:HiddenField ID="hdfEnqItemPK" runat="server" Value='<%# Eval("") %>' />--%>
                                                                    <asp:Label ID="lblBrandLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CIM_BRAND_NAME"), 13) %>'
                                                                        ToolTip='<%# Eval("CIM_BRAND_NAME")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Product %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CIM_ITEM_TEXT"), 30) %>'
                                                                        ToolTip='<%# Eval("CIM_ITEM_TEXT")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="25%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:UoM %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUoMLst" runat="server" Text='<%# Eval("CIM_UOM_TEXT") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("CIM_UOM_TEXT").ToString()) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="5%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Quantity %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuantityLst" runat="server" Text='<%# Eval("CED_ENQ_QTY", "{0:N}") %>'
                                                                        ToolTip='<%# Eval("CED_ENQ_QTY", "{0:N}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="9%" CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:PcsPerBox %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPcsPerBoxLst" runat="server" Text='<%# Eval("CIM_PCS_PER_IP") %>'
                                                                        ToolTip='<%# Eval("CIM_PCS_PER_IP")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="9%" CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:BoxPerCarton %>" HeaderStyle-CssClass="amount-numeric">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBoxPerCartonLst" runat="server" Text='<%# Eval("CIM_PCS_PER_OP") %>'
                                                                        ToolTip='<%# Eval("CIM_PCS_PER_OP")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="9%" CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:ReqByDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblReqByDateLst" runat="server" Text='<%# Eval("CED_REQUIRED_DATE", Resources.ErpRes.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval("CED_REQUIRED_DATE", Resources.ErpRes.DateFormatGrid)%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="11%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Remarks %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRemarksLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("CED_REMARKS")), 10) %>'
                                                                        ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("CED_REMARKS"))) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="11%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblRateLst" runat="server" Text='<%# Eval("CED_RATE", "{0:c}") %>'
                                                                        ToolTip='<%# Eval("CED_RATE", "{0:c}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                <ItemStyle Width="9%" CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:BookingDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBookingDateLst" runat="server" Text='<%# Eval("CED_BOOKING_DATE", Resources.ErpRes.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval("CED_BOOKING_DATE", Resources.ErpRes.DateFormatGrid)%>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="11%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="btnEditItem" runat="server" OnClick="ActionHandler" CommandName="EDITITEM"
                                                                        SkinID="imbeditgrid" ToolTip="Edit" TabIndex="12" />
                                                                    <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                                        SkinID="imbdeletegrid" ToolTip="Delete" TabIndex="13" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" />
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
                                                            <asp:Label ID="lblDeliveryTerms" runat="server" AssociatedControlID="ddlDeliveryTerms"
                                                                Text="<%$ resources:DeliveryTerms %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlDeliveryTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                                TabIndex="14">
                                                            </asp:DropDownList>
                                                            <div class="clear">
                                                            </div>
                                                            <asp:Label ID="Label3" runat="server" Text="" AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtDeliveryTerms" MaxLength="500" TabIndex="15" TextMode="MultiLine"
                                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="vreDeliveryTerms" runat="server" ControlToValidate="txtDeliveryTerms"
                                                                ErrorMessage="<%$ Resources:Err_DeliveryTerms %>" ValidationExpression="^[\s\S]{0,500}$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-L">
                                                            <asp:Label ID="lblPaymentTerms" runat="server" AssociatedControlID="ddlPaymentTerms"
                                                                Text="<%$ resources:PaymentTerms %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlPaymentTerms" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                                TabIndex="16">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="Label2" runat="server" Text="" AssociatedControlID="txtPaymentTerms"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtPaymentTerms" MaxLength="500" TabIndex="17" TextMode="MultiLine"
                                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="vrePaymentTerms" runat="server" ControlToValidate="txtPaymentTerms"
                                                                ErrorMessage="<%$ Resources:Err_PaymentTerms %>" ValidationExpression="^[\s\S]{0,500}$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <div class="div2col-L">
                                                            <asp:Label ID="lblSpecialCause" runat="server" AssociatedControlID="ddlSpecialCause"
                                                                Text="<%$ resources:SpecialCause %>"></asp:Label>
                                                            <asp:DropDownList ID="ddlSpecialCause" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler"
                                                                TabIndex="18">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="Label4" runat="server" Text="" AssociatedControlID="txtSpecialCause"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtSpecialCause" MaxLength="500" TabIndex="19" TextMode="MultiLine"
                                                                CssClass="multiline-1col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="vreSpecialCause" runat="server" ControlToValidate="txtSpecialCause"
                                                                ErrorMessage="<%$ Resources:Err_SpecialCause %>" ValidationExpression="^[\s\S]{0,500}$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="enquiry"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div class="div2col-L">
                                                            <asp:Label runat="server" ID="lblShippingAddress" Text="<%$ resources:ShippingAddress %>"
                                                                AssociatedControlID="ddlCustAddress"></asp:Label>
                                                            <asp:DropDownList ID="ddlCustAddress" runat="server" TabIndex="20" AutoPostBack="true"
                                                                OnSelectedIndexChanged="ActionHandler">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="Label1" runat="server" AssociatedControlID="txtShippingAddress"></asp:Label>
                                                            <asp:TextBox runat="server" ID="txtShippingAddress" MaxLength="500" TabIndex="21"
                                                                TextMode="MultiLine" EnableTheming="false" CssClass="multiline-1col" onkeydown="limitText(this,500);"
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
                                                            <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TabIndex="22" TextMode="MultiLine"
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
