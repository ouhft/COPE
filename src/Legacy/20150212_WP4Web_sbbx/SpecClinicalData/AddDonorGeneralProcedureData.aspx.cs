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

public partial class SpecClinicalData_AddDonorGeneralProcedureData : System.Web.UI.Page
{
    #region " Private Constants & Variables "
        private const string STRCONN = "cope4dbconn";

        private const string strExcludeOccasion = "1-7 Days";

        private const string strMainCPH = "cplMainContents";
        private const string strMainLabel = "lblDonorID";
        private const string strRecipientLabel = "lblRecipientID";

        private const string STR_OTHER_SELECTION = "Other";
        private const string STR_UNKNOWN_SELECTION = "Unknown";
        
        private const string STR_NO_SELECTION = "NO";
        private const string STR_YES_SELECTION = "YES";

        private const string STR_DD_UNKNOWN_SELECTION = "0";

        //number of days before the Date when a TrialID was created Start Date for calendar control should be
        private const int intDaysMinDate = -30;
        
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

                lblDescription.Text = "Add General Procedure Data for " + Request.QueryString["TID"].ToString();

                //declare View State variables
                ViewState["DateCreated"] = string.Empty;
                //get Date Created
                string STRSQL_DATECREATED = "SELECT DateCreated FROM trialdetails WHERE TrialID=?TrialID ";
                ViewState["DateCreated"] = GeneralRoutines.ReturnScalar(STRSQL_DATECREATED, "?TrialID", Request.QueryString["TID"], STRCONN);

                if (GeneralRoutines.IsDate(ViewState["DateCreated"].ToString())==true)
                {
                    txtTransplantCoordinatorPhoneDate_CalendarExtender.StartDate = Convert.ToDateTime(ViewState["DateCreated"]).AddDays(intDaysMinDate);
                    txtScheduledStartWithdrawlDate_CalendarExtender.StartDate = Convert.ToDateTime(ViewState["DateCreated"]).AddDays(intDaysMinDate);
                    txtTechnicianArrivalHubDate_CalendarExtender.StartDate = Convert.ToDateTime(ViewState["DateCreated"]).AddDays(intDaysMinDate);
                    txtIceBoxesFilledDate_CalendarExtender.StartDate = Convert.ToDateTime(ViewState["DateCreated"]).AddDays(intDaysMinDate);
                    txtDepartHubDate_CalendarExtender.StartDate = Convert.ToDateTime(ViewState["DateCreated"]).AddDays(intDaysMinDate);
                    txtArrivalDonorHospitalDate_CalendarExtender.StartDate = Convert.ToDateTime(ViewState["DateCreated"]).AddDays(intDaysMinDate);

                }

                txtTransplantCoordinatorPhoneDate_CalendarExtender.EndDate = DateTime.Today;
                txtScheduledStartWithdrawlDate_CalendarExtender.EndDate = DateTime.Today;
                txtTechnicianArrivalHubDate_CalendarExtender.EndDate = DateTime.Today;
                txtIceBoxesFilledDate_CalendarExtender.EndDate = DateTime.Today;
                txtDepartHubDate_CalendarExtender.EndDate = DateTime.Today;
                txtArrivalDonorHospitalDate_CalendarExtender.EndDate = DateTime.Today;


                cmdReset_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK any data entered will be lost.";
                cmdDelete_ConfirmButtonExtender.ConfirmText = "Please note, if you click on OK the selected/current data will be deleted.";

                ViewState["SortField"] = "TrialID";
                ViewState["SortDirection"] = "ASC";

                BindData();

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
                        cmdReset.Enabled = true;


                    }
                    else
                    {
                        //chkDataLocked.Checked = false;
                        pnlReasonModified.Visible = false;
                        pnlFinal.Visible = false;
                        cmdAddData.Enabled = false;
                        cmdDelete.Enabled = false;
                        cmdReset.Enabled = false;

                    }
                }

                //data final checkbox visible
                if (SessionVariablesAll.AdminSuperUser == STR_YES_SELECTION)
                {
                    pnlFinal.Visible = true;
                    //cmdUpdate.Enabled = true;
                    ////cmdDelete.Enabled = true;
                    //cmdReset.Enabled = true;
                }
                else
                {
                    pnlFinal.Visible = false;
                    //cmdUpdate.Enabled = false;
                    ////cmdDelete.Enabled = false;
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

                hlkSummaryPage.NavigateUrl = strRedirectSummary + Request.QueryString["TID"];

                //mandatory message not required
                //string strMandatoryMessage = string.Empty;
                //strMandatoryMessage = ConfigurationManager.AppSettings["MandatoryMessage"];
                //lblDescription.Text += "<br/><span class='FontMandatory'>" + strMandatoryMessage + "</span>";

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

            // get the DonorID
            string strDonorID = string.Empty;

            ContentPlaceHolder mpCPH = (ContentPlaceHolder)(Master.Master.FindControl(strMainCPH));

            if (mpCPH != null)
            {
                Label lblMainLabel = (Label)(mpCPH.FindControl(strMainLabel));

                if (lblMainLabel != null)
                {
                    strDonorID = lblMainLabel.Text.Replace("(", "");
                    strDonorID = strDonorID.Replace(")", "");
                }
            }

            string strSQL = String.Empty;

            strSQL += "SELECT t1.*, t2.DonorID MainDonorID,  ";
            strSQL += "CONCAT(IF(t1.TransplantCoordinatorPhoneDate IS NULL, NULL, DATE_FORMAT(t1.TransplantCoordinatorPhoneDate, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.TransplantCoordinatorPhoneTime IS NULL, NULL, DATE_FORMAT(t1.TransplantCoordinatorPhoneTime, '%H:%i')))  TransplantCoordinatorPhone, ";
            strSQL += "CONCAT(IF(t1.ScheduledStartWithdrawlDate IS NULL, NULL, DATE_FORMAT(t1.ScheduledStartWithdrawlDate, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.ScheduledStartWithdrawlTime IS NULL, NULL, DATE_FORMAT(t1.ScheduledStartWithdrawlTime, '%H:%i')))  ScheduledStartWithdrawl, ";
            strSQL += "CONCAT(IF(t1.TechnicianArrivalHubDate IS NULL, NULL, DATE_FORMAT(t1.TechnicianArrivalHubDate, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.TechnicianArrivalHubTime IS NULL, NULL, DATE_FORMAT(t1.TechnicianArrivalHubTime, '%H:%i')))  TechnicianArrivalHub, ";
            strSQL += "CONCAT(IF(t1.IceBoxesFilledDate IS NULL, NULL, DATE_FORMAT(t1.IceBoxesFilledDate, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.IceBoxesFilledTime IS NULL, NULL, DATE_FORMAT(t1.IceBoxesFilledTime, '%H:%i')))  IceBoxesFilled, ";
            strSQL += "CONCAT(IF(t1.DepartHubDate IS NULL, NULL, DATE_FORMAT(t1.DepartHubDate, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.DepartHubTime IS NULL, NULL, DATE_FORMAT(t1.DepartHubTime, '%H:%i')))  DepartHub, ";
            strSQL += "CONCAT(IF(t1.ArrivalDonorHospitalDate IS NULL, NULL, DATE_FORMAT(t1.ArrivalDonorHospitalDate, '%d/%m/%Y')), ' ', ";
            strSQL += "IF(t1.ArrivalDonorHospitalTime IS NULL, NULL, DATE_FORMAT(t1.ArrivalDonorHospitalTime, '%H:%i')))  ArrivalDonorHospital, ";
            strSQL += "DATE_FORMAT(t2.DateOfBirthDonor, '%d/%m/%Y') Date_OfBirthDonor ";
            strSQL += "FROM donor_generalproceduredata t1 ";
            strSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            //strSQL += "WHERE t1.DonorID=?DonorID ";
            strSQL += "WHERE t1.TrialID=?TrialID ";
            strSQL += "ORDER BY " + (string)ViewState["SortField"] + " " + (string)ViewState["SortDirection"];


            GV1.DataSource = sqldsGV1;
            sqldsGV1.SelectCommand = strSQL;
            sqldsGV1.SelectParameters.Clear();
            sqldsGV1.SelectParameters.Add("?TrialID", Request.QueryString["TID"].ToString());

            GV1.DataBind();
            lblGV1.Text = "Summary of Donor General Procedure Data";

            if (GV1.Rows.Count == 1)
            {
                cmdDelete.Enabled = true;
                cmdDelete.Visible = true;
                //cmdAddData.Text = "Update Data";
                lblDescription.Text = "Update General Procedure Data for " + Request.QueryString["TID"].ToString() + "";

                AssignData();

            }
            else if (GV1.Rows.Count == 0)
            {
                lblGV1.Text = string.Empty;
                cmdDelete.Enabled = false;
                cmdDelete.Visible = false;
                lblDescription.Text = "Add  General Procedure Data for " + Request.QueryString["TID"].ToString() + "";
            }
            else
            {
                cmdDelete.Enabled = true;
                cmdDelete.Visible = true;
                //cmdAddData.Text = "Update Data";
                lblDescription.Text = "Update Donor General Procedure Data for " + Request.QueryString["TID"].ToString() + "";
                throw new Exception("More than one Records exist. Click on TrialID to Delete an unwanted record.");
            }

            
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

            string STRSQL = "SELECT t1.*, t2.DonorID  FROM  donor_generalproceduredata  t1 ";
            STRSQL += "INNER JOIN trialdetails t2 ON t1.TrialID=t2.TrialID ";
            STRSQL += "WHERE t1.TrialID=?TrialID ";

            if (!string.IsNullOrEmpty(Request.QueryString["DonorGeneralProcedureDataID"]))
            {
                STRSQL += "AND t1.DonorGeneralProcedureDataID=?DonorGeneralProcedureDataID ";
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
            if (!string.IsNullOrEmpty(Request.QueryString["DonorGeneralProcedureDataID"]))
            {
                MyCMD.Parameters.Add("?DonorGeneralProcedureDataID", MySqlDbType.Int32).Value = Request.QueryString["DonorGeneralProcedureDataID"];
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

                            if (!DBNull.Value.Equals(myDr["TransplantTechnician"]))
                            {
                                txtTransplantTechnician.Text = (string)(myDr["TransplantTechnician"]);
                            }

                            if (!DBNull.Value.Equals(myDr["TransplantCoordinatorPhoneDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["TransplantCoordinatorPhoneDate"].ToString()) == true)
                                {
                                    txtTransplantCoordinatorPhoneDate.Text = Convert.ToDateTime(myDr["TransplantCoordinatorPhoneDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["TransplantCoordinatorPhoneTime"]))
                            {
                                if (myDr["TransplantCoordinatorPhoneTime"].ToString().Length >= 5)
                                {
                                    txtTransplantCoordinatorPhoneTime.Text = myDr["TransplantCoordinatorPhoneTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["TransplantCoordinator"]))
                            {
                                txtTransplantCoordinator.Text = (string)(myDr["TransplantCoordinator"]);

                            }

                            if (!DBNull.Value.Equals(myDr["TelephoneTransplantCoordinator"]))
                            {
                                txtTelephoneTransplantCoordinator.Text = (string)(myDr["TelephoneTransplantCoordinator"]);
                            }

                            if (!DBNull.Value.Equals(myDr["RetrievalHospital"]))
                            {
                                txtRetrievalHospital.Text = (string)(myDr["RetrievalHospital"]);
                            }

                            if (!DBNull.Value.Equals(myDr["ScheduledStartWithdrawlDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["ScheduledStartWithdrawlDate"].ToString()) == true)
                                {
                                    txtScheduledStartWithdrawlDate.Text = Convert.ToDateTime(myDr["ScheduledStartWithdrawlDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ScheduledStartWithdrawlTime"]))
                            {
                                if (myDr["ScheduledStartWithdrawlTime"].ToString().Length >= 5)
                                {
                                    txtScheduledStartWithdrawlTime.Text = myDr["ScheduledStartWithdrawlTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["TechnicianArrivalHubDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["TechnicianArrivalHubDate"].ToString()) == true)
                                {
                                    txtTechnicianArrivalHubDate.Text = Convert.ToDateTime(myDr["TechnicianArrivalHubDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["TechnicianArrivalHubTime"]))
                            {
                                if (myDr["TechnicianArrivalHubTime"].ToString().Length >= 5)
                                {
                                    txtTechnicianArrivalHubTime.Text = myDr["TechnicianArrivalHubTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["IceBoxesFilledDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["IceBoxesFilledDate"].ToString()) == true)
                                {
                                    txtIceBoxesFilledDate.Text = Convert.ToDateTime(myDr["IceBoxesFilledDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["IceBoxesFilledTime"]))
                            {
                                if (myDr["IceBoxesFilledTime"].ToString().Length >= 5)
                                {
                                    txtIceBoxesFilledTime.Text = myDr["IceBoxesFilledTime"].ToString().Substring(0, 5);
                                }
                            }

                            if (lblIceBoxesFilled.Font.Bold==true)
                            {
                                if (txtIceBoxesFilledDate.Text == string.Empty || txtIceBoxesFilledTime.Text == string.Empty)
                                {
                                    lblIceBoxesFilled.ForeColor = System.Drawing.Color.FromName(strIncompleteColour);
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DepartHubDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["DepartHubDate"].ToString()) == true)
                                {
                                    txtDepartHubDate.Text = Convert.ToDateTime(myDr["DepartHubDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["DepartHubTime"]))
                            {
                                if (myDr["DepartHubTime"].ToString().Length >= 5)
                                {
                                    txtDepartHubTime.Text = myDr["DepartHubTime"].ToString().Substring(0, 5);
                                }
                            }

                            //if (!DBNull.Value.Equals(myDr["CarNumber"]))
                            //{
                            //    txtCarNumber.Text = (string)(myDr["CarNumber"]);
                            //}


                            //if (!DBNull.Value.Equals(myDr["CarLicensePlate"]))
                            //{
                            //    txtCarLicensePlate.Text = (string)(myDr["CarLicensePlate"]);
                            //}

                            if (!DBNull.Value.Equals(myDr["ArrivalDonorHospitalDate"]))
                            {
                                if (GeneralRoutines.IsDate(myDr["ArrivalDonorHospitalDate"].ToString()) == true)
                                {
                                    txtArrivalDonorHospitalDate.Text = Convert.ToDateTime(myDr["ArrivalDonorHospitalDate"]).ToShortDateString();
                                }
                            }

                            if (!DBNull.Value.Equals(myDr["ArrivalDonorHospitalTime"]))
                            {
                                if (myDr["ArrivalDonorHospitalTime"].ToString().Length >= 5)
                                {
                                    txtArrivalDonorHospitalTime.Text = myDr["ArrivalDonorHospitalTime"].ToString().Substring(0, 5);
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

    protected void cmdAddData_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            DateTime dteTransplantCoordinatorPhone = DateTime.MinValue;
            DateTime dateTransplantCoordinatorPhone = DateTime.MinValue;

            DateTime dteScheduledStartWithdrawl = DateTime.MinValue;
            DateTime dateScheduledStartWithdrawl = DateTime.MinValue;
            
            DateTime dteTechnicianArrivalHub = DateTime.MinValue;
            DateTime dateTechnicianArrivalHub = DateTime.MinValue;
            
            DateTime dteIceBoxesFilled = DateTime.MinValue;
            DateTime dateIceBoxesFilled = DateTime.MinValue;
            
            DateTime dteDepartHub = DateTime.MinValue;
            DateTime dateDepartHub = DateTime.MinValue;
            
            DateTime dteArrivalDonorHospital = DateTime.MinValue;
            DateTime dateArrivalDonorHospital = DateTime.MinValue;

            if (txtTransplantCoordinatorPhoneDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtTransplantCoordinatorPhoneDate.Text) == false)
                {
                    throw new Exception("Please Enter '" + lblTransplantCoordinatorPhone.Text + "' Date as DD/MM/YYYY."); 
                }

                if (Convert.ToDateTime(txtTransplantCoordinatorPhoneDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblTransplantCoordinatorPhone.Text + "' Date can not be greater than Today's date.");
                }

                dateTransplantCoordinatorPhone = Convert.ToDateTime(txtTransplantCoordinatorPhoneDate.Text);
                
            }

            if (txtTransplantCoordinatorPhoneTime.Text != string.Empty && txtTransplantCoordinatorPhoneTime.Text != "__:__")
            {
               if (GeneralRoutines.IsNumeric(txtTransplantCoordinatorPhoneTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblTransplantCoordinatorPhone.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtTransplantCoordinatorPhoneTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblTransplantCoordinatorPhone.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtTransplantCoordinatorPhoneTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblTransplantCoordinatorPhone.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtTransplantCoordinatorPhoneTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblTransplantCoordinatorPhone.Text + "' Time Hour Minute not be greater than 59.");
                }
            }


            if (txtTransplantCoordinatorPhoneDate.Text != string.Empty && txtTransplantCoordinatorPhoneTime.Text != string.Empty && txtTransplantCoordinatorPhoneTime.Text != "__:__")
            {
                dteTransplantCoordinatorPhone = Convert.ToDateTime(txtTransplantCoordinatorPhoneDate.Text + " " + txtTransplantCoordinatorPhoneTime.Text);
            }

            if (txtScheduledStartWithdrawlDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtScheduledStartWithdrawlDate.Text) == false)
                {
                    throw new Exception("Please Enter '" + lblScheduledStartWithdrawl.Text + "' Date as DD/MM/YYYY.");
                }

                if (Convert.ToDateTime(txtScheduledStartWithdrawlDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblScheduledStartWithdrawl.Text + "' Date can not be greater than Today's date.");
                }

                dateScheduledStartWithdrawl = Convert.ToDateTime(txtScheduledStartWithdrawlDate.Text);


            }

            if (txtScheduledStartWithdrawlTime.Text != string.Empty && txtScheduledStartWithdrawlTime.Text != "__:__")
            {
                if (GeneralRoutines.IsNumeric(txtScheduledStartWithdrawlTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblScheduledStartWithdrawl.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtScheduledStartWithdrawlTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblScheduledStartWithdrawl.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtScheduledStartWithdrawlTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblScheduledStartWithdrawl.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtScheduledStartWithdrawlTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblScheduledStartWithdrawl.Text + "' Time Hour Minute not be greater than 59.");
                }
            }


            if (txtScheduledStartWithdrawlDate.Text != string.Empty && txtScheduledStartWithdrawlTime.Text != string.Empty && txtScheduledStartWithdrawlTime.Text != "__:__")
            {
                dteScheduledStartWithdrawl = Convert.ToDateTime(txtScheduledStartWithdrawlDate.Text + " " + txtScheduledStartWithdrawlTime.Text);
            }

            //if (dteScheduledStartWithdrawl!=DateTime.MinValue && dteTransplantCoordinatorPhone != DateTime.MinValue)
            //{
            //    if (dteScheduledStartWithdrawl)
            //}

            if (txtTechnicianArrivalHubDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtTechnicianArrivalHubDate.Text) == false)
                {
                    throw new Exception("Please Enter '" + lblTechnicianArrivalHub.Text + "' Date as DD/MM/YYYY.");
                }

                if (Convert.ToDateTime(txtTechnicianArrivalHubDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblTechnicianArrivalHub.Text + "' Date cannot be greater than Today's date.");
                }


                dateTechnicianArrivalHub = Convert.ToDateTime(txtTechnicianArrivalHubDate.Text);

            }

            if (txtTechnicianArrivalHubTime.Text != string.Empty && txtTechnicianArrivalHubTime.Text != "__:__")
            {
                if (GeneralRoutines.IsNumeric(txtTechnicianArrivalHubTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblTechnicianArrivalHub.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtTechnicianArrivalHubTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblTechnicianArrivalHub.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtTechnicianArrivalHubTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblTechnicianArrivalHub.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtTechnicianArrivalHubTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblTechnicianArrivalHub.Text + "' Time Hour Minute not be greater than 59.");
                }
            }


            if (txtTechnicianArrivalHubDate.Text != string.Empty && txtTechnicianArrivalHubTime.Text != string.Empty && txtTechnicianArrivalHubTime.Text != "__:__")
            {
                dteTechnicianArrivalHub = Convert.ToDateTime(txtTechnicianArrivalHubDate.Text + " " + txtTechnicianArrivalHubTime.Text);
            }


            if (txtIceBoxesFilledDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtIceBoxesFilledDate.Text) == false)
                {
                    throw new Exception("Please Enter '" + lblIceBoxesFilled.Text + "' Date as DD/MM/YYYY.");
                }

                if (Convert.ToDateTime(txtIceBoxesFilledDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblIceBoxesFilled.Text + "' Date can not be greater than Today's date.");
                }

                dateIceBoxesFilled = Convert.ToDateTime(txtIceBoxesFilledDate.Text);

            }

            if (txtIceBoxesFilledTime.Text != string.Empty && txtIceBoxesFilledTime.Text != "__:__")
            {
                if (GeneralRoutines.IsNumeric(txtIceBoxesFilledTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblIceBoxesFilled.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtIceBoxesFilledTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblIceBoxesFilled.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtIceBoxesFilledTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblIceBoxesFilled.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtIceBoxesFilledTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblIceBoxesFilled.Text + "' Time Hour Minute not be greater than 59.");
                }
            }


            if (txtIceBoxesFilledDate.Text != string.Empty && txtIceBoxesFilledTime.Text != string.Empty && txtIceBoxesFilledTime.Text != "__:__")
            {
                dteIceBoxesFilled = Convert.ToDateTime(txtIceBoxesFilledDate.Text + " " + txtIceBoxesFilledTime.Text);
            }

            if (dteTechnicianArrivalHub != DateTime.MinValue && dteIceBoxesFilled != DateTime.MinValue)
            {
                if (dteIceBoxesFilled < dteTechnicianArrivalHub)
                {
                    throw new Exception("'" + lblIceBoxesFilled.Text + "' cannot be earlier than '" + lblTechnicianArrivalHub.Text + "'. Please check the Date/Time you have entered.");
                }
            }
            else
            {
                if (dateTechnicianArrivalHub != DateTime.MinValue && dateIceBoxesFilled != DateTime.MinValue)
                {
                    if (dateIceBoxesFilled.Date < dateTechnicianArrivalHub.Date)
                    {
                        throw new Exception("'" + lblIceBoxesFilled.Text + "' cannot be earlier than '" + lblTechnicianArrivalHub.Text + "'. Please check the dates you have entered.");
                    }
                }

            }

            if (txtDepartHubDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtDepartHubDate.Text) == false)
                {
                    throw new Exception("Please Enter '" + lblDepartHub.Text + "' Date as DD/MM/YYYY.");
                }

                if (Convert.ToDateTime(txtDepartHubDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblDepartHub.Text + "' Date can not be greater than Today's date.");
                }

                dateDepartHub = Convert.ToDateTime(txtDepartHubDate.Text);

            }

            if (txtDepartHubTime.Text != string.Empty && txtDepartHubTime.Text != "__:__")
            {
                if (GeneralRoutines.IsNumeric(txtDepartHubTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblDepartHub.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtDepartHubTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblDepartHub.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtDepartHubTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblDepartHub.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtDepartHubTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblDepartHub.Text + "' Time Hour Minute not be greater than 59.");
                }
            }


            if (txtDepartHubDate.Text != string.Empty && txtDepartHubTime.Text != string.Empty && txtDepartHubTime.Text != "__:__")
            {
                dteDepartHub = Convert.ToDateTime(txtDepartHubDate.Text + " " + txtDepartHubTime.Text);
            }


            if (dteDepartHub != DateTime.MinValue)
            {
                if (dteIceBoxesFilled != DateTime.MinValue)
                {
                    if (dteDepartHub < dteIceBoxesFilled)
                    {
                        throw new Exception("'" + lblDepartHub.Text + "' cannot be earlier than '" + lblIceBoxesFilled.Text + "'. Please check the date/time you have entered.");

                    }

                }


                if (dteTechnicianArrivalHub != DateTime.MinValue)
                {
                    if (dteDepartHub < dteTechnicianArrivalHub)
                    {
                        throw new Exception("'" + lblDepartHub.Text + "' cannot be earlier than '" + lblTechnicianArrivalHub.Text + "'. Please check the date/time you have entered.");

                    }

                }


            }
            else
            {
                if (dateDepartHub != DateTime.MinValue)
                {
                    if (dateIceBoxesFilled != DateTime.MinValue)
                    {
                        if (dateDepartHub.Date < dateIceBoxesFilled.Date)
                        {
                            throw new Exception("'" + lblDepartHub.Text + "' cannot be earlier than '" + lblIceBoxesFilled.Text + "'. Please check the dates.");

                        }

                    }

                    if (dateTechnicianArrivalHub != DateTime.MinValue)
                    {
                        if (dateDepartHub.Date < dateTechnicianArrivalHub.Date)
                        {
                            throw new Exception("'" + lblDepartHub.Text + "' cannot be earlier than '" + lblTechnicianArrivalHub.Text + "'. Please check the dates. ");

                        }

                    }

                }

                


            }

            if (txtArrivalDonorHospitalDate.Text != string.Empty)
            {
                if (GeneralRoutines.IsDate(txtArrivalDonorHospitalDate.Text) == false)
                {
                    throw new Exception("Please Enter '" + lblArrivalDonorHospital.Text + "' Date as DD/MM/YYYY.");
                }

                if (Convert.ToDateTime(txtArrivalDonorHospitalDate.Text) > DateTime.Now)
                {
                    throw new Exception("'" + lblArrivalDonorHospital.Text + "' Date can not be greater than Today's date.");
                }

                dateArrivalDonorHospital = Convert.ToDateTime(txtArrivalDonorHospitalDate.Text);

            }

            if (txtArrivalDonorHospitalTime.Text != string.Empty && txtArrivalDonorHospitalTime.Text != "__:__")
            {
                if (GeneralRoutines.IsNumeric(txtArrivalDonorHospitalTime.Text.Substring(0, 2)) == false)
                {
                    throw new Exception("'" + lblArrivalDonorHospital.Text + "' Time Hour should be numeric.");
                }

                if (Convert.ToInt16(txtArrivalDonorHospitalTime.Text.Substring(0, 2)) > 23)
                {
                    throw new Exception("'" + lblArrivalDonorHospital.Text + "' Time Hour should not be greater than 23.");
                }

                if (GeneralRoutines.IsNumeric(txtArrivalDonorHospitalTime.Text.Substring(3, 2)) == false)
                {
                    throw new Exception("'" + lblArrivalDonorHospital.Text + "' Time Minute should be numeric.");
                }

                if (Convert.ToInt16(txtArrivalDonorHospitalTime.Text.Substring(3, 2)) > 59)
                {
                    throw new Exception("'" + lblArrivalDonorHospital.Text + "' Time Hour Minute not be greater than 59.");
                }
            }


            if (txtArrivalDonorHospitalDate.Text != string.Empty && txtArrivalDonorHospitalTime.Text != string.Empty && txtArrivalDonorHospitalTime.Text != "__:__")
            {
                dteArrivalDonorHospital = Convert.ToDateTime(txtArrivalDonorHospitalDate.Text + " " + txtArrivalDonorHospitalTime.Text);
            }

            if (dteArrivalDonorHospital  != DateTime.MinValue)
            {

                if (dteDepartHub != DateTime.MinValue)
                {
                    if (dteDepartHub > dteArrivalDonorHospital)
                    {
                        throw new Exception("'" + lblArrivalDonorHospital.Text + "' cannot be earlier than '" + lblDepartHub.Text + "'. Please check the date/time you have entered.");
                    }
                }

                if (dteIceBoxesFilled  != DateTime.MinValue)
                {
                    if (dteIceBoxesFilled > dteArrivalDonorHospital)
                    {
                        throw new Exception("'" + lblArrivalDonorHospital.Text + "' cannot be earlier than '" + lblIceBoxesFilled.Text + "'. Please check the date/time you have entered.");
                    }

                }

                if (dteTechnicianArrivalHub != DateTime.MinValue)
                {
                    if (dteTechnicianArrivalHub > dteArrivalDonorHospital)
                    {
                        throw new Exception("'" + lblArrivalDonorHospital.Text + "' cannot be earlier than '" + lblTechnicianArrivalHub.Text + "'. Please check the date/time you have entered.");
                    }
                }
                
            }
            else
            {
                if (dateArrivalDonorHospital  != DateTime.MinValue)
                {
                    if (dateDepartHub != DateTime.MinValue)
                    {
                        if (dateDepartHub.Date > dateArrivalDonorHospital.Date)
                        {
                            throw new Exception("'" + lblArrivalDonorHospital.Text + "' cannot be earlier than '" + lblDepartHub.Text + "'. Please check the dates.");
                        }
                    }

                    if (dateIceBoxesFilled != DateTime.MinValue)
                    {
                        if (dateIceBoxesFilled > dateArrivalDonorHospital)
                        {
                            throw new Exception("'" + lblArrivalDonorHospital.Text + "' cannot be earlier than '" + lblIceBoxesFilled.Text + "'. Please check the dates.");
                        }

                    }

                    if (dateTechnicianArrivalHub != DateTime.MinValue)
                    {
                        if (dateTechnicianArrivalHub > dateArrivalDonorHospital)
                        {
                            throw new Exception("'" + lblArrivalDonorHospital.Text + "' cannot be earlier than '" + lblTechnicianArrivalHub.Text + "'. Please check the dates.");
                        }
                    }


                    
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

            STRSQL += "INSERT INTO donor_generalproceduredata ";
            STRSQL += "(TrialID, TransplantTechnician, TransplantCoordinatorPhoneDate, TransplantCoordinatorPhoneTime, TransplantCoordinator,";
            STRSQL += "TelephoneTransplantCoordinator,RetrievalHospital, ScheduledStartWithdrawlDate, ScheduledStartWithdrawlTime,";
            STRSQL += "TechnicianArrivalHubDate, TechnicianArrivalHubTime,IceBoxesFilledDate, IceBoxesFilledTime, DepartHubDate,DepartHubTime,";
            //STRSQL += "CarNumber, CarLicensePlate,ArrivalDonorHospitalDate, ArrivalDonorHospitalTime,";
            STRSQL += "ArrivalDonorHospitalDate, ArrivalDonorHospitalTime,";
            STRSQL += "Comments, DateCreated, CreatedBy) ";
            STRSQL += "VALUES ";
            STRSQL += "(?TrialID, ?TransplantTechnician, ?TransplantCoordinatorPhoneDate, ?TransplantCoordinatorPhoneTime, ?TransplantCoordinator,";
            STRSQL += "?TelephoneTransplantCoordinator, ?RetrievalHospital, ?ScheduledStartWithdrawlDate, ?ScheduledStartWithdrawlTime,";
            STRSQL += "?TechnicianArrivalHubDate, ?TechnicianArrivalHubTime, ?IceBoxesFilledDate, ?IceBoxesFilledTime, ?DepartHubDate, ?DepartHubTime,";
            STRSQL += "?ArrivalDonorHospitalDate, ?ArrivalDonorHospitalTime,";
            STRSQL += "?Comments, ?DateCreated, ?CreatedBy) ";

            string STRSQL_UPDATE = "";
            STRSQL_UPDATE += "UPDATE donor_generalproceduredata SET ";
            STRSQL_UPDATE += "TransplantTechnician=?TransplantTechnician, TransplantCoordinatorPhoneDate=?TransplantCoordinatorPhoneDate, ";
            STRSQL_UPDATE += "TransplantCoordinatorPhoneTime=?TransplantCoordinatorPhoneTime,  TransplantCoordinator=?TransplantCoordinator,";
            STRSQL_UPDATE += "TelephoneTransplantCoordinator=?TelephoneTransplantCoordinator, RetrievalHospital=?RetrievalHospital, ";
            STRSQL_UPDATE += "ScheduledStartWithdrawlDate=?ScheduledStartWithdrawlDate, ScheduledStartWithdrawlTime=?ScheduledStartWithdrawlTime,";
            STRSQL_UPDATE += "TechnicianArrivalHubDate=?TechnicianArrivalHubDate, TechnicianArrivalHubTime=?TechnicianArrivalHubTime,";
            STRSQL_UPDATE += "IceBoxesFilledDate=?IceBoxesFilledDate, IceBoxesFilledTime=?IceBoxesFilledTime, ";
            STRSQL_UPDATE += "DepartHubDate=?DepartHubDate, DepartHubTime=?DepartHubTime,";
            STRSQL_UPDATE += "ArrivalDonorHospitalDate=?ArrivalDonorHospitalDate, ArrivalDonorHospitalTime=?ArrivalDonorHospitalTime,";
            STRSQL_UPDATE += "";
            STRSQL_UPDATE += "ReasonModified=?ReasonModified, ";
            STRSQL_UPDATE += "Comments=?Comments, DateUpdated=?DateCreated, UpdatedBy=?CreatedBy ";
            STRSQL_UPDATE += "WHERE TrialID=?TrialID ";


            //lock data locked in every case
            string STRSQL_LOCK = "";
            STRSQL_LOCK += "UPDATE donor_generalproceduredata SET ";
            STRSQL_LOCK += "DataLocked=?DataLocked, DateLocked=?DateCreated, LockedBy=?CreatedBy ";
            STRSQL_LOCK += "WHERE TrialID=?TrialID AND (DataLocked IS NULL OR DataLocked=0) ";
            STRSQL_LOCK += "";
            STRSQL_LOCK += "";


            // mark final
            string STRSQL_FINAL = string.Empty;
            STRSQL_FINAL += "UPDATE donor_generalproceduredata SET ";
            STRSQL_FINAL += "DataFinal=?DataFinal, DateFinal=?DateCreated, FinalAssignedBy=?CreatedBy ";
            STRSQL_FINAL += "WHERE TrialID=?TrialID ";

            string CS = string.Empty;
            CS = ConfigurationManager.ConnectionStrings[STRCONN].ConnectionString;

            MySqlConnection MyCONN = new MySqlConnection();
            MyCONN.ConnectionString = CS;

            MySqlCommand MyCMD = new MySqlCommand();
            MyCMD.Connection = MyCONN;

            MyCMD.CommandType = CommandType.Text;

            string STRSQL_FIND = "SELECT COUNT(*) CR FROM donor_generalproceduredata WHERE TrialID=?TrialID ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN));

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
                throw new Exception("More than One Donor Operation Data exists for this TrialID. Click on TrialID in the table below to select data to delete. ");
            }
            else
            {
                throw new Exception("An error occured while check if Donor Operation Data already exist in the database.");
            }


            MyCMD.Parameters.Clear();

            MyCMD.Parameters.Add("?TrialID", MySqlDbType.VarChar).Value = Request.QueryString["TID"].ToString();

            
            if (txtTransplantTechnician.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?TransplantTechnician", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TransplantTechnician", MySqlDbType.VarChar).Value =txtTransplantTechnician.Text;
            }

            if (GeneralRoutines.IsDate(txtTransplantCoordinatorPhoneDate.Text) == false)
            {
                MyCMD.Parameters.Add("?TransplantCoordinatorPhoneDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TransplantCoordinatorPhoneDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtTransplantCoordinatorPhoneDate.Text);
            }

            if (txtTransplantCoordinatorPhoneTime.Text == String.Empty || txtTransplantCoordinatorPhoneTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?TransplantCoordinatorPhoneTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TransplantCoordinatorPhoneTime", MySqlDbType.VarChar).Value = txtTransplantCoordinatorPhoneTime.Text;
            }

            if (txtTransplantCoordinator.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?TransplantCoordinator", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TransplantCoordinator", MySqlDbType.VarChar).Value = txtTransplantCoordinator.Text;
            }

            if (txtTelephoneTransplantCoordinator.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?TelephoneTransplantCoordinator", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TelephoneTransplantCoordinator", MySqlDbType.VarChar).Value = txtTelephoneTransplantCoordinator.Text;
            }
            
            if (txtRetrievalHospital.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?RetrievalHospital", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?RetrievalHospital", MySqlDbType.VarChar).Value = txtRetrievalHospital.Text;
            }

            if (GeneralRoutines.IsDate(txtScheduledStartWithdrawlDate.Text) == false)
            {
                MyCMD.Parameters.Add("?ScheduledStartWithdrawlDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ScheduledStartWithdrawlDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtScheduledStartWithdrawlDate.Text);
            }

            if (txtScheduledStartWithdrawlTime.Text == String.Empty || txtScheduledStartWithdrawlTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?ScheduledStartWithdrawlTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ScheduledStartWithdrawlTime", MySqlDbType.VarChar).Value = txtScheduledStartWithdrawlTime.Text;
            }

            if (GeneralRoutines.IsDate(txtTechnicianArrivalHubDate.Text) == false)
            {
                MyCMD.Parameters.Add("?TechnicianArrivalHubDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TechnicianArrivalHubDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtTechnicianArrivalHubDate.Text);
            }

            if (txtTechnicianArrivalHubTime.Text == String.Empty || txtTechnicianArrivalHubTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?TechnicianArrivalHubTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?TechnicianArrivalHubTime", MySqlDbType.VarChar).Value = txtTechnicianArrivalHubTime.Text;
            }

            if (GeneralRoutines.IsDate(txtIceBoxesFilledDate.Text) == false)
            {
                MyCMD.Parameters.Add("?IceBoxesFilledDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?IceBoxesFilledDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtIceBoxesFilledDate.Text);
            }

            if (txtIceBoxesFilledTime.Text == String.Empty || txtIceBoxesFilledTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?IceBoxesFilledTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?IceBoxesFilledTime", MySqlDbType.VarChar).Value = txtIceBoxesFilledTime.Text;
            }

            if (GeneralRoutines.IsDate(txtDepartHubDate.Text) == false)
            {
                MyCMD.Parameters.Add("?DepartHubDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DepartHubDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtDepartHubDate.Text);
            }

            if (txtDepartHubTime.Text == String.Empty || txtDepartHubTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?DepartHubTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?DepartHubTime", MySqlDbType.VarChar).Value = txtDepartHubTime.Text;
            }

            
            if (GeneralRoutines.IsDate(txtArrivalDonorHospitalDate.Text) == false)
            {
                MyCMD.Parameters.Add("?ArrivalDonorHospitalDate", MySqlDbType.Date).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ArrivalDonorHospitalDate", MySqlDbType.Date).Value = Convert.ToDateTime(txtArrivalDonorHospitalDate.Text);
            }

            if (txtArrivalDonorHospitalTime.Text == String.Empty || txtArrivalDonorHospitalTime.Text == "__:__")
            {
                MyCMD.Parameters.Add("?ArrivalDonorHospitalTime", MySqlDbType.VarChar).Value = DBNull.Value;
            }
            else
            {
                MyCMD.Parameters.Add("?ArrivalDonorHospitalTime", MySqlDbType.VarChar).Value = txtArrivalDonorHospitalTime.Text;
            }

            

            if (string.IsNullOrEmpty(txtComments.Text))
            { MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = DBNull.Value; }
            else
            { MyCMD.Parameters.Add("?Comments", MySqlDbType.VarChar).Value = txtComments.Text; }


            if (txtReasonModified.Text == string.Empty)
            {
                MyCMD.Parameters.Add("?ReasonModified", MySqlDbType.VarChar).Value = DBNull.Value;

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

            try
            {
                MyCMD.ExecuteNonQuery();
                BindData();
                
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

                if (chkAllDataAdded.Checked == true || chkDataLocked.Checked == true || chkDataFinal.Checked == true)
                {
                    Response.Redirect(strRedirectSummary + Request.QueryString["TID"], false);
                }
                else
                {
                    Response.Redirect(Request.Url.AbsoluteUri, false);
                }
            }

            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                {
                    MyCONN.Close();
                }
                lblUserMessages.Text = ex.Message + " An error occured while executing query.";
            }


            
        }

        catch (System.Exception ex)
        {
            lblUserMessages.Text =  ex.Message + " An error occured while adding data.";
        }
    }
   
    protected void cmdDelete_Click(object sender, EventArgs e)
    {
        try
        {
            lblUserMessages.Text = string.Empty;

            //check if only one record exists
            string STRSQL_FIND = "SELECT COUNT(*) CR FROM donor_generalproceduredata WHERE TrialID=?TrialID ";

            int intCountFind = Convert.ToInt16(GeneralRoutines.ReturnScalar(STRSQL_FIND, "?TrialID", Request.QueryString["TID"].ToString(), STRCONN));

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
                if (String.IsNullOrEmpty(Request.QueryString["DonorGeneralProcedureDataID"]))
                {
                    throw new Exception("More than one Record exists for deletion. Click on TrialID in the table below to select data to delete.");
                }

            }

            string STRSQL = string.Empty;

            STRSQL += "DELETE FROM donor_generalproceduredata ";
            STRSQL += "WHERE TrialID=?TrialID ";
            if (intCountFind > 1)
            {
                STRSQL += "AND DonorGeneralProcedureDataID=?DonorGeneralProcedureDataID ";
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

            if (!String.IsNullOrEmpty(Request.QueryString["DonorGeneralProcedureDataID"]))
            {
                MyCMD.Parameters.Add("?DonorGeneralProcedureDataID", MySqlDbType.VarChar).Value = Request.QueryString["DonorGeneralProcedureDataID"].ToString();
            }

            MyCONN.Open();

            try
            {
                MyCMD.ExecuteNonQuery();
                BindData();
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                lblUserMessages.Text = "Data Deleted.";

                Response.Redirect(Request.Url.AbsoluteUri, false);
            }

            catch (System.Exception ex)
            {
                if (MyCONN.State == ConnectionState.Open)
                { MyCONN.Close(); }
                lblUserMessages.Text = ex.Message + " An error occured while executing delete query.";
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

    //protected void txtTransplantCoordinatorPhoneDate_TextChanged(object sender, EventArgs e)
    //{
    //    if (GeneralRoutines.IsDate(txtTransplantCoordinatorPhoneDate.Text) == true)
    //    {

    //        txtScheduledStartWithdrawlDate.Text = txtTransplantCoordinatorPhoneDate.Text;
    //        txtTechnicianArrivalHubDate.Text = txtTransplantCoordinatorPhoneDate.Text;
    //        txtIceBoxesFilledDate.Text = txtTransplantCoordinatorPhoneDate.Text;
    //        txtDepartHubDate.Text = txtTransplantCoordinatorPhoneDate.Text;
    //        txtArrivalDonorHospitalDate.Text = txtTransplantCoordinatorPhoneDate.Text;
    //    }
    //}
    //protected void txtScheduledStartWithdrawlDate_TextChanged(object sender, EventArgs e)
    //{
    //    if (GeneralRoutines.IsDate(txtScheduledStartWithdrawlDate.Text) == true)
    //    {

    //        txtTechnicianArrivalHubDate.Text = txtScheduledStartWithdrawlDate.Text;
    //        txtIceBoxesFilledDate.Text = txtScheduledStartWithdrawlDate.Text;
    //        txtDepartHubDate.Text = txtScheduledStartWithdrawlDate.Text;
    //        txtArrivalDonorHospitalDate.Text = txtScheduledStartWithdrawlDate.Text;
    //    }
    //}
    //protected void txtTechnicianArrivalHubDate_TextChanged(object sender, EventArgs e)
    //{
    //    if (GeneralRoutines.IsDate(txtTechnicianArrivalHubDate.Text) == true)
    //    {

    //        txtIceBoxesFilledDate.Text = txtTechnicianArrivalHubDate.Text;
    //        txtDepartHubDate.Text = txtTechnicianArrivalHubDate.Text;
    //        txtArrivalDonorHospitalDate.Text = txtTechnicianArrivalHubDate.Text;
    //    }
    //}
    //protected void txtIceBoxesFilledDate_TextChanged(object sender, EventArgs e)
    //{
    //    if (GeneralRoutines.IsDate(txtIceBoxesFilledDate.Text) == true)
    //    {
    //        txtDepartHubDate.Text = txtIceBoxesFilledDate.Text;
    //        txtArrivalDonorHospitalDate.Text = txtIceBoxesFilledDate.Text;
    //    }
    //}
    //protected void txtDepartHubDate_TextChanged(object sender, EventArgs e)
    //{
    //    if (GeneralRoutines.IsDate(txtDepartHubDate.Text) == true)
    //    {
    //        txtArrivalDonorHospitalDate.Text = txtDepartHubDate.Text;
    //    }
    //}
}