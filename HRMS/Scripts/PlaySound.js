 function getRootWebSitePath()
{
    var _location = document.location.toString();
    var applicationNameIndex = _location.indexOf('/', _location.indexOf('://') + 3);
    var applicationName = _location.substring(0, applicationNameIndex) + '/';
    var webFolderIndex = _location.indexOf('/', _location.indexOf(applicationName) + applicationName.length);
    var webFolderFullPath = _location.substring(0, webFolderIndex);

    return webFolderFullPath;
}
function playAudio(audioFile) {
    var mediaSrc = "";
    if ($("[id$='hdfAbsolutePath']").val() == "")
        mediaSrc = location.protocol + "//" + location.host + '/MusicFiles/' + audioFile;
    else
        mediaSrc = getRootWebSitePath() + '/MusicFiles/' + audioFile;
    $("#hiddenPlayer").remove()
    $('body').append('<div style="display:none;"><audio id="hiddenPlayer" controls preload="auto" autobuffer autoplay>' +
                        '<source src="' + mediaSrc + '.ogg" type="audio/ogg">' +
                        '<source src="' + mediaSrc + '.mp3" type="audio/mpeg">' +
                        '<source src="' + mediaSrc + '.wav" type="audio/wav">' +
                     '</audio></div>');
}