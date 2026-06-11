<%@ Page  Title="<%$ Resources:Captions,Title_MaterialReturn %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
AutoEventWireup="true" CodeBehind="MaterialReturnList.aspx.cs" EnableEventValidation="false"  Theme="Classic"
Inherits="ERPSMS_v01.StoreManagement.MaterialReturnList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/MaterialConsumptionList.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <%--   <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.MaterialReturnList%>
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
                <asp:DropDownList ID="SearchType" runat="server" TabIndex="1" CssClass="srchboxtextbx"
                    onchange="javascript:SetSearchType();" EnableViewState="false">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>
                    <asp:ListItem Value="ICH_NO" Text="<%$ Resources:BindValues,ICHNO %>">
                    </asp:ListItem>
                    <asp:ListItem Value="ICH_DEPT_TEXT" Text="<%$ Resources:BindValues,ConsumptionStore %>">
                    </asp:ListItem>
                    <%-- <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:BindValues, IssueStore%>">
                </asp:ListItem>
                <asp:ListItem Value="DPT_NAME_FROM" Text="<%$ Resources:BindValues, RequestingStore%>">
                </asp:ListItem>--%>
                </asp:DropDownList>
                <div id="divSearchDtls">
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false">
                    </asp:TextBox>
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
                        <th fieldmap="ICH_PK" isvisible="false">
                        </th>
                        <th fieldmap="ICH_NO" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.ICHNo%>
                        </th>
                         <th fieldmap="ICH_DEPT_TEXT" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.ConsumptionStore%>
                        </th>

                        <th fieldmap="ICH_ITEM_TEXT" sortable="true" align="left" width="25%">
                            <%=Resources.Controls.ICHItem%>
                        </th>

                        <th fieldmap="ICH_DATE" sortable="true" align="left" width="15%">
                            <%=Resources.Controls.ICHDate%>
                        </th>
                       
                        <th fieldmap="ICH_STATUS_TEXT" sortable="true" align="left" width="10%">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th fieldmap="ICH_MOD_BY_TEXT" sortable="true" align="left" width="10%">
                            <%=Resources.Controls.DoneBy%>
                        </th>
                        <th fieldmap="ICH_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="REF_ID" isvisible="false">
                        </th>
                        <th type="Template" width="12%" align="left">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbView" SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPrint" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
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

