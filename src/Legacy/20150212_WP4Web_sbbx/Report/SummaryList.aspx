<%@ Page Title="" Language="C#" MasterPageFile="~/TopMasterPage.master" AutoEventWireup="true" CodeFile="SummaryList.aspx.cs" Inherits="Report_SummaryList" %>

<asp:Content ID="conSummaryList" ContentPlaceHolderID="cplMainContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" Font-Names="Verdana; Helvetica; Tahoma"
            Font-Size="Small" Width="100%" ForeColor="Red" />
 
<table width="80%" >
    <tr>
        <th><asp:Label runat="server" ID="lblGV1" Font-Names="Verdana; Helvetica; Tahoma"
                            Width="90%" Text="Heat Map" /></th>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="GV1" runat="server" BackColor="White" BorderColor="#999999" 
                BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                GridLines="Horizontal" Width="90%" AutoGenerateColumns="False" AllowSorting="True" 
                OnSorting="GV1_Sorting" OnRowDataBound="GV1_RowDataBound" AllowPaging="False">
                <AlternatingRowStyle BackColor="#DCDCDC" />
                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                <SortedAscendingHeaderStyle BackColor="#0000A9" />
                <SortedDescendingHeaderStyle BackColor="#000065"        />
                <Columns>
                    
                    <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID" ControlStyle-Width="10%"
                        DataNavigateUrlFields="TrialID" 
                        DataNavigateUrlFormatString="~/SpecClinicalData/ViewSummary.aspx?TID={0}">
                    </asp:HyperLinkField>
                    <asp:BoundField DataField="DonorDetails" SortExpression="Active" HeaderText="Donor" />
                    <asp:BoundField DataField="DonorPreopData" SortExpression="DonorPreopData" HeaderText="Donor Preop Data"  />
                    <asp:BoundField DataField="DonorLabData" SortExpression="DonorLabData" HeaderText="Lab Data"  />
                    <asp:BoundField DataField="KidneyInspection" SortExpression="KidneyInspection" HeaderText="Inspection"  />
                    <asp:BoundField DataField="Randomisation" SortExpression="Randomisation" HeaderText="Randomisation"  />
                    <asp:BoundField DataField="RecipientIdentification" SortExpression="RecipientIdentification" HeaderText="Recipient"  />
                    <asp:BoundField DataField="RecipientPeriOperativeData" SortExpression="RecipientPeriOperativeData" HeaderText="PeriOperative"  />
                    <asp:BoundField DataField="FUDay1to14" SortExpression="FUDay1to14" HeaderText="Day 1-14"  />
                    <asp:BoundField DataField="FUDay1Month" SortExpression="FUDay1Month" HeaderText="1 Month"  />
                    <asp:BoundField DataField="FU3Months" SortExpression="FU3Months" HeaderText="3 Months"  />
                    <asp:BoundField DataField="FU6Months" SortExpression="FU6Months" HeaderText="6 Months"  />
                    <asp:BoundField DataField="FU1Year" SortExpression="FU1Year" HeaderText="1 Year"  />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="sqldsGV1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />

            
        
        </td>
        
    </tr>
</table>
</asp:Content>

