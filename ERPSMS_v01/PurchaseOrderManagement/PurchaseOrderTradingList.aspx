<%@ Page Title="<%$ Resources:Captions,Title_PurchaseOrderTrading %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    EnableEventValidation="false" Theme="ClassicExt" CodeBehind="PurchaseOrderTradingList.aspx.cs"
    Inherits="ERPSMS_v01.PurchaseOrderManagement.PurchaseOrderTradingList" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/TransactionComments.ascx" TagName="TransactionComments"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        //var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");
        var url = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

        function InitComponents() {
            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            var poAutoCompleteURL = "POGeneration.do?Action=GetPurchaseAutoSearchValue&AUTOSEARCH=1";
            var poPageURL = "/PurchaseOrderManagement/PurchaseOrderTrading.aspx?TYPE=1";
            GrandScriptUtils.MakeAutoCompleteDDL("txtVendor", poAutoCompleteURL + "&SearchType=VEN_NAME" + "&PageURL=" + poPageURL, "hdfVendor", true, true);
            GrandScriptUtils.MakeAutoCompleteDDL("txtReqFor", poAutoCompleteURL + "&SearchType=DPT_NAME" + "&PageURL=" + poPageURL, "hdfReqFor", true, true);
            GrandScriptUtils.MakeAutoCompleteDDL("txtPONumber", poAutoCompleteURL + "&SearchType=POH_NO" + "&PageURL=" + poPageURL, "hdfPONumber", true, true);

            //To set visibility of Hierarchical grid expand button
            ShowHideExpand();
            //****************Multiple Plant**************************************************        
            var isMultiplePlant = $("[id$=hdfIsMultiplePlant]").val();
            if (parseInt(isMultiplePlant) == 1) {
                $("[id$=lblPlantCode]").show();
                $("[id$=ddlPlantCode]").show();
            }
            else {
                $("[id$=lblPlantCode]").hide();
                $("[id$=ddlPlantCode]").hide();
            }
            //***************************************************************************************
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

        function ShowHideExpand() {
            ///<summary>
            /// Used to Show/Hide Grid Expad Button
            ///</summary>

            $("[id*=hdfHasChildren]").each(function () {
                $(this).parent().parent().find('a.GridExpandCollapseButton').css("visibility", ($(this).val() == "1" ? "visible" : "hidden"));
            });
        }

        function AfterGridExpand(row) {

            if ($("[id$=grdPOList]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedOrders]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnOrderDetails]").click();
                }
            }
            else {
                var gridType = 0;
                var names = $(row).parent().parent().attr('id').split('_');

                if (names.length == 1) {
                    gridType = names[0] == "grdGRNList" ? 1 : names[0] == "grdGINList" ? 2 : 0;
                }
                else if (names.length > 1) {
                    gridType = (names[names.length - 1] == "grdGRNList" || names[names.length - 2] == "grdGRNList") ? 1
                    : (names[names.length - 1] == "grdGINList" || names[names.length - 2] == "grdGINList") ? 2 : 0;
                }
                if (gridType == 1) {
                    var hdf = $(row).find("[id*=hdfIsExpandedGRNList]");
                    if (hdf.val() == "0") {
                        $(row).find("input[id*=btnGetGIN]").click();
                    }
                }
                if (gridType == 2) {
                    var hdf = $(row).find("[id*=hdfIsExpandedGinList]");
                    if (hdf.val() == "0") {
                        $(row).find("input[id*=btnGetStockTransfer]").click();
                    }
                }
            }

            if ($("[id$=grdPOItems]").attr('id') == $(row).parent().parent().attr('id')) {
                var hdf = $(row).find("[id*=hdfIsExpandedPOItem]");
                if (hdf.val() == "0") {
                    $(row).find("input[id*=btnGetGrn]").click();
                }
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
        function ItemListSelection() {
            var selectedRowColor;
            selectedRowColor = '<%= Resources.ErpRes.selectedRowColor %>';

            $("#[id*=grdPOList] input[type=hidden][id*=hdfPOHPK]").each(function (index) {
                if ($(this).val() == $("[id$=hdfSelectedItemPOPK]").val()) {
                    $(this).closest('tr').css('background-color', selectedRowColor);
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlTradingPO" runat="server">
        <ContentTemplate>
            <div class="fixed-buttons-normal">
                <div class="Button-container">
                    <asp:Table runat="server" ID="tblButton">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlListing">
                                    <li>
                                        <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                            CommandName="NEW" OnClick="ActionHandler" ToolTip="<%$resources:Controls,Add %>" 
                                            TabIndex="21"  />
                                    </li>                                    
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
                    <asp:HiddenField ID="hdnClosePO" runat="server" Value="0" />
                    <div id="divVendorData">
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="content-wrapper">
                <%--use the width property of the below table corresponding to the contents in the page--%>
                <asp:Table runat="server" ID="tblTemplate" CssClass="tablelayout asptbllinks">
                    <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <%--Align table cell according to design--%>
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
                            <table class="table-devide" id="tbladvancedSearch" style="/*margin-top: 8px; */ background: #f2f2f2;">
                                <tr>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblTrnStatus" Text="<%$ resources:TrnStatus %>" AssociatedControlID="ddlTrnStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlTrnStatus" runat="server" CssClass="select-small-e" TabIndex="1">
                                            </asp:DropDownList>
                                            <asp:Label runat="server" ID="lblIONo" Text="<%$ resources:IoNo %>" CssClass="middle-lbl-small-b"
                                                AssociatedControlID="txtIONo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtIONo" TabIndex="2" CssClass="input-small"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblReqFor" Text="<%$ resources:ReqFor %>" AssociatedControlID="txtReqFor"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtReqFor" TabIndex="5" CssClass="select-small-c1"></asp:TextBox>
                                            <asp:HiddenField ID="hdfReqFor" runat="server" />
                                            <asp:Label runat="server" ID="lblPrNo" Text="<%$ resources:PrNo %>" AssociatedControlID="txtPrNo"
                                                CssClass="middle-lbl-small-b margnlft-minus2"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPrNo" TabIndex="6" CssClass="input-small"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <%-------------------PO type ------------------------------%>
                                            <asp:Label runat="server" ID="lblPoType" Text="<%$ Resources:Controls,Type%>" AssociatedControlID="ddlPOType"></asp:Label>
                                            <asp:DropDownList ID="ddlPOType" runat="server" CssClass="select-small-d" TabIndex="8">
                                            </asp:DropDownList>
                                            <%----------------End PO type --------------------------%>
                                            <%-----------   Plant ----------------%>
                                            <asp:Label runat="server" ID="lblPlantCode" Text="<%$ Resources:Controls,CompanyPlant%>"
                                                AssociatedControlID="ddlPlantCode" CssClass="middle-lbl-small-b"></asp:Label>
                                            <asp:DropDownList ID="ddlPlantCode" runat="server" CssClass="select-small-a" TabIndex="8">
                                            </asp:DropDownList>
                                            <%--------    End Plant ----------%>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S padgtop7">
                                            <asp:Label runat="server" ID="lblFromDate" Text="<%$ resources:FromDate %>" AssociatedControlID="txtFromDate"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtFromDate" CssClass="input-small" TabIndex="3"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <asp:Label runat="server" ID="lblToDate" Text="<%$ resources:ToDate %>" AssociatedControlID="txtToDate"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtToDate" CssClass="input-small" TabIndex="4" onkeydown="return CheckKey(event)"
                                                onpaste="return false;"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblItemName" Text="<%$ Resources:Controls, ItemName%>"
                                                AssociatedControlID="txtPrNo"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtItemname" CssClass="input-half" TabIndex="7"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblVendor" Text="<%$ resources:Vendor %>" AssociatedControlID="txtVendor"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtVendor" CssClass="select-half-b margnbotm0" TabIndex="8">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfVendor" runat="server" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S div-separatn">
                                            <asp:Label runat="server" ID="lblPoNumber" Text="<%$ resources:PoNo %>" AssociatedControlID="txtPONumber"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPONumber" TabIndex="9" CssClass="input-small margnbotm0">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="hdfPONumber" runat="server" Value="0" />
                                            <asp:Label runat="server" ID="lblStatus" Text="<%$ resources:POStatus %>" AssociatedControlID="ddlStatus"
                                                CssClass="middle-lbl-small-d"></asp:Label>
                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small-b margnbotm0 margn-rgt2"
                                                TabIndex="10" >
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterNotClosed %>" Text="<%$ Resources:BindValues, StatusFilterNotClosed%>"
                                                    Selected="True">
                                                </asp:ListItem>
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterAll %>" Text="<%$ Resources:BindValues, StatusFilterAll%>">
                                                </asp:ListItem>
                                                <asp:ListItem Value="<%$ Resources:ConfigurationsRes, StatusFilterClosed %>" Text="<%$ Resources:BindValues, StatusFilterClosed%>">
                                                </asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:Label ID="Label1" runat="server" CssClass="middle-lbl-xsmall-d style-none margnbotm0"></asp:Label>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                TabIndex="11" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                                OnClick="ActionHandler" CommandName="SEARCH" />
                                            <asp:ImageButton ID="btnClear" runat="server" TabIndex="12" Style="margin-bottom: 0px!important;
                                                margin-top: 2px;" ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                                                OnClick="ActionHandler" CommandName="CLEAR" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap hierarchical-wrap">
                                <asp:GridView runat="server" ID="grdPOList" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="emptytable"
                                    Width="100%" OnRowDataBound="ActionHandler" PageSize="<%$ resources:PageSize %>"
                                    OnRowCommand="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbPOList" CssClass="itemlist" ToolTip="<%$Resources:ItemDetails %>" TabIndex="14"
                                                    OnClick="ActionHandler" CommandName="POITEMDETAILS" alt="" />
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedOrders" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfPOHPK" Value='<%# Eval("POH_PK") %>' />
                                                <asp:HiddenField runat="server" ID="hdfUserStatus" Value='<%# Eval("USER_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPOHStatus" Value='<%# Eval("POH_STATUS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPOHDept" Value='<%# Eval("POH_DEPT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfRefId" Value='<%# Eval("REF_ID") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPOHItemFullText" Value='<%# Eval("POH_ITEM_FULLTEXT") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPohIsCommentExist" Value='<%# Eval("POH_IS_CMNT_EXISTS") %>' />
                                                <asp:HiddenField runat="server" ID="hdfPohNo" Value='<%# Eval("POH_NO") %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PODate %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPoDate" runat="server" Text='<%# Eval("POH_DATE") %>' ToolTip='<%# Eval("POH_DATE", Resources.Constants.DateFormatGrid) %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PONumber %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPONo" runat="server" Text='<%# Eval("POH_NO") %>' ToolTip='<%# Eval("POH_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="8%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCmpny" CssClass="<%# Eval(Resources.DataFieldRes.CompnayLineColor) %>"
                                                    runat="server" Text='<%# Eval("CMP_DISPLAY_CODE_TEXT") %>' ToolTip='<%# Eval("CMP_DISPLAY_CODE_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="2%" />
                                            <HeaderStyle />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:VendorCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblVendorCode" runat="server" Text='<%# Eval("VEN_CODE") %>' ToolTip='<%# Eval("VEN_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="14%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ItemDetails %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItem" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("POH_ITEM_TEXT"),80) %>'
                                                    ToolTip='<%# Eval("POH_ITEM_FULLTEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="26%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ReqFor %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblReqFor" runat="server" Text='<%# Eval("DPT_NAME") %>' ToolTip='<%# Eval("DPT_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:IONumber %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIONumber" runat="server" Text='<%# Eval("SOH_NO") %>' ToolTip='<%# Eval("SOH_NO") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="11%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Status %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("POH_STATUS_TEXT") %>' ToolTip='<%# Eval("POH_STATUS_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="7%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$Resources:Controls,Edit %>"
                                                    SkinID="imbeditgrid" TabIndex="14" CommandName="PERFORMACTION" />
                                                <asp:ImageButton runat="server" TabIndex="14" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                                    SkinID="imbdeletegrid" CommandName="DELETEPO" OnClientClick="return ShowDeleteConfirm(this);" />
                                                <asp:ImageButton runat="server" TabIndex="14" ID="imbView" ToolTip="<%$Resources:Controls,View %>"
                                                    SkinID="btnview" CommandName="VIEW" />
                                                <asp:ImageButton runat="server" TabIndex="14" ID="imbPrint" ToolTip="<%$Resources:Controls,Print %>"
                                                    SkinID="btnPrint" CommandName="PRINT" />
                                                <asp:ImageButton runat="server" TabIndex="14" ID="imbShortClose" CommandName="SHORTCLOSE"
                                                    OnClientClick="return ShowDeleteConfirm(this,'Are you sure want to close this PO.?');"
                                                    SkinID="btnclose" alt="<%$Resources:Controls,ShortClose%>" title="<%$Resources:Controls,ShortClose%>" />
                                                <asp:ImageButton runat="server" TabIndex="14" ID="imbNoComments" ToolTip="<%$Resources:Controls,Comments %>"
                                                    SkinID="popup" CommandName="COMMENTS" />
                                                <asp:ImageButton runat="server" TabIndex="14" ID="imbComnts" ToolTip="<%$Resources:Controls,Comments %>"
                                                    SkinID="popup-green" CommandName="COMMENTS" />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                            </div>
                            <h4>
                                Item Details</h4>
                            <%--Secnd Division--%>
                            <div class="gridwrap hierarchical-wrap">
                                <cc1:ExtGridView runat="server" ID="grdPOItems" AutoGenerateColumns="False" OnRowDataBound="ActionHandler"
                                    ExpandButtonCssClass="GridExpandCollapseButton" CollapseButtonCssClass="GridExpandCollapseButton"
                                    GridLines="None" ExpandButtonText="+" CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable"
                                    ShowFooter="true" PageSize="<%$ resources:PageSize %>">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Width="2%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Item %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItem" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ITM_NAME").ToString()),35) %>'
                                                    ToolTip='<%# Eval("ITM_NAME") %>'></asp:Label>
                                                <asp:HiddenField runat="server" ID="hdfIsExpandedPOItem" Value="0" />
                                                <asp:HiddenField runat="server" ID="hdfPRItem" Value='<%# Eval("ITM_PK") %>' />
                                                <asp:Button runat="server" ID="btnGetGrn" OnClick="ActionHandler" CommandName="GRNDETAILS"
                                                    CommandArgument='<%# Eval(Resources.DataFieldRes.PODetailPK) %>' EnableTheming="false"
                                                    Style="display: none" />
                                            </ItemTemplate>
                                            <ItemStyle Width="45%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:ItemCat %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemCat" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(HttpUtility.HtmlDecode(Eval("ITM_CATEGORY_TEXT").ToString()),35) %>'
                                                    ToolTip='<%# Eval("ITM_CATEGORY_TEXT") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPOQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_ORDERED")) +" "+Eval("PRD_UOM_TEXT") %>'
                                                    ToolTip='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_ORDERED")) +" "+Eval("PRD_UOM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="rate-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:PORcvdQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPORcvdQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_RECEIVED")) +" "+Eval("PRD_UOM_TEXT") %>'
                                                    ToolTip='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_RECEIVED")) +" "+Eval("PRD_UOM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="rate-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POAccQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPOAccQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_ACCEPTED")) +" "+Eval("PRD_UOM_TEXT") %>'
                                                    ToolTip='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_ACCEPTED")) +" "+Eval("PRD_UOM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="rate-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:POInvQty %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPOInvQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_INVOICED")) +" "+Eval("PRD_UOM_TEXT") %>'
                                                    ToolTip='<%#GetFormattedNumberWithSeperation(Eval("POD_QTY_INVOICED")) +" "+Eval("PRD_UOM_TEXT") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                            <HeaderStyle CssClass="rate-numeric" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <div class="hierarchical-gridwrap">
                                                    <cc1:ExtGridView runat="server" ID="grdGRNList" AutoGenerateColumns="False" ExpandButtonCssClass="GridExpandCollapseButton"
                                                        CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                                        CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" CellPadding="3"
                                                        ForeColor="#333333" AllowPaging="false">
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblGRNList" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="<%$ resources:GRNNo %>">
                                                                <ItemTemplate>
                                                                    <%--<asp:Label ID="lblGRnNo" runat="server" Text='<%# Eval("GRH_NO") %>' ToolTip='<%# Eval("GRH_NO") %>'>
                                                                                        </asp:Label>--%>
                                                                    <asp:LinkButton ID="lnkGrnNo" CssClass="text-underline" runat="server" Text='<%# Eval("GRH_NO") %>'
                                                                        OnClick="ActionHandler" CommandName="PRINTGRN" CommandArgument='<%# Eval("GRH_PK") %>'
                                                                        ToolTip='<%# Eval("GRH_NO") %>'></asp:LinkButton>
                                                                    <asp:HiddenField runat="server" ID="hdfIsExpandedGRNList" Value="0" />
                                                                    <asp:HiddenField runat="server" ID="hdfGRNPk" Value='<%# Eval("GRH_PK") %>' />
                                                                    <asp:Button runat="server" ID="btnGetGIN" OnClick="ActionHandler" CommandName="GINDETAILS"
                                                                        EnableTheming="false" Style="display: none" CommandArgument='<%# Eval("GRD_PK") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="22%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGRNDate" runat="server" Text='<%# Eval("GRH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                                        ToolTip='<%# Eval("GRH_DATE", Resources.Constants.DateFormatGrid) %>'>
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:GrnStore %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGRNStore" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("GRH_DEPT_TEXT"),23) %>'
                                                                        ToolTip='<%# Eval("GRH_DEPT_TEXT") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="30%" />
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:GRNRcvdQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGRNRecievedQty" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("GRD_QTY_RECEIVED")) +" "+Eval("GRD_UOM_TEXT") %>'
                                                                        ToolTip='<%# GetFormattedNumberWithSeperation(Eval("GRD_QTY_RECEIVED")) +" "+Eval("GRD_UOM_TEXT") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
                                                                <HeaderStyle CssClass="rate-numeric" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ resources:GRNAccQty %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblGRNQtyAccepted" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("GRD_QTY_ACCEPTED")) +" "+Eval("GRD_UOM_TEXT") %>'
                                                                        ToolTip='<%# GetFormattedNumberWithSeperation(Eval("GRD_QTY_ACCEPTED")) +" "+Eval("GRD_UOM_TEXT") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20%" HorizontalAlign="Right" />
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
                                                                                        <%--<asp:Label ID="lblGinNo" runat="server" Text='<%# Eval("GIH_NO") %>'
                                                                                                                ToolTip='<%# Eval("GIH_NO") %>'>
                                                                                                            </asp:Label>--%>
                                                                                        <asp:LinkButton ID="lnkGinNo" CssClass="text-underline" runat="server" Text='<%# Eval("GIH_NO") %>'
                                                                                            OnClick="ActionHandler" CommandName="PRINTGIN" CommandArgument='<%# Eval("GIH_PK") %>'
                                                                                            ToolTip='<%# Eval("GIH_NO") %>'></asp:LinkButton>
                                                                                        <asp:HiddenField runat="server" ID="hdfIsExpandedGinList" Value="0" />
                                                                                        <asp:HiddenField runat="server" ID="hdfGinPk" Value='<%# Eval("GIH_PK") %>' />
                                                                                        <asp:Button runat="server" ID="btnGetStockTransfer" OnClick="ActionHandler" CommandName="STDETAILS"
                                                                                            EnableTheming="false" Style="display: none" CommandArgument='<%# Eval("GID_PK") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="22%" />
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGinDate" runat="server" Text='<%# Eval("GIH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                                                            ToolTip='<%# Eval("GIH_DATE", Resources.Constants.DateFormatGrid) %>'>
                                                                                        </asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="8%" />
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:GinStore %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGinStore" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("GIH_DEPT_TEXT"),23) %>'
                                                                                            ToolTip='<%# Eval("GIH_DEPT_TEXT") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="25%" />
                                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:GinQtyInspected %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGinQtyInspected" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("GID_QTY_INSPECTED")) +" "+Eval("GID_UOM_TEXT") %>'
                                                                                            ToolTip='<%# GetFormattedNumberWithSeperation(Eval("GID_QTY_INSPECTED")) +" "+Eval("GID_UOM_TEXT") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                    <HeaderStyle CssClass="rate-numeric" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:GinQtyRejected %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGinQtyRejected" runat="server" Text='<%#GetFormattedNumberWithSeperation(Eval("GID_QTY_REJECTED")) +" "+Eval("GID_UOM_TEXT") %>'
                                                                                            ToolTip='<%# GetFormattedNumberWithSeperation(Eval("GID_QTY_REJECTED")) +" "+Eval("GID_UOM_TEXT") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                                                    <HeaderStyle CssClass="rate-numeric" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ resources:GinQtyAccepted %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGinQtyAccepted" runat="server" Text='<%# GetFormattedNumberWithSeperation(Eval("GID_QTY_ACCEPTED"))  +" "+Eval("GID_UOM_TEXT") %>'
                                                                                            ToolTip='<%# GetFormattedNumberWithSeperation(Eval("GID_QTY_ACCEPTED")) +" "+Eval("GID_UOM_TEXT") %>'></asp:Label>
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
                                                                                                CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" AllowPaging="false">
                                                                                                <EmptyDataTemplate>
                                                                                                    <asp:Label ID="lblSTEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                                                                </EmptyDataTemplate>
                                                                                                <Columns>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:STNumber %>">
                                                                                                        <ItemTemplate>
                                                                                                            <%--<asp:Label ID="lblStNumber" runat="server" Text='<%# Eval("SFH_NO") %>'
                                                                                                                                    ToolTip='<%# Eval("SFH_NO") %>'>
                                                                                                                                </asp:Label>--%>
                                                                                                            <asp:LinkButton ID="lnkStNumber" CssClass="text-underline" runat="server" Text='<%# Eval("SFH_NO") %>'
                                                                                                                OnClick="ActionHandler" CommandName="PRINTSA" CommandArgument='<%# Eval("SFH_PK") %>'
                                                                                                                ToolTip='<%# Eval("SFH_NO") %>'></asp:LinkButton>
                                                                                                            <asp:HiddenField runat="server" ID="hdfIsExpandedItem" Value="0" />
                                                                                                            <asp:HiddenField runat="server" ID="hdfHasChildren" Value="0" />
                                                                                                            <asp:HiddenField ID="hdfStockTransferPK" runat="server" Value='<%# Eval("SFH_PK") %>' />
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="22%" HorizontalAlign="Left" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:Date %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblStDate" runat="server" Text='<%# Eval("SFH_DATE", Resources.Constants.DateFormatGrid) %>'
                                                                                                                ToolTip='<%# Eval("SFH_DATE", Resources.Constants.DateFormatGrid) %>'>
                                                                                                            </asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle Width="8%" />
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="<%$ resources:QuantityUom %>">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblStQuantityUom" runat="server" Text='<%# GetFormattedNumberWithSeperation(Eval("SFD_QTY_APPROVED"))  +" "+Eval("SFD_UOM_TEXT") %>'
                                                                                                                ToolTip='<%# GetFormattedNumberWithSeperation(Eval("SFD_QTY_APPROVED"))  +" "+Eval("SFD_UOM_TEXT") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <ItemStyle HorizontalAlign="Right" Width="70%" />
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
                                                        <%--Second--%>
                                                        <RowStyle CssClass="table-thirdlevel" />
                                                        <HeaderStyle CssClass="table-thirdlevela" />
                                                    </cc1:ExtGridView>
                                                </div>
                                            </ItemTemplate>
                                            <ItemStyle CssClass="nopadding" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <RowStyle CssClass="table-firstlevel" />
                                    <HeaderStyle CssClass="table-firstlevela" />
                                </cc1:ExtGridView>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <%--  Shortclose region Start--------------------%>
                <div id="divShortClose" title="<%=Resources.Controls.ShortClose%>" style="display: none">
                    <div class="divcolmiddle-S">
                        <label for="lblPONumber">
                            <%=Resources.Controls.PoNumber%></label>
                        <asp:Label ID="lblPOH_NO" class="lbl-22perc" runat="server"></asp:Label>
                        <label for="Remarks">
                            <%=Resources.Controls.Remark%>*</label>
                        <asp:TextBox runat="server" ID="Remarks" TabIndex="18" MaxLength="200" TextMode="MultiLine"
                            Height="40px" >
                        </asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqRemarks" CssClass="star" SetFocusOnError="true"
                            EnableClientScript="true" ValidationGroup="ShortCloseSave" runat="server" ControlToValidate="Remarks"
                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:EnterRemarks %>">
                        </asp:RequiredFieldValidator>
                        <label for="RefNo">
                            <%=Resources.Controls.RefNo%></label>
                        <asp:TextBox runat="server" ID="RefNo" TabIndex="18" MaxLength="14" >
                        </asp:TextBox>
                        <div class="clear">
                        </div>
                        <asp:Label ID="lbnSpace" runat="server" AssociatedControlID="btnAddConv"></asp:Label>
                        <asp:HiddenField ID="POID" runat="server" Value="0"></asp:HiddenField>
                        <asp:Button runat="server" ID="btnAddConv" Text="<%$Resources:Controls,ShortClose%>"
                            CommandName="SHORTCLOSESAVE" OnClick="ActionHandler" ValidationGroup="ShortCloseSave"
                            class="inputbtn" Width="100px" Height="20px" OnClientClick="ValidatePageNow('ShortCloseSave');" />
                        <div class="clear">
                        </div>
                    </div>
                </div>
                <%-- End Shortclose region -----------------%>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <asp:ValidationSummary ID="vsShortCloseSave" ValidationGroup="ShortCloseSave" runat="server" />
                </div>
                <asp:HiddenField ID="hdnRoleID" runat="server" Value="0"></asp:HiddenField>
                <asp:HiddenField ID="hdnShortCloseGroup" runat="server" Value="0"></asp:HiddenField>
                <asp:HiddenField ID="hdfAppType" runat="server" />
                <asp:HiddenField ID="hdfAppSubType" runat="server" />
                <asp:HiddenField ID="hdfCmntTrxNo" runat="server" Value=""></asp:HiddenField>
                <asp:HiddenField ID="hdfCmntTrxPk" runat="server" Value=""></asp:HiddenField>
                <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
                <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
                <asp:HiddenField ID="HiddenField1" runat="server" Value="0" />
                <asp:HiddenField ID="hdfDecimalFormatWithSeperation" runat="server" />
                <asp:HiddenField ID="HiddenField2" runat="server" Value="0" />
                <asp:HiddenField ID="hdfSelectedItemPOPK" runat="server" Value="0" />
                <asp:HiddenField ID="hdfClient" runat="server" Value="" />
                 <asp:HiddenField ID="hdfShowIONo" runat="server" Value="0" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel runat="server" ID="aupdpnlComments">
        <ContentTemplate>
            <div id="divTrxComments" style="display: none;">
                <uc1:TransactionComments ID="ucTrxComments" runat="server" AppName="<%$Resources:PurchaseOrder%>" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
