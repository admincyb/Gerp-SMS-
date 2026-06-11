 <%@ Page Title="<%$ Resources:Captions,Title_RFQ %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="RFQSearch.aspx.cs" Inherits="ERPSMS_v01.PurchaseOrderManagement.RFQSearch"
    Theme="ClassicExt" %>
    <%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("FromDate", "hdfFrmDate", "ToDate", "hdfToDate", false, false);
            GrandScriptUtils.DatePickerCommon("txtItemReqDate");
            GrandScriptUtils.MakeAutoCompleteDDL("txtSearchValue", url + "?SearchBy=" + $("[id$=ddlSearchType]").val(), "hdfSearchValue", true, true, "RFQPRSEARCH");
            GrandScriptUtils.MakeAutoCompleteDDL("txtItemCategory", url, "hdfItemCategory", true, true, "CATEGORY");
            GrandScriptUtils.MakeAutoCompleteDDL("txtItem", url + "?Type=" + $("[id$=hdfItemCategory]").val(), "hdfItem", true, true, "ITEM");
            if ($("[id$=hdfItemCategory]").val() == "" || $("[id$=hdfItemCategory]").val() == "0")
                DisableAuto($("[id$=txtItem]"), $("[id$=hdfItem]"));
            SetSearchType(1);
        }
        function AfterAutoCompleteSelect(targetControlID) {
            
            if (targetControlID == "txtItemCategory") {
                $("[id$=hdfItem]").val("0");
                $("[id$=txtItem]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                if ($("[id$=hdfItemCategory]").val() != "" && $("[id$=hdfItemCategory]").val() != "0") {
                    EnableAuto($("[id$=txtItem]"));
                    GrandScriptUtils.MakeAutoCompleteDDL("txtItem", url + "?Type=" + $("[id$=hdfItemCategory]").val(), "hdfItem", true, true, "ITEM");
                }
                else {
                    $("[id$=txtItem]").attr("disabled", true);
                    DisableAuto($("[id$=txtItem]"), $("[id$=hdfItem]"));
                }
            }
            else if (targetControlID == "txtItem") {
                if ($("[id$=hdfItem]").val() != "" && $("[id$=hdfItem]").val() != "0") {
                    $("[id$=btnSelectItem]").click();
                }
                else {
                    $("[id$=txtItemDesc]").val("");
                    $("[id$=txtItemUOM]").val("");
                    $("[id$=hdfItemUOM]").val("0");
                }
            }
        }
        //To excecute after auto complete change
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtItemCategory") {
                $("[id$=hdfItem]").val("0");
                $("[id$=txtItem]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                $("[id$=txtItem]").attr("disabled", true);
                DisableAuto($("[id$=txtItem]"), $("[id$=hdfItem]"));
            }
            else if (targetControlID == "txtItem") {
                $("[id$=txtItemDesc]").val("");
                $("[id$=txtItemUOM]").val("");
                $("[id$=hdfItemUOM]").val("0");
            }
        }
        function ShowHideAdvancedSearch(flag) {
            //If flag then Show AdvancedSearch
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
        function ClearSearchDetails() {
            ///<summary>To Clear Details In Search Section</summary>

            $("[id$=SearchValue]").val("");
            $("[id$=FromDate]").val("");
            $("input[id$=hdfFrmDate]").val("");
            $("[id$=ToDate]").val("");
            $("input[id$=hdfToDate]").val("");
        }
        function SetSearchType(isNew) {
            ///<summary>Function To Enable/Disable Selected Option For Search </summary>

            var strname = $("select[id$=SearchType]").val();
            $("[id$=SearchValue]").val("");
            if (strname == "0") {
                //                if (!isNew)
                //                    ClearSearchDetails();
                $("#divSearchDtls").hide();
                $("#divDate").hide();
                //$("[id$=imbSearch]").hide();
            }
            else if (strname == "Date") {
                $("[id$=SearchValue]").val("");
                $("#divSearchDtls").hide();
                $("#divDate").show();
                //$("[id$=imbSearch]").show();
                GrandScriptUtils.AddDateRangeCommon("FromDate", "hdfFrmDate", "ToDate", "hdfToDate", false, false);
            }
            else {
                GrandScriptUtils.MakeAutoCompleteDDL("txtSearchValue", url + "?SearchBy=" + $("[id$=ddlSearchType]").val(), "hdfSearchValue", true, true, "RFQPRSEARCH");
                $("[id$=FromDate]").val("");
                $("input[id$=hdfFrmDate]").val("");
                $("[id$=ToDate]").val("");
                $("input[id$=hdfToDate]").val("");
                $("#divSearchDtls").show();
                $("#divDate").hide();
                $("[id$=imbSearch]").show();
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
        function EnableAuto(extender) {
            ///<summary>
            /// Used to enable Autocomplete
            ///</summary>
            $(extender).removeAttr("disabled");
            $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
            $(extender).autocomplete("option", "disabled", false);
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
        function gGridCheckAllSelect(chkBox) {
            var tbl = $(chkBox).parents('table:eq(0)');
            var isChecked = true;
            if ($(chkBox).is(":checked")) {
                if ($(tbl).find("tbody td input[type=checkbox]:not(:checked)").length == 0) { // if any one of the tr checkbox is uncheked then thead checkbox unchecked
                    $(tbl).find("thead input[type=checkbox],th input[type=checkbox]").attr("checked", "checked");
                }
            }
            else {
                $(tbl).find("thead input[type=checkbox],th input[type=checkbox]").removeAttr("checked");
            }
            if (typeof CheckBoxClickTrigger == 'function') {
                CheckBoxClickTrigger($(tbl).attr("id"));
            }
        }
        function gGridSelectAllCheckBoxes(chkBox) {
            var tbl = $(chkBox).parents('table:eq(0)');
            if ($(chkBox).is(":checked")) {
                $(tbl).find(":checkbox").attr("checked", "checked");
            }
            else {
                $(tbl).find(":checkbox").removeAttr("checked");
            }
        }
        function CheckSelect() {
            if ($("[id$=grdPRSearch]").find("tbody td input[type=checkbox]:checked").length == 0) {
                ShowErrorMessage('<%= Resources.Report.Msg_Not_Selected %>', '<%= Resources.Messages.Information %>');
                return false;
            }
            else {
                return true;
            }
        }
        function ShowItemConfirm(btnId, message) {
            var isNewPop = true;
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.Messages.Information %>';
            msg = message;
            if ($("[id$=" + btnId + "]").length > 0) {
                $("#divConfirmation").html(msg).dialog({
                    modal: true,
                    height: 150,
                    width: 350,
                    title: msgTitle,
                    resizable: false,
                    buttons: {
                        OK: function (e) {
                            isNewPop = false;
                            $(this).dialog("close");
                            __doPostBack($("[id$=" + btnId + "]")[0].name, '');
                        },
                        Cancel: function (e) {
                            $("[id$=hdfIsValidItem]").val("0");
                            $(this).dialog("close");
                            if (typeof AfterDeleteConfirmationCancel == "function") {
                                AfterDeleteConfirmationCancel($("[id$=" + btnId + "]").id);
                            }
                            return false;
                        }
                    },
                    close: function (e) {
                        if (isNewPop) {
                            $("[id$=hdfIsValidItem]").val("0");
                            if (typeof AfterMessageClose == "function" && PageRebind == true) {
                                AfterMessageClose();
                            }
                        }
                    }
                });
            }
            return false;
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
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="<%$resources:ErpRes,Cancel %>" OnClick="ActionHandler"
                                            CommandName="CANCEL" TabIndex="97" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel"
                                            ToolTip="<%$resources:ErpRes,Cancel %>" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnRFQSearch" runat="server" class="tab-active">
                            <asp:LinkButton runat="server" ID="lbnRFQSearch" Text="<%$resources:PageNameRes,RFQSearch %>"
                                TabIndex="1" CommandName="RFQSEARCH" OnClick="ActionHandler" CssClass="tab-active"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnRFQRequest" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnRFQRequest" Text="<%$resources:PageNameRes,RFQRequest %>"
                                TabIndex="2" CommandName="RFQREQUEST" OnClick="ActionHandler" CssClass="tab-inactive"></asp:LinkButton>
                        </span></li>
                        <li><span id="spnRFQResponse" runat="server" class="tab-inactive">
                            <asp:LinkButton runat="server" ID="lbnRFQResponse" Text="<%$resources:PageNameRes,RFQResponse %>"
                                TabIndex="3" CommandName="RFQRESPONSE" OnClick="ActionHandler" CssClass="tab-inactive"
                                OnClientClick="javascript:return false;"></asp:LinkButton>
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <%--Align table cell according to design--%>
                        <asp:TableCell>
                            <div id="searchwrap" class="search-wrap-custom1">
                                <label for="SearchType">
                                    <%=Resources.Controls.SearchBy%></label>
                                <asp:DropDownList ID="ddlSearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType();"
                                    EnableViewState="false">
                                    <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                                    </asp:ListItem>
                                    <asp:ListItem Value="<%$ Resources:DataFieldRes, PRHeaderNo%>" Text="<%$ Resources:PRNO %>">
                                    </asp:ListItem>
                                    <asp:ListItem Value="<%$ Resources:DataFieldRes, ItemCode%>" Text="<%$ Resources:Item%>">
                                    </asp:ListItem>
                                    <asp:ListItem Value="Date" Text="<%$ Resources:BindValues, DateRange%>">
                                    </asp:ListItem>
                                    <asp:ListItem Value="DPT_NAME" Text="<%$ Resources:Department %>">
                                    </asp:ListItem>
                                </asp:DropDownList>
                                <div id="divSearchDtls">
                                    <asp:TextBox ID="txtSearchValue" runat="server" EnableViewState="false" TabIndex="2">
                                    </asp:TextBox>
                                    <asp:HiddenField ID="hdfSearchValue" runat="server" />
                                </div>
                                <div id="divDate" class="w31-5perc">
                                   <%-- <span class="padglft2"> 
                                        <%=Resources.Controls.FromDate%></span>--%>
                                        <asp:Label runat="server" ID="lblFromDt" Text="<%$Resources:Controls,FromDate%>" CssClass="margnlft5per"></asp:Label>
                                    <asp:TextBox ID="FromDate" runat="server" TabIndex="3" EnableViewState="false" CssClass="w100 margn-lft0"
                                        MaxLength="12"></asp:TextBox>
                                    <asp:HiddenField ID="hdfFrmDate" runat="server" />
                                    <span class="margnlft4per margnrgt3">
                                        <%=Resources.Controls.ToDate%></span>
                                    <%--<asp:Label runat="server" ID="lblToDt" Text="<%$Resources:Controls,ToDate%>"></asp:Label>--%>
                                    <asp:TextBox ID="ToDate" runat="server" TabIndex="4" EnableViewState="false" MaxLength="12" CssClass="w100 margn-lft0">
                                        
                                    </asp:TextBox>
                                    <asp:HiddenField ID="hdfToDate" runat="server" />
                                </div>
                                <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClick="ActionHandler"
                                    CommandName="SEARCH" EnableViewState="false" CssClass="margntop2"/>
                                <div class="clear">
                                </div>
                            </div>
                            <div class="gridwrap max-250">
                                <asp:GridView runat="server" ID="grdPRSearch" Width="100%" AllowSorting="True" OnSorting="ActionHandler"
                                    AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox CssClass="rdoSelection" TabIndex="17" runat="server" onclick="gGridSelectAllCheckBoxes(this);" />
                                        </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox CssClass="rdoSelection" TabIndex="17" runat="server" ID="chkSelection" onclick="gGridCheckAllSelect(this);" />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PRDate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPRDateLst" runat="server" Text='<%# Eval("PRDetailDate") %>' ToolTip='<%# Eval("PRDetailDate")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PRNO %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPRNOLst" runat="server" Text='<%# Eval("PRHeaderNo") %>' ToolTip='<%# Eval("PRHeaderNo")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Department %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeptLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PRDeptText"), 21) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("PRDeptText").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="16%" />
                                        </asp:TemplateField>                                         
                                        <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPRItemLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("ItemCode"), 13) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("ItemCode").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Description %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPRDescriptionLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("ItemName"), 17) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("ItemName").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Specifications %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPRSpecLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PRDetailSpec"), 18) %>'
                                                    ToolTip='<%# Eval("PRDetailSpec")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPRQuantityLst" runat="server" Text='<%# GetFormattedNumber(Eval("PRDetailBalanceQty")) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(GetFormattedNumber(Eval("PRDetailBalanceQty")).ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:UoM %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPRUoMLst" runat="server" Text='<%# Eval("PRDetailUOMText") %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("PRDetailUOMText").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="6%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ReqdDt %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPRReqDateLst" runat="server" Text='<%# Eval("PRDetailReqDate") %>'
                                                    ToolTip='<%# Eval("PRDetailReqDate")%>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnAddItem" runat="server" OnClick="ActionHandler" CommandName="ADDITEM"
                                                    SkinID="imbaddnew" ToolTip="Add" />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="button-wrap-right">
                                <asp:Button ID="btnAddPR" runat="server" OnClick="ActionHandler" CommandName="ADDRFQPR"
                                    Text="<%$ resources:Addselecteditem %>" OnClientClick="return CheckSelect()" /></div>
                                    <asp:HiddenField ID="hdfIsValidItem" runat="server" Value="0" />
                                     <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
                            <div class="clear">
                            </div>
                            <h3>
                                <%= GetLocalResourceObject("AdditionalItems").ToString() %></h3>
                            <div class="gridwrap">
                                <table class="gridwraptable gridwrap">
                                    <tr>
                                        <th align="left" width="23%">
                                            <%= GetLocalResourceObject("Category").ToString()%>
                                        </th>
                                        <th align="left" width="24%">
                                            <%= GetLocalResourceObject("Item").ToString()%>
                                        </th>
                                        <%--<th align="left" width="15%">
                                            <%= GetLocalResourceObject("Description").ToString()%>
                                        </th>--%>
                                        <th align="left" width="18%">
                                            <%= GetLocalResourceObject("Specifications").ToString()%>
                                        </th>
                                        <th width="5%">
                                            <%= GetLocalResourceObject("Qty").ToString()%>
                                        </th>
                                        <th width="5%">
                                            <%= GetLocalResourceObject("UoM").ToString()%>
                                        </th>
                                        <th align="left" width="5%">
                                            <%= GetLocalResourceObject("ReqdDt").ToString()%>
                                        </th>
                                        <th style="width: 3%">
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtItemCategory" runat="server" Width="95%"></asp:TextBox>
                                            <asp:HiddenField ID="hdfItemCategory" runat="server" />
                                            <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="vrfItemCategory" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="rfq" EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtItemCategory" Display="Dynamic" Text="*"
                                                ErrorMessage="<%$ resources:Err_Category %>"></asp:RequiredFieldValidator>
                                                </div>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItem" runat="server"  Width="95%"></asp:TextBox>
                                            <asp:HiddenField ID="hdfItem" runat="server" />
                                            <asp:Button runat="server" ID="btnSelectItem" CommandName="ITEMSELECTED" Style="display: none"
                                                EnableTheming="false" OnClick="ActionHandler" />
                                                <div class="starwrap">
                                            <asp:RequiredFieldValidator ID="vrfItem" CssClass="star" SetFocusOnError="true" ValidationGroup="rfq"
                                                EnableClientScript="true" InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>"
                                                runat="server" ControlToValidate="txtItem" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Item %>"></asp:RequiredFieldValidator>
                                                </div>
                                        </td>
                                        <%--<td>
                                            <asp:TextBox ID="txtItemDesc" runat="server" Width="91%" Enabled="false"></asp:TextBox>
                                        </td>--%>
                                        <td>
                                            <asp:TextBox ID="txtItemSpec" runat="server"  Width="95%" MaxLength="100"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemQty" runat="server" CssClass="numeric input-halfsmall-a"
                                                MaxLength="17"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfItemQuantity" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="rfq" EnableClientScript="true" runat="server" ControlToValidate="txtItemQty"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Quantity %>">
                                                </asp:RequiredFieldValidator>
<%--                                                <asp:RegularExpressionValidator ID="vreItemQuantity" runat="server" ControlToValidate="txtItemQty"
                                                    ErrorMessage="<%$ resources:Err_Quantity_Valid %>" ValidationExpression="^\$?([0-9]{0,11})?(\.[0-9]{0,3})?$"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="rfq">
                                                </asp:RegularExpressionValidator>
--%>                                                 <cc1:QuantityValidationP2P ID="vreItemQuantity" runat="server" ControlToValidate="txtItemQty"
                                                            NumberDigits="11" ErrorMessage="<%$ resources:Err_Quantity_Valid %>" Display="Dynamic"
                                                            Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="rfq" NonZero="true"></cc1:QuantityValidationP2P>
                                            </div>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemUOM" runat="server" CssClass="input-halfsmall-a" Enabled="false"></asp:TextBox>
                                            <asp:HiddenField ID="hdfItemUOM" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemReqDate" runat="server" CssClass="date-picker input-halfsmall-a"
                                                MaxLength="12"></asp:TextBox>
                                            <div class="starwrap">
                                                <asp:RequiredFieldValidator ID="vrfItemDate" CssClass="star" SetFocusOnError="true"
                                                    ValidationGroup="rfq" EnableClientScript="true" runat="server" ControlToValidate="txtItemReqDate"
                                                    Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ReqdDate %>">
                                                </asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="vreItemDate" CssClass="star" ValidationGroup="rfq"
                                                    runat="server" ControlToValidate="txtItemReqDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Err_ReqdDate_Valid %>"
                                                    ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                    EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                            </div>
                                        </td>
                                        <td>
                                        <asp:HiddenField ID="hdfTtemCode" runat="server" />
                                        <asp:HiddenField ID="hdfTtemName" runat="server" />
                                            <asp:ImageButton runat="server" SkinID="imbaddnew" ID="imbAddItem" OnClick="ActionHandler"
                                                CommandName="ADDLITEM" OnClientClick="javascript:ValidatePageNow('rfq')" ValidationGroup="rfq" />
                                        </td align="right">
                                    </tr>
                                </table>
                            </div>
                            <div runat="server" id="divSelectedItems" visible="false">
                                <h3>
                                    <%= GetLocalResourceObject("SelectedItems").ToString()%></h3>
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdPRSelected" Width="100%" AllowSorting="True"
                                        OnSorting="ActionHandler" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                             <asp:TemplateField HeaderText="<%$ resources:PRDate %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPRDateLst" runat="server" Text='<%# Eval("PRDetailDate") %>' ToolTip='<%# Eval("PRDetailDate")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:PRNO %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfPRDtlPK" runat="server" Value='<%# Eval("PRDetailPK") %>' />
                                                    <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%# Eval("PRDetailItem") %>' />
                                                    <asp:Label ID="lblPRNOLst" runat="server" Text='<%# Eval("PRHeaderNo") %>' ToolTip='<%# Eval("PRHeaderNo")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>  
                                            <asp:TemplateField HeaderText="<%$ resources:Department %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPRDeptLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PRDeptText"), 25) %>'
                                                    ToolTip='<%# HttpUtility.HtmlDecode(Eval("PRDeptText").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="18%" />
                                        </asp:TemplateField>                                          
                                            <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPRItemLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("ItemCode"), 13) %>'
                                                        ToolTip='<%#  HttpUtility.HtmlDecode(Eval("ItemCode").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Description %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPRDescriptionLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("ItemName"), 15) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval("ItemName").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Specifications %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPRSpecLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("PRDetailSpec"), 18) %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval("PRDetailSpec").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Qty %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPRQuantityLst" runat="server" Text='<%# GetFormattedNumber(Eval("PRDetailBalanceQty")) %>'
                                                        ToolTip='<%# GetFormattedNumber(Eval("PRDetailBalanceQty"))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="4%" CssClass="amount-numeric" />
                                            <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDummy" runat="server"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="2%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:UoM %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPRUoMLst" runat="server" Text='<%# Eval("PRDetailUOMText") %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval("PRDetailUOMText").ToString()) %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ReqdDt %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPRReqDateLst" runat="server" Text='<%# Eval("PRDetailReqDate") %>'
                                                        ToolTip='<%# Eval("PRDetailReqDate")%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                        SkinID="imbdeletegrid" ToolTip="Remove" />
                                                    <asp:ImageButton ID="btnItemRates" runat="server" OnClick="ActionHandler" CommandName="ITEMRATES"
                                                        SkinID="history" ToolTip="Show Rates" />
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" HorizontalAlign="Right"/>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                                <div class="button-wrap-right">
                                    <asp:Button ID="btnListVendors" runat="server" OnClick="ActionHandler" CommandName="LISTVENDORS"
                                        Text="<%$ resources:SelectVendors %>" Visible="false" /><div class="clear"></div></div>
                                <div runat="server" id="divSelectedVendors" visible="false">
                                    <h3>
                                        <%= GetLocalResourceObject("SelectedVendors").ToString()%></h3>
                                    <asp:CheckBoxList runat="server" ID="chlVendors">
                                    </asp:CheckBoxList>
                                    <div class="button-wrap-right">
                                        <asp:Button ID="btnRequestQuote" runat="server" OnClick="ActionHandler" CommandName="REQUESTQUOTE"
                                            Text="<%$ resources:RequestQuote %>" />
                                    </div>
                                </div>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
               
               
               <div  style="display: none">
               <asp:Button ID="btnAddNew" runat="server" OnClick="ActionHandler"
                CommandName="ADDLITEMNEW" OnClientClick="javascript:ValidatePageNow('rfq')" ValidationGroup="rfq" />
                  <asp:Button ID="btnAddNewInGrid" runat="server" OnClick="ActionHandler"
                CommandName="ADDLITEMNEW"  />
               </div>
                <div id="ItemRateDialog" style="display: none" class="content-wrapper">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label runat="server" ID="lblItemCode" Text="<%$ resources:ItemCode %>" AssociatedControlID="lblItemCodeTxt"></asp:Label>
                                    <asp:Label runat="server" ID="lblItemCodeTxt"></asp:Label>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <asp:Label runat="server" ID="lblItemName" Text="<%$ resources:ItemName %>" AssociatedControlID="lblItemNameTxt"></asp:Label>
                                    <asp:Label runat="server" ID="lblItemNameTxt"></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdItemRates" Width="100%" AllowSorting="True" OnSorting="ActionHandler"
                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:Vendor %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVendorLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ItemRateVntTxt), 18) %>'
                                            ToolTip='<%# HttpUtility.HtmlDecode(Eval(Resources.DataFieldRes.ItemRateVntTxt).ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="16%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Rating %>" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRatingLst" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval(Resources.DataFieldRes.ItemRateRating), 10) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.ItemRateRating) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                              <%--  <asp:TemplateField HeaderText="<%$ resources:LastQuotedDate %>">--%>
                                 <asp:TemplateField HeaderText="Lst Qut Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastQuotedDateLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.ItemRateQuoteDate, Resources.ErpRes.DateFormatGrid) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.ItemRateQuoteDate, Resources.ErpRes.DateFormatGrid) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="<%$ resources:LastQuotedRate %>">--%>
                                <asp:TemplateField HeaderText="Lst Qut Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastQuotedRateLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.ItemRateQuoteRate, "{0:c}") %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.ItemRateQuoteRate, "{0:c}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                     <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Currency">
                                    <ItemTemplate>
                                        <asp:Label ID="lblVernCurrency" runat="server" Text='<%# Eval("VEN_CURRENCY_TEXT") %>'
                                            ToolTip='<%# Eval("VEN_CURRENCY_TEXT") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="6%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="<%$ resources:LastOrderDate %>">--%>
                                <asp:TemplateField HeaderText="Lst Ordr Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastOrderDateLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.ItemRateOrderDate, Resources.ErpRes.DateFormatGrid) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.ItemRateOrderDate, Resources.ErpRes.DateFormatGrid) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" />
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="<%$ resources:LastOrderRate %>">--%>
                                <asp:TemplateField HeaderText="Lst Ordr Rate">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastOrderRateLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.ItemRateOrderRate, "{0:c}") %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.ItemRateOrderRate, "{0:c}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                     <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="<%$ resources:LastOrderQty %>">--%>
                                <asp:TemplateField HeaderText="Lst Ordr Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLastOrderQtyLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.ItemRateOrderQty) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.ItemRateOrderQty) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                     <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:LeadTime %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblLeadTimeLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.ItemRateLaedTime) %>'
                                            ToolTip='<%# Eval(Resources.DataFieldRes.ItemRateLaedTime) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                      <HeaderStyle CssClass="amount-numeric" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsPage" ValidationGroup="rfq" runat="server" />
                </div>
            </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
