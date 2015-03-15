<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalRecipientPage.master" AutoEventWireup="true" CodeFile="AddFUPostTransplant.aspx.cs" Inherits="SpecClinicalData_AddFUPostTransplant" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddFUPostTransplant" ContentPlaceHolderID="SpecimenContents" Runat="Server">
    <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 1020px">
        <tr>
            <th colspan="2">
                <asp:label id="lblDescription" runat="server" Font-Size="Small" Text="" />
            </th>
        </tr>
        <tr>
            <td style="width:35%" align="right">
                <asp:label id="lblOccasion" runat="server"  Font-Size="Small" Text="Occasion *" Font-Bold="true"  />
                
            </td>
            <td style="width:65%" align="left">
                <asp:DropDownList ID="ddOccasion" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddOccasion_SelectedIndexChanged"  >
                    <asp:ListItem Selected="True" Value="0" Text="Select Occasion"   /> 
                </asp:DropDownList>
                <asp:XmlDataSource ID="XMLFollowupsDataSource" runat="server" 
                        DataFile="~/App_Data/Followups.xml" XPath="/*/*" ></asp:XmlDataSource>
                
                
                <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
            </td>
        </tr>
        <asp:Panel ID="pnlOccasionSelected" runat="server" Visible="false">
        
            <tr>
                <td align="right">
                    <asp:label id="lblFollowUpDate" runat="server"  Font-Size="Small" Text="Follow-Up Date *" Font-Bold="true"  />
                </td>
                <td align="left">
                
                    <asp:TextBox ID="txtFollowUpDate" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfv_txtFollowUpDate" runat="server" Display="Dynamic" ErrorMessage="Please Enter Date"  
                        CssClass="MandatoryFieldMessage" ControlToValidate="txtFollowUpDate"   ValidationGroup="Secondary">

                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator runat="server" ID="cv_txtFollowUpDate" Display="Dynamic" ErrorMessage="Please Enter A Valid Date as DD/MM/YYYY" Operator="DataTypeCheck" Type="Date"  
                        ControlToValidate="txtFollowUpDate" ValidationGroup="Secondary" CssClass="MandatoryFieldMessage" />

                    <asp:RangeValidator ID="rv_txtFollowUpDate" runat="server" Display="Dynamic" ControlToValidate="txtFollowUpDate" Type="Date"
                        CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Secondary">

                    </asp:RangeValidator>
                    <asp:CalendarExtender ID="txtFollowUpDate_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFollowUpDate" PopupPosition="Right">
                    </asp:CalendarExtender>
                    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                
                </td>
            </tr>
            <tr>
                <td  align="right">
                    <asp:label id="lblGraftFailure" runat="server"  Font-Size="Small" Text="Graft Failure *" Font-Bold="true"  />
                </td>
                <td  align="left">
                    <asp:RadioButtonList ID="rblGraftFailure" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblGraftFailure_SelectedIndexChanged" RepeatLayout="Flow">
                       
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLMainOptionsDataSource" runat="server" 
                            DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
                    <asp:XmlDataSource ID="XMLMainOptionsYNDataSource" runat="server" 
                            DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
                    <asp:RequiredFieldValidator ID="rfv_rblGraftFailure" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" 
                        CssClass="MandatoryFieldMessage" ControlToValidate="rblGraftFailure"   ValidationGroup="Secondary">

                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <asp:Panel ID="pnlGraftFailure" runat="server">

        
                <tr>
                    <td align="right">
                        <asp:label id="lblDateGraftFailure" runat="server"  Font-Size="Small" Text="Date of Graft Failure"  />
                    </td>
                    <td  align="left">
                        <asp:TextBox ID="txtDateGraftFailure" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                        <asp:CalendarExtender ID="txtDateGraftFailure_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateGraftFailure">
                        </asp:CalendarExtender>
                
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="lblPrimaryCause" runat="server"  Font-Size="Small" Text="Primary Cause"  />
                    </td>
                    <td  align="left">
                        <asp:DropDownList ID="ddPrimaryCause" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true"  >
                            <asp:ListItem Selected="True" Value="0" Text="Select Occasion"   /> 
                        </asp:DropDownList>
                        <asp:XmlDataSource ID="XMLPrimaryCausesDataSource" runat="server" 
                                DataFile="~/App_Data/PrimaryCauses.xml" XPath="/*/*" ></asp:XmlDataSource>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                         <asp:label id="lblPrimaryCauseOther" runat="server"  Font-Size="Small" Text="Primary Cause (If Other)"  />
                    </td>
                    <td  align="left">
                        <asp:TextBox ID="txtPrimaryCauseOther" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                    </td>
                </tr>
            </asp:Panel>
            <tr>
                <td  align="right">
                    <asp:label id="lblGraftRemoval" runat="server"  Font-Size="Small" Text="Graft Removal *" Font-Bold="true"  />
                </td>
                <td  align="left">
                    <asp:RadioButtonList ID="rblGraftRemoval" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblGraftRemoval_SelectedIndexChanged" RepeatLayout="Flow" >
                       
                    </asp:RadioButtonList>
                    <asp:RequiredFieldValidator ID="rfv_rblGraftRemoval" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" 
                        CssClass="MandatoryFieldMessage" ControlToValidate="rblGraftRemoval"   ValidationGroup="Secondary">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlGraftRemoval" Visible="false">
                <tr>
                    <td align="right">
                        <asp:label id="lblDateGraftRemoval" runat="server"  Font-Size="Small" Text="Date of Graft Removal"  />
                    </td>
                    <td  align="left">
                        <asp:TextBox ID="txtDateGraftRemoval" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                        <asp:CalendarExtender ID="txtDateGraftRemoval_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateGraftRemoval">
                        </asp:CalendarExtender>
                
                    </td>
                </tr>
            </asp:Panel> 
            <tr>
            
                <td align="right">
                    <asp:label id="lblSerumCreatinine" runat="server"  Font-Size="Small" Text="Serum Creatinine" Font-Bold="true"  />
                </td>
                <td  align="left">
                    <asp:TextBox ID="txtSerumCreatinine" runat="server" MaxLength="10" Width="15%"></asp:TextBox>
                

                    <asp:RadioButtonList ID="rblCreatinineUnits" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" Width="30%" RepeatLayout="Flow"  >
                       
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLCreatinineUnitsDataSource" runat="server" 
                            DataFile="~/App_Data/CreatinineUnits.xml" XPath="/*/*" ></asp:XmlDataSource>
                
                </td>
            
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblUrineCreatinine" runat="server"  Font-Size="Small" Text="Urine Creatinine" Font-Bold="true"  />
                </td>
                <td  align="left">
                    <asp:TextBox ID="txtUrineCreatinine" runat="server" MaxLength="10" Width="15%"></asp:TextBox>
                    <asp:RadioButtonList ID="rblUrineCreatinineUnits" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow"  >
                       
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLUrineCreatinineUnitsDataSource" runat="server" 
                                        DataFile="~/App_Data/UreaUnits.xml" XPath="/*/*" ></asp:XmlDataSource>
                
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblCreatinineClearance" runat="server"  Font-Size="Small" Text="Creatinine Clearance" Font-Bold="true"  />
                </td>
                <td  align="left">
                    <asp:TextBox ID="txtCreatinineClearance" runat="server" MaxLength="10" Width="15%"></asp:TextBox>
                    <asp:RadioButtonList ID="rblCreatinineClearanceUnits" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow"  >
                       
                    </asp:RadioButtonList>
                    <asp:XmlDataSource ID="XMLCreatinineClearanceUnitsDataSource" runat="server" 
                                        DataFile="~/App_Data/CreatinineClearanceUnits.xml" XPath="/*/*" ></asp:XmlDataSource>
                
                </td>
            </tr>
            <tr>
                <td  align="right">
                    <asp:label id="lblCurrentlyDialysis" runat="server"  Font-Size="Small" Text="Currently on Dialysis" Font-Bold="true"  />
                </td>
                <td  align="left">
                    <asp:RadioButtonList ID="rblCurrentlyDialysis" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                        AutoPostBack="true" OnSelectedIndexChanged="rblCurrentlyDialysis_SelectedIndexChanged" >
                       
                    </asp:RadioButtonList>
                
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlCurrentlyDialysis" Visible="false">
                <tr>
                    <td  align="right">
                        <asp:label id="lblDialysisType" runat="server"  Font-Size="Small" Text="Dialysis Type" Font-Bold="true"  />
                    </td>
                    <td  align="left">
                        <asp:RadioButtonList ID="rblDialysisType" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" >
                       
                        </asp:RadioButtonList>
                        <asp:XmlDataSource ID="XMLDialysisTypesDataSource" runat="server" 
                                DataFile="~/App_Data/DialysisTypes.xml" XPath="/*/*" ></asp:XmlDataSource>
                    </td>
                </tr>
            </asp:Panel>
        
        
        
            <tr>
                <td align="right">
                    <asp:label id="lblDateLastDialysis" runat="server"  Font-Size="Small" Text="Last Dialysis Date"  />
                </td>
                <td  align="left">
                    <asp:TextBox ID="txtDateLastDialysis" runat="server" MaxLength="10" Width="15%"></asp:TextBox>
                    <asp:CalendarExtender ID="txtDateLastDialysis_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateLastDialysis">
                    </asp:CalendarExtender>
                </td>
            </tr>

            <tr>
                <td align="right">
                    <asp:label id="lblDialysisSessions" runat="server"  Font-Size="Small" Text="Number of Dialysis Sessions <br/> (Since Last Follow Up)"  />
                </td>
                <td  align="left">
                    <asp:TextBox ID="txtDialysisSessions" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                
                </td>
            </tr>
            
            <tr>
                <td align="right">
                    <asp:label id="lblPostTxImmunosuppressive" runat="server"  Font-Size="Small" Text="Post Tx Immunosuppressive"  />
                </td>
                <td align="left">
                    <asp:CheckBoxList ID="cblPostTxImmunosuppressive" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Vertical">
                    
                    </asp:CheckBoxList>
                    <asp:XmlDataSource ID="XMLPostTxImmunosuppressiveDataSource" runat="server" 
                            DataFile="~/App_Data/PostTxImmunosuppressives.xml" XPath="/*/*" ></asp:XmlDataSource>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblPostTxImmunosuppressiveOther" runat="server"  Font-Size="Small" Text="Post Tx Immunosuppressive (If Other)"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtPostTxImmunosuppressiveOther" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:label id="lblRejection" runat="server"  Font-Size="Small" Text="Rejection" Font-Bold="true"  />
                </td>
                <td  align="left">
                
                    <asp:DropDownList ID="ddRejection" runat="server" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value" 
                        OnSelectedIndexChanged="ddRejection_SelectedIndexChanged" AutoPostBack="true"   >
                        <asp:ListItem Selected="True" Value="0" Text="Select an Option"   /> 
                    </asp:DropDownList>
                   
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlRejection" Visible="false">
                <tr>
                    <td align="right">
                        <asp:label id="lblRejectionTreatmentsPostTx" runat="server"  Font-Size="Small" Text="Number of Rejection Treatments Post Transplant"  />
                    </td>
                    <td  align="left">
                        <asp:TextBox ID="txtRejectionTreatmentsPostTx" runat="server" MaxLength="10" Width="15%"></asp:TextBox>   
                    </td>
                </tr>
        
                <tr>
                    <td  align="right">
                        <asp:label id="lblPostTxPrednisolon" runat="server"  Font-Size="Small" Text="Post Tx Rejection Treated with Prednisolon?" Font-Bold="true"  />
                    </td>
                    <td  align="left">
                        <asp:RadioButtonList ID="rblPostTxPrednisolon" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                        </asp:RadioButtonList>
                
                    </td>
                </tr>
                <tr>
                    <td  align="right">
                        <asp:label id="lblPostTxOther" runat="server"  Font-Size="Small" Text="Post Tx Rejection Treated with Other Drug?" Font-Bold="true" />
                    </td>
                    <td  align="left">
                        <asp:RadioButtonList ID="rblPostTxOther" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" >
                    
                        </asp:RadioButtonList>
                
                    </td>
                </tr>
                <tr>
                    <td  align="right">
                        <asp:label id="lblPostTxOtherDetails" runat="server"  Font-Size="Small" Text="Post Tx Rejection Treatment (Other Drug)"  />
                    </td>
                    <td  align="left">
                        <asp:TextBox ID="txtPostTxOtherDetails" runat="server" MaxLength="100" Width="95%"></asp:TextBox>
                
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:label id="lblRejectionBiopsyProven" runat="server"  Font-Size="Small" Text="Rejection Biopsy Proven" Font-Bold="true" />
                    </td>
                    <td  align="left">
                        <asp:RadioButtonList ID="rblRejectionBiopsyProven" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                             >
                       
                        </asp:RadioButtonList>   
                    </td>
                </tr>

            </asp:Panel>
        

            <tr>
                <td align="right">
                    <asp:label id="lblCalcineurinInhibitorToxicity" runat="server"  Font-Size="Small" Text="Calcineurin Inhibitor Toxicity"  />
                </td>
                <td  align="left">
                    <asp:RadioButtonList ID="rblCalcineurinInhibitorToxicity" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                         >
                       
                    </asp:RadioButtonList>
                
                </td>
            </tr>       
        

            <tr>
                <td align="right">
                    <asp:label id="lblComplicationsGraftFunction" runat="server"  Font-Size="Small" Text="Complications Graft Function (Not Mentioned)"  />
                </td>
                <td  align="left">
                    <asp:TextBox ID="txtComplicationsGraftFunction" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" ></asp:TextBox>   
                </td>
            </tr>
            <asp:Panel runat="server" ID="pnlQOL" Visible="false">
                <tr>
                    <td align="center" colspan="2">
                        <table rules="all" border="4" cellpadding="2" cellspacing="2" style="width:99%">
                            <tr>
                                <th colspan="4" align="center">
                                    <asp:label id="lblEq5d5lqolScore" runat="server"  Font-Size="Small" Text="Quality of Life (EQ-5D-5L) Score"   />    
                                </th>
                            </tr>
                            <tr>
                                <td style="width:32%" align="right">
                                    <asp:label id="lblQOLFilledAt" runat="server"  Font-Size="Small" Text="Filled at?" Font-Bold="true" />
                                </td>
                                <td style="width:20%"  align="left">
                                    <asp:DropDownList ID="ddQOLFilledAt" runat="server" DataTextField="Text" DataValueField="Value"   AppendDataBoundItems="true"
                                          >
                                        <asp:ListItem Selected="True" Text="Select Filled At" Value="0" />
                       
                                    </asp:DropDownList>
                                    <asp:XmlDataSource ID="XMLQOLFilledOptionsDataSource" runat="server" 
                                        DataFile="~/App_Data/QOLFilledOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
                                </td>
                                <td style="width:28%"  align="right">
                                    
                                </td>
                                <td style="width:20%"  align="left">
                                    
                                </td>
                            </tr>

                            <tr>
                                <td  align="right">
                                    <asp:label id="lblMobility" runat="server"  Font-Size="Small" Text="Mobility" Font-Bold="true" />
                                </td>
                                <td   align="left">
                                    <asp:DropDownList ID="ddMobility" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                        <asp:ListItem Selected="True" Text="Select Mobility" Value="0" />
                                    </asp:DropDownList>
                                    <asp:XmlDataSource ID="XMLQOLScoresDataSource" runat="server" 
                                            DataFile="~/App_Data/QOLScores.xml" XPath="/*/*" ></asp:XmlDataSource>
                                    <asp:XmlDataSource ID="XMLMobilityDataSource" runat="server" 
                                        DataFile="~/App_Data/QOLScoresMobility.xml" XPath="/*/*" ></asp:XmlDataSource>
                                </td>
                                <td   align="right">
                                    <asp:label id="lblSelfCare" runat="server"  Font-Size="Small" Text="Self Care" Font-Bold="true"  />
                                </td>
                                <td   align="left">
                                    <asp:DropDownList ID="ddSelfCare" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                        <asp:ListItem Selected="True" Text="Select Self Care" Value="0" />
                                    </asp:DropDownList>
                                    <asp:XmlDataSource ID="XMLSelfCareDataSource" runat="server" 
                                    DataFile="~/App_Data/QOLScoresSelfCare.xml" XPath="/*/*" ></asp:XmlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td  align="right">
                                    <asp:label id="lblUsualActivities" runat="server"  Font-Size="Small" Text="Usual Activities" Font-Bold="true"  />
                                </td>
                                <td   align="left">
                                    <asp:DropDownList ID="ddUsualActivities" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                        <asp:ListItem Selected="True" Text="Select Usual Activities" Value="0" />
                                    </asp:DropDownList>
                                    <asp:XmlDataSource ID="XMLUsualActivitiesDataSource" runat="server" 
                                    DataFile="~/App_Data/QOLScoresUsualActivities.xml" XPath="/*/*" ></asp:XmlDataSource>
                                </td>
                                <td   align="right">
                                    <asp:label id="lblPainDiscomfort" runat="server"  Font-Size="Small" Text="Pain/Discomfort" Font-Bold="true"  />
                                </td>
                                <td   align="left">
                                    <asp:DropDownList ID="ddPainDiscomfort" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                        <asp:ListItem Selected="True" Text="Select Pain/Discomfort" Value="0" />
                                    </asp:DropDownList>
                                    <asp:XmlDataSource ID="XMLPainDiscomfortDataSource" runat="server" 
                                    DataFile="~/App_Data/QOLScoresPainDiscomfort.xml" XPath="/*/*" ></asp:XmlDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td  align="right">
                                    <asp:label id="lblAnxietyDepression" runat="server"  Font-Size="Small" Text="Anxiety Depression" Font-Bold="true"  />
                                </td>
                                <td   align="left">
                                    <asp:DropDownList ID="ddAnxietyDepression" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true" >
                                        <asp:ListItem Selected="True" Text="Select Anxiety/Depression" Value="0" />
                                    </asp:DropDownList>
                                    <asp:XmlDataSource ID="XMLAnxietyDepressionDataSource" runat="server" 
                                    DataFile="~/App_Data/QOLScoresAnxietyDepression.xml" XPath="/*/*" ></asp:XmlDataSource>
                                </td>
                                <td   align="right">
                                    <asp:label id="lblVASScore" runat="server"  Font-Size="Small" Text="VAS Score" Font-Bold="true"  />
                                </td>
                                <td   align="left">
                                    <asp:TextBox ID="txtVASScore" runat="server" MaxLength="3" Width="25%"></asp:TextBox>
                                    <asp:CompareValidator runat="server" ID="cv_txtVASScore" Display="Dynamic" ErrorMessage="Please Enter a numeric value. Decimals are not allowed." 
                                        Operator="DataTypeCheck" Type="Integer"  
                                        ControlToValidate="txtVASScore" ValidationGroup="Secondary" CssClass="MandatoryFieldMessage" />

                                    <asp:label id="lblVASScoreValues" runat="server"  Font-Size="Small" Text="0 to 100/ 999 if Missing" Font-Bold="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>    
            
            </asp:Panel>       
            <tr>
                <td align="right">
                    <asp:label id="lblComments" runat="server"  Font-Size="Small" Text="Comments"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtComments" runat="server" MaxLength="500" Width="95%" TextMode="MultiLine" ></asp:TextBox>
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


        </asp:Panel>
        <tr>
            <td align="right">
                <asp:HyperLink runat="server" ID="hlkSummaryPage" Text="Summary Page" />
                <asp:Button ID="cmdDelete" runat="server" Text="Delete" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdDelete_Click"   />
                <asp:ConfirmButtonExtender ID="cmdDelete_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdDelete">
                </asp:ConfirmButtonExtender>
                <asp:Button ID="cmdReset" runat="server" Text="Reset" Height="36px" Width="75px" CausesValidation="False" UseSubmitBehavior="False" OnClick="cmdReset_Click"  />
                <asp:ConfirmButtonExtender ID="cmdReset_ConfirmButtonExtender" runat="server" 
                    ConfirmText="" Enabled="True" TargetControlID="cmdReset">
                </asp:ConfirmButtonExtender>
            </td>
            <td align="left">
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px" CausesValidation="true"    UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
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
                        <asp:BoundField DataField="RFUPostTreatmentID" SortExpression="RFUPostTreatmentID" HeaderText="RFUPostTreatmentID" Visible="false"    />
                        <asp:HyperLinkField DataTextField="FollowUp_Date" SortExpression="FollowUpDate" HeaderText="Follow Up Date"
                            DataNavigateUrlFields="TrialID, TrialIDRecipient,RFUPostTreatmentID,Occasion" 
                            DataNavigateUrlFormatString="AddFUPostTransplant.aspx?TID={0}&TID_R={1}&RFUPostTreatmentID={2}&Occasion={3}">
                        </asp:HyperLinkField>
                        <asp:TemplateField HeaderText="Occasion" Visible="false" >
                            <ItemTemplate>
                                <asp:Label runat="server" id="lblOccasion"
                                    Text='<%#Bind("Occasion")%>'>
                                </asp:Label>                                
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="Occasion" SortExpression="Occasion" HeaderText="Occasion"    />
                        <asp:BoundField DataField="GraftFailure" SortExpression="GraftFailure" HeaderText="GrafFailure"    />
                        <asp:BoundField DataField="GraftRemoval" SortExpression="GraftRemoval" HeaderText="Graft Removal"    />
                        <asp:BoundField DataField="SerumCreatinine" SortExpression="SerumCreatinine" HeaderText="Serum Creatinine" />
                        <asp:BoundField DataField="EGFR" SortExpression="EGFR" HeaderText="eGFR" />
                        <asp:BoundField DataField="CreatinineClearance" SortExpression="CreatinineClearance" HeaderText="Creatinine Clearance" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

