<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalRecipientPage.master" AutoEventWireup="true" CodeFile="EditAdvEvent.aspx.cs" Inherits="SpecClinicalData_EditAdvEvent" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conEditAdvEvent" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
        
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        
        <tr>
            <td align="right" style="width:35%">
                <asp:label id="lblDateAE" runat="server"  Font-Size="Small" Text="Date Adverse Event *"  Font-Bold="true"/>
            </td>
            <td align="left" valign="middle"   style="width:65%">
                <asp:TextBox ID="txtDateAE" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                <asp:CalendarExtender ID="txtDateAE_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateAE" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>                
                <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" /> 
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblAdverseEventType" runat="server"  Font-Size="Small" Text="Adverse Event Type *"  Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddAdverseEventType" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="True"
                    AutoPostBack="true" OnSelectedIndexChanged="ddAdverseEventType_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Text="Select an Adverse Event Type" Value="0" />
                </asp:DropDownList>
                <asp:XmlDataSource ID="XMLAdverseEventsDataSource" runat="server" 
                        DataFile="~/App_Data/AdverseEvents.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlRecipientInfection" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblRecipientInfectionType" runat="server"  Font-Size="Small" Text="Recipient Infection Type *"  Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddRecipientInfectionType" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="True"
                        AutoPostBack="true" OnSelectedIndexChanged="ddRecipientInfectionType_SelectedIndexChanged">
                        <asp:ListItem Selected="True" Text="Select Recipient Infection Type" Value="0" />
                    </asp:DropDownList>
                    <asp:XmlDataSource ID="XMLRecipientInfectionTypesDataSource" runat="server" 
                            DataFile="~/App_Data/RecipientInfectionTypes.xml" XPath="/*/*" ></asp:XmlDataSource>
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlRecipientInfectionTypeOther" Visible="false">
                <tr>
                    <td align="right">
                        <asp:label id="lblRecipientInfectionTypeOther" runat="server"  Font-Size="Small" Text="Recipient Infection Type (Other) *"  Font-Bold="true" />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtRecipientInfectionTypeOther" runat="server" MaxLength="100" Width="95%" Font-Size="Small"></asp:TextBox>
                    </td>
                </tr>
            </asp:Panel>
        
            <tr>
                <td align="right">
                    <asp:label id="lblRecipientInfectionOrganismBacteria" runat="server"  Font-Size="Small" Text="Recipient Infection Organism Bacteria *"  Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rblRecipientInfectionOrganismBacteria" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal"
                        AutoPostBack="true"  OnSelectedIndexChanged="rblRecipientInfectionOrganismBacteria_SelectedIndexChanged">
                    
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLMainOptionsDataSource" runat="server" 
                            DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlRecipientInfectionOrganismBacteriaDetails" Visible="false">        
                <tr>
                    <td align="right">
                        <asp:label id="lblRecipientInfectionOrganismBacteriaDetails" runat="server"  Font-Size="Small" Text="Recipient Infection Organism Bacteria Details *"  Font-Bold="true"  />
                    </td>
                    <td align="left">
                
                        <asp:TextBox ID="txtRecipientInfectionOrganismBacteriaDetails" runat="server" MaxLength="100" Width="95%" Font-Size="Small"></asp:TextBox>
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td align="right">
                    <asp:label id="lblRecipientInfectionOrganismViral" runat="server"  Font-Size="Small" Text="Recipient Infection Organism Viral *"  Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rblRecipientInfectionOrganismViral" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                        AutoPostBack="true" OnSelectedIndexChanged="rblRecipientInfectionOrganismViral_SelectedIndexChanged">
                    
                    </asp:RadioButtonList>
                
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlRecipientInfectionOrganismViralDetails" Visible="false"> 
        
                <tr>
                    <td align="right">
                        <asp:label id="lblRecipientInfectionOrganismViralDetails" runat="server"  Font-Size="Small" Text="Recipient Infection Organism Viral Details *"  Font-Bold="true"  />
                    </td>
                    <td align="left">
                
                        <asp:TextBox ID="txtRecipientInfectionOrganismViralDetails" runat="server" MaxLength="100" Width="95%" Font-Size="Small"></asp:TextBox>
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td align="right">
                    <asp:label id="lblRecipientInfectionOrganismFungal" runat="server"  Font-Size="Small" Text="Recipient Infection Organism Fungal *"  Font-Bold="true"  />
                </td>
                <td align="left">
                         <asp:RadioButtonList ID="rblRecipientInfectionOrganismFungal" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                             AutoPostBack="true" OnSelectedIndexChanged="rblRecipientInfectionOrganismFungal_SelectedIndexChanged" >
                    
                    </asp:RadioButtonList>           
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlRecipientInfectionOrganismFungalDetails" Visible="false"> 
        
                <tr>
                    <td align="right">
                        <asp:label id="lblRecipientInfectionOrganismFungalDetails" runat="server"  Font-Size="Small" Text="Recipient Infection Organism Fungal Details *"  Font-Bold="true"  />
                    </td>
                    <td align="left">
                
                        <asp:TextBox ID="txtRecipientInfectionOrganismFungalDetails" runat="server" MaxLength="100" Width="95%" Font-Size="Small"></asp:TextBox>
                    </td>
                </tr>
            </asp:Panel>

        </asp:Panel>
        
        <asp:Panel runat="server" ID="pnlBiopsyProvenAcuteRejectionBG" Visible="false"> 
            <tr>
                <td align="right">
                    <asp:label id="lblBiopsyProvenAcuteRejectionBG" runat="server"  Font-Size="Small" Text="Biopsy Proven Acute Rejection (Banff Grading)"  />
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddBiopsyProvenAcuteRejectionBG" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="True">
                        <asp:ListItem Selected="True" Text="Select Biopsy Proven Acute Rejection BANFF Grading" Value="0" />
                    </asp:DropDownList>
                    <asp:XmlDataSource ID="XMLBanffGradingsDataSource" runat="server" 
                            DataFile="~/App_Data/BanffGradings.xml" XPath="/*/*" ></asp:XmlDataSource>
                </td>
            </tr>
        
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlReOperationAdverseEvent" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblReOperationDetails" runat="server"  Font-Size="Small" Text="Re-Operation Details *"  Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtReOperationDetails" runat="server" TextMode="MultiLine" MaxLength="500" Width="95%" Font-Names="Arial" Font-Size="Small" ></asp:TextBox>
                </td>
            </tr>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlOtherAdverseEvent" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblOtherAdverseEvent" runat="server"  Font-Size="Small" Text="Description of Other Adverse Event *" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtOtherAdverseEvent" runat="server" TextMode="MultiLine" MaxLength="500" Width="95%" Font-Names="Arial" Font-Size="Small" ></asp:TextBox>
                </td>
            </tr>
        </asp:Panel>
        

        <tr>
            <td align="right">
                <asp:label id="lblCG" runat="server"  Font-Size="Small" Text="Clavien Grading *"  Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:DropDownList ID="ddClavienGrading" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="True">
                    <asp:ListItem Selected="True" Text="Select Clavien Grading" Value="0" />
                </asp:DropDownList>
                <asp:XmlDataSource ID="XMLClavienGradingsDataSource" runat="server" 
                        DataFile="~/App_Data/ClavienGradings.xml" XPath="/*/*" ></asp:XmlDataSource>
                <asp:BalloonPopupExtender ID="bpeRecipientInfectionCG" runat="server" TargetControlID="ddClavienGrading"
                            BalloonPopupControlID="pnlClavienGrading"
                            Position="BottomRight" 
                            BalloonStyle="Rectangle"
                            BalloonSize="Large"
                            DisplayOnClick="true" DisplayOnFocus="false" DisplayOnMouseOver="false" 
                />
                <asp:Panel ID="pnlClavienGrading" runat="server">
                    <asp:label id="lblClavienGrading" runat="server"  Font-Size="Small" Text=""  />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="More Details about Adverse Event"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtComments" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" onkeyup="AutoExpandTextBox(this, event)" Rows="2" ></asp:TextBox>
            </td>
        </tr>
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
        <tr>
            <td align="right">
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
                <asp:Button ID="cmdUpdate" runat="server" Text="Submit" Height="36px" CausesValidation="False"   UseSubmitBehavior="False" OnClick="cmdUpdate_Click"    />
                
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
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True" OnRowDataBound="GV1_RowDataBound"  Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="AEID" SortExpression="AEID" HeaderText="AEID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="TrialIDRecipient" SortExpression="TrialIDRecipient" HeaderText="TrialID (Recipient)"
                            DataNavigateUrlFields="TrialID, TrialIDRecipient, AEID" 
                            DataNavigateUrlFormatString="EditAdvEvent.aspx?TID={0}&TID_R={1}&AEID={2}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="Date_AE" SortExpression="DateAE" HeaderText="Date"    />
                        <asp:BoundField DataField="AdverseEventType" SortExpression="AdverseEventType" HeaderText="Adverse Event Type"    />
                        <asp:BoundField DataField="ClavienGrading" SortExpression="ClavienGrading" HeaderText="Clavien Grading"    />
                        <asp:BoundField DataField="RecipientInfectionType" SortExpression="RecipientInfectionType" HeaderText="Recipient Infection Type"    /> 
                        <asp:BoundField DataField="BiopsyProvenAcuteRejectionBG" SortExpression="BiopsyProvenAcuteRejectionBG" HeaderText="Biopsy Proven Acute Rejection Banff Grading"    />                       
                         
                        <asp:BoundField DataField="Date_Created" SortExpression="DateCreated" HeaderText="Date Created"    />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

