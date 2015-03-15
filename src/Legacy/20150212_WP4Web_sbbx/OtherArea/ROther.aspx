﻿<%@ Page Title="" Language="C#" MasterPageFile="~/TopMasterPage.master" AutoEventWireup="true" CodeFile="ROther.aspx.cs" Inherits="OtherArea_ROther" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conROther" ContentPlaceHolderID="cplMainContents" Runat="Server">
        <asp:Label runat="server" ID="lblUserMessages" Font-Size="Small" ForeColor="Red" Width="100%" />
    <table cellpadding="1" cellspacing="1" border="0" style="width: 800px; font-size: small;"  >
        <tr align="center" >
            <th  colspan="2">
                <asp:Label runat="server" ID="lblPageDescription" Text="Enter details for new user." />
            </th>
           
        </tr>
        <tr align="right">
            <td style="width: 40%" >
                <asp:Label runat="server" ID="lblUserName" Text="Database UserID "  />
            </td>
            <td style="width: 60%" align="left">
                <asp:TextBox runat="server" ID="txtUserName" Width="50%" MaxLength="45" 
                    Enabled="False"  />
            </td>
        </tr>
       
        <tr align="right">
            <td >
                <asp:label id="lblUserPassNew" runat="server"  
                        Text="New Password"  />
            </td>
            <td  align="left">
                <asp:TextBox runat="server" ID="txtUserPassNew" Width="50%" MaxLength="45" 
                    TextMode="Password" />
            </td>
        </tr>
        <tr align="right">
            <td >
                <asp:label id="lblReEnterUserPass" runat="server"  
                        Text="Re-Enter New Password"  />
            </td>
            <td align="left">
                <asp:TextBox runat="server" ID="txtReEnterUserPassNew" Width="50%" 
                    MaxLength="45" TextMode="Password" />
            </td>
        </tr>
        <tr align="right">
            <td style="height: 38px">
                <asp:Button ID="cmdReset" runat="server" Text="Reset Page" Height="36px" 
                    CausesValidation="False" onclick="cmdReset_Click"  />
                
            </td>
            <td align="left" style="height: 38px">
                <asp:Button ID="cmdUpdate" runat="server" Text="Reset Password" Height="36px" 
                    Width="132px" onclick="cmdUpdate_Click"  />
            </td>
        </tr>
        <tr align="center" >
            <th  colspan="2">
                <asp:Label runat="server" ID="lblGV1" Text="" />
            </th>
           
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:GridView ID="GV1" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True"   OnRowDataBound="GV1_RowDataBound" Width="95%"> 
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
                            DataNavigateUrlFormatString="ROther.aspx?OtherID={0}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"    />
                        <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name"    />
                        <asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"    />                       
                        <asp:BoundField DataField="LastLogin" SortExpression="LastLogin" HeaderText="Last Login" ItemStyle-Width="20%"></asp:BoundField>
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4usrconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

