<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalRecipientPage.master" AutoEventWireup="true" CodeFile="AddResUse.aspx.cs" Inherits="SpecClinicalData_AddResUse" MaintainScrollPositionOnPostBack="true" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddResUse" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
        <tr>
            <th colspan="4">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:25%" align="right">
                <asp:label id="lblOccasion" runat="server"  Font-Size="Small" Text="Occasion *" Font-Bold="true"  />
            </td>
            <td colspan="3" style="width:75%" align="left">
                
                <asp:DropDownList ID="ddOccasion" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true"  
                    AutoPostBack="true" OnSelectedIndexChanged="ddOccasion_SelectedIndexChanged" Font-Bold="true"  >
                     <asp:ListItem Selected="True" Value="0" Text="Select Occasion"   />

                </asp:DropDownList>
                <asp:XmlDataSource ID="XMLResUsesDataSource" runat="server" 
                        DataFile="~/App_Data/ResUses.xml" XPath="/*/*" ></asp:XmlDataSource>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
            </td>
        </tr>
       
        <tr>
            <td style="width:30%" align="right">
                <asp:label id="lblGPAppointment" runat="server"  Font-Size="Small" Text="GP Appointment" Font-Bold="true"  />
            </td>
            <td style="width:20%"  align="left">
                <asp:TextBox ID="txtGPAppointment" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbeGPAppointment" TargetControlID="txtGPAppointment" FilterType="Numbers" FilterMode="ValidChars" />
                <asp:label id="lblValuesGPAppointment" runat="server"  Font-Size="Small" Text=""  />
            </td>
            <td style="width:30%" align="right">
                <asp:label id="lblGPHomeVisit" runat="server"  Font-Size="Small" Text="GP Home visit" Font-Bold="true"/>
            </td>
            <td style="width:20%"  align="left">
                <asp:TextBox ID="txtGPHomeVisit" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbeGPHomeVisit" TargetControlID="txtGPHomeVisit" FilterType="Numbers" FilterMode="ValidChars" />
                <asp:label id="lblValuesGPHomeVisit" runat="server"  Font-Size="Small" Text=""  />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblGPTelConversation" runat="server"  Font-Size="Small" Text="GP Telephone Conversation" Font-Bold="false"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtGPTelConversation" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbeGPTelConversation" TargetControlID="txtGPTelConversation" FilterType="Numbers" FilterMode="ValidChars" />
                <asp:label id="lblValuesGPTelConversation" runat="server"  Font-Size="Small" Text=""  />
            </td>
        
            <td align="right">
                <asp:label id="lblSpecConsultantAppointment" runat="server"  Font-Size="Small" Text="Specialist/ Consultant Appointment" Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtSpecConsultantAppointment" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbeSpecConsultantAppointment" TargetControlID="txtSpecConsultantAppointment" FilterType="Numbers" FilterMode="ValidChars" />
                <asp:label id="lblValuesSpecConsultantAppointment" runat="server"  Font-Size="Small" Text=""  />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblAETreatment" runat="server"  Font-Size="Small" Text="Treated in A&E" Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAETreatment" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbeAETreatment" TargetControlID="txtAETreatment" FilterType="Numbers" FilterMode="ValidChars" />
                <asp:label id="lblValuesAETreatment" runat="server"  Font-Size="Small" Text=""  />
            </td>
            <td align="right">
                <asp:label id="lblAmbulanceAEVisit" runat="server"  Font-Size="Small" Text="Ambulance to A&E/Hospital Visit" Font-Bold="false"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAmbulanceAEVisit" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbeAmbulanceAEVisit" TargetControlID="txtAmbulanceAEVisit" FilterType="Numbers" FilterMode="ValidChars" />

                <asp:label id="lblValuesAmbulanceAEVisit" runat="server"  Font-Size="Small" Text=""  />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblNurseHomeVisit" runat="server"  Font-Size="Small" Text="Nurse Home Visit" Font-Bold="false"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtNurseHomeVisit" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbeNurseHomeVisit" TargetControlID="txtNurseHomeVisit" FilterType="Numbers" FilterMode="ValidChars" />
                <asp:label id="lblValuesNurseHomeVisit" runat="server"  Font-Size="Small" Text=""  />
            </td>
            <td align="right">
                <asp:label id="lblNursePracticeAppointment" runat="server"  Font-Size="Small" Text="Nurse Practice Appointment" Font-Bold="false"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtNursePracticeAppointment" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbeNursePracticeAppointment" TargetControlID="txtNursePracticeAppointment" FilterType="Numbers" FilterMode="ValidChars" />
                <asp:label id="lblValuesNursePracticeAppointment" runat="server"  Font-Size="Small" Text=""  />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblPhysiotherapistAppointment" runat="server"  Font-Size="Small" Text="Physiotherapist Appointment" Font-Bold="false"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtPhysiotherapistAppointment" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbePhysiotherapistAppointment" TargetControlID="txtPhysiotherapistAppointment" FilterType="Numbers" FilterMode="ValidChars" />
                <asp:label id="lblValuesPhysiotherapistAppointment" runat="server"  Font-Size="Small" Text=""  />
            </td>
            <td align="right">
                <asp:label id="lblOccupationalTherapistAppointment" runat="server"  Font-Size="Small" Text="Occupational Therapist Appointment" Font-Bold="false"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtOccupationalTherapistAppointment" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbeOccupationalTherapistAppointment" TargetControlID="txtOccupationalTherapistAppointment" FilterType="Numbers" FilterMode="ValidChars" />
                <asp:label id="lblValuesOccupationalTherapistAppointment" runat="server"  Font-Size="Small" Text=""  />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblPsychologistAppointment" runat="server"  Font-Size="Small" Text="Psychologist Appointment" Font-Bold="false"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtPsychologistAppointment" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbePsychologistAppointment" TargetControlID="txtPsychologistAppointment" FilterType="Numbers" FilterMode="ValidChars" />
                <asp:label id="lblValuesPsychologistAppointment" runat="server"  Font-Size="Small" Text=""  />
            </td>
            <td align="right">
                <asp:label id="lblCounsellorAppointment" runat="server"  Font-Size="Small" Text="Counsellor Appointment" Font-Bold="false"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCounsellorAppointment" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbeCounsellorAppointment" TargetControlID="txtCounsellorAppointment" FilterType="Numbers" FilterMode="ValidChars" />
                <asp:label id="lblValuesCounsellorAppointment" runat="server"  Font-Size="Small" Text=""  />
            </td>
        </tr>
        <tr>            
            <td align="right">
                <asp:label id="lblAttendedDayHospital" runat="server"  Font-Size="Small" Text="Attended Day Hospital" Font-Bold="false"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtAttendedDayHospital" runat="server" MaxLength="2" Width="20px"  ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbeAttendedDayHospital" TargetControlID="txtAttendedDayHospital" FilterType="Numbers" FilterMode="ValidChars" />
                <asp:label id="lblValuesAttendedDayHospital" runat="server"  Font-Size="Small" Text=""  />
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="Resource Use Comments"   />
            </td>
            <td colspan="3" align="left">
                <asp:TextBox ID="txtComments" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" 
                     onkeyup="AutoExpandTextBox(this, event)" Rows="2" ></asp:TextBox>
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlAllDataAdded" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblAllDataAdded" runat="server"  Font-Size="Small" Text="Tick Box if Data Entry Complete"  />
                </td>
                <td align="left" colspan="3">
                    <asp:CheckBox runat="server" ID="chkAllDataAdded" AutoPostBack="true" OnCheckedChanged="chkAllDataAdded_CheckedChanged"/>
                    <asp:Label runat="server" ID="lblAllDataAddedMessage" CSSClass="Incomplete" Text=""  />
                
                </td>
            </tr>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlReasonModified" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblReasonModified" runat="server"  Font-Size="Small" Text="Enter Reason for Modifying Data *" Font-Bold="true"  />
                </td>
                <td colspan="3" align="left">
                    <asp:TextBox ID="txtReasonModified" runat="server" MaxLength="10000" Width="95%" TextMode="MultiLine" Font-Names="Arial" Font-Size="Small" 
                        onkeyup="AutoExpandTextBox(this, event)" Rows="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblReasonModifiedOld" runat="server"  Font-Size="Small" Text="Reason(s) Data Modified Earlier"  />
                </td>
                <td colspan="3" align="left">
                    <asp:label id="lblReasonModifiedOldDetails" runat="server"  Font-Size="Small" Text="" BackColor="#99CCFF" Width="95%" />
                </td>
            </tr>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlFinal" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblDataFinal" runat="server"  Font-Size="Small" Text="Tick to Mark Data as Final"  />
                </td>
                <td align="left" colspan="3">
                    <asp:CheckBox runat="server" ID="chkDataFinal" />
                </td>
            </tr>
        </asp:Panel> 
        <tr>
            <td colspan="2" align="right">
                <asp:HyperLink runat="server" ID="hlkSummaryPage" Text="Summary Page" />
                <asp:Button ID="cmdDelete" runat="server" Text="Delete" Height="36px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdDelete_Click"   />
                <asp:ConfirmButtonExtender ID="cmdDelete_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdDelete">
                </asp:ConfirmButtonExtender>
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
            </td>
            <td colspan="2" align="left">
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px"  CausesValidation="False"  UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
            </td>
            
        </tr>

    </table>
    <table>
        <tr align="center" >
            <th  colspan="4">
                <asp:Label runat="server" ID="lblGV1" Text="" />
            </th>
           
        </tr>
        <tr align="center">
            <td colspan="4">
                <asp:GridView ID="GV1" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True" OnRowDataBound="GV1_RowDataBound"   Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="ResUseID" SortExpression="ResUseID" HeaderText="ResUseID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="TrialIDRecipient" SortExpression="TrialIDRecipient" HeaderText="TrialID (Recipient)"
                            DataNavigateUrlFields="TrialID, TrialIDRecipient, ResUseID, Occasion" 
                            DataNavigateUrlFormatString="AddResUse.aspx?TID={0}&TID_R={1}&ResUseID={2}&Occasion={3}">
                        </asp:HyperLinkField>
                        <asp:TemplateField HeaderText="Occasion" Visible="false" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblOccasion"
                                    Text='<%#Bind("Occasion")%>'>
                                </asp:Label>                                
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="Occasion" SortExpression="Occasion" HeaderText="Occasion"    />
                        <asp:BoundField DataField="GPAppointment" SortExpression="GPAppointment" HeaderText="GP Appointment"    />
                        <asp:BoundField DataField="GPHomeVisit" SortExpression="GPHomeVisit" HeaderText="GP Home Visit"    />
                        <asp:BoundField DataField="SpecConsultantAppointment" SortExpression="SpecConsultantAppointment" HeaderText="Specialist/Consultant Appointment"    />
                        <asp:BoundField DataField="AETreatment" SortExpression="AETreatment" HeaderText="A & E Treatment"    />
                        
                        <asp:BoundField DataField="Date_Created" SortExpression="DateCreated" HeaderText="Date Created" Visible="false"    />
                        
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
    
</asp:Content>

