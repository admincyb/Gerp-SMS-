<%@ Page Title="Configuartion Settings" Language="C#" MasterPageFile="~/Administration/Configurations/AdminConfigMaster.Master"
    EnableEventValidation="false" Theme="ERP-Admin" AutoEventWireup="true" CodeBehind="ConfiguartionSettings.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Configurations.ConfiguartionSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

        <asp:SiteMapPath ID="SiteMapPath1" runat="server" Visible="false">
        </asp:SiteMapPath>
    
    <div id="webwizard-wrap">
        <h1>
            Configuartion Settings
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="3" EnableViewState="False"
                OnClientClick="javascript:return false;" />
            <asp:ImageButton runat="server" ID="imbReset" SkinID="btnreset" TabIndex="5" EnableViewState="False"
                OnClientClick="javascript:return false;" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" TabIndex="6" EnableViewState="False"
                OnClientClick="javascript:return history.go(-1)" />
        </div>
    </div>
    <div class="clear">
    </div>
    <div id="grdTable-wrap">
        <div id="divData">
            <div class="div3col-S">
                <label for="SBUName">
                    SBU
                </label>
                <asp:DropDownList ID="SBUName" runat="server" onchange="javascript:return false;">
                    <asp:ListItem Value="0" Text="All">
                    </asp:ListItem>
                    <asp:ListItem Value="1" Text="SBU 1">
                    </asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="div3col-S">
                <label for="Department">
                    Dept
                </label>
                <asp:DropDownList ID="Department" runat="server">
                    <asp:ListItem Value="0" Text="All">
                    </asp:ListItem>
                    <asp:ListItem Value="1" Text="Purchase">
                    </asp:ListItem>
                    <asp:ListItem Value="2" Text="Invoice">
                    </asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="div3col-S">
                <div class="clear" style="height: 18px">
                </div>
            </div>
            <div class="clear">
            </div>
            <div class="grdTable">
                <table id="MaterialInsert">
                    <thead>
                        <tr>
                            <th width="25%">
                                <b>Name</b>
                            </th>
                            <th width="20%">
                                <b>Data Type</b>
                            </th>
                            <th width="15%">
                                <b>Value</b>
                            </th>
                            <th width="5%">
                                <b>Add</b>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>
                                <asp:TextBox ID="DefaultName" runat="server">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList ID="DefaultType" runat="server" Width="150px">
                                    <asp:ListItem Value="0" Text="String">
                                    </asp:ListItem>
                                    <asp:ListItem Value="1" Text="Float">
                                    </asp:ListItem>
                                    <asp:ListItem Value="2" Text="DateTime">
                                    </asp:ListItem>
                                    <asp:ListItem Value="3" Text="Boolean">
                                    </asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="DefaultValue" runat="server" Width="150px">
                                </asp:TextBox>
                            </td>
                            <td>
                                <asp:ImageButton runat="server" ID="ImageButton1" SkinID="imbaddnew" OnClientClick="javascript:return AddDispersionMaterials();" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <div class="clear">
            </div>
        </div>
</asp:Content>
