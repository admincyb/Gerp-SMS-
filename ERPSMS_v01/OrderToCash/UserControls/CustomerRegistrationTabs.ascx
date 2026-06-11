<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerRegistrationTabs.ascx.cs"
    Inherits="ERPSMS_v01.OrderToCash.UserControls.CustomerRegistrationTabs" %>
    
<div id="divTab" runat="server" class="tab-container">
    <ul id="tab-menu">
        <asp:Repeater ID="rtrDynamicTab" runat="server">
            <ItemTemplate>
                <li>
                    <span id="spnCustomerRegistrationTab" runat="server" class="tab-inactive">
                        <asp:LinkButton runat="server" ID="lnkCustomerRegistrationTab" Text='<%#Eval("ATC_NAME")%>'
                        ToolTip='<%#Eval("ATC_TOOLTIP")%>'
                            OnClick="ActionHandler" CommandName="DYNAMICTAB" CommandArgument='<%#Eval("ATC_CODE")%>'
                            OnClientClick='<%# "window.location.href=\"" + ResolveUrl("~/OrderToCash/CustomerRegistration.aspx?Tab=" + Eval("ATC_CODE")) + "\"; return false;" %>'
                            CssClass="tab-inactive"></asp:LinkButton>
                    </span>
                    <asp:HiddenField ID="hdfDynamicTabDesc" runat="server" Value='<%# Eval("ATC_MESSAGE") %>' />
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
            <asp:HiddenField runat="server" ID="hdfCurrentDepartment" Value="-1" />
</div>
