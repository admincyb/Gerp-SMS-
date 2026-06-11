<%@ Page Title="<%$ resources:TaskTracker%>" Language="C#" MasterPageFile="~/DOC.Master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="TaskHome.aspx.cs"
    Inherits="HRMS.TaskTracker.TaskHome" Theme="ClassicEdoc" %>

<%@ Register Src="UserControls/TaskCreatePopUp.ascx" TagName="PopUp" TagPrefix="uc2" %>
<%@ Register Src="UserControls/TaskStatusPopUp.ascx" TagName="PopUp" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtAssignedUser", url, "hdfUserPK", true, true, "ASSIGNTASKUSER");
            btnEditHide();
            ShowCompletionDate();
            GrandScriptUtils.MakeAutoCompleteDDL("txtAssignedUserPendingSearch", url, "hdfUserPKPendingSearch", true, true, "ASSIGNTASKUSER");

            setSearchVisibilty();
        }        
        function toggleHistory() {
            var title = $("#imgHistoryShow").attr("title");
            var src;
            if (title == 'Hide') {
                $("#imgHistoryShow").attr("title", 'Show');
                src = '../Images/Classic/Icons/arrow-colapse-inactive.png'
                $("#divHistory").slideUp();
            }
            else {
                $("#imgHistoryShow").attr("title", 'Hide');
                src = '../Images/Classic/Icons/arrow-colapse-active.png'
                $("#divHistory").slideDown();
            }
            $("#imgHistoryShow").attr("src", src);
        }
        //To Show-Hide edit button for specific users
        function btnEditHide() {
            var a = $("[id$=hdfCurrentUserPK]").val();
            var b = $("[id$=hdfassignedUserPK]").val();
            if (a == b) {
                $('[id$=btnEdit]').show();
                $('[id$=btnDelete]').show();
            }
            else {
                $('[id$=btnEdit]').hide();
                $('[id$=btnDelete]').hide();
            }
        }
        //.green-icon,.red-icon,.orange-icon,.yellow-icon,.grey-icon
        function ShowCompletionDate() {
            if ($("[id$=hdfCurrTaskStatus]").val() == 2) {
                $('[id$=divCmpDate]').show();
                $('[id$=divEstDate]').hide();
            }
            else {
                $('[id$=divCmpDate]').hide();
                $('[id$=divEstDate]').show();
            }
        }

        function toggleMyTaskSearch() {
            var title = $("#imgMyTaskSearchShow").attr("title");
            var src;
            if (title == 'Hide') {
                $("#imgMyTaskSearchShow").attr("title", 'Show');
                src = '../Images/Classic/Icons/arrow-colapse-inactive.png'
                $("#divMyTaskSearch").hide();
                $("#hdfMyTaskSearchVisible").val("0");
            }
            else {
                $("#imgMyTaskSearchShow").attr("title", 'Hide');
                src = '../Images/Classic/Icons/arrow-colapse-active.png'
                $("#divMyTaskSearch").show();
                $("#hdfMyTaskSearchVisible").val("1");
            }
            $("#imgMyTaskSearchShow").attr("src", src);
            GrandScriptUtils.MakeAutoCompleteDDL("txtAssignedUser", url, "hdfUserPK", true, true, "ASSIGNTASKUSER");
        }

        function togglePendingTaskSearch() {
            var title = $("#imgPendingTaskSearchShow").attr("title");
            var src;
            if (title == 'Hide') {
                $("#imgPendingTaskSearchShow").attr("title", 'Show');
                src = '../Images/Classic/Icons/arrow-colapse-inactive.png'
                $("#divPendingTaskSearch").hide();
                $("#hdfPendingTaskSearchVisible").val("0");
            }
            else {
                $("#imgPendingTaskSearchShow").attr("title", 'Hide');
                src = '../Images/Classic/Icons/arrow-colapse-active.png'
                $("#divPendingTaskSearch").show();
                $("#hdfPendingTaskSearchVisible").val("1");
            }
            $("#imgPendingTaskSearchShow").attr("src", src);
            GrandScriptUtils.MakeAutoCompleteDDL("txtAssignedUserPendingSearch", url, "hdfUserPKPendingSearch", true, true, "ASSIGNTASKUSER");
        }

        function setSearchVisibilty() {
            var titleMyTaskSearch = $("#imgMyTaskSearchShow").attr("title");
            var titlePendingTaskSearch = $("#imgPendingTaskSearchShow").attr("title");
            if ($("#hdfMyTaskSearchVisible").val() == '1') {
                if (titleMyTaskSearch == 'Show') {
                    toggleMyTaskSearch();
                }
            }
            else if ($("#hdfMyTaskSearchVisible").val() != '1') {
                if (titleMyTaskSearch == 'Hide') {
                    toggleMyTaskSearch();
                }
            }

            if ($("#hdfPendingTaskSearchVisible").val() == '1') {
                if (titlePendingTaskSearch == 'Show') {
                    togglePendingTaskSearch();
                }
            }
            else if ($("#hdfPendingTaskSearchVisible").val() != '1') {
                if (titlePendingTaskSearch == 'Hide') {
                    togglePendingTaskSearch();
                }
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <%--<asp:Label runat="server" ID="lblBreadCrum" CssClass=" padgtop2 txtbx" style="font-family: 'alegreya_sansthin' , sans-serif!important;    font-size: 40px!important;">Task Tracker</asp:Label>--%>
                                    <li><span style="font-family: 'alegreya_sansregular' , sans-serif; font-size: 20px;
                                        margin-top: 3px!important; padding-left: 0px!important;">Task Tracker</span></li>
                                </ul>
                                <div class="top-search-main ">
                                </div>
                                <ul runat="server" id="pnlList">
                                    <li>
                                        <asp:ImageButton ID="imgbtnNewTaskListPage" Text="<%$Resources:NewTask %>" EnableViewState="False"
                                            ToolTip="<%$resources:NewTask %>" OnClick="ActionHandler" Style="margin: 3px!important;"
                                            CommandName="NEWTASKLISTPAGE" SkinID="btnInner-New" runat="server" /></li>
                                    <li>
                                        <asp:Label ID="Label2" runat="server" CssClass="margnbotm0 margn-rgt0">
                                    <img alt="" src="../Images/ClassicEdoc/Icons/search-separtn.png" class="margntop4 margnbotm0 margn-rgt0" />
                                        </asp:Label>
                                    </li>
                                    <li>
                                        <asp:ImageButton runat="server" ID="imgbtnDetailListPage" SkinID="btnInner-View"
                                            Text="<%$Resources:Detail %>" EnableViewState="False" ToolTip="<%$resources:Detail %>"
                                            OnClick="ActionHandler" Style="margin: 3px!important;" CommandName="EDITTASKLISTPAGE" />
                                    </li>
                                </ul>
                                <ul id="pnlEntry" runat="server">
                                    <li>
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" /></li>
                                    <li>
                                        <asp:Button runat="server" ID="btnEdit" SkinID="btnInner-edit-dsd" Text="<%$Resources:Edit %>"
                                            EnableViewState="False" ToolTip="<%$resources:Edit %>" CommandName="SHOWTASKPOPUPEDIT"
                                            OnClick="ActionHandler" /></li>
                                    <li>
                                        <asp:Button runat="server" ID="btnAddSubTask" SkinID="btnInner-subtask" Text="<%$Resources:AddSubTask %>"
                                            EnableViewState="False" ToolTip="<%$resources:AddSubTask %>" CommandName="SHOWTASKPOPUPADDSUB"
                                            OnClick="ActionHandler" /></li>
                                    <li>
                                        <asp:Button ID="btnUpdateStatus" runat="server" SkinID="btnInner-Save" Text="<%$Resources:UpdateStatus %>"
                                            EnableViewState="False" ToolTip="<%$resources:UpdateStatus %>" CommandName="SHOWTASKPOPUPUPDATESTATUS"
                                            OnClick="ActionHandler" /></li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Cancel %>"
                                            EnableViewState="False" ToolTip="<%$resources:Cancel %>" CommandName="BACKTOLISTPAGE"
                                            Style="margin-right: 0px!important;" OnClick="ActionHandler" /></li>
                                </ul>
                                <asp:HiddenField runat="server" ID="hdfDefaultSubmit" />
                                <asp:HiddenField runat="server" ID="hdfIsExpandSearch" Value="0" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfMyTaskSearchVisible" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdfPendingTaskSearchVisible" runat="server" ClientIDMode="Static" />
                <div id="divListPage" runat="server">
                    <div class="tab-container" id="divTabContainer" runat="server" style="padding-right: 0px;">
                        <ul id="tab-menu">
                            <li><span id="spnPendingWorks" runat="server" purpose="TabListPage">
                                <asp:LinkButton ID="lbnPendingWorks" runat="server" Text="<%$ resources:PendingWorks %>"
                                    CommandName="PENDINGWORKS" OnClick="ActionHandler" TabIndex="2" ToolTip="<%$ resources:PendingWorks %>" />
                            </span></li>
                            <li><span id="spnMyTasks" runat="server" purpose="TabListPage">
                                <asp:LinkButton ID="lbnMyTasks" runat="server" Text="<%$ resources:MyTasks %>" CommandName="MYTASKS"
                                    OnClick="ActionHandler" TabIndex="1" ToolTip="<%$ resources:MyTasks %>" />
                            </span></li>
                            <li><span id="spnCompletedTasks" runat="server" purpose="TabListPage">
                                <asp:LinkButton ID="lbnCompletedTasks" runat="server" Text="<%$ resources:CompletedTasks %>"
                                    CommandName="COMPLETEDTASKS" OnClick="ActionHandler" TabIndex="3" ToolTip="<%$ resources:CompletedTasks %>" />
                            </span></li>
                        </ul>
                    </div>
                    <div>
                        <div id="divMyTaskList" runat="server">
                            <table class="h4-heading margntop1 margnbotm0">
                                <tr>
                                    <td style="padding: 3px 0px 3px 13px;">
                                        <h4>
                                            Advance Search</h4>
                                    </td>
                                    <td style="width: 5px;">
                                        <img id="imgMyTaskSearchShow" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                            alt="Hide" title="Hide" style="cursor: pointer;" onclick="toggleMyTaskSearch();"
                                            class="margntop5 margnbotm0" />
                                    </td>
                                </tr>
                            </table>
                            <div id="divMyTaskSearch">
                                <table class="table-3devide margntop4">
                                    <tr>
                                        <td>
                                            <div class="div2col-P">
                                                <asp:Label runat="server" ID="lbAssign" Text="<%$ resources:Controls,AssignTo %>" CssClass="margnbotm0"
                                                    AssociatedControlID="txtAssignedUser"></asp:Label>
                                                <asp:TextBox ID="txtAssignedUser" runat="server" CssClass="margnbotm0"></asp:TextBox>
                                                <asp:HiddenField ID="hdfUserPK" runat="server" Value="0" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-P">
                                                <asp:Label runat="server" ID="lblShowCompleted" Text="<%$ resources:Controls,ShowCompleted %>"
                                                    AssociatedControlID="chkShowCompleted" Width="120px" CssClass="margnbotm0"></asp:Label>
                                                <asp:CheckBox ID="chkShowCompleted" runat="server" Checked="true" TabIndex="8" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-P">
                                                <asp:Button ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                    ToolTip="<%$ resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="9"
                                                    Style="margin-bottom: 0px!important;" CommandName="SEARCH" SkinID="btnInner-search" />
                                                <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="10"
                                                    Style="margin-bottom: 0px!important;" OnClick="ActionHandler" CommandName="CLEAR"
                                                    SkinID="btnInner-cancel-dsd" ToolTip="<%$ resources:Controls,Clear %>" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap">
                                <%-- AllowPaging="true" PageSize="<%$ resources:PageSize %>"--%>
                                <asp:GridView runat="server" ID="grdMyTasks" Width="100%" OnPageIndexChanging="ActionHandler"
                                    AllowSorting="True" OnSorting="ActionHandler" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSelectedIndexChanging="ActionHandler" AutoGenerateColumns="False" AutoGenerateSelectButton="False"
                                    OnDataBound="ActionHandler" OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" AutoPostBack="true" OnCheckedChanged="ActionHandler" />
                                                <asp:HiddenField ID="hfTaskPK" runat="server" Value='<%#Eval("TSK_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskDate" Text='<%# Eval("TSK_DATE", "{0:dd-MM-yyyy}") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskName" runat="server" ToolTip='<%# Eval("TSK_NAME") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_NAME") ,35) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:No%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskNo" ToolTip='<%# Eval("TSK_NO") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_NO") ,14) %>'
                                                    runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Status%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval("TSK_TRX_STATUS_TEXT"))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_TRX_STATUS_TEXT") ,14) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssignedTo%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssignedBy" runat="server" ToolTip='<%# Eval("TSK_ASSIGN_TO_TEXT") %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_ASSIGN_TO_TEXT") ,21) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ExpCmpDate%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpComDate" runat="server" ToolTip='<%# Eval("TSK_EXP_DATE", "{0:dd-MM-yyyy}") %>'
                                                    Text='<%# Eval("TSK_EXP_DATE", "{0:dd-MM-yyyy}") %>' CssClass="gg" />
                                                <asp:Image ID="imgDateFlag" SkinID="DateFlag" runat="server" Visible='<%# Convert.ToDateTime(Eval("TSK_EST_DATE"))>Convert.ToDateTime(Eval("TSK_EXP_DATE")) ? true:false %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EstCompDate%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstCompDate" runat="server" ToolTip='<%# Eval("TSK_EST_DATE", "{0:dd-MM-yyyy}") %>'
                                                    Text='<%# Eval("TSK_EST_DATE", "{0:dd-MM-yyyy}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <%-- <asp:Image ID="imgListFlg" runat="server" />--%>
                                                <asp:Button ID="imgListFlg" runat="server" OnClientClick="javascript:return false;"
                                                    ToolTip="" />
                                                <asp:HiddenField ID="hfImageFlag" runat="server" Value='<%# Eval("TSK_IND_FLAG") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPagingMyTask" runat="server" />
                            </div>
                        </div>
                        <div id="divPendingTaskList" runat="server">
                            <table class="h4-heading margntop1 margnbotm0">
                                <tr>
                                    <td style="padding: 3px 0px 3px 13px;">
                                        <h4>
                                            Advance Search</h4>
                                    </td>
                                    <td style="width: 5px;">
                                        <img id="imgPendingTaskSearchShow" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                            alt="Hide" title="Hide" style="cursor: pointer;" onclick="togglePendingTaskSearch();"
                                            class="margntop5 margnbotm0" />
                                    </td>
                                </tr>
                            </table>
                            <div id="divPendingTaskSearch">
                                <table class="table-2devide margntop4">
                                    <tr>
                                        <td>
                                            <%--style="padding-top: 20px;"--%>
                                            <div class="div2col-P">
                                                <asp:Label runat="server" ID="lbAssignToPendingSearch" Text="<%$ resources:Controls,AssignedBy %>" CssClass="margnbotm0"
                                                    AssociatedControlID="txtAssignedUserPendingSearch"></asp:Label>
                                                <asp:TextBox ID="txtAssignedUserPendingSearch" runat="server" CssClass="margnbotm0"></asp:TextBox>
                                                <asp:HiddenField ID="hdfUserPKPendingSearch" runat="server" Value="0" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <%--style="padding-top: 20px;"--%>
                                            <div class="div2col-P">
                                                <asp:Button ID="btnSearchPendingSearch" runat="server" Text="<%$ resources:Controls,Search %>" CssClass="margnbotm0"
                                                    ToolTip="<%$ resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="9"
                                                    Style="margin-bottom: 0px;" CommandName="SEARCH" SkinID="btnInner-search" />
                                                <asp:Button ID="btnClearPendingSearch" runat="server" Text="<%$ resources:Controls,Clear %>" CssClass="margnbotm0"
                                                    TabIndex="10" OnClick="ActionHandler" CommandName="CLEAR" SkinID="btnInner-cancel-dsd"
                                                    Style="margin-bottom: 0px;" ToolTip="<%$ resources:Controls,Clear %>" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <%--PageSize="<%$ resources:PageSize %>" AllowPaging="true"--%>
                                <asp:GridView runat="server" ID="grdPendingTasks" Width="100%" OnPageIndexChanging="ActionHandler"
                                    AllowSorting="True" OnSorting="ActionHandler" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSelectedIndexChanging="ActionHandler" AutoGenerateColumns="False" AutoGenerateSelectButton="False"
                                    OnDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" OnCheckedChanged="ActionHandler"
                                                    AutoPostBack="true" />
                                                <asp:HiddenField ID="hfTaskPK" runat="server" Value='<%#Eval("TSK_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskDate" Text='<%# Eval("TSK_DATE", "{0:dd-MM-yyyy}") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskName" runat="server" ToolTip='<%# Eval("TSK_NAME") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_NAME"), 35) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:No%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskNo" ToolTip='<%# Eval("TSK_NO") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_NO"), 14) %>'
                                                    runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Status%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval("TSK_TRX_STATUS_TEXT"))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_TRX_STATUS_TEXT") ,14) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssignedBy%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssignedBy" runat="server" ToolTip='<%# Eval("TSK_ASSIGNED_BY_TEXT") %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_ASSIGNED_BY_TEXT") ,21) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ExpCmpDate%>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblExpComDate" runat="server" ToolTip='<%# Eval("TSK_EXP_DATE", "{0:dd-MM-yyyy}") %>'
                                                    Text='<%# Eval("TSK_EXP_DATE", "{0:dd-MM-yyyy}") %>' />--%>
                                                <asp:Label ID="lblExpComDate" runat="server" ToolTip='<%# Eval("TSK_EXP_DATE", "{0:dd-MM-yyyy}") %>'
                                                    Text='<%# Eval("TSK_EXP_DATE", "{0:dd-MM-yyyy}") %>' CssClass="gg" />
                                                <asp:Image ID="imgDateFlag" SkinID="DateFlag" runat="server" Visible='<%# Convert.ToDateTime(Eval("TSK_EST_DATE"))>Convert.ToDateTime(Eval("TSK_EXP_DATE")) ? true:false %>' />
                                                <%-- <asp:Label Text="*" runat="server" Visible='<%# Convert.ToDateTime(Eval("TSK_EST_DATE"))>Convert.ToDateTime(Eval("TSK_EXP_DATE")) ? true:false %>' />--%>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EstCompDate%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstCompDate" runat="server" ToolTip='<%# Eval("TSK_EST_DATE", "{0:dd-MM-yyyy}") %>'
                                                    Text='<%# Eval("TSK_EST_DATE", "{0:dd-MM-yyyy}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgListFlg" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField ID="hfImageFlag" runat="server" Value='<%# Eval("TSK_IND_FLAG") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPagingPendingTask" runat="server" />
                            </div>
                        </div>
                        <div id="divCompletedTaskList" runat="server">
                            <div class="gridwrap">
                                <%--PageSize="<%$ resources:PageSize %>" AllowPaging="true"--%>
                                <asp:GridView runat="server" ID="grdCompletedTasks" Width="100%" OnPageIndexChanging="ActionHandler"
                                    AllowSorting="True" OnSorting="ActionHandler" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSelectedIndexChanging="ActionHandler" AutoGenerateColumns="False" OnDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" OnCheckedChanged="ActionHandler"
                                                    AutoPostBack="true" />
                                                <asp:HiddenField ID="hfTaskPK" runat="server" Value='<%#Eval("TSK_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskDate" Text='<%# Eval("TSK_DATE", "{0:dd-MM-yyyy}") %>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskName" runat="server" ToolTip='<%# Eval("TSK_NAME") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_NAME") ,35) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:No%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskNo" ToolTip='<%# Eval("TSK_NO") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_NO") ,14) %>'
                                                    runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Status%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval("TSK_TRX_STATUS_TEXT"))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_TRX_STATUS_TEXT") ,14) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssignedBy%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAssignedBy" runat="server" ToolTip='<%# Eval("TSK_ASSIGNED_BY_TEXT") %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_ASSIGNED_BY_TEXT") ,21) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ExpCmpDate%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpComDate" runat="server" ToolTip='<%# Eval("TSK_EXP_DATE", "{0:dd-MM-yyyy}") %>'
                                                    Text='<%# Eval("TSK_EXP_DATE", "{0:dd-MM-yyyy}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EstCompDate%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEstCompDate" runat="server" ToolTip='<%# Eval("TSK_EST_DATE", "{0:dd-MM-yyyy}") %>'
                                                    Text='<%# Eval("TSK_EST_DATE", "{0:dd-MM-yyyy}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgListFlg" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField ID="hfImageFlag" runat="server" Value='<%# Eval("TSK_IND_FLAG") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPagingCompletedTask" runat="server" />
                            </div>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="aupHistoryList" runat="server">
                        <ContentTemplate>
                            <div id="divHistoryListPage" runat="server">
                                <div class="listdetails">
                                    <h1 class="title" id="H1">
                                        <asp:Literal ID="Literal3" runat="server" Text="<%$ resources:History%>" />
                                    </h1>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdHistoryListPage" Width="100%" AllowSorting="True"
                                            EmptyDataRowStyle-CssClass="emptytable" AutoGenerateColumns="False">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:StatusDate%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" Text='<%# Eval("TKS_DATE", "{0:dd-MM-yyyy}") %>' runat="server" />
                                                        <asp:HiddenField ID="hfTksPK" runat="server" Value='<%#Eval("TKS_PK") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:UpdatedBy%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUpdatedBy" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval("TKS_UPDATED_BY_TEXT"))%>'
                                                            Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TKS_UPDATED_BY_TEXT"),17) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Name%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" ToolTip='<%# Eval("TSK_NAME") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TSK_NAME"),28) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Remarks%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" ToolTip='<%# Eval("TKS_REMARKS") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TKS_REMARKS"),23) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Status%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" ToolTip='<%# Eval("TKS_TRX_STATUS_TEXT") %>'
                                                            Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TKS_TRX_STATUS_TEXT"),20) %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ExpCmpDate%>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExpCmpDate" runat="server" ToolTip='<%# Eval("TKS_EXP_DATE", "{0:dd-MM-yyyy}") %>'
                                                            Text='<%# Eval("TKS_EXP_DATE", "{0:dd-MM-yyyy}") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13.9%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="0.1%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="divDetailsPage" runat="server">
                    <div>
                        <asp:HiddenField ID="hfCurrentTask" runat="server" />
                        <asp:HiddenField ID="hfCurrentTaskTypeEnum" runat="server" />
                        <div class="clear">
                        </div>
                        <div class="breadcrumb">
                            <asp:DataList ID="dlBreadCrumb" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                OnItemCommand="ActionHandler">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbBreadCrumb" Text='<%#  ERP.Utilities.CommonFunctions.GetShortString( Eval("Name") ,15) %>'
                                        runat="server" ToolTip='<%#Eval("Name") %>' CommandName="BREADCRUMBCLICK" />
                                    <asp:HiddenField ID="hfBreadCrumbTaskId" runat="server" Value='<%#Eval("TaskId") %>' />
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    ►</SeparatorTemplate>
                                <SeparatorStyle CssClass="margntop3" />
                            </asp:DataList>
                        </div>
                        <div class="notify">
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="head-detail">
                                            <asp:Label ID="lblDate" runat="server" Text="<%$ resources:Date%>" AssociatedControlID="txtDate" />:
                                            <asp:TextBox ID="txtDate" runat="server" Enabled="false" CssClass="txtbx"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="head-detail">
                                            <asp:Label ID="lblNo" runat="server" Text="<%$ resources:No%>" AssociatedControlID="txtNo" />:
                                            <asp:TextBox ID="txtNo" runat="server" Enabled="false" CssClass="txtbx"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="head-detail">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:ExpCmpDate%>" AssociatedControlID="txtExpCompleteDate" />:
                                            <asp:TextBox ID="txtExpCompleteDate" runat="server" Enabled="false" CssClass="txtbx"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="divEstDate" class="head-detail">
                                            <asp:Label ID="lbEstDate" runat="server" Text="<%$ resources:EstCompDate%>" AssociatedControlID="txtEstimatedDate" />:
                                            <asp:TextBox ID="txtEstimatedDate" runat="server" Enabled="false" CssClass="txtbx"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <div id="divCmpDate" class="head-detail">
                                            <asp:Label ID="lblCompletedDate" runat="server" Text="<%$ resources:Completed%>"
                                                AssociatedControlID="txtCompletedDate" />:
                                            <asp:TextBox ID="txtCompletedDate" runat="server" Enabled="false" CssClass="txtbx"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="head-detail">
                                            <asp:Label ID="Label3" runat="server" Text="<%$ resources:TaskCategory%>" AssociatedControlID="txtCategory" />:
                                            <asp:TextBox ID="txtCategory" runat="server" Enabled="false" CssClass="txtbx"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="head-detail">
                                            <asp:Label ID="Label5" runat="server" Text="<%$ resources:Controls,Status%>" AssociatedControlID="txtStatus" />:
                                            <asp:TextBox ID="txtStatus" runat="server" Enabled="false" CssClass="txtbx"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="head-detail-single">
                                            <asp:Label ID="Label4" runat="server" Text="<%$ resources:Controls,Name%>" AssociatedControlID="txtName" />:
                                            <asp:TextBox ID="txtName" runat="server" Enabled="false" CssClass="name txtbx"></asp:TextBox>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="head-detail-single">
                                            <asp:Label ID="Label6" runat="server" Text="<%$ resources:Controls,Description%>"
                                                AssociatedControlID="txtDescription" />:
                                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" MaxLength="500"
                                                TabIndex="19" CssClass="descptn multiline-2line" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"
                                                Enabled="false" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="head-detail">
                                            <asp:Label ID="Label7" runat="server" Text="<%$ resources:Controls,AssignedTo%>"
                                                AssociatedControlID="txtAssignedTo" />:
                                            <asp:TextBox ID="txtAssignedTo" runat="server" Enabled="false" CssClass="txtbx"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="listdetails">
                            <h1 class="title">
                                <asp:Literal ID="Literal2" runat="server" Text="<%$ resources:SubTask%>" />
                            </h1>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdSubTask" Width="100%" AllowSorting="True" OnSorting="ActionHandler"
                                    EmptyDataRowStyle-CssClass="emptytable" AutoGenerateColumns="False" OnRowCommand="ActionHandler"
                                    OnDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%>">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hfTaskPK" runat="server" Value='<%#Eval("TSK_PK") %>' />
                                                <asp:Label ID="lblTaskDate" runat="server" ToolTip='<%# Eval("TSK_DATE") %>' Text='<%# Eval("TSK_DATE", "{0:dd-MM-yyyy}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:TaskNo%>">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbnTaskNo" Text='<%# Eval("TSK_NO") %>' runat="server" CommandName="SubTaskClick"
                                                    Style="text-decoration: underline; margin: 4px 0px 4px 0px; vertical-align: middle !important;" />
                                                <asp:Image ID="imgChild" SkinID="SubTask" runat="server" Visible='<%# ((int)Eval("TSK_CHILD_COUNT"))>0 ? true:false %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" VerticalAlign="Middle" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Name%>">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblTaskName" runat="server" ToolTip='<%# Eval("TSK_NAME") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_NAME") ,28) %>' />--%>
                                                <asp:LinkButton ID="lbnTaskNo" runat="server" CommandName="SubTaskClick" Style="text-decoration: underline;
                                                    margin: 4px 0px 4px 0px; vertical-align: middle !important;" ToolTip='<%# Eval("TSK_NAME") %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_NAME") ,45) %>' />
                                                <asp:Image ID="imgChild" SkinID="SubTask" runat="server" Visible='<%# ((int)Eval("TSK_CHILD_COUNT"))>0 ? true:false %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="37%" VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AssignedTo%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskCategory" runat="server" ToolTip='<%# Eval("TSK_ASSIGN_TO_TEXT") %>'
                                                    Text='<%# Eval("TSK_ASSIGN_TO_TEXT") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval("TSK_TRX_STATUS_TEXT"))%>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString( Eval("TSK_TRX_STATUS_TEXT"),20) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ExpCmpDate%>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpCmpDate" runat="server" ToolTip='<%# Eval("TSK_EXP_DATE") %>'
                                                    Text='<%# Eval("TSK_EXP_DATE", "{0:dd-MM-yyyy}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Button ID="imgListFlg" runat="server" OnClientClick="javascript:return false;" />
                                                <asp:HiddenField ID="hfImageFlag" runat="server" Value='<%# Eval("TSK_IND_FLAG") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                        <div class=" listdetails">
                            <h1 class="title" id="HistoryHeading">
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ resources:History%>" />
                                <img id="imgHistoryShow" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="Hide"
                                    title="Hide" style="cursor: pointer" onclick="toggleHistory();" />
                            </h1>
                            <div id="divHistory">
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdHistory" Width="100%" OnPageIndexChanging="ActionHandler"
                                        AllowSorting="True" OnSorting="ActionHandler" EmptyDataRowStyle-CssClass="emptytable"
                                        AutoGenerateColumns="False">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:StatusDate%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" Text='<%# Eval("TKS_DATE", "{0:dd-MM-yyyy}") %>' runat="server" />
                                                    <asp:HiddenField ID="hfTksPK" runat="server" Value='<%#Eval("TKS_PK") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:UpdatedBy%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUpdatedBy" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetEncodedString(Eval("TKS_UPDATED_BY_TEXT"))%>'
                                                        Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TKS_UPDATED_BY_TEXT"),17) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Name%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" ToolTip='<%# Eval("TSK_NAME") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TSK_NAME"),28) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="22%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Remarks%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" ToolTip='<%# Eval("TKS_REMARKS") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TKS_REMARKS"),23) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Status%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" ToolTip='<%# Eval("TKS_TRX_STATUS_TEXT") %>'
                                                        Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("TKS_TRX_STATUS_TEXT"),20) %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ExpCmpDate%>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblExpCmpDate" runat="server" ToolTip='<%# Eval("TKS_EXP_DATE", "{0:dd-MM-yyyy}") %>'
                                                        Text='<%# Eval("TKS_EXP_DATE", "{0:dd-MM-yyyy}") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="13%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <ItemStyle Width="3%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <asp:HiddenField ID="hdfassignedUserPK" runat="server" Value="" />
                <asp:HiddenField ID="hdfCurrentUserPK" runat="server" Value="" />
                <asp:HiddenField ID="hdfCurrTaskStatus" runat="server" Value="" />
                <div id="divPopUpTask" style="display: none">
                    <uc2:PopUp ID="ucPopUpTask" runat="server" OnTaskSave="ucPopUpTask_TaskSave" />
                </div>
                <div id="divPopUpStatus" style="display: none">
                    <uc3:PopUp ID="ucPopUpStatus" runat="server" OnStatusUpdate="ucPopUpStatus_StatusUpdate" />
                </div>
                <div id="diverrorAlert" style="display: none">
                    <asp:ValidationSummary runat="server" ID="vsSaveTask" ValidationGroup="SaveTask" />
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
