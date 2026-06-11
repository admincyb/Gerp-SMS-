$(document).ready(function () {

    //
    //Parms1 FileUpload Control ID
   
    //Parms2 HdnFld Id For Save FileTitle , Title As Uploded FileNamewith no extension, and Seprated By ','
    //Parms3 HdnFld Id for Save FileID - GUID+Extenstion , and Seprated By ','
    //    GrandScriptUtils.MakeFileUploader("fupUploader","NewF",  "hdfFileID" , "hdfFileTitle");
    GrandScriptUtils.MakeFileUploader("fupUploader", "FileTitle", "FileID");

});

function SaveFile() {

    
    var jSonString = GrandScriptUtils.FormToJsonString(false);
    $.post("FileUploadHandler.do?Action=SAVEFILE", jSonString, function (data) {
        // Check Machine Details Saved Successfuully or not
        if (parseInt(data) > 0) {
            //GrandScriptUtils.ShowModal("Success", "Success", "Success");
          
        }
        else {
           
            //GrandScriptUtils.ShowModal("Failed", "Failed", "failed");
        }

    });

   
   

    return false;
}