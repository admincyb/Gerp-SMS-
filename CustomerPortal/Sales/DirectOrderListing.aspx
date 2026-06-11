<%@ Page Title="<%$ Resources:Captions,Title_DirectOrderListing %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" Theme="Classic"
    AutoEventWireup="true" CodeBehind="DirectOrderListing.aspx.cs" Inherits="CustomerPortal.Sales.DirectOrderListing" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

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

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomer", true, true, "CUSTOMER");
            if ($("[id$=txtCustomer]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }
            GrandScriptUtils.MakeAutoCompleteDDL("txtEnqNumber", url, "hdfEnqNumber", true, true, "DIRECTORDERNO");
            //GrandScriptUtils.MakeAutoCompleteDDL("txtQuotNumber", url, "hdfQuotNumber", true, true, "DIRECTORDERNO");

        }
        function ShowListing(flag) {
            if (flag) {

                $("[id$=PageAction_List]").show();
                $("[id$=pnlListing]").show();
            }
            else {

                $("[id$=PageAction_List]").hide();
                $("[id$=pnlListing]").hide();
            }
            return false;
        }

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
            }
            else if (mode == 2) {
            }
        }
        function QuotationMode(mode) {
            if (mode) {
                $("[id$=pnlNew]").hide();
                $("[id$=pnlQuotation]").hide();
                $("#divEnquiryFilter").hide();
            }
            else
                $("#divQuotationFilter").hide();
        }
        function DisableAuto(extender, hfield) {
            ///<summary>
            /// Used to disable Autocomplete
            ///</summary>
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlRFQListing">
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
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li id="pnlNew">
                                        <asp:Button runat="server" TabIndex="8" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>" CommandArgument="SEC_ActionPanel"
                                            ToolTip="<%$Resources:Controls,Add%>" />
                                    </li>
                                    <li id="pnlEdit">
                                        <asp:Button runat="server" TabIndex="9" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="10" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li id="pnlQuotation">
                                        <asp:Button runat="server" ID="btnQuote" CommandName="QUOTE" TabIndex="11" Text="<%$resources: OrderAccept %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-respond"
                                            ToolTip="<%$resources: OrderAccept %>" />
                                    </li>
                                    <li id="pnlPrint">
                                    <asp:Button runat="server" TabIndex="12" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                 <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="Span1" runat="server" class="list-active">
                            <asp:LinkButton runat="server" ID="lbnList" TabIndex="13" CommandName="ENQUIRYLIST"
                                OnClick="ActionHandler" CssClass="list-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnEnquiry" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnEnquiry" Text="<%$resources:PageNameRes,Order %>"
                                TabIndex="14" CommandName="ENQUIRY" OnClick="ActionHandler" CssClass="tab-inactive"
                               ></asp:LinkButton>
                        </span></li>
                        <li><span id="spnQuotation" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnQuotation" Text="<%$resources:PageNameRes,OrderAccept %>"
                                TabIndex="15" CommandName="QUOTATION" OnClick="ActionHandler" CssClass="tab-inactive"
                                 ></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="16" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="17" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="large" MaxLength="100" TabIndex="1"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomer" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="medium" TabIndex="3" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                        <%--<div id="divQuotationFilter">
                                            <asp:Label ID="lblQuotNumber" runat="server" Text="<%$resources:QuotNo %>" AssociatedControlID="txtQuotNumber"></asp:Label>
                                            <asp:TextBox ID="txtQuotNumber" runat="server" CssClass="medium" MaxLength="100" TabIndex="2"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfQuotNumber" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            </div>--%>
                                            <div id="divEnquiryFilter">
                                            <asp:Label ID="lblEnqNumber" runat="server" Text="<%$resources:QuotNo %>" AssociatedControlID="txtEnqNumber"></asp:Label>
                                            <asp:TextBox ID="txtEnqNumber" runat="server" CssClass="medium" MaxLength="100" TabIndex="2"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfEnqNumber" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            </div>
                                            <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="medium" TabIndex="4" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblSearchButton" runat="server" AssociatedControlID="btnSearch"></asp:Label>
                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$ resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="5"
                                                CommandName="SEARCH" SkinID="btnInner-search" />
                                            <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="6"
                                                ToolTip="<%$ resources:Controls,Clear %>" OnClick="ActionHandler" CommandName="CLEAR"
                                                SkinID="btnInner-cancel-dsd" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdEnquiryList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AllowPaging="true" OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"
                                    OnRowDataBound="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="7" runat="server" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:QuotNo %>" SortExpression="CEH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuotNo" runat="server" Text='<%# Eval("CEH_NO") %>' ToolTip='<%# Eval("CEH_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:QuotDate %>" SortExpression="CEH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuotDate" runat="server" Text='<%# Eval("CEH_DATE") %>' ToolTip='<%# Eval("CEH_DATE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EnqNo %>" SortExpression="CEH_REF_NO">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfRefID" runat="server" Value='<%# Eval("CEH_PK") %>' />
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Eval("CEH_REF_NO")%>' ToolTip='<%# Eval("CEH_REF_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EnqDate %>" SortExpression="CEH_REF_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("CEH_REF_DATE") %>' ToolTip='<%# Eval("CEH_REF_DATE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="<%$ resources:CustomerName %>" SortExpression="CEH_CUSTOMER_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CEH_CUSTOMER_TEXT") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("CEH_CUSTOMER_TEXT").ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CustCountry %>" SortExpression="CEH_CUS_COUNTRY_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustCountry" runat="server" Text='<%# Eval("CEH_CUS_COUNTRY_TEXT") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("CEH_CUS_COUNTRY_TEXT").ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>                                        
                                        <asp:TemplateField HeaderText="<%$ resources:ShipToPort %>" SortExpression="CEH_TO_PORT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("CEH_TO_PORT") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("CEH_TO_PORT").ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>" SortExpression="CEH_NET_AMOUNT" HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("CEH_NET_AMOUNT") %>'
                                                    ToolTip='<%# Eval("CEH_NET_AMOUNT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>" SortExpression="CEH_STATUS_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("CEH_STATUS_TEXT") %>'
                                                    ToolTip='<%# Eval("CEH_STATUS_TEXT")%>'></asp:Label>
                                                <asp:HiddenField ID="hdfStatus" runat="server" Value='<%# Eval("CEH_STATUS") %>' />
                                                <asp:HiddenField ID="hdfTrxStatus" runat="server" Value='<%# Eval("CEH_TRX_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgSOCreated" runat="server" OnClientClick="return false" EnableTheming="false" CssClass="approved-icon" ToolTip="<%$ resources:SOCreated %>" />
                                                <asp:HiddenField runat="server" ID="hdfSOCreated" Value='<%# Eval("CEH_QUOTATION_FLAG") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="3%" />
                                        </asp:TemplateField>
                                        
                                    </Columns>
                                </asp:GridView>
                                <%--<uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>