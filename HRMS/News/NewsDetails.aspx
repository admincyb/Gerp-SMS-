<%@ Page Title="" Language="C#" Theme="ClassicExt" AutoEventWireup="true" CodeBehind="NewsDetails.aspx.cs"
    Inherits="ERPSMS_v01.News.NewsDetails" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../App_Themes/Classic/Classic.css" rel="stylesheet" type="text/css" />
</head>
<body style="overflow-y:hidden;">
    <form id="form1" runat="server">
    <div id="newswrap">
        <div id="newsbodywrap">
            <div>
                <asp:Label runat="server" CssClass="newsTitle" ID="lblTitle"></asp:Label>
                <asp:Label runat="server" CssClass="newsContent" ID="lblShrtDesc"></asp:Label>
                <div class="clear">
                </div>
                <div id="divNewsDetails" runat="server" class="news-details">
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="blockr">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
