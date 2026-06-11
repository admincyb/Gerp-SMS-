<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminSignIn.ascx.cs"
    Inherits="LatexERPV2.AdminSignIn" %>
<div class="header">
</div>
<div id="login-container">
    <asp:Login ID="LgnAdmin" runat="server" BackColor="Transparent" BorderPadding="4"
        BorderStyle="None" BorderWidth="0px" Font-Names="Verdana" LoginButtonText="LogIn"
        OnAuthenticate="LgnAdmin_Authenticate" TitleText="" Width="460px">
        <titletextstyle backcolor="Transparent" font-bold="True" />
        <checkboxstyle />
        <instructiontextstyle font-italic="True" />
        <loginbuttonstyle backcolor="#FFFBFF" bordercolor="#CCCCCC" borderstyle="Solid" borderwidth="1px"
            font-names="Verdana" forecolor="#284775" />
        <layouttemplate>
        
        
            <table border="0" cellpadding="10" cellspacing="10" align="center"  border="0"
                style="border-collapse: collapse" width="100%">
                <tr>
                    <td class="tdmakecenter">
                        <table border="0" width="100%">
                            <tr>
                                <td align="left" class="L2RmainboxtextText" colspan="3">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" EnableTheming="false"
                                      Text="User Name"></asp:Label>
                                    &nbsp;
                                </td>
                                <td width="5%">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="L2RmainboxtextText" colspan="3" style="height: 8px">
                                </td>
                                <td style="height: 8px" width="5%">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3" style="white-space: nowrap">
                                    <asp:TextBox ID="UserName" runat="server" EnableTheming="false"
                                        Width="100%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                        ControlToValidate="UserName" ErrorMessage="User Name Required" 
                                        ToolTip="User Name" ValidationGroup="LgnAdmin">*</asp:RequiredFieldValidator>
                                </td>
                                <td width="5%">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3" style="height: 8px">
                                </td>
                                <td style="height: 8px" width="5%">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" >
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" EnableTheming="false"
                                      Text="Password"></asp:Label>
                                </td>
                                <td align="right" style="width: 2%">
                                </td>
                                <td align="center" style="white-space: nowrap">
                                    &nbsp;
                                </td>
                                <td align="left" style="white-space: nowrap">
                                </td>
                            </tr>
                            <tr>
                                <td align="left"  style="height: 8px">
                                </td>
                                <td align="right" style="height: 8px">
                                </td>
                                <td align="center" style="height: 8px">
                                </td>
                                <td align="left" style="height: 8px">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="3">
                                    <asp:TextBox ID="Password" runat="server" EnableTheming="false"
                                        TextMode="Password" Width="100%"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                        ControlToValidate="Password" ErrorMessage="Password Required" 
                                        ToolTip="Password" ValidationGroup="LgnAdmin">*</asp:RequiredFieldValidator>
                                </td>
                                <td align="left" style="white-space: nowrap">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table border="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width: 95%;">
                                                <asp:CheckBox ID="RememberMeSet" runat="server" Font-Size="Small" />
                                                <asp:Label ID="lblRemember" runat="server" EnableTheming="false" 
                                                    Text="Remember me"> </asp:Label> 
                                        

                                            </td>
                                            <td align="left" style="white-space: nowrap" width="5%">
                                                <asp:Button ID="LoginButton" runat="server" CommandName="Login" Font-Names="Nina" Text="Login" ValidationGroup="LgnAdmin" Width="71px" />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                        DisplayMode="List" ValidationGroup="LgnAdmin" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4" style="color: red">
                                    <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                    <asp:Label ID="lblFailureText" runat="server" Font-Size="XX-Small" ForeColor="#cccccc"  EnableTheming="false" 
                                        Text="Login Failed" 
                                        Visible="False"></asp:Label> 
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4" style="color: red; height: 8px">
                                </td>
                            </tr>
                            <tr>
                                <td class="Forgot" colspan="4">
                                  
                                    
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
            
        </layouttemplate>
        <labelstyle font-bold="True" />
        <failuretextstyle />
    </asp:Login>
</div>
