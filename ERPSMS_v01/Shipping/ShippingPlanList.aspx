<%@ Page Title="<%$ Resources:Captions,Title_ShippingPlan %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="ShippingPlanList.aspx.cs"
    Inherits="ERPSMS_v01.Shipping.ShippingPlanList" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/ShippingPrintDocs.ascx" TagName="PrinterControl"
    TagPrefix="pc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtETD", "hdfETD", "txtETA", "hdfETA", false, false);
            GrandScriptUtils.DatePickerCommon("txtGeneratedOn");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url + "&IsSBUCustomer=" + $("[id$='hdfIsSBUCustomer']").val(), "hdfCustomerID", true, true, "CUSTOMERLIST");
            if ($("[id$=txtCustomer]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomerID]"));
            }
            $("[id*=txtDespNow]").ForceNumericOnly();
            $("[id$=btnSetTotal]").hide();
            ShowHideExpand();
        }
        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>
            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }
        function AfterGridExpand(row) {
            if ($("[id$=grdShippingPlanList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedOrders]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
                }
            }
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
        function ShowListing(flag) {
            $("[id$=PageAction_List]").show();
            $("[id$=PageAction_Entry]").hide();
            $("[id$=lnkList]").hide();
            $("[id$=lnkDetail]").hide();
            return false;
        }
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                //CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
        }
        function setTotal(sender) {
            $("[id$=btnSetTotal]").click();
        }
        function ShowError() {
            var msg = '<%=Resources.Messages.ReportError %>';
            var information = '<%=Resources.Messages.Information %>';
            GrandScriptUtils.ShowModal(msg, information);
        }
        function DisableAuto(extender, hfield) {
            ///<summary>
            /// Used to disable Autocomplete
            ///</summary>
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlPOInvoice">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="51" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('delivery')"
                                            ValidationGroup="delivery" ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="50" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('delivery')"
                                            ValidationGroup="delivery" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="52" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="54" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="55" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container padgrgt0" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnShippingPlan" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lnkShippingPlan" Text="<%$resources:PageNameRes,ShippingPlan %>"
                                CommandName="SHIPPINGPLAN" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerEval" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerEval" Text="<%$resources:PageNameRes,ContainerEvaluation %>"
                                CommandName="CONTAINEREVALUATION" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerInspection" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerInspection" Text="<%$resources:PageNameRes,ContainerInspection %>"
                                CommandName="CONTAINERINSPECTION" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnEnquiry" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkEnquiry" Text="<%$resources:PageNameRes,UploadQADocs %>"
                                CommandName="UPLOADQA" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnQuotation" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkQuotation" Text="<%$resources:PageNameRes,UploadExportDocs %>"
                                CommandName="UPLOADEXPORT" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnLoadingPlan" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkLoadingPlan" Text="<%$resources:PageNameRes,LoadingPlan %>"
                                CommandName="LOADINGPLAN" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnUploadPhotographs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkUploadPhotographs" Text="<%$resources:PageNameRes,UploadPhotographs %>"
                                TabIndex="11" CommandName="UPLOADPHOTOGRAPHS" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="Span1" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkDeliveryOrder" Text="<%$resources:PageNameRes,DeliveryOrder %>"
                                TabIndex="12" CommandName="GOODOUTWARD" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnContainerRelease" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkContainerRelease" Text="<%$resources:PageNameRes,ContainerRelease %>"
                                TabIndex="13" CommandName="CONTAINERRELEASE" CommandArgument="SEC_ActionPanel"
                                OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnBillofLoading" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkBillofLoading" Text="<%$resources:PageNameRes,BL %>"
                                TabIndex="14" CommandName="BL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnPrintShippingDocs" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lnkPrintShippingDocs" Text="<%$resources:PageNameRes,PrintShippingDocs %>"
                                TabIndex="15" CommandName="PRINT" CommandArgument="SEC_ActionPanel" OnClick="ActionHandler"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                TabIndex="16" OnClick="ActionHandler" CommandName="SHIPPINGPLANLIST" CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                TabIndex="17" OnClick="ActionHandler" CommandName="SHIPPINGPLANDETAIL" CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="background: #f2f2f2;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFrmDate" runat="server" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="1" CssClass="input-small-18-11"
                                                MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" Value="" />
                                            <asp:Label ID="lblAdjustdummy" runat="server" CssClass="middle-lbl-xsmall-f style-none"></asp:Label>
                                            <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl-small-b-18-11"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="2" CssClass="input-small-18-11" MaxLength="11"
                                                onpaste="return false;" onkeydown="return CheckKey(event)"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" Value="" />
                                            <div class="clear">
                                            </div>
                                            <div>
                                                <asp:Label ID="lblCartnAllocStatus" CssClass="lblCartnAllocStatus-label-18-11-2020" runat="server" Text="<%$resources:CartnAllocStatus %>"
                                                    AssociatedControlID="ddlCartnAllocStatus"></asp:Label>
                                                <asp:DropDownList ID="ddlCartnAllocStatus" runat="server" TabIndex="4" CssClass="input-w41-5per-18-11">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblStat" runat="server" Text="<%$ resources:Status %>" AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="3" CssClass="select-half-b">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblCustomerSearch" runat="server" Text="<%$ resources:Customer %>"
                                                CssClass="margnbotm0" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="select-small-g margnbotm0"
                                                MaxLength="100" TabIndex="4"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                            <asp:Label ID="lblSCno" runat="server" Text="<%$resources:SONo %>" CssClass="middle-lbl-xsmall-e margnbotm0"
                                                AssociatedControlID="txtSCno"></asp:Label>
                                            <asp:TextBox ID="txtSCno" runat="server" CssClass="middle-lbl-xsmall-a2 margnbotm0"
                                                MaxLength="100" TabIndex="5"> </asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblShipPlanNo" runat="server" Text="<%$resources:ShippingPlanNo %>"
                                                CssClass="middle-lbl-d margnbotm0" AssociatedControlID="txtPlanNo"></asp:Label>
                                            <asp:TextBox ID="txtPlanNo" runat="server" CssClass="input-small-a margnbotm0" MaxLength="100"
                                                TabIndex="6"></asp:TextBox>
                                            <asp:Label ID="lblDoNumber" runat="server" Text="<%$ resources:DONo %>" AssociatedControlID="txtDespatchNumber"
                                                CssClass="middle-lbl-xsmall-a1-18-11 margnbotm0"></asp:Label>
                                            <asp:TextBox ID="txtDespatchNumber" runat="server" CssClass="select-small-b margnbotm0"
                                                MaxLength="70" TabIndex="7"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfDPHPK" runat="server" Value="" />
                                            <asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$ resources:Controls,Search %>" ValidationGroup="Search" OnClick="ActionHandler"
                                                TabIndex="8" CommandName="SEARCH" CssClass="margntop2 margnbotm0" SkinID="search-ext" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                TabIndex="9" ToolTip="<%$ resources:Controls,Clear %>" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap" style="max-height: 400px; padding: 7px;">
                                <cc1:ExtGridView runat="server" ID="grdShippingPlanList" AutoGenerateColumns="False"
                                    TabIndex="24" Width="100%" ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    ShowFooter="true" OnRowDataBound="ActionHandler" PageSize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" runat="server" TabIndex="10" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping2(this);" OnCheckedChanged="ActionHandler"
                                                    AutoPostBack="true" />
                                                <asp:Button runat="server" ID="btnOrderDetails" OnClick="ActionHandler" CommandName="SODETAILS"
                                                    CommandArgument='<%# Eval(Resources.DataFieldRes.SPPk) %>' EnableTheming="false"
                                                    Style="display: none" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfShippingPlanID" Value='<%# Eval(Resources.DataFieldRes.SPPk) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ETD %>" SortExpression="SNH_ETD">
                                            <ItemTemplate>
                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPETD, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPETD, Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="SNH_CUSTOMER">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SPCustomerText),38)%>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SaleOrder),300)%>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfCustomerPK" Value='<%# Eval(Resources.DataFieldRes.SPCustomer) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="23%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PONumber %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("POH_NO"), 15)%>'
                                                 ToolTip='<%#Eval("POH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Shiptoport %>" SortExpression="SNH_SHIP_TO_PORT">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipBy" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SPShipToPort),14)%>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.SPShipToPort),300)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="SNH_UOM_TEXT"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUomText" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SNH_UOM_TEXT"),13)%>'
                                                    ToolTip='<%# Eval("SNH_UOM_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PlanedQty %>" SortExpression="SNH_PLAN_QTY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPlandQuantity" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPPlandQty, "{0:n}")%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPPlandQty, "{0:n}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Cartons %>" SortExpression="SNH_CTN_QTY">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCarrier" runat="server" Text='<%# AddCommas(Eval(Resources.DataFieldRes.SPCartonsQty))%>'
                                                    ToolTip='<%# AddCommas(Eval(Resources.DataFieldRes.SPCartonsQty))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PlanNo %>" SortExpression="SNH_NO">
                                            <ItemTemplate>
                                                <asp:Label ID="lblContainer" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPNO)%>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPNO)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:LoadingDate %>" SortExpression="SNH_LOADING_DATE"
                                            Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTransportCo" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPLoadingDate, Resources.Constants.DateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPLoadingDate, Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Donumber %>" SortExpression="SNH_DESPATCH_NO">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDoNumber" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.SPDespatchNo) %>'
                                                    OnClick="ActionHandler" CommandName="GRIDSHOWMY" CommandArgument='<%# Eval(Resources.DataFieldRes.DeliveryOrderPK) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.SPDespatchNo) %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <%-- -----------------Newly added columns   ----------------------------------------------%>
                                        <asp:TemplateField HeaderText="<%$ resources:DOQtyInCartons %>" ItemStyle-HorizontalAlign="Right"
                                            Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyCartonsDO" runat="server" Text='<%# Eval("CDR_DO_CARTON") %>'
                                                    ToolTip='<%# Eval("CDR_DO_CARTON") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblQtyCartonDOFooter"></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfQtyCartonDOFooter" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:QtyInCartons %>" ItemStyle-HorizontalAlign="Right"
                                            Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblQtyCartons" runat="server" Text='<%# Eval("CDR_CARTON_DESPATCHED") %>'
                                                    ToolTip='<%# Eval("CDR_CARTON_DESPATCHED") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                            <FooterStyle HorizontalAlign="Right" />
                                            <FooterTemplate>
                                                <asp:Label runat="server" ID="lblTotalPayNowFooterSplitSales"></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfTotalPayNowFooterSplitSales" />
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        
                                        <%--SC No--%>
                                      <%--  <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="SOH_NO" Visible="<%$ resources:ConfigurationsRes,ShowSCNoInShppingPlan %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSCNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SOH_NO"),12)%>'
                                                    ToolTip='<%# Eval("SOH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                        <%--BOIStatus--%>
                                      <%--  <asp:TemplateField HeaderText="<%$ resources:BOIStatus %>" Visible="<%$ resources:ConfigurationsRes,ShowBOIStatusInShppingPlan %>">
                                            <ItemTemplate>
                                                  <asp:Label ID="lblBOIStatus" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("SOH_NO"),12)%>'
                                                    ToolTip='<%# Eval("SOH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />    
                                        </asp:TemplateField>--%>
                                        <%-----------  End newly added fields-------------------------------------------------------%>
                                        <asp:TemplateField SortExpression="SNH_STATUS">
                                            <ItemTemplate>
                                                <asp:Button ID="imgApproved" runat="server" OnClientClick="javascript:return false;"
                                                    CssClass='<%# Eval("ASC_CSS_CLASS") %>' ToolTip='<%# Eval(Resources.DataFieldRes.SPStatusText) %>' />
                                                <asp:HiddenField runat="server" ID="hdfApproved" Value='<%# Eval(Resources.DataFieldRes.SPStatus) %>' />
                                                <asp:HiddenField runat="server" ID="hdfTrxStatus" Value='<%# Eval(Resources.DataFieldRes.SPTrxStatus) %>' />
                                                <asp:HiddenField ID="hdfDept" runat="server" Value='<%# Eval(Resources.DataFieldRes.SPDept) %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="left" Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <cc1:ExtGridView runat="server" ID="grdOrderDetails" AutoGenerateColumns="False"
                                                        ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                        GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                                        AllowPaging="false" OnRowDataBound="ActionHandler" Width="98%">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="Label3" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:SONo %>">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkSoNo" CssClass="text-underline" runat="server" Text='<%# Eval(Resources.DataFieldRes.SONumber) %>'
                                                                        OnClick="ActionHandler" CommandName="SHOWPOPUP" CommandArgument='<%# Eval(Resources.DataFieldRes.SCPK) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SONumber) %>'></asp:LinkButton>
                                                                    <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPlantCode" CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"
                                                                        Text="<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>" ToolTip="<%# Eval(Resources.DataFieldRes.CMP_DISPLAY_CODE) %>"
                                                                        runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:SODate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDetailsSoDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SaleOrderDate, Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SaleOrderDate, Resources.Constants.DateFormatGrid) %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:ShipDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDetailsShipDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.SohShipmentDate, Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.SohShipmentDate, Resources.Constants.DateFormatGrid) %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:ProductCode %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDeatilsProductCode" runat="server" Text='<%# Eval(Resources.DataFieldRes.ItemCode) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.ItemName) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:BrandName %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDeatilsBrandName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.BrandName),65) %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.BrandName) %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="45%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:PlanQtyPcs %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblplndQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.PlanQty, "{0:n}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.PlanQty, "{0:n}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="14%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="2%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Cartons %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDeatilscartons" runat="server" Text='<%# Eval(Resources.DataFieldRes.CartonsQty, "{0:n}") %>'
                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.CartonsQty, "{0:n}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="amount-numeric" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label2" Text="" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10%" />
                                                            </asp:TemplateField>--%>
                                                        </Columns>
                                                        <RowStyle CssClass="table-secondlevel" />
                                                        <HeaderStyle CssClass="table-secondlevela" />
                                                    </cc1:ExtGridView>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="nopadding" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="table-firstlevel" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                    <FooterStyle CssClass="table-firstlevela-total" />
                                </cc1:ExtGridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <div class="fields-grpwrap color-grey pad-t10 grp-after">
                                <div class="clear">
                                </div>
                                <div class="fields-group">
                                    <table class="table-devide">
                                        <tr>
                                            <td>
                                                <div class="div2col-S">
                                                    <asp:Label runat="server" ID="lblPlanNo" Text="<%$ resources:PlanNo %>" AssociatedControlID="lblShippingPlanNo"></asp:Label>
                                                    <asp:Label runat="server" ID="lblShippingPlanNo" Text="" TabIndex="25"></asp:Label>
                                                    <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                                    <asp:HiddenField ID="AST_CODE" runat="server" />
                                                    <asp:HiddenField ID="hdfShippingPlanNo" runat="server" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblShiptoPort" Text="<%$ resources:Shiptoport %>" AssociatedControlID="txtShipPort"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtShipPort" Text="" TabIndex="27"></asp:TextBox>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblContainerType" Text="<%$ resources:ContainerType %>"
                                                        AssociatedControlID="ddlContainerType"></asp:Label>
                                                    <asp:DropDownList ID="ddlContainerType" runat="server" TabIndex="29" CssClass="medium-a">
                                                    </asp:DropDownList>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="lblDelivery" Text="<%$ resources:Customer %>" AssociatedControlID="lblDeliveryTo"></asp:Label>
                                                    <asp:Label runat="server" ID="lblDeliveryTo" Text="" TabIndex="30"></asp:Label>
                                                    <asp:HiddenField ID="hdnCustomerID" runat="server" Value="0" />
                                                    <div class="clear">
                                                    </div>
                                                </div>
                                            </td>
                                            <td>
                                                <div class="div2col-S">
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label2" Text="<%$ resources:GeneratedOn %>" AssociatedControlID="txtGeneratedOn"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtGeneratedOn" Text="" CssClass="Uidate-picker"
                                                        onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;" TabIndex="26"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="delivery"
                                                            EnableClientScript="true" runat="server" ControlToValidate="txtGeneratedOn" Display="Dynamic"
                                                            Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreDate" CssClass="star" ValidationGroup="delivery"
                                                            runat="server" ControlToValidate="txtGeneratedOn" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_Date_Valid %>"
                                                            ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                            EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label3" Text="<%$ resources:ETD %>" AssociatedControlID="txtETD"></asp:Label>
                                                    <asp:TextBox runat="server" ID="txtETD" Text="" CssClass="Uidate-picker" onkeydown="return CheckKey(event)"
                                                        MaxLength="11" onpaste="return false;" TabIndex="28"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfETD" runat="server" Value="" />
                                                    <div class="clear">
                                                    </div>
                                                    <asp:Label runat="server" ID="Label1" Text="<%$ resources:Status %>" AssociatedControlID="lblSPStatus"></asp:Label>
                                                    <asp:Label runat="server" ID="lblSPStatus" Text="" TabIndex="31"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="gridwrap">
                                <asp:GridView ID="grdShippingList" runat="server" AutoGenerateColumns="False" Width="100%"
                                    PageSize="25" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false"
                                    ShowFooter="true" OnRowDataBound="ActionHandler" TabIndex="17">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:SONo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSONo" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfSONumber" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfSaleOrderHdrPK" Value='' runat="server" />
                                                <asp:HiddenField ID="hdfSaleOrderDtlPK" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:SODate %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSODate" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ProductCode %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIGPLCode" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfIGPLCode" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BrandName %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrandCode" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfBrandCode" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="24%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUOM" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdfUOM" Value='' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:OrderQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderQty" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PlanedQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPackedQty" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:DespQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDespatchedQty" runat="server" Text='' ToolTip=''></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PlanNow %>" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtPlanNow" runat="server" CssClass="small-a numeric" MaxLength="10"
                                                    TabIndex="31" onchange="setTotal(this);"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:TotalCTNs %>"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle Width="10%" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CTNQty %>" SortExpression="" ItemStyle-HorizontalAlign="Right"
                                            FooterStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCTNQty" runat="server" Text='' ToolTip=''></asp:Label>
                                                <asp:HiddenField ID="hdnTotalPcs" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalCTN" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Button ID="btnSetTotal" runat="server" CommandName="SHOW" OnClick="ActionHandler" />
                            </div>
                            <%--Print popup window --%>
                            <pc1:PrinterControl ID="PrinterControl1" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="delivery" runat="server" />
                </div>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="delivery">
                </uc1:WorkflowUserComments>
            </div>
            <asp:HiddenField ID="hdfType" runat="server" Value="" />
            <asp:HiddenField ID="hdfCurPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDOType" runat="server" Value="" />
            <asp:HiddenField ID="hdfDOPK" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
            <asp:HiddenField ID="hdfIsSBUCustomer" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
