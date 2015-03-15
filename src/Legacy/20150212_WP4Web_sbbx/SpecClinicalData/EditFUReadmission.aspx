<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalRecipientPage.master" AutoEventWireup="true" CodeFile="EditFUReadmission.aspx.cs" Inherits="SpecClinicalData_EditFUReadmission" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conEditFUReadmission" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="MandatoryFieldMessage" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:30%" align="right">
                <asp:label id="lblOccasion" runat="server"  Font-Size="Small" Text="Occasion"  />
            </td>
            <td style="width:70%" align="left">
                <asp:DropDownList ID="ddOccasion" runat="server" DataTextField="FollowUp" DataValueField="UniqueID" AppendDataBoundItems="true"  >
                    <asp:ListItem Selected="True" Value="0" Text="Select Occasion"   /> 
                </asp:DropDownList>
                
                <asp:SqlDataSource ID="SQLDS_Occasion" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                <asp:RequiredFieldValidator runat="server" ID="rfvddOccasion"  ControlToValidate="ddOccasion" Display="Dynamic"  InitialValue="0"
                    CssClass="MandatoryFieldMessage" ErrorMessage="Please Select an Occasion"/>
                <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblDateAdmission" runat="server"  Font-Size="Small" Text="Date Admission *" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtDateAdmission" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                <asp:CalendarExtender ID="txtDateAdmission_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateAdmission">
                </asp:CalendarExtender>
                <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtDateAdmission"  ControlToValidate="txtDateAdmission" Display="Dynamic"  
                    CssClass="MandatoryFieldMessage" ErrorMessage="Please Enter Date of Admission"/>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblDateDischarge" runat="server"  Font-Size="Small" Text="Date Discharge" Font-Bold="true" />
            </td>
            <td  align="left">
                <asp:TextBox ID="txtDateDischarge" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                <asp:CalendarExtender ID="txtDateDischarge_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateDischarge">
                </asp:CalendarExtender>
               
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblICU" runat="server"  Font-Size="Small" Text="ICU" Font-Bold="true" />
            </td>
            <td align="left">
                <asp:DropDownList ID="ddICU" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true"  >
                    <asp:ListItem Selected="True" Value="0" Text="Select if Admitted to ICU"   /> 
                </asp:DropDownList>
                 <asp:XmlDataSource ID="XMLMainOptionsDataYNSource" runat="server" 
                        DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblNeedDialysis" runat="server"  Font-Size="Small" Text="Need Dialysis" Font-Bold="true" />
            </td>
            <td align="left">
                <asp:DropDownList ID="ddNeedDialysis" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true"  >
                    <asp:ListItem Selected="True" Value="0" Text="Select if Dialysis Needed"   /> 
                </asp:DropDownList>   
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblBiopsyTaken" runat="server"  Font-Size="Small" Text="Biopsy Taken" Font-Bold="true" />
            </td>
            <td align="left">
                <asp:DropDownList ID="ddBiopsyTaken" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true"  >
                    <asp:ListItem Selected="True" Value="0" Text="Select if Biopsy Taken"   /> 
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblSurgery" runat="server"  Font-Size="Small" Text="Surgery" Font-Bold="true" />
            </td>
            <td align="left">
                <asp:DropDownList ID="ddSurgery" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true"  >
                    <asp:ListItem Selected="True" Value="0" Text="Select if Surgery Required"   /> 
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="Reason for Admission *"  Font-Bold="true"/>
            </td>
            <td align="left">
                <asp:TextBox ID="txtComments" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" ></asp:TextBox>
                <asp:RequiredFieldValidator runat="server" ID="rfvtxtComments"  ControlToValidate="txtComments" Display="Dynamic"  
                    CssClass="MandatoryFieldMessage" ErrorMessage="Please Enter Reason for Admission"/>
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlAllDataAdded" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblAllDataAdded" runat="server"  Font-Size="Small" Text="Tick Box if Data Entry Complete"  />
                </td>
                <td align="left">
                    <asp:CheckBox runat="server" ID="chkAllDataAdded" AutoPostBack="true" OnCheckedChanged="chkAllDataAdded_CheckedChanged"/>
                    <asp:Label runat="server" ID="lblAllDataAddedMessage" CSSClass="Incomplete" Text=""  />
                
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
        <tr>
            <td align="right">
                
                <asp:Button ID="cmdDelete" runat="server" Text="Delete" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdDelete_Click"   />
                <asp:ConfirmButtonExtender ID="cmdDelete_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdDelete">
                </asp:ConfirmButtonExtender>
            </td>
            <td align="left">
                <asp:Button ID="cmdUpdate" runat="server" Text="Update Data" Height="36px"   CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdUpdate_Click"    />
                
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
                    CellPadding="3" GridLines="Horizontal" OnSorting="GV1_Sorting" AllowSorting="True" OnRowDataBound="GV1_RowDataBound"  Width="95%" > 
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
                    <AlternatingRowStyle BackColor="#F7F7F7" HorizontalAlign="Center" />
                    <Columns>
                        <asp:BoundField DataField="RReadmissionsID" SortExpression="RReadmissionsID" HeaderText="RReadmissionsID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="Date_Admission" SortExpression="DateAdmission" HeaderText="Date Admission"
                            DataNavigateUrlFields="TrialID, TrialIDRecipient, RReadmissionsID" 
                            DataNavigateUrlFormatString="EditFUReadmission.aspx?TID={0}&TID_R={1}&RReadmissionsID={2}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="Date_Discharge" SortExpression="DateDischarge" HeaderText="Date Discharge"    />
                        <asp:BoundField DataField="Occasion" SortExpression="Occasion" HeaderText="Occasion"    />
                        <asp:BoundField DataField="ReasonAdmission" SortExpression="ReasonAdmission" HeaderText="Reason Admission" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

