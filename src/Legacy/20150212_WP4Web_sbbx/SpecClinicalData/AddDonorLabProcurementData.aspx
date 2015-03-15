<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddDonorLabProcurementData.aspx.cs" Inherits="SpecClinicalData_AddDonorLabProcurementData" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddDonorLabProcurementData" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" Font-Size="Small" ForeColor="Red" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="2" style="width: 851px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:30%" align="right">
                <asp:label id="lblCreatinineAtAdmission" runat="server"  Font-Size="Small" Text="Creatinine At Admission (umol/l)"  />
            </td>
            <td style="width:70%"  align="left">
                <asp:TextBox ID="txtCreatinineAtAdmission" runat="server" MaxLength="45" Width="25%" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblCreatinineLast" runat="server"  Font-Size="Small" Text="Creatinine Last (umol/l)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtCreatinineLast" runat="server" MaxLength="45" Width="25%" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblCreatinineMax" runat="server"  Font-Size="Small" Text="Creatinine Maximum (umol/l)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtCreatinineMax" runat="server" MaxLength="45" Width="25%" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblUrineGlucose" runat="server"  Font-Size="Small" Text="Urine Glucose"  />
            </td>
            <td align="left">
                <asp:DropDownList ID="ddUrineGlucose" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="25%">
                    <asp:ListItem Selected="True" Text="Select Urine Glucose" Value="0"  />   
                </asp:DropDownList>
                <asp:XmlDataSource ID="XMLUrineOptionsDataSource" runat="server" 
                        DataFile="~/App_Data/UrineOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblUrineProtein" runat="server"  Font-Size="Small" Text="Urine Protein"  />
            </td>
            <td align="left">
                <asp:DropDownList ID="ddUrineProtein" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="25%">
                    <asp:ListItem Selected="True" Text="Select Urine Protein" Value="0"  />   
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="Other Lab Results"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtComments" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblDateOperation" runat="server"  Font-Size="Small" Text="Date Operation"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtDateOperation" runat="server" MaxLength="10" Width="25%" ></asp:TextBox>
                <asp:CalendarExtender ID="txtDateOperation_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateOperation">
                </asp:CalendarExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblCrossClampingTime" runat="server"  Font-Size="Small" Text="Cross Clamping Time"  />
            </td>
            <td  align="left">
                                                 
                <asp:TextBox ID="txtCrossClampingTime" runat="server" MaxLength="5" Width="40px" ></asp:TextBox>
                <asp:MaskedEditExtender ID="txtCrossClampingTime_MaskedEditExtender" runat="server" 
                    Mask="99:99" MessageValidatorTip="true" MaskType="Time" InputDirection="LeftToRight"
                    ErrorTooltipEnabled="True" AutoComplete="True" AutoCompleteValue="00:00" Enabled="True" 
                    TargetControlID="txtCrossClampingTime">
                </asp:MaskedEditExtender>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblInVivoFlushSolution" runat="server"  Font-Size="Small" Text="In Vivo Flush Solution"  />
            </td>
            <td align="left">
                <asp:DropDownList ID="ddInVivoFlushSolution" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="35%">
                    <asp:ListItem Selected="True" Text="Select In Vivo Flush Solution" Value="0"  />   
                </asp:DropDownList>
                <asp:XmlDataSource ID="XmlFlushSolutionsDataSource" runat="server" 
                        DataFile="~/App_Data/FlushSolutions.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblInVivoFlushSolutionOther" runat="server"  Font-Size="Small" Text="In Vivo Flush Solution (If Other)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtInVivoFlushSolutionOther" runat="server" MaxLength="100" Width="95%"  ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblWashoutVolume" runat="server"  Font-Size="Small" Text="Washout Volume (ml)"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtWashoutVolume" runat="server" MaxLength="45" Width="25%"  ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblCommentsProcurement" runat="server"  Font-Size="Small" Text="Procurement Comments"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtCommentsProcurement" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
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
                <asp:Button ID="cmdAddData" runat="server" Text="Add Lab Results/ Procurement Data" Height="36px"   UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
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
                        <asp:BoundField DataField="DonorLabResultsID" SortExpression="DonorLabResultsID" HeaderText="DonorLabResultsID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID"
                            DataNavigateUrlFields="TrialID, DonorLabResultsID" 
                            DataNavigateUrlFormatString="AddDonorLabProcurementData.aspx?TID={0}&DonorLabResultsID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="CreatinineAtAdmission" SortExpression="CreatinineAtAdmission" HeaderText="Creatinine Admission"    />
                        <asp:BoundField DataField="CreatinineLast" SortExpression="CreatinineLast" HeaderText="Creatinine Last"    />
                        <asp:BoundField DataField="UrineGlucose" SortExpression="UrineGlucose" HeaderText="Urine Glucose"    />
                        <asp:BoundField DataField="UrineProtein" SortExpression="UrineProtein" HeaderText="Urine Protein"    />
                        <asp:BoundField DataField="Date_Operation" SortExpression="DateOperation" HeaderText="Date Operation"    />
                        <asp:BoundField DataField="CrossClamping_Time" SortExpression="CrossClampingTime" HeaderText="Cross Clamping Time"    />
                        <asp:BoundField DataField="InVivoFlushSolution" SortExpression="InVivoFlushSolution" HeaderText="In Vivo Flush Solution"    />
                        <asp:BoundField DataField="WashoutVolume" SortExpression="WashoutVolume" HeaderText="Washout Volume" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>

</asp:Content>

