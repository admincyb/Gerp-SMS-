<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS.Master" AutoEventWireup="true"
    Theme="ERP-Blue" CodeBehind="PRReport.aspx.cs" Inherits="ERPSMS_v01.Reports.PRReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/Reports/PurchaseRequestReport.js" type="text/javascript"></script>
    <style type="text/css">
    .reportviewer table tr td div
    {
        overflow:inherit!important;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="webwizard-wrap">
                <h1>
                    Purchase Request Report
                </h1>
                <div class="button-wrap">
                    <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                        TabIndex="6" />
                    <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" PostBackUrl="~/Reports/PRReport.aspx"
                        TabIndex="7" />
                </div>
            </div>
            <div id="searchwrap">
                <label for="DateFrom">
                    Date From</label>
                <asp:TextBox ID="DateFrom" runat="server" EnableViewState="false" TabIndex="3" MaxLength="12">
                </asp:TextBox>
                <asp:HiddenField ID="hdfFromDate" runat="server" />
                <label for="DateTo">
                    Date To</label>
                <asp:TextBox ID="DateTo" runat="server" EnableViewState="false" TabIndex="4" MaxLength="12">
                </asp:TextBox>
                <asp:HiddenField ID="hdfToDate" runat="server" />
                <label for="ddlFilterBy">
                    Filter By</label>
                <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="1" 
                    AutoPostBack="true" onselectedindexchanged="ddlFilterBy_SelectedIndexChanged"
                     >
                    <asp:ListItem Text="All" Value="0"></asp:ListItem>
                    <asp:ListItem Text="PR#" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Status" Value="2"></asp:ListItem>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlFilterValue" runat="server" TabIndex="2">
                </asp:DropDownList>
                <asp:ImageButton ID="imbSearch" runat="server" SkinID="btnsearchgo" Width="30px"
                    TabIndex="5" Height="20px" EnableViewState="false" 
                    OnClick="imbSearch_Click" />
            </div>
            <div class="reportviewer"  style="width: 98%;overflow-y: scroll;">
                <rsweb:ReportViewer ID="RptViewerReport" runat="server" Visible="false" EnableTheming="false" OnDrillthrough="RptViewerReport_Drillthrough"
                    Width="100%" BorderWidth="0" BorderStyle="None">
                </rsweb:ReportViewer>
            </div>
 <div class="clear">
            </div>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
