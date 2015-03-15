<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddDonorSpecimen.aspx.cs" Inherits="SpecClinicalData_AddDonorSpecimen" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddDonorSpecimen" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="2" style="width: 1020px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <asp:Panel runat="server" ID="pnlConsentQuod" Visible="false">
             <tr>
                <td align="right" style="width:60%">
                    <asp:label id="lblConsentQuod" runat="server"  Font-Size="Small" Text="Is the Donor included in QUOD (Tick if YES) *" Font-Bold="true" />
                </td>
                <td align="left" style="width:40%">
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
            <td align="right" >
                <asp:label id="lblWorksheetBarcode" runat="server"  Font-Size="Small" Text="Worksheet Barcode"  />
            </td>
            <td align="left" >
                <asp:TextBox ID="txtWorksheetBarcode" runat="server" MaxLength="10" Width="25%" ></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td colspan="2">
                <table rules="all" border="1" cellpadding="2" cellspacing="2">
                    <tr>
                        <th colspan="9">
                            <asp:label id="lblBloodSpecimenCollectedHeader" runat="server"  Font-Size="Small" Text="Blood Specimens Collected"  />
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
                        </td>
                    </tr>
                    
                    <asp:Panel runat="server" ID="pnlQuodConsent" Visible="false">
                        <tr>
                            <td  align="right">
                                <asp:label id="lblDU1_1" runat="server"  Font-Size="Small" Text="DU1" />
                            </td>
                            <td   align="center">
                                <asp:DropDownList ID="ddDU1_1"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="90%" Font-Bold="False" >
					                <asp:ListItem Value="0">Select Sample DU1.1</asp:ListItem>
										            
				                </asp:DropDownList>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkDU1_1" runat="server" AutoPostBack="True"  OnCheckedChanged="CheckedBoxSelection_Checked" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeDU1_1" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDDU1_1" Text="" />
                            </td>

                            <td align="center" >
                                <asp:TextBox ID="txtDateCollectedDU1_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCollectedDU1_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCollectedDU1_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtTimeCollectedDU1_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCollectedDU1_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCollectedDU1_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtDateCentrifugationDU1_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCentrifugationDU1_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCentrifugationDU1_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtTimeCentrifugationDU1_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCentrifugationDU1_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCentrifugationDU1_1">
                                </asp:MaskedEditExtender>
                            </td>
                       
                            <td align="center">
                                <asp:TextBox ID="txtCommentsDU1_1" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td  align="right" rowspan="2">
                                <asp:label id="lblDB1_1" runat="server"  Font-Size="Small" Text="DB1"  />
                            </td>
                            <td   align="center">
                                <asp:DropDownList ID="ddDB1_1"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="90%" Font-Bold="False" >
					                <asp:ListItem Value="0">Select Sample DB1.1</asp:ListItem>
										            
				                </asp:DropDownList>
                                <asp:XmlDataSource ID="XMLSpecimenTypesDataSource" runat="server" 
                                        DataFile="~/App_Data/SpecimenTypes.xml" XPath="/*/*" ></asp:XmlDataSource>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkDB1_1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeDB1_1" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDDB1_1" Text="" />
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtDateCollectedDB1_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCollectedDB1_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCollectedDB1_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                                </asp:ToolkitScriptManager>
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtTimeCollectedDB1_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCollectedDB1_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCollectedDB1_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtDateCentrifugationDB1_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCentrifugationDB1_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCentrifugationDB1_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center" rowspan="2">
                                <asp:TextBox ID="txtTimeCentrifugationDB1_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCentrifugationDB1_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCentrifugationDB1_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtCommentsDB1_1" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                        
                            <td   align="center">
                                <asp:DropDownList ID="ddDB1_2"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="90%" Font-Bold="False" >
					                <asp:ListItem Value="0">Select Sample DB1.1</asp:ListItem>
										            
				                </asp:DropDownList>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkDB1_2" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeDB1_2" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDDB1_2" Text="" />
                            </td>
                        
                            <td align="center">
                                <asp:TextBox ID="txtCommentsDB1_2" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td  align="right">
                                <asp:label id="lblDU2_1" runat="server"  Font-Size="Small" Text="DU2" />
                            </td>
                            <td   align="center">
                                <asp:DropDownList ID="ddDU2_1"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="90%" Font-Bold="False" >
					                <asp:ListItem Value="0">Select Sample DU1.2</asp:ListItem>
										            
				                </asp:DropDownList>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkDU2_1" runat="server" AutoPostBack="True"  OnCheckedChanged="CheckedBoxSelection_Checked" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeDU2_1" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDDU2_1" Text="" />
                            </td>
                       	    <td align="center" >
                                <asp:TextBox ID="txtDateCollectedDU2_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCollectedDU2_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCollectedDU2_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtTimeCollectedDU2_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCollectedDU2_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCollectedDU2_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtDateCentrifugationDU2_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCentrifugationDU2_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCentrifugationDU2_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtTimeCentrifugationDU2_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCentrifugationDU2_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCentrifugationDU2_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtCommentsDU2_1" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>

                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlWP7Samples" Visible="false">
                        <tr>
                            <th colspan="9">
                                <asp:label id="lblBloodPerfusateCollectedHeader" runat="server"  Font-Size="Small" Text="Perfusate Specimens Collected"  />
                            </th>

                        </tr>
                        <tr>
                            <td  align="right">
                                <asp:label id="lblRKP1_1" runat="server"  Font-Size="Small" Text="RKP1"   />
                            </td>
                            <td   align="center">
                                <asp:DropDownList ID="ddRKP1_1"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="90%" Font-Bold="False" >
					                <asp:ListItem Value="0">Select Sample RKP1.1</asp:ListItem>
										            
				                </asp:DropDownList>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkRKP1_1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked"/>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeRKP1_1" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDRKP1_1" Text="" />
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtDateCollectedRKP1_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCollectedRKP1_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCollectedRKP1_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTimeCollectedRKP1_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCollectedRKP1_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCollectedRKP1_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtDateCentrifugationRKP1_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCentrifugationRKP1_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCentrifugationRKP1_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTimeCentrifugationRKP1_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCentrifugationRKP1_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCentrifugationRKP1_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtCommentsRKP1_1" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td  align="right">
                                <asp:label id="lblLKP1_1" runat="server"  Font-Size="Small" Text="LKP1"  />
                            </td>
                            <td   align="center">
                                <asp:DropDownList ID="ddLKP1_1"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="90%" Font-Bold="False" >
					                <asp:ListItem Value="0">Select Sample LKP1.1</asp:ListItem>
										            
				                </asp:DropDownList>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkLKP1_1" runat="server" AutoPostBack="True"  OnCheckedChanged="CheckedBoxSelection_Checked" />
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeLKP1_1" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDLKP1_1" Text="" />
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtDateCollectedLKP1_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCollectedLKP1_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCollectedLKP1_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTimeCollectedLKP1_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCollectedLKP1_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCollectedLKP1_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtDateCentrifugationLKP1_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCentrifugationLKP1_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCentrifugationLKP1_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTimeCentrifugationLKP1_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCentrifugationLKP1_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCentrifugationLKP1_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtCommentsLKP1_1" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td  align="right">
                                <asp:label id="lblRKP2_1" runat="server"  Font-Size="Small" Text="RKP3"   />
                            </td>
                            <td   align="center">
                                <asp:DropDownList ID="ddRKP2_1"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="90%" Font-Bold="False" >
					                <asp:ListItem Value="0">Select Sample RKP3.1</asp:ListItem>
										            
				                </asp:DropDownList>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkRKP2_1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked"/>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeRKP2_1" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDRKP2_1" Text="" />
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtDateCollectedRKP2_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCollectedRKP2_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCollectedRKP2_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTimeCollectedRKP2_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCollectedRKP2_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCollectedRKP2_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtDateCentrifugationRKP2_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCentrifugationRKP2_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCentrifugationRKP2_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTimeCentrifugationRKP2_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCentrifugationRKP2_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCentrifugationRKP2_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtCommentsRKP2_1" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td  align="right">
                                <asp:label id="lblLKP2_1" runat="server"  Font-Size="Small" Text="LKP3"   />
                            </td>
                            <td   align="center">
                                <asp:DropDownList ID="ddLKP2_1"  runat="server" AppendDataBoundItems="True" 
                                    DataTextField="Text" DataValueField="Value" 
                                            Width="90%" Font-Bold="False" >
					                <asp:ListItem Value="0">Select Sample LKP3.1</asp:ListItem>
										            
				                </asp:DropDownList>
                            </td>
                            <td align="center">
                                <asp:CheckBox ID="chkLKP2_1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckedBoxSelection_Checked"/>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtBarcodeLKP2_1" runat="server" MaxLength="15" Width="90%"></asp:TextBox>
                                <asp:Label runat="server" ID="lblSpecimenIDLKP2_1" Text="" />
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtDateCollectedLKP2_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCollectedLKP2_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCollectedLKP2_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTimeCollectedLKP2_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCollectedLKP2_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCollectedLKP2_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center" >
                                <asp:TextBox ID="txtDateCentrifugationLKP2_1" runat="server" MaxLength="10" Width="95%"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDateCentrifugationLKP2_1_CalendarExtender" runat="server" 
                                    Enabled="True" TargetControlID="txtDateCentrifugationLKP2_1" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtTimeCentrifugationLKP2_1" runat="server" MaxLength="100" Width="75%"></asp:TextBox>
                                <asp:MaskedEditExtender ID="txtTimeCentrifugationLKP2_1_MaskedEditExtender" runat="server" 
                                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                    TargetControlID="txtTimeCentrifugationLKP2_1">
                                </asp:MaskedEditExtender>
                            </td>
                            <td align="center">
                                <asp:TextBox ID="txtCommentsLKP2_1" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                            </td>
                        </tr>
                    </asp:Panel>
                    
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
                        <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID" Visible="false"
                            DataNavigateUrlFields="TrialID, SpecimenID" 
                            DataNavigateUrlFormatString="EditDonorSpecimen.aspx?TID={0}&SpecimenID={1}">
                        </asp:HyperLinkField>
                        
                        <asp:BoundField DataField="Occasion" SortExpression="Occasion" HeaderText="Occasion"    />
                        <asp:BoundField DataField="Collected" SortExpression="Collected" HeaderText="Collected"    />
                        <asp:BoundField DataField="Barcode" SortExpression="Barcode" HeaderText="Barcode"    />
                        <asp:BoundField DataField="Date_Collected" SortExpression="DateCollected" HeaderText="Date Collected"    />
                        <asp:BoundField DataField="Time_Collected" SortExpression="TimeCollected" HeaderText="Time Collected"    />
                        <asp:BoundField DataField="SpecimenType" SortExpression="SpecimenType" HeaderText="Specimen Type"    />
                        <asp:BoundField DataField="TissueSource" SortExpression="TissueSource" HeaderText="Left/Right Kidney"    />
                        <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12.5%">
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

