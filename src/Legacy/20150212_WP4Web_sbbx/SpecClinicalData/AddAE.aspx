<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalRecipientPage.master" AutoEventWireup="true" CodeFile="AddAE.aspx.cs" Inherits="SpecClinicalData_AddAE" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddAE" ContentPlaceHolderID="SpecimenContents" Runat="Server">
        <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
        
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>

        
        <tr>
            <td align="right" style="width:65%">
                <asp:label id="lblResultDeath" runat="server"  Font-Size="Small" Text="Did the adverse event lead to death?  *"  Font-Bold="true" />
            </td>
            <td align="left" style="width:35%">
                <asp:RadioButtonList ID="rblResultDeath" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" >
                    
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLMainOptionsYNDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
                <asp:RequiredFieldValidator runat="server" ID="rfv_ResultDeath" ControlToValidate="rblResultDeath" Display="Dynamic" ErrorMessage="Please Select an Option" />

            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblLTInjury" runat="server"  Font-Size="Small" Text="Did it result in a life-threatening illness or injury? *"   Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblLTInjury" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator runat="server" ID="rfv_rblLTInjury" ControlToValidate="rblLTInjury" Display="Dynamic" ErrorMessage="Please Select an Option" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblPermanentImpairement" runat="server"  Font-Size="Small" Text="Did it result in a permanent impairment of a body structure or a body function? *"   Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblPermanentImpairement" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator runat="server" ID="rfv_rblPermanentImpairement" ControlToValidate="rblPermanentImpairement" Display="Dynamic" ErrorMessage="Please Select an Option" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblInPatientCareHospitalisation" runat="server"  Font-Size="Small" Text="Did it require in-patient care or prolongation of hospitalisation? *"   Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblInPatientCareHospitalisation" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator runat="server" ID="rfv_rblInPatientCareHospitalisation" ControlToValidate="rblInPatientCareHospitalisation" Display="Dynamic" ErrorMessage="Please Select an Option" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblSurgicalIntervention" runat="server"  Font-Size="Small" Text="Did it result in medical or surgical intervention to prevent life-threatening illness or injury or permanent impairment to a body structure or a body function? *"   Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblSurgicalIntervention" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                <asp:BalloonPopupExtender ID="bpeSurgicalIntervention" runat="server" TargetControlID="rblSurgicalIntervention"
                            BalloonPopupControlID="pnlSurgicalIntervention"
                            Position="BottomRight" 
                            BalloonStyle="Rectangle"
                            BalloonSize="Medium"
                            DisplayOnClick="true" DisplayOnFocus="false" DisplayOnMouseOver="true" 
                />
                <asp:Panel ID="pnlSurgicalIntervention" runat="server">
                    <asp:label id="lblPopUpSurgicalIntervention" runat="server"  Font-Size="Small" Text=""  />
                </asp:Panel>
                <asp:RequiredFieldValidator runat="server" ID="rfv_rblSurgicalIntervention" ControlToValidate="rblSurgicalIntervention" Display="Dynamic" ErrorMessage="Please Select an Option" />
            </td>
        </tr>
        <tr>
            <td align="right">
                
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            </td>
            <td align="left">
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px"  CausesValidation="False"  UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
            </td>
            
        </tr>
    </table>
</asp:Content>

