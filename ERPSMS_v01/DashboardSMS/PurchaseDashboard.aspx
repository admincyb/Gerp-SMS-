<%@ Page Title="<%$ Resources:Captions,Title_PODashboard %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="PurchaseDashboard.aspx.cs" Inherits="ERPSMS_v01.DashboardSMS.PurchaseDashboard"
    Theme="ClassicExt" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", url, "hdfVendor", true, true, "VENDOR");
            showProductName();
            ShowHideExpand();
        }

        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>

            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }

        
        function showProductName() {
            if ($("[id$=lblProductName]").html() == "") {
                $("#divProductName").hide();
            }
            else {
                $("#divProductName").show();
            }
        }

        function Disableautocomplete() {
            if ($("[id$=txtCustomerSearch]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomerSearch]"), $("[id$=hdfCustomerSearch]"));
            }
        }
        function DisableAuto(extender, hfield) {
            ///<summary>
            /// Used to disable Autocomplete
            ///</summary>
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
            $(extender).autocomplete("option", "disabled", true);
            $(extender).attr("disabled", true);
        }

        function ShowHideCustomersOrders(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divCustomersOrdersGid").show();
                $("[id$=imgShowCustomersOrders]").hide();
                $("[id$=imgHideCustomersOrders]").show();
            }
            else {

                $("#divCustomersOrdersGid").hide();
                $("[id$=imgShowCustomersOrders]").show();
                $("[id$=imgHideCustomersOrders]").hide();
            }
            return false;
        }       

        function ShowHideInvoice(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divInvoiceGrid").show();
                $("[id$=imbInvoiceShow]").hide();
                $("[id$=imbInvoiceHide]").show();
            }
            else {

                $("#divInvoiceGrid").hide();
                $("[id$=imbInvoiceShow]").show();
                $("[id$=imbInvoiceHide]").hide();
            }
            return false;
        }      
       
        function ShowHideQUESTIONNAIRE(flag) {
            //If flag then Show Labours
            if (flag) {

                $("#divQUESTIONNAIRE").show();
                $("[id$=imbQUESTIONNAIREShow]").hide();
                $("[id$=imbQUESTIONNAIREHide]").show();
            }
            else {

                $("#divQUESTIONNAIRE").hide();
                $("[id$=imbQUESTIONNAIREShow]").show();
                $("[id$=imbQUESTIONNAIREHide]").hide();
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
                ShowErrorMessage($("#diverror").html());
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }
        // for GO with search key
        function Search(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode == 13) {
                //                $('[id$=btnbinCardGet]').click();
                //                return false;
            }
        }

        function ShowError() {
            var msg = '<ul><li><%= GetLocalResourceObject("Err_Customer") %></li></ul>';
            $("[id$=litErrorMsg]").show();
            $("[id$=litErrorMsg]").html(msg);
            ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
        }


        function AfterGridExpand(row) {

            var gridType = 0;
            var names = $(row).parent().parent().attr('id').split('_');

            if (names.length == 1) {
                gridType = names[0] == "grdGRNList" ? 1 : names[0] == "grdGINList" ? 2 : names[0] == "grdInvoice" ? 3 : 0;
            }
            else if (names.length > 1) {
            gridType = (names[names.length - 1] == "grdGRNList" || names[names.length - 2] == "grdGRNList") ? 1
                    : (names[names.length - 1] == "grdGINList" || names[names.length - 2] == "grdGINList") ? 2
                    : (names[names.length - 1] == "grdInvoice" || names[names.length - 2] == "grdInvoice") ? 3 : 0;
            }
            if (gridType == 1) {
                var hdf = $(row).find("[id*=hdfIsExpandedGrn]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetGin]").click();
                }
            }
            if (gridType == 2) {
                var hdf = $(row).find("[id*=hdfIsExpandedGinList]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetProduct]").click();
                }
            }
            if (gridType == 3) {
                var hdf = $(row).find("[id*=hdfIsExpandedInvList]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetPayment]").click();
                }
            }

        }

    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlDashboard" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons" style="padding-bottom: 15px;">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlListing" style="display: none">
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="dashboard-wrapper">
                    <div class="dash100">
                        <div class="dashinner100">
                            <div class="dash-subhead">
                                <h1>
                                    <asp:Literal ID="ltl" runat="server" Text="<%$ resources:QUESTIONNAIRE %>"></asp:Literal>
                                </h1>
                                <div class="controls">
                                    <asp:ImageButton runat="server" ID="imbQUESTIONNAIREShow" OnClientClick="javascript:return ShowHideQUESTIONNAIRE(1);"
                                        ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:QueriesShow %>" />
                                    <asp:ImageButton runat="server" ID="imbQUESTIONNAIREHide" OnClientClick="javascript:return ShowHideQUESTIONNAIRE();"
                                        Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                        ToolTip="<%$ resources:QueriesHide %>" />
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="dash-contents1" id="divQUESTIONNAIRE">
                                <table class="table-devide" id="tbladvancedSearch">
                                    <tr id="Tr1" runat="server">
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblVendor" runat="server" Text="<%$resources:Vendor %>" AssociatedControlID="txtVendor"></asp:Label>
                                                <asp:TextBox ID="txtVendor" runat="server" CssClass="input-half" MaxLength="100"
                                                    TabIndex="105"> </asp:TextBox>
                                                <asp:HiddenField ID="hdfVendor" runat="server" />
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblpoNo" runat="server" Text="<%$resources:PONO %>" AssociatedControlID="txtPONo"></asp:Label>
                                                <asp:TextBox ID="txtPONo" runat="server" CssClass="input-medium"></asp:TextBox>
                                                <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                    OnClick="ActionHandler" TabIndex="108" CommandName="SEARCH" SkinID="search-ext"
                                                    CssClass="margntop2 margnbotm0" />
                                                <asp:ImageButton ID="btnClear" runat="server" ToolTip="<%$ resources:Controls,Clear %>"
                                                    TabIndex="109" OnClick="ActionHandler" CommandName="CLEAR" SkinID="clear-ext"
                                                    CssClass="margntop2 margnbotm0" />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div style="display: none;">
                                    <asp:DropDownList ID="ddlQuestionnaire" runat="server" OnSelectedIndexChanged="ActionHandler"
                                        AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:Label runat="server" ID="lblTransDetails" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="trv-wrapper max-425">
                        <table>
                            <tr>
                                <td class="trv-td">
                                    <div class="treeview  max-425">
                                        <asp:TreeView ID="trvCustomers" runat="server" ShowLines="true" OnTreeNodePopulate="PopulateParentsChild"
                                            TabIndex="6" OnSelectedNodeChanged="ActionHandler_onSelect" EnableClientScript="true">
                                            <SelectedNodeStyle BackColor="#C6DEFF" ForeColor="Black" BorderStyle="Dotted" BorderWidth="1px"
                                                BorderColor="#000000" />
                                        </asp:TreeView>
                                    </div>
                                </td>
                                <td>
                                    <div class="content-wrapper">
                                        <table>
                                            <tr>
                                                <td>
                                                    <div class="search-colapse-b">
                                                        <table>
                                                            <tr>
                                                                <td style="width: auto">
                                                                    <h1>
                                                                        <asp:Label ID="lblCustomerOrderHdr" AssociatedControlID="imgShowCustomersOrders"
                                                                            runat="server" Text="<%$ resources:POItems %>"></asp:Label>
                                                                    </h1>
                                                                </td>
                                                                <td style="width: auto">
                                                                    <asp:ImageButton runat="server" ID="imgShowCustomersOrders" OnClientClick="javascript:return ShowHideCustomersOrders(1);"
                                                                        ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:CustomerOrderHdrShow %>" />
                                                                    <asp:ImageButton runat="server" ID="imgHideCustomersOrders" OnClientClick="javascript:return ShowHideCustomersOrders();"
                                                                        Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                                        ToolTip="<%$ resources:CustomerOrderHdrHide %>" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="divCustomersOrdersGid">
                                            <div class="gridwrap" id="PackingGrid">
                                                <asp:GridView runat="server" ID="grdPOItems" Width="100%" 
                                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:RadioButton ID="rbtSelect" CssClass="rdoSelection" runat="server" GroupName="SelectOne"
                                                                    AutoPostBack="true" OnCheckedChanged="ActionHandler" onclick="GrandScriptUtils.EnableRbtnGrouping(this);" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:ItemDesc %>">
                                                            <ItemTemplate>
                                                                <asp:HiddenField runat="server" ID="hdfPODtlPk" Value='<%# Eval("POD_PK") %>' />
                                                                <asp:Label ID="lblItem" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("ITM_TEXT"),42) %>'
                                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("ITM_TEXT")) %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="38%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Comments %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCommants" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("POD_REMARKS"),25) %>'
                                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("POD_REMARKS")) %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="20%" HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUOM" runat="server" Text='<%#Eval("UOM_CODE") %>' ToolTip='<%#Eval("UOM_CODE") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="5%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRate" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("POD_RATE"))%>'
                                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("POD_RATE"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Quantity %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("POD_QTY_REQUESTED")) %>'
                                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("POD_QTY_REQUESTED")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:POAmount %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPOItemAmount" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("POD_AMT_VALUE")) %>'
                                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("POD_AMT_VALUE")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <div id="divProductName">
                                                <h5>
                                                    <asp:Label ID="lblProductName" runat="server" Text="<%$ resources:GRNList %>"></asp:Label></h5>
                                            </div>
                                            <div class="gridwrap hierarchical-wrap">
                                                <cc1:ExtGridView runat="server" ID="grdGRNList" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                    CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                    CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:GrnNo %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGrnNo" runat="server" Text='<%# Eval("GRH_NO") %>' ToolTip='<%# Eval("GRH_NO") %>'>
                                                                </asp:Label>
                                                                <asp:HiddenField runat="server" ID="hdfIsExpandedGrn" Value="0" />
                                                                <asp:HiddenField runat="server" ID="hdfGrnPk" Value='<%# Eval("GRD_PK") %>' />
                                                                <asp:Button runat="server" ID="btnGetGin" OnClick="ActionHandler" CommandName="GINDETAILS"
                                                                    CommandArgument='<%# Eval("GRD_PK") %>' EnableTheming="false" Style="display: none" />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGrnDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.GrnDate, Resources.Constants.DateFormatGrid) %>'
                                                                    ToolTip='<%# Eval(Resources.DataFieldRes.GrnDate, Resources.Constants.DateFormatGrid) %>'>
                                                                </asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" Wrap="false"  />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:GrnStore %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGrnStore" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("GRH_DEPT_TEXT"),25) %>'
                                                                    ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("GRH_DEPT_TEXT")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="35%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGrnUOM" runat="server" Text='<%#Eval("GRD_UOM_TEXT") %>' ToolTip='<%#Eval("GRD_UOM_TEXT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:GRNRcvdQty%>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGRNRcvdQty" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("GRD_QTY_RECEIVED"))%>'
                                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("GRD_QTY_RECEIVED"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="rate-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:GRNAccQty%>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGRNAccQty" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("GRD_QTY_ACCEPTED"))%>'
                                                                    ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("GRD_QTY_ACCEPTED"))%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="rate-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <div class="hierarchical-gridwrap">
                                                                    <cc1:ExtGridView runat="server" ID="grdGINList" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                                        CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                                        CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" CellPadding="3"
                                                                        ForeColor="#333333" AllowPaging="false">
                                                                        <EmptyDataTemplate>
                                                                            <asp:Label ID="lblGINList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                        </EmptyDataTemplate>
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ resources:GinNo %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGinNo" runat="server" Text='<%# Eval("GIH_NO") %>' ToolTip='<%# Eval("GIH_NO") %>'>
                                                                                    </asp:Label>
                                                                                    <asp:HiddenField runat="server" ID="hdfIsExpandedGinList" Value="0" />
                                                                                    <asp:HiddenField runat="server" ID="hdfGinPk" Value='<%# Eval("GID_PK") %>' />
                                                                                    <asp:Button runat="server" ID="btnGetProduct" OnClick="ActionHandler" CommandName="STKADMISSIONDETAILS"
                                                                                        EnableTheming="false" Style="display: none" CommandArgument='<%# Eval("GID_PK") %>' />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="15%" />
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGinDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.GinDate, Resources.Constants.DateFormatGrid) %>'
                                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.GinDate, Resources.Constants.DateFormatGrid) %>'>
                                                                                    </asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="10%" Wrap="false" />
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:GinStore %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGinStore" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("GIH_DEPT_TEXT"),23) %>'
                                                                                        ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("GIH_DEPT_TEXT")) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="25%" />
                                                                                <HeaderStyle HorizontalAlign="Left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGinUOM" runat="server" Text='<%#Eval("GID_UOM_TEXT") %>' ToolTip='<%#Eval("GID_UOM_TEXT") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="5%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:GinQtyInspected %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGinQtyInspected" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("GID_QTY_INSPECTED"))%>'
                                                                                        ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("GID_QTY_INSPECTED"))%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                <HeaderStyle CssClass="rate-numeric" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:GinQtyRejected %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGinQtyRejected" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("GID_QTY_REJECTED"))%>'
                                                                                        ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("GID_QTY_REJECTED"))%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                <HeaderStyle CssClass="rate-numeric" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:GinQtyAccepted %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGinQtyAccepted" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("GID_QTY_ACCEPTED"))%>'
                                                                                        ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("GID_QTY_ACCEPTED"))%>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                <HeaderStyle CssClass="rate-numeric" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <div class="hierarchical-gridwrap">
                                                                                        <cc1:ExtGridView runat="server" ID="grdStockTransfer" AutoGenerateColumns="False"
                                                                                            ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                                                                            GridLines="None" ExpandButtonText="+" DataKeyNames="<%$ resources:DataFieldRes,StHdrPK %>"
                                                                                            CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false"
                                                                                            OnRowDataBound="ActionHandler">
                                                                                            <EmptyDataTemplate>
                                                                                                <asp:Label ID="lblSTEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                                            </EmptyDataTemplate>
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="<%$ resources:STNumber %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblStNumber" runat="server" Text='<%# Eval("SFH_NO") %>' ToolTip='<%# Eval("SFH_NO") %>'>
                                                                                                        </asp:Label>
                                                                                                        <asp:HiddenField runat="server" ID="hdfIsExpandedItem" Value="0" />
                                                                                                        <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                                                        <asp:HiddenField ID="hdfStockTransferPK" runat="server" Value='<%# Eval("SFH_PK") %>' />
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblStDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.STDate, Resources.Constants.DateFormatGrid) %>'
                                                                                                            ToolTip='<%# Eval(Resources.DataFieldRes.STDate, Resources.Constants.DateFormatGrid) %>'>
                                                                                                        </asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="25%" Wrap="false" />
                                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ resources:UOM %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblStUOM" runat="server" Text='<%#Eval("SFD_UOM_TEXT") %>' ToolTip='<%#Eval("SFD_UOM_TEXT") %>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle Width="10%" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="<%$ resources:QuantityUom %>">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="lblStQuantityUom" runat="server" Text='<%# GetFormattedCurrencyWithSeperation(Eval("SFD_QTY_APPROVED"))%>'
                                                                                                            ToolTip='<%# GetFormattedCurrencyWithSeperation(Eval("SFD_QTY_APPROVED"))%>'></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                                                    <HeaderStyle CssClass="rate-numeric" />
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField>
                                                                                                    <ItemTemplate>
                                                                                                        <asp:Label ID="Label1" Text="" runat="server"></asp:Label>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                            <RowStyle CssClass="table-thirdlevel" />
                                                                                            <HeaderStyle CssClass="table-thirdlevela" />
                                                                                        </cc1:ExtGridView>
                                                                                    </div>
                                                                                </ItemTemplate>
                                                                                <ItemStyle CssClass="nopadding" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <%--Second--%>
                                                                        <RowStyle CssClass="table-thirdlevel" />
                                                                        <HeaderStyle CssClass="table-thirdlevela" />
                                                                    </cc1:ExtGridView>
                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="nopadding" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <%--First--%>
                                                    <RowStyle CssClass="table-secondlevel" />
                                                    <HeaderStyle CssClass="table-secondlevela" />
                                                </cc1:ExtGridView>
                                            </div>
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <table>
                                            <tr>
                                                <td colspan="9">
                                                    <div class="qty-colapse">
                                                        <table>
                                                            <tr>
                                                                <td style="width: auto">
                                                                    <h1>
                                                                        <asp:Label ID="lblinv" AssociatedControlID="imbInvoiceShow" runat="server" Text="<%$ resources:Invoice %>"></asp:Label>
                                                                    </h1>
                                                                </td>
                                                                <td style="width: auto">
                                                                    <asp:ImageButton runat="server" ID="imbInvoiceShow" OnClientClick="javascript:return ShowHideInvoice(1);"
                                                                        ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="<%$ resources:InvoiceShow %>" />
                                                                    <asp:ImageButton runat="server" ID="imbInvoiceHide" OnClientClick="javascript:return ShowHideInvoice();"
                                                                        Style="display: none" ImageUrl="../images/Classic/Icons/arrow-colapse-active.png"
                                                                        ToolTip="<%$ resources:InvoiceHide %>" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--Invoice --%>
                                        <div id="divInvoiceGrid">
                                            <div class="gridwrap hierarchical-wrap">
                                                <cc1:ExtGridView runat="server" ID="grdInvoice" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                    CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                    CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblInvList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%# Eval("IVH_NO") ==""?"[NEW]":Eval("IVH_NO")%>'
                                                                    ToolTip='<%# Eval("IVH_NO") ==""?"[NEW]":Eval("IVH_NO")%>'>
                                                                </asp:Label>
                                                                <asp:HiddenField runat="server" ID="hdfIsExpandedInvList" Value="0" />
                                                                <asp:HiddenField runat="server" ID="hdfInvPk" Value='<%# Eval("IVH_PK") %>' />
                                                                <asp:Button runat="server" ID="btnGetPayment" OnClick="ActionHandler" CommandName="PAYMENTDETAILS"
                                                                    EnableTheming="false" Style="display: none" CommandArgument='<%# Eval("IVH_PK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="7%" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceDate %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%#  Eval("IVH_DATE", Resources.Constants.DateFormatGrid)!=""? Convert.ToDateTime(Eval("IVH_DATE", Resources.Constants.DateFormatGrid)).ToString(Resources.Constants.ReportDateFormat):""  %>'
                                                                    ToolTip='<%# Eval("IVH_DATE", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="7%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCmpName" Font-Bold="true" runat="server" Text='<%# Eval("IVH_COMPANY_TEXT")%>'
                                                                    ToolTip='<%# Eval("IVH_COMPANY_TEXT")%>' CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="3%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:InvoiceType %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInvoiceType" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_TYPE_TEXT"),3,"")%>'
                                                                    ToolTip='<%# Eval("IVH_TYPE_TEXT")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="3%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Vendor %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblVendor" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.VENDORCode1),10)%>'
                                                                    ToolTip='<%# Eval("IVH_VENDOR_TEXT") %>'></asp:Label>
                                                                <asp:HiddenField runat="server" ID="hdfVendorPK" Value='<%# Eval("IVH_VND_PK") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="9%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:VendorInvNo %>" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSupplierInvNO" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_VENDOR_INV_NO"),10) %>'
                                                                    ToolTip='<%# Eval("IVH_VENDOR_INV_NO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:PONO %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSoNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_PO_NO"),14)%>'
                                                                    ToolTip='<%# Eval("IVH_PO_NO")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:GrnNo %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGrnNo" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_GRN_NO"),14)%>'
                                                                    ToolTip='<%# Eval("IVH_GRN_NO")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="11%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:PODate %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSoDate" runat="server" Text='<%#ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_POH_DT", Resources.Constants.DateFormatGrid),12)%>'
                                                                    ToolTip='<%# Eval("IVH_POH_DT", Resources.Constants.DateFormatGrid)%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="9%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCurrency" runat="server" Text='<%#Eval("IVH_CURRENCY_TEXT")  %>'
                                                                    ToolTip='<%#Eval("IVH_CURRENCY_TEXT")  %>'></asp:Label>
                                                                <asp:HiddenField runat="server" ID="hdfPOCurrency" Value='<%# Eval("IVH_CURRENCY") %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle Width="2%" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:Payable %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInvoiceValue" runat="server" Text='<%# Eval("IVH_AMOUNT", "{0:c}") %>'
                                                                    ToolTip='<%# Eval("IVH_AMOUNT", "{0:c}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<%$ resources:BalAmt %>">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblBalAmt" runat="server" Text='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>'
                                                                    ToolTip='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>'></asp:Label>
                                                                <%--  <asp:LinkButton ID="lbnBalAmt" runat="server" Text='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>'
                                                                        CssClass="text-underline" ToolTip='<%# Eval("IVH_BAL_AMNT_TC", "{0:c}") %>' OnClick="ActionHandler"
                                                                        CommandName="AMOUNTDETAILS" Visible='<%# GetBalanceLinkVisibility(Eval("IVH_AMOUNT").ToString(),Eval("IVH_BAL_AMNT_TC").ToString())%>'></asp:LinkButton>
                                                                --%>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="10%" CssClass="amount-numeric" />
                                                            <HeaderStyle CssClass="amount-numeric" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <div class="hierarchical-gridwrap">
                                                                    <asp:GridView runat="server" ID="grdPayments" AutoGenerateColumns="False" 
                                                                     EmptyDataRowStyle-CssClass="emptytable"  OnRowDataBound="ActionHandler" >
                                                                        <EmptyDataTemplate>
                                                                            <asp:Label ID="lblPaymentsEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                        </EmptyDataTemplate>
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ resources:PaymentNo %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPaymentNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.POPaymentNo)==""?"[NEW]":Eval(Resources.DataFieldRes.POPaymentNo) %>'
                                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.POPaymentNo)==""?"[NEW]":Eval(Resources.DataFieldRes.POPaymentNo) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="10%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:PaymentDate %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPaymentDate" runat="server" Text='<%# Eval(Resources.DataFieldRes.POPaymentDate, Resources.Constants.DateFormatGrid).ToString()  %>'
                                                                                        ToolTip='<%# Eval(Resources.DataFieldRes.POPaymentDate, Resources.Constants.DateFormatGrid).ToString() %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="7%" Wrap="false" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:Vendor %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPaymentVendor" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PVH_VENDOR_TEXT"),30) %>'
                                                                                        ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("PVH_VENDOR_TEXT")) %>'></asp:Label>                                                                                    
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="22%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:InvoiceNo %>">
                                                                                <ItemTemplate>
                                                                                <asp:Label ID="lblInvNo" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("IVH_NO"),30) %>'
                                                                                        ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("IVH_NO")) %>'></asp:Label>                                                                                  
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="10%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:ModeofPayment %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblModeofPayment" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PDM_MODE_TEXT"),30) %>'
                                                                                        ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("PDM_MODE_TEXT")) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="4%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:BankName %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblBankName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PDM_BANK_TEXT"),30) %>'
                                                                                        ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval("PDM_BANK_TEXT")) %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="18%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:CurrencyH %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPaymentCurrency" runat="server" Text='<%#Eval("PVH_CURRENCY_TEXT")  %>'
                                                                                        ToolTip='<%#Eval("PVH_CURRENCY_TEXT")  %>'></asp:Label>                                                                                    
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="2%" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ resources:Amount %>" ItemStyle-HorizontalAlign="Right">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblAmount" runat="server" Text='<%#  Eval(Resources.DataFieldRes.POPaidAmount, "{0:c}") %>'
                                                                                        ToolTip='<%#  Eval(Resources.DataFieldRes.POPaidAmount, "{0:c}") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="10%" />
                                                                                <HeaderStyle CssClass="amount-numeric" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblHdrAmountBaseCur" runat="server" Text='<%$ resources:Amount %>' ToolTip=''></asp:Label>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>                                                                                
                                                                                    <asp:Label ID="lblAmountTHB" runat="server" Text='<%#  Eval(Resources.DataFieldRes.PODPaidAmountTHB, "{0:c}") %>'
                                                                                        ToolTip='<%#  Eval(Resources.DataFieldRes.PODPaidAmountTHB, "{0:c}") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                                                <HeaderStyle CssClass="amount-numeric" Wrap="false" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <RowStyle CssClass="table-thirdlevel" />
                                                                        <HeaderStyle CssClass="table-thirdlevela" />
                                                                    </asp:GridView>
                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="nopadding" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <%--Second--%>
                                                    <RowStyle CssClass="table-thirdlevel" />
                                                    <HeaderStyle CssClass="table-thirdlevela" />
                                                </cc1:ExtGridView>
                                            </div>
                                        </div>
                                        <div class="clear">
                                        </div>                                       
                                     
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="diverror" style="display: none">
                            <%--Use this label to bind the server errors--%>
                            <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                            <asp:ValidationSummary ID="vsPlan" ValidationGroup="Allocation" runat="server" />
                        </div>                     
                    </div>
                    <div class="clear">
                    </div>
                   
                </div>
            </div>
            <asp:HiddenField ID="hdfCurrFormatWithSep" runat="server" />
            <asp:HiddenField ID="hdfBaseCurrencyText" runat="server"  />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
