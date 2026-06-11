$.validator.addMethod('selectNone', function (value, element) {
    return ($(element).val() != "0");
}, 'Translate(Pleaseselectanoption)');

$(document).ready(function () {
    $(document.forms[0]).validate({
        onclick: false,
        onkeyup: false,
        focusInvalid: false,
        errorElement: "div"
    });

});
function AddValidation() {
   
    $( "[id$=MyNametxt]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: "Please Provide Name" }
    });
    $("[id$=MyInitial]").rules("add", {
        required: true,
        maxlength: 100,
        messages: { required: "Please Provide Initial" }
    });

    $("select[id$=sex]").rules("add", {
        selectNone: true,
        maxlength: 100,
        messages: { selectNone: "Please Provide sex" }
    });

//    $("[id$=MyInitial]").rules("add", {
//        required: true,
//        maxlength: 100,
//        messages: { required: "Please Provide Name" }
//    });


}

function Savapage() {
    AddValidation();
    if ($(document.forms[0]).valid()) {

    }
}