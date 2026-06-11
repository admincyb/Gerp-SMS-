<%@ Page Theme="ClassicExt" Language="C#" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="UserProfile.aspx.cs" Inherits="ERPSMS_v01.GeneralAdmin.UserProfile"
    ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .iframe-placeholder
        {
            background: url(../Images/Classic/layout/loader-img.gif) center center no-repeat;
        }
    </style>
    <script type="text/javascript">
        function SetTabs(tab) {
            if (tab == 1) {
                $("[id$='lnkEditInfo']").removeClass("inact-tab").addClass("act-tab");
                $("[id$='lnkInboxMapping']").removeClass("inact-tab").addClass("inact-tab");
                $("[id$='lnkChangePassword']").removeClass("inact-tab").addClass("inact-tab");
            }
            if (tab == 2) {
                $("[id$='lnkEditInfo']").removeClass("inact-tab").addClass("inact-tab");
                $("[id$='lnkInboxMapping']").removeClass("inact-tab").addClass("act-tab");
                $("[id$='lnkChangePassword']").removeClass("inact-tab").addClass("inact-tab");
            }
            if (tab == 3) {
                $("[id$='lnkEditInfo']").removeClass("inact-tab").addClass("inact-tab");
                $("[id$='lnkInboxMapping']").removeClass("inact-tab").addClass("inact-tab");
                $("[id$='lnkChangePassword']").removeClass("inact-tab").addClass("act-tab");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="aupdpnlUsrProfile" runat="server">
        <ContentTemplate>
            <div class="content-wrapper">
                <ul class="bredcrum-iframe">
                    <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                </ul>
                <div class="div-lft">
                    <ul>
                        <li>
                            <%--<span id="spnEditInfo" runat="server" class="tab-active">--%>
                            <asp:LinkButton ID="lnkEditInfo" runat="server" Text="<%$Resources:UserProfile %>"
                                OnClick="ActionHandler" CommandName="EDIT">
                            </asp:LinkButton>
                            <%--</span>--%></li>
                        <li>
                            <%--<span id="spnInboxMapping" runat="server" class="tab-active">--%>
                            <asp:LinkButton ID="lnkInboxMapping" runat="server" Text="<%$Resources:InboxMapping %>"
                                OnClick="ActionHandler" CommandName="INBOXMAPPING">
                            </asp:LinkButton>
                            <%--  </span>--%></li>
                        <li>
                            <%--<span id="spnChangePassword" runat="server" class="tab-active">--%>
                            <asp:LinkButton ID="lnkChangePassword" runat="server" Text="<%$Resources:ChangePassword %>"
                                OnClick="ActionHandler" CommandName="CHANGEPASSWORD">
                            </asp:LinkButton>
                            <%-- </span>--%></li>
                        <%--<li>
                  <asp:LinkButton ID="lnkUserActivities" runat="server" Text="<%$Resources:UserActivities %>" OnClick="ActionHandler" CommandName="ACTIVITIES">
                  </asp:LinkButton>
               </li>--%>
                    </ul>
                </div>
                <div class="div-rgt">
                    <%--------iframe--------------%>
                    <div id="divFrame" visible="false" class="iframe-placeholder" runat="server" style="width: 100%;
                        height: 700px; overflow-y: auto">
                        <%--<iframe runat="server" id="frmDetails" frameborder="0" width="100%" height="100%"
                            style="border: none; overflow-x: hidden;" scrolling="no"></iframe>--%>
                    </div>
                    <%--------iframe--------------%>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
