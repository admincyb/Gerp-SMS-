<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MenuControl.ascx.cs"
    Inherits="ERPSMS_v01.UserControls.MenuControl" %>
<script type="text/javascript">


    $(document).ready(function () {

        SetAutoCompleteMenu();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });

    function EndRequestHandler() {

        SetAutoCompleteMenu();
    }

    function HideOverlay() {
        $('#divmodel').hide();
    }

    function SetAutoCompleteMenu() {
        //<summary>Function used for Autocomplete</summary>

        $("[id$=txtMenuSearchBy]").autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "MenuManagement.do?Action=GetAutoCompleteMenu",
                    data: {
                        SearchValue: request.term
                    },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return {
                                value: item.Value,
                                id: item.Text
                            }
                        }));
                    }
                });
            },
            select: function (event, ui) {
                var selecteditem = ui.item;
                
//                var pageURL = window.document.URL;
//                var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
//                window.location = pageURL.replace(location.pathname + location.search, virtualPath == "" ? selecteditem.id : "/" + virtualPath + selecteditem.id);
                window.location = selecteditem.id;
            }
        });

        $("[id$=txtMenuSearchBy]").val("Select/Type");

        $("[id$=txtMenuSearchBy]").next("a").remove();
        this.anchor = $("<a>")
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.insertAfter($("[id$=txtMenuSearchBy]"))
					.addClass("ddlSelect")
					.click(function () {
					    if ($("[id$=txtMenuSearchBy]").autocomplete("widget").is(":visible")) {
					        $("[id$=txtMenuSearchBy]").autocomplete("close");
					        return;
					    }
					    $("[id$=txtMenuSearchBy]").autocomplete("search", "%");
					    $("[id$=txtMenuSearchBy]").focus();
					});

    }

</script>
<asp:UpdatePanel ID="aupdMenu" runat="server">
    <ContentTemplate>
        <table align="center" id="tblMenuContainer" style="width: 620px">
            <tr>
                <td align="center">
                    <ul class="regfloat">
                        <%-- <li>
                            <asp:Label runat="server" ID="lblLocation" Text="<%$Resources:Controls,SBU %>" AssociatedControlID="ddlLocation"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlLocation" Width="35%" AutoPostBack="True" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <asp:Label runat="server" ID="lblCostCenter" Text="<%$Resources:Controls,Department %>"
                                AssociatedControlID="ddlCostCenter"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlCostCenter" Width="35%" AutoPostBack="True" OnSelectedIndexChanged="ddlCostCenter_SelectedIndexChanged">
                            </asp:DropDownList>
                             <div class="clear"></div>
                        </li>--%>
                        <li>
                            <asp:Label runat="server" ID="Label1" Text="<%$Resources:Controls,NavigateTo %>"
                                AssociatedControlID="txtMenuSearchBy"></asp:Label>
                            <asp:TextBox ID="txtMenuSearchBy" runat="server" onClick="$(this).select();" Width="233px"></asp:TextBox>
                        </li>
                    </ul>
                    <div class="clear">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div id="mastermenu-wrap">
                        <asp:DataList ID="rptMenu" runat="server" OnItemDataBound="rptMenu_ItemDataBound"
                            DataMember="SectionID" RepeatDirection="Horizontal" RepeatColumns="2">
                            <ItemTemplate>
                                <table align="center">
                                    <tr>
                                        <td>
                                            <ul class="hrzlayout">
                                                <li>
                                                    <div class="mastermenu-boxtype-wrap">

                                                        <table>
                                                            <div class="header">
                                                                <img runat="server" alt="" id="imgMain" src='<%#"~/images/ERP-BLUE/" + DataBinder.Eval(Container.DataItem,"ImageUrl")%>' />
                                                                <%--<asp:Literal runat=server ID="headsection" Text='<%#GetMenuRes(DataBinder.Eval(Container.DataItem, "SectionHead"))%>' ></asp:Literal>--%>
                                                                <%#DataBinder.Eval(Container.DataItem, "SectionHead")%>
                                                                <div class="clear">
                                                                </div>
                                                            </div>
                                                            <tr>
                                                                <td>
                                                                    <div class="linksWrap">
                                                                        <asp:Repeater runat="server" ID="rptGroup" OnItemDataBound="rptGroup_ItemDataBound">
                                                                            <ItemTemplate>
                                                                                <h3>
                                                                                    <%#DataBinder.Eval(Container.DataItem, "GroupName")%></h3>
                                                                                <asp:Repeater runat="server" ID="rptLinks">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton runat="server" ID="lbnLink" CommandArgument='<%#ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty ? DataBinder.Eval(Container.DataItem, "PostUrl").ToString()+DataBinder.Eval(Container.DataItem, "LinkUrl").ToString():DataBinder.Eval(Container.DataItem, "LinkUrl") %>'
                                                                                            Text='<%#DataBinder.Eval(Container.DataItem, "LinkText")%>' CommandName="LINKBUTTONCLICK"
                                                                                            OnClick="ActionHandler"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:Repeater>
                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div class="btnWrap">
                                                                        <%--<img runat="server" alt="" id="imgMain" src='<%#"~/images/Classic/" + DataBinder.Eval(Container.DataItem,"ImageUrl")%>' />--%>
                                                                        <asp:DataList runat="server" ID="dtlstIcons" RepeatDirection="Horizontal" RepeatColumns="4"
                                                                            Height="25px" Width="25px">
                                                                            <ItemStyle />
                                                                            <ItemTemplate>
                                                                                <a href='<%#DataBinder.Eval(Container.DataItem,"IconLink")%>'>
                                                                                    <img id="imgIcon" runat="server" src='<%#"~/images/Classic/" + DataBinder.Eval(Container.DataItem,"IconImage")%>'
                                                                                        style="border: 0 none" /></a>
                                                                            </ItemTemplate>
                                                                        </asp:DataList>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </li>
                                            </ul>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                    <div class="menu-fixed">
                        <asp:Label runat="server" ID="lblLocation" Text="<%$Resources:Controls,SBU %>" AssociatedControlID="ddlLocation"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlLocation"  AutoPostBack="True"
                            OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:Label runat="server" ID="lblCostCenter" Text="<%$Resources:Controls,Department %>"
                            AssociatedControlID="ddlCostCenter"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlCostCenter"  AutoPostBack="True"
                            OnSelectedIndexChanged="ddlCostCenter_SelectedIndexChanged">
                        </asp:DropDownList>
                        <div class="clear">
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
        <%-- <asp:AsyncPostBackTrigger ControlID="ddlLocation" />
                <asp:AsyncPostBackTrigger ControlID="ddlCostCenter" />--%>
    </Triggers>
</asp:UpdatePanel>
