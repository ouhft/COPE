<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddDonorGeneralProcedureData.aspx.cs" Inherits="SpecClinicalData_AddDonorGeneralProcedureData" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddDonorGeneralProcedureData" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="2" style="width: 851px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:40%" align="right">
                <asp:label id="lblTransplantTechnician" runat="server"  Font-Size="Small" Text="Name of Transplant Technician"  />
            </td>
            <td style="width:60%"  align="left">
                <asp:TextBox ID="txtTransplantTechnician" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
                <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblTransplantCoordinatorPhone" runat="server"  Font-Size="Small" Text="Phone Call from Transplant Coordinator (Date/Time)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtTransplantCoordinatorPhoneDate" runat="server" MaxLength="10" Width="80px"  ></asp:TextBox>
                <asp:CalendarExtender ID="txtTransplantCoordinatorPhoneDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" 
                    TargetControlID="txtTransplantCoordinatorPhoneDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server"></asp:ToolkitScriptManager>
                <asp:TextBox ID="txtTransplantCoordinatorPhoneTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtTransplantCoordinatorPhoneTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtTransplantCoordinatorPhoneTime">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblTransplantCoordinator" runat="server"  Font-Size="Small" Text="Name of Transplant Coordinator"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtTransplantCoordinator" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblTelephoneTransplantCoordinator" runat="server"  Font-Size="Small" Text="Telephone Number Transplant Coordinator"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtTelephoneTransplantCoordinator" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblRetrievalHospital" runat="server"  Font-Size="Small" Text="Retrieval Hospital"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtRetrievalHospital" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblScheduledStartWithdrawl" runat="server"  Font-Size="Small" Text="Scheduled Start Withdrawl/Stop Therapy (Date/Time)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtScheduledStartWithdrawlDate" runat="server" MaxLength="10" Width="80px"   ></asp:TextBox>
                <asp:CalendarExtender ID="txtScheduledStartWithdrawlDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtScheduledStartWithdrawlDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtScheduledStartWithdrawlTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtScheduledStartWithdrawlTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtScheduledStartWithdrawlTime">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblTechnicianArrivalHub" runat="server"  Font-Size="Small" Text="Technician Arrival at Hub  (Date/Time)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtTechnicianArrivalHubDate" runat="server" MaxLength="10" Width="80px"  ></asp:TextBox>
                <asp:CalendarExtender ID="txtTechnicianArrivalHubDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtTechnicianArrivalHubDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtTechnicianArrivalHubTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtTechnicianArrivalHubTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtTechnicianArrivalHubTime">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblIceBoxesFilled" runat="server"  Font-Size="Small" Text="Ice Boxes filled with Sufficient Ice (Date/Time)" Font-Bold="false" />
            </td>
            <td align="left">
                <asp:TextBox ID="txtIceBoxesFilledDate" runat="server" MaxLength="10" Width="80px"   ></asp:TextBox>
                <asp:CalendarExtender ID="txtIceBoxesFilledDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtIceBoxesFilledDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtIceBoxesFilledTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtIceBoxesFilledTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtIceBoxesFilledTime">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblDepartHub" runat="server"  Font-Size="Small" Text="Departure Hub  (Date/Time)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtDepartHubDate" runat="server" MaxLength="10" Width="80px"  ></asp:TextBox>
                <asp:CalendarExtender ID="txtDepartHubDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDepartHubDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtDepartHubTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtDepartHubTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtDepartHubTime">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblArrivalDonorHospital" runat="server"  Font-Size="Small" Text="Arrival Donor Hospital  (Date/Time)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtArrivalDonorHospitalDate" runat="server" MaxLength="10" Width="80px" ></asp:TextBox>
                <asp:CalendarExtender ID="txtArrivalDonorHospitalDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtArrivalDonorHospitalDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtArrivalDonorHospitalTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtArrivalDonorHospitalTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtArrivalDonorHospitalTime">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="Comments"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtComments" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" ></asp:TextBox>
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlAllDataAdded" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblAllDataAdded" runat="server"  Font-Size="Small" Text="Tick Box if Data Entry Complete"  />
                </td>
                <td align="left">
                    <asp:CheckBox runat="server" ID="chkAllDataAdded" AutoPostBack="true" OnCheckedChanged="chkAllDataAdded_CheckedChanged"/>
                    <asp:Label runat="server" ID="lblAllDataAddedMessage"  Text=""  CssClass="Incomplete"/>
                
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
    </table>
    <table  cellpadding="2" cellspacing="2" border="0" style="width: 851px"> 
        <tr>
            <td style="width:40%" align="right">
                <asp:HyperLink runat="server" ID="hlkSummaryPage" Text="Summary Page" />
                <asp:Button ID="cmdDelete" runat="server" Text="Delete" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdDelete_Click"  />
                <asp:ConfirmButtonExtender ID="cmdDelete_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdDelete">
                </asp:ConfirmButtonExtender>
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
            </td>
            <td style="width:60%" align="left">
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px"   UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
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
                        <asp:BoundField DataField="DonorGeneralProcedureDataID" SortExpression="DonorGeneralProcedureDataID" HeaderText="DonorGeneralProcedureDataID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID" Visible="false"
                            DataNavigateUrlFields="TrialID, DonorGeneralProcedureDataID" 
                            DataNavigateUrlFormatString="AddDonorGeneralProcedureData.aspx?TID={0}&DonorGeneralProcedureDataID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="TrialID" SortExpression="TrialID" HeaderText="TrialID"    />
                        <asp:BoundField DataField="TransplantCoordinatorPhone" SortExpression="TransplantCoordinatorPhone" HeaderText="Transplant Coordinator Phone (Date/Time)"    />
                        <asp:BoundField DataField="ScheduledStartWithdrawl" SortExpression="ScheduledStartWithdrawl" HeaderText="Start Withdrawl (Date/Time)"    />
                        <asp:BoundField DataField="TechnicianArrivalHub" SortExpression="TechnicianArrivalHub" HeaderText="Technician Arrival Hub (Date/Time)"    />
                        <asp:BoundField DataField="IceBoxesFilled" SortExpression="IceBoxesFilled" HeaderText="IceBoxesFilled (Date/Time)"    />
                        <asp:BoundField DataField="DepartHub" SortExpression="DepartHub" HeaderText="Depart Hub (Date/Time)"    />
                        <asp:BoundField DataField="ArrivalDonorHospital" SortExpression="ArrivalDonorHospital" HeaderText="Arrival Donor Hospital (Date/Time)"    />
                        
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="sqldsGV1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

