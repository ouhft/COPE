<%@ Page Title="" Language="C#" MasterPageFile="~/SpecClinicalRecipientPage.master" AutoEventWireup="true" CodeFile="AddFU1to14.aspx.cs" Inherits="SpecClinicalData_AddFU1to14" MaintainScrollPositionOnPostBack = "false" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="conAddFU1to14" ContentPlaceHolderID="SpecimenContents" Runat="Server">
        <asp:Label runat="server" ID="lblUserMessages" CssClass="Caution" Width="100%" />
    <table rules="all" cellpadding="2" cellspacing="2" border="1" style="width: 851px">
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
                <asp:DropDownList ID="ddOccasion" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true"  >
                    <asp:ListItem Selected="True" Value="0" Text="Select Occasion"   /> 
                </asp:DropDownList>
                <asp:XmlDataSource ID="XMLFollowUpFirstDataSource" runat="server" 
                        DataFile="~/App_Data/FollowUpFirst.xml" XPath="/*/*" ></asp:XmlDataSource>
                
                <asp:RequiredFieldValidator ID="rfv_ddOccasion" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" InitialValue="0"  
                    CssClass="MandatoryFieldMessage" ControlToValidate="ddOccasion"   ValidationGroup="Main">

                </asp:RequiredFieldValidator>
                <asp:CheckBox runat="server" ID="chkDataLocked" Visible="false" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:label id="lblFollowUpDate" runat="server"  Font-Size="Small" Text="Follow-Up Date (Day 1) *" Font-Bold="true"  />
            </td>
            <td align="left">
                <asp:TextBox ID="txtFollowUpDate" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfv_txtFollowUpDate" runat="server" Display="Dynamic" ErrorMessage="Please Enter Date"  
                    CssClass="MandatoryFieldMessage" ControlToValidate="txtFollowUpDate"   ValidationGroup="Main">

                </asp:RequiredFieldValidator>
                <asp:CompareValidator runat="server" ID="cv_txtFollowUpDate" Display="Dynamic" ErrorMessage="Please Enter A Valid Date as DD/MM/YYYY" Operator="DataTypeCheck" Type="Date"  
                    ControlToValidate="txtFollowUpDate" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

                <asp:RangeValidator ID="rv_txtFollowUpDate" runat="server" Display="Dynamic" ControlToValidate="txtFollowUpDate" Type="Date"
                    CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main">

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
                <asp:RadioButtonList ID="rblGraftFailure" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow" 
                    OnSelectedIndexChanged="rblGraftFailure_SelectedIndexChanged" AutoPostBack="True" >
                       
                </asp:RadioButtonList>
                <asp:XmlDataSource ID="XMLMainOptionsYNDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptionsYN.xml" XPath="/*/*" ></asp:XmlDataSource>
                <asp:XmlDataSource ID="XMLMainOptionsDataSource" runat="server" 
                        DataFile="~/App_Data/MainOptions.xml" XPath="/*/*" ></asp:XmlDataSource>
                <asp:RequiredFieldValidator ID="rfv_rblGraftFailure" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" 
                    CssClass="MandatoryFieldMessage" ControlToValidate="rblGraftFailure"   ValidationGroup="Main">

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
                    <asp:CalendarExtender ID="txtDateGraftFailure_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateGraftFailure" PopupPosition="Right">
                    </asp:CalendarExtender>

                    <asp:CompareValidator runat="server" ID="cv_txtDateGraftFailure" Display="Dynamic" ErrorMessage="Please Enter A Valid Date as DD/MM/YYYY" Operator="DataTypeCheck" Type="Date"  
                        ControlToValidate="txtDateGraftFailure" ValidationGroup="GraftFailureYes" CssClass="MandatoryFieldMessage" />

                    <asp:CompareValidator runat="server" ID="cv2_txtDateGraftFailure" Display="Dynamic" ErrorMessage="Should be later than or same as Follow Up Date (Day 1)"  ControlToValidate="txtDateGraftFailure"
                         ControlToCompare="txtFollowUpDate" Operator="GreaterThanEqual"
                         Type="Date"  
                        ValidationGroup="GraftFailureYes" CssClass="MandatoryFieldMessage" />
                
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
                <asp:RadioButtonList ID="rblGraftRemoval" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rblGraftRemoval_SelectedIndexChanged" >
                       
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="rfv_rblGraftRemoval" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" 
                    CssClass="MandatoryFieldMessage" ControlToValidate="rblGraftRemoval"   ValidationGroup="Main">

                

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
                    <asp:CalendarExtender ID="txtDateGraftRemoval_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDateGraftRemoval" PopupPosition="Right">
                    </asp:CalendarExtender>

                    <asp:CompareValidator runat="server" ID="cv_txtDateGraftRemoval" Display="Dynamic" ErrorMessage="Please Enter A Valid Date as DD/MM/YYYY" Operator="DataTypeCheck" Type="Date"  
                        ControlToValidate="txtDateGraftRemoval" ValidationGroup="GraftRemovalYes" CssClass="MandatoryFieldMessage" />

                    <asp:CompareValidator runat="server" ID="cv2_txtDateGraftRemoval" Display="Dynamic" ErrorMessage="Should be later than or same as Follow Up Date (Day 1)"  ControlToValidate="txtDateGraftRemoval"
                        ControlToCompare="txtFollowUpDate" Operator="GreaterThanEqual"
                        Type="Date"  
                        ValidationGroup="GraftRemovalYes" CssClass="MandatoryFieldMessage" />
                
                </td>
            </tr>
        </asp:Panel>
        <tr>
            <td align="center" colspan="2">
                <table rules="all" border="4" cellpadding="2" cellspacing="2" style="width:98%">
                    <tr>
                        <th colspan="4" align="center" style="width:65%; border-right:0">
                            <asp:label id="lblSerumCreatinineheaded" runat="server"  Font-Size="Small" Text="Serum Creatinine (Admission; Days 1-7) - µmol/l or mg/dL"  />
                            
                            
                        </th>
                        
                    </tr>
                    <tr>
                        <td style="width:25%" align="right">
                            <asp:label id="lblCreatinineUnit" runat="server"  Font-Size="Small" Text="Serum Creatinine Unit *" Font-Bold="true"  />
                        </td>
                        <td style="width:75%"  align="left" colspan="3">
                            <asp:RadioButtonList ID="rblCreatinineUnits" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" 
                                RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rblCreatinineUnits_SelectedIndexChanged" >
                       
                            </asp:RadioButtonList>
                            <asp:XmlDataSource ID="XMLCreatinineUnitsDataSource" runat="server" 
                                    DataFile="~/App_Data/CreatinineUnits.xml" XPath="/*/*" ></asp:XmlDataSource>

                            <asp:RequiredFieldValidator ID="rfv_rblCreatinineUnits" runat="server" Display="Dynamic" ErrorMessage="Please Select an Option" 
                                    CssClass="MandatoryFieldMessage" ControlToValidate="rblCreatinineUnits"   ValidationGroup="Main">
                            </asp:RequiredFieldValidator>

                        </td>
                        

                    </tr>
                    <tr>
                        <td style="width:25%" align="right">
                            <asp:label id="lblSerumCreatinineAdmission" runat="server"  Font-Size="Small" Text="Serum Creatinine (Admission)" Font-Bold="false" />
                        </td>
                        <td style="width:25%"  align="left">
                            <asp:TextBox ID="txtSerumCreatinineAdmission" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                            <asp:CompareValidator runat="server" ID="cv_txtSerumCreatinineAdmission" Display="Dynamic" ErrorMessage="Please Enter a numeric value. Decimals are allowed." 
                                Operator="DataTypeCheck" Type="Double"  
                                ControlToValidate="txtSerumCreatinineAdmission" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

                            <asp:RangeValidator ID="rv_txtSerumCreatinineAdmission" runat="server" Display="Dynamic" ControlToValidate="txtSerumCreatinineAdmission" Type="Double"
                                CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main" />
                        </td>
                        <td style="width:25%"  align="right">
                            <asp:label id="lblSerumCreatinine4" runat="server"  Font-Size="Small" Text="Serum Creatinine (Day 4)" Font-Bold="false" />
                        </td>
                        <td style="width:25%"  align="left">
                            <asp:TextBox ID="txtSerumCreatinine4" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                            <asp:CompareValidator runat="server" ID="cv_txtSerumCreatinine4" Display="Dynamic" ErrorMessage="Please Enter a numeric value. Decimals are allowed." 
                                Operator="DataTypeCheck" Type="Double"  
                                ControlToValidate="txtSerumCreatinine4" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

                            <asp:RangeValidator ID="rv_txtSerumCreatinine4" runat="server" Display="Dynamic" ControlToValidate="txtSerumCreatinine4" Type="Double"
                                CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main" />
                        </td>
                    </tr>
                    <tr>
                        <td  align="right">
                            <asp:label id="lblSerumCreatinine1" runat="server"  Font-Size="Small" Text="Serum Creatinine (Day 1)" Font-Bold="false" />
                        </td>
                        <td   align="left">
                            <asp:TextBox ID="txtSerumCreatinine1" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                            <asp:CompareValidator runat="server" ID="cv_txtSerumCreatinine1" Display="Dynamic" ErrorMessage="Please Enter a numeric value. Decimals are allowed." 
                                Operator="DataTypeCheck" Type="Double"  
                                ControlToValidate="txtSerumCreatinine1" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

                            <asp:RangeValidator ID="rv_txtSerumCreatinine1" runat="server" Display="Dynamic" ControlToValidate="txtSerumCreatinine1" Type="Double"
                                CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main" />
                        </td>
                        <td   align="right">
                            <asp:label id="lblSerumCreatinine5" runat="server"  Font-Size="Small" Text="Serum Creatinine (Day 5)" Font-Bold="false" />
                        </td>
                        <td   align="left">
                            <asp:TextBox ID="txtSerumCreatinine5" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                            <asp:CompareValidator runat="server" ID="cv_txtSerumCreatinine5" Display="Dynamic" ErrorMessage="Please Enter a numeric value. Decimals allowed." 
                                Operator="DataTypeCheck" Type="Double"  
                                ControlToValidate="txtSerumCreatinine5" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

                            <asp:RangeValidator ID="rv_txtSerumCreatinine5" runat="server" Display="Dynamic" ControlToValidate="txtSerumCreatinine5" Type="Double"
                                CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblSerumCreatinine2" runat="server"  Font-Size="Small" Text="Serum Creatinine (Day 2)" Font-Bold="false" />
                        </td>
                        <td  align="left">
                            <asp:TextBox ID="txtSerumCreatinine2" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                            <asp:CompareValidator runat="server" ID="cv_txtSerumCreatinine2" Display="Dynamic" ErrorMessage="Please Enter a numeric value. Decimals are allowed." 
                                Operator="DataTypeCheck" Type="Double"  
                                ControlToValidate="txtSerumCreatinine2" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

                            <asp:RangeValidator ID="rv_txtSerumCreatinine2" runat="server" Display="Dynamic" ControlToValidate="txtSerumCreatinine2" Type="Double"
                                CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main" />
                        </td>
                        <td  align="right">
                            <asp:label id="lblSerumCreatinine6" runat="server"  Font-Size="Small" Text="Serum Creatinine (Day 6)" Font-Bold="false" />
                        </td>
                        <td  align="left">
                            <asp:TextBox ID="txtSerumCreatinine6" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                            <asp:CompareValidator runat="server" ID="cv_txtSerumCreatinine6" Display="Dynamic" ErrorMessage="Please Enter a numeric value. Decimals are allowed." 
                                Operator="DataTypeCheck" Type="Double"  
                                ControlToValidate="txtSerumCreatinine6" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

                            <asp:RangeValidator ID="rv_txtSerumCreatinine6" runat="server" Display="Dynamic" ControlToValidate="txtSerumCreatinine6" Type="Double"
                                CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:label id="lblSerumCreatinine3" runat="server"  Font-Size="Small" Text="Serum Creatinine (Day 3)" Font-Bold="false" />
                        </td>
                        <td  align="left">
                            <asp:TextBox ID="txtSerumCreatinine3" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                            <asp:CompareValidator runat="server" ID="cv_txtSerumCreatinine3" Display="Dynamic" ErrorMessage="Please Enter a numeric value. Decimals are allowed." 
                                Operator="DataTypeCheck" Type="Double"  
                                ControlToValidate="txtSerumCreatinine3" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

                            <asp:RangeValidator ID="rv_txtSerumCreatinine3" runat="server" Display="Dynamic" ControlToValidate="txtSerumCreatinine3" Type="Double"
                                CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main" />
                        </td>
                        <td  align="right">
                            <asp:label id="lblSerumCreatinine7" runat="server"  Font-Size="Small" Text="Serum Creatinine (Day 7)" Font-Bold="false" />
                        </td>
                        <td  align="left">
                            <asp:TextBox ID="txtSerumCreatinine7" runat="server" MaxLength="10" Width="25%"></asp:TextBox>
                            <asp:CompareValidator runat="server" ID="cv_txtSerumCreatinine7" Display="Dynamic" ErrorMessage="Please Enter a numeric value. Decimals are allowed." 
                                Operator="DataTypeCheck" Type="Double"  
                                ControlToValidate="txtSerumCreatinine7" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

                            <asp:RangeValidator ID="rv_txtSerumCreatinine7" runat="server" Display="Dynamic" ControlToValidate="txtSerumCreatinine7" Type="Double"
                                CssClass="MandatoryFieldMessage" ErrorMessage="" ValidationGroup="Main" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        
        <tr>
            <td align="right">
                <asp:label id="lblDialysisRequirementInitial" runat="server"  Font-Size="Small" Text="Dialysis Requirement *" Font-Bold="true"  />
            </td>
            <td  align="left">
                <asp:CheckBoxList ID="cblDialysisRequirementInitial" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal">
                    
                </asp:CheckBoxList>
                <asp:XmlDataSource ID="XMLDialysisDaysDataSource" runat="server" 
                        DataFile="~/App_Data/DialysisDays.xml" XPath="/*/*" ></asp:XmlDataSource>    
                
                
            </td>
        </tr>
        
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
        <tr>
            <td align="right">
                <asp:label id="lblRequiredHyperkalemia" runat="server"  Font-Size="Small" Text="Required for Hyperkalemia (If Only 1 Dialysis)"  />
            </td>
            <td  align="left">
                <asp:RadioButtonList ID="rblRequiredHyperkalemia" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Horizontal" >
                       
                </asp:RadioButtonList>
                
            </td>
        </tr>
        <tr>
            <td  align="right">
                <asp:label id="lblInductionTherapy" runat="server"  Font-Size="Small" Text="Induction Therapy"  />
            </td>
            <td  align="left">
                <asp:DropDownList ID="ddInductionTherapy" runat="server" DataTextField="Text" DataValueField="Value"  AppendDataBoundItems="true" >
                        <asp:ListItem Selected="True" Text="Select an Option" Value="0" />
                </asp:DropDownList>
                <asp:XmlDataSource ID="XMLInductionTherapiesDataSource" runat="server" 
                        DataFile="~/App_Data/InductionTherapies.xml" XPath="/*/*" ></asp:XmlDataSource>
            </td>
        </tr>
        
        
        
        <asp:Panel runat="server" ID="pnleGFR" Visible="false" >
            <tr>
                <td align="right">
                    <asp:label id="lblEGFR" runat="server"  Font-Size="Small" Text="eGFR (Derived Value)"  />
                </td>
                <td align="left">
                    <asp:TextBox ID="txtEGFR" runat="server" MaxLength="100" Width="25%" Enabled="false"></asp:TextBox>
                </td>
            </tr>
        </asp:Panel>
        

        <tr>
            <td align="right">
                <asp:label id="lblPostTxImmunosuppressive" runat="server"  Font-Size="Small" Text="Post Tx Immunosuppressive <br/> (Day 7)"  />
            </td>
            <td align="left">
                <asp:CheckBoxList ID="cblPostTxImmunosuppressive" runat="server" DataTextField="Text" DataValueField="Value" RepeatDirection="Vertical">
                    
                </asp:CheckBoxList>
                <asp:XmlDataSource ID="XMLPostTxImmunosuppressiveDataSource" runat="server" 
                        DataFile="~/App_Data/PostTxImmunosuppressivesDay1to7.xml" XPath="/*/*" ></asp:XmlDataSource>
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
                <td  align="right">
                    <asp:label id="lblPostTxPrednisolon" runat="server"  Font-Size="Small" Text="Post Tx Rejection Treated with Prednisolon?" Font-Bold="true" />
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
                    <asp:label id="lblRejectionBiopsyProven" runat="server"  Font-Size="Small" Text="Rejection Biopsy Proven" Font-Bold="true"  />
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
                <asp:label id="lblDatePrimaryPostTxDischarge" runat="server"  Font-Size="Small" Text="Date of Primary Post Tx Discharge"  />
            </td>
            <td  align="left">
                <asp:TextBox ID="txtDatePrimaryPostTxDischarge" runat="server" MaxLength="10" Width="25%"></asp:TextBox> 
                <asp:CalendarExtender ID="txtDatePrimaryPostTxDischarge_CalendarExtender" runat="server" Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtDatePrimaryPostTxDischarge" PopupPosition="Right">
                </asp:CalendarExtender>

                <asp:CompareValidator runat="server" ID="cv_txtDatePrimaryPostTxDischarge" Display="Dynamic" ErrorMessage="Please Enter A Valid Date as DD/MM/YYYY." Operator="DataTypeCheck" Type="Date"  
                        ControlToValidate="txtDatePrimaryPostTxDischarge" ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

                <asp:CompareValidator runat="server" ID="cv2_txtDatePrimaryPostTxDischarge" Display="Dynamic" ErrorMessage="Should be later than or same as Follow Up Date (Day 1)."  ControlToValidate="txtDatePrimaryPostTxDischarge"
                        ControlToCompare="txtFollowUpDate" Operator="GreaterThanEqual"
                        Type="Date"  
                    ValidationGroup="Main" CssClass="MandatoryFieldMessage" />

            </td>
        </tr>
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
                <asp:Button ID="cmdAddData" runat="server" Text="Submit" Height="36px" CausesValidation="true" ValidationGroup="Main"   
                    UseSubmitBehavior="False" OnClick="cmdAddData_Click"   />
                
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
                        <asp:HyperLinkField DataTextField="FollowUp_Date" SortExpression="FollowUpDate" HeaderText="FollowUp Date" Visible="false"
                            DataNavigateUrlFields="TrialID, TrialIDRecipient, RFUPostTreatmentID" 
                            DataNavigateUrlFormatString="AddFU1to14.aspx?TID={0}&TID_R={1}&RFUPostTreatmentID={2}">
                        </asp:HyperLinkField>
                        <asp:BoundField DataField="Occasion" SortExpression="Occasion" HeaderText="Occasion"    />
                        <asp:BoundField DataField="FollowUp_Date" SortExpression="FollowUpDate" HeaderText="Follow Up Date"    />
                        <asp:BoundField DataField="GraftFailure" SortExpression="GraftFailure" HeaderText="Graft Failure"    />
                        <asp:BoundField DataField="GraftRemoval" SortExpression="GraftRemoval" HeaderText="Date Removal"    />
                        <asp:BoundField DataField="SerumCreatinine" SortExpression="SerumCreatinine" HeaderText="Serum Creatinine" />
                       
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ProviderName="MySql.Data.MySqlClient" ConnectionString="<%$ ConnectionStrings:cope4dbconn %>"   />
                
            </td>
        </tr>
    </table>
</asp:Content>

