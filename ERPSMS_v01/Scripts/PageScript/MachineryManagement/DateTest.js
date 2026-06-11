

///#region ---------- utility Functions
///#region ---------- Check Time valid Or Not
///<summary>Function to Check Valid Time Or Not</summary>
/// True- FromDate Less Than To Date, 
function CheckTime() {

    // Check To Time Greater Than From Time
    var start = $("input[id$=FromTime]").val();
    var end = $("input[id$=ToTime]").val();
    // Add From Time HH:MM to Array
    startArr = start.split(':');
    // Add To Time HH:MM to Array
    endArr = end.split(':');
    // Calaculate Minutes Differenc
    min = endArr[1] - startArr[1];
    hour_carry = 0;
    // Check Min <60
    if (min < 0) {
        // Yes-  Add 60 Minutes With Minutes  
        min += 60;
        // Add 1 Hour With Hour_Carry
        hour_carry += 1;
    }
    // Calculate Hour = FromTime (Hour) - ToTimeHour(Hour) - HourCarry
    hour = endArr[0] - startArr[0] - hour_carry;
    // Check Hour > 0 For Check Valid or not
    if (hour > 0)
    // Retur True- Time is valid 
        return true;
    else {
        // Check Hour ==0, then check Min > 0
        if (hour == 0) {

            if (min > 0)
            // if Min > 0 it a Valid Time
                return true;
            else
                return false;
        }
        // Check Hour < 0 For Check Time Valid or not
        else if (hour < 0) {
            // Return False - Invalid Time
            return false;
        }

    }
}
///#endregion

///#region ---------- Convert Date Fromat
// Convert 10-Feb-20111 Format to 02/10/2011 
function ConvertDateFormat(date) {

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
    // return as  10-Feb-20111 Format to 02/10/2011
    return conDate[1] + "/" + conDate[0] + "/" + conDate[2];
}
///#endregion

///#region ---------- Check Date is Valid or Not, FromDate And ToDate  Already  Enter Or Not
//<summary>Function to Check New Entry( From Date and ToDate )Already Exists </summary>
function ValidDate(machineJson, mainPK) {
    var fromDate = new Date(ConvertDateFormat($("input[id$=MaintenanceFromDate]").val()));
    var toDate = new Date(ConvertDateFormat($("input[id$=MaintenanceToDate]").val()));
    var grdFromDate;
    var grdToDate;
    // Check One by One Item in a grid With Enter From Time And ToTime - using loop
    for (var i in machineJson.MaintenanceList) {
        // Check Frequency != Day and MainPk(Edit Dtls) !=  MaintenanceList[i].MaintenaceInfoID
        if ((machineJson.MaintenanceList[i].Freequency != "3") && (mainPK != machineJson.MaintenanceList[i].MaintenaceInfoID)) {
            grdFromDate = new Date(ConvertDateFormat(machineJson.MaintenanceList[i].MaintenanceFromDate));
            grdToDate = new Date(ConvertDateFormat(machineJson.MaintenanceList[i].MaintenanceToDate));
            // Check FromDate < ListItem FromDate  OR FromDate > ListItem FromDate
            if (fromDate < grdFromDate || fromDate > grdFromDate) {
            }
            else
            // From Date Equal to List FromDate
                return false;
            // Check ToDate < ListItem ToDate  OR ToDate > ListItem ToDate
            if (toDate < grdToDate || toDate > grdToDate) {
            }
            else
            // ToDate Equal to List ToDate
                return false;

            // Check ListItem FromDate <=  FromDate  OR ListItem ToDate > = FromDate
            if (grdFromDate <= fromDate || grdToDate >= fromDate) {
            }
            else
            // FromDate Between ListItems[i] From Date and ToDate
                return false;
            // Check ListItem ToDate <=  ToDate  OR ListItem FromDate > = ToDate
            if (grdToDate <= toDate || grdFromDate >= toDate) {
            }
            else
                return false;
        }
    }
    return true;
}
///#endregion

///#region ---------- Check Time is Valid or Not, FromTime And ToTime  Already  Enter Or Not
//<summary>Function to Check New Entry (From Time and ToTime) Already Exists </summary>
function ValidDateTime(machineJson, mainPK) {
    var fromTime = parseFloat($("input[id$=FromTime]").val().replace(':', '.'));
    var toTime = parseFloat($("input[id$=ToTime]").val().replace(':', '.'));
    var grdFrmTime;
    var grdToTime;
    for (var i in machineJson.MaintenanceList) {
        // Check Freeqency Type= Day and EditItemPK != ListItem.MaintenaceInfoID
        if ((machineJson.MaintenanceList[i].Freequency == $("select[id$=Freequency]").val()) && (mainPK != machineJson.MaintenanceList[i].MaintenaceInfoID)) {
            // Check Time Already  Exists or Not
            grdFrmTime = parseFloat(machineJson.MaintenanceList[i].MaintenanceFromDate.replace(':', '.'));
            grdToTime = parseFloat(machineJson.MaintenanceList[i].MaintenanceToDate.replace(':', '.'));
            // Check FromTime < ListItem FromTime  OR FromTime > ListItem FromTime
            if (fromTime < grdFrmTime || fromTime > grdFrmTime) {
            }
            else
            // From Time Equal to List FromTime
                return false;
            // Check ToTime < ListItem ToTime  OR ToTime > ListItem ToTime
            if (toTime < grdToTime || toTime > grdToTime) {
            }
            else
            // ToTime  Equal to List ToTime
                return false;
            // Check ListItem FromTime <=  From Time  OR ListItem ToTime > = From Time
            if (grdFrmTime <= fromTime || grdToTime >= fromTime) {
            }
            else
            // FromTime Between ListItems[i] From Time and ToTime
                return false;
            // Check ListItem ToTime <=  ToTime  OR ListItem FromTime > = ToTime
            if (grdToTime <= toTime || grdFrmTime >= toTime) {
            }
            else

                return false;

        }
    }
    return true;
}
///#endregion



///#endregion