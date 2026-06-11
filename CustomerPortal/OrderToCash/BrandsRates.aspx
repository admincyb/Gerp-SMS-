<%@ Page Title="<%$ Resources:Captions,Title_BrandRates %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="BrandsRates.aspx.cs" Theme="ClassicExt" Inherits="CustomerPortal.OrderToCash.BrandsRates" %>

<%@ Register Src="~/UserControls/CheckListSearchControl.ascx" TagName="CheckListSearchControl"
    TagPrefix="uc1" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var selectedPks = "";
        var pageURL = window.document.URL;
        var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
        var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        $(document).ready(function () {
            $("#imbHideFilter").show();
            $("#lbnProductCode").hide();
            //ShowHideProducts();
            //            HideFilter();
        });

        function ValidateCategory() {
            var flag = true;
            var information = "Information";
            var message = "<ul><li>" + "Category and Currency already exist" + "</li></ul>";
            var ddl_curr = document.getElementById("<%=ddlCurrency.ClientID%>");
            var cur_value = ddl_curr.options[ddl_curr.selectedIndex].value;
            var ddl_cate = document.getElementById("<%=ddlCustSpecialCategory.ClientID%>");
            var cat_value = ddl_cate.options[ddl_cate.selectedIndex].value;
            var gvDrv = document.getElementById("<%=grdRates.ClientID %>");
            var Inputs = gvDrv.getElementsByTagName('input');
            var j = 0;
            var oRows = gvDrv.rows;
            for (i = 1; i < oRows.length; i++) {
                var categorypk = gvDrv.rows[i].cells[0].children[0].value;
                var currencypk = gvDrv.rows[i].cells[1].children[0].value;
                if (cur_value == currencypk && cat_value == categorypk) {
                    //                    alert("1");           
                    flag = false;
                }
            }

            if (flag == false) {
                ShowErrorMessage(message, information);
                return false;
            }
        }

        function hide1() {
            $("#lbnProductCode").hide();
        }

        function show1() {
            $("#lbnProductCode").show();
        }

        function SHOWGRIDHEADERPOPUP_HIDE() {
            // alert('1');
            $("#pop").hide();
            // var theControl = document.getElementById("lbnProductCode");
            //document.getElementById("<%= lbnProductCode.ClientID %>").style.display = "none";
            //theControl.style.visibility = "hidden";

            //            theControl.style.display = 'none';
            //$('#lbnProductCode').show();
            //            $('#lbnProductDesc').hide();
            //            $('#lblProductCode').hide();
            //            $('#lblProductDesc').hide();
            //            $('#btnRateApply').hide();
            //            $('#btnRateApplyAll').show(); 
        }

        function InitComponents() {
            // GrandScriptUtils.MakeAutoCompleteDDL("txtPackingSpec", uiUrl, "hdfPackingSpec", true, true, "PACKINGSPEC");
            HideFilter();
            GrandScriptUtils.AddDateRangeCommon("txtDate", "hdfDate", "txtToDate", "hdfToDate", false, false);
            //ShowHideProducts();
        }


        function HideFilter() {
            //<summary>Function Used to Hide Vendor Panel </summary>
            //$("#imbHideFilter").hide();
            //$("#imbShowFilter").show();
            //$("#divFilterDetails").hide();
        }

        function ShowFilter() {
            //<summary>Function Used to Show Purchase Request Panel </summary>
            //$("#imbHideFilter").show();
            //$("#imbShowFilter").hide();
            //$("#divFilterDetails").show();
        }

        function ShowHideDivFilter(type) {
            if (type == "0") {
                $("#imbHideFilter").hide();
                $("#imbShowFilter").show();
                $("#divFilterDetails").hide();
            }
            else {
                $("#imbHideFilter").show();
                $("#imbShowFilter").hide();
                $("#divFilterDetails").show();
            }
        }


        function ShowCusBrand() {
            //<summary>Function Used to Show Purchase Request Panel </summary>
            $("#imbHideCusBrand").show();
            $("#imbShowCusBrand").hide();
            $("#divCustomerBrandDetails").show();
        }
        function HideCusBrand() {
            //<summary>Function Used to Hide Vendor Panel </summary>
            $("#imbHideCusBrand").hide();
            $("#imbShowCusBrand").show();
            $("#divCustomerBrandDetails").hide();
        }

        function ValidateProductSelect() {
            var flag = false;
            var information = "Information";
            var message = "<ul><li>" + "Select Product" + "</li></ul>";
            $("input[id*='trvProductsn']").each(function () {
                if (this.checked) {
                    flag = true;
                }
            });

            if (flag) return true;

            ShowErrorMessage(message, information);
            return false;
        }



        function ShowHideProducts() {
            if ($("[id$=chkShowHideProducts]").is(':checked')) {
                $(".treeview").show();
            }
            else {
                $(".treeview").hide();
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
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                return false;  //Page is invalid -- stop right here
            }
            else {
                //everythings ok --- Call your function & do your stuff
                return true;
            }
        }

        function OnCheckBoxCheckChanged(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;

                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node 
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children           
                    {
                        //check or uncheck children at all levels           
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels           
                CheckUncheckParents(src, src.checked);
            }
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }
        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;



            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }

        function winClose() {
            ClosePopup();
        }

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlCustomerRegistration">
        <ContentTemplate>
            <div class="fixed-buttons-normal" id="divFixedTab">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SAVE" TabIndex="16" Text="<%$resources:ErpRes,Save %>"
                                            ValidationGroup="brandRate" OnClientClick="javascript:ValidatePageNow('brandRate')"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Save %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-submit" />
                                    </li>
                                    <li>
                                        <li runat="server" id="pnlSaveSubmit">
                                            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                            <asp:Button runat="server" ID="btnSaveSubmit" Visible="false" TabIndex="17" Text="<%$resources:ErpRes,SaveSubmit %>"
                                                OnClientClick="javascript:ValidatePageNow('enquiry')" ValidationGroup="enquiry"
                                                ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                                SkinID="btnInner-submit" />
                                        </li>
                                        <li>
                                            <asp:Button runat="server" ID="btnSave" Visible="false" CommandName="SAVE" TabIndex="18"
                                                Text="<%$resources:ErpRes,Save %>" OnClientClick="javascript:ValidatePageNow('brandRate')"
                                                ToolTip="<%$resources:ErpRes,Save %>" SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel" />
                                        </li>
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="19" Visible="false"
                                            Text="<%$resources:ErpRes,Delete %>" ToolTip="<%$resources:ErpRes,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" />
                                    </li>
                                    <%--    <li runat="server" id="pnlView">
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="20" Text="<%$resources:ErpRes,View %>"
                                            ToolTip="<%$resources:ErpRes,View %>" ValidationGroup="brandRate" OnClientClick="javascript:ValidatePageNow('brandRate')"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-View" />
                                    </li>--%>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <%--use the width property of the below table corresponding to the contents in the page--%>
            <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks tablelayout">
                <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                <asp:TableRow ID="PageAction_Entry" runat="server">
                    <%--Align table cell according to design--%>
                    <asp:TableCell>
                        <div class="content-wrapper">
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblDate" runat="server" Text="<%$resources:EffectFrom %>" AssociatedControlID="txtDate"></asp:Label>
                                            <asp:HiddenField ID="hdfDate" runat="server" />
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="input-small" ValidationGroup="brandRate"
                                                MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfFromDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="brandRate" EnableClientScript="true" runat="server" ControlToValidate="txtDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_EffectFromDate %>">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <div id="divFilter">
                                <h1 class="search-colapse-normal">
                                    <%= GetLocalResourceObject("FilterProduct").ToString()%>
                                    <img id="imbShowFilter" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                        alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowHideDivFilter(1);" />
                                    <img id="imbHideFilter" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="Hide"
                                        title="Hide" style="cursor: pointer" onclick="javascript:ShowHideDivFilter(0);" />
                                </h1>
                                <div id="divFilterDetails">
                                    <div id="divSearchProducts">
                                        <div class="grid-group">
                                            <div class="clear">
                                            </div>
                                            <div class="grid-group-table padglft0">
                                                <table class="table-devide">
                                                    <tr>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label runat="server" ID="Label2" Text="<%$ resources:ErpRes,Type%>" AssociatedControlID="ddlTypeProductListPopUp"
                                                                    Style="margin: 3px;"></asp:Label>
                                                                <asp:DropDownList ID="ddlTypeProductListPopUp" TabIndex="1" CssClass="medium" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="Label16" Text="<%$ resources:Controls,ADNL_SPEC05 %>"
                                                                    AssociatedControlID="ddlColorCategoryProductListPopUp" Style="margin: 3px;"></asp:Label>
                                                                <asp:DropDownList ID="ddlColorCategoryProductListPopUp" TabIndex="2" CssClass="medium" runat="server">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="div2col-S">
                                                                <asp:Label runat="server" ID="Label17" Text="<%$ resources:Controls,ADNL_SPEC06 %>"
                                                                    AssociatedControlID="ddlFormerTypeProductListPopUp" Style="margin: 3px;"></asp:Label>
                                                                <asp:DropDownList ID="ddlFormerTypeProductListPopUp" TabIndex="5" CssClass="medium" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="Label18" Text="<%$ resources:Controls,ADNL_SPEC07 %>"
                                                                    AssociatedControlID="ddlFormerSizeProductListPopUp" Style="margin: 3px;"></asp:Label>
                                                                <asp:DropDownList ID="ddlFormerSizeProductListPopUp" TabIndex="6" CssClass="medium" runat="server">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div class="div2col-S">
                                                                <asp:Label runat="server" ID="Label9" Text="<%$ resources:ErpRes,Size %>" AssociatedControlID="ddlSizeProductListPopUp"
                                                                    Style="margin: 3px;"></asp:Label>
                                                                <asp:DropDownList ID="ddlSizeProductListPopUp" TabIndex="3" CssClass="medium" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="Label1" Text="Print Type" AssociatedControlID="ddlPrintProductListPopUp"
                                                                    Style="margin: 3px;"></asp:Label>
                                                                <%-- <%$ resources:Controls,ADNL_SPEC05 %>--%>
                                                                <asp:DropDownList ID="ddlPrintProductListPopUp" TabIndex="4" CssClass="medium" runat="server">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="div2col-S">
                                                                <asp:Label runat="server" ID="Label3" Text="<%$ resources:Controls,Color %>" AssociatedControlID="ddlColorProductListPopUp"
                                                                    Style="margin: 3px;"></asp:Label>
                                                                <asp:DropDownList ID="ddlColorProductListPopUp" CssClass="medium" TabIndex="7" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:Label runat="server" ID="Label4" Text="<%$ resources:Controls,SubCategory %>"
                                                                    AssociatedControlID="ddlSubCategoryList" Style="margin: 3px;"></asp:Label>
                                                                <asp:DropDownList ID="ddlSubCategoryList" TabIndex="8" CssClass="medium" runat="server">
                                                                </asp:DropDownList>
                                                                <%-- <asp:Label runat="server" ID="Label4" Text="<%$ resources:ShowProducts %>" AssociatedControlID="chkShowHideProducts"></asp:Label>
                                                                <asp:CheckBox ID="chkShowHideProducts" runat="server" TabIndex="8" Text="" Checked="false"/>--%>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="clear">
                                                </div>
                                                <div class="w100perc" style="display: inline-flex;">
                                                    <div class="disp-inline" style="width:45.5%;">
                                                        <div class="tree-label2M w100perc" style="display: inline-block;">
                                                            <label class="float-left margn-rgt1" style="min-width:27.3%; max-width:27.3%;">
                                                                <%= GetLocalResourceObject("PackingSpec").ToString()%></label>
                                                                 
                                                            <uc1:CheckListSearchControl ID="chklstPackingSpec" runat="server" />
                                                          
                                                        </div>
                                                        <%-- <div class="tree-label2M w48perc" style="display: inline-block;">
                                                            <label class="lbl-30perc float-left">
                                                                <%= GetLocalResourceObject("SubCategory").ToString()%></label>
                                                            <uc1:CheckListSearchControl ID="chklstSubCategory" runat="server" />
                                                        </div>--%>
                                                    </div>
                                                    <asp:Label ID="lblDummy" Text="" style="min-width:16.8%; max-width:16.8%;"></asp:Label>
                                                    <div class="treeview disp-inline" style="width:38%;">

                                                        <asp:TreeView ID="trvProducts" runat="server" ShowLines="true" onclick="OnCheckBoxCheckChanged(event);"
                                                            ExpandDepth="0" InitialExpandDepth="2" ShowCheckBoxes="Parent,Leaf" Style="display: inline-block;
                                                            height: 153px; overflow-y: auto !important; width: 100%;">
                                                        </asp:TreeView>
                                                    </div>
                                                </div>
                                                <table class="gridwrap" id="tblEmptyRecord" visible="false" runat="server">
                                                    <tr class="emptytable">
                                                        <td>
                                                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <div class="divcol-S txt-rgt">
                                                    <asp:Button runat="server" ID="btnProductListPopUpOk" CommandName="PRODUCTLISTPOPUPOK"
                                                        TabIndex="23" Text="<%$ resources:FilterProducts %>" ToolTip="<%$ resources:FilterProducts %>"
                                                        SkinID="btn-filter" Style="margin-right: 5px;" OnClick="ActionHandler" />
                                                    <asp:Button runat="server" ID="btnApplayProduct" CommandName="PRODUCTAPPLY" TabIndex="23"
                                                        Text="Apply" ToolTip="<%$resources:ErpRes,Select %>" SkinID="btnInner-ok" OnClick="ActionHandler" />
                                                    <asp:Button runat="server" ID="btnClear" CommandName="CLEAR" TabIndex="4" Text="<%$resources:ErpRes,Clear %>"
                                                        ToolTip="<%$resources:ErpRes,Clear %>" OnClick="ActionHandler" CommandArgument="SEC_ActionPanel"
                                                        SkinID="btnInner-Cancel" />
                                                </div>
                                            </div>
                                        </div>
                                        <%--Start Individual rate set section--%>
                                        <div id="divRateSett" style="display: none;">
                                            <div class="Button-container-popup">
                                                <asp:Button runat="server" ID="btnRateApply" CommandName="RATEAPPLY" TabIndex="23"
                                                    Text="<%$resources:ErpRes,Apply %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Apply %>"
                                                    SkinID="btnInner-ok" />
                                                <asp:Button runat="server" ID="btnRateApplyAll" CommandName="RATEADDALL" TabIndex="23"
                                                    Text="RateApplyAll" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Apply %>"
                                                    SkinID="btnInner-ok" Visible="false" />
                                                <asp:Button runat="server" ID="btnCancel" CommandName="CANCEL" TabIndex="24" Text="<%$resources:ErpRes,Cancel %>"
                                                    ToolTip="<%$resources:ErpRes,Cancel %>" SkinID="btnInner-Cancel" OnClientClick="javascript:winClose()" />
                                            </div>
                                            <div class="content-wrapper">
                                                <div class="divcol-P1">
                                                    <div id="pop">
                                                        <asp:Label runat="server" ID="lbnProductCode" Text="<%$ resources:BrandProductCode %>"
                                                            AssociatedControlID="lblProductCode"></asp:Label>
                                                        <asp:Label ID="lblProductCode" runat="server"></asp:Label>
                                                        <asp:Label runat="server" ID="lbnProductDesc" Text="<%$resources:BrandProductDescription %>"
                                                            AssociatedControlID="lblProductDesc"></asp:Label>
                                                        <asp:Label ID="lblProductDesc" runat="server"></asp:Label>
                                                    </div>
                                                    <asp:Label runat="server" ID="lblCustSpecialCategory" Text="<%$ resources:CustSpecialCategory%>"
                                                        AssociatedControlID="ddlCustSpecialCategory"></asp:Label>
                                                    <asp:DropDownList ID="ddlCustSpecialCategory" CssClass="select-half valid" runat="server"
                                                        ValidationGroup="RateApply">
                                                    </asp:DropDownList>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfddlCustSpecialCategory" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="RateApply" EnableClientScript="true" runat="server" ControlToValidate="ddlCustSpecialCategory"
                                                            Display="Dynamic" InitialValue="-1" Text="*" ErrorMessage="<%$ resources:Err_Rate %>"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <asp:Label runat="server" ID="lblRate" Text="<%$ resources:Rate %>" AssociatedControlID="ddlCurrency"></asp:Label>
                                                    <asp:DropDownList ID="ddlCurrency" CssClass="input-xsmall-b" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtNewRateApply" ValidationGroup="RateApply" runat="server" CssClass="input-w58 numeric"></asp:TextBox>
                                                    <asp:ImageButton ID="btnRateAdd" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                                        ValidationGroup="RateApply" CommandName="RATEADD" TabIndex="57" OnClientClick="return ValidateCategory();" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfNewRate" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="RateApply" EnableClientScript="true" runat="server" ControlToValidate="txtNewRateApply"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreNewRate" runat="server" ControlToValidate="txtNewRateApply"
                                                            ErrorMessage="<%$ resources:Err_Rate %>" ValidationExpression="^\$?([0-9]{0,14})?(\.[0-9]{0,5})?$"
                                                            Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="RateApply">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                    <%-- <asp:Button runat="server" ID="btnRateAdd" ValidationGroup="RateApply" CommandName="RATEADD"
                                                     OnClientClick="javascript:ValidatePageNow('RateApply')" TabIndex="23" Text="Add" OnClick="ActionHandler"
                                                     ToolTip="Add" SkinID="btnInner-ok" />--%>
                                                    <div class="max-100">
                                                        <asp:GridView runat="server" ID="grdRates" Width="85%" AllowSorting="True" AutoGenerateColumns="false"
                                                            EmptyDataRowStyle-CssClass="emptytable">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ resources:CustSpecialCategory %>">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdfCategoryPK" runat="server" Value='<%# Eval("CategoryPk") %>' />
                                                                        <asp:Label ID="lblCategory" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CategoryName"),15) %>'
                                                                            ToolTip='<%# Eval("CategoryName")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="30%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdfCurrencyPK" runat="server" Value='<%# Eval("CurrencyPk") %>' />
                                                                        <asp:Label ID="lblCurrencyName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CurrencyName"),15) %>'
                                                                            ToolTip='<%# Eval("CurrencyName")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="1%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdfRate" runat="server" Value='<%# Eval("BandRate") %>' />
                                                                        <asp:Label ID="lblRate" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("BandRate"),15) %>'
                                                                            ToolTip='<%# Eval("BandRate")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="txtAlign-right" />
                                                                    <ItemStyle Width="9%" CssClass="txtAlign-right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imbRateRemove" runat="server" CommandName="REMOVERATE" TabIndex="58"
                                                                            SkinID="btnclose" CommandArgument='<%# Eval("RowNumber") %>' ToolTip="<%$ resources:Delete %>"
                                                                            OnClick="ActionHandler" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="8%" />
                                                                </asp:TemplateField>
                                                                <%-- <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imbRateEdit" runat="server" CommandName="EDITRATE" TabIndex="58"
                                                                        SkinID="btnclose" CommandArgument='<%# Eval("RowNumber") %>' ToolTip="Remove"
                                                                        OnClick="ActionHandler" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" />
                                                            </asp:TemplateField>--%>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--End Individual rate set section--%>
                                        <%--Start rate History section--%>
                                        <div id="divRateHistory" style="display: none;">
                                            <div class="Button-container-popup">
                                                <asp:Button runat="server" ID="btnHistoryCancel" CommandName="CANCEL" TabIndex="24"
                                                    Text="<%$resources:ErpRes,Cancel %>" ToolTip="<%$resources:ErpRes,Cancel %>"
                                                    SkinID="btnInner-Cancel" OnClientClick="javascript:winClose()" />
                                            </div>
                                            <div class="content-wrapper">
                                                <div class="divcol-P1">
                                                    <asp:Label runat="server" ID="lbnHisProductCode" Text="<%$ resources:BrandProductCode %>"
                                                        AssociatedControlID="lblHisProductCode"></asp:Label>
                                                    <asp:Label ID="lblHisProductCode" runat="server"></asp:Label>
                                                    <asp:Label runat="server" ID="lbnHisProductDesc" Text="<%$resources:BrandProductDescription %>"
                                                        AssociatedControlID="lblHisProductDesc"></asp:Label>
                                                    <asp:Label ID="lblHisProductDesc" runat="server"></asp:Label>
                                                    <div class="max-100">
                                                        <asp:GridView runat="server" ID="grdProductRateHistory" Width="100%" AllowSorting="True"
                                                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ resources:FromDate %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblFromDate" runat="server" Text='<%# Eval("BPH_FROM_DATE","{0:dd/MM/yyyy}") %>'
                                                                            ToolTip='<%# Eval("BPH_FROM_DATE","{0:dd/MM/yyyy}")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="8%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:ToDate %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblToDate" runat="server" Text='<%# Eval("BPH_TO_DATE","{0:dd/MM/yyyy}") %>'
                                                                            ToolTip='<%# Eval("BPH_TO_DATE","{0:dd/MM/yyyy}")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="8%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:CustSpecialCategory %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCategory" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CON_NAME"),15) %>'
                                                                            ToolTip='<%# Eval("CON_NAME")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="23%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrencyCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CUR_CODE"),15) %>'
                                                                            ToolTip='<%# Eval("CUR_CODE")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="7%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRate" runat="server" DataFormatString="{0:0.000}" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("BPH_RATE"),15) %>'
                                                                            ToolTip='<%# Eval("BPH_RATE")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="txtAlign-right" />
                                                                    <ItemStyle Width="20%" CssClass="txtAlign-right" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--End rate History section--%>
                                        <%--Start all product to rate section--%>
                                        <div id="divRateSettAllProduct" style="display: none;">
                                            <div class="Button-container-popup">
                                                <asp:Button runat="server" ID="Button1" CommandName="RATEAPPLYALL" TabIndex="23"
                                                    Text="<%$resources:ErpRes,Apply %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Apply %>"
                                                    SkinID="btnInner-ok" />
                                                <asp:Button runat="server" ID="Button2" CommandName="CANCEL" TabIndex="24" Text="<%$resources:ErpRes,Cancel %>"
                                                    ToolTip="<%$resources:ErpRes,Cancel %>" SkinID="btnInner-Cancel" OnClientClick="javascript:winClose()" />
                                            </div>
                                            <div class="content-wrapper">
                                                <div class="divcol-P1">
                                                    <asp:Label runat="server" ID="Label10" Text="<%$ resources:CustSpecialCategory%>"
                                                        AssociatedControlID="ddlCustSpecialCategory01"></asp:Label>
                                                    <asp:DropDownList ID="ddlCustSpecialCategory01" CssClass="select-half valid" runat="server"
                                                        ValidationGroup="RateApply">
                                                    </asp:DropDownList>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="RateApply" EnableClientScript="true" runat="server" ControlToValidate="ddlCustSpecialCategory"
                                                            Display="Dynamic" InitialValue="-1" Text="*" ErrorMessage="<%$ resources:Err_Rate %>"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <asp:Label runat="server" ID="Label11" Text="<%$ resources:Rate %>" AssociatedControlID="ddlCurrency01"></asp:Label>
                                                    <asp:DropDownList ID="ddlCurrency01" CssClass="input-xsmall-b" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtNewRateApply01" ValidationGroup="RateApply" runat="server" CssClass="input-w58 numeric"></asp:TextBox>
                                                    <asp:ImageButton ID="ImageButton1" SkinID="imbaddnew" runat="server" OnClick="ActionHandler"
                                                        ValidationGroup="RateApply" CommandName="RATEADDALL" TabIndex="57" OnClientClick="return ValidateCategory();" />
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="RateApply" EnableClientScript="true" runat="server" ControlToValidate="txtNewRateApply"
                                                            Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNewRateApply"
                                                            ErrorMessage="<%$ resources:Err_Rate %>" ValidationExpression="^\$?([0-9]{0,14})?(\.[0-9]{0,5})?$"
                                                            Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="RateApply">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                    <%-- <asp:Button runat="server" ID="btnRateAdd" ValidationGroup="RateApply" CommandName="RATEADD"
                                                     OnClientClick="javascript:ValidatePageNow('RateApply')" TabIndex="23" Text="Add" OnClick="ActionHandler"
                                                     ToolTip="Add" SkinID="btnInner-ok" />--%>
                                                    <div class="max-100">
                                                        <asp:GridView runat="server" ID="grdProductRateALL" Width="85%" AllowSorting="True"
                                                            AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ resources:CustSpecialCategory %>">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdfCategoryPK" runat="server" Value='<%# Eval("CategoryPk") %>' />
                                                                        <asp:Label ID="lblCategory" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CategoryName"),15) %>'
                                                                            ToolTip='<%# Eval("CategoryName")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="30%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdfCurrencyPK" runat="server" Value='<%# Eval("CurrencyPk") %>' />
                                                                        <asp:Label ID="lblCurrencyName" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CurrencyName"),15) %>'
                                                                            ToolTip='<%# Eval("CurrencyName")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="1%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdfRate" runat="server" Value='<%# Eval("BandRate") %>' />
                                                                        <asp:Label ID="lblRate" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("BandRate"),15) %>'
                                                                            ToolTip='<%# Eval("BandRate")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle CssClass="txtAlign-right" />
                                                                    <ItemStyle Width="9%" CssClass="txtAlign-right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="imbRateRemove" runat="server" CommandName="REMOVERATE" TabIndex="58"
                                                                            SkinID="btnclose" CommandArgument='<%# Eval("RowNumber") %>' ToolTip="<%$ resources:Delete %>"
                                                                            OnClick="ActionHandler" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="8%" />
                                                                </asp:TemplateField>
                                                                <%-- <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imbRateEdit" runat="server" CommandName="EDITRATE" TabIndex="58"
                                                                        SkinID="btnclose" CommandArgument='<%# Eval("RowNumber") %>' ToolTip="Remove"
                                                                        OnClick="ActionHandler" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="8%" />
                                                            </asp:TemplateField>--%>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--End all product to rate section--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <%-- End New Packing Spec Filter --%>
                        <%--<div class="button-wrap-right">--%>
                        <%--        <asp:Button runat="server" ID="btnApply" CommandName="APPLY" TabIndex="5" CssClass="BTNenable-submit"
                                Text="<%$resources:ErpRes,Apply %>" ToolTip="<%$resources:ErpRes,Apply %>" OnClick="ActionHandler" />--%>
                        <%--</div>--%>
                        </div> </div>
                        <div class="clear">
                        </div>
                        <div class="content-wrapper">
                            <div id="CustomerBrands">
                                <h1 class="search-colapse-normal">
                                    <%--  <%= GetLocalResourceObject("SelectedBrands").ToString()%>--%>
                                    <%= GetLocalResourceObject("SelectedProducts").ToString()%>
                                    <img id="imbShowCusBrand" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                        alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowCusBrand();" />
                                    <img id="imbHideCusBrand" src="../Images/Classic/Icons/arrow-colapse-active.png"
                                        alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:HideCusBrand();" />
                                </h1>
                                <div id="divCustomerBrandDetails">
                                    <asp:HiddenField ID="hdfExpandPosition" runat="server" />
                                    <asp:HiddenField ID="hdfselectedPks" runat="server" Value="" />
                                    <div class="gridwrap max-500" id="divBrand_ScrollContainer" grid="grdSelectedCusBrands">
                                        <table class="table-devide" id="tblFilter" runat="server" visible="false">
                                            <tr>
                                                <td>
                                    </div>
                                    </td> </tr>
                                    <tr>
                                        <td colspan="2" style="text-align: right">
                                            <%--<asp:Button runat="server" ID="btnSHOWGRIDHEADERPOPUP"  CssClass="IMAGEaction-popup"
                                                        OnClick="ActionHandler" TabIndex="3" CommandName="SHOWGRIDHEADERPOPUP" EnableTheming="false"
                                                        ToolTip="<%$ resources:Rate %>" style="margin-right:27px; margin-bottom:0px;" />--%>
                                            <%-- dive grid popup start  --%>
                                            <div id="divGridHeaderPopUp" style="display: none;">
                                                <div class="Button-container-popup">
                                                    <asp:Button runat="server" ID="btnGRIDHEADERPOPUPAPPLY" CommandName="GRIDHEADERPOPUPAPPLY"
                                                        TabIndex="23" Text="<%$resources:ErpRes,Select %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Select %>"
                                                        SkinID="btnInner-ok" ValidationGroup="RateApplyGRIDHEADERPOPUP" />
                                                    <asp:Button runat="server" ID="btnGRIDHEADERPOPUPCANCEL" OnClick="ActionHandler"
                                                        CommandName="GRIDHEADERPOPUPCANCEL" TabIndex="24" Text="<%$resources:ErpRes,Cancel %>"
                                                        ToolTip="<%$resources:ErpRes,Cancel %>" SkinID="btnInner-Cancel" />
                                                </div>
                                                <div style="margin: 13px 0px 4px 13px; height: 25px;">
                                                    <asp:Label runat="server" ID="Label13" Text="<%$ resources:Rate %>" AssociatedControlID="ddlGridHeaderPopUp"></asp:Label>
                                                    <asp:DropDownList ID="ddlGridHeaderPopUp" CssClass="medium" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtNewRateGridHeaderPopUp" runat="server" TabIndex="9" CssClass="small-a numeric"
                                                        MaxLength="7" ValidationGroup="RateApplyGRIDHEADERPOPUP"></asp:TextBox>
                                                    <div class="starwrap">
                                                        <asp:RequiredFieldValidator ID="vrfNewRateGRIDHEADERPOPUP" CssClass="star" SetFocusOnError="true"
                                                            ValidationGroup="RateApplyGRIDHEADERPOPUP" EnableClientScript="true" runat="server"
                                                            ControlToValidate="txtNewRateGridHeaderPopUp" Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="vreNewRateGRIDHEADERPOPUP" runat="server" ControlToValidate="txtNewRateGridHeaderPopUp"
                                                            ErrorMessage="<%$ resources:Err_Rate %>" ValidationExpression="^-?([0-9]{0,14})?(\.[0-9]{0,5})?$"
                                                            Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="RateApplyGRIDHEADERPOPUP">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </div>
                                            </div>
                                            <%-- dive grid popup end  --%>
                                        </td>
                                    </tr>
                                    </table>
                                    <asp:HiddenField ID="hdfBrand_ExpandPosition" runat="server" />
                                    <asp:GridView runat="server" ID="grdSelectedCusBrands" AutoGenerateColumns="False"
                                        Width="100%" GridLines="None" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true"
                                        PageSize="<%$ resources:PageSize %>">
                                        <EmptyDataTemplate>
                                            <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyProduct %>"></asp:Label>
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ resources:ProductCode %>">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdfIsExpand" Value="0" runat="server" />
                                                    <%--   <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%# Eval("ITM_PK") %>' />
                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ITM_CODE") %>' ToolTip='<%# Eval("ITM_CODE")%>'>
                                                        </asp:Label>--%>
                                                    <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%# Eval("BPR_ITEM") %>' />
                                                    <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("BPR_ITEM_CODE") %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval("BPR_ITEM_TEXT").ToString())%>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="20%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:ProductDescription %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("BPR_ITEM_NAME") %>'
                                                        ToolTip='<%# HttpUtility.HtmlDecode(Eval("BPR_ITEM_NAME").ToString())%>'>
                                                    </asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="70%" />
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:EffectFrom %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtdate" runat="server" Text='<%# Convert.ToString(Eval("BPR_DATE")).Equals("1/1/0001 12:00:00 AM")?"":Eval("BPR_DATE","{0:dd/MM/yyyy}")%>'
                                                        TabIndex="9" CssClass="small-a input-disabled" ReadOnly="true" MaxLength="15" ToolTip='<%# Eval("BPR_DATE") %>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle Width="8%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <%-- <%$ resources:NewRate %>--%>
                                            <asp:TemplateField HeaderText="<%$resources:InterState %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtInterstateRate" runat="server" Text='<%# Eval("Inter_State") %>'
                                                        TabIndex="9" CssClass="small-a numeric input-disabled" ReadOnly="true" MaxLength="15" DataFormatString="{0:0.000}"
                                                        ToolTip='<%# Eval("Inter_State") %>'></asp:TextBox>
                                                    <%--   <div class="starwrap-relative">
                                                   <cc1:RateValidation ID="vreNewRate" runat="server" ControlToValidate="txtInterstateRate"
                                                        ErrorMessage="<%$ resources:Err_Rate %>" NumberDigits="10" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="brandRate"></cc1:RateValidation>
                                                </div>--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <%--ToolTip='<%# Eval("BRD_RATE") %>'--%>
                                            <asp:TemplateField HeaderText="<%$resources:IntraState %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtIntrastateRate" runat="server" Text='<%#Eval("Intra_State") %>'
                                                        TabIndex="9" ValidationGroup="brandRate" ReadOnly="true" CssClass="small-a numeric input-disabled"
                                                        MaxLength="15" DataFormatString="{0:0.000}" ToolTip='<%# Eval("Intra_State") %>'></asp:TextBox>
                                                    <%--   <div class="starwrap-relative">
                                                    <cc1:RateValidation ID="vreNewRate1" runat="server" ControlToValidate="txtIntrastateRate"
                                                        ErrorMessage="<%$ resources:Err_Rate %>" NumberDigits="10" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="brandRate"></cc1:RateValidation>
                                                </div>--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$resources:Overseas %>">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtOverSeasRate" runat="server" Text='<%# Eval("Overseas") %>' TabIndex="9"
                                                        ValidationGroup="brandRate" ReadOnly="true" CssClass="small-a numeric input-disabled" DataFormatString="{0:0.000}"
                                                        MaxLength="15" ToolTip='<%# Eval("Overseas") %>'></asp:TextBox>
                                                    <%--<div class="starwrap-relative">
                                                <cc1:RateValidation ID="vreNewRate2" runat="server" ControlToValidate="txtOverSeasRate"
                                                        ErrorMessage="<%$ resources:Err_Rate %>" NumberDigits="10" Display="Dynamic"
                                                        Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="brandRate"></cc1:RateValidation>
                                                </div>--%>
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Right" />
                                                <HeaderStyle CssClass="amount-numeric" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                                <HeaderTemplate>
                                                    <asp:Button runat="server" ID="btnSHOWGRIDHEADERPOPUP" CssClass="IMAGEaction-popup"
                                                        OnClick="ActionHandler" TabIndex="13" CommandName="SHOWGRIDHEADERPOPUP" EnableTheming="false"
                                                        ToolTip="<%$ resources:Rate %>" Style="margin-right: 0px; margin-bottom: 0px;" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnRate" runat="server" OnClick="ActionHandler" CommandName="SETRATE"
                                                        CommandArgument='<%# Eval("BPR_ITEM") %>' SkinID="imbactiongrid" ToolTip="<%$ resources:Rate %>"
                                                        TabIndex="13" ValidationGroup="brandRate" OnClientClick="javascript:ValidatePageNow('brandRate')" />
                                                </ItemTemplate>
                                                <HeaderStyle CssClass="amount-numeric" />
                                                <ItemStyle HorizontalAlign="Right" />
                                                <%--  ValidationGroup="brandRate" OnClientClick="javascript:ValidatePageNow('brandRate')"--%>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="<%$ resources:History %>">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnHistory" runat="server" CommandName="RATEHISTORY" CommandArgument='<%# Eval("BPR_ITEM") %>'
                                                        SkinID="history" ToolTip="<%$ resources:History %>" OnClick="ActionHandler" TabIndex="13" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <%--First--%>
                                        <RowStyle CssClass="table-firstlevel" />
                                        <HeaderStyle CssClass="table-firstlevela" />
                                        <FooterStyle CssClass="table-firstlevela-total" />
                                    </asp:GridView>
                                    <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                                </div>
                                <div class="button-wrap-right">
                                </div>
                            </div>
                        </div>
                        <%--               </div> </div>--%>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="true">
                    <asp:TableCell>
                        <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdfCurrentUserSbu" Value="0" runat="server" />
            <asp:HiddenField ID="hdfBrandRateSbu" Value="0" runat="server" />
            <asp:HiddenField ID="hdfProductPk" Value="0" runat="server" />
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <asp:HiddenField ID="hdfbrandRatePK" runat="server" />
            </div>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="setRate" runat="server" />
                <asp:ValidationSummary ID="vsDtl" ValidationGroup="brandRate" runat="server" />
                <asp:ValidationSummary ID="vsRate" ValidationGroup="RateApply" runat="server" />
                <asp:ValidationSummary ID="vsRateCopy" ValidationGroup="brandRateCopy" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
