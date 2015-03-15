<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalRecipientPage.master" AutoEventWireup="true" CodeFile="AddRecipientSpecimen.aspx.cs" Inherits="SpecClinicalData_AddRecipientSpecimen" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddRecipientSpecimen" ContentPlaceHolderID="SpecimenContents" Runat="Server">
        <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="2" style="width: 1020px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td align="right" style="width:50%">
                <asp:label id="lblSide" runat="server"  Font-Size="Small" Text="Kidney Side"  />
                
            </td>
            <td align="left" style="width:50%">
                <asp:label id="lblSideValue" runat="server"  Font-Size="Small" Text="" Font-Bold="true"  />
                <asp:RadioButtonList ID="rblKidneyReceived" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" Visible="false" RepeatLayout="Flow">
                    </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLKidneySidesDataSource" runat="server" 
                        DataFile="~/App_Data/KidneySides.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlConsentQuod" Visible="false">
             <tr>
                <td align="right" style="width:50%">
                    <asp:label id="lblConsentQuod" runat="server"  Font-Size="Small" Text="Is the Donor included in QUOD (Tick if YES) *" Font-Bold="true" />
                </td>
                <td align="left" style="width:50%">
                     <asp:CheckBox ID="chkConsentQuod" runat="server"  />
                    <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
                </td>
            </tr>
        </asp:Panel>
        <tr>
            <td align="right" style="width:60%">
                <asp:label id="lblConsentAdditionalSamples" runat="server"  Font-Size="Small" Text="Has Recipient consented for additional samples to be taken for WP7 <br/>(Tick if YES) *" Font-Bold="true"  />
            </td>
            <td align="left" style="width:40%">                
                <asp:CheckBox ID="chkConsentAdditionalSamples" runat="server" AutoPostBack="true" OnCheckedChanged="chkConsentAdditionalSamples_CheckedChanged" /> 
            </td>
        </tr>
        <tr>
            <td align="right" >
                <asp:label id="lblResearcherName" runat="server"  Font-Size="Small" Text="Researcher Name"  />
            </td>
            <td align="left" >
                <asp:TextBox ID="txtResearcherName" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" >
                <asp:label id="lblResearcherTelephoneNumber" runat="server"  Font-Size="Small" Text="Researcher Telephone Number"  />
            </td>
            <td align="left" >
                <asp:TextBox ID="txtResearcherTelephoneNumber" runat="server" MaxLength="100" Width="50%" ></asp:TextBox>
                <asp:FilteredTextBoxExtender runat="server" ID="ftbetxtResearcherTelephoneNumber" TargetControlID="txtResearcherTelephoneNumber" FilterType="Numbers" FilterMode="ValidChars" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblSamplesStoredOxford" runat="server"  Font-Size="Small" Text="Samples Stored -80 Freezers Oxford (Date/Time)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtSamplesStoredOxfordDate" runat="server" MaxLength="10" Width="75px"    ></asp:TextBox>
                <asp:CalendarExtender ID="txtSamplesStoredOxfordDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtSamplesStoredOxfordDate" 
                    PopupPosition="Right">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtSamplesStoredOxfordTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtSamplesStoredOxfordTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtSamplesStoredOxfordTime">
                </asp:MaskedEditExtender>
                <asp:label id="lblSamplesStoredOxfordMessage" runat="server"  Font-Size="Small" Text=""  />
            </td>
        </tr>
        <tr>
            <td align="right" >
                <asp:label id="lblConsentComments" runat="server"  Font-Size="Small" Text="Consent Comments"  />
            </td>
            <td align="left" >
                <asp:TextBox ID="txtConsentComments" runat="server" MaxLength="500" Width="95%" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblWorksheetBarcode" runat="server"  Font-Size="Small" Text="Worksheet Barcode"  />
            </td>
            <td align="left" >
                <asp:TextBox ID="txtWorksheetBarcode" runat="server" MaxLength="10" Width="25%" ></asp:TextBox>
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlWP7Samples" Visible="false">
            <tr>
                <td colspan="2">
                    <table rules="all" border="1" cellpadding="2" cellspacing="2">
                        <tr>
                            <th colspan="9">
                                <asp:label id="lblPerfusateSamplesCollectedHeader" runat="server"  Font-Size="Small" Text="Perfusate Specimens Collected"  />
                            </th>

                        </tr>
                    
                        <tr>
                            <td style="width:15%" align="right">
                                <asp:label id="lblSampleCollected" runat="server"  Font-Size="Small" Text="Collection Point"  />
                            </td>
                            <td style="width:12.5%"  align="center">
                                <asp:label id="lblSampleType" runat="server"  Font-Size="Small" Text="Sample"  />
                            </td>
                            <td style="width:7.5%"  align="center">
                                <asp:label id="lblCollected" runat="server"  Font-Size="Small" Text="Collected"  />
                            </td>
                            <td style="width:12.5%"  align="center">
                                <asp:label id="lblBarcode" runat="server"  Font-Size="Small" Text="Barcode"  />
                            </td>
                            <td style="width:10%"  align="center">
                                <asp:label id="lblDateCollected" runat="server"  Font-Size="Small" Text="Date Collected"  />
                            </td>
                            <td style="width:10%"  align="center">
                                <asp:label id="lblTimeCollected" runat="server"  Font-Size="Small" Text="Time Collected"  />
                            </td>
                            <td style="width:8%"  align="center">
                                <asp:label id="lblDateCentrifugation" runat="server"  Font-Size="Small" Text="Date Centrifugation"  />
                            </td>
                            <td style="width:6%"  align="center">
                                <asp:label id="lbTimeCentrifugation" runat="server"  Font-Size="Small" Text="Time Centrifugation"  />
                            </td>
                            <td style="width:32.5%"  align="left">
                                <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="Comments"  />
                        </tr>
                        <tr>
                                <td  align="right">
                                    <asp:label id="lblP1_1" runat="server"  Font-Size="Small" Text="RKP3"   />
                                </td>
                                <td   align="center">
                                    <asp:DropDownList ID="ddP1_1"  runat="server" AppendDataBoundItems="True" 
                                        DataTextField="Text" DataValueField="Value" 
                                                Width="95%" Font-Bold="True" >
					                    <asp:ListItem Value="0">Select Sample P1.1</asp:ListItem>
										            
				                    </asp:DropDownList>
                                </td>
                                <td align="center">
                                    <asp:CheckBox ID="chkP1_1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked"/>
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtBarcodeP1_1" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblSpecimenIDP1_1" Text="" />
                                </td>
                                <td align="center" >
                                    <asp:TextBox ID="txtDateCollectedP1_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtDateCollectedP1_1_CalendarExtender" runat="server" 
                                        Enabled="True" TargetControlID="txtDateCollectedP1_1" Format="dd/MM/yyyy" PopupPosition="Right">
                                    </asp:CalendarExtender>
                            
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtTimeCollectedP1_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="txtTimeCollectedP1_1_MaskedEditExtender" runat="server" 
                                        Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                        ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                        TargetControlID="txtTimeCollectedP1_1">
                                    </asp:MaskedEditExtender>
                                </td>
                                <td align="center" >
                                    <asp:TextBox ID="txtDateCentrifugationP1_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtDateCentrifugationP1_1_CalendarExtender" runat="server" 
                                        Enabled="True" TargetControlID="txtDateCentrifugationP1_1" Format="dd/MM/yyyy" PopupPosition="Right">
                                    </asp:CalendarExtender>
                            
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtTimeCentrifugationP1_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="txtTimeCentrifugationP1_1_MaskedEditExtender" runat="server" 
                                        Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                        ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                        TargetControlID="txtTimeCentrifugationP1_1">
                                    </asp:MaskedEditExtender>
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtCommentsP1_1" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td  align="right">
                                    <asp:label id="lblP2_1" runat="server"  Font-Size="Small" Text="LKP3"   />
                                </td>
                                <td   align="center">
                                    <asp:DropDownList ID="ddP2_1"  runat="server" AppendDataBoundItems="True" 
                                        DataTextField="Text" DataValueField="Value" 
                                                Width="95%" Font-Bold="True">
					                    <asp:ListItem Value="0">Select Sample P2.1</asp:ListItem>
										            
				                    </asp:DropDownList>
                                </td>
                                <td align="center">
                                    <asp:CheckBox ID="chkP2_1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked"/>
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtBarcodeP2_1" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblSpecimenIDP2_1" Text="" />
                                </td>
                                <td align="center" >
                                    <asp:TextBox ID="txtDateCollectedP2_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtDateCollectedP2_1_CalendarExtender" runat="server" 
                                        Enabled="True" TargetControlID="txtDateCollectedP2_1" Format="dd/MM/yyyy" PopupPosition="Right">
                                    </asp:CalendarExtender>
                            
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtTimeCollectedP2_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="txtTimeCollectedP2_1_MaskedEditExtender" runat="server" 
                                        Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                        ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                        TargetControlID="txtTimeCollectedP2_1">
                                    </asp:MaskedEditExtender>
                                </td>
                                <td align="center" >
                                    <asp:TextBox ID="txtDateCentrifugationP2_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtDateCentrifugationP2_1_CalendarExtender" runat="server" 
                                        Enabled="True" TargetControlID="txtDateCentrifugationP2_1" Format="dd/MM/yyyy" PopupPosition="Right">
                                    </asp:CalendarExtender>
                            
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtTimeCentrifugationP2_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="txtTimeCentrifugationP2_1_MaskedEditExtender" runat="server" 
                                        Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                        ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                        TargetControlID="txtTimeCentrifugationP2_1">
                                    </asp:MaskedEditExtender>
                                </td>
                                <td align="center">
                                    <asp:TextBox ID="txtCommentsP2_1" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                                </td>
                            </tr>
                        <tr>
                            <td  align="right">
                                <asp:label id="lblP3_1" runat="server"  Font-Size="Small" Text="KP4"    />
                            </td>
                            <td   align="center">
                                <asp:DropDownList ID="ddP3_1"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="95%" Font-Bold="True" >
					                <asp:ListItem Value="0">Select Sample P3.1</asp:ListItem>
										            
				                </asp:DropDownList>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkP3_1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked"/>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeP3_1" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDP3_1" Text="" />
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtDateCollectedP3_1" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCollectedP3_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCollectedP3_1" Format="dd/MM/yyyy" PopupPosition="Right">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center"  >
                                <asp:TextBox ID="txtTimeCollectedP3_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCollectedP3_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCollectedP3_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtDateCentrifugationP3_1" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCentrifugationP3_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCentrifugationP3_1" Format="dd/MM/yyyy" PopupPosition="Right">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center"  >
                                <asp:TextBox ID="txtTimeCentrifugationP3_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCentrifugationP3_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCentrifugationP3_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtCommentsP3_1" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th colspan="9">
                                <asp:label id="lblBloodSpecimenCollectedHeader" runat="server"  Font-Size="Small" Text="Blood Specimens Collected"  />
                            </th>

                        </tr>
                        <tr>
                            <td  align="right" rowspan="2">
                                <asp:label id="lblRB1_1" runat="server"  Font-Size="Small" Text="RB1"  />
                            </td>
                            <td   align="center">
                                <asp:DropDownList ID="ddRB1_1"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="95%" Font-Bold="True" >
					                <asp:ListItem Value="0">Select Sample RB1.1</asp:ListItem>
										            
				                </asp:DropDownList>
                                <asp:XmlDataSource ID="XMLSpecimenTypesDataSource" runat="server" 
                                        DataFile="~/App_Data/SpecimenTypes.xml" XPath="/*/*" ></asp:XmlDataSource>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkRB1_1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeRB1_1" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDRB1_1" Text="" />
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtDateCollectedRB1_1" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCollectedRB1_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCollectedRB1_1" Format="dd/MM/yyyy" PopupPosition="Right">
                                </asp:CalendarExtender>
                                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                                </asp:ToolkitScriptManager>
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtTimeCollectedRB1_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCollectedRB1_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCollectedRB1_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtDateCentrifugationRB1_1" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCentrifugationRB1_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCentrifugationRB1_1" Format="dd/MM/yyyy" PopupPosition="Right">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtTimeCentrifugationRB1_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCentrifugationRB1_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCentrifugationRB1_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtCommentsRB1_1" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                        
                            <td   align="center">
                                <asp:DropDownList ID="ddRB1_2"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="95%" Font-Bold="True" >
					                <asp:ListItem Value="0">Select Sample RB1.1</asp:ListItem>
										            
				                </asp:DropDownList>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkRB1_2" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeRB1_2" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDRB1_2" Text="" />
                            </td>
                        
                            <td align="center">
                                <asp:TextBox ID="txtCommentsRB1_2" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td  align="right" rowspan="2">
                                <asp:label id="lblRB2_1" runat="server"  Font-Size="Small" Text="RB2"  />
                            </td>
                            <td   align="center">
                                <asp:DropDownList ID="ddRB2_1"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="95%" Font-Bold="True" >
					                <asp:ListItem Value="0">Select Sample RB2.1</asp:ListItem>
										            
				                </asp:DropDownList>
                                <asp:XmlDataSource ID="XmlDataSource1" runat="server" 
                                        DataFile="~/App_Data/SpecimenTypes.xml" XPath="/*/*" ></asp:XmlDataSource>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkRB2_1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeRB2_1" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDRB2_1" Text="" />
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtDateCollectedRB2_1" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCollectedRB2_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCollectedRB2_1" Format="dd/MM/yyyy" PopupPosition="Right">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtTimeCollectedRB2_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCollectedRB2_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCollectedRB2_1">
                                </asp:MaskedEditExtender>
                            </td>
                             <td align="center" rowspan="2">
                                <asp:TextBox ID="txtDateCentrifugationRB2_1" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCentrifugationRB2_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCentrifugationRB2_1" Format="dd/MM/yyyy" PopupPosition="Right">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtTimeCentrifugationRB2_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCentrifugationRB2_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCentrifugationRB2_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtCommentsRB2_1" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                        
                            <td   align="center">
                                <asp:DropDownList ID="ddRB2_2"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="95%" Font-Bold="True" >
					                <asp:ListItem Value="0">Select Sample RB2.1</asp:ListItem>
										            
				                </asp:DropDownList>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkRB2_2" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeRB2_2" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDRB2_2" Text="" />
                            </td>
                        
                            <td align="center">
                                <asp:TextBox ID="txtCommentsRB2_2" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th colspan="9">
                                <asp:label id="lblBiopsySamplesCollectedHeader" runat="server"  Font-Size="Small" Text="Biopsy Specimens Collected"  />
                            </th>

                        </tr>
                        <tr>
                            <td  align="right" rowspan="2">
                                <asp:label id="lblReK1R" runat="server"  Font-Size="Small" Text="RKP1"  />
                            </td>
                            <td   align="center">
                                <asp:DropDownList ID="ddPreservationReK1R"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="95%" Font-Bold="True" >
					                <asp:ListItem Value="0">Select Preservation</asp:ListItem>
										            
				                </asp:DropDownList>
                                <asp:XmlDataSource ID="XMLPreservationTypesDataSource" runat="server" 
                                        DataFile="~/App_Data/PreservationTypes.xml" XPath="/*/*" ></asp:XmlDataSource>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkReK1R" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked"/>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeReK1R" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDReK1R" Text="" />
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtDateCollectedReK1R" runat="server" MaxLength="10" Width="90%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCollectedReK1R_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCollectedReK1R" Format="dd/MM/yyyy" PopupPosition="Right">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtTimeCollectedReK1R" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCollectedReK1R_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCollectedReK1R">
                                </asp:MaskedEditExtender>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtCommentsReK1R" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                       <tr>
                        
                            <td   align="center">
                                <asp:DropDownList ID="ddPreservationReK1F"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="95%" Font-Bold="True" >
					                <asp:ListItem Value="0">Select Preservation</asp:ListItem>
										            
				                </asp:DropDownList>
                            
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkReK1F" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked"/>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeReK1F" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDReK1F" Text="" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                           <td>
                                &nbsp;
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtCommentsReK1F" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <asp:Panel runat="server" ID="pnlAllDataAdded" Visible="false">
                            <tr>
                                <td align="right"  colspan="2">
                                    <asp:label id="lblAllDataAdded" runat="server"  Font-Size="Small" Text="Tick Box if Data Entry Complete"  />
                                </td>
                                <td align="left" colspan="7">
                                    <asp:CheckBox runat="server" ID="chkAllDataAdded" AutoPostBack="true" OnCheckedChanged="chkAllDataAdded_CheckedChanged"/>
                                    <asp:Label runat="server" ID="lblAllDataAddedMessage"  Text=""  CssClass="Incomplete"/>
                
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlReasonModified" Visible="false">
                            <tr>
                                <td align="right"  colspan="2">
                                    <asp:label id="lblReasonModified" runat="server"  Font-Size="Small" Text="Enter Reason for Modifying Data *" Font-Bold="true"  />
                                </td>
                                <td align="left" colspan="7">
                                    <asp:TextBox ID="txtReasonModified" runat="server" MaxLength="10000" Width="95%" TextMode="MultiLine" Font-Names="Arial" Font-Size="Small" 
                                        onkeyup="AutoExpandTextBox(this, event)" Rows="2"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right"  colspan="2">
                                    <asp:label id="lblReasonModifiedOld" runat="server"  Font-Size="Small" Text="Reason(s) Data Modified Earlier"  />
                                </td>
                                <td align="left"  colspan="7">
                                    <asp:label id="lblReasonModifiedOldDetails" runat="server"  Font-Size="Small" Text="" BackColor="#99CCFF" Width="95%" />
                                </td>
                            </tr>
                        </asp:Panel>

                        <asp:Panel runat="server" ID="pnlFinal" Visible="false">
                            <tr>
                                <td align="right" colspan="2">
                                    <asp:label id="lblDataFinal" runat="server"  Font-Size="Small" Text="Tick to Mark Data as Final"  />
                                </td>
                                <td align="left" colspan="7">
                                    <asp:CheckBox runat="server" ID="chkDataFinal" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                                </asp:ConfirmButtonExtender>
                            </td>
                            <td align="left" colspan="7">
                                <asp:Button ID="cmdAddData" runat="server" Text="Add Specimen Data" Height="36px"   UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                            </td>
                        </tr>

                    </table>

                </td>
            
            </tr>
        </asp:Panel>
    </table>
    <table style="width:1020px">
        <tr align="center" >
            <th  colspan="2">
                <asp:Label runat="server" ID="lblGV1" Text="" />
            </th>
           
        </tr>
        <tr align="center">
            <td colspan="2">
                <asp:GridView ID="GV1" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True" OnRowDataBound="GV1_RowDataBound"   Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderText="SpecimenID" Visible="false" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblSpecimenID"
                                    Text='<%#Bind("SpecimenID")%>'>
                                </asp:Label>

                                
                            </ItemTemplate>
                            
                        </asp:TemplateField>
                        <asp:HyperLinkField DataTextField="TrialIDRecipient" SortExpression="TrialIDRecipient" HeaderText="TrialID (Recipient)" Visible="false"
                            DataNavigateUrlFields="TrialID, SpecimenID" 
                            DataNavigateUrlFormatString="EditRecipientSpecimen.aspx?TID={0}&TID_R={0}&SpecimenID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="TrialIDRecipient" SortExpression="TrialIDRecipient" HeaderText="TrialID Recipient"    />
                        <asp:BoundField DataField="Barcode" SortExpression="Barcode" HeaderText="Barcode"    />
                        <asp:BoundField DataField="Occasion" SortExpression="Occasion" HeaderText="Occasion"    />
                        <asp:BoundField DataField="Date_Collected" SortExpression="DateCollected" HeaderText="Date Collected"    />
                        <asp:BoundField DataField="Time_Collected" SortExpression="TimeCollected" HeaderText="Time Collected"    />
                        <asp:BoundField DataField="SpecimenType" SortExpression="SpecimenType" HeaderText="Specimen Type"    />
                        <asp:BoundField DataField="State" SortExpression="State" HeaderText="Preservation"    />
                        <asp:BoundField DataField="TissueSource" SortExpression="TissueSource" HeaderText="Left/Right Kidney"    />
                        <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12.5%" Visible="false">
                            <ItemTemplate>
                                <asp:Button ID="cmdDeleteRow" Text="Delete" Runat="server" OnClick="cmdDeleteRow_Click" />
                                <asp:ConfirmButtonExtender ID="cmdDeleteRow_ConfirmButtonExtender" runat="server" 
                                    ConfirmText="" Enabled="True" TargetControlID="cmdDeleteRow">
                    
                                </asp:ConfirmButtonExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="sqldsGV1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

