<%@ Page Title="<%$ Resources:Captions,Title_CustomerRegistration %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true" Theme="ClassicExt" CodeBehind="CustomerRegistration.aspx.cs"
    Inherits="ERPSMS_v01.OrderToCash.CustomerRegistration" %>

<%@ Register Src="~/OrderToCash/UserControls/CustomerRegistrationTabs.ascx" TagName="CustomerRegistrationTabs"
    TagPrefix="uc1" %>
<%@ Register Src="../WorkFlow/WorkflowUserComments.ascx" TagName="WorkflowUserComments"
    TagPrefix="uc2" %>
<%@ Register Src="~/OrderToCash/UserControls/SearchProducts.ascx" TagName="SearchProducts"
    TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="../Scripts/Jquery/ERPTimepicker.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        function SelectedDeleteConfirm(ctrl, gridID) {
            if (gridID) {
                if ($("[id$=" + gridID + "] tr td input[type=radio]:checked").length > 0) {
                    return ShowDeleteConfirm(ctrl);
                }
                else {
                    $("[id$=litErrorMsg]").html('<%= GetLocalResourceObject("msg_select_row").ToString() %>');
                    ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
                    return false;
                }
            }
            else return ShowDeleteConfirm();
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
                $("[id$=pnlSubmit]").hide();

            }
            else if (mode == 2) {

            }
            else if (mode == 3) {
                $("[id$=pnlSubmit]").hide();
            }
        }



        function ValidateText(event, ctlName) {
            var explen = 2;
            var decLen = 2;
            var cntNbr = document.getElementById(ctlName.id).value;
            var isDot = 0;
            if (57 < event.keyCode || event.keyCode < 48)
                event.returnValue = false;
            else {
                for (var i = 0; i <= (cntNbr.length - 1); i++) {
                    if (cntNbr.charAt(i) == ':')
                        isDot = 1;
                }

                if (isDot == 0) {
                    var beforeDec = cntNbr;
                    if (beforeDec.length >= explen) {
                        document.getElementById(ctlName.id).value = cntNbr.substring(0, cntNbr.length - 1);
                    }
                    event.returnValue = true;
                }
                else {
                    var afterDec = (cntNbr.split(':', 2)).pop();
                    afterDec = afterDec.replace('_', '');
                    if (afterDec.length >= decLen) {
                        document.getElementById(ctlName.id).value = cntNbr.substring(0, cntNbr.length - 1);
                        event.returnValue = true;
                    }
                }
            }
            if (event.keyCode == 58) {
                for (var i = 0; i <= (cntNbr.length - 1); i++) {
                    if (cntNbr.charAt(i) == ':')
                        isDot = 1;
                }
                if (isDot == 0)
                    event.returnValue = true;
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
                //CheckValidationDuplicate(valGroup);
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

        //        function ShowDeleteConfirm(btn, message) {
        //            var msgTitle;
        //            var msg;
        //            msgTitle = "<%= Resources.ErpRes.Title_Information %>";
        //            msg = message ? message : "<%= Resources.ErpRes.MsgDeleteConfirm %>";
        //            $("#divConfirmation").html(msg).dialog({
        //                modal: true,
        //                height: 150,
        //                width: 350,
        //                title: msgTitle,
        //                resizable: false,
        //                buttons: {
        //                    OK: function (e) {
        //                        $(this).dialog("close");
        //                        __doPostBack(btn.name, '');
        //                    },
        //                    Cancel: function (e) {
        //                        $(this).dialog("close");
        //                        if (typeof AfterDeleteConfirmationCancel == "function") {
        //                            AfterDeleteConfirmationCancel(btn.id);
        //                        }
        //                        return false;
        //                    }
        //                }
        //            });
        //            return false;
        //        }

        $(document).ready(function () {
        });

        function AfterClose(containerID) {
            if (containerID == "#divSearchProducts") {
                $("[id$=btnClearSearch]").click();
            }
        }

        function NavigateToPackingSpec(packingSpecUrl) {
            window.open(packingSpecUrl, '_blank');
        }

        function FixCustomizedProductAttributesWidth() {
            var field = document.querySelector('textarea[id$="CIM_BRAND_SPECIFICATION"], input[id$="CIM_BRAND_SPECIFICATION"], textarea[name$="$CIM_BRAND_SPECIFICATION"], input[name$="$CIM_BRAND_SPECIFICATION"]');
            var labelForField = null;
            if (!field) {
                var labels = document.querySelectorAll('label, span');
                for (var labelIndex = 0; labelIndex < labels.length; labelIndex++) {
                    if ((labels[labelIndex].textContent || '').replace(/\s+/g, ' ').indexOf('Customized Product Attributes') !== -1) {
                        labelForField = labels[labelIndex];
                        var labelWrapper = labels[labelIndex].parentElement;
                        if (labelWrapper) {
                            field = labelWrapper.querySelector('textarea, input[type="text"]');
                        }
                        if (!field && labels[labelIndex].nextElementSibling) {
                            field = labels[labelIndex].nextElementSibling.matches('textarea, input[type="text"]')
                                ? labels[labelIndex].nextElementSibling
                                : labels[labelIndex].nextElementSibling.querySelector('textarea, input[type="text"]');
                        }
                        if (field) {
                            break;
                        }
                    }
                }
            }
            if (!field) {
                var containers = Array.prototype.slice.call(document.querySelectorAll('td, div')).sort(function (a, b) {
                    return (a.textContent || '').length - (b.textContent || '').length;
                });
                for (var containerIndex = 0; containerIndex < containers.length; containerIndex++) {
                    if ((containers[containerIndex].textContent || '').replace(/\s+/g, ' ').indexOf('Customized Product Attributes') !== -1) {
                        labelForField = containers[containerIndex].querySelector('label, span') || labelForField;
                        field = containers[containerIndex].querySelector('textarea, input[type="text"]');
                        if (!field && containers[containerIndex].nextElementSibling) {
                            field = containers[containerIndex].nextElementSibling.querySelector('textarea, input[type="text"]');
                        }
                        if (field) {
                            break;
                        }
                    }
                }
            }
            if (!field) {
                var captionNodes = Array.prototype.slice.call(document.querySelectorAll('label, span, td, div')).filter(function (node) {
                    return ((node.textContent || '').replace(/\s+/g, ' ').trim() === 'Customized Product Attributes');
                });
                var allTextFields = Array.prototype.slice.call(document.querySelectorAll('textarea, input[type="text"]'));
                for (var captionIndex = 0; captionIndex < captionNodes.length && !field; captionIndex++) {
                    var captionRect = captionNodes[captionIndex].getBoundingClientRect();
                    var bestField = null;
                    var bestDistance = Number.MAX_VALUE;
                    for (var fieldIndex = 0; fieldIndex < allTextFields.length; fieldIndex++) {
                        var candidateRect = allTextFields[fieldIndex].getBoundingClientRect();
                        var sameRowDistance = Math.abs(candidateRect.top - captionRect.top);
                        if (candidateRect.left > captionRect.right && sameRowDistance < 20) {
                            var distance = sameRowDistance + Math.abs(candidateRect.left - captionRect.right);
                            if (distance < bestDistance) {
                                bestDistance = distance;
                                bestField = allTextFields[fieldIndex];
                            }
                        }
                    }
                    if (bestField) {
                        labelForField = captionNodes[captionIndex];
                        field = bestField;
                    }
                }
            }
            if (!field) {
                return;
            }

            field.className = (field.className || '').indexOf('custom-product-attributes') === -1
                ? (field.className + ' custom-product-attributes').replace(/^\s+|\s+$/g, '')
                : field.className;
            field.style.setProperty('display', 'block', 'important');
            field.style.setProperty('width', '100%', 'important');
            field.style.setProperty('min-width', '0', 'important');
            field.style.setProperty('max-width', 'none', 'important');
            field.style.setProperty('box-sizing', 'border-box', 'important');

            if (field.getBoundingClientRect) {
                var formContainer = document.querySelector('[id$="pnlControls"]') || field.closest('.fields-group') || field.closest('table') || document.body;
                var containerRect = formContainer.getBoundingClientRect();
                var fieldRect = field.getBoundingClientRect();
                var availableWidth = Math.max(350, containerRect.right - fieldRect.left - 80);
                field.style.setProperty('width', availableWidth + 'px', 'important');
                field.style.setProperty('min-width', availableWidth + 'px', 'important');
                field.style.setProperty('max-width', availableWidth + 'px', 'important');
            }

            var wrapper = field.parentElement;
            while (wrapper && wrapper.tagName && wrapper.tagName.toLowerCase() !== 'div') {
                wrapper = wrapper.parentElement;
            }
            if (wrapper) {
                wrapper.style.setProperty('display', 'grid', 'important');
                wrapper.style.setProperty('grid-template-columns', '240px minmax(0, 1fr)', 'important');
                wrapper.style.setProperty('column-gap', '10px', 'important');
                wrapper.style.setProperty('align-items', 'center', 'important');
                wrapper.style.setProperty('width', '100%', 'important');
                wrapper.style.setProperty('box-sizing', 'border-box', 'important');

                var label = wrapper.querySelector('label');
                if (label) {
                    label.style.setProperty('width', '240px', 'important');
                    label.style.setProperty('min-width', '240px', 'important');
                    label.style.setProperty('max-width', '240px', 'important');
                    label.style.setProperty('white-space', 'nowrap', 'important');
                    label.style.setProperty('text-align', 'right', 'important');
                    label.style.setProperty('box-sizing', 'border-box', 'important');
                }
            }

            var tableCell = field.closest ? field.closest('td') : null;
            if (tableCell) {
                tableCell.colSpan = Math.max(tableCell.colSpan || 1, 2);
                tableCell.style.setProperty('width', '100%', 'important');
                tableCell.style.setProperty('box-sizing', 'border-box', 'important');
            }

            var tableRow = field.closest ? field.closest('tr') : null;
            if (tableRow && tableCell) {
                for (var i = 0; i < tableRow.cells.length; i++) {
                    if (tableRow.cells[i] !== tableCell && tableRow.cells[i].textContent.replace(/\s+/g, '') === '') {
                        tableRow.cells[i].style.setProperty('display', 'none', 'important');
                    }
                }
            }

            var table = field.closest ? field.closest('table') : null;
            if (table) {
                table.style.setProperty('width', '100%', 'important');
                table.style.setProperty('table-layout', 'fixed', 'important');
            }

            var noteGroup = document.getElementById('GRP_CUST_ATR_1');
            if (noteGroup) {
                var notes = noteGroup.querySelectorAll('label, span');
                for (var n = 0; n < notes.length; n++) {
                    notes[n].style.setProperty('display', 'block', 'important');
                    notes[n].style.setProperty('margin-left', '250px', 'important');
                    notes[n].style.setProperty('width', 'calc(100% - 250px)', 'important');
                    notes[n].style.setProperty('text-align', 'left', 'important');
                    notes[n].style.setProperty('box-sizing', 'border-box', 'important');
                }
            }
        }

        function AlignTermsActiveBelowDescription() {
            var active = document.querySelector('input[id$="TCH_ACTIVE"]');
            var description = document.querySelector('textarea[id$="TCH_DESC"]');
            if (!active || !description) {
                return;
            }

            var activeWrapper = active.closest ? active.closest('.div2col-M, .divcol-M') : null;
            activeWrapper = activeWrapper || active.parentElement;

            var activeCell = activeWrapper.closest ? activeWrapper.closest('td') : null;
            var activeRow = activeWrapper.closest ? activeWrapper.closest('tr') : null;
            var activeTable = activeWrapper.closest ? activeWrapper.closest('table') : null;
            var termsPanel = description.closest ? (description.closest('[id$="pnlControls"]') || description.closest('.fields-group') || document.body) : document.body;
            var panelRect = termsPanel.getBoundingClientRect();
            var descriptionRect = description.getBoundingClientRect();
            var descriptionLabel = document.querySelector('label[for="' + description.id + '"]')
                || (description.closest('.div2col-M, .divcol-M') ? description.closest('.div2col-M, .divcol-M').querySelector('label, span') : null);
            var descriptionLabelRect = descriptionLabel ? descriptionLabel.getBoundingClientRect() : null;
            var labelLeft = descriptionLabelRect ? descriptionLabelRect.left : (descriptionRect.left - 130);
            var labelWidth = Math.max(70, descriptionRect.left - labelLeft - 4);
            var wrapperLeft = Math.max(0, labelLeft - panelRect.left);

            if (activeTable) {
                activeTable.style.setProperty('width', '100%', 'important');
                activeTable.style.setProperty('table-layout', 'fixed', 'important');
            }
            if (activeRow) {
                activeRow.style.setProperty('display', 'block', 'important');
                activeRow.style.setProperty('width', '100%', 'important');
            }
            if (activeCell) {
                activeCell.style.setProperty('display', 'block', 'important');
                activeCell.style.setProperty('width', '100%', 'important');
                activeCell.style.setProperty('box-sizing', 'border-box', 'important');
            }

            activeWrapper.style.setProperty('display', 'grid', 'important');
            activeWrapper.style.setProperty('grid-template-columns', labelWidth + 'px auto', 'important');
            activeWrapper.style.setProperty('column-gap', '7px', 'important');
            activeWrapper.style.setProperty('align-items', 'center', 'important');
            activeWrapper.style.setProperty('width', '100%', 'important');
            activeWrapper.style.setProperty('margin-left', wrapperLeft + 'px', 'important');
            activeWrapper.style.setProperty('box-sizing', 'border-box', 'important');

            var label = activeWrapper.querySelector('label, span');
            if (label) {
                label.style.setProperty('box-sizing', 'border-box', 'important');
                label.style.setProperty('display', 'inline-block', 'important');
                label.style.setProperty('width', '100%', 'important');
                label.style.setProperty('min-width', '0', 'important');
                label.style.setProperty('margin', '0', 'important');
                label.style.setProperty('padding-right', '4px', 'important');
                label.style.setProperty('text-align', 'right', 'important');
            }

            active.style.setProperty('width', 'auto', 'important');
            active.style.setProperty('margin', '0', 'important');
            active.style.setProperty('justify-self', 'start', 'important');
        }

        if (window.$) {
            $(document).ready(FixCustomizedProductAttributesWidth);
            $(document).ready(AlignTermsActiveBelowDescription);
        }
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', FixCustomizedProductAttributesWidth);
            document.addEventListener('DOMContentLoaded', AlignTermsActiveBelowDescription);
        } else {
            FixCustomizedProductAttributesWidth();
            AlignTermsActiveBelowDescription();
        }
        window.addEventListener('load', FixCustomizedProductAttributesWidth);
        window.addEventListener('load', AlignTermsActiveBelowDescription);
        window.setTimeout(FixCustomizedProductAttributesWidth, 0);
        window.setTimeout(FixCustomizedProductAttributesWidth, 500);
        window.setTimeout(FixCustomizedProductAttributesWidth, 1500);
        window.setTimeout(FixCustomizedProductAttributesWidth, 3000);
        window.setTimeout(AlignTermsActiveBelowDescription, 0);
        window.setTimeout(AlignTermsActiveBelowDescription, 500);
        window.setTimeout(AlignTermsActiveBelowDescription, 1500);
        window.setTimeout(AlignTermsActiveBelowDescription, 3000);
        window.setInterval(FixCustomizedProductAttributesWidth, 1000);
        if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(FixCustomizedProductAttributesWidth);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(AlignTermsActiveBelowDescription);
        }
    </script>
    <style type="text/css">
        [id$="pnlControls"] #GRP_BASIC_DTL .fields-group > table,
        [id$="pnlControls"] #GRP_OFF_ADRS .fields-group > table,
        [id$="pnlControls"] #GRP_OTHER .fields-group > table {
            width: 100%;
            table-layout: fixed;
        }

        [id$="pnlControls"] #GRP_BASIC_DTL .fields-group > table td,
        [id$="pnlControls"] #GRP_OFF_ADRS .fields-group > table td,
        [id$="pnlControls"] #GRP_OTHER .fields-group > table td {
            width: 50% !important;
            vertical-align: top;
        }

        [id$="pnlControls"] #GRP_BASIC_NAME .fields-group > table td {
            display: block;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_BASIC_DTL .div2col-M,
        [id$="pnlControls"] #GRP_OFF_ADRS .div2col-M,
        [id$="pnlControls"] #GRP_OTHER .div2col-M {
            display: inline-block !important;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_BASIC_DTL .div2col-M label,
        [id$="pnlControls"] #GRP_OFF_ADRS .div2col-M label,
        [id$="pnlControls"] #GRP_OTHER .div2col-M label {
            width: 30% !important;
            min-width: 130px;
        }

        [id$="pnlControls"] #GRP_BASIC_DTL .div2col-M input[type="text"],
        [id$="pnlControls"] #GRP_OFF_ADRS .div2col-M input[type="text"],
        [id$="pnlControls"] #GRP_OFF_ADRS .div2col-M select,
        [id$="pnlControls"] #GRP_OTHER .div2col-M input[type="text"]:not(.input-w23-6per):not(.input-small):not(.date-picker),
        [id$="pnlControls"] #GRP_OTHER .div2col-M select:not(.select-small-a):not(.select-small-f):not(.select-half) {
            width: min(62%, calc(100% - 145px)) !important;
            min-width: 0 !important;
            max-width: none !important;
        }

        [id$="pnlControls"] #GRP_BASIC_DTL .div2col-M input[type="file"] {
            width: auto !important;
        }

        [id$="pnlControls"] #GRP_OFF_ADRS .div2col-M select {
            width: min(63%, calc(100% - 145px)) !important;
            min-width: 0 !important;
            max-width: none !important;
        }

        [id$="pnlControls"] #GRP_OTHER .div2col-M .select-half {
            width: min(63%, calc(100% - 145px)) !important;
            min-width: 0 !important;
            max-width: none !important;
        }

        [id$="pnlControls"] #GRP_OTHER .div2col-M .select-small-f {
            width: min(40%, 300px) !important;
            min-width: 0 !important;
            max-width: 300px !important;
        }

        [id$="pnlControls"] #GRP_OTHER .div2col-M .select-small-a {
            width: 150px !important;
            min-width: 0 !important;
            max-width: 150px !important;
        }

        [id$="pnlControls"] #GRP_OTHER .div2col-M .input-w23-6per {
            width: 210px !important;
            min-width: 0 !important;
            max-width: 210px !important;
        }

        [id$="pnlControls"] #GRP_OTHER .div2col-M .input-small,
        [id$="pnlControls"] #GRP_OTHER .div2col-M .date-picker {
            width: 150px !important;
            min-width: 0 !important;
            max-width: 150px !important;
        }

        [id$="pnlControls"] #GRP_OTHER .div2col-M input[type="checkbox"] {
            width: auto !important;
            margin-top: 2px;
        }

        [id$="pnlControls"] #GRP_BASIC_NAME .div2col-M label {
            width: 15% !important;
            min-width: 130px;
        }

        [id$="pnlControls"] #GRP_BASIC_NAME .div2col-M {
            display: inline-block !important;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_BASIC_NAME .div2col-M input[type="text"] {
            width: min(81%, calc(100% - 145px)) !important;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY .fields-group > table,
        [id$="pnlControls"] #GRP_CUS_ADRS_OTH .fields-group > table {
            width: 100%;
            table-layout: fixed;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY .fields-group > table td,
        [id$="pnlControls"] #GRP_CUS_ADRS_OTH .fields-group > table td {
            width: 50% !important;
            vertical-align: top;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY .div2col-M,
        [id$="pnlControls"] #GRP_CUS_ADRS_OTH .div2col-M {
            display: inline-block !important;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY .div2col-M label,
        [id$="pnlControls"] #GRP_CUS_ADRS_OTH .div2col-M label {
            width: 30% !important;
            min-width: 130px;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY .div2col-M input[type="text"],
        [id$="pnlControls"] #GRP_CUS_ADRS_OTH .div2col-M input[type="text"],
        [id$="pnlControls"] #GRP_CUS_ADRS_OTH .div2col-M select:not(#CAD_COUNTRY) {
            width: min(62%, calc(100% - 145px)) !important;
            min-width: 0 !important;
            max-width: none !important;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY .div2col-M select,
        [id$="pnlControls"] #GRP_CUS_ADRS_OTH #CAD_COUNTRY {
            width: min(59.5%, calc(100% - 182px)) !important;
            min-width: 0 !important;
            max-width: none !important;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY .fields-group > table tr:has(#CAD_NAME) {
            display: grid;
            grid-template-columns: 50% 50%;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY .fields-group > table tr:has(#CAD_NAME) > td {
            display: block;
            width: auto !important;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY .fields-group > table tr:has(#CAD_NAME) > td:first-child {
            grid-column: 1;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY .fields-group > table tr:has(#CAD_NAME) > td:has(#CAD_NAME) {
            grid-column: 1 / 3;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY .fields-group > table tr:has(#CAD_NAME) > td:has(#CAD_NAME) .div2col-M {
            display: inline-block !important;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY .fields-group > table tr:has(#CAD_NAME) > td:has(#CAD_NAME) .div2col-M label {
            width: 15% !important;
            min-width: 130px;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY #CAD_NAME {
            width: min(80%, calc(100% - 150px)) !important;
            min-width: 0 !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] #GRP_ADRS_ENTRY label[for="CAD_NAME"],
        [id$="pnlControls"] #GRP_ADRS_ENTRY #lblCAD_NAME {
            width: 15% !important;
            min-width: 130px;
        }

        [id$="pnlControls"] #GRP_CUS_ADRS_ADRS .divcol-M {
            display: inline-block !important;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_CUS_ADRS_ADRS .divcol-M label {
            width: 15% !important;
            min-width: 130px;
        }

        [id$="pnlControls"] #GRP_CUS_ADRS_ADRS .divcol-M textarea {
            width: min(77%, calc(100% - 200px)) !important;
            min-width: 0 !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] #GRP_CUS_ADRS_OTH #CAD_ACTIVE,
        [id$="pnlControls"] #GRP_CUS_ADRS_OTH input[type="checkbox"] {
            width: auto !important;
            margin-top: 2px;
        }

        [id$="pnlControls"] #GRP_ADRS_LIST .fields-group > table:first-child td {
            display: block;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_ADRS_LIST .fields-group > table:first-child td:empty {
            display: none;
        }

        [id$="pnlControls"] #GRP_ADRS_LIST .fields-group > table:first-child .div2col-M input[type="text"] {
            width: min(77%, calc(100% - 200px)) !important;
            min-width: 0 !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] #GRP_ADRS_LIST .fields-group > table:first-child .div2col-M label {
            width: 15% !important;
            min-width: 130px;
        }

        [id$="pnlControls"] #GRP_TC_NAME .div2col-M label,
        [id$="pnlControls"] #GRP_TC_ENTRY .div2col-M label {
            width: 30% !important;
            min-width: 130px;
        }

        [id$="pnlControls"] #GRP_TC_NAME .div2col-M input[type="text"]:not(.input-halfsmall-a):not(.input-w23-6per):not(.input-small):not(.date-picker),
        [id$="pnlControls"] #GRP_TC_ENTRY .div2col-M input[type="text"]:not(.input-halfsmall-a):not(.input-w23-6per):not(.input-small):not(.date-picker) {
            width: min(62%, calc(100% - 145px)) !important;
        }

        [id$="pnlControls"] #GRP_TC_NAME .div2col-M select:not(.select-small-a):not(.select-small-f):not(.select-half),
        [id$="pnlControls"] #GRP_TC_ENTRY .div2col-M select:not(.select-small-a):not(.select-small-f):not(.select-half) {
            width: min(63.4%, calc(100% - 145px)) !important;
        }

        [id$="pnlControls"] #GRP_TC_NAME #TCH_DESC {
            width: min(80%, calc(100% - 145px)) !important;
        }

        [id$="pnlControls"] #GRP_TC_NAME label[for="TCH_DESC"],
        [id$="pnlControls"] #GRP_TC_NAME #lblTCH_DESC {
            width: 15% !important;
            min-width: 130px;
        }

        [id$="pnlControls"] #GRP_TC_DESC .fields-group > table td,
        [id$="pnlControls"] [id$="GRP_TC_DESC"] .fields-group > table td {
            display: block;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_TC_DESC .div2col-M,
        [id$="pnlControls"] [id$="GRP_TC_DESC"] .div2col-M {
            display: grid !important;
            grid-template-columns: 15% auto;
            align-items: center;
            width: 100% !important;
            margin-top: 0;
        }

        [id$="pnlControls"] #GRP_TC_DESC .div2col-M label,
        [id$="pnlControls"] [id$="GRP_TC_DESC"] .div2col-M label {
            box-sizing: border-box;
            width: 100% !important;
            min-width: 130px;
            padding-right: 4px;
            text-align: right;
        }

        [id$="pnlControls"] #GRP_TC_DESC .div2col-M:has(input[id$="TCH_ACTIVE"]),
        [id$="pnlControls"] [id$="GRP_TC_DESC"] .div2col-M:has(input[id$="TCH_ACTIVE"]) {
            display: grid !important;
            grid-template-columns: minmax(130px, 15%) auto !important;
            column-gap: 7px !important;
            align-items: center !important;
            width: 100% !important;
            margin-left: 0 !important;
            white-space: nowrap;
        }

        [id$="pnlControls"] #GRP_TC_DESC .div2col-M:has(input[id$="TCH_ACTIVE"]) label,
        [id$="pnlControls"] [id$="GRP_TC_DESC"] .div2col-M:has(input[id$="TCH_ACTIVE"]) label {
            display: inline-block !important;
            width: 100% !important;
            min-width: 0 !important;
            margin: 0 !important;
            padding-right: 4px !important;
            text-align: right !important;
            line-height: 18px;
        }

        [id$="pnlControls"] #GRP_TC_DESC #TCH_ACTIVE,
        [id$="pnlControls"] #GRP_TC_DESC input[type="checkbox"],
        [id$="pnlControls"] [id$="GRP_TC_DESC"] input[id$="TCH_ACTIVE"],
        [id$="pnlControls"] [id$="GRP_TC_DESC"] input[type="checkbox"] {
            width: auto !important;
            justify-self: start;
            margin: 0 !important;
            display: inline-block !important;
            vertical-align: middle;
        }

        [id$="pnlControls"] #GRP_TC_NAME #TCH_TYPE,
        [id$="pnlControls"] #GRP_TC_ENTRY #TCD_BASED_ON,
        [id$="pnlControls"] #GRP_TC_ENTRY #TCD_CONDITION {
            width: min(63.4%, calc(100% - 145px)) !important;
        }

        [id$="pnlControls"] #GRP_TC_ENTRY #TCD_PERCENTAGE,
        [id$="pnlControls"] #GRP_TC_ENTRY #TCD_DAYS {
            max-width: 120px;
        }

        [id$="pnlControls"] #GRP_BRAND_ENTRY .div2col-M label,
        [id$="pnlControls"] #GRP_BRAND_DESC .div2col-M label,
        [id$="pnlControls"] #GRP_PRD_ATRBT .div2col-M label,
        [id$="pnlControls"] #GRP_PACK_SPEC .div2col-M label,
        [id$="pnlControls"] #GRP_ADDNL_PACK_SPEC .div2col-M label {
            width: 30% !important;
            min-width: 130px;
        }

        [id$="pnlControls"] #GRP_BRAND_NAME .divcol-M label,
        [id$="pnlControls"] #GRP_BRAND_NAME .div2col-M label,
        [id$="pnlControls"] #GRP_IGPL_PRODUCT .divcol-M label,
        [id$="pnlControls"] #GRP_IGPL_PRODUCT .div2col-M label,
        [id$="pnlControls"] #GRP_CUST_ATR_2 .divcol-M0 label,
        [id$="pnlControls"] #GRP_CUST_ATR_2 .div2col-M label,
        [id$="pnlControls"] #GRP_PACK_SPEC_OTH .div2col-M label {
            width: 15% !important;
            min-width: 130px;
        }

        [id$="pnlControls"] #GRP_BRAND_DESC label[for="CIM_DESC"],
        [id$="pnlControls"] #GRP_BRAND_DESC #lblCIM_DESC {
            width: 15% !important;
            min-width: 130px;
        }

        [id$="pnlControls"] #GRP_BRAND_NAME .div2col-M,
        [id$="pnlControls"] #GRP_IGPL_PRODUCT .div2col-M,
        [id$="pnlControls"] #GRP_CUST_ATR_2 .divcol-M0,
        [id$="pnlControls"] #GRP_CUST_ATR_2 .div2col-M,
        [id$="pnlControls"] #GRP_PACK_SPEC_OTH .div2col-M {
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_CUST_ATR_2 .fields-group > table,
        [id$="pnlControls"] #GRP_CUST_ATR_2 .fields-group > table tr,
        [id$="pnlControls"] #GRP_CUST_ATR_2 .fields-group > table td {
            display: block;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_CUST_ATR_1 .fields-group > table,
        [id$="pnlControls"] #GRP_CUST_ATR_1 .fields-group > table tr,
        [id$="pnlControls"] #GRP_CUST_ATR_1 .fields-group > table td {
            display: block;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_CUST_ATR_1 .div2col-M,
        [id$="pnlControls"] #GRP_CUST_ATR_1 .divcol-M {
            display: block !important;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_CUST_ATR_1 label,
        [id$="pnlControls"] #GRP_CUST_ATR_1 span {
            display: block;
            margin-left: 250px;
            width: calc(100% - 250px) !important;
            text-align: left !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] [id$="GRP_CUST_ATR_1"] label,
        [id$="pnlControls"] [id$="GRP_CUST_ATR_1"] span {
            display: block;
            margin-left: 250px;
            width: calc(100% - 250px) !important;
            text-align: left !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] #GRP_CUST_ATR_2 .divcol-M0,
        [id$="pnlControls"] #GRP_CUST_ATR_2 .div2col-M {
            display: grid !important;
            grid-template-columns: 240px minmax(0, 1fr);
            column-gap: 10px;
            align-items: start;
            width: 100% !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .fields-group > table,
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .fields-group > table tr,
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .fields-group > table td {
            display: block;
            width: 100% !important;
        }

        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .divcol-M0,
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .div2col-M {
            display: grid !important;
            grid-template-columns: 240px minmax(0, 1fr);
            column-gap: 10px;
            align-items: center;
            width: 100% !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] #GRP_IGPL_PRODUCT .fields-group > table td {
            display: block;
            width: 100% !important;
        }

        [id$="pnlControls"] #GRP_IGPL_PRODUCT .fields-group > table td:empty {
            display: none;
        }

        [id$="pnlControls"] #GRP_CUST_ATR_2 .divcol-M0 label,
        [id$="pnlControls"] #GRP_CUST_ATR_2 .div2col-M label {
            display: block;
            width: 240px !important;
            min-width: 240px;
            margin-bottom: 0;
            box-sizing: border-box;
            text-align: right !important;
            white-space: nowrap;
        }

        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .divcol-M0 label,
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .div2col-M label {
            display: block;
            width: 240px !important;
            min-width: 240px !important;
            max-width: 240px !important;
            margin-bottom: 0;
            box-sizing: border-box;
            text-align: right !important;
            white-space: nowrap;
        }

        [id$="pnlControls"] #GRP_PACK_SPEC_OTH .div2col-M label {
            width: 240px !important;
            min-width: 240px;
        }

        [id$="pnlControls"] #GRP_BRAND_ENTRY .div2col-M input[type="text"],
        [id$="pnlControls"] #GRP_BRAND_DESC .div2col-M input[type="text"],
        [id$="pnlControls"] #GRP_PACK_SPEC .div2col-M input[type="text"],
        [id$="pnlControls"] #GRP_ADDNL_PACK_SPEC .div2col-M input[type="text"] {
            width: min(59.4%, calc(100% - 145px)) !important;
        }

        [id$="pnlControls"] #GRP_BRAND_ENTRY .div2col-M select,
        [id$="pnlControls"] #GRP_BRAND_DESC .div2col-M select,
        [id$="pnlControls"] #GRP_PACK_SPEC .div2col-M select {
            width: min(59.4%, calc(100% - 145px)) !important;
        }

        [id$="pnlControls"] #GRP_PRD_ATRBT .div2col-M select,
        [id$="pnlControls"] #GRP_ADDNL_PACK_SPEC .div2col-M select {
            width: min(63.4%, calc(100% - 145px)) !important;
        }

        [id$="pnlControls"] #GRP_BRAND_NAME #CIM_BRAND_NAME,
        [id$="pnlControls"] #GRP_BRAND_DESC #CIM_DESC {
            width: min(80%, calc(100% - 145px)) !important;
            min-width: 0 !important;
            max-width: none !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] #GRP_IGPL_PRODUCT #CIM_ITEM {
            width: min(80%, calc(100% - 145px)) !important;
            min-width: 0 !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] #GRP_CUST_ATR_2 #CIM_BRAND_SPECIFICATION,
        [id$="pnlControls"] #GRP_PACK_SPEC_OTH #CIM_PACKING_SPEC_DESC {
            width: calc(100% - 265px) !important;
        }

        [id$="pnlControls"] #GRP_PACK_SPEC_OTH #CIM_PACKING_SPEC_DESC {
            width: calc(100% - 335px) !important;
            min-width: 0 !important;
            max-width: none !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] #GRP_CUST_ATR_2 #CIM_BRAND_SPECIFICATION {
            display: block;
            width: 100% !important;
            min-width: 0 !important;
            max-width: none !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] #GRP_CUST_ATR_2 textarea#CIM_BRAND_SPECIFICATION,
        [id$="pnlControls"] #GRP_CUST_ATR_2 input#CIM_BRAND_SPECIFICATION,
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] textarea[id$="CIM_BRAND_SPECIFICATION"],
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] input[id$="CIM_BRAND_SPECIFICATION"],
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .custom-product-attributes {
            display: block !important;
            width: 100% !important;
            min-width: 0 !important;
            max-width: none !important;
            box-sizing: border-box !important;
        }

        [id$="pnlControls"] #GRP_CUST_ATR_2 .divcol-M0 input[type="text"],
        [id$="pnlControls"] #GRP_CUST_ATR_2 .divcol-M0 textarea,
        [id$="pnlControls"] #GRP_CUST_ATR_2 .div2col-M input[type="text"],
        [id$="pnlControls"] #GRP_CUST_ATR_2 .div2col-M textarea,
        [id$="pnlControls"] #GRP_CUST_ATR_2 input[type="text"],
        [id$="pnlControls"] #GRP_CUST_ATR_2 textarea,
        [id$="pnlControls"] #GRP_CUST_ATR_2 [id$="CIM_BRAND_SPECIFICATION"] {
            display: block;
            width: 100% !important;
            min-width: 0 !important;
            max-width: none !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .divcol-M0 input[type="text"],
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .divcol-M0 textarea,
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .div2col-M input[type="text"],
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .div2col-M textarea,
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] input[type="text"],
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] textarea,
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] [id$="CIM_BRAND_SPECIFICATION"],
        [id$="pnlControls"] [id$="GRP_CUST_ATR_2"] .custom-product-attributes {
            display: block;
            width: 100% !important;
            min-width: 0 !important;
            max-width: none !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] #GRP_IGPL_PRODUCT #CIM_ITEM {
            width: min(80%, calc(100% - 145px)) !important;
            min-width: 0 !important;
            max-width: none !important;
            box-sizing: border-box;
        }

        [id$="pnlControls"] #GRP_BRAND_DESC #CIM_SALE_UOM,
        [id$="pnlControls"] #GRP_BRAND_DESC #CIM_BRAND_GROUP,
        [id$="pnlControls"] #GRP_PACK_SPEC #CIM_PACKING_TYPE {
            width: min(59.4%, calc(100% - 145px)) !important;
        }

        [id$="pnlControls"] #GRP_PACK_SPEC #CIM_PACKING_SPEC {
            width: calc(63.4% - 60px) !important;
            max-width: calc(63.4% - 60px) !important;
        }

        @media (max-width: 1100px) {
            [id$="pnlControls"] #GRP_PACK_SPEC #CIM_PACKING_SPEC {
                width: 185px !important;
                max-width: 185px !important;
            }
        }

        [id$="pnlControls"] #GRP_BRAND_DESC #CIM_STERILIZED,
        [id$="pnlControls"] #GRP_BRAND_DESC #CIM_ACTIVE,
        [id$="pnlControls"] #GRP_PACK_SPEC #CIM_IS_NEW_SPEC,
        [id$="pnlControls"] #GRP_ADDNL_PACK_SPEC #CIM_STRAPPING,
        [id$="pnlControls"] #GRP_ADDNL_PACK_SPEC #CIM_LAYERING,
        [id$="pnlControls"] #GRP_BRAND_DESC input[type="checkbox"],
        [id$="pnlControls"] #GRP_PACK_SPEC input[type="checkbox"],
        [id$="pnlControls"] #GRP_ADDNL_PACK_SPEC input[type="checkbox"] {
            width: auto !important;
            margin-top: 2px;
        }

        [id$="pnlControls"] #GRP_BRAND_DESC #CIM_BRAND_PROD_QTY,
        [id$="pnlControls"] #GRP_BRAND_DESC #CIM_SALE_UOM_CONV,
        [id$="pnlControls"] #GRP_PACK_SPEC #CIM_TOTAL_PCS,
        [id$="pnlControls"] #GRP_ADDNL_PACK_SPEC #CIM_NO_LAYERS,
        [id$="pnlControls"] #GRP_ADDNL_PACK_SPEC #CIM_PIECES_LAYER {
            max-width: 120px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlCustomerRegistration">
        <ContentTemplate>
            <div class="fixed-buttons" id="divFixedTab">
                <div class="Button-container">
                    <asp:Table ID="Table1" runat="server">
                        <asp:TableRow>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul class="bredcrum">
                                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <%--Dynamic Tab User Control--%>
                <uc1:CustomerRegistrationTabs ID="CustomerRegistrationTabs" runat="server" />
            </div>
            <div class="content-wrapper">
                <asp:HiddenField ID="hdfCustomerPK" runat="server" />
                <asp:HiddenField ID="hdfSubTabValue" runat="server" />
                <asp:HiddenField ID="hdfSubtab" runat="server" />
                <asp:HiddenField ID="hdfIsNew" runat="server" Value="1" />
                <asp:Table runat="server" ID="tblTemplate" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <asp:Panel ID="pnlControls" runat="server">
                                <%--The UI controls will bind here--%>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label>
                    <div id="divValidationSummary" runat="server">
                        <%--The ValidationSummary controls will bind here--%>
                    </div>
                </div>
            </div>
            <div id="divSearchProducts" style="display: none;">
                <uc3:SearchProducts ID="ucrSearchProducts" runat="server" />
            </div>
            <asp:Button ID="btnClearSearch" runat="server" OnClick="ActionHandler" CommandName="CLEARSEARCH"
                EnableTheming="false" Style="display: none" />
            <div id="divWkfSubmit" style="display: none;">
                <asp:HiddenField ID="hdfProcessID" Value="0" runat="server" />
                <uc2:WorkflowUserComments ID="ucrWrkf" runat="server" />
            </div>
            <div id="divPackingSpec" style="display: none;">
                <div class="Button-container-popup">
                    <asp:Button ID="btnPackingSpecApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" CssClass="margn-rgt4"
                        OnClick="ActionHandler" CommandArgument="PageAction_Entry" CommandName="APPLYPACKSPEC" />
                    <div class="content-wrapper" style="margin-top: 13px;">
                        <div class="div2col-P">
                            <label for="ChooseTax" style="width: 105px!important;">
                                <%=GetLocalResourceObject("NewPackingSpec").ToString()%>
                            </label>
                            <asp:DropDownList ID="ddlPackingSpec" runat="server" Width="300px">
                            </asp:DropDownList>
                            <asp:ImageButton ID="imbPackingSpecEdit" runat="server" SkinID="imbeditgrid" OnClick="ActionHandler"
                                CommandName="EDITITEM" />
                        </div>
                    </div>
                </div>
            </div>
            <div id="divItemTax" style="display: none">
                <asp:HiddenField ID="hdfPopUpAssociatedControl" runat="server" Value="" />
                <asp:HiddenField ID="hdfQry" runat="server" Value="" />
                <div class="Button-container-popup">
                    <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClick="ActionHandler"
                        CommandArgument="PageAction_Entry" CommandName="TAXAPPLY" />
                </div>
                <div class="content-wrapper">
                    <div class="div2col-P">
                        <label for="ChooseTax" style="width: 105px!important;">
                            <%=Resources.Controls.ChooseType%>
                        </label>
                        <asp:DropDownList ID="ddlPopupTaxType" runat="server">
                        </asp:DropDownList>
                        <asp:ImageButton ID="imbTaxDiscountSave" SkinID="imbaddnew" runat="server" TabIndex="15"
                            ToolTip="Add" OnClick="ActionHandler" CommandArgument="PageAction_Entry" CommandName="TAXADD" />
                        <%--OnClientClick="javascript:ValidatePageNow('tax')" ValidationGroup="tax"--%>
                    </div>
                    <div class="gridwrap">
                        <asp:GridView runat="server" ID="grdTaxDetails" Width="100%" AllowSorting="false"
                            AutoGenerateColumns="false" TabIndex="106" EmptyDataRowStyle-CssClass="emptytable"
                            PageSize="<%$ resources:PageSize%>">
                            <EmptyDataTemplate>
                                <asp:Label ID="lblEmpty" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>"></asp:Label>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ resources:TaxType %>">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdfTaxPK" runat="server" Value='<%#Eval("CMT_PK") %>' />
                                        <asp:HiddenField ID="hdfCustPK" runat="server" Value='<%#Eval("CMT_CUSTOMER") %>' />
                                        <asp:HiddenField ID="hdfCusItmPK" runat="server" Value='<%#Eval("CMT_CUST_ITEM") %>' />
                                        <asp:HiddenField ID="hdfTaxCategory" runat="server" Value='<%#Eval("CMT_TAX") %>' />
                                        <asp:Label ID="lblTaxText" runat="server" Text='<%# Convert.ToString(Eval("CMT_NAME")) == string.Empty ? Resources.Report.Custom : Convert.ToString(Eval("CMT_NAME")) %>'>
                                        </asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="92%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ resources:Action %>">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbTaxRemove" runat="server" OnClick="ActionHandler" CommandName="TAXDELETE"
                                            CommandArgument="PageAction_Entry" TabIndex="53" SkinID="btnclose" ToolTip="Remove" />
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnAttach" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
