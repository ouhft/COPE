<%@ Page Title="" Language="C#" MasterPageFile="~/StudyIDMasterPage.master" AutoEventWireup="true" CodeFile="EditWP4Recipient.aspx.cs" Inherits="AddEditData_EditWP4Recipient" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="conEditWP4Recipient" ContentPlaceHolderID="AddEditStudyContent" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
            <tr>
                <th colspan="2">
                    <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
                </th>
            </tr>
            <tr>
                <td style="width:50%" align="right">
                    <asp:label id="lblTrialID" runat="server"  Font-Size="Small" Text="TrialID (Donor) *" Font-Bold="true"  />
                </td>
                <td style="width:50%" align="left">
                    
                    
                    <asp:TextBox runat="server" ID="txtTrialID" Width="50%" MaxLength="7" Enabled="true"  />
                    <asp:RequiredFieldValidator ID="rfv_txtTrialID" runat="server" Display="Dynamic" ErrorMessage="Please Enter TrialID"  
                        CssClass="MandatoryFieldMessage" ControlToValidate="txtTrialID" InitialValue=""  ValidationGroup="Main">

                    </asp:RequiredFieldValidator>
                    <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblKidneyReceived" runat="server"  Font-Size="Small" Text="Kidney Received *" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rblKidneyReceived" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow" Enabled="true">
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLKidneySidesDataSource" runat="server" 
                            DataFile="~/App_Data/KidneySides.xml" XPath="/*/*" ></asp:XmlDataSource>
                     <asp:RequiredFieldValidator ID="rfv_rblKidneyReceived" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" ValidationGroup="Main"   
                          CssClass="MandatoryFieldMessage" ControlToValidate="rblKidneyReceived"  >

                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblTransplantCentre" runat="server"  Font-Size="Small" Text="Transplant Centre *" Font-Bold="true"  />
                </td>
                <td align="left">
                    
                    <asp:DropDownList ID="ddTransplantCentre"  runat="server"  AppendDataBoundItems="True" DataTextField="CentreNameMerged" DataValueField="Centre" 
                            Width="75%"  >
                    <asp:ListItem Value="0" Text="Select Transplant Centre" Selected="True"  />
                    										            
				    </asp:DropDownList>
                    <asp:SqlDataSource ID="sqldsCentreLists" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>" >

                    </asp:SqlDataSource>
                    <asp:RequiredFieldValidator ID="rfv_ddCountry" runat="server" Display="Dynamic" CssClass="MandatoryFieldMessage" ErrorMessage="Please Select a Transplant Centre"  
                         ControlToValidate="ddTransplantCentre" InitialValue="0" ValidationGroup="Main" >

                    </asp:RequiredFieldValidator>
                
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblRecipientInformedConsent" runat="server"  Font-Size="Small" Text="Has the recipient signed an informed consent form? *" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rblRecipientInformedConsent" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="false"  Enabled="true" >
                    
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLMainOptionsYNDataSource" runat="server" 
                            DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
                    <asp:RequiredFieldValidator ID="rfv_rblRecipientInformedConsent" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" 
                        CssClass="MandatoryFieldMessage" ControlToValidate="rblRecipientInformedConsent" ValidationGroup="Main" >

                    </asp:RequiredFieldValidator>
                    <asp:Label runat="server" ID="lblRecipientInformedConsentMessage" Font-Size="Small" Text="" CssClass="Incomplete" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblRecipient18Year" runat="server"  Font-Size="Small" Text="Is the recipient >18y old? *" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rblRecipient18Year" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="false"  Enabled="true">
                    
                    </asp:RadioButtonList>
                
                    <asp:RequiredFieldValidator ID="rfv_rblRecipient18Year" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" 
                        CssClass="MandatoryFieldMessage" ControlToValidate="rblRecipient18Year" ValidationGroup="Main" >

                    </asp:RequiredFieldValidator>
                    <asp:Label runat="server" ID="lblRecipient18YearMessage" Font-Size="Small" Text="" CssClass="Incomplete" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblRecipientMultipleDualTransplant" runat="server"  Font-Size="Small" Text="Will recipient undergo a multiple/dual transplant? *" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rblRecipientMultipleDualTransplant" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="false" Enabled="true">
                    
                    </asp:RadioButtonList>
                
                    <asp:RequiredFieldValidator ID="rfv_rblRecipientMultipleDualTransplant" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" 
                        CssClass="MandatoryFieldMessage" ControlToValidate="rblRecipientMultipleDualTransplant" ValidationGroup="Main" >

                    </asp:RequiredFieldValidator>
                    <asp:Label runat="server" ID="lblRecipientMultipleDualTransplantMessage" Font-Size="Small" Text="" CssClass="Incomplete" />
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
                    <asp:HyperLink runat="server" ID="hlkSummaryPage" Text="Summary Page" Visible="true" />
                    <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="100px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                    <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                        ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                    </asp:ConfirmButtonExtender>
                    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                </td>
                <td align="left">
                    <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px"  CausesValidation="True" ValidationGroup="Main" UseSubmitBehavior="False" OnClick="cmdAddData_Click" Width="115px"    />
                    
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
                    CellPadding="3" GridLines="Horizontal" AllowSorting="False"   Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="TrialDetails_RecipientID" SortExpression="TrialDetails_RecipientID" HeaderText="TrialDetails_RecipientID" Visible="false"    />
                        
                        <asp:BoundField DataField="TrialIDRecipient" SortExpression="TrialIDRecipient" HeaderText="TrialID Recipient"    />
                        <asp:BoundField DataField="KidneyReceived" SortExpression="KidneyReceived" HeaderText="Kidney Received"    />
                        <asp:BoundField DataField="TransplantCentre" SortExpression="TransplantCentre" HeaderText="Transplant Centre"    />
                        <asp:BoundField DataField="RecipientInformedConsent" SortExpression="RecipientInformedConsent" HeaderText="Recipient Informed Consent"    />
                        <asp:BoundField DataField="Recipient18Year" SortExpression="Recipient18Year" HeaderText="Recipient >18 Year"    />
                        <asp:BoundField DataField="RecipientMultipleDualTransplant" SortExpression="RecipientMultipleDualTransplant" HeaderText="Multiple Dual Transplant" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="sqldsGV1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

