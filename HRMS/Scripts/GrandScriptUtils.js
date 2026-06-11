/// <reference path="jquery/jquery-1.5.min.js" />
/// <reference path="jquery/UI/jquery.ui.datetimepicker.js" />
/// <reference path="jquery/json2.js" />


///#region ---------- Global Variables
// For FileUpload
var fileTitle = "";
var fileID = "";
var listFileType = true;
var FileJson = new Object();

///#endregion

///#region ---------- Configuration

// For File Upload
var FileUpload = {
    UPLOADFILEURL: "FileUploadHandler.do?Action=UPLOADFILE&Type=",
    REMOVEFILEURL: "FileUploadHandler.do?Action=DELETEFILE&Type=",
    FILEUPLOADALREADYMSG: "File Already uploaded",
    DELETEDMSG: " Deleted!!",
    PLEASESELECTFILEMSG: "Please Select File",
    INFORMATIONTITLE: "Information",
    EMPTYVALUE: "",
    TEMPPATH: "Upload\\",
    TEMPFOLDER: "TempFolder",
    DOWNLOADURL: "FileUploadHandler.do?Action=DOWNLOADFILE&FileID="

}
///#endregion

var GrandScriptUtils =
{

    FormToJSON: function () {
        ///<summary> Do not call externally Only for internal!! Returns the serialized form data </summary>
        var frmID = $(document.forms[0]).attr("id");
        var gSerializedData = $("#" + frmID).serializeArray();
        for (i = 0; i < gSerializedData.length; i++) {
            // removes the unwanted form items from the list
            if ((gSerializedData[i].name.toLowerCase() == "__viewstate") || (gSerializedData[i].name.toLowerCase() == "__eventvalidation") || (gSerializedData[i].name.toLowerCase() == "__eventtarget") || (gSerializedData[i].name.toLowerCase() == "__eventargument") || (gSerializedData[i].name.toLowerCase() == "__lastfocus")) {
                gSerializedData.splice(i, 1);
                i = i - 1;
            }
        }
        return gSerializedData;
    },
    DisableControls: function (ContainerID) {
        ///<summary> Disables all the controls in a container with specific id </summary>
        $("#" + ContainerID).find("input").attr("disabled", "disabled");
        $("#" + ContainerID).find("select").attr("disabled", "disabled");
        $("#" + ContainerID).find("a").attr("disabled", "disabled");
    },

    EnableControls: function (ContainerID) {
        ///<summary> Enable all the controls in a container with specific id </summary>
        $("#" + ContainerID).find("input").removeAttr("disabled");
        $("#" + ContainerID).find("select").removeAttr("disabled");
        $("#" + ContainerID).find("a").removeAttr("disabled");
    },
    ToFixed: function (num, precision) {
        return (+(Math.round(+(num + 'e' + precision)) + 'e' + -precision)).toFixed(precision);
    },
    //Format Decimal Without Rounding Rajesh 28/08/2024
    FormatDecimalWithoutRounding: function (value, decimals) {
        const [integerPart, fractionalPart = ''] = value.toString().split('.');

        const truncatedFractionalPart = fractionalPart.substring(0, decimals);

        const paddedFractionalPart = truncatedFractionalPart.padEnd(decimals, '0');

        return `${integerPart}.${paddedFractionalPart}`;
    }, 

    //    GetMonthVal: function (month) {
    //        var monthNamesShort = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    //        for (var mnth = 0; mnth < 12; mnth++)
    //            if (monthNamesShort[mnth] == month) break;
    //        return mnth++;
    //    },
    //    CalculateAge: function (dateString) {
    //        var now = new Date();
    //        var today = new Date(now.getYear(), now.getMonth(), now.getDate());

    //        var yearNow = now.getYear();
    //        var monthNow = now.getMonth();
    //        var dateNow = now.getDate();
    //        var dateArray = dateString.split('-');

    ////        var yearDob = parseInt(dateString.substring(7, 11));
    ////        var monthDob = GrandScriptUtils.GetMonthVal(dateString.substring(3, 6));
    //        //        var dateDob = parseInt(dateString.substring(0, 2));
    //        var yearDob = parseInt(dateArray[2]);
    //        var monthDob = GrandScriptUtils.GetMonthVal(dateArray[1]);
    //        var dateDob = parseInt(dateArray[0]);
    //        var age = {};
    //        var ageString = "";
    //        var yearString = "";
    //        var monthString = "";
    //        var dayString = "";

    //        yearAge = yearNow - yearDob;

    //        if (monthNow >= monthDob)
    //            var monthAge = monthNow - monthDob;
    //        else {
    //            yearAge--;
    //            var monthAge = 12 + monthNow - monthDob;
    //        }

    //        if (dateNow >= dateDob)
    //            var dateAge = dateNow - dateDob;
    //        else {
    //            monthAge--;
    //            var dateAge = 31 + dateNow - dateDob;

    //            if (monthAge < 0) {
    //                monthAge = 11;
    //                yearAge--;
    //            }
    //        }
    //        age = {
    //            years: yearAge,
    //            months: monthAge,
    //            days: dateAge
    //        };

    //        if (age.years > 1) yearString = " years";
    //        else yearString = " year";
    //        if (age.months > 1) monthString = " months";
    //        else monthString = " month";
    //        if (age.days > 1) dayString = " days";
    //        else dayString = " day";


    //        if ((age.years > 0) && (age.months > 0))
    //            ageString = age.years + yearString + " and " + age.months + monthString + " old.";
    //        else if ((age.years == 0) && (age.months == 0))
    //            ageString = "Only " + age.days + dayString + " old.";
    //        else if ((age.years > 0) && (age.months == 0))
    //            ageString = age.years + yearString + " old.";
    //        else if ((age.years > 0) && (age.months > 0))
    //            ageString = age.years + yearString + " and " + age.months + monthString + " old.";
    //        else if ((age.years == 0) && (age.months > 0) && (age.days > 0))
    //            ageString = age.months + monthString + " and " + age.days + dayString + " old.";
    //        else if ((age.years > 0) && (age.months == 0))
    //            ageString = age.years + yearString + " old.";
    //        else if ((age.years == 0) && (age.months > 0))
    //            ageString = age.months + monthString + " old.";
    //        // else ageString = "Oops! Could not calculate age!";
    //        alert(age.years);

    //        return ageString;
    //    },
    FormToJsonString: function (containerID) {
        ///<summary>Use =  Returns the serialized form data</summary>
        /// <param name="containerID" optional="true" type="String">
        ///     Specific Container and its controls
        /// </param>
        // debugger;
        var jSonObj = new Object();
        if (!containerID)
            jSonObj = GrandScriptUtils.FormToJSON();
        else {
            jSonObj = $("#" + containerID + " *").serializeArray();
            for (i = 0; i < jSonObj.length; i++) {
                // removes the unwanted form items from the list
                if ((jSonObj[i].name.toLowerCase() == "__viewstate") || (jSonObj[i].name.toLowerCase() == "__eventvalidation") || (jSonObj[i].name.toLowerCase() == "__eventtarget") || (jSonObj[i].name.toLowerCase() == "__eventargument" || jSonObj[i].name.toLowerCase() == "__lastfocus")) {
                    jSonObj.splice(i, 1);
                    i = i - 1;
                }
            }
        }

        var currentObject = new Object();
        var PrevObject = new Object();
        var jSonString = "{";
        var innerArray = new Array();
        for (i = 0; i < jSonObj.length; i++) {
            currentObject = jSonObj[i];
            // Fix the URL encoding
            currentObject.value = GrandScriptUtils.FixURLEncoding(currentObject.value);
            if (PrevObject.name != currentObject.name) {
                // for inner controls - List type
                if (innerArray.length != 0) {
                    var innerList = "'" + GrandScriptUtils.GetOriginalControlName(PrevObject.name) + "':[";
                    for (j = 0; j < innerArray.length; j++) {
                        innerList += "'" + innerArray[j].value + "'";
                        if (j < (innerArray.length - 1)) {
                            innerList += ",";
                        }
                    }
                    innerList += "]";
                    innerArray.length = 0;
                    jSonString += innerList;
                    if (i < (jSonObj.length)) {
                        jSonString += ",";
                    }
                }
                // generate the control:value pair

                if (currentObject.value.indexOf("%24%7B") == 0) { //To handle JSON string assigned to hidden fields.
                    jSonString += "'" + GrandScriptUtils.GetOriginalControlName(currentObject.name) + "':" + currentObject.value.substr((3));
                }
                else {

                    //jSonString += "'" + GrandScriptUtils.GetOriginalControlName(currentObject.name) + "':'" + currentObject.value.replace('\'', '\\\'') + "'";
                    jSonString += "'" + GrandScriptUtils.GetOriginalControlName(currentObject.name) + "':'" + currentObject.value.replace(/'/g, "\\'") + "'";
                    ///'/g, "\\'");
                }
                if (i < (jSonObj.length - 1)) {
                    jSonString += ",";
                }

            }
            else {
                if (innerArray.length == 0) {
                    jSonString = jSonString.substr(0, (jSonString.lastIndexOf(GrandScriptUtils.GetOriginalControlName(PrevObject.name)) - 1));
                    innerArray.push(PrevObject);
                }
                innerArray.push(currentObject);
            }


            PrevObject = currentObject;
        }

        if (innerArray.length != 0) {
            var innerList = "'" + GrandScriptUtils.GetOriginalControlName(PrevObject.name) + "':[";
            for (j = 0; j < innerArray.length; j++) {
                innerList += "'" + innerArray[j].value + "'";
                if (j < (innerArray.length - 1)) {
                    innerList += ",";
                }
            }
            innerList += "]";
            innerArray.length = 0;
            jSonString += innerList;
        }
        jSonString += "}";
        PrevObject = new Object();
        return jSonString;

    },
    GetOriginalControlName: function (name) {
        ///<summary>Returns the original control name (excludes the master page renaming)</summary>
        return name.substr((name.lastIndexOf('$') + 1), name.length);
    },

    FixURLEncoding: function (value) {
        ///<summary>Fix for URL encoding</summary>
        value = value.replace('%', '%25');
        value = value.replace('+', '%2B');
        value = value.replace('$', '%24');
        value = value.replace('&', '%26');
        value = value.replace(',', '%2C');
        value = value.replace('/', '%2F');
        value = value.replace(':', '%3A');
        value = value.replace(';', '%3B');
        value = value.replace('=', '%3D');
        value = value.replace('?', '%3F');
        value = value.replace('@', '%40');
        value = value.replace(' ', '%20');
        value = value.replace('<', '%3C');
        value = value.replace('>', '%3E');
        value = value.replace('#', '%23');
        value = value.replace('{', '%7B');
        value = value.replace('}', '%7D');
        value = value.replace('|', '%7C');
        value = value.replace('\\', '%5C');
        value = value.replace('^', '%5E');
        value = value.replace('~', '%7E');
        value = value.replace('[', '%5B');
        value = value.replace(']', '%5D');
        value = value.replace('`', '%60');

        return value;
    },



    FillDropDown: function (DropDownID, BindData, Removevalues, IncludeSelect, SelectValue, IncludeAll, IncludeOthers, IncludeMinusSelect, IncludeMinusAll) {
        ///<summary>Fill Dropdown with "BindData" - should be an object array with properties "Text" and "Value"</summary>
        /// <param name="DropDownID" optional="false" type="Number">
        ///     Target Dropdown to be filled with values
        /// </param>
        /// <param name="BindData" optional="false" type="Number">
        ///     Data in Text:Value format
        /// </param>
        /// <param name="Removevalues" optional="true" type="bool">
        ///     Remove all data from drop down before bind?
        /// </param>
        /// <param name="IncludeSelect" optional="true" type="bool">
        ///    Include ----Select----- ?
        /// </param>
        /// <param name="SelectValue" optional="true" type="String">
        ///    Pre-Select the value provided
        /// </param>
        /// <param name="IncludeAll" optional="true" type="bool">
        ///    Include All ?
        /// </param> 

        var TargetDropDown = $("#" + DropDownID);

        if (Removevalues) {
            $(TargetDropDown).html("");
        }
        if (IncludeSelect) {
            $(TargetDropDown).append("<option value='0'>Translate(Select)</option>");
        }
        if (IncludeMinusSelect) {
            $(TargetDropDown).append("<option value='-1'>Translate(Select)</option>");
        }
        if (IncludeAll) {
            $(TargetDropDown).append("<option value='0'>Translate(All)</option>");
        }
        if (IncludeMinusAll) {
            $(TargetDropDown).append("<option value='-1'>Translate(All)</option>");
        }
        if (BindData && BindData.length != undefined) {
            $.each(BindData, function (index, elem) {
                $(TargetDropDown).append("<option value='" + elem.Value + "'" + "title='" + elem.Text + "'" + " >" + elem.Text + "</option>");
            });
        }
        if (!BindData && IncludeSelect != true && IncludeOthers != true && IncludeAll != true)
            $(TargetDropDown).append("<option value='0'>Translate(Select)</option>");
        if (SelectValue) {
            $(TargetDropDown).val(SelectValue);
        }
        if (IncludeOthers) {
            $(TargetDropDown).append("<option value='-1'>Others</option>");
        }
    },
    FillDropDownWithSelect: function (DropDownID, BindData, Removevalues, IncludeSelect, SelectValue, IncludeAll, IncludeOthers) {
        ///<summary>Fill Dropdown with "BindData" - should be an object array with properties "Text" and "Value"</summary>
        /// <param name="DropDownID" optional="false" type="Number">
        ///     Target Dropdown to be filled with values
        /// </param>
        /// <param name="BindData" optional="false" type="Number">
        ///     Data in Text:Value format
        /// </param>
        /// <param name="Removevalues" optional="true" type="bool">
        ///     Remove all data from drop down before bind?
        /// </param>
        /// <param name="IncludeSelect" optional="true" type="bool">
        ///    Include ----Select----- ?
        /// </param>
        /// <param name="SelectValue" optional="true" type="String">
        ///    Pre-Select the value provided
        /// </param>
        /// <param name="IncludeAll" optional="true" type="bool">
        ///    Include All ?
        /// </param>
        var TargetDropDown = $("#" + DropDownID);

        if (Removevalues) {
            $(TargetDropDown).html("");
        }
        if (IncludeSelect) {
            $(TargetDropDown).append("<option value='-1'>Translate(Select)</option>");
        }
        if (IncludeAll) {
            $(TargetDropDown).append("<option value='-1'>Translate(All)</option>");
        }
        if (BindData) {
            $.each(BindData, function (index, elem) {
                $(TargetDropDown).append("<option value='" + elem.Value + "'" + "title='" + elem.Text + "'" + " >" + elem.Text + "</option>");
            });
        }
        if (SelectValue != -1) {
            $(TargetDropDown).val(SelectValue);
        }
        if (IncludeOthers) {
            $(TargetDropDown).append("<option value='-2'>Others</option>");
        }
    },
    FillDate: function (inputID, hdfieldID, isHtml, setDefaultDate) {
        ///<summary>
        /// Fill Input Text field with current date in "dd-MMM-yyyy" format.
        ///</summary>
        /// <param name="inputID" optional="true" type="String">
        ///     TextBox ID
        /// </param>
        /// <param name="hdfieldID" optional="true" type="String">
        ///     The hidden field id used for the set max date or min date
        /// </param>
        var now = new Date();
        var dd = (now.getDate() < 10) ? "0" + now.getDate() : now.getDate();
        var mm = now.getMonth();
        var yyyy = now.getFullYear();
        var mmm = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        mm = mmm[mm];
        var toDate = dd + "-" + mm + "-" + yyyy;
        var TargetinputID = '[id$=' + inputID + ']';
        if (isHtml) {
            if (setDefaultDate == undefined || setDefaultDate)
                $(TargetinputID).html(toDate);
        }
        else {
            if (setDefaultDate == undefined || setDefaultDate)
                $(TargetinputID).val(toDate);
        }
        if (hdfieldID) {
            var TargetHiddenFldID = 'input[id$=' + hdfieldID + ']';
            $(TargetHiddenFldID).val(toDate);
        }
    },

    //16-Nov-2011 Vineeth For Grouping in grid View For Radio Button
    EnableRbtnGrouping: function (spanChk) {
        ///<summary>
        /// Used to un Check other Radio buttons in the Grid
        ///</summary>
        /// <param name="mode" optional="true" type="String">
        /// Mode = 1 Determins ites on View Mode
        /// Mode = 2 Indicates its on New Mode
        /// </param> 
        //Mode = 1 Indicates its on View Mode
        //Mode = 2 Indicates its on New Mode

        var IsChecked = spanChk.checked;
        var CurrentRdbID = spanChk.id;
        //Finding the Grid Id with RadioButoon as Parent
        var gridID = $(spanChk).parents("table:first").attr("id");
        $("[id$=" + gridID + "]").find("tr:has(td)").each(function () {
            var id = $(this).find("td:first input").attr("id");
            if (id != CurrentRdbID) {
                $(this).find("td:first input").attr("checked", false);
            }
        });
        if (typeof AfterRbtnSelect == 'function') {
            AfterRbtnSelect(gridID);
        }
    },
    //16-Nov-2011 Vineeth For Grouping in grid View For Radio Button
    EnableRbtnGrouping2: function (spanChk) {
        ///<summary>
        /// Used to un Check other Radio buttons in the Grid
        ///</summary>
        /// <param name="mode" optional="true" type="String">
        /// Mode = 1 Determins ites on View Mode
        /// Mode = 2 Indicates its on New Mode
        /// </param> 
        //Mode = 1 Indicates its on View Mode
        //Mode = 2 Indicates its on New Mode

        var IsChecked = spanChk.checked;
        var CurrentRdbID = spanChk.id;
        //Finding the Grid Id with RadioButoon as Parent
        var gridID = $(spanChk).parents("table:first").attr("id");
        $("[id$=" + gridID + "]").find("tr:has(td)").each(function () {
            var id = $(this).find("td:nth-child(2) input").attr("id");
            if (id != CurrentRdbID) {
                $(this).find("td:nth-child(2) input").attr("checked", false);
            }
        });
        if (typeof AfterRbtnSelect == 'function') {
            AfterRbtnSelect(gridID);
        }
    },
    //16-Nov-2011 Vineeth For Grouping in grid View For Radio Button
    EnableRbtnGroupingHierarchy: function (spanChk, gridID) {
        ///<summary>
        /// Used to un Check other Radio buttons in the Grid
        ///</summary>
        /// <param name="mode" optional="true" type="String">
        /// Mode = 1 Determins ites on View Mode
        /// Mode = 2 Indicates its on New Mode
        /// </param> 
        //Mode = 1 Indicates its on View Mode
        //Mode = 2 Indicates its on New Mode

        var IsChecked = spanChk.checked;
        var CurrentRdbID = spanChk.id;
        //Finding the Grid Id with RadioButoon as Parent
        if (!gridID) {
            var gridID = $(spanChk).parents("table:first").attr("id");
            var gridIDList = gridID.split('_');
            gridID = gridIDList[gridIDList.length - 1];
        }
        $("[id$=" + gridID + "]").find("tr:has(td)").each(function () {
            var id = $(this).find("td:nth-child(2) input[type=radio]").attr("id");
            if (id != CurrentRdbID) {
                $("#" + id).attr("checked", false);
            }
        });
        if (typeof AfterRbtnSelect == 'function') {
            AfterRbtnSelect(gridID);
        }
    },
    RemoveDropDown: function (DropDownID, BindData, SelectValue) {
        ///<summary>Removes the supplied Data Items from the Dropdown control</summary>
        /// <param name="DropDownID" optional="true" type="String">
        ///     DropDown ID
        /// </param>
        /// <param name="BindData" optional="true" type="Object">
        ///     The Result Data 
        /// </param>
        /// <param name="SelectValue" optional="true" type="String">
        ///     The SelectValue Set DropDown Selected
        /// </param>
        var TargetDropDown = $("#" + DropDownID);
        $.each(BindData, function (index, elem) {
            $(TargetDropDown + 'option[value=' + elem.Value + ']').remove();
        });
        if (SelectValue) {
            $(TargetDropDown).Val(SelectValue);
        }

    },

    MakeInputFloat: function (targetInputID, maxLength, decimalPrec) {

        $(targetInputID).keypress(function (event) {
            //            var regx = /^([-]?)([0-9]{1,maxLength})((.[0-9]{1,decimalPrec})?)$/;
            //            if (!(regx.test($(targetInputID).val()))) {
            //                return false;
            //            }
            var keyVal = event.keyCode;
            if ((keyVal > 48 && keyVal < 57))// Numbers
            {
                return false;
            }

        });
    },

    MakeAutoCompleteComboBox: function (targetControlID, url, targetHiddenID, targetHiddenIDForCode, makeCombo, setDropDown, searchType, className, needSingleBinding, needDefaultBinding, keepInvalidSelection, minimumLength, selectText) {
        ///<summary>
        /// To make the autocomplete functionality
        ///</summary>
        /// <param name="targetControlID" optional="true" type="String">
        ///     Autocomplete TextBox ID
        /// </param>
        /// <param name="url" optional="true" type="String">
        ///     URL - to which the ajax request is made
        /// </param>
        /// <param name="targetHiddenID" optional="true" type="String">
        ///     Set The Selected Value ID
        /// </param>
        /// <param name="makeCombo" optional="true" type="Bool">
        ///     To Make the Autocomplete Text Box as a Combo
        /// </param>
        /// <param name="setDropDown" optional="true" type="Bool">
        ///     when true will act like dropdown id when search for a not containing value will set to select.
        /// </param> 
        /// <param name="keepInvalidSelection" optional="false" type="Bool">
        ///     If invalid selection, not clear the text
        /// </param> 
        /// <param name="selectText" optional="" type="String">
        ///     Set Default select text 
        /// </param> 
        selectText = selectText == undefined ? 'Select/Type' : selectText;
        var length = 0;
        if (minimumLength)
            length = minimumLength;

        var validationLabel = $("[id$=" + targetControlID + "]").val();
        $("[id$=" + targetControlID + "]").autocomplete({
            search: function () { $(this).addClass("ui-autocomplete-loading"); },
            open: function () {
                $(this).removeClass("ui-autocomplete-loading");
                var autowidget = $("[id$=" + targetControlID + "]").autocomplete("widget")[0];
                if (parseFloat($(autowidget).css("max-width").replace("px", "")) < parseFloat($(this)[0].clientWidth)) {
                    $(autowidget).removeClass("ui-menu");
                    $(autowidget).addClass("ui-menu-large");
                    $(autowidget).css({ "max-width": $(this)[0].clientWidth + "px!important" });
                }
            },
            minLength: length,
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    data: {
                        SearchValue: request.term,
                        SearchType: searchType
                    },
                    success: function (data) {
                        if (data == null || data == "") {
                            $("[id$=" + targetHiddenID + "]").val(-1);
                        }
                        response($.map(data, function (item) {
                            return {
                                label: item.Text, // format the the data as text 
                                id: item.Value,
                                code: item.Code
                            }
                        }));
                    }
                });
            },
            cache: false,
            select: function (event, ui) {
                $("[id$=" + targetControlID + "]").val(ui.item.label);
                $("[id$=" + targetControlID + "]").attr("title", ui.item.label);
                $("[id$=" + targetHiddenID + "]").val(ui.item.id);
                if (targetHiddenIDForCode != "") {
                    $("[id$=" + targetHiddenIDForCode + "]").val(ui.item.code);
                }
                validationLabel = $("[id$=" + targetControlID + "]").val();
                if (typeof AfterAutoCompleteSelect == 'function') {

                    // if any more function want to done after the result is selected from auto complete
                    AfterAutoCompleteSelect(targetControlID);
                }
            },
            change: function (event, ui) {
                validationLabel = $("[id$=" + targetControlID + "]").val();
                if (!ui.item) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
					valid = false;
                    if (setDropDown) {
                        $(this).children("option").each(function () {
                            if ($(this).text().match(matcher)) {
                                this.selected = valid = true;
                                return false;
                            }
                        });
                        //if (!valid) {
                        if (valid == false && keepInvalidSelection != true) {

                            // remove invalid value, as it didn't match anything
                            $(this).val(selectText);
                            $("[id$=" + targetHiddenID + "]").val(0);
                            if (targetHiddenIDForCode != "") {
                                $("[id$=" + targetHiddenIDForCode + "]").val("");
                            }
                            $(this).data("autocomplete").term = "";
                            if (typeof AfterInvalidSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                                AfterInvalidSelect(targetControlID);
                            }
                            return false;
                        }
                    }
                }
            }
        });
        if (!className) {
            className = "ddlSelect";
        }
        if (makeCombo) {
            if (setDropDown) {
                if ($("[id$=" + targetControlID + "]").val() == "") {
                    $("[id$=" + targetControlID + "]").val(selectText);
                }
            }
            $("[id$=" + targetControlID + "]").next("a").remove();
            this.anchor = $("<a>")
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.insertAfter($("[id$=" + targetControlID + "]"))
            //.removeClass("ui-corner-all")
					.addClass(className)
					.click(function () {
					    // close if already visible
					    if ($("[id$=" + targetControlID + "]").autocomplete("widget").is(":visible")) {
					        $("[id$=" + targetControlID + "]").autocomplete("close");
					        return;
					    }

					    // pass empty string as value to search for, displaying all results
					    $("[id$=" + targetControlID + "]").autocomplete("search", " ");
					    $("[id$=" + targetControlID + "]").focus();
					});

        }
        $("[id$=" + targetControlID + "]").click(function () {
            $(this).select();
        });
        $("[id$=" + targetControlID + "]").focusout(function () {
            //                var isValidData = false;
            //                if ($(this).autocomplete("widget").children(".ui-menu-item").length > 0) {
            //                    $(this).autocomplete("widget").children(".ui-menu-item").each(function () {
            //                        var labelMatcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($("[id$=" + targetControlID + "]").val()) + "$", "i");
            //                        isValidData = $(this).data("item.autocomplete").label.match(labelMatcher);
            //                    });
            //                }

            //            if (validationLabel != $("[id$=" + targetControlID + "]").val()) {
            //                $("[id$=" + targetControlID + "]").val("Select/Type");
            //                $("[id$=" + targetHiddenID + "]").val(0);
            //            }
        });
        if (needSingleBinding) {
            if ($("[id$=" + targetControlID + "]").val() == "" || $("[id$=" + targetControlID + "]").val() == selectText) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    data: {
                        SearchValue: "",
                        SearchType: searchType
                    },
                    success: function (data) {
                        if (data != null && data.length == 1) {
                            $("[id$=" + targetControlID + "]").val(data[0].Text);
                            $("[id$=" + targetHiddenID + "]").val(data[0].Value);
                            if (targetHiddenIDForCode != "") {
                                $("[id$=" + targetHiddenIDForCode + "]").val(data[0].Code);
                            }
                            if (typeof AfterAutoCompleteSelect == 'function') {

                                // if any more function want to done after the result is selected from auto complete
                                AfterAutoCompleteSelect(targetControlID);
                            }
                        }
                    }
                });
            }
        }
        else if (needDefaultBinding) {
            if ($("[id$=" + targetControlID + "]").val() == "" || $("[id$=" + targetControlID + "]").val() == selectText) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    data: {
                        SearchValue: "",
                        SearchType: searchType
                    },
                    success: function (data) {
                        if (data != null && data.length >= 1) {
                            $("[id$=" + targetControlID + "]").val(data[0].Text);
                            $("[id$=" + targetHiddenID + "]").val(data[0].Value);
                            if (targetHiddenIDForCode != "") {
                                $("[id$=" + targetHiddenIDForCode + "]").val(data[0].Code);
                            }
                            if (typeof AfterAutoCompleteSelect == 'function') {

                                // if any more function want to done after the result is selected from auto complete
                                AfterAutoCompleteSelect(targetControlID);
                            }
                        }
                    }
                });
            }
        }
    },

    ///New Code Added By Juno

    MakeAutoCompleteDDL: function (targetControlID, url, targetHiddenID, makeCombo, setDropDown, searchType, className, needSingleBinding, needDefaultBinding, keepInvalidSelection, minimumLength, selectText) {
        ///<summary>
        /// To make the autocomplete functionality
        ///</summary>
        /// <param name="targetControlID" optional="true" type="String">
        ///     Autocomplete TextBox ID
        /// </param>
        /// <param name="url" optional="true" type="String">
        ///     URL - to which the ajax request is made
        /// </param>
        /// <param name="targetHiddenID" optional="true" type="String">
        ///     Set The Selected Value ID
        /// </param>
        /// <param name="makeCombo" optional="true" type="Bool">
        ///     To Make the Autocomplete Text Box as a Combo
        /// </param>
        /// <param name="setDropDown" optional="true" type="Bool">
        ///     when true will act like dropdown id when search for a not containing value will set to select.
        /// </param> 
        /// <param name="keepInvalidSelection" optional="false" type="Bool">
        ///     If invalid selection, not clear the text
        /// </param> 
        /// <param name="selectText" optional="" type="String">
        ///     Set Default select text 
        /// </param> 
        selectText = selectText == undefined ? 'Select/Type' : selectText;
        var length = 0;
        if (minimumLength)
            length = minimumLength;

        var validationLabel = $("[id$=" + targetControlID + "]").val();
        $("[id$=" + targetControlID + "]").autocomplete({
            search: function () { $(this).addClass("ui-autocomplete-loading"); },
            open: function () {
                $(this).removeClass("ui-autocomplete-loading");
                var autowidget = $("[id$=" + targetControlID + "]").autocomplete("widget")[0];
                if (parseFloat($(autowidget).css("max-width").replace("px", "")) < parseFloat($(this)[0].clientWidth)) {
                    $(autowidget).removeClass("ui-menu");
                    $(autowidget).addClass("ui-menu-large");
                    $(autowidget).css({ "max-width": $(this)[0].clientWidth + "px!important" });
                }
            },
            minLength: length,
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    data: {
                        SearchValue: request.term,
                        SearchType: searchType
                    },
                    success: function (data) {
                        if (data == null || data == "") {
                            $("[id$=" + targetHiddenID + "]").val(-1);
                        }
                        response($.map(data, function (item) {
                            return {
                                label: item.Text, // format the the data as text 
                                id: item.Value
                            }
                        }));
                    }
                });
            },
            cache: false,
            select: function (event, ui) {
                $("[id$=" + targetControlID + "]").val(ui.item.label);
                $("[id$=" + targetControlID + "]").attr("title", ui.item.label);
                $("[id$=" + targetHiddenID + "]").val(ui.item.id);
                validationLabel = $("[id$=" + targetControlID + "]").val();
                if (typeof AfterAutoCompleteSelect == 'function') {

                    // if any more function want to done after the result is selected from auto complete
                    AfterAutoCompleteSelect(targetControlID);
                }
            },
            change: function (event, ui) {
                validationLabel = $("[id$=" + targetControlID + "]").val();
                if (!ui.item) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
					valid = false;
                    if (setDropDown) {
                        $(this).children("option").each(function () {
                            if ($(this).text().match(matcher)) {
                                this.selected = valid = true;
                                return false;
                            }
                        });
                        //if (!valid) {
                        if (valid == false && keepInvalidSelection != true) {

                            // remove invalid value, as it didn't match anything
                            $(this).val(selectText);
                            $("[id$=" + targetHiddenID + "]").val(0);
                            $(this).data("autocomplete").term = "";
                            if (typeof AfterInvalidSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                                AfterInvalidSelect(targetControlID);
                            }
                            return false;
                        }
                    }
                }
            }
        });
        if (!className) {
            className = "ddlSelect";
        }
        if (makeCombo) {
            if (setDropDown) {
                if ($("[id$=" + targetControlID + "]").val() == "") {
                    $("[id$=" + targetControlID + "]").val(selectText);
                }
            }
            $("[id$=" + targetControlID + "]").next("a").remove();
            this.anchor = $("<a>")
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.insertAfter($("[id$=" + targetControlID + "]"))
            //.removeClass("ui-corner-all")
					.addClass(className)
					.click(function () {
					    // close if already visible
					    if ($("[id$=" + targetControlID + "]").autocomplete("widget").is(":visible")) {
					        $("[id$=" + targetControlID + "]").autocomplete("close");
					        return;
					    }

					    // pass empty string as value to search for, displaying all results
					    $("[id$=" + targetControlID + "]").autocomplete("search", " ");
					    $("[id$=" + targetControlID + "]").focus();
					});

        }
        $("[id$=" + targetControlID + "]").click(function () {
            $(this).select();
        });
        $("[id$=" + targetControlID + "]").focusout(function () {
            //                var isValidData = false;
            //                if ($(this).autocomplete("widget").children(".ui-menu-item").length > 0) {
            //                    $(this).autocomplete("widget").children(".ui-menu-item").each(function () {
            //                        var labelMatcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($("[id$=" + targetControlID + "]").val()) + "$", "i");
            //                        isValidData = $(this).data("item.autocomplete").label.match(labelMatcher);
            //                    });
            //                }

            //            if (validationLabel != $("[id$=" + targetControlID + "]").val()) {
            //                $("[id$=" + targetControlID + "]").val("Select/Type");
            //                $("[id$=" + targetHiddenID + "]").val(0);
            //            }
        });
        if (needSingleBinding) {
            if ($("[id$=" + targetControlID + "]").val() == "" || $("[id$=" + targetControlID + "]").val() == selectText) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    data: {
                        SearchValue: "",
                        SearchType: searchType
                    },
                    success: function (data) {
                        if (data != null && data.length == 1) {
                            $("[id$=" + targetControlID + "]").val(data[0].Text);
                            $("[id$=" + targetHiddenID + "]").val(data[0].Value);
                            if (typeof AfterAutoCompleteSelect == 'function') {

                                // if any more function want to done after the result is selected from auto complete
                                AfterAutoCompleteSelect(targetControlID);
                            }
                        }
                    }
                });
            }
        }
        else if (needDefaultBinding) {
            if ($("[id$=" + targetControlID + "]").val() == "" || $("[id$=" + targetControlID + "]").val() == selectText) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    data: {
                        SearchValue: "",
                        SearchType: searchType
                    },
                    success: function (data) {
                        if (data != null && data.length >= 1) {
                            $("[id$=" + targetControlID + "]").val(data[0].Text);
                            $("[id$=" + targetHiddenID + "]").val(data[0].Value);
                            if (typeof AfterAutoCompleteSelect == 'function') {

                                // if any more function want to done after the result is selected from auto complete
                                AfterAutoCompleteSelect(targetControlID);
                            }
                        }
                    }
                });
            }
        }
    },

    ///New Code Added By Shinto

    MakeAutoCompleteDDLNEW: function (targetControlID, url, targetHiddenID, makeCombo, setDropDown,minimumLength, searchType, className, needSingleBinding, needDefaultBinding, keepInvalidSelection,  selectText) {
        ///<summary>
        /// To make the autocomplete functionality
        ///</summary>
        /// <param name="targetControlID" optional="true" type="String">
        ///     Autocomplete TextBox ID
        /// </param>
        /// <param name="url" optional="true" type="String">
        ///     URL - to which the ajax request is made
        /// </param>
        /// <param name="targetHiddenID" optional="true" type="String">
        ///     Set The Selected Value ID
        /// </param>
        /// <param name="makeCombo" optional="true" type="Bool">
        ///     To Make the Autocomplete Text Box as a Combo
        /// </param>
        /// <param name="setDropDown" optional="true" type="Bool">
        ///     when true will act like dropdown id when search for a not containing value will set to select.
        /// </param> 
        /// <param name="keepInvalidSelection" optional="false" type="Bool">
        ///     If invalid selection, not clear the text
        /// </param> 
        /// <param name="selectText" optional="" type="String">
        ///     Set Default select text 
        /// </param> 
        selectText = selectText == undefined ? 'Select/Type' : selectText;
        var length = 0;
        if (minimumLength)
            length = minimumLength;

        var validationLabel = $("[id$=" + targetControlID + "]").val();
        $("[id$=" + targetControlID + "]").autocomplete({
            search: function () { $(this).addClass("ui-autocomplete-loading"); },
            open: function () {
                $(this).removeClass("ui-autocomplete-loading");
                var autowidget = $("[id$=" + targetControlID + "]").autocomplete("widget")[0];
                if (parseFloat($(autowidget).css("max-width").replace("px", "")) < parseFloat($(this)[0].clientWidth)) {
                    $(autowidget).removeClass("ui-menu");
                    $(autowidget).addClass("ui-menu-large");
                    $(autowidget).css({ "max-width": $(this)[0].clientWidth + "px!important" });
                }
            },
            minLength: length,
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    data: {
                        SearchValue: request.term,
                        SearchType: searchType
                    },
                    success: function (data) {
                        if (data == null || data == "") {
                            $("[id$=" + targetHiddenID + "]").val(-1);
                        }
                        response($.map(data, function (item) {
                            return {
                                label: item.Text, // format the the data as text 
                                id: item.Value
                            }
                        }));
                    }
                });
            },
            cache: false,
            select: function (event, ui) {
                $("[id$=" + targetControlID + "]").val(ui.item.label);
                $("[id$=" + targetControlID + "]").attr("title", ui.item.label);
                $("[id$=" + targetHiddenID + "]").val(ui.item.id);
                validationLabel = $("[id$=" + targetControlID + "]").val();
                if (typeof AfterAutoCompleteSelect == 'function') {

                    // if any more function want to done after the result is selected from auto complete
                    AfterAutoCompleteSelect(targetControlID);
                }
            },
            change: function (event, ui) {
                validationLabel = $("[id$=" + targetControlID + "]").val();
                if (!ui.item) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
					valid = false;
                    if (setDropDown) {
                        $(this).children("option").each(function () {
                            if ($(this).text().match(matcher)) {
                                this.selected = valid = true;
                                return false;
                            }
                        });
                        //if (!valid) {
                        if (valid == false && keepInvalidSelection != true) {

                            // remove invalid value, as it didn't match anything
                            $(this).val(selectText);
                            $("[id$=" + targetHiddenID + "]").val(0);
                            $(this).data("autocomplete").term = "";
                            if (typeof AfterInvalidSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                                AfterInvalidSelect(targetControlID);
                            }
                            return false;
                        }
                    }
                }
            }
        });
        if (!className) {
            className = "ddlSelect";
        }
        if (makeCombo) {
            if (setDropDown) {
                if ($("[id$=" + targetControlID + "]").val() == "") {
                    $("[id$=" + targetControlID + "]").val(selectText);
                }
            }
            $("[id$=" + targetControlID + "]").next("a").remove();
            this.anchor = $("<a>")
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.insertAfter($("[id$=" + targetControlID + "]"))
            //.removeClass("ui-corner-all")
					.addClass(className)
					.click(function () {
					    // close if already visible
					    if ($("[id$=" + targetControlID + "]").autocomplete("widget").is(":visible")) {
					        $("[id$=" + targetControlID + "]").autocomplete("close");
					        return;
					    }

					    // pass empty string as value to search for, displaying all results
					    $("[id$=" + targetControlID + "]").autocomplete("search", " ");
					    $("[id$=" + targetControlID + "]").focus();
					});

        }
        $("[id$=" + targetControlID + "]").click(function () {
            $(this).select();
        });
        $("[id$=" + targetControlID + "]").focusout(function () {
            //                var isValidData = false;
            //                if ($(this).autocomplete("widget").children(".ui-menu-item").length > 0) {
            //                    $(this).autocomplete("widget").children(".ui-menu-item").each(function () {
            //                        var labelMatcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($("[id$=" + targetControlID + "]").val()) + "$", "i");
            //                        isValidData = $(this).data("item.autocomplete").label.match(labelMatcher);
            //                    });
            //                }

            //            if (validationLabel != $("[id$=" + targetControlID + "]").val()) {
            //                $("[id$=" + targetControlID + "]").val("Select/Type");
            //                $("[id$=" + targetHiddenID + "]").val(0);
            //            }
        });
        if (needSingleBinding) {
            if ($("[id$=" + targetControlID + "]").val() == "" || $("[id$=" + targetControlID + "]").val() == selectText) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    data: {
                        SearchValue: "",
                        SearchType: searchType
                    },
                    success: function (data) {
                        if (data != null && data.length == 1) {
                            $("[id$=" + targetControlID + "]").val(data[0].Text);
                            $("[id$=" + targetHiddenID + "]").val(data[0].Value);
                            if (typeof AfterAutoCompleteSelect == 'function') {

                                // if any more function want to done after the result is selected from auto complete
                                AfterAutoCompleteSelect(targetControlID);
                            }
                        }
                    }
                });
            }
        }
        else if (needDefaultBinding) {
            if ($("[id$=" + targetControlID + "]").val() == "" || $("[id$=" + targetControlID + "]").val() == selectText) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    data: {
                        SearchValue: "",
                        SearchType: searchType
                    },
                    success: function (data) {
                        if (data != null && data.length >= 1) {
                            $("[id$=" + targetControlID + "]").val(data[0].Text);
                            $("[id$=" + targetHiddenID + "]").val(data[0].Value);
                            if (typeof AfterAutoCompleteSelect == 'function') {

                                // if any more function want to done after the result is selected from auto complete
                                AfterAutoCompleteSelect(targetControlID);
                            }
                        }
                    }
                });
            }
        }
    },


    //Start//
    MakeAutoCompleteText: function (targetControlID, url, targetHiddenID, makeCombo, setDropDown, searchType, minimumLength, className) {
        ///<summary>
        /// To make the autocomplete functionality
        ///</summary>
        /// <param name="targetControlID" optional="true" type="String">
        ///     Autocomplete TextBox ID
        /// </param>
        /// <param name="url" optional="true" type="String">
        ///     URL - to which the ajax request is made
        /// </param>
        /// <param name="targetHiddenID" optional="true" type="String">
        ///     Set The Selected Value ID
        /// </param>
        /// <param name="makeCombo" optional="true" type="Bool">
        ///     To Make the Autocomplete Text Box as a Combo
        /// </param>
        /// <param name="setDropDown" optional="true" type="Bool">
        ///     when true will act like dropdown id when search for a not containing value will set to select.
        /// </param> 
        var length = 0;
        if (minimumLength)
            length = minimumLength;

        var validationLabel = $("[id$=" + targetControlID + "]").val();
        $("[id$=" + targetControlID + "]").autocomplete({
            search: function () { $(this).addClass("ui-autocomplete-loading"); },
            open: function () {
                $(this).removeClass("ui-autocomplete-loading");
                var autowidget = $("[id$=" + targetControlID + "]").autocomplete("widget")[0];
                if (parseFloat($(autowidget).css("max-width").replace("px", "")) < parseFloat($(this)[0].clientWidth)) {
                    $(autowidget).removeClass("ui-menu");
                    $(autowidget).addClass("ui-menu-large");
                    $(autowidget).css({ "max-width": $(this)[0].clientWidth + "px!important" });
                }
            },
            minLength: length,
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    data: {
                        SearchValue: request.term,
                        SearchType: searchType
                    },
                    success: function (data) {
                        if (data == null || data == "") {
                            $("[id$=" + targetHiddenID + "]").val(-1);
                        }
                        response($.map(data, function (item) {
                            return {
                                label: item.Text, // format the the data as text 
                                id: item.Value
                            }
                        }));
                    }
                });
            },
            cache: false,
            select: function (event, ui) {
                $("[id$=" + targetControlID + "]").val(ui.item.label);
                $("[id$=" + targetControlID + "]").attr("title", ui.item.label);
                $("[id$=" + targetHiddenID + "]").val(ui.item.id);
                validationLabel = $("[id$=" + targetControlID + "]").val();
                if (typeof AfterAutoCompleteSelect == 'function') {

                    // if any more function want to done after the result is selected from auto complete
                    AfterAutoCompleteSelect(targetControlID);
                }
            },
            change: function (event, ui) {
                validationLabel = $("[id$=" + targetControlID + "]").val();
                if (!ui.item) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
					valid = false;
                    if (setDropDown) {
                        $(this).children("option").each(function () {
                            if ($(this).text().match(matcher)) {
                                this.selected = valid = true;
                                return false;
                            }
                        });
                        if (!valid) {
                            // remove invalid value, as it didn't match anything
                            //    $(this).val(selectText);
                            $("[id$=" + targetHiddenID + "]").val(0);
                            $(this).data("autocomplete").term = "";
                            if (typeof AfterInvalidSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                                AfterInvalidSelect(targetControlID);
                            }
                            return false;
                        }
                    }
                }
            }
        });
//        if (!className) {
//            className = "ddlSelect";
//        }
//        if (makeCombo) {
//            if (setDropDown) {
//                if ($("[id$=" + targetControlID + "]").val() == "") {
//                    //$("[id$=" + targetControlID + "]").val(selectText);
//                }
//            }
//            $("[id$=" + targetControlID + "]").next("a").remove();
//            this.anchor = $("<a>")
//					.attr("tabIndex", -1)
//					.attr("title", "Show All Items")
//					.insertAfter($("[id$=" + targetControlID + "]"))
//            //.removeClass("ui-corner-all")
//					.addClass(className)
//					.click(function () {
//					    // close if already visible
//					    if ($("[id$=" + targetControlID + "]").autocomplete("widget").is(":visible")) {
//					        $("[id$=" + targetControlID + "]").autocomplete("close");
//					        return;
//					    }

//					    // pass empty string as value to search for, displaying all results
//					    $("[id$=" + targetControlID + "]").autocomplete("search", " ");
//					    $("[id$=" + targetControlID + "]").focus();
//					});

//        }
//        $("[id$=" + targetControlID + "]").click(function () {
//            $(this).select();
//        });
//        $("[id$=" + targetControlID + "]").focusout(function () {
//        });
    },
    //End//

    MakeAutoCompletePaired: function (targetControlID, pairedControlID, url, targetHiddenID, makeCombo, setDropDown, searchType, className) {
        ///<summary>
        /// To make the autocomplete functionality
        ///</summary>
        /// <param name="targetControlID" optional="true" type="String">
        ///     Autocomplete TextBox ID
        /// </param>
        /// <param name="pairedControlID" optional="true" type="String">
        ///     Autocomplete TextBox Paired with the target TextBox
        /// </param>
        /// <param name="url" optional="true" type="String">
        ///     URL - to which the ajax request is made
        /// </param>
        /// <param name="targetHiddenID" optional="true" type="String">
        ///     Set The Selected Value ID
        /// </param>
        /// <param name="makeCombo" optional="true" type="Bool">
        ///     To Make the Autocomplete Text Box as a Combo
        /// </param>
        /// <param name="setDropDown" optional="true" type="Bool">
        ///     when true will act like dropdown id when search for a not containing value will set to select.
        /// </param> 
        $("[id$=" + targetControlID + "]").autocomplete({
            search: function () { $(this).addClass("ui-autocomplete-loading"); },
            open: function () {
                $(this).removeClass("ui-autocomplete-loading");
                var autowidget = $("[id$=" + targetControlID + "]").autocomplete("widget")[0];
                if (parseFloat($(autowidget).css("max-width").replace("px", "")) < parseFloat($(this)[0].clientWidth)) {
                    $(autowidget).removeClass("ui-menu");
                    $(autowidget).addClass("ui-menu-large");
                    $(autowidget).css({ "max-width": $(this)[0].clientWidth + "px!important" });
                }
            },
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    url: url,
                    dataType: "json",
                    data: {
                        SearchValue: request.term,
                        SearchType: searchType
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Text, // format the the data as text 
                                id: item.Value,
                                pair: item.PairText
                            }
                        }));
                    }
                });
            },
            cache: false,
            select: function (event, ui) {

                $("[id$=" + targetControlID + "]").val(ui.item.label);
                $("[id$=" + targetHiddenID + "]").val(ui.item.id);
                $("[id$=" + pairedControlID + "]").val(ui.item.pair);

                if (typeof AfterAutoCompleteSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                    AfterAutoCompleteSelect(targetControlID);
                }
            },
            change: function (event, ui) {
                if (!ui.item) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
					valid = false;
                    if (setDropDown) {
                        $(this).children("option").each(function () {
                            if ($(this).text().match(matcher)) {
                                this.selected = valid = true;
                                return false;
                            }
                        });
                        if (!valid) {
                            // remove invalid value, as it didn't match anything
                            $(this).val("Select/Type");
                            $("[id$=" + targetHiddenID + "]").val(0);
                            if ($("[id$=" + pairedControlID + "]")[0].type == "hidden")
                                $("[id$=" + pairedControlID + "]").val(0);
                            else
                                $("[id$=" + pairedControlID + "]").val("Select/Type");
                            $(this).data("autocomplete").term = "";
                            if (typeof AfterInvalidSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                                AfterInvalidSelect(targetControlID);
                            }
                            return false;
                        }
                    }
                }
            }
        });
        if (!className) {
            className = "ddlSelect";
        }
        if (makeCombo) {
            if (setDropDown) {
                if ($("[id$=" + targetControlID + "]").val() == "") {
                    $("[id$=" + targetControlID + "]").val("Select/Type");
                }
            }
            $("[id$=" + targetControlID + "]").next("a").remove();
            this.anchor = $("<a>")
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.insertAfter($("[id$=" + targetControlID + "]"))
            //.removeClass("ui-corner-all")
					.addClass(className)
					.click(function () {
					    // close if already visible
					    if ($("[id$=" + targetControlID + "]").autocomplete("widget").is(":visible")) {
					        $("[id$=" + targetControlID + "]").autocomplete("close");
					        return;
					    }

					    // pass empty string as value to search for, displaying all results
					    $("[id$=" + targetControlID + "]").autocomplete("search", " ");
					    $("[id$=" + targetControlID + "]").focus();
					});

        }
        $("[id$=" + targetControlID + "]").click(function () {
            $(this).select();
        });

    },
    MakeAutoCompleteAdvance: function (targetControlID, url, targetHiddenID, makeCombo, targetLabel, targetDrpID, setDropDown, targetCorrDrpID, targetCorrDrpID1, className, keepInvalidSelection) {
        ///<summary>
        /// To make the autocomplete functionality
        ///</summary>
        /// <param name="targetControlID" optional="true" type="String">
        ///     Autocomplete TextBox ID
        /// </param>
        /// <param name="url" optional="true" type="String">
        ///     URL - to which the ajax request is made
        /// </param>
        /// <param name="targetHiddenID" optional="true" type="String">
        ///     Set The Selected Value ID
        /// </param>
        /// <param name="makeCombo" optional="true" type="Bool">
        ///     To Make the Autocomplete Text Box as a Combo
        /// </param>
        /// <param name="keepInvalidSelection" optional="false" type="Bool">
        ///     If invalid selection, not clear the text
        /// </param> 
        $("[id$=" + targetControlID + "]").autocomplete({
            open: function () {
                var autowidget = $("[id$=" + targetControlID + "]").autocomplete("widget")[0];
                if (parseFloat($(autowidget).css("max-width").replace("px", "")) < parseFloat($(this)[0].clientWidth)) {
                    $(autowidget).removeClass("ui-menu");
                    $(autowidget).addClass("ui-menu-large");
                    $(autowidget).css({ "max-width": $(this)[0].clientWidth + "px!important" });
                }
            },
            source: function (request, response) {
                $.ajax({
                    url: url,
                    data: {
                        SearchValue: request.term,
                        SearchType: $("[id$=" + targetDrpID + "]").val(),
                        SearchCorr: $("[id$=" + targetCorrDrpID + "]").val(),
                        SearchCorr1: $("[id$=" + targetCorrDrpID1 + "]").val()
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Text, // format the the data as text 
                                id: item.Value
                            }
                        }));
                    }
                });
            },
            cache: false,
            select: function (event, ui) {

                $("[id$=" + targetControlID + "]").val(ui.item.label);
                $("[id$=" + targetControlID + "]").attr("title", ui.item.label);
                $("[id$=" + targetHiddenID + "]").val(ui.item.id);
                if (targetLabel)
                    $("[id$=" + targetLabel + "]").val(ui.item.label);
                if (typeof AfterAutoCompleteSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                    AfterAutoCompleteSelect(targetControlID);
                }
            },
            change: function (event, ui) {
                if (!ui.item) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
					valid = false;
                    if (setDropDown) {
                        $(this).children("option").each(function () {
                            if ($(this).text().match(matcher)) {
                                this.selected = valid = true;
                                return false;
                            }
                        });
                        //                        if (!valid) {
                        if (valid == false && keepInvalidSelection != true || $(this).val() == "") {

                            // remove invalid value, as it didn't match anything

                            $(this).val("Translate(AutoDefaultValue)");
                            $("[id$=" + targetHiddenID + "]").val(0);
                            $(this).data("autocomplete").term = "";
                            return false;
                        }
                    }
                }
            }
        });
        if (!className) {
            className = "ddlSelect";
        }
        if (makeCombo) {
            if (setDropDown) {
                $("[id$=" + targetControlID + "]").val("Translate(AutoDefaultValue)");
            }
            $("[id$=" + targetControlID + "]").next("a").remove();
            this.anchor = $("<a>")
					.attr("tabIndex", -1)
					.attr("title", "Translate(ShowAllItems)")
					.insertAfter($("[id$=" + targetControlID + "]"))
            //.removeClass("ui-corner-all")
					.addClass(className)
					.click(function () {
					    // close if already visible
					    if ($("[id$=" + targetControlID + "]").autocomplete("widget").is(":visible")) {
					        $("[id$=" + targetControlID + "]").autocomplete("close");
					        return;
					    }

					    // pass empty string as value to search for, displaying all results
					    $("[id$=" + targetControlID + "]").autocomplete("search", "%");
					    $("[id$=" + targetControlID + "]").focus();
					});

        }

    },
    MakeAutoComplete: function (targetControlID, url, targetHiddenID, makeCombo, targetLabel, targetDrpID, setDropDown, targetCorrDrpID, targetCorrDrpID1, className, keepInvalidSelection) {
        ///<summary>
        /// To make the autocomplete functionality
        ///</summary>
        /// <param name="targetControlID" optional="true" type="String">
        ///     Autocomplete TextBox ID
        /// </param>
        /// <param name="url" optional="true" type="String">
        ///     URL - to which the ajax request is made
        /// </param>
        /// <param name="targetHiddenID" optional="true" type="String">
        ///     Set The Selected Value ID
        /// </param>
        /// <param name="makeCombo" optional="true" type="Bool">
        ///     To Make the Autocomplete Text Box as a Combo
        /// </param>
        /// <param name="keepInvalidSelection" optional="false" type="Bool">
        ///     If invalid selection, not clear the text
        /// </param> 
        $("[id$=" + targetControlID + "]").autocomplete({
            open: function () {
                var autowidget = $("[id$=" + targetControlID + "]").autocomplete("widget")[0];
                if (parseFloat($(autowidget).css("max-width").replace("px", "")) < parseFloat($(this)[0].clientWidth)) {
                    $(autowidget).removeClass("ui-menu");
                    $(autowidget).addClass("ui-menu-large");
                    $(autowidget).css({ "max-width": $(this)[0].clientWidth + "px!important" });
                }
            },
            source: function (request, response) {
                $.ajax({
                    url: url,
                    data: {
                        SearchValue: request.term,
                        SearchType: $("[id$=" + targetDrpID + "]").val(),
                        SearchCorr: $("[id$=" + targetCorrDrpID + "]").val(),
                        SearchCorr1: $("[id$=" + targetCorrDrpID1 + "]").val()
                    },
                    success: function (data) {
                        if (data.length == 0) {
                            if (typeof ReInitAutoComplete == 'function') { //Due to failure of autocomplete initialization,no data return.In such case we need to reinitialize autocomplete.
                                ReInitAutoComplete(targetControlID);
                            }
                        }
                        response($.map(data, function (item) {
                            return {
                                label: item.Text, // format the the data as text 
                                id: item.Value
                            }
                        }));
                    }
                });
            },
            cache: false,
            select: function (event, ui) {

                $("[id$=" + targetControlID + "]").val(ui.item.label);
                $("[id$=" + targetControlID + "]").attr("title", ui.item.label);
                $("[id$=" + targetHiddenID + "]").val(ui.item.id);
                if (targetLabel)
                    $("[id$=" + targetLabel + "]").val(ui.item.label);
                if (typeof AfterAutoCompleteSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                    AfterAutoCompleteSelect(targetControlID);
                }
            },
            change: function (event, ui) {
                if (!ui.item) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
					valid = false;
                    if (setDropDown) {
                        $(this).children("option").each(function () {
                            if ($(this).text().match(matcher)) {
                                this.selected = valid = true;
                                return false;
                            }
                        });
                        //                        if (!valid) {
                        if (valid == false && keepInvalidSelection != true || $(this).val() == "") {

                            // remove invalid value, as it didn't match anything

                            $(this).val("Translate(AutoDefaultValue)");
                            $("[id$=" + targetHiddenID + "]").val(0);
                            $(this).data("autocomplete").term = "";
                            return false;
                        }
                    }
                }
            }
        });
        if (!className) {
            className = "ddlSelect";
        }
        if (makeCombo) {
            if (setDropDown) {
                $("[id$=" + targetControlID + "]").val("Translate(AutoDefaultValue)");
            }
            $("[id$=" + targetControlID + "]").next("a").remove();
            this.anchor = $("<a>")
					.attr("tabIndex", -1)
					.attr("title", "Translate(ShowAllItems)")
					.insertAfter($("[id$=" + targetControlID + "]"))
            //.removeClass("ui-corner-all")
					.addClass(className)
					.click(function () {
					    // close if already visible
					    if ($("[id$=" + targetControlID + "]").autocomplete("widget").is(":visible")) {
					        $("[id$=" + targetControlID + "]").autocomplete("close");
					        return;
					    }

					    // pass empty string as value to search for, displaying all results
					    $("[id$=" + targetControlID + "]").autocomplete("search", "%");
					    $("[id$=" + targetControlID + "]").focus();
					});

        }

    },
    MakeAutoCompleteSearchItem: function (targetControlID, url, targetDrpID, makeCombo, targetSBU) {
        ///<summary>
        /// To make the Search Box functionality
        ///</summary>
        /// <param name="targetControlID" optional="true" type="String">
        ///     Autocomplete TextBox ID
        /// </param>
        /// <param name="url" optional="true" type="String">
        ///     URL - to which the ajax request is made
        /// </param>
        /// <param name="targetDrpID" optional="true" type="String">
        ///     Search By - Dropdown Control ID
        /// </param>
        /// <param name="makeCombo" optional="true" type="Bool">
        ///     To Make the Autocomplete Text Box as a Combo
        /// </param>

        $("[id$=" + targetControlID + "]").autocomplete({
            open: function () {
                var autowidget = $("[id$=" + targetControlID + "]").autocomplete("widget")[0];
                if (parseFloat($(autowidget).css("max-width").replace("px", "")) < parseFloat($(this)[0].clientWidth)) {
                    $(autowidget).removeClass("ui-menu");
                    $(autowidget).addClass("ui-menu-large");
                    $(autowidget).css({ "max-width": $(this)[0].clientWidth + "px!important" });
                }
            },
            source: function (request, response) {
                $.ajax({
                    url: url,
                    data: {
                        SearchValue: request.term,
                        SearchType: $("[id$=" + targetDrpID + "]").val(),
                        SBU: $("[id$=" + targetSBU + "]").val()
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Text // format the the data as text 
                            }
                        }));
                    }
                });
            },
            cache: false,
            select: function (event, ui) {
                $("[id$=" + targetControlID + "]").val(ui.item.label);
                if (typeof AfterSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                    AfterSelect();
                }



            }
        });

        if (makeCombo) {
            this.anchor = $("<a>&nbsp;</a>")
					.attr("tabIndex", -1)
					.attr("title", "Translate(ShowAllItems)")
					.insertAfter($("[id$=" + targetControlID + "]"))
            //.removeClass("ui-corner-all")
					.addClass("ddlSelect")
					.click(function () {
					    // close if already visible
					    if ($("[id$=" + targetControlID + "]").autocomplete("widget").is(":visible")) {
					        $("[id$=" + targetControlID + "]").autocomplete("close");
					        return;
					    }

					    // pass empty string as value to search for, displaying all results
					    $("[id$=" + targetControlID + "]").autocomplete("search", "%");
					    $("[id$=" + targetControlID + "]").focus();
					});

        }


    },
    MakeAutoCompleteSearch: function (targetControlID, url, targetDrpID, makeCombo, targetSBU) {
        ///<summary>
        /// To make the Search Box functionality
        ///</summary>
        /// <param name="targetControlID" optional="true" type="String">
        ///     Autocomplete TextBox ID
        /// </param>
        /// <param name="url" optional="true" type="String">
        ///     URL - to which the ajax request is made
        /// </param>
        /// <param name="targetDrpID" optional="true" type="String">
        ///     Search By - Dropdown Control ID
        /// </param>
        /// <param name="makeCombo" optional="true" type="Bool">
        ///     To Make the Autocomplete Text Box as a Combo
        /// </param>

        $("[id$=" + targetControlID + "]").autocomplete({
            open: function () {
                var autowidget = $("[id$=" + targetControlID + "]").autocomplete("widget")[0];
                if (parseFloat($(autowidget).css("max-width").replace("px", "")) < parseFloat($(this)[0].clientWidth)) {
                    $(autowidget).removeClass("ui-menu");
                    $(autowidget).addClass("ui-menu-large");
                    $(autowidget).css({ "max-width": $(this)[0].clientWidth + "px!important" });
                }
            },
            source: function (request, response) {
                $.ajax({
                    url: url,
                    data: {
                        SearchValue: request.term,
                        SearchType: $("[id$=" + targetDrpID + "]").val(),
                        SBU: $("[id$=" + targetSBU + "]").val()
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Text // format the the data as text 
                            }
                        }));
                    }
                });
            },
            cache: false,
            select: function (event, ui) {
                $("[id$=" + targetControlID + "]").val(ui.item.label);
                if (typeof AfterSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                    AfterSelect();
                }



            }
        });

        if (makeCombo) {
            this.anchor = $("<a>&nbsp;</a>")
					.attr("tabIndex", -1)
					.attr("title", "Translate(ShowAllItems)")
					.insertAfter($("[id$=" + targetControlID + "]"))
            //.removeClass("ui-corner-all")
					.addClass("ddlSelect")
					.click(function () {
					    // close if already visible
					    if ($("[id$=" + targetControlID + "]").autocomplete("widget").is(":visible")) {
					        $("[id$=" + targetControlID + "]").autocomplete("close");
					        return;
					    }

					    // pass empty string as value to search for, displaying all results
					    $("[id$=" + targetControlID + "]").autocomplete("search", "%");
					    $("[id$=" + targetControlID + "]").focus();
					});

        }


    },

    //By Juno 01-04-2014
    MakeAutoCompleteLimitLen: function (targetControlID, url, targetHiddenID, makeCombo, targetLabel, targetDrpID, setDropDown, targetCorrDrpID, targetCorrDrpID1, className, minimumLength, afterAutoComplete, selectText, keepInvalidSelection) {
        ///<summary>
        /// To make the autocomplete functionality
        ///</summary>
        /// <param name="targetControlID" optional="true" type="String">
        ///     Autocomplete TextBox ID
        /// </param>
        /// <param name="url" optional="true" type="String">
        ///     URL - to which the ajax request is made
        /// </param>
        /// <param name="targetHiddenID" optional="true" type="String">
        ///     Set The Selected Value ID
        /// </param>
        /// <param name="makeCombo" optional="true" type="Bool">
        ///     To Make the Autocomplete Text Box as a Combo
        /// </param>
        /// <param name="selectText" optional="" type="String">
        ///     Set Default select text 
        /// </param> 
        selectText = selectText == undefined ? 'Select/Type' : selectText;
        var length = 0;
        if (minimumLength)
            length = minimumLength;
        $("[id$=" + targetControlID + "]").autocomplete({
            open: function () {
                var autowidget = $("[id$=" + targetControlID + "]").autocomplete("widget")[0];
                if (parseFloat($(autowidget).css("max-width").replace("px", "")) < parseFloat($(this)[0].clientWidth)) {
                    $(autowidget).removeClass("ui-menu");
                    $(autowidget).addClass("ui-menu-large");
                    $(autowidget).css({ "max-width": $(this)[0].clientWidth + "px!important" });
                }
            },
            minLength: length,
            source: function (request, response) {
                $.ajax({
                    url: url,

                    data: {
                        SearchValue: request.term,
                        SearchType: $("[id$=" + targetDrpID + "]").val(),
                        SearchCorr: $("[id$=" + targetCorrDrpID + "]").val(),
                        SearchCorr1: $("[id$=" + targetCorrDrpID1 + "]").val()
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                label: item.Text, // format the the data as text 
                                id: item.Value
                            }
                        }));
                    }
                });
            },
            cache: false,
            select: function (event, ui) {

                $("[id$=" + targetControlID + "]").val(ui.item.label);
                $("[id$=" + targetControlID + "]").attr("title", ui.item.label);
                $("[id$=" + targetHiddenID + "]").val(ui.item.id);
                if (targetLabel)
                    $("[id$=" + targetLabel + "]").val(ui.item.label);
                if (typeof AfterAutoCompleteSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                    AfterAutoCompleteSelect(targetControlID);
                }
            },
            change: function (event, ui) {
                if (!ui.item) {
                    var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
					valid = false;
                    if (setDropDown) {
                        $(this).children("option").each(function () {
                            if ($(this).text().match(matcher)) {
                                this.selected = valid = true;
                                return false;
                            }
                        });

                        if (valid == false && keepInvalidSelection != true) {
                            // remove invalid value, as it didn't match anything                             
                            $(this).val(selectText);
                            $("[id$=" + targetHiddenID + "]").val(0);
                            $(this).data("autocomplete").term = "";
                            if (typeof AfterInvalidSelect == 'function') { // if any more function want to done after the result is selected from auto complete
                                AfterInvalidSelect(targetControlID);
                            }
                            return false;
                        }
                    }
                }
            }
        });
        if (!className) {
            className = "ddlSelect";
        }
        if (makeCombo) {
            if (setDropDown) {
                $("[id$=" + targetControlID + "]").val(selectText);
            }
            $("[id$=" + targetControlID + "]").next("a").remove();
            this.anchor = $("<a>")
					.attr("tabIndex", -1)
					.attr("title", "Translate(ShowAllItems)")
					.insertAfter($("[id$=" + targetControlID + "]"))
            //.removeClass("ui-corner-all")
					.addClass(className)
					.click(function () {
					    // close if already visible
					    if ($("[id$=" + targetControlID + "]").autocomplete("widget").is(":visible")) {
					        $("[id$=" + targetControlID + "]").autocomplete("close");
					        return;
					    }

					    // pass empty string as value to search for, displaying all results
					    $("[id$=" + targetControlID + "]").autocomplete("search", "%");
					    $("[id$=" + targetControlID + "]").focus();
					});

            if (typeof afterAutoComplete == 'function') { // if any more function want to done after the result is selected from auto complete
                afterAutoComplete();
            }
        }

    },

    ShowModal: function (content, title, command, showCancel) {
        ///<summary>
        ///     Used to Create ModelPopup for messges
        ///</summary>
        /// <param name="content" optional="true" type="String">
        ///     Used to show the content in the modal window
        /// </param>
        /// <param name="title" optional="true" type="String">
        ///     Used to Show the title for modal window
        /// </param>
        /// <param name="command" optional="true" type="String">
        ///     Which Operation need to perform after the OK button click
        /// </param>
        /// <param name="showCancel" optional="true" type="Bool">
        ///      determins need to show cancel button or not
        /// </param>
        if (showCancel) {
            $("#MSGBox").html(content).dialog({
                modal: true,
                title: title,
                resizable: false,
                beforeClose: function () {
                },
                buttons: {
                    OK: function () {
                        $(this).dialog("close");
                        if (typeof ModalOk == 'function') { // if any more function want to done in the ok click, please add the ModalOk function in page 
                            ModalOk(command); // command used to identifies the which action perfomed eg: save,delete,..
                        }

                    },
                    Cancel: function () {
                        $(this).dialog("close");
                        if (typeof ModalCancel == 'function') { // if any more function want to done in the ok click, please add the ModalOk function in page 
                            ModalCancel(command); // command used to identifies the which action perfomed eg: save,delete,..
                        }
                    }
                }
            });
        }
        else {
            $("#MSGBox").html(content).dialog({
                modal: true,
                title: title,
                resizable: false,
                beforeClose: function () {
                    if (typeof ModalOk == 'function') { // if any more function want to done in the ok click, please add the ModalOk function in page 
                        ModalOk(command); // command used to identifies the which action perfomed eg: save,delete,..
                    }
                },
                buttons: {
                    OK: function () {
                        $(this).dialog("close");
                    }

                }
            });
        }

    },

    ShowModalID: function (containerID, title, command, width, height, okBtn) {
        ///<summary>
        ///     Used to Create ModelPopup for any div to popup
        ///</summary>
        /// <param name="containerID" optional="true" type="String">
        ///     The id of popup div
        /// </param>
        /// <param name="title" optional="true" type="String">
        ///     Used to Show the title for modal window
        /// </param>
        /// <param name="command" optional="true" type="String">
        ///     Which Operation need to perform after the OK button click
        /// </param>
        /// <param name="width" optional="true" type="String">
        ///      Set the width of the Popup
        /// </param>
        /// <param name="height" optional="true" type="String">
        ///      Set the Height of the Popup
        /// </param>
        /// <param name="okBtn" optional="true" type="String">
        ///      Requried Ok Button In Popup 
        /// </param>
        if (!width) {
            width = 450
        }
        if (!height) {
            height = 450
        }
        if (okBtn == undefined) {
            okBtn = true;
        }
        $("#dialog:ui-dialog").dialog("destroy");
        $("#" + containerID).dialog('open');
        if (okBtn) {
            //        if (command) {

            $("#" + containerID).dialog({ width: width, height: height, resizable: false, title: title, buttons: {
                OK: function () {
                    $(this).dialog("close");
                    if (typeof ModalOk == 'function') { // if any more function want to done in the ok click, please add the ModalOk function in page 
                        ModalOk(command); // command means identifies the which action perfomed eg: save,delete,..
                    }
                    if (typeof ModalOk1 == 'function') { // if any more function want to done in the ok click, please add the ModalOk function in page 
                        ModalOk1(command); // command means identifies the which action perfomed eg: save,delete,..
                    }
                }
            }
            });
        }
        else {
            $("#" + containerID).dialog({ width: width, height: height, resizable: false, title: title });
        }





    },
    AddDateRangeAdvance: function (fromDate, hdnFrmDate, toDate, hdnToDate, format, restrictfromDate, restrictToDate, setDefaultDate) {
        ///<summary>
        ///     Used for From Date and To Date datepicker
        ///</summary>
        /// <param name="fromDate" optional="true" type="String">
        ///     The input id of the fromdate datepicker
        /// </param>
        /// <param name="hdnFrmDate" optional="true" type="String">
        ///     The hidden field id used for the set min date of the todate datepicker
        /// </param>
        /// <param name="toDate" optional="true" type="String">
        ///     The input id of the todate datepicker
        /// </param>
        /// <param name="hdnToDate" optional="true" type="String">
        ///      The hidden field id used for the set max date of the fromdate datepicker
        /// </param>
        /// <param name="format" optional="true" type="String">
        ///      Format of the datepicker
        /// </param>
        /// <param name="restrictfromDate" optional="true" type="bool">
        ///      true used for set the it will not allow to select the  the current before date
        /// </param>
        if (!format)
            format = "dd-M-yy";
        $('input[id$=' + fromDate + ']').datepicker({
            dateFormat: format,
            changeMonth: true,
            changeYear: true,
            onSelect: function (dateText, inst) {
                if (restrictToDate == undefined || restrictToDate) {
                    $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnFrmDate + "]").val()));
                }
                if (typeof AfterDateSelect == "function") {
                    AfterDateSelect(fromDate);
                }
            },
            altField: $("[id$=" + hdnFrmDate + "]"),
            altFormat: "mm/dd/yy",
            defaultDate: GrandScriptUtils.FillDate(fromDate, hdnFrmDate, false, setDefaultDate)
        });
        $('input[id$=' + toDate + ']').datepicker({
            dateFormat: format,
            changeMonth: true,
            changeYear: true,
            onSelect: function (dateText, inst) {
                if (restrictfromDate == undefined || restrictfromDate) {
                    $('input[id$=' + fromDate + ']').datepicker("option", "maxDate", new Date($("input[id$=" + hdnToDate + "]").val()));
                }
                if (typeof AfterDateSelect == "function") {
                    AfterDateSelect(toDate);
                }
            },
            altField: $("[id$=" + hdnToDate + "]"),
            altFormat: "mm/dd/yy"

        });
        if (restrictToDate == undefined || restrictToDate) {
            $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date());
        }
        if (restrictfromDate)
            $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date());

    },
    AddDateRange: function (fromDate, hdnFrmDate, toDate, hdnToDate, format, restrictfromDate, restrictToDate, setDefaultDate) {
        ///<summary>
        ///     Used for From Date and To Date datepicker
        ///</summary>
        /// <param name="fromDate" optional="true" type="String">
        ///     The input id of the fromdate datepicker
        /// </param>
        /// <param name="hdnFrmDate" optional="true" type="String">
        ///     The hidden field id used for the set min date of the todate datepicker
        /// </param>
        /// <param name="toDate" optional="true" type="String">
        ///     The input id of the todate datepicker
        /// </param>
        /// <param name="hdnToDate" optional="true" type="String">
        ///      The hidden field id used for the set max date of the fromdate datepicker
        /// </param>
        /// <param name="format" optional="true" type="String">
        ///      Format of the datepicker
        /// </param>
        /// <param name="restrictfromDate" optional="true" type="bool">
        ///      true used for set the it will not allow to select the  the current before date
        /// </param>
        if (!format)
            format = "dd-M-yy";
        $('input[id$=' + fromDate + ']').datepicker({
            dateFormat: format,
            changeMonth: true,
            changeYear: true,
            onSelect: function (dateText, inst) {
                if (restrictToDate == undefined || restrictToDate) {
                    $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnFrmDate + "]").val()));
                }
                if (typeof AfterDateSelect == "function") {
                    AfterDateSelect(fromDate);
                }
            },
            altField: $("[id$=" + hdnFrmDate + "]"),
            altFormat: "mm/dd/yy",
            defaultDate: GrandScriptUtils.FillDate(fromDate, hdnFrmDate, false, setDefaultDate)
        });
        $('input[id$=' + toDate + ']').datepicker({
            dateFormat: format,
            changeMonth: true,
            changeYear: true,
            onSelect: function (dateText, inst) {
                if (restrictfromDate == undefined || restrictfromDate) {
                    $('input[id$=' + fromDate + ']').datepicker("option", "maxDate", new Date($("input[id$=" + hdnToDate + "]").val()));
                }
                if (typeof AfterDateSelect == "function") {
                    AfterDateSelect(toDate);
                }
            },
            altField: $("[id$=" + hdnToDate + "]"),
            altFormat: "mm/dd/yy"

        });
        if (restrictToDate == undefined || restrictToDate) {
            $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date());
        }
        if (restrictfromDate)
            $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date());

    },

    AddDateTimeRange: function (fromDate, hdnFrmDate, toDate, hdnToDate, format, restrictfromDate, isToCaptureDuration, durationCtrl) {
        ///<summary>
        ///     Used for From Date and To Date datepicker
        ///</summary>
        /// <param name="fromDate" optional="true" type="String">
        ///     The input id of the fromdate datepicker
        /// </param>
        /// <param name="hdnFrmDate" optional="true" type="String">
        ///     The hidden field id used for the set min date of the todate datepicker
        /// </param>
        /// <param name="toDate" optional="true" type="String">
        ///     The input id of the todate datepicker
        /// </param>
        /// <param name="hdnToDate" optional="true" type="String">
        ///      The hidden field id used for the set max date of the fromdate datepicker
        /// </param>
        /// <param name="format" optional="true" type="String">
        ///      Format of the datepicker
        /// </param>
        /// <param name="restrictfromDate" optional="true" type="bool">
        ///      true used for set the it will not allow to select the  the current befo    re date
        /// </param>
        /// <param name="isToCaptureDuration" optional="true" type="bool">
        ///      whether or not to capture duration
        /// </param>

        /// <param name="durationCtrl" optional="true" type="string">
        ///      Control to which duration is to be caprtured
        /// </param>

        if (!format)
            format = "dd/mmm/yyyy hh:MM TT";
        var restDate = new Date();
        restDate.setDate(restDate.getDate() - 1);
        $('input[id$=' + fromDate + ']').datetimepicker({
            dateFormat: format,
            onSelect: function (dateText, inst) {
                if (parseInt(inst.currentMinute) < 10) {
                    inst.currentMinute = "0" + inst.currentMinute;
                }

                var fDate = inst.currentMonth + "/" + inst.currentDay + "/" + inst.currentYear + " " + inst.currentHour + ":" + inst.currentMinute + " " + inst.currentAMPM;
                //min date set to date-1
                var mdate = new Date(fDate);
                $("input[id$=" + hdnFrmDate + "]").val(mdate);
                mdate.setDate(mdate.getDate() - 1);
                // $('input[id$=' + toDate + ']').datetimepicker("option", "minDate", mdate);
                //check from time is greater than to time
                var frmdate = new Date(fDate);
                var todate = new Date($('input[id$=' + hdnToDate + ']').val());
                if (frmdate > todate) {
                    $('input[id$=' + toDate + ']').val($('input[id$=' + fromDate + ']').val())
                    todate = frmdate;
                }
                //capture duration if needed
                if (isToCaptureDuration) {
                    // var duration = (todate - frmdate) / (1000 * 60 * 60);
                    var duration = (todate - frmdate) / (1000 * 60);
                    if (!isNaN(duration) && (duration > 0)) {
                        if (duration >= 60) {
                            //$('input[id$=' + durationCtrl + ']').val(GrandScriptUtils.Round(duration, 2));
                            // Convert to Hours
                            var durationInMin = (duration / 60);
                            var timeArr = String(durationInMin).split('.');
                            duration = GrandScriptUtils.RoundTime(durationInMin, 2);
                            $('input[id$=' + durationCtrl + ']').val(duration);
                        }
                        else {
                            if (duration < 10) {
                                duration = "0.0" + duration;
                            }
                            else {
                                duration = "0." + duration;
                            }
                            $('input[id$=' + durationCtrl + ']').val(duration);
                        }

                    }
                    else {
                        $('input[id$=' + durationCtrl + ']').val("");
                    }
                }

                if (typeof AfterDateTimeSelect == "function") {
                    AfterDateTimeSelect();
                }
            }
        });
        $('input[id$=' + toDate + ']').datetimepicker({
            dateFormat: format,
            changeMonth: true,
            onSelect: function (dateText, inst) {
                if (parseInt(inst.currentMinute) < 10) {
                    inst.currentMinute = "0" + inst.currentMinute;
                }
                var tDate = inst.currentMonth + "/" + inst.currentDay + "/" + inst.currentYear + " " + inst.currentHour + ":" + inst.currentMinute + " " + inst.currentAMPM;
                $("input[id$=" + hdnToDate + "]").val(new Date(tDate));
                // $('input[id$=' + fromDate + ']').datetimepicker("option", "maxDate", new Date(tDate));
                var frmdate = new Date($('input[id$=' + hdnFrmDate + ']').val());
                var todate = new Date(tDate);
                if (frmdate > todate) {
                    $('input[id$=' + fromDate + ']').val($('input[id$=' + toDate + ']').val())
                    frmdate = todate;
                }
                //capture duration if needed
                if (isToCaptureDuration) {
                    //var duration = (todate - frmdate) / (1000 * 60 * 60);
                    var duration = (todate - frmdate) / (1000 * 60);
                    if (!isNaN(duration) && (duration > 0)) {
                        //alert(duration);
                        if (duration >= 60) {
                            var durationInMin = (duration / 60);
                            //var timeArr =String(durationInMin).split('.');
                            duration = GrandScriptUtils.RoundTime(durationInMin, 2);
                            $('input[id$=' + durationCtrl + ']').val(duration);
                            // Convert to Hours
                        }
                        else {
                            if (duration < 10) {
                                duration = "0.0" + duration;
                            }
                            else {
                                duration = "0." + duration;
                            }
                            $('input[id$=' + durationCtrl + ']').val(duration);
                        }
                    }
                    else {
                        $('input[id$=' + durationCtrl + ']').val("");
                    }
                }

                if (typeof AfterDateTimeSelect == "function") {
                    AfterDateTimeSelect();
                }
            }
        });
        //  $('input[id$=' + toDate + ']').datetimepicker("option", "minDate", restDate);
        if (restrictfromDate) {
            $('input[id$=' + fromDate + ']').datetimepicker("option", "minDate", restDate);
            $('input[id$=' + toDate + ']').datetimepicker("option", "minDate", restDate);
        }
    },

    AddDateRangeInSameMonth: function (fromDate, hdnFrmDate, toDate, hdnToDate, format, restrictfromDate) {
        ///<summary>
        ///     Used for From Date and To Date datepicker
        ///</summary>
        /// <param name="fromDate" optional="true" type="String">
        ///     The input id of the fromdate datepicker
        /// </param>
        /// <param name="hdnFrmDate" optional="true" type="String">
        ///     The hidden field id used for the set min date of the todate datepicker
        /// </param>
        /// <param name="toDate" optional="true" type="String">
        ///     The input id of the todate datepicker
        /// </param>
        /// <param name="hdnToDate" optional="true" type="String">
        ///      The hidden field id used for the set max date of the fromdate datepicker
        /// </param>
        /// <param name="format" optional="true" type="String">
        ///      Format of the datepicker
        /// </param>
        /// <param name="restrictfromDate" optional="true" type="bool">
        ///      true used for set the it will not allow to select the  the current before date
        /// </param>
        if (!format)
            format = "dd-M-yy";
        $('input[id$=' + fromDate + ']').datepicker({
            dateFormat: format,
            changeMonth: true,
            changeYear: true,
            onSelect: function (dateText, inst) {
                $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnFrmDate + "]").val()));
            },
            altField: $("[id$=" + hdnFrmDate + "]"),
            altFormat: "mm/dd/yy",
            defaultDate: GrandScriptUtils.FillDate(fromDate, hdnFrmDate)
        });
        $('input[id$=' + toDate + ']').datepicker({
            dateFormat: format,
            changeMonth: false,
            changeYear: false,
            onSelect: function (dateText, inst) {
                $('input[id$=' + fromDate + ']').datepicker("option", "maxDate", new Date($("input[id$=" + hdnToDate + "]").val()));
            },
            altField: $("[id$=" + hdnToDate + "]"),
            altFormat: "mm/dd/yy"

        });
        $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date());
        if (restrictfromDate)
            $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date());


    },

    //    DatePicker: function (inputid, format, restrictfromDate, restricttoDate, fromDate, toDate) {
    //        ///<summary>
    //        ///     Used for single datepicker 
    //        ///</summary>
    //        /// <param name="inputid" optional="true" type="String">
    //        ///     the id of the datepicker
    //        /// </param>
    //        /// <param name="format" optional="true" type="String">
    //        ///      Format of the datepicker
    //        /// </param>
    //        /// <param name="restrictfromDate" optional="true" type="bool">
    //        ///      true used for set the it will not allow to select the  the current before date
    //        /// </param>
    //        if (!format)
    //            format = "dd-M-yy";
    //        //debugger;
    //        $('input[id$=' + inputid + ']').datepicker({
    //            dateFormat: format,
    //            changeMonth: true,
    //            changeYear: true,
    //            onSelect: function () {
    //                if (typeof AfterDateSelect == "function") {
    //                    AfterDateSelect(inputid);
    //                }


    //            }
    //        });
    //        if (restrictfromDate)
    //            $('input[id$=' + inputid + ']').datepicker("option", "minDate", fromDate ? new Date(fromDate) : new Date());

    //        if (restricttoDate)
    //            $('input[id$=' + inputid + ']').datepicker("option", "maxDate", toDate ? new Date(toDate) : new Date());
    //    },

    DatePickerAdvance: function (inputid, format, restrictfromDate, isDefaultDate, fromDate, toDate, hdnDate) {
        ///<summary>
        ///     Used for single datepicker 
        ///</summary>
        /// <param name="inputid" optional="true" type="String">
        ///     the id of the datepicker
        /// </param>
        /// <param name="format" optional="true" type="String">
        ///      Format of the datepicker
        /// </param>
        /// <param name="restrictfromDate" optional="true" type="bool">
        ///      true used for set the it will not allow to select the  the current before date
        /// </param>
        if (!format)
            format = "dd-M-yy";
        if (isDefaultDate || isDefaultDate == undefined) {
            $('input[id$=' + inputid + ']').datepicker({
                dateFormat: format,
                changeMonth: true,
                changeYear: true,
                defaultDate: GrandScriptUtils.FillDate(inputid),
                onSelect: function (dateText, inst) {
                    if (!fromDate || fromDate != undefined) {
                        if ($("input[id$=" + hdnDate + "]").val() != "") {
                            $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnDate + "]").val()));
                            $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnDate + "]").val()));
                        }
                    }
                    if (typeof AfterDateSelect == "function") {
                        AfterDateSelect(inputid);
                    }
                },
                altField: $("[id$=" + hdnDate + "]"),
                altFormat: "mm/dd/yy"
            });
            //            if (!fromDate || fromDate != undefined) {
            //                $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date());
            //            }
        }
        else {
            $('input[id$=' + inputid + ']').datepicker({
                dateFormat: format,
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    if (!fromDate || fromDate != undefined) {
                        if ($("input[id$=" + hdnDate + "]").val() != "") {
                            $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnDate + "]").val()));
                            $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnDate + "]").val()));
                        }
                    }
                    if (typeof AfterDateSelect == "function") {
                        AfterDateSelect(inputid);
                    }
                },
                altField: $("[id$=" + hdnDate + "]"),
                altFormat: "mm/dd/yy"
            });
            //            if (!fromDate || fromDate != undefined) {
            //                $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date());
            //            }
        }
        if (restrictfromDate)
            $('input[id$=' + inputid + ']').datepicker("option", "minDate", new Date());
    },
    DatePickerClear: function (inputid, format, restrictfromDate, clearDate) {
        ///<summary>
        ///     Used for single datepicker 
        ///</summary>
        /// <param name="inputid" optional="true" type="String">
        ///     the id of the datepicker
        /// </param>
        /// <param name="format" optional="true" type="String">
        ///      Format of the datepicker
        /// </param>
        /// <param name="restrictfromDate" optional="true" type="bool">
        ///      true used for set the it will not allow to select the  the current before date
        /// </param>
        /// <param name="clearDate" optional="true" type="bool">
        ///      true used to set the input with out default date
        /// </param>
        if (!format)
            format = "dd-M-yy";
        if (clearDate) {
            $('input[id$=' + inputid + ']').datepicker({
                dateFormat: format,
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    if (typeof AfterDateSelect == "function") {
                        AfterDateSelect(inputid);
                    }
                }
            }).val('');
        }
        else {
            $('input[id$=' + inputid + ']').datepicker({
                dateFormat: format,
                changeMonth: true,
                changeYear: true,
                defaultDate: GrandScriptUtils.FillDate(inputid),
                onSelect: function (dateText, inst) {
                    if (typeof AfterDateSelect == "function") {
                        AfterDateSelect(inputid);
                    }
                }
            });
        }
        if (restrictfromDate)
            $('input[id$=' + inputid + ']').datepicker("option", "minDate", new Date());
    },
    DatePicker: function (inputid, format, restrictfromDate, isDefaultDate, fromDate, toDate, hdnDate) {

        ///<summary>
        ///     Used for single datepicker 
        ///</summary>
        /// <param name="inputid" optional="true" type="String">
        ///     the id of the datepicker
        /// </param>
        /// <param name="format" optional="true" type="String">
        ///      Format of the datepicker
        /// </param>
        /// <param name="restrictfromDate" optional="true" type="bool">
        ///      true used for set the it will not allow to select the  the current before date
        /// </param>
        /// <param name="isDefaultDate" optional="true" type="bool">
        ///      true used to set the input with default date 
        ///      false used to set the input with out default date
        /// </param>

        if (!format)
            format = "dd-M-yy";
        if (isDefaultDate || isDefaultDate == undefined) {
            $('input[id$=' + inputid + ']').datepicker({
                dateFormat: format,
                changeMonth: true,
                changeYear: true,
                defaultDate: GrandScriptUtils.FillDate(inputid),
                onSelect: function (dateText, inst) {
                    if (!fromDate || fromDate != undefined) {
                        if ($("input[id$=" + hdnDate + "]").val() != "") {
                            $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnDate + "]").val()));
                            $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnDate + "]").val()));
                        }
                    }
                    if (typeof AfterDateSelect == "function") {
                        AfterDateSelect(inputid);
                    }
                },
                altField: $("[id$=" + hdnDate + "]"),
                altFormat: "mm/dd/yy"
            });
            //            if (!fromDate || fromDate != undefined) {
            //                $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date());
            //            }
        }
        else {//set the input with out default date
            $('input[id$=' + inputid + ']').datepicker({
                dateFormat: format,
                changeMonth: true,
                changeYear: true,
                onSelect: function (dateText, inst) {
                    if (!fromDate || fromDate != undefined) {
                        if ($("input[id$=" + hdnDate + "]").val() != "") {
                            $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnDate + "]").val()));
                            $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnDate + "]").val()));
                        }
                    }
                    if (typeof AfterDateSelect == "function") {
                        AfterDateSelect(inputid);
                    }
                },
                altField: $("[id$=" + hdnDate + "]"),
                altFormat: "mm/dd/yy"
            });
            //            if (!fromDate || fromDate != undefined) {
            //                $('input[id$=' + fromDate + ']').datepicker("option", "minDate", new Date());
            //            }
        }
        if (restrictfromDate)
            $('input[id$=' + inputid + ']').datepicker("option", "minDate", new Date());
    },
    DatePickerMonthOnly: function (inputid, format) {
        ///<summary>
        ///     Date picker with month-only visibility and customizable date format
        ///</summary>
        /// <param name="inputid" type="String">
        ///     The id of the date picker input field
        /// </param>
        /// <param name="format" type="String">
        ///     Custom date format (e.g., "MM", "MM yy")
        /// </param>

        // Set default format if none is provided
        if (!format) {
            format = "MM"; // Default to showing only month name
        }

        $('input[id$=' + inputid + ']').attr("autocomplete", "off");

        $('input[id$=' + inputid + ']').datepicker({
            dateFormat: format,       // Custom format passed as a parameter
            changeMonth: true,        // Enable month selection
            showButtonPanel: true,    // Shows a button panel with "Done"
            closeText: "Close",       // Custom close button text
            onClose: function (dateText, inst) {
                // Ensure the selected month is applied correctly without selecting a day
                $(this).datepicker('setDate', new Date(inst.selectedYear, inst.selectedMonth, 1));
            },
            beforeShow: function (input, inst) {
                // Hide the year dropdown using CSS
                setTimeout(function () {
                    $(inst.dpDiv).find('.ui-datepicker-year').hide();
                }, 0);
            }
        });
    }
,


    S4: function () {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    },

    GenerateGuid: function () {
        return (GrandScriptUtils.S4() + GrandScriptUtils.S4() + "-" + GrandScriptUtils.S4() + "-" + GrandScriptUtils.S4() + "-" + GrandScriptUtils.S4() + "-" + GrandScriptUtils.S4() + GrandScriptUtils.S4() + GrandScriptUtils.S4());
    },

    ShowGridEmptyString: function () {
        ///<summary>Shows empty result string in "#MSGBox"</summary>
        $("#MSGBox").html("No item found.").dialog({
            modal: true,
            buttons: {
                OK: function () {
                    $(this).dialog("close");
                }
            }
        });
    },

    DatePickerCommon: function (inputid, format, restrictfromDate, restricttoDate, fromDate, toDate, nextDateFromInput, nextDateToInput, onAfterDateChangeCallBack) {
        ///<summary>
        ///     Used for single datepicker 
        ///</summary>
        /// <param name="inputid" optional="true" type="String">
        ///     the id of the datepicker
        /// </param>
        /// <param name="format" optional="true" type="String">
        ///      Format of the datepicker
        /// </param>
        /// <param name="restrictfromDate" optional="true" type="bool">
        ///      true used for set the it will not allow to select the  the current before date
        /// </param>
        /// <param name="onAfterDateChangeCallBack" optional="true" type="function">
        ///      this is a custom callBack for onSelect event
        /// </param>
        if (!format)
            format = "dd-M-yy";
        $('input[id$=' + inputid + ']').attr("autocomplete", "off");
        $('input[id$=' + inputid + ']').datepicker({
            dateFormat: format,
            changeMonth: true,
            changeYear: true,
            onSelect: function () {
                if (typeof AfterDateSelect == "function") {
                    AfterDateSelect(inputid);
                }
                if (typeof onAfterDateChangeCallBack == "function") {
                    onAfterDateChangeCallBack(inputid);
                }
                if (nextDateFromInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled")) {
                    $('input[id$=' + nextDateFromInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
                }
                if (nextDateToInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled")) {
                    $('input[id$=' + nextDateToInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
                }
            }
        });
        if (restrictfromDate)
            $('input[id$=' + inputid + ']').datepicker("option", "minDate", fromDate ? new Date(fromDate) : new Date());
        if (restricttoDate)
            $('input[id$=' + inputid + ']').datepicker("option", "maxDate", toDate ? new Date(toDate) : new Date());
        if ($('input[id$=' + inputid + ']').is('focus'))
            $('input[id$=' + inputid + ']').focus();
        if (nextDateFromInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled") && $('input[id$=' + inputid + ']').val() != "") {
            $('input[id$=' + nextDateFromInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
        }
        if (nextDateToInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled") && $('input[id$=' + inputid + ']').val() != "") {
            $('input[id$=' + nextDateToInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
        }
    },
    AddDateRangeCommon: function (fromDate, hdnFrmDate, toDate, hdnToDate, format, restrictfromDate, setDefault, defaultDate, resetRange) {
        ///<summary>
        ///     Used for From Date and To Date datepicker
        ///</summary>
        /// <param name="fromDate" optional="true" type="String">
        ///     The input id of the fromdate datepicker
        /// </param>
        /// <param name="hdnFrmDate" optional="true" type="String">
        ///     The hidden field id used for the set min date of the todate datepicker
        /// </param>
        /// <param name="toDate" optional="true" type="String">
        ///     The input id of the todate datepicker
        /// </param>
        /// <param name="hdnToDate" optional="true" type="String">
        ///      The hidden field id used for the set max date of the fromdate datepicker
        /// </param>
        /// <param name="format" optional="true" type="String">
        ///      Format of the datepicker
        /// </param>
        /// <param name="restrictfromDate" optional="true" type="bool">
        ///      true used for set the it will not allow to select the  the current before date
        /// </param>
        /// <param name="setDefault" optional="true" type="bool">
        ///      true used for set the default from date
        /// </param>
        /// <param name="defaultDate" optional="true" type="String">
        ///      The default from date
        /// </param>
        /// <param name="resetRange" optional="true" type="bool">
        ///      To reset default Date Range Restrictions
        /// </param>

        if (!format)
            format = "dd-M-yy";
        $('input[id$=' + fromDate + ']').attr("autocomplete", "off");
        $('input[id$=' + toDate + ']').attr("autocomplete", "off");
        $('input[id$=' + fromDate + ']').datepicker({
            dateFormat: format,
            changeMonth: true,
            changeYear: true,
            onSelect: function (dateText, inst) {
                $('input[id$=' + toDate + ']').datepicker("option", "minDate", new Date($("input[id$=" + hdnFrmDate + "]").val()));
                if (typeof AfterDateSelect == "function") {
                    AfterDateSelect(fromDate);
                }
            },
            altField: $("[id$=" + hdnFrmDate + "]"),
            altFormat: "mm/dd/yy"
            // defaultDate: GrandScriptUtils.FillDate(fromDate, hdnFrmDate)
        });
        $('input[id$=' + toDate + ']').datepicker({
            dateFormat: format,
            changeMonth: true,
            changeYear: true,
            onSelect: function (dateText, inst) {
                $('input[id$=' + fromDate + ']').datepicker("option", "maxDate", new Date($("input[id$=" + hdnToDate + "]").val()));
                if (typeof AfterDateSelect == "function") {
                    AfterDateSelect(toDate);
                }
            },
            altField: $("[id$=" + hdnToDate + "]"),
            altFormat: "mm/dd/yy"

        });
        if ($('input[id$=' + fromDate + ']').length > 0 && $('input[id$=' + fromDate + ']').val() != "") {
            $('input[id$=' + toDate + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + fromDate + ']').val()));
        }

        if (restrictfromDate)
            $('input[id$=' + fromDate + ']').datepicker("option", "maxDate", new Date());
        if (setDefault) {
            var curDate = $('input[id$=' + fromDate + ']').val() != "" ? $.datepicker.parseDate(format, $('input[id$=' + fromDate + ']').val())
             : defaultDate ? new Date(defaultDate) : new Date();
            $('input[id$=' + fromDate + ']').val($.datepicker.formatDate(format, curDate));
            $("[id$=" + hdnFrmDate + "]").val($.datepicker.formatDate("mm/dd/yy", curDate));
            $('input[id$=' + toDate + ']').datepicker("option", "minDate", curDate);
        }
        if (resetRange) {
            $('input[id$=' + fromDate + ']').datepicker("option", "maxDate", null);
            $('input[id$=' + toDate + ']').datepicker("option", "minDate", null);
        }
        if ($('input[id$=' + fromDate + ']').is("focus"))
            $('input[id$=' + fromDate + ']').focus();
        else if ($('input[id$=' + toDate + ']').is("focus"))
            $('input[id$=' + toDate + ']').focus();
    },

    // Convert 10-Feb-20111 Format to 02/10/2011 
    ConvertDateFormat: function (date) {
        ///<summary>
        ///     Used to Convert 10-Feb-20111 Format to 02/10/2011
        ///</summary>

        // Split Date Format 10-Feb-2011 By '-' and Assign to Array
        var conDate = date.split('-');
        // Cretae Month name Array
        var mmm = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        // Assign Month Name's Index+1 as Name of Month - and Assign to conDate[1] Using loop
        for (var i = 0; i < mmm.length; i++) {
            // check Month Equal to Monthsa in a Array
            if (mmm[i] == conDate[1]) {
                // If True - Get Index of the Array +1 and Assign to conDate[1]
                conDate[1] = i + 1;
                break;
            }
        }
        // return as 10-Feb-20111 Format to 02/10/2011
        return conDate[1] + "/" + conDate[0] + "/" + conDate[2];

    },

    RestrictedDatePicker: function (inputid, format, restrictfromDate, restricttoDate, fromDate, toDate, nextDateFromInput, nextDateToInput, onAfterDateChangeCallBack) {
        ///<summary>
        ///     Used for single datepicker 
        ///</summary>
        /// <param name="inputid" optional="true" type="String">
        ///     the id of the datepicker
        /// </param>
        /// <param name="format" optional="true" type="String">
        ///      Format of the datepicker
        /// </param>
        /// <param name="restrictfromDate" optional="true" type="bool">
        ///      true used for set the it will not allow to select the  the current before date
        /// </param>
        /// <param name="onAfterDateChangeCallBack" optional="true" type="function">
        ///      this is a custom callBack for onSelect event
        /// </param>
        if (!format)
            format = "dd-M-yy";

        var curDateFrom = $.datepicker.parseDate(format, fromDate);
        var DateFrom = $.datepicker.formatDate("mm/dd/yy", curDateFrom);

        var curDateTo = $.datepicker.parseDate(format, toDate);
        var DateTo = $.datepicker.formatDate("mm/dd/yy", curDateTo);


        $('input[id$=' + inputid + ']').attr("autocomplete", "off");
        $('input[id$=' + inputid + ']').datepicker({
            dateFormat: format,
            changeMonth: true,
            changeYear: true,
            onSelect: function () {
                if (typeof AfterDateSelect == "function") {
                    AfterDateSelect(inputid);
                }
                if (typeof onAfterDateChangeCallBack == "function") {
                    onAfterDateChangeCallBack(inputid);
                }
                if (nextDateFromInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled")) {
                    $('input[id$=' + nextDateFromInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
                }
                if (nextDateToInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled")) {
                    $('input[id$=' + nextDateToInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
                }
            }
        });
        if (restrictfromDate && DateFrom != "")
            $('input[id$=' + inputid + ']').datepicker("option", "minDate", DateFrom ? new Date(DateFrom) : new Date());
        if (restricttoDate && DateTo != "")
            $('input[id$=' + inputid + ']').datepicker("option", "maxDate", DateTo ? new Date(DateTo) : new Date());
        if ($('input[id$=' + inputid + ']').is('focus'))
            $('input[id$=' + inputid + ']').focus();
        if (nextDateFromInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled") && $('input[id$=' + inputid + ']').val() != "") {
            $('input[id$=' + nextDateFromInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
        }
        if (nextDateToInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled") && $('input[id$=' + inputid + ']').val() != "") {
            $('input[id$=' + nextDateToInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
        }
    },

    RestrictedYearDatePicker: function (inputid, format, restrictfromDate, restricttoDate, fromDate, toDate, nextDateFromInput, nextDateToInput, onAfterDateChangeCallBack) {
        ///<summary>
        ///     Used for single datepicker 
        ///</summary>
        /// <param name="inputid" optional="true" type="String">
        ///     the id of the datepicker
        /// </param>
        /// <param name="format" optional="true" type="String">
        ///      Format of the datepicker
        /// </param>
        /// <param name="restrictfromDate" optional="true" type="bool">
        ///      true used for set the it will not allow to select the  the current before date
        /// </param>
        /// <param name="onAfterDateChangeCallBack" optional="true" type="function">
        ///      this is a custom callBack for onSelect event
        /// </param>       
        if (!format)
            format = "dd-M-yy";
        var setYearRange = false;

        var DateFrom = new Date();
        if (fromDate != undefined && fromDate != "") {
            setYearRange = true;
            var curDateFrom = $.datepicker.parseDate(format, fromDate);
            var DateFrom = $.datepicker.formatDate("mm/dd/yy", curDateFrom);
        }
        var DateTo = new Date();

        if (toDate != undefined && toDate != "") {
            var curDateTo = $.datepicker.parseDate(format, toDate);
            DateTo = $.datepicker.formatDate("mm/dd/yy", curDateTo);
        }

        var fromYear = new Date(DateFrom).getFullYear();
        var toYear = new Date(DateTo).getFullYear();

        if (setYearRange) {
            $('input[id$=' + inputid + ']').attr("autocomplete", "off");
            $('input[id$=' + inputid + ']').datepicker({
                dateFormat: format,
                changeMonth: true,
                changeYear: true,
                yearRange: '' + fromYear + ':' + toYear + '',
                onSelect: function () {
                    if (typeof AfterDateSelect == "function") {
                        AfterDateSelect(inputid);
                    }
                    if (typeof onAfterDateChangeCallBack == "function") {
                        onAfterDateChangeCallBack(inputid);
                    }
                    if (nextDateFromInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled")) {
                        $('input[id$=' + nextDateFromInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
                    }
                    if (nextDateToInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled")) {
                        $('input[id$=' + nextDateToInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
                    }
                }
            });
        }
        else {
            $('input[id$=' + inputid + ']').attr("autocomplete", "off");
            $('input[id$=' + inputid + ']').datepicker({
                dateFormat: format,
                changeMonth: true,
                changeYear: true,
                onSelect: function () {
                    if (typeof AfterDateSelect == "function") {
                        AfterDateSelect(inputid);
                    }
                    if (typeof onAfterDateChangeCallBack == "function") {
                        onAfterDateChangeCallBack(inputid);
                    }
                    if (nextDateFromInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled")) {
                        $('input[id$=' + nextDateFromInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
                    }
                    if (nextDateToInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled")) {
                        $('input[id$=' + nextDateToInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
                    }
                }
            });
        }
        if (restrictfromDate && DateFrom != "" && setYearRange)
            $('input[id$=' + inputid + ']').datepicker("option", "minDate", DateFrom ? new Date(DateFrom) : new Date());
        if (restricttoDate && DateTo != "" && setYearRange)
            $('input[id$=' + inputid + ']').datepicker("option", "maxDate", DateTo ? new Date(DateTo) : new Date());
        if ($('input[id$=' + inputid + ']').is('focus'))
            $('input[id$=' + inputid + ']').focus();
        if (nextDateFromInput != undefined && nextDateFromInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled") && $('input[id$=' + inputid + ']').val() != "") {
            $('input[id$=' + nextDateFromInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
        }
        if (nextDateToInput != undefined && nextDateToInput && $('input[id$=' + nextDateFromInput + ']').is(":enabled") && $('input[id$=' + inputid + ']').val() != "") {
            $('input[id$=' + nextDateToInput + ']').datepicker("option", "minDate", $.datepicker.parseDate(format, $('input[id$=' + inputid + ']').val()));
        }
    },

    BindWorkFlowCommand: function (grdID) {
        var ajaxUrl = "CommonManagement.do?Action=GetWrkfCommentList&RefPk=" + $("[id$=ReferenceID]").val() + "&AppID=" + $("[id$=ApplicationID]").val() + "&ProcID=" + $("[id$=ProcessID]").val();
        $("#grdWrkfComment").removeAttr("ajaxurl")
        $("#grdWrkfComment").attr("ajaxurl", ajaxUrl);
        GrandGrid.Utilities.ResetGrid(true, "grdWrkfComment");
        GrandGrid.MakeGrid($("#grdWrkfComment"));
    },
    ShowWorkFlowCommandList: function () {
        $("#divWrkfComment").show();
        $("#imgWrkfCommentHide").hide();
        $("#imgWrkfCommentShow").show();
    },

    HideWorkFlowCommandList: function () {
        $("#divWrkfComment").hide();
        $("#imgWrkfCommentHide").show();
        $("#imgWrkfCommentShow").hide();
    },

    ChangeMode: function (containerID) {
        ///<summary>
        //Used for Change the Mode of the provided container
        ///</summary>
        /// <param name="containerID" optional="true" type="String">
        /// ControlID - Container ID User to change the mode
        /// </param>

        var div = $("#" + containerID).clone(true); //taking clone of the container
        $(div).attr("id", $(div).attr("id") + "_cln"); //adding id to that container

        div.insertAfter("#" + containerID); //inserting clonedcontainer after orginal container
        //finding all inputs in cloned container 
        $("#" + containerID + "_cln").find("input").each(function () {

            var check = $(this);
            //checking type of the element if it is text
            //creating label 
            //assigning text value to label
            //inserting label after text
            //removing text from form.
            if ($(this).attr("type") == "text") {
                var sp = document.createElement("span");
                $(sp).html($(this).val());
                $(sp).insertAfter($(this));
                $(this).remove();
            }

            //checking type of the element if it is checkbox or radio
            //disabling that element

            if (($(this).attr("type") == "checkbox") || ($(this).attr("type") == "radio")) {

                $(this).attr("disabled", true);

            }

            //            if ($(this).attr("type") == "fupUploader" || $(this).attr("type") == "file" || $(this).attr("type") == "button") {
            //                $(this).hide();
            //            }


        });
        //finding all select elements.
        //creating label 
        //assigning text value to label
        //inserting label after text
        //removing text element from form.
        $("#" + containerID + "_cln").find("select").each(function () {
            var sp = document.createElement("span");
            var actualSelect = $("#" + containerID).find("select[id$=" + $(this).attr("id") + "]");
            if (actualSelect.length > 0) {
                $(sp).html($(actualSelect[0]).find("option:selected").text());
            }
            else {
                $(sp).html($(this).find("option:selected").text());
            }
            $(sp).insertAfter($(this));
            $(this).remove();
        });
        //finding all select textarea elements.
        //creating label 
        //assigning textarea value to label
        //inserting label after textarea
        //removing textarea from form.
        $("#" + containerID + "_cln").find("textarea").each(function () {
            var txtarea = document.createElement("label");
            $(txtarea).html($(this).val());
            $(txtarea).insertAfter($(this));
            $(this).remove();
        });
        //finding all tables from the container elements.loop of tables
        //finding all th from the each table.loop of th
        /// <param name="index" >
        ///index of each th
        //checking attribute of each index.if its attribute is template
        //finding the parent of perticular th and then find all tr in parent
        //finding the td using index from tr and removing that index

        $("#" + containerID + "_cln").find("table").each(function () {

            $(this).find("th").each(function (index) {
                if ($(this).attr("type") == "Template") {
                    $(this).find("input").each(function () {
                        if (($(this).attr("type") == "checkbox") || ($(this).attr("type") == "radio")) {
                            $(this).attr("disabled", true);
                        }

                    });

                    if ($(this).attr("type") == "text") {
                        var sp = document.createElement("label");
                        $(sp).html($(this).val());
                        $(sp).insertAfter($(this));
                        $(this).remove();
                    }

                    $(this).find("img").each(function () {
                        $(this).hide();

                    });
                    //                    $(this).parents("table:eq{0}").find("tr").each(function () {
                    //                        $(this).find("td:eq(" + index + ")").remove();
                    //                    });
                    //              $(this).remove(); //removing th from form yhat contains action



                }
            });
        });
        $("#" + containerID).hide();
    },

    ///#region ---------- FileUpload With Temporary Save  

    // New Option First Save in to Temporary Folder and save finally from Temporary Folder to Original location
    MakeFileUploader: function (ControlID, listType, fileTitl, filID, page, editMode) {
        ///<summary>
        //Used for createing File uploade control
        ///</summary>
        /// <param name="ControlID" optional="true" type="String">
        /// ControlID - ControlID used to upload the file
        /// </param>
        /// <param name="uploadFolder" optional="true" type="String">
        /// uploadFolder - Upload Folder Name
        /// </param>

        /// <param name="hdfFileID" optional="true" type="String">
        /// hdfFileID - the hidden field id used for strorng the name of the Files uploaded seprated by ','
        /// </param>

        /// <param name="Page" optional="true" type="String">
        /// Page - the hidden field id used for strorng the name of the Files uploaded seprated by ','
        /// </param>

        /// <param name="editMode" optional="true" type="Bool">
        /// editMode - True for add Upload Button and Delete Button, False fro Hide delete and upload button ','
        /// </param>

        if (listType == true) {

            FileJson = $.parseJSON($('input[id$=' + filID + ']').val());
            $("#" + fileTitl).data("FileData", FileJson);
        }

        fileTitle = fileTitl;
        fileID = filID;
        listFileType = listType;
        GrandScriptUtils.MakeUploadIFrame();

        if (editMode == true) {
            var btn = document.createElement("button");
            $(btn).attr({ "id": "btnFileUpload" });
            $(btn).html("Upload");
            $(btn).button();
            $(btn).css({ "height": "25px", "margin-left": "3px" });
            $(btn).find("span:eq(0)").css("padding-top", "0.2em");
            $(btn).click(function (e) {
                e.preventDefault();
                GrandScriptUtils.UploadFile(this, ControlID, fileTitle, fileID, page);
            });
            $(btn).attr("title", "Upload")
            $(btn).insertAfter("#" + ControlID);
        }
        else {
            var btn = document.createElement("button");
            $(btn).attr({ "id": "btnFileUpload" });
            $(btn).html("Upload");
            $(btn).button();
            $(btn).css({ "height": "25px", "margin-left": "3px", "visibility": "hidden" });
            $(btn).find("span:eq(0)").css("padding-top", "0.2em");
            $(btn).click(function (e) {
                e.preventDefault();
                GrandScriptUtils.UploadFile(this, ControlID, fileTitle, fileID, page);
            });
            $(btn).attr("title", "Upload")
            $(btn).insertAfter("#" + ControlID);
        }


        if ($("#_FileUploadTemplate").length == 0) {
            var _FileUploadTemplate = document.createElement("div");
            $(_FileUploadTemplate).attr("id", "_FileUploadTemplate");
            $(_FileUploadTemplate).css("visibility", "hidden");
            if (editMode == true) {
                $(_FileUploadTemplate).append("<div class=\"divcol-upld-FileNameListing-table\"><div style=\"clear:both\"><span id=\"GrandFileUploadFileID\"  style=\"display:none\" ></span><span class=\"fileUploadClass\"></span><span class=\"ui-button ui-widget ui-state-default\" style=\"width:10px; height: 10px\"  title=\"Delete\" onclick=\"javascript:GrandScriptUtils.RemoveUploadedFile(this);\">X</span><a target=\"_blank \"  class=\"view-BTN\"  title=\"Download\" /></div></div>");
            }
            else {
                $(_FileUploadTemplate).append("<div class=\"divcol-upld-FileNameListing-table\"><div style=\"clear:both\"><span id=\"GrandFileUploadFileID\"  style=\"display:none\" ></span><span class=\"fileUploadClass\"></span><a target=\"_blank \"  class=\"view-BTN\"  title=\"Download\" /></div></div>");
            }
            $(document.forms[0]).append(_FileUploadTemplate);
        }


        if ($("#fContainer_" + ControlID).length == 0) {
            var fContainer = document.createElement("div");
            fContainer.id = "fContainer_" + ControlID;
            $(fContainer).insertAfter($("#" + ControlID).next("button"));
        }



    },
    MakeUploadIFrame: function () {
        var hdniFrame;
        if ($.browser.msie) {
            hdniFrame = document.createElement("<iframe name=\"iFrameFUpload\"></iframe>");
        }
        else {
            hdniFrame = document.createElement("iframe");
            $(hdniFrame).attr("name", "iFrameFUpload");
        }
        $(hdniFrame).attr({
            "src": "",
            "width": "0",
            "height": "0",
            "scrolling": "no",
            "id": "iFrameFUpload"
        });
        $(document.forms[0]).append(hdniFrame);
    },
    // Btn - Upload Button, UploadFolder - name of the Folder To Upload File
    UploadFile: function (btn, controlID, FileTitle, FileID, page) {
        ///<summary>
        //Used for Upload File
        ///</summary>
        /// <param name="btn" optional="true" type="Object">
        /// btn - Button
        /// </param>
        /// <param name="FileUploader" optional="true" type="String">
        /// FileUploader Control name
        /// </param>
        /// <param name="FileTitle" optional="true" type="String">
        /// FileTitle - the hidden field id used for Store the Title of the Files uploaded seprated by ','
        /// </param>
        /// <param name="FileID" optional="true" type="String">
        /// FileID - the hidden field id used for strorng the ID of the Files uploaded seprated by ','
        /// </param>

        // Check File Selected or not
        //$(document.forms[0]).validate().resetForm();
        RemoveAllValidations();

        if ($('input[id$=' + controlID + ']').val().length > 0) {
            // Check File Already Exists or not
            if (GrandScriptUtils.CheckFileNotExists(controlID)) {
                $(document.forms[0]).attr("target", "iFrameFUpload");
                $(document.forms[0]).attr("enctype", "multipart/form-data");
                //$(document.forms[0]).attr("action", FileUpload.UPLOADFILEURL + $(btn).attr("Type") + "&TitleCntrl=" + FileTitle + "&FileIDCntrl=" + FileID + "&Page=" + page);
                $(document.forms[0]).attr("action", FileUpload.UPLOADFILEURL + controlID + "&TitleCntrl=" + FileTitle + "&FileIDCntrl=" + FileID + "&Page=" + page);
                $(document.forms[0]).submit();

            }
            else {

                GrandScriptUtils.ShowModal(FileUpload.FILEUPLOADALREADYMSG, FileUpload.INFORMATIONTITLE);
            }
        }
        else {

            GrandScriptUtils.FileMsg(FileUpload.PLEASESELECTFILEMSG);
        }
        // Not Clear the FileUpload Control
        if ($.browser.msie) {
            var uploader = $('input[id$=' + controlID + ']');
            var copyControl = $(uploader).clone(true, true);
            $(uploader).remove();
            $(copyControl).insertBefore("#btnFileUpload");
        }
        else {
            $('input[id$=' + controlID + ']').val("");
        }


    },

    FileMsg: function (msg) {
        ///<summary>
        //Used Show Msg , When FileUpload
        ///</summary>
        /// <param name="msg"  type="String">
        /// msg - To Display in Popup
        /// </param>

        GrandScriptUtils.ShowModal(msg, FileUpload.INFORMATIONTITLE);
    },
    // Check File Already Exists or Not
    CheckFileNotExists: function (controlID) {
        ///<summary>
        //Used for Check File Already uploaded Or Not
        ///</summary>
        /// <param name="ControlID" optional="true" type="String">
        /// ControlID - ControlID used to upload the file
        /// </param>
        var fileStatus = true;
        // Get File Name of the Upload File
        var uploadFileName = $('input[id$=' + controlID + ']').val().substring($('input[id$=' + controlID + ']').val().lastIndexOf("\\") + 1);
        $("#fContainer_" + controlID).find("span.fileUploadClass").each(function () {
            // Check Upload File Name With Uploaded List
            if ($(this).text().toUpperCase() == uploadFileName.toUpperCase()) {
                // True 
                fileStatus = false;

            }
        });
        return fileStatus;

    },

    ShowUploadStatus: function (fileName, Type, fileIDval, titleVal, fileID, titleCntrl, extension, path) {
        ///<summary>
        //Used for Add File Details To List Details, And Title To HiddenField Title Control,And FileID To HiddenField FileID Control,
        ///</summary>
        /// <param name="fileName" optional="true" type="String">
        /// fileName - Name Of the File Name With Extenstion
        /// </param>
        /// <param name="Type" optional="true" type="String">
        /// Type - Upload Control Name
        /// </param>
        /// <param name="fileIDval" optional="true" type="String">
        /// fileIDval - ControlID used to upload the file , With Extension
        /// </param>
        /// <param name="titleVal" optional="true" type="String">
        /// titleVal - File Title to add HiddenField FileTitle Control, With Out Extension
        /// </param>
        /// <param name="fileID" optional="true" type="String">
        /// fileID - Name of the Hidden field Control , to add FileID Details - Seperated By Comma
        /// </param>
        /// <param name="titleCntrl" optional="true" type="String">
        /// titleCntrl - Name of the Hidden field Control , to add FileTitle Details - Seperated By Comma
        /// </param>
        var template = $("#_FileUploadTemplate").clone();
        $(template).find("span:eq(1)").text(fileName);
        $(template).find("span:eq(0)").text(FileUpload.TEMPPATH + FileUpload.TEMPFOLDER + "\\" + fileIDval);
        $(template).find("a:eq(0)").attr("href", path + FileUpload.TEMPPATH + FileUpload.TEMPFOLDER + "\\" + fileIDval + "&Title=" + titleVal);
        $("#fContainer_" + Type).append($(template).html());
        if (listFileType == false) {
            if ($('input[id$=' + titleCntrl + ']').val() == FileUpload.EMPTYVALUE) {
                $('input[id$=' + titleCntrl + ']').val(titleVal);
            }
            else {
                $('input[id$=' + titleCntrl + ']').val($('input[id$=' + titleCntrl + ']').val() + "," + titleVal);
            }
            if ($('input[id$=' + fileID + ']').val() == FileUpload.EMPTYVALUE) {
                $('input[id$=' + fileID + ']').val(fileIDval);
            }
            else {
                $('input[id$=' + fileID + ']').val($('input[id$=' + fileID + ']').val() + "," + fileIDval);
            }

        }
        else {
            // Add Details To Object and Push to FileJson
            FileJson = $("#" + fileTitle).data("FileData");
            var obj = new Object();
            obj.DOC_PK = 0;
            obj.DOC_TITLE = titleVal;
            obj.DOC_NAME = fileIDval;
            obj.DOC_TYPE = extension;
            obj.DOC_SEQ_NO = FileJson.FILELIST.length + 1;
            FileJson.FILELIST.push(obj);
            $("#" + fileTitle).data("FileData", FileJson);

        }
        if ($.browser.msie) {
            $("#iFrameFUpload").remove();
            GrandScriptUtils.MakeUploadIFrame();
        }
        else {
            $(document.forms[0]).removeAttr("target");
            $(document.forms[0]).removeAttr("enctype");
            $(document.forms[0]).removeAttr("action");
        }
    },

    ShowDeleteStatus: function (fileName, Type, fileTitle, fileID) {

        ///<summary>
        //Used for Remove File From List, And FileID HiddenFiels , FileTitle , when Delete File
        ///</summary>
        /// <param name="FileName" optional="true" type="String">
        /// File Name - Name of the file to Delete
        /// </param>
        /// <param name="Type" optional="true" type="String">
        /// Type - ControlID used to upload the file
        /// </param>
        var delIndx;
        $("#fContainer_" + Type).find("span.fileUploadClass").each(function (indx) {
            // to get Index of the Delete File Name
            if ($(this).text() == fileName) {
                // Assign Delted File index
                delIndx = indx;
                // Remove the File Name From the List
                $(this).parents("div:eq(0)").remove();

            }

        });
        if (listFileType == false) {
            // Get all FileTitle To FileTitle Array
            var fileTitle = $('input[id$=' + fileTitle + ']').val().split(',');
            // Get all FileID  To FileID Array 
            var fileIds = $('input[id$=' + fileID + ']').val().split(',');
            // Remove the FileTitle From Array using Index
            fileTitle.splice(delIndx, 1);
            // Remove the FileID From Array using Index
            fileIds.splice(delIndx, 1);
            // Set FileTitle HiddenField value as Null
            $('input[id$=' + fileID + ']').val(FileUpload.EMPTYVALUE);
            // Set FileID HiddenField value as Null
            $('input[id$=' + fileTitle + ']').val(FileUpload.EMPTYVALUE);
            // Add all fileTitle To HiddenField  using loop, File Titles are Separed By ','
            for (var i in fileTitle) {

                if ($('input[id$=' + fileTitle + ']').val() == "")

                    $('input[id$=' + fileTitle + ']').val(fileTitle[i]);
                else
                    $('input[id$=' + fileTitle + ']').val($('input[id$=' + fileTitle + ']').val() + "," + fileTitle[i]);

            }
            // Add all fileID To HiddenField  using loop, File Titles are Separed By ','
            for (var i in fileIds) {

                if ($('input[id$=' + fileID + ']').val() == "")
                    $('input[id$=' + fileID + ']').val(fileIds[i]);
                else
                    $('input[id$=' + fileID + ']').val($('input[id$=' + fileID + ']').val() + "," + fileIds[i]);
            }
        }
        else {
            // Reomove From list 
            // Assign Data From Div To Object
            FileJson = $("#" + fileTitle).data("FileData");
            //FileJson = $("#divFileData").data("FileData");
            // Remove From list
            FileJson.FILELIST.splice(delIndx, 1);

            for (var index in FileJson.FILELIST) {

                FileJson.FILELIST[index].DOC_SEQ_NO = parseInt(index) + 1;
            }
            // Assign Object to Div
            $("#" + fileTitle).data("FileData", FileJson);
            //$("#divFileData").data("FileData", FileJson)

        }
        //alert(fileName + FileUpload.DELETEDMSG);
    },

    RemoveUploadedFile: function (control) {
        ///<summary>
        //Used for Remove File From List, And FileID HiddenFiels , FileTitle , when Delete File
        ///</summary>
        /// <param name="ControlID" optional="true" type="String">
        /// ControlID - ControlID used to upload the file
        /// </param>
        var fileName = $(control).prev("span").text();
        var uploadfileID = $(control).parents("div:first").children("span[id=GrandFileUploadFileID]").html();
        var Type = $(control).parents("div:eq(2)").attr("id");
        Type = Type.substr(Type.indexOf("_") + 1);
        $.ajax({
            url: FileUpload.REMOVEFILEURL + Type + "&FileName=" + fileName + "&FileID=" + uploadfileID,
            success: function (data) {
                GrandScriptUtils.ShowDeleteStatus(data.FileName, data.Type, fileTitle, fileID);
            }
        });

    },
    // End ----------------
    ///#endregion


    Round: function (x, y) {
        ///<summary>Method to round decimal no. to given no. of positions</summary>
        /// <param name="x" >
        ///     Input decimal value
        /// </param>
        ///<param name="y" >
        ///     No. of decimal points to be restricted
        /// </param>
        return Math.round(x * Math.pow(10, y)) / Math.pow(10, y);

        //return parseFloat(x).toFixed(y); 
    },

    RoundTime: function (x, y) {
        ///<summary>Method to round decimal no. to given no. of positions</summary>
        /// <param name="x" >
        ///     Input decimal value
        /// </param>
        ///<param name="y" >
        ///     No. of decimal points to be restricted
        /// </param>

        var time = x;
        var timeArr = String(time).split('.');
        var hrs = timeArr[0];
        var min = 0;
        if (parseFloat(timeArr[1]) > 0) {
            min = 60 * parseFloat(timeArr[1]);
        }
        //var minArr = String(min).split('.');
        var convertmin = parseFloat(min) / 10000000000000000;
        if (parseFloat(convertmin) > 0) {
            if (parseFloat(convertmin) < 1) {
                return hrs + ".0" + GrandScriptUtils.Round(convertmin * 10, 0);
            }
            else {
                if (parseFloat(convertmin) < 10) {
                    return hrs + "." + GrandScriptUtils.Round(convertmin, 2) * 10;
                }
                else {
                    return hrs + "." + GrandScriptUtils.Round(convertmin, 2);
                }
            }
        }
        else {
            return hrs + ".00";
        }


    },

    AllowOnlyNumbers: function (event, AllowDot) {
        ///<summary>
        ///     Allows Only Numeric Key Press
        ///</summary>
        /// <param name="event" optional="false" type="Event">
        ///     Pass the event
        /// </param>
        /// <param name="AllowDot" optional="true" type="bool">
        ///     Pass true if you want to allow "." 
        /// </param>
        var keyCode = event.keyCode ? event.keyCode : event.which;
        //alert(keyCode);
        //Backspace, Tab, Enter, End, Home, Left Arrow, Up Arrow, Right Arrow, Down Arrow
        var arrSafeKeys = [8, 9, 13, 39, 37, 190, 46]; // 35,36,40,38,37, // 190 for point
        //if you find safe char, replace keycode with the keycode of 1, it will bypass the numeric check
        keyCode = $.inArray(keyCode, arrSafeKeys) >= 0 ? 49 : keyCode;
        var char = String.fromCharCode(keyCode);

        if (!AllowDot) { // if you are not passing AllowDot, make it's value false
            AllowDot = false;
        }
        var expression = AllowDot == true ? (/[0-9.]/g) : (/[0-9]/g); //numeric  or numeric with '.'

        if (!expression.test(char)) {
            if ($.browser.msie) {
                event.returnValue = false;
            }
            else {

                event.preventDefault();
            }
        }
    },

    SetZeroDefault: function (sender, decimal) {
        ////<summary>function to set default zero </summary>
        /// <param name="sender"  type="Object">
        /// Determines the Textbox
        /// </param>
        /// <param name="decimal"  type="integer">
        /// Determines the no. of decimal points
        /// </param>

        if ($(sender).val() == "") {
            if (!decimal)
                $(sender).val("0");
            else {
                switch (decimal) {
                    case 0:
                        $(sender).val("0");
                        break;
                    case 1:
                        $(sender).val("0.0");
                        break;
                    case 2:
                        $(sender).val("0.00");
                        break;
                    case 3:
                        $(sender).val("0.000");
                        break;
                }
            }
        }
    },

    GetShortString: function (strVariable, limit) {
        ///<summary>
        ///     Method to get short string
        ///</summary>
        /// <param >
        ///     Pass the string value
        /// </param>
        if (strVariable.length > parseInt(limit)) {
            return strVariable.substring(0, parseInt(limit)) + "..";
        }
        else
            return strVariable
    }
}






