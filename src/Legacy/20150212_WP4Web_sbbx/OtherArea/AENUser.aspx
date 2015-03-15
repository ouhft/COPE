<%@ Page Title="" Language="C#" MasterPageFile="~/TopMasterPage.master" AutoEventWireup="true" CodeFile="AENUser.aspx.cs" Inherits="OtherArea_AENUser" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAENUser" ContentPlaceHolderID="cplMainContents" Runat="Server">
       
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
                <asp:TextBox runat="server" ID="txtUserName" Width="50%" MaxLength="45"  />
                
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
                <asp:label id="lblUserPass" runat="server"  
                     Text="Password"  />
            </td>
            <td   align="left">
                <asp:TextBox runat="server" ID="txtUserPass" Width="50%" MaxLength="45"  TextMode="Password" autocomplete="off"/>
                <asp:PasswordStrength ID="txtUserPass_PasswordStrength" runat="server" 
                    Enabled="True" TargetControlID="txtUserPass" 
                    MinimumLowerCaseCharacters="1" MinimumNumericCharacters="1" MinimumUpperCaseCharacters="1" 
                    RequiresUpperAndLowerCaseCharacters="True">
                </asp:PasswordStrength>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            </td>
        </tr>
        <tr align="right">
            <td >
                <asp:label id="lblReEnterUserPass" runat="server"  
                     Text="Re-Enter Password"  />
            </td>
            <td   align="left">
                <asp:TextBox runat="server" ID="txtReEnterUserPass" Width="50%" 
                    MaxLength="45" TextMode="Password" autocomplete="off" />
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
        </tr>
            <tr align="center" >
            <th  colspan="2">
                <asp:Label runat="server" ID="lblGV3" Text="" />
            </th>
           
        </tr>
        <tr>
             <td colspan="2" align="center">
                <asp:GridView ID="GV3" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" GridLines="Both" OnSorting="GV3_Sorting" AllowSorting="True" Width="98%"> 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="Centre" SortExpression="Centre" HeaderText="Centre"    />
                        <asp:BoundField DataField="CentreName" SortExpression="CentreName" HeaderText="Centre Name"    />
                        <asp:TemplateField HeaderText="Centre" Visible="false" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblCentre"
                                    Text='<%#Bind("Centre")%>'>
                                </asp:Label>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Super User"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkSuperUser" />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Randomise"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkRandomise" />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="View Randomise"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkViewRandomise" />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Donor"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkAddEdit" />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Recipient Data"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkAddEditRecipient" />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Follow Up Data"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkAddEditFollowUp" />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email TrialID Created"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkTrialIDCentre" />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email SAE"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkSAECentre" />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comments"  >
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="txtComments" MaxLength="300" Text="" />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="sqldsGV3" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4usrconn %>"   />
            </td>
        </tr>
        <tr align="right">
            <td style="height: 38px"><asp:Button ID="cmdReset" runat="server" Text="Reset" 
                    Height="36px" Width="111px" CausesValidation="False" 
                    onclick="cmdReset_Click"  /></td>
            <td  style="height: 38px"  align="left">
                <asp:Button ID="cmdAddRecord" runat="server" Text="Add Record" Height="36px" 
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
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True" Width="95%"> 
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

