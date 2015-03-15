<%@ Page Title="" Language="C#" MasterPageFile="~/TopMasterPage.master" AutoEventWireup="true" CodeFile="WP4HomePage.aspx.cs" Inherits="_WP4HomePage" %>

<asp:Content ID="conW4HomePage" ContentPlaceHolderID="cplMainContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" Font-Size="Small" Width="100%" ForeColor="Red" />
    <h2>WP4 COMPARE Home Page</h2>
    <table  cellspacing="2" cellpadding="2"   style="border: medium solid #000000; width:300px">
        <tr align="center">
            <th colspan="2" align="center">
                <asp:Label ID="lblMainMessages" runat="server" Text=""></asp:Label>
            </th>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="cmdAddKidney" runat="server" Text="Add New Donor/TrialID" BackColor="#00CC99" Font-Size="Medium" Font-Bold="True" ForeColor="White" Height="45px" OnClick="cmdAddKidney_Click" />
            </td>

        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="cmdEditExisting" runat="server" Text="Edit Data Existing Kidney/TrialID"  BackColor="#0099CC" Font-Size="Medium" Font-Bold="True" ForeColor="White" 
                    Height="45px"  OnClick="cmdEditExisting_Click" />
            </td>

        </tr>
        <tr>
            <td style="width:60%" align="right">
                <asp:TextBox ID="txtTrialID" runat="server" Width="75%" Font-Size="Medium" Height="45px" MaxLength="8" Font-Bold="True"  />
                
            </td>
            <td style="width:40%" align="left">
                <asp:Button ID="cmdGoTo" runat="server" Text="Go To"  Height="45px" Font-Size="Medium" Font-Bold="True" ForeColor="White" BackColor="#336600" OnClick="cmdGoTo_Click" />
            </td>
        </tr>
    </table>
    <br />
    <table  cellspacing="2" cellpadding="2"   style="border: medium solid #000000; width:300px">
        <tr align="center">
            <th colspan="2" align="center">
                <asp:Label ID="lblMainMessagesRecipient" runat="server" Text=""></asp:Label>
            </th>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="cmdAddRecipient" runat="server" Text="Add Recipient" BackColor="#00CC99" Font-Size="Medium" Font-Bold="True" ForeColor="White" Height="45px" OnClick="cmdAddRecipient_Click"  />
            </td>

        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="cmdEditingExisting" runat="server" Text="Edit Data Existing Recipient"  BackColor="#0099CC" Font-Size="Medium" Font-Bold="True" ForeColor="White" 
                    Height="45px" OnClick="cmdEditingExisting_Click"   />
            </td>

        </tr>
        <tr>
            <td style="width:60%" align="right">
                <asp:TextBox ID="txtRecipient" runat="server" Width="75%" Font-Size="Medium" Height="45px" MaxLength="9" Font-Bold="True"  />
                
            </td>
            <td style="width:40%" align="left">
                <asp:Button ID="cmdGoToRecipient" runat="server" Text="Go To"  Height="45px" Font-Size="Medium" Font-Bold="True" ForeColor="White" BackColor="#336600" OnClick="cmdGoToRecipient_Click"  />
            </td>
        </tr>
    </table>
     <br />
    <table style="width:1200px">
        <tr align="center" >
            <th  colspan="2">
                <asp:Label runat="server" ID="lblGV1" Text="" />
            </th>
           
        </tr>
        <tr align="center">
            <td colspan="2">
                <asp:GridView ID="GV1" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True" Width="95%"> 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="ListusersID" SortExpression="ListusersID" HeaderText="ListusersID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="UserName" SortExpression="UserName" HeaderText="UserID" Visible="false"
                            DataNavigateUrlFields="ListusersID, ID" 
                            DataNavigateUrlFormatString="AAccess.aspx?OtherID={0}&ModifyID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="User Name"    />
                        <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"    />
                        <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name"    />
                        <asp:BoundField DataField="JobTitle" SortExpression="JobTitle" HeaderText="Job Title"    />
                        <asp:BoundField DataField="CentreCode" SortExpression="CentreCode" HeaderText="Centre Code" Visible="false"    />
                        <asp:BoundField DataField="CentreDetails" SortExpression="CentreDetails" HeaderText="Centre Details" ItemStyle-Width="15%"    />
                        <asp:BoundField DataField="AdminSuperUser" SortExpression="AdminSuperUser" HeaderText="Admin Super User"    />
                        <asp:BoundField DataField="SuperUser" SortExpression="SuperUser" HeaderText="Super User"    />
                        <asp:BoundField DataField="AddEdit" SortExpression="AddEdit" HeaderText="Add/Edit/View Donor Data"    />
                        <asp:BoundField DataField="AddEditRecipient" SortExpression="AddEditRecipient" HeaderText="Add/Edit/View Recipient Data"    />
                        <asp:BoundField DataField="AddEditFollowUp" SortExpression="AddEditFollowUp" HeaderText="Add/Edit/View FollowUp Data"    />
                        <asp:BoundField DataField="Randomise" SortExpression="Randomise" HeaderText="Randomise"    />
                        <asp:BoundField DataField="ViewRandomise" SortExpression="ViewRandomise" HeaderText="View Randomised Data"    />
                        <asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"    />                       
                        <asp:BoundField DataField="LastLogin" SortExpression="LastLogin" HeaderText="Last Login" ItemStyle-Width="20%"></asp:BoundField>
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4usrconn %>"   />
                
            </td>
        </tr>
        
    </table>
</asp:Content>

