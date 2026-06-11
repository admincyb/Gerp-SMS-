<%@ Page Title="<%$ Resources:Captions,Title_AccountsMaster %>" Language="C#" Theme="ClassicExt"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="FinReportCfg.aspx.cs" Inherits="ERPSMS_v01.Finance.Administration.Masters.FinReportCfg" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        function InitComponents() {
            $("[id$='txtSequence']").ForceNumericOnly();
            //            if ($("[id$='hdfHasChild']").val() == '1')
            //                $("[id$='chkIsGroup']").attr("disabled", true);
            //            else
            //                $("[id$='chkIsGroup']").attr("disabled", false);
        }
        function ShowHideAdvancedSearch(flag) {
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
        function ShowListing(flag) {
            if (flag == 0) {
                $("[id$=PageAction_Report]").show();
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
            }
            else if (flag == 1) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
                $("[id$=PageAction_Report]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=PageAction_Report]").hide();
            }
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
            if (mode == 0) {
                $("[id$=btnNew]").hide();
                $("[id$=btnView]").hide();
                $("[id$=btnEdit]").hide();
            }
            else if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }

        function SetTabs(tab) {
            if (tab == 0) {
                $("[id$='spnReport']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnReport']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnAccountTree']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAccountTree']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAccountListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAccountListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnMngAccounts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnMngAccounts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='PageAction_Report']").show();
                $("[id$='PageAction_Tree']").hide();
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='spnMngAccounts']").hide();
            }
            else if (tab == 1) {
                $("[id$='spnReport']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnReport']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAccountTree']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnAccountTree']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnAccountListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAccountListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnMngAccounts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnMngAccounts']").removeClass("tab-active").addClass("tab-inactive");

                $("[id$='PageAction_Tree']").show();
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='ModifiedDatePnl']").hide();
            }
            else if (tab == 2) {
                $("[id$='spnReport']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnReport']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAccountTree']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAccountTree']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAccountListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnAccountListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnMngAccounts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnMngAccounts']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='PageAction_Tree']").hide();
            }
            else {
                $("[id$='spnReport']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnReport']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAccountTree']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAccountTree']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnAccountListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnAccountListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnMngAccounts']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnMngAccounts']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='PageAction_Tree']").hide();
            }
        }

        function ValidateNow() {

            if (typeof (Page_ClientValidate) == 'function') {
                Page_ClientValidate();
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false; //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlAccounts" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlEntry" runat="server" style="display: none">
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            OnClientClick="javascript:ValidateNow()" ValidationGroup="Account" SkinID="btnInner-Save"
                                            TabIndex="19" CommandArgument="SEC_ActionPanel" ToolTip="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li id="pnlDelete" runat="server">
                                        <asp:Button ID="btnDelete" runat="server" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            Visible="false" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClick="ActionHandler"
                                            TabIndex="20" ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$resources:Controls,Cancel %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClick="ActionHandler"
                                            TabIndex="21" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="NEW" Text="<%$resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$resources:Controls,New %>"
                                            OnClick="ActionHandler" TabIndex="6" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="EDIT" Text="<%$resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$resources:Controls,Edit %>"
                                            OnClick="ActionHandler" TabIndex="7" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnView" runat="server" CommandName="VIEW" Text="<%$resources:Controls,View %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-View" ToolTip="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" TabIndex="8" />
                                    </li>
                                </ul>
                            </asp:TableCell></asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnReport" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnReport" runat="server" Text="<%$resources:Controls,Report %>"
                                CssClass="tab-inactive" CommandName="REPORT" OnClick="ActionHandler" TabIndex="22" />
                        </span></li>
                        <li><span id="spnAccountTree" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnAccountTree" runat="server" Text="<%$resources:Controls,Tree %>"
                                CssClass="tab-inactive" CommandName="TREE_SELECT" OnClick="ActionHandler" TabIndex="22" />
                        </span></li>
                        <li><span id="spnAccountListing" runat="server" class="tab-active">
                            <asp:LinkButton ID="lbnAccountListing" runat="server" Text="<%$resources:Controls,List %>"
                                CommandName="SELECT" CssClass="tab-active" OnClick="ActionHandler" TabIndex="23" />
                        </span></li>
                        <li><span id="spnMngAccounts" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnMngAccounts" runat="server" Text="<%$resources:Controls,ManageAccounts %>"
                                CommandName="EDIT" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="24" />
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfHasChild" runat="server" />
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_Report" runat="server">
                        <asp:TableCell>
                            <%--<div style="padding-left: 400px;">--%>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdAccountReport" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" TabIndex="4" />
                                                <asp:HiddenField ID="hdfReportPK" runat="server" Value='<%# Eval(Resources.DataFieldRes.cfgValue) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,Name%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemRptName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.cfgData)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.cfgData)),30) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="95%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <%-- </div>--%>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="PageAction_Tree" runat="server">
                        <asp:TableCell>
                            <div style="padding-left: 400px;">
                                <div class="treeview scroll-h350">
                                    <%--<asp:Label ID="lblAccounts" runat="server" Text="Accounts" />--%>
                                    <asp:TreeView ID="trvAccounts" runat="server" ShowLines="True" TabIndex="25">
                                        <SelectedNodeStyle Font-Bold="true" />
                                    </asp:TreeView>
                                </div>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse" id="divAdvanceSearch" style="margin-top: 0px;">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>" />
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
                                            <asp:Label ID="lblSearchName" runat="server" Text="<%$ resources:Name%>" AssociatedControlID="txtSearchName"></asp:Label>
                                            <asp:TextBox ID="txtSearchName" runat="server" TabIndex="1" CssClass="input-half"
                                                MaxLength="100"> </asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblSearchDispName" runat="server" Text="<%$resources:Controls,DisplayName%>"
                                                AssociatedControlID="txtSearchDispName"></asp:Label>
                                            <asp:TextBox ID="txtSearchDispName" runat="server" TabIndex="1" CssClass="input-half"
                                                MaxLength="100"> </asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblSearchParent" runat="server" Text="<%$ resources:Parent%>" AssociatedControlID="ddlSearchParent"></asp:Label>
                                            <asp:DropDownList ID="ddlSearchParent" runat="server" CssClass="lbl-53-6perc">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="imbListSearch" runat="server" CssClass="margntop2 margnrgt5 margnbotm0 margnlft4"
                                                CommandName="SEARCH" Width="18px" OnClick="ActionHandler" SkinID="search" TabIndex="15"
                                                ClientIDMode="Static" ToolTip="<%$ Resources:Search %>" />
                                            <asp:ImageButton ID="imbClear" runat="server" CssClass="margntop2 margnrgt7 margnbotm0"
                                                SkinID="cancel" OnClick="ActionHandler" CommandName="CLEAR" Visible="true" ToolTip="<%$ Resources:Clear %>"
                                                Width="18px" TabIndex="16" />
                                            <%--<div class="clear">
                                            </div>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch"></asp:Label>
                                                <asp:Button ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                                    ValidationGroup="Search" OnClick="ActionHandler" TabIndex="11" CommandName="SEARCH"
                                                    SkinID="btnInner-search" />
                                                <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                                    TabIndex="12" OnClick="ActionHandler" CommandName="CLEAR" SkinID="btnInner-cancel-dsd" />
                                            </div>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdAccounts" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" TabIndex="4" />
                                                <asp:HiddenField ID="hdfAccPK" runat="server" Value='<%# Eval(Resources.DataFieldRes.RTC_PK) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,Name%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.RTC_NAME)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.RTC_NAME)),35) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,DisplayName%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccDispName" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.RTC_DISPLAY_NAME)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.RTC_DISPLAY_NAME)),35) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,Parent%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParent" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.RTC_PARENT_TEXT)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.RTC_PARENT_TEXT)),35) %>' />
                                                <asp:HiddenField ID="hdfParent" runat="server" Value='<%# Eval(Resources.DataFieldRes.RTC_PARENT)%> ' />
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,Type%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccType" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.RTC_TYPE_TEXT)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.RTC_TYPE_TEXT)),35) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,IsGroup%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccIsGroup" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.RTC_IS_GROUP).ToString() == "1" ? "Yes" : "No" %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.RTC_IS_GROUP).ToString() == "1" ? "Yes" : "No" %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$resources:Controls,Active%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblActive" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.RTC_ACTIVE).ToString() == "1" ? "Active" : "In Active" %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.RTC_ACTIVE).ToString() == "1" ? "Active" : "In Active" %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCoaName" runat="server" Text="<%$resources:Controls,Name%>" AssociatedControlID="txtCoaName" />
                                            <asp:TextBox ID="txtCoaName" runat="server" MaxLength="50" TabIndex="11" class="select-half" />
                                            <asp:RequiredFieldValidator ID="vrfCoaName" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Account" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtCoaName" ErrorMessage="<%$ resources:Err_AccountName %>" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCoaDispName" runat="server" Text="<%$resources:Controls,DisplayName%>"
                                                AssociatedControlID="txtCoaDispName" />
                                            <asp:TextBox ID="txtCoaDispName" runat="server" MaxLength="50" TabIndex="11" class="input-halfsmall-a" />
                                            <asp:RequiredFieldValidator ID="vrfcoaDispName" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Account" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtCoaDispName" ErrorMessage="<%$ resources:Err_AccountDispName %>" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCoaParent" runat="server" Text="<%$resources:Controls,Parent%>"
                                                AssociatedControlID="ddlParent" />
                                            <asp:DropDownList ID="ddlParent" runat="server" TabIndex="12" class="select-w61per" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCoaType" runat="server" Text="<%$resources:Controls,Type%>" AssociatedControlID="ddlType" />
                                            <asp:DropDownList ID="ddlType" runat="server" class="input-small" TabIndex="13" />
                                            <asp:RequiredFieldValidator ID="vrfCoaType" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="Account" EnableClientScript="true" runat="server"
                                                ControlToValidate="ddlType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AccountType %>" />
                                            <asp:Label ID="lblSequence" runat="server" Text="<%$resources:Controls,DisplayOrder%>"
                                                AssociatedControlID="txtSequence" CssClass="middle-lbl" />
                                            <asp:TextBox ID="txtSequence" runat="server" MaxLength="4" CssClass="lbl-21-5perc numeric"
                                                onkeypress="return isNumberKey(event)" onpaste="return false;" TabIndex="14" />
                                            <asp:RequiredFieldValidator ID="vrfCoaSequence" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Account" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtSequence" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_DisplayOrder %>" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblSplCondition" runat="server" Text="<%$resources:Controls,SpecialCondition%>"
                                                AssociatedControlID="txtSequence" />
                                            <asp:TextBox ID="txtSplCondition" runat="server" MaxLength="50" TabIndex="15" class="input-small" />
                                            <asp:Label ID="lblIsGroup" runat="server" Text="<%$resources:Controls,IsGroup%>"
                                                AssociatedControlID="chkIsGroup" CssClass="lbl-17perc" />
                                            <asp:CheckBox ID="chkIsGroup" runat="server" TabIndex="16" TextAlign="Left" />
                                            <asp:Label ID="lblIsBold" runat="server" Text="<%$resources:Controls,IsBold%>" AssociatedControlID="chkIsBold"
                                                CssClass="lbl-16perc" />
                                            <asp:CheckBox ID="chkIsBold" runat="server" TabIndex="18" TextAlign="Left" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblIsTotal1" runat="server" Text="<%$resources:Controls,IsTotal1%>"
                                                AssociatedControlID="chkIsTotal1" />
                                            <asp:CheckBox ID="chkIsTotal1" runat="server" TabIndex="18" TextAlign="Left" />
                                            <asp:Label ID="lblIsTotal2" runat="server" Text="<%$resources:Controls,IsTotal2%>"
                                                AssociatedControlID="chkIsTotal2" CssClass="lbl-34-7perc" />
                                            <asp:CheckBox ID="chkIsTotal2" runat="server" TabIndex="18" TextAlign="Left" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblCoaDesc" runat="server" Text="<%$resources:Controls,Description%>"
                                                AssociatedControlID="txtCoaDesc" />
                                            <asp:TextBox ID="txtCoaDesc" runat="server" TextMode="MultiLine" MaxLength="500"
                                                TabIndex="17" CssClass="multiline-2col input-halfsmall-a" onkeydown="limitText(this,500);"
                                                onkeyup="limitText(this,500);" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblActive" runat="server" Text="<%$resources:Controls,Active%>" AssociatedControlID="chkActive" />
                                            <asp:CheckBox ID="chkActive" runat="server" TabIndex="18" CssClass="style-none w50perc"
                                                Checked="true" TextAlign="Left" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="Account" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
