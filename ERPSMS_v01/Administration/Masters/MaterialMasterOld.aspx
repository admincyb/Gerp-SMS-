<%@ Page Title="<%$ Resources:Captions,Title_MaterialMaster %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="Classic" CodeBehind="MaterialMasterOld.aspx.cs"
    Inherits="ERPSMS_v01.Administration.Masters.MaterialMaster" ResponseEncoding="utf-8" %>

<%@ Register Src="../../UserControls/AdvanceSearch.ascx" TagName="AdvanceSearch"
    TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/JSLINQ/JSLINQ.js" type="text/javascript"></script>
    <script src="../../Scripts/PageScript/MaterialManagement/MaterialMaster.js.axd" type="text/javascript"
        charset="UTF-8"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <div id="webwizard-wrap"> 
        <h1>
            <%=Resources.Captions.MaterialManagement%></h1>
        <div class="button-wrap">
            <asp:ImageButton ID="imbAdd" runat="server" SkinID="btnadd" OnClientClick="javascript:return AddNew();"
                TabIndex="10" EnableViewState="false" />
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage();"
                TabIndex="11" EnableViewState="false" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                TabIndex="12" EnableViewState="false" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                TabIndex="13" />
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
                                <asp:Button ID="btnAdd" runat="server" SkinID="btnInner-New" ToolTip="<%$Resources:Controls,Add%>"
                                    Text="<%$Resources:Controls,Add%>" OnClientClick="javascript:return AddNew();"
                                    TabIndex="31" EnableViewState="false" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnCopy" SkinID="btnInner-copy" ToolTip="<%$Resources:Controls,NewVersion%>"
                                    Text="<%$Resources:Controls,NewVersion%>" TabIndex="32" EnableViewState="False"
                                    OnClientClick="javascript:return CopyVerion();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" ToolTip="<%$Resources:Controls,Save%>"
                                    Text="<%$Resources:Controls,Save%>" TabIndex="33" EnableViewState="False" OnClientClick="javascript:return SavePage();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" ToolTip="<%$Resources:Controls,Reset%>"
                                    Text="<%$Resources:Controls,Reset%>" TabIndex="34" EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" ToolTip="<%$Resources:Controls,Cancel%>"
                                    Text="<%$Resources:Controls,Cancel%>" EnableViewState="False" OnClientClick="javascript:return CancelMaterialMaster();"
                                    TabIndex="35" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="clear">
    </div>
    <div class="content-wrapper">
        <div id="grdTable-wrap">
            <div id="divListing">
                <div id="searchwrap" class="search-wrap-custom1">
                    <div id="divSearch">
                        <span>
                            <%=Resources.Controls.SearchBy%></span>
                        <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" onchange="javascript:SetSearchType();"
                            EnableViewState="false">
                            <%--<asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                            </asp:ListItem>--%>
                            <asp:ListItem Value="ITM_CODE" Text="<%$ Resources:BindValues, ItemCode%>">
                            </asp:ListItem>
                            <asp:ListItem Value="ITM_NAME" Text="<%$ Resources:BindValues, Name%>">
                            </asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="SearchValue" runat="server" EnableViewState="false">
                        </asp:TextBox>
                        <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" EnableTheming="true"
                            OnClientClick="javascript:return BindGrid();" EnableViewState="false" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <uc1:AdvanceSearch ID="AdvanceSearch1" runat="server" />
                <div class="gridwrap">
                    <div id="divMaterialList">
                        <table rules="all" id="grdCategory" grandtype="GrandGrid" pagesize="20" paging="true"
                            width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="ITM_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_MOQ" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_DESC" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_CODE" sortable="true" align="left" width="13%">
                                        <%=Resources.Controls.MaterialCode%>
                                    </th>
                                    <th fieldmap="ITM_NAME" sortable="true" align="left" width="23%">
                                        <%=Resources.Controls.MaterialName%>
                                    </th>
                                    <th fieldmap="ITC_PK" sortable="true" align="left" isvisible="false">
                                    </th>
                                    <th fieldmap="ITC_NAME" sortable="true" align="left" width="12%">
                                        <%=Resources.Controls.CategoryName%>
                                    </th>
                                    <th fieldmap="TmrBleRtr" sortable="true" align="left" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_TYPE_TEXT" sortable="true" align="left" width="13%">
                                        <%=Resources.Controls.ItemClass%>
                                    </th>
                                    <th fieldmap="ITM_TYPE" sortable="true" isvisible="false">
                                        <%=Resources.Controls.MaterialType%>
                                    </th>
                                    <th fieldmap="ITM_DESC" sortable="true" align="center" isvisible="false">
                                    </th>
                                    <th fieldmap="UOM_NAME" sortable="true" align="left" width="7%">
                                        <%=Resources.Controls.MaterialUOM%>
                                    </th>
                                    <th fieldmap="UOM_PK" sortable="true" align="center" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_MIN_STK" sortable="true" align="right" width="10%">
                                        <%=Resources.Controls.MinStockLevel%>
                                    </th>
                                    <th fieldmap="ITM_MAX_STK" sortable="true" align="right" width="13%">
                                        <%=Resources.Controls.MaxStockLevel%>
                                    </th>
                                    <th fieldmap="ITM_ROL_STK" sortable="true" align="right" isvisible="false">
                                        <%=Resources.Controls.ReorderLevel%>
                                    </th>
                                    <th type="Template" width="8%" align="left">
                                        <div style="text-align: left">
                                            <asp:ImageButton runat="server" ID="imbEditMast" ToolTip="<%$Resources:Controls,Edit %>"
                                                SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')"
                                                EnableViewState="false" />
                                            <asp:ImageButton runat="server" ID="imbDeleteMast" ToolTip="<%$Resources:Controls,Delete %>"
                                                SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')"
                                                EnableViewState="false" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div id="divPackingList">
                        <table rules="all" id="grdPackingList" grandtype="GrandGrid" pagesize="20" paging="true"
                            width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="ITM_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_MOQ" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_DESC" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_CODE" sortable="true" align="left" width="13%">
                                        <%=Resources.Controls.MaterialCode%>
                                    </th>
                                    <th fieldmap="ITM_NAME" sortable="true" align="left" width="20%">
                                        <%=Resources.Controls.MaterialName%>
                                    </th>
                                    <th fieldmap="ITC_PK" sortable="true" align="left" isvisible="false">
                                    </th>
                                    <th fieldmap="ITC_NAME" sortable="true" align="left" width="17%">
                                        <%=Resources.Controls.CategoryName%>
                                    </th>
                                    <th fieldmap="TmrBleRtr" sortable="true" align="left" isvisible="false">
                                    </th>
                                    <th fieldmap="IPD_TYPE_TEXT" sortable="true" align="left" width="12%">
                                        <%=Resources.Controls.Type%>
                                    </th>
                                    <th fieldmap="ITM_TYPE" sortable="true" isvisible="false">
                                        <%=Resources.Controls.MaterialType%>
                                    </th>
                                    <th fieldmap="ITM_DESC" sortable="true" align="center" isvisible="false">
                                    </th>
                                    <th fieldmap="UOM_NAME" sortable="true" align="left" width="7%">
                                        <%=Resources.Controls.MaterialUOM%>
                                    </th>
                                    <th fieldmap="UOM_PK" sortable="true" align="center" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_MIN_STK" sortable="true" align="right" width="11%">
                                        <%=Resources.Controls.MinStockLevel%>
                                    </th>
                                    <th fieldmap="ITM_MAX_STK" sortable="true" align="right" width="10%">
                                        <%=Resources.Controls.MaxStockLevel%>
                                    </th>
                                    <th fieldmap="ITM_ROL_STK" sortable="true" align="right" isvisible="false">
                                        <%=Resources.Controls.ReorderLevel%>
                                    </th>
                                    <th type="Template" width="8%" align="left">
                                        <div style="text-align: left">
                                            <asp:ImageButton runat="server" ID="imbEditPack" ToolTip="<%$Resources:Controls,Edit %>"
                                                SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')"
                                                EnableViewState="false" />
                                            <asp:ImageButton runat="server" ID="imbDeletePack" ToolTip="<%$Resources:Controls,Delete %>"
                                                SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')"
                                                EnableViewState="false" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div id="divServiceList">
                        <table rules="all" id="grdServiceList" grandtype="GrandGrid" pagesize="20" paging="true"
                            width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="ITM_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_MOQ" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_DESC" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_CODE" sortable="true" align="left" width="13%">
                                        <%=Resources.Controls.MaterialCode%>
                                    </th>
                                    <th fieldmap="ITM_NAME" sortable="true" align="left" width="20%">
                                        <%=Resources.Controls.MaterialName%>
                                    </th>
                                    <th fieldmap="ITC_PK" sortable="true" align="left" isvisible="false">
                                    </th>
                                    <th fieldmap="ITC_NAME" sortable="true" align="left" width="17%">
                                        <%=Resources.Controls.CategoryName%>
                                    </th>
                                    <th fieldmap="TmrBleRtr" sortable="true" align="left" isvisible="false">
                                    </th>
                                    <th fieldmap="IPD_TYPE_TEXT" sortable="true" align="left" isvisible="false" width="15%">
                                        <%=Resources.Controls.Type%>
                                    </th>
                                    <th fieldmap="ITM_DESC" sortable="true" align="left" width="35%">
                                        <%=Resources.Controls.Description%>
                                    </th>
                                    <th fieldmap="ITM_TYPE" sortable="true" isvisible="false">
                                        <%=Resources.Controls.MaterialType%>
                                    </th>
                                    <th fieldmap="ITM_DESC" sortable="true" align="center" isvisible="false">
                                    </th>
                                    <th fieldmap="UOM_NAME" sortable="true" align="left" width="7%">
                                        <%=Resources.Controls.MaterialUOM%>
                                    </th>
                                    <th fieldmap="UOM_PK" sortable="true" align="center" isvisible="false">
                                    </th>
                                    <th fieldmap="ITM_ROL_STK" sortable="true" align="right" isvisible="false">
                                        <%=Resources.Controls.ReorderLevel%>
                                    </th>
                                    <th type="Template" width="8%" align="left">
                                        <div style="text-align: left">
                                            <asp:ImageButton runat="server" ID="ImageButton1" ToolTip="<%$Resources:Controls,Edit %>"
                                                SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')"
                                                EnableViewState="false" />
                                            <asp:ImageButton runat="server" ID="ImageButton2" ToolTip="<%$Resources:Controls,Delete %>"
                                                SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')"
                                                EnableViewState="false" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
            <div class="clear">
            </div>
            <div id="divData">
                <div id="divFileData">
                </div>
                <div id="tabs" runat="server" class="jquery-tabs">
                    <ul>
                        <li><a href="#Material" onclick="javascript:ShowSaveImage();">
                            <%=Resources.Controls.Material%></a></li>
                        <li><a id="aVendor" href="#VendorMapping" onclick="javascript:HideSaveImage(this);">
                            <%=Resources.Controls.VendorMapping%></a></li>
                    </ul>
                    <div id="Material" class="jquery-tabs-contents">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label for="ITM_CODE">
                                            <%=Resources.Controls.MaterialCode%>*</label>
                                        <asp:TextBox runat="server" ID="ITM_CODE" TabIndex="1" EnableViewState="false">
                                        </asp:TextBox>
                                        <label for="ITM_NAME">
                                            <%=Resources.Controls.MaterialName%>*</label>
                                        <asp:TextBox runat="server" ID="ITM_NAME" TabIndex="3" EnableViewState="false">
                                        </asp:TextBox>
                                        <label for="ITC_PK">
                                            <%=Resources.Controls.MaterialCategory%>*</label>
                                        <asp:DropDownList ID="ITC_PK" runat="server" TabIndex="5" onchange="javascript:FillUomCategory();"
                                            EnableViewState="False">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="imbViewCag" runat="server" CssClass="imgbtn" SkinID="btnview"
                                            ToolTip="<%$Resources:Controls,OpenMaterialCategory%>" TabIndex="6" OnClientClick="javascript:return ShowCategory();"
                                            EnableViewState="false" />
                                        <%--<asp:ImageButton ID="ImageButton3" runat="server" CssClass="imgbtn" SkinID="btnview"
                                            ToolTip="<%$Resources:Controls,OpenMaterialCategory%>" TabIndex="6" OnClientClick="javascript:return ShowCategory();"
                                            EnableViewState="false" />--%>
                                        <div class="clear">
                                        </div>
                                        <label for="UOM_PK">
                                            <%=Resources.Controls.MaterialUOM%>*</label>
                                        <asp:DropDownList ID="UOM_PK" runat="server" TabIndex="8" EnableViewState="false"
                                            CssClass="half">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                        <div id="divClass">
                                            <label for="ITM_TYPE_TEXT">
                                                <%=Resources.Controls.ItemClass%>*</label><asp:DropDownList ID="ITM_TYPE_TEXT" runat="server"
                                                    TabIndex="9" CssClass="half" EnableViewState="false">
                                                    <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <div id="divType">
                                            <label for="IPD_TYPE">
                                                <%=Resources.Controls.Type%>*</label><asp:DropDownList ID="IPD_TYPE" runat="server"
                                                    CssClass="half" EnableViewState="false" TabIndex="10" onchange="javascript: setPackingVisibility();">
                                                </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S" style="float: right; margin-right: 0">
                                        <div id="divMaterialMeasures">
                                            <label for="ITM_MIN_STK">
                                                <%=Resources.Controls.MinStockLevel%></label>
                                            <asp:TextBox runat="server" ID="ITM_MIN_STK" TabIndex="2" MaxLength="8" CssClass="half"
                                                EnableViewState="false">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="ITM_ROL_STK">
                                                <%=Resources.Controls.ReorderLevel%></label>
                                            <asp:TextBox runat="server" ID="ITM_ROL_STK" TabIndex="4" MaxLength="8" CssClass="half"
                                                EnableViewState="false">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="ITM_MAX_STK">
                                                <%=Resources.Controls.MaxStockLevel%></label>
                                            <asp:TextBox runat="server" ID="ITM_MAX_STK" TabIndex="7" MaxLength="8" CssClass="half"
                                                EnableViewState="false">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="ITM_MAX_STK">
                                                <%=Resources.Controls.MOQ%></label>
                                            <asp:TextBox runat="server" ID="ITM_MOQ" TabIndex="9" MaxLength="9" CssClass="half"
                                                EnableViewState="false">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <label for="ITM_MAX_STK">
                                            <%=Resources.Controls.Description%></label>
                                        <asp:TextBox runat="server" ID="ITM_DESC" TabIndex="11" EnableViewState="false" EnableTheming="false"
                                            TextMode="MultiLine" Rows="2">
                                        </asp:TextBox>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="clear">
                        </div>
                    </div>
                    <div id="VendorMapping">
                        <div class="clear">
                        </div>
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label>
                                            <%=Resources.Controls.MaterialCode%></label>
                                        <asp:Label ID="MaterialCode" runat="server" Text="" CssClass="height-auto break-word"></asp:Label>
                                        <label>
                                            <%=Resources.Controls.MaterialCategoryName%></label>
                                        <asp:Label ID="MaterialCategoryName" runat="server" Text="" CssClass="height-auto break-word"></asp:Label>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label>
                                            <%=Resources.Controls.MaterialName%></label>
                                        <asp:Label ID="MaterialName" runat="server" Text="" CssClass="height-auto break-word"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <%-- <div class="div3col-VMappingListing">
                        </div>--%>
                        <div class="grdTable" style="overflow-x: auto">
                            <div id="ProductInsert">
                                <table id="tblProductInsert" class="gridwraptable gridwrap">
                                    <thead>
                                        <tr>
                                            <th width="22%" align="left">
                                                <%=Resources.Controls.VendorName%>*
                                            </th>
                                            <th width="18%" align="left">
                                                <%=Resources.Controls.msName%>*
                                            </th>
                                            <th width="9%" align="right">
                                                <%=Resources.Controls.StdPrice%>
                                            </th>
                                            <th width="8%" align="left">
                                                <%=Resources.Controls.Currency%>*
                                            </th>
                                            <th width="8%" align="right">
                                                <%=Resources.Controls.MOQ%>*
                                            </th>
                                            <th width="8%" align="left">
                                                <%=Resources.Controls.Unit%>*
                                            </th>
                                            <%--<th width="9%" align="right">
                                                <%=Resources.Controls.Discount%>
                                                *
                                            </th>
                                            <th width="6%" align="right">
                                                <%=Resources.Controls.Tax%>
                                                *
                                            </th>--%>
                                            <%--//New Start--%>
                                            <th width="11%" align="right">
                                                <%=Resources.Controls.LeadDays%>*
                                            </th>
                                            <th width="7%" align="right">
                                                <%=Resources.Controls.Active%>
                                            </th>
                                            <th>
                                            </th>
                                            <%--//New End--%>
                                            <th width="8%" align="left">
                                                <%=Resources.Controls.Action%>
                                            </th>
                                               <th>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr class="grd-rowhead">
                                            <td>
                                                <asp:DropDownList ID="ITV_VENDOR" runat="server" Width="96%" onchange="javascript:FillCurrencyByVendor();">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ITV_NAME" runat="server" Width="93%" Text=""></asp:TextBox>
                                            </td>
                                            <td align="right">
                                                <asp:TextBox runat="server" ID="ITV_PRICE" CssClass="numeric input-w58" Text=""></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ITV_CURRENCY" runat="server" CssClass="input-uom">
                                                    <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                <asp:TextBox runat="server" ID="ITV_MOQ" CssClass="numeric input-w50" Text="0.00"
                                                    onchange="GrandScriptUtils.SetZeroDefault(this, 2)"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="ITV_MOQ_UOM" runat="server" CssClass="input-uom">
                                                </asp:DropDownList>
                                            </td>
                                            <%--<td align="right" width="9%">
                                                <asp:TextBox ID="ITV_DISC_PERC" runat="server" Text="" Width="90%" CssClass="numeric"></asp:TextBox>
                                            </td>
                                            <td align="right" width="6%">
                                                <asp:TextBox ID="ITV_TAX_PERC" runat="server" Text="" Width="90%" CssClass="numeric"></asp:TextBox>
                                            </td>--%>
                                            <%--//New Start--%>
                                            <td align="right">
                                                <asp:TextBox ID="ITV_LEAD_TIME" runat="server" Text="0" CssClass="numeric input-w50"
                                                    MaxLength="5" onkeypress="javascript:MakeNumeric(event);" onchange="GrandScriptUtils.SetZeroDefault(this)"></asp:TextBox>
                                            </td>
                                            <%--//New End--%>
                                            <td>
                                             <asp:DropDownList ID="ddlActive" runat="server" Width="60px" TabIndex="8">
                                                <asp:ListItem Value="1" Text="<%$ Resources:Captions,Yes %>"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="<%$ Resources:Captions,No %>"></asp:ListItem>
                                            </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td align="left">
                                                <asp:ImageButton ID="imbAddNew" runat="server" SkinID="imbaddnew" OnClientClick="javascript:return AddMappingDetails();"
                                                    Width="16px" />
                                            </td>
                                            <td></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="clear">
                            </div>
                            <table rules="all" id="grdVendorMaterialDetails" grandtype="GrandGrid" paging="false"
                                editfunction="GridMaterialAction" editable="true" width="100%" class="gridwraptable gridwrap">
                                <thead>
                                    <tr>
                                        <th fieldmap="ITV_ACTIVE" isvisible="false">
                                        </th>
                                         <th fieldmap="ITV_SL_NO" isvisible="false">
                                        </th>
                                        <th fieldmap="ITV_VENDOR" isvisible="false">
                                        </th>
                                        <th fieldmap="ITV_VENDORNAME" width="22%" align="left">
                                            <%=Resources.Controls.VendorName%>*
                                        </th>
                                        <th fieldmap="ITV_NAME" width="18%" align="left">
                                            <%=Resources.Controls.msName%>*
                                        </th>
                                        <th fieldmap="ITV_PRICE" width="9%" align="right">
                                            <%=Resources.Controls.StdPrice%>
                                        </th>
                                        <th fieldmap="ITV_CURRENCY" isvisible="false" align="left">
                                        </th>
                                        <th fieldmap="MaterialCurrencyText" width="8%" align="left">
                                            <%=Resources.Controls.Currency%>*
                                        </th>
                                        <th fieldmap="ITV_MOQ" width="8%" align="right">
                                            <%=Resources.Controls.MOQ%>*
                                        </th>
                                        <th fieldmap="ITV_MOQ_UOM" isvisible="false">
                                        </th>
                                        <th fieldmap="UOMText" width="8%" align="left">
                                            <%=Resources.Controls.Unit%>*
                                        </th>
                                        <%--<th fieldmap="ITV_DISC_PERC" width="9%" align="right">
                                            <%=Resources.Controls.Discount%>
                                        </th>
                                        <th fieldmap="ITV_TAX_PERC" width="6%" align="right">
                                            <%=Resources.Controls.Tax%>
                                        </th>--%>
                                        <%--//New Start--%>
                                        <th fieldmap="ITV_LEAD_TIME" width="11%" align="right">
                                            <%=Resources.Controls.LeadDays%>*
                                        </th>
                                        <th fieldmap="ITV_ACTIVE_TEXT" width="7%" align="right">
                                            <%=Resources.Controls.Active%>*
                                        </th>
                                        <th fieldmap="ITV_DISC_PERC" width="3%">
                                        </th>
                                        <th fieldmap="ITV_TAX_PERC" width="3%">
                                        </th>
                                        <th type="Template" width="8%" align="left">
                                            <div>
                                                <asp:ImageButton runat="server" ID="imbEditVendor" ToolTip="<%$ resources:Controls,Edit %>"
                                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridVendorHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                                <asp:ImageButton runat="server" ID="imbDeleteVendor" ToolTip="<%$ resources:Controls,Delete %>"
                                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridVendorHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                        <div class="button-wrap-right">
                            <%-- <input type="button" value="<%=Resources.Controls.Save%>" id="AddressSave" class="inputbtn"
                                onclick="javascript:return SaveVendorPage();" />--%>
                            <asp:Button ID="AddressSave" runat="server" SkinID="btnInner-Save" Text="Save" ToolTip="<%$ resources:Controls,Save %>"
                                OnClientClick="javascript:return SaveVendorPage();" />
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="clear">
                </div>
                <%--   Packing Material Master Start--%>
                <div id="divPackingMaterial" style="display: none">
                    <h4 class="fields-groupHead">
                        <%=Resources.Controls.Dimensions%></h4>
                    <table class="table-devide">
                        <tr class="fields-group-2col">
                            <td>
                                <div class="div2col-S">
                                    <label for="IPD_INNER_LENGTH">
                                        <%=Resources.Controls.LengthMM%></label>
                                    <asp:TextBox runat="server" ID="IPD_INNER_LENGTH" TabIndex="12" MaxLength="8" CssClass="half"
                                        EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <div id="divHeight">
                                        <label for="IPD_INNER_HEIGHT">
                                            <%=Resources.Controls.HeightMM%></label><asp:TextBox runat="server" ID="IPD_INNER_HEIGHT"
                                                TabIndex="14" MaxLength="8" CssClass="half" EnableViewState="false"></asp:TextBox>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S" style="float: right; margin-right: 0">
                                    <label for="IPD_INNER_BREADTH">
                                        <%=Resources.Controls.WidhMM%></label>
                                    <asp:TextBox runat="server" ID="IPD_INNER_BREADTH" TabIndex="13" MaxLength="8" CssClass="half"
                                        EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <label for="ITM_WEIGHT">
                                        <%=Resources.Controls.WeightMM%></label>
                                    <asp:TextBox runat="server" ID="ITM_WEIGHT" TabIndex="15" MaxLength="8" CssClass="half"
                                        EnableViewState="false">
                                    </asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" id="trOuterDimensionHdr">
                                <h4 class="fields-groupHead">
                                    <%=Resources.Controls.OuterDimensions%></h4>
                            </td>
                        </tr>
                        <tr id="trOuterDimension" class="fields-group-2col">
                            <td>
                                <div class="div2col-S">
                                    <label for="IPD_OUTER_LENGTH">
                                        <%=Resources.Controls.LengthMM%></label>
                                    <asp:TextBox runat="server" ID="IPD_OUTER_LENGTH" onblur="CalcCBM();" TabIndex="16"
                                        MaxLength="8" CssClass="half" EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="IPD_OUTER_HEIGHT">
                                        <%=Resources.Controls.HeightMM%></label>
                                    <asp:TextBox runat="server" ID="IPD_OUTER_HEIGHT" onblur="CalcCBM();" TabIndex="18"
                                        MaxLength="8" CssClass="half" EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="IPD_PLY">
                                        <%=Resources.Controls.Ply%></label>
                                    <asp:TextBox runat="server" ID="IPD_PLY" TabIndex="19" MaxLength="8" CssClass="half"
                                        EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S" style="float: right; margin-right: 0">
                                    <label for="IPD_OUTER_BREADTH">
                                        <%=Resources.Controls.WidhMM%></label>
                                    <asp:TextBox runat="server" ID="IPD_OUTER_BREADTH" onblur="CalcCBM();" TabIndex="17"
                                        MaxLength="8" CssClass="half" EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="ITM_CBM">
                                        <%=Resources.Controls.CBM%></label>
                                    <asp:TextBox runat="server" ID="ITM_CBM" ReadOnly="true" MaxLength="8" CssClass="half"
                                        EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="IPD_PAPER_COLOR">
                                        <%=Resources.Controls.PaperColor%></label>
                                    <asp:TextBox runat="server" ID="IPD_PAPER_COLOR" TabIndex="20" EnableViewState="false">
                                    </asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr class="fields-group-2col">
                            <td>
                                <div class="div2col-S">
                                    <label for="IPD_CUSTOMER">
                                        <%=Resources.Controls.ApplicableFor%></label>
                                    <asp:DropDownList ID="IPD_CUSTOMER" runat="server" TabIndex="21" EnableViewState="false">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <label for="IPD_ART_WORK">
                                        <%=Resources.Controls.ArtWorkVer%></label>
                                    <asp:TextBox runat="server" ID="IPD_ART_WORK" CssClass="half" TabIndex="23" EnableViewState="false">
                                    </asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S" style="float: right; margin-right: 0">
                                    <label for="IPD_ACTIVE">
                                        <%=Resources.Controls.Status%></label>
                                    <asp:DropDownList ID="IPD_ACTIVE" runat="server" CssClass="half" EnableViewState="false"
                                        TabIndex="22">
                                        <asp:ListItem Text="<%$ resources:Controls, Active %>" Value="1" />
                                        <asp:ListItem Text="<%$ resources:Controls, InActive %>" Value="0" />
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="divcol-FileuplWrap" id="divAttachment">
                        <label for="aupDocument">
                            <%=Resources.Controls.Upload%></label>
                        <div id="FileUploader" class="input-file">
                            <asp:FileUpload ID="fupUploader" runat="server" ClientIDMode="Static" size="25" Height="22px"
                                Style="margin-top: 3px" TabIndex="30" />
                            <asp:HiddenField ID="FILELIST" runat="server" />
                            <asp:HiddenField ID="TEMPFILELIST" runat="server" />
                            <asp:HiddenField ID="curTR" runat="server" />
                            <asp:HiddenField ID="hdfCurDate" runat="server" Value="" />
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <%--  Paking Material End--%>
            </div>
        </div>
        <asp:HiddenField ID="SBU" runat="server" Value="0" />
        <asp:HiddenField ID="ITM_SET" runat="server" Value="1" />
        <asp:HiddenField runat="server" ID="UserID" Value="1" />
        <asp:HiddenField runat="server" ID="MaterialDetailId" Value="0" EnableViewState="false" />
        <asp:HiddenField runat="server" ID="ITM_PK" Value="0" EnableViewState="false" />
        <div id="divCategory" title="<%=Resources.Captions.SelectCategory%>">
            <div id="treewrap" class="treeviewrap-fxd">
                <div id="trvCategory" class="treeview-adj">
                </div>
            </div>
        </div>
        <div id="divMappingData">
        </div>
        <div id="divTaxData">
        </div>
        <asp:HiddenField runat="server" ID="EditMapping" Value="0" />
        <asp:HiddenField runat="server" ID="MappingDetailsList" />
        <asp:HiddenField ID="STATUS" runat="server" Value="1" />
        <div id="Wofkflowdiv">
            <%-- <asp:HiddenField ID="ActionID" runat="server" />--%>
            <asp:HiddenField ID="MaterialTaskID" runat="server" Value="0" />
            <asp:HiddenField ID="MaterialReferenceID" runat="server" Value="0" />
            <asp:HiddenField ID="MaterialProcessID" runat="server" Value="0" />
            <asp:HiddenField ID="MaterialApplicationID" runat="server" Value="0" />
            <asp:HiddenField ID="MaterialActionID" runat="server" Value="0" />
            <asp:HiddenField ID="MaterialDetailsObj" runat="server" />
            <asp:HiddenField ID="IPD_PK" runat="server" Value="0" />
            <asp:HiddenField ID="DOC_PK" runat="server" Value="0" />
        </div>
        <div id="divItemTax" title="<%=Resources.Messages.TaxDetails%>" style="display: none">
            <div class="Button-container-popup">
                <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClientClick="javascript:return SaveApply();" />
            </div>
            <div class="content-wrapper">
                <div class="divcolmiddle-S">
                    <label for="ChooseTax">
                        <%=Resources.Controls.ChooseType%>
                    </label>
                    <asp:DropDownList ID="ChooseTax" runat="server" EnableViewState="false">
                    </asp:DropDownList>
                    <asp:ImageButton ID="imbTaxDiscountSave" SkinID="imbaddnew" runat="server" TabIndex="15"
                        EnableViewState="False" OnClientClick="javascript:return SaveTaxDiscount();" />
                </div>
                <asp:HiddenField runat="server" ID="hdfTaxCategory" Value="1" />
                <asp:HiddenField runat="server" ID="hdnSlNo" Value="0" />
                <div>
                    <table rules="all" id="grdTaxDetails" grandtype="GrandGrid" paging="false" width="100%"
                        editfunction="GridHandler" editable="true" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="IVT_SL_NO" isvisible="false">
                                </th>
                                <th fieldmap="IVT_PK" isvisible="false">
                                </th>
                                <th fieldmap="IVT_TAX" isvisible="false">
                                </th>
                                <th fieldmap="IVT_TAX_CATEGORY" isvisible="false">
                                </th>
                                <th fieldmap="IVT_TAX_TEXT" width="96%">
                                    <%=Resources.Controls.Type%>
                                </th>
                                <th type="Template" width="4%">
                                    <div>
                                        <asp:ImageButton runat="server" ID="imbTaxDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'TAXDELETE')" />
                                    </div>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
