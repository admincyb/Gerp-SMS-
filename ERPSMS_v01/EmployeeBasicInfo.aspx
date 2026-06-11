<%@ Page Title="HRMS-Employee Basic Information" Language="C#" MasterPageFile="~/ERPSMS_2.Master"
    AutoEventWireup="true" Theme="Classic" CodeBehind="EmployeeBasicInfo.aspx.cs"
    Inherits="ERPSMS_v01.EmployeeBasicInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ShowHideJobDetails(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divJobDetails]").show();
                $("[id$=imbShowJobDetails]").hide();
                $("[id$=imbHideJobDetails]").show();
            }
            else {
                $("[id$=divJobDetails]").hide();
                $("[id$=imbShowJobDetails]").show();
                $("[id$=imbHideJobDetails]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }
        function ShowHideContactInformation(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divContactInformationDetails]").show();
                $("[id$=imbShowContactdetails]").hide();
                $("[id$=imbHideContactdetails]").show();
            }
            else {
                $("[id$=divContactInformationDetails]").hide();
                $("[id$=imbShowContactdetails]").show();
                $("[id$=imbHideContactdetails]").hide();
            }
            $("[id$=hdfIsItemDetailsVisible]").val(flag);
            return false;
        }
        function ShowHideAdditionalInfo(flag) {
            ///<summary>
            /// Used to Show/Hide ItemDetails Div
            ///</summary>

            //If flag then Show Items
            if (flag == 1) {
                $("[id$=divHideShowAdditionalInfo]").show();
                $("[id$=imbShowAdditionalInfo]").hide();
                $("[id$=imbHideAdditionalInfo]").show();
            }
            else {
                $("[id$=divHideShowAdditionalInfo]").hide();
                $("[id$=imbShowAdditionalInfo]").show();
                $("[id$=imbHideAdditionalInfo]").hide();
            }
            $("[id$=hdfIsContactInfoVisisble]").val(flag);
            return false;
        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server" ID="aupdpnlBasicInfo">
        <ContentTemplate>
            <div class="fixed-buttons">
                <div class="Button-container">
                    <asp:Table ID="Table3" runat="server">
                        <asp:TableRow>
                            <%-- SEC_ACTION is a dummy cssclass  FOR Accessing the Buttons in the Table Cell--%>
                            <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                                <ul runat="server" id="pnlEntry">
                                    <li runat="server" id="pnlSave">
                                        <asp:Button runat="server" ID="btnSave" CommandName="SAVE" TabIndex="58" Text="Save"
                                            ValidationGroup="invoice" ToolTip="Save" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Save" />
                                    </li>
                                    <li runat="server" id="pnlDelete">
                                        <asp:Button runat="server" ID="btnDeleteNew" CommandName="DELETE" Text="Delete" TabIndex="59"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Delete" ToolTip="Delete" OnClientClick="return ShowDeleteConfirm(this);" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" ID="btnCancel" Text="Cancel" ToolTip="Cancel" CommandName="CANCEL"
                                            TabIndex="61" CommandArgument="SEC_ActionPanel" SkinID="btnInner-Cancel" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="65" ID="btnNew" CommandName="NEW" Text="New"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-New" ToolTip="New" />
                                    </li>
                                    <li>
                                        <asp:Button runat="server" TabIndex="66" ID="btnEdit" CommandName="EDIT" Text="Edit"
                                            CommandArgument="SEC_ActionPanel" SkinID="btnInner-Edit" ToolTip="Edit" />
                                    </li>
                                </ul>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
            </div>
            <div class="content-wrapper">
                <asp:TableCell>
                    <asp:TableRow ID="PageAction_Entry" runat="server" Style="display: none">
                        <asp:TableCell>
                            <table class="table-devide">
                                <tr>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label ID="lblCode" runat="server" Text="Code" AssociatedControlID="txtEmpCode"></asp:Label>
                                            <asp:TextBox ID="txtEmpCode" runat="server" CssClass="large" MaxLength="100" TabIndex="1"> </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label ID="lblFirstName" runat="server" Text="First Name" AssociatedControlID="txtFirstName"></asp:Label>
                                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="large" MaxLength="100" TabIndex="2"> </asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblMiddleName" Text="Middle Name" AssociatedControlID="txtMiddleName"></asp:Label>
                                            <asp:TextBox ID="txtMiddleName" runat="server" TabIndex="3"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblLastName" Text="Last Name" AssociatedControlID="txtLastName"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtLastName" Text="" TabIndex="4" MaxLength="100"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblSurname" Text="Surname" AssociatedControlID="txtSurname"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtSurname" Text="" TabIndex="5" MaxLength="100"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblDOB" Text="DOB" AssociatedControlID="txtDOB"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtDOB" Text="" TabIndex="6" MaxLength="100"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblGender" Text="Gender" AssociatedControlID="ddlGender"></asp:Label>
                                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="medium" TabIndex="7">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblMobile" Text="Mobile" AssociatedControlID="txtMobile"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtMobile" Text="" TabIndex="8" MaxLength="100"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div class="div2col-S">
                                            <asp:Label runat="server" ID="lblImageUpload" Text="Image" AssociatedControlID="fudImage"></asp:Label>
                                            <asp:FileUpload ID="fudImage" runat="server" TabIndex="20" CssClass="upload-btn" />
                                            <%-- <asp:Button ID="btnRemove" runat="server" Text="Remove" />
                                            <asp:Button ID="btnUpload" runat="server" Text="Upload" />
                                            <asp:Image ID="ImgFoto" runat="server" />--%>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblMaritalStatus" Text="Marital Status" AssociatedControlID="ddlMaritalStatus"></asp:Label>
                                            <asp:DropDownList ID="ddlMaritalStatus" runat="server" CssClass="medium" TabIndex="10">
                                            </asp:DropDownList>
                                            <div class="clear">
                                            </div>
                                            <asp:Label runat="server" ID="lblPersonalEmail" Text="Personal Email" AssociatedControlID="txtPersonalEmail"></asp:Label>
                                            <asp:TextBox ID="txtPersonalEmail" runat="server" CssClass="medium" TabIndex="11"
                                                MaxLength="100" Enabled="true"></asp:TextBox>
                                            <div class="clear">
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="search-colapse-b">
                                <h1>
                                    Job Details</h1>
                                <asp:ImageButton runat="server" ID="imbShowJobDetails" OnClientClick="javascript:return ShowHideJobDetails(1);"
                                    SkinID="imbArrowInactive" ToolTip="Show Details " TabIndex="107" />
                                <asp:ImageButton runat="server" ID="imbHideJobDetails" OnClientClick="javascript:return ShowHideJobDetails();"
                                    Style="display: none" SkinID="imbArrowActive" TabIndex="170" ToolTip="Hide Details" />
                                <asp:HiddenField ID="hdfIsItemDetailsVisible" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divJobDetails" style="display: none">
                                <table class="table-devide" id="tblDetails">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblDepartment" runat="server" Text="Department" AssociatedControlID="ddlDepartment"></asp:Label>
                                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="medium" TabIndex="12">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblEmploymentType" runat="server" Text="Employment Type" AssociatedControlID="ddlEmploymentType"></asp:Label>
                                                <asp:DropDownList ID="ddlEmploymentType" runat="server" CssClass="medium" TabIndex="14">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblDOJ" Text="DOJ" AssociatedControlID="txtDOJ"></asp:Label>
                                                <asp:TextBox ID="txtDOJ" runat="server" TabIndex="16"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblConfirmedOn" Text="Confirmed on" AssociatedControlID="txtConfirmedOn"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtConfirmedOn" Text="" TabIndex="18" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblOfficialPhone" Text="Official Phone" AssociatedControlID="txtOfficialPhone"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtOfficialPhone" Text="" TabIndex="20" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblStatus" Text="Status" AssociatedControlID="txtStatus"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtStatus" Text="" TabIndex="22" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblActive" Text="Active" AssociatedControlID="chkActive"></asp:Label>
                                                <asp:CheckBox ID="chkActive" runat="server" Checked="true" TabIndex="24" />
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblDesignation" runat="server" Text="Designation" AssociatedControlID="ddlDesignation"></asp:Label>
                                                <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="medium" TabIndex="13">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblBranchLocation" runat="server" Text="Branch/Location" AssociatedControlID="ddlBranchLocation"></asp:Label>
                                                <asp:DropDownList ID="ddlBranchLocation" runat="server" CssClass="medium" TabIndex="15">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblCompany" Text="Company" AssociatedControlID="ddlCompany"></asp:Label>
                                                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="medium" TabIndex="17">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblReportto" Text="Report to" AssociatedControlID="ddlReportto"></asp:Label>
                                                <asp:DropDownList ID="ddlReportto" runat="server" CssClass="medium" TabIndex="19">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblOfficialMobile" Text="Official Mobile" AssociatedControlID="txtOfficialMobile"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtOfficialMobile" Text="" TabIndex="21" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblOfficialEmail" Text="DOB" AssociatedControlID="txtOfficialEmail"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtOfficialEmail" Text="" TabIndex="23" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    Contact Information
                                </h1>
                                <asp:ImageButton runat="server" ID="imbShowContactdetails" OnClientClick="javascript:return ShowHideContactInformation(1);"
                                    SkinID="imbArrowInactive" ToolTip="Show Contact Details" />
                                <asp:ImageButton runat="server" ID="imbHideContactdetails" OnClientClick="javascript:return ShowHideContactInformation();"
                                    Style="display: none" SkinID="imbArrowActive" ToolTip="Hide Contact Details" />
                                <asp:HiddenField ID="hdfShowTaxAmtBC" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divContactInformationDetails" style="display: none">
                                <table class="table-devide" id="Table1">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblPermanentAddress" runat="server" Text="Permanent Address" AssociatedControlID="txtPermanentAddress"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPermanentAddress" Text="" TextMode="MultiLine"
                                                    TabIndex="4" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <br />
                                                <asp:Label ID="lblCity" runat="server" Text="City" AssociatedControlID="txtCity"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCity" Text="" TabIndex="26" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblState" Text="State" AssociatedControlID="txtState"></asp:Label>
                                                <asp:TextBox ID="txtState" runat="server" TabIndex="28"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblCountry" Text="Country" AssociatedControlID="ddlCountry"></asp:Label>
                                                <asp:DropDownList ID="ddlCountry" runat="server" CssClass="medium" TabIndex="30">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblZipCode" Text="ZipCode" AssociatedControlID="txtZipCode"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtZipCode" Text="" TabIndex="32" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblPhone" Text="Phone" AssociatedControlID="txtPhone"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPhone" Text="" TabIndex="34" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblNationality" Text="Nationality" AssociatedControlID="ddlNationality"></asp:Label>
                                                <asp:DropDownList ID="ddlNationality" runat="server" CssClass="medium" TabIndex="36">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblCommunicationAddress" runat="server" Text="Communication Address"
                                                    AssociatedControlID="txtCommunicationAddress"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCommunicationAddress" TextMode="MultiLine" Text=""
                                                    TabIndex="4" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblSameAsAbove" Text="Same as Permanent" AssociatedControlID="chkSameAsAbove"></asp:Label>
                                                <asp:CheckBox ID="chkSameAsAbove" runat="server" />
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblCity1" runat="server" Text="City" AssociatedControlID="txtCity1"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtCity1" Text="" TabIndex="25" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblState1" Text="State" AssociatedControlID="txtState1"></asp:Label>
                                                <asp:TextBox ID="txtState1" runat="server" TabIndex="27"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblCountry1" Text="Country" AssociatedControlID="ddlCountry1"></asp:Label>
                                                <asp:DropDownList ID="ddlCountry1" runat="server" CssClass="medium" TabIndex="29">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblZipCode1" Text="ZipCode" AssociatedControlID="txtZipCode1"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtZipCode1" Text="" TabIndex="31" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblPhone1" Text="Phone" AssociatedControlID="txtPhone1"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtPhone1" Text="" TabIndex="33" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblCountryOfBirth" Text="Country Of Birth" AssociatedControlID="ddlCountryOfBirth"></asp:Label>
                                                <asp:DropDownList ID="ddlCountryOfBirth" runat="server" CssClass="medium" TabIndex="35">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="search-colapse-b">
                                <h1>
                                    Additional Information</h1>
                                <asp:ImageButton runat="server" ID="imbShowAdditionalInfo" OnClientClick="javascript:return ShowHideAdditionalInfo(1);"
                                    SkinID="imbArrowInactive" ToolTip="Show Additional Information Details " TabIndex="107" />
                                <asp:ImageButton runat="server" ID="imbHideAdditionalInfo" OnClientClick="javascript:return ShowHideAdditionalInfo();"
                                    Style="display: none" SkinID="imbArrowActive" TabIndex="170" ToolTip="Hide Additional Information Details" />
                                <asp:HiddenField ID="hdfIsContactInfoVisisble" runat="server" Value="0" />
                                <div class="clear">
                                </div>
                            </div>
                            <div id="divHideShowAdditionalInfo" style="display: none">
                                <table class="table-devide" id="Table2">
                                    <tr>
                                        <td colspan="2">
                                            <div class="divcol-S">
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label ID="lblFatherName" runat="server" Text="Father Name" AssociatedControlID="TxtFatherName"></asp:Label>
                                                <asp:TextBox ID="TxtFatherName" runat="server" TabIndex="36"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label ID="lblSpouseName" runat="server" Text="Spouse Name" AssociatedControlID="TxtSpouseName"></asp:Label>
                                                <asp:TextBox ID="TxtSpouseName" runat="server" TabIndex="38"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblBloodGroup" Text="Blood Group" AssociatedControlID="ddlBloodGroup"></asp:Label>
                                                <asp:DropDownList ID="ddlBloodGroup" runat="server" CssClass="medium" TabIndex="40">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblProfession" Text="Profession" AssociatedControlID="ddlProfession"></asp:Label>
                                                <asp:DropDownList ID="ddlProfession" runat="server" CssClass="medium" TabIndex="42">
                                                </asp:DropDownList>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="div2col-S">
                                                <asp:Label runat="server" ID="lblMotherName" Text="Mother Name" AssociatedControlID="txtMotherName"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtMotherName" Text="" TabIndex="37" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblAltContactNO" Text="Alt. Contact NO" AssociatedControlID="txtAltContactNO"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtAltContactNO" Text="" TabIndex="39" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                                <asp:Label runat="server" ID="lblNoOfchildren" Text="No Of children" AssociatedControlID="txtNoOfchildren"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtNoOfchildren" Text="" TabIndex="41" MaxLength="100"></asp:TextBox>
                                                <div class="clear">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
