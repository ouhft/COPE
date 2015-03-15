<%@ Page Title="" Language="C#" MasterPageFile="~/StudyIDMasterPage.master" AutoEventWireup="true" CodeFile="WP4ListRecipient.aspx.cs" Inherits="AddEditData_WP4ListRecipient" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conWP4ListRecipient" ContentPlaceHolderID="AddEditStudyContent" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="1" cellspacing="1" border="1" style="width: 1200px">
        <tr>
            <th colspan="4">
                <asp:Label runat="server" ID="lblDescription" Width="100%" Text="" />
            </th>
        </tr>
        <tr align="right">
            <td style="width:15%">
                <asp:label id="lblCentres" runat="server"  Font-Size="Small" Text="Select Countries" />
            </td>
            <td align="left" style="width:40%">
                <asp:CheckBoxList ID="cblCentreList" runat="server" DataTextField="CountryDetails" DataValueField="CountryCode" RepeatDirection="Horizontal" RepeatLayout="Flow" 
                    AutoPostBack="false" OnSelectedIndexChanged="cblCentreList_SelectedIndexChanged">
                    
                </asp:CheckBoxList>
                <asp:SqlDataSource ID="sqldsCentreLists" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"
                              >
                </asp:SqlDataSource>
                <asp:Button runat="server" ID="cmdToggle" Text="" OnClick="cmdToggle_Click" />
            </td>
            <td style="width:10%" >
                <asp:label id="lblActive" runat="server"  Font-Size="Small" Text="Active" />
            </td>
            <td align="left" style="width:30%" >
                <asp:CheckBoxList ID="cblActive" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    
                </asp:CheckBoxList>
                <asp:XmlDataSource ID="XMLActiveDataSource" runat="server" 
                        DataFile="~/App_Data/ActiveAll.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td colspan="4" align="right">
                <asp:Button runat="server" ID="cmdReset" Height="28px" Text="Reset" Font-Bold="true"  OnClick="cmdReset_Click"  />
                <asp:Button runat="server" ID="cmdDisplay" Height="28px" Text="Display" Font-Bold="true"  OnClick="cmdDisplay_Click"  />
            </td>
        </tr>
    </table>
    <table cellpadding="1" cellspacing="1" border="0" style="width: 1200px">
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
                        <asp:BoundField DataField="RIdentificationID" SortExpression="RIdentificationID" HeaderText="RIdentificationID" Visible="false"   />
                        <asp:HyperLinkField DataTextField="TrialIDRecipient" SortExpression="TrialIDRecipient" HeaderText="Trial ID (Recipient)"
                            DataNavigateUrlFields="TrialIDRecipient, TrialID" 
                            DataNavigateUrlFormatString="~/SpecClinicalData/ViewSummaryRecipient.aspx?TID_R={0}&TID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="t4CentreNameMerged" SortExpression="t4CentreNameMerged" HeaderText="Transplant Centre" ItemStyle-Width="15%"   />
                        <asp:BoundField DataField="DonorIdentifier" SortExpression="DonorIdentifier" HeaderText="Donor ET/ NHSBT Number"   />
                        <asp:BoundField DataField="KidneyReceived" SortExpression="KidneyReceived" HeaderText="Kidney Received"    />
                        <asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"   />
                        <asp:BoundField DataField="KidneyDiscarded" SortExpression="KidneyDiscarded" HeaderText="Kidney Discarded"   />
                        <asp:BoundField DataField="Death_Date" SortExpression="DeathDate" HeaderText="Deceased"    />
                        <asp:BoundField DataField="ReasonWithdrawn" SortExpression="ReasonWithdrawn" HeaderText="Reason Withdrawn"    />
                        <asp:BoundField DataField="ConsentAdditionalSamples" SortExpression="ConsentAdditionalSamples" HeaderText="Consent Additional Samples"    />
                        <asp:HyperLinkField Text="Edit Main Details" SortExpression="" HeaderText=""
                            DataNavigateUrlFields="TrialIDRecipient, TrialID, TrialDetails_RecipientID" 
                            DataNavigateUrlFormatString="~/AddEditData/EditWP4Recipient.aspx?TID_R={0}&TID={1}&TDRID={2}">
                        </asp:HyperLinkField>
                    </Columns>
                    
                </asp:GridView>
                <asp:SqlDataSource ID="sqldsGV1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"  />
            </td>
        
        </tr>
    </table>
</asp:Content>

