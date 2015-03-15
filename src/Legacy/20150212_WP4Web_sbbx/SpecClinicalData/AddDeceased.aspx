<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalRecipientPage.master" AutoEventWireup="true" CodeFile="AddDeceased.aspx.cs" Inherits="SpecClinicalData_AddDeceased" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddDeceased" ContentPlaceHolderID="SpecimenContents" Runat="Server">
        <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>        
        
        <tr>
            <td align="right" style="width:35%">
                <asp:label id="lblDeath" runat="server"  Font-Size="Small" Text="Date of Death *" Font-Bold="true" />
            </td>
            <td align="left" " style="width:65%">
                <asp:TextBox ID="txtDeathDate" runat="server" MaxLength="10" Width="75px"  AutoPostBack="true"  ></asp:TextBox>
                <asp:CalendarExtender ID="txtDeathDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDeathDate" TodaysDateFormat="dd/MM/yyyy"
                    PopupPosition="Right">
                </asp:CalendarExtender>
                
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>                
                <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" /> 
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtDeathDate" ControlToValidate="txtDeathDate" Display="Dynamic" ErrorMessage="Please Enter Date of Death" CssClass="MandatoryFieldMessage" />
                <asp:CompareValidator
                    id="cvtxtDeathDate" runat="server" 
                    Type="Date"
                    Operator="DataTypeCheck"
                    ControlToValidate="txtDeathDate" 
                    ErrorMessage="Please enter a valid Date."
                    CssClass="MandatoryFieldMessage">
                </asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblCauseDeath" runat="server"  Font-Size="Small" Text="Cause of Death *" Font-Bold="true" />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblCauseDeath" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                     >
                       
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLDeathCausesDataSource" runat="server" 
                        DataFile="~/App_Data/DeathCauses.xml" XPath="/*/*" ></asp:XmlDataSource>
                <asp:RequiredFieldValidator runat="server" ID="rfv_rblCauseDeath" ControlToValidate="rblCauseDeath" Display="Dynamic" ErrorMessage="Please Select Cause of Death" CssClass="MandatoryFieldMessage" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblCauseDeathDetails" runat="server"  Font-Size="Small" Text="Cause of Death (Multiple Selection)" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:CheckBoxList ID="cblCauseDeathDetails" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="vertical">
                    
                </asp:CheckBoxList>
                <asp:XmlDataSource ID="XMLCauseDeathDetailsDataSource" runat="server" 
                        DataFile="~/App_Data/CauseDeathDetails.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblCauseDeathDetailsOther" runat="server"  Font-Size="Small" Text="If Cause of Death Details Other"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtCauseDeathDetailsOther" runat="server" MaxLength="100" Width="95%" ></asp:TextBox>
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
                <asp:Button ID="cmdDelete" runat="server" Text="Delete" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdDelete_Click"  />
                <asp:ConfirmButtonExtender ID="cmdDelete_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdDelete">
                </asp:ConfirmButtonExtender>
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
                
            </td>
            <td align="left">
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
                        <asp:BoundField DataField="DonorLabResultsID" SortExpression="DonorLabResultsID" HeaderText="DonorLabResultsID" Visible="false"    />
                        <asp:BoundField DataField="TrialIDRecipient" SortExpression="TrialID" HeaderText="TrialID Recipient"    />
                        <asp:BoundField DataField="Death_Date" SortExpression="DeathDate" HeaderText="Death Date"    />
                        <asp:BoundField DataField="CauseDeath" SortExpression="CauseDeath" HeaderText="Cause Death" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

