<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    Theme="ClassicExt" CodeBehind="ProfitReport.aspx.cs" Inherits="ERPSMS_v01.Reports.ProfitReport" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="cntScript" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /* Date picker */
        
        .ui-datepicker
        {
            width: 18em;
            padding: .2em .2em 0;
            display: none;
            z-index: 999 !important;
        }
        
        
        /* scroll fixed table heaf */
        
        #data_container
        {
            border: 1px solid blue;
            width: 100%;
            height: 470px; /*649px;*/
            overflow-x: scroll;
        }
        
        #data_wrapper
        {
            width: 100%;
            border-right: 0px;
        }
        
        #data_headers, #data_body, #data_footer
        {
            width: 100%;
            padding: 0px;
            border-spacing: 0px;
            table-layout: fixed;
            position: sticky;
            top: 0;
            left:0;
        }
        
        #data-footer-wraper
        {
            width: 100%;
            border-right: 0px;
        }
        
        #data_headers
        {
            z-index: 999;
        }
        #data_footer
        {
            z-index: 999;
            bottom: 0px; /*top: 606px;*/
        }
        #data_body
        {
        }
        
        #data_headers th, #data_body td, #data_footer th
        {
            width: 142px;
            text-align: left;
            padding-left: 10px;
        }
        
        
        #data_headers th
        {
            color: #303030;
            background: #a0d5e3;
            border-bottom: 1px solid#D0D7E9;
            border-top: 1px solid#D0D7E9;
            border-left: none;
            border-right: none;
            font-size: 11px;
            padding: 3px 5px;
            vertical-align: middle;
            color: #005159;
            font-family: Verdana;
        }
        
        #data_body td
        {
            line-height: 12px;
            font-size: 11px;
            border-bottom: 1px solid #ddd !important;
            border-left: none;
            border-right: none;
            color: #303030;
            padding: 3px 5px;
            padding-left: 5px;
            vertical-align: inherit;
        }
        
        #data_footer th
        {
            color: #303030;
            background: #a0d5e3;
            border-bottom: 1px solid#D0D7E9;
            border-top: 1px solid#D0D7E9;
            border-left: none;
            border-right: none;
            font-size: 11px;
            padding: 3px 5px;
            vertical-align: middle;
            color: #005159;
            font-family: Verdana;
        }
        
        /* search collapse */
        
        .search-colapse
        {
            line-height: 7px;
            height: 20px !important;
        }
        .search-colapse input
        {
            margin-top: 3px !important;
        }
        
        /* page grid */
        .fixed-buttons-normal {
      padding-bottom: 21px !important;
        }
        .lastpro-label21-10-2020  
        {
            min-width:35% !important
            }
        .btnInner-size {
    font-size: 10px !important;
    padding: 2px 3px 2px 25px;
    line-height: 1.6em !important;
    border: 0;
    border: solid 1px #c04244;
    cursor: pointer;
    margin-bottom: 0 !important;
}    
.input-small {
    min-width: 18.3%;
    max-width: 18.3%;
    margin-bottom: 0 !important;
}
#content-container {
    min-height: 86vh !important;
}
    </style>
    <script type="text/javascript" language="javascript">
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtSearchFromDate", "hdfSearchFromDate", "txtSearchToDate", "hdfSearchToDate", false, false);
            ShowHideAdvancedSearch(0);
        }
        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
            if (flag) {
                $("[id$=tbladvancedSearch]").show();
                $("[id$=imbShowFilter]").hide();
                $("[id$=imbHideFilter]").show();
            }
            else {
                $("[id$=tbladvancedSearch]").hide();
                $("[id$=imbShowFilter]").show();
                $("[id$=imbHideFilter]").hide();
            }
            return false;
        }
    </script>
    <asp:Literal runat="server" ID="styleInsert" Text=""></asp:Literal>
</asp:Content>
<asp:Content ID="cntMain" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlPacking" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal" id="divFixedTab">
                <div class="Button-container">
                    <asp:Table ID="Table2" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlListing">
                                    <%--<li>
                                        <asp:Button ID="btnProcess" runat="server" ClientIDMode="Static" OnClick="ActionHandler"
                                            Text="Process" SkinID="btnInner-View" CommandName="PROCESS" ValidationGroup="report" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnSearch" runat="server" ClientIDMode="Static" OnClick="ActionHandler"
                                            Text="View" SkinID="btnInner-View" CommandName="VIEW" ValidationGroup="report" />
                                    </li>--%>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
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
                                    TabIndex="8" />
                                <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                    ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                    TabIndex="9" />
                            </td>
                        </tr>
                    </table>
                </div>
                <%--------------colpase btn----------%>
                <div class="clear">
                </div>
                <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                    <tr>
                        <%--<td>
                            <div class="div2col-S padgtop7">
                                <asp:Label runat="server" ID="lblSearchType" Text="<%$ Resources:Controls, ChooseType%>"
                                    AssociatedControlID="ddlSearchType"></asp:Label>
                                <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="input-half" TabIndex="10">
                                </asp:DropDownList>
                            </div>
                        </td>--%>
                        <td>
                            <div class="div2col-S padgtop7">
                            </div>
                        </td>
                    </tr>
                </table>
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S div-separatn">
                                <%--<asp:Label runat="server" ID="Label2" Text="<%$ Resources:Controls, LastProcessDate%>" CssClass="lbl-9perc"></asp:Label>--%>
                                <asp:Label runat="server" ID="Label2" Text="<%$ Resources:Controls, LastProcessDate %>"
                                    AssociatedControlID="lblLastProcess"></asp:Label>
                                <asp:Label runat="server" ID="lblLastProcess" CssClass="lastpro-label21-10-2020" Text="" AssociatedControlID="lblLastProcess"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S div-separatn">
                                <asp:Label runat="server" ID="lblSearchFromDate" Text="<%$ Resources:Controls, FromDate%>"
                                    CssClass="lbl-9perc" AssociatedControlID="txtSearchFromDate"></asp:Label>
                                <asp:TextBox ID="txtSearchFromDate" runat="server" CssClass="input-small" TabIndex="11">
                                </asp:TextBox>
                                <asp:HiddenField ID="hdfSearchFromDate" runat="server" Value="0"></asp:HiddenField>
                                <asp:Label ID="lblSearchToDate" runat="server" Text="<%$Resources:Controls,ToDate%>"
                                    CssClass="lbl-10-7perc" AssociatedControlID="txtSearchToDate"></asp:Label>
                                <asp:TextBox ID="txtSearchToDate" runat="server" CssClass="input-small" TabIndex="12">
                                </asp:TextBox>
                                <asp:HiddenField ID="hdfSearchToDate" runat="server" Value="0"></asp:HiddenField>
                                <asp:Button ID="btnProcess" runat="server" ClientIDMode="Static" OnClick="ActionHandler"
                                    Text="Process & View" SkinID="btnInner-View" CommandName="PROCESS" ValidationGroup="report" />
                                <asp:Button ID="btnSearch" runat="server" ClientIDMode="Static" OnClick="ActionHandler"
                                    Text="View" SkinID="btnInner-View" CommandName="VIEW" ValidationGroup="report" />
                                <%--<asp:Label ID="Label2" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>
                                <asp:ImageButton ID="ImageButton1" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                    TabIndex="11" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                    OnClick="ActionHandler" CommandName="SEARCH" />
                                <asp:ImageButton ID="btnClear" runat="server" TabIndex="12" Style="margin-bottom: 0px!important;
                                    margin-top: 2px;" ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                                    OnClick="ActionHandler" CommandName="CLEAR" />--%>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <div class="gridwraps">
                    <div id="data_container" runat="server" clientidmode="Static">
                        <%--<table id="data_headers">
                            <tr>
                                <th style="width: 100px">
                                    SC #
                                </th>
                                <th style="width: 100px">
                                    PO #
                                </th>
                                <th style="width: 100px">
                                    Contract Ref.#
                                </th>
                                <th style="width: 100px">
                                    PO Date
                                </th>
                                <th style="width: 100px">
                                    SC Date
                                </th>
                                <th style="width: 100px">
                                    Customer
                                </th>
                                <th style="width: 100px">
                                    Vendor
                                </th>
                                <th style="width: 100px">
                                    Previous Disp.Date
                                </th>
                                <th style="width: 100px">
                                    Current Disp. Date
                                </th>
                                <th style="width: 100px">
                                    ETD
                                </th>
                                <th style="width: 100px">
                                    ETA
                                </th>
                                <th style="width: 100px">
                                    Product Type
                                </th>
                                <th style="width: 100px">
                                    Brand Code
                                </th>
                                <th style="width: 100px">
                                    Total Carton
                                </th>
                                <th style="width: 100px">
                                    Buying Rate
                                </th>
                                <th style="width: 100px">
                                    Packaging Rate
                                </th>
                                <th style="width: 100px">
                                    Unclaim VAT 7%
                                </th>
                                <th style="width: 100px">
                                    Expense
                                </th>
                                <th style="width: 100px">
                                    Commision
                                </th>
                                <th style="width: 100px">
                                    Production Guarantee
                                </th>
                                <th style="width: 100px">
                                    Selling Rate
                                </th>
                                <th style="width: 100px">
                                    Other Income
                                </th>
                                <th style="width: 100px">
                                    Profit
                                </th>
                                <th style="width: 100px">
                                    Profit %
                                </th>
                            </tr>
                        </table>--%>
                        <%--<div id="data_wrapper">
                            <table id="data_body">
                                <tr>
                                    <td style="width: 100px">
                                        First row
                                    </td>
                                    <td style="width: 100px">
                                        PO-C-20200269-2
                                    </td>
                                    <td style="width: 100px">
                                        125853
                                    </td>
                                    <td style="width: 100px">
                                        43900
                                    </td>
                                    <td style="width: 100px">
                                        43900
                                    </td>
                                    <td style="width: 100px">
                                        Majestic Products B.V.
                                    </td>
                                    <td style="width: 100px">
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td style="width: 100px">
                                        01-07-2020
                                    </td>
                                    <td style="width: 100px">
                                        44050
                                    </td>
                                    <td style="width: 100px">
                                        44072
                                    </td>
                                    <td style="width: 100px">
                                        44101
                                    </td>
                                    <td style="width: 100px">
                                        Fishscale
                                    </td>
                                    <td style="width: 100px">
                                        1.44.570.11
                                    </td>
                                    <td style="width: 100px">
                                        20
                                    </td>
                                    <td style="width: 100px">
                                        320
                                    </td>
                                    <td style="width: 100px">
                                        79.2072
                                    </td>
                                    <td style="width: 100px">
                                        5.544504
                                    </td>
                                    <td style="width: 100px">
                                        10.9352942956574
                                    </td>
                                    <td style="width: 100px">
                                        0
                                    </td>
                                    <td style="width: 100px">
                                        60
                                    </td>
                                    <td style="width: 100px">
                                        592
                                    </td>
                                    <td style="width: 100px">
                                        0
                                    </td>
                                    <td style="width: 100px">
                                        116.313001704343
                                    </td>
                                    <td style="width: 100px">
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200373-3
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        SC-20200266-3
                                    </td>
                                    <td>
                                        PO-C-20200269-2
                                    </td>
                                    <td>
                                        125853
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        43900
                                    </td>
                                    <td>
                                        Majestic Products B.V.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        01-07-2020
                                    </td>
                                    <td>
                                        44050
                                    </td>
                                    <td>
                                        44072
                                    </td>
                                    <td>
                                        44101
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        1.44.570.11
                                    </td>
                                    <td>
                                        20
                                    </td>
                                    <td>
                                        320
                                    </td>
                                    <td>
                                        79.2072
                                    </td>
                                    <td>
                                        5.544504
                                    </td>
                                    <td>
                                        10.9352942956574
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        60
                                    </td>
                                    <td>
                                        592
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        116.313001704343
                                    </td>
                                    <td>
                                        19.6474665041119
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Last row
                                    </td>
                                    <td>
                                        PO-C-20200365-1
                                    </td>
                                    <td>
                                        5500012522-1
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        43910
                                    </td>
                                    <td>
                                        Shelby Group International, Inc.
                                    </td>
                                    <td>
                                        Safe Glove Co., Ltd.
                                    </td>
                                    <td>
                                        19-07-2020, 30-06-2020
                                    </td>
                                    <td>
                                        44057
                                    </td>
                                    <td>
                                        44066
                                    </td>
                                    <td>
                                        44097
                                    </td>
                                    <td>
                                        Fishscale
                                    </td>
                                    <td>
                                        6016OS
                                    </td>
                                    <td>
                                        44
                                    </td>
                                    <td>
                                        1540
                                    </td>
                                    <td>
                                        50.82462
                                    </td>
                                    <td>
                                        3.5577234
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        37.058769
                                    </td>
                                    <td>
                                        132
                                    </td>
                                    <td>
                                        2464
                                    </td>
                                    <td>
                                        0
                                    </td>
                                    <td>
                                        700.5588876
                                    </td>
                                    <td>
                                        28.4317730357143
                                    </td>
                                </tr>
                            </table>
                        </div>--%>
                        <%--<table id="data_footer">
                            <tr>
                                <th style="width: 100px">
                                    SC # Footer
                                </th>
                                <th style="width: 100px">
                                    PO #
                                </th>
                                <th style="width: 100px">
                                    Contract Ref.#
                                </th>
                                <th style="width: 100px">
                                    PO Date
                                </th>
                                <th style="width: 100px">
                                    SC Date
                                </th>
                                <th style="width: 100px">
                                    Customer
                                </th>
                                <th style="width: 100px">
                                    Vendor
                                </th>
                                <th style="width: 100px">
                                    Previous Disp.Date
                                </th>
                                <th style="width: 100px">
                                    Current Disp. Date
                                </th>
                                <th style="width: 100px">
                                    ETD
                                </th>
                                <th style="width: 100px">
                                    ETA
                                </th>
                                <th style="width: 100px">
                                    Product Type
                                </th>
                                <th style="width: 100px">
                                    Brand Code
                                </th>
                                <th style="width: 100px">
                                    50000
                                </th>
                                <th style="width: 100px">
                                    1000000
                                </th>
                                <th style="width: 100px">
                                    Packaging Rate
                                </th>
                                <th style="width: 100px">
                                    Unclaim VAT 7%
                                </th>
                                <th style="width: 100px">
                                    Expense
                                </th>
                                <th style="width: 100px">
                                    Commision
                                </th>
                                <th style="width: 100px">
                                    Production Guarantee
                                </th>
                                <th style="width: 100px">
                                    Selling Rate
                                </th>
                                <th style="width: 100px">
                                    Other Income
                                </th>
                                <th style="width: 100px">
                                    Profit
                                </th>
                                <th style="width: 100px">
                                    Profit %
                                </th>
                            </tr>
                        </table>--%>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
