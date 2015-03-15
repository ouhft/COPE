<%@ Page Title="" Language="C#" MasterPageFile="~/TopMasterPage.master" AutoEventWireup="true" CodeFile="ENUser.aspx.cs" Inherits="OtherArea_ENUser" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conENUser" ContentPlaceHolderID="cplMainContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" Font-Size="Small" CssClass="Caution" Width="100%" />
        
    
    <table style="width:1200px">
        <tr align="center" >
            <th  colspan="2">
                <asp:Label runat="server" ID="lblGV3" Text="" />
            </th>
           
        </tr>
        <tr>
             <td colspan="2" align="center">
                <asp:GridView ID="GV3" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" GridLines="Both" OnSorting="GV3_Sorting" AllowSorting="True" OnRowDataBound="GV3_RowDataBound" Width="98%"> 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderText="Centre" Visible="true" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblCentre"
                                    Text='<%#Bind("Centre")%>'>
                                </asp:Label>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="CentreName" SortExpression="CentreName" HeaderText="Centre Name"    />
                        
                        <asp:TemplateField HeaderText="ListusersID" Visible="false" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblListusersID"
                                    Text='<%#Bind("ListusersID")%>'>
                                </asp:Label>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Super User"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkSuperUser" Checked='<%# Eval("SuperUser").ToString().Equals("YES")  %>' />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Randomise"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkRandomise" Checked='<%# Eval("Randomise").ToString().Equals("YES")  %>' />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="View Randomise"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkViewRandomise" Checked='<%# Eval("ViewRandomise").ToString().Equals("YES")  %>' />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Donor"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkAddEdit" Checked='<%# Eval("AddEdit").ToString().Equals("YES")  %>' />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Recipient Data"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkAddEditRecipient" Checked='<%# Eval("AddEditRecipient").ToString().Equals("YES")  %>' />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Follow Up Data"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkAddEditFollowUp" Checked='<%# Eval("AddEditFollowUp").ToString().Equals("YES")  %>' />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email TrialID Created"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkTrialIDCentre" Checked='<%# Eval("TrialIDCentre").ToString().Equals("YES")  %>' />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email SAE"  >
                            <ItemTemplate>
                                <asp:CheckBox runat="server" id="chkSAECentre" Checked='<%# Eval("SAECentre").ToString().Equals("YES")  %>' />
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comments"  >
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="txtComments" MaxLength="300" Text='<%#Bind("Comments")%>' />
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
                <asp:Button ID="cmdUpdate" runat="server" Text="Update" Height="36px" 
                    Width="132px" OnClick="cmdUpdate_Click"   />
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
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True" OnRowDataBound="GV1_RowDataBound"  Width="95%"> 
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
        
        
    </table>
</asp:Content>

