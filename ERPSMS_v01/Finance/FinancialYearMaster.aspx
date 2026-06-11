<%@ Page Title="<%$ Resources:Captions,Title_FinanacialYearMaster %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="FinancialYearMaster.aspx.cs"
    Inherits="ERPSMS_v01.Finance.FinancialYearMaster" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            $("[id*=txtRate]").ForceNumericOnly();
            $("[id*=txtMaxEligibility]").ForceNumericOnly();
            $("[id*=txtFactor]").ForceNumericOnly();
            ShowHideAdvancedSearch(1);
            CarryForwardChange();
            CreditCheckboxChange();
        }
        function CarryForwardChange() {
            if ($("[id*=chkCarryForward]").attr('checked')) {
                $("[id*=txtLimit]").removeAttr('disabled');
                $("[id*=txtLimit]").removeClass("input-disabled");
            } else {
                $("[id*=txtLimit]").attr('disabled', 'disabled');
                $("[id*=txtLimit]").addClass("input-disabled");
                $("[id*=txtLimit]").val("0");
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
        function RestrictDate() {
            GrandScriptUtils.RestrictedDatePicker("txtDate", false, true, true, $("[id$=txtFromDate]").val(), $("[id$=txtToDate]").val());
        }
        function AfterDateSelect(controlID) {
            if (controlID == "txtFromDate" || controlID == "txtToDate") {
                RestrictDate();
            }
            else if (controlID == "txtDate") {
                BindEmployee();
                ResetEmployee();
            }
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


        function CreditCheckboxChange() {
            var chkCredit = $("[id*=chkCredit]");
            if (chkCredit.is(":checked")) {
                $("[id*=ddlAutoCrType]").attr('disabled', false);
                $("[id*=txtMaxEligibility]").removeAttr('disabled');
                $("[id*=txtMaxEligibility]").removeClass("input-disabled");
            } else {
                $("[id*=ddlAutoCrType]").attr('disabled', true);
                $("select[id$=ddlAutoCrType]").val(0);
                $("[id*=txtMaxEligibility]").attr('disabled', 'disabled');
                $("[id*=txtMaxEligibility]").addClass("input-disabled");
                $("[id*=txtMaxEligibility]").val("0");
            }
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
                                    <asp:Label runat="server" ID="lblBreadCrum" Text="<%$resources:Breadcrumb%>"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry" style="display: none">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" Text="<%$ resources:Controls,Save%>"
                                            ToolTip="<%$ resources:Controls,Save%>" OnClick="ActionHandler" ValidationGroup="save"
                                            OnClientClick="javascript:ValidatePageNow('Save')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Save" TabIndex="52" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" Text="<%$Resources:Controls,Delete%>"
                                            ToolTip="<%$Resources:Controls,Delete%>" OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" TabIndex="53" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCanel" Text="<%$ resources:Controls,Cancel%>" ToolTip="<%$ resources:Controls,Cancel%>"
                                            OnClick="ActionHandler" CommandName="CANCEL" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" TabIndex="54" />
                                    </li>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                    <li>
                                        <asp:Button runat="server" TabIndex="7" ID="btnNew" CommandName="NEW" OnClick="ActionHandler"
                                            Text="<%$resources:Controls,New %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-New"
                                            ToolTip="<%$resources:Controls,New %>" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="8" ID="btnEdit" CommandName="EDIT" OnClick="ActionHandler"
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
                           <%-- <div class="search-colapse" id="divAdvanceSearch" style="margin-top: 0px;">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:ShowFilter%>" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-active.png" ToolTip="<%$ resources:HideFilter%>" />
                                        </td>
                                    </tr>
                                </table>
                            </div>--%>
                            <div class="clear">
                            </div>
            
                       
                            <div class="clear">
                            </div>
                            <div class="grdTable">
                                <asp:GridView runat="server" ID="grdList" Width="100%" PageSize="<%$resources:PageSize%>"
                                    AllowSorting="True" CssClass="grdTable" AutoGenerateColumns="false" EmptyDataRowStyle-HorizontalAlign="Center"
                                    EmptyDataRowStyle-CssClass="emptytable" OnPageIndexChanging="ActionHandler" OnSorting="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:RadioButton ID="rbtSelect" runat="server" CssClass="rdoSelection" GroupName="SelectOne"
                                                    TabIndex="0" AutoPostBack="true" OnCheckedChanged="ActionHandler" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                <asp:HiddenField runat="server" ID="hdfLeavePk" Value='<%# Eval("FYR_PK") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Width="3%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Name%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("FYR_NAME")),15) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("FYR_NAME")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="22%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Desc%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("FYR_DESC")),40) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("FYR_DESC")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>                                                                 
                                        <asp:TemplateField HeaderText="<%$ resources:FromDate%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemAccural" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("FYR_DATE_FROM","{0:dd/MM/yyyy}")),30) %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("FYR_DATE_FROM")))%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ToDate%>" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemMaxEligibility" runat="server" Text='<%#  ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval("FYR_DATE_TO","{0:dd/MM/yyyy}")),30)  %>'
                                                    ToolTip='<%# System.Web.HttpUtility.HtmlDecode(Convert.ToString(Eval("FYR_DATE_TO"))) %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" CssClass="txtAlign-right" />
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                            </ItemTemplate>
                                            <HeaderStyle Width="7%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Active %>" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-Width="5%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imbActive" runat="server" SkinID="btninactive" Visible='<%# (Eval("FYR_ACTIVE").ToString() == "0") ?
                                               true  : false %>' CommandName="ACTIVATE" ToolTip="Inactive" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="7" />
                                                <asp:ImageButton ID="imbInActive" runat="server" SkinID="btnactive" Visible='<%# (Eval("FYR_ACTIVE").ToString() == "1") ?
                                               true  : false %>' CommandName="DEACTIVATE" ToolTip="Active" OnClick="ActionHandler"
                                                    CssClass="Active" TabIndex="7" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="<%$ resources:Active%>  " SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemActive" runat="server" Text='<%# Eval("LTM_ACTIVE")!=null?Eval("LTM_ACTIVE").ToString()==ERP.Utilities.CommonConstants.SELECT_VALUE_ONE?GetLocalResourceObject("Active").ToString(): GetLocalResourceObject("Inactive").ToString() :string.Empty %>'
                                                    ToolTip='<%# Eval("LTM_ACTIVE")%>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%" />
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" />
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
                                         <asp:Label ID="lblFromDate" runat="server" Text="<%$ resources:FromDate%>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="input-small" TabIndex="1"
                                                MaxLength="11" ValidationGroup="Save" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFromDate" runat="server" ControlToValidate="txtFromDate"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_FromDate%>"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                             </div>
                                    </td>
                                     <td>
                                        <div class="div2col-S">

                                             <asp:Label ID="lblToDate" runat="server" Text="<%$ resources:ToDate%>" AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="input-small" TabIndex="1" MaxLength="11"
                                                ValidationGroup="Save" onkeydown="return CheckKey(event)" onpaste="return false;"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvToDate" runat="server" ControlToValidate="txtToDate"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_ToDate%>"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />                      
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                  <td>
                                        <div class="div2col-S">
                                             <asp:Label ID="lblName" runat="server" Text="<%$ resources:Name%>" AssociatedControlID="txtName"></asp:Label>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="input-small" MaxLength="100" TabIndex="2"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Name%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                     <td>
                                        <div class="div2col-S">
                                                                   <asp:Label ID="lbldesc" runat="server" Text="<%$ resources:Desc%>" AssociatedControlID="txtDesc"></asp:Label>
                                            <asp:TextBox ID="txtDesc" runat="server" CssClass="input-small" MaxLength="100" TabIndex="2"> </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rqdfddesc" runat="server" ControlToValidate="txtDesc"
                                                CssClass="star" ValidationGroup="Save" Text="*" ErrorMessage="<%$ resources:Err_Desc%>"></asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                </tr    
                                                         
                               <tr>
                                   <td>
                                        <div class="div2col-S">                                         
                                           <asp:Label ID="lblActive" runat="server" Text="<%$ resources:Active%>" AssociatedControlID="lblActive"></asp:Label>
                                            <asp:CheckBox ID="chkActive" runat="server" TabIndex="2" Checked="true" />                                          
                                        </div>
                                    </td>
                                </tr>
                              
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="Save" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
