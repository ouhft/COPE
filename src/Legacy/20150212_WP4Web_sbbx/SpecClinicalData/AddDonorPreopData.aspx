<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddDonorPreopData.aspx.cs" Inherits="SpecClinicalData_AddDonorPreopData" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddDonorPreopData" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
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
            <td style="width:35%" align="right">
                <asp:label id="lblDonorDiagnosisOptions" runat="server"  Font-Size="Small" Text="Donor Diagnosis" Font-Bold="true" />
            </td>
            <td style="width:65%"  align="left">
                <asp:DropDownList ID="ddDonorDiagnosisOptions" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true"
                    AutoPostBack="true" OnSelectedIndexChanged="ddDonorDiagnosisOptions_SelectedIndexChanged" >
                    <asp:ListItem Selected="True" Text="Select Donor Diagnosis" Value="0" />
                </asp:DropDownList>
                <asp:XmlDataSource ID="XMLDonorDiagnosisOptionsDataSource" runat="server" 
                        DataFile="~/App_Data/DonorDiagnosisOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
                <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
                

                
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlDiagnosis" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblDiagnosisOtherDetails" runat="server"  Font-Size="Small" Text="Diagnosis (If Other)" Font-Bold="true" />
                </td>
                <td  align="left">
                    <asp:TextBox ID="txtDiagnosisOtherDetails" runat="server" MaxLength="100" Width="75%" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_txtDiagnosisOtherDetails" runat="server" Display="Dynamic" ErrorMessage="Please Enter Details"  
                    CssClass="Caution" ControlToValidate="txtDiagnosisOtherDetails" ValidationGroup="Diagnosis"  >

                </asp:RequiredFieldValidator>
                </td>
            </tr>
        </asp:Panel>
        

        <tr>
            <td align="right">
                <asp:label id="lblDiabetesMellitus" runat="server"  Font-Size="Small" Text="Diabetes Mellitus" Font-Bold="true"  />
            </td>
            <td  align="left">
                <asp:RadioButtonList ID="rblDiabetesMellitus" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XmlMainOptionsYNDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
                
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:label id="lblAlcoholAbuse" runat="server"  Font-Size="Small" Text="Alcohol Abuse" Font-Bold="true"  />
            </td>
            <td  align="left">
                <asp:RadioButtonList ID="rblAlcoholAbuse" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    
                </asp:RadioButtonList>
                
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblCardiacArrest" runat="server"  Font-Size="Small" Text="Cardiac arrest (during ICU stay prior to retrieval procedure))" Font-Bold="true"  />
            </td>
            <td  align="left">
                <asp:RadioButtonList ID="rblCardiacArrest" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    
                </asp:RadioButtonList>
                
                
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:label id="lblSystolicBloodPressure" runat="server"  Font-Size="Small" Text="Mean Systolic Blood pressure (Before Switch Off) (mmHg)" Font-Bold="true"  />
            </td>
            <td  align="left">
                <asp:TextBox ID="txtSystolicBloodPressure" runat="server" AutoPostBack="true" MaxLength="3" Width="25%" OnTextChanged="txtSystolicBloodPressure_TextChanged" ></asp:TextBox>
                

                <asp:CompareValidator ID="cv_txtSystolicBloodPressure" runat="server" Display="Dynamic" ControlToValidate="txtSystolicBloodPressure" Type="Integer" 
                    Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are not allowed" ValidationGroup="MainGroup" ></asp:CompareValidator>

                <asp:RangeValidator ID="rv_txtSystolicBloodPressure" runat="server" Display="Dynamic" ControlToValidate="txtSystolicBloodPressure" Type="Integer"
                    CssClass="Caution" ErrorMessage="" ValidationGroup="MainGroup" >

                </asp:RangeValidator>

            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:label id="lblDiastolicBloodPressure" runat="server"  Font-Size="Small" Text="Mean Diastolic Blood pressure (Before Switch Off) (mmHg)" Font-Bold="true"  />
            </td>
            <td  align="left">
                <asp:TextBox ID="txtDiastolicBloodPressure" runat="server" MaxLength="3" Width="25%" ></asp:TextBox>
                
                <asp:CompareValidator ID="cv_txtDiastolicBloodPressure" runat="server" Display="Dynamic" ControlToValidate="txtDiastolicBloodPressure" Type="Integer" 
                    Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are not allowed" ValidationGroup="MainGroup" ></asp:CompareValidator>

                <asp:RangeValidator ID="rv_txtDiastolicBloodPressure" runat="server" Display="Dynamic" ControlToValidate="txtDiastolicBloodPressure" Type="Integer"
                    CssClass="Caution" ErrorMessage="" ValidationGroup="MainGroup" >

                </asp:RangeValidator>
            </td>
        </tr>

        <tr>
            <td align="right">
                <asp:label id="lblHypotensivePeriod" runat="server"  Font-Size="Small" Text="Hypotensive Period (Syst. < 100 mmHg)" Font-Bold="true"  />
            </td>
            <td  align="left">
                <asp:RadioButtonList ID="rblHypotensivePeriod" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                    Enabled="true" RepeatLayout="Flow" >
                    
                </asp:RadioButtonList>
                <asp:label id="lblHypotensivePeriodMessage" runat="server"  Font-Size="Small" Text="&nbsp;&nbsp;(Populated when Systolic Blood Pressure is Entered)" />
                <asp:XmlDataSource ID="XmlMainOptionsDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
                
        <tr>
            <td align="right">
                <asp:label id="lblDiuresis" runat="server"  Font-Size="Small" Text="Mean diuresis / hr last 24 hrs"  Font-Bold="true"/>
            </td>
            <td  align="left">
                <asp:TextBox ID="txtDiuresis" runat="server" MaxLength="4" Width="25%"  ></asp:TextBox>
                <asp:CompareValidator ID="cv_txtDiuresis" runat="server" Display="Dynamic" ControlToValidate="txtDiuresis" Type="Integer" 
                    Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values." ValidationGroup="MainGroup" ></asp:CompareValidator>
                <asp:CheckBox runat="server" ID="chkDiuresis" Text="Tick if Unknown" AutoPostBack="true" OnCheckedChanged="chkDiuresis_CheckedChanged" />
            </td>
        </tr>

        
        

        
        <tr>
            <td colspan="2" align="center">
                <table rules="all" border="4" cellpadding="1" cellspacing="1" style="width:98%">
                    <tr>
                        <th colspan="4" align="center">
                            <asp:label id="lblVasopressors" runat="server"  Font-Size="Small" Text="Vasopressors"  />
                        </th>
                    </tr>
                    <tr>
                        <td align="right" style="width:20%">
                            <asp:label id="lblDopamine" runat="server"  Font-Size="Small" Text="Dopamine" Font-Bold="true"  />
                        </td>
                        <td  align="left" style="width:25%">
                            <asp:RadioButtonList ID="rblDopamine" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                                AutoPostback="true" OnSelectedIndexChanged="rblDopamine_SelectedIndexChanged">
                    
                            </asp:RadioButtonList>
                
                        </td>
                        <td align="right" style="width:35%">
                            <asp:label id="lblDopamineLastDose" runat="server"  Font-Size="Small" Text="Dopamine Last Dose"   />
                        </td>
                        <td  align="left" style="width:20%">
                            <asp:TextBox ID="txtDopamineLastDose" runat="server" MaxLength="45" Width="50%" ></asp:TextBox>
                            
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblDobutamine" runat="server"  Font-Size="Small" Text="Dobutamine"   Font-Bold="true"/>
                        </td>
                        <td  align="left" >
                            <asp:RadioButtonList ID="rblDobutamine" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                                AutoPostback="true" OnSelectedIndexChanged="rblDobutamine_SelectedIndexChanged">
                    
                            </asp:RadioButtonList>
                
                        </td>
                        <td align="right">
                            <asp:label id="lblDobutamineLastDose" runat="server"  Font-Size="Small" Text="Dobutamine Last Dose"  />
                        </td>
                        <td  align="left" >
                            <asp:TextBox ID="txtDobutamineLastDose" runat="server" MaxLength="45" Width="50%" ></asp:TextBox>
                            
                
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <asp:label id="lblNorAdrenaline" runat="server"  Font-Size="Small" Text="NorAdrenaline" Font-Bold="true" />
                        </td>
                        <td  align="left" >
                            <asp:RadioButtonList ID="rblNorAdrenaline" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                                AutoPostback="true" OnSelectedIndexChanged="rblNorAdrenaline_SelectedIndexChanged">
                    
                            </asp:RadioButtonList>
                
                        </td>
                        <td align="right" >
                            <asp:label id="lblNorAdrenalineLastDose" runat="server"  Font-Size="Small" Text="NorAdrenaline Last Dose (µg/kg BW/min)"  />
                        </td>
                        <td  align="left" >
                            <asp:TextBox ID="txtNorAdrenalineLastDose" runat="server" MaxLength="45" Width="50%" ></asp:TextBox>
                            
                
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <asp:label id="lblOtherMedication" runat="server"  Font-Size="Small" Text="Other Medication (1)"  />
                        </td>
                        <td  align="left" >
                            <asp:TextBox ID="txtOtherMedication" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
                
                        </td>
                        <td align="right" >
                            <asp:label id="lblOtherMedicationLastDose" runat="server"  Font-Size="Small" Text="Other Medication Last Dose (µg/kg BW/min)"  />
                        </td>
                        <td  align="left" >
                            <asp:TextBox ID="txtOtherMedicationLastDose" runat="server" MaxLength="45" Width="95%" ></asp:TextBox>
                            
                        </td>
                    </tr>
                    <tr>
                        <td align="right" >
                            <asp:label id="lblOtherMedication2" runat="server"  Font-Size="Small" Text="Other Medication (2)"  />
                        </td>
                        <td  align="left" >
                            <asp:TextBox ID="txtOtherMedication2" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
                
                        </td>
                        <td align="right" >
                            <asp:label id="lblOtherMedication2LastDose" runat="server"  Font-Size="Small" Text="Other Medication (2) Last Dose (µg/kg BW/min)"  />
                        </td>
                        <td  align="left" >
                            <asp:TextBox ID="txtOtherMedication2LastDose" runat="server" MaxLength="45" Width="95%" ></asp:TextBox>
                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
       
        <tr>
            <td align="right">
                <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="General Comments"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" MaxLength="500" Width="90%"></asp:TextBox>
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
    </table> 
        </ContentTemplate>
    </asp:UpdatePanel>
    <table  cellpadding="2" cellspacing="2" border="0" style="width: 851px">
        
        <tr>
            <td style="width:35%" align="right">
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
            <td style="width:65%" align="left">
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px" CausesValidation="true" ValidationGroup="MainGroup"    OnClick="cmdAddData_Click"   />
                
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
                        <asp:BoundField DataField="DonorPreOpClinicalDataID" SortExpression="DonorPreOpClinicalDataID" HeaderText="DonorPreOpClinicalDataID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID" Visible="false"
                            DataNavigateUrlFields="TrialID, DonorPreOpClinicalDataID" 
                            DataNavigateUrlFormatString="AddDonorPreOpData.aspx?TID={0}&DonorPreOpClinicalDataID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="TrialID" SortExpression="TrialID" HeaderText="TrialID"    />
                        <asp:BoundField DataField="Diagnosis" SortExpression="Diagnosis" HeaderText="Donor Diagnosis"    />
                        <asp:BoundField DataField="CardiacArrest" SortExpression="CardiacArrest" HeaderText="Cardiac Arrest"    />
                        <asp:BoundField DataField="HypotensivePeriod" SortExpression="HypotensivePeriod" HeaderText="Hypotensive Period"    />
                        <asp:BoundField DataField="DiabetesMellitus" SortExpression="DiabetesMellitus" HeaderText="Diabetes Mellitus"    />                       
                        <asp:BoundField DataField="Diuresis" SortExpression="Diuresis" HeaderText="Diuresis" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

