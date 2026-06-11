<%@ Page Title="<%$ Resources:Captions,Title_CompoundingPreparation %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" Theme="Classic" CodeBehind="CompoundListing.aspx.cs"
    Inherits="ERPSMS_v01.Production.CompoundListing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/Production/CompoundListing.js.axd" type="text/javascript"></script>
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
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" OnClientClick="javascript:return AddNew();"
                                    ToolTip="<%$resources:Controls,Add %>" EnableViewState="False" TabIndex="6" Text="<%$Resources:Controls,Add%>" /> 
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" ToolTip="<%$resources:Controls,Refresh %>"
                                    Text="<%$Resources:Controls,Refresh%>" SkinID="btnInner-refresh" OnClientClick="javascript:return ResetPage();"
                                    EnableViewState="False" TabIndex="7" />
                                <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="clear">
        </div>
    </div>
    <div id="grdTable-wrap" class="content-wrapper" >
        <div id="searchwrap" class="search-wrap-custom1"> 
                <label for="SearchType">
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                    onchange="javascript:SetSearchType();" EnableViewState="false">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>
                    <asp:ListItem Value="CTH_BATCH_NO" Text="<%$ Resources:BindValues, CompBatchNo%>">
                    </asp:ListItem>
                    <asp:ListItem Value="COM_NAME" Text="<%$ Resources:BindValues, CompoundName%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Date" Text="Date Range">
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
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search"  
                    TabIndex="5" OnClientClick="javascript:return BindGrid();"  EnableViewState="false" /> 
                    <div class="clear">
            </div>
        </div>
        <asp:ListItem Value="CTH_BATCH_NO" Text="Compound Batch #">
        </asp:ListItem>
        <asp:ListItem Value="CTH_PLAN" Text="Plan">
        </asp:ListItem>
        <div class="gridwrap">
            <table rules="all" id="grdCompoundDetails" grandtype="GrandGrid" paging="true" editfunction="GridAction"
                editable="true" pagesize="20" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="CTH_PK" isvisible="false" width="0%">
                        </th>
                        <%--<th fieldmap="DSD_QTY_UOM" isvisible="false" width="0%">
                                </th>
                                <th fieldmap="ITEM_CATAGORY" isvisible="false" width="0%">
                                </th>
                                <th fieldmap="DSD_QUANTITY" isvisible="false" width="0%">
                                </th>--%>
                        <th fieldmap="REF_ID" isvisible="false" width="0%">
                        </th>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="COM_NAME" align="left" width="28%" sortable="true">
                            <%=Resources.Controls.CompoundName%>
                        </th>
                        <th fieldmap="CTH_BATCH_NO" align="left" width="23%" sortable="true">
                            <%=Resources.Controls.CompoundBatchno%>
                        </th>
                        <th fieldmap="CTH_COMP_DATE" align="left" width="14%" sortable="true">
                            <%=Resources.Controls.Date%>
                        </th>
                        <th fieldmap="PLN_NAME" align="left" width="15%" isvisible="false" sortable="true">
                            <%=Resources.Controls.Plan%>
                        </th>
                        <th fieldmap="CTH_QUANTITY" align="right" width="10%">
                            <%=Resources.Controls.Quantity%>
                        </th>
                        <th fieldmap="CTH_STATUS_TEXT" align="left" width="10%" sortable="true">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th type="Template" width="15%" align="center">
                            <div style="text-align: left;">
                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:ErpRes,Delete %>" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$resources:ErpRes,View %>" SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbPrint"  ToolTip="<%$resources:ErpRes,Print %>" SkinID="btnPrint" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
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
