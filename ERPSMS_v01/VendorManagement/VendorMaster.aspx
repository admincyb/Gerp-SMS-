<%@ Page Title="<%$ Resources:Captions,Title_VendorManagement %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" CodeBehind="VendorMaster.aspx.cs" EnableEventValidation="false"
    Inherits="ERPSMS_v01.VendorManagement.VendorMaster" Theme="ClassicExt" %>

<%@ Register Src="../WorkFlow/WorkflowComments.ascx" TagName="WorkflowComments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/JSLINQ/JSLINQ.js" type="text/javascript"></script>
    <script src="../Scripts/PageScript/VendorManagement/VendorRegistration.js.axd" type="text/javascript"></script>
    <script src="../Scripts/GrandTreeMulti.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
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
                                    TabIndex="100" EnableViewState="False" ToolTip="<%$resources:Controls,Submit %>"
                                    OnClientClick="javascript:return  WkfSubmit();" />
                            </li>
                            <li>
                                <asp:Button runat="server" ID="btnSave" SkinID="btnInner-Save" Text="<%$Resources:Controls,Save%>"
                                    TabIndex="101" EnableViewState="False" ToolTip="<%$resources:Controls,Save %>"
                                    OnClientClick="javascript:return SavePage('Draft');" />
                            </li>
                            <%-- <li>
                                <asp:Button runat="server" ID="btnReset" SkinID="btnInner-refresh" Text="<%$Resources:Controls,Reset%>"
                                    TabIndex="102" EnableViewState="False" OnClientClick="javascript:return ResetPage();" />
                            </li>--%>
                            <li>
                                <asp:Button ID="btnCancel" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Cancel%>"
                                    EnableViewState="False" ToolTip="<%$resources:Controls,Cancel %>" OnClientClick="javascript:return CancelFun();"
                                    TabIndex="103" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div id="divVendorData">
            </div>
            <div id="divFileData">
            </div>
        </div>
        <%-- <asp:ImageButton ID="imbDraft" runat="server" SkinID="btnsave" OnClientClick="javascript:return SavePage('Draft');" />
            <asp:ImageButton ID="imbReset" runat="server" SkinID="btnreset" OnClientClick="javascript:return ResetPage();" />
            <asp:ImageButton ID="imbCancel" runat="server" SkinID="btncancel" OnClientClick="javascript:return CancelFun();" />--%>
    </div>
    <div class="content-wrapper">
        <asp:HiddenField runat="server" ID="VendorDetails" />
        <div id="FormData">
            <asp:HiddenField runat="server" ID="BizUnitPk" />
            <asp:HiddenField runat="server" ID="UserID" />
            <asp:HiddenField runat="server" ID="AddressBookDetails" />
            <asp:HiddenField runat="server" ID="MaterialDetails" />
            <asp:HiddenField runat="server" ID="TermsDetails" />
            <%-----------------------------------------/New Start -----------------%>
            <asp:HiddenField runat="server" ID="MaterialSamples" />
            <asp:HiddenField runat="server" ID="RateHistoryDetails" />
            <%-----------------------------------------/New End -----------------%>
            <asp:HiddenField runat="server" ID="VEN_PK" Value="0" />
            <asp:HiddenField runat="server" ID="ViewStatus" Value="0" />
            <asp:HiddenField ID="ActionID" runat="server" />
            <asp:HiddenField ID="AutoStartValue" runat="server" />
            <asp:HiddenField runat="server" ID="TaxHdr" />
            <asp:HiddenField runat="server" ID="hdfViewMode" />
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S vendormaster">
                            <label for="VEN_NAME">
                                <%=Resources.Controls.Company%>*</label>
                            <asp:TextBox runat="server" ID="VEN_NAME" CssClass="input-half" onkeypress="return this.value.length<200"
                                MaxLength="200" onpaste="return this.value.length<200" TabIndex="1"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_REG_NO">
                                <%=GetLocalResourceObject("BRNo").ToString()%></label><asp:TextBox runat="server"
                                    ID="VEN_REG_NO" TabIndex="3" CssClass="input-half" onkeypress="return this.value.length<200"
                                    MaxLength="200" onpaste="return this.value.length<200"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_CONT_NAME">
                                <%=Resources.Controls.ContactName%></label><asp:TextBox runat="server" ID="VEN_CONT_NAME"
                                    CssClass="input-half" onkeypress="return this.value.length<200" MaxLength="200"
                                    onpaste="return this.value.length<200" TabIndex="4"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_PHONE">
                                <%=Resources.Controls.Phone%></label><asp:TextBox runat="server" ID="VEN_PHONE" onkeypress="return this.value.length<100"
                                    CssClass="input-half" onpaste="return this.value.length<100" MaxLength="100"
                                    TabIndex="6"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_EMAIL">
                                <%=Resources.Controls.Email%></label>
                            <asp:TextBox runat="server" ID="VEN_EMAIL" onkeypress="return this.value.length<100"
                                CssClass="input-half" onpaste="return this.value.length<100" MaxLength="100"
                                TabIndex="7"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_WEBSITE">
                                <%=Resources.Controls.Website%></label>
                            <asp:TextBox runat="server" ID="VEN_WEBSITE" onkeypress="return this.value.length<100"
                                CssClass="input-half" onpaste="return this.value.length<100" MaxLength="100"
                                TabIndex="9"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_MOBIL">
                                <%=Resources.Controls.Mobile%></label>
                            <asp:TextBox runat="server" ID="VEN_MOBIL" onkeypress="return this.value.length<50"
                                CssClass="input-half" onpaste="return this.value.length<50" MaxLength="50" TabIndex="11"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_FAX">
                                <%=Resources.Controls.Fax%></label>
                            <asp:TextBox runat="server" ID="VEN_FAX" onkeypress="return this.value.length<50"
                                CssClass="input-half" onpaste="return this.value.length<50" MaxLength="50" TabIndex="12"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <div id="venCurr" runat="server" class="display-inline">
                                <label for="VEN_CURRENCY">
                                    <%=Resources.Controls.Currency%>
                                    *</label><asp:DropDownList runat="server" ID="VEN_CURRENCY" CssClass="select-half-a"
                                        TabIndex="14" onchange="ChangeCurrency();">
                                    </asp:DropDownList>
                            </div>
                            <div id="venCurrText" style="display: none">
                                <label for="VEN_CURRENCY_TEXT">
                                    <%=Resources.Controls.Currency%>*</label><asp:Label ID="VEN_CURRENCY_TEXT" runat="server"></asp:Label>
                                <%--  <span id="VEN_CURRENCY_TEXT"></span>--%>
                            </div>
                            <div class="clear">
                            </div>
                            <label for="VEN_TYPE">
                                <%=Resources.Controls.ApprovalStatus%>
                                *</label>
                            <asp:DropDownList runat="server" ID="VEN_TYPE" TabIndex="15" CssClass="select-half-a">
                                <asp:ListItem Value="1" Text="Approved"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Non Approved" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                            <div class="clear">
                            </div>
                            <label for="VEN_CREDIT_DAYS">
                                <%=Resources.Controls.CreditDays%></label><asp:TextBox runat="server" ID="VEN_CREDIT_DAYS"
                                    TabIndex="19" CssClass="input-half numeric"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <%-- Shamnad --%>
                            <label for="TaxByVendor">
                                <%=Resources.Controls.Taxes%></label>
                            <asp:Label runat="server" ID="TaxByVendor" Style="text-overflow: clip;" CssClass="input-half"></asp:Label>
                            <img id="imgVendorTax" onclick="javascript:AddVendorItemTax('1','21416');" src="../Images/Classic/Icons/tax.png"
                                alt="Taxes" title="Taxes" style="cursor: pointer" purpose="VendorTax" />
                            <div class="clear">
                            </div>
                            <div id="divTaxddl" runat="server" class="display-inline">
                                <label for="VEN_WHT_TAX">
                                    <%=GetLocalResourceObject("TDS").ToString()%></label><asp:DropDownList runat="server" ID="VEN_WHT_TAX"
                                        CssClass="select-half-a" TabIndex="23">
                                    </asp:DropDownList>
                                <label for="VEN_PAY_FOR_VENDOR" class="margn-rgt2">
                                    <%=Resources.Controls.Pay_Form_Vendor%>
                                </label>
                                <asp:HiddenField runat="server" ID="VEN_PAY_FOR_VENDOR" Value="false" />
                                <asp:CheckBox runat="server" ID="VEN_PAY_FOR_VENDOR1" Checked="false" TabIndex="23" />
                            </div>
                            <div id="divTaxText" runat="server" style="display: none" class="display-inline">
                                <label for="VEN_WHT_TAX_TEXT">
                                    <%=GetLocalResourceObject("TDS").ToString()%></label>
                                <asp:Label ID="VEN_WHT_TAX_TEXT" Width="150px"
                                    runat="server"></asp:Label>
                                <label for="VEN_PAY_FOR_VENDOR_TEXT" class="width-auto">
                                    <%=Resources.Controls.Pay_Form_Vendor%>
                                </label>
                                <asp:CheckBox runat="server" ID="VEN_PAY_FOR_VENDOR_TEXT" Checked="false" TabIndex="20" />
                            </div>
                            <div class="clear">
                            </div>
                            <label for="VEN_NAME2">
                                <%=Resources.Controls.LocalName%></label>
                            <asp:TextBox runat="server" ID="VEN_NAME2" onkeypress="return this.value.length<50"
                                CssClass="input-half" onpaste="return this.value.length<50" MaxLength="50" TabIndex="24"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_DATE">
                                <%=Resources.Controls.VendorRegistrationDate%></label>
                            <asp:TextBox ID="VEN_DATE" runat="server" Text="" onkeydown="return CheckKey(event)"
                                CssClass="input-half" onpaste="return false;" TabIndex="51"></asp:TextBox>

                            <div class="clear">
                            </div>
                            <label for="VEN_COMMISSION">
                                <%=Resources.Controls.PurchaseCommission%></label>
                            <asp:TextBox ID="VEN_COMMISSION" runat="server" Text="0.00" CssClass="input-small-b numeric" onpaste="return false;"></asp:TextBox>

                            <div class="clear">
                            </div>
                            <div id="divGstTypeDdl" class="display-inline">
                                <label for="VEN_GST_TYPE">
                                    <%=Resources.Controls.GstType%>*</label><asp:DropDownList runat="server" ID="VEN_GST_TYPE"
                                        CssClass="select-half-a" TabIndex="20">
                                    </asp:DropDownList>
                            </div>
                            <div id="divGstTypeText" style="display: none" class="display-inline">
                                <label for="VEN_GST_TYPE_TEXT">
                                    <%=Resources.Controls.GstType%>*</label><asp:Label ID="VEN_GST_TYPE_TEXT" runat="server"></asp:Label>
                            </div>
                            <div id="divESINo" style="display: none" class="display-inline">
                                <label for="VEN_ESI_NO"><%=Resources.Controls.ESINo%></label>
                                <asp:TextBox runat="server" ID="VEN_ESI_NO" CssClass="input-half margnlft-minus4" onpaste="return this.value.length<50" MaxLength="50" TabIndex="25"></asp:TextBox>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S vendormaster">
                            <label for="VEN_CODE">
                                <%=Resources.Controls.VendorCode%>
                                *</label>
                            <asp:TextBox runat="server" ID="VEN_CODE" onkeypress="return this.value.length<100"
                                CssClass="input-half" onpaste="return this.value.length<100" MaxLength="100"
                                TabIndex="2"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_TIN">
                                <%=GetLocalResourceObject("TaxID").ToString()%></label><asp:TextBox runat="server"
                                    ID="VEN_TIN" onkeypress="return this.value.length<100" CssClass="input-half"
                                    onpaste="return this.value.length<100" MaxLength="100" TabIndex="3"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_GST_NO_MOD_DT">
                                <%=Resources.Controls.GSTLastChangeDate%></label>
                            <asp:TextBox ID="VEN_GST_NO_MOD_DT" runat="server" Text="" onkeydown="return CheckKey(event)"
                                CssClass="input-half" onpaste="return false;" TabIndex="5"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_ADDR1">
                                <%=Resources.Controls.Address1%></label><asp:TextBox runat="server" ID="VEN_ADDR1"
                                    CssClass="input-half" onkeypress="return this.value.length<300" MaxLength="300"
                                    onpaste="return this.value.length<300" TabIndex="6"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_ADDR2">
                                <%=Resources.Controls.Address2%></label>
                            <asp:TextBox runat="server" ID="VEN_ADDR2" CssClass="input-half" TabIndex="8"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_CITY">
                                <%=Resources.Controls.City%></label>
                            <asp:TextBox runat="server" ID="VEN_CITY" TabIndex="10" CssClass="input-half"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <label for="VEN_CNTRY">
                                <%=Resources.Controls.Country%> *
                            </label>
                            <asp:DropDownList runat="server" ID="VEN_CNTRY" onchange="FillState('VENDOR',false,$(this).val())"
                                CssClass="select-half-a" TabIndex="11">
                            </asp:DropDownList>
                            <span id="VEN_CNTRY_TEXT"></span>
                            <div class="clear">
                            </div>
                            <label for="VEN_STATE">
                                <%=Resources.Controls.State%></label>
                            <asp:DropDownList runat="server" ID="VEN_STATE" TabIndex="13" CssClass="select-half-a">
                            </asp:DropDownList>
                            <span id="VEN_STATE_TEXT"></span>
                            <div class="clear">
                            </div>
                            <%--  <label for="COA_TEXT">
                                <%=Resources.Controls.Account%></label><asp:TextBox ID="COA_TEXT" runat="server"
                                    onkeypress="return this.value.length<100" onpaste="return this.value.length<100"
                                    MaxLength="100" TabIndex="18" />
                            <asp:HiddenField ID="VEN_ACCOUNT" runat="server" Value="0" />--%>
                            <div class="clear">
                            </div>
                            <label for="VEN_PIN">
                                <%=Resources.Controls.Zip%></label>
                            <asp:TextBox runat="server" ID="VEN_PIN" onkeypress="return this.value.length<50"
                                CssClass="input-half" onpaste="return this.value.length<50" MaxLength="50" TabIndex="16"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <div id="divAccountddl">
                                <label for="VEN_ACCOUNT">
                                    <%=Resources.Controls.Account%></label><asp:DropDownList runat="server" ID="VEN_ACCOUNT"
                                        CssClass="select-half-a" TabIndex="18">
                                    </asp:DropDownList>
                            </div>
                            <div id="divAccountText" style="display: none">
                                <label for="VEN_ACCOUNT_TEXT">
                                    <%=Resources.Controls.Account%></label><span id="VEN_ACCOUNT_TEXT"></span>
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divTypeddl" class="display-inline">
                                <label for="Type">
                                    <%=Resources.Controls.Type%>*</label><asp:DropDownList runat="server" ID="VEN_PO_TYPE"
                                        CssClass="select-half-a" onchange="ChangeType();" TabIndex="20">
                                    </asp:DropDownList>
                            </div>
                            <div id="divTypeText" style="display: none" class="display-inline">
                                <label for="VEN_PO_TYPE_TEXT">
                                    <%=Resources.Controls.Type%>*</label><asp:Label ID="VEN_PO_TYPE_TEXT" runat="server"></asp:Label>
                            </div>
                            <div class="clear">
                            </div>
                            <label for="VEN_ADDR3">
                                <%=Resources.Controls.LocalAddress%></label>
                            <asp:TextBox runat="server" ID="VEN_ADDR3" onkeypress="return this.value.length<500"
                                Width="357px" onpaste="return this.value.length<500" MaxLength="500" TabIndex="22"
                                CssClass="multiline-3line" TextMode="MultiLine"></asp:TextBox>
                            <span id="VEN_ADDR3"></span>
                            <div class="clear">
                            </div>
                            <div id="divCommentsText" class="display-inline">
                                <label for="VEN_COMMENTSTEXT">
                                    <%=Resources.Controls.Comments%></label><span id="VEN_COMMENTSTEXT"></span>
                            </div>
                            <div id="divComments" class="display-inline">
                                <label for="VEN_COMMENTS">
                                    <%=Resources.Controls.Comments%></label><asp:TextBox runat="server" ID="VEN_COMMENTS"
                                        TabIndex="23" CssClass="multiline-3line" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div class="clear">
                            </div>
                            <label for="VEN_PAN">
                                <%=GetLocalResourceObject("OtherRefNo").ToString()%></label><asp:TextBox runat="server"
                                    ID="VEN_PAN" onkeypress="return this.value.length<100" CssClass="input-half"
                                    onpaste="return this.value.length<100" MaxLength="100" TabIndex="24"></asp:TextBox>
                            <div class="clear">
                            </div>
                            <%--  <label for="VEN_PAY_FOR_VENDOR">
                                <%=Resources.Controls.Pay_Form_Vendor%></label>
                            <asp:HiddenField runat="server" ID="VEN_PAY_FOR_VENDOR" Value="false" />
                            <asp:CheckBox runat="server" ID="VEN_PAY_FOR_VENDOR1" Checked="false" TabIndex="22" />--%>
                            <%-- Shamnad --%>
                            <%--    18_08_2014          --%>
                            <label for="VEN_ACTIVE">
                                <%=Resources.Controls.VendorActive%>
                            </label>
                            <asp:HiddenField runat="server" ID="VEN_ACTIVE" Value="true" />
                            <asp:CheckBox runat="server" ID="VEN_ACTIVE1" Checked="true" TabIndex="25" />
                            <%--     end                       --%>
                            <div class="clear">
                            </div>
                            <label for="VEN_GST_NO">
                                <%=GetLocalResourceObject("GSTNo").ToString()%></label>
                            <asp:TextBox runat="server" ID="VEN_GST_NO" CssClass="input-half" onpaste="return this.value.length<50"
                                MaxLength="50" TabIndex="25"></asp:TextBox>
                            <%--<label for="VEN_SERVICE_PROVIDED">
                                <%=Resources.Controls.ServiceProvided%></label>
                            <asp:TextBox runat="server" ID="VEN_SERVICE_PROVIDED" TextMode="MultiLine" TabIndex="18"></asp:TextBox>--%>

                            <label for="VEN_ADP_ACCOUNT">
                                <%=Resources.Controls.AdvAccount%></label><asp:DropDownList runat="server" ID="VEN_ADP_ACCOUNT"
                                    CssClass="select-half-a" TabIndex="18">
                                </asp:DropDownList>

                            <div id="divPFNo" style="display: none" class="display-inline">
                                <label for="VEN_PF_NO"><%=Resources.Controls.PFNo%></label>
                                <asp:TextBox runat="server" ID="VEN_PF_NO" CssClass="input-half margnlft-minus4" onpaste="return this.value.length<50" MaxLength="50" TabIndex="25"></asp:TextBox>
                            </div>

                            <asp:HiddenField ID="VENCODE_AUTO" runat="server" Value="0" />
                            <asp:HiddenField ID="APT_CODE" runat="server" Value="" />
                            <asp:HiddenField ID="AST_DOC_MODE" runat="server" Value="0" />
                        </div>
                    </td>
                </tr>
            </table>
            <%--New Vendor Tax PopUp Begins--%>
            <div id="divVendorTax" title="<%=Resources.Messages.TaxDetails%>" style="display: none">
                <div class="Button-container-popup">
                    <asp:Button ID="btnApplyVendorTax" SkinID="btnInner-add-dsd" runat="server" Text="Apply"
                        OnClientClick="javascript:return SaveApplyVendorTax();" />
                </div>
                <div class="content-wrapper">
                    <div class="divcolmiddle-S">
                        <label for="ChooseTaxVendor">
                            <%=Resources.Controls.ChooseType%>
                        </label>
                        <asp:DropDownList ID="ChooseTaxVendor" runat="server" EnableViewState="false">
                        </asp:DropDownList>
                        <asp:ImageButton ID="imbTaxDiscountSaveVendor" SkinID="imbaddnew" runat="server"
                            TabIndex="15" EnableViewState="False" OnClientClick="javascript:return AddTaxDiscountVendorToGrid();" />
                    </div>
                    <div>
                        <table rules="all" id="grdTaxDetailsVendor" grandtype="GrandGrid" paging="false"
                            width="100%" editfunction="GridHandler" editable="true" class="gridwraptable gridwrap">
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
                                            <asp:ImageButton runat="server" ID="imbTaxDeleteVendor" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'TAXDELETEVENDOR')" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
            <%--New Vendor Tax PopUp Ends--%>
            <h1 class="search-colapse-normal" id="VendorHeading">
                <%=Resources.Captions.VendorDetails%>
                <img id="imgVendorShow" src="../Images/Classic/Icons/arrow-colapse-active.png" alt="Show"
                    title="Show" style="cursor: pointer" onclick="javascript:ShowVendor();" />
                <img id="imgVendorHide" src="../Images/Classic/Icons/arrow-colapse-inactive.png"
                    alt="Hide" title="Hide" style="display: none; cursor: pointer" onclick="javascript:HideVendor();" />
            </h1>
            <div id="divTaxDataVendor">
            </div>
            <asp:HiddenField ID="hdfTaxPopUpByVendor" runat="server" Value="hdfTaxPopUpByVendor Value" />
            <div id="VendorSelection" style="display: none;">
                <table class="table-devide">
                    <tr>
                        <td>
                            <div class="div2col-S vendormaster">
                                <label for="VEN_ANNUAL_SALES">
                                    <%=Resources.Controls.AnnualSales%></label>
                                <asp:TextBox runat="server" ID="VEN_ANNUAL_SALES" TabIndex="20" CssClass="input-half numeric"
                                    MaxLength="20" onkeypress="javascript:MakeNumeric(event);"></asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="VEN_MANAGER">
                                    <%=Resources.Controls.ManagerName%></label>
                                <asp:TextBox runat="server" ID="VEN_MANAGER" onkeypress="return this.value.length<100"
                                    CssClass="input-half" onpaste="return this.value.length<100" MaxLength="100"
                                    TabIndex="22"></asp:TextBox>
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="div2col-S vendormaster">
                                <label for="VEN_WAREHOUSE_DTL">
                                    <%=Resources.Controls.WarehouseDetails%></label>
                                <asp:TextBox runat="server" ID="VEN_WAREHOUSE_DTL" onkeypress="return this.value.length<200"
                                    CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                    TabIndex="21"></asp:TextBox>
                                <div class="clear">
                                </div>
                                <label for="VEN_HAS_ISO">
                                    <%=Resources.Controls.ISOCertificate%></label>
                                <asp:HiddenField runat="server" ID="VEN_HAS_ISO" Value="false" />
                                <asp:HiddenField runat="server" ID="Roles" Value="" />
                                <asp:CheckBox runat="server" ID="VEN_HAS_ISO1" Checked="false" TabIndex="23" />
                                <div class="clear">
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="divcol-FileuplWrap">
                    <label for="aupDocument">
                        <%=Resources.Controls.Attachments%></label>
                    <div id="FileUploader" class="input-file">
                        <asp:FileUpload ID="fupUploader" runat="server" ClientIDMode="Static" size="29" Height="22px"
                            Style="margin-top: 3px" TabIndex="24" />
                        <asp:HiddenField ID="FILELIST" runat="server" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
            </div>
        </div>
        <div id="divResult">
        </div>
        <div id="divTaxData">
        </div>
        <div id="divData">
            <div id="divRoleData">
            </div>
        </div>
        <div id="tabs" runat="server">
            <div class="jquery-tabs">
                <ul id="tab-menu">
                    <li><a href="#AddressBook" tabindex="39">
                        <%=Resources.Controls.AddressBook%></a> </li>
                    <li><a href="#MaterialMapping" tabindex="40" onclick="TabChange('material')">
                        <%=Resources.Controls.MaterialMapping%></a> </li>
                    <li><a href="#Terms" tabindex="41" onclick="TabChange('terms')">
                        <%=Resources.Controls.Terms%></a> </li>
                    <%---------------------------------------- /New Update Start Tab Menu ----------------------------------------%>
                    <%---------------------------------------- /New Update Start Tab Menu ----------------------------------------%>
                    <li><a href="#Samples" tabindex="42" onclick="TabChange('samples')">
                        <%=Resources.Controls.Samples%></a> </li>
                    <li><a href="#VendorRoles" tabindex="43" onclick="TabChange('VendorRoles')">
                        <%=Resources.Controls.VendorRoles%></a> </li>
                    <%---------------------------------------- New Update End ----------------------------------------%>
                    <li><a href="#Banks" tabindex="44" onclick="TabChange('Banks')">
                        <%=Resources.Controls.Bank%></a> </li>

                    <li><a href="#AddressBookLocal" tabindex="45" onclick="TabChange('AddressBookLocal')">
                        <%=Resources.Controls.AddressBookLocal%></a> </li>

                </ul>
            </div>
            <div class="clear">
            </div>
            <div id="AddressBook" class="jquery-tabs-contents">
                <div id="AddressBookData">
                    <input type="hidden" runat="server" id="EditAddress" value="0" />
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="VNC_TYPE">
                                        <%=Resources.Controls.Type%></label>
                                    <asp:DropDownList ID="VNC_TYPE" runat="server" TabIndex="25" CssClass="lbl-18perc">
                                    </asp:DropDownList>
                                    <label for="VNC_TYPE_NAME" class="lbl-13-3perc">
                                        <%=Resources.Controls.TypeID%></label>
                                    <asp:TextBox runat="server" ID="VNC_TYPE_NAME" onpaste="return this.value.length<5"
                                        CssClass="lbl-27perc" MaxLength="5" TabIndex="26"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_CONT_NAME">
                                        <%=Resources.Controls.ContactPerson%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="VNC_CONT_NAME" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="28"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_CITY">
                                        <%=Resources.Controls.City%></label>
                                    <asp:TextBox runat="server" ID="VNC_CITY" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="31"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_ADDR1">
                                        <%=Resources.Controls.Address1%></label>
                                    <asp:TextBox runat="server" ID="VNC_ADDR1" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="33"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_ADDR2">
                                        <%=Resources.Controls.Address2%></label>
                                    <asp:TextBox runat="server" ID="VNC_ADDR2" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="35"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_EMAIL">
                                        <%=Resources.Controls.Email%></label>
                                    <asp:TextBox runat="server" ID="VNC_EMAIL" onkeypress="return this.value.length<100"
                                        CssClass="input-half" onpaste="return this.value.length<100" MaxLength="100"
                                        TabIndex="37"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_NAME2">
                                        <%=Resources.Controls.LocalName%></label>
                                    <asp:TextBox runat="server" ID="VNC_NAME2" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="39"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <label for="VNC_NAME">
                                        <%=Resources.Controls.Title%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="VNC_NAME" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="27"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_DEFAULT">
                                        <%=Resources.Controls.Primary%>
                                    </label>
                                    <asp:CheckBox runat="server" ID="VNC_DEFAULT" Checked="true" TabIndex="28" />
                                    <label for="VNC_PHONE" class="lbl-25-5perc">
                                        <%=Resources.Controls.Phone%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="VNC_PHONE" onkeypress="return this.value.length<200"
                                        CssClass="input-small-b" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="30"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_CNTRY">
                                        <%=Resources.Controls.Country%></label>
                                    <asp:DropDownList ID="VNC_CNTRY" runat="server" onchange="FillState('ADDRESS',false,$(this).val())"
                                        CssClass="select-half-a" TabIndex="32">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_STATE">
                                        <%=Resources.Controls.State%></label>
                                    <asp:DropDownList ID="VNC_STATE" runat="server" TabIndex="34" CssClass="select-half-a">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_MOBIL">
                                        <%=Resources.Controls.Mobile%></label>
                                    <asp:TextBox runat="server" ID="VNC_MOBIL" onkeypress="return this.value.length<100"
                                        CssClass="lbl-25-5perc" onpaste="return this.value.length<100" MaxLength="100"
                                        TabIndex="36"></asp:TextBox>
                                    <label for="VNC_FAX" class="lbl-6perc">
                                        <%=Resources.Controls.Fax%></label>
                                    <asp:TextBox runat="server" ID="VNC_FAX" onkeypress="return this.value.length<100"
                                        CssClass="lbl-25-8perc" onpaste="return this.value.length<100" MaxLength="100"
                                        TabIndex="36"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_TAX_NO">
                                        <%=Resources.Controls.TaxID%></label>
                                    <asp:TextBox runat="server" ID="VNC_TAX_NO" onkeypress="return this.value.length<100"
                                        CssClass="input-half" onpaste="return this.value.length<100" MaxLength="100"
                                        TabIndex="38"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_ADDR3">
                                        <%=Resources.Controls.LocalAddress%></label>
                                    <asp:TextBox runat="server" ID="VNC_ADDR3" onkeypress="return this.value.length<500"
                                        Width="347px" onpaste="return this.value.length<200" MaxLength="500" CssClass="multiline-3line"
                                        TextMode="MultiLine" TabIndex="42"></asp:TextBox>
                                    <%--  <asp:Label ID="Label1" runat="server" AssociatedControlID="btnClear" Text=""></asp:Label>
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClientClick="javascript:return  ClearAddress();"
                                        SkinID="btnInner-Cancel" TabIndex="29" />
                                    <asp:Button ID="btnSaveAddBook" runat="server" Text="Save" OnClientClick="javascript:return SaveAddress();"
                                        SkinID="btnInner-Save" TabIndex="29" />--%>
                                    <%--    <input type="button" value="<%=Resources.Controls.Clear%>" id="btnBack"
                             onclick="javascript:return  ClearAddress();" tabindex="29" />--%>
                                    <%-- <input type="button" value="<%=Resources.Controls.saveandaddnew%>" id="AddressSave"
                            style="width: 130px" class="inputbtn" onclick="javascript:return SaveAddress();"
                            tabindex="29" />--%>
                                    <div class="clear">
                                    </div>
                                    <div class="button-wrap-right">
                                        <%--  <input type="button" value="<%=Resources.Controls.Clear%>" id="btnBack" style="width: 130px"
                                            class="inputbtn" onclick="javascript:return  ClearAddress();" tabindex="29" />--%>
                                        <asp:Button ID="AddressSave" runat="server" Text="<%$Resources:Controls,saveandaddnew%>"
                                            SkinID="btnInner-Save" ToolTip="<%$resources:Controls,saveandaddnew %>" EnableViewState="False"
                                            OnClientClick="javascript:return SaveAddress();" TabIndex="44" />
                                        <asp:Button ID="btnBack" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Clear%>"
                                            EnableViewState="False" ToolTip="<%$resources:Controls,Clear %>" OnClientClick="javascript:return ClearAddress();"
                                            TabIndex="43" />
                                        <%--<input type="button" value="<%=Resources.Controls.saveandaddnew%>" id="AddressSave"
                                            style="width: 130px" class="inputbtn" onclick="javascript:return SaveAddress();"
                                            tabindex="29" />--%>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="clear">
                    </div>
                    <div class="gridwrap max-200">
                        <table rules="all" id="grdVendorAddressDetails" grandtype="GrandGrid" paging="false"
                            width="100%" editfunction="GridAddressAction" editable="true" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="AddressID" isvisible="false"></th>
                                    <th fieldmap="VNC_ADDR1" isvisible="false"></th>
                                    <th fieldmap="VNC_ADDR3" isvisible="false"></th>
                                    <th fieldmap="VNC_NAME2" isvisible="false"></th>
                                    <th fieldmap="VNC_ADDR2" isvisible="false"></th>
                                    <th fieldmap="VNC_EMAIL" isvisible="false"></th>
                                    <th fieldmap="VNC_DEFAULT" isvisible="false"></th>
                                    <th fieldmap="VNC_CITY" isvisible="false"></th>
                                    <th fieldmap="VNC_CNTRY" isvisible="false"></th>
                                    <th fieldmap="VNC_STATE" isvisible="false"></th>
                                    <th fieldmap="VNC_MOBIL" isvisible="false"></th>
                                    <th fieldmap="VNC_TAX_NO" isvisible="false"></th>
                                    <%--   //Edit 18_08--%>
                                    <th fieldmap="VNC_TYPE_NAME" isvisible="false"></th>
                                    <%--  //end--%>
                                    <th fieldmap="VNC_FAX" isvisible="false"></th>
                                    <%------------------------------------------------//New start--------------------------------%>
                                    <th fieldmap="VNC_TYPE" isvisible="false"></th>
                                    <%------------------------------------------------//New end--------------------------------%>
                                    <th fieldmap="VNC_NAME" align="left" width="30%">
                                        <%=Resources.Controls.Title%>
                                    </th>
                                    <th fieldmap="VNC_CONT_NAME" align="left" width="28%">
                                        <%=Resources.Controls.ContactPerson%>
                                    </th>
                                    <th fieldmap="VNC_PHONE" align="left" width="14%">
                                        <%=Resources.Controls.Phone%>
                                    </th>
                                    <th fieldmap="VNC_TYPE_TEXT" align="left" width="12%">
                                        <%=Resources.Controls.Type%>
                                    </th>
                                    <th fieldmap="VNC_ADDRESS_TYPE" align="left" width="10%">
                                        <%=Resources.Controls.Primary%>
                                    </th>
                                    <th type="Template" width="6%">
                                        <div>
                                            <asp:ImageButton runat="server" ID="imbEdit" ToolTip="<%$Resources:Controls,Edit %>"
                                                SkinID="imbeditgrid" OnClientClick="javascript:return GridAddressHandler($(this).parents('tr:eq(0)'),'EDIT')" />
                                            <asp:ImageButton runat="server" ID="imbDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                                SkinID="imbdeletegrid" OnClientClick="javascript:return GridAddressHandler($(this).parents('tr:eq(0)'),'DELETE')" />
                                            <asp:ImageButton runat="server" ID="imbView" ToolTip="<%$Resources:Controls,View %>"
                                                SkinID="btnview" OnClientClick="javascript:return GridAddressHandler($(this).parents('tr:eq(0)'),'VIEW')" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                        <div class="clear">
                        </div>
                    </div>
                </div>
            </div>
            <div id="Terms" class="jquery-tabs-contents">
                <div id="TermsData">
                    <div class="gridwrap">
                        <table rules="all" id="grdTermsDetails" grandtype="GrandGrid" paging="false" editfunction="GridTermsAction"
                            enablecheckbox="true" editable="true" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="VET_TYPE" isvisible="false"></th>
                                    <th fieldmap="VET_PK" isvisible="false"></th>
                                    <th fieldmap="VET_TITLE" width="60%" align="left">
                                        <%=Resources.Controls.Title%>
                                    </th>
                                    <th type="Template" align="left" width="38%">
                                        <div>
                                            <input id="Text1" style="width: 90%" onkeypress="javascript:GrandScriptUtils.AllowOnlyNumbers(event,true);" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                        <div class="clear">
                        </div>
                    </div>
                </div>
            </div>
            <div id="MaterialMapping" class="jquery-tabs-contents">
                <div id="MaterialMappingData">
                    <div class="grdTable">
                        <div id="divdummyMaterial">
                            <table id="materialInsert" class="gridwraptable gridwrap">
                                <%--tablefixwidth-td--%>
                                <thead>
                                    <tr class="dummytr">
                                        <th style="width: 210px">
                                            <%=Resources.Controls.MaterialCategory%>*
                                        </th>
                                        <th style="width: 400px">
                                            <%=Resources.Controls.ItemName%>*
                                        </th>
                                        <%--<th style="width: 120px">
                                            <%=Resources.Controls.Material%>*
                                        </th>--%>
                                        <th align="right" style="width: 100px">
                                            <%=Resources.Controls.Price%>
                                        </th>
                                        <th style="width: 100px">
                                            <%=Resources.Controls.Currency%>*
                                        </th>
                                        <th style="width: 100px">
                                            <%=Resources.Controls.MOQ%>
                                        </th>
                                        <th style="width: 100px">
                                            <%=Resources.Controls.UOM%>*
                                        </th>
                                        <th style="width: 100px" align="right">
                                            <%=Resources.Controls.LeadDays%>*
                                        </th>
                                        <th style="width: 80px" align="right">
                                            <%=Resources.Controls.Active%>
                                        </th>
                                        <th></th>
                                        <%---------------------------------//NewMaterial End------------------%>
                                        <th style="width: 100px">
                                            <%=Resources.Controls.Action%>
                                        </th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr id="dummyTr" class="grd-rowhead">
                                        <td>
                                            <asp:DropDownList ID="MaterialType" runat="server" Width="170px" onchange="javascript:FillCategoryDetails($(this).val());"
                                                TabIndex="50">
                                                <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="imbViewCag" runat="server" SkinID="btnview" Width="22px" Height="22px"
                                                ToolTip="<%$ Resources:Controls, Add%>" TabIndex="51" OnClientClick="javascript:return ShowCategory();"
                                                EnableViewState="false" CssClass="floatRight" />
                                        </td>
                                        <td>
                                            <%--<asp:DropDownList ID="ITV_ITEM" runat="server" Width="90px" onchange="javascript:FillMaterialDetails($(this).val());"
                                                TabIndex="52">
                                            </asp:DropDownList>--%>
                                            <asp:TextBox ID="MaterialItem" runat="server" Width="95%" TabIndex="52">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="ITV_ITEM" runat="server" Value="0" />
                                        </td>
                                        <%--<td>
                                            <asp:TextBox ID="ITV_NAME" runat="server" Text="" onkeypress="return this.value.length<100"
                                                onpaste="return this.value.length<100" Width="110px" TabIndex="53"></asp:TextBox>
                                        </td>--%>
                                        <td>
                                            <asp:TextBox runat="server" ID="ITV_PRICE" CssClass="numeric input-w63" Text="" TabIndex="54"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ITV_CURRENCY" runat="server" CssClass="input-uom" TabIndex="55">
                                                <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="ITV_MOQ" Text="" CssClass="numeric input-w50" TabIndex="56"></asp:TextBox>
                                            <%--  onchange="GrandScriptUtils.SetZeroDefault(this, 2)"--%>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:DropDownList ID="ITV_MOQ_UOM" runat="server" TabIndex="57" CssClass="input-uom">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ITV_LEAD_TIME" runat="server" Text="0" MaxLength="5" CssClass="numeric input-w50-1"
                                                onchange="GrandScriptUtils.SetZeroDefault(this)" TabIndex="59" onkeypress="javascript:MakeNumeric(event);"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlActive" runat="server" Width="60px" TabIndex="8">
                                                <asp:ListItem Value="1" Text="<%$ Resources:Captions,Yes %>"></asp:ListItem>
                                                <asp:ListItem Value="0" Text="<%$ Resources:Captions,No %>"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td></td>
                                        <td>
                                            <asp:ImageButton runat="server" SkinID="imbaddnew" ID="imbaddnew" Text="0.00" OnClientClick="javascript:return AddMaterialDetails()"
                                                TabIndex="59" />
                                            <input type="hidden" runat="server" id="EditMaterial" value="0" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="clear">
                        </div>
                        <div id="divMaterials" class="max-250">
                            <table rules="all" id="grdVendorMaterialDetails" grandtype="GrandGrid" pagesize="25"
                                paging="true" editfunction="GridMaterialAction" editable="true" class="gridwraptable gridwrap tablefixwidth-td"
                                width="100%">
                                <thead>
                                    <tr>
                                        <th fieldmap="ITV_SL_NO" isvisible="false"></th>
                                        <%--  <th fieldmap="VendorMaterialID" isvisible="false">
                                        </th>--%>
                                        <th fieldmap="ITV_PK" isvisible="false"></th>
                                        <th fieldmap="MaterialType" isvisible="false"></th>
                                        <th fieldmap="ITV_ITEM" isvisible="false"></th>
                                        <th fieldmap="ITV_MOQ_UOM" isvisible="false"></th>
                                        <th fieldmap="ITV_CURRENCY" isvisible="false"></th>
                                        <th fieldmap="MaterialTypeText" align="left" width="200px">
                                            <%=Resources.Controls.MaterialCategory%>*
                                        </th>
                                        <th fieldmap="ITV_NAME" width="370px">
                                            <%=Resources.Controls.ItemName%>*
                                        </th>
                                        <%--<th fieldmap="ITV_NAME" width="140px">
                                            <%=Resources.Controls.Material%>*
                                        </th>--%>
                                        <th fieldmap="ITV_PRICE" width="60px" align="right">
                                            <%=Resources.Controls.Price%>
                                        </th>
                                        <th fieldmap="MaterialCurrencyText" width="80px">
                                            <%=Resources.Controls.Currency%>*
                                        </th>
                                        <th fieldmap="ITV_MOQ" width="50px" align="right">
                                            <%=Resources.Controls.MOQ%>
                                        </th>
                                        <th fieldmap="UOMText" width="50px">
                                            <%=Resources.Controls.Unit%>*
                                        </th>
                                        <th fieldmap="ITV_LEAD_TIME" width="75px" align="right">
                                            <%=Resources.Controls.LeadDays%>*
                                        </th>
                                        <th fieldmap="ITV_ACTIVE" width="45px" align="right">
                                            <%=Resources.Controls.Active%>*
                                        </th>
                                        <th fieldmap="ITV_TAX_PERC" width="15px"></th>
                                        <th fieldmap="ITV_DISC_PERC" width="15px"></th>
                                        <%---------------------------------//NewMaterial End----------------------------------------------%>
                                        <th type="Template" width="80px">
                                            <div>
                                                <asp:ImageButton runat="server" ID="imbEditMaterial" ToolTip="<%$Resources:Controls,Edit %>"
                                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridMaterialAction($(this).parents('tr:eq(0)'),'EDIT')" />
                                                <asp:ImageButton runat="server" ID="imbDeleteMaterial" ToolTip="<%$Resources:Controls,Delete %>"
                                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridMaterialAction($(this).parents('tr:eq(0)'),'DELETE')" />
                                                <asp:ImageButton runat="server" ID="imbrateHistory" ToolTip="<%$Resources:ErpRes,RateHistory %>"
                                                    SkinID="history" OnClientClick="javascript:return GridMaterialAction($(this).parents('tr:eq(0)'),'HISTORY')" />
                                                <%-- <asp:ImageButton ID="imbrateHistory" runat="server" SkinID="btnview" Width="22px" Height="22px"
                                                ToolTip="<%$ Resources:Controls, Add%>" TabIndex="51" OnClientClick="javascript:return ShowRateHistory();"
                                                EnableViewState="false" />--%>
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
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
            <%--------------------------------------/NewMaterial Tax Start----------------------------------------------------------------%>
            <%--------------------------------------/NewMaterial Tax Start----------------------------------------------------------------%>
            <%--<div id="divItemTax" title="<%=Resources.Captions.LineItemsTax%>">
                    <div class="Button-container-popup">
                        <asp:Button ID="btnApply" SkinID="btnInner-add-dsd" runat="server" Text="Apply" OnClientClick="javascript:return SaveApply();" />
                    </div>
                    <div class="content-wrapper">
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
                            <label for="TaxAmount">
                                <%=Resources.Controls.Amount%>
                            </label>
                            <asp:TextBox ID="TaxAmount" runat="server" EnableViewState="false" Enabled="false"></asp:TextBox>
                            <asp:ImageButton ID="imbTaxDiscountSave" SkinID="imbaddnew" runat="server" TabIndex="15"
                                EnableViewState="False" OnClientClick="javascript:return SaveTaxDiscount();" />
                        </div>
                        <asp:HiddenField runat="server" ID="IsLine" Value="1" />
                        <asp:HiddenField runat="server" ID="IsLineDiscount" Value="1" />
                        <div>
                            <table rules="all" id="grdTaxDetails" grandtype="GrandGrid" paging="false" width="100%"
                                editfunction="GridHandler" editable="true" class="gridwraptable gridwrap">
                                <thead>
                                    <tr>
                                        <th fieldmap="POT_SL_NO" isvisible="false">
                                        </th>
                                        <th fieldmap="POT_PK" isvisible="false">
                                        </th>
                                        <th fieldmap="POT_TAX" isvisible="false">
                                        </th>
                                        <th fieldmap="POT_TAX_TEXT" width="50%">
                                            <%=Resources.Controls.Type%>
                                        </th>
                                        <th fieldmap="POT_TAX_AMT" width="30%">
                                            <%=Resources.Controls.Amount%>
                                        </th>
                                        <th type="Template" width="20%">
                                            <div>
                                                <asp:ImageButton runat="server" ID="imbTaxEdit" SkinID="imbeditgrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'TAXEDIT')" />
                                                <asp:ImageButton runat="server" ID="imbTaxDelete" SkinID="imbdeletegrid" OnClientClick="javascript:return GridHandler($(this).parents('tr:eq(0)'),'TAXDELETE')" />
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>--%>
            <%--------------------------------------/NewMaterial Tax Update End----------------------------------------------------------------%>
            <%----%>
            <%----%>
            <%--------------------------------------/NewMaterial Rate History Start----------------------------------------------------------------%>
            <%--------------------------------------/NewMaterial Rate History Start----------------------------------------------------------------%>
            <div id="divRateHistory" title="<%=Resources.Captions.RateHistory%>" style="display: none">
                <div class="content-wrapper">
                    <div class="divcolmiddle-S">
                        <label for="ItemCode">
                            <%=Resources.Controls.ItemCode%>
                        </label>
                        <asp:Label ID="ItemCode" runat="server"></asp:Label>
                        <label for="ItemName">
                            <%=Resources.Controls.ItemName%>
                        </label>
                        <asp:Label ID="ItemName" runat="server"></asp:Label>
                    </div>
                    <div>
                        <table id="grdRateDetails" grandtype="GrandGrid" rules="all" paging="false" width="100%"
                            class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="VIH_EFCT_FROM" width="25%">
                                        <%=Resources.Controls.FromDate%>
                                    </th>
                                    <th fieldmap="VIH_EFCT_TO" width="25%">
                                        <%=Resources.Controls.ToDate%>
                                    </th>
                                    <th fieldmap="VIH_RATE" width="20%">
                                        <%=Resources.Controls.Rate%>
                                    </th>
                                    <th fieldmap="VIH_TAX_PERC" width="15%">
                                        <%=Resources.Controls.Tax%>
                                    </th>
                                    <th fieldmap="VIH_DISC_PERC" width="15%">
                                        <%=Resources.Controls.Discount%>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                    </div>
                </div>
            </div>
            <div id="VendorRoles" class="jquery-tabs-contents">
                <div id="divTree" style="overflow: auto">
                    <div id="treewrap">
                        <div id="trvRoleMap" class="treeview-adj">
                        </div>
                    </div>
                </div>
            </div>
            <%--------------------------------------/NewMaterial Rate History Update End----------------------------------------------------------------%>
            <%--------------------------------------/New Update Start Tab----------------------------------------------------------------%>
            <%--------------------------------------/New Update Start Tab----------------------------------------------------------------%>
            <div id="Samples" class="jquery-tabs-contents">
                <div id="SamplesData">
                    <div class="grdTable">
                        <div id="divdummySamples">
                            <table id="sampleInsert" class="gridwraptable gridwrap">
                                <thead>
                                    <tr class="dummytr">
                                        <th style="width: 17%">
                                            <%=Resources.Controls.MaterialCategory%>
                                            *
                                        </th>
                                        <th style="width: 15%">
                                            <%=Resources.Controls.MaterialCode%>
                                            *
                                        </th>
                                        <th style="width: 10%">
                                            <%=Resources.Controls.ReceivedDate%>
                                            *
                                        </th>
                                        <th style="width: 10%">
                                            <%=Resources.Controls.QCTest%>
                                            *
                                        </th>
                                        <th style="width: 10%">
                                            <%=Resources.Controls.Value%>
                                            *
                                        </th>
                                        <th style="width: 9%">
                                            <%=Resources.Controls.Remarks%>
                                            *
                                        </th>
                                        <th style="width: 9%">
                                            <%=Resources.Controls.Qty%>
                                            *
                                        </th>
                                        <th style="width: 9%">
                                            <%=Resources.Controls.Reference%>
                                        </th>
                                        <th style="width: 12%">
                                            <%=Resources.Controls.Acpt%>
                                            *
                                        </th>
                                        <th style="width: 10%"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr id="dummyTrSamples" class="grd-rowhead">
                                        <td>
                                            <asp:DropDownList ID="MaterialTypeSamples" runat="server" Width="70%" onchange="javascript:FillCategorySamplesDetails($(this).val());"
                                                TabIndex="50">
                                                <asp:ListItem Text="<%$ Resources:BindValues, Select%>" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:ImageButton ID="imbViewCagSamples" runat="server" SkinID="btnview" Width="22px"
                                                Height="22px" ToolTip="<%$ Resources:Controls, Add%>" TabIndex="51" OnClientClick="javascript:return ShowCategory();"
                                                EnableViewState="false" />
                                        </td>
                                        <td>
                                            <%--   <asp:DropDownList ID="ISV_ITEM" runat="server" Width="90%" TabIndex="52">
                                            </asp:DropDownList>--%>
                                            <asp:TextBox ID="txtSampleItem" runat="server" Width="90%" TabIndex="52">
                                            </asp:TextBox>
                                            <asp:HiddenField ID="ISV_ITEM" runat="server" Value="0" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ISV_RECEIVED_DATE" runat="server" Text="" onkeydown="return CheckKey(event)"
                                                onpaste="return false;" CssClass="date-picker" TabIndex="53"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="ISV_QC_TEST" onkeypress="return this.value.length<100"
                                                onpaste="return this.value.length<100" Width="60px" Text="" TabIndex="54"></asp:TextBox>
                                        </td>
                                        <td style="text-align: left">
                                            <asp:TextBox runat="server" ID="ISV_QC_VALUE" Width="90%" Text="" TabIndex="55"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ISV_REMARKS" runat="server" onkeypress="return this.value.length<400"
                                                onpaste="return this.value.length<400" Width="60px" TabIndex="56"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="ISV_QUANTITY" Width="90%" Text="" TabIndex="57"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ISV_REFERENCE" runat="server" onkeypress="return this.value.length<400"
                                                onpaste="return this.value.length<400" Width="60px" TabIndex="58"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="ISV_STATUS" runat="server" Checked="false" TabIndex="59"></asp:CheckBox>
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" SkinID="imbaddnew" ID="imbSamplesAdd" OnClientClick="javascript:return AddSampleDetails()"
                                                TabIndex="60" />
                                            <input type="hidden" runat="server" id="EditSamples" value="0" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="clear">
                        </div>
                        <div id="divSamples" class="max-250">
                            <table rules="all" id="grdMaterialSampleDetails" grandtype="GrandGrid" paging="false"
                                editfunction="GridSamplesAction" editable="true" class="gridwraptable gridwrap"
                                width="100%">
                                <thead>
                                    <tr>
                                        <th fieldmap="VendorMaterialID" isvisible="false"></th>
                                        <th fieldmap="MaterialType" isvisible="false"></th>
                                        <th fieldmap="ISV_ITEM" isvisible="false"></th>
                                        <th fieldmap="MaterialTypeText" align="left" width="17%">
                                            <%=Resources.Controls.MaterialCategory%>
                                        </th>
                                        <th fieldmap="SampleMaterialCode" width="15%">
                                            <%=Resources.Controls.MaterialCode%>
                                        </th>
                                        <th fieldmap="ISV_RECEIVED_DATE" width="12%">
                                            <%=Resources.Controls.ReceivedDate%>
                                        </th>
                                        <th fieldmap="ISV_QC_TEST" width="12%">
                                            <%=Resources.Controls.QCTest%>
                                        </th>
                                        <th fieldmap="ISV_QC_VALUE" width="10%">
                                            <%=Resources.Controls.Value%>
                                        </th>
                                        <th fieldmap="ISV_REMARKS" width="9%">
                                            <%=Resources.Controls.Remarks%>
                                        </th>
                                        <th fieldmap="ISV_QUANTITY" width="10%">
                                            <%=Resources.Controls.Qty%>
                                        </th>
                                        <th fieldmap="ISV_REFERENCE" width="9%">
                                            <%=Resources.Controls.Reference%>
                                        </th>
                                        <th fieldmap="ISV_STATUS" isvisible="false">
                                            <%=Resources.Controls.Accept%>
                                        </th>
                                        <th fieldmap="ISV_STATUS_TEXT" width="15%">
                                            <%=Resources.Controls.AcceptOrReject%>
                                        </th>
                                        <th type="Template" width="10%">
                                            <div>
                                                <%-- <asp:ImageButton runat="server" ID="imbSamplesEdit" ToolTip="<%$Resources:Controls,Edit %>" SkinID="imbeditgrid" OnClientClick="javascript:return GridSamplesAction($(this).parents('tr:eq(0)'),'EDIT')" />--%>
                                                <asp:ImageButton runat="server" ID="imbSamplesDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridSamplesAction($(this).parents('tr:eq(0)'),'DELETE')" />
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <%--------------------------------------/New Update End----------------------------------------------------------------%>
            <div class="clear">
            </div>
            <div id="Banks" class="jquery-tabs-contents">
                <div id="BankData">
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="VBD_NAME">
                                        <%=Resources.Controls.BankName%>*</label>
                                    <asp:TextBox runat="server" ID="VBD_NAME" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="61"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <label for="VBD_BRANCH">
                                        <%=Resources.Controls.Branch%></label>
                                    <asp:TextBox runat="server" ID="VBD_BRANCH" onpaste="return this.value.length<200"
                                        CssClass="input-half" MaxLength="200" onkeypress="return this.value.length<200"
                                        TabIndex="62"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%--colspan="2"--%>
                                <div class="div2col-S">
                                    <label for="VBD_ADDRESS">
                                        <%=Resources.Controls.Address%></label>
                                    <asp:TextBox runat="server" ID="VBD_ADDRESS" onkeypress="return this.value.length<500"
                                        Width="348px" onpaste="return this.value.length<500" MaxLength="500" class="multiline-2line"
                                        TextMode="MultiLine" Style="width: 695px!important;" TabIndex="63"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="div2col-S">
                                    <label for="VBD_CITY">
                                        <%=Resources.Controls.City%></label>
                                    <asp:TextBox runat="server" ID="VBD_CITY" onkeypress="return this.value.length<200"
                                        CssClass="input-small-b" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="64"></asp:TextBox>
                                    <label for="VBD_STATE_OTHER" class="middle-lbl-small">
                                        <%=Resources.Controls.State%></label>
                                    <asp:TextBox runat="server" ID="VBD_STATE_OTHER" onkeypress="return this.value.length<200"
                                        CssClass="input-small-b" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="65"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_ZIP">
                                        <%=Resources.Controls.Zip%></label>
                                    <asp:TextBox runat="server" ID="VBD_ZIP" onkeypress="return this.value.length<50"
                                        CssClass="input-small-b" onpaste="return this.value.length<50" MaxLength="50"
                                        TabIndex="67"></asp:TextBox>
                                    <label for="VBD_PHONE" class="middle-lbl-small">
                                        <%=Resources.Controls.Phone%></label>
                                    <asp:TextBox runat="server" ID="VBD_PHONE" onkeypress="return this.value.length<50"
                                        CssClass="input-small-b" onpaste="return this.value.length<50" MaxLength="50"
                                        TabIndex="68"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_EMAIL">
                                        <%=Resources.Controls.Email%></label>
                                    <asp:TextBox runat="server" ID="VBD_EMAIL" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="71"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_ACCOUNT_NO">
                                        <%=Resources.Controls.AccountNumber%></label>
                                    <asp:TextBox runat="server" ID="VBD_ACCOUNT_NO" onkeypress="return this.value.length<100"
                                        CssClass="input-half" onpaste="return this.value.length<100" MaxLength="100"
                                        TabIndex="74"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_ACCOUNT_TYPE">
                                        <%=Resources.Controls.AccountType%></label>
                                    <asp:DropDownList ID="VBD_ACCOUNT_TYPE" runat="server" class="select-w21-8per valid"
                                        TabIndex="76">
                                    </asp:DropDownList>
                                    <asp:TextBox runat="server" ID="VBD_ACCOUNT_TYPE_OTHER" onkeypress="return this.value.length<180"
                                        onpaste="return this.value.length<180" MaxLength="180" class="input-w37-7per"
                                        TabIndex="74"></asp:TextBox>
                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <label for="VBD_COUNTRY">
                                        <%=Resources.Controls.Country%></label>
                                    <asp:DropDownList ID="VBD_COUNTRY" runat="server" TabIndex="66" CssClass="select-half-a">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_MOBILE">
                                        <%=Resources.Controls.Mobile%></label>
                                    <asp:TextBox runat="server" ID="VBD_MOBILE" onkeypress="return this.value.length<50"
                                        CssClass="input-small-b" onpaste="return this.value.length<50" MaxLength="50"
                                        TabIndex="69"></asp:TextBox>
                                    <label for="VBD_FAX" class="middle-lbl-small">
                                        <%=Resources.Controls.Fax%></label>
                                    <asp:TextBox runat="server" ID="VBD_FAX" onkeypress="return this.value.length<50"
                                        CssClass="input-small-b" onpaste="return this.value.length<50" MaxLength="50"
                                        TabIndex="70"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_IFSC_CODE">
                                        <%=Resources.Controls.IFSCCode%></label>
                                    <asp:TextBox runat="server" ID="VBD_IFSC_CODE" onkeypress="return this.value.length<100"
                                        CssClass="input-small-b" onpaste="return this.value.length<100" MaxLength="100"
                                        TabIndex="72"></asp:TextBox>
                                    <label for="VBD_SWIFT_CODE" class="middle-lbl-small">
                                        <%=Resources.Controls.SWIFTCode%></label>
                                    <asp:TextBox runat="server" ID="VBD_SWIFT_CODE" onkeypress="return this.value.length<100"
                                        CssClass="input-small-b" onpaste="return this.value.length<100" MaxLength="100"
                                        TabIndex="73"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_ACCOUNT_NO_CNFM">
                                        <%=Resources.Controls.ReEnterAccountNo%></label>
                                    <asp:TextBox runat="server" ID="VBD_ACCOUNT_NO_CNFM" onkeypress="return this.value.length<100"
                                        CssClass="input-half" onpaste="return this.value.length<100" MaxLength="100"
                                        TabIndex="75"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VBD_CONTACT">
                                        <%=Resources.Controls.ContactName%></label>
                                    <asp:TextBox runat="server" ID="VBD_CONTACT" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="78"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <div class="button-wrap-right">
                                        <%--  <input type="button" value="<%=Resources.Controls.Clear%>" id="btnBack" style="width: 130px"
                                            class="inputbtn" onclick="javascript:return  ClearAddress();" tabindex="29" />--%>
                                        <asp:Button ID="BankSave" runat="server" Text="<%$Resources:Controls,saveandaddnew%>"
                                            SkinID="btnInner-Save" ToolTip="<%$resources:Controls,saveandaddnew %>" EnableViewState="False"
                                            OnClientClick="javascript:return AddBankDetails();" TabIndex="79" />
                                        <asp:Button ID="BankClear" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Clear%>"
                                            EnableViewState="False" ToolTip="<%$resources:Controls,Clear %>" OnClientClick="javascript:return clearBank();"
                                            TabIndex="80" />
                                        <%--<input type="button" value="<%=Resources.Controls.saveandaddnew%>" id="AddressSave"
                                            style="width: 130px" class="inputbtn" onclick="javascript:return SaveAddress();"
                                            tabindex="29" />--%>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="clear">
                    </div>
                    <div class="gridwrap max-200">
                        <div class="grdTable">
                            <%--class="max-250"--%>
                            <table rules="all" id="grdVendorBankDetails" grandtype="GrandGrid" pagesize="25"
                                paging="true" editfunction="GridBankAction" editable="true" class="gridwraptable gridwrap tablefixwidth-td"
                                width="100%">
                                <thead>
                                    <tr>
                                        <%--<th fieldmap="ITV_SL_NO" isvisible="true">
                                        </th>
                                          <th fieldmap="VendorMaterialID" isvisible="true">
                                        </th>--%>
                                        <th fieldmap="VBD_PK" isvisible="false"></th>
                                        <%--<th fieldmap="MaterialType" isvisible="false">
                                        </th>
                                        <th fieldmap="ITV_ITEM" isvisible="false">
                                        </th>
                                        <th fieldmap="ITV_MOQ_UOM" isvisible="false">
                                        </th>
                                        <th fieldmap="ITV_CURRENCY" isvisible="false">
                                        </th>--%>
                                        <th fieldmap="VBD_NAME" align="left" width="22%">
                                            <%=Resources.Controls.BankName%>
                                        </th>
                                        <th fieldmap="VBD_BRANCH" width="18%">
                                            <%=Resources.Controls.Branch%>
                                        </th>
                                        <th fieldmap="VBD_COUNTRY_NAME" width="11%">
                                            <%=Resources.Controls.Country%>
                                        </th>
                                        <th fieldmap="VBD_IFSC_CODE" width="11%">
                                            <%=Resources.Controls.IFSCCode%>
                                        </th>
                                        <th fieldmap="VBD_SWIFT_CODE" width="11%">
                                            <%=Resources.Controls.SWIFTCode%>
                                        </th>
                                        <th fieldmap="VBD_ACCOUNT_NO" width="12%">
                                            <%=Resources.Controls.AccountNumber%>
                                        </th>
                                        <th fieldmap="VBD_CONTACT" width="12%">
                                            <%=Resources.Controls.ContactName%>
                                        </th>
                                        <th type="Template" width="5%">
                                            <div>
                                                <asp:ImageButton runat="server" ID="imbViewBankGrid" ToolTip="<%$Resources:Controls,View %>"
                                                    SkinID="btnview" OnClientClick="javascript:return GridBankAction($(this).parents('tr:eq(0)'),'VIEW')" />
                                                <asp:ImageButton runat="server" ID="ImageButton4" ToolTip="<%$Resources:Controls,Edit %>"
                                                    SkinID="imbeditgrid" OnClientClick="javascript:return GridBankAction($(this).parents('tr:eq(0)'),'EDIT')" />
                                                <asp:ImageButton runat="server" ID="ImageButton5" ToolTip="<%$Resources:Controls,Delete %>"
                                                    SkinID="imbdeletegrid" OnClientClick="javascript:return GridBankAction($(this).parents('tr:eq(0)'),'DELETE')" />
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

            <div class="clear">
            </div>


            <div id="AddressBookLocal" class="jquery-tabs-contents">
                <div id="AddressBookLocalData">
                    <input type="hidden" runat="server" id="EditLocalAddress" value="0" />
                    <table class="table-devide">
                        <tr>
                            <td>
                                <div class="div2col-S">

                                    <label for="VNC_LC_TITTLE">
                                        <%=Resources.Controls.Title%>
                                        *</label>
                                    <asp:TextBox runat="server" ID="VNC_LC_TITTLE" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="28"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_LC_LASTNAME">
                                        <%=Resources.Controls.LastName%>
                                    </label>
                                    <asp:TextBox runat="server" ID="VNC_LC_LASTNAME" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="30"></asp:TextBox>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_LC_ADDR2">
                                        <%=Resources.Controls.Address2%></label>
                                    <asp:TextBox runat="server" ID="VNC_LC_ADDR2" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="32"></asp:TextBox>

                                    <div class="clear">
                                    </div>
                                    <label for="VNC_LC_CNTRY">
                                        <%=Resources.Controls.Country%></label>
                                    <asp:DropDownList ID="VNC_LC_CNTRY" runat="server" onchange="FillState('ADDRESS',false,$(this).val())"
                                        CssClass="select-half-a" TabIndex="34">
                                    </asp:DropDownList>
                                    <div class="clear">
                                    </div>
                                    <label for="VNC_LC_TAX_NO">
                                        <%=Resources.Controls.TaxIdNo%></label>
                                    <asp:TextBox runat="server" ID="VNC_LC_TAX_NO" onkeypress="return this.value.length<100"
                                        CssClass="input-half" onpaste="return this.value.length<100" MaxLength="100"
                                        TabIndex="36"></asp:TextBox>

                                </div>
                            </td>
                            <td>
                                <div class="div2col-S">
                                    <label for="VNC_LC_NAME">
                                        <%=Resources.Controls.MaterialName%>
                                    </label>
                                    <asp:TextBox runat="server" ID="VNC_LC_NAME" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="29"></asp:TextBox>


                                    <label for="VNC_LC_ADDR1">
                                        <%=Resources.Controls.Address1%></label>
                                    <asp:TextBox runat="server" ID="VNC_LC_ADDR1" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="31"></asp:TextBox>

                                    <div class="clear">
                                    </div>
                                    <label for="VNC_LC_ADDR3">
                                        <%=Resources.Controls.Address3%></label>
                                    <asp:TextBox runat="server" ID="VNC_LC_ADDR3" onkeypress="return this.value.length<200"
                                        CssClass="input-half" onpaste="return this.value.length<200" MaxLength="200"
                                        TabIndex="33"></asp:TextBox>
                                    <div class="clear">
                                    </div>

                                    <label for="VNC_LC_POSTAL">
                                        <%=Resources.Controls.Postal%></label>
                                    <asp:TextBox runat="server" ID="VNC_LC_POSTAL" onkeypress="return this.value.length<100"
                                        CssClass="lbl-25-5perc" onpaste="return this.value.length<100" MaxLength="100"
                                        TabIndex="34"></asp:TextBox>
                                    <label for="VNC_LC_BRANCH" class="lbl-6perc">
                                        <%=Resources.Controls.Branch%></label>
                                    <asp:TextBox runat="server" ID="VNC_LC_BRANCH" onkeypress="return this.value.length<100"
                                        CssClass="lbl-25-8perc" onpaste="return this.value.length<100" MaxLength="100"
                                        TabIndex="34"></asp:TextBox>
                                    <div class="clear">
                                    </div>


                                    <%--  <asp:Label ID="Label1" runat="server" AssociatedControlID="btnClear" Text=""></asp:Label>
                                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClientClick="javascript:return  ClearAddress();"
                                        SkinID="btnInner-Cancel" TabIndex="29" />
                                    <asp:Button ID="btnSaveAddBook" runat="server" Text="Save" OnClientClick="javascript:return SaveAddress();"
                                        SkinID="btnInner-Save" TabIndex="29" />
                                    <%--    <input type="button" value="<%=Resources.Controls.Clear%>" id="btnBack"
                             onclick="javascript:return  ClearAddress();" tabindex="29" />--%>
                                    <%-- <input type="button" value="<%=Resources.Controls.saveandaddnew%>" id="AddressSave"
                            style="width: 130px" class="inputbtn" onclick="javascript:return SaveAddress();"
                            tabindex="29" />--%>
                                    <div class="clear">
                                    </div>
                                    <div class="button-wrap-right">
                                        <%--  <input type="button" value="<%=Resources.Controls.Clear%>" id="btnBack" style="width: 130px"
                                            class="inputbtn" onclick="javascript:return  ClearAddress();" tabindex="29" />--%>
                                        <asp:Button ID="btnSaveLocalAddress" runat="server" Text="<%$Resources:Controls,saveandaddnew%>"
                                            SkinID="btnInner-Save" ToolTip="<%$resources:Controls,saveandaddnew %>" EnableViewState="False"
                                            OnClientClick="javascript:return SaveLocalAddressDetails();" TabIndex="44" />
                                        <asp:Button ID="btnLocalAddressClear" runat="server" SkinID="btnInner-Cancel" Text="<%$Resources:Controls,Clear%>"
                                            EnableViewState="False" ToolTip="<%$resources:Controls,Clear %>" OnClientClick="javascript:return ClearLocalAddress();"
                                            TabIndex="43" />
                                        <%--<input type="button" value="<%=Resources.Controls.saveandaddnew%>" id="AddressSave"
                                            style="width: 130px" class="inputbtn" onclick="javascript:return SaveAddress();"
                                            tabindex="29" />--%>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <div class="clear">
                    </div>
                    <div class="gridwrap max-200">
                        <table rules="all" id="grdVendorLocalAddressDetails" grandtype="GrandGrid" paging="false"
                            width="100%" editfunction="GridAddressAction" editable="true" class="gridwraptable gridwrap">
                            <thead>
                                <tr>
                                    <th fieldmap="LocalAddressID" isvisible="false"></th>

                                    <th fieldmap="VNC_LC_PK" isvisible="false"></th>

                                    <th fieldmap="VNC_LC_TITTLE" align="left" width="11%">
                                        <%=Resources.Controls.Title%>
                                    </th>

                                    <th fieldmap="VNC_LC_NAME" align="left" width="13%">
                                        <%=Resources.Controls.Name%>
                                    </th>

                                    <th fieldmap="VNC_LC_LASTNAME" align="left" width="10%">
                                        <%=Resources.Controls.LastName%>
                                    </th>
                                    <th fieldmap="VNC_LC_ADDR1" align="left" width="15%">
                                        <%=Resources.Controls.Address1%>
                                    </th>
                                    <th fieldmap="VNC_LC_ADDR2" align="left" width="15%">
                                        <%=Resources.Controls.Address2%>
                                    </th>
                                    <th fieldmap="VNC_LC_ADDR3" align="left" width="15%">
                                        <%=Resources.Controls.Address3%>
                                    </th>
                                    <%--  <th fieldmap="VNC_LC_CNTRY" align="left" width="12%">
                                            <%=Resources.Controls.Country%>
                                        </th>--%>

                                    <%-- <th fieldmap="VNC_LC_POSTAL" align="left" width="8%">
                                            <%=Resources.Controls.Postal%>
                                        </th>

                                   <th fieldmap="VNC_LC_BRANCH" align="left" width="8%">
                                            <%=Resources.Controls.Branch%>
                                        </th>--%>
                                    <th fieldmap="VNC_LC_TAX_NO" align="left" width="15%">
                                        <%=Resources.Controls.TaxIdNo%>
                                    </th>

                                    <th type="Template" width="11%">
                                        <div>
                                            <asp:ImageButton runat="server" ID="ImbLocalAddrrEdit" ToolTip="<%$Resources:Controls,Edit %>"
                                                SkinID="imbeditgrid" OnClientClick="javascript:return GridLocalAddressAction($(this).parents('tr:eq(0)'),'EDITLOCALADDRESS')" />
                                            <asp:ImageButton runat="server" ID="ImbLocalAddrrDelete" ToolTip="<%$Resources:Controls,Delete %>"
                                                SkinID="imbdeletegrid" OnClientClick="javascript:return GridLocalAddressAction($(this).parents('tr:eq(0)'),'DELETELOCALADDRESS')" />
                                            <asp:ImageButton runat="server" ID="ImbLocalAddrrView" ToolTip="<%$Resources:Controls,View %>"
                                                SkinID="btnview" OnClientClick="javascript:return GridLocalAddressAction($(this).parents('tr:eq(0)'),'VIEWLOCALADDRESS')" />
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                        </table>
                        <div class="clear">
                        </div>
                    </div>
                </div>
            </div>





        </div>
    </div>
    <%--##### START Remove Old Workflow Section &   Add user Controls and process Id, app ID#####--%>
    <div id="Wofkflowdiv" style="display: none">
        <uc1:WorkflowComments ID="ucrWrkf" runat="server" ValidationGroup="Save" />
        <asp:HiddenField ID="hdfProcessID" runat="server" Value="0" />
        <asp:HiddenField ID="hdfAppID" runat="server" Value="0" />
        <asp:HiddenField ID="TaskID" runat="server" Value="0" />
        <asp:HiddenField ID="hdfVendorPK" runat="server" Value="0" />
        <asp:HiddenField ID="SubmitFlag" runat="server" Value="0"></asp:HiddenField>
        <asp:HiddenField ID="hdfRefID" runat="server" Value="0" />
        <asp:HiddenField ID="hdfIsGoToInbox" runat="server" Value="0" />
        <asp:HiddenField ID="hdfVenCode" runat="server" Value="" />
    </div>
    <%--#####END Add user Controls and process Id, app ID#####--%>
    <div class="clear">
    </div>
    <asp:HiddenField runat="server" ID="MaterialDetailId" Value="0" EnableViewState="false" />
    <div id="divCategory" title="<%=Resources.Captions.SelectCategory%>">
        <div id="treewrap" class="treeviewrap-fxd">
            <div id="trvCategory" class="treeview-adj">
            </div>
        </div>
    </div>
    <input type="hidden" id="hdfVirtualPath" value='<%= (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())
    %>' />
    <asp:HiddenField ID="hdfIsVendorTax" runat="server" />
    <asp:HiddenField ID="hdfIsMaterialTax" runat="server" />
    <asp:HiddenField runat="server" ID="hdfIsLineitemTax" Value="0" />
    <asp:HiddenField runat="server" ID="hdfIsLineitemDiscount" Value="0" />
    <asp:HiddenField ID="hdfBaseCurrency" runat="server" Value="0" />
    <asp:HiddenField ID="hdfIscontYes" runat="server" Value="0" />
    <asp:HiddenField ID="hdfCurrncyChangeConfirm" runat="server" Value="0" />
    <asp:HiddenField ID="hdfResetCurrencyOnType" runat="server" Value="1" />
    <asp:HiddenField ID="hdfIsGstEnabled" runat="server" Value="0" />
    <asp:HiddenField ID="hdfIsCurrencyWithTypeValidnReqd" runat="server" Value="0" />
    <asp:HiddenField ID="hdfIsShowESINo" runat="server" Value="0" />
    <asp:HiddenField ID="hdfIsShowPFNo" runat="server" Value="0" />
    <asp:HiddenField ID="hdfIncludeFG" runat="server" Value="0" />
</asp:Content>
