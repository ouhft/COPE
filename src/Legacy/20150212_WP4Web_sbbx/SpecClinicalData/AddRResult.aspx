<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddRResult.aspx.cs" Inherits="SpecClinicalData_AddRResult" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddRResult" ContentPlaceHolderID="SpecimenContents" Runat="Server">
        <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table cellpadding="0" cellspacing="0" border="1" style="width: 851px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:40%" align="right">
                <asp:label id="lblLeftKidney" runat="server"  Font-Size="Small" Text="Left Kidney (Randomisation Arm) *" Font-Bold="true" />
            </td>
            <td style="width:60%" align="left">
                <asp:DropDownList ID="ddLeftKidneyPreservationModality"  runat="server"  AppendDataBoundItems="True" DataTextField="Text" DataValueField="Value"  Enabled="true"
                            Width="75%"   >
                    <asp:ListItem Value="0" Text="Select Randomisation Arm Left Kidney" Selected="True"  />
                    										            
				</asp:DropDownList>
                <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblRightKidney" runat="server"  Font-Size="Small" Text="Right Kidney (Randomisation Arm) *" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:DropDownList ID="ddRightKidneyPreservationModality"  runat="server"  AppendDataBoundItems="True" DataTextField="Text" DataValueField="Value"  Enabled="true"
                            Width="75%"   >
                    <asp:ListItem Value="0" Text="Select Randomisation Arm Right Kidney" Selected="True"  />
                    										            
				</asp:DropDownList>
                <asp:XmlDataSource ID="XMLPreservationModalitiesDataSource" runat="server" 
                        DataFile="~/App_Data/PreservationModalities.xml" XPath="/*/*" ></asp:XmlDataSource>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblDateCreated" runat="server"  Font-Size="Small" Text="Date/Time Randomised *" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtDateCreated" runat="server" MaxLength="10" Width="80px" AutoPostBack="false"   ></asp:TextBox>
                <asp:CalendarExtender ID="txtDateCreated_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateCreated" 
                    PopupPosition="Right">
                </asp:CalendarExtender>
                
                <asp:TextBox ID="txtTimeCreated" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtTimeCreated_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" ClearMaskOnLostFocus="true" 
                    TargetControlID="txtTimeCreated" UserTimeFormat="TwentyFourHour" ClearTextOnInvalid="True">
                </asp:MaskedEditExtender>
                <asp:MaskedEditValidator   
                        ID="txtTimeCreated_MaskedEditValidator"  
                        runat="server"  
                        ControlToValidate="txtTimeCreated"  
                        ControlExtender="txtTimeCreated_MaskedEditExtender"  
                        IsValidEmpty="true"  
                        EmptyValueMessage="Please Enter Time"  
                        InvalidValueMessage="Inputted Time Not valid"  ForeColor="Red" 
                    >  
                    </asp:MaskedEditValidator>
            </td>
        </tr>
        <tr align="right">
            <td  >
                <asp:label id="lblComments" runat="server" Text="Comments" />
            </td>
            <td align="left">
                <asp:TextBox runat="server" ID="txtComments" Width="95%" TextMode="MultiLine" MaxLength="500" />
                
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
        
        <tr>
            <td align="right">
                <asp:HyperLink runat="server" ID="hlkSummaryPage" Text="Summary Page" />
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="100px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            </td>
            <td align="left">
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px"  CausesValidation="False"  UseSubmitBehavior="False" OnClick="cmdAddData_Click" Width="115px"    />
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
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True"   Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="KidneyRID" SortExpression="KidneyRID" HeaderText="KidneyRID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="MainTrialID" SortExpression="MainTrialID" HeaderText="TrialID" Visible="false"
                            DataNavigateUrlFields="MainTrialID, KidneyRID" 
                            DataNavigateUrlFormatString="AddRTrial.aspx?TID={0}&KidneyRID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="MainTrialID" SortExpression="MainTrialID" HeaderText="TrialID"    />
                        <asp:BoundField DataField="MainDonorID" SortExpression="MainDonorID" HeaderText="DonorID"    />
                        <asp:BoundField DataField="Date_OfBirthDonor" SortExpression="DateOfBirthDonor" HeaderText="Donor DateOfBirth"    />
                        <asp:BoundField DataField="LeftRandomisationArm" SortExpression="LeftRandomisationArm" HeaderText="Left Kidney Randomisation Arm"    />
                        <asp:BoundField DataField="RightRandomisationArm" SortExpression="RightRandomisationArm" HeaderText="Right Kidney Randomisation Arm"    />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

