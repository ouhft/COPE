<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalMasterPage.master" AutoEventWireup="true" CodeFile="AddDonorLabResults.aspx.cs" Inherits="SpecClinicalData_AddDonorLabResults" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddDonorLabResults" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="upnl1">
        <ContentTemplate>

        <table rules="all" cellpadding="2" cellspacing="2" border="2" style="width: 1020px">
            <tr>
                <th colspan="3">
                    <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
                </th>
            </tr>
            <tr>
                <th style="width:30%" align="right">

                </th>
                <th style="width:20%"  align="left">
                    <asp:label id="lblUnit" runat="server"  Font-Size="Small" Text="Unit"  />
                
                </th>

                <th style="width:50%" align="left">
                    <asp:label id="lblValue" runat="server"  Font-Size="Small" Text="Value"  />
                </th>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblHb" runat="server"  Font-Size="Small" Text="Hb" Font-Bold="true"  />
                </td>
                <td   align="left">
                
                    <asp:RadioButtonList ID="rblHbUnits" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                        AutoPostBack="true" OnSelectedIndexChanged="rblHbUnits_SelectedIndexChanged" RepeatLayout="Flow"   >
                       
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLHbDataSource" runat="server" 
                            DataFile="~/App_Data/HbUnits.xml" XPath="/*/*" ></asp:XmlDataSource>

                

                

                
                 
                </td>
                <td   align="left">
                    <asp:TextBox ID="txtHb" runat="server" MaxLength="5" Width="25%" ></asp:TextBox>
                

                    <asp:CompareValidator ID="cv_txtHb" runat="server" Display="Dynamic" ControlToValidate="txtHb" Type="Double" 
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values for Hb. Decimals are allowed" ValidationGroup="MainGroup" ></asp:CompareValidator>

                    <asp:RangeValidator ID="rv_txtHb" runat="server" Display="Dynamic" ControlToValidate="txtHb" Type="Double"
                        CssClass="Caution" MinimumValue="" MaximumValue="" ErrorMessage="" ValidationGroup="MainGroup" >

                    </asp:RangeValidator>

                    <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />

                </td>
            </tr>
        
            <tr>
                <td align="right">
                    <asp:label id="lblHt" runat="server"  Font-Size="Small" Text="Ht (%)" Font-Bold="true"   />
                </td>
                <td   align="left">

                </td>
                <td align="left">
                    <asp:TextBox ID="txtHt" runat="server" MaxLength="5" Width="25%" ></asp:TextBox>
                

                    <asp:CompareValidator ID="cv_txtHt" runat="server" Display="Dynamic" ControlToValidate="txtHt" Type="Integer" 
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are not allowed." ValidationGroup="MainGroup" ></asp:CompareValidator>
                    <asp:RangeValidator ID="rv_txtHt" runat="server" Display="Dynamic" ControlToValidate="txtHt" Type="Double"
                        CssClass="Caution" MinimumValue="" MaximumValue="" ErrorMessage="" ValidationGroup="MainGroup" >

                    </asp:RangeValidator> 

                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblpH" runat="server"  Font-Size="Small" Text="pH" Font-Bold="true"   />
                </td>
                <td   align="left">

                </td>
                <td align="left">
                    <asp:TextBox ID="txtpH" runat="server" MaxLength="5" Width="25%" ></asp:TextBox>
                

                    <asp:CompareValidator ID="cv_txtpH" runat="server" Display="Dynamic" ControlToValidate="txtpH" Type="Double" 
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values." ValidationGroup="MainGroup" ></asp:CompareValidator>
                    <asp:RangeValidator ID="rv_txtpH" runat="server" Display="Dynamic" ControlToValidate="txtpH" Type="Double"
                        CssClass="Caution" MinimumValue="" MaximumValue="" ErrorMessage="Value should be between 7.35 and 7.45" ValidationGroup="MainGroup" >

                    </asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblpCO2" runat="server"  Font-Size="Small" Text="pCO2" Font-Bold="true"   />
                </td>
                <td   align="left">
                    <asp:RadioButtonList ID="rblpCO2Units" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                        AutoPostBack="true" OnSelectedIndexChanged="rblpCO2Units_SelectedIndexChanged"  RepeatLayout="Flow" >
                       
                        </asp:RadioButtonList>
                        <asp:XmlDataSource ID="XMLpCO2UnitsDataSource" runat="server" 
                                DataFile="~/App_Data/pCO2Units.xml" XPath="/*/*" ></asp:XmlDataSource>
                
                </td>
                <td align="left">
                    <asp:TextBox ID="txtpCO2" runat="server" MaxLength="5" Width="25%"  ></asp:TextBox>
                
                    <asp:CompareValidator ID="cv_txtpCO2" runat="server" Display="Dynamic" ControlToValidate="txtpCO2" Type="Double" 
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are allowed." ValidationGroup="MainGroup" ></asp:CompareValidator>
                    <asp:RangeValidator ID="rv_txtpCO2" runat="server" Display="Dynamic" ControlToValidate="txtpCO2" Type="Double"
                        CssClass="Caution" MinimumValue="" MaximumValue="" ErrorMessage="Value should be between 7.35 and 7.45" ValidationGroup="MainGroup" >

                    </asp:RangeValidator>
                
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblpO2" runat="server"  Font-Size="Small" Text="pO2" Font-Bold="true"   />
                </td>
                <td   align="left">
                    <asp:RadioButtonList ID="rblpO2Units" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                        AutoPostBack="true" OnSelectedIndexChanged="rblpO2Units_SelectedIndexChanged"  RepeatLayout="Flow" >
                       
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLpO2UnitsDataSource" runat="server" 
                            DataFile="~/App_Data/pO2Units.xml" XPath="/*/*" ></asp:XmlDataSource>
                
                
                </td>
                <td align="left">
                    <asp:TextBox ID="txtpO2" runat="server" MaxLength="5" Width="25%"  ></asp:TextBox>
                
                    <asp:CompareValidator ID="cv_txtpO2" runat="server" Display="Dynamic" ControlToValidate="txtpO2" Type="Double" 
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are allowed." ValidationGroup="MainGroup" ></asp:CompareValidator>
                    <asp:RangeValidator ID="rv_txtpO2" runat="server" Display="Dynamic" ControlToValidate="txtpO2" Type="Double"
                        CssClass="Caution" MinimumValue="" MaximumValue="" ErrorMessage="Value should be between 7.35 and 7.45" ValidationGroup="MainGroup" >

                    </asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblUrea" runat="server"  Font-Size="Small" Text="Urea" Font-Bold="true"   />
                </td>
                <td   align="left">
                    <asp:RadioButtonList ID="rblUreaUnits" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                        AutoPostBack="true" OnSelectedIndexChanged="rblUreaUnits_SelectedIndexChanged"  RepeatLayout="Flow" >
                       
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLUreaUnitsDataSource" runat="server" 
                            DataFile="~/App_Data/UreaUnits.xml" XPath="/*/*" ></asp:XmlDataSource>
                


                </td>
                <td align="left">
                    <asp:TextBox ID="txtUrea" runat="server" MaxLength="5" Width="25%"  ></asp:TextBox>                
                
                    <asp:CompareValidator ID="cv_txtUrea" runat="server" Display="Dynamic" ControlToValidate="txtUrea" Type="Integer" 
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are not allowed." ValidationGroup="MainGroup" ></asp:CompareValidator>
                    <asp:RangeValidator ID="rv_txtUrea" runat="server" Display="Dynamic" ControlToValidate="txtUrea" Type="Double"
                        CssClass="Caution" MinimumValue="" MaximumValue="" ErrorMessage="Value should be between 7.35 and 7.45" ValidationGroup="MainGroup" >

                    </asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblCreatinine" runat="server"  Font-Size="Small" Text=" Creatinine" Font-Bold="true"   />
                </td>
                <td   align="left">
                    <asp:RadioButtonList ID="rblCreatinineUnits" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                        AutoPostBack="true" OnSelectedIndexChanged="rblCreatinineUnits_SelectedIndexChanged"   RepeatLayout="Flow">
                       
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XmlDataSource1" runat="server" 
                            DataFile="~/App_Data/CreatinineUnits.xml" XPath="/*/*" ></asp:XmlDataSource>
                

                
                </td>
                <td align="left">
                    <asp:TextBox ID="txtCreatinine" runat="server" MaxLength="5" Width="25%"  ></asp:TextBox>
                
                    <asp:CompareValidator ID="cv_txtCreatinine" runat="server" Display="Dynamic" ControlToValidate="txtCreatinine" Type="Integer" 
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are not allowed." ValidationGroup="MainGroup" ></asp:CompareValidator>

                    <asp:RangeValidator ID="rv_txtCreatinine" runat="server" Display="Dynamic" ControlToValidate="txtCreatinine" Type="Double"
                        CssClass="Caution" MinimumValue="" MaximumValue="" ErrorMessage="Value should be between 7.35 and 7.45" ValidationGroup="MainGroup" >

                    </asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblMeanCreatinine" runat="server"  Font-Size="Small" Text="Mean Creatinine" Font-Bold="false"   />
                </td>
                <td   align="left">
                    <asp:RadioButtonList ID="rblMeanCreatinineUnits" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                        AutoPostBack="true" OnSelectedIndexChanged="rblMeanCreatinineUnits_SelectedIndexChanged"   RepeatLayout="Flow">
                       
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLCreatinineUnitsDataSource" runat="server" 
                            DataFile="~/App_Data/CreatinineUnits.xml" XPath="/*/*" ></asp:XmlDataSource>
                

                
                </td>
                <td align="left">
                    <asp:TextBox ID="txtMeanCreatinine" runat="server" MaxLength="5" Width="25%"  ></asp:TextBox>
                
                    <asp:CompareValidator ID="cv_txtMeanCreatinine" runat="server" Display="Dynamic" ControlToValidate="txtMeanCreatinine" Type="Integer" 
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are not allowed." ValidationGroup="MainGroup" ></asp:CompareValidator>

                    <asp:RangeValidator ID="rv_txtMeanCreatinine" runat="server" Display="Dynamic" ControlToValidate="txtMeanCreatinine" Type="Double"
                        CssClass="Caution" MinimumValue="" MaximumValue="" ErrorMessage="Value should be between 7.35 and 7.45" ValidationGroup="MainGroup" >

                    </asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblMaxCreatinine" runat="server"  Font-Size="Small" Text="Max Creatinine" Font-Bold="false"   />
                </td>
                <td   align="left">
                    <asp:RadioButtonList ID="rblMaxCreatinineUnits" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                        AutoPostBack="true" OnSelectedIndexChanged="rblMaxCreatinineUnits_SelectedIndexChanged" RepeatLayout="Flow"  >
                       
                    </asp:RadioButtonList>
                
                </td>
                <td align="left">
                    <asp:TextBox ID="txtMaxCreatinine" runat="server" MaxLength="5" Width="25%" ></asp:TextBox>
                
                    <asp:CompareValidator ID="cv_txtMaxCreatinine" runat="server" Display="Dynamic" ControlToValidate="txtMaxCreatinine" Type="Integer" 
                        Operator="DataTypeCheck" CssClass="Caution" ErrorMessage="Please enter Numeric Values. Decimals are not allowed." ValidationGroup="MainGroup" ></asp:CompareValidator>
                    <asp:RangeValidator ID="rv_txtMaxCreatinine" runat="server" Display="Dynamic" ControlToValidate="txtMaxCreatinine" Type="Double"
                        CssClass="Caution" MinimumValue="" MaximumValue="" ErrorMessage="Value should be between 7.35 and 7.45" ValidationGroup="MainGroup" >

                    </asp:RangeValidator>
                </td>
            </tr>

            <tr>
                <td align="right">
                    <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="Comments"  />
                </td>
                <td align="left" colspan="2">
                    <asp:TextBox ID="txtComments" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" ></asp:TextBox>
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlAllDataAdded" Visible="false">
                <tr>
                    <td align="right">
                        <asp:label id="lblAllDataAdded" runat="server"  Font-Size="Small" Text="Tick Box if Data Entry Complete"  />
                    </td>
                    <td align="left"  colspan="2">
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
                    <td align="left"  colspan="2">
                        <asp:TextBox ID="txtReasonModified" runat="server" MaxLength="10000" Width="95%" TextMode="MultiLine" Font-Names="Arial" Font-Size="Small" 
                            onkeyup="AutoExpandTextBox(this, event)" Rows="2"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="lblReasonModifiedOld" runat="server"  Font-Size="Small" Text="Reason(s) Data Modified Earlier"  />
                    </td>
                    <td align="left"  colspan="2">
                        <asp:label id="lblReasonModifiedOldDetails" runat="server"  Font-Size="Small" Text="" BackColor="#99CCFF" Width="95%" />
                    </td>
                </tr>
            </asp:Panel> 
            <asp:Panel runat="server" ID="pnlFinal" Visible="false">
                <tr>
                    <td align="right">
                        <asp:label id="lblDataFinal" runat="server"  Font-Size="Small" Text="Tick to Mark Data as Final"  />
                    </td>
                    <td align="left" colspan="2">
                        <asp:CheckBox runat="server" ID="chkDataFinal" />
                    </td>
                </tr>
            </asp:Panel>
        </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table  cellpadding="2" cellspacing="2" border="0" style="width: 1020px">
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
            <td style="width:70%" align="left" colspan="2">
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px" CausesValidation="true" ValidationGroup="MainGroup"   UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
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
                        <asp:BoundField DataField="DonorLabResultsID" SortExpression="DonorLabResultsID" HeaderText="DonorLabResultsID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="TrialID" SortExpression="TrialID" HeaderText="TrialID" Visible="false"
                            DataNavigateUrlFields="TrialID, DonorLabResultsID" 
                            DataNavigateUrlFormatString="AddDonorLabResults.aspx?TID={0}&DonorLabResultsID={1}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="TrialID" SortExpression="TrialID" HeaderText="TrialID"    />
                        <asp:BoundField DataField="Hb" SortExpression="Hb" HeaderText="Hb"    />
                        <asp:BoundField DataField="Ht" SortExpression="Ht" HeaderText="Ht"    />
                        <asp:BoundField DataField="pH" SortExpression="pH" HeaderText="pH"    />
                        <asp:BoundField DataField="Urea" SortExpression="Urea" HeaderText="Urea"    />
                        <asp:BoundField DataField="Creatinine" SortExpression="Creatinine" HeaderText="Creatinine"    />
                        <asp:BoundField DataField="MeanCreatinine" SortExpression="MeanCreatinine" HeaderText="Mean Creatinine"    />
                        <asp:BoundField DataField="MaxCreatinine" SortExpression="MaxCreatinine" HeaderText="Max Creatinine" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

