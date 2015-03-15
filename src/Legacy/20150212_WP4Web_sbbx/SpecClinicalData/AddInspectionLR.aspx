<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddInspectionLR.aspx.cs" Inherits="SpecClinicalData_AddInspectionLR" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddInspectionLR" ContentPlaceHolderID="SpecimenContents" Runat="Server">

    <asp:UpdatePanel runat="server" ID="upnl1">
        <ContentTemplate>
        
            <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
            <table rules="all" cellpadding="2" cellspacing="2" border="2" style="width: 1050px">
                <tr>
                    <th colspan="3">
                        <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
                    </th>
                </tr>
                <tr>
                    <th style="width:30%" align="center">
                        <asp:label id="lblFields" runat="server"  Font-Size="Small" Text=""  />
                    </th>
                    <th style="width:35%"  align="center">
                        <asp:label id="lblLeftKidney" runat="server"  Font-Size="Small" Text="Left Kidney"  />
                    </th>
                    <th style="width:35%"  align="center">
                        <asp:label id="lblRightKidney" runat="server"  Font-Size="Small" Text="Right Kidney"  />
                    </th>
                </tr>
                <asp:Panel runat="server" ID="pnlKidneySides" Visible="false">
                    <tr>
                        <td align="right">
                            <asp:label id="lblSide" runat="server"  Font-Size="Small" Text="Kidney Side"  />
                        </td>
                        <td   align="center">
                            <asp:DropDownList ID="ddSide" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="35%" 
                                AutoPostBack="false" >
                                <asp:ListItem Selected="True" Text="Select Kidney Side" Value="0"  />   
                            </asp:DropDownList>
                            <asp:XmlDataSource ID="XMLKidneySidesDataSource" runat="server" 
                                    DataFile="~/App_Data/KidneySides.xml" XPath="/*/*" ></asp:XmlDataSource>
                        </td>
                        <td   align="center">
                            <asp:DropDownList ID="ddSide_R" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="35%" 
                                AutoPostBack="false" >
                                <asp:ListItem Selected="True" Text="Select Kidney Side" Value="0"  />   
                            </asp:DropDownList>
                        </td>
                    </tr>
                </asp:Panel>
        
                <tr>
                    <td align="right">
                        <asp:label id="lblPreservationModality" runat="server"  Font-Size="Small" Text="Preservation Modality" Font-Bold="false" />
                    </td>
                    <td align="center">
                        <asp:DropDownList ID="ddPreservationModality" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Enabled="false" >
                               <asp:ListItem Selected="True" Text="Select an Option" Value="0" />
                        </asp:DropDownList>
                        <asp:XmlDataSource ID="XMLPreservationModalitiesDataSource" runat="server" 
                                DataFile="~/App_Data/PreservationModalities.xml" XPath="/*/*" ></asp:XmlDataSource>
                    </td>
                    <td  align="center">
                        <asp:DropDownList ID="ddPreservationModality_R" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Enabled="false" >
                               <asp:ListItem Selected="True" Text="Select an Option" Value="0" />
                        </asp:DropDownList>
                        <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
                    </td>
                </tr>
                <asp:Panel runat="server" ID="pnlRandomisationComplete" Visible="true">
                    <tr>
                        <td align="right">
                            <asp:label id="lblRandomisationComplete" runat="server"  Font-Size="Small" Text="Randomisation Complete"  />
                        </td>
                        <td align="center">
                            <asp:RadioButtonList ID="rblRandomisationComplete" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" Enabled="false">
                            </asp:RadioButtonList>
                            <asp:XmlDataSource ID="XMLMainOptionsYNDataSource" runat="server" 
                                    DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
                        </td>
                        <td  align="center">
                            <asp:RadioButtonList ID="rblRandomisationComplete_R" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" Enabled="false">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </asp:Panel>
                
        
       
                <tr>
                    <td align="right">
                        <asp:label id="lblNumberRenalArteries" runat="server"  Font-Size="Small" Text="Number of Renal Arteries"  />
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtNumberRenalArteries" runat="server" MaxLength="2" Width="25%" ></asp:TextBox>
                        <asp:CompareValidator ID="cv_txtNumberRenalArteries" runat="server" Display="Dynamic" ControlToValidate="txtNumberRenalArteries" Type="Integer" 
                            Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are not allowed" ValidationGroup="MainGroup" ></asp:CompareValidator>
                    </td>
                    <td style="width:30%"  align="center">
                        <asp:TextBox ID="txtNumberRenalArteries_R" runat="server" MaxLength="2" Width="25%" ></asp:TextBox>
                        <asp:CompareValidator ID="cv_txtNumberRenalArteries_R" runat="server" Display="Dynamic" ControlToValidate="txtNumberRenalArteries_R" Type="Integer" 
                            Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are not allowed" ValidationGroup="MainGroup" ></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="lblArterialProblems" runat="server"  Font-Size="Small" Text="Renal Graft Damage *" Font-Bold="true"  />
                    </td>
                    <td align="center">
                        <asp:CheckBoxList ID="cblArterialProblems" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Vertical">
                    
                        </asp:CheckBoxList>
                        <asp:XmlDataSource ID="XMLArterialProblemsDataSource" runat="server" 
                                DataFile="~/App_Data/ArterialProblems.xml" XPath="/*/*" ></asp:XmlDataSource>

                
                    </td>
                    <td  align="center">
                        <asp:CheckBoxList ID="cblArterialProblems_R" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Vertical">
                    
                        </asp:CheckBoxList>
                
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="lblKidneyTransplantable" runat="server"  Font-Size="Small" Text="Kidney Transplantable" Font-Bold="true"  />
                    </td>
                    <td align="center">
                        <asp:RadioButtonList ID="rblKidneyTransplantable" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblKidneyTransplantable_SelectedIndexChanged" >
                        </asp:RadioButtonList>
                    
                    </td>
                    <td  align="center">
                        <asp:RadioButtonList ID="rblKidneyTransplantable_R" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblKidneyTransplantable_R_SelectedIndexChanged">
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <asp:Panel runat="server" ID="pnlReasonNotTransplantable" Visible="false">
                    <tr>
                        <td align="right">
                            <asp:label id="lblReasonNotTransplantable" runat="server"  Font-Size="Small" Text="Reason Not Transplantable *" Font-Bold="true"  />
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtReasonNotTransplantable" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" ></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfv_txtReasonNotTransplantable" ControlToValidate="txtReasonNotTransplantable" Display="Dynamic" ErrorMessage="Please Provide Details" 
                                            CssClass="MandatoryFieldMessage" ValidationGroup="SecondaryLeft" />
                        </td>
                        <td align="center">
                            <asp:TextBox ID="txtReasonNotTransplantable_R" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" ></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ID="rfv_txtReasonNotTransplantable_R" ControlToValidate="txtReasonNotTransplantable_R" Display="Dynamic" ErrorMessage="Please Provide Details" 
                                            CssClass="MandatoryFieldMessage" ValidationGroup="SecondaryRight" />
                        </td>
                    </tr>

                </asp:Panel>
                <tr>
                    <td align="right">
                        <asp:label id="lblWashoutPerfusion" runat="server"  Font-Size="Small" Text="Washout Perfusion"  />
                    </td>
                    <td align="center">
                        <asp:DropDownList ID="ddWashoutPerfusion" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true">
                            <asp:ListItem Selected="True" Text="Select an Option" Value="0" />
                        </asp:DropDownList>
                        <asp:XmlDataSource ID="XMLWashoutPerfusionDataSource" runat="server" 
                                DataFile="~/App_Data/WashoutPerfusion.xml" XPath="/*/*" ></asp:XmlDataSource>
                    </td>
                    <td  align="center">
                        <asp:DropDownList ID="ddWashoutPerfusion_R" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true">
                            <asp:ListItem Selected="True" Text="Select an Option" Value="0" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="lblRemoval" runat="server"  Font-Size="Small" Text="Kidney Removal (Date Time)"  />
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtRemovalDate" runat="server" MaxLength="10" Width="75px" ></asp:TextBox>
                        <asp:CalendarExtender ID="txtRemovalDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtRemovalDate" PopupPosition="Right">
                        </asp:CalendarExtender>
                        <asp:TextBox ID="txtRemovalTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                        <asp:MaskedEditExtender ID="txtRemovalTime_MaskedEditExtender" runat="server" 
                            Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                            ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                            TargetControlID="txtRemovalTime">
                        </asp:MaskedEditExtender>
                        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                    </td>
                    <td  align="center">
                        <asp:TextBox ID="txtRemoval_RDate" runat="server" MaxLength="10" Width="75px" ></asp:TextBox>
                        <asp:CalendarExtender ID="txtRemoval_RDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtRemoval_RDate" PopupPosition="Right">
                        </asp:CalendarExtender>
                        <asp:TextBox ID="txtRemoval_RTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                        <asp:MaskedEditExtender ID="txtRemoval_RTime_MaskedEditExtender" runat="server" 
                            Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                            ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                            TargetControlID="txtRemoval_RTime">
                        </asp:MaskedEditExtender>
                    </td>
                </tr>
        
                <tr>
                    <td align="right">
                        <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="Comments"  />
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtComments" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" ></asp:TextBox>
                    </td>
                    <td align="center">
                        <asp:TextBox ID="txtComments_R" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" ></asp:TextBox>
                    </td>
                </tr>
                <asp:Panel runat="server" ID="pnlAllDataAdded" Visible="false">
                    <tr>
                        <td align="right">
                            <asp:label id="lblAllDataAdded" runat="server"  Font-Size="Small" Text="Tick Box if Data Entry Complete"  />
                        </td>
                        <td align="left"  colspan="2">
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
                        <td align="left"  colspan="2">
                            <asp:TextBox ID="txtReasonModified" runat="server" MaxLength="10000" Width="95%" TextMode="MultiLine" Font-Names="Arial" Font-Size="Small" 
                                onkeyup="AutoExpandTextBox(this, event)" Rows="2"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblReasonModifiedOld" runat="server"  Font-Size="Small" Text="Reason(s) Data Modified Earlier"  />
                        </td>
                        <td align="left"  colspan="2">
                            <asp:label id="lblReasonModifiedOldDetails" runat="server"  Font-Size="Small" Text="" BackColor="#99CCFF" Width="95%" />
                        </td>
                    </tr>
                </asp:Panel> 
                <asp:Panel runat="server" ID="pnlFinal" Visible="false">
                    <tr>
                        <td align="right">
                            <asp:label id="lblDataFinal" runat="server"  Font-Size="Small" Text="Tick to Mark Data as Final"  />
                        </td>
                        <td align="left" colspan="2">
                            <asp:CheckBox runat="server" ID="chkDataFinal" />
                        </td>
                    </tr>
                </asp:Panel>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table  cellpadding="2" cellspacing="2" border="0" style="width: 1050px">
        <tr>
            
            <td style="width:30%" align="right" >
                <asp:HyperLink runat="server" ID="hlkSummaryPage" Text="Summary Page" />
                 <asp:Button ID="cmdDelete" runat="server" Text="Delete" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdDelete_Click"   />
                <asp:ConfirmButtonExtender ID="cmdDelete_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdDelete">
                </asp:ConfirmButtonExtender>
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
            </td>
            <td style="width:70%" align="left" colspan="2">
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px" CausesValidation="true" ValidationGroup="MainGroup"   UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
            </td>
            
        </tr>
    </table>
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
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True"   Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="KidneyInspectionID" SortExpression="KidneyInspectionID" HeaderText="KidneyInspectionID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID" Visible="false" 
                            DataNavigateUrlFields="TrialID, KidneyInspectionID" 
                            DataNavigateUrlFormatString="AddInspectionLR.aspx?TID={0}&KidneyInspectionID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="TrialID" SortExpression="TrialID" HeaderText="TrialID"    />
                        <asp:BoundField DataField="PreservationModality" SortExpression="PreservationModality" HeaderText="Preservation (Left)"    />
                        <asp:BoundField DataField="RandomisationComplete" SortExpression="RandomisationComplete" HeaderText="Randomisation (Left)"    />
                        <asp:BoundField DataField="NumberRenalArteries" SortExpression="NumberRenalArteries" HeaderText="Number Renal Arteries (Left)"    />
                        <asp:BoundField DataField="ArterialProblems" SortExpression="ArterialProblems" HeaderText="Renal Graft Damage (Left)"    />
                        <asp:BoundField DataField="PreservationModality_R" SortExpression="PreservationModality_R" HeaderText="Preservation (Right)"    />
                        <asp:BoundField DataField="RandomisationComplete_R" SortExpression="RandomisationComplete_R" HeaderText="Randomisation (Right)"    />
                        <asp:BoundField DataField="NumberRenalArteries_R" SortExpression="NumberRenalArteries_R" HeaderText="Number Renal Arteries (Right)"    />
                        <asp:BoundField DataField="ArterialProblems_R" SortExpression="ArterialProblems_R" HeaderText="Renal Graft Damage (Right)"    />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

