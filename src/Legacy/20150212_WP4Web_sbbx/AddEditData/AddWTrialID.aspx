<%@ Page Title="" Language="C#" MasterPageFile="~/StudyIDMasterPage.master" AutoEventWireup="true" CodeFile="AddWTrialID.aspx.cs" Inherits="AddEditData_AddWTrialID" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddWTrialID" ContentPlaceHolderID="AddEditStudyContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>   

    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
        <asp:Panel runat="server" ID="pnlRandomise" Visible="false">
            <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
                <tr>
                        <th colspan="2">
                            <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
                        </th>
                    </tr>
                    <tr>
                    <td  align="right" style="width:60%">
                        <asp:label id="lblCentre" runat="server"  Font-Size="Small" Text="Retrieval Team"  />
                    </td>
                    <td align="left" style="width:40%">
                        <asp:DropDownList ID="ddCountry"  runat="server"  AppendDataBoundItems="True" DataTextField="CentreNameMerged" DataValueField="Centre" 
                                    Width="75%" AutoPostBack="true" OnSelectedIndexChanged="ddCountry_SelectedIndexChanged"  >
                            <asp:ListItem Value="0" Text="Select Retrieval Team" Selected="True"  />
                    										            
				        </asp:DropDownList>
                        <asp:SqlDataSource ID="sqldsCentreLists" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>" >

                        </asp:SqlDataSource>
                        <asp:RequiredFieldValidator ID="rfv_ddCountry" runat="server" Display="Dynamic" ErrorMessage="Please Select a Value"  CssClass="Caution" ControlToValidate="ddCountry" InitialValue="0"
                            ValidationGroup="Main" >

                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="lblTrialID" runat="server"  Font-Size="Small" Text="Enter TrialID"  />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtTrialID" runat="server" MaxLength="8" Width="25%" Text=""></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfv_txtTrialID" runat="server" Display="Dynamic" ErrorMessage="Please Enter TrialID"  CssClass="Caution" ControlToValidate="txtTrialID"
                            ValidationGroup="Main" >

                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rev_txtTrialID" runat="server" EnableClientScript="true" 
                                        ErrorMessage="Please enter Alphabet/Numbers only" CssClass="Caution" 
                                        ControlToValidate="txtTrialID" ValidationGroup="Main" ValidationExpression="^[0-9A-Z]*$"  Display="Dynamic"/>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="lblDonorID" runat="server"  Font-Size="Small" Text="Donor ET/ NHSBT Number"  />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtDonorID" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfd_txtDonorID" runat="server" Display="Dynamic" ErrorMessage="Please Enter NHSBT/ET Number"  CssClass="Caution" ControlToValidate="txtDonorID"
                            ValidationGroup="Main" >

                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rev_txtDonorID" runat="server" EnableClientScript="true" 
                                        ErrorMessage="Please enter Alphabet/Numbers only" CssClass="Caution" 
                                        ControlToValidate="txtDonorID" ValidationGroup="Main" ValidationExpression="^[0-9a-zA-Z]*$"  Display="Dynamic"/>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="lblAgeOrDateOfBirth" runat="server"  Font-Size="Small" Text="Select Age/Date of Birth to Enter"  />
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
                            <asp:label id="lblDonorAge" runat="server"  Font-Size="Small" Text="Donor Age"  />
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
                            <asp:label id="lblDonorDateOfBirth" runat="server"  Font-Size="Small" Text="Donor Date of Birth"  />
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
                            
                            <asp:RangeValidator ID="rb_txtDonorDateOfBirth" runat="server" Display="Dynamic" ControlToValidate="txtDonorDateOfBirth" Type="Date"
                                CssClass="Caution" ErrorMessage="">

                            </asp:RangeValidator>
                            
                        </td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td align="right">
                        <asp:label id="lblDonorAccept" runat="server"  Font-Size="Small" Text=""  />
                    </td>
                    <td align="left">
                        <asp:RadioButtonList ID="rblDonorAccept" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                        </asp:RadioButtonList>
                        <asp:XmlDataSource ID="XMLDonorAcceptDataSource" runat="server" 
                                DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
                        <asp:RequiredFieldValidator ID="rfv_rblDonorAccept" runat="server" Display="Dynamic" ErrorMessage="Please Select YES/NO" 
                            CssClass="Caution" ControlToValidate="rblDonorAccept" ValidationGroup="Main">

                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        </ContentTemplate>
        
    </asp:UpdatePanel>
        
    <asp:Panel runat="server" ID="pnlCommandButtons" Visible="false">
        <table cellpadding="2" cellspacing="2" border="0" style="width: 851px" > 
        <tr>
            <td align="right">
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="100px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
            </td>
            <td align="left">
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px"   UseSubmitBehavior="False" OnClick="cmdAddData_Click" Width="115px"    />
                <asp:ConfirmButtonExtender ID="cmdAddData_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdAddData">
                </asp:ConfirmButtonExtender>
            </td>
            
        </tr>
    </table>
    </asp:Panel>     
        
    
    <asp:Panel runat="server" ID="pnlData" Visible="false" >
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
                            <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="Trial ID"
                                DataNavigateUrlFields="TrialID" 
                                DataNavigateUrlFormatString="~/SpecClinicalData/ViewSummary.aspx?TID={0}">
                            </asp:HyperLinkField>
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
    </asp:Panel>
</asp:Content>

