<%@ Page Title="<%$ Resources:Title %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" 
    CodeBehind="MaterialIssueRateAdjustment.aspx.cs" Inherits="ERPSMS_v01.StoreManagement.MaterialIssueRateAdjustment" %>

<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            DateInit();
            GrandScriptUtils.MakeAutoCompleteDDLNEW("txtTransNoSearch", url, "hdfTransNoSearch", true, true, 0, "MIRATEADJUSTNO");
        }
        function DateInit() {
            //<summary>function used to make datepicker</summary>
            GrandScriptUtils.DatePickerCommon("txtTransDate");
            GrandScriptUtils.DatePickerCommon("txtTranDateSearch");
            GrandScriptUtils.DatePickerCommon("txtFromDate");
            GrandScriptUtils.DatePickerCommon("txtToDate");
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
                                    <%--<li id="pnlEdit">
                                        <asp:Button runat="server" TabIndex="52" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>--%>
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
                                            OnClick="ActionHandler" OnClientClick="javascript: return ValidatePageNow('mira');"
                                            ValidationGroup="wo" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="76" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript: return ValidatePageNow('mira');"
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

                                            <asp:Label runat="server" ID="lblTransSearch" Text="<%$ resources:TransNo %>" 
                                                AssociatedControlID="txtTransNoSearch" CssClass="lbl-18perc"></asp:Label>
                                            <asp:TextBox ID="txtTransNoSearch" runat="server" TabIndex="3" CssClass="lbl-26perc" MaxLength="17"></asp:TextBox>
                                            <asp:HiddenField ID="hdfTransNoSearch" runat="server" Value="0" />

                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                TabIndex="8" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                                OnClick="ActionHandler" CommandName="SEARCH" />
                                            <asp:ImageButton ID="btnClear" runat="server" TabIndex="9" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                                ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                                                OnClick="ActionHandler" CommandName="CLEAR" />
                                        </div>
                                    </td>
                                    <td>                                        
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap">
                                <asp:GridView runat="server" ID="grdTransList" Width="100%" PageSize="<%$ resources:PageSize%>"
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
                                                <asp:HiddenField ID="hdfIRH_PK" runat="server" Value='<%# Eval("IRH_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TransDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMIRTransDate" runat="server" Text='<%# Eval("IRH_DATE", Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:FromDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("IRH_FROM_DT", Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="29%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ToDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToDate" runat="server" Text='<%# Eval("IRH_TO_DATE", Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TransNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMIRTransNo" runat="server" Text='<%# string.IsNullOrEmpty(Eval("IRH_NO").ToString()) ? "[NEW]" : Eval("IRH_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
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
                                                    <asp:Label ID="lblTransNo" runat="server" TabIndex="6" CssClass="input-small margnbotm0"></asp:Label>

                                                    <asp:Label ID="lblTransDate" runat="server" Text='<%$ Resources:TransDate%>'
                                                        AssociatedControlID="txtTransDate"></asp:Label>
                                                    <asp:TextBox ID="txtTransDate" runat="server" TabIndex="12" onkeydown="return CheckKey(event)"
                                                        onpaste="return false;" CssClass="input-small" Enabled="false"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                     <asp:Label ID="lblFromDate" runat="server" Text='<%$ Resources:FromDate%>'
                                                        AssociatedControlID="txtFromDate" CssClass="lbl-20perc"></asp:Label>
                                                    <asp:TextBox ID="txtFromDate" runat="server" TabIndex="12" onkeydown="return CheckKey(event)"
                                                        onpaste="return false;" CssClass="input-small"></asp:TextBox>

                                                     <asp:Label ID="lblToDate" runat="server" Text='<%$ Resources:ToDate%>'
                                                        AssociatedControlID="txtToDate" CssClass="lbl-9perc"></asp:Label>
                                                    <asp:TextBox ID="txtToDate" runat="server" TabIndex="12" onkeydown="return CheckKey(event)"
                                                        onpaste="return false;" CssClass="input-small"></asp:TextBox>
                                                    
                                                    <asp:Button runat="server" ID="btnLoad" CommandName="LOADFROMTEMPLATE" ValidationGroup="mira"
                                                        OnClick="ActionHandler" Text="<%$resources:Controls,LoadFromTemplate %>" CommandArgument="PageAction_Entry"
                                                        SkinID="btnInner-journalize" ToolTip="<%$resources:Controls,LoadFromTemplate %>"
                                                        OnClientClick="javascript:ValidatePageNow('mira')" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div>
                                    <h1 class="search-colapse-normal"><%= GetLocalResourceObject("MIDetails").ToString() + " :"%></h1>
                                    <div class="gridwrap scroll-container">
                                        <asp:GridView ID="grdMaterialIssue" runat="server" AutoGenerateColumns="False" PageSize="<%$ resources:PageSize %>"
                                            AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                            Width="125%" ShowFooter="true" FooterStyle-CssClass="emptyfooter">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:ItemCode %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>'
                                                            ToolTip='<%# Eval("ItemCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ItemName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>'
                                                            ToolTip='<%# Eval("ItemName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:MINo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMINo" runat="server" Text='<%# Eval("MaterialIssueNo") %>'
                                                            ToolTip='<%# Eval("MaterialIssueNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:MIDate %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMIDate" runat="server" Text='<%# Eval("MaterialIssueDate", Resources.Constants.DateFormatGridExpanded) %>'
                                                            ToolTip='<%# Eval("MaterialIssueDate", Resources.Constants.DateFormatGridExpanded) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:MIRate %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMIRate" runat="server" Text='<%# Eval("Rate") %>'
                                                            ToolTip='<%# Eval("Rate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" Wrap="true" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:MINewRate %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMINewRate" runat="server" Text='<%# Eval("GRNRate") %>'
                                                            ToolTip='<%# Eval("GRNRate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" Wrap="true" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:GRNNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGRNNo" runat="server" Text='<%# Eval("GRNNo") %>'
                                                            ToolTip='<%# Eval("GRNNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:GRNRate %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGRNRate" runat="server" Text='<%# Eval("GRNRate") %>'
                                                            ToolTip='<%# Eval("GRNRate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" Wrap="true" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:DeptName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDeptName" runat="server" Text='<%# Eval("DepartmentName") %>'
                                                            ToolTip='<%# Eval("DepartmentName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:VoucherNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVoucherNo" runat="server" Text='<%# Eval("VoucherNo") %>'
                                                            ToolTip='<%# Eval("VoucherNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" Wrap="true" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:VoucherAmt %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVoucherAmt" runat="server" Text='<%# Eval("VoucherAmt") %>'
                                                            ToolTip='<%# Eval("VoucherAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" Wrap="true" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:TotalInvAmt %>" ItemStyle-HorizontalAlign="Right">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTotalInvAmt" runat="server" Text='<%# Eval("TotalInvAmt") %>'
                                                            ToolTip='<%# Eval("TotalInvAmt") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="8%" Wrap="true" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div id="divWkfSubmit" style="display: none;">
                                    <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                                    <uc1:workflowusercomments id="ucrWrkf" runat="server" validationgroup="mira" />
                                </div>

                                <div id="diverror" style="display: none">
                                    <%--Use this label to bind the server errors--%>
                                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                                    <asp:ValidationSummary ID="vsPage" ValidationGroup="mira" runat="server" />
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
