<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="EditDonorSpecimen.aspx.cs" Inherits="SpecClinicalData_EditDonorSpecimen" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conEditDonorSpecimen" ContentPlaceHolderID="SpecimenContents" Runat="Server">
        <asp:Label runat="server" ID="lblUserMessages" Font-Size="Small" ForeColor="Red" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="2" style="width: 800px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td align="right" style="width:25%">
                <asp:label id="lblBarcode" runat="server"  Font-Size="Small" Text="Barcode"  />
            </td>
            <td align="left" style="width:75%">
                <asp:TextBox ID="txtBarcode" runat="server" MaxLength="10" Width="25%" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td  align="right" >
                <asp:label id="lblOccasion" runat="server"  Font-Size="Small" Text="Occasion"  />
            </td>
            <td   align="left">
                <asp:DropDownList ID="ddOccasion"  runat="server" AppendDataBoundItems="True" 
                    DataTextField="Text" DataValueField="Value" 
                            Width="25%" Font-Bold="False" >
					<asp:ListItem Value="0">Select Occasion</asp:ListItem>
										            
				</asp:DropDownList>
                <asp:XmlDataSource ID="XMLOccasionsSpecimenDonorDataSource" runat="server" 
                        DataFile="~/App_Data/OccasionsSpecimenDonor.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td  align="right" >
                <asp:label id="lblDateTimeCollected" runat="server"  Font-Size="Small" Text="Date/Time Collected"  />
            </td>
            <td align="left" >
                <asp:TextBox ID="txtDateCollected" runat="server" MaxLength="10" Width="80px"></asp:TextBox>
                <asp:CalendarExtender ID="txtDateCollected_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="txtDateCollected" Format="dd/MM/yyyy">
                </asp:CalendarExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </asp:ToolkitScriptManager>
            
                <asp:TextBox ID="txtTimeCollected" runat="server" MaxLength="100" Width="40px"></asp:TextBox>
                <asp:MaskedEditExtender ID="txtTimeCollected_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtTimeCollected">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        
        <tr>
            <td  align="right" >
                <asp:label id="lblSpecimenType" runat="server"  Font-Size="Small" Text="Specimen Type"  />
            </td>
            <td   align="left">
                <asp:DropDownList ID="ddSpecimenType"  runat="server" AppendDataBoundItems="True" 
                    DataTextField="Text" DataValueField="Value" 
                            Width="25%" Font-Bold="False" >
					<asp:ListItem Value="0">Select Specimen Type</asp:ListItem>
										            
				</asp:DropDownList>
                <asp:XmlDataSource ID="XMLSpecimenTypesDataSource" runat="server" 
                        DataFile="~/App_Data/SpecimenTypes.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td  align="right" >
                <asp:label id="lblTissueSource" runat="server"  Font-Size="Small" Text="Tissue Source"  />
            </td>
            <td   align="left">
                <asp:DropDownList ID="ddTissueSource"  runat="server" AppendDataBoundItems="True" 
                    DataTextField="Text" DataValueField="Value" 
                            Width="25%" Font-Bold="False" >
					<asp:ListItem Value="0">Select Tissue Source</asp:ListItem>
										            
				</asp:DropDownList>
                <asp:XmlDataSource ID="XMLTissueSourcesDataSource" runat="server" 
                        DataFile="~/App_Data/TissueSources.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td  align="right" >
                <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="Comments"  />
            </td>
            <td align="left" >
                <asp:TextBox ID="txtComments" runat="server" MaxLength="100" Width="95%" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        
        <tr>
            <td align="right" >
                <asp:Button ID="cmdDelete" runat="server" Text="Delete" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdDelete_Click"   />
                <asp:ConfirmButtonExtender ID="cmdDelete_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdDelete">
                </asp:ConfirmButtonExtender>
            </td>
            <td align="left" >
                <asp:Button ID="cmdUpdate" runat="server" Text="Update" Height="36px"   UseSubmitBehavior="False" OnClick="cmdUpdate_Click"    />
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
                        <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID"
                            DataNavigateUrlFields="TrialID, SpecimenID" 
                            DataNavigateUrlFormatString="EditDonorSpecimen.aspx?TID={0}&SpecimenID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="Barcode" SortExpression="Barcode" HeaderText="Barcode"    />
                        <asp:BoundField DataField="Occasion" SortExpression="Occasion" HeaderText="Occasion"    />
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

