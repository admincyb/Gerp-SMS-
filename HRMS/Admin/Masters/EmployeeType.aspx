<%@ Page Title="<%$ Resources:Captions,Title_EmployeeType %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="EmployeeType.aspx.cs" Theme="ClassicExt" ValidateRequest="false"
    Inherits="HRMS.Admin.Masters.EmployeeType" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function InitPage() {
            // Do any page initialization here.
            $("[id*=txtWorkingDays]").ForceNumericOnly();
            $("[id*=txtOTRate]").ForceNumericOnly();
        }
        $(document).ready(function () {
            $("[id*=chkOTAvailable]").live("click", function () {
                var chkOTAvailable = $(this);
                var validator = document.getElementById("<%= rfvOTTemplate.ClientID %>");
                if (chkOTAvailable.is(":checked")) {
                    //$("[id*=divOtTemplate]").show();
                    $("[id*=ddlOTTemplate]").attr('disabled', false);
                    ValidatorEnable(validator, true);
                } else {
                    //$("[id*=divOtTemplate]").hide();
                    $("[id*=ddlOTTemplate]").attr('disabled', true);
                    $("select[id$=ddlOTTemplate]").val(-1);
                    ValidatorEnable(validator, false);
                }
            });

        });

        function WorkingDayTypeChange() {
            var validator = document.getElementById("<%= vrfWorkingDays.ClientID %>");
            var WorkingDayType = $("[id*=ddlWorkingDayType]").val();
            if (WorkingDayType == 1) {
                //$("[id*=divWorkingDay]").show();
                $("[id*=txtWorkingDays]").attr('disabled', false);
                $("[id*=txtWorkingDays]").removeClass("input-disabled");
                ValidatorEnable(validator, true);
            }
            else {
                //$("[id*=divWorkingDay]").hide();
                $("[id*=txtWorkingDays]").attr('disabled', true);
                $("[id*=txtWorkingDays]").addClass("input-disabled");
                $("[id*=txtWorkingDays]").val('');
                ValidatorEnable(validator, false);
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
    <asp:UpdatePanel ID="auplDetailList" runat="server">
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
                                            ToolTip="<%$ resources:Controls,Save%>" OnClick="ActionHandler" ValidationGroup="save"
                                            OnClientClick="javascript:ValidatePageNow('Save')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" TabIndex="2" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$Resources:Controls,Delete%>"
                                            ToolTip="<%$Resources:Controls,Delete%>" OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" TabIndex="2" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCanel" Text="<%$ resources:Controls,Cancel%>" ToolTip="<%$ resources:Controls,Cancel%>"
                                            OnClick="ActionHandler" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" TabIndex="2" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="0" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="1" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
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
                                CommandArgument="SEC_ActionPanel" TabIndex="1" OnClick="ActionHandler" CommandName="LIST"
                                CssClass="tab-active"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton runat="server" ID="lnkDetail" Text="<%$resources:PageNameRes,Detail %>"
                                CommandArgument="SEC_ActionPanel" TabIndex="1" OnClick="ActionHandler" CommandName="DETAIL"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
                <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
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
                                            <asp:Label ID="lblFilterCode" runat="server" Text="<%$ resources:FilterCode%>" AssociatedControlID="txtFilterCode"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterCode" TabIndex="3" CssClass="input-half margnbotm0"></asp:TextBox>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label ID="lblFilterName" runat="server" Text="<%$ resources:FilterName%>" AssociatedControlID="txtFilterName"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFilterName" TabIndex="3" CssClass="input-half margnbotm0"></asp:TextBox>
                                            <asp:ImageButton ID="btnSearch1" runat="server" Text="<%$ resources:Controls,Search %>"
                                                ToolTip="<%$resources:Controls,Search %>" OnClick="ActionHandler" TabIndex="4"
                                                CommandName="FILTER" SkinID="search-ext" CssClass="margntop2 margnbotm0" />
                                            <asp:ImageButton ID="btnClear1" runat="server" Text="<%$ resources:Controls,Clear %>"
                                                ToolTip="<%$resources:Controls,Clear %>" TabIndex="5" OnClick="ActionHandler"
                                                CommandName="CLEAR" SkinID="clear-ext" CssClass="margntop2 margnbotm0" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" PageSize="<%$ resources:PageSize%>"
                                    AllowSorting="True" OnSorting="ActionHandler" AutoGenerateColumns="false" OnPageIndexChanging="ActionHandler"
                                    EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    TabIndex="6" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfEmpTypePK" Value='<%# Eval("EMT_PK") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GridTypeCode%> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTypeCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMT_CODE")),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMT_CODE")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="17%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GridTypeName%>  ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTypeName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMT_NAME")),60) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMT_NAME")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GridLeaveTemplate%> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOTTemplate" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMT_LEAVE_TEMP_TEXT")),20) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMT_LEAVE_TEMP_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="15%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GridOTTemplate%> ">
                                            <ItemTemplate>
                                                <asp:Label ID="lblLeaveTemplate" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("EMT_OT_TEMP_TEXT")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("EMT_OT_TEMP_TEXT")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status%> " ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="5%">
                                            <%--<%$ resources:Active %>--%>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("EMT_ACTIVE").ToString() == "0") ?
                                               true  : false %>' CommandName="ACTIVATE" ToolTip="Inactive" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="7" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("EMT_ACTIVE").ToString() == "1") ?
                                               true  : false %>' CommandName="DEACTIVATE" ToolTip="Active" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="7" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="true" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide" style="table-layout: fixed;">
                                <tr>
                                    <td>
                                    </td>
                                    <td style="padding-left: 100px;">
                                        <asp:Label ID="lblWorkingHrs" runat="server" Text="<%$ resources:StdWorkingHrs%>"
                                            AssociatedControlID="grdWorkingDays"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCode" runat="server" Text="<%$ resources:TypeCodeReq%>" AssociatedControlID="txtCode"></asp:Label>
                                            <asp:TextBox ID="txtCode" runat="server" MaxLength="100" TabIndex="1" CssClass="input-half"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtCode"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Code%>"></asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblName" runat="server" Text="<%$ resources:TypeNameReq%>" AssociatedControlID="txtName"></asp:Label>
                                            <asp:TextBox ID="txtName" runat="server" MaxLength="200" TabIndex="1" CssClass="input-half"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Name%>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblLeaveTemplate" runat="server" Text="<%$ resources:GridLeaveTemplate%>"
                                                AssociatedControlID="ddlLeaveTemplate"></asp:Label>
                                            <asp:DropDownList ID="ddlLeaveTemplate" TabIndex="1" runat="server" CssClass="select-half-a">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvLeaveTemplate" runat="server" ControlToValidate="ddlLeaveTemplate"
                                                InitialValue="-1" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_LeaveTemplate%>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblOTAvailable" runat="server" Text="<%$ resources:OTAvailable%>"
                                                AssociatedControlID="lblOTAvailable"></asp:Label>
                                            <asp:CheckBox runat="server" ID="chkOTAvailable" TabIndex="1" />
                                            <%--  <div id="divOtTemplate" style="display: none; width:400px; float:right;" runat="server">--%>
                                            <asp:Label ID="lblOTTemplate" runat="server" Text="<%$ resources:OTTemplateReq%>"
                                                AssociatedControlID="ddlOTTemplate" CssClass="lbl-22-8perc"></asp:Label>
                                            <asp:DropDownList ID="ddlOTTemplate" TabIndex="1" runat="server" CssClass="lbl-34-9perc">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvOTTemplate" runat="server" ControlToValidate="ddlOTTemplate"
                                                Enabled="false" InitialValue="-1" CssClass="star" ValidationGroup="Save" Text="*"
                                                ErrorMessage="<%$ resources:Err_OTTemplate%>">
                                            </asp:RequiredFieldValidator>
                                            <%-- </div>--%>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblWorkingDayType" runat="server" Text="<%$ resources:WorkingDayTypeReq%>"
                                                AssociatedControlID="ddlWorkingDayType"></asp:Label>
                                            <asp:DropDownList ID="ddlWorkingDayType" TabIndex="1" runat="server" CssClass="select-small-a2"
                                                onchange="WorkingDayTypeChange()">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvWorkingDayType" runat="server" ControlToValidate="ddlWorkingDayType"
                                                Display="Static" InitialValue="-1" CssClass="star" ValidationGroup="Save" Text="*"
                                                ErrorMessage="<%$ resources:Err_ddlWorkingDayType%>">
                                            </asp:RequiredFieldValidator>
                                            <%--<div id="divWorkingDay" style="display: none;width:400px; float:right;" runat="server">--%>
                                            <asp:Label ID="lblWorkingDays" runat="server" Text="<%$ resources:WorkingDays%>"
                                                AssociatedControlID="txtWorkingDays" CssClass="lbl-18-9perc"></asp:Label>
                                            <asp:TextBox ID="txtWorkingDays" runat="server" TabIndex="1" CssClass="input-w18-3per numeric"
                                                MaxLength="4"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfWorkingDays" runat="server" ControlToValidate="txtWorkingDays"
                                                Display="Static" Enabled="false" CssClass="star" ValidationGroup="Save" Text="*"
                                                ErrorMessage="<%$ resources:Err_WorkingDays%>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="vrgWorkingDays" runat="server" ControlToValidate="txtWorkingDays"
                                                ErrorMessage="Invalid Working days" SetFocusOnError="true" EnableClientScript="true"
                                                CssClass="star padgrgt0 padglft0" Text="*" MinimumValue="0" MaximumValue="99"
                                                Type="Double" Display="Static" ValidationGroup="Save"></asp:RangeValidator>
                                            <%-- </div>--%>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblNormalWorkingHrs" runat="server" Text="<%$ resources:NormalWorkingHrsReq%>"
                                                AssociatedControlID="txtNormalWorkingHrs"></asp:Label>
                                            <asp:TextBox ID="txtNormalWorkingHrs" runat="server" MaxLength="5" TabIndex="1" CssClass="input-w18-3per numeric"
                                                onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfNormalWorkingHrs" runat="server" ControlToValidate="txtNormalWorkingHrs"
                                                Display="Static" CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_NormalWorkingHrs%>">
                                            </asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtNormalWorkingHrs"
                                                ErrorMessage="Invalid Normal Working Hrs" SetFocusOnError="true" EnableClientScript="true"
                                                CssClass="star" Text="*" MinimumValue="0" MaximumValue="24" Type="Double" Display="Static"
                                                ValidationGroup="Save"></asp:RangeValidator>
                                            <asp:Label ID="lblOtRate" runat="server" Text="<%$ resources:OTRate%>" AssociatedControlID="txtOTRate"
                                                CssClass="middle-lbl-xsmall-a"></asp:Label>
                                            <asp:TextBox ID="txtOTRate" runat="server" CssClass="input-w18-3per numeric" MaxLength="8"
                                                TabIndex="1"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfOTRate" runat="server" ControlToValidate="txtOTRate"
                                                Display="Static" CssClass="star" Enabled="false" ValidationGroup="Save" Text="*"
                                                ErrorMessage="<%$ resources:Err_OTRate%>">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" Text="<%$ resources:BreakHour%>" AssociatedControlID="txtBreakTime"
                                                CssClass="lbl-25-1perc"></asp:Label>
                                            <asp:TextBox ID="txtBreakTime" runat="server" CssClass="input-w18-3per numeric" MaxLength="4"
                                                TabIndex="1" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"> </asp:TextBox>
                                            <%-- <asp:Label ID="Label4" runat="server" Text="<%$ resources:Min%>" AssociatedControlID="txtBreakTime"></asp:Label>
                                            <%--<asp:Label ID="lblActive" runat="server" AssociatedControlID="lblActive" Text="<%$ resources:Active%>"></asp:Label>--%>
                                            <%--<asp:CheckBox ID="chkActive" runat="server" TabIndex="1" Checked="true" />--%>
                                            <asp:Label ID="Label1" runat="server" Text="<%$ resources:(Min)%>" AssociatedControlID="txtBreakTime"
                                                CssClass="lbl-3-7perc margntop4"></asp:Label>
                                        </div>
                                    </td>
                                    <td style="padding-left: 100px;">
                                        <div class="div2col-S gridwrap">
                                            <%--<br />--%>
                                            <div style="display: inline-block;">
                                                <asp:GridView runat="server" ID="grdWorkingDays" AllowPaging="false" AutoGenerateColumns="false"
                                                    EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable">
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
                                                            <ItemStyle Width="120px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$resources:GridHrs%>">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtHrs" runat="server" CssClass="small numeric margn-rgt0" TabIndex="1"
                                                                    Text='<%# Eval("Hours") %>' onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);"
                                                                    MaxLength="5">
                                                                </asp:TextBox>
                                                                <asp:RangeValidator ID="vrfTxtHrs" runat="server" ControlToValidate="txtHrs" ErrorMessage="Invalid Std. Working Hrs"
                                                                    SetFocusOnError="true" EnableClientScript="true" CssClass="star padgrgt0 padglft0"
                                                                    Text="*" MinimumValue="0" MaximumValue="24" Type="Double" Display="Static" ValidationGroup="Save"></asp:RangeValidator>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="63px" CssClass="bg-disabled txt-center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <%--<div class="div2col-S" style="display:none"> ErrorMessage="Invalid Std. Working Hrs"
                                            <asp:Label ID="lblBatchOrLocation" runat="server" Text="<%$ resources:BranchchOrLocation%>"
                                                AssociatedControlID="ddlBatchOrLocation"></asp:Label>
                                            <asp:DropDownList ID="ddlBatchOrLocation" TabIndex="1" runat="server" AutoPostBack="false"
                                                OnSelectedIndexChanged="ActionHandler" CommandName="BRANCHCHANGED">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvBatchOrLocation" runat="server" ControlToValidate="ddlBatchOrLocation"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_BranchOrLocation%>">
                                            </asp:RequiredFieldValidator>
                                        </div>--%>
                                    </td>
                                </tr>
                                <%--<tr runat="server" id="rowWorkingDays">
                                    <td valign="top">
                                    </td>
                                    <td>
                                    </td>
                                </tr>--%>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server"/>
            </div>
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormatWithComma" runat="server" />
            <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
