<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddKidneyProcedure.aspx.cs" Inherits="SpecClinicalData_AddKidneyProcedure" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddKidneyProcedure" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                    
    <asp:UpdatePanel runat="server" ID="upnl1">
        <ContentTemplate>
            <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
            <table rules="all" cellpadding="2" cellspacing="2" border="2" style="width: 851px">
                <tr>
                    <th colspan="2">
                        <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
                    </th>
                </tr>
                <tr>
                    <td style="width:40%" align="right">
                        <asp:label id="lblSide" runat="server"  Font-Size="Small" Text="Kidney Side *" Font-Bold="true"  />
                    </td>
                    <td style="width:65%" align="left">
                        <asp:DropDownList ID="ddSide" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="35%"  
                                AutoPostBack="true" OnSelectedIndexChanged="ddSide_SelectedIndexChanged"  >
                            <asp:ListItem Selected="True" Text="Select a Side" Value="0" />
                           
                        </asp:DropDownList>
                        <asp:XmlDataSource ID="XMLKidneySidesDataSource" runat="server" 
                                DataFile="~/App_Data/KidneySides.xml" XPath="/*/*" ></asp:XmlDataSource>
                        <asp:RequiredFieldValidator runat="server" ID="rfv_ddSide" ControlToValidate="ddSide" InitialValue="0" Display="Dynamic" ErrorMessage="Please Select Side" 
                                            CssClass="MandatoryFieldMessage" ValidationGroup="Main" />

                        <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
                
                    </td>
                </tr>
                <asp:Panel ID="pnlSideSelected" runat="server" Visible="false">

        
                    <tr>
                        <td  align="right">
                            <asp:label id="lblTransplantTechnician" runat="server"  Font-Size="Small" Text="Name of Transplant Technician"  />
                        </td>
                        <td   align="left">
                            <asp:TextBox ID="txtTransplantTechnician" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblDonorTechnicianPhoneDate" runat="server"  Font-Size="Small" Text="Phone Call Technician Donor Procedure (Date/Time)"  />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtDonorTechnicianPhoneDate" runat="server" MaxLength="10" Width="80px"   ></asp:TextBox>
                            <asp:CalendarExtender ID="txtDonorTechnicianPhoneDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDonorTechnicianPhoneDate">
                            </asp:CalendarExtender>
                            <asp:TextBox ID="txtDonorTechnicianPhoneTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                            <asp:MaskedEditExtender ID="txtDonorTechnicianPhoneTime_MaskedEditExtender" runat="server" 
                                Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                TargetControlID="txtDonorTechnicianPhoneTime">
                            </asp:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblTechnicianDonorProcedure" runat="server"  Font-Size="Small" Text="Name of Colleague Technician involved in Donor Procedure"  />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTechnicianDonorProcedure" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblTransplantHospital" runat="server"  Font-Size="Small" Text="Transplant Hospital"  />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTransplantHospital" runat="server" MaxLength="100" Width="25%" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblTransplantHospitalContact" runat="server"  Font-Size="Small" Text="Transplant Hospital Operating Theatre Contact Person"  />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTransplantHospitalContact" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblTransplantHospitalContactPhone" runat="server"  Font-Size="Small" Text="Phone Number Transplant Hospital Operating Theatre Contact Person"  />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTransplantHospitalContactPhone" runat="server" MaxLength="100" Width="25%" ></asp:TextBox>
                        </td>
                    </tr>
        
                    <tr>
                        <td align="right">
                            <asp:label id="lblScheduledTransplantStartDate" runat="server"  Font-Size="Small" Text="Scheduled Start Transplant Procedure (Date/Time)"  />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtScheduledTransplantStartDate" runat="server" MaxLength="10" Width="80px"   ></asp:TextBox>
                            <asp:CalendarExtender ID="txtScheduledTransplantStartDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" PopupPosition="Right" TargetControlID="txtScheduledTransplantStartDate">
                            </asp:CalendarExtender>
                            <asp:TextBox ID="txtScheduledTransplantStartTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                            <asp:MaskedEditExtender ID="txtScheduledTransplantStartTime_MaskedEditExtender" runat="server" 
                                Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                TargetControlID="txtScheduledTransplantStartTime">
                            </asp:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblTechnicianArrivalPerfusionCentreDate" runat="server"  Font-Size="Small" Text="Arrival Technician Perfusion Centre (Date/Time)"  />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTechnicianArrivalPerfusionCentreDate" runat="server" MaxLength="10" Width="80px"   ></asp:TextBox>
                            <asp:CalendarExtender ID="txtTechnicianArrivalPerfusionCentreDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" PopupPosition="Right" TargetControlID="txtTechnicianArrivalPerfusionCentreDate">
                            </asp:CalendarExtender>
                            <asp:TextBox ID="txtTechnicianArrivalPerfusionCentreTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                            <asp:MaskedEditExtender ID="txtTechnicianArrivalPerfusionCentreTime_MaskedEditExtender" runat="server" 
                                Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                TargetControlID="txtTechnicianArrivalPerfusionCentreTime">
                            </asp:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblTechnicianDeparturePerfusionCentreDate" runat="server"  Font-Size="Small" Text="Departure Technician Perfusion Centre (Date/Time)"  />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtTechnicianDeparturePerfusionCentreDate" runat="server" MaxLength="10" Width="80px"  ></asp:TextBox>
                            <asp:CalendarExtender ID="txtTechnicianDeparturePerfusionCentreDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" PopupPosition="Right" TargetControlID="txtTechnicianDeparturePerfusionCentreDate">
                            </asp:CalendarExtender>
                            <asp:TextBox ID="txtTechnicianDeparturePerfusionCentreTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                            <asp:MaskedEditExtender ID="txtTechnicianDeparturePerfusionCentreTime_MaskedEditExtender" runat="server" 
                                Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                TargetControlID="txtTechnicianDeparturePerfusionCentreTime">
                            </asp:MaskedEditExtender>
                        </td>
                    </tr>
       

                    <tr>
                        <td align="right">
                            <asp:label id="lblArrivalTransplantHospitalDate" runat="server"  Font-Size="Small" Text="Arrival at Transplant Hospital  (Date/Time)"  />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtArrivalTransplantHospitalDate" runat="server" MaxLength="10" Width="80px"   ></asp:TextBox>
                            <asp:CalendarExtender ID="txtArrivalTransplantHospitalDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" PopupPosition="Right" TargetControlID="txtArrivalTransplantHospitalDate">
                            </asp:CalendarExtender>
                            <asp:TextBox ID="txtArrivalTransplantHospitalTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                            <asp:MaskedEditExtender ID="txtArrivalTransplantHospitalTime_MaskedEditExtender" runat="server" 
                                Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                TargetControlID="txtArrivalTransplantHospitalTime">
                            </asp:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="Comments"  />
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtComments" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" Font-Size="Small" Font-Names="Arial"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblReallocated" runat="server"  Font-Size="Small" Text="Kidney Reallocated Another Transplant Centre" Font-Bold="true"  />
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rblReallocated" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblReallocated_SelectedIndexChanged" AutoPostBack="True">
                    
                            </asp:RadioButtonList>
                            <asp:XmlDataSource ID="XMLMainOptionsDataSource" runat="server" 
                                    DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
                            <asp:XmlDataSource ID="XMLMainOptionsYNDataSource" runat="server" 
                                    DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
                            <asp:label id="lblReallocatedMessage" runat="server"  Font-Size="Small" Text="" CssClass="Incomplete"  />
                        </td>
                    </tr>
                    <asp:Panel runat="server" ID="pnlReallocated"  >
                        <tr>
                            <td align="right">
                                <asp:label id="lblReasonReallocated" runat="server"  Font-Size="Small" Text="Reason Reallocated"  />
                            </td>
                            <td align="left">
                                <asp:RadioButtonList ID="rblReasonReallocated" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                                </asp:RadioButtonList>
                                <asp:XmlDataSource ID="XMLReallocatedReasonsDataSource" runat="server" 
                                        DataFile="~/App_Data/ReallocatedReasons.xml" XPath="/*/*" ></asp:XmlDataSource>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:label id="lblReasonReallocatedOther" runat="server"  Font-Size="Small" Text="Reason If Reallocated Other Hospital"  />
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtReasonReallocatedOther" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:label id="lblNewRecipientHospitalContact" runat="server"  Font-Size="Small" Text="New Recipient Hospital Operating Theatre Contact Person"  />
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtNewRecipientHospitalContact" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:label id="lblNewRecipientHospitalContactPhone" runat="server"  Font-Size="Small" Text="Phone New Recipient Hospital Operating Theatre Contact Person"  />
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtNewRecipientHospitalContactPhone" runat="server" MaxLength="100" Width="25%" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:label id="lblNewTransplantHospital" runat="server"  Font-Size="Small" Text="New Transplant Hospital"  />
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtNewTransplantHospital" runat="server" MaxLength="100" Width="25%" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:label id="lblNewScheduledTransplantStartDate" runat="server"  Font-Size="Small" Text="New Scheduled Start Transplant Procedure (Date/Time)"  />
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtNewScheduledTransplantStartDate" runat="server" MaxLength="10" Width="80px"  ></asp:TextBox>
                                <asp:CalendarExtender ID="txtNewScheduledTransplantStartDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" PopupPosition="Right" TargetControlID="txtNewScheduledTransplantStartDate">
                                </asp:CalendarExtender>
                                <asp:TextBox ID="txtNewScheduledTransplantStartTime" runat="server" MaxLength="5" Width="40px"  ></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtNewScheduledTransplantStartTime_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtNewScheduledTransplantStartTime">
                                </asp:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:label id="lblDepartureFirstTransplantHospitalDate" runat="server"  Font-Size="Small" Text="Departure First Transplant Hospital  (Date/Time)"  />
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtDepartureFirstTransplantHospitalDate" runat="server" MaxLength="10" Width="80px"   ></asp:TextBox>
                                <asp:CalendarExtender ID="txtDepartureFirstTransplantHospitalDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" PopupPosition="Right" TargetControlID="txtDepartureFirstTransplantHospitalDate">
                                </asp:CalendarExtender>
                                <asp:TextBox ID="txtDepartureFirstTransplantHospitalTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtDepartureFirstTransplantHospitalTime_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtDepartureFirstTransplantHospitalTime">
                                </asp:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:label id="lblArrivalNewTransplantHospitalDate" runat="server"  Font-Size="Small" Text="Arrival New Transplant Hospital  (Date/Time)"  />
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtArrivalNewTransplantHospitalDate" runat="server" MaxLength="10" Width="80px" AutoPostBack="True"  ></asp:TextBox>
                                <asp:CalendarExtender ID="txtArrivalNewTransplantHospitalDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" PopupPosition="Right" TargetControlID="txtArrivalNewTransplantHospitalDate">
                                </asp:CalendarExtender>
                                <asp:TextBox ID="txtArrivalNewTransplantHospitalTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtArrivalNewTransplantHospitalime_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtArrivalNewTransplantHospitalTime">
                                </asp:MaskedEditExtender>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:label id="lblNewComments" runat="server"  Font-Size="Small" Text="Reallocation Comments"  />
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtNewComments" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine"  Font-Size="Small" Font-Names="Arial"></asp:TextBox>
                            </td>
                        </tr>
                    </asp:Panel>
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
                    <asp:Panel runat="server" ID="pnlReasonModified" Visible="false">
                        <tr>
                            <td align="right">
                                <asp:label id="lblReasonModified" runat="server"  Font-Size="Small" Text="Enter Reason for Modifying Data *" Font-Bold="true"  />
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtReasonModified" runat="server" MaxLength="10000" Width="95%" TextMode="MultiLine" Font-Names="Arial" Font-Size="Small" 
                                    onkeyup="AutoExpandTextBox(this, event)" Rows="2"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:label id="lblReasonModifiedOld" runat="server"  Font-Size="Small" Text="Reason(s) Data Modified Earlier"  />
                            </td>
                            <td align="left">
                                <asp:label id="lblReasonModifiedOldDetails" runat="server"  Font-Size="Small" Text="" BackColor="#99CCFF" Width="95%" />
                            </td>
                        </tr>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlFinal" Visible="false">
                        <tr>
                            <td align="right">
                                <asp:label id="lblDataFinal" runat="server"  Font-Size="Small" Text="Tick to Mark Data as Final"  />
                            </td>
                            <td align="left">
                                <asp:CheckBox runat="server" ID="chkDataFinal" />
                            </td>
                        </tr>
                    </asp:Panel>
                </asp:Panel> 
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <table  cellpadding="2" cellspacing="2" border="0" style="width: 851px">
        

            <tr>
                <td   style="width:40%"align="right">
                    <asp:HyperLink runat="server" ID="hlkSummaryPage" Text="Summary Page" />
                    <asp:Button ID="cmdDelete" runat="server" Text="Delete" Height="36px" Width="75px" CausesValidation="true" ValidationGroup="Main"  UseSubmitBehavior="False" OnClick="cmdDelete_Click"  />
                    <asp:ConfirmButtonExtender ID="cmdDelete_ConfirmButtonExtender" runat="server" 
                        ConfirmText="" Enabled="True" TargetControlID="cmdDelete">
                    </asp:ConfirmButtonExtender>
                    <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                    <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                        ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                    </asp:ConfirmButtonExtender>
                </td>
                <td align="left">
                    <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px" CausesValidation="true" ValidationGroup="Main"   UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
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
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True"   Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="KidneyProcedureDataID" SortExpression="KidneyProcedureDataID" HeaderText="KidneyProcedureDataID" Visible="false"    />
                        <asp:BoundField DataField="TrialID" SortExpression="TrialID" HeaderText="TrialID"    />
                        <asp:TemplateField HeaderText="Side" Visible="false" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblSide"
                                    Text='<%#Bind("Side")%>'>
                                </asp:Label>                                
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:HyperLinkField DataTextField="Side" SortExpression="Side" HeaderText="Side"
                            DataNavigateUrlFields="TrialID, KidneyProcedureDataID, Side" 
                            DataNavigateUrlFormatString="AddKidneyProcedure.aspx?TID={0}&KidneyProcedureDataID={1}&Side={2}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="DonorTechnicianPhone" SortExpression="DonorTechnicianPhone" HeaderText="Donor Technician Phone"    />
                        <asp:BoundField DataField="TransplantHospital" SortExpression="TransplantHospital" HeaderText="Transplant Hospital"    />
                        <asp:BoundField DataField="ScheduledTransplantStart" SortExpression="ScheduledTransplantStart" HeaderText="Scheduled Transplant Start"    />
                        <asp:BoundField DataField="TechnicianArrivalPerfusionCentre" SortExpression="TechnicianArrivalPerfusionCentre" HeaderText="Technician Arrival"    />
                        <asp:BoundField DataField="TechnicianDeparturePerfusionCentre" SortExpression="TechnicianDeparturePerfusionCentre" HeaderText="Technician Departure"    />
                        <asp:BoundField DataField="ArrivalTransplantHospital" SortExpression="ArrivalTransplantHospital" HeaderText="Arrival Transplant Hospital"    />
                        <asp:BoundField DataField="Reallocated" SortExpression="Reallocated" HeaderText="Reallocated"    />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="sqldsGV1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>

</asp:Content>

