<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionImport.ascx.cs"
    Inherits="ERPSMS_v01.Journalize.UserControls.TransactionImport" %>
<%--   <%@ Register Assembly="ERP.Utilities" Namespace="ERP.Utilities.Validations" TagPrefix="cc1" %>--%>
<script type="text/javascript" language="javascript">

    function ValidatePageNow(valGroup) {
        if (typeof (Page_ClientValidate) == 'function') {
            //For Amount validation
            //For finding and removing duplicate and other group validation controls
            CheckValidationDuplicate(valGroup);
            //For Script validating the Page
            Page_ClientValidate(valGroup);
        }
        if (!Page_IsValid) {

            $("[id$=litErrorMsg]").hide();
            ShowErrorMessage($("#diverror").html(), '<%= Resources.Messages.Information %>');
            return false;  //Page is invalid -- stop right here
        }
        else {
            //everythings ok --- Call your function & do your stuff
            return true;
        }
    }


    //For finding and removing duplicate and other group validation controls
    //Array of present validations
    var validationArrayGroup;
    function CheckValidationDuplicate(valGroup) {
        validationArrayGroup = new Array();
        //Traversing from bottom through all the validation controls in the page
        for (var i = Page_Validators.length - 1; i >= 0; i--) {
            if (typeof (Page_Validators[i].validationGroup) == "string") {
                if (valGroup == Page_Validators[i].validationGroup) {
                    //checks if the control is already in the validation array
                    if (!CheckValidationExists(Page_Validators[i].id)) {
                        //insert new conrol to the Array of present validations
                        validationArrayGroup.push(Page_Validators[i].id);
                    }
                    //remove if control is already in Array of present validations
                    else {
                        Page_Validators.splice(i, 1);
                    }
                }
                //remove control if not in group
                else {
                    Page_Validators.splice(i, 1);
                }
            }
        }
    }

    //For checking if validation control in Array of present validations
    function CheckValidationExists(id) {
        for (var i in validationArrayGroup) {
            if (validationArrayGroup[i] == id) {
                return true;
            }
        }
        return false;
    }

</script>
<asp:UpdatePanel runat="server" ID="aupdpnlTransactionImport">
    <ContentTemplate>
        <div class="content-wrapper">

            <table class="table-devide">
                <tr>
                    <td>

                  <%--  <div id="divDIRPaymentImport" class="fileupload-strip" runat="server">
                    <h4>
                     <%= GetLocalResourceObject("lblDataImport").ToString()%>
                    </h4>--%>

                        <div id="divDIRPaymentImport" class="div2col-S">
                            <asp:Label ID="lblSourceFile" runat="server" Text="<%$ resources: SourceFile %>"
                                AssociatedControlID="fupImport" />
                            <%-- <asp:Label runat="server" ID="lblSourceFile" Text="TEXT" AssociatedControlID="fupImport"></asp:Label>--%>
                            <asp:FileUpload ID="fupImport" runat="server" TabIndex="7" />
                            <asp:RequiredFieldValidator ID="vrfFileUpload" CssClass="star" SetFocusOnError="true"
                                ValidationGroup="upload" EnableClientScript="true" runat="server" ControlToValidate="fupImport"
                                Display="Dynamic" Text="*" ErrorMessage="<%$ resources:Err_File_Upload %>">
                            </asp:RequiredFieldValidator>
                            <asp:Button runat="server" ID="btnImport" Text="<%$ resources: Import %>" ToolTip="<%$ resources: Import %>"
                                CommandName="IMPORT" OnClick="ActionHandler" ValidationGroup="Land" TabIndex="8" OnClientClick="javascript:ValidatePageNow('upload')" />
                                <%--OnClientClick="javascript:ValidatePageNow('upload')" />--%>
                            <div style="font-weight: bold; margin-bottom: 3px;">
                                <asp:Literal ID="lblNote" runat="server" Visible="false" EnableTheming="false"></asp:Literal>
                            </div>
                        </div>
                       <%-- </div>--%><!--fileupload-strip-->



                    </td>
                </tr>
            </table>
        </div>
        <div id="diverror" style="display: none">
            <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star">
            </asp:Label>
            <asp:ValidationSummary ID="vvsLanding" ValidationGroup="upload" runat="server" />
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnImport" />
    </Triggers>
</asp:UpdatePanel>
