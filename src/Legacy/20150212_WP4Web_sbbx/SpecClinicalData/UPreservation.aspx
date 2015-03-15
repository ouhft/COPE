<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="UPreservation.aspx.cs" Inherits="SpecClinicalData_UPreservation" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conUPreservation" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />

    <table rules="all" cellpadding="2" cellspacing="2" border="2" style="width: 851px">
        <tr align="right">
            <th colspan="2" align="center">
                <asp:Label runat="server" id="lblPageDescription" Text="File Description <br/> (Maximum 500 Characters)"></asp:Label>
            </th>
        </tr>
        <tr>
            <td style="width:35%" align="right">
                <asp:label id="lblSide" runat="server"  Font-Size="Small" Text="Kidney Side *" Font-Bold="true"  />
            </td>
            <td style="width:65%" align="left">
                <asp:DropDownList ID="ddSide" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" Width="35%"  
                        AutoPostBack="true" OnSelectedIndexChanged="ddSide_SelectedIndexChanged"  >
                    <asp:ListItem Selected="True" Text="Select a Side" Value="0" />
                           
                </asp:DropDownList>
                <asp:XmlDataSource ID="XMLKidneySidesDataSource" runat="server" 
                        DataFile="~/App_Data/KidneySides.xml" XPath="/*/*" ></asp:XmlDataSource>
                <asp:RequiredFieldValidator runat="server" ID="rfv_ddSide" ControlToValidate="ddSide" InitialValue="0" Display="Dynamic" ErrorMessage="Please Select Side" 
                                    CssClass="MandatoryFieldMessage" ValidationGroup="Main" />
                
            </td>
        </tr>
        <asp:Panel ID="pnlSideSelected" runat="server" Visible="false">
            <tr align="right">
                <td >
                    <asp:Label runat="server" id="lblComment" Text="General Comments"></asp:Label>
                </td>
                <td  align="left">
                    <asp:TextBox runat="server" ID="txtComments" Width="90%" MaxLength="500" TextMode="MultiLine"  />
                
                </td>
                <asp:CheckBox runat="server" ID="chkRecordExist" Visible="false" />
            </tr>
        
            <asp:Panel runat="server" ID="pnlFileUpload" Visible="false">
                <tr align="right">
                    <td >
                        <asp:Label runat="server" id="lblFileUpload" Text="To Select a file, click on the Browse button " ></asp:Label>
                    </td>
                    <td  align="left">
                        <asp:FileUpload runat="server" ID="fileUpload1" Width="75%" />
                    </td>
                </tr>
            </asp:Panel>

            <asp:Panel runat="server" ID="pnlEditComments" Visible="false">

                <tr align="right">
                    <td >
                        <asp:Label runat="server" id="lblFileNameDatabase" Text="File Name" ></asp:Label>
                    </td>
                    <td  align="left">
                        <asp:Label runat="server" id="lblUploadedFileName" Text="" ></asp:Label>
                    </td>
                </tr>
            </asp:Panel>
        
                
            <tr align="right">
                <td>
                    <asp:Button ID="cmdReset" runat="server" Text="Reset"  Width="111px"  
                        CausesValidation="false" onclick="cmdReset_Click" />
                    <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                        ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                    </asp:ConfirmButtonExtender>
                    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                    </asp:ToolkitScriptManager>
                </td>
                <td  align="left">
                    <asp:Button ID="cmdUpload" runat="server" Text="Upload File"  Width="111px" 
                        CausesValidation="false" onclick="cmdUpload_Click"   />
                
                </td>
            </tr>

        </asp:Panel>
        
    </table>

    <table>
        <tr>
            <th>
                <asp:Label runat="server" ID="lblGV1"   Width="100%" Text="Data Added" />
                
            </th>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="GV1" runat="server" BackColor="White" BorderColor="#E7E7FF" 
                    BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                   GridLines="Horizontal"  Width="850px" 
                    AutoGenerateColumns="False" 
                    Font-Names="Arial" Font-Size="Small" PageSize="20" AllowSorting="True" OnSorting="GV1_Sorting" OnRowCommand="GV1_RowCommand" OnRowDataBound="GV1_RowDataBound"
                    >
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center"/>
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <SortedAscendingCellStyle BackColor="#F4F4FD" />
                    <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
                    <SortedDescendingCellStyle BackColor="#D8D8F0" />
                    <SortedDescendingHeaderStyle BackColor="#3E3277"
                        />
                    <Columns>
                        
                        <asp:TemplateField HeaderText="RowIndex" Visible="false" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblRowIndex"
                                    Text='<%#Bind("RowIndex")%>'>
                                </asp:Label>
                            </ItemTemplate>                            
                        </asp:TemplateField>

                        <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID"
                            DataNavigateUrlFields="RowIndex,TrialID, Side" 
                            DataNavigateUrlFormatString="~/SpecClinicalData/UPreservation.aspx?RowIndex={0}&TID={1}&Side={2}">
                        </asp:HyperLinkField>
                        <asp:TemplateField HeaderText="FileName" SortExpression="FileName" Visible="false"  >
                            
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblFileName"
                                    Text='<%#Bind("FileName")%>'>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Side" Visible="false" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblSide"
                                    Text='<%#Bind("Side")%>'>
                                </asp:Label>                                
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="Side" SortExpression="Side" HeaderText="Side"    />
                        <asp:BoundField DataField="FileName" SortExpression="FileName" HeaderText="FileName"    />
                        <asp:BoundField DataField="CreatedBy" SortExpression="CreatedBy" HeaderText="Uploaded By"    />
                        <asp:BoundField DataField="Date_Created" SortExpression="DateCreated" HeaderText="Date Upload"    />
                                              
                        <asp:TemplateField HeaderText="Download File">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkdownload" runat="server" Text="Download" CommandName="Download"
                                    CommandArgument='<%#Eval("FileName") +";" + Eval("FullName") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="12.5%">
                            <ItemTemplate>
                                <asp:Button ID="cmdDelete" Text="Delete" Runat="server" OnClick="Delete_Click" />
                                <asp:ConfirmButtonExtender ID="cmdDelete_ConfirmButtonExtender" runat="server" 
                                    ConfirmText="" Enabled="True" TargetControlID="cmdDelete">
                    
                                </asp:ConfirmButtonExtender>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                     
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"
                    >
                </asp:SqlDataSource>
            </td>
        
        </tr>
        
    </table>
</asp:Content>

