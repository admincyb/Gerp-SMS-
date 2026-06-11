<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" CodeBehind="OrderPlanning.aspx.cs" Inherits="ERPSMS_v01.OrderPlanning.OrderPlanning" %>

<%@ Register Src="~/UserControls/PendingOrders.ascx" TagName="PendingOrders" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/NumericControl.ascx" TagName="NumericControl" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc3" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/LinewiseProductionPlanningRpt.ascx" TagName="LinewisePdtn"
    TagPrefix="uc5" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScripts/OrderPlanning/OrderPlanning.js" type="text/javascript"></script>
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handler/AutoComplete.ashx" : "/" + virtualPath + "Handler/AutoComplete.ashx");

        function InitAmountControl() {
            $("[id*=txtFormattedAmount]").ForceNumersOnly();
        }

        function CheckLineQty() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.Captions.Information %>';
            msg = '<%= GetLocalResourceObject("Msg_Cont_Confirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIscontYes]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnApplyLines]").click();
                    },
                    No: function (e) {
                        $("[id$=hdfIscontYes]").val(0);
                        $(this).dialog("close");
                        ShowContainerDiv('[id$=divLines]', '<%= GetLocalResourceObject("Addline").ToString() %>', '900', '400');
                        CalculateLineQtyTotal();
                        return false;
                    }
                }
            });
            return false;
        }


        function CheckSCQty() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.Captions.Information %>';
            msg = '<%= GetLocalResourceObject("Msg_Cont_SC_Confirm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                close: function (e) {
                    ShowContainerDiv('[id$=divSCDetails]', '<%= GetLocalResourceObject("ViewSCDetails").ToString() %>', '900', '400');
                },
                buttons: {
                    Yes: function (e) {
                        $("[id$=hdfIsSCcontYes]").val(1);
                        $(this).dialog("close");
                        $("[id$=btnApplySCDtls]").click();
                    },
                    No: function (e) {
                        $("[id$=hdfIsSCcontYes]").val(0);
                        $(this).dialog("close");
                        ShowContainerDiv('[id$=divSCDetails]', '<%= GetLocalResourceObject("ViewSCDetails").ToString() %>', '900', '400');
                        CalculateSCPopUpTotal();
                        return false;
                    }
                }
            });
            return false;
        }

        function ShowReport() {
            ShowContainerDiv('#divLinewiseRpt', 'Linewise Production', '900', '900');
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupConsumption" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" TabIndex="100" ID="btnSave" CommandName="SAVE" Text="<%$resources:SavePlan%>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ToolTip="<%$resources:Controls,Save %>"
                                            ViewStateMode="Disabled" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')" />
                                    </li>
                                    <li runat="server" id="pnlFinalize">
                                        <asp:Button runat="server" ID="btnFinalize" CommandName="FINALIZE" Text="<%$ resources:Finalize %>"
                                            OnClick="ActionHandler" TabIndex="101" CommandArgument="SEC_ActionPanel" SkinID="btnInner-journalize"
                                            ToolTip="<%$ resources:Finalize %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="101" ID="btnRevise" CommandName="REVISE" Text="<%$resources:Controls,Revise %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$resources:Controls,Revise %>"
                                            OnClick="ActionHandler" Style="display: none;" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="102" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlCancelSubmit">
                                        <asp:Button runat="server" ID="btnCancelSubmit" Text="<%$resources:Controls,CancelSubmit %>"
                                            SkinID="btnInner-cancel1" ToolTip="<%$resources:Controls,CancelSubmit %>" TabIndex="102"
                                            OnClientClick="return ShowDeleteConfirm(this,'Do you want to cancel the record?');"
                                            OnClick="ActionHandler" CommandName="CANCELSUBMIT" />
                                    </li>
                                    <li runat="server" id="pnlRevert">
                                        <asp:Button runat="server" TabIndex="103" ID="btnRevert" Text="<%$resources:Controls,Revert %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Revert" CommandName="REVERT"
                                            ToolTip="<%$resources:Controls,Revert %>" OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this,'Do you want to revert the selected plan to the previous version?')" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="104" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="105" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" CommandName="CANCEL"
                                            ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="2" ID="btnEdit" CommandName="EDIT" Text="<%$resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$resources:Controls,Edit %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" ToolTip="<%$resources:Controls,View %>"
                                            TabIndex="3" Text="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-View" OnClick="ActionHandler" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tbOrderPlan" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_Entry">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <label for="txtFromDate">
                                                <asp:Literal ID="ltFromDate" runat="server" Text="<%$ Resources:DateFrom%>"></asp:Literal>
                                            </label>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="input-small" TabIndex="1"
                                                onkeypress="return isDate(event)" onkeydown="return CheckKey(event)" onpaste="return false;">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfFromDate" SetFocusOnError="true" ValidationGroup="Save"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtFromDate" Text="*"
                                                    ErrorMessage="<%$ resources: Msg_Empty_FromDate %>" Display="Dynamic" CssClass="star">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="vreFromDate" SetFocusOnError="true" ValidationGroup="SelectPlan"
                                                    EnableClientScript="true" runat="server" ClientValidationFunction="CheckFromDate"
                                                    Text="*" ErrorMessage="<%$ resources: Msg_Empty_FromDate %>" Display="Dynamic"
                                                    CssClass="star">
                                                </asp:CustomValidator>
                                            </div>
                                            <label for="txtToDate" class="middle-lbl-a0">
                                                <asp:Literal ID="ltToDate" runat="server" Text="<%$ Resources:DateTo %>"></asp:Literal>
                                            </label>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="input-small" TabIndex="2" onkeypress="return isDate(event)"
                                                onkeydown="return CheckKey(event)" onpaste="return false;">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfToDate" SetFocusOnError="true" ValidationGroup="Save"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtToDate" Text="*"
                                                    ErrorMessage="<%$ resources: Msg_Empty_ToDate %>" Display="Dynamic" CssClass="star">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="vreToDate" SetFocusOnError="true" ValidationGroup="SelectPlan"
                                                    EnableClientScript="true" runat="server" ClientValidationFunction="CheckToDate"
                                                    Text="*" ErrorMessage="<%$ resources: Msg_Empty_ToDate %>" Display="Dynamic"
                                                    CssClass="star">
                                                </asp:CustomValidator>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <label for="txtPlanCode">
                                                <asp:Literal ID="ltPlanCode" runat="server" Text="<%$ Resources:PlanCode%>"></asp:Literal>
                                            </label>
                                            <asp:TextBox ID="txtPlanCode" runat="server" TabIndex="3" MaxLength="20">
                                            </asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="rfvPlanCode" SetFocusOnError="true" ValidationGroup="Save"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtPlanName" Text="*"
                                                    ErrorMessage="<%$ resources: Msg_Empty_PlanCode %>" Display="Dynamic" CssClass="star"
                                                    InitialValue="-1">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="cvPlanCode" SetFocusOnError="true" ValidationGroup="SelectPlan"
                                                    EnableClientScript="true" runat="server" ClientValidationFunction="CheckPlanCode"
                                                    Text="*" ErrorMessage="<%$ resources: Msg_Empty_PlanCode %>" Display="Dynamic"
                                                    CssClass="star">
                                                </asp:CustomValidator>
                                            </div>
                                            <label for="lblVersion">
                                                <asp:Literal ID="ltVersion" runat="server" Text="<%$ Resources:Version%>"></asp:Literal>
                                            </label>
                                            <asp:Label runat="server" ID="lblVersion" CssClass="lbl-17-5perc"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <label for="txtPlanName">
                                                <asp:Literal ID="ltPlanName" runat="server" Text="<%$ Resources:PlanName%>"></asp:Literal>
                                            </label>
                                            <asp:TextBox ID="txtPlanName" runat="server" TabIndex="3" CssClass="input-w58-5per"
                                                MaxLength="50">
                                            </asp:TextBox>
                                            <asp:ImageButton ID="imbShowVersions" runat="server" SkinID="history" OnClick="ActionHandler"
                                                CommandName="VIEW_ACTIONPOPUP" TabIndex="3" ToolTip="<%$ Resources:ShowVersions %>"
                                                CssClass="padgtop2" />
                                            <asp:HiddenField runat="server" ID="hdfPlanPK" Value="0" />
                                            <asp:HiddenField runat="server" ID="hdfAGradePerc" Value="0" />
                                            <asp:HiddenField runat="server" ID="hdfshowRevert" Value="0" />
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfPlanName" SetFocusOnError="true" ValidationGroup="Save"
                                                    EnableClientScript="true" runat="server" ControlToValidate="txtPlanName" Text="*"
                                                    ErrorMessage="<%$ resources: Msg_Empty_PlanName %>" Display="Dynamic" CssClass="star"
                                                    InitialValue="-1">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="vrePlanName" SetFocusOnError="true" ValidationGroup="SelectPlan"
                                                    EnableClientScript="true" runat="server" ClientValidationFunction="CheckPlanName"
                                                    Text="*" ErrorMessage="<%$ resources: Msg_Empty_PlanName %>" Display="Dynamic"
                                                    CssClass="star">
                                                </asp:CustomValidator>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <ul id="tab-menu" class="margnbotm-minus1">
                                <li><span id="tab1" runat="server" class="tab-inactive">
                                    <asp:LinkButton runat="server" ID="lnkPendingOrders" Text="<%$ Resources:PeningOrders %>"
                                        CssClass="tab-inactive" OnClick="ActionHandler" CommandName="PENDINGORDERS"></asp:LinkButton>
                                </span></li>
                                <li><span id="tab4" runat="server" class="tab-inactive">
                                    <asp:LinkButton runat="server" ID="lnkScenario" Text="<%$ Resources:Scenario %>"
                                        CssClass="tab-inactive" OnClick="ActionHandler" CommandName="SCENARIO"></asp:LinkButton>
                                </span></li>
                                <li><span id="tab2" runat="server" class="tab-active">
                                    <%--as per requirement hide this tab--%>
                                    <asp:LinkButton runat="server" ID="lnkPlanning" Text="<%$ Resources:Planning %>"
                                        CssClass="tab-active" OnClick="ActionHandler" CommandName="PLANNING"></asp:LinkButton>
                                </span></li>
                                <li><span id="tab3" runat="server" class="tab-active">
                                    <%--as per requirement hide this tab--%>
                                    <asp:LinkButton runat="server" ID="lnkSummary" Text="<%$ Resources:Summary %>" CssClass="tab-active"
                                        OnClick="ActionHandler" CommandName="SUMMARY"></asp:LinkButton>
                                </span></li>
                            </ul>
                            <div id="divAdvanceFilter">
                                <div>
                                    <uc1:PendingOrders ID="ucrPO" runat="server" />
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divPlanning" style="overflow-x: hidden; overflow-y: auto; max-height: 400px;"
                                class="padgtop7">
                                <div class="button-wrap-right">
                                    <asp:CheckBox runat="server" ID="chkOptimizePlan" ToolTip="<%$ Resources:Optimize %>" CssClass="margnbotm0 margntop3" onClick="ShowHidePlanSplit();" />
                                    <asp:Label runat="server" ID="lblOptimizePlan" Text="<%$ Resources:Optimize %>"></asp:Label>

                                    <asp:CheckBox runat="server" ID="chkPlanSplit" ToolTip="<%$ Resources:Split %>" CssClass="margnbotm0 margntop3" />
                                    <asp:Label runat="server" ID="lblPlanSplit" Text="<%$ Resources:Split %>"></asp:Label>

                                    <asp:Button runat="server" TabIndex="101" ID="btnPlanCalculate" CommandName="CALCULATE"
                                        Text="<%$ Resources:RePlan %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-process"
                                        ToolTip="<%$ Resources:RePlan %>" OnClick="ActionHandler" />
                                </div>
                                <div class="clear">
                                </div>
                                <div class="gridwrap hierarchical-wrap2" id="divSec_ScrollContainer" grid="grdGrouplist">
                                    <asp:HiddenField ID="hdfGroup_ExpandPosition" runat="server" />
                                    <cc1:ExtGridView runat="server" ID="grdGrouplist" AutoGenerateColumns="False" Width="100%"
                                        ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                        GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                        ShowFooter="true" FooterStyle-CssClass="grdfooter" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox runat="server" ID="ChkPlanAll" TabIndex="27" ToolTip="<%$ Resources:SelectAll %>"
                                                        CssClass="margn-lft-3" AutoPostBack="true" OnCheckedChanged="ActionHandler" />
                                                    <%-- onclick="CheckPlannedItems(event);"--%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <asp:CheckBox runat="server" ID="ChkPlanOrder" TabIndex="28" Checked='<%# Convert.ToInt32(Eval("PND_IS_CHECKED")) != 0? true:false %>'
                                                            AutoPostBack="true" OnCheckedChanged="ActionHandler" />
                                                        <%--         onclick="EnableCurrPlan($(this).parent().parent());CheckPlannedItems(event);" --%>
                                                    </div>
                                                </ItemTemplate>
                                                <HeaderStyle Width="2%" HorizontalAlign="Center" CssClass="txtAlign-center" />
                                                <ItemStyle Width="2%" HorizontalAlign="Center" CssClass="mar" />
                                                <FooterStyle Width="2%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:ProductGroup %>" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="39%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductGroup" runat="server" Text='<%#  ERP.Production.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("PND_PLAN_GROUP_TEXT")),65) %>'
                                                        ToolTip='<%# Eval("PND_PLAN_GROUP_TEXT") %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfIsExpandedGroupItem" Value="0" />
                                                    <asp:HiddenField runat="server" ID="hdfPlanDtlPK" Value='<%# Eval("PND_PK") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfPlanGroupPK" Value='<%# Eval("PND_PLAN_GROUP") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfPlanDtlSLNO" Value='<%# Eval("PND_SL_NO") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfAGradePercDtl" Value='<%# Eval("ISD_AGRADE_PER") %>' />
                                                    <asp:Button runat="server" ID="btnGroupDetails" OnClick="ActionHandler" CommandName="PLANGROUPITEMS"
                                                        CommandArgument='<%# Eval("PND_PLAN_GROUP") %>' EnableTheming="false" Style="display: none" />
                                                </ItemTemplate>
                                                <ItemStyle Width="44%" />
                                            </asp:TemplateField>
                                            <%-- <asp:TemplateField HeaderText="<%$ Resources:Size %>" HeaderStyle-Width="5%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSize" runat="server" Text='<%# Eval("ISD_SIZE_TEXT") %>' ToolTip='<%# Eval("ISD_SIZE_TEXT") %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfSize" Value='<%# Eval("PND_SIZE") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="<%$ Resources:Requiredby %>" HeaderStyle-CssClass="grd-head-center"
                                                HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequiredby" runat="server" Text='<%# Convert.ToDateTime(Eval("PND_REQUIRED_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'
                                                        ToolTip='<%# Convert.ToDateTime(Eval("PND_REQUIRED_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:TotalPlanned %>" HeaderStyle-CssClass="grd-head-rgt"
                                                HeaderStyle-Width="9%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalPlanned" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_TOT_QTY_PLANNED"))) %>'
                                                        ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_TOT_QTY_PLANNED"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="9%" HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <div class="txt-center">
                                                        <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>" CssClass="bold" />
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:BalancetoPlan %>" HeaderStyle-CssClass="grd-head-rgt"
                                                HeaderStyle-Width="8%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalancetoPlan" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_TOT_BAL_TO_PLAN"))) %>'
                                                        ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_TOT_BAL_TO_PLAN"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <div style="text-align: right;">
                                                        <asp:Label ID="lblTotalPlannedQty" runat="server" CssClass="bold" />
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:CurrentPlan %>" HeaderStyle-CssClass="grd-head-rgt"
                                                HeaderStyle-Width="12%">
                                                <ItemTemplate>
                                                    <uc2:NumericControl ID="txtTotalCurrPlan" runat="server" ControlType="NumericInteger"
                                                        CssClass="numeric medium input-disabled" Enabled="false" onchange="GroupQtyChanged($(this).parent().parent());"
                                                        onblur="CalculateGrouplistTotal();" Text='<%# Eval("PND_PLAN_QTY") %>' TabIndex="28" />
                                                </ItemTemplate>
                                                <ItemStyle Width="12%" CssClass="txtAlign-right margn-rgt0" />
                                                <FooterTemplate>
                                                    <div style="text-align: right;">
                                                        <asp:Label ID="lblTotalBalQty" runat="server" CssClass="bold" />
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="2%">
                                                <HeaderTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkbtnAGradeHdr" Text="<%$ Resources:AGradeRealizationCaption %>"
                                                        ToolTip="<%$ Resources:AGradeRealizationTooltip %>" OnClientClick="javascript:return CalculateTotalwithAGrade();"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkbtnAGradeDtl" Text="<%$ Resources:AGradeRealizationCaption %>"
                                                        ToolTip="<%$ Resources:AGradeRealizationTooltip %>" OnClientClick="javascript:return CalculateTotalwithAGrade($(this).parent().parent());"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                                <FooterTemplate>
                                                    <div style="text-align: right;">
                                                        <asp:Label ID="lblTotalPlanQty" runat="server" CssClass="bold" />
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:LinePlannedQty %>" HeaderStyle-Width="6%"
                                                HeaderStyle-CssClass="grd-head-rgt">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnbLine" runat="server" Text='<%# GetFormattedNumber(Convert.ToString( Eval("PND_LINE_PLANNED"))) %>'
                                                        ToolTip="<%$ Resources:ViewLines %>" OnClick="ActionHandler" CommandName="VIEW_ACTIONPOPUP"
                                                        Enabled='<%# Convert.ToDouble( Eval("PND_LINE_PLANNED"))> 0 ? true :false %>'
                                                        CssClass='<%# Convert.ToDouble( Eval("PND_LINE_PLANNED"))> 0 ? "text-underline" :"link-disabled" %>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="6%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imbAddLine" runat="server" SkinID="imbsplitup" OnClick="ActionHandler"
                                                        CommandName="ADD" TabIndex="28" ToolTip="<%$ Resources:Addline %>" CssClass='<%# Convert.ToInt32(Eval("PND_IS_CHECKED")) != 0? "visible-inline":"display-none" %>' />
                                                    <asp:ImageButton OnClientClick='return ShowDeleteConfirm(this,"Do you want to delete this group?");'
                                                        runat="server" ID="imbGroupDelete" SkinID="imbdeletegrid" OnClick="ActionHandler"
                                                        CommandName="DELETE_ACTION" TabIndex="28" ToolTip="<%$ Resources:Controls,Delete %>" />
                                                </ItemTemplate>
                                                <ItemStyle Width="9%" Wrap="false" />
                                                <FooterTemplate>
                                                    <div style="text-align: right;">
                                                        <asp:Label ID="lblTotalLineQty" runat="server" CssClass="bold" />
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div class="hierarchical-gridwrap">
                                                        <asp:HiddenField ID="hdfSize_ExpandPosition" runat="server" />
                                                        <asp:GridView runat="server" ID="grdSize" AutoGenerateColumns="False" Width="100%"
                                                            ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                            GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                            AllowPaging="false">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblSizeEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Size %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSizeText" runat="server" Text='<%# Eval("PNS_SIZE_TEXT") %>' ToolTip='<%# Eval("PNS_SIZE_TEXT") %>'></asp:Label>
                                                                        <asp:HiddenField runat="server" ID="hdfIsExpandedSizeItem" Value="0" />
                                                                        <asp:HiddenField runat="server" ID="hdfSizeVal" Value='<%# Eval("PNS_SIZE") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfSizeDtlSLNO" Value='<%# Eval("PND_SL_NO") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfSizePlanGroup" Value='<%# Eval("SIZE_PLAN_GROUP") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfIsApplied" Value="0" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="48%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Requiredby %>" HeaderStyle-CssClass="grd-head-center"
                                                                    HeaderStyle-Width="9.5%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSRequiredby" runat="server" Text='<%# Convert.ToDateTime(Eval("SIZE_REQUIRED_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'
                                                                            ToolTip='<%# Convert.ToDateTime(Eval("SIZE_REQUIRED_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="9.5%" HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:PlannedPcs %>" HeaderStyle-CssClass="grd-head-rgt"
                                                                    HeaderStyle-Width="9.5%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSTotalPlanned" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_PLANNED"))) %>'
                                                                            ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_PLANNED"))) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="9.5%" HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:BalancetoPlan %>" HeaderStyle-CssClass="grd-head-rgt"
                                                                    HeaderStyle-Width="8%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSBaltoPlan" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_BAL_TO_PLAN"))) %>'
                                                                            ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_BAL_TO_PLAN"))) %>'></asp:Label>
                                                                        <asp:HiddenField runat="server" ID="hdfProportionVal" Value="0" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:CurrentPlan %>" HeaderStyle-CssClass="grd-head-rgt">
                                                                    <ItemTemplate>
                                                                        <uc2:NumericControl ID="txtSizePlanNow" runat="server" ControlType="NumericInteger"
                                                                            CssClass="numeric medium input-disabled" Enabled="false" onchange="GroupQtyChanged($(this).parent().parent());"
                                                                            Text='<%# Eval("SIZE_PLAN_QTY") %>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imbViewSC" runat="server" SkinID="imbsplitup" OnClick="ActionHandler"
                                                                            CommandName="ADD" TabIndex="28" ToolTip="<%$ Resources:ViewSCDetails %>" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="13%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle CssClass="table-secondlevel" />
                                                            <HeaderStyle CssClass="table-secondlevela" />
                                                        </asp:GridView>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="table-firstlevel" />
                                        <HeaderStyle CssClass="table-firstlevela" />
                                    </cc1:ExtGridView>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divSummary" style="overflow-x: hidden; overflow-y: auto; max-height: 400px;"
                                class="padgtop7">
                                <%-- <div class="tab-container-floating">
                                    <asp:LinkButton runat="server" ID="lnkSummary1" Text="<%$ Resources:PeningOrders %>"
                                        CssClass="tab-inactive" OnClick="ActionHandler" CommandName="SUMMARY"></asp:LinkButton>
                                    <%--as per requirement hide this tab 
                                    <asp:LinkButton runat="server" ID="lnkSummary2" Text="<%$ Resources:Planning %>"
                                        CssClass="tab-active" OnClick="ActionHandler" CommandName="SUMMARY"></asp:LinkButton>
                                </div>
                                <div id="divSummary1" style="overflow-x: hidden; overflow-y: auto; max-height: 400px;"
                                    class="padgtop7">
                                    <div class="gridwrap hierarchical-wrap3" id="divSummarySec_ScrollContainer" grid="grdSummaryPlant">
                                        <asp:HiddenField ID="hdfPlant_ExpandPosition" runat="server" />
                                        <cc1:ExtGridView runat="server" ID="grdSummaryPlant" AutoGenerateColumns="False"
                                            Width="100%" PageSize="<%$ Resources:PageSize %>" ExpandButtonCssClass="GridExpandCollapseButton"
                                            CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                            CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmptyGd" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:Plant %>" HeaderStyle-HorizontalAlign="Left"
                                                    HeaderStyle-Width="25.2%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPlantText" runat="server" Text='<%#  ERP.Production.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("LNE_PLANT_TEXT")),40) %>'
                                                            ToolTip='<%# Eval("LNE_PLANT_TEXT") %>'></asp:Label>
                                                        <asp:HiddenField runat="server" ID="hdfIsExpandedPlantItem" Value="0" />
                                                        <asp:Button runat="server" ID="btnPlantDetails" OnClick="ActionHandler" CommandName="SUMMARYLINES"
                                                            CommandArgument='<%# Eval("PNL_PLANT") %>' EnableTheming="false" Style="display: none" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25.2%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PlannedPerc %>" HeaderStyle-CssClass="grd-head-rgt"
                                                    HeaderStyle-Width="11%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPlannedPerc" runat="server" Text='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_PLAN_QTY_PER"))) %>'
                                                            ToolTip='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_PLAN_QTY_PER"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="11%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PlannedPcs %>" HeaderStyle-CssClass="grd-head-rgt"
                                                    HeaderStyle-Width="14.7%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPlannedPcs" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>'
                                                            ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="14.7%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:ProducedPerc %>" HeaderStyle-CssClass="grd-head-rgt"
                                                    HeaderStyle-Width="11.85%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProducedPerc" runat="server" Text='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_PRCD_QTY_PER"))) %>'
                                                            ToolTip='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_PRCD_QTY_PER"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="11.85%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:ProducedPcs %>" HeaderStyle-CssClass="grd-head-rgt"
                                                    HeaderStyle-Width="12.7%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProducedPcs" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PRCD_QTY"))) %>'
                                                            ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PRCD_QTY"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12.7%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:BalancePerc %>" HeaderStyle-CssClass="grd-head-rgt">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBalancePerc" runat="server" Text='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_BAL_QTY_PER"))) %>'
                                                            ToolTip='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_BAL_QTY_PER"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:BalancePcs %>" HeaderStyle-CssClass="grd-head-rgt">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBalancePcs" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_BAL_QTY"))) %>'
                                                            ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_BAL_QTY"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="11.8%" HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ItemStyle-CssClass="padgrgt0">
                                                    <ItemTemplate>
                                                        <div class="hierarchical-gridwrap">
                                                            <asp:HiddenField ID="hdfLine_ExpandPosition" runat="server" />
                                                            <cc1:ExtGridView runat="server" ID="grdLineSummary" AutoGenerateColumns="False" Width="100%"
                                                                ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                                GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                                AllowPaging="false">
                                                                <EmptyDataTemplate>
                                                                    <asp:Label ID="lblLineEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                </EmptyDataTemplate>
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:Line %>" HeaderStyle-HorizontalAlign="Left">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLineText" runat="server" Text='<%# ERP.Production.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("PNL_LINE_TEXT")),40) %>'
                                                                                ToolTip='<%# Eval("PNL_LINE_TEXT") %>'></asp:Label>
                                                                            <asp:HiddenField runat="server" ID="hdfIsExpandedLineItem" Value="0" />
                                                                            <asp:Button runat="server" ID="btnLineDetails" OnClick="ActionHandler" CommandName="SUMMARYGROUP"
                                                                                CommandArgument='<%# Eval("PNL_LINE") %>' EnableTheming="false" Style="display: none" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="23.8%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:PlannedPerc %>" HeaderStyle-CssClass="grd-head-rgt">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLinePlannedPerc" runat="server" Text='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_PLAN_QTY_PER"))) %>'
                                                                                ToolTip='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_PLAN_QTY_PER"))) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="11.2%" HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:PlannedPcs %>" HeaderStyle-CssClass="grd-head-rgt">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLinePlannedPcs" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>'
                                                                                ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:ProducedPerc %>" HeaderStyle-CssClass="grd-head-rgt">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLineProducedPerc" runat="server" Text='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_PRCD_QTY_PER"))) %>'
                                                                                ToolTip='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_PRCD_QTY_PER"))) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:ProducedPcs %>" HeaderStyle-CssClass="grd-head-rgt">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLineProducedPcs" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PRCD_QTY"))) %>'
                                                                                ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PRCD_QTY"))) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:BalancePerc %>" HeaderStyle-CssClass="grd-head-rgt"
                                                                        HeaderStyle-Width="13.2%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLineBalancePerc" runat="server" Text='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_BAL_QTY_PER"))) %>'
                                                                                ToolTip='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_BAL_QTY_PER"))) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="13.2%" HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:BalancePcs %>" HeaderStyle-CssClass="grd-head-rgt">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLineBalancePcs" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_BAL_QTY"))) %>'
                                                                                ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_BAL_QTY"))) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField ItemStyle-CssClass="padgrgt0">
                                                                        <ItemTemplate>
                                                                            <div class="hierarchical-gridwrap">
                                                                                <asp:GridView runat="server" ID="grdPGroupSummary" AutoGenerateColumns="False" Width="100%"
                                                                                    EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false">
                                                                                    <EmptyDataTemplate>
                                                                                        <asp:Label ID="lblGroupEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                                    </EmptyDataTemplate>
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:ProductGroup %>">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPGroupText" runat="server" Text='<%# ERP.Production.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("PND_PLAN_GROUP_TEXT")),75) %>'
                                                                                                    ToolTip='<%# Eval("PND_PLAN_GROUP_TEXT") %>'></asp:Label>
                                                                                                <asp:HiddenField runat="server" ID="hdfIsExpandedPGroupItem" Value="0" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="24%" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:PlannedPerc %>" HeaderStyle-CssClass="grd-head-rgt">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPGroupPlannedPerc" runat="server" Text='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_PLAN_QTY_PER"))) %>'
                                                                                                    ToolTip='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_PLAN_QTY_PER"))) %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="11%" HorizontalAlign="Right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:PlannedPcs %>" HeaderStyle-CssClass="grd-head-rgt">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPGroupPlannedPcs" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>'
                                                                                                    ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:ProducedPerc %>" HeaderStyle-CssClass="grd-head-rgt">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPGroupProducedPerc" runat="server" Text='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_PRCD_QTY_PER"))) %>'
                                                                                                    ToolTip='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_PRCD_QTY_PER"))) %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:ProducedPcs %>" HeaderStyle-CssClass="grd-head-rgt">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblLineProducedPcs" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PRCD_QTY"))) %>'
                                                                                                    ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PRCD_QTY"))) %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:BalancePerc %>" HeaderStyle-CssClass="grd-head-rgt">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPGroupBalancePerc" runat="server" Text='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_BAL_QTY_PER"))) %>'
                                                                                                    ToolTip='<%# GetFormattedWeightwithComma(Convert.ToString(Eval("PNL_BAL_QTY_PER"))) %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="13%" HorizontalAlign="Right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="<%$ Resources:BalancePcs %>" HeaderStyle-CssClass="grd-head-rgt">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPGroupBalancePcs" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_BAL_QTY"))) %>'
                                                                                                    ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_BAL_QTY"))) %>'></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <RowStyle CssClass="table-thirdlevel" />
                                                                                    <HeaderStyle CssClass="table-thirdlevela" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <RowStyle CssClass="table-secondlevel" />
                                                                <HeaderStyle CssClass="table-secondlevela" />
                                                            </cc1:ExtGridView>
                                                        </div>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <div class="txtAlign-right">
                                                            <asp:Label ID="lblBalancePcsTotal" runat="server" CssClass="bold" />
                                                        </div>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="table-firstlevel" />
                                            <HeaderStyle CssClass="table-firstlevela" />
                                        </cc1:ExtGridView>
                                    </div>
                                </div>--%>
                                <div id="divSummary1" style="overflow-x: hidden; overflow-y: auto; max-height: 400px;"
                                    class="padgtop7">
                                    <asp:GridView ID="grdLinewiseSummary" runat="server" AutoGenerateColumns="False"
                                        Width="100%" AllowPaging="false" CssClass="gridwraptable gridwrap" HeaderStyle-HorizontalAlign="Center"
                                        AllowSorting="True" EmptyDataRowStyle-CssClass="emptytable" EmptyDataRowStyle-HorizontalAlign="Center"
                                        ShowFooter="true" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptyS" runat="server" Text="<%$ Resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ Resources:Line %>" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="8%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSLine" Text='<%# Eval("LNE_NAME") %>' ToolTip='<%# Eval("LNE_NAME") %>'
                                                        runat="server">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:FromDate %>" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSFromDate" Text='<%# Convert.ToDateTime(Eval("PNL_FROM_DATE")).ToString("dd-MMM-yyyy HH:mm") %>'
                                                        ToolTip='<%# Convert.ToDateTime(Eval("PNL_FROM_DATE")).ToString("dd-MMM-yyyy HH:mm") %>'
                                                        runat="server">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:ToDate %>" HeaderStyle-HorizontalAlign="Left">
                                                <ItemStyle HorizontalAlign="Left" Width="12%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSToDate" Text='<%# Convert.ToDateTime(Eval("PNL_TO_DATE")).ToString("dd-MMM-yyyy HH:mm") %>'
                                                        ToolTip='<%# Convert.ToDateTime(Eval("PNL_TO_DATE")).ToString("dd-MMM-yyyy HH:mm") %>'
                                                        runat="server">
                                                    </asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Capacity %>" HeaderStyle-CssClass="grd-head-rgt">
                                                <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSCapacity" Text='<%# GetFormattedNumber(Convert.ToString(Eval("LNE_CAPACITY"))) %>'
                                                        runat="server" ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("LNE_CAPACITY"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalCapacity"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:PlannedPcs %>" HeaderStyle-CssClass="grd-head-rgt">
                                                <ItemStyle HorizontalAlign="Right" Width="7%" CssClass="td-col-bg1" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSPlanned" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>'
                                                        runat="server" ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfPlanned" Value='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPlanned"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="grd-head-rgt">
                                                <ItemStyle HorizontalAlign="Right" Width="8%" CssClass="td-col-bg1" Font-Italic="True" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSPlannedPerc" Text='<%#  "[ " + GetFormattedWeightwithComma(Convert.ToString(Eval("PLAN_QTY_PER"))) + "% ]" %>'
                                                        runat="server" ToolTip='<%#  "[ " + GetFormattedWeightwithComma(Convert.ToString(Eval("PLAN_QTY_PER"))) + "% ]" %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPlannedPerc"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:BalancePcs %>" HeaderStyle-CssClass="grd-head-rgt">
                                                <ItemStyle HorizontalAlign="Right" Width="7%" CssClass="td-col-bg1" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSPlannedBalance" Text='<%# Convert.ToDouble(Eval("PLAN_QTY_BAL")) > 0 ? GetFormattedNumber(Convert.ToString(Eval("PLAN_QTY_BAL"))) : "0" %>'
                                                        runat="server" ToolTip='<%# Convert.ToDouble(Eval("PLAN_QTY_BAL")) > 0 ? GetFormattedNumber(Convert.ToString(Eval("PLAN_QTY_BAL"))) : "0" %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfQtyBal" Value='<%# Convert.ToDouble(Eval("PLAN_QTY_BAL")) > 0 ? GetFormattedNumber(Convert.ToString(Eval("PLAN_QTY_BAL"))) : "0" %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalPlannedBalance"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="grd-head-rgt">
                                                <ItemStyle HorizontalAlign="Right" Width="8%" CssClass="td-col-bg1" Font-Italic="True" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalancePerc" Text='<%#  Convert.ToDouble(Eval("PLAN_QTY_BAL")) > 0 ? "[ " + GetFormattedWeightwithComma(Convert.ToString(Eval("PLAN_QTY_BAL_PER"))) + "% ]" : "[ 0% ]" %>'
                                                        runat="server" ToolTip='<%# Convert.ToDouble(Eval("PLAN_QTY_BAL")) > 0 ? "[ " + GetFormattedWeightwithComma(Convert.ToString(Eval("PLAN_QTY_BAL_PER"))) + "% ]" : "[ 0% ]" %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalBalance"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:ProducedPcs %>" HeaderStyle-CssClass="grd-head-rgt">
                                                <ItemStyle HorizontalAlign="Right" Width="7%" CssClass="td-col-bg2" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSProduced" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PRDCD_QTY")))  %>'
                                                        runat="server" ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PRDCD_QTY"))) + "[ " + GetFormattedWeightwithComma(Convert.ToString(Eval("PROD_QTY_PER"))) + "% ]"  %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfProdPcs" Value='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PRDCD_QTY")))  %>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalProduced"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="grd-head-rgt">
                                                <ItemStyle HorizontalAlign="Right" Width="8%" CssClass="td-col-bg2" Font-Italic="True" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSProducedPerc" Text='<%# "[ " + GetFormattedWeightwithComma(Convert.ToString(Eval("PROD_QTY_PER"))) + "% ]" %>'
                                                        runat="server" ToolTip='<%# "[ " + GetFormattedWeightwithComma(Convert.ToString(Eval("PROD_QTY_PER"))) + "% ]" %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalProducedPerc"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:BalancePcs %>" HeaderStyle-CssClass="grd-head-rgt">
                                                <ItemStyle HorizontalAlign="Right" Width="7%" CssClass="td-col-bg2" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSProducedBalance" Text='<%# Convert.ToDouble(Eval("PROD_QTY_BAL")) > 0 ? GetFormattedNumber(Convert.ToString(Eval("PROD_QTY_BAL"))) : "0"%>'
                                                        runat="server" ToolTip='<%# Convert.ToDouble(Eval("PROD_QTY_BAL")) > 0 ? GetFormattedNumber(Convert.ToString(Eval("PROD_QTY_BAL"))) : "0" %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label runat="server" ID="lblTotalProducedBalance"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-CssClass="grd-head-rgt">
                                                <ItemStyle HorizontalAlign="Right" Width="8%" CssClass="td-col-bg2" Font-Italic="True" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSProducedBalancePerc" Text='<%# "[ " + GetFormattedWeightwithComma(Convert.ToString(Eval("PROD_QTY_BAL_PER"))) + "% ]" %>'
                                                        runat="server" ToolTip='<%# "[ " + GetFormattedWeightwithComma(Convert.ToString(Eval("PROD_QTY_BAL_PER"))) + "% ]" %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divScenario" style="overflow-x: hidden; overflow-y: auto; max-height: 400px;"
                                class="padgtop7">
                                <div class="button-wrap-right">
                                    <asp:CheckBox runat="server" ID="chkOptimizeScenario" ToolTip="<%$ Resources:Optimize %>" CssClass="margnbotm0 margntop3" onClick="ShowHideScenarioSplit();" />
                                    <asp:Label runat="server" ID="lblOptimizeScenario" Text="<%$ Resources:Optimize %>"></asp:Label>

                                    <asp:CheckBox runat="server" ID="chkScenarioSplit" ToolTip="<%$ Resources:Split %>" CssClass="margnbotm0 margntop3" />
                                    <asp:Label runat="server" ID="lblScenarioSplit" Text="<%$ Resources:Split %>"></asp:Label>

                                    <asp:Button runat="server" TabIndex="101" ID="btnCalculate" CommandName="CALCULATE"
                                        Text="<%$ Resources:Calculate %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-process"
                                        ToolTip="<%$ Resources:Calculate %>" OnClick="ActionHandler" />
                                    <asp:Button runat="server" TabIndex="101" ID="btnSavePlan" CommandName="SAVEPLAN"
                                        Text="<%$ Resources:SavePlan %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                        ToolTip="<%$ Resources:SavePlan %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')" />
                                </div>
                                <div class="clear">
                                </div>
                                <div class="gridwrap hierarchical-wrap2" id="div1" grid="grdGrouplistScenario">
                                    <asp:HiddenField ID="hdfScenario_ExpandPosition" runat="server" />
                                    <cc1:ExtGridView runat="server" ID="grdGrouplistScenario" AutoGenerateColumns="False"
                                        Width="100%" ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                        GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                        ShowFooter="true" FooterStyle-CssClass="grdfooter" OnRowDataBound="ActionHandler">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ Resources:ProductGroup %>" HeaderStyle-HorizontalAlign="Left"
                                                HeaderStyle-Width="33%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductGroup" runat="server" Text='<%#  ERP.Production.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("PND_PLAN_GROUP_TEXT")),65) %>'
                                                        ToolTip='<%# Eval("PND_PLAN_GROUP_TEXT") %>'></asp:Label>
                                                    <asp:HiddenField runat="server" ID="hdfIsExpandedGroupItem" Value="0" />
                                                    <asp:HiddenField runat="server" ID="hdfPlanDtlPK" Value='<%# Eval("PND_PK") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfPlanGroupPK" Value='<%# Eval("PND_PLAN_GROUP") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfPlanDtlSLNO" Value='<%# Eval("PND_SL_NO") %>' />
                                                    <asp:HiddenField runat="server" ID="hdfAGradePercDtl" Value='<%# Eval("ISD_AGRADE_PER") %>' />
                                                    <asp:Button runat="server" ID="btnGroupDetails" OnClick="ActionHandler" CommandName="SCENARIOGROUPITEMS"
                                                        CommandArgument='<%# Eval("PND_PLAN_GROUP") %>' EnableTheming="false" Style="display: none" />
                                                </ItemTemplate>
                                                <%--<ItemStyle Width="30%" />--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Requiredby %>" HeaderStyle-CssClass="grd-head-center"
                                                HeaderStyle-Width="12%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRequiredby" runat="server" Text='<%# Convert.ToDateTime(Eval("PND_REQUIRED_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'
                                                        ToolTip='<%# Convert.ToDateTime(Eval("PND_REQUIRED_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                                <%--Width="8%" --%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:BalancetoPlan %>" HeaderStyle-CssClass="grd-head-rgt"
                                                HeaderStyle-Width="16%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalancetoPlan" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_TOT_BAL_TO_PLAN"))) %>'
                                                        ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_TOT_BAL_TO_PLAN"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <%--Width="8%"--%>
                                                <FooterTemplate>
                                                    <div class="txt-center">
                                                        <asp:Label ID="lblTotalName" runat="server" CssClass="bold" Text="<%$ resources:Total %>" />
                                                    </div>
                                                </FooterTemplate>
                                                <FooterStyle Width="8%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="text-align: right;">
                                                        <asp:Label ID="lblTotalBalQty" runat="server" CssClass="bold" />
                                                    </div>
                                                </FooterTemplate>
                                                <FooterStyle Width="16%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:CurrentPlan %>" HeaderStyle-CssClass="grd-head-rgt"
                                                HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <uc2:NumericControl ID="txtTotalCurrPlan" runat="server" ControlType="NumericInteger"
                                                        CssClass="numeric medium" onchange="ScenarioGroupQtyChanged($(this).parent().parent());"
                                                        onblur="CalculateScenarioListTotal();" Text='<%# Eval("PND_PLAN_QTY") %>' TabIndex="28" />
                                                </ItemTemplate>
                                                <ItemStyle CssClass="txtAlign-right margn-rgt0" />
                                                <%--Width="12%"--%>
                                                <FooterStyle Width="18.5%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <%--HeaderStyle-Width="2%"--%>
                                                <HeaderTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkbtnAGradeHdr" Text="<%$ Resources:AGradeRealizationCaption %>"
                                                        ToolTip="<%$ Resources:AGradeRealizationTooltip %>" OnClientClick="javascript:return CalculateTotalwithAGradeScenario();"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkbtnAGradeDtl" Text="<%$ Resources:AGradeRealizationCaption %>"
                                                        ToolTip="<%$ Resources:AGradeRealizationTooltip %>" OnClientClick="javascript:return CalculateTotalwithAGradeScenario($(this).parent().parent());"></asp:LinkButton>
                                                </ItemTemplate>
                                                <%--<ItemStyle Width="2%" />--%>
                                                <FooterTemplate>
                                                    <div style="text-align: right;">
                                                        <asp:Label ID="lblTotalPlanQty" runat="server" CssClass="bold" />
                                                    </div>
                                                </FooterTemplate>
                                                <FooterStyle Width="10%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:ProdQty %>" HeaderStyle-CssClass="grd-head-rgt"
                                                HeaderStyle-Width="15%">
                                                <%----%>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProdQty" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PND_LINE_PLANNED"))) %>'
                                                        ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PND_LINE_PLANNED"))) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <%--Width="10%" --%>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox runat="server" ID="chkAllCombinedLines" AutoPostBack="true" OnCheckedChanged="ActionHandler"
                                                        ToolTip="<%$ Resources:ChkAllCombinedLines %>" CssClass="checkbx-inline" Style="margin-left: 22px!important;" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imbAddLineScenario" runat="server" SkinID="imbsplitup" OnClick="ActionHandler"
                                                        CommandName="ADD" ToolTip="<%$ Resources:Addline %>" />
                                                    <asp:ImageButton ID="imbMachines" runat="server" SkinID="ad-inner" OnClick="ActionHandler"
                                                        CommandName="MACHINES" ToolTip="<%$ Resources:ChooseMachines %>" />
                                                </ItemTemplate>
                                                <ItemStyle Wrap="false" />
                                                <%--Width="5%"--%>
                                                <FooterTemplate>
                                                    <div style="text-align: right;">
                                                        <asp:Label ID="lblTotalProdnQty" runat="server" CssClass="bold" />
                                                    </div>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:Line %>" HeaderStyle-Width="2%">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeadLineGroup" runat="server" Text='<%# Eval("PND_LINE_TEXT") %>'
                                                        ToolTip='<%# Eval("PND_LINE_TEXT") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ Resources:DeadLineMet %>" HeaderStyle-Width="0%"
                                                HeaderStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDeadLineMetGroup" runat="server" Text='<%# Eval("PND_DDL_MET").ToString() != "" ? Eval("PND_DDL_MET").ToString() == "0" ? "No" : "Yes" : "" %>'
                                                        ToolTip='<%# Eval("PND_DDL_MET").ToString() != "" ? Eval("PND_DDL_MET").ToString() == "0" ? "No" : "Yes" : "" %>'
                                                        CssClass='<%#  Eval("PND_DDL_MET").ToString() == "0" ? "txt-red" : "txt-green"  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <div class="hierarchical-gridwrap">
                                                        <asp:HiddenField ID="hdfSize_ExpandPosition" runat="server" />
                                                        <asp:GridView runat="server" ID="grdSizeScenario" AutoGenerateColumns="False" Width="100%"
                                                            ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                            GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                            AllowPaging="false">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblSizeEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Size %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSizeText" runat="server" Text='<%# Eval("PNS_SIZE_TEXT") %>' ToolTip='<%# Eval("PNS_SIZE_TEXT") %>'></asp:Label>
                                                                        <asp:HiddenField runat="server" ID="hdfIsExpandedSizeItem" Value="0" />
                                                                        <asp:HiddenField runat="server" ID="hdfSizeVal" Value='<%# Eval("PNS_SIZE") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfSizeDtlSLNO" Value='<%# Eval("PND_SL_NO") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfSizePlanGroup" Value='<%# Eval("SIZE_PLAN_GROUP") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfIsApplied" Value="0" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="23.5%" />
                                                                    <%--<ItemStyle Width="20%" />--%>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:Requiredby %>" HeaderStyle-CssClass="grd-head-center"
                                                                    HeaderStyle-Width="14%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSRequiredby" runat="server" Text='<%# Convert.ToDateTime(Eval("SIZE_REQUIRED_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'
                                                                            ToolTip='<%# Convert.ToDateTime(Eval("SIZE_REQUIRED_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <%--Width="11%"--%>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:BalancetoPlan %>" HeaderStyle-CssClass="grd-head-rgt"
                                                                    HeaderStyle-Width="11%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSBaltoPlan" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_BAL_TO_PLAN"))) %>'
                                                                            ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_BAL_TO_PLAN"))) %>'></asp:Label>
                                                                        <asp:HiddenField runat="server" ID="hdfProportionVal" Value="0" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <%--Width="11%"--%>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:PlannedPcs %>" HeaderStyle-CssClass="grd-head-rgt"
                                                                    HeaderStyle-Width="12%">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSTotalPlanned" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_PLANNED"))) %>'
                                                                            ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_PLANNED"))) %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources:CurrentPlan %>" HeaderStyle-CssClass="grd-head-rgt"
                                                                    HeaderStyle-Width="12.9%">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox runat="server" ID="txtDtlPlanNow" CssClass="numeric medium input-disabled"
                                                                            Enabled="false"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <%--Width="13%"--%>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-CssClass="grd-head-rgt" HeaderStyle-Width="23px">
                                                                    <ItemTemplate>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <%--Width="23%"--%>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <RowStyle CssClass="table-secondlevel" />
                                                            <HeaderStyle CssClass="table-secondlevela" />
                                                        </asp:GridView>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle CssClass="table-firstlevel" />
                                        <HeaderStyle CssClass="table-firstlevela" />
                                    </cc1:ExtGridView>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Listing">
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetLocalResourceObject("AdvanceFilter").ToString()%>
                                            </h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                SkinID="imbArrowInactive" ToolTip="<%$ Resources:ShowFilter %>" TabIndex="1" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                SkinID="imbArrowActive" ToolTip="<%$ Resources:HideFilter %>" TabIndex="41" />
                                            <asp:HiddenField ID="hdfShowHideFilter" runat="server" Value="0" ClientIDMode="Static" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div id="tblOPadvancedSearch">
                                <table class="table-devide">
                                    <tr>
                                        <td style="width: 40%" class="padg-top8">
                                            <label for="ddlFilterBy" class="lbl-15perc">
                                                <asp:Literal ID="ltFilerBy" runat="server" Text="<%$ resources:FilterBy %>"></asp:Literal></label>
                                            <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="1" onkeypress="javascript:return SetFocus(event);"
                                                onchange="SetSearchBy();" CssClass="select-22per">
                                                <asp:ListItem Text="<%$resources:SearchPlanCodeText %>" Value="<%$resources:SearchPlanCodeValue %>"></asp:ListItem>
                                                <asp:ListItem Text="<%$resources:SearchPlanNameText %>" Value="<%$resources:SearchPlanNameValue %>"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtFilterPlanName" runat="server" TabIndex="1" MaxLength="500" CssClass="display-none">
                                            </asp:TextBox>
                                            <asp:TextBox ID="txtFilterPlanCode" runat="server" TabIndex="1" MaxLength="50" CssClass="display-none">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterPlanPK" runat="server" Value="0" />
                                        </td>
                                        <td style="width: 35%" class="padg-top8">
                                            <label for="txtFilterFrom" class="lbl-22-5perc">
                                                <asp:Literal ID="ltFilterFrom" runat="server" Text="<%$ Resources:DateFrom%>"></asp:Literal>
                                            </label>
                                            <asp:TextBox ID="txtFilterFrom" runat="server" CssClass="input-w22per" TabIndex="2"
                                                onkeypress="return isDate(event)" onkeydown="return CheckKey(event)" onpaste="return false;">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterFrom" runat="server" />
                                            <label for="txtFilterTo" class="lbl-22-5perc">
                                                <asp:Literal ID="ltFilterTo" runat="server" Text="<%$ Resources:DateTo%>"></asp:Literal>
                                            </label>
                                            <asp:TextBox ID="txtFilterTo" runat="server" CssClass="input-w22per" TabIndex="3"
                                                onkeypress="return isDate(event)" onkeydown="return CheckKey(event)" onpaste="return false;">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterTo" runat="server" />
                                        </td>
                                        <td style="width: 25%;" class="padg-top8">
                                            <label for="ddlFilterStatus" class="lbl-24-7perc">
                                                <asp:Literal ID="ltFilterStatus" runat="server" Text="<%$ Resources:Controls,Status%>"></asp:Literal>
                                            </label>
                                            <asp:DropDownList runat="server" ID="ddlFilterStatus" TabIndex="5" CssClass="select-28-4per"
                                                onchange="SetSearchBy();">
                                                <asp:ListItem Text='<%$ Resources:Captions,All %>' Value="-1"></asp:ListItem>
                                                <asp:ListItem Text='<%$ Resources:Captions,Active %>' Value="1"></asp:ListItem>
                                                <asp:ListItem Text='<%$ Resources:Captions,InActive %>' Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClick="ActionHandler"
                                                CommandName="SEARCHFILTER" ToolTip="<%$ Resources:Controls,Search %>" Width="18px"
                                                TabIndex="6" Style="margin-top: 2px !important;" />
                                            <asp:ImageButton ID="imbClear" runat="server" SkinID="cancel" OnClick="ActionHandler"
                                                CommandName="CLEARFILTER" ToolTip="<%$ Resources:Controls,Clear %>" Width="18px"
                                                TabIndex="7" Style="margin-top: 2px !important;" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap">
                                <asp:GridView ID="grdPlanList" runat="server" AutoGenerateColumns="False" class="gridwraptable gridwrap tablefixwidth-td"
                                    Width="100%" AllowPaging="false" CssClass="grdWrap" HeaderStyle-HorizontalAlign="Center"
                                    OnPageIndexChanging="ActionHandler" AllowSorting="True" OnSorting="ActionHandler"
                                    EmptyDataRowStyle-CssClass="emptytable" EmptyDataRowStyle-HorizontalAlign="Center"
                                    OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ Resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="2%">
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="18" runat="server" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" AutoPostBack="true"
                                                    OnCheckedChanged="ActionHandler" />
                                                <asp:HiddenField ID="hdfIsActive" runat="server" Value='<%# Eval("PNH_ACTIVE") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:DateFrom %>" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PNH_FROM_DT" ItemStyle-Width="10%" ItemStyle-CssClass="grdText">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFromDate" runat="server" Text='<%# Convert.ToDateTime(Eval("PNH_FROM_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'
                                                    ToolTip='<%# Convert.ToDateTime(Eval("PNH_FROM_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:DateTo %>" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PNH_TO_DT" ItemStyle-Width="10%" ItemStyle-CssClass="grdText">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToDate" runat="server" Text='<%# Convert.ToDateTime(Eval("PNH_TO_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'
                                                    ToolTip='<%# Convert.ToDateTime(Eval("PNH_TO_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:PlanCode %>" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PNH_CODE" ItemStyle-Width="12%" ItemStyle-CssClass="grdText">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlanCode" runat="server" Text='<%# Eval("PNH_CODE").ToString() %>'
                                                    ToolTip='<%# Eval("PNH_CODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:PlanName %>" ItemStyle-HorizontalAlign="Left"
                                            SortExpression="PNH_NAME" ItemStyle-Width="30%" ItemStyle-CssClass="grdText">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlanName" runat="server" Text='<%# ERP.Production.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("PNH_NAME")),50) %>'
                                                    ToolTip='<%# Eval("PNH_NAME") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfPlanlistPK" runat="server" Value='<%# Eval("PNH_PK") %>' />
                                                <asp:HiddenField ID="hdfModedt" runat="server" Value='<%# Eval("LAST_MOD_DT") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Version %>" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="6%" ItemStyle-CssClass="grdText">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVersion" runat="server" Text='<%# Eval("PNH_VERSION") %>' ToolTip='<%# Eval("PNH_VERSION") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:PlannedQty %>" ItemStyle-HorizontalAlign="Right"
                                            ItemStyle-Width="9%" ItemStyle-CssClass="grdText" HeaderStyle-CssClass="grd-head-rgt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlanned" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PLANNED_QTY"))) %>'
                                                    ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PLANNED_QTY"))) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:ProducedQty %>" ItemStyle-HorizontalAlign="Right"
                                            ItemStyle-Width="9%" ItemStyle-CssClass="grdText" HeaderStyle-CssClass="grd-head-rgt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProduced" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PRODUCED_QTY"))) %>'
                                                    ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PRODUCED_QTY"))) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:BalToProduce %>" ItemStyle-HorizontalAlign="Right"
                                            ItemStyle-Width="10%" ItemStyle-CssClass="grdText" HeaderStyle-CssClass="grd-head-rgt">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBalProduce" runat="server" Text='<%# Convert.ToDouble(Eval("BAL_TO_PRODUCE_QTY")) > 0  ? GetFormattedNumber(Convert.ToString(Eval("BAL_TO_PRODUCE_QTY"))) : "0" %>'
                                                    ToolTip='<%# Convert.ToDouble(Eval("BAL_TO_PRODUCE_QTY")) > 0  ? GetFormattedNumber(Convert.ToString(Eval("BAL_TO_PRODUCE_QTY"))) : "0" %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible="false"
                                                    CommandName="ACTIVATE" ToolTip="<%$resources:Captions,Inactive %>" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="10" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible="false"
                                                    CommandName="DEACTIVATE" ToolTip="<%$resources:Captions,Active %>" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="10" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton TabIndex="10" runat="server" ID="imbPrintProcess" CssClass="notprint-icon"
                                                    ToolTip="<%$ resources:ViewProgress %>" value="" OnClick="ActionHandler" CommandName="PRINTLIST"
                                                    Visible='<%# Eval("LINE_COUNT").ToString() == "0" ? false : true %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc3:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="divLines" style="display: none;">
                <div class="content-wrapper">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnApplyLines" SkinID="btnInner-add-dsd" runat="server" Text="<%$ Resources:Controls,Apply %>"
                            CommandName="APPLY" OnClick="ActionHandler" TabIndex="53" />
                    </div>
                    <div class="binhead-search margnbotm10 padgtop0">
                        <table border="0">
                            <tr>
                                <td style="width: 40%;">
                                    <asp:Label ID="lblPlanNameCaption" runat="server" CssClass="pallet-head-lbl minw-85px txt-rgt"
                                        Text="<%$ Resources:PlanName %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblPlanNamePopup" runat="server"></asp:Label></b>
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="lblFromDateCaption" runat="server" CssClass="pallet-head-lbl txt-rgt"
                                        Text="<%$ Resources:DateFrom %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblFromDatePopup" runat="server"></asp:Label>
                                    </b>
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="lblToDateCaption" runat="server" CssClass="pallet-head-lbl lbl-20perc txt-rgt"
                                        Text="<%$ Resources:DateTo %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblToDatePopup" runat="server"></asp:Label>
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 100%;">
                                    <asp:Label ID="lblLineProductGroupCaption" runat="server" CssClass="pallet-head-lbl minw-85px txt-rgt"
                                        Text="<%$ Resources:ProductGroup %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblLineProductGroup" runat="server"></asp:Label></b>
                                    <asp:HiddenField ID="hdfLineProductGroup" runat="server" Value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%;">
                                    <%-- <asp:Label ID="lblGroupSizeCaption" runat="server" CssClass="pallet-head-lbl minw-85px txt-rgt"
                                        Text="<%$ Resources:Size %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblGroupSize" runat="server"></asp:Label></b>--%>
                                    <asp:Label ID="lblCurrPlanQtyCaption" runat="server" CssClass="pallet-head-lbl minw-85px txt-rgt"
                                        Text="<%$ Resources:PlanQty %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblCurrPlanQty" runat="server"></asp:Label></b>
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="lblRequiredbyCaption" runat="server" CssClass="pallet-head-lbl lbl-21-8perc txt-rgt"
                                        Text="<%$ Resources:Requiredby %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblRequiredbyPopup" runat="server"></asp:Label></b>
                                </td>
                                <td style="width: 30%;"></td>
                            </tr>
                        </table>
                    </div>
                    <table class="table-3devide" runat="server" id="tblLineEntry">
                        <td>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label for="ddlLine" class="lbl-33-6perc">
                                            <asp:Literal ID="ltLine" runat="server" Text="<%$ Resources:Line%>"></asp:Literal>
                                        </label>
                                        <asp:DropDownList runat="server" ID="ddlLine" TabIndex="51" CssClass="select-42per"
                                            AutoPostBack="true" OnSelectedIndexChanged="ActionHandler">
                                        </asp:DropDownList>
                                        <asp:HiddenField runat="server" ID="hdfLineDtlPK" Value="0" />
                                        <asp:HiddenField runat="server" ID="hdfLineSLNO" Value="0" />
                                        <asp:HiddenField runat="server" ID="hdfLinePlanGroup" Value="0" />
                                        <asp:HiddenField runat="server" ID="hdfLinePlanGroupSize" Value="0" />
                                        <asp:HiddenField runat="server" ID="hdfLinePlanGroupDtlPK" Value="0" />
                                        <asp:HiddenField runat="server" ID="hdfLinePlanGroupSlNO" Value="0" />
                                        <asp:HiddenField runat="server" ID="hdfLineRequiredQty" Value="0" />
                                        <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="rfvLine" SetFocusOnError="true" ValidationGroup="AddLine"
                                                EnableClientScript="true" runat="server" ControlToValidate="ddlLine" Text="*"
                                                ErrorMessage="<%$ resources: Msg_SelectLine %>" Display="Dynamic" CssClass="star"
                                                InitialValue="-1">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label for="lblLineFromDt" class="lbl-33-6perc">
                                            <asp:Literal ID="ltLineFromDt" runat="server" Text="<%$ Resources:FromDate%>"></asp:Literal>
                                        </label>
                                        <asp:TextBox runat="server" ID="txtLineFromDt" CssClass="input-w25per" TabIndex="52"
                                            onkeypress="return isDate(event)" onkeydown="return CheckKey(event)" onpaste="return false;"
                                            onblur="CalculateLineCapacity();">
                                        </asp:TextBox>
                                        <asp:TextBox runat="server" ID="txtLineFromTime" TabIndex="52" MaxLength="10" CssClass="input-w11-5per"
                                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                        <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="rfvLineFromDt" SetFocusOnError="true" ValidationGroup="AddLine"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtLineFromDt" Text="*"
                                                ErrorMessage="<%$ resources: Msg_Empty_Line_FromDt %>" Display="Dynamic" CssClass="star">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvLineFromTime" SetFocusOnError="true" ValidationGroup="AddLine"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtLineFromTime"
                                                Text="*" ErrorMessage="<%$ resources: Msg_Empty_Line_FromTime %>" Display="Dynamic"
                                                CssClass="star">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label for="lblLineToDt" class="lbl-33-6perc">
                                            <asp:Literal ID="ltLineToDt" runat="server" Text="<%$ Resources:ToDate%>"></asp:Literal>
                                        </label>
                                        <asp:TextBox runat="server" ID="txtLineToDt" CssClass="input-w25per" TabIndex="53"
                                            onkeypress="return isDate(event)" onkeydown="return CheckKey(event)" onpaste="return false;"
                                            onblur="CalculateLineCapacity();">
                                        </asp:TextBox>
                                        <asp:TextBox runat="server" ID="txtLineToTime" TabIndex="53" MaxLength="53" CssClass="input-w11-5per"
                                            onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                        <asp:ImageButton ID="imgSrchLnDtls" runat="server" SkinID="search" OnClick="ActionHandler"
                                            CommandName="SEARCH" TabIndex="55" />
                                        <asp:HiddenField ID="hdfStartDate" runat="server" />
                                        <asp:HiddenField ID="hdfEndDate" runat="server" />
                                        <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="rfvToDt" SetFocusOnError="true" ValidationGroup="AddLine"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtLineToDt" Text="*"
                                                ErrorMessage="<%$ resources: Msg_Empty_Line_ToDt %>" Display="Dynamic" CssClass="star">
                                            </asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="rfvToTime" SetFocusOnError="true" ValidationGroup="AddLine"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtLineToTime" Text="*"
                                                ErrorMessage="<%$ resources: Msg_Empty_Line_ToTime %>" Display="Dynamic" CssClass="star">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label for="lblLineSpeed" class="lbl-33-6perc">
                                            <asp:Literal ID="ltLineSpeed" runat="server" Text="<%$ Resources:LineSpeed%>"></asp:Literal>
                                        </label>
                                        <uc2:NumericControl ID="ucLineSpeed" runat="server" ControlType="NumericInteger"
                                            CssClass="input-w39-5per numeric" tabindex="54" onchange="CalculateLineCapacity();"></uc2:NumericControl>
                                        <asp:HiddenField runat="server" ID="hdfHolderPos" Value="0" />
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label for="lblPlant" class="lbl-33-6perc">
                                            <asp:Literal ID="ltPlant" runat="server" Text="<%$ Resources:Plant%>"></asp:Literal>
                                        </label>
                                        <asp:Label ID="lblPlant" runat="server" CssClass="lbl-39-3perc"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label for="lblCapacity" class="lbl-33-6perc">
                                            <asp:Literal ID="ltCapacity" runat="server" Text="<%$ Resources:Capacity%>"></asp:Literal>
                                            <asp:HiddenField ID="hdfdayCapacity" runat="server" Value="0" />
                                        </label>
                                        <asp:Label ID="lblCapacity" runat="server" CssClass="lbl-39-3perc numeric"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label for="lblLinePlanned" class="lbl-33-6perc">
                                            <asp:Literal ID="ltLinePlanned" runat="server" Text="<%$ Resources:Planned%>"></asp:Literal>
                                        </label>
                                        <asp:Label ID="lblLinePlanned" runat="server" CssClass="lbl-39-3perc numeric"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label for="lblBalLineQty" class="lbl-33-6perc">
                                            <asp:Literal ID="ltBalLineQty" runat="server" Text="<%$ Resources:LineBalance%>"></asp:Literal>
                                        </label>
                                        <asp:Label ID="lblBalLineQty" runat="server" CssClass="lbl-39-3perc numeric"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label for="txtLineQty" class="lbl-33-6perc">
                                            <asp:Literal ID="ltLineQty" runat="server" Text="<%$ Resources:LineQty%>"></asp:Literal>
                                        </label>
                                        <uc2:NumericControl ID="ucLineQty" runat="server" ControlType="NumericInteger" CssClass="input-w39-5per numeric"
                                            tabindex="55"></uc2:NumericControl>
                                        <asp:ImageButton ID="imbAddLineDtl" runat="server" SkinID="imbaddnew" OnClick="ActionHandler"
                                            OnClientClick="javascript:return ValidatePageNow('AddLine');" CommandName="ADD"
                                            TabIndex="55" />
                                    </div>
                                </td>
                            </tr>
                    </table>
                    <div class="grdTable" style="overflow-x: hidden;" runat="server" id="divLinePopUp">
                        <asp:GridView ID="grdLines" runat="server" AutoGenerateColumns="False" Width="100%"
                            AllowPaging="false" CssClass="gridwraptable gridwrap" HeaderStyle-HorizontalAlign="Center"
                            AllowSorting="True" EmptyDataRowStyle-CssClass="emptytable" EmptyDataRowStyle-HorizontalAlign="Center"
                            ShowFooter="true" OnRowDataBound="ActionHandler">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmptyText" runat="server" Text="<%$ Resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:RadioButton CssClass="rdoSelectionLine" TabIndex="18" runat="server" GroupName="SelectOne"
                                            ID="rbtSelectPlanLine" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" AutoPostBack="true"
                                            OnCheckedChanged="ActionHandler" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Line %>" HeaderStyle-HorizontalAlign="Left">
                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineName" Text='<%#Eval("LNE_CODE")%>' runat="server" ToolTip='<%#Eval("LNE_CODE")%>' />
                                        <asp:HiddenField ID="hdfLinePK" runat="server" Value='<%#Eval("PNL_LINE")%>' />
                                        <asp:HiddenField runat="server" ID="hdfDtlPK" Value='<%#Eval("PNL_PK")%>' />
                                        <asp:HiddenField runat="server" ID="hdfSLNO" Value='<%#Eval("SLNO")%>' />
                                        <asp:HiddenField runat="server" ID="hdfLineFromDate" Value='<%#Eval("PNL_FROM_DATE")%>' />
                                        <asp:HiddenField runat="server" ID="hdfLineToDate" Value='<%#Eval("PNL_TO_DATE")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Period %>" HeaderStyle-CssClass="grd-head-center">
                                    <ItemStyle HorizontalAlign="Center" Width="29%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLinePeriod" Text='<%# Convert.ToDateTime(Eval("PNL_FROM_DATE")).ToString(GetGlobalResourceObject("Constants", "DateFormatTime").ToString()) + " To " + Convert.ToDateTime(Eval("PNL_TO_DATE")).ToString(GetGlobalResourceObject("Constants", "DateFormatTime").ToString()) %>'
                                            runat="server" ToolTip='<%# Convert.ToDateTime(Eval("PNL_FROM_DATE")).ToString(GetGlobalResourceObject("Constants", "DateFormatTime").ToString()) + " To " + Convert.ToDateTime(Eval("PNL_TO_DATE")).ToString(GetGlobalResourceObject("Constants", "DateFormatTime").ToString()) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:ProdHrs  %>" HeaderStyle-CssClass="grd-head-center">
                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblProdHrs" Text='<%# Eval("PRD_HRS") %>' runat="server" ToolTip='<%# Eval("PRD_HRS") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalProdHrs" runat="server" CssClass="bold" />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:MaintainHrs  %>" HeaderStyle-CssClass="grd-head-center"
                                    Visible="false">
                                    <ItemStyle HorizontalAlign="Center" Width="7%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblMaintainHrs" Text='<%# Eval("REPAIR_HRS") %>' runat="server" ToolTip='<%# Eval("REPAIR_HRS") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:LineSpeed %>" HeaderStyle-CssClass="grd-head-rgt" Visible="<%$ Resources:ShowHideSpeed %>">
                                    <ItemStyle HorizontalAlign="Right" Width="8%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineSpeed" Text='<%# GetFormattedWeightwithComma(Eval("PNL_LNE_AVG_SPEED"))%>' runat="server"
                                            ToolTip='<%# Eval("PNL_LNE_AVG_SPEED")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Plant %>" HeaderStyle-HorizontalAlign="Left">
                                    <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLinePlant" Text='<%#Eval("LNE_PLANT_NAME")%>' runat="server" ToolTip='<%#Eval("LNE_PLANT_NAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Capacity %>" HeaderStyle-CssClass="grd-head-rgt"
                                    Visible="false">
                                    <ItemStyle HorizontalAlign="Right" Width="12%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineCapacity" Text='<%# GetFormattedNumber(Convert.ToString(Eval("LNE_CAPACITY"))) %>'
                                            runat="server" ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("LNE_CAPACITY"))) %>' />
                                        <asp:HiddenField ID="hdfLineCapacity" runat="server" Value='<%# Eval("LNE_CAPACITY") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div class="txt-rgt">
                                            <asp:Label ID="lblLineTotal" runat="server" Text="<%$ resources:Total %>" CssClass="bold" />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:LineQty %>" HeaderStyle-CssClass="grd-head-rgt">
                                    <ItemStyle HorizontalAlign="Right" Width="12%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineQty" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>'
                                            runat="server" ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalLineQty" runat="server" CssClass="bold" />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbLineEdit"
                                            SkinID="imbeditgrid" OnClick="ActionHandler" CommandName="EDIT_ACTION" TabIndex="53"
                                            ToolTip="<%$ Resources:Controls,Edit %>" />
                                        <asp:ImageButton Width="16px" Height="16px" OnClientClick="return ShowDeleteConfirm(this);"
                                            runat="server" ID="imbLineDelete" SkinID="imbdeletegrid" OnClick="ActionHandler"
                                            CommandName="DELETE_ACTION" TabIndex="53" ToolTip="<%$ Resources:Controls,Delete %>" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div id="divPlanlnFormerDtls" runat="server">
                            <h3>
                                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:FormerDetails %>" /></h3>
                            <asp:GridView ID="grdPlanlineFormer" runat="server" AutoGenerateColumns="False" Width="100%"
                                AllowPaging="false" CssClass="gridwraptable gridwrap" HeaderStyle-HorizontalAlign="Center"
                                AllowSorting="True" EmptyDataRowStyle-CssClass="emptytable" EmptyDataRowStyle-HorizontalAlign="Center"
                                ShowFooter="false">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmptyText" runat="server" Text="<%$ Resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:Size %>" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSize" Text='<%#Eval("PLD_SIZ_TEXT")%>' runat="server" ToolTip='<%#Eval("PLD_SIZ_TEXT")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Count %>" HeaderStyle-CssClass="grd-head-rgt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRatio" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PLD_FORMER_QTY"))) %>'
                                                runat="server" ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PLD_FORMER_QTY"))) %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:ProdQty %>" HeaderStyle-CssClass="grd-head-rgt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProducedQty" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PLD_PRODUCTION_QTY"))) %>'
                                                runat="server" ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PLD_PRODUCTION_QTY"))) %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divLinesScenario" style="display: none;">
                <div class="content-wrapper">
                    <div class="Button-container-popup">
                        <%--<asp:Button ID="Button1" SkinID="btnInner-add-dsd" runat="server" Text="<%$ Resources:Controls,Apply %>"
                            CommandName="APPLY" OnClick="ActionHandler" TabIndex="53" />--%>
                    </div>
                    <div class="binhead-search margnbotm10 padgtop0">
                        <table border="0">
                            <tr>
                                <td colspan="2" style="width: 100%;">
                                    <asp:Label ID="lblPlanGrScenariohdr" runat="server" CssClass="pallet-head-lbl minw-85px txt-rgt"
                                        Text="<%$ Resources:ProductGroup %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblPlanGrScenario" runat="server"></asp:Label></b>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%;">
                                    <asp:Label ID="lblPlanQtyScenariohdr" runat="server" CssClass="pallet-head-lbl minw-85px txt-rgt"
                                        Text="<%$ Resources:PlanQty %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblPlanQtyScenario" runat="server"></asp:Label></b>
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="lblPlanReqByScenariohdr" runat="server" CssClass="pallet-head-lbl lbl-21-8perc txt-rgt"
                                        Text="<%$ Resources:Requiredby %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblPlanReqByScenario" runat="server"></asp:Label></b>
                                </td>
                                <td style="width: 30%;"></td>
                            </tr>
                        </table>
                    </div>
                    <div class="grdTable max-392">
                        <asp:GridView ID="grdLineScenario" runat="server" AutoGenerateColumns="False" Width="100%"
                            AllowPaging="false" CssClass="gridwraptable gridwrap" HeaderStyle-HorizontalAlign="Center"
                            AllowSorting="True" EmptyDataRowStyle-CssClass="emptytable" EmptyDataRowStyle-HorizontalAlign="Center"
                            ShowFooter="true" OnRowDataBound="ActionHandler">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmptyText" runat="server" Text="<%$ Resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="2%">
                                    <ItemTemplate>
                                        <asp:RadioButton CssClass="rdoSelection" TabIndex="18" runat="server" GroupName="SelectOne"
                                            ID="rbtSelectLine" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" AutoPostBack="true"
                                            OnCheckedChanged="ActionHandler" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Line %>" HeaderStyle-HorizontalAlign="Left">
                                    <ItemStyle HorizontalAlign="Left" Width="5%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineName" Text='<%#Eval("LNE_CODE")%>' runat="server" ToolTip='<%#Eval("LNE_CODE")%>' />
                                        <asp:HiddenField ID="hdfLinePK" runat="server" Value='<%#Eval("PNL_LINE")%>' />
                                        <asp:HiddenField runat="server" ID="hdfDtlPK" Value='<%#Eval("PNL_PK")%>' />
                                        <asp:HiddenField runat="server" ID="hdfSLNO" Value='<%#Eval("SLNO")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Period  %>" HeaderStyle-CssClass="grd-head-center">
                                    <ItemStyle HorizontalAlign="Center" Width="29%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineFromDt" Text='<%# Convert.ToDateTime(Eval("PNL_FROM_DATE")).ToString(GetGlobalResourceObject("Constants", "DateFormatTime").ToString()) + " To " + Convert.ToDateTime(Eval("PNL_TO_DATE")).ToString(GetGlobalResourceObject("Constants", "DateFormatTime").ToString()) %>'
                                            runat="server" ToolTip='<%# Convert.ToDateTime(Eval("PNL_FROM_DATE")).ToString(GetGlobalResourceObject("Constants", "DateFormatTime").ToString()) + " To " + Convert.ToDateTime(Eval("PNL_TO_DATE")).ToString(GetGlobalResourceObject("Constants", "DateFormatTime").ToString()) %>' />
                                        <asp:HiddenField runat="server" ID="hdfLineScenarioEndDate" Value='<%# Convert.ToDateTime(Eval("PNL_TO_DATE")).ToString(GetGlobalResourceObject("Constants", "DateFormatTime").ToString()) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:ProdHrs  %>" HeaderStyle-CssClass="grd-head-center">
                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblProdHrs" Text='<%# Eval("PRD_HRS") %>' runat="server" ToolTip='<%# Eval("PRD_HRS") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalProdHrs" runat="server" CssClass="bold" />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:MaintainHrs  %>" HeaderStyle-CssClass="grd-head-center"
                                    Visible="false">
                                    <ItemStyle HorizontalAlign="Center" Width="7%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblMaintainHrs" Text='<%# Eval("REPAIR_HRS") %>' runat="server" ToolTip='<%# Eval("REPAIR_HRS") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:LineSpeed %>" HeaderStyle-CssClass="grd-head-rgt">
                                    <ItemStyle HorizontalAlign="Right" Width="6%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineSpeed" Text='<%# GetFormattedWeightwithComma(Eval("PNL_LNE_AVG_SPEED"))%>' runat="server"
                                            ToolTip='<%# Eval("PNL_LNE_AVG_SPEED")%>' Visible="<%$ Resources:ShowHideSpeed %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Plant %>" HeaderStyle-HorizontalAlign="Left">
                                    <ItemStyle HorizontalAlign="Left" Width="12%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLinePlant" Text='<%#Eval("LNE_PLANT_NAME")%>' runat="server" ToolTip='<%#Eval("LNE_PLANT_NAME")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:DeadLineMet %>" HeaderStyle-HorizontalAlign="Left"
                                    HeaderStyle-Wrap="false">
                                    <ItemStyle HorizontalAlign="Left" Width="8%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeadLineMet" Text='<%# Eval("PNL_DDL_MET").ToString() == "0" ? "No" : "Yes" %>'
                                            runat="server" ToolTip='<%# Eval("PNL_DDL_MET").ToString() == "0" ? "No" : "Yes" %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Capacity %>" HeaderStyle-CssClass="grd-head-rgt"
                                    Visible="false">
                                    <ItemStyle HorizontalAlign="Right" Width="12%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineCapacity" Text='<%# GetFormattedNumber(Convert.ToString(Eval("LNE_CAPACITY"))) %>'
                                            runat="server" ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("LNE_CAPACITY"))) %>' />
                                        <asp:HiddenField ID="hdfLineCapacity" runat="server" Value='<%# Eval("LNE_CAPACITY") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div class="txt-rgt">
                                            <asp:Label ID="lblLineTotal" runat="server" Text="<%$ resources:Total %>" CssClass="bold" />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:LineQty %>" HeaderStyle-CssClass="grd-head-rgt">
                                    <ItemStyle HorizontalAlign="Right" Width="12%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineQty" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>'
                                            runat="server" ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PNL_PLAN_QTY"))) %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalLineQty" runat="server" CssClass="bold" />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbLineEdit"
                                            SkinID="imbeditgrid" OnClick="ActionHandler" CommandName="EDIT_ACTION" TabIndex="53"
                                            ToolTip="<%$ Resources:Controls,Edit %>" />
                                        <asp:ImageButton Width="16px" Height="16px" OnClientClick="return ShowDeleteConfirm(this);"
                                            runat="server" ID="imbLineDelete" SkinID="imbdeletegrid" OnClick="ActionHandler"
                                            CommandName="DELETE_ACTION" TabIndex="53" ToolTip="<%$ Resources:Controls,Delete %>" />
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                        <div id="divFormerDtls" runat="server">
                            <h3>
                                <asp:Literal ID="ltFormerDetials" runat="server" Text="<%$ Resources:FormerDetails %>" /></h3>
                            <asp:GridView ID="grdFormerDetails" runat="server" AutoGenerateColumns="False" Width="100%"
                                AllowPaging="false" CssClass="gridwraptable gridwrap" HeaderStyle-HorizontalAlign="Center"
                                AllowSorting="True" EmptyDataRowStyle-CssClass="emptytable" EmptyDataRowStyle-HorizontalAlign="Center"
                                ShowFooter="false">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmptyText" runat="server" Text="<%$ Resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:Size %>" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSize" Text='<%#Eval("PLD_SIZ_TEXT")%>' runat="server" ToolTip='<%#Eval("PLD_SIZ_TEXT")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Count %>" HeaderStyle-CssClass="grd-head-rgt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRatio" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PLD_FORMER_QTY"))) %>'
                                                runat="server" ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PLD_FORMER_QTY"))) %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:ProdQty %>" HeaderStyle-CssClass="grd-head-rgt">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProducedQty" Text='<%# GetFormattedNumber(Convert.ToString(Eval("PLD_PRODUCTION_QTY"))) %>'
                                                runat="server" ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("PLD_PRODUCTION_QTY"))) %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divMachines" style="display: none;">
                <div class="content-wrapper">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnMachineApply" SkinID="btnInner-add-dsd" runat="server" Text="<%$ Resources:Controls,Apply %>"
                            CommandName="APPLY" OnClick="ActionHandler" TabIndex="53" />
                    </div>
                    <div style="display: none;">
                        <asp:Label runat="server" ID="lblWithFormers" Text="<%$ Resources:WithFormers %>"
                            AssociatedControlID="chkWithFormers"></asp:Label>
                        <asp:CheckBox runat="server" ID="chkWithFormers" />
                    </div>
                    <asp:CheckBox runat="server" ID="chkCombinedLines" CssClass="margntop2" />
                    <asp:Label runat="server" ID="lblCombinedLines" Text="<%$ Resources:CombinedLines %>"
                        AssociatedControlID="chkCombinedLines"></asp:Label>
                    <div class="grdTable max-250">
                        <asp:GridView ID="grdMachines" runat="server" AutoGenerateColumns="False" Width="100%"
                            AllowPaging="false" CssClass="gridwraptable gridwrap" HeaderStyle-HorizontalAlign="Center"
                            AllowSorting="True" EmptyDataRowStyle-CssClass="emptytable" EmptyDataRowStyle-HorizontalAlign="Center">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmptyText" runat="server" Text="<%$ Resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAllActive" runat="server" onClick="javascript:CheckBoxSelection();" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkMachine" Checked='<%# Eval("Ischecked").ToString() == "0" ? false : true %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="2%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Line %>">
                                    <ItemStyle HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLineName" Text='<%#Eval("LNE_CODE")%>' runat="server" ToolTip='<%#Eval("LNE_CODE")%>' />
                                        <asp:HiddenField ID="hdfLinePK" runat="server" Value='<%#Eval("PNS_LNE_PK")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div id="divSCDetails" style="display: none;">
                <div class="content-wrapper">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnApplySCDtls" SkinID="btnInner-add-dsd" runat="server" Text="<%$ Resources:Controls,Apply %>"
                            CommandName="APPLY" OnClick="ActionHandler" TabIndex="127" />
                    </div>
                    <div class="binhead-search margnbotm10 padgtop0">
                        <table border="0">
                            <tr>
                                <td style="width: 40%;">
                                    <asp:Label ID="lblSPlanNameC" runat="server" CssClass="pallet-head-lbl minw-85px txt-rgt"
                                        Text="<%$ Resources:PlanName %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblSPlanName" runat="server"></asp:Label></b>
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="lblSDFromC" runat="server" CssClass="pallet-head-lbl txt-rgt" Text="<%$ Resources:DateFrom %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblSDFrom" runat="server"></asp:Label>
                                    </b>
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="lblDDToC" runat="server" CssClass="pallet-head-lbl lbl-20perc txt-rgt"
                                        Text="<%$ Resources:DateTo %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblSDTo" runat="server"></asp:Label>
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="width: 100%;">
                                    <asp:Label ID="lblSPGroupC" runat="server" CssClass="pallet-head-lbl minw-85px txt-rgt"
                                        Text="<%$ Resources:ProductGroup %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblSPGroup" runat="server"></asp:Label></b>
                                    <asp:HiddenField ID="hdfSPGroupVal" runat="server" Value="0" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%;">
                                    <asp:Label ID="lblGroupSizeCaption" runat="server" CssClass="pallet-head-lbl minw-85px txt-rgt"
                                        Text="<%$ Resources:Size %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblGroupSize" runat="server"></asp:Label></b>
                                    <asp:HiddenField ID="hdfSCSize" runat="server" Value="0" />
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="lblSPlanQtyC" runat="server" CssClass="pallet-head-lbl lbl-21-8per txt-rgt"
                                        Text="<%$ Resources:PlanQty %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblSPlanQty" runat="server"></asp:Label></b>
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="lblSReqdByC" runat="server" CssClass="pallet-head-lbl lbl-21-8perc txt-rgt"
                                        Text="<%$ Resources:Requiredby %>"></asp:Label>
                                    : <b>
                                        <asp:Label ID="lblSReqdBy" runat="server"></asp:Label></b>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:GridView runat="server" ID="grdOrderDtls" AutoGenerateColumns="False" Width="100%"
                        EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false" ShowFooter="true">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblMenuEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:Order %>" HeaderStyle-CssClass="selected">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkOrderNo" runat="server" Text='<%# Eval("SOH_NO") %>' ToolTip='<%# Eval("SOH_NO") %>'
                                        CssClass="text-underline" OnClick="ActionHandler" CommandName="DETAILS"></asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hdfOrderPk" Value='<%# Eval("PNS_SO_DTL") %>' />
                                    <asp:HiddenField runat="server" ID="hdfSohPk" Value='<%# Eval("SOH_PK") %>' />
                                    <asp:HiddenField runat="server" ID="hdfOrderDtl" Value='<%# Eval("PNS_PK") %>' />
                                    <asp:HiddenField runat="server" ID="hdfPlanGroupDtl" Value='<%# Eval("PNS_PLAN_TRX_DTL") %>' />
                                    <asp:HiddenField runat="server" ID="hdfOrderSlNo" Value='<%# Eval("PND_SL_NO") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="11%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Requiredby %>" HeaderStyle-CssClass="grd-head-center selected">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderReqDt" runat="server" Text='<%# Convert.ToDateTime(Eval("PNS_REQUIRED_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'
                                        ToolTip='<%# Convert.ToDateTime(Eval("PNS_REQUIRED_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:OrderQty %>" HeaderStyle-CssClass="grd-head-rgt selected">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderQty" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY"))) %>'
                                        ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY"))) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:DispatchedQty %>" HeaderStyle-CssClass="grd-head-rgt selected">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderDispatchedQty" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_DISPATCHED"))) %>'
                                        ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_DISPATCHED"))) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:AllocationQty %>" HeaderStyle-CssClass="grd-head-rgt selected">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderAllocatedQty" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_ALLOCATED"))) %>'
                                        ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_ALLOCATED"))) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:ProducedQty %>" HeaderStyle-CssClass="grd-head-rgt selected">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderProducedQty" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_PRODUCED"))) %>'
                                        ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_PRODUCED"))) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                <FooterTemplate>
                                    <div style="text-align: left;">
                                        <asp:Label runat="server" ID="lblSCTotal" Text="Total"></asp:Label>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:PlannedQty %>" HeaderStyle-CssClass="grd-head-rgt selected">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderPlannedQty" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_PLANNED"))) %>'
                                        ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_PLANNED"))) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                <FooterTemplate>
                                    <div style="text-align: right;">
                                        <asp:Label runat="server" ID="lblSCPlannedTotal" Text=""></asp:Label>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:BalancetoPlan %>" HeaderStyle-CssClass="grd-head-rgt selected">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderBalQty" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_BAL_TO_PLAN"))) %>'
                                        ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_BAL_TO_PLAN"))) %>'></asp:Label>
                                    <asp:HiddenField runat="server" ID="hdfSCProportionVal" Value='<%# Eval("SOD_PERCENTAGE") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                <FooterTemplate>
                                    <div style="text-align: right;">
                                        <asp:Label runat="server" ID="lblSCBalTotal" Text=""></asp:Label>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:CurrentPlan %>" HeaderStyle-CssClass="grd-head-rgt selected">
                                <ItemTemplate>
                                    <%--<asp:TextBox runat="server" ID="txtDtlCurrPlan" Text="" CssClass="input-w50per input-disabled"
                                                                            Enabled="false"></asp:TextBox>--%>
                                    <uc2:NumericControl ID="txtDtlCurrPlan" runat="server" ControlType="NumericInteger"
                                        CssClass="numeric medium" Text='<%# Eval("PNS_PLAN_QTY") %>' TabIndex="128" onblur="CalculateSCPopUpTotal();" />
                                </ItemTemplate>
                                <ItemStyle Width="14%" CssClass="txtAlign-right margn-rgt0" />
                                <FooterTemplate>
                                    <div style="text-align: right;">
                                        <asp:Label runat="server" ID="lblSCPlanNowTotal" Text=""></asp:Label>
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="selected">
                                <ItemTemplate>
                                    <asp:ImageButton OnClientClick='return ShowDeleteConfirm(this,"Do you want to delete this item?");'
                                        runat="server" ID="imbSODelete" SkinID="imbdeletegrid" OnClick="ActionHandler"
                                        CommandName="DELETE_ACTION" TabIndex="28" ToolTip="<%$ Resources:Controls,Delete %>"
                                        CommandArgument='<%# Eval("PNS_PK") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="5%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="divVersions" style="display: none;">
                <div class="content-wrapper">
                    <div class="grdTable" style="overflow-x: hidden; overflow-y: auto; max-height: 300px;">
                        <asp:GridView ID="grdVersions" runat="server" AutoGenerateColumns="False" Width="100%"
                            AllowPaging="false" CssClass="gridwraptable gridwrap" HeaderStyle-HorizontalAlign="Center"
                            AllowSorting="True" EmptyDataRowStyle-CssClass="emptytable" EmptyDataRowStyle-HorizontalAlign="Center"
                            ShowFooter="true">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmptyVersions" runat="server" Text="<%$ Resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:DateFrom %>" HeaderStyle-HorizontalAlign="Left">
                                    <ItemStyle HorizontalAlign="Left" Width="40%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblFrom" Text='<%# Convert.ToDateTime(Eval("PNH_FROM_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'
                                            runat="server">
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:DateTo %>" HeaderStyle-HorizontalAlign="Left">
                                    <ItemStyle HorizontalAlign="Left" Width="30%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTo" Text='<%#Convert.ToDateTime(Eval("PNH_TO_DT")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString())%>'
                                            runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Version %>" HeaderStyle-CssClass="grd-head-center">
                                    <ItemStyle HorizontalAlign="Center" Width="30%" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkVersionName" Text='<%#Eval("PNH_VERSION")%>' runat="server"
                                            ToolTip='<%#Eval("PNH_VERSION")%>' CssClass="text-underline" OnClick="ActionHandler"
                                            CommandName="PRINTRECORD">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <%--           <uc5:linewisepdtn id="ucrLinewisePdtn" runat="server" visible="true" />--%>
            <asp:HiddenField ID="hdnTabListNew" runat="server" Value="1" />
            <asp:HiddenField ID="hdnSummaryTab" runat="server" Value="1" />
            <asp:HiddenField ID="hdfIscontYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsSCcontYes" runat="server" Value="0" />
            <asp:HiddenField ID="hdfSCPopUpTotal" runat="server" Value="0" />
            <asp:HiddenField ID="hdfProdHrs" runat="server" Value="0" />
            <asp:HiddenField ID="hdfRepairHrs" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="diverror" style="display: none">
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
            ID="vvsPage" ValidationGroup="Save" runat="server" />
        <asp:ValidationSummary ID="vvsUcrSelectPlan" ValidationGroup="SelectPlan" runat="server" />
        <asp:ValidationSummary ID="vvsLine" ValidationGroup="AddLine" runat="server" />
    </div>
</asp:Content>
