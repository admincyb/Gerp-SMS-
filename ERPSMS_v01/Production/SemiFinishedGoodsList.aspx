<%@ Page Title="<%$ Resources:Captions,Title_SemiFinishedGoods %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
 CodeBehind="SemiFinishedGoodsList.aspx.cs" Inherits="ERPSMS_v01.Production.SemiFinishedGoodsList" Theme="ClassicExt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/Production/DispersionPreparationList.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum1"></asp:Label>
                            <asp:Label runat="server" ID="lblbreadCrumNew"></asp:Label>
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
        <div class="search-colapse">
            <table>
                <tr>
                    <td>
                        <h1>
                            <%=  GetGlobalResourceObject("Captions","AdvSearch") .ToString()%></h1>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvSearch(1);"
                            ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                            TabIndex="1" />
                        <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvSearch();"
                            ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                            TabIndex="1" />
                        <asp:HiddenField ID="hdfShowHideFilter" runat="server" Value="0" ClientIDMode="Static" />
                    </td>
                </tr>
            </table>
        </div>
        <table class="table-3devide" id="tbladvSearch" style="background: #f2f2f2;">
            <tr>
                <td>
                    <div class="div2col-S padgtop7">
                        <asp:Label ID="lblFilterBy" AssociatedControlID="SearchType" runat="server" Text="<%$ Resources:Controls,SearchBy %>"></asp:Label>
                        <asp:DropDownList ID="SearchType" runat="server" CssClass="select-small-e2" TabIndex="1"
                            onchange="javascript:SetSearchType();" EnableViewState="false">
                            <asp:ListItem Value="DSP_NAME" Text="<%$ Resources:BindValues, BOMName%>"></asp:ListItem>
                            <asp:ListItem Value="DTH_BATCH_NO" Text="<%$ Resources:BindValues, SFGBatchNo%>"></asp:ListItem>
                           <%-- <asp:ListItem Value="MCH_NAME" Text="<%$ Resources:BindValues, MachineName%>"></asp:ListItem>--%>
                            <asp:ListItem Value="Date" Text="Date Range"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div id="divSearchDtls" class="div2col-S padgtop7">                      
                        <asp:Label ID="lblSearchText" AssociatedControlID="SearchValue" runat="server" CssClass="lbl-19-4perc"
                            Text="<%$ Resources:Captions,lbl_SearchValue %>"></asp:Label>
                        <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="2"
                            CssClass="input-small-d"></asp:TextBox>
                    </div>
                    <div id="divDate" class="div2col-S padgtop7">                      
                        <label for="FromDate" class="middle-lbl">
                            <asp:Literal ID="lblFromDate" runat="server" Text="<%$ Resources:Controls,FromDate %>" /></label>
                        <asp:TextBox ID="FromDate" runat="server" TabIndex="3" Width="100" CssClass="input-small"></asp:TextBox>
                        <asp:HiddenField ID="hdfFrmDate" runat="server" />                   
                        <label for="ToDate" class="middle-lbl">
                            <asp:Literal ID="lblToDate" runat="server" Text="<%$ Resources:Controls,ToDate %>" /></label>
                        <asp:TextBox ID="ToDate" runat="server" TabIndex="4" Width="100" CssClass="input-small"></asp:TextBox>
                        <asp:HiddenField ID="hdfToDate" runat="server" />
                        <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfQcStatus" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfQcDept" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfQcPath" runat="server" Value="" />
                        <asp:HiddenField ID="hdfCancelRight" runat="server" Value="0" />
                        <asp:HiddenField ID="hdfpageURL" runat="server" Value="" />
                    </div>
                </td>
                <td>
                    <div class="div2col-S padgtop7">
                        <asp:Label runat="server" ID="lblchckCancelled" Text="<%$ resources:lblCancelStatus %>" AssociatedControlID="ChkCancelStatus"></asp:Label>
                        <asp:CheckBox runat="server" ID="ChkCancelStatus" TabIndex="5" />
                        <%--<asp:Label runat="server" ID="lblChkShowAll" class="lbl-12-5perc margnrgt5" Text="<%$ resources:ShowAll %>" AssociatedControlID="ChkShowAll"></asp:Label>
                        <asp:CheckBox runat="server" ID="ChkShowAll" TabIndex="5"/>--%>
                        <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="5" OnClientClick="javascript:return BindGrid();"
                            EnableViewState="false" CssClass="margnrgt5 margnbotm0 margnlft4" />
                        <asp:ImageButton ID="imbClear" runat="server" SkinID="cancel" OnClientClick="javascript:return ClearSearchDetails();"
                            Visible="true" ToolTip="Clear" Width="18px" Height="18px" TabIndex="5" CssClass="margnrgt7 margnbotm0" />
                    </div>
                </td>
            </tr>
        </table>
        <div class="gridwrap">
            <table rules="all" id="grdDispersionList" grandtype="GrandGrid" width="100%" pagesize="20"
                paging="true" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="DTH_PK" isvisible="false">
                        </th>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="REF_ID" isvisible="false">
                        </th>
                        <th fieldmap="DTH_DATE" sortable="true" align="left" width="12%">
                            <%--DSP_CODE--%>
                            <%=Resources.Controls.Date%>
                        </th>
                        <th fieldmap="PLN_NAME" sortable="true" isvisible="false" align="left">
                            <%=Resources.Controls.Plan%>
                        </th>
                         <th fieldmap="DTH_BATCH_NO" sortable="true" align="left" width="25%">
                            <%=Resources.Controls.BatchNo%>
                        </th>
                        <th fieldmap="DSP_NAME" sortable="true" align="left" width="25%">
                            <%=Resources.Controls.SFGName%>
                        </th>
                       <%-- <th fieldmap="DSP_TYPE_TEXT" sortable="true" align="left" width="8%">
                            <%=Resources.Controls.Type%>
                        </th>--%>
                       <%-- <th fieldmap="MCH_NAME" sortable="true" align="left" width="10%">
                            <%=Resources.Controls.MachineName%>
                        </th>--%>
                        <th fieldmap="DTH_QUANTITY" sortable="true" align="right" width="10%">
                            <%=Resources.Controls.Quantity%>
                        </th>
                        <%--<th fieldmap="DTH_QTY_BALANCE" sortable="true" align="left" width="9%">
                            <%=Resources.Controls.BalanceQty%>
                        </th>--%>
                        <th fieldmap="DTH_QUANTITY_UOM_TEXT" sortable="true" align="left" width="10%">
                            <%=Resources.Controls.UOM%>
                        </th>
                        <th fieldmap="DTH_STATUS_TEXT" sortable="true" width="10%" align="left">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th fieldmap="DTH_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="DTH_DATE_NEW" isvisible="false">
                        </th>
                          <th fieldmap="refPK" isvisible="false">
                        </th>
                        <th fieldmap="refProcess" isvisible="false">
                        </th>
                        <th type="Template" width="8%" align="left">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:Controls,Edit %>"
                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$resources:Controls,View %>"
                                    SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:Controls,Delete %>"
                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbCancelDisp" ToolTip="<%$resources:CancelDispersion %>"
                                    SkinID="cancel" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'CANCELDISPERSION')" />
                                <asp:ImageButton runat="server" ID="imbPrint" ToolTip="<%$resources:Controls,Print %>"
                                    SkinID="btnPrint" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
</asp:Content>
