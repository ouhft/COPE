<%@ Page Title="" Language="C#" MasterPageFile="~/TopMasterPage.master" AutoEventWireup="true" CodeFile="ULList.aspx.cs" Inherits="OtherArea_ULList" %>

<asp:Content ID="conULList" ContentPlaceHolderID="cplMainContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" 
                    Font-Size="Small" ForeColor="Red" Width="100%" />
    <table style="width:300px">
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
                        <asp:BoundField DataField="UserLogsID" SortExpression="UserLogsID" HeaderText="UserLogsID" Visible="false"    />
                        <asp:BoundField DataField="Logged_In" SortExpression="LoggedIn" HeaderText="Login"    />
                        <asp:BoundField DataField="IPAddress" SortExpression="LastName" HeaderText="IP Address"    />
                        
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="sqldsGV1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:copewp2dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

