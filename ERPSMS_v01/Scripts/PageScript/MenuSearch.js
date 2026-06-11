
///<summary>Document . Ready()- Function For Menu Auto Complete Search</summary>
$(document).ready(function () {
    //Event Fired For Auto Complete __ MenuSearchList
    $('#MenuSearchList').autocomplete("CommonManagement.do?Action=MenuListSearch", {
        width: 200,
        formatItem: function (data, i, n, value) {
            var url;
            if (value.split("-")[1] == 'Child Icon') {
                url = 'Images/ERP-Blue/Buttons/erp-grid-edit.png';

            }
            else {
                url = 'Images/ERP-Blue/Buttons/erp-grid-edit.png';
            }

            return "<img style = 'width:15px;height:17px' src='" + url + "'/> " + value.split("-")[0];

        },
        formatResult: function (data, value) {
            return value.split("-")[0];
        }
    });

    $('#MenuSearchList').result = function (event, data, formatted) {
        var arr = data.toLocaleString().split('-');
        var loc = window.location;
        var pathName = loc.pathname.substring(0, loc.pathname.lastIndexOf('/') + 1);

        var url = loc.href.substring(0, loc.href.length - ((loc.pathname + loc.search + loc.hash).length - pathName.length))
        url += arr[2];
        location.href = url;

    };

});

$(document).ready(function () {

}
