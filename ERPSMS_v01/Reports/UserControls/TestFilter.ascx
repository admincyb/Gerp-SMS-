<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestFilter.ascx.cs" Inherits="ERPSMS_v01.Reports.UserControls.TestFilter" %>
<script type="text/javascript">
    function InitUserComponents() {
        UserDateInit();
    }
    function UserDateInit() {
        //<summary>function used to make datepicker</summary>
        GrandScriptUtils.DatePicker("txtFrom", false, false);
        //GrandScriptUtils.DatePicker("txtTo", false, false);
    }
</script>
<asp:UpdatePanel ID="pnlTestFilter" runat="server" class="">
    <ContentTemplate>
        <div class="fields-grpwrap color-grey grp-before pad-t10 color-white">
        <div class="fields-group">
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lbl1" runat="server" Text="first" AssociatedControlID="txtFrom"></asp:Label>
                            <asp:TextBox ID="txtFrom" runat="server"></asp:TextBox>
                        
                            <asp:Label ID="Label2" runat="server" Text="second" AssociatedControlID="ddl1"></asp:Label>
                            <asp:DropDownList ID="ddl1" runat="server" OnSelectedIndexChanged="ActionHandler" AutoPostBack="true">
                                <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Item 1" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="div2col-S">
                            <asp:Label ID="lbl3" runat="server" Text="third" AssociatedControlID="txt2"></asp:Label>
                            <asp:TextBox ID="txt2" runat="server"></asp:TextBox>

                            <asp:Label ID="lbl4" runat="server" Text="fourth" AssociatedControlID="ddl2"></asp:Label>
                            <asp:DropDownList ID="ddl2" runat="server">
                                  <asp:ListItem Text="-Select-" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
            </div>
    </ContentTemplate>
</asp:UpdatePanel>
