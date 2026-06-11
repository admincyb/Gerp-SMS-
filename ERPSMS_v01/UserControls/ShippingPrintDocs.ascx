<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShippingPrintDocs.ascx.cs" Inherits="ERPSMS_v01.UserControls.ShippingPrintDocs" %>

  <%--Print popup window --%>
                            <div  id="divPrint" style="display: none" class="content-wrapper">
                                <table class="table-devide">
                                <tr>
                                <td colspan="3">&nbsp;</td>
                                </tr>
                                    <tr>
                                        <td align ="right" style="width:60%;">
                                           <asp:Label ID="lblSDPrint" runat ="server" Text="<%$resources:Controls,PrintShippingDocuments %>" AssociatedControlID="ddlSDPrint"></asp:Label>
                                        </td> 
                                        <td style="width:30%;">
                                                <asp:DropDownList ID="ddlSDPrint" runat="server" TabIndex="101" width="150px" onmouseover="javascript:ShowTooltip('ddlSDPrint');">
                                                </asp:DropDownList>
                                        </td>
                                        <td style="width:20%;">
                                                 <asp:Button runat="server" TabIndex="102" ID="btnPrintlistSD" Text="<%$resources:Controls,Print %>"
                                                    SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" OnClick ="ActionHandler" CommandName="PRINTGON" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align ="right" style="width:60%;" >
                                           
                                               <asp:Label ID="lblCIPrint" runat ="server" Text="<%$resources:Controls,PrintCommericalInvoice %>" AssociatedControlID="ddlSDPrint"></asp:Label>
                                        </td>
                                        <td style="width:30%;">
                                                <asp:DropDownList ID="ddlCIPrint" runat="server" TabIndex="103" width="150px" onmouseover="javascript:ShowTooltip('ddlCIPrint');">
                                                </asp:DropDownList>
                                        </td>
                                        <td style="width:20%;">
                                                 <asp:Button runat="server" TabIndex="104" ID="btnPrintlistCI" Text="<%$resources:Controls,Print %>"
                                                    SkinID="btnInner-Print" ToolTip="<%$resources:Controls,Print %>" OnClick ="ActionHandler" CommandName="PRINTCI"/>

                                        </td>
                                    </tr>
                                </table>
                            </div>
