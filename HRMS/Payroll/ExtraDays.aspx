<%@ Page Title="<%$ Resources:Captions,Title_ExtraDays %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="ExtraDays.aspx.cs" Inherits="HRMS.Payroll.ExtraDays"
    Theme="ClassicExt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrLoc", url, "hdfBrLoc", true, true, "BRANCHLOCATION", false, true);

            $("[id*=txtExtraDays]").ForceNumericOnly();
            GrandScriptUtils.DatePickerCommon("txtDateHd");
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.DatePickerCommon("txtFilterFromDate");
            GrandScriptUtils.DatePickerCommon("txtFilterToDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtFilterBranch", url, "hdfFilterBranch", true, true, "BRANCHLOCATION");
            GrandScriptUtils.DatePickerCommon("txtImportDate");
            ShowHideDetails(1);
            BindEmployee();
            EnableDisableEmployee();

            var totalRows = parseInt($("#<%=grdExtraDaysList.ClientID %> tr").length);
            var currPk = parseInt($("[id$=hdfCurrentPk]").val());
            if ((!isNaN(totalRows) && totalRows > 1) || (!isNaN(currPk) && currPk > 0)) {
                DisableAuto($("[id$=txtBrLoc]"), $("[id$=hdfBrLoc]"));
                $("[id$=txtMonth]").addClass("input-disabled");
                $("[id$=txtMonth]").attr("disabled", true);
            }
            else {
                EnableAuto($("[id$=txtBrLoc]"), $("[id$=hdfBrLoc]"));
                $("[id$=txtMonth]").removeClass("input-disabled")
                $("[id$=txtMonth]").attr("disabled", false);
            }
        }

        function BindEmployee() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?Type=" + $("[id$=hdfBrLoc]").val() + "&EmpCategory=2&ToDate=" + $("[id$=txtDate]").val(), "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETEBYFILTER", "", true, true, false, 1);
        }
        function ResetEmployee() {
            var defText = '<%= Resources.ErpRes.AutoDefaultValue %>';
            $("[id$=txtEmployee]").val(defText);
            $("[id$=hdfEmployee]").val('-1');
        }

        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtBrLoc") {
                BindEmployee();
                ResetEmployee();
                EnableDisableEmployee();
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtEmployee") {
                $("[id$=hdfEmployee]").val("0");
            }
            else if (targetControlID == "txtBrLoc") {
                BindEmployee();
                ResetEmployee();
                EnableDisableEmployee();
            }
        }

        function AfterDateSelect(controlID) {
            if (controlID == "txtDate") {
                BindEmployee();
                ResetEmployee();
                EnableDisableEmployee();
            }
        }
        function ClearDateSelect() {
            if ($("[id$=txtDate]").val() == '') {
                BindEmployee();
                ResetEmployee();
                EnableDisableEmployee();
            }
        }
        function EnableDisableEmployee() {
            var branchid = parseInt($("[id$=hdfBrLoc]").val());
            if (isNaN(branchid) || branchid <= 0)
                DisableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));
            else
                EnableAuto($("[id$=txtEmployee]"), $("[id$=hdfEmployee]"));
        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$='PageAction_List']").show();
                $("[id$='PageAction_Entry']").hide();
                $("[id$='pnlListing']").show();
                $("[id$='pnlEntry']").hide();
            }
            else {
                $("[id$='PageAction_List']").hide();
                $("[id$='PageAction_Entry']").show();
                $("[id$='pnlListing']").hide();
                $("[id$='pnlEntry']").show();
            }
            return false;
        }
        function ViewMode(mode) {
            ///<summary>
            /// Used to handle the view Mode
            ///</summary>
            /// <param name="mode" optional="true" type="String">
            /// Mode = 1 Determins ites on View Mode
            /// Mode = 2 Indicates its on New Mode
            /// </param>         
            if (mode == 1) {
                $("[id$='pnlSave']").hide();
                $("[id$='pnlDelete']").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
            }
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
        function ValidateNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup); //For finding and removing duplicate and other group validation controls
                Page_ClientValidate(valGroup); //For Script validating the Page
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverrorAlert").html());
                return false; //Page is invalid -- stop right here
            }
            else {
                return true; //everythings ok --- Call your function & do your stuff
            }
        }



        function ShowHideDetails(flag) {
            if (flag == 1) {
                $("[id$=divDetails").show();
                $("[id$=imbShowDetails").hide();
                $("[id$=imbHideDetails").show();
            }
            else {
                $("[id$=divDetails").hide();
                $("[id$=imbShowDetails").show();
                $("[id$=imbHideDetails").hide();
            }
            return false;
        }

        /// Used to disable Autocomplete
        function DisableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }

        /// Used to disable Autocomplete
        function EnableAuto(extender, hfield) {
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
            $(extender).attr("disabled", false);
        }

        function ShowHideImportSec(flag) {
            ///<summary>
            /// Used to Show/Hide Import Section div
            ///</summary>
            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divImportSec]").show();
                $("[id$=imbShowImportSec]").hide();
                $("[id$=imbHideImportSec]").show();
            }
            else {
                $("[id$=divImportSec]").hide();
                $("[id$=imbShowImportSec]").show();
                $("[id$=imbHideImportSec]").hide();
            }
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlTaskHome" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidateNow('Save')"
                                            TabIndex="150" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="151" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="152" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="152" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="153" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                            ToolTip="<%$resources:Controls,Edit %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass=" tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse-b">
                                <h1>
                                    <%= GetLocalResourceObject("ImportExtraDays").ToString()%></h1>
                                <asp:ImageButton runat="server" ID="imbShowImportSec" OnClientClick="javascript:return ShowHideImportSec(1);"
                                    SkinID="imbArrowInactive" ToolTip="<%$ resources:Show%>" TabIndex="5" />
                                <asp:ImageButton runat="server" ID="imbHideImportSec" OnClientClick="javascript:return ShowHideImportSec();"
                                    Style="display: none" SkinID="imbArrowActive" TabIndex="5" ToolTip="<%$ resources:Hide%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divImportSec" style="display: none">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7 padgbotm3">
                                                <asp:Label ID="lblImprtDate" runat="server" Text="<%$ resources:DateReq%>" AssociatedControlID="txtImportDate"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtImportDate" TabIndex="70" CssClass="input-small margnbotm0"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="tfvImportDate" CssClass="star input-medium" SetFocusOnError="false"
                                                    ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="txtImportDate"
                                                    Display="Static" Text="*" ErrorMessage="<%$ resources:Err_ImportDate %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblCompanyImpt" runat="server" Text="<%$ resources:Controls,CompanyReq%>"
                                                    AssociatedControlID="ddlCompanyImpt"></asp:Label>
                                                <asp:DropDownList ID="ddlCompanyImpt" runat="server" TabIndex="1" CssClass="select-w61per">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvCompanyImpt" CssClass="star" SetFocusOnError="true"
                                                    runat="server" ControlToValidate="ddlCompanyImpt" Display="Dynamic" Text="*"
                                                    InitialValue="-1" ValidationGroup="upload" ErrorMessage="<%$ resources:Err_SelectComapny %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S padgtop7 padgbotm3 margnbotm5">
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Label runat="server" ID="lblSourceFile" Text="<%$ resources: SourceFileStar %>"
                                                            AssociatedControlID="fupImport"></asp:Label>
                                                        <div class="fileupload-main">
                                                            <asp:FileUpload ID="fupImport" runat="server" TabIndex="75" CssClass="margnbotm0 margn-rgt0 upload-area3" />
                                                            <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star input-w27per" SetFocusOnError="true"
                                                                ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupImport"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                                            </asp:RequiredFieldValidator>
                                                        </div>
                                                        <asp:Button runat="server" ID="btnImport" CommandName="SAVEIMPORT" TabIndex="76"
                                                            Text="<%$resources:Import %>" OnClick="ActionHandler" ToolTip="<%$resources:Import %>"
                                                            SkinID="btnInner-add" ValidationGroup="upload" OnClientClick="javascript:ValidateNow('upload')"
                                                            Style="margin-bottom: 3px !important;" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:PostBackTrigger ControlID="btnImport" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S txt-rgt">
                                                <a id="aTmpDwn" class="btnInner-dwn btnInner-med-size decoration-none margnrgt13-5per"
                                                    href='<%= Page.ResolveClientUrl((string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()) ? "~/Upload/" : System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() )+ "Template/" + GetGlobalResourceObject("ConfigurationsRes", "HrmsImportTemplateExtradays").ToString())%>'>
                                                    <%= Resources.Controls.Template.ToString() %>
                                                </a>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="search-colapse" id="divAdvanceSearch">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>"
                                                TabIndex="2" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="2" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblFilterFromDate" runat="server" Text="<%$ resources:FromDate%>"
                                                AssociatedControlID="txtFilterFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterFromDate" TabIndex="50" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:Label ID="lblFilterToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtFilterToDate"
                                                CssClass="middle-lbl"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterToDate" TabIndex="51" CssClass="input-small margnbotm0"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblfilterBranch" runat="server" Text="<%$ resources:BrLoc%>" AssociatedControlID="txtFilterBranch"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterBranch" Text="" TabIndex="52" CssClass="select-half margnbotm0"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterBranch" Value="" runat="server" />
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Search%>" ToolTip="<%$ resources:Search%>"
                                                OnClick="ActionHandler" TabIndex="53" CommandName="FILTER" SkinID="search-ext"
                                                CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Clear%>" ToolTip="<%$ resources:Clear%>"
                                                TabIndex="54" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" onclick="GrandScriptUtils.EnableRbtnGrouping(this);"
                                                    TabIndex="55" />
                                                <asp:HiddenField ID="hdfEmpLeavePk" Value='<%# Eval("EXH_PK")%>' runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EXH_NO")))?Resources.ErpRes.Draft:Eval("EXH_NO")%>'
                                                    ToolTip='<%# string.IsNullOrEmpty(Convert.ToString(Eval("EXH_NO")))?Resources.ErpRes.Draft:Eval("EXH_NO")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstDate" runat="server" Text='<%# Eval("EXH_SAL_MONTH", Resources.Constants.HRMSDateFormatGrid)  %>'
                                                    ToolTip='<%# Eval("EXH_SAL_MONTH", Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BrLoc%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstBranch" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELR_BRANCH_TEXT"))),35) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("ELR_BRANCH_TEXT")))  %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                            <HeaderStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLstRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EXH_DESC"))),85) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EXH_DESC"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="50%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" TabIndex="4" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblTrxNoHdr" runat="server" Text="<%$ resources:TrxNo%>" AssociatedControlID="lblTrxNo"></asp:Label>
                                            <asp:Label runat="server" ID="lblTrxNo" CssClass="input-small"></asp:Label>
                                            <asp:Label ID="lblDateHd" runat="server" Text="<%$ resources:DateReq%>" AssociatedControlID="txtDateHd"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox ID="txtDateHd" runat="server" MaxLength="200" CssClass="input-small"
                                                TabIndex="1" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ControlToValidate="txtDateHd"
                                                Display="Dynamic" CssClass="star" ValidationGroup="Save" Text="*" EnableClientScript="true"
                                                ErrorMessage="<%$ resources:Err_Date%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCompanyHd" runat="server" Text="<%$ resources:Controls,CompanyReq%>"
                                                AssociatedControlID="ddlCompany"></asp:Label>
                                            <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" CssClass="select-w61per">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvCompany" CssClass="star" SetFocusOnError="true"
                                                runat="server" ControlToValidate="ddlCompany" Display="Dynamic" Text="*" InitialValue="-1"
                                                ValidationGroup="Save" ErrorMessage="<%$ resources:Err_SelectComapny %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBrLoc" runat="server" Text="<%$ resources:BrLocReq%>" AssociatedControlID="txtBrLoc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBrLoc" Text="" TabIndex="1" CssClass="input-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfBrLoc" Value="" runat="server" />
                                            <asp:RequiredFieldValidator ID="vrfBrLoc" runat="server" SetFocusOnError="true" ControlToValidate="txtBrLoc"
                                                InitialValue="<%$resources:ErpRes,AutoDefaultValue %>" CssClass="star" ValidationGroup="AddToList"
                                                Text="*" ErrorMessage="<%$ resources:Err_BrLoc%>"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="vrfBrLocSave" runat="server" SetFocusOnError="true"
                                                ControlToValidate="txtBrLoc" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_BrLoc%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblDescription" runat="server" Text="<%$ resources:Description%>"
                                                AssociatedControlID="txtDescription"></asp:Label>
                                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="multiline-2a-line input-full"
                                                MaxLength="500" TabIndex="1" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    <asp:Literal ID="ltrLeaveAddDed" runat="server" Text="<%$ resources:ExtraDays%>" /></h1>
                                <asp:ImageButton runat="server" ID="imbShowDetails" OnClientClick="javascript:return ShowHideDetails(1);"
                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                <asp:ImageButton runat="server" ID="imbHideDetails" OnClientClick="javascript:return ShowHideDetails();"
                                    Style="display: none" SkinID="imbArrowHide" ToolTip="<%$ resources:Controls,HideDetails%>" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divDetails">
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblDate" runat="server" Text="<%$ resources:DateReq%>" AssociatedControlID="txtDate"></asp:Label>
                                                <asp:TextBox ID="txtDate" runat="server" MaxLength="200" CssClass="input-small" TabIndex="2"
                                                    onblur="ClearDateSelect()" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                                <asp:RequiredFieldValidator ID="vrfDate" runat="server" ControlToValidate="txtDate"
                                                    CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_Date%>"></asp:RequiredFieldValidator>
                                                <asp:Label ID="lblExtraDays" runat="server" Text="<%$ resources:ExtraDaysReq%>" AssociatedControlID="txtExtraDays"
                                                    CssClass="lbl-19-6perc"></asp:Label>
                                                <asp:TextBox ID="txtExtraDays" runat="server" CssClass="input-small numeric" TabIndex="2"
                                                    MaxLength="8" onkeyup="limitText(this,10);" onkeydown="limitText(this,10);" onDrop="return false;"
                                                    onPaste="return false;"> </asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvExtraDays" runat="server" ControlToValidate="txtExtraDays"
                                                    Display="Dynamic" CssClass="star" ValidationGroup="AddToList" Text="*" ErrorMessage="<%$ resources:Err_ExtraDays%>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="rngExtraDays" runat="server" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="AddToList" EnableClientScript="true" Display="Dynamic" Text="*"
                                                    MinimumValue="0.001" MaximumValue="999999999" ControlToValidate="txtExtraDays"
                                                    Type="Double" ErrorMessage="<%$ resources:Err_ZeroExtraDays %>">
                                                </asp:RangeValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblEmployee" runat="server" Text="<%$ resources:EmployeeStar%>" AssociatedControlID="txtEmployee"></asp:Label>
                                                <asp:TextBox ID="txtEmployee" runat="server" TabIndex="2" CssClass="input-half" MaxLength="200"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfEmployee" runat="server" Value="0" />
                                                <asp:RequiredFieldValidator ID="reqEmployee" CssClass="star" SetFocusOnError="true"
                                                    runat="server" ControlToValidate="txtEmployee" Display="Dynamic" Text="*" ValidationGroup="AddToList"
                                                    ErrorMessage="<%$ resources:Err_SelectEmployee %>" InitialValue="<%$resources:ErpRes,AutoDefaultValue %>">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks%>" AssociatedControlID="txtRemarks"></asp:Label>
                                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" CssClass="multiline-2a-line input-full"
                                                    MaxLength="500" TabIndex="2" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                                                <asp:ImageButton ID="btnAdd" runat="server" OnClick="ActionHandler" CommandName="ADD"
                                                    ToolTip="<%$ resources:Add%>" ValidationGroup="AddToList" OnClientClick="javascript:ValidateNow('AddToList')"
                                                    TabIndex="3" SkinID="plus" CssClass="margntop2 margnbotm0 margn-rgt4" />
                                                <asp:ImageButton ID="imbDetSearch" runat="server" ToolTip="<%$ resources:Clear%>"
                                                    OnClick="ActionHandler" TabIndex="3" CommandName="CLEARADDTOLIST" SkinID="clear-ext"
                                                    CssClass="margntop2 margnbotm0 margn-rgt4" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap">
                                <%--class="grdTable"--%>
                                <asp:GridView runat="server" ID="grdExtraDaysList" Width="100%" AllowPaging="false"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:Date%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.EXD_DATE, Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.EXD_DATE, Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Employee%> " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EXD_EMPLOYEE_TEXT")),85) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EXD_EMPLOYEE_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EXD_REMARK")),75) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EXD_REMARK")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ExtraDays%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblgrdExDays" runat="server" Text='<%# GetFormattedNumber(Eval("EXD_DAYS")) %>'
                                                    ToolTip='<%# GetFormattedNumber(Eval("EXD_DAYS")) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                    SkinID="imbeditgrid" CommandName="EDIT_ACTION" TabIndex="4" ToolTip="<%$resources:Controls,Edit %>"
                                                    Visible='<%# Convert.ToInt32(Eval("EXD_PAYROLL_DTL")) <= 0 ? true : false %>' />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteDetails"
                                                    SkinID="imbdeletegrid" ToolTip="<%$resources:Controls,Delete %>" CommandName="DELETE_ACTION"
                                                    OnClientClick="return ShowDeleteConfirm(this);" TabIndex="4" Visible='<%# Convert.ToInt32(Eval("EXD_PAYROLL_DTL")) <= 0 ? true : false %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverrorAlert" style="display: none">
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
                <asp:ValidationSummary ID="vsAddToList" ValidationGroup="AddToList" runat="server" />
                <asp:ValidationSummary ID="vsUpload" ValidationGroup="upload" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label></div>
            <asp:HiddenField ID="hdfIscontYes" runat="server" />
            <asp:HiddenField ID="hdfNoOfLeaveValidation" runat="server" />
            <asp:HiddenField ID="hdfCurrentPk" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurRelodPk" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
