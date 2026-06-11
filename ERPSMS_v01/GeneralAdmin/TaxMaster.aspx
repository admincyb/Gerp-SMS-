<%@ Page Title="<%$ Resources:Captions,Title_TaxSettings %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" EnableEventValidation="false" Theme="ClassicExt" CodeBehind="TaxMaster.aspx.cs"
    Inherits="ERPSMS_v01.GeneralAdmin.TaxMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        
    </style>
    <script src="../Scripts/PageScript/Administration/Masters/TaxMaster.js.axd" type="text/javascript"></script>
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
                                <asp:Button runat="server" ID="btnAdd" TabIndex="21" SkinID="btnInner-New" ToolTip="<%$Resources:Controls,Add%>"
                                    Text="<%$Resources:Controls,Add%>" EnableViewState="False" OnClientClick="javascript:return AddNew();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" TabIndex="22" SkinID="btnInner-Save" ToolTip="<%$Resources:Controls,Save%>"
                                    Text="<%$Resources:Controls,Save%>" EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" TabIndex="23" SkinID="btnInner-refresh"
                                    ToolTip="<%$Resources:Controls,Reset%>" Text="<%$Resources:Controls,Reset%>"
                                    EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>
                            <li>
                                <%--<asp:Button ID="btnCancel" runat="server" TabIndex="16" SkinID="btnInner-Cancel"
                                    ToolTip="<%$Resources:Controls,Cancel%>" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelFun();" />--%>
                                <%--<asp:Button ID="btnCancel" runat="server" TabIndex="24" SkinID="btnInner-Cancel"
                                    ToolTip="<%$Resources:Controls,Cancel%>" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return RedirectToInbox();" />--%>
                                <asp:Button ID="btnCancel" runat="server" TabIndex="24" SkinID="btnInner-Cancel"
                                    ToolTip="<%$Resources:Controls,Cancel%>" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return ReloadePage();" />
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
                 <asp:HiddenField ID="hdfGstEnabled" runat="server" Value="0" />
                <asp:HiddenField ID="taxfromdt" runat="server" />
                <asp:HiddenField ID="TAX_PK" runat="server" Value="0" />
                <asp:HiddenField ID="Type" runat="server" Value="1" />
                <asp:HiddenField ID="UserID" runat="server" Value="0" />
                <asp:HiddenField ID="BIZUNIT" runat="server" Value="0" />
                <asp:HiddenField ID="TaxType" runat="server" Value="0" />
                <asp:HiddenField ID="TaxDate" runat="server" />
                <asp:HiddenField ID="ExtDate" runat="server" />
                <asp:HiddenField ID="DisDate" runat="server" />
                <asp:HiddenField ID="hdfAccountPK" runat="server" />
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <div class="content">
                                    <label for="TAX_CATEGORY">
                                        <%=Resources.Controls.CategoryName %>
                                        *</label>
                                    <asp:DropDownList runat="server" ID="TAX_CATEGORY" TabIndex="1" onchange="javascript:SetPOChargeAccount();"
                                        CssClass="select-half-a" EnableViewState="false">
                                    </asp:DropDownList>
                                    <div id="divTaxcode">
                                        <label for="TAX_CODE">
                                            <%=Resources.Controls.TaxCode%>
                                        </label>
                                        <asp:TextBox runat="server" ID="TAX_CODE" MaxLength="100" TabIndex="3" CssClass="input-half"></asp:TextBox>
                                        <label for="TAX_RATE">
                                            <%=Resources.Controls.TaxRate%>
                                        </label>
                                        <asp:TextBox ID="TAX_RATE" runat="server" TabIndex="5" CssClass="numeric input-small"
                                            MaxLength="9" onchange="javascript:DisplayFormula();" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);">
                                        </asp:TextBox>
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <label for="TAX_FROM_DT">
                                        <%=Resources.Controls.FromDate%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="TAX_FROM_DT" ondrop="return CheckKey(event)" onkeydown="return CheckKey(event)"
                                        CssClass="input-small" onpaste="return false;" TabIndex="7"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="taxdisFrm"></asp:HiddenField>
                                    <label for="TAX_TO_DT" class="lbl-19-6perc">
                                        <%=Resources.Controls.ToDate%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="TAX_TO_DT" ondrop="return CheckKey(event)" onkeydown="return CheckKey(event)"
                                        CssClass="input-small" onpaste="return false;" TabIndex="8"></asp:TextBox>
                                    <asp:HiddenField runat="server" ID="taxtodt"></asp:HiddenField>
                                    <div class="clear">
                                    </div>
                                    <label for="TAX_Formula" id="lblTAX_Formula" runat="server">
                                        <%=Resources.Controls.Formula%>
                                    </label>
                                    <asp:TextBox runat="server" ID="TAX_Formula" MaxLength="500" TabIndex="10" CssClass="input-half" placeholder="0 means lump sum amount" ></asp:TextBox>
                                    
                                      <span class="label-text-2">(0 means lump sum amount)</span>
                                    <div class="clear"></div>
                                    <label for="COA_NAME">
                                        <%=Resources.Controls.Account%>
                                    </label>
                                    <asp:DropDownList ID="TAX_ACCOUNT" runat="server" TabIndex="12" CssClass="select-half-a">
                                    </asp:DropDownList>
                                    <div id="divTaxActive">
                                        <label for="TAX_ACTIVE">
                                            <%=Resources.Controls.Status%>
                                        </label>
                                        <asp:DropDownList ID="TAX_ACTIVE" runat="server" TabIndex="14" CssClass="select-small-a">
                                            <asp:ListItem Text="<%$ resources:Controls, Active %>" Value="1" />
                                            <asp:ListItem Text="<%$ resources:Controls, InActive %>" Value="0" />
                                        </asp:DropDownList>
                                    </div>
                                    <div>
                                        <asp:Label runat="server" ID="lblSpace" AssociatedControlID="TAX_EXT" CssClass="checkboxalign margn-lft0"></asp:Label>
                                        <asp:RadioButton runat="server" ID="TAX_EXT" TabIndex="16" onClick="javascript:TaxDetails(this);" />
                                        <asp:Label runat="server" ID="lblExtendTill" AssociatedControlID="TAX_EXT" Text="<%$ Resources:Controls, ExtendTill%>"
                                            CssClass="txt-lft middle-lbl-xsmall-c2 margntop-minus3"></asp:Label>
                                        <asp:RadioButton runat="server" ID="TAX_DISC" onClick="javascript:TaxDetails(this);"
                                            TabIndex="16" />
                                        <asp:Label runat="server" ID="lblDiscontinuFrom" AssociatedControlID="TAX_DISC" Text="<%$ Resources:Controls, DiscontinueFrom%>"
                                            CssClass="txt-lft margntop-minus3"></asp:Label>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <div class="content">
                                    <div id="divTaxType">
                                        <label for="TAX_PARM">
                                            <%=Resources.Controls.SubCategory%>
                                            *</label>
                                        <asp:DropDownList runat="server" ID="TAX_SUB_CATEGORY" TabIndex="2" onchange="javascript:SetAccount();"
                                            CssClass="select-half-a">
                                        </asp:DropDownList>
                                    </div>
                                    <label for="TAX_HEAD">
                                        <%=Resources.Controls.TaxHead%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="TAX_HEAD" MaxLength="100" TabIndex="4" CssClass="input-half"></asp:TextBox>
                                    <label for="TAX_DISP_NAME">
                                        <%=Resources.Controls.DisplayName%>
                                    </label>
                                    <asp:TextBox runat="server" ID="TAX_DISP_NAME" MaxLength="100" TabIndex="6" CssClass="input-half"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="TAX_PARM">
                                        <%=Resources.Controls.TaxParameter%>
                                        *</label>
                                    <asp:DropDownList runat="server" ID="TAX_PARM" TabIndex="9" onchange="javascript:MakeFormula();"
                                        CssClass="select-half-a">
                                    </asp:DropDownList>
                                    <div id="divTaxApplicability">
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="Label1" AssociatedControlID="TAX_IS_SALE" Text="<%$ Resources:Controls,Applicability%>"
                                                CssClass="checkboxalign margn-lft0"></asp:Label>
                                            <asp:CheckBox ID="TAX_IS_SALE" runat="server" TabIndex="11" />
                                            <asp:Label runat="server" ID="lblsupply" AssociatedControlID="TAX_IS_SALE" CssClass="txt-lft middle-lbl-xsmall-c2"
                                                Text="<%$ Resources:Controls,Supply%>"></asp:Label>
                                            <asp:CheckBox ID="TAX_IS_PURCHASE" runat="server" TabIndex="11" />
                                            <asp:Label runat="server" ID="lblPurchase" AssociatedControlID="TAX_IS_PURCHASE"
                                                CssClass="txt-lft" Text="<%$ Resources:Controls,Purchase%>"></asp:Label>
                                            
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <%--   <label for="COA_NAME">
                                        <%=Resources.Controls.Account%>
                                    </label>
                                    <asp:TextBox ID="COA_NAME" runat="server" MaxLength="100" TabIndex="8" />
                                    <asp:HiddenField ID="TAX_ACCOUNT" runat="server" Value="0" />--%>
                                        <div class="clear">
                                        </div>
                                        <div id="div2">
                                            <label for="TAX_IS_RETURN">
                                                <%=Resources.Controls.IsGSTReturn%>
                                            </label>
                                            <asp:CheckBox ID="TAX_IS_RETURN" runat="server" TabIndex="13" />
                                        </div>
                                    </div>
                                    <div id="divAutoCalculate">
                                        <asp:Label runat="server" ID="llAutoCalc" AssociatedControlID="TAX_AUTO_OTHER_ENABLE" Text="<%$ Resources:Controls,AutoCalculate%>"
                                                CssClass="checkboxalign margn-lft0"></asp:Label>
                                            <asp:CheckBox ID="TAX_AUTO_OTHER_ENABLE" runat="server" TabIndex="11" />
                                    </div>
                                    <%--  <div class="clear">
                                    </div>--%>
                                    <div id="divTaxnotDue">
                                        <label for="TAX_NOT_DUE">
                                            <%=Resources.Controls.TaxNotDue%>
                                        </label>
                                        <asp:CheckBox ID="TAX_NOT_DUE" runat="server" TabIndex="15" />
                                    </div>
                                    <div id="divGSTGroup">
                                        <label for="TAX_GST_GROUP">
                                            <%=Resources.Controls.gstGroup%>
                                        </label>
                                        <asp:DropDownList ID="TAX_GST_GROUP" runat="server" TabIndex="17" CssClass="select-half-a">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <div id="divTaxFob">
                                        <label for="TAX_IS_FOB_CAL">
                                            <%=Resources.Controls.TaxFob%>
                                        </label>
                                        <asp:CheckBox ID="TAX_IS_FOB_CAL" runat="server" TabIndex="18" />
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <div id="DIVTAX_EXT">
                                        <div class="div2col-S">
                                            <label for="EXT_TO" style="margin-right: 11px;">
                                                <%=Resources.Controls.ToDate%>*</label>
                                            <asp:TextBox runat="server" ID="EXT_TO" TabIndex="18"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="hdfextto"></asp:HiddenField>
                                        </div>
                                        <div class="clear">
                                        </div>
                                    </div>
                                    <div id="DIVTAX_DISC">
                                        <label for="DISC_FROM">
                                            <%=Resources.Controls.FromDate%>
                                            *</label>
                                        <asp:TextBox runat="server" ID="DISC_FROM" TabIndex="19"></asp:TextBox>
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
                                <asp:TextBox runat="server" ID="TAX_DESC" TabIndex="20" onkeydown="limitText(this,500);"
                                    onchange="limitText(this,500);" MaxLength="500" EnableViewState="false" EnableTheming="false"
                                    TextMode="MultiLine" Rows="1">
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
                            <asp:ListItem Value="TAX_CODE" Text="<%$ Resources:BindValues, TaxCode%>">
                            </asp:ListItem>
                            <asp:ListItem Value="TAX_IS_SALE_TEXT" Text="<%$ Resources:BindValues, Supply%>">
                            </asp:ListItem>
                            <asp:ListItem Value="TAX_IS_PURCHASE_TEXT" Text="<%$ Resources:BindValues, Purchase%>">
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
                        <table rules="all" id="grdTaxDetails" grandtype="GrandGrid" paging="true" pagesize="20"
                            editfunction="GridActionHandler" editable="true" width="100%" class="gridwraptable gridwrap filter-arrow">
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
                                    <th fieldmap="TAX_IS_PURCHASE" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_IS_SALE" isvisible="false">
                                    </th>
                                     <th fieldmap="TAX_GST_GROUP" isvisible="false">
                                    </th>
                                    <%--  <th fieldmap="TAX_RATE" isvisible="false">
                                    </th>--%>
                                    <th fieldmap="TAX_IS_RETURN" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_CATEGORY_TEXT" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_ACTIVE" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_CODE" width="9%" sortable="true">
                                        <%=Resources.Controls.TaxCode%>
                                    </th>
                                    <th fieldmap="TAX_SUB_CATEGORY_TEXT" width="12  %" sortable="true">
                                        <%=Resources.Controls.SubCategory%>
                                    </th>
                                    <th fieldmap="TAX_HEAD" width="18%" sortable="true">
                                        <%=Resources.Controls.TaxHead%>
                                    </th>
                                    <th fieldmap="TAX_RATE" width="9%" sortable="true" align="center">
                                        <%=Resources.Controls.RatePerc%>
                                    </th>
                                    <th fieldmap="TAX_FORMULA" sortable="true" isvisible="false">
                                        <%=Resources.Controls.Formula%>
                                    </th>
                                    <th fieldmap="TAX_FROM_DT" width="8%" sortable="true">
                                        <%=Resources.Controls.FromDate%>
                                    </th>
                                    <th fieldmap="TAX_TO_DT" width="8%" sortable="true">
                                        <%=Resources.Controls.ToDate%>
                                    </th>
                                    <th fieldmap="TAX_DISP_NAME" width="17%" sortable="true">
                                        <%=Resources.Controls.DisplayName%>
                                    </th>
                                    <th fieldmap="TAX_IS_SALE_TEXT" width="3%" sortable="true">
                                        <%=Resources.Controls.SaleStatus%>
                                    </th>
                                    <th fieldmap="TAX_IS_PURCHASE_TEXT" width="3%" sortable="true">
                                        <%=Resources.Controls.PurchaseStatus%>
                                    </th>
                                    <th fieldmap="TAX_STATUS" width="5%" align="center">
                                        <%=Resources.Controls.Status%>
                                    </th>
                                    <th type="Template" width="5%">
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
                                    <th fieldmap="TAX_DISP_NAME" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_DESC" isvisible="false">
                                    </th>
                                    <th fieldmap="TAX_IS_FOB_CAL" isvisible="false">
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
                                    <th fieldmap="TAX_AUTO_OTHER_ENABLE" isvisible="false">
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div id="divDeductionTaxList">
                        <table rules="all" id="grdDeductionTax" grandtype="GrandGrid" paging="false" editfunction="GridActionHandler"
                            editable="true" width="100%" class="gridwraptable gridwrap filter-arrow">
                            <thead>
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
                                <th fieldmap="TAX_SUB_CATEGORY" isvisible="false">
                                </th>
                                <th fieldmap="TAX_ACCOUNT" isvisible="false">
                                </th>
                                <th fieldmap="COA_NAME" isvisible="false">
                                </th>
                                <th fieldmap="COA_CODE" isvisible="false">
                                </th>
                                <th fieldmap="TAX_ACTIVE" isvisible="false">
                                </th>
                                <%--<th fieldmap="TAX_DISP_NAME" isvisible="false">
                                </th>--%>
                                <th fieldmap="TAX_DESC" isvisible="false">
                                </th>
                                <th fieldmap="TAX_IS_FOB_CAL" isvisible="false">
                                </th>
                                <th fieldmap="TAX_CODE" width="9%" sortable="true">
                                    <%=Resources.Controls.TaxCode%>
                                </th>
                                <th fieldmap="TAX_SUB_CATEGORY_TEXT" width="12%" sortable="true">
                                    <%=Resources.Controls.SubCategory%>
                                </th>
                                <th fieldmap="TAX_HEAD" width="16%" sortable="true">
                                    <%=Resources.Controls.TaxHead%>
                                </th>
                                <th fieldmap="TAX_RATE" width="9%" sortable="true" align="center">
                                    <%=Resources.Controls.RatePerc%>
                                </th>
                                <th fieldmap="TAX_FORMULA" sortable="true" isvisible="false">
                                    <%=Resources.Controls.Formula%>
                                </th>
                                <th fieldmap="TAX_FROM_DT" width="10%" sortable="true">
                                    <%=Resources.Controls.FromDate%>
                                </th>
                                <th fieldmap="TAX_TO_DT" width="10%" sortable="true">
                                    <%=Resources.Controls.ToDate%>
                                </th>
                                <th fieldmap="TAX_DISP_NAME" width="14%" sortable="true">
                                    <%=Resources.Controls.DisplayName%>
                                </th>
                                <th fieldmap="TAX_STATUS" width="5%" align="center">
                                    <%=Resources.Controls.Status%>
                                </th>
                                <th type="Template" width="5%">
                                    <div>
                                        <asp:ImageButton runat="server" ID="imbEditDeduction" ToolTip="<%$Resources:Controls,Edit %>"
                                            SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                        <asp:ImageButton runat="server" ID="imbDeleteDeduction" ToolTip="<%$Resources:Controls,Delete %>"
                                            SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                    </div>
                                </th>
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
