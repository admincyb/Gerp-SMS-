<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Income_Tax_01.aspx.cs"
    Inherits="HRMS.Payroll.Income_Tax_01" ValidateRequest="false" MasterPageFile="~/ERPSMS_2.Master"
    Theme="ClassicExt" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="System.Web.UI.HtmlControls" %>
<%@ Register Src="~/Employees/UserControls/GtiTabControl.ascx" TagName="GtiTabControl"
    TagPrefix="ucGtiTab" %>
<%@ Register Src="../Payroll/UserControls/PayrollTabControl.ascx" TagName="PayrollTabControl"
    TagPrefix="uc1" %>
<%--Ext Gridview control--%>
<%@ Register Src="UserControls/EmpBasicInfoControl.ascx" TagName="EmpBasicInfoControl"
    TagPrefix="ucBasicHdr" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <script type="text/javascript">
        function InitComponents() {
            $("[id*=txt_ITCol3]").ForceNumericOnly();
            $("[id*=txt_ITCol4]").ForceNumericOnly();
            TaxComputation('B2001-B2009', 'Sec2');
            TaxComputation('C3001-C3027', 'Sec3');
        }
        function ImbHideTaxDetails_Click(divID, imbHide, imbShow) {
            var pnlelement = document.getElementById(divID);
            var imbHideelement = document.getElementById(imbHide);
            var imbShowelement = document.getElementById(imbShow);
            pnlelement.style.display = 'none';
            imbHideelement.style.display = 'none';
            imbShowelement.style.display = 'inline';
            return false;
        }
        function ImbShowTaxDetails_Click(divID, imbHide, imbShow) {
            var pnlelement = document.getElementById(divID);
            var imbHideelement = document.getElementById(imbHide);
            var imbShowelement = document.getElementById(imbShow);
            pnlelement.style.display = 'inline';
            imbHideelement.style.display = 'inline';
            imbShowelement.style.display = 'none';
            return false;
        }

        function ShowListing(flag) {

            if (flag) {
                $("[id$=div_tabcontainerList]").addClass('tab-container-floating');
                $("[id$=divFixedBtns]").removeClass('fixed-buttons');
                $("[id$=divFixedBtns]").addClass('fixed-buttons');
                $("[id$='divPayrollHeader']").show();
                $("[id$='div_hrmsTab']").hide();
                $("[id$='divUCempBasicHdr']").hide();
                $("[id$='div_payrollTab']").show();
            }
            else {
                $("[id$=div_tabcontainerList]").addClass('tab-container-floating visible-hidden');
                $("[id$=divFixedBtns]").removeClass('fixed-buttons');
                $("[id$=divFixedBtns]").addClass('fixed-buttons-normal');
                $("[id$='divPayrollHeader']").hide();
                $("[id$='div_hrmsTab']").show();
                $("[id$='divUCempBasicHdr']").show();
                $("[id$='div_payrollTab']").hide();
            }
            return false;
        }

        //For finding and removing duplicate and other group validation controls
        //Array of present validations
        var validationArrayGroup;
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
                        Page_Validators.splice(i, 1);
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
        function ValidatePageNow(valGroup) {
            if (typeof (Page_ClientValidate) == 'function') {
                //For finding and removing duplicate and other group validation controls
                CheckValidationDuplicate(valGroup);
                //For Script validating the Page
                Page_ClientValidate(valGroup);
            }
            if (!Page_IsValid) {
                $("[id$=litErrorMsg]").hide();
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                RemoveTextboxDisable();     // To Enable Disabled textbox for getting values
                return true;
            }
        }

        function RemoveTextboxDisable() {
            $("#[id*=rprIncomeTax] input[type=text][id*=txt_ITCol3]").each(function (index) {
                if ($(this).closest('tr').find("[id*=txt_ITCol3]").length > 0 && !isNaN(parseFloat($(this).closest('tr').find("[id*=txt_ITCol3]").val()))) {
                    var isDisabled = $(this).closest('tr').find("[id*=txt_ITCol3]").attr('disabled');
                    if (isDisabled == true) { $(this).closest('tr').find("[id*=txt_ITCol3]").removeAttr('disabled'); }
                }
            });
        }

        function TaxComputation(funName, sectionId) {
            var arrayFunIdIndex = -1;
            var arrayFunId, arrayFunExpr, arrayExcludeId, arraySumCopyId;

            switch (sectionId) {
                case 'Sec1':
                    arrayFunId = $("[id*=hdfSec1Col3FunId]").val().split('#');              // get function names
                    arrayFunExpr = $("[id*=hdfSec1Col3FunExpression]").val().split('#');    // get expressions
                    arrayExcludeId = $("[id*=hdfSec1Col3ExcludeId]").val().split('#');      // get total field
                    arraySumCopyId = $("[id*=hdfSec1Col3SumCopyId]").val().split('#');      // get calculated value copy into another field
                    break;
                case 'Sec2':
                    arrayFunId = $("[id*=hdfSec2Col3FunId]").val().split('#');
                    arrayFunExpr = $("[id*=hdfSec2Col3FunExpression]").val().split('#');
                    arrayExcludeId = $("[id*=hdfSec2Col3ExcludeId]").val().split('#');
                    arraySumCopyId = $("[id*=hdfSec2Col3SumCopyId]").val().split('#');
                    break;
                case 'Sec3':
                    arrayFunId = $("[id*=hdfSec3Col3FunId]").val().split('#');
                    arrayFunExpr = $("[id*=hdfSec3Col3FunExpression]").val().split('#');
                    arrayExcludeId = $("[id*=hdfSec3Col3ExcludeId]").val().split('#');
                    arraySumCopyId = $("[id*=hdfSec3Col3SumCopyId]").val().split('#');
                    break;
            }

            arrayFunIdIndex = GetArrayIndex(arrayFunId, funName);
            if (arrayFunIdIndex > -1) {
                for (arrayFunIdIndex; arrayFunIdIndex < arrayFunId.length; arrayFunIdIndex++) {     // calculation run based on function index
                    var operators = GetOperators(arrayFunExpr[arrayFunIdIndex]);                    // get all operators
                    var dbIds = GetDbIds(arrayFunExpr[arrayFunIdIndex]);                            // get all DbId's
                    var ExcludeDBId = GetDbIds(arrayExcludeId[arrayFunIdIndex]);                    // get targer DbId
                    if (arraySumCopyId != "")
                        var CopyDbId = GetDbIds(arraySumCopyId[arrayFunIdIndex]);
                    var DbIndex = -1;
                    var result = 0.0;

                    $("#[id*=rprIncomeTax] input[type=text][id*=txt_ITCol3]").each(function (index) {
                        DbIndex = GetArrayIndex(dbIds, $(this).closest('tr').find("[id*=hdfItemDbId]").val());
                        if ($(this).closest('tr').find("[id*=txt_ITCol3]").length > 0 && !isNaN(parseFloat($(this).closest('tr').find("[id*=txt_ITCol3]").val())) && DbIndex > -1) {
                            if (DbIndex == 0)
                                result = parseFloat($(this).closest('tr').find("[id*=txt_ITCol3]").val());
                            else
                                result = GetResult(operators[DbIndex - 1], result, parseFloat($(this).closest('tr').find("[id*=txt_ITCol3]").val()));
                        }
                        else if ($(this).closest('tr').find("[id*=hdfItemDbId]").val() == ExcludeDBId[0]) {
                            $(this).closest('tr').find("[id*=txt_ITCol3]").val(result);
                            return false;
                        }
                    });

                    // To Copy Calaculated value into another field
                    if (sectionId != 'Sec1' && arraySumCopyId != "") {
                        $("#[id*=rprIncomeTax] input[type=text][id*=txt_ITCol3]").each(function (index) {
                            if ($(this).closest('tr').find("[id*=hdfItemDbId]").val() == CopyDbId[0]) {
                                $(this).closest('tr').find("[id*=txt_ITCol3]").val(result);
                                return false;
                            }
                        });
                        var tempArrayFunId = $("[id*=hdfSec1Col3FunId]").val().split('#');
                        TaxComputation(tempArrayFunId[0], 'Sec1');                              // run again first section after copy 
                    }

                }
            }
        }

        // To Get Calculated values
        function GetResult(operator, preResult, curValue) {
            var result = 0;
            switch (operator) {
                case '+':
                    result = preResult + curValue; return result; break;
                case '-':
                    result = preResult - curValue; return result; break;
                case '*':
                    result = preResult * curValue; return result; break;
                case '/':
                    result = preResult / curValue; return result; break;
                case '>':
                    if (preResult > curValue)
                        result = preResult - curValue;
                    else
                        result = 0;
                    return result; break;
            }
        }

        function GetOperators(IncludeExp) {
            if (IncludeExp.length > 0) {
                var array = IncludeExp.split(',');
                var result = [];
                for (var index = 1; index < array.length; index++) {
                    if (isOperator(array[index]))
                        result.push(array[index]);
                }
            }
            return result;
        }

        function GetDbIds(IncludeExp) {
            if (IncludeExp.length > 0) {
                var array = IncludeExp.split(',');
                var result = [];
                for (var index = 1; index < array.length; index++) {
                    if (!isOperator(array[index]))
                        result.push(array[index]);
                }
            }
            return result;
        }
        function isOperator(value) {
            var result = false;
            switch (value) {
                case "+":
                case "-":
                case "*":
                case "/":
                case ">":
                    result = true;
                    break;
            }
            return result;
        }

        // Checking array existance
        function GetArrayIndex(array, arrayValue) {
            var arrayIndex = -1;
            for (var index = 0; index < array.length; index++) {
                if (array[index].indexOf(arrayValue) > -1) {
                    arrayIndex = index;
                    return arrayIndex;
                }
            }
            return arrayIndex;
        }

        $("[id*=chkDefualtValue]").live("click", function () {
            if (!$(this).is(":checked")) {
                $("td", $(this).closest("tr")).find("input:text[id*=txt_ITCol3]").val('');
            } else {
                $("td", $(this).closest("tr")).find("input:text[id*=txt_ITCol3]").val($("td", $(this).closest("tr")).find("[id*=hdfDefaultAmount]").val());
            }
            TaxComputation('B2001-B2009', 'Sec2');
            TaxComputation('C3001-C3027', 'Sec3');
        });

        function ViewMode(mode) {
            //Mode = 1 Indicates its on View Mode           
            if (mode == 1) {
                $("[id$=pnlSave]").hide();
            }
        }       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="upPayrollProcess" runat="server">
        <ContentTemplate>
            <div id="div_hrmsTab">
                <ucGtiTab:GtiTabControl ID="hrmsTab" runat="server" CurrentTab="11" />
            </div>
            <div id="div_payrollTab">
                <uc1:PayrollTabControl ID="payrollTab" runat="server" CurrentTab="2" />
            </div>
            <div class="fixed-buttons" id="divFixedBtns">
                <div class="Button-container">
                    <asp:Table ID="Table2" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="TableCell1" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSaveAndContinue">
                                        <asp:Button runat="server" ID="btnSaveContinue" OnClick="ActionHandler" CommandName="SAVEANDCONTINUE"
                                            TabIndex="18" Text="Save & Continue" ToolTip="Save & Continue" ValidationGroup="salary"
                                            OnClientClick="javascript:return ValidatePageNow('salary')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$resources:Controls,Save %>"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save"
                                            ValidationGroup="Save" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('save')"
                                            TabIndex="10" />
                                    </li>
                                    <li runat="server" id="pnlDelete" style="display: none">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="10" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li runat="server" id="pnlPrint">
                                        <asp:Button runat="server" TabIndex="10" ID="btnPrint" CommandName="PRINT" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,Print %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Print"
                                            ToolTip="<%$resources:Controls,Print %>" />
                                    </li>
                                    <li runat="server" id="pnlCancel">
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            CssClass="popupclose" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" OnClick="ActionHandler"
                                            TabIndex="10" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="tab-container-floating" id="div_tabcontainerList">
                    <%--Container for List and Detail tabs--%>
                    <ul>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="99" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="99" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkIncomeTax" Text="<%$resources:IncomeTax %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="99" OnClick="ActionHandler" CommandName="INCOMETAX"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <div id="divUCempBasicHdr">
                    <ucBasicHdr:EmpBasicInfoControl ID="UCempBasicHdr" runat="server" /></div>
                <asp:Table runat="server" ID="tblPage" CssClass="tablelayout asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div id="divPayrollHeader" runat="server" border="0" cellpadding="0" cellspacing="0"
                                class="head-info" style="display: none">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 1%;">
                                        </td>
                                        <td style="width: 45%;">
                                            <asp:Label runat="server" ID="lblPayrollEmployeeH" Text="<%$ resources:EmployeeH%>"
                                                AssociatedControlID="lblPayrollEmployee" class="margnbotm0"></asp:Label>
                                            <asp:Label ID="lblPayrollEmployee" runat="server" CssClass="margnbotm0 bold"></asp:Label>
                                            <asp:HiddenField ID="hdfEmployeePK" runat="server" Value="0"></asp:HiddenField>
                                        </td>
                                        <td style="width: 39%;">
                                            <asp:Label runat="server" ID="lblPayrollProcessH" Text="<%$ resources:PayrollProcessH%>"
                                                AssociatedControlID="lblPayrollProcess" class="margnbotm0"></asp:Label>
                                            <asp:Label ID="lblPayrollProcess" runat="server" CssClass="margnbotm0 bold"></asp:Label>
                                        </td>
                                        <td style="width: 15%;">
                                            <asp:Label runat="server" ID="lblPayrollProcessDateH" Text="<%$ resources:ProcessDateH%>"
                                                AssociatedControlID="lblPayrollProcessDate" class="margnbotm0"></asp:Label>
                                            <asp:Label ID="lblPayrollProcessDate" runat="server" CssClass="margnbotm0 bold"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <%--   Outer Repeater --%>
                            <asp:Repeater ID="rprIncomeTaxMaster" runat="server" OnItemDataBound="ActionHandler">
                                <HeaderTemplate>
                                    <table cellspacing="0">
                                        <tr>
                                        </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <div class="search-colapse-b">
                                                <h1 style="width: 80%">
                                                    <asp:Literal ID="ltrTaxDetails" runat="server" Text='<%# Eval("Title") %>' /></h1>
                                                <asp:ImageButton runat="server" Style="display: none" ID="imbShowTaxDetails" TabIndex="4"
                                                    SkinID="imbArrowShow" ToolTip="<%$ resources:Controls,ShowDetails%>" />
                                                <asp:ImageButton runat="server" ID="imbHideTaxDetails" TabIndex="4" SkinID="imbArrowHide"
                                                    ToolTip="<%$ resources:Controls,HideDetails%>" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnlIncomeTaxDetails" runat="server">
                                                <%--Style="display: none"--%>
                                                <div class="gridwrap" id="divIncomeTaxDetails">
                                                    <%-- Inner Repeater--%>
                                                    <table id="tblDetailsSection" class='<%# (Container.ItemIndex == 0)?"gridwraptable":"gridwrap-bg" %>'>
                                                        <tr>
                                                            <th>
                                                                <asp:Label ID="lbl_HTCol1" runat="server" />
                                                            </th>
                                                            <th>
                                                                <asp:Label ID="lbl_HTCol2" runat="server" />
                                                            </th>
                                                            <th>
                                                                <asp:Label ID="lblReqChk" runat="server" />
                                                            </th>
                                                            <th class="padglft2  txt-rgt">
                                                                <asp:Label ID="lbl_HTCol3" runat="server" />
                                                            </th>
                                                            <th>
                                                            </th>
                                                            <th class="padglft2  txt-rgt">
                                                                <asp:Label ID="lbl_HTCol4" runat="server" />
                                                            </th>
                                                            <asp:Panel ID="pnlthCol5" runat="server">
                                                                <th>
                                                                    <asp:Label ID="lbl_HTCol5" runat="server" />
                                                                </th>
                                                            </asp:Panel>
                                                        </tr>
                                                        <asp:Repeater ID="rprIncomeTax" runat="server" OnItemDataBound="Inner_ActionHandler">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td width="1%" class="padglft2 padgrgt2 txt-center">
                                                                        <asp:Label ID="lbl_ITCol1" runat="server" Text='<%# Eval("Col1") %>' ToolTip='<%# Eval("Col1") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfItemDbId" Value='<%# Eval("DbId") %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfIT1_PK" Value='<%# Eval("IT1_PK") %>' />
                                                                    </td>
                                                                    <td class="padglft2 padgrgt2">
                                                                        <asp:Label ID="lbl_ITCol2" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("Col2")),150) %>'
                                                                            ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("Col2")))%>' />
                                                                    </td>
                                                                    <td class="padglft2 txt-rgt">
                                                                        <asp:CheckBox ID="chkDefualtValue" runat="server" Visible='<%# Convert.ToInt32(Eval("ChkBoxReq")) == 1 ? true : false %>'
                                                                            Checked='<%# Convert.ToInt32(Eval("IT1_CHECKBOX_1")) == 1 ? true : false %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfDefaultAmount" Value='<%# Eval("DefaultAmount") %>' />
                                                                    </td>
                                                                    <td width="9%" class="padgrgt2 txt-rgt">
                                                                        <asp:TextBox ID="txt_ITCol3" runat="server" Text='<%# GetFormattedCurrency(Eval("Col3")) %>'
                                                                            MaxLength="12" Enabled='<%# Convert.ToInt32(Eval("Col3_Edit")) == 1 ? true : false %>'
                                                                            ToolTip='<%# Eval("Col3") %>' CssClass='<%# Convert.ToInt32(Eval("Col3_Edit")) == 1 ? "input-w80 numeric" : "input-w80 numeric input-disabled" %>'
                                                                            Visible='<%# Convert.ToInt32(Eval("Col3_Hide")) == 0 ? true : false %>' />
                                                                        <asp:HiddenField runat="server" ID="hdfColScript" Value='<%# Eval("ColScript") %>' />
                                                                        <asp:HiddenField ID="ColResult" runat="server" Value='<%# Eval("ColResult") %>' />
                                                                        <asp:HiddenField ID="hdfColResulCopyDbID" runat="server" Value='<%# Eval("ColResulCopyDbID") %>' />
                                                                    </td>
                                                                    <td width="1">
                                                                        <asp:RangeValidator ID="rngQuantity" runat="server" CssClass="star" SetFocusOnError="true"
                                                                            ValidationGroup="save" EnableClientScript="true" Display="Dynamic" Text="*" ControlToValidate="txt_ITCol3"
                                                                            Type="Double" ErrorMessage='<%# String.Format("{0} {1}", GetLocalResourceObject("Msg_ValidAmount"), Eval("Col3_Max")) %>'
                                                                            MaximumValue='<%#(Eval("Col3_Max")) == "" ? "" : Convert.ToDouble(Eval("Col3_Max")) > 0 ? Eval("Col3_Max") : "" %>'></asp:RangeValidator>
                                                                    </td>
                                                                    <td width="9%" class="padgrgt2 txt-rgt">
                                                                        <asp:TextBox ID="txt_ITCol4" runat="server" Text='<%#GetFormattedCurrency( Eval("Col4")) %>'
                                                                            Enabled='<%# Convert.ToInt32(Eval("Col4_Edit")) == 1 ? true : false %>' ToolTip='<%# Eval("Col4") %>'
                                                                            CssClass='<%# Convert.ToInt32(Eval("Col4_Edit")) == 1 ? "input-w80 numeric" : "input-w80 numeric input-disabled" %>'
                                                                            Visible='<%# Convert.ToInt32(Eval("Col4_Hide")) == 0 ? true : false %>' />
                                                                    </td>
                                                                    <asp:Panel ID="pnltdCol5" runat="server">
                                                                        <td width="32%" class="padglft2 padgrgt2">
                                                                            <asp:Label ID="lbl_ITCol5" runat="server" ToolTip='<%# Eval("Col5") %>' Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("Col5")),60) %>' />
                                                                        </td>
                                                                    </asp:Panel>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                            <asp:HiddenField runat="server" ID="hdfSectionDbId" Value='<%# Eval("SectionID") %>' />
                                            <asp:HiddenField ID="hdfSecCol3FunId" runat="server" Value='<%# Eval("Col3FunId") %>' />
                                            <asp:HiddenField ID="hdfSecCol3FunExpression" runat="server" Value='<%# Eval("Col3FunExpression") %>' />
                                            <asp:HiddenField ID="hdfSecCol3ExcludeId" runat="server" Value='<%# Eval("Col3ExcludeId") %>' />
                                            <asp:HiddenField ID="hdfSecCol3SumCopyId" runat="server" Value='<%# Eval("Col3CopyId") %>' />
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:ValidationSummary ID="vsPage" ValidationGroup="save" runat="server" />
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:HiddenField
                    ID="hdfIscontYes" runat="server" />
            </div>
            <asp:HiddenField ID="hdfPageFlag" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfSec1Col3FunId" runat="server" Value="" />
            <asp:HiddenField ID="hdfSec1Col3FunExpression" runat="server" Value="" />
            <asp:HiddenField ID="hdfSec1Col3ExcludeId" runat="server" Value="" />
            <asp:HiddenField ID="hdfSec1Col3SumCopyId" runat="server" Value="" />
            <asp:HiddenField ID="hdfSec2Col3FunId" runat="server" Value="" />
            <asp:HiddenField ID="hdfSec2Col3FunExpression" runat="server" Value="" />
            <asp:HiddenField ID="hdfSec2Col3ExcludeId" runat="server" Value="" />
            <asp:HiddenField ID="hdfSec2Col3SumCopyId" runat="server" Value="" />
            <asp:HiddenField ID="hdfSec3Col3FunId" runat="server" Value="" />
            <asp:HiddenField ID="hdfSec3Col3FunExpression" runat="server" Value="" />
            <asp:HiddenField ID="hdfSec3Col3ExcludeId" runat="server" Value="" />
            <asp:HiddenField ID="hdfSec3Col3SumCopyId" runat="server" Value="" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
