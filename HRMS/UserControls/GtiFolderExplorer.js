function afterFolderClose(containerID) {
    if (containerID == "[id$=gtiFolderExplorerBody]") {
        $("[id$=imbSelect]").click();
    }
}


function closeOverlayDiv() {
    $('html').css('overflow', 'auto');
    $('body').css('overflow', 'visible');
    if ($('#divmodel').length > 0) {
        $('#divmodel').hide();
    }
}

function afterFolderBrowserMessageClose() {
    $("[id$=btnFolderBrowserEventInvoker]").click();
}