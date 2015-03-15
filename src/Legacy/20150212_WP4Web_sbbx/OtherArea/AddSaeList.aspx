<%@ Page Title="" Language="C#" MasterPageFile="~/TopMasterPage.master" AutoEventWireup="true" CodeFile="AddSaeList.aspx.cs" Inherits="OtherArea_AddSaeList" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddSaeList" ContentPlaceHolderID="cplMainContents" Runat="Server">
     <asp:Label runat="server" ID="lblUserMessages" 
                    Font-Size="Small" ForeColor="Red" Width="100%" />
    <table  cellpadding="0" cellspacing="0"   style="width: 800px; font-size: small;"  >
        <tr >
            <th  colspan="2">
                <asp:Label runat="server" ID="lblPageDescription" Text="Enter details for new user" />
            </th>
           
        </tr>
    </table>
    <table style="width:850px">
        <tr align="center" >
            <th  colspan="2">
                <asp:Label runat="server" ID="lblGV1" Text="" />
            </th>
           
        </tr>
        <tr align="center">
            <td colspan="2">
                <asp:GridView ID="GV1" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" GridLines="Both" OnSorting="GV1_Sorting" AllowSorting="True" OnRowDataBound="GV1_RowDataBound" Width="95%"> 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        
                        <asp:TemplateField HeaderText="ListusersID" Visible="false" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblListusersID"
                                    Text='<%#Bind("ListusersID")%>'>
                                </asp:Label>                                
                            </ItemTemplate>                            
                        </asp:TemplateField> 
                        <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"    />
                        <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name"    />
                        <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Email"    />
                        <asp:TemplateField HeaderText="Email All SAE">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkIncluded" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SAEAllCentre" Visible="false" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblSAEAllCentre"
                                    Text='<%#Bind("SAEAllCentre")%>'>
                                </asp:Label>                                
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comments" Visible="true" ItemStyle-Width="30%" >
                            <ItemTemplate>
                                <asp:TextBox runat="server" id="txtSAEAllCentreComments" MaxLength="500" Width="95%"
                                    Text='<%#Bind("SAEAllCentreComments")%>'>
                                </asp:TextBox>                                
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SAEAllCentreComments" Visible="false" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblSAEAllCentreComments"
                                    Text='<%#Bind("SAEAllCentreComments")%>'>
                                </asp:Label>                                
                            </ItemTemplate>                            
                        </asp:TemplateField>                      
                        
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4usrconn %>"   />
                
            </td>
        </tr>
        <tr align="center" >
            <td style="width:50%" align="right">
                
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                            </asp:ToolkitScriptManager>
            </td>
            <td style="width:50%" align="left">
                <asp:Button ID="cmdUpdate" runat="server" Text="Update" Height="36px"   UseSubmitBehavior="False" OnClick="cmdUpdate_Click"    />
                
            </td>
           
        </tr>
    </table>
</asp:Content>

