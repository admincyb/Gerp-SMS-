<%@ Page Title="<%$ Resources:Captions,Title_RFQ %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="RequestForQuote.aspx.cs" Inherits="ERPSMS_v01.PurchaseOrderManagement.RequestForQuote"
    Theme="ClassicExt" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        function ShowListing(flag) {
            ///<summary>
            /// Used to handle the Listing And Enrty Section in Page
            ///</summary>
            /// <param name="flag" optional="true" type="String">
            /// flag Determines the Mode if flag then in Listing else in Edit Mode
            /// </param>           
            if (flag) {
                $("[id$=PageAction_Entry]").hide();
                $("[id$=pnlEntry]").hide();
            }
            else {
                $("[id$=PageAction_Entry]").show();
                $("[id$=pnlEntry]").show();
            }
            InitComponents();
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
                $("[id$=pnlSave]").hide();
                $("[id$=pnlDelete]").hide();
                //$("[id$=pnlSubmit]").hide();
            }
            else if (mode == 2) {
                $("[id$=pnlDelete]").hide();
                //$("[id$=pnlSubmit]").hide();
            }
        }
        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtRFQDate");
            GrandScriptUtils.DatePickerCommon("txtItemDateLst");
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();
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
                return true;
            }
        }
        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlAvtivity" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="51" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('rfq')" ValidationGroup="rfq"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSaveSubmit">
                                        <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                        <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="23"
                                            Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiry')"
                                            ValidationGroup="enquiry" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="91" Text="<%$resources:ErpRes,Save %>"
                                            OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('rfq')" ValidationGroup="rfq"
                                            ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="92" Text="<%$resources:ErpRes,Delete %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Delete" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:ErpRes,Cancel %>" OnClick="ActionHandler"
                                            CommandName="CANCEL" TabIndex="93" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:ErpRes,Cancel %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnRFQSearch" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnRFQSearch" Text="<%$resources:PageNameRes,RFQSearch %>"
                                TabIndex="1" CommandName="RFQSEARCH" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnRFQRequest" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnRFQRequest" Text="<%$resources:PageNameRes,RFQRequest %>"
                                TabIndex="2" CommandName="RFQREQUEST" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-active" OnClientClick="javascript:return false;"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnRFQResponse" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnRFQResponse" Text="<%$resources:PageNameRes,RFQResponse %>"
                                TabIndex="3" CommandName="RFQRESPONSE" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                            <div class="contentwrapper">
                                <asp:HiddenField ID="hdfStatus" runat="server" Value="0" />
                                <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblRFQTrxNo" Text="<%$ resources:RFQNo %>" AssociatedControlID="lblRFQTrxNoTxt"
                                                    TabIndex="1"></asp:Label>
                                                <asp:Label runat="server" ID="lblRFQTrxNoTxt" CssClass="input-small"></asp:Label>
                                                <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                                <asp:Label runat="server" ID="lblRFQDate" Text="<%$ resources:RFQDate %>" AssociatedControlID="txtRFQDate"
                                                    CssClass="lbl-20-8perc"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtRFQDate" TabIndex="1" MaxLength="12" CssClass="date-picker input-small"
                                                    onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfRFQDate" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="rfq" EnableClientScript="true" runat="server" ControlToValidate="txtRFQDate"
                                                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_RFQDate %>">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vreRFQDate" CssClass="star" ValidationGroup="rfq"
                                                        runat="server" ControlToValidate="txtRFQDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_RFQDate_Valid %>"
                                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                        EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblPageDept" runat="server" Text="<%$ resources:Controls,Department %>"
                                                    AssociatedControlID="lblPageDeptText"></asp:Label>
                                                <asp:Label ID="lblPageDeptText" runat="server" CssClass="input-medium"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblCompany" Text="<%$ resources:DDLCompany %>" AssociatedControlID="ddlCompany"></asp:Label>
                                                <asp:DropDownList ID="ddlCompany" runat="server" TabIndex="1" CssClass="input-half"
                                                    onmouseover="javascript:ShowTooltip('ddlCompany');">
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdfCompanyPk" runat="server" Value="0" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div runat="server" id="divSelectedItems">
                                    <h3>
                                        <%=GetLocalResourceObject("SelectedItems").ToString()%></h3>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdItemSelected" Width="100%" PageSize="<%$ resources:PageSize%>"
                                            OnRowDataBound="ActionHandler" AllowSorting="True" OnSorting="ActionHandler"
                                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" CssClass="grdTable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("RFD_ITEM_TEXT"), 25) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("RFD_ITEM_TEXT").ToString()) %>'></asp:Label>
                                                        <asp:HiddenField ID="hdfItemPKLst" runat="server" Value='<%# Eval("RFD_ITEM") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="25%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Description %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemDescLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("RFD_ITEM_DESC"), 23) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("RFD_ITEM_DESC").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="21%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Specifications %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtItemSpecLst" runat="server" Text='<%# HttpUtility.HtmlDecode(Eval("RFD_ITEM_SPEC").ToString()) %>'
                                                            TabIndex="2" MaxLength="200" CssClass="medium-a" ToolTip='<%# HttpUtility.HtmlDecode(Eval("RFD_ITEM_SPEC").ToString()) %>'></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="22%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:UoM %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemUoMLst" runat="server" Text='<%# Eval("RFD_UOM_TEXT") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("RFD_UOM_TEXT").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtItemQuantityLst" runat="server" Text='<%# GetFormattedNumber(Eval("RFD_QTY_REQUESTED")) %>'
                                                            TabIndex="2" CssClass="input-w51per numeric" MaxLength="20" ToolTip='<%# GetFormattedNumber(Eval("RFD_QTY_REQUESTED"))%>'></asp:TextBox>
                                                        <div class="starwrap">
                                                            <asp:RequiredFieldValidator ID="vrfItemQuantityLst" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="rfq" EnableClientScript="true" runat="server" ControlToValidate="txtItemQuantityLst"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                            </asp:RequiredFieldValidator>
                                                            <%--  <asp:RegularExpressionValidator ID="vreItemQuantityLst" runat="server" ControlToValidate="txtItemQuantityLst"
                                                                ErrorMessage="<%$ resources:Err_Quantity_Valid %>" ValidationExpression="^\$?([0-9]{0,14})?(\.[0-9]{0,2})?$"
                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="rfq">
                                                            </asp:RegularExpressionValidator>--%>
                                                            <cc1:QuantityValidationP2P ID="vreItemQuantityLst" runat="server" ControlToValidate="txtItemQuantityLst"
                                                                NumberDigits="14" ErrorMessage="<%$ resources:Err_Quantity_Valid %>" Display="Dynamic"
                                                                Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="rfq" NonZero="true"></cc1:QuantityValidationP2P>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="9%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ReqdDt %>">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtItemDateLst" runat="server" Text='<%# Eval("RFD_REQD_DATE", Resources.ErpRes.DateFormatGrid) %>'
                                                            TabIndex="2" CssClass="input-w51per date-picker" ToolTip='<%# Eval("RFD_REQD_DATE", Resources.ErpRes.DateFormatGrid)%>'></asp:TextBox>
                                                        <div class="starwrap">
                                                            <asp:RequiredFieldValidator ID="vrfItemDateLst" CssClass="star" SetFocusOnError="true"
                                                                ValidationGroup="rfq" EnableClientScript="true" runat="server" ControlToValidate="txtItemDateLst"
                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqdDate %>">
                                                            </asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="vreItemDateLst" CssClass="star" ValidationGroup="rfq"
                                                                runat="server" ControlToValidate="txtItemDateLst" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_ReqdDate_Valid %>"
                                                                ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                                EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                            CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                            SkinID="imbdeletegrid" ToolTip="Remove" OnClientClick="return ShowDeleteConfirm(this);" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="3%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <h3>
                                        <%=GetLocalResourceObject("SelectedVendors").ToString()%></h3>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdVendorSelected" Width="100%" PageSize="<%$ resources:PageSize%>"
                                            OnRowDataBound="ActionHandler" AllowSorting="True" OnSorting="ActionHandler"
                                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:VenderCode %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorCodeLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("RVM_VENDOR_CODE"), 17) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("RVM_VENDOR_CODE").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Name %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorNameLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("RVM_VENDOR_NAME"), 25) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval("RVM_VENDOR_NAME").ToString()) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Location %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorLocLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("RVM_VENDOR_LOCATION"), 21) %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("RVM_VENDOR_LOCATION"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ContactPhone %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorPhoneLst" runat="server" Text='<%# Eval("RVM_VENDOR_PHONE") %>'
                                                            ToolTip='<%# Eval("RVM_VENDOR_PHONE")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ContactEmail %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblVendorEmailLst" runat="server" Text='<%# Eval("RVM_VENDOR_EMAIL") %>'
                                                            ToolTip='<%# HttpUtility.HtmlDecode(Convert.ToString(Eval("RVM_VENDOR_EMAIL"))) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnRemove" runat="server" OnClick="ActionHandler" CommandName="REMOVEVENDOR"
                                                            CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                            SkinID="imbdeletegrid" ToolTip="Remove" OnClientClick="return ShowDeleteConfirm(this);" />
                                                        <asp:ImageButton ID="btnRespond" runat="server" OnClick="ActionHandler" CommandName="RFQVENDORRESPONSE"
                                                            CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender" OnLoad="btnAction_Load"
                                                            SkinID="respond" ToolTip="Response" />
                                                        <asp:ImageButton runat="server" ID="imbPrint" SkinID="btnPrint" OnClick="ActionHandler"
                                                            CommandName="PRINT" CommandArgument="PageAction_Entry" OnPreRender="btnAction_PreRender"
                                                            ToolTip="Print" />
                                                        <%--OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'))"--%>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="5%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                                <table class="table-devide">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblPaymentTerms" Text="<%$ resources:PaymentTerms %>"
                                                    AssociatedControlID="txtPaymentTerms"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPaymentTerms" MaxLength="500" TabIndex="15" TextMode="MultiLine"
                                                    EnableTheming="false" CssClass="multiline-3col input-full" onkeydown="limitText(this,500);"
                                                    onkeyup="limitText(this,500);"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vrePaymentTerms" runat="server" ControlToValidate="txtPaymentTerms"
                                                    ErrorMessage="<%$ Resources:Err_PaymentTerms %>" ValidationExpression="^[\s\S]{0,500}$"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="rfq"></asp:RegularExpressionValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblDeliveryTerms" Text="<%$ resources:DeliveryTerms %>"
                                                    AssociatedControlID="txtDeliveryTerms"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtDeliveryTerms" MaxLength="500" TabIndex="15" TextMode="MultiLine"
                                                    EnableTheming="false" CssClass="multiline-3col" onkeydown="limitText(this,500);"
                                                    onkeyup="limitText(this,500);"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vreDeliveryTerms" runat="server" ControlToValidate="txtDeliveryTerms"
                                                    ErrorMessage="<%$ Resources:Err_DeliveryTerms %>" ValidationExpression="^[\s\S]{0,500}$"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="rfq"></asp:RegularExpressionValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label runat="server" ID="lblOtherdetails" Text="<%$ resources:Otherdetails %>"
                                                    AssociatedControlID="txtOtherdetails"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtOtherdetails" MaxLength="500" TabIndex="15" TextMode="MultiLine"
                                                    EnableTheming="false" CssClass="multiline-3col" onkeydown="limitText(this,500);"
                                                    onkeyup="limitText(this,500);"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vreOtherdetails" runat="server" ControlToValidate="txtOtherdetails"
                                                    ErrorMessage="<%$ Resources:Err_Otherdetails %>" ValidationExpression="^[\s\S]{0,500}$"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="rfq"></asp:RegularExpressionValidator>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="rfq" runat="server" />
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                </div>
            </div>
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" ValidationGroup="rfq" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
