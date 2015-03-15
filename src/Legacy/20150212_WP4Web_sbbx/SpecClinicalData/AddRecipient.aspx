<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalRecipientPage.master" AutoEventWireup="true" CodeFile="AddRecipient.aspx.cs" Inherits="SpecClinicalData_AddRecipient" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddRecipient" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 1020px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:25%" align="right">
                <asp:label id="lblTrialID" runat="server"  Font-Size="Small" Text="TrialID/ DonorID *" Font-Bold="true"  />
            </td>
            <td style="width:75%" align="left">
                <asp:DropDownList ID="ddTrialID" runat="server" AppendDataBoundItems="true" DataTextField="TrialIDDetails" DataValueField="TrialID" Width="50%" ToolTip="Cannot be changed in this page" 
                    AutoPostBack="False" Enabled="false">
                       <asp:ListItem Selected="True" Text="Select TrialID" Value="0" />
                </asp:DropDownList>
                <asp:SqlDataSource ID="sqldsTrialID" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                <asp:RequiredFieldValidator ID="rfv_ddTrialID" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option"  
                    CssClass="MandatoryFieldMessage" ControlToValidate="ddTrialID" InitialValue="0"  ValidationGroup="Main">

                </asp:RequiredFieldValidator>
                <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
                <asp:label id="lblTrialIDMessage" runat="server"  Font-Size="Small" Text="Obtained when Recipient added using Add Recipient Page" CssClass="Incomplete"  />
            </td>
        </tr>
        
        
        <tr>
            <td align="right">
                <asp:label id="lblKidneyReceived" runat="server"  Font-Size="Small" Text="Kidney Received *" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblKidneyReceived" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" Enabled="false" RepeatLayout="Flow" ToolTip="Cannot be changed in this page">
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLKidneySidesDataSource" runat="server" 
                        DataFile="~/App_Data/KidneySides.xml" XPath="/*/*" ></asp:XmlDataSource>
                 <asp:RequiredFieldValidator ID="rfv_rblKidneyReceived" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" ValidationGroup="Main"   
                      CssClass="MandatoryFieldMessage" ControlToValidate="rblKidneyReceived"  >

                </asp:RequiredFieldValidator>
                <asp:label id="lblKidneyReceivedMessage" runat="server"  Font-Size="Small" Text="Obtained when Recipient added using Add Recipient Page" CssClass="Incomplete"  />
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblRecipientID" runat="server"  Font-Size="Small" Text="Recipient ET /NHSBT Number" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtRecipientID" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                
                <asp:RegularExpressionValidator ID="revtxtRecipientID" runat="server" EnableClientScript="true" 
                        ErrorMessage="Please Enter Number/Alphabets Only" CssClass="MandatoryFieldMessage" 
                        ControlToValidate="txtRecipientID" ValidationExpression="^[0-9a-zA-Z]*$"  Display="Dynamic" ValidationGroup="Main"/>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblRecipientDateOfBirth" runat="server"  Font-Size="Small" Text="Recipient Date of Birth" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtRecipientDateOfBirth" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                
                <asp:CompareValidator runat="server" ID="cv_txtRecipientDateOfBirth" Display="Dynamic" ErrorMessage="Please Enter A Valid Date as DD/MM/YYYY" Operator="DataTypeCheck" Type="Date"  
                    ControlToValidate="txtRecipientDateOfBirth" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />
                <asp:RangeValidator ID="rv_txtRecipientDateOfBirth" runat="server" Display="Dynamic" ControlToValidate="txtRecipientDateOfBirth" Type="Date"
                    CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main">

                </asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblWeight" runat="server"  Font-Size="Small" Text="Weight (in kgs)" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtWeight" runat="server" MaxLength="6" Width="25%"></asp:TextBox>
                
                <asp:CompareValidator ID="cv_txtWeight" runat="server" Display="Dynamic" ControlToValidate="txtWeight" Type="Double" 
                    Operator="DataTypeCheck" CssClass="MandatoryFieldMessage" ErrorMessage="Please enter Numeric Values. Decimals are allowed"  ValidationGroup="Main"></asp:CompareValidator>

                <asp:RangeValidator ID="rv_txtWeight" runat="server" Display="Dynamic" ControlToValidate="txtWeight" Type="Double"
                    CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main" >

                </asp:RangeValidator>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblHeight" runat="server"  Font-Size="Small" Text="Height (in cms)" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtHeight" runat="server" MaxLength="3" Width="25%"></asp:TextBox>
                
                <asp:CompareValidator ID="cv_txtHeight" runat="server" Display="Dynamic" ControlToValidate="txtHeight" Type="Integer" 
                    Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are not allowed."  ValidationGroup="Main"></asp:CompareValidator>
                <asp:RangeValidator ID="rv_txtHeight" runat="server" Display="Dynamic" ControlToValidate="txtHeight" Type="Integer"
                    CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main" >

                </asp:RangeValidator>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblSex" runat="server"  Font-Size="Small" Text="Sex" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblSex" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    
                </asp:RadioButtonList>
                
                <asp:XmlDataSource ID="XMLSexDataSource" runat="server" 
                        DataFile="~/App_Data/Sex.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblEthnicityBlack" runat="server"  Font-Size="Small" Text="Ethnicity (If Black)" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblEthnicityBlack" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    
                </asp:RadioButtonList>
                
                <asp:XmlDataSource ID="XMLMainOptionsDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblRenalDisease" runat="server"  Font-Size="Small" Text="Renal Disease" Font-Bold="true"  />
            </td>
            <td  align="left">
                <asp:DropDownList ID="ddRenalDisease" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="50%">
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
                <asp:TextBox ID="txtRenalDiseaseOther" runat="server" MaxLength="100" Width="75%" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_txtRenalDiseaseOther" runat="server" Display="Dynamic" ErrorMessage="Please provide details."  
                    CssClass="MandatoryFieldMessage" ControlToValidate="txtRenalDiseaseOther" ValidationGroup="RenalDiseaseOther"   >
                </asp:RequiredFieldValidator>
            </td>
        </tr>
        
        
        <tr>
            <td align="right">
                <asp:label id="lblPreTransplantDiuresis" runat="server"  Font-Size="Small" Text="Pre Transplant Diuresis (ml/24h)" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtPreTransplantDiuresis" runat="server" MaxLength="6" Width="25%" ></asp:TextBox>
                
                <asp:CompareValidator ID="cv_txtPreTransplantDiuresis" runat="server" Display="Dynamic" ControlToValidate="txtPreTransplantDiuresis" Type="Integer" 
                    Operator="DataTypeCheck" CssClass="MandatoryFieldMessage" ErrorMessage="Please enter Numeric Values. Decimals are not allowed."  ValidationGroup="Main">

                </asp:CompareValidator>
                <asp:RangeValidator ID="rv_txtPreTransplantDiuresis" runat="server" Display="Dynamic" ControlToValidate="txtPreTransplantDiuresis" Type="Integer"
                    CssClass="MandatoryFieldMessage" ErrorMessage="" MinimumValue="0" MaximumValue="1000000" ValidationGroup="Main" >

                </asp:RangeValidator>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblBloodGroup" runat="server"  Font-Size="Small" Text="Blood Group" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblBloodGroup" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    
                </asp:RadioButtonList>
                
                <asp:XmlDataSource ID="XmlBloodGroupDataSource" runat="server" 
                        DataFile="~/App_Data/BloodGroups.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblHLA_A_Mismatch" runat="server"  Font-Size="Small" Text="HLA Mismatch A" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblHLA_A_Mismatch" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    
                </asp:RadioButtonList>
                
                <asp:XmlDataSource ID="XMLHLAMismatchDataSource" runat="server" 
                        DataFile="~/App_Data/HLAMismatches.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblHLA_B_Mismatch" runat="server"  Font-Size="Small" Text="HLA Mismatch B" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblHLA_B_Mismatch" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblHLA_DR_Mismatch" runat="server"  Font-Size="Small" Text="HLA Mismatch DR" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblHLA_DR_Mismatch" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <table rules="all" border="4" cellpadding="2" cellspacing="2" style="width:99%">
                    <tr>
                        <th colspan="4" align="center">
                            <asp:label id="lblEq5d5lqolScore" runat="server"  Font-Size="Small" Text="Pretransplant Quality of Life (EQ-5D-5L) Score"  />    
                        </th>
                    </tr>
                    <tr>
                        <td style="width:20%" align="right">
                            <asp:label id="lblDateQOLFIlled" runat="server"  Font-Size="Small" Text="Date Filled" Font-Bold="true" />
                        </td>
                        <td style="width:30%"  align="left">
                            <asp:TextBox ID="txtDateQOLFIlled" runat="server" MaxLength="10" Width="50%" ></asp:TextBox>
                            <asp:CalendarExtender ID="txtDateQOLFIlled_CalendarExtender" runat="server" Format="dd/MM/yyyy"  TargetControlID="txtDateQOLFIlled" PopupPosition="Right" EnableViewState="True" TodaysDateFormat="dd/MM/yyyy"></asp:CalendarExtender>
                
                        </td>
                        <td style="width:20%"  align="right">
                            <asp:label id="lblQOLFilledAt" runat="server"  Font-Size="Small" Text="Filled at?" Font-Bold="true" />
                        </td>
                        <td style="width:30%"  align="left">
                            <asp:DropDownList ID="ddQOLFilledAt" runat="server" DataTextField="Text" DataValueField="Value"   AppendDataBoundItems="true"
                                  >
                                <asp:ListItem Selected="True" Text="Select Filled At" Value="0" />
                       
                            </asp:DropDownList>
                            <asp:XmlDataSource ID="XMLQOLFilledOptionsDataSource" runat="server" 
                                DataFile="~/App_Data/QOLFilledOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
                        </td>
                    </tr>

                    <tr>
                        <td  align="right">
                            <asp:label id="lblMobility" runat="server"  Font-Size="Small" Text="Mobility" Font-Bold="true" />
                        </td>
                        <td   align="left">
                            <asp:DropDownList ID="ddMobility" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                <asp:ListItem Selected="True" Text="Select Mobility" Value="0" />
                            </asp:DropDownList>
                            <asp:XmlDataSource ID="XMLMobilityDataSource" runat="server" 
                                    DataFile="~/App_Data/QOLScoresMobility.xml" XPath="/*/*" ></asp:XmlDataSource>
                        </td>
                        <td   align="right">
                            <asp:label id="lblSelfCare" runat="server"  Font-Size="Small" Text="Self Care" Font-Bold="true" />
                        </td>
                        <td   align="left">
                            <asp:DropDownList ID="ddSelfCare" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                <asp:ListItem Selected="True" Text="Select Self Care" Value="0" />
                            </asp:DropDownList>
                            <asp:XmlDataSource ID="XMLSelfCareDataSource" runat="server" 
                                    DataFile="~/App_Data/QOLScoresSelfCare.xml" XPath="/*/*" ></asp:XmlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td  align="right">
                            <asp:label id="lblUsualActivities" runat="server"  Font-Size="Small" Text="Usual Activities" Font-Bold="true" />
                        </td>
                        <td   align="left">
                            <asp:DropDownList ID="ddUsualActivities" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                <asp:ListItem Selected="True" Text="Select Usual Activities" Value="0" />
                            </asp:DropDownList>
                            <asp:XmlDataSource ID="XMLUsualActivitiesDataSource" runat="server" 
                                    DataFile="~/App_Data/QOLScoresUsualActivities.xml" XPath="/*/*" ></asp:XmlDataSource>
                        </td>
                        <td   align="right">
                            <asp:label id="lblPainDiscomfort" runat="server"  Font-Size="Small" Text="Pain/Discomfort" Font-Bold="true" />
                        </td>
                        <td   align="left">
                            <asp:DropDownList ID="ddPainDiscomfort" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                <asp:ListItem Selected="True" Text="Select Pain/Discomfort" Value="0" />
                            </asp:DropDownList>
                            <asp:XmlDataSource ID="XMLPainDiscomfortDataSource" runat="server" 
                                    DataFile="~/App_Data/QOLScoresPainDiscomfort.xml" XPath="/*/*" ></asp:XmlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td  align="right">
                            <asp:label id="lblAnxietyDepression" runat="server"  Font-Size="Small" Text="Anxiety Depression" Font-Bold="true" />
                        </td>
                        <td   align="left">
                            <asp:DropDownList ID="ddAnxietyDepression" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                <asp:ListItem Selected="True" Text="Select Anxiety/Depression" Value="0" />
                            </asp:DropDownList>
                            <asp:XmlDataSource ID="XMLAnxietyDepressionDataSource" runat="server" 
                                    DataFile="~/App_Data/QOLScoresAnxietyDepression.xml" XPath="/*/*" ></asp:XmlDataSource>
                        </td>
                        <td   align="right">
                            <asp:label id="lblVASScore" runat="server"  Font-Size="Small" Text="VAS Score" Font-Bold="true" />
                        </td>
                        <td   align="left">
                            <asp:TextBox ID="txtVASScore" runat="server" MaxLength="3" Width="25%"></asp:TextBox>
                            <asp:FilteredTextBoxExtender runat="server" ID="ftbeVASScore" TargetControlID="txtVASScore" FilterType="Numbers" FilterMode="ValidChars" />
                            <asp:label id="lblVASScoreValues" runat="server"  Font-Size="Small" Text="0 to 100/ 999 if Missing" Font-Bold="true" />
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
        <asp:Panel runat="server" ID="pnlAllDataAdded" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblAllDataAdded" runat="server"  Font-Size="Small" Text="Tick Box if Data Entry Complete"  />
                </td>
                <td align="left">
                    <asp:CheckBox runat="server" ID="chkAllDataAdded" AutoPostBack="true" OnCheckedChanged="chkAllDataAdded_CheckedChanged"/>
                    <asp:Label runat="server" ID="lblAllDataAddedMessage" Font-Size="Small" Text="" CssClass="Incomplete" />
                
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
                    <asp:label id="lblReasonModifiedOld" runat="server"  Font-Size="Small" Text="Reason(s) Data Modified Earlier" />
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
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px" CausesValidation="true" ValidationGroup="Main"  UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
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
                        <asp:BoundField DataField="TrialIDRecipient" SortExpression="KidneyReceived" HeaderText="TrialID Recipient"    />
                        <asp:BoundField DataField="RecipientID" SortExpression="RecipientID" HeaderText="RecipientID"    />
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

