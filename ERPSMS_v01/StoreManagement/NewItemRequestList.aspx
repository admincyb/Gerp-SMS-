<%@ Page Title="<%$ Resources:Captions,Title_NewItemRequest %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    Theme="Classic" AutoEventWireup="true" CodeBehind="NewItemRequestList.aspx.cs"
    Inherits="ERPSMS_v01.StoreManagement.NewItemRequestList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/NewItemRequestList.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.NewItemRequestList%>
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
                            <li>
                                <asp:Button ID="btnCancel" runat="server" OnClientClick="javascript:return CancelFun();"
                                    SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" />
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
        <div id="Wofkflowdiv">
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
        </div>
        <div id="searchwrap" class="search-wrap-custom1">
            <div id="divSearch">
                <label for="SearchType">
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                    onchange="javascript:SetSearchType();" EnableViewState="false" Width="150px">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>
                    <asp:ListItem Value="ITR_NO" Text="<%$ Resources:BindValues,NIR %>">
                    </asp:ListItem>
                    <asp:ListItem Value="ITR_NAME" Text="<%$ Resources:BindValues, ItemName%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Date" Text="<%$ Resources:BindValues, RequestDateRange%>">
                    </asp:ListItem>
                    <asp:ListItem Value="ITR_STATUS_TEXT" Text="<%$ Resources:BindValues, RequestStatus%>">
                    </asp:ListItem>
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
            <table rules="all" id="grdNewItemList" grandtype="GrandGrid" pagesize="20" paging="true"
                editfunction="GridAction" editable="true" width="100%" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="ITR_PK" isvisible="false">
                        </th>
                        <th fieldmap="ITR_NO" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.NIR%>
                        </th>
                        <th fieldmap="ITR_DEPT_STORE" sortable="true" align="left" isvisible="false">
                            <%=Resources.Controls.RequestTo%>
                        </th>
                        <th fieldmap="DPT_NAME" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.RequestTo%>
                        </th>
                        <th fieldmap="ITR_NAME" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.Item%>
                        </th>
                        <th fieldmap="ITR_REQD_DATE" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.RequiredBy%>
                        </th>
                        <th fieldmap="ITR_QTY_REQD" sortable="true" align="right" width="10%">
                            <%=Resources.Controls.RequestQuantity%>
                            <th fieldmap="ITR_QTY_UOM" sortable="true" align="left" isvisible="false">
                                <%=Resources.Controls.UOM%>
                            </th>
                            <th fieldmap="UOM_CODE" sortable="true" align="left" width="8%">
                                <%=Resources.Controls.UOM%>
                            </th>
                            <th fieldmap="ITR_STATUS_TEXT" sortable="true" align="left" width="10%">
                                <%=Resources.Controls.Status%>
                            </th>
                            <%--<th fieldmap="MRH_STATUS_TEXT" sortable="true" align="center" width="15%"></th> --%>
                            <th fieldmap="USER_STATUS" isvisible="false">
                            </th>
                            <th fieldmap="REF_ID" isvisible="false">
                            </th>
                            <th type="Template" width="12%" align="left">
                                <div style="text-align: left">
                                    <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                    <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                    <asp:ImageButton runat="server" ID="imbView" SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                    <%-- <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPrint" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />--%>
                                </div>
                            </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
