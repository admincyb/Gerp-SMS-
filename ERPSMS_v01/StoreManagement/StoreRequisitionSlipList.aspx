<%@ Page Title="<%$ Resources:Captions,Title_StoreRequisitionSlip %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" EnableEventValidation="false" AutoEventWireup="true"
    Theme="ClassicExt" CodeBehind="StoreRequisitionSlipList.aspx.cs" Inherits="ERPSMS_v01.StoreManagement.StoreRequisitionSlipList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/StoreRequisitionSlipList.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div id="webwizard-wrap"> 
        <h1>
            <%=Resources.Captions.StoreRequisitionSlipList%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();" />
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return ClearPage();" />
            <asp:ImageButton ID="imbReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();" />
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
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    OnClientClick="javascript:return AddNew();" TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                    OnClientClick="javascript:return ResetPage();" TabIndex="8" />
                            </li>
                            <%-- <li>
                              <asp:Button ID="btnCancel" runat="server" OnClientClick="javascript:return CancelFun();"
                                    SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" />
                            </li>--%>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="content-wrapper">
        <div id="Wofkflowdiv">
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
                <asp:HiddenField ID="hdfType" runat="server" Value="0" />
        </div>
        <div id="searchwrap" class="search-wrap-custom1">
            <div id="divSearch">
                <label for="SearchType">
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" TabIndex="1" CssClass="srchboxtextbx"
                    onchange="javascript:SetSearchType();" EnableViewState="false">
                    <%--  <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                </asp:ListItem>--%>
                    <asp:ListItem Value="MRH_NO" Text="<%$ Resources:BindValues,SRNo %>">
                    </asp:ListItem>
                    <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:BindValues, IssueStore%>">
                    </asp:ListItem>
                    <asp:ListItem Value="DPT_NAME_FROM" Text="<%$ Resources:BindValues, RequestingStore%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Date" Text="<%$ Resources:BindValues, DateRange%>">
                    </asp:ListItem>
                    <asp:ListItem Value="MRH_STATUS" Text="<%$ Resources:BindValues, Status%>">
                    </asp:ListItem>
                    <%--<asp:ListItem Value="CMP_DISPLAY_CODE" Text="<%$ Resources:Controls, Plant%>">
                    </asp:ListItem>--%>
                </asp:DropDownList>
                <div id="divSearchDtls">
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false">
                    </asp:TextBox>
                </div>
                <div id="divSearchStatus">               
                    <asp:DropDownList ID="ddltrxstatus" runat="server" TabIndex="1" CssClass="srchboxtextbx"
                        EnableViewState="false">
                       <%--  <asp:ListItem Value="0" Text="<%$ Resources:BindValues,Select %>">
                    </asp:ListItem>--%>
                    </asp:DropDownList>
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
            <table rules="all" id="grdRequsitionList" grandtype="GrandGrid" pagesize="20" paging="true"
                editfunction="GridAction" editable="true" width="100%" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="MRH_PK" isvisible="false">
                        </th>
                        <th fieldmap="MRH_DATE" sortable="true" align="left" width="8%">
                            <%=Resources.Controls.MRDate%>
                        </th>
                        <th fieldmap="MRH_NO" sortable="true" align="left" width="9%">
                            <%=Resources.Controls.MRNo%>
                        </th>
                        <%--<th fieldmap="CMP_DISPLAY_CODE"  align="left" width="2%">
                        </th>--%>
                        <th fieldmap="MRH_ITEM_TEXT" sortable="true" align="left" width="41%">
                            <%=Resources.Controls.ItemDetails%>
                        </th>
                        <th fieldmap="DPT_NAME" sortable="true" align="left" width="12%">
                            <%=Resources.Controls.IssueStore%>
                        </th>
                        <th fieldmap="DPT_NAME_FROM" sortable="true" align="left" width="12%">
                            <%=Resources.Controls.RequestingStore%>
                        </th>
                        <th fieldmap="MRH_STATUS_TEXT" sortable="true" align="left" width="8%">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="REF_ID" isvisible="false">
                        </th>
                        <th fieldmap="MRH_STATUS" isvisible="false">
                        </th>
                        <th type="Template" width="10%" align="left">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" ToolTip="<%$ resources:Controls,Edit %>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$ resources:Controls,Delete %>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbModify" SkinID="imbeditgrid" title="<%$Resources:Controls,Modify%>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'MODIFY')" />
                                <asp:ImageButton runat="server" ID="imbCancel" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'CANCEL')"
                                    SkinID="cancel" alt="<%$Resources:Controls,Cancel%>" title="<%$Resources:Controls,Cancel%>" />
                                <asp:ImageButton runat="server" ID="imbView" SkinID="btnview" ToolTip="<%$ resources:Controls,View %>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPrint" ToolTip="<%$ resources:Controls,Print %>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div class="clear">
        </div>
    </div>
    <asp:HiddenField ID="hdnModifyMR" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCancelMR" runat="server" Value="0" />
</asp:Content>
