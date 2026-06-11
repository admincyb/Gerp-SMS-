<%@ Page Title="<%$ Resources:Captions,Title_StockAccept %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="MaterialAcceptList.aspx.cs" Inherits="ERPSMS_v01.StoreManagement.MaterialAcceptList"
    Theme="ClassicExt" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/MaterialAcceptList.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <div id="webwizard-wrap">
        <h1>
              <%=Resources.Captions.MaterialAccept%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();"
                EnableViewState="False" TabIndex="6" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="False" TabIndex="7" />
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
        <div id="searchwrap" class="search-wrap-custom1">
            <div id="divSearch">
                <label for="SearchType">
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                    onchange="javascript:SetSearchType();" EnableViewState="false">
                    <%-- <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>--%>
                    <asp:ListItem Value="MAH_NO" Text="<%$ Resources:BindValues, MANO%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Date" Text="Date Range">
                    </asp:ListItem>
                    <asp:ListItem Value="MAH_MIH_NO" Text="<%$ Resources:BindValues, STNo%>">
                    </asp:ListItem>
                    <asp:ListItem Value="MAH_STATUS" Text="<%$ Resources:BindValues, Status%>">
                    </asp:ListItem>
                    <%-- <asp:ListItem Value="CMP_DISPLAY_CODE" Text="<%$ Resources:Controls, Plant%>">
                    </asp:ListItem>--%>
                </asp:DropDownList>
                <div id="divSearchDtls">
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="2">
                    </asp:TextBox>
                </div>
                <div id="divSearchStatus">
                    <asp:DropDownList ID="ddltrxstatus" runat="server" CssClass="srchboxtextbx" TabIndex="1"
                        EnableViewState="false">
                        <%--   <asp:ListItem Value="0" Text="<%$ Resources:BindValues,Select %>">
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
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClientClick="javascript:return BindGrid();"
                    EnableViewState="false" />
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="gridwrap">
            <table rules="all" id="grdMAList" grandtype="GrandGrid" pagesize="20" paging="true"
                width="100%" width="110%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="MAH_PK" isvisible="false"></th>
                        <th fieldmap="MAH_IS_CONVERSION_REQD" isvisible="false"></th>
                        <th fieldmap="MAH_IS_CONVERTED" isvisible="false"></th>
                        <th fieldmap="CMP_LINE_COLOUR" isvisible="false"></th>
                        <th fieldmap="MAH_STATUS" isvisible="false"></th>
                        <th fieldmap="USER_STATUS" isvisible="false"></th>
                        <th fieldmap="MAH_NO" sortable="true" width="10%" align="left">
                            <%=Resources.Controls.MaterialAcceptNo%>
                        </th>
                        <th fieldmap="CMP_DISPLAY_CODE" isvisible="false" align="left" width="2%"></th>
                        <th fieldmap="MAH_DATE" sortable="true" width="10%" align="left">
                            <%=Resources.Controls.Date%>
                        </th>
                        <th fieldmap="MAH_ITEM_TEXT" sortable="true" width="48%" align="left">
                            <%=Resources.Controls.ItemDetails%>
                        </th>
                        <th fieldmap="DPT_NAME" sortable="true" width="15%" align="left">
                            <%=Resources.Controls.Department%>
                        </th>
                        <th fieldmap="MAH_STATUS_TEXT" sortable="true" align="left" width="8%">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th fieldmap="REF_ID" isvisible="false"></th>
                        <th type="Template" width="9%" align="left">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" ToolTip="<%$ resources:Controls,Edit %>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" ToolTip="<%$ resources:Controls,Delete %>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbModify" SkinID="imbeditgrid" ToolTip="<%$Resources:Controls,Modify%>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'MODIFY')" />
                                <asp:ImageButton runat="server" ID="imbCancel" SkinID="cancel" ToolTip="<%$Resources:Controls,Cancel%>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'CANCEL')" />
                                <asp:ImageButton runat="server" ID="imbView" SkinID="btnview" ToolTip="<%$ resources:Controls,View %>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPrint" ToolTip="<%$ resources:Controls,Print %>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />

                                <asp:ImageButton runat="server" ID="imbConvert" SkinID="btnConvert" ToolTip="<%$ resources:Controls,Convert %>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'CONVERT')" />

                                <asp:ImageButton runat="server" ID="imbNotConvert" SkinID="btnNotConvert" ToolTip="<%$ resources:Controls,NotConverted %>"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'NOTCONVERT')" />


                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>

        <div id="divConvert" style="display: none">
            <div class="Button-container button-container-1">
                <asp:Table ID="Table2" runat="server">
                    <asp:TableRow>
                        <asp:TableCell ID="CONV_Panel" CssClass="" HorizontalAlign="Right">
                            <ul>
                                <li>
                                    <asp:Button runat="server" ID="btnConvert" TabIndex="2" Text="<%$resources:Convert %>"
                                        OnClientClick="javascript:return SaveSTAConversion();" ValidationGroup="Convert"
                                        ToolTip="<%$resources:Convert %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                </li>
                                <li>
                                    <asp:Button runat="server" ID="btnDeleteConvert" CommandName="DELETECONVERSION" Text="<%$resources:Controls,Delete %>"
                                        TabIndex="4" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClientClick="javascript:return DeleteSTAConversion();" 
                                        ToolTip="<%$resources:Controls,Delete %>" />
                                </li>
                            </ul>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>

            <h3 class="fontWGT-Nrml"><%=GetLocalResourceObject("ItemReceived").ToString()%></h3>

                <div class="gridwrap">
                    <table rules="all" id="grdReceived" grandtype="GrandGrid" pagesize="20" paging="true"
                        width="100%" width="110%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                        <thead>
                            <tr>
                                  <th fieldmap="MAD_SL_NO" isvisible="false">
                                  </th>                                               
                                 <th fieldmap="MAH_PK" isvisible="false">
                                 </th>
                                 <th fieldmap="MAD_PK" isvisible="false">
                                 </th>
                                   <th fieldmap="SBD_PK" isvisible="false">
                                 </th>
                                 <th fieldmap="ITM_PK" isvisible="false">
                                 </th>
                                 <th fieldmap="ITM_UOM" isvisible="false">
                                 </th>
                                <th fieldmap="MID_NO" sortable="true" width="10%" align="left">
                                  ST#
                                </th>
                                 <th fieldmap="ITM_NAME" sortable="true" width="25%" align="left">
                                    <%=GetLocalResourceObject("Item").ToString()%>
                                </th>
                                
                                 <th fieldmap="ITM_UOM_CODE" sortable="true" width="10%" align="left">
                                    <%=GetLocalResourceObject("UOM").ToString()%>
                                </th>
                                
                                  <th fieldmap="MAD_QTY_ACCEPTED" sortable="true" width="10%" align="right">
                                    <%=GetLocalResourceObject("ReceivedQty").ToString()%>
                                </th>
                                <th fieldmap="BALANCE" sortable="true" width="20%" align="right">
                                    <%=GetLocalResourceObject("StockQty").ToString()%>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            <div>

            </div>
             <h3 class="fontWGT-Nrml"><%=GetLocalResourceObject("ItemToBeConverted").ToString()%></h3>
            <div class="gridwrap">
                <table rules="all" id="grdConvert" grandtype="GrandGrid" pagesize="20" paging="true"
                        width="100%" width="110%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                        <thead>
                            <tr>
                                
                                

                             <%--   <th fieldmap="WIH_NO" sortable="true" width="10%" align="left">
                                  <%=GetLocalResourceObject("WoNo").ToString()%>
                                </th>--%>
                                 <th fieldmap="MAD_SL_NO" isvisible="false">
                                  </th>
                                  <th fieldmap="MAH_PK" isvisible="false">
                                  </th>
                                 <th fieldmap="MAD_PK" isvisible="false">
                                 </th>
                                 <th fieldmap="SBD_PK" isvisible="false">
                                 </th>
                                 <th fieldmap="ITM_PK" isvisible="false">
                                 </th>
                                 <th fieldmap="ITM_UOM" isvisible="false">
                                 </th>
                                <th fieldmap="MID_NO" sortable="true" width="10%" align="left">
                                  ST#
                                </th>
                                 <th fieldmap="ITM_NAME" sortable="true" width="25%" align="left">
                                    <%=GetLocalResourceObject("Item").ToString()%>
                                </th>
                                
                                 <th fieldmap="ITM_UOM_CODE" sortable="true" width="10%" align="left">
                                    <%=GetLocalResourceObject("UOM").ToString()%>
                                </th>
                                
                                  <th fieldmap="MAD_QTY_ACCEPTED" sortable="true" width="10%" align="right">
                                    <%=GetLocalResourceObject("ReceivedQty").ToString()%>
                                </th>
                                <th fieldmap="BALANCE" sortable="true" width="20%" align="right">
                                    <%=GetLocalResourceObject("StockQty").ToString()%>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>

<%--                   <asp:GridView ID="grdReceived" runat="server" Width="100%" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmptyReceived" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText='<%$ resources:WoNo %>'>
                                <ItemTemplate>
                                    <asp:Label ID="lblWONumber" runat="server" Text='<%# Eval("WIH_NO") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfWOPK" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdfSBDPK" runat="server" Value='<%# Eval("SBD_PK") %>' />
                                    <asp:HiddenField ID="hdfGRNPK" runat="server" Value='<%# Eval("GRH_PK") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='<%$ resources:Item %>'>
                                <ItemTemplate>
                                    <asp:Label ID="lblItem" runat="server" Text='<%# Eval("ITM_NAME") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%# Eval("ITM_PK") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="65%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='<%$ resources:UOM %>'>
                                <ItemTemplate>
                                    <asp:Label ID="lblUOM" runat="server" Text='<%# Eval("ITM_UOM_CODE") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfUOMPK" runat="server" Value='<%# Eval("ITM_UOM") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText='<%$ resources:ReceivedQty %>'>
                                <ItemTemplate>
                                    <asp:Label ID="lblReceivedQty" runat="server" Text='<%# Eval("GRD_QTY_RECEIVED") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfReceivedQty" runat="server" Value="0" />
                                </ItemTemplate>
                                <ItemStyle Width="10%" CssClass="amount-numeric" />
                                <HeaderStyle CssClass="amount-numeric" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stock Qty">
                                <ItemTemplate>
                                    <asp:Label ID="lblStockQty" runat="server" Text='<%# Eval("BALANCE") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" CssClass="amount-numeric" />
                                <HeaderStyle CssClass="amount-numeric" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>--%>

        

          <%--      <asp:GridView ID="grdConvert" runat="server" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                    <EmptyDataTemplate>
                        <asp:Label ID="lblEmptyConvert" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText='<%$ resources:WoNo %>'>
                            <ItemTemplate>
                                <asp:Label ID="lblConvWONo" runat="server" Text='<%# Eval("WIH_NO") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText='<%$ resources:Item %>'>
                            <ItemTemplate>
                                <asp:Label ID="lblConvItem" runat="server" Text='<%# Eval("ITM_NAME") %>'></asp:Label>
                                <asp:HiddenField ID="hdfConvItemPK" runat="server" Value='<%# Eval("ITM_PK") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="65%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText='<%$ resources:UOM %>'>
                            <ItemTemplate>
                                <asp:Label ID="lblConvUOM" runat="server" Text='<%# Eval("ITM_UOM_CODE") %>'></asp:Label>
                                <asp:HiddenField ID="hdfConvUOMPK" runat="server" Value='<%# Eval("ITM_UOM") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="5%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText='<%$ resources:ConvertedQty %>'>
                            <ItemTemplate>
                                <asp:TextBox ID="txtConvQty" runat="server" Text='<%# Eval("BALANCE") %>' CssClass="small-a numeric"
                                    onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqReceived" CssClass="star" SetFocusOnError="true"
                                    ValidationGroup="Convert" runat="server" ControlToValidate="txtConvQty"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EnterValidQuantity %>">
                                </asp:RequiredFieldValidator>
                            </ItemTemplate>
                            <ItemStyle Width="20%" CssClass="amount-numeric" />
                            <HeaderStyle CssClass="amount-numeric" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>--%>
        <%--    </div>
        </div>--%>




              <div id="divData">
                </div>

        <asp:HiddenField ID="hdfAppType" runat="server" />
        <asp:HiddenField ID="hdfAppSubType" runat="server" />
        <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
        <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
        <asp:HiddenField ID="hdnModify" runat="server" Value="0" />
        <asp:HiddenField ID="hdnCancel" runat="server" Value="0" />
             <asp:HiddenField ID="USER_PK" runat="server" Value="0" />
             <asp:HiddenField ID="BIZUNIT" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="ConvertDetails" Value="" />
            <asp:HiddenField ID="MAH_PK" runat="server" Value="0" />
    </div>
</asp:Content>
