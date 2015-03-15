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

public partial class SpecClinicalData_AddKidneyProcedure : System.Web.UI.Page
{
    #region " Private Constants & Variables "
        private const string STRCONN = "cope4dbconn";

        private const string strExcludeOccasion = "1-7 Days";

        private const string strMainCPH = "cplMainContents";
        private const string strSpecimenContents = "SpecimenContents"; //content place holder in SpecClinicalMasterPage

        private const string strPanel = "pnlSideSelected";//main panel containingcontrols when side is selected

        private const string strMainLabel = "lblDonorID";
        private const string strRecipientLabel = "lblRecipientID";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";

        private const string strDefaultFontColor = "Black";

        private const string strRedirectSummary = "~/SpecClinicalData/ViewSummary.aspx?TID=";

    


    #endregion

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                lblUserMessages.Text = string.Empty;

                if (string.IsNullOrEmpty(Request.QueryString["TID"].ToString()))
                {
                    throw new Exception("Could not obtain TrialID.");
                }

                lblDescription.Text = "Add Kidney Allocation Data for " + Request.QueryString["TID"].ToString();

                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                ddSide.DataSource = XMLKidneySidesDataSource;
                ddSide.DataBind();

                

                rblReallocated.DataSource = XMLMainOptionsYNDataSource;
                rblReallocated.DataBind();
                //rblReallocated.SelectedValue = STR_UNKNOWN_SELECTION;

                pnlReallocated.Visible = false;

                rblReasonReallocated.DataSource = XMLReallocatedReasonsDataSource;
                rblReasonReallocated.DataBind();
                rblReasonReallocated.SelectedValue = STR_UNKNOWN_SELECTION;

                txtDonorTechnicianPhoneDate_CalendarExtender.EndDate = DateTime.Today;
                txtScheduledTransplantStartDate_CalendarExtender.EndDate = DateTime.Today;
                txtTechnicianArrivalPerfusionCentreDate_CalendarExtender.EndDate = DateTime.Today;
                txtTechnicianDeparturePerfusionCentreDate_CalendarExtender.EndDate = DateTime.Today;
                txtArrivalTransplantHospitalDate_CalendarExtender.EndDate = DateTime.Today;
                txtNewScheduledTransplantStartDate_CalendarExtender.EndDate = DateTime.Today;
                txtDepartureFirstTransplantHospitalDate_CalendarExtender.EndDate = DateTime.Today;
                //txtArrivalNewTransplantHospitalDate_CalendarExtender.EndDate = DateTime.Today;
                //txtNewScheduledTransplantStartDate_CalendarExtender.EndDate = DateTime.Today;
                //txtNewScheduledTransplantStartDate_CalendarExtender.EndDate = DateTime.Today;

                ViewState["SortField"] = "TrialID";
                ViewState["SortDirection"] = "ASC";

                BindData();

                ListItem liSide = ddSide.Items.FindByValue(STR_UNKNOWN_SELECTION);
                if (liSide != null)
                {
                    ddSide.Items.Remove(liSide);
                }


                if (string.IsNullOrEmpty(Request.QueryString["Side"]) == false)
                {
                    liSide = ddSide.Items.FindByValue(Request.QueryString["Side"]);

                    if (liSide != null)
                    {

                        ddSide.SelectedIndex = -1;

                        liSide.Selected = true;

                        ddSide_SelectedIndexChanged(this, EventArgs.Empty);
                    }


                }

                


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
                        //cmdUpdate.Enabled = true;
                        cmdAddData.Enabled = true;
                        cmdDelete.Enabled = true;
                        cmdReset.Enabled = true;


                    }
                    else
                    {
                        //chkDataLocked.Checked = false;
                        pnlReasonModified.Visible = false;
                        pnlFinal.Visible = false;
                        //cmdUpdate.Enabled = false;
                        cmdAddData.Enabled = false;
                        cmdDelete.Enabled = false;
                        cmdReset.Enabled = false;

                    }
                }

                string strMandatoryMessage = string.Empty;
                strMandatoryMessage = ConstantsGeneral.MandatoryMessage;
                lblDescription.Text += "<br/><span class='FontMandatory'>" + strMandatoryMessage + "</span>";

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
                        strSCode1Message = ConstantsGeneral.SCode1;
                        lblUserMessages.Text = strSCode1Message;
                    }
                }


                hlkSummaryPage.NavigateUrl = strRedirectSummary + Request.QueryString["TID"];

                //locked button
                if (chkDataLocked.Checked == false)
                {
                    pnlAllDataAdded.Visible = true;
                }
                else
                {
                    pnlAllDataAdded.Visible = false;
                }

            }

            ////loop though rows to highlight selected occasion

            //if (ddSide.SelectedValue != STR_DD_UNKNOWN_SELECTION)
            //{
            //    Label lbl;

            //    foreach (GridViewRow row in GV1.Rows)
            //    {
            //        lbl = (Label)row.FindControl("lblSide");

            //        if (lbl != null)
            //        {
            //            if (row.RowType == DataControlRowType.DataRow)
            //            {
            //                if (lbl.Text == ddSide.SelectedValue)
            //                {
            //                    row.BackColor = System.Drawing.Color.LightBlue;
            //                }
            //            }
            //        }
            //    }

            //    AssignData();
            //}


        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while loading the page.";
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

            //// get the DonorID
            //string strDonorID = string.Empty;
            //string strRecipientID = string.Empty;

            //ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));

            //if (mpCPH != null)
            //{
            //    Label lblMainLabel = (Label)(mpCPH.FindControl(strMainLabel));

            //    if (lblMainLabel != null)
            //    {
            //        strDonorID = lblMainLabel.Text.Replace("(", "");
            //        strDonorID = strDonorID.Replace(")", "");
            //    }

            //    Label lblRecipientLabel = (Label)(mpCPH.FindControl(strRecipientLabel));

            //    if (lblRecipientLabel != null)
            //    {
            //        strRecipientID = lblRecipientLabel.Text.Replace("(", "");
            //        strRecipientID = strRecipientID.Replace(")", "");
            //    }

            //}

            string strSQL = String.Empty;

            strSQL += "SELECT t1.*,   ";
            strSQL += "CONCAT(IF(t1.DonorTechnicianPhoneDate IS NULL, 'NA', DATE_FORMAT(t1.DonorTechnicianPhoneDate, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.DonorTechnicianPhoneTime IS NULL, 'NA', DATE_FORMAT(t1.DonorTechnicianPhoneTime, '%H:%i')))  DonorTechnicianPhone, ";
            strSQL += "CONCAT(IF(t1.ScheduledTransplantStartDate IS NULL, 'NA', DATE_FORMAT(t1.ScheduledTransplantStartDate, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.ScheduledTransplantStartTime IS NULL, 'NA', DATE_FORMAT(t1.ScheduledTransplantStartTime, '%H:%i')))  ScheduledTransplantStart, ";
            strSQL += "CONCAT(IF(t1.TechnicianArrivalPerfusionCentreDate IS NULL, 'NA', DATE_FORMAT(t1.TechnicianArrivalPerfusionCentreDate, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.TechnicianArrivalPerfusionCentreTime IS NULL, 'NA', DATE_FORMAT(t1.TechnicianArrivalPerfusionCentreTime, '%H:%i')))  TechnicianArrivalPerfusionCentre, ";
            strSQL += "CONCAT(IF(t1.TechnicianDeparturePerfusionCentreDate IS NULL, 'NA', DATE_FORMAT(t1.TechnicianDeparturePerfusionCentreDate, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.TechnicianDeparturePerfusionCentreTime IS NULL, 'NA', DATE_FORMAT(t1.TechnicianDeparturePerfusionCentreTime, '%H:%i')))  TechnicianDeparturePerfusionCentre, ";
            strSQL += "CONCAT(IF(t1.ArrivalTransplantHospitalDate IS NULL, 'NA', DATE_FORMAT(t1.ArrivalTransplantHospitalDate, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.ArrivalTransplantHospitalTime IS NULL, 'NA', DATE_FORMAT(t1.ArrivalTransplantHospitalTime, '%H:%i')))  ArrivalTransplantHospital, ";
            strSQL += "DATE_FORMAT(t2.DateOfBirthDonor, '%d/%m/%Y') Date_OfBirthDonor ";
            strSQL += "FROM kidneyproceduredata t1 ";
            strSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialID=?TrialID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = sqldsGV1;
            sqldsGV1.SelectCommand = strSQL;
            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();
            lblGV1.Text = "Summary of Kidney Allocation Data.";

            //if (string.IsNullOrEmpty(Request.QueryString["KidneyProcedureDataID"]))
            //{
            //    if (GV1.Rows.Count == 1)
            //    {
            //        //cmdDelete.Enabled = true;
            //        //cmdDelete.Visible = true;
            //        //cmdAddData.Text = "Update Data";
            //        lblDescription.Text = "Update Kidney Procedure Data for " + Request.QueryString["TID"].ToString() + ". ";

            //        //if (strRecipientID != string.Empty)
            //        //{
            //        //    lblDescription.Text += " and RecipientID " + strRecipientID;
            //        //}
            //        AssignData();

            //    }
            //    else if (GV1.Rows.Count == 0)
            //    {
            //        //cmdDelete.Enabled = false;
            //        //cmdDelete.Visible = false;
            //        lblDescription.Text = "Add  Kidney Procedure Data for " + Request.QueryString["TID"].ToString() + ". ";
            //        //if (strRecipientID != string.Empty)
            //        //{
            //        //    lblDescription.Text += " and RecipientID " + strRecipientID;
            //        //}
            //    }
            //    else
            //    {
            //        throw new Exception("More than one Records exist. Click on TrialID to Delete an unwanted record.");
            //    }
            //}
            //else
            //{
            //    cmdDelete.Enabled = true;
            //    cmdDelete.Visible = true;
            //    //cmdAddData.Text = "Update Data";
            //    lblDescription.Text = "Update Donor General Procedure Data for " + Request.QueryString["TID"].ToString() + ". ";

            //    AssignData();
            //}


            
            

        }
        catch (Exception excep)
        {
            lblGV1.Text = excep.Message + " An error occured while binding the page.";
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


    protected void AssignData()
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            string STRSQL = "SELECT t1.*, t2.DonorID  FROM  kidneyproceduredata  t1 ";
            STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "WHERE t1.TrialID=?TrialID ";
            STRSQL += "AND t1.Side=?Side ";
            if (!string.IsNullOrEmpty(Request.QueryString["KidneyProcedureDataID"]))
            {
                STRSQL += "AND t1.KidneyProcedureDataID=?KidneyProcedureDataID ";
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

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"];
            MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = ddSide.SelectedValue;

            if (!string.IsNullOrEmpty(Request.QueryString["KidneyProcedureDataID"]))
            {
                MyCMD.Parameters.Add("?KidneyProcedureDataID", MySqlDbType.Int32).Value = Request.QueryString["KidneyProcedureDataID"];
            }


            MyCONN.Open();

            try
            {
                using (MySqlDataReader myDr = MyCMD.ExecuteReader())
                {
                    if (myDr.HasRows)
                    {
                        string strIncompleteColour = ConstantsGeneral.IncompleteColour;

                        while (myDr.Read())
                        {

                            //if (!DBNull.Value.Equals(myDr["Side"]))
                            //{
                            //    ddSide.SelectedValue = (string)(myDr["Side"]);
                            //}

                            //ddSide_SelectedIndexChanged(this, EventArgs.Empty);

                            if (!DBNull.Value.Equals(myDr["TransplantTechnician"]))
                            {
                                txtTransplantTechnician.Text = (string)(myDr["TransplantTechnician"]);
                            }

                            if (lblTransplantTechnician.Font.Bold==true)
                            {
                                if(txtTransplantTechnician.Text==string.Empty)
                                {
                                    lblTransplantTechnician.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }
                            
                            if (!DBNull.Value.Equals(myDr["DonorTechnicianPhoneDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DonorTechnicianPhoneDate"].ToString()) == true)
                                {
                                    txtDonorTechnicianPhoneDate.Text = Convert.ToDateTime(myDr["DonorTechnicianPhoneDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DonorTechnicianPhoneTime"]))
                            {
                                if (myDr["DonorTechnicianPhoneTime"].ToString().Length >= 5)
                                {
                                    txtDonorTechnicianPhoneTime.Text = myDr["DonorTechnicianPhoneTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblDonorTechnicianPhoneDate.Font.Bold == true)
                            {
                                if (txtDonorTechnicianPhoneDate.Text == string.Empty || txtDonorTechnicianPhoneTime.Text==string.Empty)
                                {
                                    lblDonorTechnicianPhoneDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["TechnicianDonorProcedure"]))
                            {
                                txtTechnicianDonorProcedure.Text = (string)(myDr["TechnicianDonorProcedure"]);
                            }

                            if (lblTransplantTechnician.Font.Bold == true)
                            {
                                if (txtTransplantTechnician.Text == string.Empty)
                                {
                                    lblTransplantTechnician.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["TransplantHospital"]))
                            {
                                txtTransplantHospital.Text = (string)(myDr["TransplantHospital"]);
                            }

                            if (lblTransplantHospital.Font.Bold == true)
                            {
                                if (txtTransplantHospital.Text == string.Empty)
                                {
                                    lblTransplantHospital.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["TransplantHospitalContact"]))
                            {
                                txtTransplantHospitalContact.Text = (string)(myDr["TransplantHospitalContact"]);
                            }

                            if (lblTransplantTechnician.Font.Bold == true)
                            {
                                if (txtTransplantTechnician.Text == string.Empty)
                                {
                                    lblTransplantTechnician.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["TransplantHospitalContactPhone"]))
                            {
                                txtTransplantHospitalContactPhone.Text = (string)(myDr["TransplantHospitalContactPhone"]);
                            }

                            if (lblTransplantHospital.Font.Bold == true)
                            {
                                if (txtTransplantHospital.Text == string.Empty)
                                {
                                    lblTransplantHospital.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ScheduledTransplantStartDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["ScheduledTransplantStartDate"].ToString()) == true)
                                {
                                    txtScheduledTransplantStartDate.Text = Convert.ToDateTime(myDr["ScheduledTransplantStartDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ScheduledTransplantStartTime"]))
                            {
                                if (myDr["ScheduledTransplantStartTime"].ToString().Length >= 5)
                                {
                                    txtScheduledTransplantStartTime.Text = myDr["ScheduledTransplantStartTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblScheduledTransplantStartDate.Font.Bold == true)
                            {
                                if (txtScheduledTransplantStartDate.Text == string.Empty || txtScheduledTransplantStartTime.Text ==string.Empty)
                                {
                                    lblScheduledTransplantStartDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["TechnicianArrivalPerfusionCentreDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["TechnicianArrivalPerfusionCentreDate"].ToString()) == true)
                                {
                                    txtTechnicianArrivalPerfusionCentreDate.Text = Convert.ToDateTime(myDr["TechnicianArrivalPerfusionCentreDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["TechnicianArrivalPerfusionCentreTime"]))
                            {
                                if (myDr["TechnicianArrivalPerfusionCentreTime"].ToString().Length >= 5)
                                {
                                    txtTechnicianArrivalPerfusionCentreTime.Text = myDr["TechnicianArrivalPerfusionCentreTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblTechnicianArrivalPerfusionCentreDate.Font.Bold == true)
                            {
                                if (txtTechnicianArrivalPerfusionCentreDate.Text == string.Empty || txtTechnicianArrivalPerfusionCentreTime.Text==string.Empty)
                                {
                                    lblTechnicianArrivalPerfusionCentreDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["TechnicianDeparturePerfusionCentreDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["TechnicianDeparturePerfusionCentreDate"].ToString()) == true)
                                {
                                    txtTechnicianDeparturePerfusionCentreDate.Text = Convert.ToDateTime(myDr["TechnicianDeparturePerfusionCentreDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["TechnicianDeparturePerfusionCentreTime"]))
                            {
                                if (myDr["TechnicianDeparturePerfusionCentreTime"].ToString().Length >= 5)
                                {
                                    txtTechnicianDeparturePerfusionCentreTime.Text = myDr["TechnicianDeparturePerfusionCentreTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblTechnicianDeparturePerfusionCentreDate.Font.Bold == true)
                            {
                                if (txtTechnicianDeparturePerfusionCentreDate.Text == string.Empty || txtTechnicianDeparturePerfusionCentreTime.Text==string.Empty)
                                {
                                    lblTechnicianDeparturePerfusionCentreDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ArrivalTransplantHospitalDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["ArrivalTransplantHospitalDate"].ToString()) == true)
                                {
                                    txtArrivalTransplantHospitalDate.Text = Convert.ToDateTime(myDr["ArrivalTransplantHospitalDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ArrivalTransplantHospitalTime"]))
                            {
                                if (myDr["ArrivalTransplantHospitalTime"].ToString().Length >= 5)
                                {
                                    txtArrivalTransplantHospitalTime.Text = myDr["ArrivalTransplantHospitalTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblArrivalTransplantHospitalDate.Font.Bold == true)
                            {
                                if (txtArrivalTransplantHospitalDate.Text == string.Empty || txtArrivalTransplantHospitalTime.Text ==string.Empty)
                                {
                                    lblArrivalTransplantHospitalDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["Comments"]))
                            {
                                txtComments.Text = (string)(myDr["Comments"]);
                            }

                            if (!DBNull.Value.Equals(myDr["Reallocated"]))
                            {
                                rblReallocated.SelectedValue = (string)(myDr["Reallocated"]);
                            }

                            if (lblReallocated.Font.Bold==true)
                            {
                                if (rblReallocated.SelectedIndex == -1)
                                {
                                    lblReallocated.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                                else
                                {
                                    lblReallocated.ForeColor = System.Drawing.Color.FromName("Black");
                                }
                            }
                            //if (lblReallocated.Font.Bold == true)
                            //{
                            //    if (rblReallocated.SelectedIndex == -1)
                            //    {
                            //        lblReallocated.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                            //    }
                            //}

                            //rblReallocated_SelectedIndexChanged(this, EventArgs.Empty);

                            if (!DBNull.Value.Equals(myDr["ReasonReallocated"]))
                            {
                                rblReasonReallocated.SelectedValue = (string)(myDr["ReasonReallocated"]);
                            }

                            if (lblReasonReallocated.Font.Bold == true)
                            {
                                if (rblReasonReallocated.SelectedIndex == -1)
                                {
                                    lblReasonReallocated.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ReasonReallocatedOther"]))
                            {
                                txtReasonReallocatedOther.Text = (string)(myDr["ReasonReallocatedOther"]);
                            }

                            if (lblReasonReallocatedOther.Font.Bold == true)
                            {
                                if (txtReasonReallocatedOther.Text == string.Empty)
                                {
                                    lblReasonReallocatedOther.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["NewRecipientHospitalContact"]))
                            {
                                txtNewRecipientHospitalContact.Text = (string)(myDr["NewRecipientHospitalContact"]);
                            }

                            if (lblNewRecipientHospitalContact.Font.Bold == true)
                            {
                                if (txtNewRecipientHospitalContact.Text == string.Empty)
                                {
                                    lblNewRecipientHospitalContact.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["NewRecipientHospitalContactPhone"]))
                            {
                                txtNewRecipientHospitalContactPhone.Text = (string)(myDr["NewRecipientHospitalContactPhone"]);
                            }

                            if (lblNewRecipientHospitalContactPhone.Font.Bold == true)
                            {
                                if (txtNewRecipientHospitalContactPhone.Text == string.Empty)
                                {
                                    lblNewRecipientHospitalContactPhone.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["NewTransplantHospital"]))
                            {
                                txtNewTransplantHospital.Text = (string)(myDr["NewTransplantHospital"]);
                            }

                            if (lblNewTransplantHospital.Font.Bold == true)
                            {
                                if (txtNewTransplantHospital.Text == string.Empty)
                                {
                                    lblNewTransplantHospital.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["NewScheduledTransplantStartDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["NewScheduledTransplantStartDate"].ToString()) == true)
                                {
                                    txtNewScheduledTransplantStartDate.Text = Convert.ToDateTime(myDr["NewScheduledTransplantStartDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["NewScheduledTransplantStartTime"]))
                            {
                                if (myDr["NewScheduledTransplantStartTime"].ToString().Length >= 5)
                                {
                                    txtNewScheduledTransplantStartTime.Text = myDr["NewScheduledTransplantStartTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblNewScheduledTransplantStartDate.Font.Bold == true)
                            {
                                if (txtNewScheduledTransplantStartDate.Text == string.Empty || txtNewScheduledTransplantStartTime.Text==string.Empty)
                                {
                                    lblNewScheduledTransplantStartDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DepartureFirstTransplantHospitalDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DepartureFirstTransplantHospitalDate"].ToString()) == true)
                                {
                                    txtDepartureFirstTransplantHospitalDate.Text = Convert.ToDateTime(myDr["DepartureFirstTransplantHospitalDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DepartureFirstTransplantHospitalTime"]))
                            {
                                if (myDr["DepartureFirstTransplantHospitalTime"].ToString().Length >= 5)
                                {
                                    txtDepartureFirstTransplantHospitalTime.Text = myDr["DepartureFirstTransplantHospitalTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblDepartureFirstTransplantHospitalDate.Font.Bold == true)
                            {
                                if (txtDepartureFirstTransplantHospitalDate.Text == string.Empty || txtDepartureFirstTransplantHospitalTime.Text==string.Empty)
                                {
                                    lblDepartureFirstTransplantHospitalDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ArrivalNewTransplantHospitalDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["ArrivalNewTransplantHospitalDate"].ToString()) == true)
                                {
                                    txtArrivalNewTransplantHospitalDate.Text = Convert.ToDateTime(myDr["ArrivalNewTransplantHospitalDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ArrivalNewTransplantHospitalTime"]))
                            {
                                if (myDr["ArrivalNewTransplantHospitalTime"].ToString().Length >= 5)
                                {
                                    txtArrivalNewTransplantHospitalTime.Text = myDr["ArrivalNewTransplantHospitalTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblArrivalNewTransplantHospitalDate.Font.Bold == true)
                            {
                                if (txtArrivalNewTransplantHospitalDate.Text == string.Empty || txtArrivalNewTransplantHospitalTime.Text==string.Empty)
                                {
                                    lblArrivalNewTransplantHospitalDate.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }


                            if (!DBNull.Value.Equals(myDr["NewComments"]))
                            {
                                txtNewComments.Text = (string)(myDr["NewComments"]);
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
                            pnlSideSelected.Visible = true;

                        }                       

                    }
                    else //reset to empty values
                    {
                        ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));

                        if (mpCPH !=null)
                        {
                            ContentPlaceHolder mpCPH_SCMP = (ContentPlaceHolder)(mpCPH.FindControl(strSpecimenContents));

                            //get the panel

                            Panel pnl = (Panel)(mpCPH_SCMP.FindControl(strPanel));

                            if (pnl !=null)
                            {
                                Label lbl;
                                TextBox tb;
                                RadioButtonList rbl;
                                Panel pnlNested;

                                foreach (Control c in pnl.Controls)
                                {

                                    if (c.GetType() == typeof(Label))
                                    {
                                        lbl = (Label)c;

                                        lbl.ForeColor = System.Drawing.Color.FromName(strDefaultFontColor);
                                        //lblComments.Text += tb.ID;
                                    }

                                    if (c.GetType() == typeof(TextBox))
                                    {
                                        tb = (TextBox)c;
                                        tb.Text = string.Empty;

                                        //lblComments.Text += tb.ID;
                                    }

                                    else if (c.GetType() == typeof(RadioButtonList))
                                    {
                                        rbl = (RadioButtonList)c;
                                        rbl.SelectedIndex = -1;

                                        //lblComments.Text += rbl.ID;
                                    }
                                    else if (c.GetType() == typeof(Panel))
                                    {
                                        pnlNested = (Panel)(c);

                                        if (pnlNested !=null)
                                        {
                                            foreach (Control cNested in pnlNested.Controls)
                                            {

                                                if (cNested.GetType() == typeof(Label))
                                                {
                                                    lbl = (Label)cNested;

                                                    lbl.ForeColor = System.Drawing.Color.FromName(strDefaultFontColor);
                                                    //lblComments.Text += tb.ID;
                                                }
                                                
                                                if (cNested.GetType() == typeof(TextBox))
                                                {
                                                    tb = (TextBox)cNested;
                                                    tb.Text = string.Empty;

                                                    //lblComments.Text += tb.ID;
                                                }

                                                else if (cNested.GetType() == typeof(RadioButtonList))
                                                {
                                                    rbl = (RadioButtonList)cNested;
                                                    rbl.SelectedIndex = -1;

                                                    //lblComments.Text += rbl.ID;
                                                }
                                                else
                                                {
                                                    //do nothing
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //do nothing
                                        //lblComments.Text += c.ID;
                                    }
                                    //lblComments.Text += c.ID + "<br/>";
                                } 
                            }

                                                        
                            cmdDelete.Visible = false;

                        }

                        
                    }
                    rblReallocated_SelectedIndexChanged(this, EventArgs.Empty);

                    
                }

                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }

                Label lblSide;

                foreach (GridViewRow row in GV1.Rows)
                {
                    lblSide = (Label)row.FindControl("lblSide");

                    if (lblSide != null)
                    {
                        
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            if (lblSide.Text == ddSide.SelectedValue)
                            {
                                row.BackColor = System.Drawing.Color.LightBlue;
                            }
                            else
                            {
                                if (row.RowState == DataControlRowState.Alternate)
                                {
                                    row.BackColor = GV1.AlternatingRowStyle.BackColor;
                                }
                                else
                                {
                                    row.BackColor = GV1.RowStyle.BackColor;
                                }


                            }
                        }
                    }
                }


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
            lblUserMessages.Text = ex.Message + " An error occured while assiging data.";
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

    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //check if only one record exists
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM kidneyproceduredata WHERE TrialID=?TrialID AND Side=?Side";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), "?Side", ddSide.SelectedValue, STRCONN));

            if (intCountFind < 0)
            {
                throw new Exception("An error occured while checking if Record exists for deletion.");
            }

            if (intCountFind == 0)
            {
                throw new Exception("No Record exists for deletion.");
            }

            if (intCountFind > 1)
            {
                if (String.IsNullOrEmpty(Request.QueryString["KidneyProcedureDataID"]))
                {
                    throw new Exception("More than one Record exists for deletion. Click on TrialID in the table below to select data to delete.");
                }

            }

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM kidneyproceduredata ";
            STRSQL += "WHERE TrialID=?TrialID ";
            STRSQL += "AND Side=?Side ";
            if (intCountFind > 1)
            {
                STRSQL += "AND KidneyProcedureDataID=?KidneyProcedureDataID ";
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

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();

            MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = ddSide.SelectedValue;

            if (!String.IsNullOrEmpty(Request.QueryString["KidneyProcedureDataID"]))
            {
                MyCMD.Parameters.Add("?KidneyProcedureDataID", MySqlDbType.VarChar).Value = Request.QueryString["KidneyProcedureDataID"].ToString();
            }

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();
                BindData();

                lblUserMessages.Text = "Data Deleted.";
            }

            catch (System.Exception ex)
            {
                lblUserMessages.Text = ex.Message + " An error occured while executing delete query.";
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


    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (ddSide.SelectedValue==STR_DD_UNKNOWN_SELECTION)
            {
                throw new Exception("Please Select a Side.");
            }

            DateTime dteDonorTechnicianPhone = DateTime.MinValue;
            DateTime date_DonorTechnicianPhone = DateTime.MinValue;
            
            DateTime dteScheduledTransplantStart = DateTime.MinValue;
            DateTime date_ScheduledTransplantStart = DateTime.MinValue;
            
            DateTime dteTechnicianArrivalPerfusionCentre = DateTime.MinValue;
            DateTime date_TechnicianArrivalPerfusionCentre = DateTime.MinValue;

            DateTime dteTechnicianDeparturePerfusionCentre = DateTime.MinValue;
            DateTime date_TechnicianDeparturePerfusionCentre = DateTime.MinValue;

            DateTime dteArrivalTransplantHospital = DateTime.MinValue;
            DateTime date_ArrivalTransplantHospital = DateTime.MinValue;
            
            DateTime dteNewScheduledTransplantStart = DateTime.MinValue;
            DateTime date_NewScheduledTransplantStart = DateTime.MinValue;
            
            DateTime dteDepartureFirstTransplantHospital = DateTime.MinValue;
            DateTime date_DepartureFirstTransplantHospital = DateTime.MinValue;
            
            DateTime dteArrivalNewTransplantHospital = DateTime.MinValue;
            DateTime date_ArrivalNewTransplantHospital = DateTime.MinValue;
            //if (txtTransplantTechnician.Text == string.Empty)
            //{
            //    throw new Exception("Please Enter the Name of Transplant Technician.");
            //}

            if (txtDonorTechnicianPhoneDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtDonorTechnicianPhoneDate.Text) == false)
                {
                    throw new Exception("Please enter 'Date' for '" + lblDonorTechnicianPhoneDate.Text+ "' in the correct format.");
                }

                date_DonorTechnicianPhone = Convert.ToDateTime(txtDonorTechnicianPhoneDate.Text);
                if (Convert.ToDateTime(txtDonorTechnicianPhoneDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblDonorTechnicianPhoneDate.Text + "' Date can not be later than Today's date.");
                }

            }
            if (txtDonorTechnicianPhoneTime.Text != string.Empty && txtDonorTechnicianPhoneTime.Text!="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtDonorTechnicianPhoneTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblDonorTechnicianPhoneDate.Text + "'' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtDonorTechnicianPhoneTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblDonorTechnicianPhoneDate.Text + "'' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtDonorTechnicianPhoneTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblDonorTechnicianPhoneDate.Text + "'' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtDonorTechnicianPhoneTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblDonorTechnicianPhoneDate.Text + "'' Time Hour Minute not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtDonorTechnicianPhoneDate.Text) && GeneralRoutines.IsDate(txtDonorTechnicianPhoneTime.Text))
            {
                //dteDonorTechnicianPhone = new DateTime();
                dteDonorTechnicianPhone = Convert.ToDateTime(txtDonorTechnicianPhoneDate.Text + " " + txtDonorTechnicianPhoneTime.Text);

                if (dteDonorTechnicianPhone>DateTime.Now)
                {
                    throw new Exception("'" + lblDonorTechnicianPhoneDate.Text + "' can not be later than current Date/Time.");
                }
            }

            if (txtScheduledTransplantStartDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtScheduledTransplantStartDate.Text) == false)
                {
                    throw new Exception("Please enter 'Date' for '" + lblScheduledTransplantStartDate.Text + "' in the correct format.");
                }

                date_ScheduledTransplantStart = Convert.ToDateTime(txtScheduledTransplantStartDate.Text);

                if (Convert.ToDateTime(txtScheduledTransplantStartDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblScheduledTransplantStartDate.Text + "' Date can not be greater than Today's date.");
                }

            }
            if (txtScheduledTransplantStartTime.Text != string.Empty && txtScheduledTransplantStartTime.Text !="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtScheduledTransplantStartTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblScheduledTransplantStartDate.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtScheduledTransplantStartTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblScheduledTransplantStartDate.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtScheduledTransplantStartTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblScheduledTransplantStartDate.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtScheduledTransplantStartTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblScheduledTransplantStartDate.Text + "' Time Hour Minute not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtScheduledTransplantStartDate.Text ) && GeneralRoutines.IsDate(txtScheduledTransplantStartTime.Text))
            {
                //dteScheduledTransplantStart = new DateTime();
                dteScheduledTransplantStart = Convert.ToDateTime(txtScheduledTransplantStartDate.Text + " " + txtScheduledTransplantStartTime.Text);

                if (dteScheduledTransplantStart > DateTime.Now)
                {
                    throw new Exception("'" + lblScheduledTransplantStartDate.Text + "' can not be later than current Date/Time.");
                }

            }

            if (txtTechnicianArrivalPerfusionCentreDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtTechnicianArrivalPerfusionCentreDate.Text) == false)
                {
                    throw new Exception("Please enter 'Date' for '" + lblTechnicianArrivalPerfusionCentreDate.Text + "' in the correct format.");
                }


                date_TechnicianArrivalPerfusionCentre = Convert.ToDateTime(txtTechnicianArrivalPerfusionCentreDate.Text);

                if (Convert.ToDateTime(txtTechnicianArrivalPerfusionCentreDate.Text) > DateTime.Now)
                {
                    throw new Exception("'"+ lblTechnicianArrivalPerfusionCentreDate.Text + "' Date can not be later than Today's date.");
                }

            }
            if (txtTechnicianArrivalPerfusionCentreTime.Text != string.Empty && txtTechnicianArrivalPerfusionCentreTime.Text !="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtTechnicianArrivalPerfusionCentreTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblTechnicianArrivalPerfusionCentreDate.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtTechnicianArrivalPerfusionCentreTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblTechnicianArrivalPerfusionCentreDate.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtTechnicianArrivalPerfusionCentreTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblTechnicianArrivalPerfusionCentreDate.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtTechnicianArrivalPerfusionCentreTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblTechnicianArrivalPerfusionCentreDate.Text + "' Time Hour Minute not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtTechnicianArrivalPerfusionCentreDate.Text) && GeneralRoutines.IsDate(txtTechnicianArrivalPerfusionCentreTime.Text))
            {
                //dteTechnicianArrivalPerfusionCentre = new DateTime();
                dteTechnicianArrivalPerfusionCentre = Convert.ToDateTime(txtTechnicianArrivalPerfusionCentreDate.Text + " " + txtTechnicianArrivalPerfusionCentreTime.Text);

                if (dteTechnicianArrivalPerfusionCentre > DateTime.Now)
                {
                    throw new Exception("'" + lblTechnicianArrivalPerfusionCentreDate.Text + "' can not be later than current Date/Time.");
                }

            }


            if (txtTechnicianDeparturePerfusionCentreDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtTechnicianDeparturePerfusionCentreDate.Text) == false)
                {
                    throw new Exception("Please enter 'Date' for '" + lblTechnicianDeparturePerfusionCentreDate.Text + "' in the correct format.");
                }

                date_TechnicianDeparturePerfusionCentre = Convert.ToDateTime(txtTechnicianDeparturePerfusionCentreDate.Text);
                if (Convert.ToDateTime(txtTechnicianArrivalPerfusionCentreDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblTechnicianDeparturePerfusionCentreDate.Text + "' Date can not be later than Today's date.");
                }

            }
            if (txtTechnicianDeparturePerfusionCentreTime.Text != string.Empty && txtTechnicianDeparturePerfusionCentreTime.Text !="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtTechnicianDeparturePerfusionCentreTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblTechnicianDeparturePerfusionCentreDate.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtTechnicianDeparturePerfusionCentreTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblTechnicianDeparturePerfusionCentreDate.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtTechnicianDeparturePerfusionCentreTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblTechnicianDeparturePerfusionCentreDate.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtTechnicianDeparturePerfusionCentreTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblTechnicianDeparturePerfusionCentreDate.Text + "' Time Hour Minute not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtTechnicianDeparturePerfusionCentreDate.Text ) && GeneralRoutines.IsDate(txtTechnicianDeparturePerfusionCentreTime.Text))
            {
                //dteTechnicianDeparturePerfusionCentre = new DateTime();
                dteTechnicianDeparturePerfusionCentre = Convert.ToDateTime(txtTechnicianDeparturePerfusionCentreDate.Text + " " + txtTechnicianDeparturePerfusionCentreTime.Text);

                if (dteTechnicianDeparturePerfusionCentre > DateTime.Now)
                {
                    throw new Exception("'" + lblTechnicianDeparturePerfusionCentreDate.Text + "' can not be later than current Date/Time.");
                }

            }

            if (txtArrivalTransplantHospitalDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtArrivalTransplantHospitalDate.Text) == false)
                {
                    throw new Exception("Please enter 'Date' for '" + lblArrivalTransplantHospitalDate.Text + "' in the correct format.");
                }


                date_ArrivalTransplantHospital = Convert.ToDateTime(txtArrivalTransplantHospitalDate.Text);

                if (Convert.ToDateTime(txtTechnicianArrivalPerfusionCentreDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblArrivalTransplantHospitalDate.Text + "' Date can not be later than Today's date.");
                }

            }
            if (txtArrivalTransplantHospitalTime.Text != string.Empty && txtArrivalTransplantHospitalTime.Text != "__:__")
            {
                if (GeneralRoutines.IsNumeric(txtArrivalTransplantHospitalTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblArrivalTransplantHospitalDate.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtArrivalTransplantHospitalTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblArrivalTransplantHospitalDate.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtArrivalTransplantHospitalTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblArrivalTransplantHospitalDate.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtArrivalTransplantHospitalTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblArrivalTransplantHospitalDate.Text + "' Time Hour Minute not be greater than 59.");
                }
            }


            if (GeneralRoutines.IsDate(txtArrivalTransplantHospitalDate.Text) && GeneralRoutines.IsDate(txtArrivalTransplantHospitalTime.Text))
            {
                //dteArrivalTransplantHospital = new DateTime();
                dteArrivalTransplantHospital = Convert.ToDateTime(txtArrivalTransplantHospitalDate.Text + " " + txtArrivalTransplantHospitalTime.Text);

                if (dteArrivalTransplantHospital > DateTime.Now)
                {
                    throw new Exception("'" + lblArrivalTransplantHospitalDate.Text + "' can not be later than current Date/Time.");
                }

            }

            if (txtNewScheduledTransplantStartDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtNewScheduledTransplantStartDate.Text) == false)
                {
                    throw new Exception("Please enter 'New Date' of '" + lblNewScheduledTransplantStartDate.Text + "' in the correct format.");
                }


                date_NewScheduledTransplantStart = Convert.ToDateTime(txtNewScheduledTransplantStartDate.Text);

                if (Convert.ToDateTime(txtNewScheduledTransplantStartDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblNewScheduledTransplantStartDate.Text + "' Date can not be greater than Today's date.");
                }

            }
            if (txtNewScheduledTransplantStartTime.Text != string.Empty && txtNewScheduledTransplantStartTime.Text !="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtNewScheduledTransplantStartTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblNewScheduledTransplantStartDate.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtNewScheduledTransplantStartTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblNewScheduledTransplantStartDate.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtNewScheduledTransplantStartTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblNewScheduledTransplantStartDate.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtNewScheduledTransplantStartTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblNewScheduledTransplantStartDate.Text + "' Time Hour Minute not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtNewScheduledTransplantStartDate.Text) && GeneralRoutines.IsDate(txtNewScheduledTransplantStartTime.Text ))
            {
                //dteNewScheduledTransplantStart = new DateTime();
                dteNewScheduledTransplantStart = Convert.ToDateTime(txtNewScheduledTransplantStartDate.Text + " " + txtNewScheduledTransplantStartTime.Text);

                if (dteNewScheduledTransplantStart > DateTime.Now)
                {
                    throw new Exception("'" + lblNewScheduledTransplantStartDate.Text + "' can not be later than current Date/Time.");
                }

            }


            if (txtDepartureFirstTransplantHospitalDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtDepartureFirstTransplantHospitalDate.Text) == false)
                {
                    throw new Exception("Please enter 'Date' for '" + lblDepartureFirstTransplantHospitalDate.Text + "' in the correct format.");
                }

                date_DepartureFirstTransplantHospital = Convert.ToDateTime(txtDepartureFirstTransplantHospitalDate.Text);

                if (Convert.ToDateTime(txtDepartureFirstTransplantHospitalDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblDepartureFirstTransplantHospitalDate.Text + "' Date can not be later than Today's date.");
                }

            }
            if (txtDepartureFirstTransplantHospitalTime.Text != string.Empty && txtDepartureFirstTransplantHospitalTime.Text !="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtDepartureFirstTransplantHospitalTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblDepartureFirstTransplantHospitalDate.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtDepartureFirstTransplantHospitalTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblDepartureFirstTransplantHospitalDate.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtDepartureFirstTransplantHospitalTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblDepartureFirstTransplantHospitalDate.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtDepartureFirstTransplantHospitalTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblDepartureFirstTransplantHospitalDate.Text + "' Time Hour Minute not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtDepartureFirstTransplantHospitalDate.Text) && GeneralRoutines.IsDate(txtDepartureFirstTransplantHospitalTime.Text))
            {
                //dteDepartureFirstTransplantHospital = new DateTime();
                dteDepartureFirstTransplantHospital = Convert.ToDateTime(txtDepartureFirstTransplantHospitalDate.Text + " " + txtDepartureFirstTransplantHospitalTime.Text);

                if (dteDepartureFirstTransplantHospital > DateTime.Now)
                {
                    throw new Exception("'" + lblDepartureFirstTransplantHospitalDate.Text + "' can not be later than current Date/Time.");
                }

            }


            if (txtArrivalNewTransplantHospitalDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtArrivalNewTransplantHospitalDate.Text) == false)
                {
                    throw new Exception("Please enter 'Date' of '" + lblArrivalNewTransplantHospitalDate.Text +  "'in the correct format.");
                }

                date_ArrivalNewTransplantHospital = Convert.ToDateTime(txtArrivalNewTransplantHospitalDate.Text);

                if (Convert.ToDateTime(txtArrivalNewTransplantHospitalDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblArrivalNewTransplantHospitalDate.Text + "' Date can not be greater than Today's date.");
                }

            }
            if (txtArrivalNewTransplantHospitalTime.Text != string.Empty && txtArrivalNewTransplantHospitalTime.Text !="__:__")
            {
                if (GeneralRoutines.IsNumeric(txtArrivalNewTransplantHospitalTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblArrivalNewTransplantHospitalDate.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtArrivalNewTransplantHospitalTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblArrivalNewTransplantHospitalDate.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtArrivalNewTransplantHospitalTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblArrivalNewTransplantHospitalDate.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtArrivalNewTransplantHospitalTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblArrivalNewTransplantHospitalDate.Text + "' Time Hour Minute not be greater than 59.");
                }
            }

            if (GeneralRoutines.IsDate(txtArrivalNewTransplantHospitalDate.Text) && GeneralRoutines.IsDate(txtArrivalNewTransplantHospitalTime.Text))
            {

                //dteArrivalNewTransplantHospital = new DateTime();
                dteArrivalNewTransplantHospital = Convert.ToDateTime(txtArrivalNewTransplantHospitalDate.Text + " " + txtArrivalNewTransplantHospitalTime.Text);

                if (dteArrivalNewTransplantHospital > DateTime.Now)
                {
                    throw new Exception("'" + lblArrivalNewTransplantHospitalDate.Text + "' can not be later than current Date/Time.");
                }

            }


            if (chkDataLocked.Checked == true)
            {
                if (txtReasonModified.Text == string.Empty)
                {
                    throw new Exception("Since this record is locked, please enter " + lblReasonModified.Text);
                }
            }

            string STRSQL = "";

            STRSQL += "INSERT INTO kidneyproceduredata ";
            STRSQL += "(TrialID, Side, TransplantTechnician, DonorTechnicianPhoneDate, DonorTechnicianPhoneTime, TechnicianDonorProcedure, TransplantHospitalContact,";
            STRSQL += "TransplantHospitalContactPhone, TransplantHospital, ScheduledTransplantStartDate, ScheduledTransplantStartTime,";
            STRSQL += "TechnicianArrivalPerfusionCentreDate, TechnicianArrivalPerfusionCentreTime, TechnicianDeparturePerfusionCentreDate, TechnicianDeparturePerfusionCentreTime,";
            //STRSQL += "CarNumber, CarLicensePlate, ArrivalTransplantHospitalDate, ArrivalTransplantHospitalTime, Comments, ";
            STRSQL += "ArrivalTransplantHospitalDate, ArrivalTransplantHospitalTime, Comments, ";
            STRSQL += "Reallocated, ReasonReallocated, ReasonReallocatedOther, NewRecipientHospitalContact, NewRecipientHospitalContactPhone, NewTransplantHospital, ";
            STRSQL += "NewScheduledTransplantStartDate, NewScheduledTransplantStartTime, DepartureFirstTransplantHospitalDate, DepartureFirstTransplantHospitalTime, ";
            STRSQL += "ArrivalNewTransplantHospitalDate, ArrivalNewTransplantHospitalTime, NewComments, ";
            STRSQL += "DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID, ?Side, ?TransplantTechnician, ?DonorTechnicianPhoneDate, ?DonorTechnicianPhoneTime, ?TechnicianDonorProcedure, ?TransplantHospitalContact,";
            STRSQL += "?TransplantHospitalContactPhone, ?TransplantHospital, ?ScheduledTransplantStartDate, ?ScheduledTransplantStartTime,";
            STRSQL += "?TechnicianArrivalPerfusionCentreDate, ?TechnicianArrivalPerfusionCentreTime, ?TechnicianDeparturePerfusionCentreDate, ?TechnicianDeparturePerfusionCentreTime,";
            //STRSQL += "?CarNumber, ?CarLicensePlate, ?ArrivalTransplantHospitalDate, ?ArrivalTransplantHospitalTime, ?Comments, ?";
            STRSQL += "?ArrivalTransplantHospitalDate, ?ArrivalTransplantHospitalTime, ?Comments, ?";
            STRSQL += "?Reallocated, ?ReasonReallocated, ?ReasonReallocatedOther, ?NewRecipientHospitalContact, ?NewRecipientHospitalContactPhone, ?NewTransplantHospital, ?";
            STRSQL += "?NewScheduledTransplantStartDate, ?NewScheduledTransplantStartTime, ?DepartureFirstTransplantHospitalDate, ?DepartureFirstTransplantHospitalTime, ?";
            STRSQL += "?ArrivalNewTransplantHospitalDate, ?ArrivalNewTransplantHospitalTime, ?NewComments, ?";
            STRSQL += "?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = "";
            STRSQL_UPDATE += "UPDATE kidneyproceduredata SET ";
            STRSQL_UPDATE += "TransplantTechnician=?TransplantTechnician, DonorTechnicianPhoneDate=?DonorTechnicianPhoneDate, DonorTechnicianPhoneTime=?DonorTechnicianPhoneTime, ";
            STRSQL_UPDATE += "TechnicianDonorProcedure=?TechnicianDonorProcedure, TransplantHospitalContact=?TransplantHospitalContact,";
            STRSQL_UPDATE += "TransplantHospitalContactPhone=?TransplantHospitalContactPhone, TransplantHospital=?TransplantHospital, ";
            STRSQL_UPDATE += "ScheduledTransplantStartDate=?ScheduledTransplantStartDate, ScheduledTransplantStartTime=?ScheduledTransplantStartTime,";
            STRSQL_UPDATE += "TechnicianArrivalPerfusionCentreDate=?TechnicianArrivalPerfusionCentreDate, TechnicianArrivalPerfusionCentreTime=?TechnicianArrivalPerfusionCentreTime, ";
            STRSQL_UPDATE += "TechnicianDeparturePerfusionCentreDate=?TechnicianDeparturePerfusionCentreDate, TechnicianDeparturePerfusionCentreTime=?TechnicianDeparturePerfusionCentreTime,";
            //STRSQL_UPDATE += "CarNumber=?CarNumber, CarLicensePlate=?CarLicensePlate, ArrivalTransplantHospitalDate=?ArrivalTransplantHospitalDate,  ";
            STRSQL_UPDATE += "ArrivalTransplantHospitalDate=?ArrivalTransplantHospitalDate,  ";
            STRSQL_UPDATE += "ArrivalTransplantHospitalTime=?ArrivalTransplantHospitalTime, Comments=?Comments,";
            STRSQL_UPDATE += "Reallocated=?Reallocated, ReasonReallocated=?ReasonReallocated, ReasonReallocatedOther=?ReasonReallocatedOther, ";
            STRSQL_UPDATE += "NewRecipientHospitalContact=?NewRecipientHospitalContact,  NewRecipientHospitalContactPhone=?NewRecipientHospitalContactPhone, ";
            STRSQL_UPDATE += "NewTransplantHospital=?NewTransplantHospital,";
            STRSQL_UPDATE += "NewScheduledTransplantStartDate=?NewScheduledTransplantStartDate, NewScheduledTransplantStartTime=?NewScheduledTransplantStartTime, ";
            STRSQL_UPDATE += "DepartureFirstTransplantHospitalDate=?DepartureFirstTransplantHospitalDate, DepartureFirstTransplantHospitalTime=?DepartureFirstTransplantHospitalTime, ";
            STRSQL_UPDATE += "";
            STRSQL_UPDATE += "ArrivalNewTransplantHospitalDate=?ArrivalNewTransplantHospitalDate, ArrivalNewTransplantHospitalTime=?ArrivalNewTransplantHospitalTime,  ";
            STRSQL_UPDATE += "NewComments=?NewComments,";
            STRSQL_UPDATE += "DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialID=?TrialID AND Side=?Side ";


            // lock data if all required fields have been entered
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE kidneyproceduredata SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, ";
            STRSQL_LOCK += "LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialID=?TrialID AND (DataLocked IS NULL OR DataLocked=0) AND Side=?Side  ";




            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE kidneyproceduredata SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialID=?TrialID AND Side=?Side ";


            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM kidneyproceduredata WHERE TrialID=?TrialID AND Side=?Side ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalarTwo(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), "?Side", ddSide.SelectedValue, STRCONN));

            if (intCountFind == 1)
            {
                MyCMD.CommandText = STRSQL_UPDATE;
            }
            else if (intCountFind == 0)
            {
                MyCMD.CommandText = STRSQL;
            }
            else if (intCountFind > 1)
            {
                throw new Exception("More than one Kidney Allocation Data exists for this kidney. Click on Side in the table below to select data to delete a record. ");
            }
            else
            {
                throw new Exception("An error occured while checking if Kidney Allocation Data already exists in the database.");
            }


            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();

            MyCMD.Parameters.Add("?Side", MySqlDbType.VarChar).Value = ddSide.SelectedValue;

            if (txtTransplantTechnician.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?TransplantTechnician", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TransplantTechnician", MySqlDbType.VarChar).Value = txtTransplantTechnician.Text;
            }

            if (GeneralRoutines.IsDate(txtDonorTechnicianPhoneDate.Text) == false)
            {
                MyCMD.Parameters.Add("?DonorTechnicianPhoneDate", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DonorTechnicianPhoneDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtDonorTechnicianPhoneDate.Text);
            }

            if (txtDonorTechnicianPhoneTime.Text == string.Empty || txtDonorTechnicianPhoneTime.Text =="__:__")
            {
                MyCMD.Parameters.Add("?DonorTechnicianPhoneTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DonorTechnicianPhoneTime", MySqlDbType.VarChar).Value = txtDonorTechnicianPhoneTime.Text;
            }

            if (txtTechnicianDonorProcedure.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?TechnicianDonorProcedure", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TechnicianDonorProcedure", MySqlDbType.VarChar).Value = txtTechnicianDonorProcedure.Text;
            }

            if (txtTransplantHospitalContact.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?TransplantHospitalContact", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TransplantHospitalContact", MySqlDbType.VarChar).Value = txtTransplantHospitalContact.Text;
            }

            if (txtTransplantHospitalContactPhone.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?TransplantHospitalContactPhone", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TransplantHospitalContactPhone", MySqlDbType.VarChar).Value = txtTransplantHospitalContactPhone.Text;
            }

            if (txtTransplantHospital.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?TransplantHospital", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TransplantHospital", MySqlDbType.VarChar).Value = txtTransplantHospital.Text;
            }

            if (GeneralRoutines.IsDate(txtScheduledTransplantStartDate.Text) == false)
            {
                MyCMD.Parameters.Add("?ScheduledTransplantStartDate", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ScheduledTransplantStartDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtScheduledTransplantStartDate.Text);
            }

            if (txtScheduledTransplantStartTime.Text == string.Empty || txtScheduledTransplantStartTime.Text =="__:__")
            {
                MyCMD.Parameters.Add("?ScheduledTransplantStartTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ScheduledTransplantStartTime", MySqlDbType.VarChar).Value = txtScheduledTransplantStartTime.Text;
            }


            if (GeneralRoutines.IsDate(txtTechnicianArrivalPerfusionCentreDate.Text) == false)
            {
                MyCMD.Parameters.Add("?TechnicianArrivalPerfusionCentreDate", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TechnicianArrivalPerfusionCentreDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtTechnicianArrivalPerfusionCentreDate.Text);
            }

            if (txtTechnicianArrivalPerfusionCentreTime.Text == string.Empty || txtTechnicianArrivalPerfusionCentreTime.Text =="__:__")
            {
                MyCMD.Parameters.Add("?TechnicianArrivalPerfusionCentreTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TechnicianArrivalPerfusionCentreTime", MySqlDbType.VarChar).Value = txtTechnicianArrivalPerfusionCentreTime.Text;
            }

            if (GeneralRoutines.IsDate(txtTechnicianDeparturePerfusionCentreDate.Text) == false)
            {
                MyCMD.Parameters.Add("?TechnicianDeparturePerfusionCentreDate", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TechnicianDeparturePerfusionCentreDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtTechnicianDeparturePerfusionCentreDate.Text);
            }

            if (txtTechnicianDeparturePerfusionCentreTime.Text == string.Empty || txtTechnicianDeparturePerfusionCentreTime.Text =="__:__")
            {
                MyCMD.Parameters.Add("?TechnicianDeparturePerfusionCentreTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TechnicianDeparturePerfusionCentreTime", MySqlDbType.VarChar).Value = txtTechnicianDeparturePerfusionCentreTime.Text;
            }


            //if (txtCarNumber.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?CarNumber", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?CarNumber", MySqlDbType.VarChar).Value = txtCarNumber.Text;
            //}

            //if (txtCarLicensePlate.Text == string.Empty)
            //{
            //    MyCMD.Parameters.Add("?CarLicensePlate", MySqlDbType.VarChar).Value = DBNull.Value;
            //}
            //else
            //{
            //    MyCMD.Parameters.Add("?CarLicensePlate", MySqlDbType.VarChar).Value = txtCarLicensePlate.Text;
            //}


            if (GeneralRoutines.IsDate(txtArrivalTransplantHospitalDate.Text) == false)
            {
                MyCMD.Parameters.Add("?ArrivalTransplantHospitalDate", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ArrivalTransplantHospitalDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtArrivalTransplantHospitalDate.Text);
            }

            if (txtArrivalTransplantHospitalTime.Text == string.Empty || txtArrivalTransplantHospitalTime.Text =="__:__")
            {
                MyCMD.Parameters.Add("?ArrivalTransplantHospitalTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ArrivalTransplantHospitalTime", MySqlDbType.VarChar).Value = txtArrivalTransplantHospitalTime.Text;
            }

            if (string.IsNullOrEmpty(txtComments.Text))
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text;
            }

            if (rblReallocated.SelectedIndex == -1)
            {
                MyCMD.Parameters.Add("?Reallocated", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?Reallocated", MySqlDbType.VarChar).Value = rblReallocated.SelectedValue;
            }

            if (rblReasonReallocated.SelectedIndex==-1)
            {
                MyCMD.Parameters.Add("?ReasonReallocated", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ReasonReallocated", MySqlDbType.VarChar).Value = rblReasonReallocated.SelectedValue;
            }

            if (string.IsNullOrEmpty(txtReasonReallocatedOther.Text))
            {
                MyCMD.Parameters.Add("?ReasonReallocatedOther", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ReasonReallocatedOther", MySqlDbType.VarChar).Value = txtReasonReallocatedOther.Text;
            }


            if (txtNewRecipientHospitalContact.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?NewRecipientHospitalContact", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NewRecipientHospitalContact", MySqlDbType.VarChar).Value = txtNewRecipientHospitalContact.Text;
            }

            if (txtNewRecipientHospitalContactPhone.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?NewRecipientHospitalContactPhone", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NewRecipientHospitalContactPhone", MySqlDbType.VarChar).Value = txtNewRecipientHospitalContactPhone.Text;
            }

            if (txtNewTransplantHospital.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?NewTransplantHospital", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NewTransplantHospital", MySqlDbType.VarChar).Value = txtNewTransplantHospital.Text;
            }

            if (GeneralRoutines.IsDate(txtNewScheduledTransplantStartDate.Text) == false)
            {
                MyCMD.Parameters.Add("?NewScheduledTransplantStartDate", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NewScheduledTransplantStartDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtNewScheduledTransplantStartDate.Text);
            }

            if (txtNewScheduledTransplantStartTime.Text == string.Empty || txtNewScheduledTransplantStartTime.Text =="__:__")
            {
                MyCMD.Parameters.Add("?NewScheduledTransplantStartTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NewScheduledTransplantStartTime", MySqlDbType.VarChar).Value = txtNewScheduledTransplantStartTime.Text;
            }


            if (GeneralRoutines.IsDate(txtDepartureFirstTransplantHospitalDate.Text) == false)
            {
                MyCMD.Parameters.Add("?DepartureFirstTransplantHospitalDate", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DepartureFirstTransplantHospitalDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtDepartureFirstTransplantHospitalDate.Text);
            }

            if (txtDepartureFirstTransplantHospitalTime.Text == string.Empty || txtDepartureFirstTransplantHospitalTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?DepartureFirstTransplantHospitalTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DepartureFirstTransplantHospitalTime", MySqlDbType.VarChar).Value = txtDepartureFirstTransplantHospitalTime.Text;
            }

            if (GeneralRoutines.IsDate(txtArrivalNewTransplantHospitalDate.Text) == false)
            {
                MyCMD.Parameters.Add("?ArrivalNewTransplantHospitalDate", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ArrivalNewTransplantHospitalDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtArrivalNewTransplantHospitalDate.Text);
            }

            if (txtArrivalNewTransplantHospitalTime.Text == string.Empty || txtArrivalNewTransplantHospitalTime.Text =="__:__")
            {
                MyCMD.Parameters.Add("?ArrivalNewTransplantHospitalTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ArrivalNewTransplantHospitalTime", MySqlDbType.VarChar).Value = txtArrivalNewTransplantHospitalTime.Text;
            }

            if (string.IsNullOrEmpty(txtNewComments.Text))
            {
                MyCMD.Parameters.Add("?NewComments", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?NewComments", MySqlDbType.VarChar).Value = txtNewComments.Text;
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
            MyCMD.Parameters.Add("?DataLocked", MySqlDbType.VarChar).Value = 1;

            if (chkDataFinal.Checked == true)
            {
                MyCMD.Parameters.Add("?DataFinal", MySqlDbType.VarChar).Value = 1;
            }
            else
            {
                MyCMD.Parameters.Add("?DataFinal", MySqlDbType.VarChar).Value = 0;
            }


            MyCMD.Parameters.Add("?DateCreated", MySqlDbType.DateTime).Value = DateTime.Now;

            MyCMD.Parameters.Add("?CreatedBy", MySqlDbType.VarChar).Value = SessionVariablesAll.UserName;

            MyCONN.Open();

            MySqlTransaction myTrans = MyCONN.BeginTransaction();

            MyCMD.Transaction = myTrans;

            try
            {
                MyCMD.ExecuteNonQuery();

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


                myTrans.Commit();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }


                BindData();
                lblUserMessages.Text = "Data Added/Updated";

                //redirect to summary page if data complete is not no
                string strSQLCOMPLETE = string.Empty;
                strSQLCOMPLETE += "SELECT ";
                strSQLCOMPLETE += " IF(t2.Side  IS NOT NULL ";
                strSQLCOMPLETE += "AND t2.Reallocated  IS NOT NULL ";
                strSQLCOMPLETE += "";
                strSQLCOMPLETE += ", 'Complete', 'Incomplete') ";
                strSQLCOMPLETE += "FROM kidneyproceduredata t2 ";
                strSQLCOMPLETE += "WHERE t2.TrialID=?TrialID AND Side=?Side";
                strSQLCOMPLETE += "";


                string strComplete = GeneralRoutines.ReturnScalarTwo(strSQLCOMPLETE, "?TrialID", Request.QueryString["TID"],"?Side", ddSide.SelectedValue, STRCONN);

                //lblDonorRiskIndex.Text = strComplete;

                if (strComplete == "Complete")
                {
                    Response.Redirect(strRedirectSummary + Request.QueryString["TID"], false);
                }
                else
                {

                    string strUri = Request.Url.AbsoluteUri;

                    if (strUri.Contains("SCode=1"))
                    {
                        //Response.Redirect(strUri, false);

                    }
                    else
                    {
                        //Response.Redirect(strUri + "&SCode=1", false);
                        strUri += "&SCode=1";

                    }

                    if (strUri.Contains("Side"))
                    {
                        if (strUri.Contains("Side=Left"))
                        {
                            strUri.Replace("Left", ddSide.SelectedValue);
                        }
                        if (strUri.Contains("Side=Right"))
                        {
                            strUri.Replace("Right", ddSide.SelectedValue);
                        }

                    }
                    else
                    {
                        strUri += "&Side=" + ddSide.SelectedValue;
                    }
                    Response.Redirect(strUri, false);
                    lblUserMessages.Text = "Data Submitted";
                }


            }

            catch (System.Exception ex)
            {
                myTrans.Rollback();

                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }

                lblUserMessages.Text = ex.Message + " An error occured while executing query.";
            }


            



        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding data.";
        }
    }

    protected void rblReallocated_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            if (rblReallocated.SelectedValue == STR_YES_SELECTION)
            {
                pnlReallocated.Visible = true;

                lblReallocatedMessage.Text = @"If the kidney was taken off the machine before reallocation, the kidney should not go back on the machine, 
                                                but should be cold stored. If the kidney was not taken off the machine yet, it can stay on the machine.";
            }
            else
            {
                pnlReallocated.Visible = false;

                rblReasonReallocated.SelectedIndex=-1;
                txtNewRecipientHospitalContact.Text = string.Empty;
                txtNewRecipientHospitalContactPhone.Text = string.Empty;
                txtNewTransplantHospital.Text = string.Empty;
                txtNewScheduledTransplantStartDate.Text = string.Empty;
                txtNewScheduledTransplantStartTime.Text = string.Empty;
                txtDepartureFirstTransplantHospitalDate.Text = string.Empty;
                txtDepartureFirstTransplantHospitalTime.Text = string.Empty;
                txtArrivalNewTransplantHospitalDate.Text = string.Empty;
                txtArrivalNewTransplantHospitalTime.Text = string.Empty;
                txtNewComments.Text = string.Empty;

                lblReallocatedMessage.Text = string.Empty;
            }

        }
        catch (System.Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while adding data.";
        }
    }

    
    protected void ddSide_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddSide.SelectedValue != STR_UNKNOWN_SELECTION)
            {
                pnlSideSelected.Visible = true;
                AssignData();
                Label lbl;

                foreach (GridViewRow row in GV1.Rows)
                {
                    lbl = (Label)row.FindControl("lblSide");

                    if (lbl != null)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            if (lbl.Text == ddSide.SelectedValue)
                            {
                                row.BackColor = System.Drawing.Color.LightBlue;
                            }
                            else
                            {
                                if (row.RowState == DataControlRowState.Alternate)
                                {
                                    row.BackColor = GV1.AlternatingRowStyle.BackColor;
                                }
                                else
                                {
                                    row.BackColor = GV1.RowStyle.BackColor;
                                }


                            }
                        }
                    }
                }
            }
            else
            {
                pnlSideSelected.Visible = false;
                throw new Exception("Please Select 'Side' of Kidney.");
            }
        }
        catch (Exception ex)
        {
            lblUserMessages.Text = ex.Message + " An error occured while selecting Side of Kidney.";
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