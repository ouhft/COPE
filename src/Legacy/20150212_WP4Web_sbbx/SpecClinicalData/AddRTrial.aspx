<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddRTrial.aspx.cs" Inherits="SpecClinicalData_AddRTrial" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddRTrial" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <table rules="all" cellpadding="4" cellspacing="4" border="1" style="width: 851px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:40%" align="right">
                <asp:label id="lblCentre" runat="server"  Font-Size="Small" Text="Retrieval Team"  />
            </td>
            <td style="width:60%" align="left">
                <asp:DropDownList ID="ddCentreCode"  runat="server"  AppendDataBoundItems="True" DataTextField="CentreNameMerged" DataValueField="Centre"  Enabled="false"
                            Width="75%"   >
                    <asp:ListItem Value="0" Text="Select Centre" Selected="True"  />
                    										            
				</asp:DropDownList>
                <asp:SqlDataSource ID="SQLDS_CentreCode" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>" 
                >

                </asp:SqlDataSource>
                <asp:label id="lblRetrievalTeamData" runat="server"  Font-Size="Small" Text=""  />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblDonorID" runat="server"  Font-Size="Small" Text="ET Donor/NHSBT Number"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtDonorID" runat="server" MaxLength="10" Width="25%" ></asp:TextBox>
                <asp:label id="lblDonorIDDetails" runat="server"  Font-Size="Small" Text=""  />
                
            </td>
        </tr>
       <tr style="display:none">
            <td align="right">
                <asp:label id="lblAgeOrDateOfBirth" runat="server"  Font-Size="Small" Text="Age/Date of Birth to Enter"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblAgeOrDateOfBirth" runat="server" ClientIDMode="AutoID" Display="Dynamic" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow"
                    AutoPostBack="true" OnSelectedIndexChanged="rblAgeOrDateOfBirth_SelectedIndexChanged" Enabled="false" >
                    
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XmlAgeOrDateOfBirthDataSource" runat="server" 
                        DataFile="~/App_Data/AgeOrDateOfBirth.xml" XPath="/*/*" ></asp:XmlDataSource>

                    
            </td>
        </tr>
        <asp:Panel runat="server" ID="pnlDonorAge" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblDonorAge" runat="server"  Font-Size="Small" Text="Donor Age"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtDonorAge" runat="server" MaxLength="3" Width="50px" ></asp:TextBox>
                    <asp:FilteredTextBoxExtender runat="server" ID="ftbetxtDonorAge" TargetControlID="txtDonorAge" FilterType="Numbers" FilterMode="ValidChars" />
                    <asp:RangeValidator id="rv_txtDonorAge"
                        ControlToValidate="txtDonorAge"                   
                        MinimumValue=""
                        MaximumValue=""
                        Type="Integer"
                        EnableClientScript="True"
                        Text=""
                        runat="server"  CssClass="Caution" Display="Dynamic" ValidationGroup="Main" />
                    <asp:label ID="lblDonorAgeDetails" runat="server"  Width="50px" ></asp:label>
                </td>
            </tr>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlDonorDateOfBirth" Visible="false">
            <tr>
                <td align="right">
                    <asp:label id="lblDonorDateOfBirth" runat="server"  Font-Size="Small" Text="Donor Date of Birth"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtDonorDateOfBirth" runat="server" MaxLength="10" Width="25%" Enabled="false"></asp:TextBox>
                        
                        
                    <asp:CompareValidator ID="cv_txtDonorDateOfBirth" runat="server" Display="Dynamic" ControlToValidate="txtDonorDateOfBirth" Type="Date" 
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Date as DD/MM/YYYY" ></asp:CompareValidator>
                    <asp:RangeValidator ID="rb_txtDonorDateOfBirth" runat="server" Display="Dynamic" ControlToValidate="txtDonorDateOfBirth" Type="Date"
                        CssClass="Caution" ErrorMessage="">

                    </asp:RangeValidator>

                    <asp:label id="lblDonorDateOfBirthDetails" runat="server"  Font-Size="Small" Text="Donor Date of Birth"  />
                </td>
            </tr>
        </asp:Panel>

        
    </table>
    <br />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px; " >
        <tr>
            <td style="width:40%" align="right">
                <asp:label id="lblKidneyLeftDonated" runat="server"  Font-Size="Small" Text="Kidney Donated (Left)"  />
            </td>
            <td style="width:60%" align="left">
                <asp:RadioButtonList ID="rblKidneyLeftDonated" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" Enabled="True">
                    
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblKidneyRightDonated" runat="server"  Font-Size="Small" Text="Kidney Donated (Right)"  />
            </td>
            <td align="left">
                <asp:RadioButtonList ID="rblKidneyRightDonated" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" Enabled="True">
                    
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <tr>
            <td  align="right">
                <asp:label id="lblDonorEligibilityCriteria" runat="server"  Font-Size="Small" Text=""  />
            </td>
            <td  align="left">
                <asp:RadioButtonList ID="rblInclusionCriteriaChecked" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLKidneyMainDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
                <asp:XmlDataSource ID="XMLKidneyYNMainDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" border="0" style="width: 851px">
        
        
        <tr>
            <td style="width:40%" align="right">
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="100px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"   />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
            </td>
            <td style="width:60%" align="left">
                <asp:Button ID="cmdAddData" runat="server" Text="Randomise" Height="36px"  CausesValidation="False"  UseSubmitBehavior="False" OnClick="cmdAddData_Click" Width="115px"    />
                <asp:ConfirmButtonExtender ID="cmdAddData_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdAddData">
                </asp:ConfirmButtonExtender>
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
                        <asp:BoundField DataField="KidneyRID" SortExpression="KidneyRID" HeaderText="KidneyRID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="MainTrialID" SortExpression="MainTrialID" HeaderText="TrialID" Visible="false"
                            DataNavigateUrlFields="MainTrialID, KidneyRID" 
                            DataNavigateUrlFormatString="AddRTrial.aspx?TID={0}&KidneyRID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="MainTrialID" SortExpression="MainTrialID" HeaderText="TrialID"    />
                        <asp:BoundField DataField="MainDonorID" SortExpression="MainDonorID" HeaderText="DonorID"    />
                        <asp:BoundField DataField="Date_OfBirthDonor" SortExpression="DateOfBirthDonor" HeaderText="Donor DateOfBirth"    />
                        <asp:BoundField DataField="LeftKidneyDonate" SortExpression="LeftKidneyDonate" HeaderText="Left Kidney Donated"    />
                        <asp:BoundField DataField="RightKidneyDonate" SortExpression="RightKidneyDonate" HeaderText="Right Kidney Donated"    />
                        <asp:BoundField DataField="InclusionCriteriaChecked" SortExpression="InclusionCriteriaChecked" HeaderText="Donor Eligibility"    />
                        <asp:BoundField DataField="LeftRandomisationArm" SortExpression="LeftRandomisationArm" HeaderText="Left Kidney Randomisation Arm"    />
                        <asp:BoundField DataField="RightRandomisationArm" SortExpression="RightRandomisationArm" HeaderText="Right Kidney Randomisation Arm"    />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
    
</asp:Content>

