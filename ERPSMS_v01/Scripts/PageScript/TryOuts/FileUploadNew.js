
//var FileJson = new Object();

$(document).ready(function () {
   
    // For comma Type
    //GrandScriptUtils.MakeFileUploader("fupUploader", false, "FileTitle", "FileID"); //, FileJson);
    // For list type

    GrandScriptUtils.MakeFileUploader("fupUploader", true, "divFileData", "FILELIST");
});


function SaveFile() {
    FileUpJSON = $("#divDatas").data("FileData");
    // Check Have The Conversion List have value
    if (FileUpJSON.FILELIST.length > 0) {
            $("[id$=FILELIST]").val(JSON.stringify(FileUpJSON.FILELIST));
            var jSonString = GrandScriptUtils.FormToJsonString(false);
            $.post("FileUploadHandler.do?Action=SAVEFILE1", jSonString, function (data) {
                // Check Machine Details Saved Successfuully or not
                if (parseInt(data) > 0) {
                    //GrandScriptUtils.ShowModal(UOMMaster.SAVESUCESSMSG, UOMMaster.INFORMATIONTITLE, UOMMaster.SAVE);
                   // ResetPage();
                }
               // else if (parseInt(data) == 0)
                    //GrandScriptUtils.ShowModal(UOMMaster.UOMCODEALREADYEXISTS, UOMMaster.INFORMATIONTITLE);
               // else
                    //GrandScriptUtils.ShowModal(UOMMaster.ACTIONFAILEDMSG, UOMMaster.INFORMATIONTITLE);


            });

        

    }
   
    return false;
}