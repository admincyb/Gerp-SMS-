<%@ Page Title="<%$ Resources:Captions,Title_PackingMasterMapping %>" Language="C#"
    Theme="ClassicExt" EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="PackingSpecMapping.aspx.cs" Inherits="ERPSMS_v01.Inventory.Masters.PackingSpecMapping"
    ValidateRequest="false" %>

<%@ Register Src="~/UserControls/PgerControlNew.ascx" TagName="PagerControl" TagPrefix="uc1" %>
<asp:Content ID="cntScript" runat="server" ContentPlaceHolderID="head">
    <script type="text/javascript">
        var pageURL = window.document.URL;
        var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>';
        // var urlauto = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        var urlauto = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");

        //        //************************ For Autocomplete Bug Fixing ************************
        //        $.widget("ui.ExtAutocomplete", $.extend({}, $.ui.autocomplete.prototype, {
        //            ValidationLabel: $(this).val(),
        //            _create: function () {

        //                // call autocomplete's default create method:
        //                $.ui.autocomplete.prototype._create.apply(this, arguments);

        //                // this.element is the element the widget was invoked with
        //                this.element.bind("focusout", this._focusout);
        //            },
        //            _focusout: function (event, ui) {
        //                // Code to be executed upon every focusout.
        //                this.ValidationLabel = $(this).val();
        //                if ($(this).ExtAutocomplete('option') && $(this).ExtAutocomplete('option').focusout
        //                && typeof ($(this).ExtAutocomplete('option').focusout) == "function") {
        //                    $(this).ExtAutocomplete('option').focusout(event, ui);
        //                }
        //            },
        //            _trigger: function (type, event, data) {
        //                if (type == "select") {
        //                    this.ValidationLabel = $(this).val();
        //                    $.Widget.prototype._trigger.apply(this, ["select", event, data]);
        //                    return false;
        //                }
        //                else if (type == "change") {
        //                    this.ValidationLabel = $(this).val();
        //                    $.Widget.prototype._trigger.apply(this, ["change", event, data]);
        //                    return false;
        //                }
        //                else {
        //                    return $.Widget.prototype._trigger.apply(this, arguments);
        //                }
        //            }
        //        }));


        //        function MakeExtAutoCompleteDDL(targetControlID, url, targetHiddenID, makeCombo, setDropDown, searchType, className, needSingleBinding, needDefaultBinding) {
        //            ///<summary>
        //            /// To make the autocomplete functionality
        //            ///</summary>
        //            /// <param name="targetControlID" optional="true" type="String">
        //            ///     Autocomplete TextBox ID
        //            /// </param>
        //            /// <param name="url" optional="true" type="String">
        //            ///     URL - to which the ajax request is made
        //            /// </param>
        //            /// <param name="targetHiddenID" optional="true" type="String">
        //            ///     Set The Selected Value ID
        //            /// </param>
        //            /// <param name="makeCombo" optional="true" type="Bool">
        //            ///     To Make the Autocomplete Text Box as a Combo
        //            /// </param>
        //            /// <param name="setDropDown" optional="true" type="Bool">
        //            ///     when true will act like dropdown id when search for a not containing value will set to select.
        //            /// </param> 

        //            $("[id$=" + targetControlID + "]").ExtAutocomplete({
        //                search: function () { $(this).addClass("ui-autocomplete-loading"); },
        //                open: function () {
        //                    $(this).removeClass("ui-autocomplete-loading");
        //                    var autowidget = $("[id$=" + targetControlID + "]").ExtAutocomplete("widget")[0];
        //                    if (parseFloat($(autowidget).css("max-width").replace("px", "")) < parseFloat($(this)[0].clientWidth)) {
        //                        $(autowidget).removeClass("ui-menu");
        //                        $(autowidget).addClass("ui-menu-large");
        //                        $(autowidget).css({ "max-width": $(this)[0].clientWidth + "px!important" });
        //                    }
        //                },
        //                source: function (request, response) {
        //                    $.ajax({
        //                        type: "POST",
        //                        url: url,
        //                        dataType: "json",
        //                        data: {
        //                            SearchValue: request.term,
        //                            SearchType: searchType
        //                        },
        //                        success: function (data) {
        //                            response($.map(data, function (item) {
        //                                return {
        //                                    label: item.Text, // format the the data as text 
        //                                    id: item.Value
        //                                }
        //                            }));
        //                        }
        //                    });
        //                },
        //                cache: false,
        //                select: function (event, ui) {
        //                    $("[id$=" + targetControlID + "]").val(ui.item.label);
        //                    $("[id$=" + targetHiddenID + "]").val(ui.item.id);

        //                    if (typeof AfterAutoCompleteSelect == 'function') {

        //                        // if any more function want to done after the result is selected from auto complete
        //                        AfterAutoCompleteSelect(targetControlID);
        //                    }
        //                },
        //                focusout: function (event, ui) {
        //                    if ($("[id$=" + targetControlID + "]").ExtAutocomplete()[0].ValidationLabel != $("[id$=" + targetControlID + "]").val()) {
        //                        $("[id$=" + targetControlID + "]").val("Select/Type");
        //                        $("[id$=" + targetHiddenID + "]").val(0);
        //                    }
        //                },
        //                change: function (event, ui) {
        //                    if (!ui.item) {
        //                        var matcher = new RegExp("^" + $.ui.ExtAutocomplete.escapeRegex($(this).val()) + "$", "i"),
        //					valid = false;
        //                        if (setDropDown) {
        //                            $(this).children("option").each(function () {
        //                                if ($(this).text().match(matcher)) {
        //                                    this.selected = valid = true;
        //                                    return false;
        //                                }
        //                            });
        //                            if (!valid) {
        //                                // remove invalid value, as it didn't match anything
        //                                $(this).val("Select/Type");
        //                                $("[id$=" + targetHiddenID + "]").val(0);
        //                                $(this).data("autocomplete").term = "";
        //                                if (typeof AfterInvalidSelect == 'function') { // if any more function want to done after the result is selected from auto complete
        //                                    AfterInvalidSelect(targetControlID);
        //                                }
        //                                return false;
        //                            }
        //                        }
        //                    }
        //                }
        //            });
        //            if (!className) {
        //                className = "ddlSelect";
        //            }
        //            if (makeCombo) {
        //                if (setDropDown) {
        //                    if ($("[id$=" + targetControlID + "]").val() == "") {
        //                        $("[id$=" + targetControlID + "]").val("Select/Type");
        //                    }
        //                }
        //                $("[id$=" + targetControlID + "]").next("a").remove();
        //                this.anchor = $("<a>")
        //					.attr("tabIndex", -1)
        //					.attr("title", "Show All Items")
        //					.insertAfter($("[id$=" + targetControlID + "]"))
        //                //.removeClass("ui-corner-all")
        //					.addClass(className)
        //					.click(function () {
        //					    // close if already visible
        //					    if ($("[id$=" + targetControlID + "]").ExtAutocomplete("widget").is(":visible")) {
        //					        $("[id$=" + targetControlID + "]").ExtAutocomplete("close");
        //					        return;
        //					    }

        //					    // pass empty string as value to search for, displaying all results
        //					    $("[id$=" + targetControlID + "]").ExtAutocomplete("search", " ");
        //					    $("[id$=" + targetControlID + "]").focus();
        //					});

        //            }
        //            $("[id$=" + targetControlID + "]").click(function () {
        //                $(this).select();
        //            });
        //            if (needSingleBinding) {
        //                $.ajax({
        //                    type: "POST",
        //                    url: url,
        //                    dataType: "json",
        //                    data: {
        //                        SearchValue: "",
        //                        SearchType: searchType
        //                    },
        //                    success: function (data) {
        //                        if (data != null && data.length == 1) {
        //                            $("[id$=" + targetControlID + "]").val(data[0].Text);
        //                            $("[id$=" + targetHiddenID + "]").val(data[0].Value);
        //                        }
        //                    }
        //                });
        //            }
        //            else if (needDefaultBinding) {
        //                $.ajax({
        //                    type: "POST",
        //                    url: url,
        //                    dataType: "json",
        //                    data: {
        //                        SearchValue: "",
        //                        SearchType: searchType
        //                    },
        //                    success: function (data) {
        //                        if (data != null && data.length > 1) {
        //                            $("[id$=" + targetControlID + "]").val(data[0].Text);
        //                            $("[id$=" + targetHiddenID + "]").val(data[0].Value);
        //                        }
        //                    }
        //                });
        //            }
        //        }
        //        //************************ For Autocomplete Bug Fixing ************************

        function InitComponents() {
            $("[id$='lblValidCustomer']").hide();
            $("[id$='lblValidBrand']").hide();
            $("[id$='lblValidArtversion']").hide();
            //GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", urlauto, "hdfCustomer", true, true, "CUSTOMER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtCustomerID", urlauto, "hdfCustomerID", true, true, "CUSTOMER");
            GrandScriptUtils.MakeAutoCompleteDDL("txtBrandID", urlauto + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrandID", true, true, "CUTOMERBRAND");

            //            if ($("[id$=hdfCustomer]").val() == "" || $("[id$=hdfCustomer]").val() == "0") {
            //                DisableAuto($("[id$=txtBrand]"), $("[id$=hdfBrand]"));
            //            }
            if ($("[id$=hdfCustomerID]").val() == "" || $("[id$=hdfCustomerID]").val() == "0") {
                DisableAuto($("[id$=txtBrandID]"), $("[id$=hdfBrandID]"));
            }
            DisableStatus();
            setPackingMatAuto();
            Disableautocomplete();
        }

        function AfterAutoCompleteSelect(targetControlID) {           
            if (targetControlID == "txtCustomerID") {
                $("[id$=hdfBrandID]").val("0");
                $("[id$=txtBrandID]").val("<%= Resources.ErpRes.AutoDefaultValue %>");
                if ($("[id$=hdfCustomerID]").val() != "" && $("[id$=hdfCustomerID]").val() != "0") {
                    EnableAuto($("[id$=txtBrandID]"));
                    GrandScriptUtils.MakeAutoCompleteDDL("txtBrandID", urlauto + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrandID", true, true, "CUTOMERBRAND");
                    //                    MakeExtAutoCompleteDDL("txtBrandID", urlauto + "?Type=" + $("[id$=hdfCustomerID]").val(), "hdfBrandID", true, true, "CUTOMERBRAND");
                    if ($("[id$=hdfCustomerID]").val() == "" || $("[id$=hdfCustomerID]").val() == "0") {
                        DisableAuto($("[id$=txtBrandID]"), $("[id$=hdfBrandID]"));
                    }
                }
                else {
                    DisableAuto($("[id$=txtBrandID]"), $("[id$=hdfBrandID]"));
                }

            }
            if (targetControlID == "txtPouchPack" || targetControlID == "txtInnerBox" || targetControlID == "txtMiniInnerCarton" || targetControlID == "txtZipperBag" || targetControlID == "txtMasterCarton"
                || targetControlID == "txtSackBag" || targetControlID == "txtWallet" || targetControlID == "txtPOB" || targetControlID == "txtPolybag") {
                $("[id$=hdfSelectedTextboxId]").val(targetControlID);
                $("[id$=btnDummySelChang]").click();
            }
        }
        function AfterInvalidSelect(targetControlID) {
            if (targetControlID == "txtPouchPack") {
                $("[id$=hdfAutoPouchPack]").val("-1");
                $("[id$=hdfSelectedTextboxId]").val(targetControlID);
                $("[id$=btnDummySelChang]").click();
            }
            if (targetControlID == "txtInnerBox") {
                $("[id$=hdfAutoInnerBox]").val("-1");
                $("[id$=hdfSelectedTextboxId]").val(targetControlID);
                $("[id$=btnDummySelChang]").click();
            }
            if (targetControlID == "txtMiniInnerCarton") {
                $("[id$=hdfAutoMiniInnerCarton]").val("-1");
                $("[id$=hdfSelectedTextboxId]").val(targetControlID);
                $("[id$=btnDummySelChang]").click();
            }
            if (targetControlID == "txtZipperBag") {
                $("[id$=hdfAutoZipperBag]").val("-1");
                $("[id$=hdfSelectedTextboxId]").val(targetControlID);
                $("[id$=btnDummySelChang]").click();
            }
            if (targetControlID == "txtMasterCarton") {
                $("[id$=hdfAutoMasterCarton]").val("-1");
                $("[id$=hdfSelectedTextboxId]").val(targetControlID);
                $("[id$=btnDummySelChang]").click();
            }
            if (targetControlID == "txtSackBag") {
                $("[id$=hdfAutoSackBag]").val("-1");
                $("[id$=hdfSelectedTextboxId]").val(targetControlID);
                $("[id$=btnDummySelChang]").click();
            }
            if (targetControlID == "txtWallet") {
                $("[id$=hdfAutoWallet]").val("-1");
                $("[id$=hdfSelectedTextboxId]").val(targetControlID);
                $("[id$=btnDummySelChang]").click();
            }
            if (targetControlID == "txtPOB") {
                $("[id$=hdfAutoPOB]").val("-1");
                $("[id$=hdfSelectedTextboxId]").val(targetControlID);
                $("[id$=btnDummySelChang]").click();
            }
            if (targetControlID == "txtPolybag") {
                $("[id$=hdfAutoPolyBag]").val("-1");
                $("[id$=hdfSelectedTextboxId]").val(targetControlID);
                $("[id$=btnDummySelChang]").click();
            }
        }
        function Disableautocomplete() {
            if ($("[id$=txtPouchPack]").attr("disabled") == true) {
                DisableAuto($("[id$=txtPouchPack]"), $("[id$=hdfAutoPouchPack]"));
            }
            if ($("[id$=txtInnerBox]").attr("disabled") == true) {
                DisableAuto($("[id$=txtInnerBox]"), $("[id$=hdfAutoInnerBox]"));
            }
            if ($("[id$=txtMiniInnerCarton]").attr("disabled") == true) {
                DisableAuto($("[id$=txtMiniInnerCarton]"), $("[id$=hdfAutoMiniInnerCarton]"));
            }
            if ($("[id$=txtZipperBag]").attr("disabled") == true) {
                DisableAuto($("[id$=txtZipperBag]"), $("[id$=hdfAutoZipperBag]"));
            }
            if ($("[id$=txtMasterCarton]").attr("disabled") == true) {
                DisableAuto($("[id$=txtMasterCarton]"), $("[id$=hdfAutoMasterCarton]"));
            }
            if ($("[id$=txtSackBag]").attr("disabled") == true) {
                DisableAuto($("[id$=txtSackBag]"), $("[id$=hdfAutoSackBag]"));
            }
            if ($("[id$=txtWallet]").attr("disabled") == true) {
                DisableAuto($("[id$=txtWallet]"), $("[id$=hdfAutoWallet]"));
            }
            if ($("[id$=txtPOB]").attr("disabled") == true) {
                DisableAuto($("[id$=txtPOB]"), $("[id$=hdfAutoPOB]"));
            }
            if ($("[id$=txtPolybag]").attr("disabled") == true) {
                DisableAuto($("[id$=txtPolybag]"), $("[id$=hdfAutoPolybag]"));
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
            //            if (tab == 1) {
            //                $("[id$='spnPackingListing']").removeClass("tab-inactive").addClass("tab-active");
            //                $("[id$='lbnPackingListing']").removeClass("tab-inactive").addClass("tab-active");
            //                $("[id$='spnPackingSpecs']").removeClass("tab-active").addClass("tab-inactive");
            //                $("[id$='lbnPackingSpecs']").removeClass("tab-active").addClass("tab-inactive");
            //            }
            //            else {
            //                $("[id$='spnPackingListing']").removeClass("tab-active").addClass("tab-inactive");
            //                $("[id$='lbnPackingListing']").removeClass("tab-active").addClass("tab-inactive");
            //                $("[id$='spnPackingSpecs']").removeClass("tab-inactive").addClass("tab-active");
            //                $("[id$='lbnPackingSpecs']").removeClass("tab-inactive").addClass("tab-active");
            //            }
        }



        function ValidateNow() {
            var isValid = true;
            var msg = "";

            $("[id$='lblValidCustomer']").hide();
            $("[id$='lblValidBrand']").hide();
            $("[id$='lblValidArtversion']").hide();

            if ($("[id$='lblCustomer']").text() == '' || $("[id$='lblCustomer']").text() == "Select/Type") {
                $("[id$='lblValidCustomer']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Customer") %></li></ul>';
            }

            if ($("[id$='lblBrand']").text() == '' || $("[id$='lblBrand']").text() == "Select/Type") {
                $("[id$='lblValidBrand']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Brand") %></li></ul>';
            }

            if ($("[id$='txtArtworkVersion']").val() == '') {
                $("[id$='lblValidArtversion']").show();
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_ArtVersion") %></li></ul>';
            } else {
                if ($("[id$=hdfMode]").val() == 'NEW') {
                    if ($("[id$='txtArtworkVersion']").val() == $("[id$=hdfOldVersion]").val()) {
                        $("[id$='lblValidArtversion']").show();
                        isValid = false;
                        msg += '<ul><li><%= GetLocalResourceObject("Err_MustChangeArtVersion") %></li></ul>';
                    }
                }
            }

            var flagQuantity = true;
            $("input[id$=Quantity]").each(function () {
                var txtQuantity = $(this);
                //  $(this).removeClass("error");
                var quantity = $(this).val();
                if (quantity != '') {
                    if (!(/^\-?([0-9]+(\.[0-9]+)?|Infinity)$/.test(quantity))) {
                        // txtQuantity.addClass("error");
                        flagQuantity = false;
                    }
                }
            });

            if (!flagQuantity) {
                isValid = false;
                msg += '<ul><li><%= GetLocalResourceObject("Err_Quantity") %></li></ul>';
            }

            if (!isValid) {
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }
            return isValid;
        }



        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function isFloatNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                if (charCode == 46)
                    return true;
                return false;
            }

            return true;
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

        function DisableStatus() {
            if ($("[id$=chkStatus]").parent("span") != null) {
                $("[id$=chkStatus]").parent("span").addClass("span-normal");
            }
        }

        function FileNotFound() {
            msg = '<ul><li><%= GetLocalResourceObject("Err_File") %></li></ul>';
            $("[id$=litErrorMsg]").show();
            $("[id$=litErrorMsg]").html(msg);
            ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
        }

        function ShowMaterial(ddl) {

            if ($("[id$='" + ddl + "']").val() != '-1') {
                var text = $("[id$='" + ddl + "']").find(":selected").text();
                if (text != "- NA -") {
                    //window.open("../../Administration/Masters/MaterialMaster.aspx?Type=3&Code=" + text.split('-')[1].trim(), "_blank");
                    window.open("../../GeneralAdmin/MaterialMaster.aspx?Type=3&Code=" + text.split('-')[1].trim(), "_blank");
                }
            }
            else {
                msg = '<ul><li><%= GetLocalResourceObject("Err_Item") %></li></ul>';
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }
            return false;
        }
        function ShowMaterialWithCode(hdf) {
            if ($("[id$='" + hdf + "']").val() != '0' & $("[id$='" + hdf + "']").val() != '') {
                var text = $("[id$='" + hdf + "']").val();
                if (text != "- NA -") {
                    window.open("../../GeneralAdmin/MaterialMaster.aspx?Type=3&Code=" + text.trim(), "_blank");
                }
            }
            else {
                msg = '<ul><li><%= GetLocalResourceObject("Err_Item") %></li></ul>';
                $("[id$=litErrorMsg]").show();
                $("[id$=litErrorMsg]").html(msg);
                ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            }
            return false;
        }


        function ShowArtworkActivateConfirm() {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%=GetLocalResourceObject("Msg_Activate_confrm").ToString() %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 350,
                title: msgTitle,
                resizable: false,
                buttons: {
                    OK: function (e) {
                        $("[id$=hdfActivate]").val("1");
                        $(this).dialog("close");
                        $("[id$=btnActivate]").click();
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        return false;
                    }
                }
            });
            return false;
        }
     
        function setPackingMatAuto(control) {          
                var custId = 0;
                if ($("[id$=hdfCustomerID]").val() != "") {
                    custId = $("[id$=hdfCustomerID]").val()
                }
                GrandScriptUtils.MakeAutoCompleteDDL("txtPouchPack", urlauto + "?CustomerID=" + custId + "&itemPK=" + $("[id$=hdfPouchPack]").val(), "hdfAutoPouchPack", true, true, "PMAUTO", false, false, false, false, false, "- NA -");
                GrandScriptUtils.MakeAutoCompleteDDL("txtInnerBox", urlauto + "?CustomerID=" + custId + "&itemPK=" + $("[id$=hdfInnerBox]").val(), "hdfAutoInnerBox", true, true, "PMAUTO", false, false, false, false, false, "- NA -");
                GrandScriptUtils.MakeAutoCompleteDDL("txtMiniInnerCarton", urlauto + "?CustomerID=" + custId + "&itemPK=" + $("[id$=hdfMiniInnerCarton]").val(), "hdfAutoMiniInnerCarton", true, true, "PMAUTO", false, false, false, false, false, "- NA -");
                GrandScriptUtils.MakeAutoCompleteDDL("txtZipperBag", urlauto + "?CustomerID=" + custId + "&itemPK=" + $("[id$=hdfZipperBag]").val(), "hdfAutoZipperBag", true, true, "PMAUTO", false, false, false, false, false, "- NA -");
                GrandScriptUtils.MakeAutoCompleteDDL("txtMasterCarton", urlauto + "?CustomerID=" + custId + "&itemPK=" + $("[id$=hdfMasterCarton]").val(), "hdfAutoMasterCarton", true, true, "PMAUTO", false, false, false, false, false, "- NA -");
                GrandScriptUtils.MakeAutoCompleteDDL("txtSackBag", urlauto + "?CustomerID=" + custId + "&itemPK=" + $("[id$=hdfSackBag]").val(), "hdfAutoSackBag", true, true, "PMAUTO", false, false, false, false, false, "- NA -");
                GrandScriptUtils.MakeAutoCompleteDDL("txtWallet", urlauto + "?CustomerID=" + custId + "&itemPK=" + $("[id$=hdfWallet]").val(), "hdfAutoWallet", true, true, "PMAUTO", false, false, false, false, false, "- NA -");
                GrandScriptUtils.MakeAutoCompleteDDL("txtPOB", urlauto + "?CustomerID=" + custId + "&itemPK=" + $("[id$=hdfPOB]").val(), "hdfAutoPOB", true, true, "PMAUTO", false, false, false, false, false, "- NA -");
                GrandScriptUtils.MakeAutoCompleteDDL("txtPolybag", urlauto + "?CustomerID=" + custId + "&itemPK=" + $("[id$=hdfPolybag]").val(), "hdfAutoPolybag", true, true, "PMAUTO", false, false, false, false, false, "- NA -");           
        }       

    </script>
</asp:Content>
<asp:Content ID="cntMain" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="aupdpnlPacking" runat="server">
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
                                        <asp:Button ID="btnSave" runat="server" CommandName="SAVE" Text="<%$ resources:Controls,Save %>"
                                            OnClientClick="return ValidateNow();" ValidationGroup="Packing" SkinID="btnInner-Save"
                                            TabIndex="31" CommandArgument="SEC_ActionPanel" ToolTip="<%$ resources:Controls,Save %>"
                                            OnClick="ActionHandler" />
                                    </li>
                                    <%-- <li id="pnlDelete" runat="server">
                                        <asp:Button ID="btnDelete" runat="server" CommandName="DELETE" Text="<%$resources:Controls,Delete %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" OnClick="ActionHandler" TabIndex="32"
                                            ToolTip="<%$ resources:Controls,Delete %>" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>--%>
                                    <li>
                                        <asp:Button ID="btnCancel" runat="server" CommandName="CANCEL" Text="<%$resources:Controls,Cancel %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" OnClick="ActionHandler"
                                            TabIndex="33" ToolTip="<%$ resources:Controls,Cancel %>" />
                                    </li>
                                </ul>
                                <ul id="pnlListing" runat="server" style="display: none">
                                    <%--<li>
                                        <asp:Button ID="btnNew" runat="server" CommandName="NEW" Text="<%$ resources:Controls,New %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="<%$ resources:Controls,New %>"
                                            OnClick="ActionHandler" TabIndex="14" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnEdit" runat="server" CommandName="EDIT" Text="<%$ resources:Controls,Edit %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="<%$ resources:Controls,Edit %>"
                                            OnClick="ActionHandler" TabIndex="15" />
                                    </li>
                                    <li>
                                        <asp:Button ID="btnView" runat="server" CommandName="VIEW" Text="<%$ resources:Controls,View %>"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-View" ToolTip="<%$ resources:Controls,View %>"
                                            OnClick="ActionHandler" TabIndex="16" />
                                    </li>--%>
                                </ul>
                            </asp:TableCell></asp:TableRow>
                    </asp:Table>
                </div>
                <div class="tab-container" id="divTabContainer" runat="server">
                    <ul id="tab-menu">
                        <li><span id="spnPackingListing" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnPackingListing" runat="server" Text="<%$ resources:Controls,List %>"
                                CommandName="PACKINGLIST" CssClass="tab-inactive" OnClick="ActionHandler" TabIndex="1"
                                ToolTip="<%$ resources:Controls,List %>" />
                        </span></li>
                        <li><span id="spnPackingSpecs" runat="server" class="tab-inactive">
                            <asp:LinkButton ID="lbnPackingSpecs" runat="server" Text="<%$ resources:Controls,PackingSpecs %>"
                                CommandName="PACKINGDETAILS" CssClass="tab-inactive" OnClick="ActionHandler"
                                TabIndex="2" ToolTip="<%$ resources:Controls,PackingSpecs %>" />
                        </span></li>
                        <li><span id="spnPackingSpecsMapping" runat="server" class="tab-active">
                            <asp:LinkButton ID="lblPackingSpecsMapping" runat="server" Text="<%$ resources:Controls,packingSpecsMapping %>"
                                CommandName="CLEAR" CssClass="tab-active" OnClick="ActionHandler" TabIndex="3"
                                ToolTip="<%$ resources:Controls,packingSpecsMapping %>" />
                        </span></li>
                    </ul>
                </div>
            </div>
            <div class="content-wrapper">
                <div class="pack-material" runat="server" id="PackingHeaderDiv">
                    <div class="lft-col">
                        <asp:Label ID="lbl1" runat="server" AssociatedControlID="lblPackingSpecCodeHdr" Text="<%$ resources:PackingSpecCode %>"></asp:Label>
                        <asp:Label ID="lblPackingSpecCodeHdr" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="rgt-col">
                        <asp:Label ID="lbl2" runat="server" AssociatedControlID="lblPackingSpecTotalHdr"
                            Text="<%$ resources:TotalPcs1 %>"></asp:Label>
                        <asp:Label ID="lblPackingSpecTotalHdr" runat="server" Text=""></asp:Label>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <asp:Table ID="tblTemplate" runat="server" CssClass="asptbllinks">
                    <asp:TableRow ID="PageAction_List" runat="server">
                        <asp:TableCell>
                            <div class="search-colapse">
                                <table>
                                    <tr>
                                        <td>
                                            <h1>
                                                <%= GetGlobalResourceObject("Controls", "AdvanceSearch").ToString()%></h1>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                                ImageUrl="~/Images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                                TabIndex="4" />
                                            <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                                ImageUrl="~/images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                                TabIndex="5" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <%--------------colpase btn----------%>
                            <div class="clear">
                            </div>
                            <table class="table-devide" id="tbladvancedSearch" style="margin-top: 8px;">
                                <tr id="Tr1" runat="server">
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCustomerSearch" runat="server" Text="<%$ resources:Controls,Customer %>"
                                                AssociatedControlID="txtCustomerID"></asp:Label>
                                            <asp:TextBox ID="txtCustomerID" runat="server" CssClass="input-half" MaxLength="100" TabIndex="6"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblPackingActive" runat="server" Text="<%$ resources:Controls,Active %>"
                                                AssociatedControlID="ddlActive"></asp:Label>
                                            <asp:DropDownList ID="ddlActive" runat="server" CssClass="w15perc" TabIndex="8">
                                                <asp:ListItem Value="-1" Text="<%$ Resources:Captions,All %>"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="<%$ Resources:Captions,Yes %>"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="<%$ Resources:Captions,No %>"></asp:ListItem>
                                            </asp:DropDownList>

                                             <asp:Label ID="lblAwversion" runat="server" CssClass="lbl-18perc" Text="<%$ resources:Controls,ArtworkVersion %>"
                                                AssociatedControlID="txtAWVersion"></asp:Label>
                                            <asp:TextBox ID="txtAWVersion" runat="server" TabIndex="9" CssClass="input-small-c0-23-11"> </asp:TextBox>
                                            <div class="clear">
                                            </div>                                           
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBrandSearch" runat="server" Text="<%$ resources:Controls,Brand%>"
                                                AssociatedControlID="txtBrandID" />
                                            <asp:TextBox ID="txtBrandID" runat="server" CssClass="input-half-20-11-9" TabIndex="7" />
                                            <asp:HiddenField ID="hdfBrandID" runat="server" />
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblPackSpecCode" runat="server" Text="<%$ resources:Controls,PackingSpecCode %>"
                                                AssociatedControlID="txtPackSpecCode"></asp:Label>
                                            <asp:TextBox ID="txtPackSpecCode" runat="server" TabIndex="9" CssClass="input-half-20-11-9"> </asp:TextBox>
                                            <asp:Label ID="lblSearch" runat="server" AssociatedControlID="btnSearch" CssClass="middle-lbl-xsmall-d style-none"></asp:Label>
                                            <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                                 ValidationGroup="Search" OnClick="ActionHandler" TabIndex="10" CommandName="SEARCH"  SkinID="search-ext" />
                                            <asp:ImageButton ID="btnClear" runat="server" Text="<%$ resources:Controls,Clear %>" TabIndex="11"
                                                ToolTip="<%$ resources:Controls,Clear %>" OnClick="ActionHandler" CommandName="CLEAR"
                                                SkinID="clear-ext" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="clear">
                            </div>
                            <div class="gridwrap">
                                <asp:Button ID="btnActivate" runat="server" EnableTheming="false" Style="display: none"
                                    OnClick="ActionHandler" CommandName="ACTIVATEARTWORK" />
                                <asp:GridView runat="server" ID="grdPackingMst" Width="100%" PageSize="<%$ resources:PageSize %>"
                                    AllowSorting="True" AutoGenerateColumns="false" EmptyDataRowStyle-CssClass="emptytable"
                                    OnSorting="ActionHandler" AllowPaging="true" OnPageIndexChanging="ActionHandler"
                                    OnRowDataBound="ActionHandler">
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblNoRecord" runat="server" Text="<%$ resources:Messages,Msg_EmptyGrid %>" />
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <%--                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ProductCode %>" SortExpression="<%$ resources:DataFieldRes,ItemCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCProductCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.ItemCode)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.ItemCode)),20) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="12%" />
                                        </asp:TemplateField>
                                        --%>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,PacSpecCode %>" SortExpression="<%$ resources:DataFieldRes,PacSpecCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCProductCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.PacSpecCode)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.PacSpecCode)),50) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Customer %>" SortExpression="<%$ resources:DataFieldRes,CusCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCBrand" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.CusCode)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.CusCode)),10) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="9%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,BrandName %>" SortExpression="<%$ resources:DataFieldRes,PackingMasterItemText %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpBrand" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString( Eval(Resources.DataFieldRes.PackingMasterItemText)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString( Eval(Resources.DataFieldRes.PackingMasterItemText)),200) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="40%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,BrandCode %>" SortExpression="<%$ resources:DataFieldRes,BrandCode %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblpBrandCode" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString( Eval(Resources.DataFieldRes.BrandCode)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString( Eval(Resources.DataFieldRes.BrandCode)),72) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="13%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,ArtworkVersionShort%>" SortExpression="<%$ resources:DataFieldRes,PackingMasterArtWork %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblaVersion" runat="server" ToolTip='<%# ERP.Utilities.CommonFunctions.GetDecodedString(Eval(Resources.DataFieldRes.PackingMasterArtWork)) %>'
                                                    Text='<%# ERP.Utilities.CommonFunctions.GetShortString(ERP.Utilities.CommonFunctions.GetEncodedString(Eval(Resources.DataFieldRes.PackingMasterArtWork)),15) %>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="10%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ resources:Controls,Active %>" SortExpression="<%$ resources:DataFieldRes,PackingActive %>">
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imInactive" SkinID="inactiveIcon" OnClick="ActionHandler"
                                                    CommandName="GRIDACTIVATE" ToolTip="<%$ resources:Controls,InActive  %>" CommandArgument="<%# Eval(Resources.DataFieldRes.PIMPK) %>"
                                                    TabIndex="12" Enabled='<%# Eval(Resources.DataFieldRes.PackingActive).ToString() =="1"? false : true %>'
                                                    Visible='<%# Eval(Resources.DataFieldRes.PackingActive).ToString() =="1"? false : true %>' />
                                                <asp:ImageButton runat="server" ID="imbActive" SkinID="activeIcon" ToolTip="<%$ resources:Controls,Active  %>"
                                                    Enabled='<%# Eval(Resources.DataFieldRes.PackingActive).ToString() =="1"? true : false %>'
                                                    Visible='<%# Eval(Resources.DataFieldRes.PackingActive).ToString() =="1"? true : false %>'
                                                    TabIndex="12" OnClientClick="return false;" style="cursor:default;" />
                                                <%--<asp:Label ID="lblPstatus" runat="server" ToolTip='<%# Eval(Resources.DataFieldRes.PackingActive).ToString() =="1"? Resources.Captions.Yes : Resources.Captions.No %>'
                                                    Text='<%# Eval(Resources.DataFieldRes.PackingActive).ToString() =="1"? Resources.Captions.Yes : Resources.Captions.No %>' />--%>
                                                <asp:HiddenField runat="server" ID="hdfStatus" Value='<%# Eval(Resources.DataFieldRes.PackingActive)%>' />
                                            </ItemTemplate>
                                            <ItemStyle Width="3%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClick="ActionHandler"
                                                    CommandName="GRIDEDIT" ToolTip="<%$ resources:Controls,Edit  %>" CommandArgument="<%# Eval(Resources.DataFieldRes.PIMPK) %>"
                                                    TabIndex="12" />
                                                <asp:ImageButton runat="server" ID="imgNew" SkinID="imbaddnew" OnClick="ActionHandler"
                                                    CommandName="NEWVERSION" ToolTip="<%$ resources:Controls,New  %>" CommandArgument="<%# Eval(Resources.DataFieldRes.PIMPK) %>"
                                                    TabIndex="12" />
                                                <%-- <asp:ImageButton runat="server" ID="imbRemove" SkinID="imbdeletegrid" OnClick="ActionHandler" CommandName="GRIDDELETE" ToolTip="<%$ resources:Captions, Remove %>" CommandArgument="<%# Eval(Resources.DataFieldRes.PIMPK) %>" OnClientClick="return ShowDeleteConfirm(this);"  TabIndex ="13"  />
                                                --%>
                                            </ItemTemplate>
                                            <ItemStyle Width="5%" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%-- <uc1:PagerControl ID="uclPaging" runat="server" />--%>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="PageAction_Entry" runat="server">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCust" runat="server" Text="<%$ resources:Controls,Customer%>" AssociatedControlID="lblCustomer" />
                                            <asp:Label ID="lblCustomer" runat="server" CssClass="input-half" TabIndex="20" />
                                            <asp:HiddenField ID="hdfCustomer" runat="server" />
                                            <asp:Label ID="lblValidCustomer" runat="server" Text="*" CssClass="star" />
                                            <asp:Button ID="btnCustomer" runat="server" EnableTheming="false" Style="display: none"
                                                CommandName="CUSTOMERCHANGE" OnClick="ActionHandler" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblBrandCode" runat="server" Text="<%$ resources:Controls,ItemCode%>"
                                                AssociatedControlID="lblBrandCodeText" />
                                            <asp:Label runat="server" ID="lblBrandCodeText" CssClass="input-half"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblB" runat="server" Text="<%$ resources:Controls,Brand%>" AssociatedControlID="lblBrand" />
                                            <asp:Label ID="lblBrand" runat="server" TabIndex="21" CssClass="height-auto break-word" />
                                            <asp:HiddenField ID="hdfBrand" runat="server" />
                                            <asp:Label ID="lblValidBrand" runat="server" Text="*" CssClass="star" />
                                            <div style="display: none;">
                                                <asp:Button runat="server" ID="btnBrandDetails" CommandName="BRANDDETAILS" OnClick="ActionHandler" /></div>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div class="divcol-S">
                                            <asp:Label ID="lblBrandDetails" runat="server" Text="<%$ resources:Controls,BrandDetails%>"
                                                AssociatedControlID="lblBrandDetailsText" Visible="false" />
                                            <asp:Label runat="server" ID="lblBrandDetailsText" Visible="false"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblArtworkVersion" runat="server" Text="<%$ resources:Controls,ArtworkVersion%>"
                                                AssociatedControlID="txtArtworkVersion" />
                                            <asp:TextBox ID="txtArtworkVersion" runat="server" CssClass="medium" TabIndex="22" />
                                            <asp:Label ID="lblValidArtversion" runat="server" Text="*" CssClass="star" />
                                            <asp:HiddenField ID="hdfOldVersion" runat="server" />
                                            <asp:HiddenField ID="hdfMode" runat="server" />
                                            <%--  <asp:RequiredFieldValidator ID="vrfArtworkVersion" runat="server" ControlToValidate="txtArtworkVersion">
                                            </asp:RequiredFieldValidator>--%>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblStatus" runat="server" Text="<%$ resources:Controls,Active%>" AssociatedControlID="chkStatus" />
                                            <asp:CheckBox ID="chkStatus" runat="server" Checked="true" TabIndex="23" />
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="gridwrap">
                                <table class="gridwraptable tble-border" style="width: 86%;">
                                    <tr class="grdhead">
                                        <th style="width: 14%">
                                            <%= Resources.Controls.PackingMaterial %>
                                        </th>
                                        <th style="width: 56%">
                                            <%= Resources.Controls.MaterialCodePacking %>
                                        </th>
                                        <th style="width: 6%">
                                            <%= Resources.Controls.Artwork %>
                                        </th>
                                        <th style="width: 10%">
                                            <%= Resources.Controls.Dimension %>
                                        </th>
                                        <th style="width: 14%">
                                            <%= Resources.Controls.Color %>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdfPouchPack" runat="server" Value="0" />
                                            <asp:Label ID="lblPouchPack" runat="server" Text="<%$ resources:Controls,PouchPack%>"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:DropDownList ID="ddlPouchPack" Width="90%" runat="server" AutoPostBack="true"
                                                onmouseover="javascript:ShowTooltip('ddlPouchPack');" TabIndex="24" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>--%>
                                            <asp:TextBox ID="txtPouchPack" runat="server" Width="93%" ></asp:TextBox>
                                            <asp:HiddenField ID="hdfAutoPouchPack" runat="server" Value="0" />

                                            <asp:HiddenField ID="hdfPouchPackCode" runat="server" Value="0" />
                                            <%--<asp:ImageButton ID="imbPouchPack" runat ="server" SkinID ="btnview" OnClientClick="return ShowMaterial('ddlPouchPack')" ToolTip="<%$ resources:ViewMaterial%>" />--%>
                                            <asp:ImageButton ID="imbPouchPack" runat="server" SkinID="btnview" OnClientClick="return ShowMaterialWithCode('hdfPouchPackCode')"
                                                ToolTip="<%$ resources:ViewMaterial%>" />
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblPPArtWork" runat="server"></asp:Label>
                                            <a runat="server" id="lnkPPArtWork" title="<%$ resources:Controls,View %>" href=''>
                                            </a>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblPPDimension" runat="server"></asp:Label>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblPPColour" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdfInnerBox" runat="server" Value="0" />
                                            <asp:Label ID="lblInnerBox" runat="server" Text="<%$ resources:Controls,InnerBox%>"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:DropDownList ID="ddlInnerBox" Width="90%" runat="server" AutoPostBack="true"
                                                onmouseover="javascript:ShowTooltip('ddlInnerBox');" TabIndex="25" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>--%>
                                            <asp:TextBox ID="txtInnerBox" runat="server" Width="93%" ></asp:TextBox>
                                            <asp:HiddenField ID="hdfAutoInnerBox" runat="server" Value="0" />

                                            <asp:HiddenField ID="hdfInnerBoxCode" runat="server" Value="0" />
                                            <%--<asp:ImageButton ID="imbInnerBox" runat ="server" SkinID ="btnview" OnClientClick="return ShowMaterial('ddlInnerBox')" ToolTip="<%$ resources:ViewMaterial%>" />--%>
                                            <asp:ImageButton ID="imbInnerBox" runat="server" SkinID="btnview" OnClientClick="return ShowMaterialWithCode('hdfInnerBoxCode')"
                                                ToolTip="<%$ resources:ViewMaterial%>" />
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblIBArtWork" runat="server"></asp:Label>
                                            <a runat="server" id="lnkIBArtWork" title="<%$ resources:Controls,View %>" href=''>
                                            </a>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblIBDimension" runat="server"></asp:Label>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblIBColour" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdfMiniInnerCarton" runat="server" Value="0" />
                                            <asp:Label ID="lblMiniInnerCarton" runat="server" Text="<%$ resources:Controls,MiniInnerCarton%>"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:DropDownList ID="ddlMiniInnerCarton" Width="90%" runat="server" AutoPostBack="true"
                                                onmouseover="javascript:ShowTooltip('ddlMiniInnerCarton');" TabIndex="26" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>--%>
                                             <asp:TextBox ID="txtMiniInnerCarton" runat="server" Width="93%"></asp:TextBox>
                                            <asp:HiddenField ID="hdfAutoMiniInnerCarton" runat="server" Value="0" />

                                            <asp:HiddenField ID="hdfMiniInnerCartonCode" runat="server" Value="0" />
                                            <%--<asp:ImageButton ID="imbMiniInnerCarton" runat ="server" SkinID ="btnview" OnClientClick="return ShowMaterial('ddlMiniInnerCarton')" ToolTip="<%$ resources:ViewMaterial%>" />--%>
                                            <asp:ImageButton ID="imbMiniInnerCarton" runat="server" SkinID="btnview" OnClientClick="return ShowMaterialWithCode('hdfMiniInnerCartonCode')"
                                                ToolTip="<%$ resources:ViewMaterial%>" />
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblMICArtWork" runat="server"></asp:Label>
                                            <a runat="server" id="lnkMICArtWork" title="<%$ resources:Controls,View %>" href=''>
                                            </a>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblMICDimension" runat="server"></asp:Label>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblMICColour" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdfZipperBag" runat="server" Value="0" />
                                            <asp:Label ID="lblZipperBag" runat="server" Text="<%$ resources:Controls,ZipperBag%>"></asp:Label>
                                        </td>
                                        <td>
                                          <%--  <asp:DropDownList ID="ddlZipperBag" Width="90%" runat="server" AutoPostBack="true"
                                                onmouseover="javascript:ShowTooltip('ddlZipperBag');" TabIndex="27" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>--%>
                                            <asp:TextBox ID="txtZipperBag" runat="server" Width="93%" ></asp:TextBox>
                                            <asp:HiddenField ID="hdfAutoZipperBag" runat="server" Value="0" />

                                            <asp:HiddenField ID="hdfZipperBagCode" runat="server" Value="0" />
                                            <%--<asp:ImageButton ID="imbZipperBag" runat ="server" SkinID ="btnview" OnClientClick="return ShowMaterial('ddlZipperBag')" ToolTip="<%$ resources:ViewMaterial%>" />--%>
                                            <asp:ImageButton ID="imbZipperBag" runat="server" SkinID="btnview" OnClientClick="return ShowMaterialWithCode('hdfZipperBagCode')"
                                                ToolTip="<%$ resources:ViewMaterial%>" />
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblZBArtWork" runat="server"></asp:Label>
                                            <a runat="server" id="lnkZBArtWork" title="<%$ resources:Controls,View %>" href=''>
                                            </a>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblZBDimension" runat="server"></asp:Label>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblZBColour" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdfMasterCarton" runat="server" Value="0" />
                                            <asp:Label ID="lblMasterCarton" runat="server" Text="<%$ resources:Controls,MasterCarton%>"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:DropDownList ID="ddlMasterCarton" Width="90%" runat="server" AutoPostBack="true"
                                                onmouseover="javascript:ShowTooltip('ddlMasterCarton');" TabIndex="28" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>--%>
                                             <asp:TextBox ID="txtMasterCarton" runat="server" Width="93%" ></asp:TextBox>
                                            <asp:HiddenField ID="hdfAutoMasterCarton" runat="server" Value="0" />

                                            <asp:HiddenField ID="hdfMasterCartonCode" runat="server" Value="0" />
                                            <%--<asp:ImageButton ID="imbMasterCarton" runat ="server" SkinID ="btnview" OnClientClick="return ShowMaterial('ddlMasterCarton')"  ToolTip="<%$ resources:ViewMaterial%>" />--%>
                                            <asp:ImageButton ID="imbMasterCarton" runat="server" SkinID="btnview" OnClientClick="return ShowMaterialWithCode('hdfMasterCartonCode')"
                                                ToolTip="<%$ resources:ViewMaterial%>" />
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblMCArtWork" runat="server"></asp:Label>
                                            <a runat="server" id="lnkMCArtWork" title="<%$ resources:Controls,View %>" href=''>
                                            </a>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblMCDimension" runat="server"></asp:Label>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblMCColour" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdfSackBag" runat="server" Value="0" />
                                            <asp:Label ID="lblSackBag" runat="server" Text="<%$ resources:Controls,SackBag%>"></asp:Label>
                                        </td>
                                        <td>
                                           <%-- <asp:DropDownList ID="ddlSackBag" Width="90%" runat="server" AutoPostBack="true"
                                                onmouseover="javascript:ShowTooltip('ddlSackBag');" TabIndex="29" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>--%>
                                            <asp:TextBox ID="txtSackBag" runat="server" Width="93%" ></asp:TextBox>
                                            <asp:HiddenField ID="hdfAutoSackBag" runat="server" Value="0" />

                                            <asp:HiddenField ID="hdfSackBagCode" runat="server" Value="0" />
                                            <%--<asp:ImageButton ID="imbSackBag" runat ="server" SkinID ="btnview" OnClientClick="return ShowMaterial('ddlSackBag')" ToolTip="<%$ resources:ViewMaterial%>" />--%>
                                            <asp:ImageButton ID="imbSackBag" runat="server" SkinID="btnview" OnClientClick="return ShowMaterialWithCode('hdfSackBagCode')"
                                                ToolTip="<%$ resources:ViewMaterial%>" />
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblSBArtWork" runat="server"></asp:Label>
                                            <a runat="server" id="lnkSBArtWork" title="<%$ resources:Controls,View %>" href=''>
                                            </a>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblSBDimension" runat="server"></asp:Label>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblSBColour" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdfWallet" runat="server" Value="0" />
                                            <asp:Label ID="lblWallet" runat="server" Text="<%$ resources:Controls,Wallet%>"></asp:Label>
                                        </td>
                                        <td>
                                          <%--  <asp:DropDownList ID="ddlWallet" Width="90%" runat="server" AutoPostBack="true" onmouseover="javascript:ShowTooltip('ddlWallet');"
                                                TabIndex="30" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>--%>
                                             <asp:TextBox ID="txtWallet" runat="server" Width="93%" ></asp:TextBox>
                                             <asp:HiddenField ID="hdfAutoWallet" runat="server" Value="0" />

                                            <asp:HiddenField ID="hdfWalletCode" runat="server" Value="0" />
                                            <asp:ImageButton ID="imbWallet" runat="server" SkinID="btnview" OnClientClick="return ShowMaterialWithCode('hdfWalletCode')"
                                                ToolTip="<%$ resources:ViewMaterial%>" />
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblWLTArtWork" runat="server"></asp:Label>
                                            <a runat="server" id="lnkWLTArtWork" title="<%$ resources:Controls,View %>" href=''>
                                            </a>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblWLTDimension" runat="server"></asp:Label>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblWLTColour" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdfPOB" runat="server" Value="0" />
                                            <asp:Label ID="lblPOB" runat="server" Text="<%$ resources:Controls,POB%>"></asp:Label>
                                        </td>
                                        <td>
                                           <%-- <asp:DropDownList ID="ddlPOB" Width="90%" runat="server" AutoPostBack="true" onmouseover="javascript:ShowTooltip('ddlEnvelope');"
                                                TabIndex="31" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>--%>
                                             <asp:TextBox ID="txtPOB" runat="server" Width="93%" ></asp:TextBox>
                                            <asp:HiddenField ID="hdfAutoPOB" runat="server" Value="0" />

                                            <asp:HiddenField ID="hdfPOBCode" runat="server" Value="0" />
                                            <asp:ImageButton ID="imbPOB" runat="server" SkinID="btnview" OnClientClick="return ShowMaterialWithCode('hdfPOBCode')"
                                                ToolTip="<%$ resources:ViewMaterial%>" />
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblPOBArtWork" runat="server"></asp:Label>
                                            <a runat="server" id="lnkPOBArtWork" title="<%$ resources:Controls,View %>" href=''>
                                            </a>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblPOBDimension" runat="server"></asp:Label>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblPOBColour" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdfPolybag" runat="server" Value="0" />
                                            <asp:Label ID="lblPolybag" runat="server" Text="<%$ resources:Controls,PolyBag%>"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:DropDownList ID="ddlPolybag" Width="90%" runat="server" AutoPostBack="true"
                                                onmouseover="javascript:ShowTooltip('ddlPolybag');" TabIndex="32" OnSelectedIndexChanged="ActionHandler">
                                            </asp:DropDownList>--%>
                                             <asp:TextBox ID="txtPolybag" runat="server" Width="93%"></asp:TextBox>
                                             <asp:HiddenField ID="hdfAutoPolybag" runat="server" Value="0" />

                                            <asp:HiddenField ID="hdfPolybagCode" runat="server" Value="0" />
                                            <asp:ImageButton ID="imbPolybag" runat="server" SkinID="btnview" OnClientClick="return ShowMaterialWithCode('hdfPolybagCode')"
                                                ToolTip="<%$ resources:ViewMaterial%>" />
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblPRBArtWork" runat="server"></asp:Label>
                                            <a runat="server" id="lnkPRBArtWork" title="<%$ resources:Controls,View %>" href=''>
                                            </a>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblPRBDimension" runat="server"></asp:Label>
                                        </td>
                                        <td class="bggrey">
                                            <asp:Label ID="lblPRBColour" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="gridwrap">
                            </div>
                            <div class="gridwrap">
                                <asp:Table runat="server" ID="tblDynamicMaterials" class="gridwraptable tble-border"
                                    Style="width: 86%;">
                                </asp:Table>
                            </div>
                        </asp:TableCell></asp:TableRow>
                    <asp:TableRow ID="ModifiedDatePnl" CssClass="last-modified" runat="server" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblLastModifiedHDR" runat="server"></asp:Label>
                        </asp:TableCell></asp:TableRow>
                </asp:Table>
                <div id="diverror" style="display: none">
                    <%--Use this label to bind the server errors--%>
                    <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star"></asp:Label><asp:ValidationSummary
                        ID="vsPage" ValidationGroup="Packing" runat="server" />
                </div>
            </div>
            <asp:HiddenField runat="server" ID="hdfActivate" Value="0" />
             <asp:Button ID="btnDummySelChang" runat="server" EnableTheming="false" Style="display: none"
                                    OnClick="ActionHandler" CommandName="SELECTEDINDEXCHANGED" />
             <asp:HiddenField runat="server" ID="hdfSelectedTextboxId" Value="" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
