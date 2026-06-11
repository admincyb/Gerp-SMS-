<%@ Page Title="<%$ Resources:Captions,Title_StockTransfer %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    Theme="ClassicExt" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="StockTransfer.aspx.cs"
    Inherits="ERPSMS_v01.StoreManagement.StockTransfer" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/StockTransfer.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <div id="webwizard-wrap">
        <h1> 
            <%=Resources.Captions.StockTransfer%>
        </h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton runat="server" ID="imbSave" SkinID="btnsave" TabIndex="11" EnableViewState="False"
                OnClientClick="javascript:return SavePage('Draft');" />
            <asp:ImageButton runat="server" ID="imbReset" SkinID="btnreset" TabIndex="12" EnableViewState="False"
                OnClientClick="javascript:return CancelPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" EnableViewState="False"
                TabIndex="13" OnClientClick="javascript:return CancelPage();" />
        </div>
    </div>--%>
    <asp:HiddenField ID="ConfirmStockValueChange" runat="server" Value="0" />
    <asp:HiddenField runat="server" ID="GinPk" Value='0' />
    <asp:HiddenField ID="hdfIsContFutureDate" Value="0" runat="server" />
    <asp:HiddenField ID="hdfCurrentDate" runat="server" />
     <asp:HiddenField ID="hdfIsDuplicateLotNo" Value="0" runat="server" />
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
                                <asp:Button runat="server" ID="btnSubmit" SkinID="btnInner-submit" Text="<%$Resources:Controls,Submit%>"
                                    TabIndex="34" EnableViewState="False" ToolTip="<%$resources:ErpRes,Submit %>"
                                    OnClientClick="javascript:return  WkfSubmit();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="34" EnableViewState="False" ToolTip="<%$resources:ErpRes,Save %>" OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <%-- <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="35" EnableViewState="False" ToolTip="<%$resources:ErpRes,Reset %>"
                                    OnClientClick="javascript:return ResetPage();" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnCancelPage" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" ToolTip="<%$resources:ErpRes,Cancel %>" OnClientClick="javascript:return CancelPage();"
                                    TabIndex="36" />
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
            <table class="table-devide" id="tblDetailHdr">
                <tr>
                    <td>
                        <div class="div2col-S">
                            <label for="SFH_NO">
                                <%=Resources.Controls.StockTransferNo%>
                            </label>
                            <asp:Label ID="SFH_NO" runat="server" CssClass="input-small"></asp:Label>
                            <label for="SFH_DATE" class="middle-lbl">
                                <%=Resources.Controls.Date%>
                            </label>
                            <asp:TextBox ID="SFH_DATE" onkeydown="return CheckKey(event)" TabIndex="1" onpaste="return false;"
                                runat="server" CssClass="date-picker"></asp:TextBox>
                            <asp:HiddenField ID="APT_CODE" runat="server" />
                            <asp:HiddenField ID="WKF_FLAG" runat="server" Value="0" />
                             <asp:HiddenField ID="WKF_PROCESS" runat="server" Value="" />
                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                            <div id="divPlantCompany">
                                <label for="SFH_COMPANY">
                                     <%=GetGlobalResourceObject("Controls","CompanyPlant")%>*</label>
                                <asp:DropDownList ID="SFH_COMPANY" runat="server" TabIndex="16" ClientIDMode="Static" CssClass="select-halfsmall margnlft-minus4">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                            <label for="SFH_DEPT">
                                <%=Resources.Controls.TransferFrom%>
                            </label>
                            <asp:DropDownList ID="SFH_DEPT" runat="server" TabIndex="2" onchange="javascript:BindInspecteditemForTransferingGrid();" CssClass="select-medium-a">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="clear">
            </div>
            <%--Start Select inspected items for transferringAdd--%>
            <div>
                <%--    <div style="float: right">
                    <img id="imgGINHide" src="../Images/ERP-Blue/Buttons/arrow-dwn.png" alt="<%= Resources.Controls.Show%>"
                        title="<%=Resources.Controls.Show%>" style="display: none; cursor: pointer" onclick="javascript:ShowDetails(1);" />
                    <img id="imgGINShow" src="../Images/ERP-Blue/Buttons/arrow-up.png" alt="<%= Resources.Controls.Hide%>"
                        title="<%= Resources.Controls.Hide%>" style="cursor: pointer" onclick="javascript:HideDetails(1);" />
                </div>--%>
                <h1 id="h1" class="search-colapse-normal">
                    <%=Resources.Controls.SelectInspectedItemsForTransferring%>
                    <img id="imgGINHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png" alt="<%= Resources.Controls.Show%>"
                        title="<%=Resources.Controls.Show%>" style="display: none; cursor: pointer" onclick="javascript:ShowDetails(1);" />
                    <img id="imgGINShow" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="<%= Resources.Controls.Hide%>"
                        title="<%= Resources.Controls.Hide%>" style="cursor: pointer" onclick="javascript:HideDetails(1);" />
                </h1>
                <div id="divGIN">
                    <div class="scroll-h150">
                        <div class="gridwrap">
                            <table rules="all" id="grdGIN" style="width: 100%" grandtype="GrandGrid" enablecheckbox="true"
                                paging="false" class="gridwraptable gridwrap">
                                <thead>
                                    <tr>
                                        <th fieldmap="GID_PK" isvisible="false">
                                        </th>
                                        <th fieldmap="GID_ITEM" isvisible="false">
                                        </th>
                                         <th fieldmap="POD_RATE" isvisible="false">
                                        </th>
                                        <th fieldmap="GIH_NO" align="left" width="10%">
                                            <%=Resources.Controls.GINNumber%>
                                        </th>
                                        <th fieldmap="GIH_DATE" align="left" width="10%">
                                            <%=Resources.Controls.InspectionDt%>
                                        </th>
                                        <th fieldmap="GID_ITEM_TEXT" align="left" width="50%">
                                            <%=Resources.Controls.Item%>
                                        </th>
                                        <th fieldmap="GID_UOM_TEXT" align="left" width="5%">
                                            <%=Resources.Controls.UOM%>
                                        </th>
                                        <th fieldmap="GRD_QA_LOT_NO" align="left" width="15%">
                                            <%=Resources.Controls.QALotNo%>
                                        </th>
                                        <th fieldmap="GID_QTY_APPROVED" align="right" width="10%">
                                            <%=Resources.Controls.AcceptedQty%>
                                        </th>
                                        <th fieldmap="GIH_COMPANY" isvisible="false">
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                    <div class="button-wrap-right">
                        <asp:Button ID="AddToList" runat="server" Text="<%$ Resources:Controls, AddSelectedItemtoList%>"
                            TabIndex="8" ToolTip="<%$resources:Controls,AddSelectedItemtoList %>" OnClientClick="javascript:return AddPOItemToList();" />
                    </div>
                </div>
            </div>
            <%--End Select inspected items for transferringAdd--%>
            <div class="clear">
            </div>
            <%--Start Allocate transfer Qty. based on POs--%>
            <div>
                <%--  <div style="float: right">
                    <img id="imgPOHide" src="../Images/ERP-Blue/Buttons/arrow-dwn.png" alt="<%= Resources.Controls.Show%>"
                        title="<%=Resources.Controls.Show%>" style="cursor: pointer" onclick="javascript:ShowDetails(2);" />
                    <img id="imgPOShow" src="../Images/ERP-Blue/Buttons/arrow-up.png" alt="<%= Resources.Controls.Hide%>"
                        title="<%= Resources.Controls.Hide%>" style="display: none; cursor: pointer"
                        onclick="javascript:HideDetails(2);" />
                </div>--%>
                <h1 id="h2" class="search-colapse-normal">
                    <%=Resources.Controls.AllocateTransferQtyBasedOnPOs%>
                    <img id="imgPOHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png" alt="<%= Resources.Controls.Show%>"
                        title="<%=Resources.Controls.Show%>" style="display: none; cursor: pointer" onclick="javascript:ShowDetails(2);" />
                    <img id="imgPOShow" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="<%= Resources.Controls.Hide%>"
                        title="<%= Resources.Controls.Hide%>" style="cursor: pointer" onclick="javascript:HideDetails(2);" />
                </h1>
                <div id="divPO" class="scroll-h150">
                    <div class="grdTable">
                        <table id="grdPO" style="width: 100%" paging="false" grandtype="GrandGrid" rules="all"
                            enablecheckbox="true" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="GID_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="POD_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="POR_ITEM" isvisible="false">
                                    </th>
                                    <th fieldmap="POR_UOM" isvisible="false">
                                    </th>
                                    <th fieldmap="GID_QTY_APPROVED" isvisible="false">
                                    </th>
                                    <th fieldmap="POH_NO" align="left" width="10%">
                                        <%=Resources.Controls.PONo%>
                                    </th>
                                    <th fieldmap="POR_ITEM_NAME" align="left" width="33%">
                                        <%=Resources.Controls.Item%>
                                    </th>
                                    <th fieldmap="POR_UOM_NAME" align="left" width="3%">
                                        <%=Resources.Controls.UOM%>
                                    </th>
                                    <th fieldmap="QtyOrdered" align="right" width="8%">
                                        <%=Resources.Controls.PRQty%>
                                    </th>
                                    <th fieldmap="QtyAdditional" align="right" width="8%">
                                        <%=Resources.Controls.AddnlQty%>
                                    </th>
                                    <th fieldmap="BAL_PO_QTY" align="right" width="8%">
                                        <%=Resources.Controls.BalancePOQty%>
                                    </th>
                                    <th fieldmap="BAL_PO_ADDL_QTY" align="right" width="10%">
                                        <%=Resources.Controls.BalanceAdnlQty%>
                                    </th>
                                    <th fieldmap="ALLOCATE_PO_QTY" align="right" width="10%">
                                        <%=Resources.Controls.AllocatePOQty%>
                                    </th>
                                    <th fieldmap="ALLOCATE_PO_ADDL_QTY" align="right" width="10%">
                                        <%=Resources.Controls.AllocateAdnlQty%>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                    <div class="button-wrap-right">
                        <asp:Button ID="AddPRDtlsToList" runat="server" Text="<%$ Resources:Controls, AddSelectedItemtoList%>"
                            TabIndex="8" ToolTip="<%$resources:Controls,AddSelectedItemtoList %>" OnClientClick="javascript:return AddPRItemToList();" />
                    </div>
                </div>
            </div>
            <%--End Allocate transfer Qty. based on POs--%>
            <div class="clear">
            </div>
            <%--Start Allocate transfer Qty. based on POs--%>
            <div>
                <%--  <div style="float: right">
                    <img id="imgPRHide" src="../Images/ERP-Blue/Buttons/arrow-dwn.png" alt="<%= Resources.Controls.Show%>"
                        title="<%= Resources.Controls.Show%>" style="cursor: pointer" onclick="javascript:ShowDetails(3);" />
                    <img id="imgPRShow" src="../Images/ERP-Blue/Buttons/arrow-up.png" alt="<%= Resources.Controls.Hide%>"
                        title="<%= Resources.Controls.Hide%>" style="display: none; cursor: pointer"
                        onclick="javascript:HideDetails(3);" />
                </div>
                <h1 id="hPR">
                    <%=Resources.Controls.AllocateTransferQtyBasedonPRs%>
                </h1>--%>
                <h1 id="hPR" class="search-colapse-normal">
                    <%=Resources.Controls.AllocateTransferQtyBasedonPRs%>
                    <img id="imgPRHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png" alt="<%= Resources.Controls.Show%>"
                        title="<%=Resources.Controls.Show%>" style="display: none; cursor: pointer" onclick="javascript:ShowDetails(3);" />
                    <img id="imgPRShow" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="<%= Resources.Controls.Hide%>"
                        title="<%= Resources.Controls.Hide%>" style="cursor: pointer" onclick="javascript:HideDetails(3);" />
                </h1>
                <div class="scroll-h150" id="divPR">
                    <div class="gridwrap">
                    <asp:HiddenField ID="hdfLocationPK" runat="server" />
                        <table id="grdPR" style="width: 100%" paging="false" grandtype="GrandGrid" rules="all"
                            class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="GID_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="PRD_DEPT" isvisible="false">
                                    </th>
                                    <th fieldmap="POD_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="PRH_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="POR_ITEM" isvisible="false">
                                    </th>
                                    <th fieldmap="POR_UOM" isvisible="false">
                                    </th>
                                    <th fieldmap="SFD_PK" isvisible="false">
                                    </th>
                                    <th fieldmap="POH_NO" align="left" width="10%">
                                        <%=Resources.Controls.PONo%>
                                    </th>
                                    <th fieldmap="PRH_NO" align="left" width="10%">
                                        <%--PR#--%>
                                        <%=Resources.Controls.PRNumber%>
                                    </th>
                                    <th fieldmap="POR_ITEM_CODE" align="left" width="18%">
                                        <%=Resources.Controls.ItemCode%>
                                    </th>
                                    <th fieldmap="POR_UOM_NAME" align="left" width="3%">
                                        <%=Resources.Controls.UOM%>
                                    </th>
                                    <th fieldmap="POR_QTY_ORDERED" align="right" width="8%">
                                        <%=Resources.Controls.POQuantity%>
                                    </th>
                                    <th fieldmap="POR_QTY_BAL" align="right" width="8%">
                                        <%=Resources.Controls.Balance%>
                                    </th>
                                    <th fieldmap="ALLOCATE_PR_QTY" align="right" width="8%">
                                        <%=Resources.Controls.Allocation%>
                                    </th>
                                    <%-- *************  QALotNo**************************** --%>
                                    <th fieldmap="SFD_QA_LOT_NO" align="left" width="10%">
                                        <%=Resources.Controls.QALotNo%>
                                    </th>
                                    <%--  **************END*************************--%>
                                    <th fieldmap="SFD_LOCATION" align="left" width="10%">
                                        <%=Resources.Controls.Location%>
                                    </th>
                                    <th fieldmap="PRD_DEPT_NAME" align="left" width="15%">
                                        <%=Resources.Controls.Store%>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
            <%--End Allocate transfer Qty. based on POs--%>
            <div class="clear">
            </div>
            <%-- Start Allocate additional items in the selected GINs--%>
            <div>
                <%-- <div style="float: right">
                    <img id="imgADNLHide" src="../Images/ERP-Blue/Buttons/arrow-dwn.png" alt="<%= Resources.Controls.Show%>"
                        title="<%= Resources.Controls.Show%>" style="cursor: pointer" onclick="javascript:ShowDetails(4);" />
                    <img id="imgADNLShow" src="../Images/ERP-Blue/Buttons/arrow-up.png" alt="<%= Resources.Controls.Hide%>"
                        title="<%= Resources.Controls.Hide%>" style="display: none; cursor: pointer"
                        onclick="javascript:HideDetails(4);" />
                </div>
                <h1 id="hAdnlDtls">
                    <%=Resources.Controls.AllocateAdditionalItemsInTheSelectedGINs%>
                </h1>--%>
                <h1 id="hAdnlDtls" class="search-colapse-normal">
                    <%=Resources.Controls.AllocateAdditionalItemsInTheSelectedGINs%>
                    <img id="imgADNLHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png" alt="<%= Resources.Controls.Show%>"
                        title="<%=Resources.Controls.Show%>" style="display: none; cursor: pointer" onclick="javascript:ShowDetails(4);" />
                    <img id="imgADNLShow" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="<%= Resources.Controls.Hide%>"
                        title="<%= Resources.Controls.Hide%>" style="cursor: pointer" onclick="javascript:HideDetails(4);" />
                </h1>
                <div id="divAdnlDtls">
                    <div id="searchwrap">
                        <div id="divSearch" class="gridwrap">
                            <table class="gridwraptable gridwrap" style="margin-bottom: 0px!important;">
                                <tr>
                                    <th>
                                        <label style="margin-bottom: 0px!important;">
                                            <%=Resources.Controls.Item%>
                                        </label>
                                    </th>
                                    <th>
                                        <label style="margin-bottom: 0px!important;">
                                            <%=Resources.Controls.Store%>
                                        </label>
                                    </th>
                                    <th>
                                   <%-- style="margin-bottom: 0px!important; text-align:right;"--%>
                                        <label class="txtAlign-right" style="margin-bottom: 0px!important;" >
                                            <%=Resources.Controls.AdditionalQuantity%>
                                        </label>
                                    </th>
                                    <th>
                                        <label style="margin-bottom: 0px!important;">
                                            <%=Resources.Controls.UOM%>
                                        </label>
                                    </th>
                                    <th>
                                        <label style="margin-bottom: 0px!important;">
                                            <%=Resources.Controls.QALotNo%>
                                        </label>
                                    </th>
                                    <th>
                                        <label style="margin-bottom: 0px!important;">
                                            <%=Resources.Controls.Location%>
                                        </label>
                                    </th>
                                    <th>
                                        <%-- <label>
                                        </label>--%>
                                    </th>
                                    <th>
                                        <%-- <label>
                                        </label>--%>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ITEM" runat="server" onchange="javascript:FillStore('0');"
                                            EnableViewState="false" TabIndex="8" Width="180px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="STORE" runat="server" EnableViewState="false" TabIndex="8"
                                            Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="QTY_ALLOCATED" runat="server" EnableViewState="false" TabIndex="8"
                                            CssClass="numeric input-w63 ">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="UOM_TEXT" runat="server" EnableViewState="false" TabIndex="8" Enabled="false"
                                            Width="50px">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SFD_QA_LOT_NO" runat="server" EnableViewState="false" TabIndex="8"
                                            MaxLength="100" Width="90px">
                                        </asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SFD_LOCATION" runat="server" EnableViewState="false" TabIndex="8"
                                            MaxLength="100" Width="110px">
                                        </asp:TextBox>
                                    </td>
                                    <td class="width:60px;">
                                        <asp:Button ID="imbAddAdditionalDtls" runat="server" Text="<%$ Resources:Controls, Allocate%>" TabIndex="8"
                                            ToolTip="<%$resources:Controls,Allocate %>" OnClientClick="javascript:return AllocateAdditionalItem();" />
                                    </td>
                                    <td>
                                        <asp:ImageButton runat="server" ID="imgbtnclear" SkinID="btnrefresh" Width="16px" TabIndex="8"
                                            OnClientClick="javascript:return ClearAllocationAdditionalDtls();" />
                                        <asp:HiddenField runat="server" ID="UOM" Value="1" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="scroll-h150">
                        <div id="divAdditinalQtyDtls">
                            <div id="divAdditionalItemDtlsList" class="gridwrap">
                                <table id="grdAdditionalItemList" rules="all" paging="false" grandtype="GrandGrid"
                                    rules="all" editable="true" editfunction="GridAction" class="gridwraptable gridwrap">
                                    <thead>
                                        <tr>
                                            <th fieldmap="SFD_PK" isvisible="false">
                                            </th>
                                            <th fieldmap="PRD_DEPT" isvisible="false">
                                            </th>
                                            <th fieldmap="POR_ITEM" isvisible="false">
                                            </th>
                                            <th fieldmap="POR_UOM" isvisible="false">
                                            </th>
                                            <th fieldmap="SFD_SL_NO" isvisible="false">
                                            </th>
                                            <th fieldmap="SFD_NO" isvisible="false">
                                            </th>
                                            <th fieldmap="POR_ITEM_NAME" align="left" width="40%">
                                                <%=Resources.Controls.Item%>
                                            </th>
                                            <th fieldmap="PRD_DEPT_NAME" align="left" width="10%">
                                                <%=Resources.Controls.Store%>
                                            </th>
                                            <th fieldmap="ALLOCATE_ADDL_PR_QTY" align="right" width="10%">
                                                <%=Resources.Controls.AdditionalQuantity%>
                                            </th>
                                            <th fieldmap="POR_UOM_NAME" align="left" width="5%">
                                                <%=Resources.Controls.UOM%>
                                            </th>
                                            <th fieldmap="SFD_QA_LOT_NO" width="15%">
                                                <%=Resources.Controls.QALotNo%>
                                            </th>
                                            <th fieldmap="SFD_LOCATION" align="left" width="15%">
                                                <%=Resources.Controls.Location%>
                                            </th>
                                            <th type="Template" width="5%">
                                                <div>
                                                    <asp:ImageButton runat="server" ID="ImageButton1" ToolTip="<%$Resources:Controls,Edit %>" TabIndex="8"
                                                        SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'edit')" />
                                                    <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>" TabIndex="8"
                                                        SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'delete')" />
                                                </div>
                                            </th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </div>
                        <asp:HiddenField ID="DtlPK" runat="server" />
                    </div>
                </div>
            </div>
            <%-- End Allocate additional items in the selected GINs--%>
            <div class="clear">
            </div>
            <%--##### START  Add user Controls and process Id, app ID#####--%>
            <div id="WrkflwActions" style="display: none">
            </div>
            <div id="Wofkflowdiv" style="display: none">
                <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
                <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
                <asp:HiddenField runat="server" ID="SFH_PK" Value="0" />
                <asp:HiddenField ID="SFH_SUBMITTED_BY" runat="server" Value="0" />
                <asp:HiddenField ID="SFH_APPROVED_BY" runat="server" Value="0" />
                <asp:HiddenField ID="SFH_BIZUNIT" runat="server" Value="0" />
                <asp:HiddenField ID="SFH_CRTD_BY" runat="server" Value="0" />
                <asp:HiddenField runat="server" ID="ViewStatus" Value="0" />
                <asp:HiddenField runat="server" ID="SFD_PK" Value="0" />
                <asp:HiddenField runat="server" ID="SFH_STATUS" Value="0" />
                <asp:HiddenField runat="server" ID="POEdit" Value="1" />
                <asp:HiddenField runat="server" ID="PREdit" Value="1" />
                <asp:HiddenField runat="server" ID="GINEdit" Value="1" />
                <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
                <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
                <asp:HiddenField ID="ActionID" runat="server" Value="0" />
                <asp:HiddenField ID="AppNo" runat="server" Value="0" />
                <asp:HiddenField ID="LastModDate" runat="server" />
                <asp:HiddenField ID="hdfDeptID" runat="server" Value="0" />
                <asp:HiddenField ID="hdfGinStore" runat="server" Value="0" />
                <asp:HiddenField ID="hdfTransactionPK" runat="server" Value="0" />
                <asp:HiddenField ID="hdfCompany" runat="server" Value="0" />
                <asp:HiddenField ID="hdfSBUCompany" runat="server" Value="0" />
            </div>
            <%--#####END Add user Controls and process Id, app ID#####--%>
            <%--##### START Hidden Fields#####--%>
            <%-- To Maintain Inspected items list--%>
            <div id="divGINPKList">
                <asp:HiddenField ID="GINPKList" runat="server" />
                <asp:HiddenField ID="GINSelectedItems" runat="server" />
            </div>
            <div id="divPOPKList">
                <asp:HiddenField ID="POPKList" runat="server" />
            </div>
            <div id="divAllocatedQtyList">
                <asp:HiddenField ID="AllocatedQtyList" runat="server" />
            </div>
            <div id="divTotalItem">
                <asp:HiddenField ID="TotalItem" runat="server" />
            </div>
            <div id="divAdditionalItem">
                <asp:HiddenField ID="AdditionalItem" runat="server" />
            </div>
            <div id="divAllocationItem">
                <asp:HiddenField ID="AllocationItem" runat="server" />
            </div>
            <asp:HiddenField ID="GINList" runat="server" />
            <asp:HiddenField ID="POList" runat="server" />
            <asp:HiddenField ID="PRList" runat="server" />
            <asp:HiddenField ID="IsPrefID" runat="server" Value="0" />
            <asp:HiddenField ID="StockTransferList" runat="server" />
            <%--#####END Add user Controls and process Id, app ID#####--%>
            <%-- Start For user Name and DateTime--%>
            <asp:HiddenField ID="SFH_MOD_BY" runat="server" Value="0" />
            <asp:HiddenField ID="SFH_SUBMITTED_DATE" runat="server" Value="0" />
            <asp:HiddenField ID="SFH_APPROVED_DATE" runat="server" Value="0" />
            <asp:HiddenField ID="SFH_CRTD_DT" runat="server" Value="0" />
            <asp:HiddenField ID="SFH_MOD_DT" runat="server" Value="0" />
            <asp:HiddenField ID="hdfRefID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfIsGoToInbox" runat="server" Value="0" />
            <asp:HiddenField ID="SFH_IS_EDIT" runat="server" Value="0" />
            <%--Comma Separation for Quantity & Amount Based on Configuration(Table)--%>
            <asp:HiddenField ID="hdfCurrencyGroup1" Value="3" runat="server" />
            <asp:HiddenField ID="hdfCurrencyGroup2" Value="3" runat="server" />
            <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" Value="0" />
            <%--##### END Hidden Fields#####--%>
            <div id="divData">
            </div>
            <div class="clear">
            </div>
        </div>
        <div id="GinItemAlreadyUsedPopUp">
            <h3>
                <%=Resources.Messages.GINAlreadyAssignedInStockTransferConcurrencyEdit%>
            </h3>
            <div class="clear">
            </div>
            <div style="float: left; margin-left: 2px">
                <asp:Button runat="server" ID="AssignNewDate" Text='<%$ Resources:Controls,Continue %>'
                    EnableViewState="false" ToolTip="<%$resources:Controls,Continue %>" EnableTheming="false"
                    CssClass="inputbtn" Width="70px" Height="20px" OnClientClick="Javascript:return  FillGinDetailsForGinAlreadyUsedCase();" />
                <asp:Button runat="server" ID="btnCancel" Text='<%$ Resources:Controls,Cancel %>'
                    EnableViewState="false" EnableTheming="false" CssClass="inputbtn" Width="70px"
                    Height="20px" ToolTip="<%$resources:Controls,Cancel %>" OnClientClick="javascript:return CancelPage();" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function fnConfirmStockValueChange(command) {
            var msgTitle;
            var msg;
            msgTitle = '<%= Resources.ErpRes.Title_Information %>';
            msg = '<%= Resources.Messages.MayAffectStockValueConfirmation %>';
            $("#divConfirmation").html(msg).dialog({
                modal: true,
                height: 150,
                width: 275,
                title: msgTitle,
                resizable: false,
                buttons: {
                    Yes: function (e) {
                        $(this).dialog("close");
                        $("[id$=ConfirmStockValueChange]").val('1');
                        SavePage(command);
                    },
                    Cancel: function (e) {
                        $(this).dialog("close");
                        $("[id$=ConfirmStockValueChange]").val('0');
                    }
                }
            });
            return false;
        }
    </script>
</asp:Content>
