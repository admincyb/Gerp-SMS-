<%@ Page Title="<%$ Resources:Captions,Title_StoreAudit %>" Language="C#" Theme="ClassicExt"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="StoreAuditListing.aspx.cs"
    Inherits="ERPSMS_v01.StoreManagement.StoreAuditListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/StoreAuditList.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.StoreAuditList%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" PostBackUrl="~/StoreManagement/StoreAuditing.aspx"
                EnableViewState="False" TabIndex="6" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="False" TabIndex="7" />
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
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    ToolTip="<%$Resources:Controls,Add%>" PostBackUrl="~/StoreManagement/StoreAuditing.aspx"
                                    TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                    ToolTip="<%$Resources:Controls,Refresh%>" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="8" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="content-wrapper">
        <div id="searchwrap" class="search-wrap-custom1">
            <div id="divSearch">
                <span>
                    <%=Resources.Controls.SearchBy%></span>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                    onchange="javascript:SetSearchType();" EnableViewState="false">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>
                    <asp:ListItem Value="SAH_NO" Text="<%$ Resources:BindValues, StoreAuditNo%>"></asp:ListItem>
                    <asp:ListItem Value="STK_VARIANCE_TEXT" Text="<%$ Resources:BindValues, Type%>"></asp:ListItem>
                    <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:BindValues, Store%>"></asp:ListItem>
                    <asp:ListItem Value="Date" Text="<%$ Resources:BindValues, DateRange%>"></asp:ListItem>
                </asp:DropDownList>
                <div id="divSearchDtls">
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="2">
                    </asp:TextBox>
                </div>
                <div id="divDate">
                    <span>
                        <%=Resources.Controls.FromDate%></span>
                    <asp:TextBox ID="FromDate" runat="server" TabIndex="3" EnableViewState="false">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfFrmDate" runat="server" />
                    <span>
                        <%=Resources.Controls.ToDate%></span>
                    <asp:TextBox ID="ToDate" runat="server" TabIndex="4" EnableViewState="false">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfToDate" runat="server" />
                </div>
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="5" OnClientClick="javascript:return BindGrid();"
                    EnableViewState="false" />
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="gridwrap">
            <table rules="all" id="grdStoreAuditList" grandtype="GrandGrid" pagesize="20" paging="true"
                width="100%" width="110%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="SAH_PK" isvisible="false">
                        </th>
                        <th fieldmap="REF_ID" isvisible="false">
                        </th>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="STK_VARIANCE" isvisible="false">
                        </th>
                        <th fieldmap="SAH_NO" isvisible="true" align="left" width="10%">
                            <%=Resources.Controls.AuditNo%>
                        </th>
                        <th fieldmap="SAH_DATE" sortable="true" align="left" width="10%">
                            <%=Resources.Controls.Date%>
                        </th>
                        <th fieldmap="SAH_ITEM_TEXT" sortable="true" width="43%" align="left">
                            <%=Resources.Controls.ItemDetails%>
                        </th>
                        <th fieldmap="DPT_NAME" sortable="true" width="10%" align="left">
                            <%=Resources.Controls.Store%>
                        </th>
                        <th fieldmap="STK_VARIANCE_TEXT" sortable="true" width="10%" align="left">
                            <%=Resources.Controls.Type%>
                        </th>
                        <th fieldmap="SAH_STATUS_TEXT" sortable="true" width="10%" align="left">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th type="Template" width="7%" align="left">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$Resources:Controls,Edit %>"
                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$Resources:Controls,View %>"
                                    SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbPrint" ToolTip="<%$Resources:Controls,Print %>"
                                    SkinID="btnPrint" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="Wofkflowdiv">
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
        </div>
    </div>
</asp:Content>
