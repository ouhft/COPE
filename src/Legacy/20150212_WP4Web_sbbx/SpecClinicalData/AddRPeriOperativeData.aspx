<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalRecipientPage.master" AutoEventWireup="true" CodeFile="AddRPeriOperativeData.aspx.cs" Inherits="SpecClinicalData_AddRPeriOperativeData" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddRPeriOperativeData" ContentPlaceHolderID="SpecimenContents" Runat="Server">
        
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 1020px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:30%" align="right">
                <asp:label id="lblTransplantationDate" runat="server"  Font-Size="Small" Text="Transplantation Date *" Font-Bold="true"  />
            </td>
            <td style="width:70%" align="left">
                <asp:TextBox ID="txtTransplantationDate" runat="server" MaxLength="10" Width="80px" AutoPostBack="false" ></asp:TextBox>
                <asp:CalendarExtender ID="txtTransplantationDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtTransplantationDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                <asp:RequiredFieldValidator ID="rfv_txtTransplantationDate" runat="server" Display="Dynamic" ErrorMessage="Please enter Transplantation Date"  
                        CssClass="MandatoryFieldMessage" ControlToValidate="txtTransplantationDate"   ValidationGroup="Main"  />
                <asp:CompareValidator runat="server" ID="cv_txtTransplantationDate" Display="Dynamic" ErrorMessage="Please Enter A Valid Date as DD/MM/YYYY" Operator="DataTypeCheck" Type="Date"  
                    ControlToValidate="txtTransplantationDate" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

                <asp:RangeValidator ID="rv_txtTransplantationDate" runat="server" Display="Dynamic" ControlToValidate="txtTransplantationDate" Type="Date"
                    CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main">

                </asp:RangeValidator>
                <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblMachinePerfusionStopDate" runat="server"  Font-Size="Small" Text="Machine Perfusion Stop Date/Time" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtMachinePerfusionStopDate" runat="server" MaxLength="10" Width="80px" AutoPostBack="false" ></asp:TextBox>
                

                <asp:CompareValidator runat="server" ID="cvtxtMachinePerfusionStopDate" Display="Dynamic" ErrorMessage="Please Enter A Valid Date as DD/MM/YYYY" Operator="DataTypeCheck" Type="Date"  
                    ControlToValidate="txtMachinePerfusionStopDate" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />
                
                <asp:RangeValidator ID="rv_txtMachinePerfusionStopDate" runat="server" Display="Dynamic" ControlToValidate="txtMachinePerfusionStopDate" Type="Date"
                    CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main">

                </asp:RangeValidator>

                <asp:CalendarExtender ID="txtMachinePerfusionStopDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtMachinePerfusionStopDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtMachinePerfusionStopTime" runat="server" MaxLength="5" Width="40px" AutoPostBack="false" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtMachinePerfusionStopTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtMachinePerfusionStopTime">
                </asp:MaskedEditExtender>

                

            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblTapeBroken" runat="server"  Font-Size="Small" Text="Was the Tape over the regulator Broken" Font-Bold="true"  />
            </td>
            <td align="left">
                
                <asp:RadioButtonList ID="rblTapeBroken" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
                       
                </asp:RadioButtonList>
                
                

            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblKidneyRemovedFromIceDate" runat="server"  Font-Size="Small" Text="Kidney Removed From Ice Date/Time" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtKidneyRemovedFromIceDate" runat="server" MaxLength="10" Width="80px" AutoPostBack="false" ></asp:TextBox>
                

                <asp:CompareValidator runat="server" ID="cv_txtKidneyRemovedFromIceDate2" Display="Dynamic" ErrorMessage="Please Enter A Valid Date as DD/MM/YYYY" Operator="DataTypeCheck" 
                    ControlToValidate="txtKidneyRemovedFromIceDate" Type="Date"  
                    ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

                <asp:CompareValidator runat="server" ID="cvtxtKidneyRemovedFromIceDate" Display="Dynamic" ErrorMessage="Date should be greater than or equal to Machine Perfusion Stop Date" Operator="GreaterThanEqual" 
                    ControlToValidate="txtKidneyRemovedFromIceDate" ControlToCompare="txtMachinePerfusionStopDate" Type="Date"  
                    ValidationGroup="Main" CssClass="MandatoryFieldMessage" />
                
                

                <asp:CalendarExtender ID="txtKidneyRemovedFromIceDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtKidneyRemovedFromIceDate" PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtKidneyRemovedFromIceTime" runat="server" MaxLength="5" Width="40px" AutoPostBack="false" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtKidneyRemovedFromIceTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtKidneyRemovedFromIceTime">
                </asp:MaskedEditExtender>

                

            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlOxygenBottleFullOpen" Visible="false">

            <tr>
                <td align="right">
                    <asp:label id="lblOxygenBottleFullAndTurnedOpen" runat="server"  Font-Size="Small" Text="Oxygen Bottle Full and Turned Open" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rblOxygenBottleFullAndTurnedOpen" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" AutoPostBack="false" 
                         RepeatLayout="Flow" >
                       
                    </asp:RadioButtonList>
                </td>

        </tr>

        </asp:Panel>
        
        <tr>
            <td align="right">
                <asp:label id="lblTimeOnMachine" runat="server"  Font-Size="Small" Text="Time On Machine (Minutes)" Font-Bold="false"  />
            </td>
            <td>
                <asp:TextBox ID="txtTimeOnMachine" runat="server" MaxLength="5" Width="40px" Enabled="false" ToolTip="Calculated Automatically when data is saved. The value is Machine Perfusion Stop Date/Time - Machine Perfusion Start Date/Time" ></asp:TextBox>
                
            </td>
        </tr>
        <tr>
                <td align="right">
                    <asp:label id="lblColdIschemiaPeriod" runat="server"  Font-Size="Small" Text="Cold Ischemia Period (Hours/Minutes)"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtColdIschemiaPeriod" runat="server" MaxLength="5" Width="40px" AutoPostBack="false"
                        Enabled="false" ToolTip="Calculated Automatically when data is saved. The value is Kidney Removed From Ice Date/Time - Start In Situ Cold Perfusion Date/Time" ></asp:TextBox>
                    
                </td>
            </tr>
        <tr>
            <td align="right">
                <asp:label id="lblKidneyDiscarded" runat="server"  Font-Size="Small" Text="Kidney Discarded" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblKidneyDiscarded" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" AutoPostBack="true" 
                    OnSelectedIndexChanged="rblKidneyDiscarded_SelectedIndexChanged" RepeatLayout="Flow" >
                       
                </asp:RadioButtonList>
                

                <asp:XmlDataSource ID="XMLMainOptionsYNDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
                <asp:XmlDataSource ID="XMLMainOptionsDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <asp:Panel ID="pnlKidneyDiscardedYes" runat="server" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblKidneyDiscardedYes" runat="server"  Font-Size="Small" Text="Kidney Discarded (If Yes)"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtKidneyDiscardedYes" runat="server" MaxLength="500" TextMode="MultiLine" Height="50px" Width="95%"></asp:TextBox>
                    
                
                </td>
            </tr>
        </asp:Panel>
        <asp:Panel ID="pnlKidneyDiscardedNo" runat="server" Visible="false">
        
            
            <tr>
                <td align="right">
                    <asp:label id="lblOperationStart" runat="server"  Font-Size="Small" Text="Operation Start Date Time" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtOperationStartDate" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtOperationStartDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtOperationStartDate" PopupPosition="Right">
                    </asp:CalendarExtender>
                    

                    <asp:CompareValidator runat="server" ID="cv_txtOperationStartDate" Display="Dynamic" ErrorMessage="Please Enter A Valid Date as DD/MM/YYYY" Operator="DataTypeCheck" Type="Date"  
                        ControlToValidate="txtOperationStartDate" ValidationGroup="KidneyDiscardedNo" CssClass="MandatoryFieldMessage" />

                    <asp:TextBox ID="txtOperationStartTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                    

                    <asp:MaskedEditExtender ID="txtOperationStartTime_MaskedEditExtender" runat="server" 
                        Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                        ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                        TargetControlID="txtOperationStartTime">
                    </asp:MaskedEditExtender>
                </td>
            </tr>
            <tr>
                <td  align="right">
                    <asp:label id="lblIncision" runat="server"  Font-Size="Small" Text="Incision"  />
                </td>
                <td  align="left">
                    <asp:RadioButtonList ID="rblIncision" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" >
                       
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLIncisionsDataSource" runat="server" 
                            DataFile="~/App_Data/Incisions.xml" XPath="/*/*" ></asp:XmlDataSource>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblTransplantSide" runat="server"  Font-Size="Small" Text="Transplant Side" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rblTransplantSide" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLKidneySidesDataSource" runat="server" 
                            DataFile="~/App_Data/KidneySides.xml" XPath="/*/*" ></asp:XmlDataSource>

                    
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblArterialProblems" runat="server"  Font-Size="Small" Text="Arterial Problems" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddArterialProblems" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="50%" >
                        <asp:ListItem Selected="True" Text="Select Arterial Problems" Value="0" />
                    
                    </asp:DropDownList>
                   

                    <asp:XmlDataSource ID="XMLArterialProblemsRecipientDataSource" runat="server" 
                            DataFile="~/App_Data/ArterialProblemsRecipient.xml" XPath="/*/*" ></asp:XmlDataSource>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblArterialProblemsOther" runat="server"  Font-Size="Small" Text="Arterial Problems (If Other)"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtArterialProblemsOther" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_txtArterialProblemsOther" runat="server" Display="Dynamic" ErrorMessage="Please provide details"  
                        CssClass="MandatoryFieldMessage" ControlToValidate="txtArterialProblemsOther"   ValidationGroup="ArterialProblemsOther"  />
                </td>
            </tr>
            <tr>
                <td  align="right">
                    <asp:label id="lblVenousProblems" runat="server"  Font-Size="Small" Text="Venous Problems"  />
                </td>
                <td  align="left">
                    <asp:RadioButtonList ID="rblVenousProblems" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" >                       
                    </asp:RadioButtonList>                
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblStartAnastomosisDateTime" runat="server"  Font-Size="Small" Text="Anastomosis Start Date Time"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtStartAnastomosisDate" runat="server" MaxLength="10" Width="80px" AutoPostBack="false" ></asp:TextBox>
                    <asp:CalendarExtender ID="txtStartAnastomosisDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtStartAnastomosisDate" PopupPosition="Right">
                    </asp:CalendarExtender>

                    <asp:CompareValidator runat="server" ID="cv_txtStartAnastomosisDate" Display="Dynamic" ErrorMessage="Please Enter A Valid Date as DD/MM/YYYY" Operator="DataTypeCheck" Type="Date"  
                        ControlToValidate="txtStartAnastomosisDate" ValidationGroup="KidneyDiscardedNo" CssClass="MandatoryFieldMessage" />


                    <asp:TextBox ID="txtStartAnastomosisTime" runat="server" MaxLength="5" Width="40px" AutoPostBack="false" ></asp:TextBox>
                    <asp:MaskedEditExtender ID="txtStartAnastomosis_MaskedEditExtender" runat="server" 
                        Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                        ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                        TargetControlID="txtStartAnastomosisTime">
                    </asp:MaskedEditExtender>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblReperfusionDateTime" runat="server"  Font-Size="Small" Text="Reperfusion Date Time"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtReperfusionDate" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                    <asp:CalendarExtender ID="txtReperfusionDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtReperfusionDate" PopupPosition="Right">
                    </asp:CalendarExtender>

                    <asp:CompareValidator runat="server" ID="cv_txtReperfusionDate" Display="Dynamic" ErrorMessage="Please Enter A Valid Date as DD/MM/YYYY" Operator="DataTypeCheck" Type="Date"  
                        ControlToValidate="txtReperfusionDate" ValidationGroup="KidneyDiscardedNo" CssClass="MandatoryFieldMessage" />


                    <asp:TextBox ID="txtReperfusionTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                    <asp:MaskedEditExtender ID="txtReperfusion_MaskedEditExtender" runat="server" 
                        Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                        ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                        TargetControlID="txtReperfusionTime">
                    </asp:MaskedEditExtender>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblTotalAnastomosisTime" runat="server"  Font-Size="Small" Text="Total Anastomosis Time (minutes)"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtTotalAnastomosisTime" runat="server" MaxLength="10" Width="15%" 
                        Enabled="false" ToolTip="Calculated Automatically when data is saved. The value is Reperfusion Date Time - Anastomosis Start Date Time"></asp:TextBox>
                    
                </td>
            </tr>
            
        
            
            <tr>
                <td  align="right">
                    <asp:label id="lblMannitolUsed" runat="server"  Font-Size="Small" Text="Mannitol Used"  />
                </td>
                <td  align="left">
                    <asp:RadioButtonList ID="rblMannitolUsed" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" >
                       
                    </asp:RadioButtonList>
                
                </td>
            </tr>

            <tr>
                <td  align="right">
                    <asp:label id="lblDiureticsUsed" runat="server"  Font-Size="Small" Text="Other Diuretics Used"  />
                </td>
                <td  align="left">
                    <asp:RadioButtonList ID="rblDiureticsUsed" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" >
                       
                    </asp:RadioButtonList>
                
                </td>
            </tr>
            <tr>
                <td  align="right">
                    <asp:label id="lblHypotensivePeriod" runat="server"  Font-Size="Small" Text="Hypotensive Period (Syst. < 100 mmHg)"  />
                </td>
                <td  align="left">
                    <asp:RadioButtonList ID="rblHypotensivePeriod" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" >
                       
                    </asp:RadioButtonList>
                
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblCVPReperfusion" runat="server"  Font-Size="Small" Text="CVP at Reperfusion (mm/Hg)"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCVPReperfusion" runat="server" MaxLength="4" Width="25%"></asp:TextBox>
                    <asp:CompareValidator runat="server" ID="cv_txtCVPReperfusion" Display="Dynamic" ErrorMessage="Please Enter Numeric value. Decimals are not allowed." 
                            Operator="DataTypeCheck" Type="Integer"  
                        ControlToValidate="txtCVPReperfusion" ValidationGroup="KidneyDiscardedNo" CssClass="MandatoryFieldMessage" />
                </td>
            </tr>
            
            <tr>
                <td  align="right">
                    <asp:label id="lblIntraOperativeDiuresis" runat="server"  Font-Size="Small" Text="Intra-operative Diuresis"  />
                </td>
                <td  align="left">
                    <asp:RadioButtonList ID="rblIntraOperativeDiuresis" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" >
                       
                    </asp:RadioButtonList>
                
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
        
        </asp:Panel>
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
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px" CausesValidation="true" ValidationGroup="Main"   UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
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
                        <asp:BoundField DataField="RPerioperativeID" SortExpression="RPerioperativeID" HeaderText="RIdentificationID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="RecipientID" SortExpression="RecipientID" HeaderText="RecipientID" Visible="false"
                            DataNavigateUrlFields="TrialID, TrialIDRecipient, RPerioperativeID " 
                            DataNavigateUrlFormatString="EditRPeriOperativeData.aspx?TID={0}&TID_R={1}&RPerioperativeID={2}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="TrialIDRecipient" SortExpression="TrialIDRecipient" HeaderText="TrialID Recipient"    />
                        <asp:BoundField DataField="Transplantation_Date" SortExpression="TransplantationDate" HeaderText="Transplant Date"    />
                        <asp:BoundField DataField="MachinePerfusion" SortExpression="MachinePerfusion" HeaderText="Machine Perfusion"    />
                        <asp:BoundField DataField="Operation_Start" SortExpression="OperationStartTime" HeaderText="Op Start"    />
                        <asp:BoundField DataField="KidneyDiscarded" SortExpression="KidneyDiscarded" HeaderText="Kidney Discarded"    />
                        <asp:BoundField DataField="Incision" SortExpression="Incision" HeaderText="Incision"    />
                        <asp:BoundField DataField="TransplantSide" SortExpression="TransplantSide" HeaderText="Transplant Side" />
                        <asp:BoundField DataField="TotalAnastomosisTime" SortExpression="TotalAnastomosisTime" HeaderText="Anastomosis Time"    />                       
                        <asp:BoundField DataField="StartAnastomosis" SortExpression="StartAnastomosis" HeaderText="Anastomosis Start" />
                        <asp:BoundField DataField="Reperfusion" SortExpression="Reperfusion" HeaderText="Reperfusion" />
                        <asp:BoundField DataField="IntraOperativeDiuresis" SortExpression="IntraOperativeDiuresis" HeaderText="Diuresis" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

