<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PendingOrders.ascx.cs" 
    Inherits="CustomerPortal.UserControls.PendingOrders" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<script language="javascript" type="text/javascript">
    //To select/ Deselect All cartons from list
    function UcrInitComponents() {
        GrandScriptUtils.DatePickerCommon("txtDispatchDate");
        CheckColor();
    }
    function CheckAllItems(evt) {
        var isChecked;
        if (!evt) {
            $("[id*=ChkOrder]").each(function () {
                var cblId = $(this).attr("id");
                var allNode;
                allNode = $("[id$=" + cblId + "]").find('input[value="-1"]');
                isChecked = allNode.attr("checked") == "checked";
                if (isChecked) {
                    $("[id$=" + cblId + "]").find("tr:has(td)").each(function () {
                        $(this).find("td:first input[type=checkbox]").attr("checked", true);
                    });
                }
            });
        }
        else {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var listID;
                isChecked = src.checked;
                var listID = $(src).parents("table:first").attr("id");
                if ($(src).attr("id") == $("[id$=ChkAll]").attr("id")) {
                    var selectedRowColor;
                    if (isChecked) {
                        $("[id$=" + listID + "]").find("tr:has(td)").each(function () {
                            $(this).find("td:first input[type=checkbox]").attr("checked", true);
                            selectedRowColor = '<%= Resources.Controls.selectedRowColor %>';
                            $(this).closest('tr').css('background-color', selectedRowColor);
                        });
                    }
                    else {
                        $("[id$=" + listID + "]").find("tr:has(td)").each(function () {
                            $(this).find("td:first input[type=checkbox]").removeAttr("checked");
                            selectedRowColor = 'White';
                            $(this).closest('tr').css('background-color', selectedRowColor);
                        });
                    }
                }
                else {
                    var isAll = true;
                    $("[id$=" + listID + "]").find("tr:has(td)").each(function () {
                        if (($(this).find("td:first input[type=checkbox]").attr("id") != $("[id$=ChkAll]").attr("id")) && ($(this).find("td:first input[type=checkbox]").attr("checked") != "checked")) {
                            isAll = false;
                        }
                        if (isAll) {
                            $("[id$=" + listID + "]").find('input[id$=ChkAll]').attr("checked", true);
                        }
                        else {
                            $("[id$=" + listID + "]").find('input[id$=ChkAll]').removeAttr("checked");
                        }
                    });
                }
            }
        }
    }



    //for validation atleast one checkbox selected in grid
    function IsRowSelected(source, arguments) {
        var retval = 0;
        var grid = $('[id$=grdPendingOrders]');
        if (grid != null) {
            $("[id$=grdPendingOrders] tr:has(td)").each(function (index) {
                var isChecked = $(this).find("[id$=ChkOrder_" + index + "]").attr("checked");
                if (isChecked) {
                    retval = 1;
                }
            });
            if (retval == 0) {
                arguments.IsValid = false;
            }
            else {
                arguments.IsValid = true;
            }
        }
        else {
            arguments.IsValid = false;
        }
    }

    function SelectAllPageItems() {
        var grid = $('[id$=grdPendingOrders]');
        if (grid != null) {
            $("[id$=grdPendingOrders] tr:has(td)").each(function (index) {
                if ($("[id$=hdfIsAllPagsSelected]").val() == "1") {
                    $("[id$=ChkAll]").attr("checked", false);
                    $(this).find("td:first input[type=checkbox]").removeAttr("checked");
                    selectedRowColor = 'White';
                    $(this).closest('tr').css('background-color', selectedRowColor);
                }
                else {
                    $("[id$=ChkAll]").attr("checked", true);
                    $(this).find("td:first input[type=checkbox]").attr("checked", true);
                    selectedRowColor = '<%= Resources.Controls.selectedRowColor %>';
                    $(this).closest('tr').css('background-color', selectedRowColor);
                }
            });
        }
    }

    //for validating UI data
    function ValidatePageNowUcr(validationGroup) {
        if (typeof (Page_ClientValidate) == 'function') {
            CheckValidationDuplicate(validationGroup);
            Page_ClientValidate(validationGroup);
        }
        if (!Page_IsValid) {
            $("[id$=litErrorMsgUcr]").hide();
            ShowErrorMessage($("#divUcrerror").html());
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }

    function SelectSBU() {
        var CHK = document.getElementById("<%=chkSBU.ClientID%>");
        var checkbox = CHK.getElementsByTagName("input");
        var counter = 0;
        var val = false;
        if ($("[id$=chkHdrSbu]").attr("checked"))
            val = true;
        for (var i = 0; i < checkbox.length; i++) {
            checkbox[i].checked = val;
        }
    }

    <%--function SelectPlanGroup() {

        var CHK = document.getElementById("<%=chkProductGroup.ClientID%>");
        var checkbox = CHK.getElementsByTagName("input");
        var counter = 0;
        var val = false;
        if ($("[id$=chkHdrPlanGroup]").attr("checked"))
            val = true;
        for (var i = 0; i < checkbox.length; i++) {
            checkbox[i].checked = val;
        }
    }--%>

    function SelectOrders() {

        var CHK = document.getElementById("<%=chkOrders.ClientID%>");
        var checkbox = CHK.getElementsByTagName("input");
        var counter = 0;
        var val = false;
        if ($("[id$=chkHdrOrders]").attr("checked"))
            val = true;
        for (var i = 0; i < checkbox.length; i++) {
            checkbox[i].checked = val;
        }
    }

    function SelectCustomers() {
        var CHK = document.getElementById("<%=chkCustomer.ClientID%>");
        var checkbox = CHK.getElementsByTagName("input");
        var counter = 0;
        var val = false;
        if ($("[id$=chkHdrCustomer]").attr("checked"))
            val = true;
        for (var i = 0; i < checkbox.length; i++) {
            checkbox[i].checked = val;
        }
    }

    function SelectAll(first) {
        var stat = false;
        var firstR = first.find("[id*=chkHdrSelect]").attr("id").split("_");
        var firstV = firstR[firstR.length - 1];
        if (first.find("[id*=chkHdrSelect_" + firstV + "]").attr("checked"))
            stat = true;
        $("[id$=dtlProperties] tr:has(td)").each(function (index) {
            if (index == firstV) {
                $("[id*=chk_" + index + "]").attr("checked", stat);
            }
        });
    }
    function SetCurrentRowColor(SelRow) {
        var selectedRowColor;
        if (SelRow.checked)
            selectedRowColor = '<%= Resources.Controls.selectedRowColor %>';
        else
            selectedRowColor = 'White';
        $(SelRow).closest('tr').css('background-color', selectedRowColor);
    }
    function CheckColor() {
        $("[id$=grdPendingOrders]").find("tr:has(td)").each(function (index) {
            var selectedRowColor;
            if ($(this).find("[id*=ChkOrder]").attr("checked")) {
                selectedRowColor = '<%= Resources.Controls.selectedRowColor %>';
            }
            else {
                selectedRowColor = 'White';
            }
            $(this).closest('tr').css('background-color', selectedRowColor);
        });
    }
    function IsChkBoxRowSelected() {
        var Retval = 0;
        $("[id$=grdPendingOrders]").find("tr:has(td)").each(function (index) {
            if ($(this).find("[id*=ChkOrder]").attr("checked")) {
                Retval = 1;
            }
        });
        if (Retval == 1) {
            $("[id$=btnSelectDeAllocation]").click();
        }
        else {
            ShowErrorMessage('<ul><li>Need to select atleast one row</li></ul>');
        }
        return false;
    }


    function SelectAllChild(gridName) {
        var isChecked = $("[id$=" + gridName + "] tr:has(th)").find("[id$=chkAllActive]").attr("checked");

        $("[id$=" + gridName + "] tr:has(td)").each(function (index) {
            if (isChecked) {
                $(this).find("[id$=chkSelect_" + index + "]").attr("checked", true);
            }
            else {
                $(this).find("[id$=chkSelect_" + index + "]").attr("checked", false);
            }
        });
    }

    function SelectHeaderChk(gridName) {
        var chkedCount = 0;
        var totalRows = $('[id*=' + gridName + '] tr').length - 1;
        $("[id$=" + gridName + "] tr:has(td)").each(function (index) {
            var isChecked = $(this).find("[id$=chkSelect_" + index + "]").attr("checked");
            if (isChecked) {
                chkedCount++;
            }
        });

        // if (gridName == '')
        //    totalRows = totalRows - 1; //this grid contains footer row

        if (chkedCount == totalRows)
            $("[id$=" + gridName + "] tr:has(th)").find("[id$=chkAllActive]").attr("checked", true);
        else
            $("[id$=" + gridName + "] tr:has(th)").find("[id$=chkAllActive]").attr("checked", false);
    }
    function AfterClose(containerID) {
        if (containerID == '[id$=divBinDetials]') {
            $("[id$=btnView]").click();
        }
    }

    function SelectItems() {
        var stat = false;
        if ($("[id$=chkSelectAll]").attr("checked"))
            stat = true;

        $("[id$=chkHdrSbu]").attr("checked", stat);
        $("[id$=chkHdrPlanGroup]").attr("checked", stat);
        $("[id$=chkHdrOrders]").attr("checked", stat);
        $("[id$=chkHdrCustomer]").attr("checked", stat);
        $("[id$=chkWithStock]").attr("checked", stat);
        $("[id$=chkFullyAllocated]").attr("checked", stat);
        $("[id$=chkFullyPlanned]").attr("checked", stat);
        SelectSBU();
        SelectPlanGroup();
        SelectOrders();
        SelectCustomers();

        $("[id$=dtlProperties] tr:has(td)").each(function (index) {
            $("[id*=chkHdrSelect_" + index + "]").attr("checked", stat);
            $("[id*=chk_" + index + "]").attr("checked", stat);
        });
    }

    //    function HideLoading() {
    //        alert(0);
    //        $('#updateProgress').hide();
    //    }
</script>
<div class="content-wrapper">
    <div class="Button-container-popup">
        <asp:Button ID="btnShowFilter" SkinID="btnInner-filter" runat="server" Text="<%$ Resources:AdvanceFilter %>"
            CommandName="POPUPADD" OnClick="ActionHandler" TabIndex="151" ToolTip="<%$ Resources:AdvanceFilter %>" />
        <%--     <%$ Resources:PeningOrders %>--%>
        <div style="display: none;">
            <asp:Button ID="btnSelectOrder" SkinID="btnInner-add-dsd" runat="server" Text="<%$ Resources:SelectToPlan %>"
                CommandName="APPLY" OnClick="ActionHandler" TabIndex="152" OnClientClick="javascript:ValidatePageNow('SelectPlan')"
                ToolTip="<%$ Resources:SelectToPlan %>" />
        </div>
        <asp:Button ID="btnSelectAllocation" SkinID="btnInner-allocation" runat="server"
            Text="<%$ Resources:SelectToAllocation %>" CommandName="APPLY" OnClick="ActionHandler"
            TabIndex="152" OnClientClick="javascript:ValidatePageNowUcr('Allocation')" ToolTip="<%$ Resources:SelectToAllocation %>" />
        <div style="display: none;">
            <asp:Button ID="btnSelectDeAllocation" runat="server" CommandName="DEALLOCATION"
                OnClick="ActionHandler" OnClientClick="return ShowDeleteConfirmYesNO(this,'Do you want to deallocate the selected Sale Orders?');" />
        </div>
        <asp:Button ID="btnClickDeAllocation" SkinID="btnInner-deallocate" runat="server"
            Text="<%$ Resources:Deallocate %>" TabIndex="152" OnClientClick="return IsChkBoxRowSelected();"
            ToolTip="<%$ Resources:Deallocate %>" />
        <asp:Button ID="btnRelease" SkinID="btnInner-releaseallocate" runat="server" Text="<%$ Resources:ReleaseAllocation %>"
            CommandName="RELEASE" OnClick="ActionHandler" TabIndex="152" ToolTip="<%$ Resources:ReleaseAllocation %>" />
        <asp:Button runat="server" TabIndex="101" ID="btnPendingOrderPrint" CommandName="PRINTPENDINGORDERS"
            Text="Pending Orders" SkinID="btnInner-Print" ToolTip="Pening Orders" OnClick="ActionHandler" />
    </div>
    <div>
        <asp:CheckBox runat="server" ID="ChkAllPages" TabIndex="1" ToolTip="<%$ Resources:SelectAllPageItems %>"
            OnCheckedChanged="ActionHandler" AutoPostBack="true" onclick="SelectAllPageItems();"
            CssClass="margnbotm0 margntop3" style="margin-left:8px!important;" />
        <asp:Label runat="server" ID="lblChkAllPages" Text="<%$ Resources:SelectAllPageItems %>"></asp:Label>
        <asp:HiddenField runat="server" ID="hdfIsAllPagsSelected" Value="0" />
    </div>
    <div class="gridwrap" style="overflow-x: hidden; overflow-y: auto; max-height: 400px;">
        <asp:GridView ID="grdPendingOrders" runat="server" AutoGenerateColumns="False" Width="100%"
            AllowPaging="false" AllowSorting="True" CssClass="grdTable" HeaderStyle-HorizontalAlign="Center"
            EmptyDataRowStyle-HorizontalAlign="Center" EmptyDataRowStyle-CssClass="emptytable"
            TabIndex="1">
            <EmptyDataTemplate>
                <asp:Label ID="lblEmpty" runat="server" Text="<%$ Resources:Messages,Msg_EmptyGrid %>"></asp:Label>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox runat="server" ID="ChkAll" TabIndex="1" ToolTip="<%$ Resources:SelectAll %>"
                            CssClass="checkbox" onclick="CheckAllItems(event);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="ChkOrder" TabIndex="1" onclick="CheckAllItems(event);SetCurrentRowColor(this)" />
                    </ItemTemplate>
                    <HeaderStyle  HorizontalAlign="Center" CssClass="txtAlign-center" />
                    <ItemStyle Width="2%" HorizontalAlign="Center" CssClass="mar" />
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="grdText" HeaderText="<%$ resources:Order %>">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval(BusinessObject.Constants.OrderPlanning.F_SOH_NO) %>'
                            ToolTip='<%# Eval(BusinessObject.Constants.OrderPlanning.F_SOH_NO) %>'></asp:Label>
                        <asp:HiddenField ID="hdfSOPk" runat="server" Value='<%#Eval(BusinessObject.Constants.OrderPlanning.F_SOH_PK)%>' />
                        <asp:HiddenField ID="hdfOrderDtlPk" runat="server" Value='<%#Eval(BusinessObject.Constants.OrderPlanning.F_SOD_PK)%>' />
                        <asp:HiddenField ID="hdfItemPlanGroup" runat="server" Value='<%#Eval(BusinessObject.Constants.OrderPlanning.F_ITM_PLAN_GROUP)%>' />
                        <asp:HiddenField ID="hdfItemPlanGroupText" runat="server" Value='<%#Eval(BusinessObject.Constants.OrderPlanning.F_ITM_PLAN_GROUP_TEXT)%>' />
                        <asp:HiddenField ID="hdfAvailQty" runat="server" Value='<%# Eval(BusinessObject.Constants.OrderPlanning.F_GRP_AVAILABLE_QTY) %>' />
                        <asp:HiddenField ID="hdfAllocatedQty" runat="server" Value='<%# Eval(BusinessObject.Constants.OrderPlanning.F_GRP_ALLOCATED_QTY) %>' />
                        <asp:HiddenField ID="hdfBaltoAllocate" runat="server" Value='<%# Eval(BusinessObject.Constants.OrderPlanning.F_SOD_BAL_TO_ALLOCATE) %>' />
                        <asp:HiddenField ID="hdfSizeSequence" runat="server" Value='<%# Eval(BusinessObject.Constants.OrderPlanning.F_ISD_SIZE_SEQUENCE) %>' />
                        <asp:HiddenField ID="hdfAGradePerc" runat="server" Value='<%# Eval(BusinessObject.Constants.OrderPlanning.F_ISD_AGRADE_PER) %>' />
                    </ItemTemplate>
                    <ItemStyle Width="11%" />
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="grdText" HeaderText="<%$ resources:lblCustomer %>">
                    <ItemTemplate>
                        <asp:Label ID="lblCustomer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval(BusinessObject.Constants.OrderPlanning.F_SOH_CUSTOMER_CODE)),20) %>'
                            ToolTip='<%# Eval(BusinessObject.Constants.OrderPlanning.F_SOH_CUSTOMER_NAME) %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="17%" />
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="grdText" HeaderText="<%$ resources:Brand %>">
                    <ItemTemplate>
                        <asp:Label ID="lblProduct" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Convert.ToString(Eval("SOD_BRAND_CODE")),20) %>'
                            ToolTip='<%# Eval("SOD_BRAND_NAME") %>'></asp:Label>
                        <asp:HiddenField ID="hdfProductPk" runat="server" Value='<%#Eval("SOD_ITEM")%>' />
                    </ItemTemplate>
                    <ItemStyle Width="22%" />
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="grdText" HeaderText="<%$ resources:Size %>">
                    <ItemTemplate>
                        <asp:Label ID="lblProductSizeText" runat="server" Text='<%# Eval(BusinessObject.Constants.OrderPlanning.F_ISD_SIZE_TEXT) %>'
                            ToolTip='<%# Eval(BusinessObject.Constants.OrderPlanning.F_ISD_SIZE_TEXT) %>'></asp:Label>
                        <asp:HiddenField ID="hdfProductSize" runat="server" Value='<%#Eval(BusinessObject.Constants.OrderPlanning.F_ISD_SIZE)%>' />
                    </ItemTemplate>
                    <ItemStyle Width="3%" />
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="grdText" HeaderText="<%$ resources:Quantity %>"
                    HeaderStyle-CssClass="grd-head-rgt">
                    <ItemTemplate>
                        <asp:Label ID="lblQuantity" runat="server" Text='<%# GetFormattedNumber(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_QTY)) %>'
                            ToolTip='<%# GetFormattedNumber(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_QTY)) %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="9%" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="grdText" HeaderText="<%$ resources:Requiredby %>" HeaderStyle-CssClass="grd-head-center">
                    <ItemTemplate>
                        <asp:Label ID="lblRequiredby" runat="server" Text='<%# Convert.ToDateTime(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_REQUIRED_BY)).ToString("dd-MMM-yyyy") %>'
                            ToolTip='<%# Convert.ToDateTime(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_REQUIRED_BY)).ToString("dd-MMM-yyyy") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="9%" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="grdText" HeaderText="<%$ resources:DispatchedQty %>"
                    HeaderStyle-CssClass="grd-head-rgt">
                    <ItemTemplate>
                        <asp:Label ID="lblDispatchedQty" runat="server" Text='<%# GetFormattedNumber(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_QTY_DISPATCHED)) %>'
                            ToolTip='<%# GetFormattedNumber(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_QTY_DISPATCHED)) %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="9%" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="grdText" Visible="false" HeaderText="<%$ resources:AllocationQty %>"
                    HeaderStyle-CssClass="grd-head-rgt">
                    <ItemTemplate>
                        <%--<asp:Label ID="lblAllocatedQty" runat="server" Text='<%# GetFormattedNumber(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_QTY_ALLOCATED)) %>'
                            ToolTip='<%# GetFormattedNumber(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_QTY_ALLOCATED)) %>'></asp:Label>--%>
                        <asp:LinkButton ID="lnkAllocatedQty" runat="server" Text='<%# GetFormattedNumber(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_QTY_ALLOCATED)) %>'
                            ToolTip='<%# GetFormattedNumber(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_QTY_ALLOCATED)) %>'
                            CssClass="text-underline" OnClick="ActionHandler" CommandName="LISTBIN"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Width="9%" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="grdText" HeaderText="<%$ resources:PlannedQty %>"
                    HeaderStyle-CssClass="grd-head-rgt">
                    <ItemTemplate>
                        <asp:Label ID="lblPlannedQty" runat="server" Text='<%# GetFormattedNumber(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_QTY_PLANNED)) %>'
                            ToolTip='<%# GetFormattedNumber(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_QTY_PLANNED)) %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="9%" HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-CssClass="grdText" HeaderText="<%$ resources:BalancetoPlan %>"
                    HeaderStyle-CssClass="grd-head-rgt">
                    <ItemTemplate>
                        <asp:Label ID="lblBalPlanQty" runat="server" Text='<%# GetFormattedNumber(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_BAL_TO_PLAN)) %>'
                            ToolTip='<%# GetFormattedNumber(Eval(BusinessObject.Constants.OrderPlanning.F_SOD_BAL_TO_PLAN)) %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="9%" HorizontalAlign="Right" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <uc1:PagerControl ID="uclPagingPO" runat="server" />
    </div>
    <asp:CustomValidator ID="vcvRowSelected" runat="server" ToolTip="<%$ Resources:Msg_NeedToSelectAtleastOneRow %>"
        ErrorMessage="<%$ Resources:Msg_NeedToSelectAtleastOneRow %>" CssClass="star"
        Display="None" ValidationGroup="Allocation" ClientValidationFunction="IsRowSelected"></asp:CustomValidator>
    <asp:CustomValidator ID="vcvRowSelectedOP" runat="server" ToolTip="<%$ Resources:Msg_NeedToSelectAtleastOneRow %>"
        ErrorMessage="<%$ Resources:Msg_NeedToSelectAtleastOneRow %>" CssClass="star"
        Display="None" ValidationGroup="SelectPlan" ClientValidationFunction="IsRowSelected"></asp:CustomValidator>
    <div id="tbladvancedSearch" style="display: none;">
        <div class="content-wrapper">
            <div class="Button-container-popup">
                <asp:Button ID="btnApplyFilter" SkinID="btnInner-add-dsd" runat="server" Text="<%$ Resources: Controls,Apply %>"
                    OnClick="ActionHandler" CommandName="APPLY" ToolTip="<%$ Resources: Controls,Apply %>" />
                <asp:Button ID="btnPopupClear" SkinID="btnInner-reset" runat="server" Text="<%$ Resources: Controls,Clear %>"
                    OnClick="ActionHandler" CommandName="CLEAR" ToolTip="<%$ Resources: Controls,Clear %>" />
                <asp:Button ID="btnPopupCancel" SkinID="btnInner-cancel-dsd" runat="server" Text="<%$ Resources: Controls,Cancel %>"
                    OnClick="ActionHandler" CommandName="CANCEL" ToolTip="<%$ Resources: Controls,Cancel %>" />
            </div>
            <asp:CheckBox runat="server" ID="chkSelectAll" onchange="SelectItems();" CssClass="margntop2" />
            <asp:Label runat="server" ID="lblSelectAll" AssociatedControlID="chkSelectAll" Text="<%$ resources:SelectAll %>"></asp:Label>
            <asp:DataList runat="server" ID="dtlProperties" OnItemDataBound="ActionHandler" RepeatDirection="Horizontal"
                RepeatLayout="Table" CssClass="filter-grid margnbotm10">
                <%-- tablelayout--%>
                <ItemTemplate>
                    <div class="headdiv">
                        <asp:Label runat="server" ID="lbl" AssociatedControlID="chk" Text='<%# Eval("CNG_NAME") %>'
                            CssClass="headtitle"></asp:Label>
                        <asp:CheckBox runat="server" ID="chkHdrSelect" onchange="SelectAll($(this));" CssClass="float-left" />
                    </div>
                    <div class="innerdiv maxh-200">
                        <%--<asp:DropDownList ID="ddl" runat="server" CssClass="select-44-8per">
                </asp:DropDownList>--%>
                        <asp:CheckBoxList ID="chk" runat="server" Style="text-align: left;">
                        </asp:CheckBoxList>
                        <asp:HiddenField runat="server" ID="hdfName" Value='<%# Eval("CNG_CODE") %>' />
                        <asp:HiddenField runat="server" ID="hdfValue" Value='<%# Eval("CNG_PK") %>' />
                    </div>
                </ItemTemplate>
            </asp:DataList>
            <table class="groupplan-grid margnbotm10">
                <tr>
                    <td class="w14-2perc">
                        <div class="headdiv">
                            <asp:Label runat="server" ID="lblSBUCaption" Text="<%$ resources:SBU %>" CssClass="headtitle"></asp:Label>
                            <asp:CheckBox runat="server" ID="chkHdrSbu" onchange="SelectSBU();" CssClass="float-left" />
                        </div>
                        <div class="innerdiv maxh-200">
                            <asp:CheckBoxList ID="chkSBU" runat="server" Style="text-align: left;">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                   <%-- <td class="w43perc">
                        <div class="headdiv">
                            <asp:Label runat="server" ID="lblProductPlanGroup" Text="<%$ resources:PlanGroup %>"
                                CssClass="headtitle"></asp:Label>
                            <asp:CheckBox runat="server" ID="chkHdrPlanGroup" onchange="SelectPlanGroup();" CssClass="float-left" />
                        </div>
                        <div class="innerdiv maxh-200">
                            <asp:CheckBoxList ID="chkProductGroup" runat="server" Style="text-align: left;">
                            </asp:CheckBoxList>
                        </div>
                    </td>--%>
                    <td class="border-rgt0 w17perc">
                        <div class="headdiv">
                            <asp:Label runat="server" ID="lblOrders" Text="<%$ resources:Orders %>" CssClass="headtitle"></asp:Label>
                            <asp:CheckBox runat="server" ID="chkHdrOrders" onchange="SelectOrders();" CssClass="float-left" />
                        </div>
                        <div class="innerdiv maxh-200">
                            <asp:CheckBoxList ID="chkOrders" runat="server" Style="text-align: left;">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td  class="w14-2perc">
                        <div class="headdiv">
                            <asp:Label runat="server" ID="lblCustomer" Text="<%$ resources:lblCustomer %>" CssClass="headtitle"></asp:Label>
                            <asp:CheckBox runat="server" ID="chkHdrCustomer" onchange="SelectCustomers();" CssClass="float-left" />
                        </div>
                        <div class="innerdiv maxh-200">
                            <asp:CheckBoxList ID="chkCustomer" runat="server" Style="text-align: left;">
                            </asp:CheckBoxList>
                        </div>
                    </td>
                    <td class="w10perc">
                        <asp:Label runat="server" ID="lblDispatchDate" Text="<%$ resources:DispatchDate %>"
                            CssClass="lbl-80perc"></asp:Label>
                        <asp:TextBox runat="server" ID="txtDispatchDate" onkeypress="return isDate(event)"
                            CssClass="input-w50per" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                        <div class="clear">
                        </div>
                        <asp:CheckBox runat="server" Visible="false" ID="chkWithStock" Checked="false" />
                        <asp:Label runat="server" ID="lblWithStock" Visible="false"  Text="<%$ resources:WithStock %>" CssClass="lbl-80perc"
                            ToolTip="<%$ resources:WithStock %>"></asp:Label>
                         <div class="clear">
                        </div>
                        <asp:CheckBox runat="server" ID="chkFullyAllocated" Checked="true" />
                        <asp:Label runat="server" Visible="false" ID="lblFullyAllocated" Text="<%$ resources:FullyAllocated %>"
                            CssClass="lbl-80perc" ToolTip="<%$ resources:ToolTipFullyAllocated %>"></asp:Label>
                       
                        <asp:CheckBox runat="server" Visible="false" ID="chkFullyPlanned" Checked="false" />
                        <asp:Label runat="server" ID="lblFullyPlanned" Text="<%$ resources:FullyPlanned %>"
                            CssClass="lbl-80perc" ToolTip="<%$ resources:ToolTipFullyPlanned %>"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divReleaseSC" style="display: none;">
        <div class="content-wrapper">
            <div class="Button-container-popup">
                <asp:Button ID="btnreleasePopUp" SkinID="btnInner-add-dsd" runat="server" Text="<%$ Resources:Release %>"
                    OnClick="ActionHandler" CommandName="RELEASESAVE" ToolTip="<%$ Resources:Release %>" />
                <asp:Button ID="btnCancelReleasePopUp" SkinID="btnInner-cancel-dsd" runat="server"
                    Text="<%$ Resources: Controls,Cancel %>" OnClick="ActionHandler" CommandName="CANCEL"
                    ToolTip="<%$ Resources: Controls,Cancel %>" />
            </div>
            <div style="overflow-x: hidden; overflow-y: auto; max-height: 400px;">
                <asp:GridView runat="server" ID="grdReleaseSc" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="emptytable"
                    AllowPaging="false" ShowFooter="false">
                    <EmptyDataTemplate>
                        <asp:Label ID="lblMenuEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox runat="server" ID="chkAllActive" TabIndex="1" ToolTip="<%$ Resources:SelectAll %>"
                                    CssClass="checkbox" onClick="javascript:SelectAllChild('grdReleaseSc');" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox runat="server" ID="chkSelect" onClick="javascript:SelectHeaderChk('grdReleaseSc');" />
                            </ItemTemplate>
                            <ItemStyle Width="3%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Order %>">
                            <ItemTemplate>
                                <asp:Label ID="lblSCNo" runat="server" Text='<%# Eval("SOH_NO") %>' ToolTip='<%# Eval("SOH_NO") %>'></asp:Label>
                                <asp:HiddenField ID="hdfDtlPK" runat="server" Value='<%# Eval("SOD_PK") %>' />
                            </ItemTemplate>
                            <ItemStyle Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:lblCustomer %>">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("SOH_CUSTOMER_NAME") %>'
                                    ToolTip='<%# Eval("SOH_CUSTOMER_NAME") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="25%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Item %>">
                            <ItemTemplate>
                                <asp:Label ID="lblItem" runat="server" Text='<%# Eval("SOD_ITEM_CODE") %>' ToolTip='<%# Eval("SOD_ITEM_NAME") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Qty %>" HeaderStyle-CssClass="grd-head-rgt">
                            <ItemTemplate>
                                <asp:Label ID="lblQty" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY"))) %>'
                                    ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY"))) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:AllocatedQty %>" HeaderStyle-CssClass="grd-head-rgt">
                            <ItemTemplate>
                                <asp:Label ID="lblAllocatedQty" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_ALLOCATED"))) %>'
                                    ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_ALLOCATED"))) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:ProducedQty %>" HeaderStyle-CssClass="grd-head-rgt">
                            <ItemTemplate>
                                <asp:Label ID="lblProducedQty" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_PRODUCED"))) %>'
                                    ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_PRODUCED"))) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:PackedQty %>" HeaderStyle-CssClass="grd-head-rgt">
                            <ItemTemplate>
                                <asp:Label ID="lblPackedQty" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_PACKED"))) %>'
                                    ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("SOD_QTY_PACKED"))) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="true">
                            <ItemTemplate>
                                <asp:ImageButton ID="imbinPopup" runat="server" SkinID="show-carton" CommandName="DETAILS"
                                    OnClick="ActionHandler" CssClass="Active" TabIndex="6" ToolTip="<%$ Resources:BinDetails %>" />
                            </ItemTemplate>
                            <ItemStyle Width="3%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <div id="divBinDetials" style="display: none;">
        <div class="content-wrapper">
            <div class="Button-container-popup">
                <asp:Button ID="btnCancelBinDtls" SkinID="btnInner-cancel-dsd" runat="server" Text="<%$ Resources: Controls,Cancel %>"
                    OnClick="ActionHandler" CommandName="CANCEL" ToolTip="<%$ Resources: Controls,Cancel %>" />
            </div>
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label runat="server" ID="lblSc" AssociatedControlID="lblScDisplay" Text="<%$ resources:Order %>"></asp:Label>
                            <b>:
                                <asp:Label ID="lblScDisplay" runat="server" Text="<%$ resources:Order %>" AssociatedControlID="lblSc"
                                    CssClass="txtAlign-left"></asp:Label></b>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                        </div>
                    </td>
                </tr>
            </table>
            <div style="overflow-x: hidden; overflow-y: auto; max-height: 400px;">
                <asp:GridView runat="server" ID="grdBins" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="emptytable"
                    AllowPaging="false" ShowFooter="false">
                    <EmptyDataTemplate>
                        <asp:Label ID="lblMenuEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:BinNo %>">
                            <ItemTemplate>
                                <asp:Label ID="lblBinNo" runat="server" Text='<%# Eval("BCH_NO") %>' ToolTip='<%# Eval("BCH_NO") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="30%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Date %>">
                            <ItemTemplate>
                                <asp:Label ID="lblBinDate" runat="server" Text='<%# Convert.ToDateTime(Eval("BCH_DATE")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'
                                    ToolTip='<%# Convert.ToDateTime(Eval("BCH_DATE")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Stock %>" HeaderStyle-CssClass="grd-head-rgt">
                            <ItemTemplate>
                                <asp:Label ID="lblStock" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("BCH_QTY_IN_PCS"))) %>'
                                    ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("BCH_QTY_IN_PCS"))) %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="20%" HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Item %>">
                            <ItemTemplate>
                                <asp:Label ID="lblItem" runat="server" Text='<%# Eval("ITM_CODE") %>' ToolTip='<%# Eval("ITM_NAME") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="30%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <div id="divBinHistory" style="display: none;">
        <div class="content-wrapper">
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label runat="server" ID="lblScPopup" AssociatedControlID="txtScPopup" Text="<%$ Resources:ScNo %>"></asp:Label>
                            <asp:TextBox runat="server" ID="txtScPopup" CssClass="input-disabled input-half"
                                Enabled="false"></asp:TextBox>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="divcol-S">
                            <asp:Label runat="server" ID="lblProductGroupPopup" AssociatedControlID="txtProductGroupPopup"
                                Text="<%$ Resources:ProductGroup %>" CssClass="margn-rgt0"></asp:Label>
                            <asp:TextBox runat="server" ID="txtProductGroupPopup" CssClass="input-disabled input-w68per"
                                Enabled="false"></asp:TextBox>
                            <asp:TextBox runat="server" ID="txtSizePopup" CssClass="input-disabled input-w10-5per"
                                Enabled="false"></asp:TextBox>
                        </div>
                    </td>
                </tr>
            </table>
            <div style="overflow-x: hidden; overflow-y: auto; max-height: 400px;">
                <asp:GridView runat="server" ID="grdBinDtls" AutoGenerateColumns="False" EmptyDataRowStyle-CssClass="emptytable"
                    AllowPaging="false" ShowFooter="true" OnRowDataBound="ActionHandler">
                    <EmptyDataTemplate>
                        <asp:Label ID="lblMenuEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:BinNO %>">
                            <ItemTemplate>
                                <asp:Label ID="lblBinNo" runat="server" Text='<%# Eval("BCH_NO") %>' ToolTip='<%# Eval("BCH_NO") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="40%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:BinDate %>">
                            <ItemTemplate>
                                <asp:Label ID="lblBinDate" runat="server" Text='<%# Convert.ToDateTime(Eval("BCH_DATE")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'
                                    ToolTip='<%# Convert.ToDateTime(Eval("BCH_DATE")).ToString(GetGlobalResourceObject("Constants", "dd_MMM_yy").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <div class="txt-rgt">
                                    <asp:Label ID="lblTotal" runat="server" Text="Total" CssClass="bold" />
                                </div>
                            </FooterTemplate>
                            <ItemStyle Width="40%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:AllocatedQty %>" HeaderStyle-CssClass="grd-head-rgt">
                            <ItemTemplate>
                                <asp:Label ID="lblAllocated" runat="server" Text='<%# GetFormattedNumber(Convert.ToString(Eval("BAM_QTY_ALLOCATED"))) %>'
                                    ToolTip='<%# GetFormattedNumber(Convert.ToString(Eval("BAM_QTY_ALLOCATED"))) %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <div class="txt-rgt">
                                    <asp:Label ID="lblTotalQty" runat="server" CssClass="bold" />
                                </div>
                            </FooterTemplate>
                            <ItemStyle Width="20%" HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <div style="display: none;">
        <asp:Button runat="server" ID="btnView" OnClick="ActionHandler" CommandName="POPUPSHOW" /></div>
    <div id="divUcrerror" style="display: none">
        <asp:Label runat="server" ID="litErrorMsgUcr" ClientIDMode="Static" CssClass="star"></asp:Label>
        <asp:ValidationSummary ID="vvsSelectPlanUcr" ValidationGroup="Allocation" runat="server" />
    </div>
</div>

