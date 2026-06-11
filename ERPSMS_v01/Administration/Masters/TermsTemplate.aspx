<%@ Page Title="<%$ Resources:Captions,Title_GeneralTemplateMaster %>" Language="C#"
    MasterPageFile="~/ERPSMS_2.Master" EnableEventValidation="false" AutoEventWireup="true"
    Theme="ClassicExt" CodeBehind="TermsTemplate.aspx.cs" Inherits="ERPSMS_01.Administration.Masters.TermsTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Masters/GeneralTemplateMaster.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%-- <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.GeneralTemplateMaster %></h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();" />
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                TabIndex="9" />
        </div>
    </div>--%>
    <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li>
                                <asp:Button runat="server" ID="btnAddNew" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    EnableViewState="False" OnClientClick="javascript:return AddNew();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelPage();" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <div id="divData">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <label for="TemplateGroup">
                                    <%=Resources.Controls.TemplateGroup %>*</label>
                                <asp:DropDownList runat="server" ID="TMH_TERM_GROUP" Width="320px">
                                </asp:DropDownList>
                                <asp:ImageButton ID="btnAddTemplate" runat="server" SkinID="imbaddnew" ToolTip='<%=Resources.Messages.AddTemplateGroup%>'
                                    OnClientClick="javascript:return ShowTemplateGroup()" />
                                <%--<a href="javascript:;" onclick="javascript:ShowTemplateGroup()" class="imgbtnwrap">
                    <img src="../../Images/ERP-Blue/Buttons/add.png" border="none" style="vertical-align: top;"
                        title='<%=Resources.Messages.AddTemplateGroup%>' /></a>--%>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <label for="TemplateName">
                                    <%=Resources.Controls.TemplateName %>*</label>
                                <asp:TextBox runat="server" ID="TMH_NAME" CssClass="input-half" MaxLength="100">
                                </asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="grdTable">
                    <h3>
                        <%=Resources.Controls.TermsDetails %></h3>
                    <div id="TermsInsert">
                        <table id="TermsInsertTable" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th width="4%" style="text-align: left">
                                        <%=Resources.Controls.SL %>
                                    </th>
                                    <th width="32%" style="text-align: left">
                                        <%=Resources.Controls.Title %>*
                                    </th>
                                    <th width="43%" style="text-align: left">
                                        <%=Resources.Controls.Description %>*
                                    </th>
                                    <th width="8%" style="text-align: left">
                                        <%=Resources.Controls.IsRequired %>
                                    </th>
                                    <th width="2%" style="text-align: left">
                                        <%=Resources.Controls.Active%>
                                    </th>
                                    <th width="6%" style="text-align: left">
                                        <%=Resources.Controls.MaxPoint %>
                                    </th>
                                    <th width="5%" style="text-align: left">
                                        <%=Resources.Controls.Action %>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr class="grd-rowhead">
                                    <td>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TMD_NAME" runat="server" Width="90%" MaxLength="50">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="TMD_PK" runat="server" Value="0" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TMD_DESC" runat="server" Width="90%" EnableTheming="false" MaxLength="1000"
                                            TextMode="MultiLine" onkeypress="return (this.value.length<1000)" onpaste="return this.value.length<1000"
                                            Height="50px">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="TMD_REQD" runat="server"></asp:CheckBox>
                                    </td>
                                     <td>
                                        <asp:CheckBox ID="TMD_ACTIVE" runat="server"></asp:CheckBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TMD_MAX_POINT" MaxLength="3" CssClass="small-a1" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:ImageButton runat="server" ID="btnAdd" SkinID="imbaddnew" OnClientClick="return AddTemplateDetails();" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="clear">
                </div>
                <div class="grdTable">
                    <table rules="all" id="grdTermsDetails" grandtype="GrandGrid" ajaxurl="VendorTermsManagement.do?Action=GetVenderTermsList"
                        paging="false" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="TMD_PK" isvisible="false" width="0%">
                                    SL
                                </th>
                                <th fieldmap="SL" align="center" width="4%">
                                    <%=Resources.Controls.SL %>
                                </th>
                                <th fieldmap="TMD_NAME" align="left" width="32%">
                                    <%=Resources.Controls.Title %>
                                    *
                                </th>
                                <th fieldmap="TMD_DESC" align="left" width="43%">
                                    <%=Resources.Controls.Description %>*
                                </th>
                                <th fieldmap="TMD_REQD" isvisible="false">
                                    Is Required
                                </th>
                                <th fieldmap="TMD_REQD_TEXT" align="left" width="8%">
                                    <%=Resources.Controls.IsRequired %>
                                </th>
                                 <th fieldmap="TMD_ACTIVE" isvisible="false">
                                </th>
                                 <th fieldmap="TMD_ACTIVE_TEXT" align="left" width="2%">
                                     <%=Resources.Controls.Action %>
                                </th>
                                <th fieldmap="TMD_MAX_POINT" align="left" width="6%">
                                    <%=Resources.Controls.MaxPoint %>
                                </th>
                                <th type="Template" width="5%" align="center">
                                    <div style="text-align: left;">
                                        <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Edit')" />
                                        <asp:ImageButton runat="server" ID="imbDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'Delete')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div id="divListing">
                <%--class="clear"--%>
                <div id="searchwrap" class="search-wrap-custom1">
                    <asp:Label runat="server" ID="Search_By" Text="Search By" CssClass="srchboxlabel"></asp:Label>
                    <asp:DropDownList ID="SearchType" runat="server" TabIndex="4" CssClass="srchboxtextbx">
                        <asp:ListItem Value="0" Text="<%$ Resources:BindValues,Select%>"></asp:ListItem>
                        <asp:ListItem Value="TMH_NAME" Text="<%$ Resources:BindValues,TemplateName%>"></asp:ListItem>
                        <asp:ListItem Value="TMG_NAME" Text="<%$ Resources:BindValues,TemplateGroup%>"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="SearchValue" runat="server" CssClass="input-medium" MaxLength="30">
                    </asp:TextBox>
                    <div class="srchbtnwrap">
                        <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClientClick="javascript:return false;" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="grdTable">
                    <table rules="all" id="grdTermsList" grandtype="GrandGrid" ajaxurl="GeneralTemplateMaster.do?Action=GetTemplateList&Status="
                        pagesize="20" paging="true" editfunction="GridAction" editable="true" style="width: 100%"
                        class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="TMH_PK" isvisible="false">
                                </th>
                                <%--<th>
                             SL
                             </th>--%>
                                <th fieldmap="TMH_NAME" sortable="true" align="left" width="40%">
                                    <%=Resources.Controls.TemplateName %>
                                </th>
                                <th fieldmap="TMH_TERM_GROUP" isvisible="false">
                                </th>
                                <th fieldmap="TMG_NAME" sortable="true" align="left" width="55%">
                                    <%=Resources.Controls.TemplateGroup %>
                                </th>
                                <th type="Template" width="5%">
                                    <div style="text-align: left">
                                        <asp:ImageButton runat="server" ID="ImageButton6" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandlerMain($(this).parents('tr:eq(0)'),'Edit')" />
                                        <asp:ImageButton runat="server" ID="ImageButton7" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandlerMain($(this).parents('tr:eq(0)'),'Delete')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
            <div class="clear">
            </div>
            <div id="dialog-Template" style="width: 750px; height: 250px; display: none" title="Create Template Group">
                <div class="content-wrapper">
                    <div class="divcolmiddle-S">
                        <label for="TermName" class="lbl-32-2perc">
                            <%=Resources.Controls.TemplateGroupName %>
                            *</label>
                        <asp:TextBox ID="TermName" runat="server" MaxLength="100" CssClass="input-small-d">
                        </asp:TextBox>
                        <asp:ImageButton ID="imbSaveTemplate" runat="server" SkinID="imbaddnew" OnClientClick="return SaveTemplateGroup()" />
                        <%--<asp:Button runat="server" ID="Button1" Text="<%$ Resources:Controls, Save%>" 
                   SkinID="btnInner-Save" OnClientClick="return SaveTemplateGroup()" />--%>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="grdTable">
                        <table rules="all" id="grdTemplateGroup" grandtype="GrandGrid" ajaxurl="MachineryManagement.do?Action=GetMachineTypeDetails"
                            pagesize="5" paging="true" editfunction="GridAction" style="width: 100%" editable="true"
                            class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="TMG_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="TMG_NAME" sortable="true" align="left" width="92%">
                                        <%=Resources.Controls.TemplateGroup %>
                                    </th>
                                    <th type="Template" width="8%">
                                        <div style="text-align: left">
                                            <asp:ImageButton runat="server" ID="imbMachineEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandlerTemplateGroup($(this).parents('tr:eq(0)'),'editTemplateGroup')" />
                                            <asp:ImageButton runat="server" ID="imbMachineDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandlerTemplateGroup($(this).parents('tr:eq(0)'),'deleteTemplateGroup')" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="TMH_PK" />
    <asp:HiddenField runat="server" ID="TemplateDetails" />
    <asp:HiddenField runat="server" ID="TermsID" Value="0" />
    <asp:HiddenField runat="server" ID="BizUnitPk" Value="1" />
    <asp:HiddenField runat="server" ID="UserPk" Value="1" />
    <asp:HiddenField runat="server" ID="DeptPk" Value="1" />
    <asp:HiddenField runat="server" ID="TemplateGrpPK" Value="0" />
</asp:Content>
