<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddInspection.aspx.cs" Inherits="SpecClinicalData_AddInspection" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddInspection" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" Font-Size="Small" ForeColor="Red" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="2" style="width: 851px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:35%" align="right">
                <asp:label id="lblSide" runat="server"  Font-Size="Small" Text="Kidney Side"  />
            </td>
            <td style="width:65%"  align="left">
                <asp:DropDownList ID="ddSide" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="35%" 
                    AutoPostBack="True" OnSelectedIndexChanged="ddSide_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Text="Select Kidney Side" Value="0"  />   
                </asp:DropDownList>
                <asp:XmlDataSource ID="XMLKidneySidesDataSource" runat="server" 
                        DataFile="~/App_Data/KidneySides.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblPreservationModality" runat="server"  Font-Size="Small" Text="Preservation Modality"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblPreservationModality" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" Enabled="false" >
                       
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLPreservationModalitiesDataSource" runat="server" 
                        DataFile="~/App_Data/PreservationModalities.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblRandomisationComplete" runat="server"  Font-Size="Small" Text="Randomisation Complete"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblRandomisationComplete" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLMainOptionsDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblNumberRenalArteries" runat="server"  Font-Size="Small" Text="Number of Renal Arteries"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtNumberRenalArteries" runat="server" MaxLength="100" Width="25%" ></asp:TextBox>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblArterialProblems" runat="server"  Font-Size="Small" Text="Renal Graft Function"  />
            </td>
            <td align="left">
                <asp:CheckBoxList ID="cblArterialProblems" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Vertical">
                    
                </asp:CheckBoxList>
                <asp:XmlDataSource ID="XMLArterialProblemsDataSource" runat="server" 
                        DataFile="~/App_Data/ArterialProblems.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblWashoutPerfusion" runat="server"  Font-Size="Small" Text="Washout Perfusion"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblWashoutPerfusion" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLWashoutPerfusionDataSource" runat="server" 
                        DataFile="~/App_Data/WashoutPerfusion.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblRemoval" runat="server"  Font-Size="Small" Text="Kidney Removal (Date Time)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtRemovalDate" runat="server" MaxLength="10" Width="75px" ></asp:TextBox>
                <asp:CalendarExtender ID="txtRemovalDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtRemovalDate">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtRemovalTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtRemovalTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtRemovalTime">
                </asp:MaskedEditExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblVisiblePerfusionDefects" runat="server"  Font-Size="Small" Text="Visible Perfusion Defects?"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblVisiblePerfusionDefects" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                       
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblMachinePerfusionStart" runat="server"  Font-Size="Small" Text="Machine Perfusion Start (Date Time)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtMachinePerfusionStartDate" runat="server" MaxLength="10" Width="75px" ></asp:TextBox>
                <asp:CalendarExtender ID="txtMachinePerfusionStartDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtMachinePerfusionStartDate">
                </asp:CalendarExtender>
                <asp:TextBox ID="txtMachinePerfusionStartTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtMachinePerfusionStartTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtMachinePerfusionStartTime">
                </asp:MaskedEditExtender>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblRecipientTransplantationCentre" runat="server"  Font-Size="Small" Text="Recipient Centre"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtRecipientTransplantationCentre" runat="server" MaxLength="100" Width="95%" ></asp:TextBox>
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
                <asp:Button ID="cmdAddData" runat="server" Text="Add Inspection Data" Height="36px"   UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
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
                        <asp:BoundField DataField="KidneyInspectionID" SortExpression="KidneyInspectionID" HeaderText="KidneyInspectionID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID"
                            DataNavigateUrlFields="TrialID, KidneyInspectionID" 
                            DataNavigateUrlFormatString="EditInspection.aspx?TID={0}&KidneyInspectionID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="Side" SortExpression="Side" HeaderText="Side"    />
                        <asp:BoundField DataField="PreservationModality" SortExpression="PreservationModality" HeaderText="Preservation"    />
                        <asp:BoundField DataField="RandomisationComplete" SortExpression="RandomisationComplete" HeaderText="Randomisatione"    />
                        <asp:BoundField DataField="NumberRenalArteries" SortExpression="NumberRenalArteries" HeaderText="Number Renal Arteries"    />
                        <asp:BoundField DataField="ArterialProblems" SortExpression="ArterialProblems" HeaderText="Renal Graft Function"    />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

