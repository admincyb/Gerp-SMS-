<%@ Page Title="<%$ Resources:Captions,Title_WorkOrder %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="Project.aspx.cs" Inherits="CustomerPortal.Projects.Project" Theme="BlueExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register TagPrefix="Gti" Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" %>
<%@ Register Src="~/WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        label#ctl00_MainContent_lblProjectBudget + span {
            background: none;
            border: none;
            margin-top: -3px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var Url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        function InitPage() {
        }
        function AfterAutoCompleteSelect(targetControlID) {
        }
        function InitComponents() {
            $("[id*=txtProjectBudjet]").ForceNumericOnly();
            $("[id*=txtDefectLiability]").ForceNumericOnly();
            $("[id*=txtRetention]").ForceNumericOnly();
            $("[id*=txtLimit]").ForceNumericOnly();
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.DatePickerCommon("txtReffDate");
            GrandScriptUtils.DatePickerCommon("txtAmendmentDate");
            GrandScriptUtils.AddDateRangeCommon("txtEstimatedStart", "hdfEstimatedStartDate", "txtEstimatedEnd", "hdfEstimatedEndDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtActualStart", "hdfActualStartDate", "txtActualEnd", "hdfActualEndDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", uiUrl, "hdfCustomer", true, true, "CUSTOMER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtConsultant", uiUrl, "hdfConsultant", true, true, "CONSULTANT");
            GrandScriptUtils.AddDateRangeCommon("txtFromDateSearch", "hdfFromDateSearch", "txtToDateSearch", "hdfToDateSearch", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomerSearch", uiUrl, "hdfCustomerSearch", true, true, "USEDCUSTOMER");

            checkLumpSumPrj($("[id$='chkLumpSumPrj']"));
        }
        function IsValidQty(sender, args) {
            var exchangeRate = parseFloat($('#[id$=txtExchangeRate]').val());
            if (exchangeRate <= 0)
                args.IsValid = false;
            else
                args.IsValid = true;
        }
        function ViewMode(t) {
        }
        function ShowHideAdvancedSearch(flag) {
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
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Details]").hide();
                $("[id$=pnlListing]").show();
                $("[id$=pnlEntry]").hide();
                $("[id$=spnListing]").removeClass("tab-inactive").addClass("tab-active");
                $("[id$=spnDetail]").removeClass("tab-active").addClass("tab-inactive");
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Details]").show();
                $("[id$=pnlListing]").hide();
                $("[id$=pnlEntry]").show();
                $("[id$=spnDetail]").removeClass("tab-inactive").addClass("tab-active");
                $("[id$=spnListing]").removeClass("tab-active").addClass("tab-inactive");
            }
            return false;
        }
        function PageViewMode(mode,userstatus) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                $("[id$=pnlPrint]").hide();
                EnableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));

            }
            else if (mode == 3) {//edit mode
            if (userstatus == "0"||userstatus==6)//||$("[id$=hdfUserStatus]").val() != "6"
            {
                EnableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }
            else
            {
             DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }

        }
        }
        function DefectLiability_ClientValidate(source, args) {
            if ($('#[id$=txtDefectLiability]').val() == "" && $("#[id$=ddlDefectLiabilityUOM]").val() != "-1") {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }
        function DefectLiabilityUOM_ClientValidate(source, args) {
            if ($('#[id$=txtDefectLiability]').val() != "" && $("#[id$=ddlDefectLiabilityUOM]").val() == "-1") {
                args.IsValid = false;
            }
            else {
                args.IsValid = true;
            }
        }
        function Percentage_ClientValidate(source, args) {
            if ($('#[id$=txtRetention]').val() == "") {
                args.IsValid = true;
            }
            else {
                var percentage = 0;
                percentage = !isNaN(parseInt($('#[id$=txtRetention]').val())) ? $('#[id$=txtRetention]').val() : 0;
                if (percentage > 100)
                    args.IsValid = false;
                else
                    args.IsValid = true;
            }
        }
        function ValidatePageNow(valGroup) {

            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                // CheckValidationDuplicate(valGroup);
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

        function checkLumpSumPrj(ctrl) {
            if ($("[id$='chkLumpSumPrj']").is(":checked")) {
                $("[id$='txtLumpSumPrjValue']").removeAttr("disabled");
                $("[id$='txtLumpSumPrjValue']").focus();
                $("[id$='txtLumpSumPrjValue']").removeClass('disbldfield');
            }
            else {
                $("[id$='txtLumpSumPrjValue']").attr("disabled", "disabled");
                $("[id$='txtLumpSumPrjValue']").val("0.0000");
                $("[id$='txtLumpSumPrjValue']").addClass('disbldfield');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel runat="server" ID="aupdpnlWOItem">
        <ContentTemplate>
            <asp:HiddenField ID="hdfFromDateSearch" runat="server" />
            <asp:HiddenField ID="hdfToDateSearch" runat="server" />
            <asp:HiddenField ID="hdfCustomerSearch" runat="server" />
            <asp:HiddenField ID="hdfEstimatedStartDate" runat="server" />
            <asp:HiddenField ID="hdfEstimatedEndDate" runat="server" />
            <asp:HiddenField ID="hdfActualStartDate" runat="server" />
            <asp:HiddenField ID="hdfActualEndDate" runat="server" />
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <%-- <div class="clear">--%>
                                <%--</div>--%>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <%-- <li runat="server" id="Li1">
                                        <asp:Button runat="server" ID="btnSaveAndContinue" CommandName="SAVECONTINUE" TabIndex="3"
                                            Text="<%$resources:Controls,SaveAndContinue %>" OnClick="ActionHandler" ValidationGroup="Save"
                                            ToolTip="<%$resources:Controls,SaveAndContinue %>" CommandArgument="SEC_ActionPanel"
                                            OnClientClick="javascript:ValidatePageNow('Save')" SkinID="btnInner-ok" />
                                    </li>--%>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="53"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')"
                                            ValidationGroup="Save" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="54" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Save')" ValidationGroup="Save"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>

                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="55" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" ValidationGroup="Save" ToolTip="<%$resources:Controls,Save %>"
                                            CommandArgument="SEC_ActionPanel" OnClientClick="javascript:ValidatePageNow('Save')"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" TabIndex="56" ID="btnDelete" CommandName="DELETE" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Delete %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlPrint" visible="false">
                                        <asp:Button runat="server" ID="btnPrint" CommandName="PRINT" TabIndex="57" Text="<%$resources:Controls,Print %>"
                                            ToolTip="<%$resources:Controls,Print %>" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Print" />
                                    </li>
                                    <li runat="server" id="pnlExcelDetails" visible="false">
                                        <asp:Button runat="server" TabIndex="58" ID="btnEXCELDETAILS" CommandName="EXCELDETAILS"
                                            OnClick="ActionHandler" Text="<%$resources:Controls,Export %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Excel" ToolTip="<%$resources:Controls,Export %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="WORKORDERLIST" TabIndex="59" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <ul id="Ul1" runat="server">
                                        <li>
                                            <asp:Button runat="server" TabIndex="1" ID="btnShortClose" CommandName="SHORTCLOSE" OnClick="ActionHandler" Visible="false"
                                                Text="<%$resources:Controls,ShortClose %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                                ToolTip="<%$resources:Controls,ShortClose %>" OnClientClick="return ShowDeleteConfirm(this,'Are you sure want to close this?');" />
                                        </li>
                                        <li>
                                            <asp:Button runat="server" TabIndex="2" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                                Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                                ToolTip="<%$resources:Controls,New %>" />
                                        </li>
                                    </ul>
                                    <li>
                                        <asp:Button runat="server" TabIndex="3" ID="btnEdit" CommandName="WORKORDER" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                    <li id="pnlAmend">
                                        <asp:Button runat="server" TabIndex="4" ID="btnAmend" CommandName="AMEND" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Amend %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-amend"
                                            ToolTip="<%$resources:Controls,Amend %>" />
                                    </li>
                                    <li id="pnlView">
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="5" Text="<%$resources:Controls,View %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnPrintList" CommandName="PRINTLIST" TabIndex="6"
                                            Text="<%$resources:Controls,Print %>" ToolTip="<%$resources:Controls,Print %>"
                                            OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print" Visible="false" />
                                    </li>
                                    <li runat="server" id="pnlEXCELPRINT" visible="false">
                                        <asp:Button runat="server" TabIndex="7" ID="btnExport" CommandName="EXCELPRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Export  %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Excel"
                                            ToolTip="<%$resources:Controls,Export %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <%--Tabs--%>
                <div id="divTabContainer" class="tab-container" runat="server" style="margin-top: 2%; margin-bottom: 1%">

                    <ul id="tab-menu">
                        <li><span id="spnListing" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnList" Text="<%$resources:List %>" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-active" OnClick="ActionHandler" CommandName="WORKORDERLIST" TabIndex="59"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnDetail" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnDetail" Text="<%$resources:WorkOrder %>"
                                CommandArgument="SEC_ActionPanel" CommandName="WORKORDER" OnClick="ActionHandler"
                                CssClass="tab-active" TabIndex="60"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">

                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>

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
                                                TabIndex="8" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="margin-top: 2%">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomerSearch" runat="server" Text="<%$resources:Customer %>" AssociatedControlID="txtCustomerSearch"
                                                CssClass=""></asp:Label>
                                            <asp:TextBox ID="txtCustomerSearch" runat="server" TabIndex="9" onDrop="return false;" CssClass="input-half"
                                                onPaste="return false;" MaxLength="200" onkeyup="limitText(this,200);" onkeydown="limitText(this,200);"></asp:TextBox>

                                        </div>

                                    </td>

                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTypeSearch" runat="server" Text="<%$resources:Type %>" AssociatedControlID="ddlTypeSearch"
                                                CssClass="input-half-05-01-2021" Visible="false"></asp:Label>
                                            <asp:DropDownList ID="ddlTypeSearch" runat="server" TabIndex="10" ValidationGroup="Save" CssClass="input-half" Visible="false">
                                            </asp:DropDownList>
                                            <asp:Label ID="lblFilterProjectCode" runat="server" Text="<%$resources:FilterProjectCode %>"
                                                AssociatedControlID="txtFilterProjectCode" CssClass="middle-lbl-d"></asp:Label>
                                            <asp:TextBox ID="txtFilterProjectCode" runat="server" TabIndex="11" MaxLength="200"
                                                onkeyup="limitText(this,200);" onkeydown="limitText(this,200);" CssClass="input-half-59-5"></asp:TextBox>

                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">

                                            <asp:Label ID="lblFromDateSearch" runat="server" CssClass="" Text="<%$resources:From %>"
                                                AssociatedControlID="txtFromDateSearch"></asp:Label>
                                            <asp:TextBox ID="txtFromDateSearch" runat="server" TabIndex="12" CssClass=""
                                                onkeydown="return CheckKey(event)" onDrop="return false;" onPaste="return false;"></asp:TextBox>
                                            <asp:Label ID="lblToDateSearch" runat="server" Text="<%$resources:To %>" AssociatedControlID="txtToDateSearch"
                                                CssClass="min-width-21-07-19"></asp:Label>
                                            <asp:TextBox ID="txtToDateSearch" runat="server" TabIndex="13" CssClass=""
                                                onkeydown="return CheckKey(event)" onDrop="return false;" onPaste="return false;"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblFilterProjectNo" runat="server" Text="<%$resources:FilterProjectNo %>"
                                                AssociatedControlID="txtFilterProjectNo" CssClass="managelbl"></asp:Label>
                                            <asp:TextBox ID="txtFilterProjectNo" runat="server" TabIndex="14" MaxLength="200"
                                                onkeyup="limitText(this,200);" onkeydown="limitText(this,200);" CssClass="input-half-59"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label ID="lblTransactionStatus" runat="server" Text="<%$resources:TransactionStatus %>" AssociatedControlID="ddlTranStatus"
                                                CssClass="input-half-05-01-2021"></asp:Label>
                                            <asp:DropDownList ID="ddlTranStatus" runat="server" ValidationGroup="Save" CssClass="minw-20per" TabIndex="15" Width="27%">
                                            </asp:DropDownList>
                                            <asp:Label ID="Label1" runat="server" Text="<%$Resources:Controls,Status%>" CssClass="lbl-10-7perc-04-01-2021"
                                                AssociatedControlID="ddlStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="lbl-10-7perc-04-01-2021" TabIndex="16" Width="27%">
                                                <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Active %>" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Captions,Inactive %>" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 marginleft-01-01-21">

                                            <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" ToolTip="<%$resources:Controls,Clear %>"
                                                TabIndex="17" OnClick="ActionHandler" CommandName="CLEAR" SkinID="btnInner-cancel-dsd"
                                                Style="margin-right: 0px!important;" />
                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="18"
                                                CommandName="SEARCH" SkinID="btnInner-search" Style="margin-right: 2px!important;" />
                                        </div>
                                    </td>
                                </tr>

                            </table>
                            <%--=============End Advance Search Region=====================--%>


                            <div class="gridwrap hierarchical-wrap maxh-290">
                                <%-- OnSorting="ActionHandler" OnPageIndexChanging="ActionHandler"--%>
                                <asp:GridView ID="grdWorkOrderList" runat="server" PageSize="<%$resources:PageSize %>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler"
                                    OnRowCommand="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton CssClass="rdoSelection" TabIndex="19" runat="server" GroupName="SelectOne"
                                                    ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" AutoPostBack="true"
                                                    OnCheckedChanged="ActionHandler" />
                                                <asp:HiddenField runat="server" ID="hdfWorkOrderPk" Value='<%# Eval("WOH_PK") %>' />
                                                <asp:HiddenField ID="hdfLastModDate" runat="server" Value='<%# Eval("LAST_MOD_DT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfUserStatus" Value='<%# Eval("WOH_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfWoNo" Value='<%# Eval("WOH_NO") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date %>" SortExpression="" HeaderStyle-CssClass="col-md-1 col-sm-1">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%# (Eval("WOH_DATE"))==null?string.Empty:Convert.ToDateTime(Eval("WOH_DATE")).ToString(Resources.Constants.DateFormatShort) %>'
                                                    ToolTip='<%# (Eval("WOH_DATE"))==null?string.Empty:Convert.ToDateTime(Eval("WOH_DATE")).ToString(Resources.Constants.DateFormatShort) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:WoNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWoNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("WOH_NO"),15) %>' ToolTip='<%# Eval("WOH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Customer %>" SortExpression="" HeaderStyle-CssClass="col-md-3 col-sm-3">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("WOH_CUSTOMER_TEXT"), 30)   %>'
                                                    ToolTip='<%# Eval("WOH_CUSTOMER_TEXT") !=null ? HttpUtility.HtmlDecode(Eval("WOH_CUSTOMER_TEXT").ToString()) :string.Empty%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:WorkProject %>" SortExpression="" HeaderStyle-CssClass="col-md-3 col-sm-3">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkProject" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("WOH_WORK"), 40) %>'
                                                    ToolTip='<%# Eval("WOH_WORK")!=null?HttpUtility.HtmlDecode(Eval("WOH_WORK").ToString()):string.Empty%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="27%" />
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:WorkCode %>" SortExpression="" HeaderStyle-CssClass="col-md-1 col-sm-1">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWorkProjectCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("WOH_SITE"), 10) %>'
                                                    ToolTip='<%# Eval("WOH_SITE")!=null?HttpUtility.HtmlDecode(Eval("WOH_SITE").ToString()):string.Empty%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" Width="7%" />
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="<%$ resources:Type %>" SortExpression="" HeaderStyle-CssClass="col-md-2 col-sm-2">
                                            <ItemTemplate>
                                                <asp:Label ID="lblWork" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("WOH_TYPE_TEXT"),25)%>'
                                                    ToolTip='<%# Eval("WOH_TYPE_TEXT")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:GrdStatus %>" SortExpression="" HeaderStyle-CssClass="col-md-1 col-sm-1">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("WOH_STATUS_TEXT")%>'
                                                    ToolTip='<%#Eval("WOH_STATUS_TEXT")%>'></asp:Label>

                                                <%--                                            <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("WOH_ACTIVE") !=null? Eval("WOH_ACTIVE").ToString() ==  ERP.Utilities.CommonConstants.SELECT_VALUE_ONE? GetLocalResourceObject("StatusActive").ToString() : GetLocalResourceObject("StatusInactive").ToString():string.Empty%>'
                                                ToolTip='<%#Eval("WOH_ACTIVE") !=null? Eval("WOH_ACTIVE").ToString() ==  ERP.Utilities.CommonConstants.SELECT_VALUE_ONE? GetLocalResourceObject("StatusActive").ToString() : GetLocalResourceObject("StatusInactive").ToString():string.Empty%>'></asp:Label>--%>
                                                <asp:HiddenField runat="server" ID="hdfWOStatus" Value='<%# Eval("WOH_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfRefID" Value='<%# (Eval("refPK")).ToString() == string.Empty ? 0 : Eval("refPK") %>' />

                                            </ItemTemplate>
                                            <ItemStyle Wrap="false" Width="7%" />
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Action %>">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" TabIndex="16" ID="imbShortClose" CommandName="SHORTCLOSE" OnClientClick="return ShowDeleteConfirm(this,'Are you sure want to close this?');"
                                                    SkinID="btnclose" alt="<%$Resources:Controls,ShortClose%>" title="<%$Resources:Controls,ShortClose%>" />--%>

                                        <%-- </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ Resources:Active %>" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" runat="server" ID="imbActive" SkinID="btnactive" TabIndex="20"
                                                    CommandName="DEACTIVATE" Visible='<%# (Eval("WOH_ACTIVE").ToString() == "1") ? Convert.ToBoolean("true") : Convert.ToBoolean("false") %>'
                                                    Enabled="true" OnClick="ActionHandler" Style="cursor: default;" ToolTip="Active" />
                                                <asp:ImageButton Width="16px" Height="16px" runat="server" ID="imbInActive" SkinID="btninactive" TabIndex="20"
                                                    CommandName="ACTIVATE" Visible='<%# (Eval("WOH_ACTIVE").ToString() == "0") ? Convert.ToBoolean("true") : Convert.ToBoolean("false") %>'
                                                    Enabled="true" OnClick="ActionHandler" Style="cursor: default;" ToolTip="Inactive" />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                    </Columns>

                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Details" runat="server">

                        <asp:TableCell>
                            <div class="contentwrapper margintop29-06">


                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblCode" runat="server" CssClass=" " Text="<%$resources:WoNoStar %>"
                                                    AssociatedControlID="txtWoNo"></asp:Label>
                                                <asp:TextBox ID="txtWoNo" Text="" runat="server" CssClass="input-half" TabIndex="22" />
                                                <asp:ImageButton ID="btnHistory" runat="server" OnClick="ActionHandler" CommandName="HISTORY" TabIndex="23"
                                                    SkinID="history" ToolTip="<%$resources:Controls,History %>" Visible="false" Style="margin-bottom: 0px; margin-top: 2px;" />

                                            </div>

                                        </td>

                                        <td>
                                            <div class="div2col-S">
                                                <%-- <asp:Label ID="lblIName" runat="server" AssociatedControlID="lblItemName" Text="<%$ resources:ItemName %>"></asp:Label>
                                            <asp:Label ID="lblItemName" runat="server" CssClass="input-half lblItemNameheight"></asp:Label>--%>
                                                <asp:Label ID="lblDate" runat="server" Text="<%$resources:DateStar %>" AssociatedControlID="txtDate"
                                                    CssClass=""></asp:Label>
                                                <asp:TextBox ID="txtDate" runat="server" TabIndex="24" CssClass="" ValidationGroup="Save"
                                                    onpaste="return false;" onDrop="return false;" onkeydown="return CheckKey(event)"></asp:TextBox>
                                                <asp:Label ID="lblSite" runat="server" Text="<%$resources:Site %>" AssociatedControlID="txtSite"
                                                    CssClass="minwith-11"></asp:Label>
                                                <asp:TextBox ID="txtSite" runat="server" TabIndex="25" onkeyup="limitText(this,50);"
                                                    onkeydown="limitText(this,50);" MaxLength="50" CssClass="" Width="20.5%"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<%$resources:Err_Date %>"
                                                    Text="*" CssClass="star" ControlToValidate="txtDate" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rfvtxtSite" runat="server" ErrorMessage="<%$resources:Err_Code %>"
                                                    Text="*" CssClass="star" ControlToValidate="txtSite" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7">


                                                <asp:Label ID="lblCustomer" runat="server" CssClass="Typewidth" Text="<%$resources:Customer %>"
                                                    AssociatedControlID="txtCustomer"></asp:Label>
                                                <asp:TextBox ID="txtCustomer" runat="server" TabIndex="26" ValidationGroup="Save"
                                                    onDrop="return false;" onPaste="return false;" CssClass="input-half Typewidth-23-2"></asp:TextBox>
                                                <asp:HiddenField ID="hdfCustomer" runat="server" />
                                                <asp:RequiredFieldValidator ID="vrfCustomer" runat="server" ErrorMessage="<%$resources:Err_Customer %>"
                                                    Text="*" CssClass="star" ControlToValidate="txtCustomer" ValidationGroup="Save" Visible="false"
                                                    InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label ID="lblType" runat="server" Text="<%$resources:TypeStar %>" AssociatedControlID="ddlType"
                                                    CssClass="Typewidth" Visible="false"></asp:Label>
                                                <asp:DropDownList ID="ddlType" runat="server" TabIndex="27" ValidationGroup="Save" Width="60.5%" Visible="false">
                                                </asp:DropDownList>
                                                <%-- <asp:RequiredFieldValidator ID="reqddlType" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="-1" ValidationGroup="Save" EnableClientScript="true" runat="server"
                                                    ControlToValidate="ddlType" Display="Dynamic" Text="*" ErrorMessage="<%$resources:Err_Type %>">
                                                </asp:RequiredFieldValidator>--%>
                                                <asp:Label ID="lblRefNo" runat="server" CssClass="" Text="<%$resources:RefNo %>"
                                                    AssociatedControlID="txtRefNo"></asp:Label>
                                                <asp:TextBox ID="txtRefNo" runat="server" TabIndex="28" onkeyup="limitText(this,200);"
                                                    onkeydown="limitText(this,200);" MaxLength="200" CssClass=""></asp:TextBox>
                                                <asp:Label ID="lblReffDate" runat="server" Text="<%$resources:RefDate %>" AssociatedControlID="txtReffDate"
                                                    CssClass="minwith-11"></asp:Label>
                                                <asp:TextBox ID="txtReffDate" runat="server" TabIndex="29" CssClass="" Width="20.5%"
                                                    onpaste="return false;" onDrop="return false;" onkeydown="return CheckKey(event)"></asp:TextBox>




                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7">

                                                <asp:Label ID="lblConsultant" runat="server" CssClass="" Text="<%$resources:ConsultantStar %>"
                                                    AssociatedControlID="txtConsultant" Visible="false"></asp:Label>
                                                <asp:TextBox ID="txtConsultant" runat="server" TabIndex="30" ValidationGroup="Save" Visible="false"
                                                    onDrop="return false;" onPaste="return false;" CssClass="input-half"></asp:TextBox>
                                                <asp:HiddenField ID="hdfConsultant" runat="server" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7" id="divAmendment" runat="server" visible="false">
                                                <asp:Label ID="lblAmendmentNo" runat="server" CssClass="managelbl margnrgt-minus1"
                                                    Text="<%$resources:AmendmentNo %>" AssociatedControlID="txtAmendmentNo"></asp:Label>
                                                <asp:TextBox ID="txtAmendmentNo" runat="server" TabIndex="31" onkeyup="limitText(this,200);"
                                                    onkeydown="limitText(this,200);" MaxLength="200" CssClass="group-txtbx" Enabled="false"></asp:TextBox><%--CssClass in code also--%>
                                                <asp:RequiredFieldValidator ID="rfvAmendmentNo" runat="server" ErrorMessage="<%$resources:Err_AmendmentNo %>"
                                                    Text="*" CssClass="star" ControlToValidate="txtAmendmentNo" ValidationGroup="Save"
                                                    Enabled="false"></asp:RequiredFieldValidator>
                                                <asp:Label ID="lblAmendmentDate" runat="server" Text="<%$resources:AmendmentDate %>"
                                                    AssociatedControlID="txtAmendmentDate" CssClass="lft-lbl"></asp:Label>
                                                <asp:TextBox ID="txtAmendmentDate" runat="server" TabIndex="32" CssClass="group-txtbx"
                                                    onpaste="return false;" onDrop="return false;" onkeydown="return CheckKey(event)"
                                                    Enabled="false" Width="25%"></asp:TextBox><%--CssClass in code also--%>
                                                <asp:RequiredFieldValidator ID="rfvAmendmentDate" runat="server" ErrorMessage="<%$resources:Err_AmendmentDate %>"
                                                    Text="*" CssClass="star" ControlToValidate="txtAmendmentDate" ValidationGroup="Save"
                                                    Enabled="false"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <%--row--%>
                                            </div>
                                        </td>
                                    </tr>
                                </table>

                                <table class="table-devide">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblWorkProject" runat="server" CssClass="desclbl" Text="<%$resources:WorkProjectStar %>"
                                                    AssociatedControlID="txtWorkProject"></asp:Label>
                                                <asp:TextBox ID="txtWorkProject" runat="server" CssClass="desctxtbx" ValidationGroup="Save"
                                                    TabIndex="33" onkeyup="limitText(this,100);" onkeydown="limitText(this,100);"
                                                    MaxLength="100" TextMode="MultiLine" Height="40"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqWorkProject" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtWorkProject"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$resources:Err_WorkProject %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblScope" runat="server" CssClass="desclbl" Text="<%$resources:Scope %>"
                                                    AssociatedControlID="txtScope"></asp:Label>
                                                <asp:TextBox ID="txtScope" runat="server" CssClass="desctxtbx" TabIndex="34" onkeyup="limitText(this,500);"
                                                    onkeydown="limitText(this,500);" MaxLength="500" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblDescription" runat="server" CssClass="desclbl" Text="<%$resources:Description %>"
                                                    AssociatedControlID="txtDescription"></asp:Label>
                                                <asp:TextBox ID="txtDescription" CssClass="desctxtbx" runat="server" TextMode="MultiLine"
                                                    TabIndex="35" onkeyup="limitText(this,2000);" onkeydown="limitText(this,2000);"
                                                    MaxLength="2000"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                </table>


                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label ID="lblLocation" runat="server" Text="<%$resources:Location %>" AssociatedControlID="txtLocation"
                                                    CssClass="managelbl"></asp:Label>
                                                <asp:TextBox ID="txtLocation" runat="server" TabIndex="36" onkeyup="limitText(this,200);"
                                                    onkeydown="limitText(this,200);" MaxLength="200" Width="61%"></asp:TextBox>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label ID="lblCompany" runat="server" CssClass="managelbl" Text="<%$resources:Company %>"
                                                    AssociatedControlID="ddlCompany"></asp:Label>
                                                <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="37" Width="61%">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label ID="lblEstimatedStart" runat="server" CssClass="managelbl" Text="<%$resources:EstimatedStartStar %>"
                                                    AssociatedControlID="txtEstimatedStart"></asp:Label>
                                                <asp:TextBox ID="txtEstimatedStart" runat="server" TabIndex="38" ValidationGroup="Save"
                                                    CssClass="group-txtbx" onpaste="return false;" onDrop="return false;" onkeydown="return CheckKey(event)"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rqvalEstimatedStart" runat="server" ErrorMessage="<%$resources:Err_EstimatedStart %>"
                                                    Text="*" CssClass="star" ControlToValidate="txtEstimatedStart" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                <asp:Label ID="lblEstimatedEnd" runat="server" Text="<%$resources:EndStar %>" AssociatedControlID="txtEstimatedEnd"
                                                    CssClass="lft-lbl"></asp:Label>
                                                <asp:TextBox ID="txtEstimatedEnd" runat="server" TabIndex="39" ValidationGroup="Save" Width="25%"
                                                    CssClass="input-small-c" onpaste="return false;" onDrop="return false;" onkeydown="return CheckKey(event)"></asp:TextBox>

                                                <asp:RequiredFieldValidator ID="rqvalEstimatedEnd" runat="server" ErrorMessage="<%$resources:Err_EstimatedEnd %>"
                                                    Text="*" CssClass="star" ControlToValidate="txtEstimatedEnd" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label ID="lblActualStart" runat="server" CssClass="managelbl" Text="<%$resources:ActualStart %>"
                                                    AssociatedControlID="txtActualStart"></asp:Label>
                                                <asp:TextBox ID="txtActualStart" runat="server" TabIndex="40" CssClass="group-txtbx"
                                                    onpaste="return false;" onDrop="return false;" onkeydown="return CheckKey(event)"></asp:TextBox>
                                                <asp:Label ID="lblActualEnd" runat="server" Text="<%$resources:End %>" AssociatedControlID="txtActualEnd"
                                                    CssClass="middle-lbl-xsmall-f"></asp:Label>
                                                <asp:TextBox ID="txtActualEnd" runat="server" TabIndex="41" CssClass="group-txtbx" Width="24.3%"
                                                    onpaste="return false;" onDrop="return false;" onkeydown="return CheckKey(event)"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label ID="lblDefectLiability" runat="server" CssClass="managelbl" Text="<%$resources:DefectLiability %>"
                                                    AssociatedControlID="txtDefectLiability"></asp:Label>
                                                <asp:TextBox ID="txtDefectLiability" runat="server" TabIndex="42" CssClass="medium-responsive margn-rgt-2-1 numeric"
                                                    MaxLength="10" onkeyup="limitText(this,13);" onkeydown="limitText(this,13);"></asp:TextBox>
                                                <asp:DropDownList ID="ddlDefectLiabilityUOM" runat="server" TabIndex="43" CssClass="small-responsive DefectLiabilityUOM-ddl" Width="26%">
                                                </asp:DropDownList>
                                                <Gti:AmountValidation ID="vretxtDefectLiability" runat="server" ControlToValidate="txtDefectLiability"
                                                    ErrorMessage="<%$ resources:Err_ValidDefectLiability %>" NumberDigits="13" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Save"></Gti:AmountValidation>
                                                <asp:CustomValidator ID="csvDefectLiability" runat="server" ErrorMessage="<%$resources:Err_DefectLiability %>"
                                                    Display="Dynamic" OnServerValidate="DefectLiability_ServerValidate" ClientValidationFunction="DefectLiability_ClientValidate"
                                                    ValidationGroup="Save" Text="*" CssClass="star" />
                                                <asp:CustomValidator ID="csvDefectLiabilityUOM" runat="server" ErrorMessage="<%$resources:Err_DefectLiabilityUOM %>"
                                                    Display="Dynamic" OnServerValidate="DefectLiabilityUOM_ServerValidate" ClientValidationFunction="DefectLiabilityUOM_ClientValidate"
                                                    ValidationGroup="Save" Text="*" CssClass="star" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label ID="lblCurrency" runat="server" CssClass="managelbl" Text="<%$resources:CurrencyStar %>"
                                                    AssociatedControlID="ddlCurrency"></asp:Label>
                                                <asp:DropDownList ID="ddlCurrency" runat="server" TabIndex="44" ValidationGroup="Save"
                                                    AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" CssClass="group-select">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblExchangeRate" runat="server" Text="<%$resources:ExchangeRateStar %>" AssociatedControlID="txtExchangeRate"
                                                    CssClass="group-lbl6 margnrgt3-5px"></asp:Label>
                                                <asp:TextBox ID="txtExchangeRate" runat="server" TabIndex="45" ValidationGroup="Save" Width="24.3%"
                                                    CssClass="group-txtbx numeric" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                                    MaxLength="15" onkeyup="limitText(this,10);" onkeydown="limitText(this,10);"></asp:TextBox>
                                                <Gti:ExchangeRateValidation ID="vretxtExchangeRate" runat="server" ControlToValidate="txtExchangeRate"
                                                    ErrorMessage="<%$ resources:Err_ValidExchangeRate %>" NumberDigits="13" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Save"></Gti:ExchangeRateValidation>
                                                <asp:RequiredFieldValidator ID="rvCurrency" CssClass="star" SetFocusOnError="true"
                                                    InitialValue="-1" ValidationGroup="Save" EnableClientScript="true" runat="server"
                                                    ControlToValidate="ddlCurrency" Display="Dynamic" Text="*" ErrorMessage="<%$resources:Err_Currency %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="rvExchangeRate" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="Save" EnableClientScript="true" runat="server" ControlToValidate="txtExchangeRate"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$resources:Err_ExchangeRate %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:CustomValidator runat="server" ID="reqZeroQtyValidation" Text="*" CssClass="star"
                                                    ControlToValidate="txtExchangeRate" ValidationGroup="Save" ErrorMessage="<%$resources:Err_ZeroQty %>"
                                                    ClientValidationFunction="IsValidQty" OnServerValidate="ZeroQtyValidation_Validate" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7" style="display: none">
                                                <asp:Label ID="lblRetention" runat="server" CssClass="managelbl" Text="<%$resources:Retention %>"
                                                    AssociatedControlID="txtRetention"></asp:Label>
                                                <asp:TextBox ID="txtRetention" runat="server" TabIndex="46" CssClass="group-txtbx numeric"
                                                    MaxLength="6" onkeyup="limitText(this,6);" onkeydown="limitText(this,6);"></asp:TextBox>
                                                <asp:Label ID="lblLimit" runat="server" Text="<%$resources:Limit %>" AssociatedControlID="txtLimit"
                                                    CssClass="group-lbl6"></asp:Label>
                                                <asp:TextBox ID="txtLimit" runat="server" TabIndex="47" CssClass="group-txtbx numeric"
                                                    MaxLength="16" onkeyup="limitText(this,13);" onkeydown="limitText(this,13);"></asp:TextBox>
                                                <Gti:AmountValidation ID="vretxtRetention" runat="server" ControlToValidate="txtRetention"
                                                    ErrorMessage="<%$ resources:Err_Retention %>" NumberDigits="6" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Save"></Gti:AmountValidation>
                                                <Gti:AmountValidation ID="vretxtLimit" runat="server" ControlToValidate="txtLimit"
                                                    ErrorMessage="<%$ resources:Err_Limit %>" NumberDigits="13" Display="Dynamic"
                                                    Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="Save"></Gti:AmountValidation>
                                                <asp:CustomValidator ID="csvPercentage" runat="server" ErrorMessage="<%$resources:Err_Retention %>"
                                                    Display="Dynamic" OnServerValidate="Percentage_ServerValidate" ClientValidationFunction="Percentage_ClientValidate"
                                                    ValidationGroup="Save" Text="*" CssClass="star" />
                                            </div>
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                                <table class="table-devide">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblRemarks" runat="server" CssClass="desclbl" Text="<%$resources:Remarks %>"
                                                    AssociatedControlID="txtRemarks"></asp:Label>
                                                <asp:TextBox ID="txtRemarks" CssClass="desctxtbx" runat="server" TextMode="MultiLine"
                                                    TabIndex="48" onkeyup="limitText(this,2000);" onkeydown="limitText(this,2000);"
                                                    MaxLength="2000"></asp:TextBox>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <table class="table-devide">
                                    <tr>

                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label ID="lblProjectBudjet" runat="server" Text="<%$resources:ProjectBudget %>" AssociatedControlID="txtProjectBudjet"
                                                    CssClass="txtbx-36-9per" Enabled="false"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtProjectBudjet" CssClass="txtbx-18per numeric"
                                                    TabIndex="49"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="vrfBudget" runat="server" ErrorMessage="<%$resources:Err_ProjectBudget %>" Visible="false"
                                                    Text="*" CssClass="star" ControlToValidate="txtProjectBudjet" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                <asp:Label ID="lblProjectBudget" runat="server" CssClass="managelbl" Text="Lump sum Project"
                                                    AssociatedControlID="chkLumpSumPrj" Visible="false"></asp:Label>
                                                <asp:Label ID="lblStatus" runat="server" CssClass="managelbl" Text="<%$resources:Status %>"
                                                    AssociatedControlID="chkStatus"></asp:Label>
                                                <asp:CheckBox runat="server" ID="chkStatus" TabIndex="50" Checked="true" />
                                                <asp:CheckBox runat="server" ID="chkLumpSumPrj" TabIndex="51" Checked="false" onchange="checkLumpSumPrj(this);" Visible="false" />


                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S padgtop7">
                                                <asp:Label ID="lblLumpSumPrjValue" runat="server" Text="Value" AssociatedControlID="txtLumpSumPrjValue"
                                                    CssClass="txtbx-36-9per" Enabled="false" Visible="false"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtLumpSumPrjValue" CssClass="txtbx-18per numeric"
                                                    TabIndex="52" Visible="false"></asp:TextBox>

                                            </div>
                                        </td>
                                    </tr>

                                </table>


                            </div>



                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:workflowusercomments id="ucrWrkf" runat="server" validationgroup="wo" />
            </div>
            <%--  Shortclose region Start--------------------%>
            <div id="divShortClose" title="<%=Resources.Controls.ShortClose%>" style="display: none">
                <div class="divcolmiddle-S">
                    <label for="lblWoNo">
                        <%=Resources.Controls.WoNo%></label>
                    <asp:Label ID="lblWoNo" class="lbl-22perc" runat="server"></asp:Label><label for="Remarks"><%=Resources.Controls.Remark%>*</label>
                    <asp:TextBox runat="server" ID="Remarks" TabIndex="60" MaxLength="200" TextMode="MultiLine" Height="40px">
                    </asp:TextBox><asp:RequiredFieldValidator ID="reqRemarks" CssClass="star" SetFocusOnError="true" EnableClientScript="true"
                        ValidationGroup="ShortCloseSave" runat="server" ControlToValidate="Remarks" Display="Dynamic"
                        Text="*" ErrorMessage="<%$ resources:EnterRemarks %>">
                    </asp:RequiredFieldValidator><label for="RefNo"><%=Resources.Controls.RefNo%></label><asp:TextBox runat="server" ID="RefNo" TabIndex="61" MaxLength="14">
                    </asp:TextBox><div class="clear">
                    </div>
                    <asp:Label ID="lbnSpace" runat="server" AssociatedControlID="btnAddConv"></asp:Label><asp:HiddenField ID="PRJID" runat="server" Value="0"></asp:HiddenField>
                    <asp:Button runat="server" ID="btnAddConv" Text="<%$Resources:Controls,ShortClose%>" CommandName="SHORTCLOSESAVE" OnClick="ActionHandler" ValidationGroup="ShortCloseSave"
                        class="inputbtn" Width="100px" Height="20px" OnClientClick="ValidatePageNow('ShortCloseSave');" TabIndex="62" />
                    <div class="clear">
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdfCloseWO" runat="server" Value="0" />
            <%-- End Shortclose region -----------------%>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star">
                </asp:Label><asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsCancelWO" ValidationGroup="ShortCloseSave" runat="server" />
            </div>
            <div id="divHistoryDetail" style="display: none;">
                <div class="manage-container w-auto padg-0">
                    <div class="asptbllinks">
                        <div class="gridwrap col-md-12 col-sm-12 col-xs-12">
                            <asp:GridView ID="grdHistoryList" runat="server" AutoGenerateColumns="False" Width="100%"
                                AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ resources:GRDDtlVersion %>">
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdfPk" Value='<%# Eval("WOH_PK") %>' />
                                            <asp:HiddenField runat="server" ID="hdfVersion" Value='<%# Eval("WOH_VERSION") %>' />
                                            <asp:Label ID="lblDtlVersion" runat="server" Text='<%# ERP.Utilities.CommonFunctions.HtmlDecodeUtility(Eval("WOH_VERSION"))%>'
                                                ToolTip='<%#ERP.Utilities.CommonFunctions.HtmlDecodeUtility(Eval("WOH_VERSION"))%>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-4 col-sm-4 col-xs-4" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:GRDDtlNo %>">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDtlNo" runat="server" CssClass="text-underline" OnClick="ActionHandler"
                                                CommandName="PRINTREPORT" CommandArgument='<%# Container.DataItemIndex %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("WOH_NO"),25) %>'
                                                ToolTip='<%# ERP.Utilities.CommonFunctions.HtmlDecodeUtility(Eval("WOH_NO")) %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-5 col-sm-5 col-xs-5" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:GRDDtlDate %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDtlDate" runat="server" Text='<%#Convert.ToDateTime(Eval("WOH_DATE")).ToString(Resources.Constants.DateFormatShort) %>'
                                                ToolTip='<%#Convert.ToDateTime(Eval("WOH_DATE")).ToString(Resources.Constants.DateFormatShort) %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="col-md-3 col-sm-3 col-xs-3" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
