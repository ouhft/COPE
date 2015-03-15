<%@ Page Title="" Language="C#" MasterPageFile="~/StudyIDMasterPage.master" AutoEventWireup="true" CodeFile="AddWP4RecipientData.aspx.cs" Inherits="AddEditData_AddWP4RecipientData" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddWP4RecipientData" ContentPlaceHolderID="AddEditStudyContent" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    
    <asp:UpdatePanel runat="server" ID="pnl1" >
        <ContentTemplate>
            <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
            <asp:Panel runat="server" ID="pnlMain" Visible="false">
                <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
                    <tr>
                        <th colspan="2">
                            <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
                        </th>
                    </tr>
                    <tr>
                        <td style="width:50%" align="right">
                            <asp:label id="lblTrialID" runat="server"  Font-Size="Small" Text="Enter TrialID (Donor)"  />
                        </td>
                        <td style="width:50%" align="left">
                    
                    
                            <asp:TextBox runat="server" ID="txtTrialID" Width="50%" MaxLength="8" Enabled="true"  />
                            <asp:RequiredFieldValidator ID="rfv_txtTrialID" runat="server" Display="Dynamic" ErrorMessage="Please Enter TrialID"  
                                CssClass="MandatoryFieldMessage" ControlToValidate="txtTrialID" InitialValue=""  ValidationGroup="Main">

                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblKidneyReceived" runat="server"  Font-Size="Small" Text="Kidney Received"  />
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rblKidneyReceived" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
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
                            <asp:label id="lblTransplantCentre" runat="server"  Font-Size="Small" Text="Transplant Centre"  />
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
                            <asp:label id="lblRecipientInformedConsent" runat="server"  Font-Size="Small" Text="Has the recipient signed an informed consent form?"  />
                        </td>
                        <td align="left">
                            <asp:RadioButtonList ID="rblRecipientInformedConsent" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rblRecipientInformedConsent_SelectedIndexChanged" >
                    
                            </asp:RadioButtonList>
                            <asp:XmlDataSource ID="XMLMainOptionsYNDataSource" runat="server" 
                                    DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
                            <asp:RequiredFieldValidator ID="rfv_rblRecipientInformedConsent" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" 
                                CssClass="MandatoryFieldMessage" ControlToValidate="rblRecipientInformedConsent" ValidationGroup="Main" >

                            </asp:RequiredFieldValidator>
                            <asp:Label runat="server" ID="lblRecipientInformedConsentMessage" Font-Size="Small" Text="" CssClass="Incomplete" />
                        </td>
                    </tr>
                    <asp:Panel runat="server" ID="pnlInclusionCriteria" Visible="false" >
                        <tr>
                            <td align="right">
                                <asp:label id="lblRecipient18Year" runat="server"  Font-Size="Small" Text="Is the recipient >18y old?"  />
                            </td>
                            <td align="left">
                                <asp:RadioButtonList ID="rblRecipient18Year" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rblRecipient18Year_SelectedIndexChanged" >
                    
                                </asp:RadioButtonList>
                
                                <asp:RequiredFieldValidator ID="rfv_rblRecipient18Year" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" 
                                    CssClass="MandatoryFieldMessage" ControlToValidate="rblRecipient18Year" ValidationGroup="Inclusion" >

                                </asp:RequiredFieldValidator>
                                <asp:Label runat="server" ID="lblRecipient18YearMessage" Font-Size="Small" Text="" CssClass="Incomplete" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:label id="lblRecipientMultipleDualTransplant" runat="server"  Font-Size="Small" Text="Will recipient undergo a multiple/dual transplant?"  />
                            </td>
                            <td align="left">
                                <asp:RadioButtonList ID="rblRecipientMultipleDualTransplant" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rblRecipientMultipleDualTransplant_SelectedIndexChanged" >
                    
                                </asp:RadioButtonList>
                
                                <asp:RequiredFieldValidator ID="rfv_rblRecipientMultipleDualTransplant" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" 
                                    CssClass="MandatoryFieldMessage" ControlToValidate="rblRecipientMultipleDualTransplant" ValidationGroup="Inclusion" >

                                </asp:RequiredFieldValidator>
                                <asp:Label runat="server" ID="lblRecipientMultipleDualTransplantMessage" Font-Size="Small" Text="" CssClass="Incomplete" />
                            </td>
                        </tr>
                    </asp:Panel>
                    
                    <tr>
                        <td align="right">
                            <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="100px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                            <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                                ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                            </asp:ConfirmButtonExtender>
                    
                        </td>
                        <td align="left">
                            <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px"  CausesValidation="True" ValidationGroup="Main" UseSubmitBehavior="False" OnClick="cmdAddData_Click" Width="115px"    />
                            <asp:ConfirmButtonExtender ID="cmdAddData_ConfirmButtonExtender" runat="server" 
                                ConfirmText="" Enabled="True" TargetControlID="cmdAddData">
                            </asp:ConfirmButtonExtender>
                        </td>
            
                    </tr>
                </table>
            </asp:Panel>
    
            <asp:Panel runat="server" ID="pnlTrialIDRecipient" Visible="false">
                <table style="width:850px">
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
                                    <asp:HyperLinkField DataTextField="TrialIDRecipient" SortExpression="TrialIDRecipient" HeaderText="TrialIDRecipient"
                                        DataNavigateUrlFields="TrialID, TrialIDRecipient" 
                                        DataNavigateUrlFormatString="~/SpecClinicalData/ViewSummaryRecipient.aspx?TID={0}&TID_R={1}">
                                    </asp:HyperLinkField>
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

            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

