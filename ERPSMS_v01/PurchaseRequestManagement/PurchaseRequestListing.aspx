<%@ Page Title="<%$ Resources:Captions,Title_PurchaseRequest %>" Language="C#" Theme="ClassicExt"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="PurchaseRequestListing.aspx.cs"
    Inherits="ERPSMS_v01.PurchaseRequestManagement.PurchaseRequestListing" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/PurchaseRequestManagement/PurchaseRequestListing.js.axd"
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
                                    OnClientClick="javascript:return AddNew();" ToolTip="<%$resources:ErpRes,Add %>"
                                    TabIndex="10" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button ID="btnReset" runat="server" SkinID="btnInner-refresh" ToolTip="<%$resources:Controls,Refresh %>"
                                    Text="<%$Resources:Controls,Refresh%>" OnClientClick="javascript:return ResetPage();"
                                    TabIndex="8" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAppType" runat="server" />
            <asp:HiddenField ID="hdfAppSubType" runat="server" />
            <asp:HiddenField ID="hdnClosePO" runat="server" Value="0" />
            <asp:HiddenField ID="hdnModifyPR" runat="server" Value="0" />
            <asp:HiddenField ID="hdnCancelPR" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
            <asp:HiddenField ID="hdfShowTransactionTypeFilter" runat="server" Value="0" />
        </div>
        <div class="clear">
        </div>
    </div>
    <%--  <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.PurchaseRequestList%>
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();"
                EnableViewState="False" TabIndex="6" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="False" TabIndex="7" />
               
        </div>
    </div>--%>
    <div class="content-wrapper">
        <%--*************************************--%>
        <%--         <div style="display: none">--%>
        <%--==================================--%>
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
                        <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                        <asp:TextBox runat="server" ID="txtFromDate" CssClass="input-small" TabIndex="3"
                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                        <asp:HiddenField ID="hdfFromDate" runat="server" />
                        <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"
                            CssClass="middle-lbl-small-d"></asp:Label>
                        <asp:TextBox runat="server" ID="txtToDate" CssClass="input-small" TabIndex="4" onkeydown="return CheckKey(event)"
                            onpaste="return false;"></asp:TextBox>
                        <%--     <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate" CssClass="lbl-25-1perc"></asp:Label>
                <asp:TextBox ID="txtFromDate" runat="server" TabIndex="51" EnableViewState="false" CssClass="input-small-a Uidate-picker" >
                </asp:TextBox>
                <asp:HiddenField ID="hdfFromDateFilter" runat="server" />
                 <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"
                            CssClass="lbl-19-6perc"></asp:Label>
                <asp:TextBox ID="txtToDate" runat="server" TabIndex="52" EnableViewState="false" CssClass="input-small-a Uidate-picker">
                </asp:TextBox>
                <asp:HiddenField ID="hdfTODateFilter" runat="server" />--%>
                        <div class="clear">
                        </div>
                        <asp:Label runat="server" ID="lblItemName" Text="<%$ Resources:Controls, ItemName%>"
                            AssociatedControlID="txtItemname"></asp:Label>
                        <asp:TextBox runat="server" ID="txtItemname" CssClass="input-half" TabIndex="55"></asp:TextBox>
                        <%-- <asp:Label ID="lblSearchButton" runat="server" AssociatedControlID="btnSearch"></asp:Label>--%>
                    </div>
                </td>
                <td>
                    <div class="div2col-S padgtop7">
                        <asp:Label runat="server" ID="lblReqStore" Text="<%$ resources:ReqStore %>" AssociatedControlID="txtReqStore"></asp:Label>
                        <asp:TextBox ID="txtReqStore" runat="server" CssClass="select-small-e" TabIndex="53"
                            EnableViewState="false">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfReqStore" runat="server" />
                        <asp:Label runat="server" ID="lblIONo" Text="<%$ resources:IoNo %>" CssClass="lbl-13perc"
                            AssociatedControlID="txtIONo"></asp:Label>
                        <asp:TextBox runat="server" ID="txtIONo" TabIndex="54" CssClass="input-small-c0"></asp:TextBox>
                        <asp:HiddenField ID="hdfIONo" runat="server" />
                        <div class="clear">
                        </div>
                        <asp:Label runat="server" ID="lblReqDept" Text="<%$ resources:ReqDept %>" AssociatedControlID="txtReqDept"></asp:Label>
                        <asp:TextBox runat="server" ID="txtReqDept" TabIndex="56" CssClass="select-small-e"></asp:TextBox>
                        <asp:HiddenField ID="hdfReqDept" runat="server" />
                        <asp:Label runat="server" ID="lblReqBy" Text="<%$ resources:ReqBy %>" AssociatedControlID="txtReqBy"
                            CssClass="lbl-13perc"></asp:Label>
                        <asp:TextBox runat="server" ID="txtReqBy" TabIndex="57" CssClass="input-small-c0"></asp:TextBox>
                        <asp:HiddenField ID="hdfReqBy" runat="server" />
                        <div class="clear">
                        </div>
                        <%-----------   Plant ----------------%>
                        <%--<div id="divPlantCode" class="div2col-S">
                              <asp:Label runat="server" ID="lblPlantCode" Text="<%$ Resources:Controls,CompanyPlant%>" AssociatedControlID="ddlPlantCode"
                                            ></asp:Label>                                        
                                    <asp:DropDownList ID="ddlPlantCode" runat="server" CssClass="select-small-d" TabIndex="8" EnableViewState="false">                   
                                    </asp:DropDownList>
                                </div>
                        <div class="clear">
                        </div>--%>
                        <%--------    End Plant ----------%>
                    </div>
                </td>
            </tr>
        </table>
        <table class="table-devide">
            <tr>
                <td>
                    <div class="div2col-S div-separatn">
                        <asp:Label runat="server" ID="lblTrnStatus" Text="<%$ resources:TransactionStatus %>"
                            AssociatedControlID="ddlTrnStatus"></asp:Label>
                        <asp:DropDownList ID="ddlTrnStatus" runat="server" CssClass="input-small-c margnbotm0 margn-rgt2"
                            TabIndex="58" EnableViewState="false">
                        </asp:DropDownList>
                        <asp:Label runat="server" ID="lblPlantCode" Text="<%$ Resources:Controls,CompanyPlant%>"
                            AssociatedControlID="ddlPlantCode" CssClass="lbl-15-1perc"></asp:Label>
                        <asp:DropDownList ID="ddlPlantCode" runat="server" CssClass="select-small-b margnbotm0 margn-rgt2"
                            TabIndex="59" EnableViewState="false">
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <div class="div2col-S div-separatn">
                        <asp:Label runat="server" ID="lblPrNumber" Text="<%$ resources:PrNo %>" AssociatedControlID="txtPRNumber"></asp:Label>
                        <asp:TextBox runat="server" ID="txtPRNumber" TabIndex="60" CssClass="select-small-e margnbotm0 margn-rgt2 h14">
                        </asp:TextBox>
                        <asp:HiddenField ID="hdfPRNumber" runat="server" Value="0" />
                        <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:PrStatus %>" AssociatedControlID="ddlStatus"
                            CssClass="lbl-13perc"></asp:Label>
                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b margnbotm0 margn-rgt2"
                            TabIndex="61" EnableViewState="false">
                            <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterNotClosed %>" Text="<%$ Resources:BindValues, StatusFilterNotClosed%>"
                                Selected="True">
                            </asp:ListItem>
                            <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterAll %>" Text="<%$ Resources:BindValues, StatusFilterAll%>">
                            </asp:ListItem>
                            <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterClosed %>" Text="<%$ Resources:BindValues, StatusFilterClosed%>">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:Label ID="Label1" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>
                        <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                            TabIndex="62" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                            OnClientClick="javascript:return BindGrid();" />
                        <asp:ImageButton ID="btnClear" runat="server" TabIndex="63" Style="margin-bottom: 0px!important;
                            margin-top: 2px;" ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                            OnClientClick="javascript:return SearchAutoInit();" />
                    </div>
                </td>
            </tr>
        </table>
        <div class="clear">
        </div>
        <%--======================================--%>
        <%--     </div>--%>
        <%--*************************************--%>
        <%--  ----------------------------------------%>
        <div style="display: none">
            <%--  ----------------------------------------%>
            <div id="searchwrap" class="search-wrap-custom1">
                <label for="SearchType">
                    <%=Resources.Controls.SearchBy%></label>
                <asp:DropDownList ID="SearchType" runat="server" CssClass="medium" TabIndex="1" onchange="javascript:SetSearchType();"
                    EnableViewState="false">
                    <%--  <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                </asp:ListItem>--%>
                    <asp:ListItem Value="PRH_NO" Text="<%$ Resources:BindValues, PurchaseRequestNo%>">
                    </asp:ListItem>
                    <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:BindValues, RequestingStore%>">
                    </asp:ListItem>
                    <asp:ListItem Value="Date" Text="<%$ Resources:BindValues, DateRange%>">
                    </asp:ListItem>
                    <asp:ListItem Value="SOH_NO" Text="<%$ Resources:Controls, IONumber%>">
                    </asp:ListItem>
                    <asp:ListItem Value="CON_NAME" Text="<%$ Resources:Controls, RequestedDept%>"> 
                    </asp:ListItem>
                    <%--PRH_ISSUE_DEPT--%>
                    <asp:ListItem Value="PRH_USER" Text="<%$ Resources:Controls, RequestedBy%>">
                    </asp:ListItem>
                    <asp:ListItem Value="CMP_DISPLAY_CODE" Text="<%$ Resources:Controls, CompanyPlant%>">
                    </asp:ListItem>
                </asp:DropDownList>
                <div id="divSearchDtls">
                    <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false" TabIndex="2">
                    </asp:TextBox>
                </div>
                <div id="divDate">
                    <label for="FromDate">
                        <%=Resources.Controls.FromDate%></label>
                    <asp:TextBox ID="FromDate" runat="server" TabIndex="3" EnableViewState="false" CssClass="Uidate-picker">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfFrmDate" runat="server" />
                    <label for="ToDate">
                        <%=Resources.Controls.ToDate%></label>
                    <asp:TextBox ID="ToDate" runat="server" TabIndex="4" EnableViewState="false" CssClass="Uidate-picker">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfToDate" runat="server" />
                </div>
                <div id="divFilterStatus">
                    <label for="FilterStatus">
                        <%=Resources.Controls.Status%></label>
                    <asp:DropDownList ID="FilterStatus" runat="server" CssClass="medium" TabIndex="5"
                        onchange="javascript:BindGrid();" EnableViewState="false">
                        <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterAll %>" Text="<%$ Resources:BindValues, StatusFilterAll%>">
                        </asp:ListItem>
                        <%--<asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterClosed %>" Text="<%$ Resources:BindValues, StatusFilterClosed%>">
                    </asp:ListItem>--%>
                        <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterNotClosed %>" Text="<%$ Resources:BindValues, StatusFilterNotClosed%>"
                            Selected="True">
                        </asp:ListItem>
                        <%--<asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterCancelled %>" Text="<%$ Resources:BindValues, StatusFilterCancelled%>"> </asp:ListItem>--%>
                    </asp:DropDownList>
                    <label for="TransactionStatus">
                        <%=Resources.Controls.TransactionTypes%>
                    </label>
                    <asp:DropDownList ID="TransactionStatus" runat="server" CssClass="medium" TabIndex="5"
                        onchange="javascript:BindGrid();" EnableViewState="false">
                    </asp:DropDownList>
                </div>
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="5" OnClientClick="javascript:return BindGrid();"
                    EnableViewState="false" />
                <div class="clear">
                </div>
            </div>
            <%--  ----------------------------------------%>
        </div>
        <%--  ----------------------------------------%>
        <div class="gridwrap">
            <table rules="all" id="grdPurchaseRequest" grandtype="GrandGrid" pagesize="20" paging="true"
                width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap filter-arrow">
                <thead>
                    <tr>
                        <th fieldmap="PRH_DEPT" isvisible="false">
                        </th>
                        <th fieldmap="PRH_PK" isvisible="false">
                        </th>
                        <th fieldmap="PRH_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="CMP_LINE_COLOUR" isvisible="false">
                        </th>
                        <%--    <th fieldmap="CMP_DISPLAY_CODE" isvisible="false">
                        </th>--%>
                        <th fieldmap="USER_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="PRH_DEL_STATUS" isvisible="false">
                        </th>
                        <th fieldmap="POH_PO_FLAG" isvisible="false">
                            <%--1 => PO exists against this PR--%>
                        </th>
                        <th fieldmap="POH_PO_WKF_FLAG" isvisible="false">
                            <%--1 => PO exists against this PR and [POH_STATUS] > 0 (Not Draft) --%>
                        </th>
                        <th fieldmap="REF_ID" isvisible="false">
                            <%=Resources.Controls.ReferenceID%>
                        </th>
                        <th fieldmap="PRH_ITEM_FULL_TEXT" isvisible="false">
                        </th>
                        <th fieldmap="PRH_DATE" sortable="true" width="9%" align="left">
                            <%=Resources.Controls.RequestDate%>
                        </th>
                        <th fieldmap="PRH_NO" sortable="true" align="left" width="8%">
                            <%=Resources.Controls.RequestNo%>
                        </th>
                        <th fieldmap="CMP_DISPLAY_CODE" align="left" width="2%">
                        </th>
                        <th fieldmap="DPT_NAME" sortable="true" width="13%" align="left">
                            <%=Resources.Controls.RequestingStore%>
                        </th>
                        <th fieldmap="PRH_ITEM_TEXT" sortable="true" width="20%">
                            <%=Resources.Controls.ItemDetails%>
                        </th>
                        <th fieldmap="SOH_NO" sortable="true" width="9%">
                            <%=Resources.Controls.IONumber%>
                        </th>
                        <th fieldmap="PRH_ISSUE_DEPT_TEXT" sortable="true" width="11%" align="left">
                            <%=Resources.Controls.RequestedDept%>
                        </th>
                        <th fieldmap="PRH_USER" sortable="true" width="10%">
                            <%=Resources.Controls.RequestedBy%>
                        </th>
                        <th fieldmap="PRH_STATUS_TEXT" sortable="true" width="8%" align="left">
                            <%=Resources.Controls.Status%>
                        </th>
                        <th type="Template" width="9%" align="left">
                            <div style="text-align: left">
                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>"
                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PERFORMACTION')" />
                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:ErpRes,Delete %>"
                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$resources:ErpRes,View %>"
                                    SkinID="btnview" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                <asp:ImageButton runat="server" ID="imbPrint" ToolTip="<%$resources:ErpRes,Print %>"
                                    SkinID="btnPrint" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'PRINT')" />
                                <asp:ImageButton runat="server" ID="imbPRShorClose" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'SHORTCLOSEPR')"
                                    SkinID="btnclose" alt="<%$Resources:Controls,ShortClose%>" title="<%$Resources:Controls,ShortClose%>" />
                                <asp:ImageButton runat="server" ID="imbPRCancel" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'CANCELPR')"
                                    SkinID="cancel" alt="<%$Resources:Controls,Cancel%>" title="<%$Resources:Controls,Cancel%>" />
                            </div>
                        </th>
                    </tr>
                </thead>
            </table>
        </div>
        <div id="divDeletePR" title="<%=Resources.Captions.DeletePR%>">
            <div class="Button-container-popup">
                <asp:Button ID="btnDeletePR" SkinID="btnInner-add-dsd" runat="server" ToolTip="<%$ Resources:Controls, Continue%>"
                    Text="<%$ Resources:Controls, Continue%>" OnClientClick="javascript:return DeletePRDetails();" />
            </div>
            <div class="comments-container commentswrap">
                <div class="submitwrap commentswrap">
                    <asp:Label ID="lblDeleteComment" runat="server" AssociatedControlID="txtPRDeleteComment"
                        Text="<%$ resources:Controls,Comments %>"></asp:Label>
                    <asp:TextBox ID="txtPRDeleteComment" runat="server" CssClass="multiline-3line" TextMode="MultiLine"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
