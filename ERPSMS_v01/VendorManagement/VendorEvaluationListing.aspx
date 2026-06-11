<%@ Page Title="<%$ Resources:Captions,Title_SupplierEvaluationForm %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="VendorEvaluationListing.aspx.cs"
    Inherits="ERPSMS_v01.VendorManagement.VendorEvaluationListing" Theme="ClassicExt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/VendorManagement/VendorEvaluationListing.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.VendorEvaluationManagement%></h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();"
                TabIndex="10" EnableViewState="false" />
            <asp:ImageButton ID="imbReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                TabIndex="8" />
            <asp:ImageButton ID="imbCancel" Visible="false" runat="server" SkinID="btncancel"
                OnClientClick="javascript:return CancelFun();" TabIndex="9" />
            <div id="divVendorData">
            </div>
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
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    OnClientClick="javascript:return AddNew();" TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                    OnClientClick="javascript:return ResetPage();" TabIndex="8" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
         
        </div>
    </div>
        <div class="content-wrapper">
          <div id="searchwrap" class="search-wrap-custom1">
            <div id="divSearch">
                <span>
                    <%=Resources.Controls.SearchBy%></span>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType();"
                    EnableViewState="false">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>"></asp:ListItem>
                    <asp:ListItem Value="VEN_CODE" Text="<%$ Resources:BindValues, VendorCode%>"></asp:ListItem>
                    <asp:ListItem Value="VEN_NAME" Text="<%$ Resources:BindValues, VendorName%>"></asp:ListItem>
                    <asp:ListItem Value="ITM_NAME" Text="<%$ Resources:BindValues, ProductName%>"></asp:ListItem>
                   <%-- <asp:ListItem Value="VEH_RAT_DESC" Text="<%$ Resources:BindValues, Rating%>"></asp:ListItem>--%>
                </asp:DropDownList>
                <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" onkeypress="javascript:SerachEnterKey(event);">
                </asp:TextBox>
                <div id="divDate">
                    <span>
                        <%=Resources.Controls.FromDate%></span>
                    <asp:TextBox runat="server" ID="FromDate">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfFromDate" runat="server" />
                    <span>
                        <%=Resources.Controls.ToDate%></span>
                    <asp:TextBox runat="server" ID="ToDate">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfToDate" runat="server" />
                </div>
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClientClick="javascript:return BindGrid();"
                EnableViewState="false" />
                <div style="float: right; display: none">
                    <asp:Button ID="btnAdvSearch" runat="server" Text="Advance Search" OnClientClick="javascript:return ShowAdvSearch();" />
                </div>
            </div>
             <div class="clear">
            </div>
        </div>
         <div class="gridwrap">
            <table rules="all" id="grdVendorDetails" grandtype="GrandGrid" paging="true" editfunction="GridAddressAction"
                pagesize="20" width="100%" editable="true" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="REF_ID" isvisible="false">
                        </th>
                        <th fieldmap="VEH_PK" isvisible="false">
                        </th>
                        <th fieldmap="VEH_VENDOR" isvisible="false">
                        </th>
                        <th fieldmap="VEH_ITEM" isvisible="false">
                        </th>
                        <th fieldmap="VEN_CODE" align="left" width="14%" sortable="true">
                            <%=Resources.Controls.VendorCode%>
                        </th>
                        <th fieldmap="VEN_NAME" width="23%" sortable="true">
                            <%=Resources.Controls.VendorName%>
                        </th>
                        <th fieldmap="VEH_CRTD_DT" align="left" sortable="true" width="9%">
                            <%=Resources.Controls.Date%>
                        </th>
                        <th fieldmap="ITM_NAME" width="28%" sortable="true">
                            <%=Resources.Controls.Product%>
                        </th>
                       <%-- <th fieldmap="VEH_PERC" width="14%" sortable="true">
                            <%=Resources.Controls.Performance%>
                        </th>
                        <th fieldmap="VEH_RAT_DESC" width="10%" sortable="true">
                            <%=Resources.Controls.Rating%>
                        </th>--%>
                        <th fieldmap="VEH_STATUS_TEXT" width="10%" sortable="true">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th fieldmap="VEH_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                        <th type="Template" width="10%">
                            <div align="right">
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" ToolTip="<%$Resources:Controls,Edit %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$Resources:Controls,Delete %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbView" SkinID="btnview" ToolTip="<%$Resources:Controls,View %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPrint" ToolTip="<%$Resources:Controls,Print %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
            <div class="clear">
            </div>
        </div>
        <asp:HiddenField runat="server" ID="VND_PK" Value="0" />
        <asp:HiddenField runat="server" ID="BizUnitPk" Value="1" />
        <asp:HiddenField runat="server" ID="UserRoles" Value="0" />
        <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
        <asp:HiddenField ID="hdfAppType" runat="server" />
        <asp:HiddenField ID="hdfAppSubType" runat="server" />
    </div>
</asp:Content>
