<%@ Page Title="" Language="C#" MasterPageFile="~/TopMasterPage.master" AutoEventWireup="true" CodeFile="AAccess.aspx.cs" Inherits="OtherArea_AAccess" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAAccess" ContentPlaceHolderID="cplMainContents" Runat="Server">
       <asp:Label runat="server" ID="lblUserMessages" Font-Size="Small" ForeColor="Red" Width="100%" />


    <table rules="all" cellpadding="0" cellspacing="0" border="1"  style="width: 800px; font-size: small;"  >
        
        <tr >
            <th  colspan="2">
                <asp:Label runat="server" ID="lblPageDescription" Text="Enter details for new user." />
            </th>
           
        </tr>
        <tr align="right">
            <td style="width: 40%" >
                <asp:Label runat="server" ID="Label1" Text="Centre"  />
            </td>
            <td style="width: 60%" align="left">
                <asp:Label runat="server" ID="lblCentreDetails" Width="90%" Text="" Font-Bold="true"  />
                <asp:DropDownList ID="ddCentreList"  runat="server"  AppendDataBoundItems="True" DataTextField="CentreDetails" DataValueField="CentreCodeMerged" 
                            Width="5%" Visible="false"   >
                            
                    										            
				</asp:DropDownList>
                <asp:SqlDataSource ID="sqldsCentreLists" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"
                        >
                </asp:SqlDataSource>
                
            </td>
        </tr>
        <tr align="right">
            <td  >
                <asp:Label runat="server" ID="lblUserName" Text="Database UserID "  />
            </td>
            <td  align="left">
                <asp:TextBox runat="server" ID="txtUserName" Width="50%" MaxLength="45" Enabled="False"  />
                
            </td>
        </tr>
        <tr align="right">
            <td >
                <asp:Label runat="server" ID="lblFirstName" Text="First Name"  />
            </td>
            <td  align="left" >
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
                <asp:TextBox runat="server" ID="txtJobTitle" Width="50%" MaxLength="100"  />
            </td>
        </tr>
        
        <tr align="right">
            <td >
                <asp:Label runat="server" ID="lblSuperUser" Text="Super User?"  />
            </td>
            <td   align="left">
                <asp:RadioButtonList ID="rdoSuperUser" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="YES" Text="Yes"/>
                    <asp:ListItem Value="NO" Text="No" Selected="True"/>
                    
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr align="right">
            <td >
                <asp:Label runat="server" ID="lblAddEdit" Text="Add/Edit/View TrialID Data?"  />
            </td>
            <td   align="left">
                <asp:RadioButtonList ID="rdoAddEdit" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="YES" Text="Yes"/>
                    <asp:ListItem Value="NO" Text="No" Selected="True"/>
                    
                </asp:RadioButtonList>
            </td>
        </tr>
        <tr align="right">
            <td >
                <asp:Label runat="server" ID="lblRandomise" Text="Randomise?"  />
            </td>
            <td   align="left">
                <asp:RadioButtonList ID="rdoRandomise" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="YES" Text="Yes"/>
                    <asp:ListItem Value="NO" Text="No" Selected="True"/>
                    
                </asp:RadioButtonList>
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
        <tr align="right">
            <td >
                <asp:label id="lblDataComments" runat="server"  
                     Text="Database Comments"  />
            </td>
            <td   align="left">
                <asp:TextBox runat="server" ID="txtDataComments" Width="95%" 
                    MaxLength="300" Height="50px" TextMode="MultiLine" />
            </td>
        </tr>
        <tr align="right">
            <td style="height: 38px">
                <asp:Button ID="cmdDelete" runat="server" Text="Remove Access" 
                    Height="36px" Width="111px" CausesValidation="false" UseSubmitBehavior="false" OnClick="cmdDelete_Click"
                      />
                
                <asp:ConfirmButtonExtender ID="cmdDelete_ConfirmButtonExtender" runat="server" ConfirmText="" Enabled="True" TargetControlID="cmdDelete">
                </asp:ConfirmButtonExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                <asp:Button ID="cmdReset" runat="server" Text="Reset" 
                    Height="36px" Width="111px" CausesValidation="false" UseSubmitBehavior="false" 
                    onclick="cmdReset_Click"  />

            </td>
            <td  style="height: 38px"  align="left">
                <asp:Button ID="cmdAddRecord" runat="server" Text="Add Record" Height="36px" 
                    Width="132px" CausesValidation="false" UseSubmitBehavior="false" onclick="cmdAddRecord_Click"  />
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
                            DataNavigateUrlFields="ListusersID, ID" 
                            DataNavigateUrlFormatString="AAccess.aspx?OtherID={0}&ModifyID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"    />
                        <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name"    />
                        <asp:BoundField DataField="CentreCode" SortExpression="CentreCode" HeaderText="Centre Code"    />
                        <asp:BoundField DataField="SuperUser" SortExpression="SuperUser" HeaderText="Super User"    />
                        <asp:BoundField DataField="AddEdit" SortExpression="AddEdit" HeaderText="Add/Edit/View TrialID Data"    />
                        <asp:BoundField DataField="Randomise" SortExpression="Randomise" HeaderText="Randomise"    />
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
            <td  colspan="2">
                <asp:GridView ID="GV2" runat="server" AutoGenerateColumns="False" 
                    BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV2_Sorting"  AllowSorting="True"  OnRowDataBound="GV2_RowDataBound"  Width="95%"> 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C"  />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="ListusersID" SortExpression="ListusersID" HeaderText="ListusersID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="UserName" SortExpression="UserName" HeaderText="UserID"
                            DataNavigateUrlFields="ListusersID" 
                            DataNavigateUrlFormatString="AAccess.aspx?OtherID={0}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name"    />
                        <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name"    />
                        <asp:BoundField DataField="Email" SortExpression="Email" HeaderText="Email"    />
                        <asp:BoundField DataField="Active" SortExpression="Active" HeaderText="Active"    />  
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4usrconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

