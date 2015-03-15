<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalRecipientPage.master" AutoEventWireup="true" CodeFile="EditResUseReadmissions.aspx.cs" Inherits="SpecClinicalData_EditResUseReadmissions" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conEditResUseReadmissions" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" Font-Size="Small" ForeColor="Red" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:30%" align="right">
                <asp:label id="lblOccasion" runat="server"  Font-Size="Small" Text="Occasion"  />
            </td>
            <td style="width:70%" align="left">
                <asp:DropDownList ID="ddOccasion" runat="server" >
                     
                </asp:DropDownList>                
                 
                
            </td>
        </tr>
        <tr>
            <td style="width:30%" align="right">
                <asp:label id="lblReAdmissionType" runat="server"  Font-Size="Small" Text="Re-Admission Type"  />
            </td>
            <td style="width:70%" align="left">
                <asp:DropDownList ID="ddReAdmissionType" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" 
                    AutoPostBack="true"  OnSelectedIndexChanged="ddReAdmissionType_SelectedIndexChanged"  >
                    <asp:ListItem Selected="True" Value="0" Text="Select Re-Admission Type"   /> 
                     
                </asp:DropDownList>                
                <asp:XmlDataSource ID="XMLReAdmissionTypesDataSource" runat="server" 
                        DataFile="~/App_Data/ReAdmissionTypes.xml" XPath="/*/*" ></asp:XmlDataSource> 
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblDateAdmission" runat="server"  Font-Size="Small" Text="Date Admission"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtDateAdmission" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                <asp:CalendarExtender ID="txtDateAdmission_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateAdmission" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblDateDischarge" runat="server"  Font-Size="Small" Text="Date Discharge"  />
            </td>
            <td  align="left">
                <asp:TextBox ID="txtDateDischarge" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                <asp:CalendarExtender ID="txtDateDischarge_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateDischarge" PopupPosition="BottomRight">
                </asp:CalendarExtender>
               
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlHospitalAdmission" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblRequiredSurgery" runat="server"  Font-Size="Small" Text="Required Surgery"  />
                </td>
                <td  align="left">
                    <asp:RadioButtonList ID="rblRequiredSurgery" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" >
                       
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLMainOptionsDataSource" runat="server" 
                            DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
               
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblITU_HDU" runat="server"  Font-Size="Small" Text="ITU/HDU Admission"  />
                </td>
                <td  align="left">
                    <asp:RadioButtonList ID="rblITU_HDU" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" >
                       
                    </asp:RadioButtonList>
                
               
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblDaysHospital" runat="server"  Font-Size="Small" Text="Number of Days in ITU"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtDaysHospital" runat="server" MaxLength="45" Width="25%"  ></asp:TextBox>
                
                </td>
            </tr>

        </asp:Panel>
            
       
        <tr>
            <td align="right">
                <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="Reasons for Admission"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtComments" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" onkeyup="AutoExpandTextBox(this, event)" Rows="2"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                
                <asp:Button ID="cmdDelete" runat="server" Text="Delete" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdDelete_Click"   />
                <asp:ConfirmButtonExtender ID="cmdDelete_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdDelete">
                </asp:ConfirmButtonExtender>
            </td>
            <td align="left">
                <asp:Button ID="cmdUpdate" runat="server" Text="Update Data" Height="36px"   CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdUpdate_Click"    />
                
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
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True" OnRowDataBound="GV1_RowDataBound"   Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="RReadmissionsID" SortExpression="RReadmissionsID" HeaderText="RReadmissionsID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="Date_Admission" SortExpression="DateAdmission" HeaderText="Date Admission"
                            DataNavigateUrlFields="TrialID, TrialIDRecipient, RReadmissionsID, Occasion" 
                            DataNavigateUrlFormatString="EditResUseReadmissions.aspx?TID={0}&TID_R={1}&RReadmissionsID={2}&Occasion={3}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="Date_Discharge" SortExpression="DateDischarge" HeaderText="Date Discharge"    />
                        <asp:BoundField DataField="Occasion" SortExpression="Occasion" HeaderText="Occasion"    />
                        <asp:BoundField DataField="ReAdmissionType" SortExpression="ReAdmissionType" HeaderText="ReAdmission Type"    />
                        <asp:BoundField DataField="RequiredSurgery" SortExpression="RequiredSurgery" HeaderText="Required Surgery"    />
                        <asp:BoundField DataField="ITU_HDU" SortExpression="ITU_HDU" HeaderText="ITU HDU"    />
                        <asp:BoundField DataField="DaysHospital" SortExpression="DaysHospital" HeaderText="Days in ITU"    />
                        <asp:BoundField DataField="Comments" SortExpression="Comments" HeaderText="Comments" />
                        
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
    <table>
        <tr align="center" >
            <th  colspan="4">
                <asp:Label runat="server" ID="lblGV2" Text="" />
            </th>
           
        </tr>
        <tr align="center">
            <td colspan="4">
                <asp:GridView ID="GV2" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV2_Sorting" AllowSorting="True"    Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="ResUseID" SortExpression="ResUseID" HeaderText="ResUseID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID"
                            DataNavigateUrlFields="TrialID, TrialIDRecipient, ResUseID, Occasion" 
                            DataNavigateUrlFormatString="EditResUse.aspx?TID={0}&TID_R={1}&ResUseID={2}&Occasion={3}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="Occasion" SortExpression="Occasion" HeaderText="Occasion"    />
                        <asp:BoundField DataField="GPAppointment" SortExpression="GPAppointment" HeaderText="GP Appointment"    />
                        <asp:BoundField DataField="GPHomeVisit" SortExpression="GPHomeVisit" HeaderText="GP Home Visit"    />
                        <asp:BoundField DataField="GPTelConversation" SortExpression="GPTelConversation" HeaderText="GP Tel Conversation"    />
                        <asp:BoundField DataField="SpecConsultantAppointment" SortExpression="SpecConsultantAppointment" HeaderText="Specialist/Consultant Appointment"    />
                        <asp:BoundField DataField="AETreatment" SortExpression="AETreatment" HeaderText="A & E Treatment"    />
                        <asp:BoundField DataField="AmbulanceAEVisit" SortExpression="AmbulanceAEVisit" HeaderText="Ambulance to AE/ Hospital"    />
                        <asp:BoundField DataField="NurseHomeVisit" SortExpression="NurseHomeVisit" HeaderText="Nurse Home Visit"    />
                        <asp:BoundField DataField="NursePracticeAppointment" SortExpression="NursePracticeAppointment" HeaderText="Nurse Practice Appointment"    />
                        <asp:BoundField DataField="PhysiotherapistAppointment" SortExpression="PhysiotherapistAppointment" HeaderText="Physiotherapist Appointment"    />
                        <asp:BoundField DataField="AttendedDayHospital" SortExpression="AttendedDayHospital" HeaderText="Attended Day Hospital"    />
                        <asp:BoundField DataField="Date_Created" SortExpression="DateCreated" HeaderText="Date Created"    />
                        
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
    
</asp:Content>

