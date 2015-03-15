<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddDonorOperationData.aspx.cs" Inherits="SpecClinicalData_AddDonorOperationData" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddDonorOperationData" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages"  CssClass="Caution" Width="100%" />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="upnl1">
        <ContentTemplate>
    <table rules="all" cellpadding="2" cellspacing="2" border="2" style="width: 851px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:40%" align="right">
                <asp:label id="lblWithdrawlLifeSupportTreatment" runat="server"  Font-Size="Small" Text="Withdrawal Life Support Treatment (Date/Time)" Font-Bold="true"  />
            </td>
            <td style="width:60%"  align="left">
                <asp:TextBox ID="txtWithdrawlLifeSupportTreatmentDate" runat="server" MaxLength="10" Width="80px" ></asp:TextBox>
                <asp:CalendarExtender ID="txtWithdrawlLifeSupportTreatmentDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtWithdrawlLifeSupportTreatmentDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtWithdrawlLifeSupportTreatmentTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtWithdrawlLifeSupportTreatmentTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtWithdrawlLifeSupportTreatmentTime">
                </asp:MaskedEditExtender>
                <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblSystolicArterialPressureBelow50" runat="server"  Font-Size="Small" Text="Systolic Arterial Pressure Below 50mmHg (Date/Time)" Font-Bold="false" />
            </td>
            <td align="left">
                <asp:TextBox ID="txtSystolicArterialPressureBelow50Date" runat="server" MaxLength="10" Width="80px" ></asp:TextBox>
                <asp:CalendarExtender ID="txtSystolicArterialPressureBelow50Date_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtSystolicArterialPressureBelow50Date" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtSystolicArterialPressureBelow50Time" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtSystolicArterialPressureBelow50Time_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtSystolicArterialPressureBelow50Time">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblStartNoTouchPeriod" runat="server"  Font-Size="Small" Text="Start of No Touch Period (Date/Time)" Font-Bold="true" />
            </td>
            <td align="left">
                <asp:TextBox ID="txtStartNoTouchPeriodDate" runat="server" MaxLength="10" Width="80px" ></asp:TextBox>
                <asp:CalendarExtender ID="txtStartNoTouchPeriodDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtStartNoTouchPeriodDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtStartNoTouchPeriodTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtStartNoTouchPeriodTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtStartNoTouchPeriodTime">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblCirculatoryArrest" runat="server"  Font-Size="Small" Text="End of Cardiac Output (Date/Time)"  Font-Bold="false"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtCirculatoryArrestDate" runat="server" MaxLength="10" Width="80px" ></asp:TextBox>
                <asp:CalendarExtender ID="txtCirculatoryArrestDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtCirculatoryArrestDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtCirculatoryArrestTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtCirculatoryArrestTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtCirculatoryArrestTime">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblLengthNoTouchPeriod" runat="server"  Font-Size="Small" Text="Length of No Touch Period (minutes)" Font-Bold="true" />
            </td>
            <td align="left">
                <asp:TextBox ID="txtLengthNoTouchPeriod" runat="server" MaxLength="3" Width="15%" ></asp:TextBox>
                <asp:CompareValidator ID="cv_txtLengthNoTouchPeriod" runat="server" Display="Dynamic" ControlToValidate="txtLengthNoTouchPeriod" Type="Integer" 
                    Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are not allowed" ValidationGroup="MainGroup" ></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblConfirmationDeath" runat="server"  Font-Size="Small" Text="Confirmation of Death (Date/Time)" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtConfirmationDeathDate" runat="server" MaxLength="10" Width="80px" ></asp:TextBox>
                <asp:CalendarExtender ID="txtConfirmationDeathDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtConfirmationDeathDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtConfirmationDeathTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtConfirmationDeathTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtConfirmationDeathTime">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblStartInSituColdPerfusion" runat="server"  Font-Size="Small" Text="Start In Situ Cold Perfusion (Date/Time)" Font-Bold="true" />
            </td>
            <td align="left">
                <asp:TextBox ID="txtStartInSituColdPerfusionDate" runat="server" MaxLength="10" Width="80px" ></asp:TextBox>
                <asp:CalendarExtender ID="txtStartInSituColdPerfusionDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtStartInSituColdPerfusionDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtStartInSituColdPerfusionTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtStartInSituColdPerfusionTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtStartInSituColdPerfusionTime">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblSystemicFlushSolutionUsed" runat="server"  Font-Size="Small" Text="Systemic (Aortic) Flush Solution Used" Font-Bold="true" />
            </td>
            <td align="left">
                <asp:DropDownList ID="ddSystemicFlushSolutionUsed" runat="server" DataTextField="Text" DataValueField="Value" 
                    AutoPostBack="true" AppendDataBoundItems="true" Width="50%" OnSelectedIndexChanged="ddSystemicFlushSolutionUsed_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Text="Select Systemic Flush Solution" Value="0"  />   
                </asp:DropDownList>
                <asp:XmlDataSource ID="XmlFlushSolutionsDataSource" runat="server" 
                        DataFile="~/App_Data/FlushSolutions.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlSystemicFlushSolutionUsedOther" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblSystemicFlushSolutionUsedOther" runat="server"  Font-Size="Small" Text="Systemic Flush Solution Used (If Other) *" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtSystemicFlushSolutionUsedOther" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_txtSystemicFlushSolutionUsedOther" runat="server" Display="Dynamic" ErrorMessage="Please Enter a Value" 
                        CssClass="Caution" ControlToValidate="txtSystemicFlushSolutionUsedOther" ValidationGroup="ValSystemicFlushSolutionUsedOther"   >

                </asp:RequiredFieldValidator>
                </td>
            </tr>

        </asp:Panel>
        
        <tr>
            <td align="right">
                <asp:label id="lblPreservationSolutionColdPerfusion" runat="server"  Font-Size="Small" Text="Preservation Solution Used for Cold Perfusion" Font-Bold="true" />
            </td>
            <td align="left">
                <asp:DropDownList ID="ddPreservationSolutionColdPerfusion" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" 
                    AutoPostback="true" Width="50%" OnSelectedIndexChanged="ddPreservationSolutionColdPerfusion_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Text="Select Preservation Solution Used" Value="0"  />   
                </asp:DropDownList>
                <asp:XmlDataSource ID="XmlDataSource1" runat="server" 
                        DataFile="~/App_Data/FlushSolutions.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlPreservationSolutionColdPerfusionOther" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblPreservationSolutionColdPerfusionOther" runat="server"  Font-Size="Small" Text="Preservation Solution Used for Cold Perfusion (If Other) *" Font-Bold="true"/>
                </td>
                <td align="left">
                    <asp:TextBox ID="txtPreservationSolutionColdPerfusionOther" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_txtPreservationSolutionColdPerfusionOther" runat="server" Display="Dynamic" ErrorMessage="Please Enter a Value" 
                        CssClass="Caution" ControlToValidate="txtPreservationSolutionColdPerfusionOther" ValidationGroup="ValPreservationSolutionColdPerfusion"   >

                    </asp:RequiredFieldValidator>
                </td>
            </tr>
        </asp:Panel>
        
        
        <tr>
            <td align="right">
                <asp:label id="lblHeparin" runat="server"  Font-Size="Small" Text="Heparin" Font-Bold="false" />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblHeparin" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XmlMainOptionsDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
                <asp:XmlDataSource ID="XMLMainOptionsYNDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
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
                <td align="left"  colspan="2">
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
                <td align="left"  colspan="2">
                    <asp:TextBox ID="txtReasonModified" runat="server" MaxLength="10000" Width="95%" TextMode="MultiLine" Font-Names="Arial" Font-Size="Small" 
                        onkeyup="AutoExpandTextBox(this, event)" Rows="2"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblReasonModifiedOld" runat="server"  Font-Size="Small" Text="Reason(s) Data Modified Earlier"  />
                </td>
                <td align="left"  colspan="2">
                    <asp:label id="lblReasonModifiedOldDetails" runat="server"  Font-Size="Small" Text="" BackColor="#99CCFF" Width="95%" />
                </td>
            </tr>
        </asp:Panel> 
        <asp:Panel runat="server" ID="pnlFinal" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblDataFinal" runat="server"  Font-Size="Small" Text="Tick to Mark Data as Final"  />
                </td>
                <td align="left" colspan="2">
                    <asp:CheckBox runat="server" ID="chkDataFinal" />
                </td>
            </tr>
        </asp:Panel>
    </table>
            </ContentTemplate>
    </asp:UpdatePanel>
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
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px" CausesValidation="true" ValidationGroup="MainGroup"   UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
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
                        <asp:BoundField DataField="DonorOperatationDataID" SortExpression="DonorOperatationDataID" HeaderText="DonorOperatationDataID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID" Visible="false"
                            DataNavigateUrlFields="TrialID, DonorOperatationDataID" 
                            DataNavigateUrlFormatString="AddDonorOperationData.aspx?TID={0}&DonorOperatationDataID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="TrialID" SortExpression="TrialID" HeaderText="TrialID"    />
                        <asp:BoundField DataField="WithdrawlLifeSupportTreatment" SortExpression="WithdrawlLifeSupportTreatmentTime" HeaderText="Withdrawl Life Support Treatment"    />
                        <asp:BoundField DataField="TotalWarmIschaemicPeriod" SortExpression="TotalWarmIschaemicPeriod" HeaderText="Total Warm Ischaemic Period"    />
                        <asp:BoundField DataField="WithdrawalPeriod" SortExpression="WithdrawalPeriod" HeaderText="Withdrawal Period"    />
                        <asp:BoundField DataField="FunctionalWarmIschaemicPeriod" SortExpression="FunctionalWarmIschaemicPeriod" HeaderText="Functional Warm Ischaemic Period"    />
                        <asp:BoundField DataField="AsystolicWarmIschaemicPeriod" SortExpression="AsystolicWarmIschaemicPeriod" HeaderText="Asystolic Warm Ischaemic Period"    />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

