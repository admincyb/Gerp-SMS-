<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckOutControl.ascx.cs"
    Inherits="HRMS.Employees.UserControls.CheckOutControl" %>
<style type="text/css">
    .lblSizeAdjust
    {
        width: 136px !important;
    }
</style>
<script type="text/javascript">
    var pageURL = window.document.URL;
    var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
    var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
    var msgChkOutTitle = '<%= Resources.ErpRes.Information %>';
    var msgChkOutContent = '';

    function InitCheckoutComponents() {
        GrandScriptUtils.AddDateRange("txtIssuedOutOn", "hdfIssuedOutOn", "txtExpectedReturnDate", "hdfExpectedReturnDate", "dd-M-yy", false, true, true);
        $("[id$=txtIssuedOutOnTime]").timepicker();

        if ($("select[id$=ddlInternalOrExternal]").val() == "1") { // Internal
            $("[id$=divNonAutoIssued]").hide();
            $("[id$=divAutoIssued]").show();
            $("[id$=hdfCheckOutAutoIssuedTo]").val('');
            $("[id$=txtCheckOutAutoIssuedTo]").val('');
            $("[id$=txtCheckOutIssuedTo]").val('');
            ValidatorEnable($("[id$=reqIssuedTo]")[0], true);
            ValidatorEnable($("[id$=reqIssuedToExt]")[0], false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCheckOutAutoIssuedTo", url, "hdfCheckOutAutoIssuedTo", true, true, "EMPLOYEEAUTOCOMPLETE");
        } else { // External
            $("[id$=divNonAutoIssued]").show();
            $("[id$=divAutoIssued]").hide();

            $("[id$=txtCheckOutIssuedTo]").val('');
            $("[id$=hdfCheckOutAutoIssuedTo]").val('');
            $("[id$=txtCheckOutAutoIssuedTo]").val('');

            ValidatorEnable($("[id$=reqIssuedTo]")[0], false);
            ValidatorEnable($("[id$=reqIssuedToExt]")[0], true);
        }
    }

    function onCheckOutAfterDateSelect(controlID) {
        if (controlID == "txtExpectedReturnDate" || controlID == "txtIssuedOutOn") {
            validateExpectedReturnDate();
        }
    }

    function AfterClose(containerID) {
        if (containerID == "[id$=divCheckOutControlCheckInCheckOutLog]") {
            $("[id$=dummyCheckOutPopup]").click();
        }
    }

    function onInternalOrExternalChange() {
        InitCheckoutComponents();
    }

    function validateExpectedReturnDate() {
        var issuedOutOn = $('[id$=txtIssuedOutOn]').val();
        var expectedReturn = $('[id$=txtExpectedReturnDate]').val();
        if (expectedReturn == undefined || expectedReturn == "") return true;
        if (compareDate(expectedReturn, issuedOutOn) == false) {
            //$('[id$=txtExpectedReturnDate]').val('');
            msgChkOutContent = '<%= Resources.ErrorMessages.Msg_ExpectedReturnDate_Warning %>'
            ShowErrorMessage(msgChkOutContent, msgChkOutTitle);
        }
    }

    //For finding and removing duplicate and other group validation controls
    //Array of present validations
    var checkOutValidationArrayGroup;
    function CheckCheckOutValidationDuplicate(valGroup) {
        checkOutValidationArrayGroup = new Array();
        //Traversing from bottom through all the validation controls in the page
        for (var i = Page_Validators.length - 1; i >= 0; i--) {
            if (typeof (Page_Validators[i].validationGroup) == "string") {
                if (valGroup == Page_Validators[i].validationGroup) {
                    //checks if the control is already in the validation array
                    if (!CheckCheckOutValidationExists(Page_Validators[i].id)) {
                        //insert new conrol to the Array of present validations
                        checkOutValidationArrayGroup.push(Page_Validators[i].id);
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
    function CheckCheckOutValidationExists(id) {
        for (var i in checkOutValidationArrayGroup) {
            if (checkOutValidationArrayGroup[i] == id) {
                return true;
            }
        }
        return false;
    }
    function ValidateCheckOutPageNow(valGroup) {
        if (typeof (Page_ClientValidate) == 'function') {
            //For finding and removing duplicate and other group validation controls
            CheckCheckOutValidationDuplicate(valGroup);
            //For Script validating the Page
            Page_ClientValidate(valGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litCheckOutErrorMsg]").hide();
            ShowErrorMessage($("#divCheckOutError").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }
</script>
<div class="Button-container-popup">
    <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="310" Text="<%$resources:Controls,Save %>"
        OnClick="ActionHandler" OnClientClick="javascript:ValidateCheckOutPageNow('checkOutSave')"
        ValidationGroup="checkOutSave" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
        SkinID="btnInner-Save" />
    <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
        OnClick="ActionHandler" CommandName="CANCEL" TabIndex="311" CommandArgument="SEC_ActionPanel"
        SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
</div>
<div class="contentwrapper">
    <div style="display: none;">
        <asp:Button Text="" runat="server" ID="dummyCheckOutPopup" CommandName="CHECKOUTLOGCLOSE"
            OnClick="ActionHandler" />
    </div>
    <div class="gridwrap">
        <asp:GridView runat="server" ID="grdEmpDocList" Width="100%" AutoGenerateColumns="false"
            EmptyDataRowStyle-CssClass="emptytable">
            <EmptyDataTemplate>
                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="<%$ resources:EmpCode %>">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" ID="hdfDocPk" Value='<%# Eval(Resources.DataFieldRes.EmpDocPk) %>' />
                        <asp:Label ID="lblEmpCode" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpCode).ToString())%>'
                            Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpCode).ToString()),8)%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="11%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:Employee %>">
                    <ItemTemplate>
                        <asp:Label ID="lblEmployee" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.Employee).ToString())%>'
                            Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.Employee).ToString()),8)%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="12%" />
                </asp:TemplateField>
                <asp:TemplateField Visible="true" HeaderText="<%$ resources:Nationality %>">
                    <ItemTemplate>
                        <asp:Label ID="lblNationality" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.Nationality).ToString())%>'
                            Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.Nationality).ToString()),8)%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="11%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:DocType %>">
                    <ItemTemplate>
                        <asp:Label ID="lblDocType" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.DocType).ToString())%>'
                            Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.DocType).ToString()),8)%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="11%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:DocNo %>">
                    <ItemTemplate>
                        <asp:Label ID="lblDocNo" runat="server" ToolTip='<%#  HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.DocNo).ToString())%>'
                            Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.DocNo).ToString()),8)%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="11%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:IssuedBy %>" SortExpression="<%$ resources:DataFieldRes,EmpDocIssuedBy %>">
                    <ItemTemplate>
                        <asp:Label ID="lblIssuedBy" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocIssuedBy).ToString())%>'
                            Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocIssuedBy).ToString()),8)%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="11%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:IssuedOn %>">
                    <ItemTemplate>
                        <asp:Label ID="lblIssuedOn" runat="server" Text='<%# Eval(Resources.DataFieldRes.IssuedOn, Resources.Constants.DateFormatGridExpanded) %>'
                            ToolTip='<%# Eval(Resources.DataFieldRes.IssuedOn, Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:DocExpiresOn %>">
                    <ItemTemplate>
                        <asp:Label ID="lblExpiresOn" runat="server" Text='<%# Eval(Resources.DataFieldRes.DocExpiresOn, Resources.Constants.DateFormatGridExpanded) %>'
                            ToolTip='<%# Eval(Resources.DataFieldRes.DocExpiresOn, Resources.Constants.DateFormatGridExpanded)%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="10%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ resources:DocDaysLeft %>">
                    <ItemTemplate>
                        <asp:Label ID="lblDaysLeft" runat="server" Text='<%# Eval(Resources.DataFieldRes.DocDaysLeft) %>'
                            ToolTip='<%# Eval(Resources.DataFieldRes.DocDaysLeft)%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="9%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="lnkLog" runat="server" ImageUrl="~/Images/Classic/Icons/log.jpg"
                            OnClick="ActionHandler" CommandName="SHOWLOG" TabIndex="8" ToolTip="Log" CommandArgument='<%# Eval(Resources.DataFieldRes.EmpDocPk) %>' />
                    </ItemTemplate>
                    <ItemStyle Width="3%" HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <table class="table-devide">
        <tr>
            <td>
                <div class="div2col-S">
                    <asp:Label runat="server" ID="lblIssuedOutOn" Text="<%$ resources:IssuedOutOn%>"
                        AssociatedControlID="txtIssuedOutOn" CssClass="middle-lbl-small-f"></asp:Label>
                    <asp:HiddenField ID="hdfIssuedOutOn" runat="server" Value="" />
                    <asp:TextBox runat="server" ID="txtIssuedOutOn" TabIndex="301" MaxLength="12" CssClass="input-small"
                        onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtIssuedOutOnTime" TabIndex="302" MaxLength="12"
                        CssClass="small" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqIssuedOutOn" CssClass="star" SetFocusOnError="true"
                        ValidationGroup="checkOutSave" EnableClientScript="true" runat="server" ControlToValidate="txtIssuedOutOn"
                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_IssuedOn %>" />
                    <asp:RequiredFieldValidator ID="reqIssuedOutOnTime" CssClass="star" SetFocusOnError="true"
                        ValidationGroup="checkOutSave" EnableClientScript="true" runat="server" ControlToValidate="txtIssuedOutOnTime"
                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_IssuedOnTime %>" />
                    <div class="clear">
                    </div>
                    <asp:Label runat="server" ID="lblInternalOrExternal" Text="<%$ resources:InternalOrExternal%>"
                        AssociatedControlID="ddlInternalOrExternal" CssClass="middle-lbl-small-f"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlInternalOrExternal" onchange="onInternalOrExternalChange();"
                        CssClass="select-small-a1" TabIndex="304">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqInternalOrExternal" CssClass="star" SetFocusOnError="true"
                        InitialValue="-1" ValidationGroup="checkOutSave" EnableClientScript="true" runat="server"
                        ControlToValidate="ddlInternalOrExternal" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_InternalOrExternal %>">
                    </asp:RequiredFieldValidator>
                    <div class="clear">
                    </div>
                    <asp:Label runat="server" ID="lblPurpose" Text="<%$ resources:PurposeReq%>" AssociatedControlID="txtPurpose"
                        CssClass="middle-lbl-small-f"></asp:Label>
                    <asp:TextBox runat="server" ID="txtPurpose" CssClass="input-half" TabIndex="307"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqPurpose" CssClass="star" SetFocusOnError="true"
                        ValidationGroup="checkOutSave" EnableClientScript="true" runat="server" ControlToValidate="txtPurpose"
                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Purpose %>" />
                    <div class="clear">
                    </div>
                </div>
            </td>
            <td>
                <div class="div2col-S">
                    <asp:Label runat="server" ID="lblIssuedFor" Text="<%$ resources:IssuedFor%>" AssociatedControlID="ddlIssuedFor"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlIssuedFor" CssClass="select-small-c1" TabIndex="303">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqIssuedFor" CssClass="star" SetFocusOnError="true"
                        InitialValue="-1" ValidationGroup="checkOutSave" EnableClientScript="true" runat="server"
                        ControlToValidate="ddlIssuedFor" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_IssuedFor %>">
                    </asp:RequiredFieldValidator>
                    <div class="clear">
                    </div>
                </div>
                <div class="div2col-S" id="divAutoIssued">
                    <asp:Label runat="server" ID="lblIssuedTo" Text="<%$ resources:IssuedTo%>" AssociatedControlID="txtCheckOutAutoIssuedTo"
                        CssClass="lblSizeAdjust"></asp:Label>
                    <asp:TextBox runat="server" ID="txtCheckOutAutoIssuedTo" CssClass="select-small-c"
                        TabIndex="305" />
                    <asp:HiddenField ID="hdfCheckOutAutoIssuedTo" runat="server" Value="" />
                    <asp:RequiredFieldValidator ID="reqIssuedTo" CssClass="star" SetFocusOnError="true"
                        InitialValue="Select/Type" ValidationGroup="checkOutSave" EnableClientScript="true"
                        runat="server" ControlToValidate="txtCheckOutAutoIssuedTo" Display="Dynamic"
                        Text="*" ErrorMessage="<%$ resources:Err_IssuedTo %>" />
                </div>
                <div class="div2col-S" id="divNonAutoIssued" style="display: none;">
                    <asp:Label runat="server" ID="Label1" Text="<%$ resources:IssuedTo%>" AssociatedControlID="txtCheckOutIssuedTo"></asp:Label>
                    <asp:TextBox runat="server" ID="txtCheckOutIssuedTo" CssClass="select-small-c" TabIndex="306" />
                    <asp:RequiredFieldValidator ID="reqIssuedToExt" CssClass="star" SetFocusOnError="true"
                        ValidationGroup="checkOutSave" EnableClientScript="true" runat="server" ControlToValidate="txtCheckOutIssuedTo"
                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_IssuedTo %>" />
                </div>
                <div class="div2col-S">
                    <asp:Label runat="server" ID="lblExpectedReturnDate" Text="<%$ resources:ExpectedReturn %>"
                        AssociatedControlID="txtExpectedReturnDate"></asp:Label>
                    <asp:HiddenField ID="hdfExpectedReturnDate" runat="server" Value="" />
                    <asp:TextBox runat="server" ID="txtExpectedReturnDate" TabIndex="308" MaxLength="12"
                        CssClass="input-small" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="divcol-S">
                    <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks"
                        CssClass="middle-lbl-small-f"></asp:Label>
                    <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TabIndex="309" TextMode="MultiLine"
                        CssClass="multiline-2col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                </div>
                <div class="clear">
                </div>
            </td>
        </tr>
    </table>
    <div id="divCheckOutControlCheckInCheckOutLog" style="display: none;">
        <div class="gridwrap">
            <asp:GridView runat="server" ID="grdCheckIncheckOutLog" Width="100%" AutoGenerateColumns="false"
                EmptyDataRowStyle-CssClass="emptytable">
                <EmptyDataTemplate>
                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label></EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="<%$ resources:SlNo %>" SortExpression="<%$ resources:DataFieldRes,EmpDocSLNo %>">
                        <ItemTemplate>
                            <asp:Label ID="lblDocSlNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.EmpDocSLNo) %>'
                                ToolTip='<%# Eval(Resources.DataFieldRes.EmpDocSLNo)%>'></asp:Label></ItemTemplate>
                        <ItemStyle Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:EmpDocCheckedOnDate %>">
                        <ItemTemplate>
                            <asp:Label ID="lblDocChkdOn" runat="server" Text='<%# Eval(Resources.DataFieldRes.EmpDocCheckedOnDate, Resources.Constants.DateFormatGridExpanded) %> '
                                ToolTip='<%# Eval(Resources.DataFieldRes.EmpDocCheckedOnDate, Resources.Constants.DateFormatGridExpanded)%>'></asp:Label></ItemTemplate>
                        <ItemStyle Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:EmpDocCheckedType %>">
                        <ItemTemplate>
                            <asp:Label ID="lblDocChkdType" runat="server" Text='<%# Eval(Resources.DataFieldRes.EmpDocCheckedType) %> '
                                ToolTip='<%# Eval(Resources.DataFieldRes.EmpDocCheckedType)%>'></asp:Label></ItemTemplate>
                        <ItemStyle Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="true" HeaderText="<%$ resources:EmpDocCheckedBy %>">
                        <ItemTemplate>
                            <asp:Label ID="lblIssuedBy" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckedBy).ToString())%>'
                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckedBy).ToString()),20)%>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="22%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:Purpose %>" SortExpression="<%$ Resources:DataFieldRes,EmpDocCheckOutPurpose %>">
                        <ItemTemplate>
                            <asp:Label ID="lblIssuedOn" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckOutPurpose).ToString())%>'
                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckOutPurpose).ToString()),20)%>'></asp:Label></ItemTemplate>
                        <ItemStyle Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:Remarks %>">
                        <ItemTemplate>
                            <asp:Label ID="lblExpiresOn" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckInOutRemarks).ToString()) %>'
                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckInOutRemarks).ToString()),20)%>'></asp:Label></ItemTemplate>
                        <ItemStyle Width="22%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <div id="divCheckOutError" style="display: none">
        <asp:Label runat="server" ID="litCheckOutErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
        <asp:ValidationSummary ID="vsCheckOutPageSave" ValidationGroup="checkOutSave" runat="server" />
    </div>
</div>
