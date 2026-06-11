<%@ Page Title="<%$ Resources:Captions,Title_MRIssue %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="MRissueList.aspx.cs" Inherits="ERPSMS_v01.Inventory.MRissueList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/PurchaseRequestManagement/MRissueList.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div id="webwizard-wrap">
        <h1>
             <%=Resources.Captions.MaterialIssueSlip%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();"
                EnableViewState="False" TabIndex="6" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="False" TabIndex="7" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                TabIndex="8" />
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
                            <%-- <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                    OnClientClick="javascript:return ResetPage();" TabIndex="8" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" OnClientClick="javascript:return CancelFun();"
                                    SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>" />
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
                <label for="SearchType">
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                    onchange="javascript:SetSearchType();" EnableViewState="false">
                    <%--  <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>--%>
                    <asp:ListItem Value="ICH_NO" Text="<%$ Resources:BindValues, MINo%>">
                    </asp:ListItem>
                    <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:BindValues, IssuedBy%>">
                    </asp:ListItem>
                 <%--   <asp:ListItem Value="DPT_NAME_TO" Text="<%$ Resources:BindValues, IssuedTo%>">
                    </asp:ListItem>--%>
                    <asp:ListItem Value="Date" Text="<%$ Resources:BindValues, DateRange%>">
                    </asp:ListItem>
                    <asp:ListItem Value="ICH_PRH_NO" Text="<%$ Resources:BindValues, MR%>">
                    </asp:ListItem>
                    <%--<asp:ListItem Value="CMP_DISPLAY_CODE" Text="<%$ Resources:Controls, Plant%>">
                    </asp:ListItem>--%>
                </asp:DropDownList>
                <div id="divSearchDtls">
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="2">
                    </asp:TextBox>
                </div>
                <div id="divDate">
                    <label for="FromDate">
                        <%=Resources.Controls.FromDate%></label>
                    <asp:TextBox ID="FromDate" runat="server" TabIndex="3" EnableViewState="false">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfFrmDate" runat="server" />
                    <label for="ToDate">
                        <%=Resources.Controls.ToDate%></label>
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
            <table rules="all" id="grdMIList" grandtype="GrandGrid" pagesize="20" paging="true"
                width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="ICH_PK" isvisible="false">
                        </th>
                        <th fieldmap="CMP_LINE_COLOUR" isvisible="false">
                        </th>
                        <th fieldmap="ICH_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="ICH_NO" sortable="true" align="left" width="10%">
                            <%=Resources.Controls.MINo%>
                        </th>
                         <th fieldmap="CMP_DISPLAY_CODE" isvisible="false" align="left" width="2%">                           
                        </th> 
                        <th fieldmap="ICH_DATE" sortable="true" width="10%" align="left">
                            <%=Resources.Controls.MIDate%>
                        </th>
                        <th fieldmap="ICH_ITEM_TEXT" sortable="true" width="39%" align="left">
                            <%=Resources.Controls.ItemDetails%>
                        </th>
                        <th fieldmap="DPT_NAME" sortable="true" width="12%" align="left">
                            <%=Resources.Controls.IssuedBy%>
                        </th>
                        <th fieldmap="ICH_COST_CENTER_TEXT" sortable="true" width="10%" align="left" runat="server" id="thCostCenter" visible="false" >
                            <%=Resources.Controls.CostCenter%>
                        </th>
                        <th fieldmap="ICH_STATUS_TEXT" sortable="true" width="10%" align="left">
                            <%=Resources.Controls.RequestStatus%>
                        </th>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="REF_ID" isvisible="false">
                        </th>
                        <th type="Template" width="9%" align="left">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" ToolTip="<%$ resources:Controls,Edit %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbModify" SkinID="imbeditgrid" title="<%$Resources:Controls,Modify%>"  OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'MODIFY')" />
                                <asp:ImageButton runat="server" ID="imbCancel" SkinID="cancel"  title="<%$Resources:Controls,Cancel%>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'CANCEL')"  /> 
                                <asp:ImageButton runat="server" ID="imbView" SkinID="btnview" ToolTip="<%$ resources:Controls,View %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPrint" ToolTip="<%$ resources:Controls,Print %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="Wofkflowdiv">
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
            <asp:HiddenField ID="hdnModify" runat="server" Value="0" />
            <asp:HiddenField ID="hdnCancel" runat="server" Value="0" />
        </div>
        <asp:HiddenField ID="hdfMenuType" runat="server" Value="0" />
    </div>
</asp:Content>
