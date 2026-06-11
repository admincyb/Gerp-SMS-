<%@ Page Title="<%$ Resources:Captions,Title_Budget %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="Budget.aspx.cs" Inherits="ERPSMS_v01.Finance.Administration.Masters.Budget"
    Theme="ClassicExt" ValidateRequest="false" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/NumericControl.ascx" TagName="NumericControl" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="../../../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .grid-rowcolor { background: #d2d6d3 !important; }
        .div2col-s-span { background: none !important; padding: 0 !important; border: none !important; }
        .textalignRight { text-align: right !important; }
    </style>
    <script type="text/javascript">

        var pageURL = window.document.URL;
        var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitAmountControl() {
            $("[id*=txtBudget]").ForceNumersOnly();
        }

        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtBudgetNumber", url, "hdfBudgetNumber", true, true, "GETBUDGETNO");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCostCenter", url + "?Group=0", "hdnBadgetCostCenter", true, true, "COSTCENTER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAccountNo", url + "&SearchBy=COA_CODE", "hdnBadgetAccNo", true, true, "GETCOAPARENT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAccountName", url + "&SearchBy=COA_NAME_ONLY", "hdnBadgetAccName", true, true, "GETCOAPARENT");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCostCenterSrch", url +"?Group=0" + "&BudgetPk="+$("[id$='hdfCurrBudgetPk']").val(), "hdfCostCenterSrch", true, true, "BUDGETCOSTCENTER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtAccNoSrch", url + "&SearchBy=COA_CODE"+ "&BudgetPk="+$("[id$='hdfCurrBudgetPk']").val(), "hdfAccNoSrch", true, true, "GETBUDGETCOAPARENT"); // Changed to Make Account search Data same as AccountNo
        }

        function ShowListing(flag) {
            if (flag) {
                $("[id$=PageAction_List]").show();
                $("[id$=PageAction_Entry]").hide();
            }
            else {
                $("[id$=PageAction_List]").hide();
                $("[id$=PageAction_Entry]").show();
            }
            return false;
        }

        function PageViewMode(mode) {
            //Mode = 1 Indicates its on View Mode
            //Mode = 2 Indicates its on New Mode
            //Mode = 3 Indicates its on Amend Mode
            if (mode == 1) {
                $("[id$=btnAdd]").hide();
                $("[id$=btnSaveSubmit]").hide();
                $("[id$=btnAmendSubmit]").hide();
                $("[id$=btnAmendSave]").hide();
                $("[id$=btnSave]").hide();
                $("[id$=btnClose]").show();
                $("[id$=ImportTd]").hide();
                $("[id$=btnAddList]").hide();
                $("[id$=BudgetDtlAdvSerch]").show();
                $("[id$=BudgetDetailsAdd]").hide();
                $("[id$=BudgetDetailsAddHd]").hide();
            }
            else if (mode == 2) {
                $("[id$=btnAdd]").hide();
                $("[id$=btnSaveSubmit]").show();
                $("[id$=btnAmendSubmit]").hide();
                $("[id$=btnAmendSave]").hide();
                $("[id$=btnSave]").show();
                $("[id$=btnClose]").show();
                $("[id$=ImportTd]").show();
                $("[id$=BudgetDtlAdvSerch]").hide();
                $("[id$=BudgetDetailsAdd]").show();
                $("[id$=BudgetDetailsAddHd]").show();
            }
            else if (mode == 3) {
                $("[id$=btnAdd]").hide();
                $("[id$=btnSaveSubmit]").hide();
                $("[id$=btnAmendSave]").show();
                $("[id$=btnAmendSubmit]").show();
                $("[id$=btnSave]").hide();
                $("[id$=btnClose]").show();
                $("[id$=ImportTd]").show();
                $("[id$=BudgetDtlAdvSerch]").show();
                $("[id$=BudgetDetailsAdd]").show();
                $("[id$=BudgetDetailsAddHd]").show();
            }
            else if (mode == 4) {
                $("[id$=btnAdd]").hide();
                $("[id$=btnSaveSubmit]").show();
                $("[id$=btnAmendSubmit]").hide();
                $("[id$=btnAmendSave]").hide();
                $("[id$=btnSave]").show();
                $("[id$=btnClose]").show();
                $("[id$=ImportTd]").hide();
                $("[id$=BudgetDtlAdvSerch]").hide();
                $("[id$=BudgetDetailsAdd]").show();
                $("[id$=BudgetDetailsAddHd]").show();
            }
            else {
                $("[id$=btnAmendSubmit]").hide();
                $("[id$=btnAmendSave]").hide();
                $("[id$=btnSaveSubmit]").hide();
                $("[id$=btnSave]").hide();
                $("[id$=btnClose]").hide();
                $("[id$=ImportTd]").hide();
                $("[id$=BudgetDtlAdvSerch]").hide();
                $("[id$=BudgetDetailsAdd]").hide();
                $("[id$=BudgetDetailsAddHd]").hide();
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

        function ShowHideAdvancedDtlSearch(flag) {
            if (flag) {
                $("[id$=tbladvancedDtlSearch]").show();
                $("[id$=ImgShowBtnDtl]").hide();
                $("[id$=ImgHideBtnDtl]").show();
            }
            else {
                $("[id$=tbladvancedDtlSearch]").hide();
                $("[id$=ImgShowBtnDtl]").show();
                $("[id$=ImgHideBtnDtl]").hide();
            }
            return false;
        }



        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                CheckValidationDuplicate(valGroup);
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html());
                return false;
            }
            else {
                return true;
            }
        }
        function CheckValidationDuplicate(valGroup) {
            validationArrayGroup = new Array();
            for (var i = Page_Validators.length - 1; i >= 0; i--) {
                if (typeof (Page_Validators[i].validationGroup) == "string") {
                    if (valGroup == Page_Validators[i].validationGroup) {
                        if (!CheckValidationExists(Page_Validators[i].id)) {
                            validationArrayGroup.push(Page_Validators[i].id);
                        }
                        else {
                            Page_Validators.splice(i, 1);
                        }
                    }
                    else {
                        Page_Validators.splice(i, 1);
                    }
                }
            }
        }
        function CheckValidationExists(id) {
            for (var i in validationArrayGroup) {
                if (validationArrayGroup[i] == id) {
                    return true;
                }
            }
            return false;
        }

        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtAccountNo") {
                $("[id$='btnAccountNo']").click();
            }
            if (targetControlID == "txtAccountName") {
                $("[id$='btnAccountName']").click();
            }
        }
        function AfterInvalidSelect(targetControlID) {
        }
        function InitAmountControl() {
            $("[id*=txtAmendAmount]").ForceNumersOnly();
        }
    </script>
    <style type="text/css">
  
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlExpenses">
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
                                <ul runat="server" id="pnlListing">
                                    <li>
                                        <asp:Button runat="server" TabIndex="2" ID="btnAmendSubmit" CommandName="AMENDSUBMIT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,AmendSubmit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-amend"
                                            ToolTip="<%$resources:Controls,AmendSubmit %>" />
                                    </li>
                                    <%--                                    <li>
                                        <asp:Button runat="server" TabIndex="2" ID="btnAmendSave" CommandName="SAVE" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-amend"
                                            ToolTip="<%$resources:Controls,Save %>" />
                                    </li>--%>
                                    <li>
                                        <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                            CommandName="NEW" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Add %>"
                                            TabIndex="11" />
                                    </li>
                                    <li>
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="1"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="return ValidatePageNow('Save');"
                                            ValidationGroup="Save" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <%--                                    <li>
                                        <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                            TabIndex="30" EnableViewState="False" ToolTip="<%$resources:ErpRes,Save %>"
                                            CommandName="SAVE" OnClick="ActionHandler" />
                                    </li>--%>
                                    <li>
                                        <asp:Button ID="btnClose" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                            EnableViewState="False" ToolTip="<%$resources:ErpRes,Cancel %>" CommandName="CANCEL" OnClick="ActionHandler"
                                            TabIndex="32" />
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
                                CommandArgument="SEC_ActionPanel" TabIndex="79" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="80" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
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
                                                TabIndex="45" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="45" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide " id="tbladvancedSearch">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <%-- <asp:Label ID="lblBudgetNoSrch" runat="server" Text="<%$resources:BudgetlblNo %>" CssClass="lbl-11-3perc" AssociatedControlID="txtBudgetNumber"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBudgetNumber" TabIndex="9" CssClass="input-small margnbotm0">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfBudgetNumber" runat="server" Value="0" />--%>
                                            <asp:Label ID="lblFinYearSrch" runat="server" Text="<%$resources:FinYear %>" CssClass="lbl-11-3perc" AssociatedControlID="ddlFinYearSrch"></asp:Label>
                                            <asp:DropDownList ID="ddlFinYearSrch" runat="server" CssClass="medium3 margnbotm0" TabIndex="4">
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="imgSearch" runat="server" Text="" ToolTip="<%$resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="6" CommandName="SEARCH" SkinID="search-ext"
                                                Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                            <asp:ImageButton ID="imgClear" runat="server" Text="" ToolTip="<%$resources:Controls,Clear %>"
                                                TabIndex="7" OnClick="ActionHandler" CommandName="CLEAR" CssClass="margntop2"
                                                SkinID="clear-ext" Style="margin-bottom: 0px!important;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdBudgetList" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="emptytable"
                                    Width="100%" OnRowDataBound="ActionHandler" PageSize="<%$ resources:PageSize %>"
                                    OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdfBudgetPK" Value='<%# Eval("BGH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfBudgetStatus" Value='<%# Eval("BGH_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfBudgetType" Value='<%# Eval("BGH_TYPE") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <%--                                        <asp:TemplateField HeaderText="<%$ resources:BudgetListNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgetListNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.BudgetListNo)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BudgetListNo)%>'></asp:Label>      
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="<%$ resources:Year %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgetYear" runat="server" Text='<%# Eval(Resources.DataFieldRes.BudgetYear)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BudgetYear)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:BudgetAmount %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgetAmount" runat="server" Text='<%# Eval(Resources.DataFieldRes.BudgetAmount,"{0:N0}")  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BudgetAmount,"{0:N0}")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Plant %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgetPlant" runat="server" Text='<%# Eval(Resources.DataFieldRes.BudgetPlantName) %>'
                                                    ToolTip='<%#Eval(Resources.DataFieldRes.BudgetPlantName)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgetStatusText" runat="server" Text='<%# Eval(Resources.DataFieldRes.BudgetStatusText)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BudgetStatusText)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Actions %>" HeaderStyle-CssClass="txt-center">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbAmmend" ToolTip="<%$resources:ErpRes,Ammend %>"
                                                    TabIndex="10" SkinID="imbamendgrid" CommandName="AMMEND" />
<%--                                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$resources:ErpRes,Edit %>"
                                                    TabIndex="10" SkinID="imbeditgrid" CommandName="PERFORMACTION" />--%>
                                                <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$resources:ErpRes,View %>"
                                                    TabIndex="10" SkinID="btnview" CommandName="VIEW" />
<%--                                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:ErpRes,Delete %>"
                                                    TabIndex="10" SkinID="imbdeletegrid" CommandName="DELETEBUDGET" OnClientClick="return ShowDeleteConfirm(this);" />--%>
                                                <asp:ImageButton runat="server" ID="imbHistory" ToolTip="<%$resources:ErpRes,ShowHistory %>"
                                                    TabIndex="7" SkinID="history" CommandName="HISTORYHDR" />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <%--                                            <asp:Label ID="lblBdgNo" runat="server" Text="<%$ resources:BudgetNo%>" AssociatedControlID="lblBudgetNo" CssClass="lbl-11-3perc"></asp:Label>
                                            <asp:Label runat="server" ID="lblBudgetNo" CssClass="input-small"></asp:Label>
                                            <asp:HiddenField ID="hdfBudgetNo" runat="server" Value="" />--%>
                                            <asp:Label runat="server" ID="lblFinYear" Text="<%$ resources:FinYear%>" AssociatedControlID="ddlFinYear"
                                                CssClass="lbl-11-8perc"></asp:Label>
                                            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlFinYear" CssClass="medium2 margnbotm0" TabIndex="8" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfFinYear" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="Budget" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="ddlFinYear" ErrorMessage="<%$ resources:Err_FinYear %>" />
                                            <asp:Label ID="lblchkForcast" runat="server" AssociatedControlID="chkForcast"
                                                Text="<%$ resources:ForcastBudget %>" />
                                            <asp:CheckBox runat="server" AutoPostBack="true" ID="chkForcast" CssClass="div2col-s-span" Checked="false" OnCheckedChanged="ActionHandler" TabIndex="9" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <h3 class="fontWGT-Nrml" id="BudgetDetailsAddHd">
                                <%= GetLocalResourceObject("AddDetails").ToString()%>
                            </h3>
                            <table class="table-devide margntop18" id="ImportTd">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:UpdatePanel ID="aupdpnlImport" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label runat="server" ID="lblSourceFile" Text="<%$ resources: SourceFileStar %>"
                                                        AssociatedControlID="fupImport"></asp:Label>
                                                    <div class="fileupload-main">
                                                        <asp:FileUpload ID="fupImport" runat="server" TabIndex="75" CssClass="margnbotm0 margn-rgt0 upload-area3" />
                                                    </div>
                                                    <asp:Button runat="server" ID="btnImport" CommandName="IMPORT" TabIndex="76" Text="<%$resources:Import %>"
                                                        OnClick="ActionHandler" OnClientClick="return ValidatePageNow('Save');"
                                                        ValidationGroup="Save" ToolTip="<%$resources:Import %>" SkinID="btnInner-add"
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
                                            <a id="aTmpDwn" class="btnInner-dwn btnInner-med-size decoration-none margnrgt12-5per" style="margin-right: 73px!important; margin-left: 5px!important;"
                                                href='<%= Page.ResolveClientUrl((string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()) ? "~/Upload/" : System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() )+ "Template/" + GetGlobalResourceObject("ConfigurationsRes", "ImportTemplateBudget").ToString())%>'>
                                                <%= Resources.Controls.Template.ToString() %>
                                            </a>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <h3 class="fontWGT-Nrml"></h3>
                            <table class="table-devide" id="BudgetDetailsAdd">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblMonth" Text="<%$ resources:Month%>" AssociatedControlID="ddlMonth"
                                                CssClass="lbl-19-4perc"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlMonth" CssClass="select-small-c1" TabIndex="10">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfMonth" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="Budget" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="ddlMonth" ErrorMessage="<%$ resources:Err_Month %>" />
                                            <asp:Label runat="server" ID="lblPlant" Text="<%$ resources:Plant%>" AssociatedControlID="ddlPlant"
                                                CssClass="lbl-21-1perc"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlPlant" CssClass="select-small-c1" TabIndex="11">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="vrfPlant" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="Budget" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="ddlPlant" ErrorMessage="<%$ resources:Err_Plant %>" />
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblAccountNo" Text="<%$ resources:AccountNo%>" AssociatedControlID="txtAccountNo"
                                                CssClass="lbl-16perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtAccountNo" CssClass="select-small-c1" TabIndex="12"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfAccountNo" CssClass="star" SetFocusOnError="true"
                                                InitialValue="Select/Type" ValidationGroup="Budget" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtAccountNo" ErrorMessage="<%$ resources:Err_AccountNumber %>" />
                                            <asp:HiddenField runat="server" ID="hdnBadgetAccNo" Value="0" />
                                            <asp:Button ID="btnAccountNo" runat="server" EnableTheming="false" Style="display: none"
                                                OnClick="ActionHandler" CommandName="ACCNOCHANGE" />
                                            <asp:Label runat="server" ID="lblAccountName" Text="<%$ resources:AccountName%>" AssociatedControlID="txtAccountName"
                                                CssClass="lbl-16perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtAccountName" CssClass="select-small-c1" TabIndex="13"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfAccountName" CssClass="star" SetFocusOnError="true"
                                                InitialValue="Select/Type" ValidationGroup="Budget" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtAccountName" ErrorMessage="<%$ resources:Err_AccountName %>" />
                                            <asp:HiddenField runat="server" ID="hdnBadgetAccName" Value="0" />
                                            <asp:Button ID="btnAccountName" runat="server" EnableTheming="false" Style="display: none"
                                                OnClick="ActionHandler" CommandName="ACCNAMECHANGE" />
                                        </div>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblCostCenter" Text="<%$ resources:CostCenter%>" AssociatedControlID="txtCostCenter"
                                                CssClass="lbl-19-4perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCostCenter" CssClass="select-small-c1" TabIndex="14"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfCostCenter" CssClass="star" SetFocusOnError="true"
                                                InitialValue="Select/Type" ValidationGroup="Budget" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtCostCenter" ErrorMessage="<%$ resources:Err_CostCenter %>" />
                                            <asp:HiddenField runat="server" ID="hdnBadgetCostCenter" Value="0" />
                                            <asp:Label runat="server" ID="lblBudget" Text="<%$ resources:Budget%>" AssociatedControlID="txtBudget"
                                                CssClass="lbl-20-5perc"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBudget" CssClass="input-small-c" TabIndex="15" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfBudget" CssClass="star" SetFocusOnError="true"
                                                InitialValue="" ValidationGroup="Budget" EnableClientScript="true" runat="server"
                                                Display="Dynamic" Text="*" ControlToValidate="txtBudget" ErrorMessage="<%$ resources:Err_Budget %>" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S txt-rgt">
                                            <asp:Button Text="<%$ resources:AddToList %>" runat="server" ID="btnAddList"
                                                ToolTip="<%$ resources:AddToList %>" TabIndex="16" CommandName="ADDLIST"
                                                SkinID="btnInner-add" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('Budget');" CommandArgument="PageAction_Entry"
                                                Style="margin-right: 73px!important; margin-left: 5px!important;" />
                                        </div>
                                    </td>
                                </tr>
                            </table>                     
                            <div id="BudgetDtlAdvSerch" style="display: none">
                                <div class="search-colapse">
                                    <table>
                                        <tr>
                                            <td>
                                                <h1>
                                                    <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                            </td>
                                            <td>
                                                <asp:ImageButton runat="server" ID="ImgShowBtnDtl" OnClientClick="javascript:return ShowHideAdvancedDtlSearch(1);"
                                                    ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                    TabIndex="45" />
                                                <asp:ImageButton runat="server" ID="ImgHideBtnDtl" OnClientClick="javascript:return ShowHideAdvancedDtlSearch();"
                                                    ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                    TabIndex="45" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%--------------colpase btn----------%>
                                <div class="clear">
                                </div>
                                <table class="table-devide " id="tbladvancedDtlSearch">
                                    <tr>
                                        <td>
                                            <div class="div2col-S div-separatn">
                                                <asp:Label ID="lblPlantSrch" runat="server" Text="<%$resources:Plant %>" CssClass="lbl-4-1perc" AssociatedControlID="ddlPlantSrch"></asp:Label>
                                                <asp:DropDownList runat="server" ID="ddlPlantSrch" CssClass="select-small-ax " TabIndex="10">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblCostCenterSrch" runat="server" Text="<%$resources:CostCenter %>" CssClass="lbl-15-1perc" AssociatedControlID="txtCostCenterSrch"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCostCenterSrch" CssClass="select-small-ax" TabIndex="11"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hdfCostCenterSrch" Value="0" />
                                                <asp:Label ID="lblAccNoSrch" runat="server" Text="<%$resources:AccountNo %>" CssClass="lbl-15-1perc" AssociatedControlID="txtAccNoSrch"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtAccNoSrch" CssClass="select-small-ax" TabIndex="11"></asp:TextBox>
                                                <asp:HiddenField runat="server" ID="hdfAccNoSrch" Value="0" />
                                                <asp:ImageButton ID="imgDtlSearch" runat="server" Text="" ToolTip="<%$resources:Controls,Search %>"
                                                    OnClick="ActionHandler" TabIndex="6" CommandName="SEARCHBUDGET" SkinID="search-ext"
                                                    Style="margin-bottom: 0px!important; margin-top: 2px;" />
                                                <asp:ImageButton ID="imgDtlClear" runat="server" Text="" ToolTip="<%$resources:Controls,Clear %>"
                                                    TabIndex="7" OnClick="ActionHandler" CommandName="CLEARBUDGET" CssClass="margntop2"
                                                    SkinID="clear-ext" Style="margin-bottom: 0px!important;" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdSelectedBudget" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="emptytable"
                                    Width="100%" OnRowDataBound="ActionHandler" PageSize="<%$ resources:PageSize %>" ShowFooter="true"
                                    OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="hdfBudgetDtlPK" Value='<%# Eval("BDG_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfBudgetDtlSlNo" Value='<%# Eval("BDG_SL_NO") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="1%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Plant %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSelBudgetPlantName" runat="server" Text='<%# Eval(Resources.DataFieldRes.BudgetPlantName)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BudgetPlantName)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:CostCenter %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgetCostCenter" runat="server" Text='<%# Eval(Resources.DataFieldRes.BudgetCostCenterName)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BudgetCostCenterName)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AccountNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgeAccountNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.BudgetAccountNo)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BudgetAccountNo)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AccountName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgteAccountName" runat="server" Text='<%# Eval(Resources.DataFieldRes.BudgetAccountName)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BudgetAccountName)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Year/Month %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBudgetYear" runat="server" Text='<%# Eval(Resources.DataFieldRes.DtlBudgetYear)+"-"+Eval(Resources.DataFieldRes.DtlBudgetMonth)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.DtlBudgetYear)%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Budget %>" HeaderStyle-CssClass="textalignRight">
                                            <ItemTemplate>
                                                <%--<asp:Label ID="lblBudgeBudget" runat="server" Text='<%# Eval(Resources.DataFieldRes.BudgetListAmount)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BudgetListAmount)%>'></asp:Label>--%>
                                                <asp:Label ID="lblBudgeBudget" runat="server"
                                                    Text='<%# Eval(Resources.DataFieldRes.BudgetListAmount, "{0:N0}") %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BudgetListAmount, "{0:N0}") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:AmmendAmount %>" HeaderStyle-CssClass="textalignRight">
                                            <ItemTemplate>
                                                <asp:TextBox runat="server" ID="txtAmendAmount" CssClass="numeric input-w70" Text='<%# Eval(Resources.DataFieldRes.BudgetAmendAmount)  %>'
                                                    ToolTip='<%# Eval(Resources.DataFieldRes.BudgetAmendAmount)%>' onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" />
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Actions %>" HeaderStyle-CssClass="txt-center">
                                            <ItemTemplate>
<%--                                                <asp:ImageButton runat="server" ID="imbDtlEdit" ToolTip="<%$resources:ErpRes,Edit %>"
                                                    TabIndex="10" SkinID="imbeditgrid" CommandName="BUDGETDTLEDIT" />--%>
                                                <asp:ImageButton runat="server" ID="imbDtlDelete" ToolTip="<%$resources:ErpRes,Delete %>"
                                                    TabIndex="10" SkinID="imbdeletegrid" CommandName="BUDGETDTLDELET" OnClientClick="return ShowDeleteConfirm(this);" />
                                                <asp:ImageButton runat="server" ID="imbBudgetDtlHistory" ToolTip="<%$resources:ErpRes,ShowHistory %>"
                                                    TabIndex="10" SkinID="history" CommandName="BUDGETDTLHISTORY" />
                                                <asp:ImageButton runat="server" ID="imbBudgetDtlUndo" ToolTip="<%$resources:ErpRes,Undo %>"
                                                    TabIndex="10" SkinID="imbundogrid" CommandName="BUDGETDTLUNDO" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="true" />
                            </div>

                            <asp:Table ID="tblBudget" runat="server" Width="100%" Visible="true">
                                <asp:TableRow>
                                    <asp:TableCell></asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Right">
                                        <b>
                                            <asp:Label ID="Label1" runat="server" Text="Total : "></asp:Label>
                                        </b>
                                        <b>
                                            <asp:Label ID="lblBudgetTotal" runat="server" Text="0.00"></asp:Label>
                                        </b>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>



                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <asp:HiddenField runat="server" ID="hdnTotalBudget" Value="0" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="invoice">
                </uc1:WorkflowUserComments>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="Budget" runat="server" />
                <asp:ValidationSummary ID="vsPageSave" ValidationGroup="Save" runat="server" />
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
                                    <asp:TemplateField HeaderText="<%$ resources:HistoryVersion %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDtlVersion" runat="server" Text='<%# Eval(Resources.DataFieldRes.BudgetVersion)  %>'
                                                ToolTip='<%# Eval(Resources.DataFieldRes.BudgetVersion)%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:HistoryAmount %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDtlVersion" runat="server" Text='<%# Eval(Resources.DataFieldRes.Bdgtamount)  %>'
                                                ToolTip='<%# Eval(Resources.DataFieldRes.Bdgtamount)%>'></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:HistoryModifiedBy %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDtlVersion" runat="server" Text='<%# Eval(Resources.DataFieldRes.ModifiedBy)  %>'
                                                ToolTip='<%# Eval(Resources.DataFieldRes.ModifiedBy)%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:HistoryModifiedDate %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDtlVersion" runat="server" Text='<%# Eval(Resources.DataFieldRes.ModifiedDate)  %>'
                                                ToolTip='<%# Eval(Resources.DataFieldRes.ModifiedDate)%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdfCurrBudgetPk" runat="server" Value="0" />
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
