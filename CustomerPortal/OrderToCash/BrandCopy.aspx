<%@ Page Title="<%$ Resources:Captions,Title_BrandCopy %>" Language="C#" MasterPageFile="~/ERPSMS_2.Master" Theme="ClassicExt" AutoEventWireup="true" CodeBehind="BrandCopy.aspx.cs" Inherits="CustomerPortal.OrderToCash.BrandCopy" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <script type="text/javascript">
          var pageURL = window.document.URL;
          var virtualPath = '<%=(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString())%>';
          var url = pageURL.replace(location.pathname, virtualPath == "" ? "/Handlers/AutoComplete.ashx" : "/" + virtualPath + "Handlers/AutoComplete.ashx");

          function InitComponents() {             
              GrandScriptUtils.MakeAutoCompleteDDL("txtCustomer", url, "hdfCustomerID", true, true, "CUSTOMERLIST"); 
          }
          
          function AfterAutoCompleteSelect(targetControlID) {
              if (targetControlID == "txtCustomer") {                  
              }
          }
          function AfterInvalidSelect(targetControlID) {
              if (targetControlID == "txtCustomer") {
                
              }
          }
          function OnCheckBoxCheckChanged(evt) {
              var src = window.event != window.undefined ? window.event.srcElement : evt.target;
              var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
              if (isChkBoxClick) {
                  var parentTable = GetParentByTagName("table", src);
                  var nxtSibling = parentTable.nextSibling;

                  if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node 
                  {
                      if (nxtSibling.tagName.toLowerCase() == "div") //if node has children           
                      {
                          //check or uncheck children at all levels           
                          CheckUncheckChildren(parentTable.nextSibling, src.checked);
                      }
                  }
                  //check or uncheck parents at all levels           
                  CheckUncheckParents(src, src.checked);
              }
          }
          function CheckUncheckChildren(childContainer, check) {
              var childChkBoxes = childContainer.getElementsByTagName("input");
              var childChkBoxCount = childChkBoxes.length;
              for (var i = 0; i < childChkBoxCount; i++) {
                  childChkBoxes[i].checked = check;
              }
          }
          function CheckUncheckParents(srcChild, check) {
              var parentDiv = GetParentByTagName("div", srcChild);
              var parentNodeTable = parentDiv.previousSibling;



              if (parentNodeTable) {
                  var checkUncheckSwitch;

                  if (check) //checkbox checked
                  {
                      var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                      if (isAllSiblingsChecked)
                          checkUncheckSwitch = true;
                      else
                          return; //do not need to check parent if any(one or more) child not checked
                  }
                  else //checkbox unchecked
                  {
                      checkUncheckSwitch = false;
                  }

                  var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                  if (inpElemsInParentTable.length > 0) {
                      var parentNodeChkBox = inpElemsInParentTable[0];
                      parentNodeChkBox.checked = checkUncheckSwitch;
                      //do the same recursively
                      CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                  }
              }
          }
          function AreAllSiblingsChecked(chkBox) {
              var parentDiv = GetParentByTagName("div", chkBox);
              var childCount = parentDiv.childNodes.length;
              for (var i = 0; i < childCount; i++) {
                  if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                  {
                      if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                          var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                          //if any of sibling nodes are not checked, return false
                          if (!prevChkBox.checked) {
                              return false;
                          }
                      }
                  }
              }
              return true;
          }
          //utility function to get the container of an element by tagname
          function GetParentByTagName(parentTagName, childElementObj) {
              var parent = childElementObj.parentNode;
              while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                  parent = parent.parentNode;
              }
              return parent;
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
          function ValidatePageNow(valGroup) {
              if (typeof (Page_ClientValidate) == 'function') {
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
          function DisableAuto(extender) {
              $(extender).next($(".ddlSelect")).removeClass("ddlSelect").addClass("ddlSelect-disable");
              $(extender).autocomplete("option", "disabled", true);
              $(extender).attr("disabled", true);
          }
          function EnableAuto(extender) {
              $(extender).removeAttr("disabled");
              $(extender).next($(".ddlSelect")).removeClass("ddlSelect-disable").addClass("ddlSelect");
              $(extender).autocomplete("option", "disabled", false);
          }          
                  
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   <div class="fixed-buttons-normal">
        <div class="Button-container">
            <asp:Table runat="server" ID="tblButton">
                <asp:TableRow>
                    <asp:TableCell ID="SEC_ActionPanel" CssClass="SEC_ACTION" HorizontalAlign="Right">
                        <ul class="bredcrum">
                            <asp:Label runat="server" ID="lblBreadCrum" Text="<%$ resources:Breadcrumb%>"></asp:Label>
                        </ul>
                        <ul runat="server" id="pnlEntry">
                            <li>
                                <asp:Button ID="btnCopy" runat="server" SkinID="btnInner-Save" Text="<%$resources:ErpRes,Copy %>"  ToolTip="<%$resources:ErpRes,Copy %>"
                                   OnClientClick="javascript:ValidatePageNow('vgSave')" CommandName="COPY" TabIndex="23" OnClick="ActionHandler" ValidationGroup="vgSave"  />
                            </li>                           
                            <li>
                                <asp:Button runat="server" ID="btnCancel" Text="<%$resources:Controls,Cancel %>"
                                    OnClick="ActionHandler" CommandName="CANCEL" SkinID="btnInner-Cancel" ToolTip="<%$resources:Controls,Cancel %>"
                                    TabIndex="25" />
                            </li>
                        </ul>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="content-wrapper">

  <asp:Table runat="server" ID="tblPage" CssClass="asptbllinks">
   <asp:TableRow ID="PageAction_Entry" runat="server">
     <asp:TableCell>
        <div id="gridwrap">         
            <table class="table-devide">
                <tr>
                    <td>
                        <div class="div2col-S">                          
                            <asp:Label ID="lblCustomer" runat="server" Text="<%$resources:FromCustomer %>" AssociatedControlID="txtCustomer"></asp:Label>
                                            <asp:TextBox ID="txtCustomer" runat="server" CssClass="select-half margnbotm0" MaxLength="100"
                                                TabIndex="5"> </asp:TextBox>
                                            <asp:HiddenField ID="hdfCustomerID" runat="server" />
                            <div class="starwrap">
                                <asp:RequiredFieldValidator ID="vrfCustomer" CssClass="star" SetFocusOnError="true"
                                                InitialValue="<%$ resources:ErpRes, AutoDefaultValue %>" ValidationGroup="vgSave"
                                                EnableClientScript="true" runat="server" ControlToValidate="txtCustomer" Display="Dynamic"
                                                Text="*" ErrorMessage="<%$ resources:Msg_Select_Customer %>">
                                </asp:RequiredFieldValidator>                             
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="div2col-S">
                          
                        </div>
                    </td>
                </tr>                     
            </table>             
            <div class="treeview-center w46perc float-right">
                <div class="treeview max-380">
                    <h4>
                       <%= GetLocalResourceObject("CopyBrandTo").ToString()%>
                    </h4>                   
                    <div class="clear">
                    </div>
                    <asp:TreeView ID="trvCustomers" runat="server" onclick="OnCheckBoxCheckChanged(event);"
                          ShowLines="true" ExpandDepth="0" InitialExpandDepth="2" ShowCheckBoxes="Parent,Leaf">
                    </asp:TreeView>
                </div>
            </div>
        </div>  
         </asp:TableCell>
        </asp:TableRow>     
     </asp:Table> 
    </div>
    <div id="diverror" style="display: none">
        <%--Use this label to bind the server errors--%>
        <asp:Label runat="server" ID="litErrorMsg" ClientIDMode="Static" CssClass="star" />
        <asp:ValidationSummary ID="vvsUser" ValidationGroup="vgSave" runat="server" />
    </div>
    <asp:HiddenField ID="hdfIsMultiplePlant" runat="server" />
</asp:Content>