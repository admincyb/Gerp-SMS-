<%@ Page Title="<%$ Resources:Captions,Title_MaterialMaster %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" EnableEventValidation="false" Theme="ClassicExt" CodeBehind="MaterialMaster.aspx.cs"
    Inherits="ERPSMS_v01.GeneralAdmin.MaterialMaster" %>

<%@ Register Src="../UserControls/AdvanceSearch.ascx" TagName="AdvanceSearch" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/JSLINQ/JSLINQ.js" type="text/javascript"></script>
    <script src="../Scripts/PageScript/MaterialManagement/MaterialMaster.js.axd" type="text/javascript"
        charset="UTF-8"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    function getQueryStringValue(key) {
        return decodeURIComponent(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + encodeURIComponent(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
    }
    $(document).ready(function () {
        var url = window.location.href;
        var qsValue = /Type=([^&]+)/.exec(url)[1];
        var type = qsValue ? qsValue : '0';
        //var type = getQueryStringValue("Type");
        if (type != 3)
        {
            $("[id$=lblConversionRequired]").hide();
            $("[id$=chbConversionRequired]").hide();
        }
    });
</script>
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
                                <asp:Button ID="AddressSave" runat="server" SkinID="btnInner-Save" Text="Save" ToolTip="<%$ resources:Controls,Save %>"
                                    OnClientClick="javascript:return SaveVendorPage();" />
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
                <div style="display: none">
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
                                <asp:ListItem Value="ITC_NAME" Text="<%$ Resources:BindValues, ItemCategory%>">
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
                </div>
                <%--=========Advance Search Region Begin=========================--%>
                <div class="search-colapse">
                    <table>
                        <tr>
                            <td>
                                <h1>
                                    <%= GetGlobalResourceObject("Captions", "AdvanceSearch").ToString()%></h1>
                            </td>
                            <td>
                                <asp:ImageButton runat="server" ID="imbShowFilter" OnClientClick="javascript:return ShowHideAdvancedSearch(1);"
                                    ImageUrl="../images/Classic/Icons/arrow-colapse-inactive.png" ToolTip="Show Filter"
                                    TabIndex="65" />
                                <asp:ImageButton runat="server" ID="imbHideFilter" OnClientClick="javascript:return ShowHideAdvancedSearch();"
                                    ImageUrl="../images/Classic/Icons/arrow-colapse-active.png" ToolTip="Hide Filter"
                                    TabIndex="66" />
                            </td>
                        </tr>
                    </table>
                </div>
                <%--------------colpase btn----------%>
                <div class="clear">
                </div>
                <table class="table-devide" id="tbladvancedSearch" style="/*margin-top: 8px; */ background: #f2f2f2;">
                    <tr>
                        <td>
                            <div class="div2col-S padgtop7">
                                <asp:Label runat="server" ID="lblCategory" Text="<%$ Resources:BindValues, ItemCategory%>"
                                    AssociatedControlID="MaterialCategory"></asp:Label>
                                <asp:TextBox ID="MaterialCategory" CssClass="input-half" runat="server" TabIndex="8">
                                </asp:TextBox>
                                <asp:HiddenField ID="MaterialCategoryPK" runat="server" Value="0"></asp:HiddenField>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S padgtop7">
                                <asp:Label runat="server" ID="lblItemCodeName" Text="<%$ Resources:BindValues, Item%>"
                                    CssClass="lbl-9perc" AssociatedControlID="ItemCodeMaterial"></asp:Label>
                                <asp:TextBox runat="server" ID="ItemCodeMaterial" CssClass="input-small-e" TabIndex="8">
                                </asp:TextBox>
                                <asp:HiddenField ID="MaterialPK" runat="server" Value="0"></asp:HiddenField>
                                <asp:Label ID="lblStatus" runat="server" Text="<%$Resources:Controls,Status%>" CssClass="lbl-10-7perc"
                                    AssociatedControlID="ddlStatus"></asp:Label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="select-small" TabIndex="8">
                                    <asp:ListItem Text="<%$ Resources:Captions,All %>" Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Captions,Active %>" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="<%$ Resources:Captions,Inactive %>" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:ImageButton ID="btnSearch" runat="server" ToolTip="<%$ resources:Controls,Search %>"
                                    TabIndex="8" SkinID="search-ext" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                    OnClientClick="javascript:return BindGrid();" />
                                <asp:ImageButton ID="btnClear" runat="server" TabIndex="8" Style="margin-bottom: 0px!important; margin-top: 2px;"
                                    ToolTip="<%$ resources:Controls,Clear %>" SkinID="clear-ext"
                                    OnClientClick="javascript:return ClearSearch();" />
                            </div>
                        </td>
                    </tr>
                </table>
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S div-separatn">
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S div-separatn">
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <%--=============End Advance Search Region=====================--%>
                <div class="gridwrap">
                    <div id="divMaterialList">
                        <table rules="all" id="grdCategory" grandtype="GrandGrid" pagesize="20" paging="true"
                            width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="ITC_IS_STOCK" isvisible="false"></th>
                                    <th fieldmap="ITM_PK" isvisible="false"></th>
                                    <th fieldmap="ITM_MOQ" isvisible="false"></th>
                                    <th fieldmap="ITM_MAX_OQ" isvisible="false"></th>
                                    <th fieldmap="ITM_DESC" isvisible="false"></th>
                                    <th fieldmap="ITM_NEED_QC_INSP" isvisible="false"></th>
                                    <th fieldmap="ITM_NEED_BATCH_STK" isvisible="false"></th>
                                    <th fieldmap="ITM_IS_LINKED_ITEM" isvisible="false"></th>
                                    <th fieldmap="ITM_ACTIVE" isvisible="false"></th>
                                     <th fieldmap="ITM_IS_ASSET" isvisible="false"></th>
                                    <th fieldmap="ITM_CODE" sortable="true" align="left" width="14%">
                                        <%=Resources.Controls.MaterialCode%>
                                    </th>
                                    <th fieldmap="ITM_NAME" sortable="true" align="left" width="36%">
                                        <%=Resources.Controls.MaterialName%>
                                    </th>
                                    <th fieldmap="ITC_PK" sortable="true" align="left" isvisible="false"></th>
                                    <th fieldmap="ITC_NAME" sortable="true" align="left" width="15%">
                                        <%=Resources.Controls.CategoryName%>
                                    </th>
                                    <th fieldmap="TmrBleRtr" sortable="true" align="left" isvisible="false"></th>
                                    <%-- <th fieldmap="ITM_TYPE_TEXT" sortable="true" align="left" width="13%">
                                        <%=Resources.Controls.ItemClass%>
                                    </th>--%>
                                    <th fieldmap="ITM_TYPE" sortable="true" isvisible="false">
                                        <%=Resources.Controls.MaterialType%>
                                    </th>
                                    <th fieldmap="ITM_DESC" sortable="true" align="center" isvisible="false"></th>
                                    <th fieldmap="UOM_NAME" sortable="true" align="left" width="7%">
                                        <%=Resources.Controls.MaterialUOM%>
                                    </th>
                                    <th fieldmap="ITM_IS_WORK_ORDER" sortable="true" align="right" isvisible="false">
                                        <th fieldmap="UOM_PK" sortable="true" align="center" isvisible="false"></th>
                                        <th fieldmap="ITM_UOM_PURCHASE" sortable="true" align="center" isvisible="false"></th>
                                        <th fieldmap="ITM_UOM_SALE" sortable="true" align="center" isvisible="false"></th>
                                        <th fieldmap="ITM_MIN_STK" sortable="true" align="right" width="8%">
                                            <%=Resources.Controls.MinStockLevel%>
                                        </th>
                                        <th fieldmap="ITM_MAX_STK" sortable="true" align="right" width="8%">
                                            <%=Resources.Controls.MaxStockLevel%>
                                        </th>
                                        <th fieldmap="ITM_ROL_STK" sortable="true" align="right" isvisible="false">
                                            <%=Resources.Controls.ReorderLevel%>
                                        </th>
                                        <th fieldmap="ITM_INACTIVE_PERIOD" sortable="true" align="right" isvisible="false">
                                            <%=Resources.Controls.InactivePeriod%>
                                        </th>
                                        <th fieldmap="ITM_PHR" sortable="true" align="right" isvisible="false"></th>
                                        <th fieldmap="ITM_TSC" sortable="true" align="right" isvisible="false"></th>
                                        <th fieldmap="ITM_BATCH_CODE" sortable="true" align="right" isvisible="false"></th>
                                        <%--  including active/in active status--%>
                                        <th fieldmap="ITM_ACTIVE_TEXT" width="5%" align="center">
                                            <%=Resources.Controls.Active%>
                                        </th>
                                        <th fieldmap="ITM_GST_CLASS" sortable="true" align="right" isvisible="false">
                                            <th type="Template" width="7%" align="left">
                                                <div style="text-align: left">
                                                    <asp:ImageButton runat="server" ID="imbEditMast" ToolTip="<%$Resources:Controls,Edit %>"
                                                        CssClass="imgbutton-wrap margntop2" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')"
                                                        EnableViewState="false" />
                                                    <asp:ImageButton runat="server" ID="imbDeleteMast" ToolTip="<%$Resources:Controls,Delete %>"
                                                        CssClass="imgbutton-wrap margntop2" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')"
                                                        EnableViewState="false" />
                                                    <%-- For Showing Vendor Rate--%>
                                                    <asp:ImageButton ID="imbMaterialShow" runat="server" SkinID="tax" CssClass="imgbutton-wrap margntop2"
                                                        ToolTip="<%$resources:ErpRes,VendorRate %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'SHOWRATE')"
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
                                    <th fieldmap="ITC_IS_STOCK" isvisible="false"></th>
                                    <th fieldmap="ITM_PK" isvisible="false"></th>
                                    <th fieldmap="ITM_NEED_QC_INSP" isvisible="false"></th>
                                    <th fieldmap="ITM_NEED_BATCH_STK" isvisible="false"></th>
                                    <th fieldmap="ITM_IS_CONVERSION_REQD" isvisible="false"></th>
                                    <th fieldmap="ITM_ACTIVE" isvisible="false"></th>
                                    <th fieldmap="ITM_IS_LINKED_ITEM" isvisible="false"></th>
                                    <th fieldmap="ITM_MOQ" isvisible="false"></th>
                                    <th fieldmap="ITM_MAX_OQ" isvisible="false"></th>
                                    <th fieldmap="ITM_DESC" isvisible="false"></th>
                                    <th fieldmap="IPD_TYPE" isvisible="false"></th>
                                    <th fieldmap="ITM_CODE" sortable="true" align="left" width="13%">
                                        <%=Resources.Controls.MaterialCode%>
                                    </th>
                                    <th fieldmap="ITM_NAME" sortable="true" align="left" width="20%">
                                        <%=Resources.Controls.MaterialName%>
                                    </th>
                                    <th fieldmap="ITC_PK" sortable="true" align="left" isvisible="false"></th>
                                    <th fieldmap="ITC_NAME" sortable="true" align="left" width="17%">
                                        <%=Resources.Controls.CategoryName%>
                                    </th>
                                    <th fieldmap="TmrBleRtr" sortable="true" align="left" isvisible="false"></th>
                                    <th fieldmap="IPD_TYPE_TEXT" sortable="true" align="left" width="10%">
                                        <%=Resources.Controls.Type%>
                                    </th>
                                    <th fieldmap="ITM_TYPE" sortable="true" isvisible="false">
                                        <%=Resources.Controls.MaterialType%>
                                    </th>
                                    <th fieldmap="ITM_DESC" sortable="true" align="center" isvisible="false"></th>
                                    <th fieldmap="UOM_NAME" sortable="true" align="left" width="7%">
                                        <%=Resources.Controls.MaterialUOM%>
                                    </th>
                                    <th fieldmap="UOM_PK" sortable="true" align="center" isvisible="false"></th>
                                    <th fieldmap="ITM_UOM_PURCHASE" sortable="true" align="center" isvisible="false"></th>
                                    <th fieldmap="ITM_UOM_SALE" sortable="true" align="center" isvisible="false"></th>
                                    <th fieldmap="ITM_MIN_STK" sortable="true" align="right" width="9%">
                                        <%=Resources.Controls.MinStockLevel%>
                                    </th>
                                    <th fieldmap="ITM_MAX_STK" sortable="true" align="right" width="10%">
                                        <%=Resources.Controls.MaxStockLevel%>
                                    </th>
                                    <th fieldmap="ITM_INACTIVE_PERIOD" sortable="true" align="right" isvisible="false">
                                        <%=Resources.Controls.InactivePeriod%>
                                    </th>
                                    <th fieldmap="ITM_ROL_STK" sortable="true" align="right" isvisible="false">
                                        <%=Resources.Controls.ReorderLevel%>
                                    </th>
                                    <th fieldmap="ITM_IS_WORK_ORDER" sortable="true" align="right" isvisible="false">
                                        <th fieldmap="ITM_TSC" sortable="true" align="right" isvisible="false"></th>
                                        <th fieldmap="ITM_BATCH_CODE" sortable="true" align="right" isvisible="false"></th>
                                        <th fieldmap="ITM_ACTIVE_TEXT" sortable="true" align="center" width="5%">
                                            <%=Resources.Controls.Active%>
                                        </th>
                                        <th fieldmap="ITM_GST_CLASS" sortable="true" align="right" isvisible="false">
                                            <th type="Template" width="6%" align="left">
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
                                    <th fieldmap="ITC_IS_STOCK" isvisible="false"></th>
                                    <th fieldmap="ITM_PK" isvisible="false"></th>
                                    <th fieldmap="ITM_NEED_QC_INSP" isvisible="false"></th>
                                    <th fieldmap="ITM_NEED_BATCH_STK" isvisible="false"></th>
                                    <th fieldmap="ITM_ACTIVE" isvisible="false"></th>
                                    <th fieldmap="ITM_IS_LINKED_ITEM" isvisible="false"></th>
                                    <th fieldmap="ITM_MOQ" isvisible="false"></th>
                                    <th fieldmap="ITM_MAX_OQ" isvisible="false"></th>
                                    <th fieldmap="ITM_DESC" isvisible="false"></th>
                                    <th fieldmap="ITM_IS_ASSET" isvisible="false"></th>
                                    <th fieldmap="ITM_CODE" sortable="true" align="left" width="12%">
                                        <%=Resources.Controls.MaterialCode%>
                                    </th>
                                    <th fieldmap="ITM_NAME" sortable="true" align="left" width="24%">
                                        <%=Resources.Controls.MaterialName%>
                                    </th>
                                    <th fieldmap="ITC_PK" sortable="true" align="left" isvisible="false"></th>
                                    <th fieldmap="ITC_NAME" sortable="true" align="left" width="20.5%">
                                        <%=Resources.Controls.CategoryName%>
                                    </th>
                                    <th fieldmap="TmrBleRtr" sortable="true" align="left" isvisible="false"></th>
                                    <th fieldmap="IPD_TYPE_TEXT" sortable="true" align="left" isvisible="false">
                                        <%=Resources.Controls.Type%>
                                    </th>
                                    <th fieldmap="ITM_DESC" sortable="true" align="left" width="24%">
                                        <%=Resources.Controls.Description%>
                                    </th>
                                    <th fieldmap="ITM_TYPE" sortable="true" isvisible="false">
                                        <%=Resources.Controls.MaterialType%>
                                    </th>
                                    <th fieldmap="ITM_DESC" sortable="true" align="center" isvisible="false"></th>
                                    <th fieldmap="UOM_NAME" sortable="true" align="left" width="7%">
                                        <%=Resources.Controls.MaterialUOM%>
                                    </th>
                                    <th fieldmap="ITM_IS_WORK_ORDER" sortable="true" align="right" isvisible="false">
                                        <th fieldmap="UOM_PK" sortable="true" align="center" isvisible="false"></th>
                                        <th fieldmap="ITM_UOM_PURCHASE" sortable="true" align="center" isvisible="false"></th>
                                        <th fieldmap="ITM_UOM_SALE" sortable="true" align="center" isvisible="false"></th>
                                        <th fieldmap="ITM_ROL_STK" sortable="true" align="right" isvisible="false">
                                            <%=Resources.Controls.ReorderLevel%>
                                        </th>
                                        <th fieldmap="ITM_ACTIVE_TEXT" sortable="true" align="center" width="6%">
                                            <%=Resources.Controls.Active%>
                                        </th>
                                        <th fieldmap="ITM_GST_CLASS" sortable="true" align="right" isvisible="false">
                                            <th type="Template" width="8.5%" align="left">
                                                <div style="text-align: left">
                                                    <asp:ImageButton runat="server" ID="ImageButton1" ToolTip="<%$Resources:Controls,Edit %>"
                                                        SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')"
                                                        EnableViewState="false" />
                                                    <asp:ImageButton runat="server" ID="ImageButton2" ToolTip="<%$Resources:Controls,Delete %>"
                                                        SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')"
                                                        EnableViewState="false" />
                                                    <%-- For Showing Vendor Rate--%>
                                                    <asp:ImageButton ID="imgbtnShowvendorRate" runat="server" SkinID="tax" CssClass="imgbutton-wrap margntop2"
                                                        ToolTip="<%$resources:ErpRes,VendorRate %>" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'SHOWRATE')"
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
                    <ul class="bg-none">
                        <li class="padgbotm0"><a href="#Material" onclick="javascript:ShowSaveImage();" class="padgtop3 padgbotm3">
                            <%=Resources.Controls.Material%></a></li>
                        <li class="padgbotm0"><a id="aVendor" href="#VendorMapping" onclick="javascript:HideSaveImage(this);"
                            class="padgtop3 padgbotm3">
                            <%=Resources.Controls.VendorMapping%></a></li>
                        <li class="padgbotm0"><a id="aStores" href="#MaterialStores" onclick="javascript:HideStore(this);"
                            class="padgtop3 padgbotm3">
                            <%=Resources.Controls.StoreMapping%></a></li>
                        <li class="padgbotm0"><a id="aRMaterial" href="#RelatedMaterial" onclick="javascript:ShowRelatedMaterial(this);"
                            class="padgtop3 padgbotm3">
                            <%=Resources.Controls.RelatedMaterial%>
                        </a></li>
                        <li class="padgbotm0"><a id="aBOMaterial" href="#BillOfMaterial" onclick="javascript:ShowBOMaterial(this);"
                            class="padgtop3 padgbotm3">
                            <%=Resources.Controls.BOM%>
                        </a></li>
                    </ul>
                    <div id="Material" class="jquery-tabs-contents">
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label for="ITM_CODE">
                                            <%=Resources.Controls.MaterialCode%>*</label>
                                        <asp:TextBox runat="server" ID="ITM_CODE" TabIndex="1" EnableViewState="false" CssClass="input-half">
                                        </asp:TextBox>
                                        <div class="clear">
                                        </div>
                                        <label for="ITM_NAME">
                                            <%=Resources.Controls.MaterialName%>*</label>
                                        <asp:TextBox runat="server" ID="ITM_NAME" TabIndex="3" EnableViewState="false" CssClass="input-half">
                                        </asp:TextBox>
                                        <div class="clear">
                                        </div>
                                        <label for="ITC_PK">
                                            <%=Resources.Controls.MaterialCategory%>*</label>
                                        <asp:DropDownList ID="ITC_PK" runat="server" TabIndex="5" EnableViewState="False"
                                            CssClass="select-w61per" onchange="javascript:SetCategoryType();">
                                        </asp:DropDownList>
                                        <asp:ImageButton ID="imbViewCag" runat="server" CssClass="imgbtn" SkinID="btnview"
                                            ToolTip="<%$Resources:Controls,OpenMaterialCategory%>" TabIndex="6" OnClientClick="javascript:return ShowCategory();"
                                            EnableViewState="false" />
                                        <%--<asp:ImageButton ID="ImageButton3" runat="server" CssClass="imgbtn" SkinID="btnview"
                                            ToolTip="<%$Resources:Controls,OpenMaterialCategory%>" TabIndex="6" OnClientClick="javascript:return ShowCategory();"
                                            EnableViewState="false" />--%>
                                        <div class="clear">
                                        </div>
                                        <label for="ITC_VALUE">
                                            <%=Resources.Controls.CategoryType%></label>
                                        <asp:Label ID="ITC_VALUE" runat="server" EnableViewState="False" CssClass="input-half"></asp:Label>
                                        <div class="clear">
                                        </div>
                                        <label for="UOM_PK" id="lblUOMPK">
                                            <%=Resources.Controls.StockUOM%>*</label>
                                        <asp:DropDownList ID="UOM_PK" runat="server" TabIndex="8" EnableViewState="false"
                                            CssClass="select-small-a" onchange="StockUOMChanged();">
                                        </asp:DropDownList>
                                        <label for=" ITM_GST_CLASS" id="lblGCMPK" class="middle-lbl-small-d">
                                            <%=Resources.Controls.HSNCode%></label>
                                        <asp:DropDownList ID="ITM_GST_CLASS" runat="server" TabIndex="8" EnableViewState="false"
                                            CssClass="select-small-a">
                                        </asp:DropDownList>
                                        <div class="clear">
                                        </div>
                                        <%-- ****Purchase Uom & Sale UOM ************************************ --%>
                                        <div id="divPurchaseSalesUOM" class="margnlft-minus6">
                                            <label for="ITM_UOM_PURCHASE" id="lblPurchaseUom">
                                                <%=Resources.Controls.PurchaseUOM%>*</label>
                                            <asp:DropDownList ID="ITM_UOM_PURCHASE" runat="server" TabIndex="8" EnableViewState="false"
                                                CssClass="select-small-a">
                                            </asp:DropDownList>
                                            <label for="ITM_UOM_SALE" class="middle-lbl-small-b" id="lblSaleUom">
                                                <%=Resources.Controls.SaleUOM%>*</label>
                                            <asp:DropDownList ID="ITM_UOM_SALE" runat="server" TabIndex="8" EnableViewState="false"
                                                CssClass="select-small-a">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <%--*********End Purchase UOM & Sale UOM***********--%>
                                        <div id="divClass">
                                            <label for="ITM_TYPE_TEXT">
                                                <%=Resources.Controls.ItemClass%>*</label><asp:DropDownList ID="ITM_TYPE_TEXT" runat="server"
                                                    TabIndex="10" EnableViewState="false">
                                                    <%--CssClass="half"--%>
                                                    <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <%-- //Hide PH & TSC for both packing material and service--%>
                                        <div id="divPHTSC">
                                            <label for="ITM_PHR">
                                                <%=Resources.Controls.PHlevel%></label><asp:TextBox runat="server" ID="ITM_PHR" TabIndex="12"
                                                    CssClass="input-small" MaxLength="9" EnableViewState="false">
                                                </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <%-- //END Hide PH & TSC for both packing material and service--%>
                                        <div id="divType">
                                            <label for="IPD_TYPE" class="margn-rgt2">
                                                <%=Resources.Controls.Type%>*</label>
                                            <asp:DropDownList ID="IPD_TYPE" runat="server" EnableViewState="false" TabIndex="12"
                                                CssClass="select-half-a" onchange="javascript: setPackingVisibility();">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                            <label for="IPD_CLASSIFICATION" class="margn-rgt2">
                                                <%=Resources.Controls.Classification%>*</label>
                                            <asp:DropDownList ID="IPD_CLASSIFICATION" runat="server" EnableViewState="false"
                                                CssClass="select-half-a" TabIndex="12">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <%--Inactive Period--%>
                                        <div id="divInactivePeriod">
                                            <label for="InactivePeriod" class="margn-rgt2">
                                                <%=Resources.Controls.InactivePeriod%></label>
                                            <asp:TextBox ID="InactivePeriod" CssClass="input-small numeric" TabIndex="14" runat="server"
                                                MaxLength="5" EnableViewState="False">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                        <%--Inactive Period End--%>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <div id="divMaterialMeasures">
                                            <label for="ITM_MIN_STK">
                                                <%=Resources.Controls.MinStockLevel%></label>
                                            <asp:TextBox runat="server" ID="ITM_MIN_STK" TabIndex="2" MaxLength="8" EnableViewState="false"
                                                CssClass="input-small">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="ITM_ROL_STK">
                                                <%=Resources.Controls.ReorderLevel%></label>
                                            <asp:TextBox runat="server" ID="ITM_ROL_STK" TabIndex="4" MaxLength="8" EnableViewState="false"
                                                CssClass="input-small">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="ITM_MAX_STK">
                                                <%=Resources.Controls.MaxStockLevel%></label>
                                            <asp:TextBox runat="server" ID="ITM_MAX_STK" TabIndex="7" MaxLength="8" EnableViewState="false"
                                                CssClass="input-small">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="ITM_MOQ">
                                                <%=Resources.Controls.MOQ%></label>
                                            <asp:TextBox runat="server" ID="ITM_MOQ" TabIndex="7" MaxLength="9" EnableViewState="false"
                                                CssClass="input-small">
                                            </asp:TextBox><div class="clear">
                                            </div>
                                            <label for="ITM_MAX_OQ" id="lblITM_MAX_OQ" runat="server">
                                                <%=Resources.Controls.MaxOQ%></label>
                                            <asp:TextBox runat="server" ID="ITM_MAX_OQ" TabIndex="8" MaxLength="9" EnableViewState="false"
                                                CssClass="input-small">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="ITM_BATCH_CODE">
                                                <%=Resources.Controls.BatchCode%></label>
                                            <asp:TextBox runat="server" ID="ITM_BATCH_CODE" TabIndex="11" MaxLength="20" EnableViewState="false"
                                                CssClass="input-small">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <label for="ITM_TSC">
                                                <%=Resources.Controls.TSClevel%></label>
                                            <asp:TextBox runat="server" ID="ITM_TSC" TabIndex="12" MaxLength="9" EnableViewState="false"
                                                CssClass="input-small">
                                            </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <div id="divIsWorkOrder">
                                                <label for="chkIsWorkOrder">
                                                    <%=Resources.Controls.IsWorkOrder%></label>
                                                <asp:CheckBox ID="chkIsWorkOrder" runat="server" TabIndex="14" EnableViewState="False"></asp:CheckBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label for="RequireInspection" class="margnrgt5">
                                            <%=Resources.Controls.RequireInspection%></label>
                                        <asp:CheckBox ID="RequireInspection" runat="server" TabIndex="14" EnableViewState="False"
                                            CssClass="disable" Checked="false" Enabled="<%$resources:ConfigurationsRes,RequireInspection %>"></asp:CheckBox>
                                        <%--   <div style="display:none">--%>
                                        <label for="RequireBatch" class="lbl-54-4perc">
                                            <%=Resources.Controls.RequireBatch%></label>
                                        <asp:CheckBox ID="RequireBatch" runat="server" TabIndex="14" EnableViewState="False"></asp:CheckBox>
                                        <%-- </div>--%>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label for="RequireProductMapping" class="lbl-20-5perc" id="lblReqProductMapping"
                                            runat="server">
                                            <%=Resources.Controls.RequireProductMap%></label>
                                        <asp:CheckBox ID="RequireProductMapping" runat="server" TabIndex="14" EnableViewState="False"></asp:CheckBox>
                                        <%--Active--%>
                                        <label for="chkItemActive" class="lbl-21-5perc">
                                            <%=Resources.Controls.Active%></label>
                                        <asp:CheckBox ID="chkItemActive" runat="server" TabIndex="14" EnableViewState="False"
                                            Checked="true"></asp:CheckBox>
                                        <%--Active End--%>

                                           <asp:Label ID="lblIsAsset" runat="server" CssClass="lbl-8-5mperc style-none"
                                                Text="<%$ resources:Controls,IsAsset %>" />
                                        <asp:CheckBox ID="chkIsAsset" runat="server" TabIndex="14" EnableViewState="False"/>
                                        

                                        <asp:Label ID="lblConversionRequired" runat="server" CssClass="lbl-20-5mperc style-none"
                                                Text="<%$ resources:Controls,ConversionRequired %>" />
                                        <asp:CheckBox ID="chbConversionRequired" runat="server" TabIndex="14" EnableViewState="False"/>
                                        
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div class="divcol-S">
                                        <label for="ITM_DESC">
                                            <%=Resources.Controls.Description%></label><asp:TextBox runat="server" ID="ITM_DESC"
                                                TabIndex="15" EnableViewState="false" EnableTheming="false" TextMode="MultiLine"
                                                Rows="2">
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
                                        <asp:Label ID="MaterialCode" runat="server" Text="" CssClass="input-half height-auto break-word"></asp:Label>
                                        <div class="clear">
                                        </div>
                                        <label>
                                            <%=Resources.Controls.MaterialCategoryName%></label>
                                        <asp:Label ID="MaterialCategoryName" runat="server" Text="" CssClass="input-half height-auto break-word"></asp:Label>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label>
                                            <%=Resources.Controls.MaterialName%></label>
                                        <asp:Label ID="MaterialName" runat="server" Text="" CssClass="input-half height-auto break-word"></asp:Label>
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
                                            <th width="23%" align="left">
                                                <%=Resources.Controls.VendorName%>*
                                            </th>
                                            <th width="38%" align="left">
                                                <%=Resources.Controls.msName%>*
                                            </th>
                                            <th width="5%" align="right">
                                                <%=Resources.Controls.StdPrice%>
                                            </th>
                                            <th width="6%" align="left">
                                                <%=Resources.Controls.Currency%>*
                                            </th>
                                            <th width="6%" align="right">
                                                <%=Resources.Controls.MOQ%>*
                                            </th>
                                            <th width="5.5%" align="left">
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
                                            <th width="7.5%" align="right">
                                                <%=Resources.Controls.LeadDays%>
                                            </th>
                                            <th width="4%" align="right">
                                                <%=Resources.Controls.Active%>
                                            </th>
                                            <th></th>
                                            <%--//New End--%>
                                            <th width="5%" align="left">
                                                <%=Resources.Controls.Action%>
                                            </th>
                                            <th></th>
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
                                                <asp:DropDownList ID="ITV_CURRENCY" runat="server" CssClass="input-w80">
                                                    <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td align="right">
                                                <asp:TextBox runat="server" ID="ITV_MOQ" CssClass="numeric input-w80" Text="0.00"
                                                    onchange="GrandScriptUtils.SetZeroDefault(this, 2)"></asp:TextBox>
                                            </td>
                                            <td style="text-align: left">
                                                <asp:DropDownList ID="ITV_MOQ_UOM" runat="server" CssClass="input-w80">
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
                                                <asp:TextBox ID="ITV_LEAD_TIME" runat="server" Text="0" CssClass="numeric input-w58"
                                                    MaxLength="5" onkeypress="javascript:MakeNumeric(event);" onchange="GrandScriptUtils.SetZeroDefault(this)"></asp:TextBox>
                                            </td>
                                            <%--//New End--%>
                                            <td>
                                                <asp:DropDownList ID="ddlActive" runat="server" Width="60px" TabIndex="8">
                                                    <asp:ListItem Value="1" Text="<%$ Resources:Captions,Yes %>"></asp:ListItem>
                                                    <asp:ListItem Value="0" Text="<%$ Resources:Captions,No %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td></td>
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
                                        <th fieldmap="ITV_ACTIVE" isvisible="false"></th>
                                        <th fieldmap="ITV_SL_NO" isvisible="false"></th>
                                        <th fieldmap="ITV_VENDOR" isvisible="false"></th>
                                        <th fieldmap="ITV_VENDORNAME" width="23%" align="left">
                                            <%=Resources.Controls.VendorName%>*
                                        </th>
                                        <th fieldmap="ITV_NAME" width="34%" align="left">
                                            <%=Resources.Controls.msName%>*
                                        </th>
                                        <th fieldmap="ITV_PRICE" width="5%" align="right">
                                            <%=Resources.Controls.StdPrice%>
                                        </th>
                                        <th fieldmap="ITV_CURRENCY" isvisible="false" align="left"></th>
                                        <th fieldmap="MaterialCurrencyText" width="6%" align="left">
                                            <%=Resources.Controls.Currency%>*
                                        </th>
                                        <th fieldmap="ITV_MOQ" width="5%" align="right">
                                            <%=Resources.Controls.MOQ%>
                                        </th>
                                        <th fieldmap="ITV_MOQ_UOM" isvisible="false"></th>
                                        <th fieldmap="UOMText" width="5%" align="left">
                                            <%=Resources.Controls.Unit%>*
                                        </th>
                                        <%--<th fieldmap="ITV_DISC_PERC" width="9%" align="right">
                                            <%=Resources.Controls.Discount%>
                                        </th>
                                        <th fieldmap="ITV_TAX_PERC" width="6%" align="right">
                                            <%=Resources.Controls.Tax%>
                                        </th>--%>
                                        <%--//New Start--%>
                                        <th fieldmap="ITV_LEAD_TIME" width="7%" align="right">
                                            <%=Resources.Controls.LeadDays%>
                                        </th>
                                        <th fieldmap="ITV_ACTIVE_TEXT" width="4%" align="right">
                                            <%=Resources.Controls.Active%>*
                                        </th>
                                        <th fieldmap="ITV_DISC_PERC" width="2%"></th>
                                        <th fieldmap="ITV_TAX_PERC" width="2%"></th>
                                        <th type="Template" width="5%" align="left">
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
                            <%-- Vendor Save button was moved to top          --%>
                            <%--  <asp:Button ID="AddressSave" runat="server" SkinID="btnInner-Save" Text="Save" ToolTip="<%$ resources:Controls,Save %>"
                            OnClientClick="javascript:return SaveVendorPage();" />--%>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="MaterialStores" class="jquery-tabs-contents">
                        <div id="divTree" style="overflow: auto">
                            <div id="treewrap">
                                <div id="trvStores" class="treeview-adj">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="RelatedMaterial" class="jquery-tabs-contents">
                        <div class="clear">
                        </div>
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label>
                                            <%=Resources.Controls.MaterialCode%></label>
                                        <asp:Label ID="lblBaseMaterialCode" runat="server" Text="" CssClass="input-half height-auto break-word"></asp:Label>
                                        <div class="clear">
                                        </div>
                                        <label>
                                            <%=Resources.Controls.MaterialCategoryName%></label>
                                        <asp:Label ID="lblBaseMaterialCatName" runat="server" Text="" CssClass="input-half height-auto break-word"></asp:Label>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label>
                                            <%=Resources.Controls.MaterialName%></label>
                                        <asp:Label ID="lblBaseMaterialName" runat="server" Text="" CssClass="input-half-20-11-9 height-auto break-word"></asp:Label>
                                        <label>
                                            <%=Resources.Controls.RelatedMaterial%></label>
                                        <asp:TextBox ID="RelatedMaterialItem" runat="server" CssClass="input-half-20-11-9">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="R_ITEM" runat="server" Value="0" />
                                        <asp:ImageButton runat="server" SkinID="imbaddnew" ID="ImageButton3" Text="0.00"
                                            OnClientClick="javascript:return AddMaterialDetails()" TabIndex="59" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="divRelMaterial">
                            <table rules="all" id="grdRelMaterial" grandtype="GrandGrid" pagesize="20" paging="true"
                                width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                                <thead>
                                    <tr>
                                        <th fieldmap="ITC_IS_STOCK" isvisible="false"></th>
                                        <th fieldmap="ITM_PK" isvisible="false"></th>
                                        <th fieldmap="ITM_CODE" sortable="true" align="left" width="12%">
                                            <%=Resources.Controls.MaterialCode%>
                                        </th>
                                        <th fieldmap="ITM_TEXT" sortable="true" align="left" width="54%">
                                            <%=Resources.Controls.MaterialName%>
                                        </th>

                                        <th fieldmap="ITC_NAME" sortable="true" align="left" width="20.5%">
                                            <%=Resources.Controls.CategoryName%>
                                        </th>


                                        <th fieldmap="UOM_NAME" sortable="true" align="left" width="7%">
                                            <%=Resources.Controls.MaterialUOM%>
                                        </th>
                                        <th type="Template" width="8.5%" align="left">
                                            <div style="text-align: left">
                                                <asp:ImageButton runat="server" ID="ImageButton5" ToolTip="<%$Resources:Controls,Delete %>"
                                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETERELATED')"
                                                    EnableViewState="false" />
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="BillOfMaterial" class="jquery-tabs-contents">
                        <div class="clear">
                        </div>
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label>
                                            <%=Resources.Controls.MaterialCode%></label>
                                        <asp:Label ID="lblBOMaterialCode" runat="server" Text="" CssClass="input-half height-auto break-word"></asp:Label>
                                        <div class="clear">
                                        </div>
                                        <label for="ITC_BOM_CAT">
                                            <%=Resources.Controls.MaterialCategory%>*</label>
                                        <asp:DropDownList ID="ITC_BOM_CAT" runat="server" TabIndex="5" EnableViewState="False"
                                            CssClass="select-w61per" onchange="javascript:SetBOMCategoryItem();">
                                        </asp:DropDownList>
                                        <label for="lblUOM">
                                            <%=Resources.Controls.UOM%></label>
                                        <asp:Label ID="lblUOM" runat="server" Text="" CssClass="input-small"></asp:Label>

                                        <div class="clear">
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label>
                                            <%=Resources.Controls.MaterialName%></label>
                                        <asp:Label ID="lblBOMaterialName" runat="server" Text="" CssClass="input-half-20-11-9 height-auto break-word"></asp:Label>
                                        <label>
                                            <%=Resources.Controls.Material%></label>
                                        <asp:TextBox ID="txtBOMaterial" runat="server" CssClass="input-half-20-11-9">
                                        </asp:TextBox>
                                        <asp:HiddenField ID="B_ITEM" runat="server" Value="0" />


                                        <label for="txtPcsInKG">
                                            <%=Resources.Controls.PcsInKG%></label>
                                        <asp:TextBox ID="txtPcsInKG" runat="server" Text="" CssClass="numeric input-small"></asp:TextBox>

                                        <asp:ImageButton runat="server" SkinID="imbaddnew" ID="imgAddBOM" Text="0.00"
                                            OnClientClick="javascript:return AddBOMDetails()" TabIndex="59" />
                                        <div class="clear">
                                        </div>

                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="divBOMaterial">
                            <table rules="all" id="grdBOMaterial" grandtype="GrandGrid" pagesize="20" paging="true"
                                width="100%" editfunction="GridAction" editable="true" class="gridwraptable gridwrap">
                                <thead>
                                    <tr>
                                        <th fieldmap="ITC_IS_STOCK" isvisible="false"></th>
                                        <th fieldmap="ITM_PK" isvisible="false"></th>
                                        <th fieldmap="ITM_CODE" sortable="true" align="left" width="12%">
                                            <%=Resources.Controls.MaterialCode%>
                                        </th>
                                        <th fieldmap="ITM_TEXT" sortable="true" align="left" width="44%">
                                            <%=Resources.Controls.MaterialName%>
                                        </th>

                                        <th fieldmap="ITC_NAME" sortable="true" align="left" width="20.5%">
                                            <%=Resources.Controls.CategoryName%>
                                        </th>


                                        <th fieldmap="UOM_NAME" sortable="true" align="left" width="7%">
                                            <%=Resources.Controls.MaterialUOM%>
                                        </th>
                                        <th fieldmap="IBM_CONV" sortable="true" align="left" width="10%">
                                            <%=Resources.Controls.PcsInKG%>
                                        </th>
                                        <th type="Template" width="8.5%" align="left">
                                            <div style="text-align: left">
                                                <asp:ImageButton runat="server" ID="ImageButton6" ToolTip="<%$Resources:Controls,Delete %>"
                                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETEBOM')"
                                                    EnableViewState="false" />
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                    <div class="clear">
                    </div>
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
                                    <asp:TextBox runat="server" ID="IPD_INNER_LENGTH" TabIndex="16" MaxLength="8" CssClass="input-small"
                                        EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <div id="divHeight">
                                        <label for="IPD_INNER_HEIGHT">
                                            <%=Resources.Controls.HeightMM%></label><asp:TextBox runat="server" ID="IPD_INNER_HEIGHT"
                                                TabIndex="17" MaxLength="8" CssClass="input-small" EnableViewState="false"></asp:TextBox>
                                        <div class="clear">
                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S" style="float: right; margin-right: 0">
                                    <label for="IPD_INNER_BREADTH">
                                        <%=Resources.Controls.WidhMM%></label>
                                    <asp:TextBox runat="server" ID="IPD_INNER_BREADTH" TabIndex="16" MaxLength="8" CssClass="input-small"
                                        EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <div class="clear">
                                    </div>
                                    <label for="ITM_WEIGHT">
                                        <%=Resources.Controls.WeightMM%></label>
                                    <asp:TextBox runat="server" ID="ITM_WEIGHT" TabIndex="17" MaxLength="8" CssClass="input-small"
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
                                        MaxLength="8" CssClass="input-small" EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="IPD_OUTER_HEIGHT">
                                        <%=Resources.Controls.HeightMM%></label>
                                    <asp:TextBox runat="server" ID="IPD_OUTER_HEIGHT" onblur="CalcCBM();" TabIndex="18"
                                        MaxLength="8" CssClass="input-small" EnableViewState="false">
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
                                        MaxLength="8" CssClass="input-small" EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="ITM_CBM">
                                        <%=Resources.Controls.CBM%></label>
                                    <asp:TextBox runat="server" ID="ITM_CBM" ReadOnly="true" MaxLength="8" CssClass="input-small"
                                        TabIndex="18" EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr class="fields-group-2col">
                            <td>
                                <div class="div2col-S">
                                    <label for="IPD_PLY">
                                        <%=Resources.Controls.Ply%></label>
                                    <asp:TextBox runat="server" ID="IPD_PLY" TabIndex="19" MaxLength="100" CssClass="input-small"
                                        EnableViewState="false">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="IPD_CUSTOMER">
                                        <%=Resources.Controls.ApplicableFor%></label>
                                    <asp:DropDownList ID="IPD_CUSTOMER" runat="server" TabIndex="21" EnableViewState="false"
                                        CssClass="select-half-a">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <label for="IPD_ART_WORK">
                                        <%=Resources.Controls.ArtWorkVer%></label>
                                    <asp:TextBox runat="server" ID="IPD_ART_WORK" CssClass="input-small" TabIndex="23"
                                        EnableViewState="false" MaxLength="100">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="IPD_PAPER_TYPE">
                                        <%=Resources.Controls.PaperType%></label>
                                    <asp:TextBox runat="server" ID="IPD_PAPER_TYPE" CssClass="input-small" TabIndex="24"
                                        EnableViewState="false" MaxLength="100">
                                    </asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S" style="float: right; margin-right: 0">
                                    <label for="IPD_PAPER_COLOR">
                                        <%=Resources.Controls.PaperColor%></label>
                                    <asp:TextBox runat="server" ID="IPD_PAPER_COLOR" TabIndex="20" EnableViewState="false"
                                        CssClass="input-half" MaxLength="100">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="IPD_THICKNESS">
                                        <%=Resources.Controls.ThicknessMM%></label>
                                    <asp:TextBox runat="server" ID="IPD_THICKNESS" TabIndex="22" EnableViewState="false"
                                        CssClass="input-small">
                                    </asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <%--  Regarding Task NO: 844 ,we added an active checkbox in material div.After discussion with anish no need to use below ddl--%>
                                    <%--<label for="IPD_ACTIVE">
                                        <%=Resources.Controls.Status%></label>
                                    <asp:DropDownList ID="IPD_ACTIVE" runat="server" CssClass="select-small-a" EnableViewState="false"
                                        TabIndex="23">
                                        <asp:ListItem Text="<%$ resources:Controls, Active %>" Value="1" />
                                        <asp:ListItem Text="<%$ resources:Controls, InActive %>" Value="0" />
                                    </asp:DropDownList>--%>
                                    <%--<div class="clear">
                                    </div>--%>
                                    <asp:HiddenField ID="IPD_ACTIVE" runat="server" />
                                    <%--  End Regarding Task NO: 844 --%>
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
                            <asp:HiddenField ID="ITM_NEED_QC_INSP" runat="server" />
                            <asp:HiddenField ID="ITM_IS_WORK_ORDER" runat="server" />

                            <asp:HiddenField ID="ITM_NEED_BATCH_STK" runat="server" />
                            <asp:HiddenField ID="ITM_IS_CONVERSION_REQD" runat="server"/>
                            <asp:HiddenField ID="ITM_IS_LINKED_ITEM" runat="server" />
                            <asp:HiddenField ID="TEMPFILELIST" runat="server" />
                            <asp:HiddenField ID="curTR" runat="server" />
                            <asp:HiddenField ID="hdfCurDate" runat="server" Value="" />
                            <asp:HiddenField runat="server" ID="StoreDtl" />
                            <asp:HiddenField runat="server" ID="RelatedItemMap" />
                            <asp:HiddenField runat="server" ID="BomItemMap" />
                            <%-- For checking Finaltab or not--%>
                            <asp:HiddenField ID="hdfIsFinalTab" runat="server" Value="0" />
                            <asp:HiddenField ID="hdfEnableWorkOrderItem" runat="server" Value="0" />
                            <asp:HiddenField ID="ITM_IS_ASSET" runat="server" />
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <%--  Paking Material End--%>
                <%--   Vendor Rate Popup which is showing in Listing--%>
                <div id="divVendorRateDetails" title="<%=Resources.Captions.ItemRateDetails%>" style="display: none">
                    <div class="content-wrapper">
                        <table rules="all" id="grdVendorRate" grandtype="GrandGrid" paging="false" editable="false"
                            class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="ITV_VENDOR" isvisible="false"></th>
                                    <th fieldmap="ITV_ACTIVE" isvisible="false"></th>
                                    <th fieldmap="ITV_VENDORNAME" width="53%" align="left">
                                        <%=Resources.Controls.VendorName%>
                                    </th>
                                    <th fieldmap="ITV_PRICE" width="25%" align="right">
                                        <%=Resources.Controls.StdPrice%>
                                    </th>
                                    <th fieldmap="ITV_CURRENCY" isvisible="false" align="left"></th>
                                    <th fieldmap="MaterialCurrencyText" width="15%" align="left">
                                        <%=Resources.Controls.Currency%>
                                    </th>
                                    <th fieldmap="ITV_ACTIVE_TEXT" width="7%" align="right">
                                        <%=Resources.Controls.Active%>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="SBU" runat="server" Value="0" />
        <asp:HiddenField ID="AutoStartValue" runat="server" Value="0" />
        <asp:HiddenField ID="ITM_SET" runat="server" Value="1" />
        <asp:HiddenField ID="hdfFromPackSpec" runat="server" Value="0" />
        <asp:HiddenField runat="server" ID="UserID" Value="1" />
        <asp:HiddenField runat="server" ID="MaterialDetailId" Value="0" EnableViewState="false" />
        <asp:HiddenField runat="server" ID="ITM_PK" Value="0" EnableViewState="false" />
        <asp:HiddenField runat="server" ID="ItemType" Value="0" EnableViewState="false" />
        <asp:HiddenField runat="server" ID="isTaxAdd" Value="0" />
        <asp:HiddenField runat="server" ID="isDiscountAdd" Value="0" />
        <asp:HiddenField ID="hdfShowMultipleUOM" runat="server" Value="0" />
        <asp:HiddenField ID="hdfShowPurchaseUOM" runat="server" Value="0" />
        <asp:HiddenField ID="hdfItemType" runat="server" Value="0" />
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
        <asp:HiddenField runat="server" ID="MappingDetailsList" Value="" />
        <asp:HiddenField ID="STATUS" runat="server" Value="1" />
        <%--Used for saving value in ITM_ACTIVE column--%>
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
                        EnableViewState="False" OnClientClick="javascript:return AddTaxDiscount();" />
                </div>
                <asp:HiddenField runat="server" ID="hdfTaxCategory" Value="1" />
                <asp:HiddenField runat="server" ID="hdnSlNo" Value="0" />
                <asp:HiddenField runat="server" ID="hdfEnableMaxOrderQty" Value="0" />
                <div>
                    <table rules="all" id="grdTaxDetails" grandtype="GrandGrid" paging="false" width="100%"
                        editfunction="GridHandler" editable="true" class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="IVT_SL_NO" isvisible="false"></th>
                                <th fieldmap="IVT_PK" isvisible="false"></th>
                                <th fieldmap="IVT_TAX" isvisible="false"></th>
                                <th fieldmap="IVT_TAX_CATEGORY" isvisible="false"></th>
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
