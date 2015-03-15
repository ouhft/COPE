<%@ Page Title="" Language="C#" MasterPageFile="~/TopMasterPage.master" AutoEventWireup="true" CodeFile="PENUser.aspx.cs" Inherits="OtherArea_PENUser" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conPENUser" ContentPlaceHolderID="cplMainContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
        
    
    <table rules="all" cellpadding="0" cellspacing="0" border="1"   style="width: 1268px; font-size: small;"  >
        <tr >
            <th  colspan="2">
                <asp:Label runat="server" ID="lblPageDescription" Text="Add a New user" />
            </th>
           
        </tr>
        
        <tr align="right">
            <td style="width: 40%" >
                <asp:Label runat="server" ID="lblUserName" Text="Database UserID "  />
            </td>
            <td  style="width: 60%"  align="left">
                <asp:TextBox runat="server" ID="txtUserName" Width="50%" MaxLength="45" Enabled="false" />
                
            </td>
        </tr>
        <tr align="right">
            <td  >
                <asp:Label runat="server" ID="lblFirstName" Text="First Name"  />
            </td>
            <td align="left" >
                <asp:TextBox runat="server" ID="txtFirstName" Width="50%" MaxLength="100"  />
            </td>
        </tr>
        <tr align="right">
            <td  >
                <asp:Label runat="server" ID="lblLastName" Text="Last Name"  />
            </td>
            <td  align="left" >
                <asp:TextBox runat="server" ID="txtLastName" Width="50%" MaxLength="100"  />
            </td>
        </tr>
        <tr align="right">
            <td  >
                <asp:Label runat="server" ID="lblEmail" Text="Email "  />
            </td>
            <td  align="left" >
                <asp:TextBox runat="server" ID="txtEmail" Width="50%" MaxLength="100"  />
            </td>
        </tr>
        <tr align="right">
            <td  >
                <asp:Label runat="server" ID="lblJobTitle" Text="Job Title "  />
            </td>
            <td   align="left">
                <asp:TextBox runat="server" ID="txtJobTitle" Width="50%" MaxLength="100" autocomplete="off" />
            </td>
        </tr>
        
        
        
        
        <tr align="right">
            <td >
                <asp:label id="lblComments" runat="server"  
                     Text="User Comments"  />
            </td>
            <td   align="left">
                <asp:TextBox runat="server" ID="txtComments" Width="95%" 
                    MaxLength="300" Height="50px" TextMode="MultiLine" />
            </td>
        <tr align="right">
            <td style="height: 38px"><asp:Button ID="cmdReset" runat="server" Text="Reset" 
                    Height="36px" Width="111px" CausesValidation="False" 
                    onclick="cmdReset_Click"  /></td>
            <td  style="height: 38px"  align="left">
                <asp:Button ID="cmdAddRecord" runat="server" Text="Update" Height="36px" 
                    Width="132px" onclick="cmdAddRecord_Click"  />
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
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True" OnRowDataBound="GV1_RowDataBound" Width="95%"> 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="ListusersID" SortExpression="ListusersID" HeaderText="ListusersID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="UserName" SortExpression="UserName" HeaderText="UserID"
                            DataNavigateUrlFields="ListusersID" 
                            DataNavigateUrlFormatString="ENUser.aspx?OtherID={0}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"    />
                        <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name"    />
                        <asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"    />                       
                        <asp:BoundField DataField="LastLogin" SortExpression="LastLogin" HeaderText="Last Login" ItemStyle-Width="20%"></asp:BoundField>
                        <asp:HyperLinkField Text="Edit Main Details" SortExpression="" HeaderText=""
                            DataNavigateUrlFields="ListusersID" 
                            DataNavigateUrlFormatString="PENUser.aspx?OtherID={0}">
                        </asp:HyperLinkField>
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

