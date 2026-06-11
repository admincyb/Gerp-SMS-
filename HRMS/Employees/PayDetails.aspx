<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="PayDetails.aspx.cs" Inherits="HRMS.Employees.PayDetails" Theme="Classic" %>

<%@ Register Src="UserControls/GtiTabControl.ascx" TagName="GtiTabControl" TagPrefix="ucGtiTab" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function initComponents() {
            GrandScriptUtils.DatePickerCommon("txtPFEffectiveDate");
            GrandScriptUtils.DatePickerCommon("txtSOCSOEffectiveDate");
        }

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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <ucGtiTab:GtiTabControl ID="hrmsTab" runat="server" CurrentTab="7" />
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="20" Text="<%$resources:Controls,Save %>"
                                            OnClick="ActionHandler" OnClientClick="return ValidatePageNow('save')" ValidationGroup="save"
                                            ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            OnClick="ActionHandler" TabIndex="21" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                            ToolTip="<%$resources:Controls,Delete %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                            OnClick="ActionHandler" CommandName="CANCEL" TabIndex="22" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" ValidationGroup="none"
                                            OnClientClick="Page_BlockSubmit = false;" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="detail-co3" runat="server" id="divEmployeeHeader">
                <div class="div3col-S">
                    <asp:Label ID="lblhdrEmployeeNo" runat="server" AssociatedControlID="lblhdrEmployeeNoTxt"
                        Text="Employee No:"></asp:Label>
                    <asp:Label ID="lblhdrEmployeeNoTxt" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblhdrEmployeeName" runat="server" AssociatedControlID="lblhdrEmployeeNameTxt"
                        Text="Employee Name:"></asp:Label>
                    <asp:Label ID="lblhdrEmployeeNameTxt" runat="server" Text=""></asp:Label>
                </div>
                <div class="div3col-S">
                    <asp:Label ID="lblhdrDOJ" runat="server" AssociatedControlID="lblhdrDOJText" Text="DOJ:"></asp:Label>
                    <asp:Label ID="lblhdrDOJText" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblhdrDOB" runat="server" AssociatedControlID="lblhdrDOBTxt" Text="DOB:"></asp:Label>
                    <asp:Label ID="lblhdrDOBTxt" runat="server" Text=""></asp:Label>
                    <asp:HiddenField ID="hdfHdrDOB" runat="server"></asp:HiddenField>
                </div>
                <div class="div3col-S">
                    <asp:Label ID="lblhdrDesignation" runat="server" AssociatedControlID="lblhdrDesignationTxt"
                        Text="Designation:"></asp:Label>
                    <asp:Label ID="lblhdrDesignationTxt" runat="server" Text=""></asp:Label>
                    <asp:Label ID="lblhdrDepartment" runat="server" AssociatedControlID="lblhdrDepartmentTxt"
                        Text="Department:"></asp:Label>
                    <asp:Label ID="lblhdrDepartmentTxt" runat="server" Text=""></asp:Label>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide" runat="server" id="pnlPayDetails">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblBasic" AssociatedControlID="txtBasic" Text="<%$resources:BasicReq %>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBasic" TabIndex="1" MaxLength="100" />
                                            <asp:RequiredFieldValidator ID="reqBasic" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtBasic"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Basic %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblPaymentMode" AssociatedControlID="ddlPaymentMode"
                                                Text="<%$resources:PaymentModeReq %>"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlPaymentMode" TabIndex="1" AutoPostBack="true"
                                                OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="reqPaymentMode" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="save" EnableClientScript="true" runat="server"
                                                ControlToValidate="ddlPaymentMode" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PaymentMode %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblBankName" AssociatedControlID="ddlBankName" Text="<%$resources:BankNameReq %>"
                                                Enabled="false"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlBankName" TabIndex="1" Enabled="false">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBankName" CssClass="star" SetFocusOnError="true"
                                                Enabled="false" InitialValue="-1" ValidationGroup="save" EnableClientScript="true"
                                                runat="server" ControlToValidate="ddlBankName" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_BankName %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblAccountCode" AssociatedControlID="txtAccountCode"
                                                Text="<%$resources:AccountCodeReq %>" Enabled="false"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtAccountCode" TabIndex="1" MaxLength="100" Enabled="false" />
                                            <asp:RequiredFieldValidator ID="rfvAccountCode" CssClass="star" SetFocusOnError="true"
                                                Enabled="false" ValidationGroup="save" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtAccountCode" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AccountCode %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblIFSCCode" AssociatedControlID="txtIFSCCode" Text="<%$resources:IFSCCodeReq %>"
                                                Enabled="false"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtIFSCCode" TabIndex="1" MaxLength="100" Enabled="false" />
                                            <asp:RequiredFieldValidator ID="rfvIFSCCode" CssClass="star" SetFocusOnError="true"
                                                Enabled="false" ValidationGroup="save" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtIFSCCode" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_IFSCCode %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblPFAccount" AssociatedControlID="txtPFAccount" Enabled="false"
                                                Text="<%$resources:PFAccount %>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPFAccount" TabIndex="1" MaxLength="100" Enabled="false" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblSOCSOAccount" AssociatedControlID="txtSOCSOAccount"
                                                Text="<%$resources:SOCSOAccount %>" Enabled="false"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSOCSOAccount" TabIndex="1" MaxLength="100" Enabled="false" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblEmployementType" AssociatedControlID="ddlEmployementType"
                                                Text="<%$resources:EmployementTypeReq %>"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlEmployementType" TabIndex="1" AutoPostBack="true"
                                                OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvEmployementType" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="save" EnableClientScript="true" runat="server"
                                                ControlToValidate="ddlEmployementType" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EmployementType %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblPANNo" AssociatedControlID="txtPANNo" Text="<%$resources:PANNoReq %>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPANNo" TabIndex="1" MaxLength="100" />
                                            <asp:RequiredFieldValidator ID="rfvPANNo" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtPANNo"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_PANNo %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblSalaryTemplate" AssociatedControlID="ddlSalaryTemplate"
                                                Text="<%$resources:SalaryTemplateReq %>"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlSalaryTemplate" TabIndex="1">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvSalaryTemplate" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="save" EnableClientScript="true" runat="server"
                                                ControlToValidate="ddlSalaryTemplate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_SalaryTemplate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblBranch" AssociatedControlID="txtBranch" Enabled="false"
                                                Text="<%$resources:BranchReq %>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtBranch" TabIndex="1" MaxLength="100" Enabled="false" />
                                            <asp:RequiredFieldValidator ID="rfvBranch" CssClass="star" SetFocusOnError="true"
                                                Enabled="false" ValidationGroup="save" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtBranch" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Branch %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblAccountName" AssociatedControlID="txtAccountName"
                                                Enabled="false" Text="<%$resources:AccountNameReq %>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtAccountName" TabIndex="1" MaxLength="100" Enabled="false" />
                                            <asp:RequiredFieldValidator ID="rfvAccountName" CssClass="star" SetFocusOnError="true"
                                                Enabled="false" ValidationGroup="save" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtAccountName" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_AccountName %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblPFEffectiveDate" AssociatedControlID="txtPFEffectiveDate"
                                                Enabled="false" Text="<%$resources:PFEffectiveDate %>"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPFEffectiveDate" CssClass="Uidate-picker" TabIndex="1"
                                                Enabled="false" onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblSOCSOEffectiveDate" AssociatedControlID="txtSOCSOEffectiveDate"
                                                Text="<%$resources:SOCSOEffectiveDate %>" Enabled="false"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSOCSOEffectiveDate" CssClass="Uidate-picker" TabIndex="1"
                                                onkeydown="return CheckKey(event)" onpaste="return false;" Enabled="false" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblLeaveTemplate" AssociatedControlID="ddlLeaveTemplate"
                                                Text="<%$resources:LeaveTemplateReq %>"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlLeaveTemplate" TabIndex="1">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvLeaveTemplate" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="save" EnableClientScript="true" runat="server"
                                                ControlToValidate="ddlLeaveTemplate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_LeaveTemplate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblOTTemplate" AssociatedControlID="ddlOTTemplate"
                                                Text="<%$resources:OTTemplateReq %>"></asp:Label>
                                            <asp:DropDownList runat="server" ID="ddlOTTemplate" TabIndex="1">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvOTTemplate" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="save" EnableClientScript="true" runat="server"
                                                ControlToValidate="ddlOTTemplate" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_OTTemplate %>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S gridwrap">
                                            <asp:GridView runat="server" ID="grdWorkingDays" Width="150" AllowPaging="false"
                                                AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable">
                                                <EmptyDataTemplate>
                                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                </EmptyDataTemplate>
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ resources:GridDay%> ">
                                                        <ItemTemplate>
                                                            <asp:HiddenField runat="server" ID="hdfWHrsPK" Value='<%# Eval("Pk") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfWHrsEmpTypeID" Value='<%# Eval("EmployeeType") %>' />
                                                            <asp:HiddenField runat="server" ID="hdfWeekDay" Value='<%# Eval("WeekDay") %>' />
                                                            <asp:Label ID="lblWeekDay" runat="server" Text='<%# Eval("WeekDayTest") %>' ToolTip='<%# Eval("WeekDayTest") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="80%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$resources:GridHrs%>">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtHrs" runat="server" CssClass="small numeric" TabIndex="1" Text='<%# Eval("Hours") %>'
                                                                onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);">
                                                            </asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="20%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <div id="diverror" style="display: none">
                                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                                <asp:ValidationSummary ID="vsPageSave" ValidationGroup="save" runat="server" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
