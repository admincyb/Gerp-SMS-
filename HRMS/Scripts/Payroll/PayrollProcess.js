
$(window).load(function EndRequest() {
    FormatCalendar('4');
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


$("[id*=chkEmpHeader]").live("click", function () {
    var chkHeader = $(this);
    var grid = $(this).closest("table");
    $("input[type=checkbox]", grid).each(function () {
        if (chkHeader.is(":checked")) {
            $(this).attr("checked", "checked");
            $("td", $(this).closest("tr")).addClass("selected");
        } else {
            if ($(this).is(':disabled') == false) {
                $(this).removeAttr("checked");
                $("td", $(this).closest("tr")).removeClass("selected");
            }
        }


    });
});

$("[id*=chkEmpselect]").live("click", function () {
    var grid = $(this).closest("table");
    var chkHeader = $("[id*=chkEmpHeader]", grid);
    if (!$(this).is(":checked")) {
        $("td", $(this).closest("tr")).removeClass("selected");
        chkHeader.removeAttr("checked");
    } else {
        $("td", $(this).closest("tr")).addClass("selected");
        if ($("[id*=chkEmpselect]", grid).length == $("[id*=chkEmpselect]:checked", grid).length) {
            chkHeader.attr("checked", "checked");
        }
    }
});


function ChangeColor(GridViewId, SelectedRowId) {
    var GridViewControl = document.getElementById(GridViewId); //document.getElementById("<%=grdPeriodList.ClientID%>");
    if (GridViewControl != null) {
        var GridViewRows = GridViewControl.rows;
        if (GridViewRows != null) {
            var SelectedRow = GridViewRows[SelectedRowId];
            //Remove Selected Row color if any
            for (var i = 1; i < GridViewRows.length; i++) {
                var row = GridViewRows[i];
                if (row == SelectedRow) {
                    //Apply Yellow color to selected Row
                    row.style.backgroundColor = "#ecf0f6"; 
                    row.style.fontweight = "bold";                         
                }
                else {
                    //Apply White color to rest of rows
                    row.style.backgroundColor = "#ffffff";
                }
            }
        }
    }
}

function RemoveGridRowColor(GridViewId) {
    var GridViewControl = document.getElementById(GridViewId);
    if (GridViewControl != null) {
        var GridViewRows = GridViewControl.rows;
        if (GridViewRows != null) {
            for (var i = 1; i < GridViewRows.length; i++) {
                var row = GridViewRows[i];
                row.style.backgroundColor = "#ffffff";
            }
        }
    }
}