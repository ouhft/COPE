<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddMPGData.aspx.cs" Inherits="SpecClinicalData_AddMPGData" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddMPGData" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlUpdate1">
        <ContentTemplate>
            <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
                <tr>
                    <th colspan="2">
                        <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
                    </th>
                </tr>
                <tr>
                    <td style="width:35%" align="right">
                        <asp:label id="lblSide" runat="server"  Font-Size="Small" Text="Kidney Side *" Font-Bold="true"  />
                    </td>
                    <td style="width:65%" align="left">
                        <asp:DropDownList ID="ddSide" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="35%"  
                                AutoPostBack="true" OnSelectedIndexChanged="ddSide_SelectedIndexChanged"  >
                            <asp:ListItem Selected="True" Text="Select a Side" Value="0" />
                           
                        </asp:DropDownList>
                        <asp:XmlDataSource ID="XMLKidneySidesDataSource" runat="server" 
                                DataFile="~/App_Data/KidneySides.xml" XPath="/*/*" ></asp:XmlDataSource>
                        <asp:RequiredFieldValidator runat="server" ID="rfv_ddSide" ControlToValidate="ddSide" InitialValue="0" Display="Dynamic" ErrorMessage="Please Select Side" 
                                            CssClass="MandatoryFieldMessage" ValidationGroup="Main" />
                
                    </td>
                </tr>
                <asp:Panel ID="pnlSideSelected" runat="server" Visible="false">
        
                    <tr>
                        <td style="width:35%" align="right">
                            <asp:label id="lblKidneyOnMachine" runat="server"  Font-Size="Small" Text="Is Machine Perfusion Possible? *" Font-Bold="true"  />
                        </td>
                        <td style="width:65%" align="left">
                            <asp:RadioButtonList ID="rblKidneyOnMachine" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblKidneyOnMachine_SelectedIndexChanged" RepeatLayout="Flow">
                       
                            </asp:RadioButtonList>
                            <asp:XmlDataSource ID="XMLMainOptionsYNDataSource" runat="server" 
                                    DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
                            <asp:RequiredFieldValidator runat="server" ID="rfvrblKidneyOnMachine" ControlToValidate="rblKidneyOnMachine" Display="Dynamic" ErrorMessage="Please Select YES/NO" 
                                CssClass="MandatoryFieldMessage" ValidationGroup="Main" />
                            <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
                        </td>
                    </tr>
            
            
                        <asp:Panel ID="pnlKidneyOnMachineSelectedNo" runat="server">
                            <tr>
                                <td align="right">
                                    <asp:label id="lblKidneyOnMachineNo" runat="server"  Font-Size="Small" Text="If NO, please give reason *" Font-Bold="true"  />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtKidneyOnMachineNo" runat="server" MaxLength="500" TextMode="MultiLine" Width="95%" ></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="rfv_txtKidneyOnMachineNo" ControlToValidate="rblKidneyOnMachine" Display="Dynamic" ErrorMessage="Please provide details" 
                                            CssClass="MandatoryFieldMessage" ValidationGroup="KidneyOnMachineNo" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="pnlKidneyOnMachineSelectedYes" runat="server">
                            <tr>
                                <td align="right">
                                    <asp:label id="lblPerfusionStartDate" runat="server"  Font-Size="Small" Text="Perfusion Start (Date Time) *" Font-Bold="true"  />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPerfusionStartDate" runat="server" MaxLength="10" Width="75px" ></asp:TextBox>
                                    <asp:CalendarExtender ID="txtPerfusionStartDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtPerfusionStartDate" PopupPosition="Right">
                                    </asp:CalendarExtender>
                                    <asp:RequiredFieldValidator runat="server" ID="rfv_txtPerfusionStartDate" ControlToValidate="txtPerfusionStartDate" Display="Dynamic" ErrorMessage="Please Enter Date" 
                                            CssClass="MandatoryFieldMessage" ValidationGroup="KidneyOnMachineYes" />

                                    <asp:TextBox ID="txtPerfusionStartTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="rfv_txtPerfusionStartTime" ControlToValidate="txtPerfusionStartTime" Display="Dynamic" ErrorMessage="Please Enter Time" 
                                            CssClass="MandatoryFieldMessage" ValidationGroup="KidneyOnMachineYes" />
                                    <asp:MaskedEditExtender ID="txtPerfusionStartTime_MaskedEditExtender" runat="server" 
                                        Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                        ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                        TargetControlID="txtPerfusionStartTime">
                                    </asp:MaskedEditExtender>
                                    <asp:MaskedEditValidator   
                                        ID="mevtxtPerfusionStartTime"  
                                        runat="server"  
                                        ControlToValidate="txtPerfusionStartTime"  
                                        ControlExtender="txtPerfusionStartTime_MaskedEditExtender"  
                                        IsValidEmpty="True" 
                      
                                        InvalidValueMessage="Please enter Time in 24 hour format"   CssClass="MandatoryFieldMessage">
                                    </asp:MaskedEditValidator>
                            
                                </td>
                            </tr>

                            <tr>
                                <td align="right">
                                    <asp:label id="lblMachineSerialNumber" runat="server"  Font-Size="Small" Text="Serial Number (SN) of Kidney Assist Machine" Font-Bold="true"  />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtMachineSerialNumber" runat="server" MaxLength="45" Width="25%"  ></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="rev_txtMachineSerialNumber" runat="server" EnableClientScript="true" 
                                        ErrorMessage="Please Enter numbers only" CssClass="MandatoryFieldMessage" 
                                        ControlToValidate="txtMachineSerialNumber" ValidationGroup="KidneyOnMachineYes" ValidationExpression="^[0-9]*$"  Display="Dynamic"/>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:label id="lblMachineReferenceModelNumber" runat="server"  Font-Size="Small" Text="Machine Reference Model (REF) Number" Font-Bold="true"  />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtMachineReferenceModelNumber" runat="server" MaxLength="45" Width="25%"  ></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="rev_txtMachineReferenceModelNumber" runat="server" EnableClientScript="true" 
                                        ErrorMessage="Please enter Alphabet/Numbers only" CssClass="MandatoryFieldMessage" 
                                        ControlToValidate="txtMachineReferenceModelNumber" ValidationGroup="KidneyOnMachineYes" ValidationExpression="^[0-9a-zA-Z]*$"  Display="Dynamic"/>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:label id="lblLotNumberPerfusionSolution" runat="server"  Font-Size="Small" Text="Lot Number of Perfusion Solution" Font-Bold="true"  />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtLotNumberPerfusionSolution" runat="server" MaxLength="45" Width="25%"  ></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="rev_txtLotNumberPerfusionSolution" runat="server" EnableClientScript="true" 
                                        ErrorMessage="Please enter Alphabet/Numbers/Space only" CssClass="MandatoryFieldMessage" 
                                        ControlToValidate="txtLotNumberPerfusionSolution" ValidationGroup="KidneyOnMachineYes" ValidationExpression="^[0-9a-zA-Z\s]*$"  Display="Dynamic"/>
                                </td>
                            </tr>

                            <tr>
                                <td align="right">
                                    <asp:label id="lblLotNumberDisposables" runat="server"  Font-Size="Small" Text="Lot Number of Disposables" Font-Bold="true"  />
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtLotNumberDisposables" runat="server" MaxLength="45" Width="25%"  ></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="rev_txtLotNumberDisposables" runat="server" EnableClientScript="true" 
                                        ErrorMessage="Please enter Alphabet/Numbers/Space only" CssClass="MandatoryFieldMessage" 
                                        ControlToValidate="txtLotNumberDisposables" ValidationGroup="KidneyOnMachineYes" ValidationExpression="^[0-9a-zA-Z\s]*$"  Display="Dynamic"/>
                                </td>
                            </tr>

                            <tr>
                                <td align="right">
                                    <asp:label id="lblUsedPatchHolder" runat="server"  Font-Size="Small" Text="Used Patch Holder" Font-Bold="true"  />
                                </td>
                                <td  align="left">
                                    <asp:DropDownList ID="ddUsedPatchHolder" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="25%">
                                        <asp:ListItem Selected="True" Text="Select an Option" Value="0"  />   
                                    </asp:DropDownList>
                                    <asp:XmlDataSource ID="XMLUsedPatchHolderSizesDataSource" runat="server" 
                                            DataFile="~/App_Data/UsedPatchHolderSizes.xml" XPath="/*/*" ></asp:XmlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:label id="lblArtificialPatchUsed" runat="server"  Font-Size="Small" Text="Artificial Patch Used" Font-Bold="true"  />
                                </td>
                                <td  align="left">
                                    <asp:DropDownList ID="ddArtificialPatchUsed" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="25%"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddArtificialPatchUsed_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Text="Select an Option" Value="0"  />   
                                    </asp:DropDownList>
                        
                                </td>
                            </tr>
                            <asp:Panel ID="pnlArtificialPatchUsedYes" runat="server" Visible="false">
                                <tr>
                                    <td align="right">
                                        <asp:label id="lblArtificialPatchSize" runat="server"  Font-Size="Small" Text="Artificial Patch Size" Font-Bold="false"  />
                                    </td>
                                    <td  align="left">
                                        <asp:DropDownList ID="ddArtificialPatchSize" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="25%">
                                            <asp:ListItem Selected="True" Text="Select an Option" Value="0"  />   
                                        </asp:DropDownList>
                                        <asp:XmlDataSource ID="XMLArtificialPatchSizesDataSource" runat="server" 
                                            DataFile="~/App_Data/ArtificialPatchSizes.xml" XPath="/*/*" ></asp:XmlDataSource>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:label id="lblArtificialPatchNumber" runat="server"  Font-Size="Small" Text="Artificial Patch Number" Font-Bold="false"  />
                                    </td>
                                    <td  align="left">
                                        <asp:DropDownList ID="ddArtificialPatchNumber" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="25%">
                                            <asp:ListItem Selected="True" Text="Select an Option" Value="0"  />   
                                        </asp:DropDownList>
                                        <asp:XmlDataSource ID="XMLArtificialPatchNumbersDataSource" runat="server" 
                                            DataFile="~/App_Data/ArtificialPatchNumbers.xml" XPath="/*/*" ></asp:XmlDataSource>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnlOxygenBottleFullOpen" Visible="false">
                                <tr>
                                    <td align="right">
                                        <asp:label id="lblOxygenBottleFull" runat="server"  Font-Size="Small" Text="Is the Oxygen bottle full?" Font-Bold="true"  />
                                    </td>
                                    <td  align="left">
                                        <asp:DropDownList ID="ddOxygenBottleFull" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="25%"
                                            AutoPostBack="false" >
                                            <asp:ListItem Selected="True" Text="Select an Option" Value="0"  />   
                                        </asp:DropDownList>
                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:label id="lblOxygenBottleOpened" runat="server"  Font-Size="Small" Text="Opened Oxygen Bottle?" Font-Bold="true"  />
                                    </td>
                                    <td  align="left">
                                        <asp:DropDownList ID="ddOxygenBottleOpened" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="25%"
                                            AutoPostBack="false" >
                                            <asp:ListItem Selected="True" Text="Select an Option" Value="0"  />   
                                        </asp:DropDownList>
                        
                                    </td>
                                </tr>
                            </asp:Panel>
                            
                            <tr>
                                <td align="right">
                                    <asp:label id="lblOxygenTankChanged" runat="server"  Font-Size="Small" Text="Oxygen Tank Changed" Font-Bold="false"  />
                                </td>
                                <td  align="left">
                                    <asp:DropDownList ID="ddOxygenTankChanged" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="25%"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddOxygenTankChanged_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Text="Select an Option" Value="0"  />   
                                    </asp:DropDownList>
                        
                                </td>
                            </tr>
                            <asp:Panel ID="pnlOxygenTankChanged" runat="server" Visible="false">
                                <tr>                    
                                    <td align="right">
                                        <asp:label id="lblOxygenTankChangedDateTime" runat="server"  Font-Size="Small" Text="Oxygen Tank Changed (Date/Time)" Font-Bold="false"  />
                                    </td>
                                    <td  align="left">                        
                                        <asp:TextBox ID="txtOxygenTankChangedDate" runat="server" MaxLength="10" Width="75px" ></asp:TextBox>
                                        <asp:CalendarExtender ID="txtOxygenTankChangedDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtOxygenTankChangedDate" PopupPosition="Right">
                                        </asp:CalendarExtender>
                        

                                        <asp:TextBox ID="txtOxygenTankChangedTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                                        <asp:MaskedEditExtender ID="txtOxygenTankChangedTime_MaskedEditExtender" runat="server" 
                                            Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                            ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                            TargetControlID="txtOxygenTankChangedTime">
                                        </asp:MaskedEditExtender>
                                        <asp:MaskedEditValidator   
                                            ID="mevtxtOxygenTankChangedTime"  
                                            runat="server"  
                                            ControlToValidate="txtOxygenTankChangedTime"  
                                            ControlExtender="txtOxygenTankChangedTime_MaskedEditExtender"  
                                            IsValidEmpty="True" 
                      
                                            InvalidValueMessage="Please enter Time in 24 hour format"   CssClass="MandatoryFieldMessage">
                                        </asp:MaskedEditValidator>
                                    </td>
                                </tr>

                            </asp:Panel>
                            <tr>
                                <td align="right">
                                    <asp:label id="lblIceContainerReplenished" runat="server"  Font-Size="Small" Text="Ice Container Replenished" Font-Bold="false"  />
                                </td>
                                <td  align="left">
                                    <asp:DropDownList ID="ddIceContainerReplenished" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="25%"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddIceContainerReplenished_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Text="Select an Option" Value="0"  />   
                                    </asp:DropDownList>
                        
                                </td>
                            </tr>
                            <asp:Panel ID="pnlIceContainerReplenished" runat="server" Visible="false">
                                <tr>                    
                                    <td align="right">
                                        <asp:label id="lblIceContainerReplenishedDateTime" runat="server"  Font-Size="Small" Text="Ice Container Replenished (Date/Time)" Font-Bold="false"  />
                                    </td>
                                    <td  align="left">                        
                                        <asp:TextBox ID="txtIceContainerReplenishedDate" runat="server" MaxLength="10" Width="75px" ></asp:TextBox>
                                        <asp:CalendarExtender ID="txtIceContainerReplenishedDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtIceContainerReplenishedDate" PopupPosition="Right">
                                        </asp:CalendarExtender>
                        

                                        <asp:TextBox ID="txtIceContainerReplenishedTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                                        <asp:MaskedEditExtender ID="txtIceContainerReplenishedTime_MaskedEditExtender" runat="server" 
                                            Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                                            ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                                            TargetControlID="txtIceContainerReplenishedTime">
                                        </asp:MaskedEditExtender>
                                        <asp:MaskedEditValidator   
                                            ID="mevtxtIceContainerReplenishedTime"  
                                            runat="server"  
                                            ControlToValidate="txtIceContainerReplenishedTime"  
                                            ControlExtender="txtIceContainerReplenishedTime_MaskedEditExtender"  
                                            IsValidEmpty="True" 
                      
                                            InvalidValueMessage="Please enter Time in 24 hour format"   CssClass="MandatoryFieldMessage">
                                        </asp:MaskedEditValidator>
                                    </td>
                                </tr>

                            </asp:Panel>

                            <tr>
                                <td align="right">
                                    <asp:label id="lblLogisticallyPossibleMeasurepO2Perfusate" runat="server"  Font-Size="Small" Text="Is it Logistically possible to measure pO2 of Perfusate?" Font-Bold="true"  />
                                </td>
                                <td  align="left">
                                    <asp:DropDownList ID="ddLogisticallyPossibleMeasurepO2Perfusate" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="25%"
                                        AutoPostBack="true" OnSelectedIndexChanged="ddLogisticallyPossibleMeasurepO2Perfusate_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Text="Select an Option" Value="0"  />   
                                    </asp:DropDownList>
                        
                                </td>
                            </tr>
                            <asp:Panel ID="pnlLogisticallyPossibleMeasurepO2Perfusate" runat="server" Visible="false">
                                <asp:Panel ID="pnlValuepO2Perfusate" runat="server" Visible="false">
                                    <tr>                    
                                        <td align="right">
                                            <asp:label id="lblValuepO2Perfusate" runat="server"  Font-Size="Small" Text="Value pO2 of Perfusate" Font-Bold="true"  />
                                        </td>
                                        <td  align="left">                        
                                            <asp:TextBox ID="txtValuepO2Perfusate" runat="server" MaxLength="10" Width="25%" ></asp:TextBox>
                                            <asp:CompareValidator runat="server" ID="cv_txtValuepO2Perfusate" Display="Dynamic" ErrorMessage="Please Enter Numeric value. Decimals are allowed." 
                                            Operator="DataTypeCheck" Type="Double"  
                                                ControlToValidate="txtValuepO2Perfusate" ValidationGroup="KidneyOnMachineYes" CssClass="MandatoryFieldMessage" />
                                        </td>
                                </tr>

                                </asp:Panel>
                        
                                <tr>                    
                                    <td align="right">
                                        <asp:label id="lblValuepO2PerfusateMeasured" runat="server"  Font-Size="Small" Text="How was pO2 measured?" Font-Bold="true"  />
                                    </td>
                                    <td  align="left">                        
                                        <asp:TextBox ID="txtValuepO2PerfusateMeasured" runat="server" MaxLength="100" Width="75%" ></asp:TextBox>
                                    </td>
                                </tr>
                            </asp:Panel>
                        </asp:Panel>
            
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
                    
                </asp:Panel>
            </table>
            
        </ContentTemplate>

    </asp:UpdatePanel>
    <table  cellpadding="2" cellspacing="2" border="0" style="width: 851px">
        <tr>
            <td align="right" style="width:35%" >
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
            <td align="left" style="width:65%" >
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
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True"   Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="MachinePerfusionID" SortExpression="MachinePerfusionID" HeaderText="MachinePerfusionID" Visible="false"    />
                        
                        <asp:BoundField DataField="TrialID" SortExpression="TrialID" HeaderText="TrialID"    />
                        <asp:TemplateField HeaderText="Side" Visible="false" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblSide"
                                    Text='<%#Bind("Side")%>'>
                                </asp:Label>                                
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:HyperLinkField DataTextField="Side" SortExpression="Side" HeaderText="Side" Visible="false"
                            DataNavigateUrlFields="TrialID, MachinePerfusionID, Side" 
                            DataNavigateUrlFormatString="AddMPGData.aspx?TID={0}&MachinePerfusionID={1}&Side={2}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="Side" SortExpression="Side" HeaderText="Side"    />
                        <asp:BoundField DataField="KidneyOnMachine" SortExpression="KidneyOnMachine" HeaderText="Kidney On Machine"    />
                        <asp:BoundField DataField="UsedPatchHolder" SortExpression="UsedPatchHolder" HeaderText="Used Patch Holder"    />
                        <asp:BoundField DataField="ArtificialPatchUsed" SortExpression="ArtificialPatchUsed" HeaderText="Artificial Patch Used"    />
                        <asp:BoundField DataField="PerfusionStart_Date" SortExpression="PerfusionStartDate" HeaderText="Perfusion Start Date"    />
                        <asp:BoundField DataField="PerfusionStart_Time" SortExpression="PerfusionStartTime" HeaderText="Perfusion Start Time" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

