<%@ Page Title="<%$ Resources:Captions,Title_Print %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="GenerateTelerikReport.aspx.cs" Inherits="ERPSMS_v01.Reports.GenerateTelerikReport" Theme="ClassicExt" %>


<%@ Register Assembly="Telerik.ReportViewer.Html5.WebForms, Version=18.1.24.709, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.Html5.WebForms" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/GrantPrintUtility.js" type="text/javascript"></script>
    <script src="../TelDll/Scripts/jquery-3.3.1.min.js"></script>
    <script>
        $.noConflict();
// Code that uses other library's $ can follow here.
    </script>
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var UIurl = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        var typeText = "";

        $(document).ready(function () { InitComponents(); });
        function InitComponents() {

            GetReferrer();
        }



        function ModalOk(cmd) {
            // $("[id$=btnSearch]").click();
            // GetSelectedNode();

        }



        function GetReferrer() {
            if (document.referrer.trim() != "" && $("[id$=hdfRefUrl]").val().trim() == "")
                $("[id$=hdfRefUrl]").val(document.referrer);
        }

    </script>

    <style>
        #reportViewer1 {
            position: absolute;
            left: 5px;
            right: 5px;
            top: 5px;
            bottom: 5px;
            overflow: hidden;
            font-family: Verdana, Arial;
            width: 100%;
        }

        .content-wrapper {
            padding: 0px 10px 10px 10px !important;
        }

        .k-menu .k-item, .k-menu.k-header {
            border-color: #a3d0e4;
            height: 28px !important;
        }

            .k-menu .k-item > .k-link, .k-menu-scroll-wrapper .k-item > .k-link, .k-popups-wrapper .k-item > .k-link {
                padding: 5px 1.1em .4em !important;
                line-height: 15px !important;
            }

        .trv-nav input.k-textbox {
            margin: 2px 0 0 0 !important;
        }

        .trv-page-wrapper.active {
            background-image: none;
            background-color: rgb(255, 255, 255);
            height: 100% !important;
            width: 100% !important;
            position: relative;
        }

        .trv-pages-area.printpreview .trv-page-container .trv-page-wrapper .trv-report-page {
            border-right: 0;
        }

        .trv-page-overlay {
            border-color: lightgray !important;
            background-color: lightgray !important;
            color: #000;
        }

        .trv-pages-area.printpreview .trv-page-container .trv-page-wrapper.active .trv-report-page:not(.k-state-default) {
            border-color: #000 !important;
            border-right-color: rgb(0, 0, 0) !important;
            border-right: 0 !important;
        }

        .s1-1b805b50454aa863dcff53 {
            border-bottom: 1px solid #4F81BD !important;
        }

        /*.k-widget.k-tooltip-validation {
         border-color: red;
         background-color: red;
         color: #000;
     }*/
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
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <%-- <div>class="visiblefalse"--%>
        <div id="divAccPopUp" title="<%=Resources.Captions.ChooseAccount%>" runat="server">
        </div>
        <%--   </div>--%>
        <div id="divTelReport">
            <telerik:ReportViewer
                ID="ReportViewer1"
                Width="100%"
                Height="900px"
                EnableAccessibility="false"
                PageMode="SinglePage"
                Scale="1"
                ScaleMode="Specific"
                PageNumber="1" EnableViewState="true" ViewMode="PrintPreview"
                runat="server">
                <ReportSource IdentifierType="UriReportSource">
                </ReportSource>
            </telerik:ReportViewer>
        </div>
        <div id="divNodata" class="nodata" runat="server" visible="false">
            No Record Found
        </div>
        <asp:HiddenField ID="hdfAppTypeRpt" Value="" runat="server" />
        <asp:HiddenField ID="hdfRptNameRpt" Value="" runat="server" />
        <asp:HiddenField ID="hdfAccount" runat="server" />
        <asp:HiddenField ID="hdfSubType" runat="server" />
    </div>
    <div class="clear">
    </div>
    <div class="visiblefalse">
        <asp:HiddenField ID="hdfAppType" runat="server" />
        <asp:HiddenField ID="hdfRefUrl" runat="server" ClientIDMode="Static" />
    </div>
    <div id="diverror" class="visiblefalse">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
        <asp:HiddenField ID="hdfCompanyPK" runat="server" />
    </div>
</asp:Content>
