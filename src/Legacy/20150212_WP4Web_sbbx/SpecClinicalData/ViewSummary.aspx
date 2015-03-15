<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="ViewSummary.aspx.cs" Inherits="SpecClinicalData_ViewSummary" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conViewSummary" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" Font-Size="Small" Width="100%" ForeColor="Red" />

    <table cellpadding="0" cellspacing="0" border="0" width="95%">
        <tr align="center">
            <td>
                <table style="width:100%" cellpadding="0" cellspacing="1" border="0" >
                    <tr>
                        <th colspan="2">
                            <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
                        </th>
                    </tr>
                    <tr align="center">
                        <th style="width: 100%" colspan="2">
                            <asp:label  id="lblGV1" runat="server"  Font-Size="Small"></asp:label>
                        </th>
                     </tr>
                                      
                    <tr align="center">
                        
                        <td style="width: 100%" colspan="2">
                             
                           <asp:GridView ID="GV1" runat="server" AutoGenerateColumns="False" 
                                BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" 
                                CellPadding="3" GridLines="Horizontal" Width="750px" OnRowDataBound="GV1_RowDataBound" 
                                > 
                                <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                                <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C"  />
                                <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" VerticalAlign="Middle" />
                                <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                                <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                                <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:HyperLinkField DataTextField="EventName" SortExpression="EventName" HeaderText="Event"
                                        DataNavigateUrlFields="PageLink,TrialID, PageIdentifier,EventID, DateLink,Event_Date,EventCode " 
                                        DataNavigateUrlFormatString="{0}?TID={1}&{2}={3}&{4}={5}&EventCode={6}">
                                    </asp:HyperLinkField>
                                    <asp:BoundField DataField="Completed" SortExpression="Completed" HeaderText="Added"></asp:BoundField>
                                    <asp:BoundField  DataField="DataLocked" SortExpression="DataLocked" HeaderText="Locked"></asp:BoundField>
                                    <asp:BoundField  DataField="DataFinal" SortExpression="DataFinal" HeaderText="Final"></asp:BoundField>
			                        <asp:BoundField DataField="StudyID" SortExpression="StudyID" HeaderText="StudyID" Visible="false"></asp:BoundField>
			                        <asp:BoundField DataField="EventID" SortExpression="eventID" HeaderText="eventID" Visible="false"></asp:BoundField>
                                </Columns>
                           </asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                        </td>
                        
                    </tr>
                    
                </table>
                
            </td>
        </tr>
    </table>
</asp:Content>

