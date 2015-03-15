<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddDonorDetails.aspx.cs" Inherits="SpecClinicalData_AddDonorDetails" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddDonorDetails" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="upnl1">
        <ContentTemplate>
            <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 1020px">
        
            <tr>
                <th colspan="2">
                    <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
                </th>
            </tr>
        
            <tr>
                <td style="width:30%" align="right">
                    <asp:label id="lblDateDonorAdmission" runat="server"  Font-Size="Small" Text="Donor Date of Admission" Font-Bold="true" />
                </td>
                <td style="width:70%" align="left">
                    <asp:TextBox ID="txtDateDonorAdmission" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                    <asp:CalendarExtender ID="txtDateDonorAdmission_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateDonorAdmission" PopupPosition="Right">
                    </asp:CalendarExtender>
                
                    <asp:CompareValidator ID="cv_txtDateDonorAdmission" runat="server" Display="Dynamic" ControlToValidate="txtDateDonorAdmission" Type="Date" ValidationGroup="Main"
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Date as DD/MM/YYYY" ></asp:CompareValidator>
                    <asp:RangeValidator ID="rv_txtDateDonorAdmission" runat="server" Display="Dynamic" ControlToValidate="txtDateDonorAdmission" Type="Date" ValidationGroup="Main"
                        CssClass="Caution" ErrorMessage="">

                    </asp:RangeValidator>

                    <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblDateDonorOperation" runat="server"  Font-Size="Small" Text="Donor Date of Operation" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtDateDonorOperation" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                    <asp:CalendarExtender ID="txtDateDonorOperation_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateDonorOperation" PopupPosition="Right">
                    </asp:CalendarExtender>
                
                    <asp:CompareValidator ID="cv_txtDateDonorOperation" runat="server" Display="Dynamic" ControlToValidate="txtDateDonorOperation" Type="Date" ValidationGroup="Main"
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Date as DD/MM/YYYY" ></asp:CompareValidator>
                    <asp:RangeValidator ID="rv_txtDateDonorOperation" runat="server" Display="Dynamic" ControlToValidate="txtDateDonorOperation" Type="Date" ValidationGroup="Main"
                        CssClass="Caution" ErrorMessage="">

                    </asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblSex" runat="server"  Font-Size="Small" Text="Gender" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rblSex" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLSexDataSource" runat="server" 
                            DataFile="~/App_Data/Sex.xml" XPath="/*/*" ></asp:XmlDataSource>
                    <asp:RequiredFieldValidator ID="rfv_Sex" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option"  
                        CssClass="Caution" ControlToValidate="rblSex"  >
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblWeight" runat="server"  Font-Size="Small" Text="Weight (kg)" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtWeight" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                
                    <asp:CompareValidator ID="cv_txtWeight" runat="server" Display="Dynamic" ControlToValidate="txtWeight" Type="Double" ValidationGroup="Main"
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are allowed" ></asp:CompareValidator>
                    <asp:RangeValidator ID="rv_txtWeight" runat="server" Display="Dynamic" ControlToValidate="txtWeight" Type="Double"
                        CssClass="Caution" ErrorMessage="" ValidationGroup="Main" >

                    </asp:RangeValidator>
                </td>
            </tr>
             <tr>
                <td align="right">
                    <asp:label id="lblHeight" runat="server"  Font-Size="Small" Text="Height (cm)" Font-Bold="true"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtHeight" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                
                    <asp:CompareValidator ID="cv_txtHeight" runat="server" Display="Dynamic" ControlToValidate="txtHeight" Type="Integer" ValidationGroup="Main"
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are not allowed." ></asp:CompareValidator>
                    <asp:RangeValidator ID="rv_txtHeight" runat="server" Display="Dynamic" ControlToValidate="txtHeight" Type="Integer" ValidationGroup="Main"
                        CssClass="Caution" ErrorMessage=""  >

                    </asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblBloodGroup" runat="server"  Font-Size="Small" Text="Blood Group"  />
                </td>
                <td align="left">
                    <asp:RadioButtonList ID="rblBloodGroup" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XmlBloodGroupDataSource" runat="server" 
                            DataFile="~/App_Data/BloodGroups.xml" XPath="/*/*" ></asp:XmlDataSource>
                </td>
            </tr>
        
            <tr>
                <td align="right">
                    <asp:label id="lblOtherOrgansDonated" runat="server"  Font-Size="Small" Text="Other Organs Offered for Donation"   />
                </td>
                <td align="left">
                    <asp:CheckBoxList ID="cblOtherOrgansDonated" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="cblOtherOrgansDonated_SelectedIndexChanged">
                    
                    </asp:CheckBoxList>
                    <asp:XmlDataSource ID="XMLOtherOrgansDataSource" runat="server" 
                            DataFile="~/App_Data/OtherOrgans.xml" XPath="/*/*" ></asp:XmlDataSource>
                </td>
            </tr>
        
            <tr>
                <td align="right">
                    <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="General Comments"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" MaxLength="500" Width="90%"></asp:TextBox>
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlAllDataAdded" Visible="false">
                <tr>
                    <td align="right">
                        <asp:label id="lblAllDataAdded" runat="server"  Font-Size="Small" Text="Tick Box if Data Entry Complete"  />
                    </td>
                    <td align="left">
                        <asp:CheckBox runat="server" ID="chkAllDataAdded" AutoPostBack="true" OnCheckedChanged="chkAllDataAdded_CheckedChanged"/>
                        <asp:Label runat="server" ID="lblAllDataAddedMessage"  Text=""  CssClass="Incomplete"/>
                
                    </td>
                </tr>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlReasonModified" Visible="false">
                <tr>
                    <td align="right">
                        <asp:label id="lblReasonModified" runat="server"  Font-Size="Small" Text="Enter Reason for Modifying Data *" Font-Bold="true"  />
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtReasonModified" runat="server" MaxLength="10000" Width="95%" TextMode="MultiLine" Font-Names="Arial" Font-Size="Small" 
                            onkeyup="AutoExpandTextBox(this, event)" Rows="2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="lblReasonModifiedOld" runat="server"  Font-Size="Small" Text="Reason(s) Data Modified Earlier"  />
                    </td>
                    <td align="left">
                        <asp:label id="lblReasonModifiedOldDetails" runat="server"  Font-Size="Small" Text="" BackColor="#99CCFF" Width="95%" />
                    </td>
                </tr>
            </asp:Panel> 
            <asp:Panel runat="server" ID="pnlFinal" Visible="false">
                <tr>
                    <td align="right">
                        <asp:label id="lblDataFinal" runat="server"  Font-Size="Small" Text="Tick to Mark Data as Final"  />
                    </td>
                    <td align="left">
                        <asp:CheckBox runat="server" ID="chkDataFinal" />
                    </td>
                </tr>
            </asp:Panel>
        </table>

        </ContentTemplate>
    </asp:UpdatePanel>
    
    <table cellpadding="2" cellspacing="2" border="0" style="width: 1020px"> 
        <tr>
            <td style="width:30%" align="right">
                <asp:HyperLink runat="server" ID="hlkSummaryPage" Text="Summary Page" />
                <asp:Button ID="cmdDelete" runat="server" Text="Delete" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdDelete_Click"  />
                <asp:ConfirmButtonExtender ID="cmdDelete_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdDelete">
                </asp:ConfirmButtonExtender>
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
            </td>
            <td style="width:70%" align="left">
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px" CausesValidation="true" ValidationGroup="Main"  UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
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
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True"   Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="DonorIdentificationID" SortExpression="DonorIdentificationID" HeaderText="DonorIdentificationID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="MainDonorID" SortExpression="MainDonorID" HeaderText="DonorID"
                            DataNavigateUrlFields="TrialID, DonorIdentificationID" 
                            DataNavigateUrlFormatString="AddDonorDetails.aspx?TID={0}&DonorIdentificationID={1}" Visible="false">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="TrialID" SortExpression="TrialID" HeaderText="TrialID"    />
                        <asp:BoundField DataField="MainDonorID" SortExpression="MainDonorID" HeaderText="Donor ET/ NHSBT Number" Visible="false"   />
                        <asp:BoundField DataField="Date_DonorOperation" SortExpression="DateDonorOperation" HeaderText="Donor Operation"    />
                        <asp:BoundField DataField="AgeOperation" SortExpression="AgeOperation" HeaderText="Age at Operation"  Visible="false"  />
                        <asp:BoundField DataField="Sex" SortExpression="Sex" HeaderText="Gender"    />
                        <asp:BoundField DataField="Weight" SortExpression="Weight" HeaderText="Weight"    />
                        <asp:BoundField DataField="Height" SortExpression="Height" HeaderText="Height"    />                       
                        <asp:BoundField DataField="BMI" SortExpression="BMI" HeaderText="BMI" />
                        <asp:BoundField DataField="BloodGroup" SortExpression="BloodGroup" HeaderText="Blood Group" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

