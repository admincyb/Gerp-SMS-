<%@ Page Title="<%$ Resources:Captions,Title_PackingTransaction %>"  Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="PackingTransaction.aspx.cs" Inherits="ERPSMS_v01.ProductionPlanning.PackingTransaction"
    Theme="Classic" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
        var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.DatePickerCommon("txtDate");
            //GrandScriptUtils.DatePicker("txtFromDate");
            //GrandScriptUtils.DatePicker("txtToDate");
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomer", true, true, "CUSTOMERLIST");

        }
        function AfterAutoCompleteSelect(targetControlID) {
            if (targetControlID == "txtCustomer") {
                $("[id$=btnCustSelected]").click();
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
        // Calculate A grade realization for a particular row
        function CalculateBalance() {
            var valQty = parseFloat($("[id*=txtQty]").val());
            var valOrderQty = parseFloat($("[id*=hdfOrderQty]").val());
            var valBalance = 0.0;
            if (!isNaN(valQty) && !isNaN(valOrderQty)) {
                valBalance = valOrderQty - valQty;
            }
            $("[id*=txtBalToPack]").val(valBalance);
            $("[id*=hdfBalToPack]").val(valBalance);

            return false;
        }

        function ShowDeleteConfirm(btn) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.Messages.Information %>';
            msg = 'Do you want to delete the record?';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $(this).dialog("close");
                        __doPostBack(btn.name, '');
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        if (typeof AfterDeleteConfirmationCancel == "function") {
                            AfterDeleteConfirmationCancel(btn.id);
                        }
                        return false;
                    }
                }
            });
            return false;
        }

        function CheckQty(sender, args) {
            var packedQty = parseInt($("[id$=txtQty]").val());
            var balToPack = parseInt($("[id$=hdfBalToPack]").val());
            var orderQty = parseInt($("[id$=txtOrderQty]").val());
            if ((packedQty <= orderQty) && balToPack >=0)
                args.IsValid = true;
            else
                args.IsValid = false;
            if (isNaN(packedQty)) {
                args.IsValid = true;
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
                $("[id$='spnListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnListing']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='spnDetails']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnDetails']").removeClass("tab-active").addClass("tab-inactive");
            }
            else {
                $("[id$='spnListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='lbnListing']").removeClass("tab-active").addClass("tab-inactive");
                $("[id$='spnDetails']").removeClass("tab-inactive").addClass("tab-active");
                $("[id$='lbnDetails']").removeClass("tab-inactive").addClass("tab-active");
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
    </script>
</asp:Content>
<asp:Content ID="cntMain" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupDespatch" runat="server">
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
                                          <asp:Button ID="btnSave" runat="server" Text="<%$ resources:Save %>" ToolTip ="<%$ resources:Save %>" OnClick="ActionHandler"
                                        CommandName="SAVE" CommandArgument="SEC_ActionPanel" TabIndex="22" ValidationGroup="pack"
                                        SkinID="btnInner-Save" CausesValidation="true" OnClientClick="javascript:ValidatePageNow('pack')" />
                                    </li>
                                    <li id="pnlCancel" runat="server">
                                        <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>" ToolTip="<%$Resources:Controls,Cancel%>"
                                        CommandName="CANCEL" TabIndex="23" OnClick="ActionHandler" />
                                    </li>

                                    
                                    
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <li>
                                          <asp:Button CommandName="ADD_PACKING"  runat="server" OnClick="ActionHandler" TabIndex ="10"
                                                    ID="btnAddPackingDtl" SkinID="btnInner-Save" Text="<%$ resources:AddPacking %>" ToolTip ="<%$ resources:AddPacking %>" />
                                    </li>
                                   
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnListing" runat="server" class="tab-active">
                            <asp:LinkButton ID="lbnListing" runat="server" Text="<%$ resources:Controls,List %>"
                                CommandName="CANCEL" CssClass="tab-active" OnClick="ActionHandler" TabIndex="1" ToolTip="<%$ resources:Controls,List %>"/>
                        </span></li>
                        <li><span id="spnDetails" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnDetails" runat="server" Text="<%$ resources:Controls,Details %>"
                                CommandName="ADD_PACKING" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="2"  ToolTip="<%$ resources:Controls,Details %>" />
                        </span></li>
                    </ul>
                </div>
            </div>




            <div class="content-wrapper">
                <div id="divListing" runat="server">
                    <asp:Table runat="server" ID="Table1" CssClass="asptbllinks">
                        <asp:TableRow>
                            <asp:TableCell>
                               
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="65" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="66" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFromDate" TabIndex="4" CssClass="Uidate-picker"
                                                MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtToDate"  TabIndex="5" CssClass="Uidate-picker"
                                                MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />
                                            <div class="clear">
                                            </div>
                                             <asp:Label ID="lblSearchButton" runat="server" AssociatedControlID="btnSearch"></asp:Label>
                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ resources:Controls,Search %>" ToolTip="<%$ resources:Controls,Search %>"
                                                OnClick="ActionHandler" TabIndex="8" CommandName="SEARCH" SkinID="btnInner-search" />
                                            <asp:Button ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="9" ToolTip="<%$ resources:Controls,Clear %>"
                                                OnClick="ActionHandler" CommandName="CLEAR" SkinID="btnInner-cancel-dsd" />
                                        </div>
                                    </td>

                                </tr>
                              
                            </table>
                            <div class="clear">
                            </div>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <div class="gridwrap">
                                    <asp:GridView runat="server" ID="grdPackingList" Width="100%" AutoGenerateColumns="false"
                                        EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true" OnRowDataBound="grdPackingList_RowDataBound">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:SlNo %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSlNo" runat="server" Text='<%# Eval(Resources.DataFieldRes.PackingRowNo)%>' ToolTip='<%# Eval(Resources.DataFieldRes.PackingRowNo)%>'>
                                                    </asp:Label>
                                                    <asp:HiddenField ID="hdfPkhPK" runat="server" Value='<%# Eval(Resources.DataFieldRes.PackingPK)%>' />
                                                    <%-- <asp:Label ID="lblPkhPK" runat="server" Text='<%# Eval(Resources.DataFieldRes.PackingPK)%>' Visible="false">--%>
                                                    <%-- </asp:Label>--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="5%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Shift %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblShift" runat="server" Text='<%# Eval(Resources.DataFieldRes.PackingShiftText)%>' ToolTip='<%# Eval(Resources.DataFieldRes.PackingShiftText) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,Date %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDateLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.PackingDate,Resources.Constants.DateFormatGrid) %>'
                                                        ToolTip='<%# Eval("PKH_DATE",Resources.Constants.DateFormatGrid) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:PackedItems %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPackedItem" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.PackingItemText)),50)%>'  ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.PackingItemText)) %>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="39%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Controls,Remarks %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.PackingRemarks)),20)%>'
                                                        ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.PackingRemarks))%>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotal" runat="server" Text="<%$ resources:Total %>"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign ="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:QtyPack %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval(Resources.DataFieldRes.PackingQtyPacked,"{0:n0}")%>' ToolTip='<%# Eval(Resources.DataFieldRes.PackingQtyPacked,"{0:n0}")%>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                 <HeaderStyle CssClass="amount-numeric" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lbllQtyTotal" runat="server">
                                                    </asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle Width="10%" HorizontalAlign="Right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Action %>">
                                                <ItemTemplate>
                                                    <asp:ImageButton CssClass="btnInner-Edit" runat="server" ToolTip="<%$ resources:Controls,Edit  %>" 
                                                        ID="imbEdit" SkinID="imbeditgrid" EnableViewState="false" OnClick="ActionHandler" TabIndex ="8"
                                                        CommandName="EDIT_LIST_ACTION" />
                                                    <asp:ImageButton  runat="server" Visible="true" ID="imbDelete" ToolTip="<%$ resources:Captions, Remove %>"
                                                        SkinID="imbdeletegrid" EnableViewState="false" OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);"
                                                        CommandName="DELETE_LIST_ACTION" TabIndex ="9"/>
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                                </div>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div id="divEntryForm" runat="server" visible="false">
                    <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                        <asp:TableRow>
                            <asp:TableCell>
                                <%-- <div class="button-container-bottom">
                                <asp:Button CommandName="ADD" runat="server" OnClick="ActionHandler" ID="btnAdd"
                                    SkinID="btnInner-New" Text="<%$ resources:gErpProductionRes,Add %>" />
                            </div>--%>
                              
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <table class="table-devide">
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <%--To Date  --%>
                                                <asp:Label ID="litDate" Text="<%$ resources:Controls,Date %>" AssociatedControlID="txtDate"
                                                    runat="server"></asp:Label>
                                                <asp:TextBox ID="txtDate" TabIndex="11" runat="server" CssClass="Uidate-picker"
                                                MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                                <div class="starwrap">
                                                    <asp:RequiredFieldValidator ID="vrfDate" CssClass="star" SetFocusOnError="true" ValidationGroup="pack"
                                                        EnableClientScript="true" Text="*" runat="server" ControlToValidate="txtDate"
                                                        Display="Dynamic" ErrorMessage="<%$ resources:Msg_Date %>"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="vrgDate" CssClass="star" ValidationGroup="pack"
                                                        runat="server" ControlToValidate="txtDate" SetFocusOnError="true" ErrorMessage="<%$ resources:Msg_DateValid %>"
                                                        ValidationExpression="^(?:((31-(Jan|Mar|May|Jul|Aug|Oct|Dec))|((([0-2]\d)|30)-(Jan|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec))|(([01]\d|2[0-8])-Feb))|(29-Feb(?=-((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00)))))-((1[6-9]|[2-9]\d)\d{2})$"
                                                        EnableClientScript="true" Display="Dynamic" Text="*"></asp:RegularExpressionValidator>
                                                </div>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblCustomer" Text="<%$ resources:Controls,Customer %>" AssociatedControlID="ddlCustomer"
                                                    runat="server"></asp:Label>
                                                <asp:DropDownList ID="ddlCustomer" CssClass="Large" TabIndex="13" runat="server"
                                                    ValidationGroup="detail" AutoPostBack="true" OnSelectedIndexChanged="ActionHandler" Visible="false">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtCustomer" runat="server" MaxLength="100" TabIndex="13"></asp:TextBox>
                                                <asp:HiddenField ID="hdfCustomer" runat="server" />
                                                <asp:Button ID="btnCustSelected" runat="server" OnClick="ActionHandler" CommandName="CUSTOMERSELECTED"
                                                EnableTheming="false" Style="display: none" />
                                                <asp:RequiredFieldValidator ID="vrfddlCustomer" CssClass="star" SetFocusOnError="true"
                                                    EnableClientScript="true" ValidationGroup="detail" Text="*" runat="server" InitialValue="0"
                                                    ControlToValidate="txtCustomer" Display="Dynamic" ErrorMessage="<%$ resources:MSg_Customer %>"></asp:RequiredFieldValidator>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblShift" runat="server" Text="<%$ resources:Shift %>" AssociatedControlID="ddlShift"></asp:Label>
                                                <asp:DropDownList ID="ddlShift" TabIndex="12" runat="server" CssClass="medium">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="vrfShift" SetFocusOnError="true" ValidationGroup="pack"
                                                    CssClass="star" runat="server" ControlToValidate="ddlShift" Text="*" ErrorMessage="<%$ resources:Msg_SelectShift%>"
                                                    InitialValue="0" Display="Dynamic"></asp:RequiredFieldValidator>
                                                <%--   <asp:Label ID="lblDespatchNo" Text="<%$ resources:DispatchNo %>" AssociatedControlID="lblDespatchNoTxt"
                                                    runat="server"></asp:Label>
                                                <asp:Label ID="lblDespatchNoTxt" runat="server" CssClass="medium"></asp:Label>--%>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                                <asp:Label ID="lblRemarks" runat="server" AssociatedControlID="txtRemarks" Text="<%$ resources:Controls,Remarks %>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtRemarks" MaxLength="500" TabIndex="14" TextMode="MultiLine"
                                                    onkeydown="limitText(this,500);" onkeyup="limitText(this,500);" EnableTheming="false"
                                                    CssClass="multiline-2col"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="vreRemarks" runat="server" ControlToValidate="txtRemarks"
                                                    ErrorMessage="<%$ resources:Msg_Exceed_MaxLen %>" ValidationExpression="^[\s\S]{0,500}$"
                                                    Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="pack"></asp:RegularExpressionValidator>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <div id="divOrderDetails" runat="server">
                                    <h5>
                                        <asp:Label ID="lblHdr" runat="server" Text="<%$ resources:PackingDetails %>"></asp:Label>
                                    </h5>
                                    <div class="detail-co3-packg">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblLoad" runat="server" Text="<%$ resources:SaleContractNo %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ resources:ItemDescription %>"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ resources:SaleOrderQty %>"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label2" Text="<%$ resources:BalToPack %>" runat="server"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label6" runat="server" Text="<%$ resources:Controls,Quantity %>"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label7" runat="server" Text="<%$ resources:Controls,UOM %>"></asp:Label>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:DropDownList ID="ddlSaleOrderNo" AutoPostBack="true" Width="130px" TabIndex="15"
                                                        OnSelectedIndexChanged="ActionHandler" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfSaleOrderNo" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="detail" EnableClientScript="true" Text="*" runat="server" InitialValue="0"
                                                        ControlToValidate="ddlSaleOrderNo" Display="Dynamic" ErrorMessage="<%$ resources:MSg_SaleOrderNo %>"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlSaleOrderItem" runat="server" TabIndex="16" AutoPostBack="true"
                                                        Width="450px" OnSelectedIndexChanged="ActionHandler">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="vrfSaleOrderItem" CssClass="star" SetFocusOnError="true"
                                                        ValidationGroup="detail" EnableClientScript="true" Text="*" runat="server" InitialValue="0"
                                                        ControlToValidate="ddlSaleOrderItem" Display="Dynamic" ErrorMessage="<%$ resources:MSg_SaleOrderItem %>"></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOrderQty" ReadOnly="true" Width="75px" runat="server" CssClass ="numeric"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfOrderQty" runat="server" Value="0" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtBalToPack" ReadOnly="true" Width="75px" runat="server" CssClass ="numeric"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfBalToPack" runat="server" Value="0" />
                                                </td>
                                                <td style="width:15%;">
                                                    <asp:TextBox ID="txtQty" runat="server" Width="75px" onkeyup="return CalculateBalance();" CssClass ="numeric"
                                                        TabIndex="17" MaxLength="9"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfQty" CssClass="star" SetFocusOnError="true" ValidationGroup="detail"
                                                            EnableClientScript="true" Text="*" runat="server" ControlToValidate="txtQty"
                                                            Display="Dynamic" ErrorMessage="<%$ resources:Msg_Qty %>"></asp:RequiredFieldValidator>
                                                        <%--<asp:RegularExpressionValidator ID="vrgQty" runat="server" ControlToValidate="txtQty"
                                                            ErrorMessage="<%$ resources: Msg_QtyValid %>" ValidationExpression="^\$?([0-9]{0,14})?(\.[0-9]{0,3})?$"
                                                            Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="detail">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:CompareValidator ID="cmpQty" runat="server" ControlToValidate="txtQty" Display="Dynamic"
                                                            Text="*" CssClass="star" ErrorMessage="<%$ Resources:Msg_Qty_GreaterThanZero %>"
                                                            ValidationGroup="detail" Operator="GreaterThan" Type="Integer" ValueToCompare="0" />--%>
                                                        <asp:RangeValidator ID="vrnQty" runat="server" ControlToValidate="txtQty" Display="Dynamic"
                                                            Text="*" CssClass="star" ErrorMessage="<%$ resources:Valid_Qty %>" ValidationGroup="detail"
                                                            MinimumValue="1" Type="Integer" MaximumValue="999999999">
                                                        </asp:RangeValidator>
                                                        <%-- <asp:CompareValidator ID="cmpQty" CssClass="star" SetFocusOnError="true" ValidationGroup="detail"
                                                            EnableClientScript="true" Text="*" runat="server" ControlToValidate="txtQty"
                                                            ControlToCompare="txtBalToPack" Operator="LessThanEqual" ErrorMessage="<%$ resources:Err_Qty %>"></asp:CompareValidator>--%>
                                                        <asp:CustomValidator ID="customQty" runat="server" ValidateEmptyText="true" ClientValidationFunction="CheckQty"
                                                            ErrorMessage="<%$ resources:Err_Qty %>" Text="*" EnableClientScript="true" ControlToValidate="txtQty"
                                                            CssClass="star" Display="Dynamic" ValidationGroup="detail"></asp:CustomValidator>
                                                    </div>
                                                    <%-- <div class="clear">
                                                    </div>--%>
                                                </td>
                                                <td>
                                                    <%-- <asp:DropDownList ID="ddlUOM" TabIndex="7" runat="server" Width="90px">
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtUOM" runat="server" Enabled="false" CssClass="small"></asp:TextBox>
                                                    <asp:HiddenField ID="hdfUomPK" runat="server" Value="0" />
                                                    <%-- <asp:RequiredFieldValidator ID="vrfUOM" CssClass="star" SetFocusOnError="true" ValidationGroup="detail"
                                                        EnableClientScript="true" Text="*" runat="server" InitialValue="-1" ControlToValidate="ddlUOM"
                                                        Display="Dynamic" ErrorMessage="<%$ resources:MSg_UOM %>"></asp:RequiredFieldValidator>--%>
                                                    <asp:HiddenField ID="hdfProductPk" runat="server" Value="0" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imdAdd" runat="server" OnClick="ActionHandler" TabIndex="18"
                                                        ValidationGroup="detail" OnClientClick="javascript:ValidatePageNow('detail')"
                                                        SkinID="imbaddnew" CommandName="ADD_ACTION" CommandArgument="SEC_ActionPanel"
                                                        CausesValidation="true" />
                                                    <%--  <asp:ImageButton ID="imbCancel" runat="server" OnClick="ActionHandler" SkinID="imgbtnAdd"
                                                    CommandName="CANCEL_ACTION" CommandArgument="SEC_ActionPanel" />--%>
                                                    <asp:HiddenField ID="hdfSLNo" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdfCustomerPK" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdfLastModDate" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdfSaleOrderDetailPK" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="gridwrap">
                                        <asp:GridView runat="server" ID="grdPackingDetails" Width="100%" AutoGenerateColumns="false"
                                            OnRowEditing="ActionHandler" EmptyDataRowStyle-CssClass="emptytable" OnRowDataBound="ActionHandler">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:SaleContractNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSaleOrderNo" runat="server" Text='<%#Eval(Resources.DataFieldRes.PackingSaleOrderText) %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="14%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ItemDescription %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItemName" runat="server" Text='<%#Eval(Resources.DataFieldRes.PackingSaleOrderDetailsText) %>'
                                                            Visible="true"></asp:Label>
                                                        <asp:Label ID="lblSlNo" runat="server" Text='<%#Eval(Resources.DataFieldRes.PackingSlNo) %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="44%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:SaleOrderQty %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSaleOrderQty" runat="server" Text='<%#Eval(Resources.DataFieldRes.PackingOrderQty,"{0:n0}") %>'
                                                            Visible="true"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="10%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:BalToPack %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBalToPack" runat="server" Text='<%#Eval("BalToPack","{0:n0}") %>'
                                                            Visible="true"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" HorizontalAlign="Right" />
                                                    <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Controls,Quantity %>">
                                                    <ItemTemplate>
                                                        <%-- <asp:Label ID="lblQtyLst" runat="server" Text='<%#Eval("DespatchedQty", "{0:n3}") %>'
                                                        ToolTip='<%#Eval("DespatchedQty", "{0:n3}") %>'>
                                                    </asp:Label>--%>
                                                        <asp:TextBox ID="txtQty" runat="server" ValidationGroup="despatch"
                                                            Text='<%# Eval("QtyPacked")%>' CssClass="medium numeric"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="vrfGrdQty" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="despatch" EnableClientScript="true" Text="*" runat="server"
                                                            ControlToValidate="txtQty" Display="Dynamic" ErrorMessage="<%$ resources:Msg_Qty %>"></asp:RequiredFieldValidator>
                                                        <%-- <asp:RegularExpressionValidator ID="vreGrdQty" runat="server" ErrorMessage="<%$ Resources:Msg_ValidNumber %>"
                                                            Text="*" ControlToValidate="txtQty" CssClass="star" Display="Dynamic" ValidationGroup="despatch"
                                                            ValidationExpression="[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?[0-9]?(\.[0-9][0-9]?)?">
                                                            <asp:CompareValidator ID="cmpGrdQty" runat="server" ControlToValidate="txtQty" Display="Dynamic"
                                                                Text="*" CssClass="star" ErrorMessage="<%$ Resources:Msg_Qty_GreaterThanZero %>"
                                                                ValidationGroup="despatch" Operator="GreaterThan" Type="Integer" ValueToCompare="0" />
                                                        </asp:RegularExpressionValidator>--%>
                                                        <asp:RangeValidator ID="vrnGrdQty" runat="server" ControlToValidate="txtQty" Display="Dynamic"
                                                            Text="*" CssClass="star" ErrorMessage="Enter Valid Quantity" ValidationGroup="despatch"
                                                            MinimumValue="1" Type="Integer" MaximumValue="999999999">
                                                        </asp:RangeValidator>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7%" HorizontalAlign="Right" />
                                                     <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Controls,UOM %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblItem" runat="server" Text='<%# Eval("UomCode ")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" Width="7%" />
                                                     <HeaderStyle CssClass="amount-numeric" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Action %>">
                                                    <ItemTemplate>
                                                        <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" EnableViewState="false" TabIndex ="20"
                                                            OnClick="ActionHandler" CommandName="EDIT_ACTION" ToolTip="<%$ resources:Controls,Edit  %>" />
                                                        <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" TabIndex ="21"
                                                            EnableViewState="false" OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirm(this);"
                                                            CommandName="DELETE_ACTION" ToolTip="<%$ resources:Captions, Remove %>" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="6%" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div id="diverror" style="display: none">
                <%--Use this label to bind the server errors--%>
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vspack" ValidationGroup="pack" runat="server" />
                  <asp:ValidationSummary ID="vsSearch" ValidationGroup="search" runat="server" />
                <asp:ValidationSummary ID="vsDetail" ValidationGroup="detail" runat="server" />
            </div>
            <div id="DiverrorMessages" runat="server" style="display: none">
                <asp:GridView runat="server" ID="grdError" Width="100%" AutoGenerateColumns="false"
                    EmptyDataRowStyle-CssClass="emptytable" EnableTheming="false" ShowHeader="false"
                    BorderWidth="0">
                    <RowStyle HorizontalAlign="Center" />
                    <EmptyDataTemplate>
                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblErrorMsgLst" runat="server" Text='<%# Eval(Resources.DataFieldRes.dbRetValTxt) %>' Width ="89%"></asp:Label>
                                <%--  ((int)Eval(Resources.DataFieldRes.dbRetVal) > 0 ? "<span >" : "<span style='color:Red' >") + Eval(Resources.DataFieldRes.dbRetValTxt).ToString() + "</span>"--%>
                            </ItemTemplate>
                            <ItemStyle Width="100%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
