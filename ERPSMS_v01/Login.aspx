<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="LatexERPV2.Login"
    Title="<%$ Resources:Captions,Title_Login %>" %>

<%@ Register Src="SignIn.ascx" TagName="SignIn" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="shortcut icon" type="image/x-icon" href="images/favicon.ico" />
    <link id="Link1" runat="server" href="Css/login-page.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/Classic/jquery-ui-1.8.13.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        var delayb4scroll = 2000 //Specify initial delay before marquee starts to scroll on page (2000=2 seconds)
        var marqueespeed = 2 //Specify marquee scroll speed (larger is faster 1-10)
        var pauseit = 1 //Pause marquee onMousever (0=no. 1=yes)?

        ////NO NEED TO EDIT BELOW THIS LINE////////////

        var copyspeed = marqueespeed
        var pausespeed = (pauseit == 0) ? copyspeed : 0
        var actualheight = ''

        function scrollmarquee() {
            if (parseInt(cross_marquee.style.top) > (actualheight * (-1) - 8)) //if scroller hasn't reached the end of its height
                cross_marquee.style.top = parseInt(cross_marquee.style.top) - copyspeed + "px" //move scroller upwards
            else //else, reset to original position{
                cross_marquee.style.top = parseInt(marqueeheight) + 8 + "px"
        }

        function initializemarquee() {
            cross_marquee = document.getElementById("vmarquee")
            cross_marquee.style.top = 0
            marqueeheight = document.getElementById("news-container").offsetHeight  //marqueecontainer
            actualheight = cross_marquee.offsetHeight //height of marquee content (much of which is hidden from view)
            if (window.opera || navigator.userAgent.indexOf("Netscape/7") != -1) { //if Opera or Netscape 7x, add scrollbars to scroll and exit
                cross_marquee.style.height = marqueeheight + "px"
                cross_marquee.style.overflow = "scroll"
                return
            }
            setTimeout('lefttime=setInterval("scrollmarquee()",30)', delayb4scroll)
        }

        if (window.addEventListener)
            window.addEventListener("load", initializemarquee, false)
        else if (window.attachEvent)
            window.attachEvent("onload", initializemarquee)
        else if (document.getElementById)
            window.onload = initializemarquee


        function ShowInvalidMessage(Msg) {
            $("#ValidationSummary").html("");
            $("#errorContainer").dialog({
                resizable: false,
                title: "Information",
                modal: true
            });
            return false;
        }
//         $(document).ready(function () {
//             sessionStorage.clear();
//         });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="loginScriptManager" CompositeScript-ScriptMode="Release"
        LoadScriptsBeforeUI="true" ScriptMode="Release">
        <CompositeScript>
            <Scripts>
                <asp:ScriptReference Path="~/Scripts/jquery/jquery-1.6.2.min.js" />
                <asp:ScriptReference Path="~/Scripts/Jquery/jquery-ui.min.js" />
                <asp:ScriptReference Path="~/Scripts/Jquery/jquery.innerfade.js" />
            </Scripts>
        </CompositeScript>
    </asp:ScriptManager>
    <div id="mainwrap">
        <uc1:SignIn ID="SignIn1" runat="server" />
        <div id="banner-container">
            <ul id="portfolio">
                <asp:Repeater ID="repeaterBanner" runat="server">
                    <ItemTemplate>
                        <li>
                            <%--<img src='<%# GetFormattedSrc(Eval(gBudget.Utilities.Constants.DA.Administration.RolloverImages.F_Path).ToString())%>' alt="<%# Eval(gBudget.Utilities.Constants.DA.Administration.RolloverImages.F_Title)%>" />--%>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
        <div class="copyright">
            <asp:Label runat="server" ID="lbnReleaseDate" Text="<%$resources:Controls,ReleaseDate %>"
                AssociatedControlID="lblReleaseDate"></asp:Label>
            <asp:Label ID="lblReleaseDate" runat="server" Text="dd-mm-yy"></asp:Label>
            <asp:Label ID="lblVersion" runat="server"></asp:Label>
            <div class="sitelinks">
                <a href="http://g-erp.com/" target="_blank">www.g-erp.com</a> | <a href="http://www.thegti.com/"
                    target="_blank">www.thegti.com</a></div>
        </div>
        <div id="news-container">
            <div id="vmarquee" onmouseover="copyspeed=pausespeed" onmouseout="copyspeed=marqueespeed">
                <asp:Repeater ID="repeaterLatestNews" runat="server">
                    <ItemTemplate>
                        <h2>
                            <%# DataBinder.Eval(Container.DataItem, "NWE_Title")%></h2>
                        <p>
                            <%# DataBinder.Eval(Container.DataItem, "NWE_Desc")%></p>
                        <a id="A1" href='<%#"~/News/NewsDetails.aspx?NEWSPK="+ DataBinder.Eval(Container.DataItem, "NWE_PK")%>'
                            runat="server" target="_blank">more </a>
                        <hr />
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
    </form>
    <script type="text/javascript">
        $(document).ready(
				function () {

				    $('#portfolio').innerfade({
				        speed: 1500,
				        timeout: 4000,
				        type: 'random_start'
				    });
				    $(document.forms[0]).submit(function () {
				        if (Page_IsValid != null && !Page_IsValid) {
				            $("#ErrorText").html("");
				            $("#errorContainer").dialog({
				                resizable: false,
				                title: "Error!",
				                modal: true

				            });
				        }
				    });
				    if (top !== self || top != self) {
				        window.parent.location.reload();
				    }
				});
    </script>
</body>
</html>
