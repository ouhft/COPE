<%@ Page Title="" Language="C#" MasterPageFile="~/StudyIDMasterPage.master" AutoEventWireup="true" CodeFile="WP4TList.aspx.cs" Inherits="AddEditData_WP4TList" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conWP4TList" ContentPlaceHolderID="AddEditStudyContent" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="1" cellspacing="1" border="1" style="width: 1200px">
        <tr>
            <th colspan="2">
                <asp:Label runat="server" ID="lblDescription" Width="100%" Text="" />
            </th>
        </tr>
        <tr align="right">
            <td style="width:20%">
                <asp:label id="lblCentres" runat="server"  Font-Size="Small" Text="Select Countries" />
            </td>
            <td align="left" >
                <asp:CheckBoxList ID="cblCentreList" runat="server" DataTextField="CountryDetails" DataValueField="CountryCode" RepeatDirection="Horizontal" RepeatLayout="Flow" 
                    AutoPostBack="true" OnSelectedIndexChanged="cblCentreList_SelectedIndexChanged">
                    
                </asp:CheckBoxList>
                <asp:SqlDataSource ID="sqldsCentreLists" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"
                              >
                </asp:SqlDataSource>
                <asp:Button runat="server" ID="cmdToggle" Text="" OnClick="cmdToggle_Click" />
            </td>
        </tr>
    </table>
    <table cellpadding="1" cellspacing="1" border="0" style="width: 1020px">
        
        <tr>
            <th colspan="2">
                <asp:Label runat="server" ID="lblGV1" Width="100%" Text="" />
            </th>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="GV1" runat="server" BackColor="White" BorderColor="#999999" 
                    BorderStyle="Solid" BorderWidth="1px" CellPadding="3" 
                    GridLines="Vertical"  Width="100%" 
                    AutoGenerateColumns="False" OnSorting="GV1_Sorting" AllowSorting="True">
                    <AlternatingRowStyle BackColor="#DCDCDC" />
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White"  />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#0000A9" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#000065"
                     />
                     <Columns>
                        <asp:BoundField DataField="TrialDetailsID" SortExpression="TrialDetailsID" HeaderText="TrialDetailsID" Visible="false"   />
                        <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID"
                            DataNavigateUrlFields="TrialID" 
                            DataNavigateUrlFormatString="~/SpecClinicalData/ViewSummary.aspx?TID={0}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="t3CentreNameMerged" SortExpression="t3CentreNameMerged" HeaderText="Retrieval Team"   />
                        <asp:BoundField DataField="DonorID" SortExpression="DonorID" HeaderText="Donor ET/ NHSBT Number"   />
                        <asp:BoundField DataField="AgeOrDateOfBirth" SortExpression="AgeOrDateOfBirth" HeaderText="Age/ Date Of Birth"    />
                        <asp:BoundField DataField="Date_OfBirthDonor" SortExpression="DateOfBirthDonor" HeaderText="Date Of Birth Donor"    />
                        <asp:BoundField DataField="DonorAge" SortExpression="DonorAge" HeaderText="Age Donor"    />
                        <asp:BoundField DataField="Randomised" SortExpression="Randomised" HeaderText="Randomised"    />
                    </Columns>
                    
                </asp:GridView>
                <asp:SqlDataSource ID="sqldsGV1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"  />
            </td>
        
        </tr>
    </table>
</asp:Content>

