<%@ Page Title="<%$ Resources:Captions,Title_NewItemRequest %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    EnableEventValidation="false" Theme="Classic" AutoEventWireup="true" CodeBehind="NewItemRequest.aspx.cs"
    Inherits="ERPSMS_v01.StoreManagement.NewItemRequest" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/PageScript/StoreManagement/NewItemRequest.js.axd" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <div id="webwizard-wrap">
        <h1>
            <%=Resources.Captions.NIR%></h1>
        <div class="button-wrap" style="width: auto; padding: 5px; margin: 0px; float: right;
            text-align: right">
            <asp:ImageButton ID="imbSave" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage('Draft');"
                EnableViewState="false" TabIndex="19" />
            <asp:ImageButton ID="imdReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();"
                EnableViewState="false" TabIndex="20" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();"
                EnableViewState="false" TabIndex="21" />
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
                                <asp:Button runat="server" ID="btnSubmit" SkinID="btnInner-submit" Text="<%$Resources:Controls,Submit%>"
                                    TabIndex="34" EnableViewState="False" OnClientClick="javascript:return  WkfSubmit();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="34" EnableViewState="False" OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                           <%-- <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="35" EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" OnClientClick="javascript:return CancelFun();" TabIndex="36" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="clear">
        </div>
    </div>
    <div class="content-wrapper">
        <div id="divData">
            <div id="divXml">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <label>
                                    <%=Resources.Controls.NIR%></label>
                                <asp:Label ID="lblNIR" runat="server" Text=""></asp:Label>
                                <asp:HiddenField ID="NIR_NO" runat="server" />
                                <div class="clear">
                                </div>
                                <label for="MRH_DEPT_STR">
                                    <%=Resources.Controls.PlaceRequestTo%>*</label>
                                <asp:DropDownList ID="MRH_DEPT_STR" runat="server" TabIndex="1"  onchange="javascript:FillDepartement();">
                                </asp:DropDownList>
                                <label for="DeptPk">
                                    <%=Resources.Controls.RequiredFor%>*</label>
                                <asp:DropDownList ID="DeptPk" runat="server" TabIndex="2">
                                </asp:DropDownList>
                                <asp:HiddenField runat="server" ID="NewItemDetailsList" />
                                <asp:HiddenField runat="server" ID="ActionStatus" Value="0" />
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <label>
                                    <%=Resources.Controls.Date%>*</label>
                                <asp:TextBox ID="NIR_SUBMITTED_DATE" runat="server" TabIndex="3"></asp:TextBox>
                                <label>
                                    <%=Resources.Controls.RequiredByDate%>*</label>
                                <asp:TextBox ID="NIR_REQUIRED_DATE" runat="server" TabIndex="4"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <h4 class="head-2col"><%=Resources.Controls.ItemDetails%></h4>
                <div class="clear">
                </div>
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S">
                                <label for="Store"><%=Resources.Controls.Category%>*</label>
                                <asp:DropDownList ID="MaterialType" runat="server" TabIndex="5" onchange="javascript:FillUOM($(this).val());">
                                </asp:DropDownList>
                                <div class="clear">
                                </div>
                                <label for="Store">
                                    <%=Resources.Controls.NameTitle%>*</label>
                                <asp:TextBox ID="NameTitle" runat="server" EnableViewState="False"
                                    TabIndex="6"></asp:TextBox>
                                <label for="Store"><%=Resources.Controls.RequestFrequency%>*</label>
                                
                                    <asp:RadioButtonList ID="RequestFrequency" runat="server" TabIndex="7" onclick="javascript:SettingVisibiltyForTextbox();"
                                        EnableViewState="False" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="checkbx-inline">
                                        <asp:ListItem Text="<%$Resources:BindValues,OneTime%>" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="<%$Resources:BindValues,Recurring%>" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                
                                <div class="clear"></div>
                                <label for="FRD">
                                    <%=Resources.Controls.FRD%>*</label>
                                <asp:TextBox ID="FRD" runat="server" TextMode="MultiLine" TabIndex="8"
                                    EnableViewState="False"></asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="KnownVendors">
                                    <%=Resources.Controls.KnownVendors%></label>
                                <asp:TextBox ID="KnownVendors" runat="server" TextMode="MultiLine"
                                    EnableViewState="False" TabIndex="9"></asp:TextBox>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S">
                                <label for="Store">
                                    <%=Resources.Controls.QtyRequired%>*</label>
                                <asp:TextBox ID="QtyRequired" runat="server"  CssClass="half" EnableViewState="False"
                                    TabIndex="10"></asp:TextBox>
                                <asp:DropDownList ID="UOM" runat="server" TabIndex="11" CssClass="half" onchange="javascript:FillDepartement();"
                                    EnableViewState="False">
                                </asp:DropDownList>
                                <div class="clear"></div>
                                <label for="Store"> <%=Resources.Controls.Description%></label>
                                <asp:TextBox ID="Description" runat="server" TextMode="MultiLine" TabIndex="12"
                                    EnableViewState="False"></asp:TextBox>
                                <label for="Store">
                                    <%=Resources.Controls.Purpose%></label>
                                <asp:TextBox ID="Purpose" runat="server" TextMode="MultiLine"  EnableViewState="False"
                                    TabIndex="13"></asp:TextBox>
                                <label for="Store">
                                    <%=Resources.Controls.CommercialDetails%></label>
                                <asp:TextBox ID="CommercialDetails" runat="server" TextMode="MultiLine"
                                    TabIndex="14" EnableViewState="False"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="clear">
                </div>
                <div style="padding-left: 20.5%" id="divIM">
                    <div class="checkbx">
                        <asp:CheckBox ID="chkItemCode" runat="server" TabIndex="15" Text="<%$Resources:Controls,CreateItemCode%>"
                            Width="280px" /></div>
                    <asp:TextBox ID="txtItemCode" runat="server" TabIndex="16" EnableViewState="False"></asp:TextBox>
                    <div class="clear">
                    </div>
                    <div class="checkbx">
                        <asp:CheckBox ID="chkPR" runat="server" TabIndex="17" Text="<%$Resources:Controls,CreatePRForItem%>"
                            Width="280px" /></div>
                    <asp:TextBox ID="txtPR" runat="server" TabIndex="18" EnableViewState="False"></asp:TextBox>
                </div>
                <asp:HiddenField runat="server" ID="ITR_PK" Value="0" />
                <asp:HiddenField runat="server" ID="Remarks" Value="" />
            </div>
        </div>
        <div class="clear">
        </div>
        <%--##### START Remove Old Workflow Section &   Add user Controls and process Id, app ID#####--%>
        <div id="Wofkflowdiv" style="display:none">
            <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
            <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
            <asp:HiddenField ID="AppNo" runat="server" />
            <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
        </div>
        <%--#####END Add user Controls and process Id, app ID#####--%>
        <asp:HiddenField runat="server" ID="EditRequisition" Value="0" />
        <asp:HiddenField runat="server" ID="MRD_PK" Value="0" />
        <asp:HiddenField runat="server" ID="hdnMaterialPk" Value="0" />
        <asp:HiddenField runat="server" ID="ActionID" />
        <!-- Stores the Order Json Object Jquery Data Store -->
        <div id="divRequisitionData">
        </div>
        <div id="divItemRequestPopUp" title="<%=Resources.Messages.Conformation%>">
            <div class="divcol-inline">
                <label for="LocationName">
                    <%=Resources.Messages.NewRequestConfirmation%>
                </label>
                <div class="clear">
                </div>
                <asp:Button runat="server" ID="btnYes" Text="Yes" EnableViewState="false" CssClass="inputbtn"
                    TabIndex="16" Width="50px" Height="20px" OnClientClick="javascript:return SavePageAfterPopup();" />
                <asp:Button runat="server" ID="btnNo" Text="No" EnableViewState="false" CssClass="inputbtn"
                    TabIndex="16" Width="50px" Height="20px" OnClientClick="javascript:return CancelPageAfterPopup();" />
            </div>
            <div class="clear">
            </div>
            <div class="gridwrap">
                <table rules="all" id="grdItemRequestPopup" grandtype="GrandGrid" pagesize="10" paging="false"
                    width="100%" class="gridwraptable gridwrap">
                    <thead>
                        <tr>
                            <th fieldmap="ITM_PK" isvisible="false">
                            </th>
                            <th fieldmap="ITM_CODE" sortable="false" align="left" width="30%">
                                <%=Resources.Controls.Code%>
                            </th>
                            <th fieldmap="ITM_NAME" sortable="false" align="left" width="30%">
                                <%=Resources.Controls.Item%>
                            </th>
                            <th fieldmap="ITM_DESC" sortable="false" align="left" width="40%">
                                <%=Resources.Controls.Description%>
                            </th>
                        </tr>
                    </thead>
                </table>
            </div>
        </div>
    </div>
</asp:Content>
