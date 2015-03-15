using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Configuration;

using MySql.Data;
using MySql.Data.MySqlClient;

public partial class SpecClinicalData_AddResUse : System.Web.UI.Page
{
    #region " Private Constants & Variables "

    private const string STRCONN = "cope4dbconn";

        private const string strEditRedirect = "~/SpecClinicalData/AddResUse.aspx?TID=";

        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_DD_UNKNOWN_SELECTION = "0";
        private const string STR_YES_SELECTION = "YES";
        private const string STR_NO_SELECTION = "NO";

        //private const int intMinDaysHospital = 0;    
        //private const int intMaxDaysHospital = 100;

        private const string strMainCPH = "cplMainContents";
        private const string strSpecimenCPH = "SpecimenContents";

        private const string strPageBaseUrl = "~/SpecClinicalData/AddResUse.aspx?TID=";

        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummaryRecipient.aspx?TID=";

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Request.QueryString["TID_R"]))
                {
                    throw new Exception("Could not obtain the TrialID (Recipient).");
                }

                

                lblDescription.Text = "Add Resource Use Log for " + Request.QueryString["TID_R"].ToString();



                if (string.IsNullOrEmpty(Request.QueryString["ResUseID"]) == false || string.IsNullOrEmpty(Request.QueryString["Occasion"])==false)
                {
                    AssignData();
                }
                else
                {
                    ddOccasion_SelectedIndexChanged(this, EventArgs.Empty);
                }

                //string strOccasionMonth6 = "Month 6";
                //li = new ListItem();
                //li.Text = strOccasionMonth6;
                //li.Value = strOccasionMonth6;
                //ddOccasion.Items.Add(li);

                ddOccasion.DataSource = XMLResUsesDataSource;
                ddOccasion.DataBind();


                if (!string.IsNullOrEmpty(Request.QueryString["Occasion"]))
                {
                    ListItem liItem = ddOccasion.Items.FindByText(Request.QueryString["Occasion"]);
                    if (liItem != null)
                    {
                        ddOccasion.SelectedValue = Request.QueryString["Occasion"];
                    }

                }
                


                lblValuesGPAppointment.Text += "(";
                lblValuesGPAppointment.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesGPAppointment.Text += "-";
                lblValuesGPAppointment.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesGPAppointment.Text += ")";


                lblValuesGPHomeVisit.Text += "(";
                lblValuesGPHomeVisit.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesGPHomeVisit.Text += "-";
                lblValuesGPHomeVisit.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesGPHomeVisit.Text += ")";

                lblValuesGPTelConversation.Text += "(";
                lblValuesGPTelConversation.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesGPTelConversation.Text += "-";
                lblValuesGPTelConversation.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesGPTelConversation.Text += ")";


                lblValuesSpecConsultantAppointment.Text += "(";
                lblValuesSpecConsultantAppointment.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesSpecConsultantAppointment.Text += "-";
                lblValuesSpecConsultantAppointment.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesSpecConsultantAppointment.Text += ")";

                lblValuesAETreatment.Text += "(";
                lblValuesAETreatment.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesAETreatment.Text += "-";
                lblValuesAETreatment.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesAETreatment.Text += ")";

                lblValuesAmbulanceAEVisit.Text += "(";
                lblValuesAmbulanceAEVisit.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesAmbulanceAEVisit.Text += "-";
                lblValuesAmbulanceAEVisit.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesAmbulanceAEVisit.Text += ")";

                lblValuesNurseHomeVisit.Text += "(";
                lblValuesNurseHomeVisit.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesNurseHomeVisit.Text += "-";
                lblValuesNurseHomeVisit.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesNurseHomeVisit.Text += ")";

                lblValuesNursePracticeAppointment.Text += "(";
                lblValuesNursePracticeAppointment.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesNursePracticeAppointment.Text += "-";
                lblValuesNursePracticeAppointment.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesNursePracticeAppointment.Text += ")";

                lblValuesPhysiotherapistAppointment.Text += "(";
                lblValuesPhysiotherapistAppointment.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesPhysiotherapistAppointment.Text += "-";
                lblValuesPhysiotherapistAppointment.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesPhysiotherapistAppointment.Text += ")";

                lblValuesOccupationalTherapistAppointment.Text += "(";
                lblValuesOccupationalTherapistAppointment.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesOccupationalTherapistAppointment.Text += "-";
                lblValuesOccupationalTherapistAppointment.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesOccupationalTherapistAppointment.Text += ")";

                lblValuesPsychologistAppointment.Text += "(";
                lblValuesPsychologistAppointment.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesPsychologistAppointment.Text += "-";
                lblValuesPsychologistAppointment.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesPsychologistAppointment.Text += ")";

                lblValuesCounsellorAppointment.Text += "(";
                lblValuesCounsellorAppointment.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesCounsellorAppointment.Text += "-";
                lblValuesCounsellorAppointment.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesCounsellorAppointment.Text += ")";

                lblValuesAttendedDayHospital.Text += "(";
                lblValuesAttendedDayHospital.Text += ConstantsGeneral.intMinResUse.ToString();
                lblValuesAttendedDayHospital.Text += "-";
                lblValuesAttendedDayHospital.Text += ConstantsGeneral.intMaxResUse.ToString();
                lblValuesAttendedDayHospital.Text += ")";

                //if (! string.IsNullOrEmpty(Request.QueryString["EventCode"]))
                //{
                //    if (Request.QueryString["EventCode"]=="65")
                //    {
                //        ddOccasion.SelectedValue = "Day 30";
                //    }
                //    if (Request.QueryString["EventCode"] == "66")
                //    {
                //        ddOccasion.SelectedValue = "Month 6";
                //    }
                //}
                //if occasion has been selected redirect to the relaevant page
                

                //ddOccasion_SelectedIndexChanged(this, EventArgs.Empty);

                ViewState["SortField"] = "Occasion";
                ViewState["SortDirection"] = "ASC";

                BindData();


                //ddReAdmissionType.DataSource = XMLReAdmissionTypesDataSource;
                //ddReAdmissionType.DataBind();


                //txtDateAdmission_CalendarExtender.EndDate = DateTime.Today;
                //txtDateDischarge_CalendarExtender.EndDate = DateTime.Today;


                //rblRequiredSurgery.DataSource = XMLMainOptionsDataSource;
                //rblRequiredSurgery.DataBind();
                //rblRequiredSurgery.SelectedValue = STR_UNKNOWN_SELECTION;

                //rblITU_HDU.DataSource = XMLMainOptionsDataSource;
                //rblITU_HDU.DataBind();
                //rblITU_HDU.SelectedValue = STR_UNKNOWN_SELECTION;

                //ViewState["SortFieldGV2"] = "DateAdmission";
                //ViewState["SortDirectionGV2"] = "ASC";

                //BindDataGV2();

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any 'Resource Use'/'Readmissions' data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK selected 'Resource Use' data will be deleted.";
                //cmdResetReadmissions_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any 'Resource Use'/'Readmission' data entered will be lost.";

                //lock data
                pnlReasonModified.Visible = false;
                txtReasonModified.ToolTip = "Enter Reasons if you are Modifying any Data on this page";
                lblReasonModifiedOldDetails.ToolTip = "Displays Reasons that have been entered for modifying data in the past.";
                if (chkDataLocked.Checked == true || chkDataFinal.Checked == true)
                {

                    if (chkDataLocked.Checked == true && chkDataFinal.Checked == true)
                    {
                        string strDataLockedFinalMessage = ConfigurationManager.AppSettings["LockedFinalMessage"];
                        lblDescription.Text += "<br/>" + strDataLockedFinalMessage;
                    }
                    else
                    {
                        if (chkDataFinal.Checked == true)
                        {
                            string strDataFinalMessage = ConfigurationManager.AppSettings["FinalMessage"];
                            lblDescription.Text += "<br/>" + strDataFinalMessage;
                        }
                        else
                        {
                            string strDataLocked = ConfigurationManager.AppSettings["LockedMessage"];
                            lblDescription.Text += "<br/>" + strDataLocked;
                        }
                    }


                    if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
                    {
                        if (chkDataLocked.Checked == true)
                        {
                            pnlReasonModified.Visible = true;
                        }
                        else
                        {
                            pnlReasonModified.Visible = false;
                        }
                        pnlFinal.Visible = true;
                        cmdAddData.Enabled = true;
                        cmdDelete.Enabled = true;
                        //cmdReset.Enabled = true;


                    }
                    else
                    {
                        //chkDataLocked.Checked = false;
                        pnlReasonModified.Visible = false;
                        pnlFinal.Visible = false;
                        cmdAddData.Enabled = false;
                        cmdDelete.Enabled = false;
                        //cmdReset.Enabled = false;

                    }
                }

                //data final checkbox visible
                if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
                {
                    pnlFinal.Visible = true;
                    //cmdAddData.Enabled = true;
                    //cmdDelete.Enabled = true;
                    //cmdReset.Enabled = true;
                }
                else
                {
                    pnlFinal.Visible = false;
                    //cmdAddData.Enabled = false;
                    //cmdDelete.Enabled = false;
                    //cmdReset.Enabled = false;
                }

                if (!String.IsNullOrEmpty(Request.QueryString["SCode"]))
                {
                    if (Request.QueryString["SCode"].ToString() == "1")
                    {
                        string strSCode1Message = string.Empty;
                        strSCode1Message = ConfigurationManager.AppSettings["SCode1"];
                        lblUserMessages.Text = strSCode1Message;
                    }
                }

                hlkSummaryPage.NavigateUrl = strRedirectSummary + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"];

                string strMandatoryMessage = string.Empty;
                strMandatoryMessage = ConfigurationManager.AppSettings["MandatoryMessage"];
                lblDescription.Text += "<br/><span class='FontMandatory'>" + strMandatoryMessage + "</span>";
                //lblDescriptionReadmissions.Text += "<br/><span class='FontMandatory'>" + strMandatoryMessage + "</span>";

                //locked button
                if (chkDataLocked.Checked == false)
                {
                    pnlAllDataAdded.Visible = true;
                }
                else
                {
                    pnlAllDataAdded.Visible = false;
                }

                MaintainScrollPositionOnPostBack = true;
            }
        }
        catch (System.Exception ex)
        {
            lblDescription.Text = ex.Message + " An error occured while loading the page.";
        }
    }
    protected void BindData()
    {
        try
        {
            if (Request.QueryString["TID_R"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID (Recipient).");
            }
            string strSQL = String.Empty;

            strSQL += "SELECT t1.*, t2.TrialID, t3.RecipientID,  ";
            strSQL += "DATE_FORMAT(t1.DateEntered, '%d/%m/%Y') Date_AE, ";
            strSQL += "DATE_FORMAT(t1.DateCreated, '%d/%m/%Y %H:%i') Date_Created, ";
            strSQL += "DATE_FORMAT(t3.DateOfBirth, '%d/%m/%Y') Recipient_DateOfBirth ";
            strSQL += "FROM resuse t1 ";
            strSQL += "INNER JOIN trialdetails_recipient t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            strSQL += "LEFT JOIN r_identification t3 ON t1.TrialIDRecipient=t3.TrialIDRecipient ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV1.DataBind();

            if (GV1.Rows.Count > 0)
            {
                lblGV1.Text = "Click on TrialID (Recipient) to Edit 'Resource Use Log' Data.";
            }
            else
            {
                lblGV1.Text = "'Resource Use Log' Data has not been added.";
            }


            //loop though rows to highlight selected occasion
            Label lbl;

            foreach (GridViewRow row in GV1.Rows)
            {
                lbl = (Label)row.FindControl("lblOccasion");

                if (lbl != null)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        if (String.IsNullOrEmpty(Request.QueryString["Occasion"]) == false)
                        {
                            if (lbl.Text == Request.QueryString["Occasion"])
                            {
                                row.BackColor = System.Drawing.Color.LightBlue;
                            }

                        }
                    }
                }
            }

        }
        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured while binding the page.";
        }

        //lblUserMessages.Text = strSQL;
    }

    //sorting main datagrid
    protected void GV1_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression.ToString() == ViewState["SortField"].ToString())
        {
            switch (ViewState["SortDirection"].ToString())
            {
                case "ASC":
                    ViewState["SortDirection"] = "DESC";
                    break;
                case "DESC":
                    ViewState["SortDirection"] = "ASC";
                    break;
            }

        }
        else
        {
            ViewState["SortField"] = e.SortExpression;
            ViewState["SortDirection"] = "DESC";
        }
        BindData();
        //BindDataGV2();
    }

    protected void GV1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.DataItem != null)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)(e.Row.DataItem);
                if (String.IsNullOrEmpty(Request.QueryString["ResUseID"]) == false)
                {
                    {
                        if (drv["ResUseID"].ToString() == Request.QueryString["ResUseID"].ToString())
                        {
                            e.Row.BackColor = System.Drawing.Color.LightBlue;
                        }
                    }
                }
            }
        }
    }

    protected void cmdReset_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(Request.Url.AbsoluteUri, false);

        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
        }
    }

    protected void AssignData()
    {
        try
        {
            string STRSQL = "SELECT t1.* FROM  resuse t1 ";
            STRSQL += "INNER JOIN trialdetails_recipient t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            STRSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            if (string.IsNullOrEmpty(Request.QueryString["ResUseID"]) == false)
            {
                STRSQL += "AND ResUseID=?ResUseID ";
            }
            if (string.IsNullOrEmpty(Request.QueryString["Occasion"]) == false)
            {
                STRSQL += "AND Occasion=?Occasion ";
            }

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];
            if (string.IsNullOrEmpty(Request.QueryString["ResUseID"]) == false)
            {
                MyCMD.Parameters.Add("?ResUseID", MySqlDbType.VarChar).Value = Request.QueryString["ResUseID"];
            }
            if (string.IsNullOrEmpty(Request.QueryString["Occasion"]) == false)
            {
                MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = Request.QueryString["Occasion"];
            }


            MyCONN.Open();

            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        string strIncompleteColour = ConfigurationManager.AppSettings["IncompleteColour"];

                        while (myDr.Read())
                        {
                            if (!DBNull.Value.Equals(myDr["Occasion"]))
                            {
                                ddOccasion.SelectedValue = myDr["Occasion"].ToString();
                            }

                            if (!DBNull.Value.Equals(myDr["GPAppointment"]))
                            {
                                txtGPAppointment.Text = (string)(myDr["GPAppointment"]);
                            }

                            if (lblGPAppointment.Font.Bold == true)
                            {
                                if (txtGPAppointment.Text == string.Empty)
                                {
                                    lblGPAppointment.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["GPHomeVisit"]))
                            {
                                txtGPHomeVisit.Text = (string)(myDr["GPHomeVisit"]);
                            }

                            if (lblGPHomeVisit.Font.Bold == true)
                            {
                                if (txtGPHomeVisit.Text == string.Empty)
                                {
                                    lblGPHomeVisit.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["GPTelConversation"]))
                            {
                                txtGPTelConversation.Text = (string)(myDr["GPTelConversation"]);
                            }

                            if (lblGPTelConversation.Font.Bold == true)
                            {
                                if (txtGPTelConversation.Text == string.Empty)
                                {
                                    lblGPTelConversation.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["SpecConsultantAppointment"]))
                            {
                                txtSpecConsultantAppointment.Text = (string)(myDr["SpecConsultantAppointment"]);
                            }

                            if (lblSpecConsultantAppointment.Font.Bold == true)
                            {
                                if (txtSpecConsultantAppointment.Text == string.Empty)
                                {
                                    lblSpecConsultantAppointment.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["AETreatment"]))
                            {
                                txtAETreatment.Text = (string)(myDr["AETreatment"]);
                            }

                            if (lblAETreatment.Font.Bold == true)
                            {
                                if (txtAETreatment.Text == string.Empty)
                                {
                                    lblAETreatment.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["AmbulanceAEVisit"]))
                            {
                                txtAmbulanceAEVisit.Text = (string)(myDr["AmbulanceAEVisit"]);
                            }

                            if (lblAmbulanceAEVisit.Font.Bold == true)
                            {
                                if (txtAmbulanceAEVisit.Text == string.Empty)
                                {
                                    lblAmbulanceAEVisit.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["NurseHomeVisit"]))
                            {
                                txtNurseHomeVisit.Text = (string)(myDr["NurseHomeVisit"]);
                            }

                            if (lblNurseHomeVisit.Font.Bold == true)
                            {
                                if (txtNurseHomeVisit.Text == string.Empty)
                                {
                                    lblNurseHomeVisit.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["NursePracticeAppointment"]))
                            {
                                txtNursePracticeAppointment.Text = (string)(myDr["NursePracticeAppointment"]);
                            }

                            if (lblNursePracticeAppointment.Font.Bold == true)
                            {
                                if (txtNursePracticeAppointment.Text == string.Empty)
                                {
                                    lblNursePracticeAppointment.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PhysiotherapistAppointment"]))
                            {
                                txtPhysiotherapistAppointment.Text = (string)(myDr["PhysiotherapistAppointment"]);
                            }

                            if (lblPhysiotherapistAppointment.Font.Bold == true)
                            {
                                if (txtPhysiotherapistAppointment.Text == string.Empty)
                                {
                                    lblPhysiotherapistAppointment.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["OccupationalTherapistAppointment"]))
                            {
                                txtOccupationalTherapistAppointment.Text = (string)(myDr["OccupationalTherapistAppointment"]);
                            }

                            if (lblOccupationalTherapistAppointment.Font.Bold == true)
                            {
                                if (txtOccupationalTherapistAppointment.Text == string.Empty)
                                {
                                    lblOccupationalTherapistAppointment.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["PsychologistAppointment"]))
                            {
                                txtPsychologistAppointment.Text = (string)(myDr["PsychologistAppointment"]);
                            }

                            if (lblPsychologistAppointment.Font.Bold == true)
                            {
                                if (txtPsychologistAppointment.Text == string.Empty)
                                {
                                    lblPsychologistAppointment.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["CounsellorAppointment"]))
                            {
                                txtCounsellorAppointment.Text = (string)(myDr["CounsellorAppointment"]);
                            }

                            if (lblCounsellorAppointment.Font.Bold == true)
                            {
                                if (txtCounsellorAppointment.Text == string.Empty)
                                {
                                    lblCounsellorAppointment.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["AttendedDayHospital"]))
                            {
                                txtAttendedDayHospital.Text = (string)(myDr["AttendedDayHospital"]);
                            }

                            if (lblAttendedDayHospital.Font.Bold == true)
                            {
                                if (txtAttendedDayHospital.Text == string.Empty)
                                {
                                    lblAttendedDayHospital.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Comments"]))
                            {
                                txtComments.Text = (string)(myDr["Comments"]);
                            }

                            //lock data if DataLocked=1
                            if (!DBNull.Value.Equals(myDr["DataLocked"]))
                            {
                                if (myDr["DataLocked"].ToString() == "1")
                                {
                                    chkDataLocked.Checked = true;
                                }
                                else
                                {
                                    chkDataLocked.Checked = false;
                                }
                            }
                            if (!DBNull.Value.Equals(myDr["ReasonModified"]))
                            {
                                lblReasonModifiedOldDetails.Text = myDr["ReasonModified"].ToString().Replace("\r\n", "<br />");
                            }

                            //Mark Data Final Assign 
                            if (!DBNull.Value.Equals(myDr["DataFinal"]))
                            {
                                if (myDr["DataFinal"].ToString() == "1")
                                {
                                    chkDataFinal.Checked = true;
                                }
                                else
                                {
                                    chkDataFinal.Checked = false;
                                }
                            }

                            cmdDelete.Visible = true;
                        }
                    }
                    else
                    {
                        cmdDelete.Visible = false;
                    }
                }
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
            }

            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                lblUserMessages.Text = ex.Message + " An error occured while executing assign query. ";
            }

        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error orrcured while assigning data.";
        }
    }
    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            MaintainScrollPositionOnPostBack = false;

            if (ddOccasion.SelectedValue == "0")
            {
                throw new Exception("Please Select an Occasion.");
            }

            //check if data has been added
            string STRSQLFIND = "SELECT COUNT(*) FROM resuse WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion";

            int INT_COUNTRECORDS = 0;

            INT_COUNTRECORDS = Convert.ToInt32(GeneralRoutines.ReturnScalarTwo(STRSQLFIND, "?TrialIDRecipient", Request.QueryString["TID_R"], "?Occasion", ddOccasion.SelectedValue, STRCONN));

            //if (INT_COUNTRECORDS > 0)
            //{
            //    throw new Exception("'Resource Use Data' has been already been entered for the Selected Occasion. Please edit existing data.");
            //}


            if (INT_COUNTRECORDS > 1)
            {
                throw new Exception("More than 1 'Resource Use Data' has been entered for the Selected Occasion. Please delete one of the records.");
            }

            if (INT_COUNTRECORDS < 0)
            {
                throw new Exception("An error occured while checking if 'Resource Use Data' has already been added for this TrialID (Recipient).");
            }
            if (txtGPAppointment.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtGPAppointment.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for GP Appointment.");
                }
                if (Convert.ToInt16(txtGPAppointment.Text)<0)
                {
                    throw new Exception("The value for GP Appointment should be at greater than 0.");
                }
                if (Convert.ToInt16(txtGPAppointment.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for GP Appointment can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }
            }

            if (txtGPHomeVisit.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtGPHomeVisit.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for GP Home Visit.");
                }
                if (Convert.ToInt16(txtGPHomeVisit.Text) < 0)
                {
                    throw new Exception("The value for GP Home Visit should be at greater than 0.");
                }
                if (Convert.ToInt16(txtGPHomeVisit.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for GP Home Visit can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }
            }

            if (txtGPTelConversation.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtGPTelConversation.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for GP Telephone Conversation.");
                }
                if (Convert.ToInt16(txtGPTelConversation.Text) < 0)
                {
                    throw new Exception("The value for GP Telephone Conversation should be at greater than 0.");
                }
                if (Convert.ToInt16(txtGPTelConversation.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for GP Telephone Conversation can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }
            }


            if (txtSpecConsultantAppointment.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtSpecConsultantAppointment.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for Specialist/Consultant Appointment.");
                }
                if (Convert.ToInt16(txtSpecConsultantAppointment.Text) < 0)
                {
                    throw new Exception("The value for Specialist/Consultant Appointment should be at greater than 0.");
                }
                if (Convert.ToInt16(txtSpecConsultantAppointment.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for Specialist/Consultant Appointment can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }
            }

            if (txtAETreatment.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtAETreatment.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for '" + lblAETreatment.Text + "'.");
                }
                if (Convert.ToInt16(txtAETreatment.Text) < 0)
                {
                    throw new Exception("The value for '" + lblAETreatment.Text + "' should be at greater than 0.");
                }
                if (Convert.ToInt16(txtAETreatment.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for '" + lblAETreatment.Text + "' can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }
            }

            if (txtAmbulanceAEVisit.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtAmbulanceAEVisit.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for '" + lblAmbulanceAEVisit.Text + "'.");
                }
                if (Convert.ToInt16(txtAmbulanceAEVisit.Text) < 0)
                {
                    throw new Exception("The value for '" + lblAmbulanceAEVisit.Text + "' should be at greater than 0.");
                }
                if (Convert.ToInt16(txtAmbulanceAEVisit.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for '" + lblAmbulanceAEVisit.Text + "' can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }
            }

            if (txtNurseHomeVisit.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtNurseHomeVisit.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for '" + lblNurseHomeVisit.Text + "'.");
                }
                if (Convert.ToInt16(txtNurseHomeVisit.Text) < 0)
                {
                    throw new Exception("The value for '" + lblNurseHomeVisit.Text + "' should be at greater than 0.");
                }
                if (Convert.ToInt16(txtNurseHomeVisit.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for '" + lblNurseHomeVisit.Text + "' can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }
            }

            if (txtNursePracticeAppointment.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtNursePracticeAppointment.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for '" + lblNursePracticeAppointment.Text + "'.");
                }
                if (Convert.ToInt16(txtNursePracticeAppointment.Text) < 0)
                {
                    throw new Exception("The value for '" + lblNursePracticeAppointment.Text + "' should be at greater than 0.");
                }
                if (Convert.ToInt16(txtNursePracticeAppointment.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for '" + lblNursePracticeAppointment.Text + "' can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }
            }


            if (txtPhysiotherapistAppointment.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtPhysiotherapistAppointment.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for '" + lblPhysiotherapistAppointment.Text + "'.");
                }
                if (Convert.ToInt16(txtPhysiotherapistAppointment.Text) < 0)
                {
                    throw new Exception("The value for '" + lblPhysiotherapistAppointment.Text + "' should be at greater than 0.");
                }
                if (Convert.ToInt16(txtPhysiotherapistAppointment.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for '" + lblPhysiotherapistAppointment.Text + "' can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }
            }

            if (txtOccupationalTherapistAppointment.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtOccupationalTherapistAppointment.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for '" + lblOccupationalTherapistAppointment.Text + "'.");
                }
                if (Convert.ToInt16(txtOccupationalTherapistAppointment.Text) < 0)
                {
                    throw new Exception("The value for '" + lblOccupationalTherapistAppointment.Text + "' should be at greater than 0.");
                }
                if (Convert.ToInt16(txtOccupationalTherapistAppointment.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for '" + lblOccupationalTherapistAppointment.Text + "' can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }
            }

            if (txtPsychologistAppointment.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtPsychologistAppointment.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for '" + lblPsychologistAppointment.Text + "'.");
                }
                if (Convert.ToInt16(txtPsychologistAppointment.Text) < 0)
                {
                    throw new Exception("The value for '" + lblPsychologistAppointment.Text + "' should be at greater than 0.");
                }
                if (Convert.ToInt16(txtPsychologistAppointment.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for '" + lblPsychologistAppointment.Text + "' can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }

            }

            if (txtCounsellorAppointment.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtCounsellorAppointment.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for '" + lblCounsellorAppointment.Text + "'.");
                }
                if (Convert.ToInt16(txtCounsellorAppointment.Text) < 0)
                {
                    throw new Exception("The value for '" + lblCounsellorAppointment.Text + "' should be at greater than 0.");
                }
                if (Convert.ToInt16(txtCounsellorAppointment.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for '" + lblCounsellorAppointment.Text + "' can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }
            }


            if (txtAttendedDayHospital.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtAttendedDayHospital.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for '" + lblAttendedDayHospital.Text + "'.");
                }
                if (Convert.ToInt16(txtAttendedDayHospital.Text) < 0)
                {
                    throw new Exception("The value for '" + lblAttendedDayHospital.Text + "' should be at greater than 0.");
                }
                if (Convert.ToInt16(txtAttendedDayHospital.Text) >ConstantsGeneral.intMaxResUse)
                {
                    throw new Exception("The value for '" + lblAttendedDayHospital.Text + "' can not be greater than " + ConstantsGeneral.intMaxResUse.ToString() + ".");
                }
            }

            if (chkDataLocked.Checked == true)
            {
                if (txtReasonModified.Text == string.Empty)
                {
                    throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                }
            }

            //now add the data
            string STRSQL = string.Empty;

            STRSQL += "INSERT INTO resuse ";
            STRSQL += "(TrialIDRecipient, Occasion, GPAppointment, GPHomeVisit, GPTelConversation, SpecConsultantAppointment,";
            STRSQL += "AETreatment, AmbulanceAEVisit, NurseHomeVisit, NursePracticeAppointment, PhysiotherapistAppointment,";
            STRSQL += "OccupationalTherapistAppointment, PsychologistAppointment,CounsellorAppointment, AttendedDayHospital,";
            STRSQL += "Comments, DateCreated, CreatedBy, EventCode) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialIDRecipient, ?Occasion, ?GPAppointment, ?GPHomeVisit, ?GPTelConversation, ?SpecConsultantAppointment,";
            STRSQL += "?AETreatment, ?AmbulanceAEVisit, ?NurseHomeVisit, ?NursePracticeAppointment, ?PhysiotherapistAppointment,";
            STRSQL += "?OccupationalTherapistAppointment, ?PsychologistAppointment, ?CounsellorAppointment, ?AttendedDayHospital,";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy, ?EventCode) ";

            //update ata
            string STRSQL_UPDATE = string.Empty;

            STRSQL_UPDATE += "UPDATE resuse SET ";
            STRSQL_UPDATE += "GPAppointment=?GPAppointment, GPHomeVisit=?GPHomeVisit, GPTelConversation=?GPTelConversation, ";
            STRSQL_UPDATE += "SpecConsultantAppointment=?SpecConsultantAppointment, AETreatment=?AETreatment, AmbulanceAEVisit=?AmbulanceAEVisit,";
            STRSQL_UPDATE += "NurseHomeVisit=?NurseHomeVisit, NursePracticeAppointment=?NursePracticeAppointment, PhysiotherapistAppointment=?PhysiotherapistAppointment,";
            STRSQL_UPDATE += "OccupationalTherapistAppointment=?OccupationalTherapistAppointment, PsychologistAppointment=?PsychologistAppointment,";
            STRSQL_UPDATE += "CounsellorAppointment=?CounsellorAppointment, AttendedDayHospital=?AttendedDayHospital, ";
            STRSQL_UPDATE += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy, EventCode=?EventCode ";
            STRSQL_UPDATE += "WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion ";

            // lock data if all required fields have been entered
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE resuse SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, ";
            STRSQL_LOCK += "LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialIDRecipient=?TrialIDRecipient AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "AND Occasion=?Occasion ";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE resuse SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialIDRecipient =?TrialIDRecipient ";

            //STRSQL_LOCK += "AND Occasion IS NOT NULL AND  GPAppointment IS NOT NULL AND  GPHomeVisit IS NOT NULL AND  GPTelConversation IS NOT NULL AND ";
            //STRSQL_LOCK += "SpecConsultantAppointment IS NOT NULL AND  AETreatment IS NOT NULL AND  AmbulanceAEVisit IS NOT NULL AND ";
            //STRSQL_LOCK += "NurseHomeVisit IS NOT NULL AND  NursePracticeAppointment IS NOT NULL AND  PhysiotherapistAppointment IS NOT NULL AND ";
            //STRSQL_LOCK += "OccupationalTherapistAppointment IS NOT NULL AND  PsychologistAppointment IS NOT NULL AND ";
            //STRSQL_LOCK += "CounsellorAppointment IS NOT NULL AND  AttendedDayHospital IS NOT NULL ";
           
            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            if (INT_COUNTRECORDS == 0)
            {
                MyCMD.CommandText = STRSQL;
            }
            if (INT_COUNTRECORDS == 1)
            {
                MyCMD.CommandText = STRSQL_UPDATE;
            }

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];

            if (string.IsNullOrEmpty(Request.QueryString["ResUseID"]) == false)
            {
                MyCMD.Parameters.Add("?ResUseID", MySqlDbType.VarChar).Value = Request.QueryString["ResUseID"];
            }

            if (ddOccasion.SelectedValue=="0")
            {
                MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;
            }
            
            if (txtGPAppointment.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?GPAppointment", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?GPAppointment", MySqlDbType.VarChar).Value = txtGPAppointment.Text;
            }

            if (txtGPHomeVisit.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?GPHomeVisit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?GPHomeVisit", MySqlDbType.VarChar).Value = txtGPHomeVisit.Text;
            }

            if (txtGPTelConversation.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?GPTelConversation", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?GPTelConversation", MySqlDbType.VarChar).Value = txtGPTelConversation.Text;
            }

            if (txtSpecConsultantAppointment.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?SpecConsultantAppointment", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?SpecConsultantAppointment", MySqlDbType.VarChar).Value = txtSpecConsultantAppointment.Text;
            }

            if (txtAETreatment.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?AETreatment", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?AETreatment", MySqlDbType.VarChar).Value = txtAETreatment.Text;
            }

            if (txtAmbulanceAEVisit.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?AmbulanceAEVisit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?AmbulanceAEVisit", MySqlDbType.VarChar).Value = txtAmbulanceAEVisit.Text;
            }

            if (txtNurseHomeVisit.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?NurseHomeVisit", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NurseHomeVisit", MySqlDbType.VarChar).Value = txtNurseHomeVisit.Text;
            }

            if (txtNursePracticeAppointment.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?NursePracticeAppointment", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NursePracticeAppointment", MySqlDbType.VarChar).Value = txtNursePracticeAppointment.Text;
            }

            if (txtPhysiotherapistAppointment.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?PhysiotherapistAppointment", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PhysiotherapistAppointment", MySqlDbType.VarChar).Value = txtPhysiotherapistAppointment.Text;
            }

            if (txtOccupationalTherapistAppointment.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?OccupationalTherapistAppointment", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?OccupationalTherapistAppointment", MySqlDbType.VarChar).Value = txtOccupationalTherapistAppointment.Text;
            }

            if (txtPsychologistAppointment.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?PsychologistAppointment", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?PsychologistAppointment", MySqlDbType.VarChar).Value = txtPsychologistAppointment.Text;
            }

            if (txtCounsellorAppointment.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?CounsellorAppointment", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?CounsellorAppointment", MySqlDbType.VarChar).Value = txtCounsellorAppointment.Text;
            }

            if (txtAttendedDayHospital.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?AttendedDayHospital", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?AttendedDayHospital", MySqlDbType.VarChar).Value = txtAttendedDayHospital.Text;
            }


            if (txtComments.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
            }

            if (txtReasonModified.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ReasonModified", MySqlDbType.VarChar).Value = DBNull.Value;
                // MyCMD.Parameters.Add("?DateModified", MySqlDbType.VarChar).Value = DBNull.Value;
                //MyCMD.Parameters.Add("?ModifiedBy", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                string strReasonModified = "";
                strReasonModified += DateTime.Now.ToString() + " Modified By " + SessionVariablesAll.UserName + System.Environment.NewLine;
                strReasonModified += txtReasonModified.Text;
                if (lblReasonModifiedOldDetails.Text != string.Empty)
                {
                    strReasonModified += System.Environment.NewLine + lblReasonModifiedOldDetails.Text;
                }



                MyCMD.Parameters.Add("?ReasonModified", MySqlDbType.VarChar).Value = strReasonModified;
                //MyCMD.Parameters.Add("?DateModified", MySqlDbType.VarChar).Value = DateTime.Now;
                //MyCMD.Parameters.Add("?ModifiedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;
            }

            if (ddOccasion.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?EventCode", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                if (ddOccasion.SelectedValue == "3 Months")
                {
                    MyCMD.Parameters.Add("?EventCode", MySqlDbType.VarChar).Value = "65";
                }
                else if (ddOccasion.SelectedValue == "1 Year")
                {
                    MyCMD.Parameters.Add("?EventCode", MySqlDbType.VarChar).Value = "66";
                }
                else 
                {
                    MyCMD.Parameters.Add("?EventCode", MySqlDbType.VarChar).Value = DBNull.Value; ;
                }
                
            }

            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCMD.Parameters.Add("?DataLocked", MySqlDbType.VarChar).Value = 1;

            if (chkDataFinal.Checked == true)
            {
                MyCMD.Parameters.Add("?DataFinal", MySqlDbType.VarChar).Value = 1;
            }
            else
            {
                MyCMD.Parameters.Add("?DataFinal", MySqlDbType.VarChar).Value = 0;
            }


            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();

                //lock data
                if (chkAllDataAdded.Checked == true)
                {
                    MyCMD.CommandType = CommandType.Text;
                    MyCMD.CommandText = STRSQL_LOCK;
                    MyCMD.ExecuteNonQuery();

                }

                if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
                {
                    MyCMD.CommandType = CommandType.Text;
                    MyCMD.CommandText = STRSQL_FINAL;
                    MyCMD.ExecuteNonQuery();
                }

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                BindData();
                lblUserMessages.Text = "Resource Use Data Added";

                //Response.Redirect(Request.Url.AbsoluteUri, false);

                //redirect to summary page if data complete is not no
                string strSQLCOMPLETE = string.Empty;

                strSQLCOMPLETE += "SELECT  ";
                strSQLCOMPLETE += "IF(t2.GPAppointment IS NOT NULL AND t2.GPHomeVisit IS NOT NULL AND t2.SpecConsultantAppointment IS NOT NULL  ";
                strSQLCOMPLETE += "AND t2.AETreatment IS NOT NULL ";
                strSQLCOMPLETE += " ";
                strSQLCOMPLETE += ",'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM resuse t2 ";
                strSQLCOMPLETE += "WHERE t2.TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion ";


                //lblVenousProblems.Text = strSQLCOMPLETE;

                string strComplete = GeneralRoutines.ReturnScalarTwo(strSQLCOMPLETE, "?TrialIDRecipient", Request.QueryString["TID_R"],"?Occasion", ddOccasion.SelectedValue, STRCONN);

                //redirect to summary page
                if (strComplete == "Complete")
                {
                    Response.Redirect(strRedirectSummary + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"], false);
                }
                else
                {
                    Response.Redirect(strPageBaseUrl + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"] + "&Occasion=" + ddOccasion.SelectedValue + "&SCode=1", false);

                    lblUserMessages.Text = "Data Submitted";
                }

            }
            catch (System.Exception ex)
            {

               

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }
                lblUserMessages.Text = ex.Message + " An error occured while executing Resource Use query.";
            }

            
            
            //finally //close connection
            //{
            //    if (MyCONN.State == ConnectionState.Open)
            //    {
            //        MyCONN.Close();
            //    }
            //}


        }
        catch (System.Exception ex)
        {
            //MaintainScrollPositionOnPostBack = false;
            lblUserMessages.Text = ex.Message + " An error occured while adding 'Resource Use' data.";
        }
    }
    protected void ddOccasion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            Response.Redirect(strEditRedirect + Request.QueryString["TID"] + "&TID_R=" + Request.QueryString["TID_R"] + "&Occasion=" + ddOccasion.SelectedValue, false);

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting a dropdown option.";
        }

    }

    
    
    //protected void BindDataGV2()
    //{
    //    try
    //    {
    //        if (Request.QueryString["TID"].ToString() == String.Empty)
    //        {
    //            throw new Exception("Could not obtain TrialID.");
    //        }
                       

    //        string strSQL = String.Empty;

    //        strSQL += "SELECT t1.*,     ";
    //        strSQL += "DATE_FORMAT(t1.DateAdmission, '%d/%m/%Y') Date_Admission, ";
    //        strSQL += "DATE_FORMAT(t1.DateDischarge, '%d/%m/%Y') Date_Discharge ";
    //        strSQL += "FROM r_readmissions t1 ";
    //        strSQL += "WHERE t1.TrialID=?TrialID ";
    //        strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
    //        strSQL += ", " + (string)ViewState["SortFieldGV2"] + " " + (string)ViewState["SortDirectionGV2"];

    //        GV2.DataSource = SqlDataSource2;
    //        SqlDataSource2.SelectCommand = strSQL;
    //        SqlDataSource2.SelectParameters.Clear();
    //        SqlDataSource2.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

    //        GV2.DataBind();

    //        if (GV2.Rows.Count > 0)
    //        {
    //            lblGV2.Text = "Click on 'Date Admission' to Edit 'Recipient Readmissions' Data.";
    //        }
    //        else
    //        {
    //            lblGV2.Text = "'Recipient Readmissions' Data has not been added.";
    //        }
            

    //        lblDescriptionReadmissions.Text = "Add  'Recipient Readmissions' Data for " + Request.QueryString["TID"].ToString();




    //    }
    //    catch (System.Exception excep)
    //    {
    //        lblGV2.Text = excep.Message + " An error occured while binding the page.";
    //    }

    //    //lblUserMessages.Text = strSQL;
    //}



    ////sorting main datagrid
    //protected void GV2_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    if (e.SortExpression.ToString() == ViewState["SortFieldGV2"].ToString())
    //    {
    //        switch (ViewState["SortDirectionGV2"].ToString())
    //        {
    //            case "ASC":
    //                ViewState["SortDirectionGV2"] = "DESC";
    //                break;
    //            case "DESC":
    //                ViewState["SortDirectionGV2"] = "ASC";
    //                break;
    //        }

    //    }
    //    else
    //    {
    //        ViewState["SortFieldGV2"] = e.SortExpression;
    //        ViewState["SortDirectionGV2"] = "DESC";
    //    }
    //    BindDataGV2();
    //}

    //protected void ddReAdmissionType_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    try
    //    {
    //        if (ddReAdmissionType.SelectedValue == "Hospital Admission")
    //        {
    //            pnlHospitalAdmission.Visible = true;

    //        }
    //        else
    //        {
    //            pnlHospitalAdmission.Visible = false;
    //            rblRequiredSurgery.SelectedValue = STR_UNKNOWN_SELECTION;
    //            rblITU_HDU.SelectedValue = STR_UNKNOWN_SELECTION;
    //            txtDaysHospital.Text = string.Empty;

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        MaintainScrollPositionOnPostBack=false;
    //        lblUserMessages.Text = ex.Message + " An error occured while selecting an option for '" + lblReAdmissionType.Text + "' ";
    //    }
        

        
    //}
    //protected void cmdResetReadmissions_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Response.Redirect(Request.Url.AbsoluteUri, false);
    //        //lblUserMessages.Text = "";
    //    }

    //    catch (System.Exception ex)
    //    {
    //        lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
    //    }
    //}
    //protected void cmdAddReadmissions_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lblUserMessages.Text = string.Empty;

    //        if (ddOccasion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
    //        {
    //            throw new Exception("Please Select an Occasion.");
    //        }

    //        if (ddReAdmissionType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
    //        {
    //            throw new Exception("Please Select Re-Admission Type.");
    //        }

    //        if (txtDateAdmission.Text == string.Empty)
    //        {
    //            throw new Exception("Please Enter Date of Admission.");
    //        }

    //        if (GeneralRoutines.IsDate(txtDateAdmission.Text) == false)
    //        {
    //            throw new Exception("Please Enter Date of Admission as dd/mm/yyyy.");
    //        }

    //        if (Convert.ToDateTime(txtDateAdmission.Text) > DateTime.Today)
    //        {
    //            throw new Exception("Date of Admission cannot be greater than Today's Date.");
    //        }

            

    //        //get the follow up date
    //        string STRSQL_FOLLOWUPDATE = "SELECT FollowUpDate FROM r_fuposttreatment WHERE TrialID=?TrialID AND Occasion=?Occasion ";

    //        string strFollowUpDate = GeneralRoutines.ReturnScalarTwo(STRSQL_FOLLOWUPDATE, "?TrialID", Request.QueryString["TID"].ToString(), "?Occasion", ddOccasion.SelectedValue, STRCONN);

    //        //if (GeneralRoutines.IsDate(strFollowUpDate) == false)
    //        //{
    //        //    throw new Exception("Could not obtain the follow up date for selected Occasion.");
    //        //}
    //        //else
    //        //{
    //        //    if (Convert.ToDateTime(txtDateAdmission.Text) > Convert.ToDateTime(strFollowUpDate))
    //        //    {
    //        //        throw new Exception("'Date of Admission' can not be later than 'Follow Up Date' " + Convert.ToDateTime(strFollowUpDate).ToShortDateString() + ".");
    //        //    }
    //        //}

    //        if (GeneralRoutines.IsDate(strFollowUpDate) == true)
    //        {
    //            if (Convert.ToDateTime(txtDateAdmission.Text) > Convert.ToDateTime(strFollowUpDate))
    //            {
    //                throw new Exception("'Date of Admission' can not be later than 'Follow Up Date' " + Convert.ToDateTime(strFollowUpDate).ToShortDateString() + ".");
    //            }
    //        }

    //        string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_readmissions WHERE TrialID=?TrialID AND DateAdmission=?DateAdmission ";

    //        int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), "?DateAdmission", Convert.ToDateTime(txtDateAdmission.Text).ToString("yyyy-MM-dd"), STRCONN));

    //        if (intCountFind > 0)
    //        {
    //            throw new Exception("There already exists an Admission data for the date you have entered. Please Edit the existing data.");
    //        }

    //        if (intCountFind < 0)
    //        {
    //            throw new Exception("Could not check if there already exists an Admission Up data for the date you have entered.");
    //        }




    //        if (txtDateDischarge.Text != string.Empty)
    //        {
    //            if (GeneralRoutines.IsDate(txtDateDischarge.Text) == false)
    //            {
    //                throw new Exception("Please Enter Date of Discharge as dd/mm/yyyy.");
    //            }


    //            if (Convert.ToDateTime(txtDateDischarge.Text) > DateTime.Today)
    //            {
    //                throw new Exception("Date of Discharge cannot be greater than Today's Date.");
    //            }

    //            if (Convert.ToDateTime(txtDateAdmission.Text) > Convert.ToDateTime(txtDateDischarge.Text))
    //            {
    //                throw new Exception("Date of Admission cannot be greater than Date of Discharge.");
    //            }


    //        }

    //        //if (rblITU_HDU.SelectedValue == STR_UNKNOWN_SELECTION)
    //        //{
    //        //    throw new Exception("Please Select if 'ITU/HDU Admission' was also required.");
    //        //}

    //        if (txtDaysHospital.Text != string.Empty)
    //        {
    //            if (GeneralRoutines.IsNumeric(txtDaysHospital.Text) == false)
    //            {
    //                throw new Exception("Please Enter " + lblDaysHospital.Text + " in Numeric Format.");
    //            }

    //            if (Convert.ToInt32(txtDaysHospital.Text) < intMinDaysHospital)
    //            {
    //                throw new Exception("The minimum allowed value for " + lblDaysHospital.Text + "  is " + intMinDaysHospital.ToString());
    //            }

    //            if (Convert.ToInt32(txtDaysHospital.Text) > intMaxDaysHospital)
    //            {
    //                throw new Exception("The maximum allowed value for " + lblDaysHospital.Text + "  is " + intMaxDaysHospital.ToString());
    //            }


    //            if (txtDaysHospital.Text.Contains("."))
    //            {
    //                throw new Exception("Decimal values are not allowed for " + lblDaysHospital.Text);
    //            }

                

    //            if (txtDateDischarge.Text != string.Empty && txtDateAdmission.Text != string.Empty)
    //            {
    //                Double dblTotalDaysInHospital = 0;
    //                dblTotalDaysInHospital = (Convert.ToDateTime(txtDateDischarge.Text) - Convert.ToDateTime(txtDateAdmission.Text)).TotalDays;

    //                if (dblTotalDaysInHospital < Convert.ToDouble(txtDaysHospital.Text))
    //                {
    //                    throw new Exception(lblDaysHospital.Text + " can not be greater than the Total Number of Days between date of Admission and Date of Dishcarge " + dblTotalDaysInHospital.ToString());
    //                }

    //            }

    //        }


            

    //        //if (txtComments.Text == string.Empty)
    //        //{
    //        //    throw new Exception("Please Enter Reason for Readmission.");
    //        //}


    //        //add the data
    //        string STRSQL = String.Empty;
    //        STRSQL += "INSERT INTO r_readmissions ";
    //        STRSQL += "(TrialID, Occasion, ReAdmissionType, DateAdmission,DateDischarge, RequiredSurgery, ";
    //        STRSQL += "ITU_HDU, DaysHospital, ";
    //        STRSQL += "Comments, DateCreated, CreatedBy) ";
    //        STRSQL += "VALUES ";
    //        STRSQL += "(?TrialID, ?Occasion, ?ReAdmissionType, ?DateAdmission, ?DateDischarge, ?RequiredSurgery,  ";
    //        STRSQL += "?ITU_HDU,  ?DaysHospital, ";
    //        STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

    //        string CS = string.Empty;
    //        CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

    //        MySqlConnection MyCONN = new MySqlConnection();
    //        MyCONN.ConnectionString = CS;

    //        MySqlCommand MyCMD = new MySqlCommand();
    //        MyCMD.Connection = MyCONN;

    //        MyCMD.CommandType = CommandType.Text;
    //        MyCMD.CommandText = STRSQL;

    //        MyCMD.Parameters.Clear();

    //        MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();
    //        // MyCMD.Parameters.Add("?RFUPostTreatmentID", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;

    //        MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;

    //        if (ddReAdmissionType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
    //        {
    //            MyCMD.Parameters.Add("?ReAdmissionType", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?ReAdmissionType", MySqlDbType.VarChar).Value = ddReAdmissionType.SelectedValue;
    //        }



    //        if (GeneralRoutines.IsDate(txtDateAdmission.Text) == false)
    //        {
    //            MyCMD.Parameters.Add("?DateAdmission", MySqlDbType.Date).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?DateAdmission", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateAdmission.Text);
    //        }

    //        if (GeneralRoutines.IsDate(txtDateDischarge.Text) == false)
    //        {
    //            MyCMD.Parameters.Add("?DateDischarge", MySqlDbType.Date).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?DateDischarge", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateDischarge.Text);
    //        }

    //        if (rblRequiredSurgery.SelectedValue == STR_UNKNOWN_SELECTION)
    //        {
    //            MyCMD.Parameters.Add("?RequiredSurgery", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?RequiredSurgery", MySqlDbType.VarChar).Value = rblRequiredSurgery.SelectedValue;
    //        }

    //        if (rblITU_HDU.SelectedValue == STR_UNKNOWN_SELECTION)
    //        {
    //            MyCMD.Parameters.Add("?ITU_HDU", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?ITU_HDU", MySqlDbType.VarChar).Value = rblITU_HDU.SelectedValue;
    //        }

    //        if (string.IsNullOrEmpty(txtDaysHospital.Text))
    //        {
    //            MyCMD.Parameters.Add("?DaysHospital", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?DaysHospital", MySqlDbType.VarChar).Value = txtDaysHospital.Text;
    //        }

    //        if (string.IsNullOrEmpty(txtReasonsReadmission.Text))
    //        {
    //            MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
    //        }
    //        else
    //        {
    //            MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtReasonsReadmission.Text;
    //        }

    //        MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

    //        MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

    //        MyCONN.Open();

    //        try
    //        {
    //            MyCMD.ExecuteNonQuery();

    //            if (MyCONN.State == ConnectionState.Open)
    //            { MyCONN.Close(); }

    //            BindDataGV2();
    //            lblUserMessages.Text = "'Readmissions Data' Added.";
    //            //set controls empty
    //            ddReAdmissionType.SelectedValue = "0";
    //            txtDateAdmission.Text = String.Empty;
    //            txtDateDischarge.Text = String.Empty;
    //            txtDaysHospital.Text = String.Empty;
    //            txtReasonsReadmission.Text = String.Empty;
    //            rblRequiredSurgery.SelectedValue = STR_UNKNOWN_SELECTION;
    //            rblITU_HDU.SelectedValue = STR_UNKNOWN_SELECTION;

    //        }

    //        catch (System.Exception ex)
    //        {
    //            if (MyCONN.State == ConnectionState.Open)
    //            { MyCONN.Close(); }
    //            lblUserMessages.Text = ex.Message + " An error occured while executing 'Readmissions Data' insert query.";
    //        }

    //        MaintainScrollPositionOnPostBack = false;
    //        //finally //close connection
    //        //{
    //        //    if (MyCONN.State == ConnectionState.Open)
    //        //    { MyCONN.Close(); }
    //        //}


    //    }
    //    catch (System.Exception ex)
    //    {
    //        MaintainScrollPositionOnPostBack = false;
    //        lblUserMessages.Text = ex.Message + " An error occured while Adding 'Readmissions Data' Data.";
    //    }
    //}
    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            
            
            if (Request.QueryString["TID_R"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID (Recipient).");
            }

            if (ddOccasion.SelectedValue==STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please select an Occasion.");
            }
            

            string STRSQL = String.Empty;

            STRSQL += "DELETE FROM resuse WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion ";


            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];

            MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;


            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();
                BindData();
                lblUserMessages.Text = "'Resource Use Data' Deleted";
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                Response.Redirect(Request.Url.AbsoluteUri, false);
            }

            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                lblUserMessages.Text = ex.Message + " An error occured while executing 'Resource Use Data' delete query.";
            }


            

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while deleting data.";
        }
    }


    protected void chkAllDataAdded_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;
            if (chkAllDataAdded.Checked == true)
            {

                string strDataLockedOther = ConfigurationManager.AppSettings["LockedMessageManual"];
                //strDataLockedOther = "";
                lblAllDataAddedMessage.Text = strDataLockedOther;
                lblAllDataAddedMessage.Visible = true;


            }
            else
            {
                lblAllDataAddedMessage.Visible = false;

            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while clicking Data Entry Complete CheckBox";
        }
    }

}