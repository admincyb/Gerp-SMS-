<%@ Page Title="<%$ Resources:Captions,Title_BrandRates %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="ClassicExt" CodeBehind="BrandRates.aspx.cs" Inherits="CustomerPortal.OrderToCash.BrandRates" %>

<%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<%@ Register src="~/UserControls/CheckListSearchControl.ascx" tagname="CheckListSearchControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="../Scripts/Jquery/ERPTimepicker.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        var selectedPks = "";
        function HideFilter() {
            //<summary>Function Used to Hide Vendor Panel </summary>
            $("#imbHideFilter").hide();
            $("#imbShowFilter").show();
            $("#divFilterDetails").hide();
        }

        function ShowFilter() {
            //<summary>Function Used to Show Purchase Request Panel </summary>
            $("#imbHideFilter").show();
            $("#imbShowFilter").hide();
            $("#divFilterDetails").show();
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
            return false;
        }
        //Extra Grid
        function AfterGridExpand(row) {
            if ($("[id$=grdSelectedCusBrands]").attr('id') == $(row).parent().parent().attr('id')) {
                {
                    $(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").val($(row)[0].rowIndex);
                    var hdfPK = $(row).find("[id*=hdfItemPK]");
                    $("[id$=hdfselectedPks]").val($("[id$=hdfselectedPks]").val() + hdfPK.val() + ",");
                    SetGridScroll($(row).parent().parent().parent().parent().find("[id$=_ExpandPosition]").attr("id"));
                    // IsAllExpand();
                }
            }
        }
        function ExpandSelected() {
            if ($("[id$=chkExpandAll]").is(':checked') == false) {
                $("[id$=grdSelectedCusBrands]").find("[id*=hdfItemPK]").each(function () {
                    var pk_array = $("[id$=hdfselectedPks]").val().split(',');

                    for (var i = 0; i < pk_array.length; i++) {
                        if ($(this).val() == pk_array[i]) {
                            $(this).closest("tr").find("a.GridExpandCollapseButton").click();
                            break;
                        }
                    }
                });
            }
            else
                ExpandAll();
        }

        function IsAllExpand() {
            $("[id$=grdSelectedCusBrands]").find("[id*=hdfItemPK]").each(function () {
                var val = $(this).closest("tr").find("a");

            });
        }
        //Expand And Collaps 
        function ExpandAll() {
            $("[id$=grdSelectedCusBrands]").find("[id*=hdfItemPK]").each(function () {
                $(this).closest("tr").find("a.GridExpandCollapseButton").click();
                var colpsBtn = $(this).closest("tr").find("a.GridExpandCollapseButton");
                if ($("[id$=chkExpandAll]").is(':checked')) {
                    $(colpsBtn).closest("tr").next("tr").show();
                    $(colpsBtn).text("-");
                }
                else {
                    $(colpsBtn).closest("tr").next("tr").hide();
                    $(colpsBtn).text("+");
                }
            });
        }
        function SetGridScroll(rowId) {
            if (rowId)
                rowArray = $("[id$=" + rowId + "]");
            else
                rowArray = $("[id$=_ExpandPosition]");
            rowArray.each(function () {
                if ($.trim($(this).val()) != "") {
                    var containerDiv = $(this).parent("[id$=_ScrollContainer]");
                    if (containerDiv != null) {
                        $(containerDiv).scrollTop(document.getElementById($(containerDiv).attr('id')).querySelectorAll('[id$=' + $(containerDiv).attr('grid') + ']')[0].children[0].children[$(this).val()].offsetTop);
                    }
                }
                $(this).val("")
            });
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
                $("[id$=btnClear]").hide();
                $("[id$=btnApply]").hide();
                $("[id$=btnShowProducts]").hide();
                $("[id$=btnShowCustomer]").hide();
                $("[id$=btnView]").show();
                $("[id$=btnSubmit]").show();
                $("[id$=btnCopy]").show();

                //                $("[id$=btnDelete]").hide(); 
            }
            else if (mode == 2) {
                $("[id$=btnDelete]").hide();
                $("[id$=btnClear]").show();
                //                $("[id$=btnCopy]").hide();
                $("[id$=btnApply]").show();
                $("[id$=btnShowProducts]").show();
                $("[id$=btnShowCustomer]").show();
                // $("[id$=btnView]").hide();
                //                $("[id$=btnSubmit]").show();
                //                $("[id$=btnDelete]").hide(); 
            }
            else if (mode == 4) {
                $("[id$=btnClear]").show();
                $("[id$=btnApply]").show();
                $("[id$=btnShowProducts]").show();
                $("[id$=btnShowCustomer]").show();
                //                $("[id$=btnView]").show();
                //                $("[id$=btnSubmit]").show();
                //                $("[id$=btnDelete]").show(); 
            }
            else if (mode == 3) {
                $("[id$=btnClear]").hide();
                $("[id$=btnApply]").hide();
                $("[id$=btnShowProducts]").hide();
                $("[id$=btnShowCustomer]").hide();
                //                $("[id$=btnView]").hide();
                //                $("[id$=btnSubmit]").hide();
                //                $("[id$=btnDelete]").show(); 
            }
            else if (mode == 5) {
                $("[id$=btnClear]").show();
                $("[id$=btnApply]").hide();
                $("[id$=btnShowProducts]").hide();
                $("[id$=btnShowCustomer]").hide();
                //                $("[id$=btnView]").hide();
                //                $("[id$=btnSubmit]").show();
                //                $("[id$=btnDelete]").show(); 
            }

        }

        function SetBtnVisibilityForDiffSBU() {
            // Avoid to fill workflow from different SBU (hiding buttons)
            var userSbu = $("[id$=hdfCurrentUserSbu]").val();
            var brandrateSbu = $("[id$=hdfBrandRateSbu]").val();
            if (!isNaN(parseInt(userSbu)) && !isNaN(parseInt(brandrateSbu))) {
                if (userSbu != brandrateSbu) {
                    $("[id$=btnSubmit]").hide();
                    $("[id$=btnDelete]").hide();
                    $("[id$=btnSaveSubmit]").hide();
                    $("[id$=btnSave]").hide();
                }
            }
        }


        function SetDate(controlID) {
            $("[id$=btnSet]").click();
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
        function ValidateAndConfirm(btn, valGroup) {
            if (ValidatePageNow(valGroup)) {
                ShowDeleteConfirm(btn, '<%= Resources.ErpRes.MsgCopyConfirm %>');
            }
            return false;

        }
        function ShowDeleteConfirm(btn, message) {
            var msgTitle;
            var msg;
            msgTitle = "<%= Resources.ErpRes.Title_Information %>";
            msg = message ? message : "<%= Resources.ErpRes.MsgDeleteConfirm %>";
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
        function InitComponents() {
            $("[id*=txtRate]").ForceNumericOnly();
            InitCustomer();
            if ($('[id$=btnSaveSubmit]').is(":visible"))
                $('[id$=pnlSubmit]').hide();

            GrandScriptUtils.AddDateRangeCommon("txtFromDate", "hdfFromDate", "txtToDate", "hdfToDate", false, false);
            GrandScriptUtils.AddDateRangeCommon("txtCopyFrom", "hdfCopyFrom", "txtCopyTo", "hdfCopyTo", false, false);
        }

        function InitCustomer() {
            var all = '<%= GetLocalResourceObject("All") %>';
            var cus = '<%= GetLocalResourceObject("Customer") %>';
            var pro = '<%= GetLocalResourceObject("Product")%>';
            var pageURL = window.document.URL;
            var virtualPath = $("[id$='hdfAbsolutePath']").val();
            var urlauto = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
            //            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", urlauto, "hdfCustomer", true, true, "CUSTOMER");
            //            GrandScriptUtils.MakeAutoCompleteDDL("txtProduct", urlauto + "?Type=ITM_NAME", "hdfProduct", true, true, "PRODUCTMASTER");


            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", urlauto + "?CustomerID=0&BrandID=" + $("[id$=hdfbrandRatePK]").val(), "hdfCustomer", true, true, "BRANDCUSTOMER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtProduct", urlauto + "?itemPK=0&BrandID=" + $("[id$=hdfbrandRatePK]").val(), "hdfProduct", true, true, "BRANDPRODUCT");


            if ($("[id$='ddlPrint']").val() == '1') {
                $("[id$='lblBProductCustomer']").html(cus);
                $("[id$=txtCustomer]").show();
                if ($("[id$=txtCustomer]").attr("disabled") == false) {
                    $("[id$=txtCustomer]").val(all);
                }
                $("[id$=txtCustomer]").next($(".ddlSelect")).show();
                $("[id$=txtProduct]").hide();
                $("[id$=txtProduct]").next($(".ddlSelect")).hide();
            }
            else {
                $("[id$='lblBProductCustomer']").html(pro);
                $("[id$=txtCustomer]").hide();
                $("[id$=txtCustomer]").next($(".ddlSelect")).hide();
                $("[id$=txtProduct]").show();
                $("[id$=txtProduct]").val(all);
                $("[id$=txtProduct]").next($(".ddlSelect")).show();
            }
            if ($("[id$=txtCustomer]").attr("disabled") == true) {
                DisableAuto($("[id$=txtCustomer]"), $("[id$=hdfCustomer]"));
            }

        }
        //To excecute after auto complete selection
        function AfterAutoCompleteSelect(targetControlID) {

            if (targetControlID == "txtCustomer") {
                $("[id$=txtCustomer]").attr("title", $("[id$=txtCustomer]").val());
            }
            else if (targetControlID == "txtProduct") {
                $("[id$=txtProduct]").attr("title", $("[id$=txtProduct]").val());
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

        $(document).ready(function () {

        });

        function SelectText(obj) {
            var all = '<%= GetLocalResourceObject("All") %>';
            $(obj).val(all);
        }

        function SelectCustomerProduct() {
            var all = '<%= GetLocalResourceObject("All") %>';
            var cus = '<%= GetLocalResourceObject("Customer") %>';
            var pro = '<%= GetLocalResourceObject("Product")%>';
            if ($("[id$='ddlPrint']").val() == '1') {
                $("[id$='lblBProductCustomer']").html(cus);
                $("[id$=txtCustomer]").show();
                $("[id$=txtCustomer]").val(all);
                $("[id$=hdfCustomer]").val("0");
                $("[id$=txtCustomer]").next($(".ddlSelect")).show();
                $("[id$=txtProduct]").hide();
                $("[id$=txtProduct]").next($(".ddlSelect")).hide();
            }
            else {
                $("[id$='lblBProductCustomer']").html(pro);
                $("[id$=txtCustomer]").hide();
                $("[id$=txtCustomer]").next($(".ddlSelect")).hide();
                $("[id$=txtProduct]").show();
                $("[id$=txtProduct]").val(all);
                $("[id$=hdfProduct]").val("0");
                $("[id$=txtProduct]").next($(".ddlSelect")).show();
            }
        }

//        function AfterDateSelect(controlID) {
//            if (controlID == "txtFromDate" || controlID == "txtToDate") {
//                SetDate();
//            }
//        }
    </script>
    <script type="text/javascript">

        $(window).load(function EndRequest() {
            FormatCalendar('4');

            HideFilter();
        });


        var ControlID = 'calendar1|calendar2';
        function EndRequest() { FormatCalendar('4'); }
        function FormatCalendar(type) {
            var ctrlBehaviourarray = ControlID.split('|');
            for (var i = 0; i < ctrlBehaviourarray.length; i++) {
                var calenderCtrl = $find(ctrlBehaviourarray[i]);
                if (calenderCtrl) {
                    switch (type) {
                        case "6":
                            return;
                            break;
                        case "4":
                            $(calenderCtrl).attr('CalenderType', '2');
                            modifyMontDelegates(calenderCtrl);
                            break;
                        case "1":
                            $(calenderCtrl).attr('CalenderType', '3');
                            modifyYearDelegates(calenderCtrl);
                            break;
                    }
                }
            }
        }
        function modifyMontDelegates(cal) {
            //we need to modify the original delegate of the month cell.
            cal._cell$delegates = {
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover),
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout),
                click: Function.createDelegate(cal, function (e) {
                    /// <summary>
                    /// Handles the click event of a cell
                    /// </summary>
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
                    e.stopPropagation();
                    e.preventDefault();
                    if (!cal._enabled) return;
                    var target = e.target;
                    var visibleDate = cal._getEffectiveVisibleDate();
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
                    switch (target.mode) {
                        case "prev":
                        case "next":
                            cal._switchMonth(target.date);
                            break;
                        case "title":
                            switch (cal._mode) {
                                case "days": cal._switchMode("months"); break;
                                case "months": cal._switchMode("years"); break;
                            }
                            break;
                        case "month":
                            //if the mode is month, then stop switching to day mode.
                            if (target.month == visibleDate.getMonth()) {
                                //this._switchMode("days");
                            } else {
                                cal._visibleDate = target.date;
                                //this._switchMode("days");
                            }
                            cal.set_selectedDate(target.date);
                            cal._switchMonth(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                        case "year":
                            if (target.date.getFullYear() == visibleDate.getFullYear()) {
                                cal._switchMode("months");
                            } else {
                                cal._visibleDate = target.date;
                                cal._switchMode("months");
                            }
                            break;
                        // case "day":                                                                               
                        // this.set_selectedDate(target.date);                                                                               
                        // this._switchMonth(target.date);                                                                               
                        // this._blur.post(true);                                                                               
                        // this.raiseDateSelectionChanged();                                                                               
                        // break;                                                                               
                        case "today":
                            cal.set_selectedDate(target.date);
                            cal._switchMonth(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                    }
                })
            }
        }
        function modifyYearDelegates(cal) {
            //we need to modify the original delegate of the month cell.
            cal._cell$delegates = {
                mouseover: Function.createDelegate(cal, cal._cell_onmouseover),
                mouseout: Function.createDelegate(cal, cal._cell_onmouseout),
                click: Function.createDelegate(cal, function (e) {
                    /// <summary>
                    /// Handles the click event of a cell
                    /// </summary>
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param>
                    e.stopPropagation();
                    e.preventDefault();
                    if (!cal._enabled) return;
                    var target = e.target;
                    var visibleDate = cal._getEffectiveVisibleDate();
                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");
                    switch (target.mode) {
                        case "prev":
                        case "next":
                            cal._switchMonth(target.date);
                            break;
                        case "title":
                            switch (cal._mode) {
                                case "days": cal._switchMode("months"); break;
                                case "months": cal._switchMode("years"); break;
                            }
                            break;
                        // case "month":                                                                            
                        // //if the mode is month, then stop switching to day mode.                                                                            
                        // if (target.month == visibleDate.getMonth()) {                                                                            
                        // //this._switchMode("days");                                                                            
                        // } else {                                                                            
                        // cal._visibleDate = target.date;                                                                            
                        // //this._switchMode("days");                                                                            
                        // }                                                                            
                        // cal.set_selectedDate(target.date);                                                                            
                        // cal._switchMonth(target.date);                                                                            
                        // cal._blur.post(true);                                                                            
                        // cal.raiseDateSelectionChanged();                                                                            
                        // break;                                                                            
                        case "year":
                            if (target.date.getFullYear() == visibleDate.getFullYear()) {
                                // cal._switchMode("months");
                            } else {
                                cal._visibleDate = target.date;
                                //cal._switchMode("months");
                            }
                            cal.set_selectedDate(target.date);
                            //cal._switchYear(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                        // case "day":                                                                            
                        // this.set_selectedDate(target.date);                                                                            
                        // this._switchMonth(target.date);                                                                            
                        // this._blur.post(true);                                                                            
                        // this.raiseDateSelectionChanged();                                                                            
                        // break;                                                                            
                        case "today":
                            cal.set_selectedDate(target.date);
                            //cal._switchYear(target.date);
                            cal._blur.post(true);
                            cal.raiseDateSelectionChanged();
                            break;
                    }
                })
            }
        }

        function changeMonthCellHandlers(cal) {
            if (cal._monthsBody) {
                //remove the old handler of each month body.
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
                //add the new handler of each month body.
                for (var i = 0; i < cal._monthsBody.rows.length; i++) {
                    var row = cal._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
            }
        }
        function changeYearCellHandlers(cal) {
            if (cal._monthsBody) {
                //remove the old handler of each month body.
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
                //add the new handler of each month body.
                for (var i = 0; i < cal._yearsBody.rows.length; i++) {
                    var row = cal._yearsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, cal._cell$delegates);
                    }
                }
            }
        }
        function AfterClose(containerID) {
            if (containerID == "#divWkfSubmit") {
                $("[id$=hdfIsSaveSubmit]").val("0");
            }
        }

        function onCalendarShown(cal, args) {
            cal._switchMode("months", true);
            cal._popupBehavior._element.style.zIndex = 10005;
        }

        function onCalendarHidden(sender, args) {
            //                        if (sender.get_selectedDate()) {
            //                            if (sender.get_selectedDate() && sender.get_selectedDate() && cal1.get_selectedDate() > cal2.get_selectedDate()) {
            //                                alert('The "From" Date should smaller than the "To" Date, please reselect!');
            //                                sender.show();
            //                                return;
            //                            }
            //                            //get the final date
            //                            var finalDate = new Date(sender.get_selectedDate());
            //                            var selectedMonth = finalDate.getMonth();
            //                            finalDate.setDate(1);
            //                            if (sender == cal2) {
            //                                // set the calender2's default date as the last day
            //                                finalDate.setMonth(selectedMonth + 1);
            //                                finalDate = new Date(finalDate - 1);
            //                            }
            //                            //set the date to the TextBox
            //                            sender.get_element().value = finalDate.format(sender._format);
            //                        }
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlCustomerRegistration">
        <ContentTemplate>
            <asp:HiddenField ID="hdfDecimalFormat" runat="server" />
            <asp:HiddenField ID="hdfCurrencyFormat" runat="server" />
            <asp:HiddenField ID="hdfRateFormat" runat="server" />
            <div class="fixed-buttons-normal" id="divFixedTab">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlCopy">
                                        <asp:Button runat="server" ID="btnCopy" CommandName="COPY" TabIndex="15" Text="<%$resources:ErpRes,Copy %>"
                                            OnClick="ActionHandler" ValidationGroup="brandRate" OnClientClick="javascript:ValidatePageNow('brandRate')"
                                            ToolTip="<%$resources:ErpRes,Copy %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-copy" />
                                        <%--OnClientClick="javascript: return ValidateAndConfirm(this,'brandRate')"--%>
                                    </li>
                                    <li runat="server" id="pnlSubmit">
                                        <asp:Button runat="server" ID="btnSubmit" CommandName="SUBMIT" TabIndex="16" Text="<%$resources:ErpRes,Submit %>"
                                            OnClick="ActionHandler" ValidationGroup="brandRate" OnClientClick="javascript:ValidatePageNow('brandRate')"
                                            ToolTip="<%$resources:ErpRes,Submit %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-submit" />
                                    </li>
                                    <li>
                                        <li runat="server" id="pnlSaveSubmit">
                                            <asp:HiddenField ID="hdfIsSaveSubmit" runat="server" Value="0" />
                                            <asp:Button runat="server" ID="btnSaveSubmit" CommandName="SAVESUBMIT" TabIndex="17"
                                                Text="<%$resources:ErpRes,SaveSubmit %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('enquiry')"
                                                ValidationGroup="enquiry" ToolTip="<%$resources:ErpRes,SaveSubmit %>" CommandArgument="SEC_ActionPanel"
                                                SkinID="btnInner-submit" />
                                        </li>
                                        <li>
                                            <asp:Button runat="server" ID="btnSave" Visible="false" CommandName="SAVE" TabIndex="18"
                                                Text="<%$resources:ErpRes,Save %>" OnClick="ActionHandler" OnClientClick="javascript:ValidatePageNow('brandRate')"
                                                ToolTip="<%$resources:ErpRes,Save %>" SkinID="btnInner-Save" CommandArgument="SEC_ActionPanel" />
                                        </li>
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnDelete" CommandName="DELETE" TabIndex="19" Visible="false"
                                            Text="<%$resources:ErpRes,Delete %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" />
                                    </li>
                                    <li runat="server" id="pnlView">
                                        <asp:Button runat="server" ID="btnView" CommandName="VIEW" TabIndex="20" Text="<%$resources:ErpRes,View %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,View %>" ValidationGroup="brandRate"
                                            OnClientClick="javascript:ValidatePageNow('brandRate')" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-View" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <%--use the width property of the below table corresponding to the contents in the page--%>
            <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                <%--Rename this ID Page_Entry with the corresponding section Id in the documet--%>
                <asp:TableRow ID="PageAction_Entry" runat="server">
                    <%--Align table cell according to design--%>
                    <asp:TableCell>
                        <div class="content-wrapper">
                            <%--Filter area--%>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <%--<asp:Label runat="server" ID="lblControl" Text="<%$ resources:Month %>" AssociatedControlID="txtCalender"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtCalender" ClientIDMode="Static" CssClass="medium"
                                                TabIndex="1" MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"
                                                ValidationGroup="brandRate"></asp:TextBox>
                                            <div style="display: none">
                                                <asp:Button runat="server" ID="btnSet" CommandName="SET" Text="<%$resources:ErpRes,Go %>"
                                                    OnClick="ActionHandler" ValidationGroup="enquiry" ToolTip="<%$resources:ErpRes,Go %>"
                                                    CommandArgument="SEC_ActionPanel" SkinID="btnInner-set" OnClientClick="javascript:ValidatePageNow('brandRate')" />
                                            </div>
                                            <cc1:CalendarExtender ID="txtCalender_CalendarExtender" runat="server" BehaviorID="calendar1"
                                                TargetControlID="txtCalender" Format="MMM-yyyy" OnClientShown="onCalendarShown"
                                                ClientIDMode="Static" OnClientHidden="onCalendarHidden" OnClientDateSelectionChanged="SetDate">
                                            </cc1:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="vrftxtCalender" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="brandRate" EnableClientScript="true" runat="server" ControlToValidate="txtCalender"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Month %>">
                                            </asp:RequiredFieldValidator>--%>
                                            <asp:Label ID="lblFromDate" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtFromDate" ></asp:Label>
                                            <asp:HiddenField ID="hdfFromDate" runat="server" />
                                            <asp:TextBox ID="txtFromDate" runat="server" TabIndex="2" CssClass="input-small"  ValidationGroup="brandRate"
                                                MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfFromDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="brandRate" EnableClientScript="true" runat="server" ControlToValidate="txtFromDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_FromDate %>">
                                            </asp:RequiredFieldValidator>                                            
                                            <div style="display: none">
                                                <asp:Button runat="server" ID="btnSet" CommandName="SET" Text="<%$resources:ErpRes,Go %>"
                                                    OnClick="ActionHandler" ValidationGroup="brandRate" ToolTip="<%$resources:ErpRes,Go %>"
                                                    CommandArgument="SEC_ActionPanel" SkinID="btnInner-set" OnClientClick="javascript:ValidatePageNow('brandRate')" />
                                            </div>

                                             <asp:Label ID="lblToDate" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtToDate"></asp:Label>
                                            <asp:HiddenField ID="hdfToDate" runat="server" />
                                            <asp:TextBox ID="txtToDate" runat="server" TabIndex="3" CssClass="input-small" MaxLength="13"  ValidationGroup="brandRate"
                                                onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="vrfToDate" CssClass="star" SetFocusOnError="true"
                                                ValidationGroup="brandRate" EnableClientScript="true" runat="server" ControlToValidate="txtToDate"
                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ToDate %>">
                                            </asp:RequiredFieldValidator>                                            
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lbnStatus" Text="<%$ resources:Status %>" AssociatedControlID="lblStatus"></asp:Label>
                                            <asp:Label ID="lblStatus" runat="server" CssClass="input-small-c"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div id="divFilter">
                                <h1 class="search-colapse-normal">
                                    <%= GetLocalResourceObject("Filter").ToString() %>
                                    <img id="imbShowFilter" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                                        alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowFilter();" />
                                    <img id="imbHideFilter" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="Hide"
                                        title="Hide" style="cursor: pointer" onclick="javascript:HideFilter();" />
                                </h1>
                                <div id="divFilterDetails">
                                    <div class="head-btn">
                                        <h1>
                                            <%= GetLocalResourceObject("Products").ToString()%>
                                        </h1>
                                        <asp:Button runat="server" ID="btnShowProducts" CssClass="IMAGEenable-popup" OnClick="ActionHandler"
                                            TabIndex="2" CommandName="SHOWPRODUCTS" EnableTheming="false" ToolTip="<%$ resources:ShowProducts %>" />
                                        <%--<asp:ImageButton ID="btnShowProducts"  CssClass="IMAGEenable-popup" 
                                            runat="server" OnClick="ActionHandler" TabIndex="3" CommandName="SHOWPRODUCTS" />--%>
                                        <%--  SkinID="popup"  ToolTip="<%$ resources:ShowProducts %>"--%>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div class="gridwrap max-200">
                                        <asp:GridView runat="server" ID="grdProducts" Width="100%" AllowSorting="True" AutoGenerateColumns="false"
                                            EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyBrands %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:ProductCode %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfProductPK" runat="server" Value='<%# Eval("ITM_PK") %>' />
                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("ITM_CODE"),15) %>'
                                                            ToolTip='<%# Eval("ITM_CODE")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ProductDescription %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("ITM_DESC") %>' ToolTip='<%# Eval("ITM_DESC")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="85%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div class="head-btn">
                                        <h1>
                                            <%= GetLocalResourceObject("Customers").ToString()%>
                                        </h1>
                                        <asp:Button runat="server" ID="btnShowCustomer" EnableTheming="false" CssClass="IMAGEenable-popup"
                                            ToolTip="<%$ resources:ShowCustomers %>" OnClick="ActionHandler" TabIndex="3"
                                            CommandName="SHOWCUSTOMER" />
                                        <%-- <asp:ImageButton ID="btnShowCustomer" CssClass="IMAGEenable-popup" ToolTip="<%$ resources:ShowCustomers %>" 
                                            runat="server" OnClick="ActionHandler" TabIndex="4" CommandName="SHOWCUSTOMER" />--%>
                                        <%--  SkinID="popup"--%>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div class="gridwrap max-200">
                                        <asp:GridView runat="server" ID="grdCustomers" Width="100%" AllowSorting="True" AutoGenerateColumns="false"
                                            EmptyDataRowStyle-CssClass="emptytable">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyCustomer %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:CustomerCode %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfCusPk" runat="server" Value='<%# Eval("CUS_PK") %>' />
                                                        <asp:Label ID="lblCustomerCode" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("CUS_CODE"),15) %>'
                                                            ToolTip='<%#HttpUtility.HtmlDecode( Eval("CUS_CODE").ToString())%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="15%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:CustomerName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("CUS_NAME") %>' ToolTip='<%#HttpUtility.HtmlDecode( Eval("CUS_NAME").ToString())%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="85%" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                             <%--  New Packing Spec filter Start--%>
                                   <div class="head-btn">
                                        <h1>
                                            <%= GetLocalResourceObject("PackingSpec").ToString()%>
                                        </h1> 
                                        <div class="clear"></div>
                                      <div class="tree-label2M w64perc">                                                                                                                 
                                        <label class="lbl-30perc float-left"> <%= GetLocalResourceObject("PackingSpecName").ToString()%></label>                                
                                        <uc1:CheckListSearchControl ID="chklstPackingSpec" runat="server" />
                                      </div>  
                                   </div>
                                </div>
                            <%-- End New Packing Spec Filter --%>

                                    <div class="button-wrap-right">
                                        <asp:Button runat="server" ID="btnApply" CommandName="APPLY" TabIndex="5" CssClass="BTNenable-submit"
                                            Text="<%$resources:ErpRes,Apply %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Apply %>" />
                                        <%--  SkinID="btnInner-submit"--%>
                                        <asp:Button runat="server" ID="btnClear" CommandName="CLEAR" TabIndex="4" Text="<%$resources:ErpRes,Clear %>"
                                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Clear %>" CommandArgument="SEC_ActionPanel"
                                            SkinID="btnInner-Cancel" />
                                    </div>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="CustomerBrands">
                                <h1 class="search-colapse-normal">
                                    <%= GetLocalResourceObject("SelectedBrands").ToString()%>
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
                                                    <div class="div2col-S">
                                                        <asp:Label runat="server" ID="lblGroupBy" Text="<%$ resources:GroupBy %>" AssociatedControlID="rdbCustomer"></asp:Label>
                                                        <div class="check-inline check-inlinespan">
                                                            <asp:RadioButton ID="rdbCustomer" OnCheckedChanged="ActionHandler" AutoPostBack="true"
                                                                GroupName="rdbType"  runat="server" Checked="true" TabIndex="6" />
                                                                 <asp:Label runat="server" ID="lblCust" Text="Customer" AssociatedControlID="rdbCustomer" CssClass="lbl-34perc"></asp:Label>
                                                            <asp:RadioButton ID="rdbProduct" OnCheckedChanged="ActionHandler" TabIndex="7" AutoPostBack="true"
                                                                GroupName="rdbType"  runat="server" Checked="false" />
                                                                <asp:Label runat="server" ID="lblPrdct" Text="Product" AssociatedControlID="rdbProduct" CssClass="lbl-26-5perc"></asp:Label>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div class="div2col-S" style="text-align: right">
                                                        <asp:Label runat="server" ID="lblExpandAll" Text="<%$ resources:ExpandAll %>" AssociatedControlID="chkExpandAll"></asp:Label>
                                                        <%--<asp:CheckBox ID="chkExpandAll" onclick="javascript:return ExpandAll();" Text="."  runat="server" AutoPostBack="false" />--%>
                                                        <asp:CheckBox ID="chkExpandAll" runat="server" TabIndex="8" onclick="javascript:ExpandAll();"
                                                            Text="" Checked="false" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" style="text-align: right">
                                                    <%--<asp:Button runat="server" ID="btnSHOWGRIDHEADERPOPUP"  CssClass="IMAGEaction-popup"
                                                        OnClick="ActionHandler" TabIndex="3" CommandName="SHOWGRIDHEADERPOPUP" EnableTheming="false"
                                                        ToolTip="<%$ resources:Rate %>" style="margin-right:27px; margin-bottom:0px;" />--%>
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
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="hdfBrand_ExpandPosition" runat="server" />
                                        <cc2:ExtGridView runat="server" ID="grdSelectedCusBrands" AutoGenerateColumns="False"
                                            OnRowDataBound="ActionHandler" Width="100%" ExpandButtonCssClass="GridExpandCollapseButton"
                                            CollapseButtonCssClass="GridExpandCollapseButton" GridLines="None" ExpandButtonText="+"
                                            CollapseButtonText="-" EmptyDataRowStyle-CssClass="emptytable" ShowFooter="true"
                                            PageSize="<%$ resources:PageSize %>">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyBrands %>"></asp:Label>
                                            </EmptyDataTemplate>
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ resources:ProductCode %>">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdfIsExpand" Value="0" runat="server" />
                                                        <%--   <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%# Eval("ITM_PK") %>' />
                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("ITM_CODE") %>' ToolTip='<%# Eval("ITM_CODE")%>'>
                                                        </asp:Label>--%>
                                                        <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%# Eval("HDR_PK") %>' />
                                                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("HDR_CODE") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("HDR_TEXT").ToString())%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="12%" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:ProductDescription %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("HDR_NAME") %>' ToolTip='<%# HttpUtility.HtmlDecode(Eval("HDR_NAME").ToString())%>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="80%" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ resources:Rate %>">
                                                    <HeaderTemplate>
                                                        <asp:Button runat="server" ID="btnSHOWGRIDHEADERPOPUP" CssClass="IMAGEaction-popup"
                                                            OnClick="ActionHandler" TabIndex="13" CommandName="SHOWGRIDHEADERPOPUP" EnableTheming="false"
                                                            ToolTip="<%$ resources:Rate %>" Style="margin-right: 27px; margin-bottom: 0px;" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnRate" runat="server" OnClick="ActionHandler" CommandName="SETRATE"
                                                            CommandArgument='<%# Eval("HDR_PK") %>' SkinID="imbactiongrid" ToolTip="<%$ resources:Rate %>"
                                                            TabIndex="13" ValidationGroup="brandRate" OnClientClick="javascript:ValidatePageNow('brandRate')" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ControlStyle-Width="98%">
                                                    <ItemTemplate>
                                                        <asp:GridView runat="server" ID="grdSelectdCustomers" AutoGenerateColumns="False"
                                                            GridLines="None" HeaderStyle-BackColor="#EEF3FB" AllowPaging="false" EmptyDataRowStyle-CssClass="emptytable"
                                                            OnRowDataBound="ActionHandler" OnRowDeleted="ActionHandler">
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                                                            </EmptyDataTemplate>
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ resources:CustomerCode %>">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="hdfCusPK" runat="server" Value='<%# Eval("HDR_PK") %>' />
                                                                        <asp:HiddenField ID="hdfCurrPK" runat="server" Value='<%# Eval("CUR_PK") %>' />
                                                                        <asp:HiddenField ID="hdfCustomerItem" runat="server" Value='<%# Eval("CIM_PK") %>' />
                                                                        <asp:HiddenField ID="hdfItemPK" runat="server" Value='<%# Eval("HDR_PK") %>' />
                                                                        <asp:Label ID="lblCustomer" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("DTL_CODE"),25) %>'
                                                                            ToolTip='<%# Eval("DTL_TEXT") %>'> 
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="10%" />
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:BrandName %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBrandCode" runat="server" Text='<%# Eval("CIM_BRAND_NAME") %>'
                                                                            ToolTip='<%# Eval("CIM_BRAND_TEXT") %>'> 
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="45%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Packing %>">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPacking" runat="server" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("APS_TEXT"),36) %>'
                                                                            ToolTip='<%# Eval("APS_TEXT") %>'> 
                                                                        </asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="22%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:Currency %>">
                                                                    <ItemTemplate>
                                                                        <%--<asp:DropDownList ID="ddlCurrency" runat="server" Width="60px" Enabled="false">
                                                                        </asp:DropDownList>--%>
                                                                        <asp:Label ID="lblGridCurrency" runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="6%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ resources:NewRate %>">
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtNewRate" runat="server" Text='<%# GetFormattedRate(Eval("BRD_RATE")) %>'
                                                                            TabIndex="9" ToolTip='<%# Eval("BRD_RATE") %>' ValidationGroup="brandRate" CssClass="small-a numeric"
                                                                            MaxLength="15"></asp:TextBox>
                                                                        <div class="starwrap-relative">
                                                                            <%-- '<%# GetFormattedRate(Eval("SOD_RATE")) %>'<asp:RegularExpressionValidator ID="vreNewRate" runat="server" ControlToValidate="txtNewRate"
                                                                                ErrorMessage="<%$ resources:Err_Rate %>" ValidationExpression="^\$?([0-9]{0,14})?(\.[0-9]{0,6})?$"
                                                                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="brandRate">
                                                                            </asp:RegularExpressionValidator>--%>
                                                                            <%-- <asp:RequiredFieldValidator ID="reqNewRate" CssClass="star" SetFocusOnError="true"
                                                                                ValidationGroup="brandRate" EnableClientScript="true" runat="server" ControlToValidate="txtNewRate"
                                                                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>">
                                                                            </asp:RequiredFieldValidator>--%>
                                                                            <cc1:RateValidation ID="vreNewRate" runat="server" ControlToValidate="txtNewRate"
                                                                                ErrorMessage="<%$ resources:Err_Rate %>" NumberDigits="10" Display="Dynamic"
                                                                                Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="brandRate"></cc1:RateValidation>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="8%" HorizontalAlign="Right" />
                                                                    <HeaderStyle CssClass="amount-numeric" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblUOM" Text='<%# ERP.Utilities.CommonFunctions.GetShortString(Eval("BRD_SALE_UOM_TEXT"),8) %>'
                                                                            ToolTip='<%# Eval("BRD_SALE_UOM_TEXT") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="4%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="btnHistory" runat="server" OnClick="ActionHandler" CommandName="RATEHISTORY"
                                                                            CommandArgument='<%# Eval("CIM_PK") %>' SkinID="history" ToolTip="<%$ resources:History %>"
                                                                            TabIndex="13" />
                                                                        <asp:ImageButton ID="btnRemoveItem" runat="server" OnClick="ActionHandler" CommandName="REMOVEITEM"
                                                                            CommandArgument='<%# Eval("CIM_PK") %>' SkinID="imbdeletegrid" ToolTip="Delete"
                                                                            OnClientClick="return ShowDeleteConfirm(this);" TabIndex="13" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="5%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <%--Second--%>
                                                            <RowStyle CssClass="table-secondlevel" />
                                                            <HeaderStyle CssClass="table-secondlevela" />
                                                        </asp:GridView>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <%--First--%>
                                            <RowStyle CssClass="table-firstlevel" />
                                            <HeaderStyle CssClass="table-firstlevela" />
                                            <FooterStyle CssClass="table-firstlevela-total" />
                                        </cc2:ExtGridView>
                                        <uc1:PagerControl ID="uclPaging" runat="server" Visible="false" />
                                    </div>
                                    <div class="button-wrap-right">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow ID="ModifiedDatePnl" runat="server" CssClass="last-modified" Visible="true">
                    <asp:TableCell>
                        <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div id="diverror" style="display: none">
                <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                <asp:ValidationSummary ID="vsPage" ValidationGroup="setRate" runat="server" />
                <asp:ValidationSummary ID="vsDtl" ValidationGroup="brandRate" runat="server" />
                <asp:ValidationSummary ID="vsRate" ValidationGroup="RateApply" runat="server" />
                <asp:ValidationSummary ID="vsRateCopy" ValidationGroup="brandRateCopy" runat="server" />
            </div>
            <div id="divSearchProducts" style="display: none;">
                <div style="margin: 4px 0px 4px 13px;">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label2" Text="<%$ resources:ErpRes,Type %>" AssociatedControlID="ddlTypeProductListPopUp"
                                    Style="margin: 3px;"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label4" Text="<%$ resources:ErpRes,Thickness %>" AssociatedControlID="ddlThicknessProductListPopUp"
                                    Style="margin: 3px;"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label5" Text="<%$ resources:ErpRes,Category %>" AssociatedControlID="ddlCategoryProductListPopUp"
                                    Style="margin: 3px;"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label6" Text="<%$ resources:ErpRes,Surface %>" AssociatedControlID="ddlSurfaceProductListPopUp"
                                    Style="margin: 3px;"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label7" Text="<%$ resources:ErpRes,Shade %>" AssociatedControlID="ddlShadeProductListPopUp"
                                    Style="margin: 3px;"></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlTypeProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlThicknessProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCategoryProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSurfaceProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlShadeProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="Label8" Text="<%$ resources:ErpRes,Classification %>"
                                    AssociatedControlID="ddlClassificationProductListPopUp" Style="margin: 3px;"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label9" Text="<%$ resources:ErpRes,Size %>" AssociatedControlID="ddlSizeProductListPopUp"
                                    Style="margin: 3px;"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label10" Text="<%$ resources:ErpRes,Length %>" AssociatedControlID="ddlLengthProductListPopUp"
                                    Style="margin: 3px;"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label11" Text="<%$ resources:ErpRes,Chlorination %>"
                                    Style="margin: 3px;" AssociatedControlID="ddlChlorinationProductListPopUp"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label12" Text="<%$ resources:ErpRes,Side %>" AssociatedControlID="ddlSideProductListPopUp"
                                    Style="margin: 3px;"></asp:Label>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlClassificationProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSizeProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlLengthProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlChlorinationProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSideProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                            </td>
                        </tr>

               <%-- ************Additional Spec Fields *******************************--%>
                         <tr>
                            <td>
                                <asp:Label runat="server" ID="Label16" Text="<%$ resources:Controls,ADNL_SPEC05 %>" AssociatedControlID="ddlAdnlSpec05ProductListPopUp"
                                    Style="margin: 3px;"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label17" Text="<%$ resources:Controls,ADNL_SPEC06 %>" AssociatedControlID="ddlAdnlSpec06ProductListPopUp"
                                    Style="margin: 3px;"></asp:Label>
                            </td>
                            <td>
                                <asp:Label runat="server" ID="Label18" Text="<%$ resources:Controls,ADNL_SPEC07 %>" AssociatedControlID="ddlAdnlSpec07ProductListPopUp"
                                    Style="margin: 3px;"></asp:Label>
                            </td>                           
                            <td>
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <asp:DropDownList ID="ddlAdnlSpec05ProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAdnlSpec06ProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAdnlSpec07ProductListPopUp" CssClass="medium" runat="server">
                                </asp:DropDownList>
                            </td>                           
                            <td>
                                <asp:Button runat="server" ID="btnProductListPopUpOk" CommandName="PRODUCTLISTPOPUPOK"
                                    TabIndex="23" Text="<%$resources:ErpRes,Go %>" ToolTip="<%$resources:ErpRes,Go %>"
                                    OnClick="ActionHandler" SkinID="btnInner-ok" Style="margin: 0px;" />
                            </td>
                        </tr>
               <%-- ************End Additional Spec Fields *************************--%>
                    </table>
                </div>
                <div class="Button-container-popup">
                    <asp:Button runat="server" ID="btnApplayProduct" CommandName="PRODUCTAPPLY" TabIndex="23"
                        OnClientClick="return ValidateProductSelect();" Text="<%$resources:ErpRes,Select %>"
                        OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Select %>" SkinID="btnInner-ok" />
                    <asp:Button runat="server" ID="btnCancelProduct" CommandName="PRODUCTCANCEL" TabIndex="24"
                        Text="<%$resources:ErpRes,Cancel %>" OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Cancel %>"
                        SkinID="btnInner-Cancel" />
                </div>
                <div class="content-wrapper">
                    <div class="treeview max-200">
                        <asp:TreeView ID="trvProducts" runat="server" ShowLines="true" onclick="OnCheckBoxCheckChanged(event);"
                            ExpandDepth="0" InitialExpandDepth="2" ShowCheckBoxes="Parent,Leaf">
                        </asp:TreeView>
                    </div>
                    <table class="gridwrap" id="tblEmptyRecord" visible="false" runat="server">
                        <tr class="emptytable">
                            <td>
                                <asp:Label ID="lblEmptyGrid" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="divCopyBrand" style="display: none;">
                <div class="content-wrapper">
                    <%--<asp:Label runat="server" ID="lblCopyMonth" Text="<%$ resources:CopyTo %>" AssociatedControlID="txtCopyMonth"></asp:Label>
                    <asp:TextBox runat="server" ID="txtCopyMonth" ClientIDMode="Static" CssClass="medium"
                        TabIndex="1" MaxLength="11" onkeydown="return CheckKey(event)" onpaste="return false;"
                        ValidationGroup="brandRate"></asp:TextBox>
                    <cc1:CalendarExtender ID="ceCopyMonth" runat="server" BehaviorID="calendar2" TargetControlID="txtCopyMonth"
                        Format="MMM-yyyy" OnClientShown="onCalendarShown" ClientIDMode="Static" OnClientHidden="onCalendarHidden">
                    </cc1:CalendarExtender>
                    <asp:RequiredFieldValidator ID="reqCopyMonth" CssClass="star" SetFocusOnError="true"
                        ValidationGroup="brandRateCopy" EnableClientScript="true" runat="server" ControlToValidate="txtCopyMonth"
                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Month %>">
                    </asp:RequiredFieldValidator>--%>
                    <asp:Label ID="Label14" runat="server" Text="<%$resources:FromDate %>" AssociatedControlID="txtCopyFrom" ></asp:Label>
                    <asp:HiddenField ID="hdfCopyFrom" runat="server" />
                    <asp:TextBox ID="txtCopyFrom" runat="server" TabIndex="2" CssClass="input-small-c"
                        MaxLength="13" onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="star" SetFocusOnError="true"
                        ValidationGroup="gst" EnableClientScript="true" runat="server" ControlToValidate="txtCopyFrom"
                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_FromDate %>">
                    </asp:RequiredFieldValidator>

                     <asp:Label ID="Label15" runat="server" Text="<%$resources:ToDate %>" AssociatedControlID="txtCopyTo"></asp:Label>
                    <asp:HiddenField ID="hdfCopyTo" runat="server" />
                    <asp:TextBox ID="txtCopyTo" runat="server" TabIndex="3" CssClass="input-small-c" MaxLength="13"
                        onkeydown="return CheckKey(event)" onpaste="return false;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="star" SetFocusOnError="true"
                        ValidationGroup="gst" EnableClientScript="true" runat="server" ControlToValidate="txtCopyTo"
                        Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_ToDate %>">
                    </asp:RequiredFieldValidator>                   

                    <h5>
                        <%= GetLocalResourceObject("CopyMsg").ToString()%>
                    </h5>
                    <div class="button-wrap-center">
                        <asp:Button runat="server" ID="btnCopySave" CommandName="SAVECOPY" TabIndex="23"
                            OnClientClick="javascript:ValidatePageNow('brandRateCopy')" Text="<%$resources:ErpRes,Ok %>"
                            OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Save %>" SkinID="btnInner-ok" />
                        <asp:Button runat="server" ID="btnCancelSave" OnClientClick="javascript:ClosePopup()"
                            CommandName="CANCELCOPY" TabIndex="24" Text="<%$resources:ErpRes,Cancel %>" ToolTip="<%$resources:ErpRes,Cancel %>"
                            SkinID="btnInner-Cancel" />
                    </div>
                </div>
            </div>
            <div id="divRateSett" style="display: none;">
                <div class="Button-container-popup">
                    <asp:Button runat="server" ID="btnRateApply" ValidationGroup="RateApply" CommandName="RATEAPPLY"
                        OnClientClick="javascript:ValidatePageNow('RateApply')" TabIndex="23" Text="<%$resources:ErpRes,Apply %>"
                        OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Apply %>" SkinID="btnInner-ok" />
                    <asp:Button runat="server" ID="btnCancel" CommandName="CANCEL" TabIndex="24" Text="<%$resources:ErpRes,Cancel %>"
                        OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Cancel %>" SkinID="btnInner-Cancel" />
                </div>
                <div class="content-wrapper">
                    <div class="divcol-P1">
                        <asp:Label runat="server" ID="lbnProductCode" Text="<%$ resources:ProductCode %>"
                            AssociatedControlID="lblProductCode"></asp:Label>
                        <asp:Label ID="lblProductCode" runat="server"></asp:Label>
                        <asp:Label runat="server" ID="lbnProductDesc" Text="<%$resources:ProductDescription %>"
                            AssociatedControlID="lblProductDesc"></asp:Label>
                        <asp:Label ID="lblProductDesc" runat="server"></asp:Label>
                        <asp:Label runat="server" ID="lblRate" Text="<%$ resources:Rate %>" AssociatedControlID="ddlCurrency"></asp:Label>
                        <asp:DropDownList ID="ddlCurrency" CssClass="half" runat="server">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtNewRateApply" ValidationGroup="RateApply" runat="server" CssClass="input-w58 numeric"></asp:TextBox>
                        <div class="starwrap">
                            <asp:RequiredFieldValidator ID="vrfNewRate" CssClass="star" SetFocusOnError="true"
                                ValidationGroup="RateApply" EnableClientScript="true" runat="server" ControlToValidate="txtNewRateApply"
                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_Rate %>"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="vreNewRate" runat="server" ControlToValidate="txtNewRateApply"
                                ErrorMessage="<%$ resources:Err_Rate %>" ValidationExpression="^\$?([0-9]{0,14})?(\.[0-9]{0,5})?$"
                                Display="Dynamic" Text="*" EnableClientScript="true" CssClass="star" ValidationGroup="RateApply">
                            </asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div id="divHistory" style="display: none;">
                <div class="divcol-P1">
                    <asp:Label runat="server" ID="lbnHCustomer" CssClass="middle-lbl" Text="<%$ resources:Customer %>" AssociatedControlID="lblHCustomer"></asp:Label>
                    <asp:Label ID="lblHCustomer" runat="server" CssClass="input-w72-6per"></asp:Label>
                    <asp:Label runat="server" ID="lbnHBrand" Text="<%$ resources:Brand %>" AssociatedControlID="lblHBrand" CssClass="middle-lbl"></asp:Label>
                    <asp:Label ID="lblHBrand" runat="server" CssClass="input-w72-6per"></asp:Label>
                    <asp:Label runat="server" ID="lbnHProduct" Text="<%$ resources:Product %>" AssociatedControlID="lblHProduct" CssClass="middle-lbl"></asp:Label>
                    <asp:Label ID="lblHProduct" runat="server" CssClass="input-small-d"></asp:Label>
                </div>
                <div class="gridwrap">
                    <asp:GridView runat="server" ID="grdRateHistory" Width="100%" AllowSorting="True"
                        AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable" ShowHeader="true">
                        <EmptyDataTemplate>
                            <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid
            %>"></asp:Label>
                        </EmptyDataTemplate>
                        <Columns>
                            <asp:TemplateField HeaderText="<%$
            resources:Month %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblPeriod" runat="server" Text='<%#
            Eval("BRH_DATE_FROM", Resources.Constants.MonthFormatGrid) %>' ToolTip='<%# Eval("BRH_DATE_FROM",
            Resources.Constants.MonthFormatGrid) %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="35%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Currency
            %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# Eval("BRD_CURR_CODE")
            %>' ToolTip='<%# Eval("BRD_CURR_NAME")%>'> </asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="35%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ resources:Rate
            %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRate" runat="server" Text='<%# Eval("BRD_RATE")
            %>' ToolTip='<%# Eval("BRD_RATE")%>'> </asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="30%" HorizontalAlign="Right" />
                                <HeaderStyle CssClass="amount-numeric" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="divSearchCustomers" style="display: none;">
                <div style="margin: 4px 0px 4px 13px; height: 50px;">
                      <asp:Label runat="server" ID="lblCustSpecialCategory" Text="<%$ resources:CustSpecialCategory%>"  CssClass="lbl-26perc" AssociatedControlID="ddlCustSpecialCategory"></asp:Label>
                    <asp:DropDownList ID="ddlCustSpecialCategory" CssClass="select-small-e2" runat="server"
                        TabIndex="5" >
                    </asp:DropDownList>
                    <div class="clear"></div>
                    <asp:Label runat="server" ID="Label1" Text="<%$ resources:Currency %>"  CssClass="lbl-26perc" AssociatedControlID="ddlCurrencyCustomerListPopUp"></asp:Label>
                    <asp:DropDownList ID="ddlCurrencyCustomerListPopUp" CssClass="select-small-e2" runat="server">
                    </asp:DropDownList>  
                    <asp:Button runat="server" ID="btnCustomerListPopUpOk" CommandName="CUSTOMERLISTPOPUPOK"
                        TabIndex="23" Text="<%$resources:ErpRes,Go %>" ToolTip="<%$resources:ErpRes,Go %>"
                        OnClick="ActionHandler" SkinID="btnInner-ok" />                     
                </div>               
                <div class="Button-container-popup">
                    <asp:Button runat="server" ID="btnApplyCustomer" CommandName="CUSTOMERAPPLY" TabIndex="23"
                        OnClientClick="return ValidateCustomerSelect();" Text="<%$resources:ErpRes,Select %>"
                        OnClick="ActionHandler" ToolTip="<%$resources:ErpRes,Select %>" SkinID="btnInner-ok" />
                    <asp:Button runat="server" ID="btnCancleCustomer" OnClick="ActionHandler" CommandName="CUSTOMERCANCEL"
                        TabIndex="24" Text="<%$resources:ErpRes,Cancel %>" ToolTip="<%$resources:ErpRes,Cancel %>"
                        SkinID="btnInner-Cancel" />
                </div>
                <div class="content-wrapper">
                    <div class="treeview max-200">
                        <asp:TreeView ID="trvCustomers" runat="server" onclick="OnCheckBoxCheckChanged(event);"
                            ShowLines="true" ExpandDepth="0" InitialExpandDepth="2" ShowCheckBoxes="Parent,Leaf">
                        </asp:TreeView>
                    </div>
                </div>
            </div>
            <%--Print popup window --%>
            <div id="divPrint" style="display: none" class="content-wrapper">
                <table class="table-devide">
                    <tr>
                        <td align="right" style="width: 30%;">
                            <asp:Label runat="server" ID="lblPrint" Text="<%$ resources:GroupBy
            %>" AssociatedControlID="ddlPrint"></asp:Label>
                        </td>
                        <td align="left" style="width: 70%;">
                            <asp:DropDownList ID="ddlPrint" runat="server" Width="200px" TabIndex="101" CssClass="medium"
                                onchange="SelectCustomerProduct()">
                                <asp:ListItem Text="Customer" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Product" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" style="width: 30%;">
                            <asp:Label runat="server" ID="lblBProductCustomer" Text="<%$ resources:Customer %>"
                                AssociatedControlID="txtCustomer"></asp:Label>
                        </td>
                        <td align="left" style="width: 70%;">
                            <asp:TextBox ID="txtCustomer" Width="270px" runat="server" MaxLength="100" TabIndex="102"
                                onchange="javascript:SelectText(this);"></asp:TextBox>
                            <asp:HiddenField ID="hdfCustomer" runat="server" />
                            <asp:TextBox ID="txtProduct" Width="270px" runat="server" TabIndex="102" onchange="javascript:SelectText(this);"></asp:TextBox>
                            <asp:HiddenField ID="hdfProduct" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button runat="server" TabIndex="103" ID="btnPrintlist" Text="<%$resources:Controls,Print
            %>" SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" CommandName="PRINT" OnClick="ActionHandler" />
                            <asp:Button runat="server" ID="btnCancellist" Text="<%$resources:Controls,Cancel
            %>" TabIndex="104" OnClientClick="javascript:ClosePopup();" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hdfCurrentUserSbu" Value="0" runat="server" />
            <asp:HiddenField ID="hdfBrandRateSbu" Value="0" runat="server" />
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <asp:HiddenField ID="hdfbrandRatePK" runat="server" />
                <uc1:WorkflowUserComments ID="ucrWrkf" runat="server" />
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnAttach" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">

        function changeRateValueSHOWGRIDHEADERPOPUP(type, value) {
            // alert('In changeRateValueSHOWGRIDHEADERPOPUP');  
            try {
                value = parseFloat(value);
                if (type == 'Value') {
                    $("[id$=txtNewRate]").each(function () {
                        var oldVal = parseFloat($(this).val());
                        var newVal = oldVal + value;
                        $(this).val(newVal.toFixed(5));
                    });
                }
                else {
                    $("[id$=txtNewRate]").each(function () {
                        var oldVal = parseFloat($(this).val());
                        var percentageResult = (oldVal * value) / 100;
                        var newVal = oldVal + percentageResult;
                        $(this).val(newVal.toFixed(5));
                    });
                }
                ClosePopup();

            } catch (err) {
                alert(err.message);
            }

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

        function ValidateCustomerSelect() {
            var flag = false;
            var information = "Information";
            var message = "<ul><li>" + "Select Customer" + "</li></ul>";
            $("input[id*='trvCustomersn']").each(function () {
                if (this.checked) {
                    flag = true;
                }
            });

            if (flag) return true;

            ShowErrorMessage(message, information);
            return false;
        }    

    </script>
</asp:Content>
