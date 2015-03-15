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
public partial class SpecClinicalData_EditResUse : System.Web.UI.Page
{
    #region " Private Constants & Variables "

    private const string STRCONN = "cope4dbconn";

    private const string STR_UNKNOWN_SELECTION = "Unknown";
    private const string STR_DD_UNKNOWN_SELECTION = "0";
    private const string STR_YES_SELECTION = "YES";
    private const string STR_NO_SELECTION = "NO";

    private const string strMainCPH = "cplMainContents";
    private const string strSpecimenCPH = "SpecimenContents";

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

                if (string.IsNullOrEmpty(Request.QueryString["ResUseID"]))
                {
                    throw new Exception("Could not obtain the Unique Identifier.");
                }
                lblDescription.Text = "Edit Resource Use Log for " + Request.QueryString["TID"].ToString();

                ddOccasion.Items.Clear();
                ListItem li = new ListItem();

                li.Text = "Select Occasion";
                li.Value = "0";
                li.Selected = true;
                ddOccasion.Items.Add(li);


                string strOccasionDay30 = "1 Month";
                li = new ListItem();
                li.Text = strOccasionDay30;
                li.Value = strOccasionDay30;
                li.Selected = false;

                ddOccasion.Items.Add(li);

                string strOccasionMonth6 = "6 Months";
                li = new ListItem();
                li.Text = strOccasionMonth6;
                li.Value = strOccasionMonth6;
                ddOccasion.Items.Add(li);

                //pnlClavienGrading.GroupingText += "<br/><b>Grade V:</b> Death of a patient.";

                ViewState["SortField"] = "Occasion";
                ViewState["SortDirection"] = "DESC";

                BindData();

                AssignData();

                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK selected 'Resource Use' data will be deleted.";

                ddReAdmissionType.DataSource = XMLReAdmissionTypesDataSource;
                ddReAdmissionType.DataBind();


                txtDateAdmission_CalendarExtender.EndDate = DateTime.Today;
                txtDateDischarge_CalendarExtender.EndDate = DateTime.Today;


                rblRequiredSurgery.DataSource = XMLMainOptionsDataSource;
                rblRequiredSurgery.DataBind();
                rblRequiredSurgery.SelectedValue = STR_UNKNOWN_SELECTION;

                rblITU_HDU.DataSource = XMLMainOptionsDataSource;
                rblITU_HDU.DataBind();
                rblITU_HDU.SelectedValue = STR_UNKNOWN_SELECTION;

                ViewState["SortFieldGV2"] = "DateAdmission";
                ViewState["SortDirectionGV2"] = "ASC";

                BindDataGV2();
                cmdResetReadmissions_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any 'Resource Use'/'Readmission' data entered will be lost.";

            }
        }
        catch (System.Exception ex)
        {
            lblGV1.Text = ex.Message + " An error occured while loading the page.";
        }
    }
    protected void BindData()
    {
        try
        {
            if (Request.QueryString["TID"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID.");
            }
            string strSQL = String.Empty;

            strSQL += "SELECT t1.*, t2.RecipientID,  t2.TrialID,  ";
            strSQL += "DATE_FORMAT(t1.DateEntered, '%d/%m/%Y') Date_AE, ";
            strSQL += "DATE_FORMAT(t1.DateCreated, '%d/%m/%Y %H:%i') Date_Created, ";
            strSQL += "DATE_FORMAT(t2.DateOfBirth, '%d/%m/%Y') Recipient_DateOfBirth ";
            strSQL += "FROM resuse t1 ";
            strSQL += "INNER JOIN r_identification t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = SqlDataSource1;
            SqlDataSource1.SelectCommand = strSQL;
            SqlDataSource1.SelectParameters.Clear();
            SqlDataSource1.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV1.DataBind();


            lblGV1.Text = "Click on TrialID (Recipient) to Edit an Resource Use Log.";

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

    protected void AssignData()
    {

        try
        {
            string STRSQL = string.Empty;

            STRSQL += "SELECT * FROM resuse WHERE TrialIDRecipient=?TrialIDRecipient AND ResUseID=?ResUseID  ";
                       

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
            MyCMD.Parameters.Add("?ResUseID", MySqlDbType.VarChar).Value = Request.QueryString["ResUseID"];

           
            MyCONN.Open();

            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
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

                            if (!DBNull.Value.Equals(myDr["GPHomeVisit"]))
                            {
                                txtGPHomeVisit.Text = (string)(myDr["GPHomeVisit"]);
                            }

                            if (!DBNull.Value.Equals(myDr["GPTelConversation"]))
                            {
                                txtGPTelConversation.Text = (string)(myDr["GPTelConversation"]);
                            }

                            if (!DBNull.Value.Equals(myDr["SpecConsultantAppointment"]))
                            {
                                txtSpecConsultantAppointment.Text = (string)(myDr["SpecConsultantAppointment"]);
                            }

                            if (!DBNull.Value.Equals(myDr["AETreatment"]))
                            {
                                txtAETreatment.Text = (string)(myDr["AETreatment"]);
                            }

                            if (!DBNull.Value.Equals(myDr["AmbulanceAEVisit"]))
                            {
                                txtAmbulanceAEVisit.Text = (string)(myDr["AmbulanceAEVisit"]);
                            }

                            if (!DBNull.Value.Equals(myDr["NurseHomeVisit"]))
                            {
                                txtNurseHomeVisit.Text = (string)(myDr["NurseHomeVisit"]);
                            }

                            if (!DBNull.Value.Equals(myDr["NursePracticeAppointment"]))
                            {
                                txtNursePracticeAppointment.Text = (string)(myDr["NursePracticeAppointment"]);
                            }

                            if (!DBNull.Value.Equals(myDr["PhysiotherapistAppointment"]))
                            {
                                txtPhysiotherapistAppointment.Text = (string)(myDr["PhysiotherapistAppointment"]);
                            }

                            if (!DBNull.Value.Equals(myDr["OccupationalTherapistAppointment"]))
                            {
                                txtOccupationalTherapistAppointment.Text = (string)(myDr["OccupationalTherapistAppointment"]);
                            }

                            if (!DBNull.Value.Equals(myDr["PsychologistAppointment"]))
                            {
                                txtPsychologistAppointment.Text = (string)(myDr["PsychologistAppointment"]);
                            }

                            if (!DBNull.Value.Equals(myDr["CounsellorAppointment"]))
                            {
                                txtCounsellorAppointment.Text = (string)(myDr["CounsellorAppointment"]);
                            }

                            if (!DBNull.Value.Equals(myDr["CounsellorAppointment"]))
                            {
                                txtCounsellorAppointment.Text = (string)(myDr["CounsellorAppointment"]);
                            }

                            if (!DBNull.Value.Equals(myDr["AttendedDayHospital"]))
                            {
                                txtAttendedDayHospital.Text = (string)(myDr["AttendedDayHospital"]);
                            }


                            if (!DBNull.Value.Equals(myDr["Comments"]))
                            {
                                txtComments.Text = (string)(myDr["Comments"]);
                            }

                        }
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
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while assigning data.";
        }
    }
    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["TID_R"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID (Recipient).");
            }

            if (Request.QueryString["ResUseID"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain unique Identifier.");
            }

            string STRSQL = String.Empty;

            STRSQL += "DELETE FROM resuse WHERE TrialIDRecipient=?TrialIDRecipient AND ResUseID=?ResUseID ";

            
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

            MyCMD.Parameters.Add("?ResUseID", MySqlDbType.VarChar).Value = Request.QueryString["ResUseID"];
            

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();
                BindData();
                lblUserMessages.Text = "'Resource Use Data' Deleted";
            }

            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while executing 'Resource Use Data' delete query.";
            }


            finally //close connection
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
            }

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while deleting data.";
        }
    }
    protected void cmdUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;
            if (ddOccasion.SelectedValue == "0")
            {
                throw new Exception("Please Select an Occasion.");
            }

            //check if data has been added
            string STRSQLFIND = "SELECT COUNT(*) FROM resuse WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion AND ResUseID <> ?ResUseID";

            int INT_COUNTRECORDS = 0;

            INT_COUNTRECORDS = Convert.ToInt32(GeneralRoutines.ReturnScalarThree(STRSQLFIND, "?TrialIDRecipient", Request.QueryString["TID_R"], "?Occasion", ddOccasion.SelectedValue, "?ResUseID", Request.QueryString["ResUseID"], STRCONN));

            if (INT_COUNTRECORDS > 0)
            {
                throw new Exception("'Resource Use Data' has been already been entered for the Selected Occasion. Please edit/delete existing data.");
            }
                       

            if (INT_COUNTRECORDS < 0)
            {
                throw new Exception("An error occured while checking if 'Resource Use Data' has already been added with this TrialID.");
            }
            if (txtGPAppointment.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtGPAppointment.Text) == false)
                {
                    throw new Exception("Please enter a value between 0 and 20 for GP Appointment.");
                }
                if (Convert.ToInt16(txtGPAppointment.Text) < 0)
                {
                    throw new Exception("The value for GP Appointment should be at greater than 0.");
                }
                if (Convert.ToInt16(txtGPAppointment.Text) > 20)
                {
                    throw new Exception("The value for GP Appointment can not be greater than 20.");
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
                if (Convert.ToInt16(txtGPHomeVisit.Text) > 20)
                {
                    throw new Exception("The value for GP Home Visit can not be greater than 20.");
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
                if (Convert.ToInt16(txtGPTelConversation.Text) > 20)
                {
                    throw new Exception("The value for GP Telephone Conversation can not be greater than 20.");
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
                if (Convert.ToInt16(txtSpecConsultantAppointment.Text) > 20)
                {
                    throw new Exception("The value for Specialist/Consultant Appointment can not be greater than 20.");
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
                if (Convert.ToInt16(txtAETreatment.Text) > 20)
                {
                    throw new Exception("The value for '" + lblAETreatment.Text + "' can not be greater than 20.");
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
                if (Convert.ToInt16(txtAmbulanceAEVisit.Text) > 20)
                {
                    throw new Exception("The value for '" + lblAmbulanceAEVisit.Text + "' can not be greater than 20.");
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
                if (Convert.ToInt16(txtNurseHomeVisit.Text) > 20)
                {
                    throw new Exception("The value for '" + lblNurseHomeVisit.Text + "' can not be greater than 20.");
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
                if (Convert.ToInt16(txtNursePracticeAppointment.Text) > 20)
                {
                    throw new Exception("The value for '" + lblNursePracticeAppointment.Text + "' can not be greater than 20.");
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
                if (Convert.ToInt16(txtPhysiotherapistAppointment.Text) > 20)
                {
                    throw new Exception("The value for '" + lblPhysiotherapistAppointment.Text + "' can not be greater than 20.");
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
                if (Convert.ToInt16(txtOccupationalTherapistAppointment.Text) > 20)
                {
                    throw new Exception("The value for '" + lblOccupationalTherapistAppointment.Text + "' can not be greater than 20.");
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
                if (Convert.ToInt16(txtPsychologistAppointment.Text) > 20)
                {
                    throw new Exception("The value for '" + lblPsychologistAppointment.Text + "' can not be greater than 20.");
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
                if (Convert.ToInt16(txtCounsellorAppointment.Text) > 20)
                {
                    throw new Exception("The value for '" + lblCounsellorAppointment.Text + "' can not be greater than 20.");
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
                if (Convert.ToInt16(txtAttendedDayHospital.Text) > 20)
                {
                    throw new Exception("The value for '" + lblAttendedDayHospital.Text + "' can not be greater than 20.");
                }
            }

            //now add the data
            string STRSQL = string.Empty;

            STRSQL += "UPDATE resuse SET ";
            STRSQL += "Occasion=?Occasion, GPAppointment=?GPAppointment, GPHomeVisit=?GPHomeVisit, GPTelConversation=?GPTelConversation, ";
            STRSQL += "SpecConsultantAppointment=?SpecConsultantAppointment, AETreatment=?AETreatment, AmbulanceAEVisit=?AmbulanceAEVisit,";
            STRSQL += "NurseHomeVisit=?NurseHomeVisit, NursePracticeAppointment=?NursePracticeAppointment, PhysiotherapistAppointment=?PhysiotherapistAppointment,";
            STRSQL += "OccupationalTherapistAppointment=?OccupationalTherapistAppointment, PsychologistAppointment=?PsychologistAppointment,";
            STRSQL += "CounsellorAppointment=?CounsellorAppointment, AttendedDayHospital=?AttendedDayHospital, ";
            STRSQL += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy, EventCode=?EventCode ";
            STRSQL += "WHERE TrialIDRecipient=?TrialIDRecipient AND ResUseID=?ResUseID ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"];
            MyCMD.Parameters.Add("?ResUseID", MySqlDbType.VarChar).Value = Request.QueryString["ResUseID"];

            if (ddOccasion.SelectedValue == "0")
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

            if (ddOccasion.SelectedValue == "0")
            {
                MyCMD.Parameters.Add("?EventCode", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                if (ddOccasion.SelectedValue == "Day 30")
                {
                    MyCMD.Parameters.Add("?EventCode", MySqlDbType.VarChar).Value = "65";
                }
                else if (ddOccasion.SelectedValue == "Month 6")
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


            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();
                BindData();
                lblUserMessages.Text = "Data Added";

            }
            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while executing query.";
            }


            finally //close connection
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }
            }


        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding data.";
        }
    }


    protected void BindDataGV2()
    {
        try
        {
            if (Request.QueryString["TID"].ToString() == String.Empty)
            {
                throw new Exception("Could not obtain TrialID.");
            }


            string strSQL = String.Empty;

            strSQL += "SELECT t1.*,  t2.RecipientID, t2.TrialID,   ";
            strSQL += "DATE_FORMAT(t1.DateAdmission, '%d/%m/%Y') Date_Admission, ";
            strSQL += "DATE_FORMAT(t1.DateDischarge, '%d/%m/%Y') Date_Discharge ";
            strSQL += "FROM r_readmissions t1 ";
            strSQL += "INNER JOIN r_identification t2 ON t1.TrialIDRecipient=t2.TrialIDRecipient ";
            strSQL += "WHERE t1.TrialIDRecipient=?TrialIDRecipient ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];
            strSQL += ", " + (string)ViewState["SortFieldGV2"] + " " + (string)ViewState["SortDirectionGV2"];

            GV2.DataSource = SqlDataSource2;
            SqlDataSource2.SelectCommand = strSQL;
            SqlDataSource2.SelectParameters.Clear();
            SqlDataSource2.SelectParameters.Add("?TrialIDRecipient", Request.QueryString["TID_R"].ToString());

            GV2.DataBind();

            if (GV2.Rows.Count > 0)
            {
                lblGV2.Text = "Click on' Date Admission' to Edit 'Recipient Readmissions' Data.";
            }
            else
            {
                lblGV2.Text = "'Recipient Readmissions' Data hass not been added.";
            }


            lblDescriptionReadmissions.Text = "Add  Recipient Readmissions Data for " + Request.QueryString["TID"].ToString();




        }
        catch (System.Exception excep)
        {
            lblGV2.Text = excep.Message + " An error occured while binding the page.";
        }

        //lblUserMessages.Text = strSQL;
    }



    //sorting main datagrid
    protected void GV2_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (e.SortExpression.ToString() == ViewState["SortFieldGV2"].ToString())
        {
            switch (ViewState["SortDirectionGV2"].ToString())
            {
                case "ASC":
                    ViewState["SortDirectionGV2"] = "DESC";
                    break;
                case "DESC":
                    ViewState["SortDirectionGV2"] = "ASC";
                    break;
            }

        }
        else
        {
            ViewState["SortFieldGV2"] = e.SortExpression;
            ViewState["SortDirectionGV2"] = "DESC";
        }
        BindDataGV2();
    }

    protected void ddReAdmissionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        {
            if (ddReAdmissionType.SelectedValue == "Hospital Admission")
            {
                pnlHospitalAdmission.Visible = true;

            }
            else
            {
                pnlHospitalAdmission.Visible = false;
                rblRequiredSurgery.SelectedValue = STR_UNKNOWN_SELECTION;
                rblITU_HDU.SelectedValue = STR_UNKNOWN_SELECTION;
                txtDaysHospital.Text = string.Empty;

            }

        }
    }
    protected void cmdResetReadmissions_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect(Request.Url.AbsoluteUri, false);
            //lblUserMessages.Text = "";
        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while resetting the page.";
        }
    }
    protected void cmdAddReadmissions_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (ddOccasion.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select an Occasion.");
            }

            if (ddReAdmissionType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select Re-Admission Type.");
            }

            if (txtDateAdmission.Text == string.Empty)
            {
                throw new Exception("Please Enter Date of Admission.");
            }

            if (GeneralRoutines.IsDate(txtDateAdmission.Text) == false)
            {
                throw new Exception("Please Enter Date of Admission in the correct format.");
            }

            if (Convert.ToDateTime(txtDateAdmission.Text) > DateTime.Today)
            {
                throw new Exception("Date of Admission cannot be greater than Today's Date.");
            }

            //get the follow up date
            string STRSQL_FOLLOWUPDATE = "SELECT FollowUpDate FROM r_fuposttreatment WHERE TrialIDRecipient=?TrialIDRecipient AND Occasion=?Occasion ";

            string strFollowUpDate = GeneralRoutines.ReturnScalarTwo(STRSQL_FOLLOWUPDATE, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?Occasion", ddOccasion.SelectedValue, STRCONN);

            //if (GeneralRoutines.IsDate(strFollowUpDate) == false)
            //{
            //    throw new Exception("Could not obtain the follow up date for selected Occasion.");
            //}
            //else
            //{
            //    if (Convert.ToDateTime(txtDateAdmission.Text) > Convert.ToDateTime(strFollowUpDate))
            //    {
            //        throw new Exception("'Date of Admission' can not be later than 'Follow Up Date' " + Convert.ToDateTime(strFollowUpDate).ToShortDateString() + ".");
            //    }
            //}

            if (GeneralRoutines.IsDate(strFollowUpDate) == true)
            {
                if (Convert.ToDateTime(txtDateAdmission.Text) > Convert.ToDateTime(strFollowUpDate))
                {
                    throw new Exception("'Date of Admission' can not be later than 'Follow Up Date' " + Convert.ToDateTime(strFollowUpDate).ToShortDateString() + ".");
                }
            }

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM r_readmissions WHERE TrialIDRecipient=?TrialIDRecipient AND DateAdmission=?DateAdmission ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialIDRecipient", Request.QueryString["TID_R"].ToString(), "?DateAdmission", Convert.ToDateTime(txtDateAdmission.Text).ToString("yyyy-MM-dd"), STRCONN));

            if (intCountFind > 0)
            {
                throw new Exception("There already exists an Admission data for the date you have entered. Please Edit the existing data.");
            }

            if (intCountFind < 0)
            {
                throw new Exception("Could not check if there already exists an Admission Up data for the date you have entered.");
            }




            if (txtDateDischarge.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtDateDischarge.Text) == false)
                {
                    throw new Exception("Please Enter Date of Discharge in the correct format.");
                }


                if (Convert.ToDateTime(txtDateDischarge.Text) > DateTime.Today)
                {
                    throw new Exception("Date of Discharge cannot be greater than Today's Date.");
                }

                if (Convert.ToDateTime(txtDateAdmission.Text) > Convert.ToDateTime(txtDateDischarge.Text))
                {
                    throw new Exception("Date of Admission cannot be greater than Date of Discharge.");
                }


            }

            //if (rblITU_HDU.SelectedValue == STR_UNKNOWN_SELECTION)
            //{
            //    throw new Exception("Please Select if 'ITU/HDU Admission' was also required.");
            //}

            if (txtDaysHospital.Text != string.Empty)
            {
                if (GeneralRoutines.IsNumeric(txtDaysHospital.Text) == false)
                {
                    throw new Exception("The 'Number of Days' should be numeric.");
                }
            }

            //if (txtComments.Text == string.Empty)
            //{
            //    throw new Exception("Please Enter Reason for Readmission.");
            //}


            //add the data
            string STRSQL = String.Empty;
            STRSQL += "INSERT INTO r_readmissions ";
            STRSQL += "(TrialIDRecipient, Occasion, ReAdmissionType, DateAdmission,DateDischarge, RequiredSurgery, ";
            STRSQL += "ITU_HDU, DaysHospital, ";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialIDRecipient, ?Occasion, ?ReAdmissionType, ?DateAdmission, ?DateDischarge, ?RequiredSurgery,  ";
            STRSQL += "?ITU_HDU,  ?DaysHospital, ";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;
            MyCMD.CommandText = STRSQL;

            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialIDRecipient", MySqlDbType.VarChar).Value = Request.QueryString["TID_R"].ToString();
            // MyCMD.Parameters.Add("?RFUPostTreatmentID", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;

            MyCMD.Parameters.Add("?Occasion", MySqlDbType.VarChar).Value = ddOccasion.SelectedValue;

            if (ddReAdmissionType.SelectedValue == STR_DD_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?ReAdmissionType", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ReAdmissionType", MySqlDbType.VarChar).Value = ddReAdmissionType.SelectedValue;
            }



            if (GeneralRoutines.IsDate(txtDateAdmission.Text) == false)
            {
                MyCMD.Parameters.Add("?DateAdmission", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateAdmission", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateAdmission.Text);
            }

            if (GeneralRoutines.IsDate(txtDateDischarge.Text) == false)
            {
                MyCMD.Parameters.Add("?DateDischarge", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DateDischarge", MySqlDbType.Date).Value = Convert.ToDateTime(txtDateDischarge.Text);
            }

            if (rblRequiredSurgery.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?RequiredSurgery", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RequiredSurgery", MySqlDbType.VarChar).Value = rblRequiredSurgery.SelectedValue;
            }

            if (rblITU_HDU.SelectedValue == STR_UNKNOWN_SELECTION)
            {
                MyCMD.Parameters.Add("?ITU_HDU", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ITU_HDU", MySqlDbType.VarChar).Value = rblITU_HDU.SelectedValue;
            }

            if (string.IsNullOrEmpty(txtDaysHospital.Text))
            {
                MyCMD.Parameters.Add("?DaysHospital", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DaysHospital", MySqlDbType.VarChar).Value = txtDaysHospital.Text;
            }

            if (string.IsNullOrEmpty(txtReasonsReadmission.Text))
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtReasonsReadmission.Text;
            }

            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();
                BindDataGV2();
                lblUserMessages.Text = "'Readmissions Data' Added.";

                //set controls empty
                ddReAdmissionType.SelectedValue = "0";
                txtDateAdmission.Text = String.Empty;
                txtDateDischarge.Text = String.Empty;
                txtDaysHospital.Text = String.Empty;
                txtReasonsReadmission.Text = String.Empty;
                rblRequiredSurgery.SelectedValue = STR_UNKNOWN_SELECTION;
                rblITU_HDU.SelectedValue = STR_UNKNOWN_SELECTION;
            }

            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while executing 'Readmissions Data' insert query.";
            }


                finally //close connection
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
            }


        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while Adding 'Readmissions Data' Data.";
        }
    }
}