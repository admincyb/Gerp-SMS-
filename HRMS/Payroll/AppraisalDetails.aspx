<%@ Page Title="<%$ Resources:Captions,Title_AppraisalDetails %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="AppraisalDetails.aspx.cs" Inherits="HRMS.Payroll.AppraisalDetails"
    Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/Admin/Masters/UserControls/FormulaMaster.ascx" TagName="PopUp"
    TagPrefix="uc1" %>
<%@ Register Src="../Employees/UserControls/EmpSalaryControl.ascx" TagName="EmpSalaryControl"
    TagPrefix="ucgti" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        function InitComponents() {
            ShowHideEarnings(1);
            ShowHideDeductions(1);
            var pageURL = window.document.URL;
            var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
            var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

            GrandScriptUtils.AddDateRangeCommon("txtFilterFromDate", "hdfFilterFromDate", "txtFilterToDate", "hdfFilterToDate", false, false, false);

            GrandScriptUtils.DatePickerCommon("txtTransactionDate");
            GrandScriptUtils.DatePickerCommon("txtEffectiveFrom");
            // in url Location for location dropdown and Type for employeement type 
//            GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee", url + "?Location=" + $("[id$='ddlBranchLoaction']").val() + "&Type=" + $("[id$='ddlEmploymentType']").val(), "hdfEmployee", true, true, "EMPLOYEEAUTO");

//            if ($("[id$=ddlBranchLoaction]").val() > 0) {
//                GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?Type=" + $("[id$=ddlBranchLoaction]").val() + "&SearchValue=" + $("[id$='ddlEmploymentType']").val() + "&EmpCategory=2", "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETEBYFILTER");
//            }
//            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtEmployee ", url + "?EmpCategory=2", "hdfEmployee", true, true, "EMPLOYEEAUTOCOMPLETE");
//            }
        }
        function AfterAutoCompleteSelect(cntrl) {
            if (cntrl == 'txtEmployee') {
                $("[id$='btnEmployeeSelect']").click();
            }
        }
        function AfterInvalidSelect(cntrl) {
            if (cntrl == 'txtEmployee') {
                $("[id$='btnEmployeeSelect']").click();
            }
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
        function ShowHideAdvancedSearch(flag) {
            if (flag) {
                $("[id$=tbladvancedSearchEntry]").show();
                $("[id$=imbShowFilterEntry]").hide();
                $("[id$=imbHideFilterEntry]").show();
            }
            else {
                $("[id$=tbladvancedSearchEntry]").hide();
                $("[id$=imbShowFilterEntry]").show();
                $("[id$=imbHideFilterEntry]").hide();
            }
            return false;
        }
        function ShowHideAdvancedFilter(flag) {
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
        //For finding and removing duplicate and other group validation controls
        //Array of present validations

        var validationArrayGroup;

        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
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
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            //Traversing from bottom through all the validation controls in the page
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        //checks if the control is already in the validation array
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            //insert new conrol to the Array of present validations
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        //remove if control is already in Array of present validations
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                    //remove control if not in group
                    else {
                        //Page_Validators.splice(i, 1);
                    }
                }
            }
        }
        //For checking if validation control in Array of present validations
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
        } 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlExpenses">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table runat="server" ID="tblButton">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$ resources:Controls,Save%>"
                                            ToolTip="<%$ resources:Controls,Save%>" OnClientClick="javascript:ValidatePageNow('Save')"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" TabIndex="22" OnClick="ActionHandler" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$Resources:Controls,Delete%>"
                                            ToolTip="<%$Resources:Controls,Delete%>" OnClientClick="return ShowDeleteConfirm(this);"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" TabIndex="23" OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCanel" Text="<%$ resources:Controls,Cancel%>" ToolTip="<%$ resources:Controls,Cancel%>"
                                            CommandName="CANCEL" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            TabIndex="24" OnClick="ActionHandler" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="4" ID="btnNew" CommandName="NEW" Text="<%$resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$resources:Controls,New %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="5" ID="btnEdit" CommandName="EDIT" Text="<%$resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$resources:Controls,Edit %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="5" ID="btnView" CommandName="VIEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,View %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                            ToolTip="<%$resources:Controls,View %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating">
                    <%--Container for List and Detail tabs--%>
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" TabIndex="1" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" TabIndex="1" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse" id="divAdvanceSearch" style="margin-top: 0px;">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedFilter(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>"
                                                TabIndex="6" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedFilter();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>"
                                                TabIndex="6" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-devide" id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 div-separatn">
                                            <asp:Label runat="server" ID="lblFilterFromDate" Text="<%$ resources:FilterFromDate%>"
                                                AssociatedControlID="txtFilterFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterFromDate" CssClass="input-small margnbotm0"
                                                TabIndex="7" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterFromDate" runat="server" Value="" />
                                            <asp:Label runat="server" ID="lblFilterToDate" Text="<%$ resources:FilterToDate%>"
                                                AssociatedControlID="txtFilterToDate" CssClass="lbl-24perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterToDate" CssClass="date-picker input-small margnbotm0"
                                                TabIndex="7" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFilterToDate" runat="server" Value="" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7 div-separatn">
                                            <asp:Label ID="lblFilterName" runat="server" Text="<%$ resources:FilterName%>" AssociatedControlID="txtFilterName"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterName" TabIndex="7" CssClass="input-small margnbotm0"></asp:TextBox>
                                            <asp:Label ID="lblFilterType" runat="server" Text="<%$ resources:FilterType%>" AssociatedControlID="ddlFilterType"
                                                CssClass="lbl-11-7perc"></asp:Label>
                                            <asp:DropDownList ID="ddlFilterType" runat="server" TabIndex="7" CssClass="select-medium margnbotm0">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="8"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="9" OnClick="ActionHandler"
                                                CommandName="CLEARSEARCH" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="grdTable">
                                <asp:GridView runat="server" ID="grdList" Width="100%" AllowPaging="false" CssClass="grdTable"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" TabIndex="3"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfAppraisalPk" Value='<%# Eval("EIH_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxNo %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTransactNo" Text='<%# Eval("EIH_NO") != "" ? Eval("EIH_NO") : "[NEW]" %>'
                                                    ToolTip='<%# Eval("EIH_NO") != "" ? Eval("EIH_NO") : "[NEW]" %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxDate %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTransactDate" Text='<%# Eval("EIH_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("EIH_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" Wrap="false" />
                                            <HeaderStyle Wrap="false" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:TrxName %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblTransactName" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("EIH_TRN_NAME"),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Eval("EIH_TRN_NAME").ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="25%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ resources:Employee %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEmpName" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("EIH_EMPLOYEE_TEXT"),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Eval("EIH_EMPLOYEE_TEXT").ToString())%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="28%" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="<%$ resources:Type %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblType" Text='<%# Eval("EIH_TYPE_TEXT") %>' ToolTip='<%# Eval("EIH_TYPE_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:EffFrom %>">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="lblEffectiveDate" Text='<%# Eval("EIH_EFFECT_DATE", Resources.Constants.HRMSDateFormatGrid) %>'
                                                    ToolTip='<%# Eval("EIH_EFFECT_DATE", Resources.Constants.HRMSDateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                                <div class="clear">
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <%--EntryPage Table Row--%>
                        <asp:TableCell>
                            <div class="search-colapse" id="divAdvanceSearchEntry" style="margin-top: 0px;">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilterEntry" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter %>"
                                                TabIndex="6" />
                                            <asp:ImageButton runat="server" ID="imbHideFilterEntry" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter %>"
                                                TabIndex="7" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table class="table-devide" id="tbladvancedSearchEntry" style="background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7 div-separatn">
                                            <asp:Label runat="server" ID="lblBranchLoaction" Text="<%$ resources:BranchLoaction %>"
                                                AssociatedControlID="ddlBranchLoaction"></asp:Label>
                                            <asp:DropDownList ID="ddlBranchLoaction" runat="server" TabIndex="8" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true" CssClass="select-small-a1">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblEmploymentType" Text="<%$ resources:EmploymentType %>"
                                                AssociatedControlID="ddlEmploymentType" CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:DropDownList ID="ddlEmploymentType" runat="server" TabIndex="9" OnSelectedIndexChanged="ActionHandler"
                                                AutoPostBack="true" CssClass="select-small-a1">
                                            </asp:DropDownList>
                                        </div>
                                    </td>
                                    <td class="div-separatn">
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblTransactionNo" Text="<%$ resources:TransactionNo %>"
                                                AssociatedControlID="txtTransactionNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTransactionNo" Enabled="false" CssClass="input-disabled input-small"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblTransactionDate" Text="<%$ resources:TransactionDate%>"
                                                AssociatedControlID="txtTransactionDate" CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTransactionDate" CssClass="input-small"
                                                TabIndex="10" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="vrfTransactionDate" ControlToValidate="txtTransactionDate"
                                                Text="*" ErrorMessage="<%$ Resources:Msg_SelectTransactionDate %>" CssClass="star"
                                                Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblEmployee" Text="<%$ resources:Employee %>" AssociatedControlID="txtEmployee"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtEmployee" CssClass="select-half" TabIndex="12"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdfEmployee" Value="" />
                                            <div style="display: none;">
                                                <asp:Button runat="server" ID="btnEmployeeSelect" CommandName="AFTERAUTOSELECT" OnClick="ActionHandler" /></div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblTransactName" Text="<%$ resources:TransactName %>"
                                                AssociatedControlID="txtTransactName" ></asp:Label>
                                            <asp:TextBox runat="server" ID="txtTransactName" CssClass="select-half" TabIndex="11"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblType" Text="<%$ resources:Type%>" AssociatedControlID="ddlType"
                                                ></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlType" CssClass="select-small-a" TabIndex="13">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="vrfType" ControlToValidate="ddlType"
                                                Text="*" ErrorMessage="<%$ Resources:Msg_SelectType %>" InitialValue="-1" CssClass="star"
                                                Display="Static" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            <asp:Label runat="server" ID="lblEffectiveFrom" Text="<%$ resources:EffectiveFrom%>"
                                                AssociatedControlID="txtEffectiveFrom" CssClass="middle-lbl-a0"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtEffectiveFrom" CssClass="input-small"
                                                TabIndex="14" onkeydown="return CheckKey(event)" MaxLength="11" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="vrfEffectiveFrom" ControlToValidate="txtEffectiveFrom"
                                                Text="*" ErrorMessage="<%$ Resources:Msg_SelectEffectiveFrom %>" CssClass="star"
                                                Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <h3>
                                <%=   GetLocalResourceObject("DetlsHdr").ToString() %></h3>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblIncrementOn" Text="<%$ resources:IncrementOn %>"
                                                AssociatedControlID="ddlPayElement"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlPayElement" CssClass="select-half-a" TabIndex="15">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator runat="server" ID="vrfPayElement" ControlToValidate="ddlPayElement"
                                                InitialValue="-1" Text="*" ErrorMessage="<%$ Resources:Msg_SelectIncrementOn %>"
                                                CssClass="star" Display="Static" ValidationGroup="AddToList"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator runat="server" ID="vrfPayElementPopup" ControlToValidate="ddlPayElement"
                                                InitialValue="-1" Text="*" ErrorMessage="<%$ Resources:Msg_SelectIncrementOn %>"
                                                CssClass="star" Display="Static" ValidationGroup="Formula"></asp:RequiredFieldValidator>
                                                <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblAmountOrFormula" Text="<%$ resources:Formula %>"
                                                AssociatedControlID="txtAmountOrFormula" ></asp:Label>
                                            <asp:TextBox runat="server" ID="txtAmountOrFormula" Enabled="false" CssClass="input-disabled input-half"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFormula" runat="server" Value="" />
                                            <asp:RequiredFieldValidator runat="server" ID="vrftxtProductTextPopup" ControlToValidate="txtAmountOrFormula"
                                                Text="*" ErrorMessage="<%$ Resources:Msg_NeedFormula %>" CssClass="star" Display="Dynamic"
                                                ValidationGroup="AddToList"></asp:RequiredFieldValidator>
                                            <asp:ImageButton ID="imgFromula" runat="server" CommandName="SHOWPOPUP" SkinID="salary-formula"
                                                OnClientClick="javascript:ValidatePageNow('Formula')" OnClick="ActionHandler"
                                                ToolTip="<%$ resources:ApplyFormula %>" TabIndex="16" CssClass="margntop2" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblRemarks" Text="<%$ resources:Remarks %>" 
                                                AssociatedControlID="txtRemarks"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtRemarks" CssClass="input-half multiline-3line" TextMode="MultiLine"
                                                TabIndex="17"></asp:TextBox>
                                            <asp:ImageButton ID="imgAddNewDetails" runat="server" CommandName="ADD" OnClick="ActionHandler"
                                                SkinID="imbaddnew" OnClientClick="javascript:ValidatePageNow('AddToList')" TabIndex="18"
                                                CssClass="margntop29" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView ID="grdIncrementDetails" runat="server" AutoGenerateColumns="False"
                                    Width="100%" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable" AllowSorting="false">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblMsgEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ resources:IncrementOn %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIncrementOnText" runat="server" Text='<%# Eval("IncrementOnText") %>'
                                                    ToolTip='<%# Eval("IncrementOnText") %>'></asp:Label>
                                                <asp:HiddenField ID="hdfIncrementOn" runat="server" Value='<%# Eval("IncrementOn") %>' />
                                                <asp:HiddenField ID="hdfFormula" runat="server" Value='<%# Eval("Formula") %>' />
                                                <asp:HiddenField ID="hdfDetPk" runat="server" Value='<%# Eval("DetPK") %>' />
                                                <asp:HiddenField ID="hdfMinAmount" runat="server" Value='<%# Eval("MinAmount") %>' />
                                                <asp:HiddenField ID="hdfMaxAmount" runat="server" Value='<%# Eval("MaxAmount") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Formula %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFormula" runat="server" Text='<%# Eval("FormulaText") %>' ToolTip='<%# Eval("FormulaText").ToString() + ((Convert.ToDecimal(Eval("MinAmount")) > 0 || Convert.ToDecimal(Eval("MaxAmount")) > 0)? ("(" + Resources.Controls.Min.ToString() + GetFormattedCurrencyWithComma(Eval("MinAmount")) +", "+ Resources.Controls.Max.ToString() + GetFormattedCurrencyWithComma(Eval("MaxAmount"))+")") : "") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Remarks %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("Remarks").ToString(),45) %>' ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Eval("Remarks").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Action %>">
                                            <ItemTemplate>
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_edit" runat="server" ID="imbEditDetails"
                                                    SkinID="imbeditgrid" EnableViewState="false" CommandName="ITEMGRIDEDIT" OnClick="ActionHandler"
                                                    TabIndex="19" ToolTip="<%$ resources:Controls,Edit %>" />
                                                <asp:ImageButton Width="16px" Height="16px" CssClass="_delete" runat="server" ID="imbDeleteDetails"
                                                    OnClick="ActionHandler" SkinID="imbdeletegrid" EnableViewState="false" CommandName="DELETE_ACTION"
                                                    OnClientClick="return ShowDeleteConfirm(this);" TabIndex="19" ToolTip="<%$ resources:Controls,Delete %>" />
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" Wrap="false" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div id="divEmployee" runat="server" visible="false">
                                <h3>
                                    <%=  GetLocalResourceObject("EmployeeHdr").ToString()  %></h3>
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblCurrDepartment" Text="<%$ resources:CurrDepartment %>"
                                                    AssociatedControlID="txtCurrDepartment"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCurrDepartment" Enabled="false" CssClass="input-disabled input-small"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblDepartment" Text="<%$ resources:Department %>" AssociatedControlID="ddlDepartment"
                                                CssClass="middle-lbl-small" ></asp:Label>
                                                <asp:DropDownList ID="ddlDepartment" runat="server" TabIndex="20" CssClass="select-small-c">
                                                </asp:DropDownList>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblCurrDesignation" Text="<%$ resources:CurrDesignation %>"
                                                    AssociatedControlID="txtCurrDesignation"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCurrDesignation" Enabled="false" CssClass="input-disabled input-small-b"></asp:TextBox>
                                                <asp:Label runat="server" ID="lblDesignation" Text="<%$ resources:Designation %>"
                                                    AssociatedControlID="ddlDesignation" CssClass="lbl-13-3perc"></asp:Label>
                                                <asp:DropDownList ID="ddlDesignation" runat="server" TabIndex="21" CssClass="select-small-c">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <ucgti:EmpSalaryControl id="UCEmpSalary" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="divFormulaPopUp" style="display: none">
                <uc1:PopUp ID="ucFormulaMaster" runat="server" AfterApply="ucPopUpFormula_AfterApply"
                    IsSlab="0" ShowMinMaxAmount="1" PopupDivId="divFormulaPopUp" HeaderText="<%$ resources:IncrementOn %>" />
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="AddToList" runat="server" />
                <asp:ValidationSummary ID="vsFormula" ValidationGroup="Formula" runat="server" />
                <asp:ValidationSummary ID="vsSave" ValidationGroup="Save" runat="server" />
            </div>

            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
