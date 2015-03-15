<%@ Page Title="" Language="C#" MasterPageFile="~/StudyIDMasterPage.master" AutoEventWireup="true" CodeFile="AddWP4Recipient.aspx.cs" Inherits="AddEditData_AddWP4Recipient" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddWP4Recipient" ContentPlaceHolderID="AddEditStudyContent" Runat="Server">

        <asp:Label runat="server" ID="lblUserMessages" Font-Size="Small" ForeColor="Red" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:25%" align="right">
                <asp:label id="lblTrialID" runat="server"  Font-Size="Small" Text="Select TrialID/ DonorID"  />
            </td>
            <td style="width:75%" align="left">
                <asp:DropDownList ID="ddTrialID" runat="server" AppendDataBoundItems="true" DataTextField="TrialIDDetails" DataValueField="TrialID" Width="50%" AutoPostBack="True" OnSelectedIndexChanged="ddTrialID_SelectedIndexChanged">
                       <asp:ListItem Selected="True" Text="Select TrialID" Value="0" />
                </asp:DropDownList>
                <asp:SqlDataSource ID="sqldsTrialID" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblTrialIDDetails" runat="server"  Font-Size="Small" Text="TrialID Details"  />
            </td>
            <td align="left"  >
                <asp:Label ID="lblDonorIDDetails" runat="server"  Width="95%" Enabled="false"></asp:Label>
                
                
            </td>
            
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblKidneyReceived" runat="server"  Font-Size="Small" Text="Kidney Received"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblKidneyReceived" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLKidneySidesDataSource" runat="server" 
                        DataFile="~/App_Data/KidneySides.xml" XPath="/*/*" ></asp:XmlDataSource>
                
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblRecipientID" runat="server"  Font-Size="Small" Text="RecipientID"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtRecipientID" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblRecipientDateOfBirth" runat="server"  Font-Size="Small" Text="Recipient Date of Birth"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtRecipientDateOfBirth" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                <asp:CalendarExtender ID="txtRecipientDateOfBirth_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtRecipientDateOfBirth">
                </asp:CalendarExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblWeight" runat="server"  Font-Size="Small" Text="Weight (in kgs)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtWeight" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblHeight" runat="server"  Font-Size="Small" Text="Height (in cms)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtHeight" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblSex" runat="server"  Font-Size="Small" Text="Sex"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblSex" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLSexDataSource" runat="server" 
                        DataFile="~/App_Data/Sex.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblEthnicityBlack" runat="server"  Font-Size="Small" Text="Ethnicity (If Black)"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblEthnicityBlack" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLMainOptionsDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblRenalDisease" runat="server"  Font-Size="Small" Text="Renal Disease"  />
            </td>
            <td  align="left">
                <asp:DropDownList ID="ddRenalDisease" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="75%">
                    <asp:ListItem Selected="True" Text="Select a Renal Disease" Value="0"  />   
                </asp:DropDownList>
                <asp:XmlDataSource ID="XMLRenalDiseasesDataSource" runat="server" 
                        DataFile="~/App_Data/RenalDiseases.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblRenalDiseaseOther" runat="server"  Font-Size="Small" Text="Renal Disease (If Other)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtRenalDiseaseOther" runat="server" MaxLength="100" Width="95%" ></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblPreTransplantDiuresis" runat="server"  Font-Size="Small" Text="Pre Transplant Diuresis (ml/24h)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtPreTransplantDiuresis" runat="server" MaxLength="100" Width="25%" ></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblBloodGroup" runat="server"  Font-Size="Small" Text="Blood Group"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblBloodGroup" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XmlBloodGroupDataSource" runat="server" 
                        DataFile="~/App_Data/BloodGroups.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblHLA_A" runat="server"  Font-Size="Small" Text="HLA Typing A"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblHLA_A" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblHLA_B" runat="server"  Font-Size="Small" Text="HLA Typing B"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblHLA_B" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblHLA_DR" runat="server"  Font-Size="Small" Text="HLA Typing DR"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblHLA_DR" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:label id="lblET_Urgency" runat="server"  Font-Size="Small" Text="ET Urgency"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblET_Urgency" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLETUrgenciesDataSource" runat="server" 
                        DataFile="~/App_Data/ETUrgencies.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblHLA_A_Mismatch" runat="server"  Font-Size="Small" Text="HLA Mismatch A"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblHLA_A_Mismatch" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLHLAMismatchDataSource" runat="server" 
                        DataFile="~/App_Data/HLAMismatches.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblHLA_B_Mismatch" runat="server"  Font-Size="Small" Text="HLA Mismatch B"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblHLA_B_Mismatch" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblHLA_DR_Mismatch" runat="server"  Font-Size="Small" Text="HLA Mismatch DR"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblHLA_DR_Mismatch" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <table rules="all" border="4" cellpadding="2" cellspacing="2" style="width:99%">
                    <tr>
                        <th colspan="4" align="center">
                            <asp:label id="lblEq5d5lqolScore" runat="server"  Font-Size="Small" Text="Quality of Life (EQ-5D-5L) Score"  />    
                        </th>
                    </tr>
                    <tr>
                        <td style="width:32%" align="right">
                            <asp:label id="lblMobility" runat="server"  Font-Size="Small" Text="Mobility (1 to 5/ 9 if Missing)"  />
                        </td>
                        <td style="width:20%"  align="left">
                            <asp:DropDownList ID="ddMobility" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                <asp:ListItem Selected="True" Text="Select Mobility" Value="0" />
                            </asp:DropDownList>
                            <asp:XmlDataSource ID="XMLQOLScoresDataSource" runat="server" 
                                    DataFile="~/App_Data/QOLScores.xml" XPath="/*/*" ></asp:XmlDataSource>
                        </td>
                        <td style="width:28%"  align="right">
                            <asp:label id="lblSelfCare" runat="server"  Font-Size="Small" Text="Self Care (1 to 5/ 9 if Missing)"  />
                        </td>
                        <td style="width:20%"  align="left">
                            <asp:DropDownList ID="ddSelfCare" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                <asp:ListItem Selected="True" Text="Select Self Care" Value="0" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td  align="right">
                            <asp:label id="lblUsualActivities" runat="server"  Font-Size="Small" Text="Usual Activities (1 to 5/ 9 if Missing)"  />
                        </td>
                        <td   align="left">
                            <asp:DropDownList ID="ddUsualActivities" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                <asp:ListItem Selected="True" Text="Select Usual Activities" Value="0" />
                            </asp:DropDownList>
                        </td>
                        <td   align="right">
                            <asp:label id="lblPainDiscomfort" runat="server"  Font-Size="Small" Text="Pain/Discomfort (1 to 5/ 9 if Missing)"  />
                        </td>
                        <td   align="left">
                            <asp:DropDownList ID="ddPainDiscomfort" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                <asp:ListItem Selected="True" Text="Select Pain/Discomfort" Value="0" />
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td  align="right">
                            <asp:label id="lblAnxietyDepression" runat="server"  Font-Size="Small" Text="Anxiety Depression (1 to 5/ 9 if Missing)"  />
                        </td>
                        <td   align="left">
                            <asp:DropDownList ID="ddAnxietyDepression" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                <asp:ListItem Selected="True" Text="Select Anxiety/Depression" Value="0" />
                            </asp:DropDownList>
                        </td>
                        <td   align="right">
                            <asp:label id="lblVASScore" runat="server"  Font-Size="Small" Text="VAS Score (0 to 100/ 999 if Missing)"  />
                        </td>
                        <td   align="left">
                            <asp:TextBox ID="txtVASScore" runat="server" MaxLength="3" Width="25%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
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
        <tr>
            <td align="right">
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
            </td>
            <td align="left">
                <asp:Button ID="cmdAddData" runat="server" Text="Add Recipient Data" Height="36px"   UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
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
                        <asp:BoundField DataField="RIdentificationID" SortExpression="RIdentificationID" HeaderText="RIdentificationID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="RecipientID" SortExpression="RecipientID" HeaderText="RecipientID"
                            DataNavigateUrlFields="TrialID, TrialIDRecipient,RIdentificationID" 
                            DataNavigateUrlFormatString="~/SpecClinicalData/EditRecipient.aspx?TID={0}&TID_R={1}&RIdentificationID={2}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="KidneyReceived" SortExpression="KidneyReceived" HeaderText="Kidney Received"    />
                        <asp:BoundField DataField="Date_OfBirth" SortExpression="DateOfBirth" HeaderText="Date Of Birth"    />
                        <asp:BoundField DataField="Sex" SortExpression="Sex" HeaderText="Sex"    />
                        <asp:BoundField DataField="Weight" SortExpression="Weight" HeaderText="Weight"    />
                        <asp:BoundField DataField="Height" SortExpression="Height" HeaderText="Height"    />                       
                        <asp:BoundField DataField="BloodGroup" SortExpression="BloodGroup" HeaderText="Blood Group" />
                        <asp:BoundField DataField="EthnicityBlack" SortExpression="EthnicityBlack" HeaderText="Ethnicity Black" />
                        <asp:BoundField DataField="RenalDisease" SortExpression="RenalDisease" HeaderText="Renal Disease" />
                        <asp:BoundField DataField="BloodGroup" SortExpression="BloodGroup" HeaderText="Blood Group" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="sqldsGV1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

