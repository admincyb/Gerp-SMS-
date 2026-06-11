<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" Theme="Classic"
    AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="ERPSMS_v01.DashboardSMS.Dashboard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/PagerControl.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <script type="text/javascript">
        function ShowEmpty(isempty) {
            if (isempty) {
                $("[id$=ChartDiv]").hide();
                $("[id$=emptyDiv]").show();
            }
            else {
                $("[id$=ChartDiv]").show();
                $("[id$=emptyDiv]").hide();
            }
        }
        function InitComponents() {
            GrandScriptUtils.AddDateRange("txtFromDateSch", "hdfFromDateSch", "txtToDateSch", "hdftxtToDateSch", false, false);
        }
        function AfterClose(containerID) {
            if (containerID == "#divFilterPopup") {
                var ZoomCtrl = $("[id$=hdfZoomPK]").val();
                if (ZoomCtrl != "") {
                    $("#" + ZoomCtrl).click();
                }
            }
            else if (containerID == "#divPopup") {
                $("[id$=hdfZoomPK]").val("");
            }
        }
        function CheckListChange(evt) {
            var isChecked;
            if (!evt) {
                $("[id*=cblFilterParameter]").each(function () {
                    var cblId = $(this).attr("id");
                    var allNode;
                    allNode = $("[id$=" + cblId + "]").find('input[value="-1"]');
                    isChecked = allNode.attr("checked") == "checked";
                    if (isChecked) {
                        $("[id$=" + cblId + "]").find("tr:has(td)").each(function () {
                            $(this).find("td:first input").attr("checked", true);
                        });
                    }
                });
            }
            else {
                var src = window.event != window.undefined ? window.event.srcElement : evt.target;
                var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
                if (isChkBoxClick) {
                    var listID;
                    isChecked = src.checked;
                    var listID = $(src).parents("table:first").attr("id");
                    if ($(src).attr("value") == "-1") {
                        if (isChecked) {
                            $("[id$=" + listID + "]").find("tr:has(td)").each(function () {
                                $(this).find("td:first input").attr("checked", true);
                            });
                        }
                        else {
                            $("[id$=" + listID + "]").find("tr:has(td)").each(function () {
                                $(this).find("td:first input").removeAttr("checked");
                            });
                        }
                    }
                    else {
                        var isAll = true;
                        $("[id$=" + listID + "]").find("tr:has(td)").each(function () {
                            if (($(this).find("td:first input").attr("value") != "-1") && ($(this).find("td:first input").attr("checked") != "checked")) {
                                isAll = false;
                            }
                            if (isAll) {
                                $("[id$=" + listID + "]").find('input[value="-1"]').attr("checked", true);
                            }
                            else {
                                $("[id$=" + listID + "]").find('input[value="-1"]').removeAttr("checked");
                                //Code Here
                            }
                        });
                    }
                }
            }
        }
        function IsAlreadyFiltered() {
            if ($("[id$=hdfIsAlreadyFiltered]").val() == "1") {
                GrandScriptUtils.ShowModalID('divFilterPopup', 'Filter Parameters', false, '960', '600');
                return false;
            }
            else return true;
        }

        function ClosePopup() {
            $('#divPopup').hide();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlDashboard">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
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
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div id="ChartDiv" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Table runat="server" ID="ChartTable0" CssClass="ChartTable">
                                                <asp:TableRow runat="server" ID="ChartRow0">
                                                    <asp:TableCell runat="server" ID="ChartCell00">
                                                        <div class="dash-report">
                                                            <table id="HeaderTable00">
                                                                <tr class="popup-btns-co">
                                                                    <td>
                                                                        <asp:Label ID="lblHeader00" runat="server" Font-Bold="true"></asp:Label>
                                                                        <asp:HiddenField ID="hdfDashlet00" runat="server" />
                                                                    </td>
                                                                    <td>
                                                                        <div class="buttons-dashboard">
                                                                            <asp:Button runat="server" ID="btnFilter00" TabIndex="3" SkinID="filter-btn" CommandName="FILTER"
                                                                                OnClick="ActionHandler" />
                                                                            <asp:Button runat="server" ID="btnZoom00" TabIndex="4" SkinID="zoom-btn" CommandName="ZOOM"
                                                                                OnClick="ActionHandler" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr class="popup-btns-co">
                                                                    <td>
                                                                        <asp:Label ID="lblHeadFilter00" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </asp:TableCell>
                                                    <asp:TableCell runat="server" ID="ChartCell01">
                                                        <table id="HeaderTable01">
                                                            <tr class="popup-btns-co">
                                                                <td>
                                                                    <asp:Label ID="lblHeader01" runat="server" Font-Bold="true"></asp:Label>
                                                                    <asp:HiddenField ID="hdfDashlet01" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <div class="buttons-dashboard">
                                                                        <asp:Button runat="server" ID="btnFilter01" TabIndex="3" SkinID="filter-btn" CommandName="FILTER"
                                                                            OnClick="ActionHandler" />
                                                                        <asp:Button runat="server" ID="btnZoom01" TabIndex="4" SkinID="zoom-btn" CommandName="ZOOM"
                                                                            OnClick="ActionHandler" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr class="popup-btns-co">
                                                                <td>
                                                                    <asp:Label ID="lblHeadFilter01" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                            <asp:Table runat="server" ID="ChartTable1" CssClass="ChartTable">
                                                <asp:TableRow runat="server" ID="ChartRow1">
                                                    <asp:TableCell runat="server" ID="ChartCell10">
                                                        <table id="HeaderTable10">
                                                            <tr class="popup-btns-co">
                                                                <td>
                                                                    <asp:Label ID="lblHeader10" runat="server" Font-Bold="true"></asp:Label>
                                                                    <asp:HiddenField ID="hdfDashlet10" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <div class="buttons-dashboard">
                                                                        <asp:Button runat="server" ID="btnFilter10" TabIndex="3" SkinID="filter-btn" CommandName="FILTER"
                                                                            OnClick="ActionHandler" />
                                                                        <asp:Button runat="server" ID="btnZoom10" TabIndex="4" SkinID="zoom-btn" CommandName="ZOOM"
                                                                            OnClick="ActionHandler" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr class="popup-btns-co">
                                                                <td>
                                                                    <asp:Label ID="lblHeadFilter10" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:TableCell>
                                                    <asp:TableCell runat="server" ID="ChartCell11">
                                                        <table id="HeaderTable11" class="dash-head">
                                                            <tr class="popup-btns-co">
                                                                <td>
                                                                    <asp:Label ID="lblHeader11" runat="server" Font-Bold="true"></asp:Label>
                                                                    <asp:HiddenField ID="hdfDashlet11" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <div class="buttons-dashboard">
                                                                        <asp:Button runat="server" ID="btnFilter11" TabIndex="3" SkinID="filter-btn" CommandName="FILTER"
                                                                            OnClick="ActionHandler" />
                                                                        <asp:Button runat="server" ID="btnZoom11" TabIndex="4" SkinID="zoom-btn" CommandName="ZOOM"
                                                                            OnClick="ActionHandler" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr class="popup-btns-co">
                                                                <td>
                                                                    <asp:Label ID="lblHeadFilter11" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                            <asp:Table runat="server" ID="ChartTable2" CssClass="ChartTable">
                                                <asp:TableRow runat="server" ID="ChartRow2">
                                                    <asp:TableCell runat="server" ID="ChartCell20">
                                                        <table id="HeaderTable20" class="dash-head">
                                                            <tr class="popup-btns-co">
                                                                <td>
                                                                    <asp:Label ID="lblHeader20" runat="server" Font-Bold="true"></asp:Label>
                                                                    <asp:HiddenField ID="hdfDashlet20" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <div class="buttons-dashboard">
                                                                        <asp:Button runat="server" ID="btnFilter20" TabIndex="3" SkinID="filter-btn" CommandName="FILTER"
                                                                            OnClick="ActionHandler" />
                                                                        <asp:Button runat="server" ID="btnZoom20" TabIndex="4" SkinID="zoom-btn" CommandName="ZOOM"
                                                                            OnClick="ActionHandler" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr class="popup-btns-co">
                                                                <td>
                                                                    <asp:Label ID="lblHeadFilter20" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:TableCell>
                                                    <asp:TableCell runat="server" ID="ChartCell21">
                                                        <table id="HeaderTable21" class="dash-head">
                                                            <tr class="popup-btns-co">
                                                                <td>
                                                                    <asp:Label ID="lblHeader21" runat="server" Font-Bold="true"></asp:Label>
                                                                    <asp:HiddenField ID="hdfDashlet21" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <div class="buttons-dashboard">
                                                                        <asp:Button runat="server" ID="btnFilter21" TabIndex="3" SkinID="filter-btn" CommandName="FILTER"
                                                                            OnClick="ActionHandler" />
                                                                        <asp:Button runat="server" ID="btnZoom21" TabIndex="4" SkinID="zoom-btn" CommandName="ZOOM"
                                                                            OnClick="ActionHandler" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr class="popup-btns-co">
                                                                <td>
                                                                    <asp:Label ID="lblHeadFilter21" runat="server"></asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="RdlcDiv" runat="server">
                                <table id="HeaderTableRdlc" class="dash-head" runat="server">
                                    <tr class="dash-bottom-border">
                                        <td>
                                            <asp:Label ID="lblHeaderRdlc" runat="server" Font-Bold="true"></asp:Label>
                                            <asp:HiddenField ID="hdfDashletRdlc" runat="server" />
                                        </td>
                                        <td>
                                            <div class="buttons-dashboard">
                                                <asp:HiddenField ID="hdfIsAlreadyFiltered" runat="server" Value="" />
                                                <asp:Button runat="server" ID="btnFilterRdlc" TabIndex="3" SkinID="filter-btn" CommandName="FILTERRDLC"
                                                    OnClick="ActionHandler" ToolTip="Filter" OnClientClick="javascript:return IsAlreadyFiltered();" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr class="dash-bottom-border">
                                        <td>
                                            <asp:Label ID="lblHeadFilterRdlc" runat="server"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                           <%-- <div class="chart-border  reportviewer dash-reportviewer">--%>
                                                <%--  <rsweb:ReportViewer ID="rvrChart" runat="server" SizeToReportContent="true" HyperlinkTarget="">
                                                    <LocalReport EnableHyperlinks="true">
                                                    </LocalReport>
                                                </rsweb:ReportViewer>--%>
                                                <div class="reportviewer chart-border dash-reportviewer">
                                                    <rsweb:ReportViewer ID="rvrChart" runat="server" BorderWidth="0" SizeToReportContent="true" HyperlinkTarget=""
                                                        Width="98%">
                                                    </rsweb:ReportViewer>
                                                <%--</div>--%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap" id="emptyDiv" style="display: none">
                                <asp:Table runat="server" ID="EmptyChart" CssClass="grdTable">
                                    <asp:TableRow CssClass="emptytable">
                                        <asp:TableCell>
                                            <asp:Label ID="Label2" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </div>
                            <uc1:PagerControl ID="uclPaging" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="divPopup" style="display: none; width: 960; height: 560; vertical-align: middle;
                padding: 8px;">
                <div class="content-wrapper" id="divPopupDashlet" runat="server">
                    <table id="Table1">
                        <tr class="popup-btns-co">
                            <td>
                            </td>
                            <td>
                                <div class="buttons-dashboard">
                                    <asp:Button runat="server" ID="btnPopDashletFilter" TabIndex="3" SkinID="filter-btn"
                                        CommandName="FILTERDASHLET" OnClick="ActionHandler" />
                                </div>
                            </td>
                        </tr>
                        <tr class="popup-btns-co">
                            <td style="text-align: center" colspan="2">
                                <asp:Label ID="lblPopHeadFilter" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="divFilterPopup" style="display:none;width:960;height:560;">
                <div id="Table2" class="Button-container-popup">
                        <asp:Button runat="server" ID="btnOk" Text="Apply" ToolTip="Apply" OnClick="ActionHandler"
                            CommandName="SELECT" TabIndex="1" SkinID="btnInner-ok" />
                    </div>
                <div class="content-wrapper" id="div2" runat="server"> 
                    <table class="table-devide">
                    <tr>
                    <td><div class="div2col-S">
                        <asp:Label ID="lblFromDateSch" runat="server" Text="<%$resources:Controls,FromDate %>"
                            AssociatedControlID="txtFromDateSch"></asp:Label>
                        <asp:TextBox ID="txtFromDateSch" runat="server" TabIndex="26"></asp:TextBox>
                        <asp:HiddenField ID="hdfFromDateSch" runat="server" />
                    </div></td>
                    <td><div class="div2col-S">
                        <asp:Label ID="lblToDateSch" runat="server" Text="<%$resources:Controls,ToDate %>"
                            AssociatedControlID="txtToDateSch"></asp:Label>
                        <asp:TextBox ID="txtToDateSch" runat="server" TabIndex="27"></asp:TextBox>
                        <asp:HiddenField ID="hdfToDateSch" runat="server" />
                    </div></td>
                    </tr>
                    </table>
                    
                    
                    <asp:Repeater ID="rptFilter" runat="server" OnItemCreated="ActionHandler" OnItemDataBound="ItemBoundActionHandler">
                        <ItemTemplate>
                            <div class="asset-list">
                                <div class="asset-head">
                                    <asp:Label ID="lblFilterField" runat="server" Text='<%# GTIService.CommonFunctions.GetShortString(Eval("ParamLabel"), 25) %>'
                                        EnableTheming="false" ToolTip='<%# Eval("ParamLabel") %>'></asp:Label>
                                </div>
                                <div class="asset-scroll">
                                    <asp:CheckBoxList ID="cblFilterParameter" runat="server" onclick="CheckListChange(event);">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                            <asp:HiddenField ID="hdfFilterField" runat="server" Value='<%# Eval("ParamName") %>' />
                            <asp:HiddenField ID="hdfParamPk" runat="server" Value='<%# Eval("ParamPk") %>' />
                            <asp:HiddenField ID="hdfParamMethod" runat="server" Value='<%# Eval("ParamMethod") %>' />
                            <asp:HiddenField ID="hdfParamService" runat="server" Value='<%# Eval("ParamDatasource") %>' />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <asp:HiddenField ID="hdfPopupDashletPk" runat="server" />
            <asp:HiddenField ID="hdfZoomPK" runat="server" />
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
