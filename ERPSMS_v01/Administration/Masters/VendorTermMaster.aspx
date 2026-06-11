<%@ Page Title="<%$ Resources:Captions,Title_VendorTermMaster %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    Theme="ClassicExt" AutoEventWireup="true" CodeBehind="VendorTermMaster.aspx.cs"
    Inherits="LatexERPV2.Masters.VendorTermMaster" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Masters/VendorTermsMaster.js.axd"
        type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.VendorTermMaster%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();" />
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage();"
                TabIndex="4" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                TabIndex="5" />
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
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" Text="<%$Resources:Controls,Add%>"
                                    OnClientClick="javascript:return AddNew();" ToolTip="<%$Resources:Controls,Add%>"
                                    EnableViewState="true" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="4" EnableViewState="true" ToolTip="<%$Resources:Controls,Save%>" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="5" EnableViewState="true" ToolTip="<%$Resources:Controls,Reset%>" OnClientClick="javascript:return ResetPage();" />
                            </li>
                            <li>
                                <%--<asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="true" ToolTip="<%$Resources:Controls,Cancel%>" OnClientClick="javascript:return CancelFun();"
                                    TabIndex="9" />--%>
                                    <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="true" ToolTip="<%$Resources:Controls,Cancel%>"  OnClientClick="javascript:return ResetPage();"
                                    TabIndex="9" />
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
                                <label for="TermTitle">
                                    <%=Resources.Controls.TermsTitle%>*</label>
                                <asp:TextBox ID="TermTitle" runat="server" EnableViewState="false" MaxLength="100"
                                    CssClass="input-half" TabIndex="1"></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <label for="TermType">
                                    <%=Resources.Controls.TermsType%>
                                    *</label>
                                <asp:DropDownList runat="server" ID="TermType" TabIndex="2" CssClass="select-small-a">
                                    <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:BindValues,Date %>" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:BindValues, Text%>" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:BindValues, Numeric%>" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                                <%--Active--%>
                                <label for="chkItemActive" class="lbl-21-5perc">
                                    <%=Resources.Controls.Active%></label>
                                <asp:CheckBox ID="chkItemActive" runat="server" TabIndex="14" EnableViewState="False">
                                </asp:CheckBox>
                                <%--Active End--%>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="divcol-S">
                                <label for="TermDescr">
                                    <%=Resources.Controls.TermsDescription%>*</label><asp:TextBox runat="server" ID="TermDescr"
                                        TextMode="MultiLine" TabIndex="3" onkeypress="return (this.value.length<200)"
                                        onpaste="return this.value.length<200"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divListing">
                <%--class="clear"--%>
                <div id="searchwrap" class="search-wrap-custom1">
                    <asp:Label runat="server" ID="Search_By" Text="Search By" CssClass="srchboxlabel"></asp:Label>
                    <asp:DropDownList ID="SearchType" runat="server" TabIndex="4" CssClass="srchboxtextbx">
                        <%-- <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>"></asp:ListItem>--%>
                        <asp:ListItem Value="VET_TITLE" Text="<%$ Resources:BindValues, TermTitle%>"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="SearchValue" runat="server" CssClass="srchboxtextbx" Width="300px"
                        MaxLength="30">
                    </asp:TextBox>
                    <div class="srchbtnwrap">
                        <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClientClick="javascript:return AfterSelect();" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div id="divGridListing" class="grdTable">
                    <table rules="all" id="grdTermsList" grandtype="GrandGrid" ajaxurl="VendorTermsManagement.do?Action=GetVenderTermsList"
                        pagesize="20" paging="true" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="VET_ACTIVE_TEXT" isvisible="false">
                                </th>
                                 <th fieldmap="VET_PK" isvisible="false">
                                </th>
                                <th fieldmap="VET_TYPE" isvisible="false">
                                </th>
                                <th fieldmap="VET_TITLE" sortable="true" align="left" width="40%">
                                    <%=Resources.Controls.TermsTitle%>
                                </th>
                                <th fieldmap="VET_TYPE_STRING" align="left" width="10%">
                                    <%=Resources.Controls.TermsType%>
                                </th>
                                <th fieldmap="VET_DESC" align="left" width="43%">
                                    <%=Resources.Controls.TermsDescription%>
                                </th>
                                <th fieldmap="VET_ACTIVE" align="center" width="2%">
                                    <%=Resources.Controls.Active%>
                                </th>
                                <th type="Template" width="5%">
                                    <div style="text-align: center">
                                        <asp:ImageButton runat="server" ToolTip="<%$Resources:Controls,Edit %>" ID="ImageButton2"
                                            SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                        <asp:ImageButton runat="server" ToolTip="<%$Resources:Controls,Delete %>" ID="ImageButton3"
                                            SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
                <div class="clear">
                </div>
            </div>
            <asp:HiddenField ID="TermPK" Value="0" runat="server" />
            <asp:HiddenField runat="server" ID="DeptPk" Value="1" />
            <asp:HiddenField ID="ACTIVE" runat="server" Value="1" />
            <%--Used for saving value in VET_ACTIVE column--%>
            <asp:HiddenField ID="SBU" runat="server" Value="0" />
            <div class="clear">
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#TermGrpDiv").dialog({ autoOpen: false });
        });

        function ShowLocation() {
            $("#dialog:ui-dialog").dialog("destroy");
            $("#TermGrpDiv").dialog('open');
        }
    </script>
</asp:Content>
