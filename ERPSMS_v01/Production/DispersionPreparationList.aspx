<%@ Page Title="<%$ Resources:Captions,Title_DispersionPreparation %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="DispersionPreparationList.aspx.cs" Inherits="ERPSMS_v01.Production.DispersionPreparationList"
    Theme="Classic" %>

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
            <label for="SearchType">
                <%=Resources.Controls.SearchBy%></label>
            <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                onchange="javascript:SetSearchType();" EnableViewState="false">    
                 <asp:ListItem Value="DSP_NAME" Text="<%$ Resources:BindValues, DispersionName%>">
                </asp:ListItem>          
                <asp:ListItem Value="DTH_BATCH_NO" Text="<%$ Resources:BindValues, DispBatchNo%>">
                </asp:ListItem>               
                <%-- <asp:ListItem Value="PLN_NAME" Text="<%$ Resources:BindValues, Plan%>">
                    </asp:ListItem>--%>
                <asp:ListItem Value="MCH_NAME" Text="<%$ Resources:BindValues, MachineName%>">
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
                <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
                <asp:HiddenField ID="hdfQcStatus" runat="server" Value="0" />
                <asp:HiddenField ID="hdfQcDept" runat="server" Value="0" />
                <asp:HiddenField ID="hdfQcPath" runat="server" Value="" />
                <asp:HiddenField ID="hdfCancelRight" runat="server" Value="0" />
            </div>
            <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="5" OnClientClick="javascript:return BindGrid();"
                EnableViewState="false" />
            <div class="clear">
            </div>
        </div>
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
                    
                        <th fieldmap="DTH_DATE" sortable="true" align="left" width="9%">
                            <%--DSP_CODE--%>
                            <%=Resources.Controls.Date%>
                        </th>
                            <th fieldmap="DTH_BATCH_NO" sortable="true" align="left" width="17%">
                            <%=Resources.Controls.DispBatchNo%>
                        </th>
                        <th fieldmap="PLN_NAME" sortable="true" isvisible="false" align="left" width="13%">
                            <%=Resources.Controls.Plan%>
                        </th>
                        <th fieldmap="DSP_NAME" sortable="true" align="left" width="17%">
                            <%=Resources.Controls.DispersionName%>
                        </th>
                         <th fieldmap="DSP_TYPE_TEXT" sortable="true" align="left" width="8%">
                            <%=Resources.Controls.Type%>
                        </th>
                        <th fieldmap="MCH_NAME" sortable="true" align="left" width="12%">
                            <%=Resources.Controls.MachineName%>
                        </th>
                         <th fieldmap="DTH_QUANTITY" sortable="true" align="right" width="8%">
                            <%=Resources.Controls.Quantity%>
                        </th>
                         <th fieldmap="DTH_QUANTITY_UOM_TEXT" sortable="true" align="left" width="6%">
                            <%=Resources.Controls.UOM%>
                        </th>
                        <th fieldmap="DTH_STATUS_TEXT" sortable="true" width="9%" align="left">
                            <%=Resources.Controls.Status%>
                        </th> 
                        <th fieldmap="DTH_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="DTH_DATE_NEW" isvisible="false">
                        </th>
                        <th type="Template" width="10%" align="left">
                            <div style="text-align: left"> 
                                <asp:ImageButton runat="server" ID="imbEdit"  ToolTip="<%$resources:ErpRes,Edit %>" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$resources:ErpRes,View %>" SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:ErpRes,Delete %>" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbCancelDisp" ToolTip="<%$resources:CancelDispersion %>" SkinID="cancel" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'CANCELDISPERSION')" />                                
                                <asp:ImageButton runat="server" ID="imbPrint"  ToolTip="<%$resources:ErpRes,Print %>" SkinID="btnPrint" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
                                <asp:ImageButton runat="server" ID="imbQc" ToolTip="<%$resources:ErpRes,QC %>" SkinID="btnInner-Qa" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'QC')" />                                
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
</asp:Content>
