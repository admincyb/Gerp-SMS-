<%@ Page Title="<%$ Resources:Captions,Title_TaxSettings %>" Language="C#" Theme="Classic"
    EnableEventValidation="false" MasterPageFile="~/ERPSMS_2.Master" AutoEventWireup="true"
    CodeBehind="TaxMasterOld.aspx.cs" Inherits="ERPSMS_v01.Administration.Masters.TaxMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/PageScript/Administration/Masters/TaxMaster.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.TaxSettings%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();" />
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" TabIndex="10" OnClientClick="javascript:return SavePage();" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" TabIndex="11" OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();" />
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
                                <asp:Button runat="server" ID="btnAdd" TabIndex="13" SkinID="btnInner-New" ToolTip="<%$Resources:Controls,Add%>"
                                    Text="<%$Resources:Controls,Add%>" EnableViewState="False" OnClientClick="javascript:return AddNew();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" TabIndex="14" SkinID="btnInner-Save" ToolTip="<%$Resources:Controls,Save%>"
                                    Text="<%$Resources:Controls,Save%>" EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" TabIndex="15" SkinID="btnInner-refresh"
                                    ToolTip="<%$Resources:Controls,Reset%>" Text="<%$Resources:Controls,Reset%>"
                                    EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" TabIndex="16" SkinID="btnInner-Cancel"
                                    ToolTip="<%$Resources:Controls,Cancel%>" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelFun();" />
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
                <asp:HiddenField ID="hdfchkToDate" runat="server" />
                <asp:HiddenField ID="taxfromdt" runat="server" />
                <asp:HiddenField ID="TAX_PK" runat="server" Value="0" />
                <asp:HiddenField ID="Type" runat="server" Value="1" />
                <asp:HiddenField ID="UserID" runat="server" Value="0" />
                <asp:HiddenField ID="BIZUNIT" runat="server" Value="0" />
                <asp:HiddenField ID="TaxType" runat="server" Value="0" />
                <asp:HiddenField ID="TaxDate" runat="server" />
                <asp:HiddenField ID="ExtDate" runat="server" />
                <asp:HiddenField ID="DisDate" runat="server" />
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <div class="content">
                                    <label for="TAX_CATEGORY">
                                        <%=Resources.Controls.CategoryName %>
                                        *</label>
                                    <asp:DropDownList runat="server" ID="TAX_CATEGORY" TabIndex="1">
                                    </asp:DropDownList>
                                    <label for="TAX_HEAD">
                                        <%=Resources.Controls.TaxHead%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="TAX_HEAD" MaxLength="100" TabIndex="3"></asp:TextBox>
                                    <label for="TAX_PARM">
                                        <%=Resources.Controls.TaxParameter%>
                                        *</label>
                                    <asp:DropDownList runat="server" ID="TAX_PARM" TabIndex="5" onchange="javascript:MakeFormula();">
                                    </asp:DropDownList>
                                    <div id="divTaxType">
                                        <label for="TAX_PARM">
                                            <%=Resources.Controls.SubCategory%>
                                            *</label>
                                        <asp:DropDownList runat="server" ID="TAX_SUB_CATEGORY" TabIndex="7">
                                        </asp:DropDownList>
                                    </div>
                                    <div id="divTaxnotDue">
                                        <label for="TAX_NOT_DUE">
                                            <%=Resources.Controls.TaxNotDue%>
                                        </label>
                                        <asp:CheckBox ID="TAX_NOT_DUE" runat="server" TabIndex="11" />
                                    </div>
                                    <div id="DIVTAX_EXT">
                                        <div class="div2col-S">
                                            <label for="EXT_TO">
                                                <%=Resources.Controls.ToDate%>*</label>
                                            <asp:TextBox runat="server" ID="EXT_TO" TabIndex="9"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdfextto"></asp:HiddenField>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <div class="content">
                                    <label for="TAX_FROM_DT">
                                        <%=Resources.Controls.FromDate%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="TAX_FROM_DT" ondrop="return CheckKey(event)" onkeydown="return CheckKey(event)"
                                        onpaste="return false;" TabIndex="2"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="taxdisFrm"></asp:HiddenField>
                                    <label for="TAX_TO_DT">
                                        <%=Resources.Controls.ToDate%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="TAX_TO_DT" ondrop="return CheckKey(event)" onkeydown="return CheckKey(event)"
                                        onpaste="return false;" TabIndex="4"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="taxtodt"></asp:HiddenField>
                                    <label for="TAX_Formula">
                                        <%=Resources.Controls.Formula%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="TAX_Formula" MaxLength="500" TabIndex="6"></asp:TextBox>
                                    <label for="COA_NAME">
                                        <%=Resources.Controls.Account%>
                                    </label>
                                    <asp:TextBox ID="COA_NAME" runat="server" MaxLength="100" TabIndex="8" />
                                    <asp:HiddenField ID="TAX_ACCOUNT" runat="server" Value="0" />
                                    <asp:Label runat="server" ID="lblSpace" AssociatedControlID="TAX_EXT" CssClass="floatLeft"></asp:Label>
                                    <div class="checkbx">
                                        <asp:RadioButton runat="server" ID="TAX_EXT" Text="<%$ Resources:Controls, ExtendTill%>"
                                            TabIndex="9" onClick="javascript:TaxDetails(this);" />
                                        <asp:RadioButton runat="server" ID="TAX_DISC" Text="<%$ Resources:Controls, DiscontinueFrom%>"
                                            onClick="javascript:TaxDetails(this);" TabIndex="10" />
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <div id="DIVTAX_DISC">
                                        <label for="DISC_FROM">
                                            <%=Resources.Controls.FromDate%>
                                            *</label>
                                        <asp:TextBox runat="server" ID="DISC_FROM" TabIndex="11"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="divcol-S">
                                <label for="TAX_DESC">
                                    <%=Resources.Controls.Description%></label>
                                <asp:TextBox runat="server" ID="TAX_DESC" TabIndex="12" onkeydown="limitText(this,500);"
                                    onchange="limitText(this,500);" MaxLength="500" EnableViewState="false" EnableTheming="false"
                                    TextMode="MultiLine" Rows="2">
                                </asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <%--       <div id="DIVTAX_EXT">
                    <div class="div2col-S">
                        <label for="EXT_TO">
                            <%=Resources.Controls.ToDate%>*</label>
                        <asp:TextBox runat="server" ID="EXT_TO" TabIndex="9"></asp:TextBox>
                        <asp:HiddenField runat="server" ID="hdfextto"></asp:HiddenField>
                    </div>
                    <div class="clear">
                    </div>
                </div>--%>
            </div>
            <div id="divListing">
                <div id="searchwrap" class="search-wrap-custom1">
                    <div id="divSearch">
                        <span>
                            <%=Resources.Controls.SearchBy%></span>
                        <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType();"
                            EnableViewState="false">
                            <%-- <asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                            </asp:ListItem>--%>
                            <asp:ListItem Value="TAX_HEAD" Text="<%$ Resources:BindValues, Name%>">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false">
                        </asp:TextBox>
                        <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" OnClientClick="javascript:return BindGrid();"
                            EnableViewState="false" />
                        <div style="float: right; display: none">
                            <asp:Button ID="btnAdvSearch" runat="server" Text="Advance Search" OnClientClick="javascript:return ShowAdvSearch();" />
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="grdTable">
                    <div id="divTaxList">
                        <table rules="all" id="grdTaxDetails" grandtype="GrandGrid" paging="false" editfunction="GridActionHandler"
                            editable="true" width="100%" class="gridwraptable gridwrap filter-arrow">
                            <thead>
                                <tr>
                                    <th fieldmap="TAX_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_Type" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_DISC_FROM_DT" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_EXT_FROM_DT" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_EXT_TO_DATE" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_SUB_CATEGORY" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_CATEGORY" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_ACCOUNT" isvisible="false">
                                    </th>
                                     <th fieldmap="TAX_DESC" isvisible="false">
                                    </th>
                                    <th fieldmap="COA_NAME" isvisible="false">
                                    </th>
                                    <th fieldmap="COA_CODE" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_NOT_DUE" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_CATEGORY_TEXT" width="15%" sortable="true">
                                        <%=Resources.Controls.CategoryName%>
                                    </th>
                                    <th fieldmap="TAX_SUB_CATEGORY_TEXT" width="15%" sortable="true">
                                        <%=Resources.Controls.SubCategory%>
                                    </th>
                                    <th fieldmap="TAX_HEAD" width="15%" sortable="true">
                                        <%=Resources.Controls.TaxHead%>
                                    </th>
                                    <th fieldmap="TAX_FORMULA" width="20%" sortable="true">
                                        <%=Resources.Controls.Formula%>
                                    </th>
                                    <th fieldmap="TAX_FROM_DT" width="15%" sortable="true">
                                        <%=Resources.Controls.FromDate%>
                                    </th>
                                    <th fieldmap="TAX_TO_DT" width="15%" sortable="true">
                                        <%=Resources.Controls.ToDate%>
                                    </th>
                                    <th type="Template" width="10%">
                                        <div>
                                            <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$Resources:Controls,Edit %>"
                                                SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                            <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                                SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div id="divPocharges">
                        <table rules="all" id="grdPOCharges" grandtype="GrandGrid" paging="false" editfunction="GridActionHandler"
                            editable="true" width="100%" class="gridwraptable gridwrap filter-arrow">
                            <thead>
                                <tr>
                                    <th fieldmap="TAX_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_Type" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_DISC_FROM_DT" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_EXT_FROM_DT" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_EXT_TO_DATE" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_CATEGORY" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_ACCOUNT" isvisible="false">
                                    </th>
                                    <th fieldmap="COA_NAME" isvisible="false">
                                    </th>
                                    <th fieldmap="COA_CODE" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_CATEGORY_TEXT" width="20%">
                                        <%=Resources.Controls.CategoryName%>
                                    </th>
                                    <th fieldmap="TAX_HEAD" width="20%">
                                        <%=Resources.Controls.TaxHead%>
                                    </th>
                                    <th fieldmap="TAX_FORMULA" width="20%">
                                        <%=Resources.Controls.Formula%>
                                    </th>
                                    <th fieldmap="TAX_FROM_DT" width="15%">
                                        <%=Resources.Controls.FromDate%>
                                    </th>
                                    <th fieldmap="TAX_TO_DT" width="15%">
                                        <%=Resources.Controls.TaxToDate%>
                                    </th>
                                    <th type="Template" width="10%">
                                        <div>
                                            <asp:ImageButton runat="server" ID="imbEditPO" ToolTip="<%$Resources:Controls,Edit %>"
                                                SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                            <asp:ImageButton runat="server" ID="imgDeletePO" ToolTip="<%$Resources:Controls,Delete %>"
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
            </div>
        </div>
        <input type="hidden" id="hdfVirtualPath" value='<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString()) %>' />
    </div>
</asp:Content>
