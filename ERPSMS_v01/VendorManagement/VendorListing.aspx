<%@ Page Title="<%$ Resources:Captions,Title_VendorManagement %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" EnableEventValidation="false" 
    AutoEventWireup="true" CodeBehind="VendorListing.aspx.cs" Inherits="ERPSMS_v01.VendorManagement.VendorListing"
    Theme="ClassicExt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/VendorManagement/VendorListing.js.axd?v=20260603activefilter3" type="text/javascript"></script>
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
                                <%--<asp:ImageButton ID="imbAdd" runat="server" SkinID="btnInner-New" OnClientClick="javascript:return AddNew();"
                                    TabIndex="10" EnableViewState="false" />--%>
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    OnClientClick="javascript:return AddNew();" TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <%--<asp:ImageButton ID="imbReset" runat="server" SkinID="btnInner-refresh" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="8" />--%>
                               <%-- <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                    OnClientClick="javascript:return ResetPage();" TabIndex="8" />--%>
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
         
        </div>
    </div>
    <div class="content-wrapper">
    <div style="display:none">
        <div id="searchwrap" class="search-wrap-custom1">
            <label for="SearchType">
                <%=Resources.Controls.SearchBy%></label>
            <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType();"
                EnableViewState="false">
               <%-- <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>"></asp:ListItem>--%>
                <asp:ListItem Value="VEN_CODE" Text="<%$ Resources:BindValues, VendorCode%>"></asp:ListItem>
                <asp:ListItem Value="VEN_NAME" Text="<%$ Resources:BindValues, VendorName%>"></asp:ListItem>
                <%--<asp:ListItem Value="VEN_TYPE" Text="<%$ Resources:BindValues, VendorType%>"></asp:ListItem>--%>
                <asp:ListItem Value="VEN_PHONE" Text="<%$ Resources:BindValues, VendorPhone%>"></asp:ListItem>
            </asp:DropDownList>
            <div id="divSearchDtls">
                <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false">
                </asp:TextBox>
            </div>
             
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0"></asp:HiddenField>
            <div id="divVendorData">
            </div>

            <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClientClick="javascript:return BindGrid();"
                EnableViewState="false" />
            <div style="float: right; display: none">
                <asp:Button ID="btnAdvSearch" runat="server" Text="Advance Search" OnClientClick="javascript:return ShowAdvSearch();" />
            </div>
            <div class="clear">
            </div>
        </div>
    </div>
        <%--=========Advance Search Region Begin=========================--%>
        <div class="search-colapse">
            <table>
                <tr>
                    <td>
                        <h1>
                            <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString()%></h1>
                    </td>
                    <td>
                        <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                            ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                            TabIndex="65" />
                        <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                            ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                            TabIndex="66" />
                    </td>
                </tr>
            </table>
        </div>
        <%--------------colpase btn----------%>
        <div class="clear">
        </div>
        <table class="table-devide" id="tbladvancedSearch" style="/*margin-top: 8px; */ background: #f2f2f2;">
            <tr>
                <td>
                    <div class="div2col-S padgtop7">
                        <div class="clear">
                        </div>
                        <asp:Label runat="server" ID="lblVendorCode" Text="<%$ Resources:BindValues, VendorCode%>" AssociatedControlID="txtVendorCode"></asp:Label>
                        <asp:TextBox runat="server" ID="txtVendorCode" TabIndex="2" CssClass="input-w65per"></asp:TextBox>                                            
                        <div class="clear">
                        </div>
                    </div>
                </td>
                <td>
                    <div class="div2col-S padgtop7">
                        <label for="chkActive"><%=Resources.Controls.Active%></label>
                        <input type="checkbox" id="chkActive" checked="checked" />
                        <div class="clear">
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S div-separatn">
                         <asp:Label runat="server" ID="lblVendorName" Text="<%$ Resources:BindValues,VendorName %>" AssociatedControlID="txtVendorName"></asp:Label>
                        <asp:TextBox runat="server" ID="txtVendorName" CssClass="select-half-b margnbotm0" TabIndex="3">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfVendor" runat="server" />                                                              
                    </div>
                </td>
                <td>
                    <div class="div2col-S div-separatn">
                        <asp:Label runat="server" ID="lblPoNumber" Text="<%$ Resources:BindValues, VendorPhone%>" AssociatedControlID="txtPhone" ></asp:Label>
                        <asp:TextBox runat="server" ID="txtPhone" TabIndex="4" CssClass="input-small margnbotm0"> </asp:TextBox>                       

                         <asp:Label runat="server" ID="lblVendorType" Text="<%$ Resources:Controls, Type%>" AssociatedControlID="ddlVendorType" CssClass="middle-lbl-small-d"></asp:Label> 
                        <asp:DropDownList runat="server" ID="ddlVendorType"    TabIndex="5" CssClass="select-small-b margnbotm0 margn-rgt2">  </asp:DropDownList>                    
                         <asp:Label ID="Label1" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>
                        <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                            TabIndex="6" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                            OnClientClick="javascript:return BindGrid();" />
                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="6" Style="margin-bottom: 0px!important;
                            margin-top: 2px;" ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                            OnClientClick="javascript:return ClearSearch();" />
                    </div>
                </td>
            </tr>
        </table>
        <div class="clear">
        </div>
        <%--=============End Advance Search Region=====================--%>
         <div class="gridwrap">
            <table rules="all" id="grdVendorDetails" grandtype="GrandGrid" pagesize="20" paging="true"
                editfunction="GridAddressAction" editable="true" width="100%"  class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="REF_ID" isvisible="false">
                        </th>
                        <th fieldmap="VEN_PK" isvisible="false">
                        </th>
                        <th fieldmap="VEN_TYPE" isvisible="false">
                        </th>
                        <th fieldmap="VEN_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="VEN_CODE" align="left" sortable="true" width="15%">
                            <%=Resources.Controls.VendorCode%>
                        </th>
                        <th fieldmap="VEN_NAME" width="13%" sortable="true">
                            <%=Resources.Controls.VendorName%>
                        </th>
                        <th fieldmap="VEN_CONT_NAME" width="13%" sortable="true">
                            <%=Resources.Controls.ContactName%>
                        </th>
                          <th fieldmap="VEN_ADDR1" width="17%" sortable="true">
                            <%=Resources.Controls.Address%>
                        </th>
                        <th fieldmap="VEN_CITY" width="8%" sortable="true">
                            <%=Resources.Controls.City%>
                        </th>
                        <th fieldmap="VEN_PHONE" width="6%" sortable="true">
                            <%=Resources.Controls.Phone%>
                        </th>
                        <%-- Local/Import--%>
                         <th fieldmap="VEN_PO_TYPE_TEXT" width="6%" sortable="true">
                            <%=Resources.Controls.Type%>
                        </th>
                        <th fieldmap="VEN_TYPE_TEXT" width="5%" sortable="true" isvisible="false">
                            <%=Resources.Controls.Type%>
                        </th>
                        <th fieldmap="VEN_STATUS_TEXT" width="8%" sortable="true">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                      <%--  including active/in active status--%>
                         <th fieldmap="VEN_ACTIVE" width="5%" align="center">
                                        <%=Resources.Controls.Active%>
                         </th>           

                        <asp:HiddenField runat="server" ID="UserRoles" Value="0" />
                         <asp:HiddenField runat="server" ID="hdnFinalApprovar" Value="0" />
                        <th type="Template" width="14%">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$ resources:Controls,Edit %>" SkinID="imbactiongrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbUpdate" SkinID="imbeditgrid" Style="display: none"
                                    OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'UPDATE')" />
                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$ resources:Controls,Delete %>" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$ resources:Controls,View %>" SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbEvaluate" ToolTip="<%$ resources:Controls,Evaluate %>" SkinID="btnevaluate" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EVALUATE')" />
                                <asp:ImageButton runat="server" ID="imbPrint" ToolTip="<%$ resources:Controls,Print %>" SkinID="btnPrint" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')"
                                    Style="display: none" />
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
