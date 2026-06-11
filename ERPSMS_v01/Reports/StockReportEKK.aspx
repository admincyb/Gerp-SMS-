<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockReportEKK.aspx.cs"
    EnableEventValidation="false" Theme="Classic" MasterPageFile="~/ERPSMS_2.Master"
    Inherits="ERPSMS_v01.Reports.StockReportEKK" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/StockDetailsReport.js" type="text/javascript"></script>
    <script type="text/javascript">
        function InitComponents() {

          //  GrandScriptUtils.AddDateRange("StockDate", "hdfDateStock", "StockToDate", "hdfStockToDate", "dd-M-yy", false, true false);
              GrandScriptUtils.AddDateRangeCommon("StockDate", "hdfDateStock", "StockToDate", "hdfStockToDate", false, false);

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--     <div id="webwizard-wrap">
                <h1>
                    Stock Report
                </h1>
                <div class="button-wrap">
                    <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                        TabIndex="6" />
                    <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" PostBackUrl="~/Reports/StockDetailsReport.aspx"
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
                                        <%--<asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" PostBackUrl="PurchaseOrderListing.aspx" />--%>
                                        <asp:Button ID="btnCancel" TabIndex="6" runat="server" OnClientClick="javascript:return CancelFun();"
                                            SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Close%>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
                </div>
            </div>
            <%--  <div class="reportviewer" style="width: 98%; overflow-y: scroll; overflow-x: auto">
             
                <rsweb:ReportViewer ID="RptViewerReport" runat="server" Visible="false" EnableTheming="false"
                    Width="99%" BorderWidth="0" BorderStyle="None">
                </rsweb:ReportViewer>
            </div>--%>
            <div class="content-wrapper">
                <div id="searchwrap" class="search-wrap-custom1">
                    <label for="ddlStore">
                        Store</label>
                    <asp:DropDownList ID="ddlStore" CssClass="medium" runat="server" TabIndex="2">
                    </asp:DropDownList>
                    <label for="ddlMaterialCatg">
                        Category</label>
                    <asp:DropDownList ID="ddlCategory" runat="server" TabIndex="3">
                    </asp:DropDownList>
                    <label for="StockDate">
                        Date From</label>
                    <asp:TextBox ID="StockDate" runat="server" CssClass="medium" EnableViewState="false"
                        TabIndex="1">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfDateStock" runat="server" />
                    <label for="StockToDate">
                        Date To</label>
                    <asp:TextBox ID="StockToDate" runat="server" CssClass="medium" EnableViewState="false"
                        TabIndex="1">
                    </asp:TextBox>
                    <asp:HiddenField ID="hdfStockToDate" runat="server" />
                    <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="5" EnableViewState="false"
                        OnClick="imbSearch_Click" />
                    <div class="clear">
                    </div>
                </div>
                <div class="reportviewer max-500">
                    <rsweb:ReportViewer ID="RptViewerReport" runat="server" BorderWidth="0" SizeToReportContent="true"
                        Width="98%">
                    </rsweb:ReportViewer>
                </div>
            </div>
            <div class="clear">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
