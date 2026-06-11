<%@ Page Title="<%$ Resources:Captions,Title_AgentManagement %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="AgentListing.aspx.cs" Inherits="ERPSMS_v01.VendorManagement.AgentListing"
    Theme="ClassicExt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/VendorManagement/AgentListing.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    ToolTip="<%$ resources:Controls,New %>" OnClientClick="javascript:return AddNew();"
                                    TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                    Visible="false" OnClientClick="javascript:return ResetPage();" TabIndex="8" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="searchwrap" class="search-wrap-custom1">
            <label for="SearchType">
                <%=Resources.Controls.SearchBy%></label>
            <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType();"
                EnableViewState="false">
                <asp:ListItem Value="VEN_CODE" Text="<%$ Resources:BindValues, AgentCode%>"></asp:ListItem>
                <asp:ListItem Value="VEN_NAME" Text="<%$ Resources:BindValues, AgentName%>"></asp:ListItem>
                <asp:ListItem Value="VEN_PHONE" Text="<%$ Resources:BindValues, AgentPhone%>"></asp:ListItem>
            </asp:DropDownList>
            <div id="divSearchDtls">
                <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false">
                </asp:TextBox>
            </div>
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0"></asp:HiddenField>
            <div id="divVendorData">
            </div>
            <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClientClick="javascript:return BindGrid();" CssClass="margntop2"
                EnableViewState="false" />
            <div style="float: right; display: none">
                <asp:Button ID="btnAdvSearch" runat="server" Text="Advance Search" OnClientClick="javascript:return ShowAdvSearch();" />
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="gridwrap">
            <table rules="all" id="grdVendorDetails" grandtype="GrandGrid" pagesize="20" paging="true"
                editfunction="GridAddressAction" editable="true" width="100%" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="REF_ID" isvisible="false">
                        </th>
                        <th fieldmap="VEN_PK" isvisible="false">
                        </th>
                        <th fieldmap="VEN_TYPE" isvisible="false">
                        </th>
                        <th fieldmap="VEN_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="VEN_MOD_DT" isvisible="false">
                        </th>
                        <th fieldmap="VEN_CODE" align="left" sortable="true" width="15%">
                            <%=Resources.Controls.AgentCode%>
                        </th>
                        <th fieldmap="VEN_NAME" width="25%" sortable="true">
                            <%=Resources.Controls.AgentName%>
                        </th>
                        <th fieldmap="VEN_CONT_NAME" width="22%" sortable="true">
                            <%=Resources.Controls.ContactName%>
                        </th>
                        <th fieldmap="VEN_PHONE" width="15%" sortable="true">
                            <%=Resources.Controls.Phone%>
                        </th>
                        <th fieldmap="VEN_TYPE_TEXT" width="12%" sortable="true" isvisible="false">
                            <%=Resources.Controls.Type%>
                        </th>
                        <th fieldmap="VEN_STATUS_TEXT" width="15%" sortable="true">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                        <asp:HiddenField runat="server" ID="UserRoles" Value="0" />
                        <asp:HiddenField runat="server" ID="hdnFinalApprovar" Value="0" />
                        <th type="Template" width="13%">
                            <div style="text-align: right">
                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$ resources:Controls,Edit %>"
                                    SkinID="imbactiongrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbUpdate" SkinID="imbeditgrid" Style="display: none"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'UPDATE')" />
                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$ resources:Controls,Delete %>"
                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$ resources:Controls,View %>"
                                    SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbEvaluate" ToolTip="<%$ resources:Controls,Evaluate %>"
                                    SkinID="btnevaluate" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EVALUATE')" />
                                <asp:ImageButton runat="server" ID="imbPrint" ToolTip="<%$ resources:Controls,Print %>"
                                    SkinID="btnPrint" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')"
                                    Style="display: none" />
                                <asp:ImageButton runat="server" ID="imbCommRate" ToolTip="<%$ resources:Controls,CommissionRate %>"
                                    SkinID="btnCommissionRate" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'COMMISSIONRATE')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
            <div class="clear">
            </div>
        </div>
    </div>
</asp:Content>
