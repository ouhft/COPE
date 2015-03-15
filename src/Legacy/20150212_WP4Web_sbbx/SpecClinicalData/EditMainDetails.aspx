<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="EditMainDetails.aspx.cs" Inherits="SpecClinicalData_EditMainDetails" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conEditMainDetails" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
                <tr>
                    <th colspan="2">
                        <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
                    </th>
                </tr>
                <tr>
                    <td  align="right" style="width:30%">
                        <asp:label id="lblCentre" runat="server"  Font-Size="Small" Text="Retrieval Team *" Font-Bold="true" />
                    </td>
                    <td align="left" style="width:70%">
                        <asp:DropDownList ID="ddCountry"  runat="server"  AppendDataBoundItems="True" DataTextField="CentreNameMerged" DataValueField="Centre" 
                                    Width="50%" Enabled="false"  >
                            <asp:ListItem Value="0" Text="Select Centre" Selected="True"  />
                    										            
				        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfv_ddCountry" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option"  
                            CssClass="Caution" ControlToValidate="ddCountry" InitialValue="0" ValidationGroup="Main">

                        </asp:RequiredFieldValidator>
                        <asp:SqlDataSource ID="sqldsCentreLists" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>" >

                        </asp:SqlDataSource>
                        <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="lblDonorID" runat="server"  Font-Size="Small" Text="Donor ET /NHSBT Number *" Font-Bold="true"  />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtDonorID" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfd_txtDonorID" runat="server" Display="Dynamic" ErrorMessage="Please Enter NHSBT/ET Number"  CssClass="Caution" ControlToValidate="txtDonorID" ValidationGroup="Main">

                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rev_txtDonorID" runat="server" EnableClientScript="true" 
                                        ErrorMessage="Please enter Alphabet/Numbers only" CssClass="Caution" 
                                        ControlToValidate="txtDonorID" ValidationGroup="Main" ValidationExpression="^[0-9a-zA-Z]*$"  Display="Dynamic"/>
                    </td>
                </tr>
                <tr>
                <td align="right">
                    <asp:label id="lblAgeOrDateOfBirth" runat="server"  Font-Size="Small" Text="Select Age/Date of Birth to Enter *" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rblAgeOrDateOfBirth" runat="server" ClientIDMode="AutoID" Display="Dynamic" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow"
                        AutoPostBack="true" OnSelectedIndexChanged="rblAgeOrDateOfBirth_SelectedIndexChanged" >
                    
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XmlAgeOrDateOfBirthDataSource" runat="server" 
                            DataFile="~/App_Data/AgeOrDateOfBirth.xml" XPath="/*/*" ></asp:XmlDataSource>

                    <asp:RequiredFieldValidator  ID="rfv_rblAgeOrDateOfBirth" Display="Dynamic"    runat="server"    ControlToValidate="rblAgeOrDateOfBirth"    ErrorMessage="Please Select an Option"  CssClass="MandatoryFieldMessage" ValidationGroup="Main" />
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlDonorAge" Visible="false">
                <tr>
                    <td align="right">
                        <asp:label id="lblDonorAge" runat="server"  Font-Size="Small" Text="Donor Age *" Font-Bold="true"  />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtDonorAge" runat="server" MaxLength="3" Width="50px"></asp:TextBox>
                        <asp:FilteredTextBoxExtender runat="server" ID="ftbetxtDonorAge" TargetControlID="txtDonorAge" FilterType="Numbers" FilterMode="ValidChars" />
                        <asp:RequiredFieldValidator  ID="rfv_txtDonorAge" Display="Dynamic"    runat="server"    ControlToValidate="txtDonorAge" ValidationGroup="DonorAge"    ErrorMessage="Please Enter Donor Age"  CssClass="Caution"  />
                        <asp:RangeValidator id="rv_txtDonorAge"
                           ControlToValidate="txtDonorAge"                   
                           MinimumValue=""
                           MaximumValue=""
                           Type="Integer"
                           EnableClientScript="True"
                           Text=""
                           runat="server"  CssClass="Caution" Display="Dynamic" ValidationGroup="Main" />
                    </td>
                </tr>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlDonorDateOfBirth" Visible="false">
                <tr>
                    <td align="right">
                        <asp:label id="lblDonorDateOfBirth" runat="server"  Font-Size="Small" Text="Donor Date of Birth *" Font-Bold="true"  />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtDonorDateOfBirth" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                        <asp:MaskedEditExtender ID="txtDonorDateOfBirth_MaskedEditExtender" runat="server" 
                                 TargetControlID="txtDonorDateOfBirth"  
                                Mask="99/99/9999"  
                                MaskType="Date"  
                                MessageValidatorTip="true" AutoComplete="False" CultureName="en-GB" UserDateFormat="DayMonthYear">
                        </asp:MaskedEditExtender>
                        <asp:MaskedEditValidator   
                            ID="mev_txtDonorDateOfBirth" CssClass="Caution"  
                            runat="server"  
                            ControlToValidate="txtDonorDateOfBirth"  
                            ControlExtender="txtDonorDateOfBirth_MaskedEditExtender"  
                            IsValidEmpty="true"  
                            EmptyValueMessage="Please Enter Donor Date of Birth"  
                            InvalidValueMessage="Please Enter Donor Date of Birth as DD/MM/YYYY" 
                            ForeColor="Red" 
                            >  
                        </asp:MaskedEditValidator>
                        <asp:RequiredFieldValidator ID="rfv_txtDonorDateOfBirth" runat="server" Display="Dynamic" ErrorMessage="Please Enter Donor Date of Birth" 
                            CssClass="Caution" ControlToValidate="txtDonorDateOfBirth" ValidationGroup="DonorDateOfBirth">

                        </asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cv_txtDonorDateOfBirth" runat="server" Display="Dynamic" ControlToValidate="txtDonorDateOfBirth" Type="Date" 
                            Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Date as DD/MM/YYYY" ></asp:CompareValidator>
                        <asp:RangeValidator ID="rb_txtDonorDateOfBirth" runat="server" Display="Dynamic" ControlToValidate="txtDonorDateOfBirth" Type="Date"
                            CssClass="Caution" ErrorMessage="">

                        </asp:RangeValidator>
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
        </ContentTemplate>
        
    </asp:UpdatePanel>      
    <table cellpadding="2" cellspacing="2" border="0" style="width: 851px" >
        <tr>
            <td align="right" style="width:30%">
                <asp:HyperLink runat="server" ID="hlkSummaryPage" Text="Summary Page" />
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="100px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
            </td>
            <td align="left">
                <asp:Button ID="cmdUpdate" runat="server" Text="Submit" Height="36px" CausesValidation="true" ValidationGroup="Main"   UseSubmitBehavior="False"  Width="115px" OnClick="cmdUpdate_Click"    />
                
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
                    CellPadding="3" GridLines="Horizontal"   Width="95%"> 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="TrialDetailsID" SortExpression="TrialDetailsID" HeaderText="TrialDetailsID" Visible="false"   />
                        <asp:BoundField DataField="TrialID" SortExpression="TrialID" HeaderText="TrialID"   />
                        <asp:BoundField DataField="DonorID" SortExpression="ContactDetails" HeaderText="DonorID"   />
                        <asp:BoundField DataField="AgeOrDateOfBirth" SortExpression="AgeOrDateOfBirth" HeaderText="Age/ Date Of Birth"    />
                        <asp:BoundField DataField="Date_OfBirthDonor" SortExpression="DateOfBirthDonor" HeaderText="Date Of Birth Donor"    />
                        <asp:BoundField DataField="DonorAge" SortExpression="DonorAge" HeaderText="Age Donor"    />
                        
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="sqldsGV1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

