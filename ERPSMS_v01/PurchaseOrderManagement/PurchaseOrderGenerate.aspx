<%@ Page Title="<%$ Resources:Captions,Title_PurchaseOrder %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" EnableEventValidation="false" Theme="ClassicExt" CodeBehind="PurchaseOrderGenerate.aspx.cs"
    Inherits="ERPSMS_v01.PurchaseOrderManagement.PurchaseOrderGenerate" ValidateRequest="false" %>

<%--<%@ Register Src="../UserControls/RelatedItemWidget.ascx" TagName="RelatedItemWidget"
    TagPrefix="uc2" %>--%>
<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/PurchaseOrderManagement/PurchaseOrderGenerate.js.axd"
        type="text/javascript"></script>
    <script src="../Scripts/JSLINQ/JSLINQ.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        //    var uiUrl = pageURL.replace(window.document.location.search, "").replace(location.pathname, virtualPath == "" ? "/Handlers/UIAutoComplete.ashx" : "/" + virtualPath + "Handlers/UIAutoComplete.ashx");
        //    GrandScriptUtils.MakeAutoCompleteDDL("txtFromPort", uiUrl + "?SIType=" + $("[id$=POH_TYPE]").val() + "&PurFromPort=1", "hdfFromPortID", true, true, "FILLPORTDETAILS");
        //    GrandScriptUtils.MakeAutoCompleteText("txtToPort", uiUrl + "?SIType=" + $("[id$=POH_TYPE]").val() + "&PurFromPort=1", "hdfToPortID", true, true, "FILLPORTDETAILS");

        function CheckConfigForShowDescPM() {
            var ShowDescPM = <%=GetGlobalResourceObject("ConfigurationsRes", "ShowDescPM") %>;
            $("[id$=hdfShowDescPM]").val(ShowDescPM);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="fixed-buttons">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlListing">
                            <li id="pnlAmend">
                                <asp:Button runat="server" TabIndex="2" ID="btnAmend" CommandName="AMEND" OnClick="ActionHandler"
                                    Text="<%$resources:Controls,Amend %>" CommandArgument="SEC_ActionPanel" SkinID="btnInner-amend"
                                    ToolTip="<%$resources:Controls,Amend %>" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSubmit" SkinID="btnInner-submit" Text="<%$Resources:Controls,Submit%>"
                                    TabIndex="35" EnableViewState="False" ToolTip="<%$resources:ErpRes,Submit %>"
                                    OnClientClick="javascript:return  POWkfSubmit();" />
                            </li>
                            <%-- <li>
                                <asp:Button runat="server" ID="btnTraceability" SkinID="btnInner-available" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="36" EnableViewState="False" ToolTip="<%$resources:ErpRes,Save %>" OnClientClick="javascript:return ShowTraceability();" />
                            </li>--%>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="36" EnableViewState="False" ToolTip="<%$resources:ErpRes,Save %>" OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" ToolTip="<%$resources:ErpRes,Cancel %>" OnClientClick="javascript:return CancelPO();"
                                    TabIndex="37" />
                            </li>
                            <li id="pnlAlert" runat="server" style="display: none">
                                <asp:Button runat="server" ID="btnAlert" TabIndex="38" Text="<%$resources:Controls,Alert %>"
                                    ToolTip="<%$resources:Controls,Alert %>" OnClick="ActionHandler" CommandName="ALERT"
                                    OnClientClick="javascript:return AlertShow();" SkinID="btnInner-alert" />
                            </li>
                            <li id="LiAlertSave" runat="server" style="display: none">
                                <asp:Button runat="server" ID="btnAlertSave" TabIndex="38" Text="" OnClick="ActionHandler"
                                    CommandName="ALERTSAVE" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnPrint" SkinID="btnInner-Print" Text="<%$Resources:Controls,Print%>"
                                    TabIndex="39" EnableViewState="False" ToolTip="<%$resources:ErpRes,Print %>"
                                    OnClientClick="javascript:return PrintPage();" /></li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:HiddenField ID="hdfProcId" runat="server" Value="0" />
            <asp:HiddenField ID="hdfShowAmendmentButton" runat="server" Value="0" />
            <asp:HiddenField ID="hdfDeptID" runat="server" Value="1" />
        </div>
        <div class="large-tabs">
            <div class="tab-container-large">
                <ul id="tab-menu">
                    <li><span class="tab-inactive" id="aSearch"><a href="#" onclick="javascript:ActiveSearch();">
                        <%=Resources.Captions.Search%></a></span></li>
                    <li><span class="tab-inactive" id="aCreatePO"><a href="#">
                        <%=Resources.Captions.CreatePO%></a></span></li>
                </ul>
                <div class="clear">
                </div>
            </div>
        </div>
    </div>
    <%-- fixed button end here--%>
    <div class="content-wrapper">
        <asp:HiddenField ID="POH_VENDOR" runat="server" Value="0" />
        <asp:HiddenField ID="LastModifiedTime" runat="server" Value="" />
        <asp:HiddenField ID="POH_PK" runat="server" Value="0" />
        <asp:HiddenField ID="MaterialDetails" runat="server" />
        <div>
            <div id="tab1Content">
                <h1 class="search-colapse-normal">
                    <%=Resources.Captions.FilterPurchaseRequests%>
                    <img id="imgPRShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png" alt="<%= Resources.Controls.Show%>"
                        title="<%= Resources.Controls.Show%>" style="display: none; cursor: pointer"
                        onclick="javascript:ShowPRShow();" />
                    <img id="imgPRHide" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="<%= Resources.Controls.Hide%>"
                        title="<%= Resources.Controls.Hide%>" style="cursor: pointer" onclick="javascript:HidePRShow();" />
                </h1>
                <div id="divPendingPR">
                    <div class="div2col-S button-alignright">
                        <label for="Store" class="lft-lbl">
                            <%=Resources.Controls.RequestingStore%>
                            *</label>
                        <asp:DropDownList ID="Store" runat="server" TabIndex="1" CssClass="srchboxtextbx request-select"
                            onchange="javascript:BindPendingPRGrid();" EnableViewState="true">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdfStorePK" runat="server" Value="0" />
                        <asp:Button ID="btnDirectPO" runat="server" ToolTip="<%$resources:Controls,DirctPO %>"
                            TabIndex="1" Text="<%$ Resources:Controls, DirctPO%>" OnClientClick="javascript:return DirectPO();" />
                        <div class="clear">
                        </div>
                    </div>
                    <%-- <div class="div2col-S">
                        <label for="Store">
                            <%=Resources.Controls.RequestingStore%>
                            *</label>
                        <asp:DropDownList ID="Store" runat="server" TabIndex="1" CssClass="srchboxtextbx floatLeft"
                            onchange="javascript:BindPendingPRGrid();" EnableViewState="true">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdfStorePK" runat="server" Value="0" />
                        
                    </div>--%>
                    <div class="search-wrap-custom1">
                        <div id="divSearch">
                            <label for="SearchType">
                                <%=Resources.Controls.SearchBy%></label>
                            <asp:DropDownList ID="SearchType" runat="server" CssClass="srchboxtextbx" TabIndex="2"
                                onchange="javascript:SetSearchType();" EnableViewState="false">
                                <%--<asp:ListItem Value="0" Text="<%$ Resources:BindValues, Select%>">
                                </asp:ListItem>--%>
                                <asp:ListItem Value="PRH_NO" Text="<%$ Resources:BindValues, PRNO%>">
                                </asp:ListItem>
                                <asp:ListItem Value="ITM_CODE" Text="<%$ Resources:BindValues, Item%>">
                                </asp:ListItem>
                                <asp:ListItem Value="Date" Text="<%$ Resources:BindValues, RequiredBy%>">
                                </asp:ListItem>
                                <asp:ListItem Value="CMP_DISPLAY_CODE" Text="<%$ Resources:BindValues, Plant%>">
                                </asp:ListItem>
                                <asp:ListItem Value="PRH_ISSUE_DEPT" Text="<%$ Resources:BindValues, RequestedDept%>">
                                </asp:ListItem>
                            </asp:DropDownList>
                            <div id="divSearchDtls">
                                <asp:TextBox ID="SearchValue" runat="server" Width="150px" EnableViewState="false"
                                    onkeydown="return AvoidSpecialChar(event)" TabIndex="3">
                                </asp:TextBox>
                            </div>
                            <div id="divDate">
                                <span>
                                    <%=Resources.Controls.FromDate%></span>
                                <asp:TextBox ID="FromDate" runat="server" TabIndex="3" EnableViewState="false" onkeydown="return CheckKey(event)"
                                    onpaste="return false;" CssClass="date-picker" MaxLength="12">
                                </asp:TextBox>
                                <asp:HiddenField ID="hdfFrmDate" runat="server" />
                                <span>
                                    <%=Resources.Controls.ToDate%></span>
                                <asp:TextBox ID="ToDate" runat="server" TabIndex="4" EnableViewState="false" onkeydown="return CheckKey(event)"
                                    onpaste="return false;" CssClass="date-picker" MaxLength="12">
                                </asp:TextBox>
                                <asp:HiddenField ID="hdfToDate" runat="server" />
                            </div>
                            <asp:ImageButton ID="imbSearch" runat="server" SkinID="search" TabIndex="4" OnClientClick="javascript:BindPendingPRGrid(); return false;"
                                EnableViewState="false" />
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="scroll-h150">
                        <div class="gridwrap " id="divPendPRList">
                            <table rules="all" id="grdPendingPRList" grandtype="GrandGrid" paging="false" enablecheckbox="true"
                                width="100%" class="gridwraptable gridwrap tablefixwidth-td">
                                <thead>
                                    <tr>
                                        <th fieldmap="PRD_PK" isvisible="false"></th>
                                        <th fieldmap="PRH_PK" isvisible="false"></th>
                                        <th fieldmap="PRD_ITEM" isvisible="false"></th>
                                        <th fieldmap="PRD_UOM" isvisible="false"></th>
                                        <th fieldmap="ITC_PO_ITEM_TYPE" isvisible="false"></th>
                                        <th fieldmap="PRH_COMPANY" isvisible="false"></th>
                                        <th fieldmap="PRH_DEPT" isvisible="false"></th>
                                        <th fieldmap="PRH_IS_GLOVE" isvisible="false"></th>
                                        <%--<th fieldmap="PRD_UOM_TEXT" isvisible="false">
                                        </th>--%>
                                        <th fieldmap="ITV_PRICE" isvisible="false"></th>
                                        <th fieldmap="PRD_QTY_ORDERED" isvisible="false">
                                            <%=Resources.Controls.OrdQty%>
                                        </th>
                                        <th fieldmap="PRD_REQD_DATE" sortable="true" width="70px">
                                            <%=Resources.Controls.ReqBy%>
                                        </th>
                                        <th fieldmap="PRH_NO" align="left" sortable="true" width="110px">
                                            <%=Resources.Controls.PRNo%>
                                        </th>
                                        <th fieldmap="ITM_TEXT" isvisible="false"></th>
                                        <th fieldmap="PRH_COMPANY_TEXT" align="left" width="25px"></th>
                                        <th fieldmap="ITM_CODE" align="left" sortable="true" width="120px">
                                            <%=Resources.Controls.ItemCode%>
                                        </th>
                                        <th fieldmap="ITM_NAME" align="left" sortable="true" width="300px">
                                            <%=Resources.Controls.Description%>
                                        </th>
                                        <th runat="server" id="thPRReqDept" fieldmap="PRH_ISSUE_DEPT_TEXT" align="left" sortable="true"
                                            width="140px">
                                            <%=Resources.Controls.ReqDept%>
                                        </th>
                                        <th fieldmap="PRH_ISSUE_DEPT" isvisible="false"></th>
                                        <%--<th fieldmap="PRD_REQD_DATE" width="10%">
                                            <%=Resources.Controls.ReqdDate%>
                                        </th>--%>
                                        <th fieldmap="PRD_QTY_APPROVED" isvisible="false">
                                            <%=Resources.Controls.ReQty%>
                                        </th>
                                        <th fieldmap="PRD_SORT_DATE" isvisible="false" sortable="true"></th>
                                        <th fieldmap="PRD_DATE" align="left" sortable="true" width="70px">
                                            <%=Resources.Controls.PRDate%>
                                        </th>
                                        <th fieldmap="PRD_QTY_BALANCE" width="90px" sortable="true" align="right">
                                            <%=Resources.Controls.Qty%>
                                        </th>
                                        <th fieldmap="PRD_UOM_TEXT" align="left" width="30px">
                                            <%=Resources.Controls.MaterialUOM%>
                                        </th>
                                        <th fieldmap="POR_QTY_ORDERED" width="60px">
                                            <%=Resources.Controls.PoQty%>
                                        </th>
                                        <th fieldmap="POR_QTY_ADDITIONAL" width="65px">
                                            <%=Resources.Controls.AdlQty%>
                                        </th>
                                        <th fieldmap="PRD_ITEM_SPEC" align="left" width="200px">
                                            <%=Resources.Controls.Spec%>
                                        </th>
                                        <th fieldmap="PRD_PURPOSE" isvisible="false">
                                            <%=Resources.Controls.Spec%>
                                        </th>
                                        <th fieldmap="IO_NO" isvisible="false"></th>
                                        <th fieldmap="PRH_PO_CATEGORY" isvisible="false"></th>
                                        <th fieldmap="PRH_GROUP" isvisible="false"></th>
                                        <th fieldmap="PRH_GROUP_TEXT" width="50px">
                                            <%=Resources.Controls.Group%>
                                        </th>
                                        <th fieldmap="PRH_COST_CENTER" isvisible="false"></th>
                                        <th fieldmap="PRH_COST_CENTER_TEXT" isvisible="false"></th>
                                        <th fieldmap="PRH_INVESTOR_CODE" isvisible="false"></th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                    <div class="button-wrap-right">
                        <asp:Button ID="btnAddSelectedItems" runat="server" ToolTip="<%$resources:Controls,AddToList %>"
                            Text="<%$ Resources:Controls, AddToList%>" OnClientClick="javascript:return AddPORequestList();"
                            TabIndex="7" />
                        <%--<asp:Button ID="btnContinueRequest" Text="<%$ Resources:Controls, Continue%>" runat="server"
                            OnClientClick="javascript:return POContinue();" TabIndex="8" />--%>
                        <div class="clear">
                        </div>
                    </div>
                    <div id="divPoListing" style="display: none">
                        <div class="gridwrap">
                            <table rules="all" id="grdPOList" grandtype="GrandGrid" paging="false" width="100%"
                                editfunction="GridAction" editable="true" class="gridwraptable gridwrap tablefixwidth-td">
                                <thead>
                                    <tr>
                                        <th fieldmap="POR_ITEM" isvisible="false"></th>
                                        <th fieldmap="PRH_PK" isvisible="false"></th>
                                        <th fieldmap="PRH_DEPT" isvisible="false"></th>
                                        <th fieldmap="PRH_COMPANY" isvisible="false"></th>
                                        <th fieldmap="ITC_PO_ITEM_TYPE" isvisible="false"></th>
                                        <th fieldmap="SLNO" align="center" width="20px">SN.
                                        </th>
                                        <th fieldmap="PRH_NO" align="left" width="110px">
                                            <%=Resources.Controls.PRNo%>
                                        </th>
                                        <th fieldmap="PRD_DATE" align="left" width="60px">
                                            <%=Resources.Controls.PRDate%>
                                        </th>
                                        <th fieldmap="ITM_TEXT" isvisible="false"></th>
                                        <th fieldmap="PRH_COMPANY_TEXT" align="left" width="25px"></th>
                                        <th fieldmap="ITM_CODE" align="left" width="148px">
                                            <%=Resources.Controls.Item%>
                                        </th>
                                        <th fieldmap="ITM_NAME" align="left" width="175px">
                                            <%=Resources.Controls.Description%>
                                        </th>
                                        <th runat="server" id="thPOReqDept" fieldmap="PRH_ISSUE_DEPT_TEXT" align="left" width="140px">
                                            <%=Resources.Controls.ReqDept%>
                                        </th>
                                        <th fieldmap="PRH_ISSUE_DEPT" isvisible="false"></th>
                                        <th fieldmap="PRH_GROUP" isvisible="false"></th>
                                        <th runat="server" id="thPOGroup" fieldmap="PRH_GROUP_TEXT" width="50px">
                                            <%=Resources.Controls.Group%>
                                        </th>
                                        <th fieldmap="PRD_ITEM_SPEC" align="left" width="110px">
                                            <%=Resources.Controls.Spec%>
                                        </th>
                                        <th fieldmap="PRD_REQD_DATE" width="68px">
                                            <%=Resources.Controls.ReqBy%>
                                        </th>
                                        <th fieldmap="PRD_QTY_BALANCE" width="85px" align="right">
                                            <%=Resources.Controls.Qty%>
                                        </th>
                                        <th fieldmap="PRD_UOM_TEXT" align="left" width="30px">
                                            <%=Resources.Controls.MaterialUOM%>
                                        </th>
                                        <th fieldmap="PRD_QTY_APPROVED" align="right" width="83px">
                                            <%=Resources.Controls.PRQty%>
                                        </th>
                                        <th fieldmap="POR_QTY_ORDERED" align="right" width="83px">
                                            <%=Resources.Controls.OrdQty%>
                                        </th>
                                        <th fieldmap="PRH_COST_CENTER" isvisible="false"></th>
                                        <th fieldmap="PRH_COST_CENTER_TEXT" isvisible="false"></th>
                                        <th fieldmap="PRH_INVESTOR_CODE" isvisible="false"></th>
                                        <th type="Template" width="50px">
                                            <div>
                                                <asp:ImageButton runat="server" ID="imbDeleteItem" ToolTip="<%$resources:ErpRes,Delete %>"
                                                    TabIndex="7" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETEITEM')" />
                                                <asp:ImageButton runat="server" ID="imbrateHistory" SkinID="history" ToolTip="<%$resources:ErpRes,ShowHistory %>"
                                                    TabIndex="7" OnClientClick="javascript:return GridMaterialAction($(this).parents('tr:eq(0)'),'HISTORY')" />
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
                <div id="divRelatedWidget" style="display: none">
                    <%-- <uc2:RelatedItemWidget ID="RelatedItemWidget1" runat="server" />--%>
                </div>
                <div id="VendorSelection" style="display: none">
                    <h1 class="search-colapse-normal" id="VendorHeading">
                        <%=Resources.Captions.SelectVendors%>
                        <img id="imgVendorShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                            alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowVendor();" />
                        <img id="imgVendorHide" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="Hide"
                            title="Hide" style="cursor: pointer" onclick="javascript:HideVendor();" />
                    </h1>
                    <div id="divVendors" class="checkbx scroll-h100">
                    </div>
                </div>
                <div class="clear">
                </div>
                <div id="divOtherVendor" style="display: none">
                    <h1 class="search-colapse-normal" id="OtherVendorHeading">
                        <%=Resources.Captions.SelectOtherVendor%>
                        <img id="imgOtherVendorShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                            alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowOtherVendor();" />
                        <img id="imgOtherVendorHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                            alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:HideOtherVendor();" />
                    </h1>
                    <div id="divSelectOtherVendor">
                        <asp:Label runat="server" ID="lblVnd" Text="<%$ Resources:Controls,OtherVendor %>"
                            AssociatedControlID="txtVendorName"></asp:Label>
                        <asp:TextBox runat="server" ID="txtVendorName" CssClass="srchboxtextbx request-select"
                            TabIndex="3">
                        </asp:TextBox>
                        <asp:HiddenField ID="VEN_PK" runat="server" />
                    </div>
                </div>
                <div id="divContinue" style="display: none">
                    <div class="button-wrap-right">
                        <asp:Button ID="btnClearAllSelectedItem" runat="server" Text="<%$ Resources:Controls, ClearAll%>"
                            ToolTip="<%$resources:Controls,ClearAll %>" OnClientClick="javascript:return ClearAllSelectedItem();"
                            TabIndex="9" />
                        <asp:Button ID="btnContinue" Text="<%$ Resources:Controls, Continue%>" ToolTip="<%$resources:Controls,Continue %>"
                            runat="server" OnClientClick="javascript:return ChangeWorkFlow();" TabIndex="8" />
                        <%--  POContinue()--%>
                        <asp:Button ID="btnChangeWorkFlow" runat="server" OnClick="ActionHandler" CommandName="REQDEPTCHANGE"
                            Style="display: none" EnableTheming="false" />
                    </div>
                </div>
            </div>
            <div id="tab2Content">
                <h1 class="search-colapse-normal">
                    <%=Resources.Captions.PODetails%>
                    <img id="imbShowPoDetails" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                        alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowPoDetails();" />
                    <img id="imbHidePoDetails" src="../Images/Classic/Icons/arrow-colapse-active.png"
                        alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:HidePoDetails();" />
                </h1>
                <div id="divPoDetails">
                    <div>
                        <table class="table-devide">
                            <tr>
                                <td>
                                    <div class="div2col-S">
                                        <label>
                                            <%=Resources.Controls.PoNumber%></label>
                                        <asp:Label ID="lblPOH_NO" runat="server" Text="" CssClass="input-w24per"></asp:Label>
                                        <div style="width: 18px; display: inline-block;">
                                            <asp:ImageButton ID="btnRevision" runat="server" OnClientClick="javascript:return RevisionHistory();"
                                                TabIndex="10" SkinID="history" ToolTip="<%$resources:RevisionHistory %>" />
                                        </div>
                                        <label for="RequiredBy" class="middle-lbl-xsmall-a">
                                            <%=Resources.Controls.PODate%>*</label>
                                        <asp:TextBox ID="POH_DATE" runat="server" TabIndex="10" onkeydown="return CheckKey(event)"
                                            onpaste="return false;" CssClass="date-picker"></asp:TextBox>
                                        <div class="clear">
                                        </div>
                                        <%--    --%>
                                        <asp:HiddenField ID="isAlert" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdfDisablePOCategory" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdfIsGoToInbox" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdfIsGoInbox" runat="server" Value="0" />
                                        <asp:HiddenField ID="saveMsg" runat="server" Value="" />
                                        <asp:HiddenField ID="POH_NO" runat="server" Value="0" />
                                        <asp:HiddenField ID="POH_IS_AMEND" runat="server" Value="0" />
                                        <asp:HiddenField ID="APT_CODE" runat="server" />
                                        <asp:HiddenField ID="WKF_FLAG" runat="server" Value="0" />
                                        <%--<asp:HiddenField ID="WKF_PROCESS" runat="server" Value="" />--%>
                                        <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdfSBUcompany" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
                                        <asp:HiddenField ID="hdfCreator" runat="server" Value="0" />
                                        <div class="clear">
                                        </div>
                                        <asp:Label ID="Label1" runat="server" Text='<%$ Resources:ConfigurationsRes,POContrRef%>'
                                            CssClass="bg-none border0 lbl-25-1perc numeric"></asp:Label>
                                        <%--<label for="POH_CONTRACT_REF_NO">
                                            <%=Resources.ConfigurationsRes.POContrRef%></label>--%>
                                        <asp:TextBox ID="POH_CONTRACT_REF_NO" runat="server" MaxLength="50" TabIndex="14"
                                            onkeypress="return this.value.length<50" CssClass="input-w24per" onpaste="return this.value.length<50"></asp:TextBox>
                                        <label for="Category" class="middle-lbl-a">
                                            <%=Resources.Controls.POCategory%>*</label>
                                        <asp:DropDownList runat="server" ID="POH_ITEM_TYPE" CssClass="select-small-a" TabIndex="15">
                                        </asp:DropDownList>
                                        <div style="display: none">
                                            <label>
                                                <%=Resources.Controls.createdBy%></label>
                                            <asp:Label ID="CreatedBy" runat="server" Text="" CssClass="lbl-half"></asp:Label>
                                            <asp:HiddenField ID="POH_CRTD_BY" runat="server" />
                                        </div>
                                        <div id="divFromPort">
                                            <asp:Label ID="lblFromPort" runat="server" AssociatedControlID="POH_FROM_PORT_TEXT"
                                                Text='<%$ Resources:Controls,FromPort%>'></asp:Label>
                                            <asp:TextBox runat="server" ID="POH_FROM_PORT_TEXT" CssClass="input-w65per"></asp:TextBox>
                                            <asp:HiddenField runat="server" ID="POH_FROM_PORT" />
                                        </div>
                                        <div id="divToPort">
                                            <asp:Label ID="lblToPort" runat="server" AssociatedControlID="POH_TO_PORT_TEXT" Text='<%$ Resources:Controls,ToPort%>'></asp:Label>
                                            <asp:TextBox runat="server" ID="POH_TO_PORT_TEXT" CssClass="input-w65per"></asp:TextBox>
                                            <asp:HiddenField ID="POH_TO_PORT" runat="server" />
                                        </div>
                                        <div id="divInvestor">
                                            <asp:Label ID="lblInvestorCode" runat="server" Text='<%$ Resources:Controls,InvestorCode%>'
                                                CssClass="bg-none border0 lbl-25-1perc-7-10-sms numeric"></asp:Label>
                                            <%--  <asp:Label ID="POH_INVESTOR" runat="server" Text="" CssClass="input-w24per"></asp:Label>--%>
                                            <asp:TextBox ID="POH_INVESTOR_CODE" runat="server" Enabled="false" MaxLength="50"
                                                TabIndex="14" onkeypress="return this.value.length<50" CssClass="input-w24per"
                                                onpaste="return this.value.length<50"></asp:TextBox>
                                        </div>
                                        <div id="BudgetLink">
                                            <a href="#" class="lbl-bdget-summary" onclick="javascript:ShowBudgetSummary();">Budget Summary</a>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div class="div2col-S">
                                        <label for="PO_CURRENCY">
                                            <%=Resources.Controls.Currency%>
                                            *</label>
                                        <asp:DropDownList runat="server" ID="POH_CURRENCY" TabIndex="12" CssClass="input-uom input-small margnrgt1per"
                                            onchange="javascript:GetExchangeRate()">
                                        </asp:DropDownList>
                                        <span id="POH_CURRENCY_TEXT" style="display: none"></span>
                                        <asp:HiddenField ID="POH_CURRENCY_BC" runat="server" />
                                        <asp:HiddenField ID="POH_EXCHG_RATE" runat="server" />
                                        <asp:HiddenField ID="POH_TOTAL_VALUE_BC" runat="server" />
                                        <%-- <div class="clear">
                                        </div>--%>
                                        <label for="PurType" class="middle-lbl-small-c">
                                            <%=Resources.Controls.PurType%>*</label>
                                        <asp:DropDownList runat="server" ID="POH_PO_CATEGORY" TabIndex="13" CssClass="input-small">
                                        </asp:DropDownList>
                                        <asp:DropDownList runat="server" ID="POH_TYPE" TabIndex="13" CssClass="input-small"
                                            onchange="javascript:SetInitialPortList();">
                                        </asp:DropDownList>
                                        <label for="Required For">
                                            <%=Resources.Controls.Department%>*</label>
                                        <asp:DropDownList runat="server" ID="POH_DEPT" TabIndex="11" CssClass="select-half">
                                        </asp:DropDownList>
                                        <%--  Po Amendment Date --%>
                                        <div id="divAmendDate" style="display: none">
                                            <label for="AmendDate">
                                                <%=Resources.Controls.POAmendDate%>*</label>
                                            <asp:TextBox ID="POH_AMEND_DATE" runat="server" TabIndex="10" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="date-picker input-disabled"></asp:TextBox>
                                        </div>
                                        <div id="divPlantCompany">
                                            <label for="GIH_COMPANY" class="margnrgt3">
                                                <%= GetGlobalResourceObject("Controls", "CompanyPlant").ToString()%>*</label>
                                            <asp:DropDownList ID="POH_COMPANY" runat="server" CssClass="select-half" TabIndex="16">
                                            </asp:DropDownList>
                                        </div>
                                        <%-- End PO Amendment Date--%>
                                        <div id="divRequestedDept">
                                            <label for="lblRequestedDept" class="margnrgt3">
                                                <%=GetGlobalResourceObject("Controls", "RequestedDept")%></label>
                                            <asp:Label ID="lblRequestedDept" runat="server" Text="" CssClass="select-halfsmall"></asp:Label>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div class="clear">
                        </div>
                    </div>
                    <asp:HiddenField ID="PurchaseOrderID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfIsNew" runat="server" Value="1" />
                    <asp:HiddenField ID="PurchaseOrderStatus" runat="server" Value="0" />
                    <asp:HiddenField ID="PurchaseOrderList" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="PurchaseOrderListPostback" runat="server"></asp:HiddenField>
                    <asp:HiddenField ID="hdfAppType" runat="server" />
                    <asp:HiddenField ID="hdfAppSubType" runat="server" />
                    <asp:HiddenField ID="RefID" runat="server" />
                    <asp:HiddenField ID="hdfCurrentDate" runat="server" Value="0" />
                    <%-- For setting Current Date,which is needed for saving amend save--%>
                    <div class="clear">
                    </div>
                    <h1 class="search-colapse-normal">
                        <%=Resources.Captions.VendorDetails%>
                        <img id="imgVendorDtlShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                            alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowVendorDtl();" />
                        <img id="imgVendorDtlHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                            alt="Hide" title="Hide" style="cursor: pointer" onclick="javascript:HideVendorDtl();" />
                    </h1>
                    <div id="divVendorDtl">
                        <div class="div3col-Hi-Lbx">
                            <div id="VendorName" class="title">
                            </div>
                            <div id="divVndDtls" class="div3col-Hi-Lbx  Hi-Lbx-content">
                                <p id="ContactName">
                                </p>
                                <div class="clear">
                                </div>
                                <div class="clear">
                                </div>
                                <p id="TinNo">
                                </p>
                                <div class="clear">
                                </div>
                                <p id="VendorAddressDtls">
                                </p>
                            </div>
                        </div>
                        <div class="div3col-Hi-Lbx">
                            <div class="title">
                                <label for="ShippingAddress" style="width: auto">
                                    <%--width:auto given for resolution issue --%>
                                    <%=Resources.Controls.Location%>
                                    *</label>
                                <asp:DropDownList ID="POH_SHIPPING" runat="server" onchange="javascript:FillDepartmentDetails($(this).val(),'Shipping')"
                                    TabIndex="17" CssClass="select-medium margnbotm0">
                                </asp:DropDownList>
                            </div>
                            <div id="divShippingDtl" class="div3col-Hi-Lbx  Hi-Lbx-content">
                                <p id="ShippingAddress1">
                                </p>
                                <div class="clear">
                                </div>
                                <p id="ShippingAddress2">
                                </p>
                                <div class="clear">
                                </div>
                                <p id="ShippingCity">
                                </p>
                                <div class="clear">
                                </div>
                                <p id="ShippingCountry">
                                </p>
                            </div>
                        </div>
                        <div class="div3col-Hi-Lbx" style="margin-right: 0">
                            <div class="title" runat="server" id="divBillingDpt">
                                <label for="BillingAddress" style="width: auto">
                                    <%--width:auto given for resolution issue --%>
                                    <%=Resources.Controls.BillingDept%>
                                    *</label>
                                <asp:DropDownList ID="POH_BILLING" runat="server" onchange="javascript:FillDepartmentDetails($(this).val(),'Billing')"
                                    TabIndex="18" CssClass="select-medium margnbotm0">
                                </asp:DropDownList>
                            </div>
                            <div class="title" runat="server" id="divBillingVendor">
                                <label for="txtBillVendor" style="width: auto">
                                    <%--width:auto given for resolution issue --%>
                                    <%=Resources.Controls.DeliveryTo%>
                                </label>
                                <asp:TextBox runat="server" ID="txtBillVendor" CssClass="srchboxtextbx request-select"
                                    TabIndex="18">
                                </asp:TextBox>
                                <asp:HiddenField ID="POH_DELIVERY" Value="0" runat="server" />
                            </div>
                            <div id="divBillingDtls" class="div3col-Hi-Lbx  Hi-Lbx-content">
                                <p id="BillingAddress1">
                                </p>
                                <div class="clear">
                                </div>
                                <p id="BillingAddress2">
                                </p>
                                <div class="clear">
                                </div>
                                <p id="BillingCity">
                                </p>
                                <div class="clear">
                                </div>
                                <p id="BillingCountry">
                                </p>
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                        <div class="clear">
                        </div>
                    </div>
                </div>
                <h1 class="search-colapse-normal">
                    <%=Resources.Captions.POItems%>
                    <img id="imgShowPOItems" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                        alt="Show" title="Show" style="display: none; cursor: pointer" onclick="javascript:ShowPoItems();" />
                    <img id="imgHidePOItems" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="Hide"
                        title="Hide" style="cursor: pointer" onclick="javascript:HidePoItems();" />
                </h1>
                <div id="divPOItems">
                    <div class="gridwrap grid-maxw1400">
                        <asp:HiddenField ID="EditMaterial" runat="server" />
                        <%-- <table style="text-align: center ! important;" class="gridwraptable gridwrap">--%>
                        <table class="gridwraptable ">
                            <tr>
                                <th fieldmap="ITM_CODE" align="left" width="16%">
                                    <%=Resources.Controls.MaterialCode%>
                                </th>
                                <th fieldmap="POD_UOM" align="left" width="5%">
                                    <%=Resources.Controls.UOM%>
                                </th>
                                <th fieldmap="POD_RATE" class="txtAlign-right padgrgt8" width="7%">
                                    <%=Resources.Controls.Rate%>
                                </th>
                                <th fieldmap="POD_QTY" class="txtAlign-right padgrgt8" width="7%">
                                    <%=Resources.Controls.Qty%>
                                </th>
                                <th fieldmap="POD_AMOUNT" class="txtAlign-right padgrgt8" width="7%">
                                    <%=Resources.Controls.Amount%>
                                </th>
                                <th fieldmap="POD_DISCOUNT" class="txtAlign-right padgrgt8" width="7%">
                                    <%=Resources.Controls.Discount%>
                                </th>
                                <th fieldmap="POD_TAX" class="txtAlign-right padgrgt8" width="7%">
                                    <%=Resources.Controls.Tax%>
                                </th>
                                <th fieldmap="POD_SUBTOTAL" class="txtAlign-right" width="7%">
                                    <%=Resources.Controls.SubTotal%>
                                </th>
                                <th fieldmap="POD_REMARKS" align="left" width="14%">
                                    <%=Resources.Controls.Comments%>
                                </th>
                                <th fieldmap="POD_REQD_DATE" align="left" width="10%">
                                    <%=Resources.Controls.RequiredBy%>
                                </th>
                                <th style="width: 3%"></th>
                            </tr>
                            <tr class="grd-rowhead">
                                <td>
                                    <%--   <asp:DropDownList ID="ITM_CODE" CssClass="txt-left" runat="server" onchange="javascript:FillMaterialDetails($(this).val())"
                                        Width="98%" TabIndex="19">
                                    </asp:DropDownList>--%>
                                    <asp:TextBox ID="txtItemCode" runat="server" TabIndex="20">
                                    </asp:TextBox>
                                    <asp:HiddenField ID="ITM_CODE" runat="server" Value="0"></asp:HiddenField>
                                </td>
                                <td>
                                    <asp:TextBox ID="POD_UOM" runat="server" CssClass="input-uom-small input-disabled"
                                        Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="POD_RATE" runat="server" CssClass="numeric input-w80 margn-rgt0"
                                        onchange="javascript:CalculateAmount();" MaxLength="15" TabIndex="20"></asp:TextBox>
                                    <asp:HiddenField ID="POD_RATE_PREV" runat="server" Value="0"></asp:HiddenField>
                                </td>
                                <td>
                                    <asp:TextBox ID="POD_QTY" runat="server" CssClass="numeric input-w80 margn-rgt0"
                                        onchange="javascript:CalculateAmount();" MaxLength="11" TabIndex="21"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="POD_AMOUNT" CssClass="numeric input-w80 margn-rgt0 input-disabled"
                                        runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="POD_DISCOUNT" CssClass="numeric input-w80 margn-rgt0 input-disabled"
                                        runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="POD_TAX" CssClass="numeric input-w80 margn-rgt0 input-disabled"
                                        runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="POD_SUBTOTAL" CssClass="numeric input-w80 margn-rgt0 input-disabled"
                                        runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="POD_REMARKS" CssClass="txt-left" runat="server" TextMode="MultiLine"
                                        Height="30px" Width="150px" MaxLength="30" TabIndex="22"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="POD_REQD_DATE" CssClass="date-picker" onkeydown="return CheckKey(event)"
                                        onpaste="return false;" runat="server" TabIndex="23"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:ImageButton runat="server" SkinID="imbaddnew" ID="imbAddItem" OnClientClick="javascript:return AddPODetails();"
                                        TabIndex="24" />
                                    <asp:HiddenField runat="server" ID="PerPiecePrice" Value="0" />
                                    <asp:HiddenField runat="server" ID="POD_CONV_FACT" Value="1" />
                                    <asp:HiddenField runat="server" ID="UOM_PK" Value="0" />
                                    <asp:HiddenField runat="server" ID="leastDate" Value="0" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="poDetails">
                        <%--<asp:HiddenField ID="POH_CURRENCY" runat="server" Value="0" />--%>
                        <asp:HiddenField ID="VendorCurrencyCode" runat="server" Value="" />
                        <div class="gridwrap">
                            <table rules="all" id="grdPODetails" grandtype="GrandGrid" paging="false" editfunction="GridHandler"
                                style="width: 1248px;" editable="true" class="gridwraptable gridwrap tablefixwidth-td">
                                <thead>
                                    <tr>
                                        <th fieldmap="POD_PK" isvisible="false"></th>
                                        <th fieldmap="POD_PO" isvisible="false"></th>
                                        <th fieldmap="POD_ITEM" isvisible="false"></th>
                                        <th fieldmap="POD_RATE_UPDATE" isvisible="false"></th>
                                        <th fieldmap="POD_UOM" isvisible="false"></th>
                                        <th fieldmap="POD_SL_NO" isvisible="false"></th>
                                        <th fieldmap="POD_CONV_FACT" isvisible="false"></th>
                                        <th fieldmap="POD_REASON" isvisible="false"></th>
                                        <th fieldmap="ITM_TEXT" width="268px">
                                            <%=Resources.Controls.ItmDesc%>
                                        </th>
                                        <th fieldmap="POD_REMARKS" width="124px" height="40px">
                                            <%=Resources.Controls.Comments%>
                                        </th>
                                        <th fieldmap="UOM_CODE" width="40px" align="left">
                                            <%=Resources.Controls.UOM%>
                                        </th>
                                        <th fieldmap="POD_RATE" width="85px" align="right">
                                            <%=Resources.Controls.Rate%>
                                        </th>
                                        <th fieldmap="POD_QTY_REQUESTED" width="90px" align="right">
                                            <%=Resources.Controls.Quantity%>
                                        </th>
                                        <th fieldmap="POD_AMOUNT" width="80px" align="right">
                                            <%=Resources.Controls.Amount%>
                                        </th>
                                        <th fieldmap="POD_DISC_AMT" width="100px" align="right">
                                            <%=Resources.Controls.Discount%>
                                        </th>
                                        <th fieldmap="POD_TAX" width="100px" align="right">
                                            <%=Resources.Controls.Tax%>
                                        </th>
                                        <th fieldmap="POD_REQD_DATE" width="90px">
                                            <%=Resources.Controls.ReqdBy%>
                                        </th>
                                        <th fieldmap="POD_AMT_VALUE" width="80px" align="right">
                                            <%=Resources.Controls.SubTotal%>
                                        </th>
                                        <th type="Template" width="70px" align="right">
                                            <div style="margin-left: 4px !important;">
                                                <asp:ImageButton runat="server" ID="imbEdit" SkinID="imbeditgrid" ToolTip="<%$resources:ErpRes,Edit %>"
                                                    Style="/*float: left; */" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'EDIT')"
                                                    TabIndex="26" />
                                                <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$resources:ErpRes,Delete %>"
                                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'DELETE')"
                                                    TabIndex="26" />
                                                <asp:ImageButton runat="server" ID="imbrateHistoryPODetails" SkinID="history" TabIndex="26"
                                                    ToolTip="<%$resources:ErpRes,ShowHistory %>" OnClientClick="javascript:return GridMaterialAction($(this).parents('tr:eq(0)'),'HISTORYPODETAILS')" />
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                    <div id="divCalc">
                        <div class="gridwrap">
                            <table id="tblCalc" class="gridwraptable gridwrap">
                                <tr>
                                    <td style="text-align: right; width: 87.6%;">
                                        <%=Resources.Controls.SubTotal%>
                                    </td>
                                    <td></td>
                                    <td style="text-align: left;">
                                        <asp:TextBox runat="server" ID="POH_SUB_TOTAL" EnableTheming="false" CssClass="numeric input-w70 input-disabled"
                                            Enabled="false" TabIndex="22"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right;">
                                        <%=Resources.Controls.Discounts%>
                                    </td>
                                    <td>
                                        <img onclick="javascript:AddItemHeaderDiscount();" alt="<%=Resources.Controls.Discounts%>"
                                            id="imbHdrDiscount" title="<%=Resources.Controls.Discounts%>" style="cursor: pointer"
                                            src="../Images/Classic/Icons/discount.png" />
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox runat="server" ID="POH_DISC_AMT" Enabled="false" EnableTheming="false"
                                            onkeyup="CalculateTotal();" CssClass="numeric input-w70 input-disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <%=Resources.Controls.OtherCharges%>
                                    </td>
                                    <td>
                                        <img onclick="javascript:AddItemHeaderShipping();" alt="<%=Resources.Controls.OtherCharges%>"
                                            title="<%=Resources.Controls.OtherCharges%>" style="cursor: pointer" src="../Images/Classic/Icons/shipping.png" />
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox runat="server" ID="POH_SHIP_CHARGE" EnableTheming="false" Enabled="false"
                                            ReadOnly="true" onkeyup="CalculateTotal();" CssClass="numeric input-w70 input-disabled"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <%=Resources.Controls.Taxes%>
                                    </td>
                                    <td>
                                        <img onclick="javascript:AddItemHeaderTax();" alt="<%=Resources.Controls.Taxes%>"
                                            id="imbHdrTax" title="<%=Resources.Controls.Taxes%>" style="cursor: pointer"
                                            src="../Images/Classic/Icons/tax.png" />
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:TextBox runat="server" ID="POH_ADD_TAX_AMT" EnableTheming="false" CssClass="numeric input-w70 input-disabled"
                                            onkeyup="CalculateTotal();" Enabled="false"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <%=Resources.Controls.PriceAdjustment%>
                                    </td>
                                    <td></td>
                                    <td style="text-align: left;">
                                        <asp:TextBox runat="server" ID="POH_PRICE_ADJUST" onblur="javascript:CalculateTotal();"
                                            EnableTheming="false" CssClass="numeric input-w70" TabIndex="28"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">
                                        <asp:Label ID="lblGrandTotal" runat="server" Text="<%$Resources:Controls,GrandTotal%>"></asp:Label>
                                        <%--<%=Resources.Controls.GrandTotal%> --%>
                                    </td>
                                    <td></td>
                                    <td style="text-align: left;">
                                        <asp:TextBox runat="server" ID="POH_TOTAL_VALUE" Enabled="false" EnableTheming="false"
                                            CssClass="numeric input-w70 input-disabled" TabIndex="24"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <h1 class="search-colapse-normal">
                    <%=Resources.Controls.TermsAndConditions%>
                    <img id="imbShowTerms" src="../Images/Classic/Icons/arrow-colapse-inactive.png" alt="<%= Resources.Controls.Show%>"
                        title="<%= Resources.Controls.Show%>" style="display: none; cursor: pointer"
                        onclick="javascript:ShowTerms();" />
                    <img id="imbHideTerm" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="<%= Resources.Controls.Hide%>"
                        title="<%= Resources.Controls.Hide%>" style="cursor: pointer" onclick="javascript:HideTerms();" />
                </h1>
                <div id="divTerms">
                    <div class="div3col-val">
                        <label for="VENDOR_TERMS">
                            <%=Resources.Controls.VendorTerms%></label>
                        <asp:DropDownList ID="VENDOR_TERMS" runat="server" onchange="javascript:FillVendorTermsDetails($(this).val());"
                            Width="120px" TabIndex="29">
                        </asp:DropDownList>
                        <asp:ImageButton runat="server" ID="imbVendorTerms" SkinID="btnrefresh" OnClientClick="javascript:return ClearTerms('Vendor')"
                            Width="16px" TabIndex="30" />
                        <div class="clear">
                        </div>
                        <div class="scroll-h150" style="margin: 0">
                            <asp:TextBox runat="server" TextMode="MultiLine" Height="80px" Width="95%" ID="txtVendorTermText"
                                EnableTheming="false"></asp:TextBox>
                            <%-- <asp:Label runat="server" ID="LblPOH_VENDOR_TERMS"></asp:Label>--%>
                        </div>
                        <asp:HiddenField runat="server" ID="POH_VENDOR_TERMS" />
                        <asp:HiddenField ID="POH_VENDOR_TERMS_TEXT" runat="server" />
                    </div>
                    <div class="div3col-val">
                        <label for="GENERAL_TERMS">
                            <%=GetGlobalResourceObject("Controls", "GeneralTerms")%></label>
                        <asp:DropDownList ID="GENERAL_TERMS" runat="server" onchange="javascript:FillGeneralTerms($(this).val());"
                            Width="120px" TabIndex="31">
                        </asp:DropDownList>
                        <asp:ImageButton runat="server" ID="imgbtnclear" SkinID="btnrefresh" OnClientClick="javascript:return ClearTerms('General')"
                            Width="16px" TabIndex="32" />
                        <div class="clear">
                        </div>
                        <div class="scroll-h150" style="margin: 0">
                            <asp:TextBox runat="server" ID="txtTermText" TextMode="MultiLine" Height="80px" Width="95%"
                                EnableViewState="false" EnableTheming="false"></asp:TextBox>
                            <%-- <asp:Label runat="server" ID="LblPOH_TERMS"></asp:Label>--%>
                            <asp:HiddenField ID="POH_TERMS_TEXT" runat="server" />
                            <asp:HiddenField ID="POH_GROUP" runat="server" />
                        </div>
                        <asp:HiddenField runat="server" ID="POH_TERMS" />
                    </div>
                    <div class="div3col-val">
                        <label for="POH_COMMENTS" class="txt-lft" style="margin-bottom: 14px!important;">
                            <%=Resources.Controls.Comments%></label>
                        <div class="clear">
                        </div>
                        <asp:TextBox runat="server" ID="POH_COMMENTS" TextMode="MultiLine" EnableTheming="false"
                            Width="95%" Height="80px" TabIndex="33"></asp:TextBox>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="dvPoCreator" runat="server">
                        <label for="POH_EMPLOYEE" class="lbl-9-8perc">Creator</label>
                        <asp:DropDownList ID="POH_EMPLOYEE" runat="server" Width="120px" TabIndex="29">
                        </asp:DropDownList>
                    </div>
                </div>
                <h1 class="search-colapse-normal" id="h1ShortClose" style="display: none">
                    <%=Resources.Controls.ShortCloseInfo%>
                    <img id="imgShrtCloseShow" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                        alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none; cursor: pointer"
                        onclick="javascript:ShowShortClose();" />
                    <img id="imgShrtCloseHide" src="../Images/Classic/Icons/arrow-colapse-active.png"
                        alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                        onclick="javascript:HideShortClose()" />
                </h1>
                <div id="divShortCloseInfo" class="max-100" style="display: none">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="lblShrtDoneBy" style="width: 130px">
                                        <%=Resources.Controls.DoneBy%>
                                    </label>
                                    <asp:Label ID="lblShrtDoneBy" runat="server" class="lbl-24perc"></asp:Label>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <label for="lblShrtDate" style="width: 130px">
                                        <%=Resources.Controls.Date%>
                                    </label>
                                    <asp:Label ID="lblShrtDate" runat="server" class="lbl-17perc"></asp:Label>
                                    <label for="lblShrtTime" style="width: 130px">
                                        <%=Resources.Controls.Time%>
                                    </label>
                                    <asp:Label ID="lblShrtTime" runat="server" class="lbl-17perc"></asp:Label>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="divcol-S">
                                    <label for="lblShrtRemarks" class="margnrgt3">
                                        <%=Resources.Controls.Remarks%>
                                    </label>
                                    <asp:Label ID="lblShrtRemarks" runat="server" class="input-w81per hauto"></asp:Label>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="lblShrtRefNo">
                                        <%=Resources.Controls.ReferenceNo%>
                                    </label>
                                    <asp:Label ID="lblShrtRefNo" runat="server" class="lbl-24perc"></asp:Label>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                    <div class="clear">
                    </div>
                </div>
                <h1 class="search-colapse-normal">
                    <%=Resources.Controls.AttachmentsAndRemarks%>
                    <img id="imgShowAttachment" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                        alt="<%= Resources.Controls.Show%>" title="<%= Resources.Controls.Show%>" style="display: none; cursor: pointer"
                        onclick="javascript:ShowAttachment();" />
                    <img id="imgHideAttachment" src="../Images/Classic/Icons/arrow-colapse-active.png"
                        alt="<%= Resources.Controls.Hide%>" title="<%= Resources.Controls.Hide%>" style="cursor: pointer"
                        onclick="javascript:HideAttachment();" />
                </h1>
                <div class="divcol-FileuplWrap" id="divAttachment">
                    <label for="aupDocument">
                        <%=Resources.Controls.Attachments%></label>
                    <div id="FileUploader" class="input-file">
                        <asp:FileUpload ID="fupUploader" runat="server" ClientIDMode="Static" size="29" Height="22px"
                            Style="margin-top: 3px" TabIndex="34" />
                        <asp:HiddenField ID="FILELIST" runat="server" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div id="Wofkflowdiv" style="display: none">
                    <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
                    <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfRefID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
                    <asp:HiddenField ID="hdfApplicationPK" runat="server" Value="0" />
                    <asp:HiddenField ID="AppNo" runat="server" />
                    <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
                    <asp:HiddenField runat="server" ID="ActionID" />
                </div>
                <div class="clear">
                </div>
                <%--Vendor--%>
                <div id="divVendor" title="<%=Resources.Captions.VendorList%>">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnContinuePO" runat="server" ToolTip="<%$ Resources:Controls, Continue%>"
                            Text="<%$ Resources:Controls, Continue%>" OnClientClick="javascript:return ChangeWorkFlowDirectPO();" />
                        <%--ContinueDirectPO--%>
                    </div>
                    <div class="content-wrapper">
                        <div class="divcolmiddle-S">
                            <label for="ddlVendors">
                                <%=Resources.Controls.SupplierName%>
                            </label>
                            <asp:DropDownList ID="ddlVendorList" runat="server" EnableViewState="false">
                            </asp:DropDownList>
                            <div id="divReqDepVendorPopup">
                                <label for="ddlIssuingDept" class="margnrgt3">
                                    <%=GetGlobalResourceObject("Controls", "RequestedDept")%>
                                </label>
                                <asp:DropDownList ID="ddlIssuingDept" runat="server">
                                </asp:DropDownList>
                            </div>
                            <div id="divPoCategory">
                                <label for="ddlPOhCategory" class="margnrgt3">
                                    <%=Resources.Controls.PurType%>*
                                </label>
                                <asp:DropDownList ID="ddlPOhCategory" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divItemTax" title="<%=Resources.Captions.LineItemsTax%>">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" ToolTip="<%$resources:ErpRes,Apply %>"
                            Text="<%$resources:ErpRes,Apply %>" OnClientClick="javascript:return SaveApply();" />
                    </div>
                    <div class="content-wrapper">
                        <%-- *********************Checkbox Region Start*******************************************************************--%>
                        <div id="divTaxApplicableAmount" style="display: none">
                            <label for="chkSubTotal" style="width: 130px">
                                <%=Resources.Controls.SubTotal%>
                            </label>
                            <asp:CheckBox ID="chkSubTotal" runat="server" Checked="false" onchange="javascript:SetTaxApplicableAmount();" />
                            <label for="chkDiscount" style="width: 130px">
                                <%=Resources.Controls.Discounts%>
                            </label>
                            <asp:CheckBox ID="chkDiscount" runat="server" Checked="false" onchange="javascript:SetTaxApplicableAmount();" />
                            <label for="chkOtherCharges" style="width: 130px">
                                <%=Resources.Controls.OtherCharges%>
                            </label>
                            <asp:CheckBox ID="chkOtherCharges" runat="server" Checked="true" onchange="javascript:SetTaxApplicableAmount();" />
                        </div>
                        <%--   *******************End Check Box Region ****************************************************************--%>
                        <div class="divcolmiddle-S">
                            <label for="ItemAmount">
                                <%=Resources.Controls.ItemAmount%>
                            </label>
                            <asp:TextBox ID="ItemAmount" runat="server" EnableViewState="false" Enabled="false"></asp:TextBox>
                            <label for="ChooseTax">
                                <%=Resources.Controls.ChooseType%>
                            </label>
                            <asp:DropDownList ID="ChooseTax" runat="server" EnableViewState="false" onchange="javascript:GetFormula($(this).val());">
                            </asp:DropDownList>
                            <label for="ItemAmount">
                                <%=Resources.Controls.TaxName%>
                            </label>
                            <asp:TextBox ID="TaxName" runat="server" EnableViewState="false" MaxLength="100"></asp:TextBox>
                            <div id="divTxRatePer" style="display: none">
                                <label for="txtPercentage" class="margnrgt3">
                                    <%=Resources.Controls.Percentage%>
                                </label>
                                <asp:TextBox ID="txtPercentage" runat="server" onChange="CalcPercentage();" EnableViewState="false"></asp:TextBox>
                            </div>
                            <label for="TaxAmount">
                                <%=Resources.Controls.Amount%>
                            </label>
                            <asp:TextBox ID="TaxAmount" runat="server" EnableViewState="false" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="imbTaxDiscountSave" SkinID="imbaddnew" runat="server" TabIndex="15"
                                EnableViewState="False" OnClientClick="javascript:return AddTaxDiscount();" />
                        </div>
                        <asp:HiddenField runat="server" ID="IsLine" Value="1" />
                        <asp:HiddenField runat="server" ID="IsLineDiscount" Value="1" />
                        <%-- <div class="present-divcol-S">--%>
                        <%-- <div class="clear">
                     
                        <%-- <asp:Button ID="btnTaxDiscountSave" runat="server" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                            TabIndex="15" EnableViewState="False" OnClientClick="javascript:return SaveTaxDiscount();" />--%>
                        <%-- </div>--%>
                        <div>
                            <table rules="all" id="grdTaxDetails" grandtype="GrandGrid" paging="false" width="100%"
                                editfunction="GridHandler" editable="true" class="gridwraptable gridwrap">
                                <thead>
                                    <tr>
                                        <th fieldmap="POT_SL_NO" isvisible="false"></th>
                                        <th fieldmap="POT_PK" isvisible="false"></th>
                                        <th fieldmap="POT_TAX" isvisible="false"></th>
                                        <th fieldmap="POT_TYPE" isvisible="false"></th>
                                        <th fieldmap="POT_NAME" width="38%">
                                            <%=Resources.Controls.Name%>
                                        </th>
                                        <th fieldmap="POT_TAX_TEXT" width="38%">
                                            <%=Resources.Controls.Type%>
                                        </th>
                                        <th fieldmap="POT_TAX_AMT" width="20%">
                                            <%=Resources.Controls.Amount%>
                                        </th>
                                        <th type="Template" width="4%">
                                            <div>
                                                <%--<asp:ImageButton runat="server" ID="imbTaxEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'TAXEDIT')" />--%>
                                                <asp:ImageButton runat="server" ID="imbTaxDelete" ToolTip="<%$resources:ErpRes,Delete %>"
                                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'TAXDELETE')" />
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
                <div id="divRevisionHistory" title="<%=Resources.Captions.RevisionHistory%>">
                    <div class="content-wrapper">
                        <table rules="all" id="grdRevisionHistory" grandtype="GrandGrid" paging="false" editable="false"
                            class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="POH_PK" isvisible="false"></th>
                                    <th fieldmap="POH_VERSION" isvisible="false"></th>
                                    <th fieldmap="POH_DATE" align="left" width="30%">
                                        <%=Resources.Controls.RevDate%>
                                    </th>
                                    <th fieldmap="POH_NO" align="left" width="32%">
                                        <%=Resources.Controls.PoNumber%>
                                    </th>
                                    <th fieldmap="POH_CURRENCY_TEXT" align="left" width="10%">
                                        <%=Resources.Controls.Currency%>
                                    </th>
                                    <th fieldmap="POH_TOTAL_VALUE" align="right" width="28%">
                                        <%=Resources.Controls.TotalAmount%>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
                <div id="divItemRate" title="<%=Resources.Captions.LineItemsRate%>">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnRateApply" runat="server" SkinID="btnInner-Save" ToolTip="<%$resources:ErpRes,Apply %>"
                            Text="<%$resources:ErpRes,Apply %>" OnClientClick="javascript:return SaveRateApply();" />
                        <asp:Button ID="btnRateCancel" runat="server" SkinID="btnInner-Cancel" ToolTip="<%$resources:ErpRes,Cancel %>"
                            Text="<%$resources:ErpRes,Cancel %>" OnClientClick="javascript:return SaveRateCancel();" />
                    </div>
                    <div class="div2col-S">
                        <label for="VendorName" style="width: 130px">
                            <%=Resources.Controls.VendorName%>
                        </label>
                        <asp:TextBox ID="VendorNameText" runat="server" EnableViewState="false" Enabled="false"
                            Width="250px"></asp:TextBox>
                        <div class="clear">
                        </div>
                        <label for="ItemName" style="width: 130px">
                            <%=Resources.Controls.Item%>
                        </label>
                        <asp:TextBox ID="ItemName" runat="server" EnableViewState="false" Enabled="false"
                            Width="250px"></asp:TextBox>
                        <div class="clear">
                        </div>
                        <label for="ItemQty" style="width: 130px">
                            <%=Resources.Controls.Qty%>
                        </label>
                        <asp:TextBox ID="ItemQty" runat="server" EnableViewState="false" Enabled="false"
                            Width="250px"></asp:TextBox>
                        <div class="clear">
                        </div>
                        <label for="TaxRate" style="width: 130px">
                            <%=Resources.Controls.Rate%>
                        </label>
                        <asp:TextBox ID="TaxRate" runat="server" MaxLength="15" Width="250px"></asp:TextBox>
                        <div class="clear">
                        </div>
                        <%--  RateChangeReason Start --%>
                        <label for="RateChangeReason" style="width: 130px">
                            <%=Resources.Controls.ReasonForChange%>
                        </label>
                        <asp:TextBox ID="txtRateChangeReason" CssClass="txt-left" runat="server" TextMode="MultiLine"
                            Height="30px" Width="250px" onkeydown="limitText(this,500);" onchange="limitText(this,500);"></asp:TextBox>
                        <%--  RateChangeReason End --%>
                        <label for="chkIsUpdateRate" style="width: 130px">
                            <%=Resources.Controls.UpdateVendorRate%>
                        </label>
                        <asp:CheckBox ID="chkIsUpdateRate" runat="server" Checked="true" />
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="clear">
                    </div>
                    <%--   <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
                        text-align: right">
                    </div>--%>
                </div>
                <div id="divItemDiscount" title="<%=Resources.Captions.LineItemsDiscount%>">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnDiscApply" runat="server" ToolTip="<%$resources:ErpRes,Apply %>"
                            Text="<%$resources:ErpRes,Apply %>" SkinID="btnInner-Save" OnClientClick="javascript:return SaveDiscApply();" />
                        <asp:Button ID="btnDiscCancel" runat="server" ToolTip="<%$resources:ErpRes,Cancel %>"
                            Text="<%$resources:ErpRes,Cancel %>" SkinID="btnInner-Cancel" OnClientClick="javascript:return SaveDiscCancel();" />
                    </div>
                    <div class="div2col-S">
                        <label for="ItemAmount" style="width: 130px">
                            <%=Resources.Controls.ItemAmount%>
                        </label>
                        <asp:TextBox ID="ItemDiscAmount" runat="server" EnableViewState="false" Width="250px"
                            Enabled="false"></asp:TextBox>
                        <div class="clear">
                        </div>
                        <label for="TaxAmount" style="width: 130px">
                            <%=Resources.Controls.Amount%>
                        </label>
                        <asp:TextBox ID="TaxDiscAmount" runat="server" MaxLength="15" Width="250px"></asp:TextBox>
                        <div class="clear">
                        </div>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="clear">
                    </div>
                    <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right; text-align: right">
                    </div>
                </div>
                <div class="clear">
                </div>
                <div id="divPurchaseRequest" title="<%=Resources.Captions.PurchaseRequestDetails%>">
                    <div class="content-wrapper">
                        <table rules="all" id="grdSelectedPurchaseRequest" grandtype="GrandGrid" paging="false"
                            width="100%" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="PRH_PK" isvisible="false"></th>
                                    <th fieldmap="PRH_NO" width="18%">
                                        <%=Resources.Controls.PRNo%>
                                    </th>
                                    <th fieldmap="PRD_QTY_APPROVED" align="right" width="25%">
                                        <%=Resources.Controls.RequestQuantity%>
                                    </th>
                                    <th fieldmap="PRD_REQD_DATE" width="20%" align="center">
                                        <%=Resources.Controls.RequestDate%>
                                    </th>
                                    <th fieldmap="PRD_UOM_TEXT" align="left" width="7%">
                                        <%=Resources.Controls.UOM%>
                                    </th>
                                    <th fieldmap="POR_COST_CENTER_TEXT" align="left" width="15%">
                                        <%=Resources.Controls.CostCenter%>
                                    </th>
                                    <th fieldmap="POR_QTY_ORDERED" align="left" width="15%">
                                        <%=Resources.Controls.PoQty%>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
                <div class="clear">
                </div>
                <div id="divData">
                </div>
                <div id="divFileData">
                </div>
            </div>
        </div>
        <div id="divRateHistory" title="<%=Resources.Captions.LineItemsTax%>" style="display: none">
            <div class="content-wrapper">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-P">
                                <label for="lblItemCode">
                                    <%=Resources.Controls.ItemCode%></label>
                                <asp:Label ID="lblItemCode" runat="server" EnableViewState="false"></asp:Label>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-P">
                                <label for="lblItemName">
                                    <%=Resources.Controls.ItemName%>
                                </label>
                                <asp:Label ID="lblItemName" runat="server" EnableViewState="false"></asp:Label>
                            </div>
                        </td>
                    </tr>
                </table>
                <div>
                    <table id="grdRateDetails" grandtype="GrandGrid" rules="all" paging="false" width="100%"
                        class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="VIH_VENDOR_TEXT" width="25%">
                                    <%=Resources.Controls.Vendor%>
                                </th>
                                <%--<th fieldmap="VIH_RATING" width="9%">
                                    <%=Resources.Controls.Rating%>
                                </th>--%>
                                <th fieldmap="VIH_LAST_QUOT_DATE" width="12%">
                                    <%=Resources.Controls.LastQuotedDate%>
                                </th>
                                <th fieldmap="VIH_LAST_QUOT_RATE" width="10%" align="right">
                                    <%=Resources.Controls.LastQuotedRate%>
                                </th>
                                <%-- <th fieldmap="VIH_LEAD_TIME" width="10%" align="right">
                                    <%=Resources.Controls.LeadDays%>
                                </th>--%>
                                <th fieldmap="VEN_CURRENCY_TEXT" width="10%">
                                    <%=Resources.Controls.Currency%>
                                </th>
                                <th fieldmap="VIH_LAST_ORDR_RATE" width="11%" align="right">
                                    <%=Resources.Controls.LastOrderRate%>
                                </th>
                                <th fieldmap="VIH_LAST_ORDR_QTY" width="11%" align="right">
                                    <%=Resources.Controls.LastOrderQty%>
                                </th>
                                <th fieldmap="VIH_LEAD_TIME" width="9%" align="right">
                                    <%=Resources.Controls.LeadDays%>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
        </div>

        <div id="divBudgetHistory" title="<%=Resources.Captions.LineItemsTax%>" style="display: none">
            <div class="content-wrapper">
                <div>
                    <table id="grdBudgetDetails" grandtype="GrandGrid" rules="all" paging="false" width="100%"
                        class="gridwraptable gridwrap">
                        <thead>
                            <tr>
                                <th fieldmap="VIH_VENDOR_TEXT" width="25%">
                                    <%=Resources.Controls.AccountCode%>
                                </th>
                                <th fieldmap="VIH_LAST_QUOT_DATE" width="12%">
                                    <%=Resources.Controls.Category%>
                                </th>
                                <th fieldmap="VIH_LAST_QUOT_RATE" width="10%" align="right">
                                    <%=Resources.Controls.Budget%>
                                </th>
                                <th fieldmap="VEN_CURRENCY_TEXT" width="10%">
                                    <%=Resources.Controls.RemainBalance%>
                                </th>
                            </tr>
                        </thead>
                    </table>
                </div>
            </div>
        </div>
        <%--      PO Qty Amend Strat --------------------------------------------%>
        <div id="divAmendPRDetails" title="<%=Resources.Captions.AmendPRQty%>" style="display: none">
            <div class="Button-container-popup">
                <asp:Button ID="btnSaveAmendPR" SkinID="btnInner-add-dsd" runat="server" ToolTip="<%$resources:ErpRes,Apply %>"
                    Text="<%$resources:ErpRes,Apply %>" OnClientClick="javascript:return UpdateAmendPOPRDetails();" />
            </div>
            <div class="content-wrapper">
                <table rules="all" id="grdAmendPRDetails" grandtype="GrandGrid" paging="false" editable="false"
                    class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="POR_PK" isvisible="false"></th>
                            <th fieldmap="POR_PR_DTL" isvisible="false"></th>
                            <th fieldmap="POR_PO_DTL" isvisible="false"></th>
                            <th fieldmap="POR_ITEM" isvisible="false"></th>
                            <th fieldmap="POR_UOM" isvisible="false"></th>
                            <th fieldmap="PRH_NO" align="left" sortable="true" width="100px">
                                <%=Resources.Controls.PRNo%>
                            </th>
                            <th fieldmap="ITM_TEXT" isvisible="false"></th>
                            <th fieldmap="ITM_CODE" align="left" sortable="true" width="110px">
                                <%=Resources.Controls.ItemCode%>
                            </th>
                            <th fieldmap="ITM_NAME" align="left" sortable="true" width="210px">
                                <%=Resources.Controls.Description%>
                            </th>
                            <th fieldmap="PRD_QTY_APPROVED" isvisible="false">
                                <%=Resources.Controls.ReQty%>
                            </th>
                            <th fieldmap="PRD_SORT_DATE" isvisible="false" sortable="true"></th>
                            <th fieldmap="PRD_DATE" align="left" sortable="true" width="70px" isvisible="false">
                                <%=Resources.Controls.PRDate%>
                            </th>
                            <th fieldmap="PRD_QTY_BALANCE" width="65px" sortable="true" align="right" isvisible="false">
                                <%=Resources.Controls.Qty%>
                            </th>
                            <th fieldmap="POR_UOM_TEXT" align="left" width="50px">
                                <%=Resources.Controls.MaterialUOM%>
                            </th>
                            <th fieldmap="POR_QTY_ORDERED" align="right" width="75px">
                                <%=Resources.Controls.PoQty%>
                            </th>
                            <th fieldmap="POR_QTY_ADDITIONAL" align="right" width="75px">
                                <%=Resources.Controls.AdlQty%>
                            </th>
                            <%-- <th fieldmap="ITV_PRICE" align="right" width="75px">
                                            <%=Resources.Controls.Price%>
                                        </th>    --%>
                            <th fieldmap="PRD_ITEM_SPEC" align="left" width="150px" isvisible="false">
                                <%=Resources.Controls.Spec%>
                            </th>
                            <th fieldmap="IO_NO" isvisible="false"></th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
        <asp:HiddenField ID="hdfShowTransactionPort" runat="server" Value="0" />
        <%--     End PO Qty Amend ---------------------------------------%>
        <asp:HiddenField runat="server" ID="hdfCurrncyChangeConfirm" Value="0" />
        <asp:HiddenField runat="server" ID="isPOEditable" />
        <asp:HiddenField runat="server" ID="IS_RATE_UPDATE" />
        <asp:HiddenField runat="server" ID="isTaxAdd" Value="0" />
        <asp:HiddenField runat="server" ID="isDiscountAdd" Value="0" />
        <asp:HiddenField runat="server" ID="poURL" />
        <asp:HiddenField runat="server" ID="isEditMode" />
        <asp:HiddenField runat="server" ID="isMoreEditMode" />
        <asp:HiddenField runat="server" ID="hdnSlNo" />
        <asp:HiddenField ID="AutoStartValue" runat="server" Value="0" />
        <asp:HiddenField runat="server" ID="IsTaxForOtherCharge" Value="0" />
        <asp:HiddenField runat="server" ID="hdfEnableDirectPO" Value="0" />
        <asp:HiddenField runat="server" ID="hdnIsTaxNotDue" Value="0" />
        <asp:HiddenField ID="hdfShowDescPM" Value="0" runat="server" />
        <asp:HiddenField ID="hdfIsPRFromInbox" Value="0" runat="server" />
        <%--Comma Separation for Quantity & Amount Based on Configuration(Table)--%>
        <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
        <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
        <asp:HiddenField ID="hdfIsShowAlert" Value="0" runat="server" />
        <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsWkfSettingPostBackReqd" runat="server" Value="0" />
        <asp:HiddenField ID="hdfPOCatWkfChange" runat="server" Value="0" />
        <asp:HiddenField ID="POH_ISSUE_DEPT" runat="server" Value="0" />
        <asp:HiddenField ID="hdfPOCategoryVal" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIssueDeptText" runat="server" Value="" />
        <asp:HiddenField ID="hdfReqPOList" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsReqDeptPostback" runat="server" Value="0" />
        <asp:HiddenField ID="hdfStoreBeforePostback" runat="server" Value="0" />
        <asp:HiddenField ID="hdfSelectedVendorPK" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsPostbackDirectPO" runat="server" Value="0" />
        <asp:HiddenField ID="hdfRestrictEditOption" runat="server" Value="0" />
        <asp:HiddenField ID="hdfVendorCurrency" runat="server" Value="0" />
        <asp:HiddenField ID="hdfDefaultPOCategory" runat="server" Value="0" />
        <asp:HiddenField ID="POH_VERIFIED" runat="server" Value="0" />
        <asp:HiddenField ID="POH_REQ_BUDG_VALID" runat="server" Value="0" />

        <asp:HiddenField ID="IsShowAmendBt" runat="server" Value="" />

        <asp:HiddenField ID="POH_CONVERTED" runat="server" Value="" />
        <asp:HiddenField ID="hdfBackUrl" runat="server" Value=""></asp:HiddenField>
        <asp:HiddenField ID="hdfVerificationRequired" runat="server" Value="0" />
        <asp:HiddenField ID="hdfRateHistoryGridHide" runat="server" Value="0" />
        <asp:HiddenField ID="hdfPRtypeEnabled" runat="server" Value="0" />
        <asp:HiddenField ID="hdfPRGroupEnabled" runat="server" Value="0" />
        <asp:HiddenField ID="hdfPoCommentPrefix" runat="server" Value="0" />
        <asp:HiddenField ID="hdfShowPOAmendAfterInvoice" runat="server" Value="0" />
        <asp:HiddenField ID="hdfEnablePOCountryValidation" runat="server" Value="false" />
        <asp:HiddenField ID="hdfEnablePOCurrencyValidation" runat="server" Value="false" />
        <asp:HiddenField ID="hdfSBUCountry" runat="server" />
        <asp:HiddenField ID="hdfVendorCountry" runat="server" />
        <%--  For New Workflow SP--%>
        <asp:HiddenField runat="server" ID="WKF_REFERENCE" Value="0" />
        <asp:HiddenField runat="server" ID="WKF_APPLICATION" Value="0" />
        <asp:HiddenField runat="server" ID="WKF_PROCESS" Value="0" />
        <asp:HiddenField runat="server" ID="WKF_TASK" Value="0" />
        <asp:HiddenField runat="server" ID="WKF_TASK_ACTION" Value="0" />
        <asp:HiddenField runat="server" ID="WKF_COMMENTS" Value="" />
        <asp:HiddenField runat="server" ID="WKF_TRX_FLAG" Value="0" />
        <asp:HiddenField runat="server" ID="USER_PK" Value="" />
        <asp:HiddenField runat="server" ID="hdfEnableGlovePR" Value="" />
        <asp:HiddenField runat="server" ID="POH_IS_GLOVE" Value="" />
        <%-- <asp:HiddenField runat="server" ID="hdfItemPoType" Value="=" />--%>
        <asp:HiddenField runat="server" ID="hdfIsValidateCurrencyPoType" Value="0" />
        <asp:HiddenField ID="hdfEnbleCostCenter" runat="server" Value="0" />
        <asp:HiddenField ID="hdfShowPoOtherVendor" runat="server" Value="0" />
        <asp:HiddenField ID="hdfShowInvestor" runat="server" Value="0" />
        <asp:HiddenField ID="hdnBudgetValidationReq" runat="server" Value="<%$ Resources:ConfigurationsRes, BudgetValidationRequired%>" />
        <asp:HiddenField ID="hdfInvestorCode" runat="server" Value="" />
        <asp:HiddenField ID="POH_INVESTOR" runat="server" Value="" />
        <asp:HiddenField runat="server" ID="POH_MENU_TYPE" Value="1" />
        <asp:HiddenField runat="server" ID="hdfPohEmployee" Value="0" />
        <asp:HiddenField runat="server" ID="hdfZeroRateConfirm" Value="0" />
        <%--  End For New Workflow SP--%>
        <div id="divCommentPopup" title="<%=Resources.Controls.Comments%>" style="display: none">
            <div class="Button-container-popup" id="btnContainer">
                <asp:Button runat="server" ID="btnPopupCommentSave" SkinID="btnInner-Save" Text="Save"
                    OnClientClick="return savePopupComment();" />
            </div>
            <div class="content-wrapper">
                <asp:TextBox ID="txtPopupComment" CssClass="txt-left" runat="server" TextMode="MultiLine"
                    Height="50px" Width="300px" MaxLength="30"></asp:TextBox>
                <asp:HiddenField ID="hdfPopupCommentSlNo" runat="server" />
            </div>
        </div>
        <div style="display: none">
            <asp:Button ID="btnDummyAmend" runat="server" CommandName="AMEND" OnClick="ActionHandler" />
        </div>
        <div style="display: none">
            <asp:Button ID="btnWorkFlowDirectPO" runat="server" OnClick="ActionHandler" CommandName="REQDEPTCHANGE" />
        </div>
    </div>
</asp:Content>
