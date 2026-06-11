<%@ Page Title="<%$ Resources:Captions,Title_POS %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PurchaseOrderNonStockListing.aspx.cs"
    Inherits="ERPSMS_v01.PurchaseOrderManagement.PurchaseOrderNonStockListing" Theme="ClassicExt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/PurchaseOrderManagement/PurchaseOrderNonStockListing.js.axd"
        type="text/javascript"></script>
    <style type="text/css">
        /*Start ToolTip Css for any dom element */
        [tooltip]:before
        {
            position: absolute;
            content: attr(tooltip);
            opacity: 0;
            top: -50;
        }
        
        [tooltip]:hover:before
        {
            opacity: 1;
            background: #feffcd;
            border: 1px solid black;
            padding: 2px;
            margin-top: 12px;
        }
        
        
        [tooltip]:not([tooltip-persistent]):before
        {
            pointer-events: none;
        }
        /*End ToolTip Css for any dom element */
    </style>
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
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    ToolTip="<%$resources:Controls,Add %>" OnClientClick="javascript:return AddNew();"
                                    TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Refresh%>"
                                    ToolTip="<%$resources:Controls,Refresh %>" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="8" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
            <div id="divVendorData">
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="content-wrapper">
        <div style="display: none">
            <div id="searchwrap" class="search-wrap-custom1">
                <label for="SearchType">
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="medium" onchange="javascript:SetSearchType();"
                    EnableViewState="false">
                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                    </asp:ListItem>
                    <asp:ListItem Value="POH_NO" Text="<%$ Resources:BindValues, PONumber%>">
                    </asp:ListItem>
                    <asp:ListItem Value="VEN_NAME" Text="<%$ Resources:BindValues, VendorName%>">
                    </asp:ListItem>
                    <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:BindValues, RequiredFor%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Date" Text="<%$ Resources:BindValues, DateRange%>">
                    </asp:ListItem>
                    <asp:ListItem Value="POH_ITEM_FULLTEXT" Text="<%$ Resources:BindValues, Item%>">
                    </asp:ListItem>
                </asp:DropDownList>
                <div id="divSearchDtls">
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="2">
                    </asp:TextBox>
                </div>
                <div id="divDateOld">
                    <span>
                        <%=Resources.Controls.FromDate%></span>
                    <asp:TextBox ID="FromDate" runat="server" TabIndex="3" EnableViewState="false" CssClass="Uidate-picker">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfFrmDate" runat="server" />
                    <span>
                        <%=Resources.Controls.ToDate%></span>
                    <asp:TextBox ID="ToDate" runat="server" TabIndex="4" EnableViewState="false" CssClass="Uidate-picker">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfToDate" runat="server" />
                </div>
                <div id="divFilterStatus">
                    <label for="FilterStatus">
                        <%=Resources.Controls.Status%></label>
                    <asp:DropDownList ID="FilterStatus" runat="server" CssClass="medium" TabIndex="5"
                        onchange="javascript:BindGrid();" EnableViewState="false">
                        <asp:ListItem Value="0" Text="<%$ Resources:BindValues, StatusFilterAll%>"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Approved"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Cancelled"></asp:ListItem>
                        <asp:ListItem Value="4" Text="Short closure"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <%-- <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false">
                </asp:TextBox>--%>
                <asp:HiddenField ID="hdnRoleID" runat="server" Value="0"></asp:HiddenField>
                <asp:HiddenField ID="hdnShortCloseGroup" runat="server" Value="0"></asp:HiddenField>
                <asp:HiddenField ID="hdfAppType" runat="server" />
                <asp:HiddenField ID="hdfAppSubType" runat="server" />
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClientClick="javascript:return BindGrid();"
                    EnableViewState="false" />
                <div style="float: right; display: none">
                    <asp:Button ID="btnAdvSearch" runat="server" Text="Advance Search" OnClientClick="javascript:return ShowAdvSearch();" />
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <%--=======New Search Region Start======================----------------=====--%>
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
        <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
            <tr>
                <td>
                    <div class="div2col-S padgtop7">                       
                        <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="input-small" TabIndex="1" onkeydown="return CheckKey(event)"
                            onpaste="return false;"></asp:TextBox>
                        <asp:HiddenField ID="hdfFromDate" runat="server" />
                         <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" CssClass="middle-lbl-small-d" AssociatedControlID="txtToDate"></asp:Label>
                        <asp:TextBox runat="server" ID="txtToDate" CssClass="input-small" TabIndex="2" onkeydown="return CheckKey(event)"
                            onpaste="return false;"></asp:TextBox>
                        <div class="clear">
                        </div>                       
                     
                        <div style="display: none">
                            <asp:Label runat="server" ID="lblIONo" Text="<%$ resources:IoNo %>" AssociatedControlID="txtIONo"></asp:Label>
                            <asp:TextBox runat="server" ID="txtIONo" CssClass="input-small" TabIndex="2"></asp:TextBox>
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="div2col-S padgtop7">   
                        <asp:Label runat="server" ID="lblReqFor" Text="<%$ resources:ReqFor %>" CssClass="middle-lbl-small" AssociatedControlID="txtReqFor"></asp:Label>
                        <asp:TextBox runat="server" ID="txtReqFor" CssClass="input-small-a" TabIndex="3"></asp:TextBox>
                        <asp:HiddenField ID="hdfReqFor" runat="server" /> 
                         <asp:Label runat="server" ID="lblTrnStatus" Text="<%$ resources:TrnStatus %>" CssClass="middle-lbl-small-b" AssociatedControlID="ddlTrnStatus"></asp:Label>
                        <asp:DropDownList ID="ddlTrnStatus" runat="server" CssClass="lbl-35-2perc" TabIndex="4"
                            EnableViewState="false">
                        </asp:DropDownList>                       
                        <div class="clear">
                        </div>
                       
                        <div style="display: none">
                            <asp:Label runat="server" ID="lblPrNo" Text="<%$ resources:PrNo %>" AssociatedControlID="txtPrNo"></asp:Label>
                            <asp:TextBox runat="server" ID="txtPrNo" CssClass="Input-small" TabIndex="4"></asp:TextBox>
                            <div class="clear">
                            </div>
                        </div>                     
                    </div>
                </td>
            </tr>
        </table>
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S div-separatn">
                     <asp:Label runat="server" ID="lblVendor" Text="<%$ resources:Vendor %>" AssociatedControlID="txtVendor"></asp:Label>
                        <asp:TextBox runat="server" ID="txtVendor" TabIndex="5"  CssClass="select-half margnbotm0">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfVendor" runat="server" />
                        <div class="clear">
                        </div>
                    </div>
                </td>
                <td>
                    <div class="div2col-S div-separatn">
                     <asp:Label runat="server" ID="lblPoNumber" Text="<%$ resources:PoNo %>" CssClass="middle-lbl-small" AssociatedControlID="txtPONumber"></asp:Label>
                        <asp:TextBox runat="server" ID="txtPONumber" CssClass="input-small-a margnbotm0" TabIndex="6">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfPONumber" runat="server" Value="0" />  
                         <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:POStatus %>" class="lbl-19perc"
                             AssociatedControlID="ddlStatus"></asp:Label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b margnbotm0" TabIndex="7" EnableViewState="false">
                            <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterNotClosed %>" Text="<%$ Resources:BindValues, StatusFilterNotClosed%>"
                                Selected="True">
                            </asp:ListItem>
                            <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterAll %>" Text="<%$ Resources:BindValues, StatusFilterAll%>">
                            </asp:ListItem>
                            <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterClosed %>" Text="<%$ Resources:BindValues, StatusFilterClosed%>">
                            </asp:ListItem>
                        </asp:DropDownList>                        
                          <asp:Label ID="lblSearchButton" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0" Width="5px" AssociatedControlID="btnSearch"></asp:Label>
                        <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                            ToolTip="<%$ resources:Controls,Search %>" TabIndex="7" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                            OnClientClick="javascript:return BindGrid();" />
                        <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="7"
                            ToolTip="<%$ resources:Controls,Clear %>" Style="margin-bottom: 0px!important;margin-top: 2px;" SkinID="clear-ext" OnClientClick="javascript:return SearchAutoInit();" />                  
                    </div>
                </td>
            </tr>
        </table>
        <div class="clear">
        </div>
        <%--============End New Search Region=========================-----------------------------------=--%>
        <div class="gridwrap">
            <table rules="all" id="grdPODetails" grandtype="GrandGrid" pagesize="20" paging="true"
                editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <%--[POH_PK], [POH_NO],CREATED_BY,[POH_DATE],[VEN_NAME],USER_STATUS--%>
                        <th fieldmap="POH_PK" isvisible="false">
                        </th>
                        <th fieldmap="REF_ID" isvisible="false">
                        </th>
                        <th fieldmap="POH_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="POH_ITEM_FULLTEXT" isvisible="false">
                        </th>
                        <th fieldmap="POH_NO" align="left" sortable="true" width="10%">
                            <%=Resources.Controls.PoNumber%>
                        </th>
                        <th fieldmap="VEN_NAME" sortable="true" width="22%">
                            <%=Resources.Controls.VendorName%>
                        </th>
                        <th fieldmap="POH_ITEM_TEXT" sortable="true" width="35  %">
                            <%=Resources.Controls.ItemDetails%>
                        </th>
                        <th fieldmap="DPT_NAME" sortable="true" width="9%">
                            <%=Resources.Controls.RequiredFor%>
                        </th>
                        <th fieldmap="POH_DATE" sortable="true" width="8%">
                            <%=Resources.Controls.PODate%>
                        </th>
                        <th fieldmap="POH_STATUS_TEXT" sortable="true" width="8%" align="left">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th type="Template" width="8%">
                            <div>
                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$Resources:Controls,Edit %>" TabIndex="7"
                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>" TabIndex="7"
                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$Resources:Controls,View %>" TabIndex="7"
                                    SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbPrint" ToolTip="<%$Resources:Controls,Print %>" TabIndex="7"
                                    SkinID="btnPrint" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
                                <asp:ImageButton runat="server" ID="imbShortClose" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'SHORTCLOSE')" TabIndex="7"
                                    SkinID="btnclose" alt="<%$Resources:Controls,ShortClose%>" title="<%$Resources:Controls,ShortClose%>" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
            <div class="clear">
            </div>
        </div>
        <div id="divShortClose" title="<%=Resources.Controls.ShortClose%>">
            <div class="divcolmiddle-S">
                <label for="Remarks">
                    <%=Resources.Controls.Remark%>*</label>
                <asp:TextBox runat="server" ID="Remarks" TabIndex="13" MaxLength="200" EnableViewState="false">
                </asp:TextBox>
                <label for="RefNo">
                    <%=Resources.Controls.RefNo%></label>
                <asp:TextBox runat="server" ID="RefNo" TabIndex="13" MaxLength="14" EnableViewState="false">
                </asp:TextBox>
                <div class="clear">
                </div>
                <asp:Label ID="lbnSpace" runat="server" AssociatedControlID="btnAddConv"></asp:Label>
                <asp:HiddenField ID="POID" runat="server" Value="0"></asp:HiddenField>
                <asp:Button runat="server" ID="btnAddConv" Text="<%$Resources:Controls,ShortClose%>"
                    EnableViewState="false" class="inputbtn" Width="100px" Height="20px" OnClientClick="javascript:return ConfirmSaveShortClose();" />
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
