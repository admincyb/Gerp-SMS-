<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="COAFinYearOpen.aspx.cs" Inherits="ERPSMS_v01.Finance.COAFinYearOpen" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            DateInit();
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtTransNoSearch", url, "hdfTransNoSearch", true, true, 0, "FINYEAROPENINGNO");
        }
        function DateInit() {
            //<summary>function used to make datepicker</summary>
            GrandScriptUtils.DatePickerCommon("txtTransDate");
            GrandScriptUtils.DatePickerCommon("txtTranDateSearch");
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlCOAFinYearOpen">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPnl" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="10" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li id="pnlEdit">
                                        <asp:Button runat="server" TabIndex="52" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="56" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="75" Text="<%$resources:ErpRes,SaveSubmit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript: return ValidatePageNow('fy');"
                                            ValidationGroup="wo" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="76" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript: return ValidatePageNow('fy');"
                                            ValidationGroup="wo" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ToolTip="<%$resources:Controls,Save %>" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="102" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            CommandName="CANCEL" TabIndex="78" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:Controls,Cancel %>" />
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
                                            <asp:Label ID="lblTranDateSearch" runat="server" Text='<%$ Resources:TransDate%>'
                                                AssociatedControlID="txtTranDateSearch"></asp:Label>
                                            <asp:TextBox ID="txtTranDateSearch" runat="server" TabIndex="12" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="input-small"></asp:TextBox>

                                            <asp:Label runat="server" ID="lblTransSearch" Text="<%$ resources:TransNo %>" AssociatedControlID="txtTransNoSearch"></asp:Label>
                                            <asp:TextBox ID="txtTransNoSearch" runat="server" TabIndex="3" CssClass="lbl-26perc" MaxLength="17"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTransNoSearch" runat="server" Value="0" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblYearSearch" runat="server" Text="<%$resources:FinYear %>" AssociatedControlID="ddlYearSearch"></asp:Label>
                                            <asp:DropDownList ID="ddlYearSearch" runat="server" TabIndex="15" CssClass="input-half">
                                            </asp:DropDownList>

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
                                <asp:GridView runat="server" ID="grdOpeningList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler" AutoGenerateColumns="false"
                                    EmptyDataRowStyle-CssClass="emptytable" OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="Label1" runat="server" TabIndex="23" Text="<%$ resources:ErpRes,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" Checked="false" TabIndex="14"
                                                    GroupName="SelectOne" ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    OnCheckedChanged="ActionHandler" AutoPostBack="true" />
                                                <asp:HiddenField ID="hdfCOH_PK" runat="server" Value='<%# Eval("COH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval("COH_STATUS") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TransDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFYTransDate" runat="server" Text='<%# Eval("COH_DATE", Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TransNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFYTransNo" runat="server" Text='<%# string.IsNullOrEmpty(Eval("COH_NO").ToString()) ? "[NEW]" : Eval("COH_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FinYear %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFY" runat="server" Text='<%# Eval("COH_FIN_YEAR_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Action %>">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>" OnClick="ActionHandler"
                                                    TabIndex="14" SkinID="imbeditgrid" CommandName="EDITITEM" CommandArgument='<%# Eval("COH_PK") %>' />
                                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$resources:ErpRes,View %>" OnClick="ActionHandler"
                                                    TabIndex="14" SkinID="btnview" CommandName="VIEW" CommandArgument='<%# Eval("COH_PK") %>' />
                                                <asp:HiddenField ID="hdfCOH_PK" runat="server" Value='<%# Eval("COH_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <uc2:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <div class="contentwrapper">
                                <div>
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblForTransNo" runat="server" Text="<%$resources:TransNo %>" AssociatedControlID="lblTransNo"></asp:Label>
                                                    <asp:Label ID="lblTransNo" runat="server" TabIndex="6" CssClass="input-half margnbotm0"></asp:Label>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblFinYear" runat="server" AssociatedControlID="ddlFinYear" Text='<%$ Resources:FinYear%>'></asp:Label>
                                                    <asp:DropDownList ID="ddlFinYear" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="ActionHandler" TabIndex="15" CssClass="input-small">
                                                    </asp:DropDownList>
                                                    <asp:Button runat="server" ID="btnLoad" CommandName="LOADFROMTEMPLATE" ValidationGroup="ycvLoad"
                                                        OnClick="ActionHandler" Text="<%$resources:Controls,LoadFromTemplate %>" CommandArgument="PageAction_Entry"
                                                        SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,LoadFromTemplate %>"
                                                        OnClientClick="javascript:ValidatePageNow('fy')" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label ID="lblTransDate" runat="server" Text='<%$ Resources:TransDate%>'
                                                        AssociatedControlID="txtTransDate"></asp:Label>
                                                    <asp:TextBox ID="txtTransDate" runat="server" TabIndex="12" onkeydown="return CheckKey(event)"
                                                        onpaste="return false;" CssClass="input-small" Enabled="false"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <h1 class="search-colapse-normal"><%= GetLocalResourceObject("ChartOfAccounts").ToString() %></h1>
                                    <div class="gridwrap scroll-container">
                                        <asp:GridView ID="grdCOA" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                            AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                            Width="100%" ShowFooter="true" FooterStyle-CssClass="emptyfooter">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:AccountCode %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccCode" runat="server" Text='<%# Eval("ChartOfAccountCode") %>'
                                                            ToolTip='<%# Eval("ChartOfAccountCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Name %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAccName" runat="server" Text='<%# Eval("ChartOfAccountName") %>'
                                                            ToolTip='<%# Eval("ChartOfAccountName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="35%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Parent %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblParent" runat="server" Text='<%# Eval("Parent") %>'
                                                            ToolTip='<%# Eval("Parent") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="23%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("COAType") %>'
                                                            ToolTip='<%# Eval("COAType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="6%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:IsGroup %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIsGroup" runat="server" Text='<%# Eval("IsGroup").ToString() == "True" ? "Yes" : "No" %>'
                                                            ToolTip='<%# Eval("IsGroup").ToString() == "True" ? "Yes" : "No" %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="6%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Category %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCatgeory" runat="server" Text='<%# Eval("Category") %>'
                                                            ToolTip='<%# Eval("Category") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Balance %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBalance" runat="server" Text='<%# GetFormattedNumberWithComma(Eval("Balance")) %>'
                                                            ToolTip='<%# GetFormattedNumberWithComma(Eval("Balance")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" Wrap="true" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div id="divWkfSubmit" style="display: none;">
                                    <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                                    <uc1:workflowusercomments id="ucrWrkf" runat="server" validationgroup="fy" />
                                </div>

                                <div id="diverror" style="display: none">
                                    <%--Use this label to bind the server errors--%>
                                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                                    <asp:ValidationSummary ID="vsPage" ValidationGroup="fy" runat="server" />
                                </div>

                                <asp:HiddenField ID="hdfDecimalFormatWithComma" runat="server" />

                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
