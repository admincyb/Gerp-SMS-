<%@ Page Title="<%$ Resources:Captions,Title_DirectOrderListing %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="DOListing.aspx.cs"
    Inherits="CustomerPortal.Sales.DOListing" Theme="ClassicExt" %>

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
            GrandScriptUtils.MakeAutoCompleteDDL("txtEnqNumber", url, "hdfEnqNumber", true, true, "CUSTOMERORDERNO");
            GrandScriptUtils.MakeAutoCompleteDDL("txtQuotNumber", url, "hdfQuotNumber", true, true, "SALEORDERNO");

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
                                        <asp:Button runat="server" ID="btnQuote" CommandName="QUOTE" TabIndex="11" Text="<%$resources:SO %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-respond"
                                            ToolTip="<%$resources:SO %>" />
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
                        <li><span id="spnEnqList" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnList" TabIndex="13" CommandName="ENQUIRYLIST"
                                Text="<%$resources:PageNameRes,DirectOrderListing %>" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnQtnList" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnQtnList" TabIndex="13" CommandName="QUOTATIONLIST"
                                Text="<%$resources:PageNameRes,OrderAcceptListing %>" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnEnquiry" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnEnquiry" Text="<%$resources:PageNameRes,DirectOrder %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="14" CommandName="ENQUIRY" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnQuotation" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnQuotation" Text="<%$resources:PageNameRes,DirectOrderAccept %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="15" CommandName="QUOTATION" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks asptbllinks">
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
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="input-small margnrgt1-5per" TabIndex="3" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate" CssClass="middle-lbl-a"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="input-small" TabIndex="4" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="div-separatn">
                                <table class="table-devide" id="Table2" style="background: #f2f2f2;">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomer"
                                                    CssClass="margnbotm0"></asp:Label>
                                                <asp:TextBox ID="txtCustomer" runat="server" MaxLength="100" TabIndex="1" CssClass="select-half margnbotm0"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfCustomer" runat="server" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <div class="floatLeft w60perc txt-rgt">
                                                    <div id="divQuotationFilter">
                                                        <asp:Label ID="lblQuotNumber" runat="server" Text="<%$resources:QuotNo %>" AssociatedControlID="txtQuotNumber"
                                                            CssClass="middle-lbl-small margnbotm0"></asp:Label>
                                                        <asp:TextBox ID="txtQuotNumber" runat="server" MaxLength="100" TabIndex="2" CssClass="input-small-d margnbotm0"></asp:TextBox>
                                                        <asp:HiddenField ID="hdfQuotNumber" runat="server" Value="" />
                                                    </div>
                                                    <div id="divEnquiryFilter">
                                                        <asp:Label ID="lblEnqNumber" runat="server" Text="<%$resources:EnqNo %>" AssociatedControlID="txtEnqNumber"
                                                            CssClass="middle-lbl-small margnbotm0"></asp:Label>
                                                        <asp:TextBox ID="txtEnqNumber" runat="server" MaxLength="100" TabIndex="2" CssClass="input-small-d margnbotm0"> </asp:TextBox>
                                                        <asp:HiddenField ID="hdfEnqNumber" runat="server" Value="" />
                                                    </div>
                                                    
                                                </div>
                                                <div class="floatLeft w30perc">
                                                    <%--<asp:Label ID="lblSearchButton" runat="server" AssociatedControlID="btnSearch"></asp:Label>--%>
                                                    <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                        ToolTip="<%$ resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="5"
                                                        CommandName="SEARCH" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                                    <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                        TabIndex="6" ToolTip="<%$ resources:Controls,Clear %>" OnClick="ActionHandler"
                                                        CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
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
                                                    AutoPostBack="true" OnCheckedChanged="ActionHandler" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EnqNo %>" SortExpression="CEH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuotNo" runat="server" Text='<%# Eval("CEH_NO") %>' ToolTip='<%# Eval("CEH_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EnqDate %>" SortExpression="CEH_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQuotDate" runat="server" Text='<%# Eval("CEH_DATE") %>' ToolTip='<%# Eval("CEH_DATE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RefNo %>" SortExpression="CEH_REF_NO">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdfRefID" runat="server" Value='<%# Eval("CEH_PK") %>' />
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Eval("CEH_REF_NO")%>' ToolTip='<%# Eval("CEH_REF_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:RefDate %>" SortExpression="CEH_REF_DATE">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("CEH_REF_DATE") %>' ToolTip='<%# Eval("CEH_REF_DATE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CustomerName %>" SortExpression="CEH_CUSTOMER_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerName" runat="server" Text='<%# Eval("CEH_CUSTOMER_TEXT") %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("CEH_CUSTOMER_TEXT").ToString()),25)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CustCountry %>" SortExpression="CEH_CUS_COUNTRY_TEXT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustCountry" runat="server" Text='<%# Eval("CEH_CUS_COUNTRY_TEXT") %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("CEH_CUS_COUNTRY_TEXT").ToString()),16)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ShipToPort %>" SortExpression="CEH_TO_PORT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString( Eval("CEH_TO_PORT"),18) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("CEH_TO_PORT").ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TotalAmount %>" SortExpression="CEH_NET_AMOUNT"
                                            HeaderStyle-CssClass="amount-numeric">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Convert.ToDouble(Eval("CEH_NET_AMOUNT")).ToString("c") %>'
                                                    ToolTip='<%# Convert.ToDouble(Eval("CEH_NET_AMOUNT")).ToString("c") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="amount-numeric" Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="CEH_TRX_STATUS_TEXT">
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval("CEH_STATUS_TEXT") %>' />
                                                <asp:HiddenField ID="hdfStatus" runat="server" Value='<%# Eval("CEH_STATUS") %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval("CEH_DEPT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgSOCreated" runat="server" OnClientClick="return false" EnableTheming="false"
                                                    CssClass="order-icon" ToolTip="<%$ resources:SOCreated %>" />
                                                <asp:HiddenField runat="server" ID="hdfSOCreated" Value='<%# Eval("CEH_QUOTATION_FLAG") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="2%" />
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
