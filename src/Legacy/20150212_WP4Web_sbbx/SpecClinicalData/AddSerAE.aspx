<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalRecipientPage.master" AutoEventWireup="true" CodeFile="AddSerAE.aspx.cs" Inherits="SpecClinicalData_AddSerAE" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddSerAE" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
        
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        
        <tr>
            <td align="right" style="width:35%">
                <asp:label id="lblDateOnset" runat="server"  Font-Size="Small" Text="Date of Onset *" Font-Bold="true"  />
            </td>
            <td align="left"   style="width:65%">
                <asp:TextBox ID="txtDateOnset" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                <asp:CalendarExtender ID="txtDateOnset_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateOnset" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                <asp:label id="lblName" runat="server"  Font-Size="Small" Text="" Visible="false"  />
                <asp:label id="lblEmail" runat="server"  Font-Size="Small" Text="" Visible="false"  />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblOngoing" runat="server"  Font-Size="Small" Text="Still Ongoing *"   Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblOngoing" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblOngoing_SelectedIndexChanged">
                    
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLMainOptionsDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlDateResolution" Visible="false">
        <tr>
            <td align="right">
                <asp:label id="lblDateResolution" runat="server"  Font-Size="Small" Text="Date of Resolution"  Font-Bold="true" />
            </td>
            <td align="left">
                <asp:TextBox ID="txtDateResolution" runat="server" MaxLength="10" Width="80px" ></asp:TextBox>
                <asp:CalendarExtender ID="txtDateResolution_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateResolution" 
                    PopupPosition="Right">
                </asp:CalendarExtender>
               
            </td>
        </tr>
        </asp:Panel>
        
        <tr>
            <td align="right">
                <asp:label id="lblDescriptionEvent" runat="server"  Font-Size="Small" Text="Description of Event *"   Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtDescriptionEvent" runat="server" TextMode="MultiLine" MaxLength="500" Width="95%" Font-Names="Arial" Font-Size="Small"
                    onkeyup="AutoExpandTextBox(this, event)" Rows="2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblActionTaken" runat="server"  Font-Size="Small" Text="Action Taken"  Font-Bold="true" />
            </td>
            <td align="left">
                <asp:TextBox ID="txtActionTaken" runat="server" TextMode="MultiLine" MaxLength="500" Width="95%" Font-Names="Arial" Font-Size="Small"
                    onkeyup="AutoExpandTextBox(this, event)" Rows="2"
                    ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblOutcome" runat="server"  Font-Size="Small" Text="Outcome"  Font-Bold="true" />
            </td>
            <td align="left">
                <asp:TextBox ID="txtOutcome" runat="server" TextMode="MultiLine" MaxLength="500" Width="95%" Font-Names="Arial" Font-Size="Small"
                    onkeyup="AutoExpandTextBox(this, event)" Rows="2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" >
                <asp:label id="lblContactDetails" runat="server"  Font-Size="Small" Text="Who is the best person to contact for more information about this Serious Adverse Event? *"  Font-Bold="true" />
            </td>
            <td align="left">
                <asp:TextBox ID="txtContactName" runat="server" MaxLength="100" Width="50%"  Font-Size="Small"></asp:TextBox>
                <asp:label id="lblContactName" runat="server"  Font-Size="Small" Text="Contact Name"  />
                <br />
                <asp:TextBox ID="txtContactPhone" runat="server" MaxLength="100" Width="50%"  Font-Size="Small" ToolTip="Only Numbers are allowed"></asp:TextBox>
                <asp:label id="lblContactPhone" runat="server"  Font-Size="Small" Text="Contact Phone"  />
                <asp:FilteredTextBoxExtender runat="server" ID="ftbeContactPhone" TargetControlID="txtContactPhone" FilterType="Numbers" FilterMode="ValidChars" />
                <br />
                <asp:TextBox ID="txtContactEmail" runat="server" MaxLength="100" Width="50%"  Font-Size="Small"></asp:TextBox>
                <asp:label id="lblContactEmail" runat="server"  Font-Size="Small" Text="Contact Email"  />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblResultDeath" runat="server"  Font-Size="Small" Text="Did it result in Death?  *"  Font-Bold="true" />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblResultDeath" runat="server" DataTextField="Text" DataValueField="Value" 
                    RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblResultDeath_SelectedIndexChanged" RepeatLayout="flow" >
                    
                </asp:RadioButtonList>
                <asp:Label runat="server" ID="lblResultDeathDetails" Text="" Visible="false" CssClass="Incomplete" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblPermanentDisability" runat="server"  Font-Size="Small" Text="Did it result in Permanent Disability *"   Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblPermanentDisability" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblSignInterferenceUsualActivity" runat="server"  Font-Size="Small" Text="Did it result in a sign/symptom that interferes with subject’s usual activity? *"  Font-Bold="true" />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblSignInterferenceUsualActivity" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblWorkIncapacityInability" runat="server"  Font-Size="Small" Text="Did it result in incapacity/inability to do work? *"   Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblWorkIncapacityInability" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblResolvedWithNoSequelae" runat="server"  Font-Size="Small" Text="Did it cause signs/symptoms that resolved with no sequelae? *"  Font-Bold="true" />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblResolvedWithNoSequelae" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblDeviceDeficiency" runat="server"  Font-Size="Small" Text="Did this SAE arise from Device Deficiency? *"   Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblDeviceDeficiency" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblDeviceUserError" runat="server"  Font-Size="Small" Text="Did this SAE arise from Device User Error? *"  Font-Bold="true" />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblDeviceUserError" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlAllDataAdded" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblAllDataAdded" runat="server"  Font-Size="Small" Text="Tick Box if Data Entry Complete"  />
                </td>
                <td align="left">
                    <asp:CheckBox runat="server" ID="chkAllDataAdded" AutoPostBack="true" OnCheckedChanged="chkAllDataAdded_CheckedChanged"/>
                    <asp:Label runat="server" ID="lblAllDataAddedMessage" CSSClass="Incomplete" Text=""  />
                
                </td>
            </tr>
        </asp:Panel>
        <tr>
            <td align="right">
                
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
            </td>
            <td align="left">
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px" CausesValidation="False"  UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                <asp:ConfirmButtonExtender ID="cmdAddData_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdAddData">
                </asp:ConfirmButtonExtender>
            </td>
            
        </tr>
    </table>
    <table style="width:800px">
        <tr align="center" >
            <th  colspan="2">
                <asp:Label runat="server" ID="lblGV1" Text="" />
            </th>
           
        </tr>
        <tr align="center">
            <td colspan="2">
                <asp:GridView ID="GV1" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True"  Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="SerAEID" SortExpression="SerAEID" HeaderText="SerAEID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="TrialIDRecipient" SortExpression="TrialIDRecipient" HeaderText="TrialID (Recipient)"
                            DataNavigateUrlFields="TrialID, TrialIDRecipient, SerAEID" 
                            DataNavigateUrlFormatString="EditSerAE.aspx?TID={0}&TID_R={1}&SerAEID={2}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="SerialNumber" SortExpression="SerialNumber" HeaderText="SAE Number"    />
                        <asp:BoundField DataField="Date_Onset" SortExpression="DateOnset" HeaderText="Date Onset"    />
                        <asp:BoundField DataField="Ongoing" SortExpression="Ongoing" HeaderText="Ongoing"    />
                        <asp:BoundField DataField="Date_Resolution" SortExpression="DateResolution" HeaderText="Date Resolution"    />
                        <asp:BoundField DataField="OtherDetails" SortExpression="OtherDetails" HeaderText="Other Details"    />                       
                        
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

