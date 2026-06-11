<%@ Page Title="" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="BankGuarantee.aspx.cs" Inherits="ERPSMS_v01.Finance.Administration.Masters.BankGuarantee"
    Theme="ClassicExt" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtBank", url + "&Type=4", "hdfBank", true, true, "BANK");
            GrandScriptUtils.MakeAutoCompleteDDL("txtParty", url, "hdfParty", true, true, "VENDOR");
            GrandScriptUtils.DatePickerCommon("txtDate");
            GrandScriptUtils.DatePickerCommon("txtRefDate");
            GrandScriptUtils.DatePickerCommon("txtExpiryDate");
            GrandScriptUtils.DatePickerCommon("txtClaimDate");
            GrandScriptUtils.AddDateRangeCommon("txtPeriod", "hdfPeriod", "txtPeriodTo", "hdfPeriodTo", "dd-M-yy", false, false, false);
            $("[id*=txtAmount]").ForceNumericOnly();
            FilterByChange();
        }
        function FilterByChange() {
            $("[id$=txtSearchBy]").val('Select/Type');
            $("[id$=hdfSearchBy]").val('');
            if ($("[id$=ddlFilterBy]").val() == "Bank") {
                GrandScriptUtils.MakeAutoCompleteDDL("txtSearchBy", url + "&Type=4", "hdfSearchBy", true, true, "BANK");
            }
            if ($("[id$=ddlFilterBy]").val() == "Party") {
                GrandScriptUtils.MakeAutoCompleteDDL("txtSearchBy", url, "hdfSearchBy", true, true, "VENDOR");
            }
        }
        function ShowListing(flag) {
            if (flag) {
                $("[id$='PageAction_Entry']").hide();
                $("[id$='PageAction_List']").show();
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
        function SetTabs(tab) {
            if (tab == 1) {
                $("[id$='spnBankListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnBankListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnBankDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnBankDetails']").removeClass("tab-active").addClass("tab-inactive");
            }
            else {
                $("[id$='spnBankListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnBankListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnBankDetails']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnBankDetails']").removeClass("tab-inactive").addClass("tab-active");
            }
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
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlBank" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="tblButton" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label ID="lblBreadCrum" runat="server" />
                                </ul>
                                <ul id="pnlEntry" runat="server" style="display: none">
                                    <li id="pnlSave" runat="server">
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            OnClientClick="javascript:ValidatePageNow('save')" ValidationGroup="save" SkinID="btnInner-Save"
                                            TabIndex="97" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <li id="pnlDelete" runat="server">
                                        <asp:Button ID="btnDelete" runat="server" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClick="ActionHandler"
                                            TabIndex="98" ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$resources:Controls,Cancel %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClick="ActionHandler"
                                            TabIndex="99" ToolTip="<%$ resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="NEW" Text="<%$ resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$ resources:Controls,New %>"
                                            OnClick="ActionHandler" TabIndex="5" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="EDIT" Text="<%$ resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$ resources:Controls,Edit %>"
                                            OnClick="ActionHandler" TabIndex="6" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnView" runat="server" CommandName="VIEW" Text="<%$ resources:Controls,View %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-View" ToolTip="<%$ resources:Controls,View %>"
                                            OnClick="ActionHandler" TabIndex="7" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnBankListing" runat="server" class="tab-active">
                            <asp:LinkButton ID="lbnListing" runat="server" Text="<%$ resources:Controls,List %>"
                                CommandName="CANCEL" CssClass="tab-active" OnClick="ActionHandler" TabIndex="8" />
                        </span></li>
                        <li><span id="spnBankDetails" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnDetails" runat="server" Text="<%$ resources:Controls,Details %>"
                                CommandName="DETAILS" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="9" />
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-wrap-custom">
                                <asp:Label ID="lblFilterBy" runat="server" Text="<%$ resources:Controls,FilterBy %>"
                                    AssociatedControlID="ddlFilterBy" />
                                <asp:DropDownList ID="ddlFilterBy" runat="server" TabIndex="1" onChange="javascript:FilterByChange()">
                                    <asp:ListItem Text="<%$ resources:SearchByBank %>" Value="<%$ resources:SearchByBank %>" />
                                    <asp:ListItem Text="<%$ resources:SearchByParty %>" Value="<%$ resources:SearchByParty %>" />
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchBy" runat="server" onkeydown="return Search(event);" TabIndex="2" CssClass="input-w41-5per" />
                                <asp:RequiredFieldValidator ID="vrfSearchBy" CssClass="star" SetFocusOnError="false"
                                    ValidationGroup="search" EnableClientScript="true" runat="server" ControlToValidate="txtSearchBy"
                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Item %>" InitialValue="Select/Type">
                                </asp:RequiredFieldValidator>
                                <asp:HiddenField runat="server" ID="hdfSearchBy" />
                                <asp:ImageButton ID="btnSearch" SkinID="search-ext" runat="server" ToolTip="<%$ resources:Controls,Go %>" CommandName="SEARCH" OnClick="ActionHandler"
                                    TabIndex="3" OnClientClick="javascript:ValidatePageNow('search')" />
                            </div>
                            <div class="gridwrap">
                                <asp:GridView runat="server" ID="grdList" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    onclick="GrandScriptUtils.EnableRbtnGrouping(this);" TabIndex="4" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrdNo%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblNo" runat="server" Text='<%# Eval("BGM_NO")!=null? Eval("BGM_NO").ToString():string.Empty %>'
                                                    ToolTip='<%# Eval("BGM_NO")!=null? Eval("BGM_NO").ToString():string.Empty %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrdDate%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDate" runat="server" Text='<%#Eval("BGM_DATE")!=null? Eval("BGM_DATE",Resources.Constants.DateFormatGrid):string.Empty %>'
                                                    ToolTip='<%#Eval("BGM_DATE")!=null? Eval("BGM_DATE",Resources.Constants.DateFormatGrid):string.Empty %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrdParty%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblParty" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("BGM_PARTY_TEXT"),48) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("BGM_PARTY_TEXT")) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="33%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrdBank%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBank" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("BGM_BANK_TEXT"),44) %>'
                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("BGM_BANK_TEXT")) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="26%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrdAmount%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("BGM_AMOUNT", "{0:c}") %>'
                                                    ToolTip='<%# Eval("BGM_AMOUNT", "{0:c}") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" HorizontalAlign="Right" />
                                            <HeaderStyle HorizontalAlign="Right" CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrdExpiryDate%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpiryDate" runat="server" Text='<%#Eval("BGM_EXP_DATE")!=null? Eval("BGM_EXP_DATE",Resources.Constants.DateFormatGrid):string.Empty %>'
                                                    ToolTip='<%#Eval("BGM_EXP_DATE")!=null? Eval("BGM_EXP_DATE",Resources.Constants.DateFormatGrid):string.Empty %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:GrdStatus%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("BGM_ACTIVE").ToString() == "1" ? "Active":"Inactive" %>'
                                                    ToolTip='<%# Eval("BGM_ACTIVE").ToString() == "1" ? "Active":"Inactive" %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblNo" runat="server" Text="<%$ resources:BGNo%>" AssociatedControlID="lblNoText" />
                                            <asp:Label ID="lblNoText" runat="server" Text="<%$ resources:NewNo%>" CssClass="input-small" />
                                            
                                            <asp:Label ID="lblDate" runat="server" Text="<%$ resources:BGDate%>" CssClass="middle-lbl-c" AssociatedControlID="txtDate" />
                                            <asp:TextBox ID="txtDate" runat="server" MaxLength="15" TabIndex="11" onkeydown="return CheckKey(event)" CssClass="input-small"
                                                onpaste="return false;" />
                                            <asp:RequiredFieldValidator ID="vrftxtDate" CssClass="star" SetFocusOnError="false"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Date %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblRefNo" runat="server" Text="<%$ resources:BGRefNo%>"  AssociatedControlID="txtRefNo" />
                                            <asp:TextBox ID="txtRefNo" runat="server" MaxLength="190" TabIndex="11" onkeydown="limitText(this,190);"  CssClass="input-small"
                                                onkeyup="limitText(this,190);" />

                                       <asp:Label ID="lblRefDate" runat="server" Text="<%$ resources:BGRefDate%>" CssClass="middle-lbl-c" AssociatedControlID="txtRefDate" />
                                            <asp:TextBox ID="txtRefDate" runat="server" MaxLength="15" TabIndex="11" onkeydown="return CheckKey(event)" CssClass="input-small"
                                                onpaste="return false;" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblParty" runat="server" Text="<%$ resources:BGParty%>" AssociatedControlID="txtParty" />
                                            <asp:TextBox ID="txtParty" runat="server" TabIndex="11" CssClass="input-half" />
                                            <asp:HiddenField runat="server" ID="hdfParty" />
                                            <asp:RequiredFieldValidator ID="vrfParty" CssClass="star" SetFocusOnError="false"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtParty"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Party %>" InitialValue="Select/Type">
                                            </asp:RequiredFieldValidator>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblAmount" runat="server" Text="<%$ resources:BGAmount%>" AssociatedControlID="txtAmount" />
                                            <asp:TextBox ID="txtAmount" runat="server" MaxLength="15" TabIndex="11" onkeyup="limitText(this,12);" 
                                                onkeydown="limitText(this,12);" CssClass="input-small numeric"/>
                                            <asp:RequiredFieldValidator ID="vrftxtAmount" CssClass="star" SetFocusOnError="false"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtAmount"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Amount %>">
                                            </asp:RequiredFieldValidator>

                                            <asp:Label ID="lblExpiryDate" runat="server" Text="<%$ resources:BGExpiryDate%>" CssClass="middle-lbl-c"
                                                AssociatedControlID="txtExpiryDate" />
                                            <asp:TextBox ID="txtExpiryDate" runat="server" MaxLength="15" TabIndex="11" CssClass="input-small" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBank" runat="server" Text="<%$ resources:BGBank%>" AssociatedControlID="txtBank" />
                                            <asp:TextBox ID="txtBank" runat="server" TabIndex="11" CssClass="input-half" />
                                            <asp:HiddenField runat="server" ID="hdfBank" Value="0" />
                                            <asp:RequiredFieldValidator ID="vrfBank" CssClass="star" SetFocusOnError="false"
                                                ValidationGroup="save" EnableClientScript="true" runat="server" ControlToValidate="txtBank"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Bank %>" InitialValue="Select/Type">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                             <asp:Label ID="lblPeriod" runat="server" Text="<%$ resources:BGPeriod%>" AssociatedControlID="txtPeriod" />
                                            <asp:TextBox ID="txtPeriod" runat="server" MaxLength="15" TabIndex="11"  CssClass="input-small"
                                                onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:HiddenField runat="server" ID="hdfPeriod" />
                                            &nbsp;&nbsp;
                                            <asp:Label runat="server" ID="lblPeriodTo"  Text="<%$ resources:BGTo%>" CssClass="middle-lbl-c"
                                                AssociatedControlID="txtPeriodTo"></asp:Label>
                                            <asp:TextBox ID="txtPeriodTo" runat="server" MaxLength="15" TabIndex="11" Width="108px" CssClass="input-small"
                                                onkeydown="return CheckKey(event)" onpaste="return false;" />
                                            <asp:HiddenField runat="server" ID="hdfPeriodTo" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                           <asp:Label ID="lblClaimDate" runat="server" Text="<%$ resources:BGClaimDate%>" AssociatedControlID="txtClaimDate" />
                                            <asp:TextBox ID="txtClaimDate" runat="server" CssClass="input-small" MaxLength="15" TabIndex="11" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" />

                                           <asp:Label ID="lblStatus" runat="server" CssClass="middle-lbl-a" Text="<%$ resources:BGStatus%>" AssociatedControlID="ddlStatus" />
                                            <asp:DropDownList runat="server" ID="ddlStatus" CssClass="select-small-b" TabIndex="11">
                                                <asp:ListItem Selected="True" Text="Active" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblRemarks" runat="server" Text="<%$ resources:BGRemarks%>" AssociatedControlID="txtRemarks" />
                                            <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" MaxLength="500"
                                                TabIndex="11" CssClass="multiline-3line" onkeydown="limitText(this,490);" onkeyup="limitText(this,490);" />
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </td>
                                </tr>                               
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="save" runat="server" />
                    <asp:ValidationSummary ID="vsPageSearch" ValidationGroup="search" runat="server" />
                </div>
                <asp:HiddenField ID="hdfNumberDigits" runat="server" />
                <asp:HiddenField ID="hdfCurrencyDigits" runat="server" />
                <asp:HiddenField ID="hdfExchangeDigits" runat="server" />
                <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
                <asp:HiddenField ID="hdfRateFormat" runat="server" />
                <asp:HiddenField ID="hdfDecimalDigits" Value="0" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
