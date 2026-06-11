<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckInControl.ascx.cs"
    Inherits="HRMS.Employees.UserControls.CheckInControl" %>
<script type="text/javascript">
    var pageURL = window.document.URL;
    var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
    var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

    function InitCheckinComponents() {
        GrandScriptUtils.DatePickerCommon("txtCheckedInOn");
        $("[id$=txtCheckedInOnTime]").timepicker();
        GrandScriptUtils.MakeAutoCompleteDDL("txtCheckInAutoCheckedInBy ", url, "hdfCheckInAutoCheckedInBy", true, true, "EMPLOYEEAUTOCOMPLETE");
    }

    //For finding and removing duplicate and other group validation controls
    //Array of present validations
    //CheckIn
    var checkInValidationArrayGroup;
    function CheckCheckInValidationDuplicate(valGroup) {
        checkInValidationArrayGroup = new Array();
        //Traversing from bottom through all the validation controls in the page
        for (var i = Page_Validators.length - 1; i >= 0; i--) {
            if (typeof (Page_Validators[i].validationGroup) == "string") {
                if (valGroup == Page_Validators[i].validationGroup) {
                    //checks if the control is already in the validation array
                    if (!CheckCheckInValidationExists(Page_Validators[i].id)) {
                        //insert new conrol to the Array of present validations
                        checkInValidationArrayGroup.push(Page_Validators[i].id);
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
    function CheckCheckInValidationExists(id) {
        for (var i in checkInValidationArrayGroup) {
            if (checkInValidationArrayGroup[i] == id) {
                return true;
            }
        }
        return false;
    }
    function ValidateCheckInPageNow(valGroup) {
        if (typeof (Page_ClientValidate) == 'function') {
            //For finding and removing duplicate and other group validation controls
            CheckCheckInValidationDuplicate(valGroup);
            //For Script validating the Page
            Page_ClientValidate(valGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litCheckInErrorMsg]").hide();
            ShowErrorMessage($("#divCheckInerror").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }

    function AfterClose(containerID) {
        if (containerID == "[id$=divCheckInControlCheckInCheckOutLog]") {
            $("[id$=dummyCheckInPopup]").click();
        }
    }
   
</script>
<div class="Button-container-popup">
    <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="14" Text="<%$resources:Controls,Save %>"
        OnClick="ActionHandler" OnClientClick="javascript:ValidateCheckInPageNow('checkInSave')"
        ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
    <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
        OnClick="ActionHandler" CommandName="CANCEL" TabIndex="15" CommandArgument="SEC_ActionPanel"
        SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
</div>
<div class="contentwrapper">
    <div style="display: none;">
        <asp:Button Text="" runat="server" ID="dummyCheckInPopup" CommandName="CHECKINLOGCLOSE"
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
                        <asp:Label ID="lblDocNo" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.DocNo).ToString())%>'
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
                    <asp:Label runat="server" ID="lblCheckedInOn" Text="<%$ resources:CheckedInOn%>"
                        AssociatedControlID="txtCheckedInOn"></asp:Label>
                    <asp:TextBox runat="server" ID="txtCheckedInOn" TabIndex="1" MaxLength="12" CssClass="Uidate-picker"
                        onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                    <asp:TextBox runat="server" ID="txtCheckedInOnTime" TabIndex="1" MaxLength="12" CssClass="small"
                        onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                    <div class="clear">
                    </div>
                </div>
            </td>
            <td>
                <div class="div2col-S">
                    <asp:Label runat="server" ID="lblCheckedInBy" Text="<%$ resources:CheckedInBy%>"
                        AssociatedControlID="txtCheckInAutoCheckedInBy"></asp:Label>
                    <asp:TextBox runat="server" ID="txtCheckInAutoCheckedInBy" CssClass="input-half" />
                    <asp:HiddenField ID="hdfCheckInAutoCheckedInBy" runat="server" Value="0" />
                    <asp:RequiredFieldValidator ID="reqAutoCheckedInBy" CssClass="star" SetFocusOnError="true"
                        InitialValue="Seletct/Type" ValidationGroup="checkInSave" EnableClientScript="true"
                        runat="server" ControlToValidate="txtCheckInAutoCheckedInBy" Display="Dynamic"
                        Text="*" ErrorMessage="<%$ resources:Err_CheckedBy %>" />
                    <div class="clear">
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="divcol-S">
                    <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:Remarks %>" AssociatedControlID="txtRemarks" CssClass="margnrgt-minus1"></asp:Label>
                    <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TabIndex="3" TextMode="MultiLine"
                        CssClass="multiline-2col" onkeydown="limitText(this,500);" onkeyup="limitText(this,500);"></asp:TextBox>
                </div>
                <div class="clear">
                </div>
            </td>
        </tr>
    </table>
    <div id="divCheckInControlCheckInCheckOutLog" style="display: none;">
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
                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckedBy).ToString()),20)%>'></asp:Label></ItemTemplate>
                        <ItemStyle Width="22%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ resources:Purpose %>">
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
    <div id="divCheckInerror" style="display: none">
        <asp:Label runat="server" ID="litCheckInErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
        <asp:ValidationSummary ID="vsCheckInPageSave" ValidationGroup="checkInSave" runat="server" />
    </div>
</div>
