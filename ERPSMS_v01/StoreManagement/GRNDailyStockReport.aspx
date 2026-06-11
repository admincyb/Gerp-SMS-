<%@ Page Title="<%$ Resources:Captions,Title_GRNReport %>" Theme="Classic" EnableEventValidation="false"
    Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="GRNDailyStockReport.aspx.cs"
    Inherits="ERPSMS_v01.StoreManagement.GRNDailyStockReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/GRNDailyReport.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div id="webwizard-wrap">
        <h1>
            GRN Daily Report 
        </h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                TabIndex="6" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset"  PostBackUrl="~/StoreManagement/GRNDailyStockReport.aspx"
                 TabIndex="7" />
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
                                <asp:Button ID="btnCancel" runat="server" OnClientClick="javascript:return CancelFun();"
                                    SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="searchwrap" class="search-wrap-custom1">
            <label for="SearchType">
                Store</label>
            <asp:DropDownList ID="Store" runat="server" CssClass="srchboxtextbx" TabIndex="1">
            </asp:DropDownList>
            <label for="SearchType">
                Date</label>
            <asp:TextBox ID="GRNDate" runat="server" EnableViewState="false" TabIndex="2">
            </asp:TextBox>
            <asp:HiddenField ID="hdfDateGRN" runat="server" />
            <label for="Category">
                Category</label>
            <asp:DropDownList ID="ddlMaterialCatg" runat="server" CssClass="srchboxtextbx" TabIndex="3">
            </asp:DropDownList>
            <asp:ImageButton ID="imbViewCag" runat="server" SkinID="imbselect" Width="22px" Height="22px"
                ToolTip="Add" TabIndex="4" OnClientClick="javascript:return AddItemCategoryDetails();"
                EnableViewState="false" />
            <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="5" EnableViewState="false"
                OnClick="imbSearch_Click" />
            <div class="clear">
            </div>
        </div>
        <%--<div class="grdTable">--%>
        <%--<div class="reportviewer" style="width: 98%">
            <rsweb:ReportViewer ID="RptViewerReport" runat="server" Visible="false" Width="99%"
                BorderWidth="0" BorderStyle="None">
            </rsweb:ReportViewer>
        </div>--%>
        <div class="reportviewer">
            <rsweb:ReportViewer ID="RptViewerReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                Width="98%">
            </rsweb:ReportViewer>
        </div>

        <%-- </div>--%>
        <div class="clear">
        </div>
        <div id="divCatgDtls" style="margin-top: 25px">
            <div id="treewrap" class="treeviewrap-fxd">
                <div id="trvCategory" class="treeview-adj">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
