<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmpDocument.ascx.cs"
    Inherits="HRMS.Employees.UserControls.EmpDocument" %>
<%@ Register Src="~/Employees/UserControls/CheckInControl.ascx" TagName="CheckInControl"
    TagPrefix="ucChkIn" %>
<%@ Register Src="~/Employees/UserControls/CheckOutControl.ascx" TagName="CheckOutControl"
    TagPrefix="ucChkOut" %>
<%@ Register Src="~/Employees/UserControls/GtiTabControl.ascx" TagName="GtiTabControl"
    TagPrefix="ucGtiTab" %>
<%@ Register Src="~/Employees/UserControls/EmpBasicInfoControl.ascx" TagName="EmpBasicInfoControl"
    TagPrefix="ucBasicHdr" %>
<style type="text/css">
    .divcol-S label
    {
        max-width: 136px !important;
    }
    /*Start ToolTip Css for any dom element */
    [tooltip]:before
    {
        position: absolute;
        content: attr(tooltip);
        opacity: 0;
        top: -50;
    }
    
    [tooltip]:hover:before
    {
        opacity: 1;
        background: #feffcd;
        border: 1px solid black;
        padding: 2px;
    }
    
    [tooltip]:not([tooltip-persistent]):before
    {
        pointer-events: none;
    }
    /*End ToolTip Css for any dom element */
</style>
<script type="text/javascript">
    var msgEmpDocTitle = '<%= Resources.ErpRes.Information %>';
    var msgContent = "";
    var pagURL = window.document.URL;
    var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
    var handlerUrl = pagURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

    function AfterDateSelect(controlID) {
        if (controlID == "txtDocumentExpiresOn") {
            if (checkValidExpiryDateCallBack() == false) {
                $('[id$=txtDocumentExpiresOn]').val('');
                $('[id$=lblExpiryDaysLeft]').text('');
                msgContent = '<%= Resources.ErrorMessages.Msg_ExpiryDate_Warning %>'
                ShowErrorMessage(msgContent, msgEmpDocTitle);
            } else {
                expiryDateChangedCallBack();
            }
        }
        else if (controlID == "txtDocumentIssuedOn") {
            if (checkValidIssuedDateCallBack() == false) {
                $('[id$=txtDocumentIssuedOn]').val('');
                msgContent = '<%= Resources.ErrorMessages.Msg_IssuedDate_Warning %>'
                ShowErrorMessage(msgContent, msgEmpDocTitle);
            }
        }
    }

    function checkValidIssuedDateCallBack() {
        var dob = $('[id$=hdfHdrDOB]').val();
        var issuedOn = $('[id$=txtDocumentIssuedOn]').val();
        return compareDate(issuedOn, dob);
    }

    function checkValidExpiryDateCallBack() {
        var issuedOn = $('[id$=txtDocumentIssuedOn]').val();
        var expiredOn = $('[id$=txtDocumentExpiresOn]').val();
        return compareDate(expiredOn, issuedOn);
    }

    function expiryDateChangedCallBack() {
        var selectedDate = $('[id$=txtDocumentExpiresOn]').val();
        var daysLeft = getDaysLeft(selectedDate);

        if (daysLeft > 1) {
            $('[id$=lblExpiryDaysLeft]').text(daysLeft + " days left");
            $('[id$=hdfExpiryDaysLeft]').val(daysLeft + " days left");
        }
        else if (daysLeft > -1) {
            $('[id$=lblExpiryDaysLeft]').text(daysLeft + " day left");
            $('[id$=hdfExpiryDaysLeft]').val(daysLeft + " day left");
        }
        else if (daysLeft > -2) {
            $('[id$=lblExpiryDaysLeft]').text(Math.abs(daysLeft) + " day overdue");
            $('[id$=hdfExpiryDaysLeft]').val(Math.abs(daysLeft) + " day overdue");
        }
        else {
            $('[id$=lblExpiryDaysLeft]').text(Math.abs(daysLeft) + " days overdue");
            $('[id$=hdfExpiryDaysLeft]').val(Math.abs(daysLeft) + " days overdue");
        }
    }

    function compareDate(dateStrig1, dateString2) {
        var date1 = new moment(dateStrig1, "DD-MMM-YYYY");
        var date2 = new moment(dateString2, "DD-MMM-YYYY");
        if (isNaN(date2)) {
            return false;
        }
        return date1 > date2;
    }

    function getDaysLeft(dateString) {
        var now = new moment(new Date());
        var expiry = new moment(dateString, "DD-MMM-YYYY");
        return Math.ceil(expiry.diff(now, "days", true));
    }

    function InitializeComponents() {
        //GrandScriptUtils.DatePickerCommon("txtDocumentIssuedOn");
        //GrandScriptUtils.DatePickerCommon("txtDocumentExpiresOn");
        //GrandScriptUtils.DatePickerCommon("txtDocumentRefDate");
        GrandScriptUtils.RestrictedYearDatePicker("txtDocumentRefDate", false, true, true, '<%= GetGlobalResourceObject("ConfigurationsRes", "HrmsFromDate").ToString() %>');
        GrandScriptUtils.DatePickerCommon("txtCheckedInOn");

        setDocumentIssuedDate();
        GrandScriptUtils.MakeAutoCompleteDDL("txtAutoCheckedInBy", handlerUrl + "?EmpCategory=2", "hdfAutoCheckedInBy", true, true, "EMPLOYEEAUTOCOMPLETE");
    }

    function setDocumentIssuedDate() {
        GrandScriptUtils.AddDateRangeCommon("txtDocumentIssuedOn", "hdfDocumentEntryPageIssuedOn", "txtDocumentExpiresOn", "hdfDocumentEntryPageExpiresOn", false, false);
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

    function ValidateEmpDocControl(valGroup, btn) {
        if (typeof (Page_ClientValidate) == 'function') {
            //For finding and removing duplicate and other group validation controls
            //CheckValidationDuplicate(valGroup);
            //For Script validating the Page
            Page_ClientValidate(valGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsg]").hide();
            ShowErrorMessage($("#divEmpDocError").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff  
            if (($("[id$=hdfExpiryDaysLeft]").val().indexOf('overdue') > 0 || $("[id$=hdfExpiryDaysLeft]").val() == '0 day left') && valGroup == 'save') {
                msgEmpDocTitle = 'Information';
                msgContent = 'Document Expired. Do you want to Continue?';
                $("#divConfirmation").html(msgContent).dialog({
                    modal: true,
                    height: 150,
                    width: 350,
                    title: msgEmpDocTitle,
                    resizable: false,
                    buttons: {
                        OK: function (e) {
                            $(this).dialog("close");
                            __doPostBack(btn.name, '');
                            return true;
                        },
                        Cancel: function (e) {
                            $(this).dialog("close");
                            return false;
                        }
                    }
                });
            }
            else if (valGroup == 'save') {
                return true;
            }
        }
        return false;
    }
    function ShowListing(flag) {
        if (flag) {
            $("[id$=PageAction_List]").show();
            $("[id$=PageAction_Entry]").hide();
            $("[id$=pnlListing]").show();
            $("[id$=pnlEntry]").hide();
            //$("[id$=ddlCompany]").hide();//Hide by biju
        }
        else {
            $("[id$=PageAction_List]").hide();
            $("[id$=PageAction_Entry]").show();
            $("[id$=pnlListing]").hide();
            $("[id$=pnlEntry]").show();
            //$("[id$=ddlCompany]").show(); //Hide by biju
        }
        return false;
    }
    function EmpDocViewMode(mode) {
        //Mode = 1 Indicates its on View Mode
        //Mode = 2 Indicates its on New Mode
        if (mode == 1) {
            $("[id$=pnlSave]").hide();
            $("[id$=pnlSaveContinue]").hide(); 
            $("[id$=pnlDelete]").hide();
            $("[id$=pnlSubmit]").hide();
            $("[id$=btnSavePaymentSplit]").hide();
            $("[id$=btnAddItem]").hide();

        }
        else if (mode == 2) {
            $("[id$=pnlDelete]").hide();
            $("[id$=pnlPrint]").hide();
        }
    }
    function ViewModeHdr(mode) {
        //Mode = 1 Indicates its on View Mode           
        if (mode == 1) {
            $("[id$=pnlSave]").hide();
            $("[id$=pnlSaveContinue]").hide(); 
            $("[id$=pnlDelete]").hide();
            $("[id$=pnlSubmit]").hide();
            $("[id$=btnSavePaymentSplit]").hide();
            $("[id$=btnAddItem]").hide();
            $("[id$=pnlNew]").hide();
            $("[id$=pnlEdit]").hide();
            $("[id$=pnlView]").hide();

        }
    }

    //show confirmation msg for reload leave details
    function ShowConfirmDocDetails(msg) {
        var msgEmpDocTitle;
        var msg;
        msgEmpDocTitle = '<%= Resources.ErpRes.Title_Information %>';
        $("#divConfirmation").html(msg).dialog({
            modal: true,
            height: 150,
            width: 350,
            title: msgEmpDocTitle,
            resizable: false,
            buttons: {
                Yes: function (e) {
                    $("[id$=hdfIscontYes]").val(1);
                    $(this).dialog("close");
                    $("[id$=btnSave]").click();
                },
                Cancel: function (e) {
                    $("[id$=hdfIscontYes]").val(0);
                    $(this).dialog("close");
                    return false;
                }
            }
        });
        return false;
    }

    function GetEmpDocumentType(DocType) {
        var type = $("[id$=ddlDocumentTypeList]").val();
        var rfvIssuedOn = document.getElementById("<%=reqDocumentIssuedOn.ClientID%>");
        var rfvExpiresOn = document.getElementById("<%=reqDocumentExpiresOn.ClientID%>");
        if (type == 1 || type == 13) {

            $("[id$=txtDocumentIssuedOn]").attr("disabled", true);
            $("[id$=txtDocumentExpiresOn]").attr("disabled", true);
            ValidatorEnable(rfvIssuedOn, true);
            ValidatorEnable(rfvExpiresOn, true);
            var lblIssuedOn = '<%= GetLocalResourceObject("EmpDocumentIssuedOn").ToString() %>';
            var lblExpires = '<%= GetLocalResourceObject("EmpDocumentExpiresOnStar").ToString() %>';
            $("[id$=lblDocumentIssuedOn]").html(lblIssuedOn);
            $("[id$=lblDocumentExpiresOn]").html(lblExpires);
            $("[id$=txtDocumentIssuedOn]").attr("disabled", false);
            $("[id$=txtDocumentExpiresOn]").attr("disabled", false);
            $("[id$=txtDocumentNo]").focus();
        }
        else {
            ValidatorEnable(rfvIssuedOn, false);
            ValidatorEnable(rfvExpiresOn, false);
            var lblIssuedOn = '<%= GetLocalResourceObject("EmpDocumentIssuedOn1").ToString() %>';
            var lblExpires = '<%= GetLocalResourceObject("EmpDocumentExpiresOn").ToString() %>';
            $("[id$=lblDocumentIssuedOn]").html(lblIssuedOn);
            $("[id$=lblDocumentExpiresOn]").html(lblExpires);
        }
    }
</script>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="fixed-buttons" id="divNormalButtons" runat="server">
            <ucGtiTab:GtiTabControl ID="hrmsTab" runat="server" CurrentTab="6" />
            <div class="Button-container">
                <asp:Table ID="Table1" runat="server">
                    <asp:TableRow>
                        <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                            <ul class="bredcrum">
                                <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                            </ul>
                            <ul runat="server" id="pnlEntry" style="display: none">
                                <li runat="server" id="pnlSaveContinue" visible="false">
                                    <asp:Button runat="server" ID="btnSaveContinue" OnClick="ActionHandler" CommandName="SAVEANDCONTINUE"
                                        TabIndex="150" Text="<%$ resources:ErpRes,SaveContinue%>" ToolTip="<%$ resources:ErpRes,SaveContinue%>"
                                        ValidationGroup="Employee" OnClientClick="javascript:ValidatePageNow('Employee')"
                                        CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                </li>
                                <li runat="server" id="pnlSave">
                                    <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="150" Text="<%$resources:Controls,Save %>"
                                        OnClick="ActionHandler" OnClientClick="return ValidateEmpDocControl('save',this)"
                                        ValidationGroup="save" ToolTip="<%$resources:Controls,Save %>" CommandArgument="SEC_ActionPanel"
                                        SkinID="btnInner-Save" />
                                </li>
                                <li runat="server" id="pnlDelete">
                                    <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                        OnClick="ActionHandler" TabIndex="151" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete"
                                        ToolTip="<%$resources:Controls,Delete %>" />
                                </li>
                                <li>
                                    <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                        OnClick="ActionHandler" CommandName="CANCEL" TabIndex="152" CommandArgument="SEC_ActionPanel"
                                        SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" ValidationGroup="none"
                                        OnClientClick="Page_BlockSubmit = false;" />
                                </li>
                            </ul>
                            <ul runat="server" id="pnlListing" style="display: none">
                                <li id="pnlNew">
                                    <asp:Button runat="server" TabIndex="153" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                        Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                        ToolTip="<%$resources:Controls,New %>" />
                                </li>
                                <li id="pnlEdit">
                                    <asp:Button runat="server" TabIndex="154" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
                                        Text="<%$resources:Controls,Edit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit"
                                        ToolTip="<%$resources:Controls,Edit %>" />
                                </li>
                                <li id="pnlView">
                                    <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="155" Text="<%$resources:Controls,View %>"
                                        OnClick="ActionHandler" CommandArgument="SEC_ActionPanel" SkinID="btnInner-View"
                                        ToolTip="<%$resources:Controls,View %>" />
                                </li>
                                <li>
                                    <asp:Button runat="server" ID="btnListCancel" Text="<%$resources:Controls,Cancel %>"
                                        OnClick="ActionHandler" CommandName="GOHOME" TabIndex="156" CommandArgument="SEC_ActionPanel"
                                        SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                                </li>
                            </ul>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
        <div class="Button-container-popup" id="divPopupButtons" runat="server">
            <asp:Button runat="server" ID="btnApply" CommandName="APPLY" OnClick="ActionHandler"
                Text="<%$resources:Controls,Apply %>" ToolTip="<%$resources:Controls,Apply %>"
                CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" ValidationGroup="save"
                OnClientClick="return ValidateEmpDocControl('save',this)" TabIndex="157" />
            <asp:Button runat="server" ID="btnPopUpCancel" Text="<%$resources:Controls,Cancel %>"
                CssClass="popupclose" CommandName="POPUPCANCEL" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" TabIndex="158" />
        </div>
        <div class="content-wrapper">
            <div id="divEmpBasicInfo" runat="server">
                <ucBasicHdr:EmpBasicInfoControl ID="UCempBasicHdr" runat="server" />
            </div>
            <asp:HiddenField ID="hdfHdrDOB" runat="server"></asp:HiddenField>
            <asp:HiddenField ID="hdfSelectedItemPk" runat="server" Value="0" />
            <div class="tab-container-floating" id="divBtnListDtl" runat="server">
                <ul>
                    <li>
                        <asp:LinkButton runat="server" ID="lnkList" Text="<%$resources:PageNameRes,List %>"
                            CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="EMPDOCLIST"
                            CssClass="tab-active" Style="margin-top: -7px;"></asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                            CommandArgument="SEC_ActionPanel" OnClick="ActionHandler" CommandName="EMPDOCDETAIL"
                            CssClass="tab-inactive" Style="margin-top: -7px;"></asp:LinkButton>
                    </li>
                </ul>
            </div>
            <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks tablelayout">
                <asp:TableRow ID="PageAction_List" runat="server">
                    <asp:TableCell>
                        <div class="gridwrap" style="margin-top: -5px;">
                            <asp:GridView runat="server" ID="grdEmpDocList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                AllowSorting="True" OnSorting="ActionHandler" OnRowDataBound="ActionHandler"
                                AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" AutoPostBack="true"
                                OnCheckedChanged="ActionHandler">
                                <EmptyDataTemplate>
                                    <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                </EmptyDataTemplate>
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton CssClass="rdoSelection" TabIndex="7" runat="server" GroupName="SelectOne"
                                                ID="rbtSelect" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" AutoPostBack="true"
                                                OnCheckedChanged="ActionHandler" />
                                            <asp:HiddenField runat="server" ID="hdfDocPk" Value='<%# Eval(Resources.DataFieldRes.EmpDocPk) %>' />
                                            <asp:HiddenField runat="server" ID="hdfIsCheckedIn" Value='<%# Eval(Resources.DataFieldRes.IsCheckedIn) %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="1%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:EmpDocType %>" SortExpression="<%$ resources:DataFieldRes,EmpDocTypeText %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocType" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocTypeText).ToString())%>'
                                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocTypeText).ToString()),20)%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="18%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:EmpDocNo %>" SortExpression="<%$ resources:DataFieldRes,EmpDocNo %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocNo" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocNo).ToString())%>'
                                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocNo).ToString()),20)%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="true" HeaderText="<%$ resources:EmpDocIssuedBy %>" SortExpression="<%$ resources:DataFieldRes,EmpDocIssuedBy %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIssuedBy" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocIssuedBy).ToString())%>'
                                                Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocIssuedBy).ToString()),24)%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="24%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:EmpDocIssuedOn %>" SortExpression="<%$ Resources:DataFieldRes,EmpDocIssuedOn %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblIssuedOn" runat="server" Text='<%# Eval(Resources.DataFieldRes.EmpDocIssuedOn, Resources.Constants.HRMSDateFormatGrid) %>'
                                                ToolTip='<%# Eval(Resources.DataFieldRes.EmpDocIssuedOn, Resources.Constants.HRMSDateFormatGrid)%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="8%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:EmpDocExpiresOn %>" SortExpression="<%$ Resources:DataFieldRes,EmpDocExpiresOn %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblExpiresOn" runat="server" Text='<%# Eval(Resources.DataFieldRes.EmpDocExpiresOn, Resources.Constants.HRMSDateFormatGrid)  %>'
                                                ToolTip='<%# Eval(Resources.DataFieldRes.EmpDocExpiresOn, Resources.Constants.HRMSDateFormatGrid)  %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="9%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ resources:DaysLeft %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDaysLeft" runat="server" Text='<%# Eval(Resources.DataFieldRes.EmpDocDaysLeft)  %>'
                                                ToolTip='<%# Eval(Resources.DataFieldRes.EmpDocDaysLeft) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="8%" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval(Resources.DataFieldRes.StatusText) %>' />
                                            <div id="imbStatusIndicator" runat="server">
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle Width="4%" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lnkLog" runat="server" ImageUrl="~/Images/Classic/Icons/log.jpg"
                                                OnClick="ActionHandler" CommandName="SHOWLOG" TabIndex="19" ToolTip="Log" CommandArgument='<%# Eval(Resources.DataFieldRes.EmpDocPk) %>' />
                                            <asp:ImageButton ID="lnkCheckIn" ImageUrl="~/Images/Classic/Icons/check-in.jpg" runat="server"
                                                CommandName="CHECKIN" OnClick="ActionHandler" ToolTip="<%$resources:CheckIn %>"
                                                CommandArgument='<%# Eval(Resources.DataFieldRes.EmpDocPk) %>' />
                                            <asp:ImageButton ID="lnkCheckOut" ImageUrl="~/Images/Classic/Icons/check-out.jpg"
                                                runat="server" CommandName="CHECKOUT" OnClick="ActionHandler" ToolTip="<%$resources:CheckOut %>"
                                                CommandArgument='<%# Eval(Resources.DataFieldRes.EmpDocPk) %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="6%" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                    <asp:TableCell>
                        <table class="table-devide" runat="server" id="pnlEmpDocDetails">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label runat="server" ID="lblDocumentTypeList" AssociatedControlID="ddlDocumentTypeList"
                                            Text="<%$resources:EmpDocumentType %>"></asp:Label>
                                        <asp:DropDownList runat="server" ID="ddlDocumentTypeList" TabIndex="1" CssClass="select-w16per"
                                            onchange="GetEmpDocumentType(this)">
                                        </asp:DropDownList>
                                        <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="reqDocTypeList" CssClass="star" SetFocusOnError="true"
                                                InitialValue="-1" ValidationGroup="save" EnableClientScript="true" runat="server"
                                                ControlToValidate="ddlDocumentTypeList" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_DocTypeList %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                        <asp:Label runat="server" ID="lblDocumentNo" AssociatedControlID="txtDocumentNo"
                                            Text="<%$resources:EmpDocumentNo %>" CssClass="middle-lbl"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtDocumentNo" TabIndex="2" MaxLength="100" CssClass="input-w24-5per" />
                                        <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="reqDocNo" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtDocumentNo"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_DocNo %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <asp:Label runat="server" ID="lblDocRefNo" AssociatedControlID="txtDocRefNo" Text="<%$resources:EmpDocRefNo %>"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtDocRefNo" TabIndex="5" MaxLength="100" CssClass="input-w23-6per" />
                                        <asp:Label runat="server" ID="lblDocumentRefDate" AssociatedControlID="txtDocumentRefDate"
                                            CssClass="lbl-16perc" Text="<%$resources:EmpDocumentRefDate %>"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtDocumentRefDate" CssClass="input-small" TabIndex="6"
                                            onkeydown="return CheckKey(event)" onpaste="return false;" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label runat="server" ID="lblIssuedBy" AssociatedControlID="txtEmpDocIssuedBy"
                                            Text="<%$resources:EmpDocumentIssuedBy %>"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtEmpDocIssuedBy" TabIndex="3" MaxLength="100" CssClass="input-small" />
                                        <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="reqEmpDocIssuedBy" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtEmpDocIssuedBy"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EmpDocIssuedBy %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                        <asp:Label runat="server" ID="lblPlaceOfIssue" AssociatedControlID="txtPlaceOfIssue"
                                            Text="<%$resources:EmpPalceOfIssue %>" CssClass="lbl-21-5perc"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtPlaceOfIssue" TabIndex="4" MaxLength="100" CssClass="input-small" />
                                        <div class="clear">
                                        </div>
                                        <asp:Label runat="server" ID="lblDocumentIssuedOn" AssociatedControlID="txtDocumentIssuedOn"
                                            Text="<%$resources:EmpDocumentIssuedOn1 %>"></asp:Label>
                                        <asp:HiddenField ID="hdfDocumentEntryPageIssuedOn" runat="server" Value="" />
                                        <asp:TextBox runat="server" ID="txtDocumentIssuedOn" CssClass="input-w12-5per" TabIndex="7"
                                            onkeydown="return CheckKey(event)" onpaste="return false;" />
                                        <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="reqDocumentIssuedOn" CssClass="star" SetFocusOnError="true"
                                                Enabled="false" ValidationGroup="save" EnableClientScript="true" runat="server"
                                                ControlToValidate="txtDocumentIssuedOn" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_DocumentIssuedOn %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                        <asp:Label runat="server" ID="lblDocumentExpiresOn" AssociatedControlID="txtDocumentExpiresOn"
                                            Text="<%$resources:EmpDocumentExpiresOn %>" CssClass="lbl-13-3perc"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtDocumentExpiresOn" CssClass="input-w12-5per" TabIndex="8"
                                            onkeydown="return CheckKey(event)" onpaste="return false;" />
                                        <asp:HiddenField ID="hdfDocumentEntryPageExpiresOn" runat="server" Value="" />
                                        <asp:RequiredFieldValidator ID="reqDocumentExpiresOn" CssClass="star" SetFocusOnError="true"
                                            Enabled="false" ValidationGroup="save" EnableClientScript="true" runat="server"
                                            ControlToValidate="txtDocumentExpiresOn" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_DocumentExpiresOn %>">
                                        </asp:RequiredFieldValidator>
                                        <asp:Label Text="0 Days Left" runat="server" ID="lblExpiryDaysLeft" CssClass="input-small" />
                                        <asp:HiddenField runat="server" ID="hdfExpiryDaysLeft" Value="" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="divcol-S">
                                        <asp:Label ID="lblDocumentTitle" runat="server" Text="<%$ resources:EmpDocumentTitle %>"
                                            AssociatedControlID="txtDocumentTitle" CssClass="margn-rgt0"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtDocumentTitle" MaxLength="200" TabIndex="9"></asp:TextBox>
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <div class="divcol-S">
                                        <asp:Label ID="lblDocumetAddlInfo" runat="server" Text="<%$ resources:EmpDocumetAddlInfo %>"
                                            AssociatedControlID="txtDocumetAddlInfo" CssClass="margn-rgt0"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtDocumetAddlInfo" MaxLength="500" TabIndex="10"></asp:TextBox>
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <div class="divcol-S">
                                        <asp:Label ID="lblDocumentRemarks" runat="server" Text="<%$ resources:EmpDocumentRemarks %>"
                                            AssociatedControlID="txtDocumentRemarks" CssClass="margn-rgt0"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtDocumentRemarks" MaxLength="500" TabIndex="11"
                                            TextMode="MultiLine" CssClass="multiline-2col" onkeydown="limitText(this,500);"
                                            onkeyup="limitText(this,500);"></asp:TextBox>
                                    </div>
                                    <div class="clear">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <asp:Label runat="server" ID="lblIntimateBefore" AssociatedControlID="txtIntimateBefore"
                                            Text="<%$resources:IntimateBefore %>"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtIntimateBefore" CssClass="small" TabIndex="12"
                                            onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" />
                                        <%--  <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ErrorMessage="<%$ resources:Err_InitmateBefore %>" CssClass="star"
                                                SetFocusOnError="true" ToolTip="<%$ resources:Err_InitmateBefore %>" ControlToValidate="txtIntimateBefore"
                                                runat="server" ValidationExpression="<%$resources:Constants,PositiveNumberRegEx %>"
                                                ValidationGroup="save" Display="Dynamic" />--%>
                                        <span style="background: none; border: 0; height: 2px;">days</span>
                                        <asp:Label runat="server" ID="lblCheckedInBy" AssociatedControlID="txtAutoCheckedInBy"
                                            Text="<%$resources:EmpDocCheckedInBy %>" CssClass="lbl-16-5perc"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtAutoCheckedInBy" TabIndex="13" CssClass="select-medium" />
                                        <asp:HiddenField ID="hdfAutoCheckedInBy" runat="server" Value="0" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S-chk">
                                        <div class="clear">
                                        </div>
                                        <asp:Label runat="server" ID="lblOriginalSubmitted" AssociatedControlID="lblOriginalSubmitted"
                                            CssClass="margnrgt-minus1" Text="<%$resources:OriginalSubmitted %>"></asp:Label>
                                        <asp:CheckBox runat="server" ID="chkOriginalSubmitted" TabIndex="14" />
                                        <asp:ImageButton ID="btnCheckIn" ImageUrl="~/Images/Classic/Icons/check-in.jpg" ToolTip="<%$resources:CheckIn %>"
                                            runat="server" CommandName="CHECKIN" OnClick="ActionHandler" Visible="false" />
                                        <asp:ImageButton ID="btnCheckOut" ImageUrl="~/Images/Classic/Icons/check-out.jpg"
                                            ToolTip="<%$resources:CheckOut %>" runat="server" CommandName="CHECKOUT" OnClick="ActionHandler"
                                            Visible="false" />
                                        <asp:Label runat="server" ID="lblCheckedInOn" AssociatedControlID="txtCheckedInOn"
                                            Text="<%$resources:EmpDocCheckedInOn %>" CssClass="lbl-37-2perc"></asp:Label>
                                        <asp:TextBox runat="server" ID="txtCheckedInOn" CssClass="input-small" TabIndex="15"
                                            onkeydown="return CheckKey(event)" onpaste="return false;" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="divcol-S">
                                        <asp:Label ID="lblFileUpload" runat="server" Text="Attach File" AssociatedControlID="fupUpload"
                                            CssClass="margnrgt-minus1"></asp:Label>
                                        <div class="fileupload-main">
                                            <asp:FileUpload ID="fupUpload" runat="server" TabIndex="16" CssClass="margn-rgt0 upload-area"
                                                EnableViewState="false" />
                                            <%-- <div class="starwrap">--%>
                                            <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupUpload"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                                            </asp:RequiredFieldValidator>
                                            <%--  </div>--%>
                                        </div>
                                        <a id="anchorFile" runat="server" target="_blank"></a>
                                        <asp:Button runat="server" ID="btnAddItem" CommandName="ADDITEM" TabIndex="17" OnClick="ActionHandler"
                                            OnClientClick="javascript:ValidateEmpDocControl('upload')" ToolTip="<%$resources:ErpRes,Add %>"
                                            CommandArgument="PageAction_Entry" ValidationGroup="upload" Text="<%$resources:ErpRes,Add %>"
                                            SkinID="btnInner-add" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdUploads" Width="100%" PageSize="<%$ resources:PageSize%>"
                                            DataKeyNames="DOC_PK" AllowSorting="false" AllowPaging="false" OnSorting="ActionHandler"
                                            OnPageIndexChanging="ActionHandler" OnRowDataBound="ActionHandler" AutoGenerateColumns="false"
                                            EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                                    <ItemTemplate>
                                                        <%# Container.DataItemIndex + 1 %>
                                                        <asp:HiddenField runat="server" ID="hdfPK" Value='<%# Eval("DOC_PK") %>' />
                                                        <asp:HiddenField runat="server" ID="hdfSlNo" Value='<%# Container.DataItemIndex + 1 %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="4%" HorizontalAlign="Center" Wrap="false" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:File %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFile" runat="server" Text='<%# Eval("DOC_NAME") %>' ToolTip='<%# Eval("DOC_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="90%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <a runat="server" id="fileView" class="download-icon nomargin" title="View" target="_blank"
                                                            href='<%# Page.ResolveClientUrl(Eval("DOC_PATH").ToString()) %>'></a>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Button ID="lnkRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                            SkinID="delete-icon" ToolTip="Delete" CommandArgument="PageAction_Entry" OnClientClick="return ShowDeleteConfirm(this);"
                                                            OnPreRender="btnAction_PreRender" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="divEmpDocError" style="display: none">
                            <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                            <asp:ValidationSummary ID="vsPage" ValidationGroup="upload" runat="server" />
                            <asp:ValidationSummary ID="vsPageSave" ValidationGroup="save" runat="server" />
                        </div>
                    </asp:TableCell></asp:TableRow>
                <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                    <asp:TableCell>
                        <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                    </asp:TableCell></asp:TableRow>
            </asp:Table>
            <div id="divCheckInCheckOutLog" style="display: none;">
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
                                    <asp:Label ID="lblExpiresOn" runat="server" ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckInOutRemarks).ToString())  %>'
                                        Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.EmpDocCheckInOutRemarks).ToString()),20)%>'></asp:Label></ItemTemplate>
                                <ItemStyle Width="22%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="divCheckInControlContainer" style="display: none;">
                <ucChkIn:CheckInControl ID="CheckInControl1" runat="server" OnAfterSave="ResetForm"
                    OnLogClosed="ActionHandler" />
            </div>
            <div id="divCheckOutControlContainer" style="display: none;">
                <ucChkOut:CheckOutControl ID="CheckOutControl1" runat="server" OnAfterSave="ResetForm"
                    OnError="ActionHandler" OnLogClosed="ActionHandler" />
            </div>
            <asp:HiddenField ID="hdfIscontYes" runat="server" Value="0" />
            <asp:HiddenField runat="server" ID="hdfCurrentDepartment" Value="-1" />
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnAddItem" />
    </Triggers>
</asp:UpdatePanel>
