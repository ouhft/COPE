<%@ Page Title="" Language="C#" MasterPageFile="~/TopMasterPage.master" AutoEventWireup="true" CodeFile="AUList.aspx.cs" Inherits="OtherArea_AUList" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAUList" ContentPlaceHolderID="cplMainContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" Font-Size="Small" ForeColor="Red" Width="100%" />
    <table style="width:1000px">
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
                        <asp:BoundField DataField="CentreCode" SortExpression="CentreCode" HeaderText="Centre Code"    />
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
        <tr align="center" >
            <th  colspan="2">
                <asp:Label runat="server" ID="lblGV2" Text="" />
            </th>
           
        </tr>
        <tr align="center">
            <td colspan="2">
                <asp:GridView ID="GV2" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV2_Sorting" AllowSorting="True" Width="95%"> 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="ListusersID" SortExpression="ListusersID" HeaderText="ListusersID" Visible="false"    />
                        <asp:BoundField DataField="UserName" SortExpression="UserName" HeaderText="User Name"    />
                        <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"    />
                        <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name"    />
                        <asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"    />                       
                        <asp:BoundField DataField="LastLogin" SortExpression="LastLogin" HeaderText="Last Login" ItemStyle-Width="20%"></asp:BoundField>
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4usrconn %>"   />
                
            </td>
        </tr>
    </table>
    
</asp:Content>

