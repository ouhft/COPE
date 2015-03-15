<%@ Page Title="" Language="C#" MasterPageFile="~/TopMasterPage.master" AutoEventWireup="true" CodeFile="RUser.aspx.cs" Inherits="OtherArea_RUser" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conRUser" ContentPlaceHolderID="cplMainContents" Runat="Server">
    
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
                <asp:label id="lblUserPass" runat="server"  
                        Text="Current Password"  />
            </td>
            <td  align="left">
                <asp:TextBox runat="server" ID="txtUserPass" Width="50%" MaxLength="45" 
                    TextMode="Password" />
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
    </table>


</asp:Content>

