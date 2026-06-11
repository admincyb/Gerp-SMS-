<%@ Page Title="<%$ Resources:Captions,Title_UserManagement %>" Theme="ClassicExt"
    Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="UsersList.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Configurations.UsersList" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Configurations/UsersList.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.UsersList%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" TabIndex="11" OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();" />
        </div>
    </div>--%>
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" ToolTip="<%$Resources:Controls,Add%>"
                                    Text="<%$Resources:Controls,Add%>" OnClientClick="javascript:return AddNew();"
                                    TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" ToolTip="<%$Resources:Controls,Refresh%>"
                                    Text="<%$Resources:Controls,Refresh%>" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="8" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" ToolTip="<%$Resources:Controls,Cancel%>"
                                    Text="<%$Resources:Controls,Cancel%>" OnClientClick="javascript:return CancelFun();"
                                    TabIndex="8" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <asp:HiddenField ID="SBU" runat="server" Value="0" />
    <asp:HiddenField ID="hdfSBUSpecificUser" runat="server" Value="0" />
    <%--   <div id="divData">
        <div id="grdTable-wrap">
            <asp:HiddenField ID="hdfchkToDate" runat="server" />
            <asp:HiddenField ID="LOC_PK" runat="server" Value="0" />
            <asp:HiddenField ID="UserID" runat="server" Value="0" />
            <asp:HiddenField ID="LOC_BIZUNIT" runat="server" Value="0" />
            <asp:HiddenField ID="LOC_ACTIVE" runat="server" Value="1" />
            <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
            <asp:HiddenField ID="LOC_MOD_DT" runat="server" />
            <asp:HiddenField ID="hdnSlNo" runat="server" Value="0" />
            <div class="div2col-S">
                <div class="content">
                    <label for="LOC_CODE">
                        <%=Resources.Controls.SiteID%>
                        *</label>
                    <asp:TextBox ID="LOC_CODE" runat="server" TabIndex="1"></asp:TextBox>
                    <label for="LOC_DESC">
                        <%=Resources.Controls.Description%></label>
                    <asp:TextBox runat="server" ID="LOC_DESC" TabIndex="3" MaxLength="500" EnableViewState="false"
                        EnableTheming="false" TextMode="MultiLine" Rows="3">
                    </asp:TextBox>
                </div>
            </div>
            <div class="div2col-S">
                <div class="content">
                    <label for="LOC_NAME">
                        <%=Resources.Controls.SiteName%>
                        *</label>
                    <asp:TextBox ID="LOC_NAME" runat="server" TabIndex="2"></asp:TextBox>
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
    </div>--%>
    <div class="content-wrapper">
        <div id="divListing">
            <div id="searchwrap" class="search-wrap-custom1">
                <div id="divSearch">
                    <div id="divUserType" runat="server" visible="false">
                        <asp:Label ID="lblUserType" runat="server" AssociatedControlID="ddlUserType" Text="<%$ resources:UserType %>" />
                        <asp:DropDownList runat="server" ID="ddlUserType" CssClass="srchboxtextbx" onchange="javascript:SearchInit();"
                            EnableViewState="false">
                            <%-- <asp:ListItem Text="<%$ resources:Controls, ApplicationUser %>" Value="0" />
                            <asp:ListItem Text="<%$ resources:Controls, SystemUser %>" Value="1" />--%>
                        </asp:DropDownList>
                    </div>
                    <span>
                        <%=Resources.Controls.SearchBy%></span>
                    <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType();"
                        EnableViewState="false">
                        <%-- <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                        </asp:ListItem>--%>
                        <asp:ListItem Value="usrName" Text="<%$ Resources:Controls, UserName%>">
                        </asp:ListItem>
                       <%--  <asp:ListItem Value="usrTypeText" Text="<%$ Resources:Controls, UserType%>">
                        </asp:ListItem>--%>
                    </asp:DropDownList>
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false">
                    </asp:TextBox>
                      <asp:Label ID="lblIsActive" runat="server" AssociatedControlID="chkIsActive"
                                        Text="<%$ resources:IsActive %>" CssClass="margntop4 margnbotm0"/>
                                    <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" CssClass="margntop5 margnbotm0"/>
                                       <asp:HiddenField ID="STATUS" runat="server" Value="1" />
                    <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClientClick="javascript:return BindGrid();"
                        EnableViewState="false" CssClass="nofloat" />
                    <asp:HiddenField ID="hdfUserType" runat="server" Value="0" />
                    <div style="float: right; display: none">
                        <asp:Button ID="btnAdvSearch" runat="server" Text="Advance Search" OnClientClick="javascript:return ShowAdvSearch();" />
                    </div>
                    <div id="divUserModule" class="usrlimit floatRight" runat="server" visible="false"  >
                    <a href="UserModuleLimit.aspx" title="<%=Resources.Controls.UserModuleLimit%>" ><%=Resources.Controls.UserModuleLimit%></a>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="gridwrap">
                <table rules="all" id="grdUsersList" grandtype="GrandGrid" paging="true" editfunction="GridActionHandler"
                    editable="true" width="100%" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="usrPK" isvisible="false">
                            </th>
                            <th fieldmap="empCategory" isvisible="false">
                            </th>
                            <th fieldmap="usrStatus" isvisible="false">
                            </th>
                            <th fieldmap="usrName" sortable="true" width="15%">
                                <%=Resources.Controls.UserName%>
                            </th>
                            <th fieldmap="usrEmployeeText" sortable="true" width="15%">
                                <%=Resources.Controls.Employee%>
                            </th>
                            <th fieldmap="usrEmail" sortable="true" width="25%">
                                <%=Resources.Controls.EmailID%>
                            </th>
                            <th fieldmap="usrIsSysUserText" sortable="true" width="15%">
                                <%=Resources.Controls.UserType%>
                            </th>
                            <th fieldmap="usrStatusText" sortable="true" width="10%">
                                <%=Resources.Controls.Status%>
                            </th>
                            <th fieldmap="usrIsPublic" sortable="true" width="10%">
                                <%=Resources.Controls.IsPublicUser%>
                            </th>
                            <th type="Template" width="10%">
                                <div>
                                    <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" ToolTip="<%$ resources:Controls,Edit %>"
                                        OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                    <asp:ImageButton runat="server" ID="imbusermapping" SkinID="user-map" ToolTip="User Mapping"
                                        OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'ACTION')" />
                                    <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$ resources:Controls,Delete %>"
                                        OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                </div>
                            </th>
                        </tr>
                    </thead>
                </table>
                <%--<div class="clear">
                </div>--%>
            </div>

           <%-- ---------------------------------------------%>
            <div id="divRoleDetails" style="display: none;">
                    <div id="Div1" class="search-wrap-custom1">
                        <asp:Label runat="server" ID="lblUser" Text="<%$ resources:User %>" AssociatedControlID="lblUserName"></asp:Label>
                       <asp:Label runat="server" ID="lblUserName"></asp:Label>                        
                        <div class="clear">
                        </div>
                    </div>
                    <div class="gridwrap">
                       <table id="grdUserRoles" grandtype="GrandGrid" rules="all" paging="false" width="100%"
                        class="gridwraptable gridwrap">
                        <thead>
                            <tr>                                                       
                                <th fieldmap="USR_DEPT_TEXT" width="25%">
                                    <%=Resources.Controls.Department%>
                                </th>
                                <th fieldmap="USR_USER_GROUP_TEXT" width="50%" >
                                    <%=Resources.Controls.UserGroup%>
                                </th>             
                            </tr>
                        </thead>
                    </table>
                    </div>
                </div>
           <%-- ---------------------------------------------%>
        </div>
    </div>
</asp:Content>
